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
        IEnumerable<string>? sameAs = null,
        string  schemaType    = "CollegeOrUniversity")
    {
        var obj = new Dictionary<string, object?> {
            ["@context"] = "https://schema.org",
            ["@type"]    = schemaType,
            ["name"]     = name,
        };

        if (!string.IsNullOrWhiteSpace(description)) obj["description"] = description;
        if (!string.IsNullOrWhiteSpace(url))          obj["url"]         = url;
        if (!string.IsNullOrWhiteSpace(telephone))    obj["telephone"]   = telephone;
        if (!string.IsNullOrWhiteSpace(email))        obj["email"]       = email;
        if (!string.IsNullOrWhiteSpace(foundingYear)) obj["foundingDate"] = foundingYear;

        if (!string.IsNullOrWhiteSpace(logoUrl))
            obj["logo"] = new Dictionary<string, string> { ["@type"] = "ImageObject", ["url"] = CleanUrl(logoUrl) };

        if (!string.IsNullOrWhiteSpace(imageUrl))
            obj["image"] = new Dictionary<string, string> { ["@type"] = "ImageObject", ["url"] = CleanUrl(imageUrl) };

        if (!string.IsNullOrWhiteSpace(address) || !string.IsNullOrWhiteSpace(country))
            obj["address"] = PostalAddress(address, country);

        var sameAsList = sameAs?.Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        if (sameAsList is { Count: > 0 }) obj["sameAs"] = sameAsList;

        return Wrap(obj);
    }

    // ──────────────────────────────────────────────────────────────
    // Địa chỉ: mã quốc gia ISO + tách thành phố / bang / mã bưu chính
    // ──────────────────────────────────────────────────────────────

    // Tên quốc gia (như hiển thị trên site) → mã ISO 3166-1 alpha-2 mà Google yêu cầu ở addressCountry
    private static readonly Dictionary<string, string> CountryCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Mỹ"] = "US", ["My"] = "US", ["Hoa Kỳ"] = "US", ["USA"] = "US", ["United States"] = "US",
        ["Úc"] = "AU", ["Uc"] = "AU", ["Australia"] = "AU",
        ["Canada"] = "CA",
        ["Anh"] = "GB", ["Vương quốc Anh"] = "GB", ["UK"] = "GB", ["United Kingdom"] = "GB",
        ["Thụy Sĩ"] = "CH", ["Thuy Si"] = "CH", ["Switzerland"] = "CH",
        ["New Zealand"] = "NZ",
        ["Ireland"] = "IE",
    };

    private static readonly System.Text.RegularExpressions.Regex ZipOnly =
        new(@"^(\d{5}(-\d{4})?|\d{4}|[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d|[A-Za-z]{1,2}\d[A-Za-z\d]?\s?\d[A-Za-z]{2})$");

    // "WI 53144", "Ontario M5V 2T6"
    private static readonly System.Text.RegularExpressions.Regex RegionZip =
        new(@"^(?<region>[A-Za-z][A-Za-z .]*?)\s+(?<zip>\d{5}(-\d{4})?|\d{4}|[A-Za-z]\d[A-Za-z]\s?\d[A-Za-z]\d)$");

    // "Sydney NSW 2000"
    private static readonly System.Text.RegularExpressions.Regex AuTail =
        new(@"^(?<city>.+?)\s+(?<region>NSW|VIC|QLD|WA|SA|TAS|ACT|NT)\s+(?<zip>\d{4})$");

    /// <summary>Đổi tên quốc gia sang mã ISO 2 chữ; không nhận ra thì trả lại nguyên tên.</summary>
    public static string? CountryCode(string? country)
    {
        if (string.IsNullOrWhiteSpace(country)) return null;
        var key = country.Trim();
        return CountryCodes.TryGetValue(key, out var code) ? code : key;
    }

    /// <summary>
    /// Tạo PostalAddress từ địa chỉ dạng một dòng, vd. "9026 12th Street, Kenosha, WI 53144"
    /// → streetAddress / addressLocality / addressRegion / postalCode. Chỉ tách khi nhận ra
    /// chắc chắn phần đuôi "bang + mã bưu chính"; không thì giữ nguyên cả dòng ở streetAddress.
    /// </summary>
    private static Dictionary<string, string?> PostalAddress(string? address, string? country)
    {
        var postal = new Dictionary<string, string?> { ["@type"] = "PostalAddress" };
        var code   = CountryCode(country);

        if (!string.IsNullOrWhiteSpace(address))
        {
            var parts = address.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();

            // Bỏ tên quốc gia ở cuối địa chỉ ("..., USA")
            if (parts.Count > 1 && CountryCodes.ContainsKey(parts[^1]))
            {
                code ??= CountryCodes[parts[^1]];
                parts.RemoveAt(parts.Count - 1);
            }

            string? locality = null, region = null, zip = null;
            var streetCount = parts.Count;

            if (parts.Count >= 2 && AuTail.Match(parts[^1]) is { Success: true } au)
            {
                locality = au.Groups["city"].Value; region = au.Groups["region"].Value; zip = au.Groups["zip"].Value;
                streetCount = parts.Count - 1;
            }
            else if (parts.Count >= 3 && RegionZip.Match(parts[^1]) is { Success: true } rz)
            {
                region = rz.Groups["region"].Value; zip = rz.Groups["zip"].Value; locality = parts[^2];
                streetCount = parts.Count - 2;
            }
            else if (parts.Count >= 4 && ZipOnly.IsMatch(parts[^1]))
            {
                zip = parts[^1]; region = parts[^2]; locality = parts[^3];
                streetCount = parts.Count - 3;
            }
            else if (parts.Count >= 3 && ZipOnly.IsMatch(parts[^1]))
            {
                zip = parts[^1]; locality = parts[^2];
                streetCount = parts.Count - 2;
            }

            postal["streetAddress"] = zip is null ? address.Trim() : string.Join(", ", parts.Take(streetCount));
            if (!string.IsNullOrWhiteSpace(locality)) postal["addressLocality"] = locality;
            if (!string.IsNullOrWhiteSpace(region))   postal["addressRegion"]   = region;
            if (!string.IsNullOrWhiteSpace(zip))      postal["postalCode"]      = zip;
        }

        if (!string.IsNullOrWhiteSpace(code)) postal["addressCountry"] = code;
        return postal;
    }

    /// <summary>Loại schema theo mã Loai của trường: HS/BF → HighSchool, ESL → EducationalOrganization, còn lại → CollegeOrUniversity.</summary>
    public static string SchoolSchemaType(string? loai) => loai switch
    {
        "HS" or "BF" => "HighSchool",
        "ESL"        => "EducationalOrganization",
        _            => "CollegeOrUniversity",
    };

    /// <summary>URL có khoảng trắng (tên file upload) → mã hoá để Google tải được ảnh.</summary>
    private static string CleanUrl(string url) => url.Trim().Replace(" ", "%20");

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

    // ──────────────────────────────────────────────────────────────
    // NewsArticle  –  trang chi tiết tin tức
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Schema NewsArticle cho trang chi tiết tin. Thời gian là giờ Việt Nam (+07:00).
    /// Tác giả: tên hợp lệ → Person; rỗng / rác (vd. "&amp;#1072;") → Organization là chính
    /// đơn vị xuất bản (có url), để Google không nhận tên tác giả vô nghĩa.
    /// </summary>
    public static string NewsArticle(
        string    headline,
        string    url,
        DateTime  datePublished,
        string?   description      = null,
        string?   imageUrl         = null,
        DateTime? dateModified     = null,
        string?   authorName       = null,
        string?   section          = null,
        string    publisherName    = "Capstone Vietnam",
        string?   publisherUrl     = null,
        string?   publisherLogoUrl = null)
    {
        var obj = new Dictionary<string, object?> {
            ["@context"]      = "https://schema.org",
            ["@type"]         = "NewsArticle",
            ["headline"]      = headline,
            ["inLanguage"]    = "vi",
            ["datePublished"] = datePublished.ToString("yyyy-MM-ddTHH:mm:ss+07:00"),
            ["dateModified"]  = (dateModified ?? datePublished).ToString("yyyy-MM-ddTHH:mm:ss+07:00"),
        };

        if (!string.IsNullOrWhiteSpace(description)) obj["description"]    = description;
        if (!string.IsNullOrWhiteSpace(section))     obj["articleSection"] = section;
        if (!string.IsNullOrWhiteSpace(imageUrl))    obj["image"]          = new[] { imageUrl };

        var author = System.Net.WebUtility.HtmlDecode(authorName ?? string.Empty).Trim();
        if (author.Count(char.IsLetter) >= 2)
        {
            obj["author"] = new Dictionary<string, string?> { ["@type"] = "Person", ["name"] = author };
        }
        else
        {
            var org = new Dictionary<string, string?> { ["@type"] = "Organization", ["name"] = publisherName };
            if (!string.IsNullOrWhiteSpace(publisherUrl)) org["url"] = publisherUrl;
            obj["author"] = org;
        }

        var publisher = new Dictionary<string, object?> { ["@type"] = "Organization", ["name"] = publisherName };
        if (!string.IsNullOrWhiteSpace(publisherUrl)) publisher["url"] = publisherUrl;
        if (!string.IsNullOrWhiteSpace(publisherLogoUrl))
            publisher["logo"] = new Dictionary<string, string> { ["@type"] = "ImageObject", ["url"] = publisherLogoUrl };
        obj["publisher"] = publisher;

        obj["mainEntityOfPage"] = new Dictionary<string, string> { ["@type"] = "WebPage", ["@id"] = url };

        return Wrap(obj);
    }

    // ──────────────────────────────────────────────────────────────
    // BreadcrumbList  –  dùng chung cho mọi trang có breadcrumb
    // ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Schema BreadcrumbList. Truyền các cấp theo thứ tự Trang chủ → trang hiện tại,
    /// Url phải là URL tuyệt đối. Cấp nào tên rỗng sẽ bị bỏ qua.
    /// </summary>
    public static string BreadcrumbList(IEnumerable<(string Name, string? Url)> items)
    {
        var list = new List<Dictionary<string, object?>>();
        foreach (var (name, itemUrl) in items)
        {
            if (string.IsNullOrWhiteSpace(name)) continue;
            var el = new Dictionary<string, object?> {
                ["@type"]    = "ListItem",
                ["position"] = list.Count + 1,
                ["name"]     = name.Trim(),
            };
            if (!string.IsNullOrWhiteSpace(itemUrl)) el["item"] = itemUrl;
            list.Add(el);
        }

        var obj = new Dictionary<string, object?> {
            ["@context"]        = "https://schema.org",
            ["@type"]           = "BreadcrumbList",
            ["itemListElement"] = list,
        };

        return Wrap(obj);
    }

    // ─────────────────────────────────────────────────────────────────────────

    private static string Wrap(object data)
    {
        // "</" -> "<\/": JSON vẫn hợp lệ, tránh nội dung CMS chứa "</script>" làm vỡ thẻ script
        var json = JsonSerializer.Serialize(data, JsonOptions).Replace("</", "<\\/");
        return $"<script type=\"application/ld+json\">{json}</script>";
    }
}
