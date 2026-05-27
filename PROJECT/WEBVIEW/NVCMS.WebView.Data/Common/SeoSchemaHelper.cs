using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NVCMS.WebView.Data.Common;

/// <summary>
/// Tạo Google Structured Data (JSON-LD) dùng chung cho nhiều loại trang:
/// trang chi tiết trường, sự kiện, giới thiệu.
/// Đặt output trong thẻ &lt;script type="application/ld+json"&gt;.
/// </summary>
public static class SeoSchemaHelper
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented             = false,
        Encoder                   = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        DefaultIgnoreCondition    = JsonIgnoreCondition.WhenWritingNull,
    };

    // ─────────────────────────────────────────────────────────────────────────
    // CollegeOrUniversity  –  trang chi tiết trường
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Schema CollegeOrUniversity cho trang chi tiết một trường đối tác.</summary>
    public static string CollegeOrUniversity(
        string  name,
        string? description   = null,
        string? url           = null,
        string? logoUrl       = null,
        string? imageUrl      = null,
        string? address       = null,
        string? country       = null,
        string? telephone     = null,
        string? email         = null,
        string? foundingYear  = null,
        IEnumerable<string>? sameAs = null)
    {
        var obj = new Dictionary<string, object?> {
            ["@context"] = "https://schema.org",
            ["@type"]    = "CollegeOrUniversity",
            ["name"]     = name,
        };

        if (!string.IsNullOrWhiteSpace(description)) obj["description"] = description;
        if (!string.IsNullOrWhiteSpace(url))          obj["url"]         = url;
        if (!string.IsNullOrWhiteSpace(telephone))    obj["telephone"]   = telephone;
        if (!string.IsNullOrWhiteSpace(email))        obj["email"]       = email;
        if (!string.IsNullOrWhiteSpace(foundingYear)) obj["foundingDate"] = foundingYear;

        if (!string.IsNullOrWhiteSpace(logoUrl))
            obj["logo"] = new Dictionary<string, string> { ["@type"] = "ImageObject", ["url"] = logoUrl };

        if (!string.IsNullOrWhiteSpace(imageUrl))
            obj["image"] = new Dictionary<string, string> { ["@type"] = "ImageObject", ["url"] = imageUrl };

        if (!string.IsNullOrWhiteSpace(address) || !string.IsNullOrWhiteSpace(country))
        {
            var postal = new Dictionary<string, string?> { ["@type"] = "PostalAddress" };
            if (!string.IsNullOrWhiteSpace(address)) postal["streetAddress"]  = address;
            if (!string.IsNullOrWhiteSpace(country)) postal["addressCountry"] = country;
            obj["address"] = postal;
        }

        var sameAsList = sameAs?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
        if (sameAsList is { Count: > 0 }) obj["sameAs"] = sameAsList;

        return Wrap(obj);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Event  –  trang chi tiết sự kiện / hội thảo
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Schema Event cho trang chi tiết sự kiện.</summary>
    public static string Event(
        string    name,
        DateTime? startDate      = null,
        DateTime? endDate        = null,
        string?   description    = null,
        string?   url            = null,
        string?   imageUrl       = null,
        string?   organizerName  = null,
        string?   organizerUrl   = null,
        string?   locationName   = null,
        string?   locationAddress = null,
        bool      isOnline       = false)
    {
        var obj = new Dictionary<string, object?> {
            ["@context"]    = "https://schema.org",
            ["@type"]       = "Event",
            ["name"]        = name,
            ["eventStatus"] = "https://schema.org/EventScheduled",
        };

        if (startDate.HasValue)
            obj["startDate"] = startDate.Value.ToString("yyyy-MM-ddTHH:mm:ss+07:00");
        if (endDate.HasValue)
            obj["endDate"] = endDate.Value.ToString("yyyy-MM-ddTHH:mm:ss+07:00");

        if (!string.IsNullOrWhiteSpace(description)) obj["description"] = description;
        if (!string.IsNullOrWhiteSpace(url))          obj["url"]         = url;

        if (!string.IsNullOrWhiteSpace(imageUrl))
            obj["image"] = new Dictionary<string, string> { ["@type"] = "ImageObject", ["url"] = imageUrl };

        if (!string.IsNullOrWhiteSpace(organizerName))
        {
            var org = new Dictionary<string, string?> { ["@type"] = "Organization", ["name"] = organizerName };
            if (!string.IsNullOrWhiteSpace(organizerUrl)) org["url"] = organizerUrl;
            obj["organizer"] = org;
        }

        if (isOnline)
        {
            obj["eventAttendanceMode"] = "https://schema.org/OnlineEventAttendanceMode";
            obj["location"] = new Dictionary<string, string>
            {
                ["@type"] = "VirtualLocation",
                ["url"]   = url ?? string.Empty,
            };
        }
        else if (!string.IsNullOrWhiteSpace(locationName) || !string.IsNullOrWhiteSpace(locationAddress))
        {
            obj["eventAttendanceMode"] = "https://schema.org/OfflineEventAttendanceMode";
            var loc = new Dictionary<string, object?> { ["@type"] = "Place" };
            if (!string.IsNullOrWhiteSpace(locationName)) loc["name"] = locationName;
            if (!string.IsNullOrWhiteSpace(locationAddress))
                loc["address"] = new Dictionary<string, string>
                {
                    ["@type"]         = "PostalAddress",
                    ["streetAddress"] = locationAddress,
                };
            obj["location"] = loc;
        }

        return Wrap(obj);
    }

    // ─────────────────────────────────────────────────────────────────────────
    // WebPage  –  trang giới thiệu / trang tĩnh
    // ─────────────────────────────────────────────────────────────────────────

    /// <summary>Schema WebPage / AboutPage dùng cho trang giới thiệu.</summary>
    public static string WebPage(
        string  name,
        string? description   = null,
        string? url           = null,
        string? imageUrl      = null,
        string? publisherName = null,
        string? publisherUrl  = null,
        bool    isAboutPage   = false)
    {
        var obj = new Dictionary<string, object?> {
            ["@context"] = "https://schema.org",
            ["@type"]    = isAboutPage ? "AboutPage" : "WebPage",
            ["name"]     = name,
        };

        if (!string.IsNullOrWhiteSpace(description)) obj["description"] = description;
        if (!string.IsNullOrWhiteSpace(url))          obj["url"]         = url;

        if (!string.IsNullOrWhiteSpace(imageUrl))
            obj["image"] = new Dictionary<string, string> { ["@type"] = "ImageObject", ["url"] = imageUrl };

        if (!string.IsNullOrWhiteSpace(publisherName))
        {
            var pub = new Dictionary<string, string?> { ["@type"] = "Organization", ["name"] = publisherName };
            if (!string.IsNullOrWhiteSpace(publisherUrl)) pub["url"] = publisherUrl;
            obj["publisher"] = pub;
        }

        return Wrap(obj);
    }

    // ─────────────────────────────────────────────────────────────────────────

    private static string Wrap(object data)
    {
        var json = JsonSerializer.Serialize(data, JsonOptions);
        return $"<script type=\"application/ld+json\">{json}</script>";
    }
}
