using Microsoft.Extensions.Caching.Memory;
using NVCMS.WebView.Data.Contracts.Repository;

namespace Capstone.View.Middleware;

/// <summary>
/// Middleware ưu tiên cao nhất: nếu path segment đầu tiên khớp short_url trong DB
/// thì redirect 301 tới real_url. Đặt TRƯỚC UseRouting() trong Program.cs.
/// </summary>
public class ShortUrlMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ShortUrlMiddleware> _logger;
    private readonly IMemoryCache _cache;

    private const string CachePrefix   = "shorty:";
    private const string MissSentinel  = "__MISS__";
    private static readonly TimeSpan MissTtl = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan HitTtl  = TimeSpan.FromMinutes(30);

    public ShortUrlMiddleware(RequestDelegate next, ILogger<ShortUrlMiddleware> logger, IMemoryCache cache)
    {
        _next   = next;
        _logger = logger;
        _cache  = cache;
    }

    public async Task InvokeAsync(HttpContext context, IShortyUrlRepository repo)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        var slug = path.TrimStart('/');

        // Chỉ xử lý path đơn cấp, không rỗng, không phải file tĩnh (có extension)
        if (string.IsNullOrEmpty(slug) || slug.Contains('/') || slug.Contains('.'))
        {
            await _next(context);
            return;
        }

        var cacheKey = CachePrefix + slug.ToLowerInvariant();

        if (!_cache.TryGetValue(cacheKey, out string? cached))
        {
            try
            {
                cached = await repo.GetRealUrlAsync(slug) ?? MissSentinel;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "ShortUrl DB lookup failed for '{Slug}', skipping.", slug);
                cached = MissSentinel;
            }

            _cache.Set(cacheKey, cached, cached == MissSentinel ? MissTtl : HitTtl);
        }

        if (cached is not null && cached != MissSentinel)
        {
            _ = Task.Run(async () =>
            {
                try   { await repo.IncrementClickAsync(slug); }
                catch (Exception ex) { _logger.LogWarning(ex, "ShortUrl increment failed for '{Slug}'.", slug); }
            });

            _logger.LogInformation("ShortUrl redirect: /{Slug} -> {RealUrl}", slug, cached);
            context.Response.Redirect(cached, permanent: true);
            return;
        }

        await _next(context);
    }
}
