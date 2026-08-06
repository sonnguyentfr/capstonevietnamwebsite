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

    /// <summary>
    /// Extract numeric ID from end of slug pattern "some-text-123" -> 123
    /// </summary>
    public static int? ExtractIdFromSlug(string slug)
    {
        if (string.IsNullOrWhiteSpace(slug)) return null;

        var match = IdExtractRegex().Match(slug);
        if (match.Success && int.TryParse(match.Groups[1].Value, out var id))
            return id;

        return null;
    }

    /// <summary>
    /// Remove "-{id}" suffix from slug: "some-text-123" -> "some-text"
    /// </summary>
    public static string RemoveIdSuffix(string slug, int id)
    {
        if (string.IsNullOrWhiteSpace(slug)) return string.Empty;

        var suffix = $"-{id}";
        if (slug.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
            return slug[..^suffix.Length];

        return slug;
    }

    [GeneratedRegex(@"-(\d+)$")]
    private static partial Regex IdExtractRegex();
}