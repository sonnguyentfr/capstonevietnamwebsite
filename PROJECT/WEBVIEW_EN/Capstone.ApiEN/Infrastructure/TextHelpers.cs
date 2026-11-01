using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Capstone.ApiEN.Infrastructure;

/// <summary>URL-safe ASCII slugs, also for Vietnamese titles ("Đại học Sheffield" → "dai-hoc-sheffield").</summary>
public static partial class SlugText
{
    [GeneratedRegex("[^a-z0-9]+")]
    private static partial Regex NonAlnum();

    public static string From(string? text, int maxLength = 90)
    {
        if (string.IsNullOrWhiteSpace(text)) return "item";
        var normalized = text.Replace('đ', 'd').Replace('Đ', 'D').Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder(normalized.Length);
        foreach (var c in normalized)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        var slug = NonAlnum().Replace(sb.ToString().ToLowerInvariant(), "-").Trim('-');
        if (slug.Length > maxLength) slug = slug[..maxLength].TrimEnd('-');
        return slug.Length == 0 ? "item" : slug;
    }
}

/// <summary>Turns database file paths (/DATA/..., /Portals/...) into absolute file-server URLs. Same rules as the Vietnamese site.</summary>
public sealed partial class FileUrls(string fileServerBaseUrl)
{
    private readonly string _base = fileServerBaseUrl.TrimEnd('/');
    private static readonly string[] Prefixes = ["/data/", "/DATA/", "/Portals/"];

    [GeneratedRegex(@"(src|href|poster)\s*=\s*([""'])(/[Dd]ata/|/Portals/)([^""']*)\2", RegexOptions.IgnoreCase)]
    private static partial Regex AttrPattern();

    public string? Url(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return null;
        if (path.StartsWith("http", StringComparison.OrdinalIgnoreCase)) return path;
        foreach (var prefix in Prefixes)
            if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return $"{_base}/{path[prefix.Length..]}";
        return path.StartsWith('/') ? _base + path : $"{_base}/{path}";
    }

    public string? Html(string? html)
        => string.IsNullOrEmpty(html)
            ? html
            : AttrPattern().Replace(html, m => $"{m.Groups[1].Value}={m.Groups[2].Value}{_base}/{m.Groups[4].Value}{m.Groups[2].Value}");
}

public static partial class PlainText
{
    [GeneratedRegex("<[^>]+>")]
    private static partial Regex Tags();

    [GeneratedRegex(@"\s+")]
    private static partial Regex Spaces();

    /// <summary>Strips HTML and shortens to a meta-description sized summary.</summary>
    public static string? Summary(string? html, int max = 240)
    {
        if (string.IsNullOrWhiteSpace(html)) return null;
        var text = Spaces().Replace(WebUtility.HtmlDecode(Tags().Replace(html, " ")), " ").Trim();
        if (text.Length <= max) return text;
        var cut = text.LastIndexOf(' ', max);
        return text[..(cut > max / 2 ? cut : max)].TrimEnd(',', '.', ';', ':') + "…";
    }

    /// <summary>"12, 34 ,56" → [12, 34, 56] (school/org ids stored as CSV in NV_Events and NV_Events_Cat).</summary>
    public static List<int> Ids(params string?[] csvs)
    {
        var seen = new HashSet<int>();
        var result = new List<int>();
        foreach (var csv in csvs)
        {
            if (string.IsNullOrWhiteSpace(csv)) continue;
            foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                if (int.TryParse(part, out var id) && seen.Add(id))
                    result.Add(id);
        }
        return result;
    }

    public static string? FirstNonEmpty(params string?[] values) => values.FirstOrDefault(v => !string.IsNullOrWhiteSpace(v))?.Trim();
}

public static class VnTime
{
    private static readonly TimeSpan Offset = TimeSpan.FromHours(7);

    /// <summary>Database datetimes are Vietnam local time without offset.</summary>
    public static DateTimeOffset From(DateTime value) => new(DateTime.SpecifyKind(value, DateTimeKind.Unspecified), Offset);

    public static DateTimeOffset? From(DateTime? value) => value is null ? null : From(value.Value);
}
