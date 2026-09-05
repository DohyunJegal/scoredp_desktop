using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace ScoreDp.Desktop.Support;

public static partial class TitleNormalizer
{
    private static readonly Dictionary<string, string> Aliases = new()
    {
        ["ACTØ"] = "ACT0",
        ["CODE:Ø"] = "CODE:0",
        ["ÆTHER"] = "ATHER",
        ["BLO§OM"] = "BLOSSOM",
        ["火影"] = "焱影",
        ["POLꓘAMAИIA"] = "POLꞰAMAИIA",
        ["Τeλοs"] = "Τέλος",
        ["ZEИITH"] = "zenith2",
    };

    private static readonly Dictionary<char, string> Trans = new()
    {
        ['¡'] = "!",
        ['Ø'] = "O",
        ['ø'] = "o",
        ['Ʞ'] = "K",
        ['æ'] = "ae",
        ['Æ'] = "AE",
        ['Λ'] = "A",
        ['∧'] = "A",
        ['ə'] = "e",
        ['Χ'] = "X",
        ['χ'] = "x",
        ['ƒ'] = "f",
        ['<'] = "",
        ['>'] = "",
        ['И'] = "N",
    };

    [GeneratedRegex("[\\s\\t\\-_.'’\"“”()~〜～♡♥♪♫♬《》・·♨​﻿!]")]
    private static partial Regex StripPattern();

    public static string Normalize(string title)
    {
        if (Aliases.TryGetValue(title, out var alias)) title = alias;

        var sb = new StringBuilder();
        foreach (var c in title)
        {
            sb.Append(Trans.TryGetValue(c, out var repl) ? repl : c.ToString());
        }
        title = sb.ToString();

        title = title.Normalize(NormalizationForm.FormKD);
        title = new string(title.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray());
        title = title.ToLowerInvariant();
        title = StripPattern().Replace(title, "");
        return title;
    }
}
