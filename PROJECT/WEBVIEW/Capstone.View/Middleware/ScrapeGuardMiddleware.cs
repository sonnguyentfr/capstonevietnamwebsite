using Capstone.View.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Capstone.View.Middleware;

/// <summary>
/// Chống scrape dữ liệu trường (chạy SAU BotGuardMiddleware, sau UseRouting):
///  - Honeypot: link ẩn trong layout, Disallow trong robots.txt. Người dùng không thấy,
///    bot tốt không vào → ai vào là crawler bỏ qua robots.txt → ban IP N giờ.
///  - Quota / 24h / IP cho trang chi tiết trường và trang danh sách/tìm trường.
///    Người dùng thật hiếm khi xem quá vài chục trường/ngày; scraper cần lấy hết.
/// Bot search engine đã xác minh, IP whitelist, localhost được bỏ qua.
/// </summary>
public class ScrapeGuardMiddleware
{
    private sealed class Counter { public int Value; }

    private static readonly TimeSpan QuotaWindow = TimeSpan.FromHours(24);

    // (controller, action) được tính quota
    private static readonly HashSet<(string, string)> DetailActions = new(Comparer())
    {
        ("Truong", "Detail"),
    };

    private static readonly HashSet<(string, string)> ListActions = new(Comparer())
    {
        ("Truong", "Index"), ("Truong", "TimTruong"), ("Truong", "QuocGia"),
        ("Truong", "TimNganhHoc"), ("ThongTinDuHoc", "DanhSachTruong"),
    };

    private readonly RequestDelegate _next;
    private readonly ILogger<ScrapeGuardMiddleware> _logger;
    private readonly IMemoryCache _cache;
    private readonly RateLimitOptions _opt;

    public ScrapeGuardMiddleware(RequestDelegate next, ILogger<ScrapeGuardMiddleware> logger,
        IMemoryCache cache, IOptions<RateLimitOptions> opt)
    {
        _next   = next;
        _logger = logger;
        _cache  = cache;
        _opt    = opt.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Items.ContainsKey(BotGuardMiddleware.ExemptItem)
            || context.Items.ContainsKey(BotGuardMiddleware.VerifiedBotItem)
            || context.Items[BotGuardMiddleware.PartitionKeyItem] is not string key)
        {
            await _next(context);
            return;
        }

        var banKey = "scrape:ban:" + key;
        if (_cache.TryGetValue(banKey, out _))
        {
            await Forbidden(context);
            return;
        }

        if (!string.IsNullOrEmpty(_opt.HoneypotPath)
            && context.Request.Path.Equals(_opt.HoneypotPath, StringComparison.OrdinalIgnoreCase))
        {
            _cache.Set(banKey, true, TimeSpan.FromHours(_opt.HoneypotBanHours));
            _logger.LogWarning("Honeypot hit → ban {Key} {Hours}h UA={UserAgent}",
                key, _opt.HoneypotBanHours, context.Request.Headers.UserAgent.ToString());
            await Forbidden(context);
            return;
        }

        var route = (context.GetRouteValue("controller") as string ?? "",
                     context.GetRouteValue("action") as string ?? "");

        if (_opt.DailyDetailQuota > 0 && DetailActions.Contains(route)
            && Increment("scrape:d:" + key) > _opt.DailyDetailQuota)
        {
            await QuotaExceeded(context, key, "detail");
            return;
        }

        if (_opt.DailyListQuota > 0 && ListActions.Contains(route)
            && Increment("scrape:l:" + key) > _opt.DailyListQuota)
        {
            await QuotaExceeded(context, key, "list");
            return;
        }

        await _next(context);
    }

    /// <summary>Đếm trong cửa sổ cố định 24h kể từ lần truy cập đầu tiên.</summary>
    private int Increment(string cacheKey)
    {
        var counter = _cache.GetOrCreate(cacheKey, e =>
        {
            e.AbsoluteExpirationRelativeToNow = QuotaWindow;
            return new Counter();
        })!;
        return Interlocked.Increment(ref counter.Value);
    }

    private static Task Forbidden(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "text/plain; charset=utf-8";
        return context.Response.WriteAsync("Forbidden");
    }

    private Task QuotaExceeded(HttpContext context, string key, string kind)
    {
        _logger.LogWarning("Daily quota exceeded ({Kind}) {Key} {Path} UA={UserAgent}",
            kind, key, context.Request.Path, context.Request.Headers.UserAgent.ToString());

        context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.Response.Headers.RetryAfter = ((int)QuotaWindow.TotalSeconds).ToString();
        context.Response.ContentType = "text/html; charset=utf-8";
        return context.Response.WriteAsync("""
            <!doctype html><html lang="vi"><head><meta charset="utf-8">
            <meta name="viewport" content="width=device-width, initial-scale=1"><meta name="robots" content="noindex">
            <title>Capstone Vietnam</title></head>
            <body style="font-family:system-ui,sans-serif;max-width:560px;margin:15vh auto;padding:0 16px;text-align:center;color:#222">
            <h1 style="font-size:22px">Bạn đã xem rất nhiều trang trong hôm nay</h1>
            <p>Để được tư vấn chọn trường phù hợp nhanh hơn, hãy để lại thông tin — chuyên viên Capstone sẽ liên hệ với bạn.</p>
            <p><a href="/dang-ky-tu-van" style="display:inline-block;padding:10px 20px;background:#c8102e;color:#fff;border-radius:6px;text-decoration:none">Đăng ký tư vấn miễn phí</a></p>
            <p><a href="/">Về trang chủ</a></p>
            </body></html>
            """);
    }

    private static IEqualityComparer<(string, string)> Comparer() => new PairComparer();

    private sealed class PairComparer : IEqualityComparer<(string, string)>
    {
        public bool Equals((string, string) a, (string, string) b) =>
            string.Equals(a.Item1, b.Item1, StringComparison.OrdinalIgnoreCase)
            && string.Equals(a.Item2, b.Item2, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode((string, string) p) => HashCode.Combine(
            StringComparer.OrdinalIgnoreCase.GetHashCode(p.Item1),
            StringComparer.OrdinalIgnoreCase.GetHashCode(p.Item2));
    }
}
