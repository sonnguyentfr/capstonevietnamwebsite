using System.Text;
using System.Text.RegularExpressions;

namespace Capstone.View.Helpers;

/// <summary>
/// Doc cookie cv_lt (last touch) / cv_ft (first touch) do script trong _Layout ghi, va dung thanh 1 dong
/// "(utm):last=facebook/paid-social/tuyen-sinh-t11; first=google/organic/(none); landing=/su-kien/x"
/// de gan vao ghi chu cua lead (CRM). Khong doi DB: dong nay di cung "(boot):..." trong TuVanKhac / NoiDung.
/// </summary>
public static class LeadAttribution
{
    private const string Marker = "(utm):";
    private const int MaxLength = 250;
    private static readonly Regex Unsafe = new(@"[^\p{L}\p{N}._~\-/():]", RegexOptions.Compiled);

    /// <summary>Tra ve text da them dong nguon (neu co cookie); khong them 2 lan.</summary>
    public static string? AppendTo(string? text, HttpRequest request)
    {
        if (!string.IsNullOrEmpty(text) && text.Contains(Marker, StringComparison.OrdinalIgnoreCase))
            return text;

        var line = BuildLine(request);
        if (line is null) return text;
        return string.IsNullOrWhiteSpace(text) ? line : text.TrimEnd() + "\n" + line;
    }

    public static string? BuildLine(HttpRequest request)
    {
        var last = Parse(request.Cookies["cv_lt"]);
        var first = Parse(request.Cookies["cv_ft"]);
        if (last is null && first is null) return null;

        var parts = new List<string>();
        if (last is not null)
        {
            parts.Add("last=" + Touch(last));
            if (last.TryGetValue("n", out var content)) parts.Add("content=" + content);
            if (last.TryGetValue("t", out var term)) parts.Add("term=" + term);
        }
        if (first is not null) parts.Add("first=" + Touch(first));

        var landing = (last ?? first)!.GetValueOrDefault("lp");
        if (!string.IsNullOrEmpty(landing)) parts.Add("landing=" + landing);

        var line = Marker + string.Join("; ", parts);
        return line.Length <= MaxLength ? line : line[..MaxLength];
    }

    private static string Touch(Dictionary<string, string> t) =>
        $"{t.GetValueOrDefault("s", "(none)")}/{t.GetValueOrDefault("m", "(none)")}/{t.GetValueOrDefault("c", "(none)")}";

    private static Dictionary<string, string>? Parse(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return null;
        string decoded;
        try { decoded = Uri.UnescapeDataString(raw); }
        catch (UriFormatException) { return null; }

        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var part in decoded.Split('&', StringSplitOptions.RemoveEmptyEntries))
        {
            var i = part.IndexOf('=');
            if (i <= 0) continue;
            var key = part[..i];
            var value = part[(i + 1)..];
            try { value = Uri.UnescapeDataString(value); } catch (UriFormatException) { }
            value = Clean(value);
            if (value.Length > 0) map[key] = value;
        }
        return map.Count > 0 ? map : null;
    }

    /// <summary>Cookie la du lieu tu client -> chi giu ky tu an toan, toi da 60 ky tu.</summary>
    private static string Clean(string value)
    {
        value = Unsafe.Replace(value.Trim().ToLowerInvariant(), string.Empty);
        return value.Length <= 60 ? value : value[..60];
    }
}
