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

    /// <summary>Trang tìm trường / lọc.</summary>
    public int SearchPerMinute { get; set; } = 30;

    /// <summary>Trang chi tiết trường (mục tiêu chính của scraper).</summary>
    public int DetailPerMinute { get; set; } = 20;

    /// <summary>Form POST (đăng ký tư vấn, sự kiện...).</summary>
    public int FormPerMinute { get; set; } = 3;

    /// <summary>Beacon thống kê banner (click / view).</summary>
    public int TrackPerMinute { get; set; } = 30;

    /// <summary>Bot search engine đã xác minh qua reverse DNS (Googlebot, Bingbot...).</summary>
    public int VerifiedBotPerMinute { get; set; } = 300;

    /// <summary>Chặn 403 request không có User-Agent.</summary>
    public bool BlockEmptyUserAgent { get; set; } = true;

    /// <summary>
    /// UA xưng là Googlebot/Bingbot... nhưng reverse DNS không khớp: true = chặn 403, false = cho qua
    /// như khách thường (vẫn chịu rate limit / quota). Để false cho an toàn SEO: nếu server không
    /// thấy đúng IP thật hoặc không tra được DNS thì Googlebot thật cũng bị coi là giả.
    /// </summary>
    public bool BlockUnverifiedSearchBots { get; set; } = false;

    /// <summary>
    /// UA chứa 1 trong các chuỗi này (chữ thường) KHÔNG bị chặn theo BlockedUserAgents:
    /// bot lấy preview khi share link lên mạng xã hội / app chat. Vẫn chịu rate limit như khách thường.
    /// </summary>
    public string[] AllowedUserAgents { get; set; } =
    [
        "facebookexternalhit", "facebookcatalog", "zalo", "linkedinbot", "twitterbot",
        "telegrambot", "whatsapp", "slackbot", "discordbot", "skypeuripreview",
        "pinterest", "viber", "line-poker", "redditbot"
    ];

    /// <summary>Header chứa IP thật của khách do proxy gắn (Cloudflare: CF-Connecting-IP). Rỗng = tắt.</summary>
    public string ClientIpHeader { get; set; } = "CF-Connecting-IP";

    /// <summary>
    /// Dải IP proxy tin cậy: chỉ khi kết nối đến từ các dải này mới đọc ClientIpHeader.
    /// Mặc định = dải IP Cloudflare (https://www.cloudflare.com/ips-v4 và /ips-v6, lấy ngày 2026-11-20).
    /// Lưu ý: khai báo mục này trong appsettings sẽ ghi đè theo từng vị trí, nên phải ghi đủ cả danh sách.
    /// </summary>
    public string[] TrustedProxyNetworks { get; set; } =
    [
        "173.245.48.0/20", "103.21.244.0/22", "103.22.200.0/22", "103.31.4.0/22",
        "141.101.64.0/18", "108.162.192.0/18", "190.93.240.0/20", "188.114.96.0/20",
        "197.234.240.0/22", "198.41.128.0/17", "162.158.0.0/15", "104.16.0.0/13",
        "104.24.0.0/14", "172.64.0.0/13", "131.0.72.0/22",
        "2400:cb00::/32", "2606:4700::/32", "2803:f800::/32", "2405:b500::/32",
        "2405:8100::/32", "2a06:98c0::/29", "2c0f:f248::/32"
    ];

    /// <summary>User-Agent chứa 1 trong các chuỗi này (không phân biệt hoa thường) sẽ bị chặn 403.</summary>
    public string[] BlockedUserAgents { get; set; } =
    [
        "python-requests", "python-urllib", "aiohttp", "httpx", "scrapy",
        "curl/", "wget/", "go-http-client", "okhttp", "java/", "apache-httpclient",
        "libwww-perl", "node-fetch", "axios/", "undici", "colly", "httrack",
        "headlesschrome", "phantomjs", "puppeteer", "playwright", "selenium",
        "bytespider", "ahrefsbot", "semrushbot", "mj12bot", "dotbot", "petalbot",
        "blexbot", "dataforseobot", "serpstatbot", "megaindex", "ccbot",
        // Bot AI thu thập dữ liệu (không ảnh hưởng Google Search / Bing).
        // KHÔNG chặn "facebookexternalhit" / Zalo (preview link khi share).
        "gptbot", "chatgpt-user", "oai-searchbot", "claudebot", "claude-web", "claude-user",
        "claude-searchbot", "anthropic-ai", "perplexitybot", "perplexity-user",
        "meta-externalagent", "meta-externalfetcher", "facebookbot", "amazonbot",
        "cohere-ai", "cohere-training-data-crawler", "diffbot", "youbot", "ai2bot",
        "imagesiftbot", "omgilibot", "timpibot", "pangubot", "img2dataset", "duckassistbot",
        "mistralai-user", "firecrawl", "jina", "crawl4ai"
    ];

    /// <summary>
    /// IP không bị rate limit / quota / honeypot (văn phòng Capstone...).
    /// Ví dụ: ["113.161.10.20", "2402:800:1234::/64"] — IPv6 ghi theo dạng khoá /64.
    /// </summary>
    public string[] WhitelistIps { get; set; } = [];

    /// <summary>Số trang chi tiết trường / IP / 24 giờ. 0 = tắt.</summary>
    public int DailyDetailQuota { get; set; } = 100;

    /// <summary>Số trang danh sách / tìm trường / ngành / IP / 24 giờ. 0 = tắt.</summary>
    public int DailyListQuota { get; set; } = 300;

    /// <summary>Path bẫy: link ẩn trong layout + Disallow trong robots.txt. Ai vào = bot → ban.</summary>
    public string HoneypotPath { get; set; } = "/truong-doi-tac/xuat-du-lieu-truong";

    /// <summary>Thời gian ban IP dính bẫy (giờ).</summary>
    public int HoneypotBanHours { get; set; } = 24;

    /// <summary>Path bắt đầu bằng các prefix này không bị chặn UA / rate limit (API nội bộ, tracking email).</summary>
    public string[] ExemptPathPrefixes { get; set; } = ["/api/cache", "/api/emailtracking"];
}
