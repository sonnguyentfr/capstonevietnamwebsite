using System.Globalization;
using System.Text.RegularExpressions;
using Capstone.Content.Models;

namespace Capstone.ViewEN.Helpers;

/// <summary>Canonical URLs for content. Detail URLs follow /{section}/{slug}-{id}, like the Vietnamese site.</summary>
public static class Links
{
    public static string School(SchoolDto s) => $"/partners/{s.Slug}-{s.Id}";
    public static string Event(EventDto e) => $"/events/{e.Slug}-{e.Id}";
    public static string News(NewsDto n) => $"/news/{n.Slug}-{n.Id}";
    public static string Service(ServiceDto s) => $"/services/{s.Slug}";
    public static string PartnersIn(string countrySlug) => $"/partners?country={Uri.EscapeDataString(countrySlug)}";
}

/// <summary>Parses the "{slug}-{id}" route segment (Blazor routes cannot express it directly).</summary>
public static partial class SlugId
{
    [GeneratedRegex(@"^(?<slug>.*?)-?(?<id>\d+)$")]
    private static partial Regex Pattern();

    public static bool TryParse(string? value, out string slug, out int id)
    {
        slug = string.Empty;
        id = 0;
        if (string.IsNullOrWhiteSpace(value)) return false;
        // Old site URLs end in ".html" (/news/some-title-5958.html). Accept them; the detail page then
        // redirects to the canonical URL when the slug differs (and sets a canonical link anyway).
        if (value.EndsWith(".html", StringComparison.OrdinalIgnoreCase)) value = value[..^5];
        var m = Pattern().Match(value);
        if (!m.Success || !int.TryParse(m.Groups["id"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out id))
            return false;
        slug = m.Groups["slug"].Value;
        return id > 0;
    }
}

public static class Fmt
{
    private static readonly CultureInfo En = CultureInfo.GetCultureInfo("en-GB");

    public static string Day(DateTimeOffset d) => d.ToString("dd", En);
    public static string MonthYear(DateTimeOffset d) => d.ToString("MMM yyyy", En);
    public static string Month(DateTimeOffset d) => d.ToString("MMMM", En);
    public static string Date(DateTimeOffset d) => d.ToString("d MMM yyyy", En);
    public static string Weekday(DateTimeOffset d) => d.ToString("ddd d MMM", En);
    public static string Time(DateTimeOffset d) => d.ToString("HH:mm", En);

    public static string TimeRange(DateTimeOffset start, DateTimeOffset? end)
        => end is null ? Time(start) : $"{Time(start)}–{Time(end.Value)}";

    /// <summary>"14" or "14–15" when the event spans days in the same month.</summary>
    public static string DayRange(DateTimeOffset start, DateTimeOffset? end)
        => end is { } e && e.Date != start.Date && e.Month == start.Month ? $"{start.Day}–{e.Day}" : start.Day.ToString(En);

    public static string DateRange(DateTimeOffset start, DateTimeOffset? end)
    {
        if (end is not { } e || e.Date == start.Date)
            return $"{start.ToString("dddd d MMMM yyyy", En)}, {TimeRange(start, end)}";
        return $"{start.ToString("d MMM", En)} – {e.ToString("d MMM yyyy", En)}";
    }

    /// <summary>Two-letter monogram used when a school has no logo. Skips generic words.</summary>
    public static string Monogram(string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "?";
        string[] generic = ["the", "of", "and", "university", "college", "school", "academy", "institute", "community", "high", "international"];
        var words = name.Split(new[] { ' ', ',', '-', '\'' }, StringSplitOptions.RemoveEmptyEntries)
            .Where(w => char.IsLetter(w[0]) && !generic.Contains(w, StringComparer.OrdinalIgnoreCase))
            .ToList();
        if (words.Count == 0)
            words = name.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        // One meaningful word → its initial ("Northeastern University" → "N"); otherwise two initials.
        return words.Count == 1
            ? char.ToUpperInvariant(words[0][0]).ToString()
            : string.Concat(words.Take(2).Select(w => char.ToUpperInvariant(w[0])));
    }

    public static string FirstPart(string? city) => city?.Split(',')[0].Trim() ?? string.Empty;
}
