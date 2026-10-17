namespace Capstone.View.Options;

/// <summary>
/// Cấu hình chống scrape / rate limit. Đọc từ appsettings "RateLimiting".
/// Tất cả giới hạn tính theo IP (IPv6 gộp theo /64), cửa sổ 1 phút.
/// </summary>
public class RateLimitOptions
{
    public const string SectionName = "RateLimiting";

    public bool Enabled { get; set; } = true;

    /// <summary>Mọi request động (không tính file tĩnh) / phút / IP.</summary>
    public int GlobalPerMinute { get; set; } = 60;

    /// <summary>Số request xử lý đồng thời tối đa / IP (chống flood song song).</summary>
    public int ConcurrentPerIp { get; set; } = 8;

    /// <summary>Trang tìm trường / lọc / search-json.</summary>
    public int SearchPerMinute { get; set; } = 30;

    /// <summary>Trang chi tiết trường (mục tiêu chính của scraper).</summary>
    public int DetailPerMinute { get; set; } = 20;

    /// <summary>Form POST (đăng ký tư vấn, sự kiện...).</summary>
    public int FormPerMinute { get; set; } = 3;

    /// <summary>Bot search engine đã xác minh qua reverse DNS (Googlebot, Bingbot...).</summary>
    public int VerifiedBotPerMinute { get; set; } = 300;

    /// <summary>Chặn 403 request không có User-Agent.</summary>
    public bool BlockEmptyUserAgent { get; set; } = true;

    /// <summary>User-Agent chứa 1 trong các chuỗi này (không phân biệt hoa thường) sẽ bị chặn 403.</summary>
    public string[] BlockedUserAgents { get; set; } =
    [
        "python-requests", "python-urllib", "aiohttp", "httpx", "scrapy",
        "curl/", "wget/", "go-http-client", "okhttp", "java/", "apache-httpclient",
        "libwww-perl", "node-fetch", "axios/", "undici", "colly", "httrack",
        "headlesschrome", "phantomjs", "puppeteer", "playwright", "selenium",
        "bytespider", "ahrefsbot", "semrushbot", "mj12bot", "dotbot", "petalbot",
        "blexbot", "dataforseobot", "serpstatbot", "megaindex", "ccbot"
    ];

    /// <summary>Path bắt đầu bằng các prefix này không bị chặn UA / rate limit (API nội bộ, tracking email).</summary>
    public string[] ExemptPathPrefixes { get; set; } = ["/api/cache", "/api/emailtracking"];
}
