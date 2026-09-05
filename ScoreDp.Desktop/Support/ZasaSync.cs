using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using ScoreDp.Desktop.Data;
using ScoreDp.Desktop.Models;

namespace ScoreDp.Desktop.Support;

public static partial class ZasaSync
{
    private const string ZasaUrl = "https://zasa.sakura.ne.jp/dp/run.php";

    private static readonly Dictionary<string, string> DifficultyMap = new()
    {
        ["5"] = "HYPER",
        ["7"] = "ANOTHER",
        ["9"] = "LEGGENDARIA",
    };

    [GeneratedRegex(@"☆(\d+)\s*\(([0-9.]+)\)")]
    private static partial Regex CellRegex();

    [GeneratedRegex(@"music\.php\?id=(\d{5})-([579])-[01]")]
    private static partial Regex LinkRegex();

    private record ParsedCell(string ZasaId, string Chart, int Level, double UnofficialLevel);

    private record ZasaSong(string ZasaId, string Chart, int Level, double UnofficialLevel, string Title, string? Version);

    public static async Task<(int Updated, int Added)> SyncAsync(ScoreDpDbContext db)
    {
        var songs = await FetchAsync();

        var updated = 0;
        var added = 0;
        var versionCache = new Dictionary<string, int>();

        foreach (var s in songs)
        {
            var titleNormalized = TitleNormalizer.Normalize(s.Title);
            int? versionId = s.Version is not null ? await GetOrCreateVersionIdAsync(db, s.Version, versionCache) : null;

            var existing = await db.Songs.FirstOrDefaultAsync(sg =>
                sg.TitleNormalized == titleNormalized && sg.Chart == s.Chart && sg.Level == s.Level);

            if (existing is not null)
            {
                if (existing.UnofficialLevel != s.UnofficialLevel || existing.ZasaId != s.ZasaId || existing.VersionId != versionId)
                {
                    existing.UnofficialLevel = s.UnofficialLevel;
                    existing.ZasaId = s.ZasaId;
                    existing.VersionId = versionId;
                    updated++;
                }
            }
            else
            {
                db.Songs.Add(new Song
                {
                    Title = s.Title,
                    TitleNormalized = titleNormalized,
                    Level = s.Level,
                    Chart = s.Chart,
                    UnofficialLevel = s.UnofficialLevel,
                    ZasaId = s.ZasaId,
                    VersionId = versionId,
                });
                added++;
            }
        }

        await db.SaveChangesAsync();
        return (updated, added);
    }

    private static async Task<int> GetOrCreateVersionIdAsync(ScoreDpDbContext db, string name, Dictionary<string, int> cache)
    {
        if (cache.TryGetValue(name, out var id)) return id;

        var version = await db.Versions.FirstOrDefaultAsync(v => v.Name == name);
        if (version is null)
        {
            version = new GameVersion { Name = name };
            db.Versions.Add(version);
            await db.SaveChangesAsync();
        }
        cache[name] = version.Id;
        return version.Id;
    }

    private static async Task<List<ZasaSong>> FetchAsync()
    {
        using var http = new HttpClient();
        var html = await http.GetStringAsync(ZasaUrl);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var songs = new List<ZasaSong>();
        string? version = null;

        var rows = doc.DocumentNode.SelectNodes("//tr");
        if (rows is null) return songs;

        foreach (var row in rows)
        {
            var header = row.SelectSingleNode("./th");
            if (header is not null)
            {
                version = header.InnerText.Trim();
                continue;
            }

            var cells = row.SelectNodes("./td");
            if (cells is null || cells.Count < 4) continue;

            var title = HtmlEntity.DeEntitize(cells[^1].InnerText).Trim();
            if (string.IsNullOrEmpty(title)) continue;

            foreach (var cell in cells.Take(3))
            {
                var info = ParseCell(cell);
                if (info is not null)
                {
                    songs.Add(new ZasaSong(info.ZasaId, info.Chart, info.Level, info.UnofficialLevel, title, version));
                }
            }
        }

        return songs;
    }

    private static ParsedCell? ParseCell(HtmlNode cell)
    {
        var a = cell.SelectSingleNode(".//a");
        if (a is null) return null;

        var href = a.GetAttributeValue("href", "");
        var linkMatch = LinkRegex().Match(href);
        if (!linkMatch.Success) return null;

        if (!DifficultyMap.TryGetValue(linkMatch.Groups[2].Value, out var chart)) return null;

        var cellMatch = CellRegex().Match(HtmlEntity.DeEntitize(a.InnerText));
        if (!cellMatch.Success) return null;

        return new ParsedCell(
            linkMatch.Groups[1].Value,
            chart,
            int.Parse(cellMatch.Groups[1].Value, CultureInfo.InvariantCulture),
            double.Parse(cellMatch.Groups[2].Value, CultureInfo.InvariantCulture));
    }
}
