namespace ScoreDp.Desktop.Support;

public record ChartStyle(string Color, string Prefix);

public static class SongStyle
{
    public static readonly Dictionary<string, ChartStyle> ChartStyles = new()
    {
        ["LEGGENDARIA"] = new ChartStyle("#fd067c", "† "),
        ["ANOTHER"] = new ChartStyle("inherit", ""),
        ["HYPER"] = new ChartStyle("#ffa500", ""),
    };

    private const string NewestVersionColor = "#4ade80";
    public const int CurrentVersionId = 34;

    public static ChartStyle GetChartStyle(string chart) =>
        ChartStyles.GetValueOrDefault(chart, new ChartStyle("inherit", ""));

    public static string GetTitleColor(string chart, int? versionId)
    {
        if (versionId is CurrentVersionId) return NewestVersionColor;
        return GetChartStyle(chart).Color;
    }
}
