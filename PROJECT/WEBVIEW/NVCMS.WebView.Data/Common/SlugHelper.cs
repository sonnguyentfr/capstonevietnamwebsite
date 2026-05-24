using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NVCMS.WebView.Data.Common;

public static partial class SlugHelper
{
    private static readonly Dictionary<char, char> VietnameseMap = new()
    {
        {'đ','d'}, {'Đ','d'}
    };

    public static string ToSlug(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return string.Empty;

        var sb = new StringBuilder();
        foreach (var c in input)
            sb.Append(VietnameseMap.TryGetValue(c, out var r) ? r : c);

        var normalized = sb.ToString()
            .Normalize(NormalizationForm.FormD);

        var clean = new StringBuilder();
        foreach (var c in normalized)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                clean.Append(c);

        return SlugRegex()
            .Replace(clean.ToString()
                .Normalize(NormalizationForm.FormC)
                .ToLowerInvariant(), "-")
            .Trim('-');
    }

    [GeneratedRegex(@"[^a-z0-9]+")]
    private static partial Regex SlugRegex();
}