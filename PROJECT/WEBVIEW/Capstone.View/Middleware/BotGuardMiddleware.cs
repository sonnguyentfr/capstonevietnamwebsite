using System.Net;
using System.Net.Sockets;
using Capstone.View.Options;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Capstone.View.Middleware;

/// <summary>
/// Chạy TRƯỚC UseRateLimiter:
///  - Chặn 403 User-Agent của tool scrape / crawler SEO không mong muốn, UA rỗng.
///  - UA xưng là Googlebot/Bingbot... thì xác minh bằng reverse DNS + forward DNS.
///    Thật → gắn cờ VerifiedBot (rate limit nới hơn). Giả mạo → 403.
///  - Gán khoá phân vùng rate limit (IP, IPv6 gộp /64) vào HttpContext.Items.
/// </summary>
public class BotGuardMiddleware
{
    public const string PartitionKeyItem = "rl:key";
    public const string VerifiedBotItem  = "rl:bot";
    public const string ExemptItem       = "rl:exempt";

    // UA token → hậu tố hostname hợp lệ khi reverse DNS
    private static readonly (string Token, string[] Suffixes)[] SearchBots =
    [
        // Không chấp nhận .googleusercontent.com: đó là VM Google Cloud ai cũng thuê được
        ("googlebot",        [".googlebot.com", ".google.com"]),
        ("google-inspectiontool", [".googlebot.com", ".google.com"]),
        ("adsbot-google",    [".googlebot.com", ".google.com"]),
        ("bingbot",          [".search.msn.com"]),
        ("coccocbot",        [".coccoc.com"]),
        ("applebot",         [".applebot.apple.com"]),
        ("yandexbot",        [".yandex.ru", ".yandex.net", ".yandex.com"]),
        ("duckduckbot",      [".duckduckgo.com"]),
    ];

    private static readonly TimeSpan DnsTimeout = TimeSpan.FromSeconds(2);
    private static readonly TimeSpan VerifyTtl  = TimeSpan.FromHours(6);

    private readonly RequestDelegate _next;
    private readonly ILogger<BotGuardMiddleware> _logger;
    private readonly IMemoryCache _cache;
    private readonly RateLimitOptions _opt;

    public BotGuardMiddleware(RequestDelegate next, ILogger<BotGuardMiddleware> logger,
        IMemoryCache cache, IOptions<RateLimitOptions> opt)
    {
        _next   = next;
        _logger = logger;
        _cache  = cache;
        _opt    = opt.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Lượt re-execute nội bộ (/error, /404) của request đã được tính rồi → không tính lần 2
        if (!_opt.Enabled
            || context.Features.Get<IExceptionHandlerPathFeature>() is not null
            || context.Features.Get<IStatusCodeReExecuteFeature>() is not null)
        {
            context.Items[ExemptItem] = true;
            await _next(context);
            return;
        }

        var path = context.Request.Path.Value ?? string.Empty;
        if (_opt.ExemptPathPrefixes.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
        {
            context.Items[ExemptItem] = true;
            await _next(context);
            return;
        }

        var ip = context.Connection.RemoteIpAddress;
        context.Items[PartitionKeyItem] = PartitionKey(ip);

        // Loopback (health check, gọi nội bộ trên server) không chặn
        if (ip is not null && IPAddress.IsLoopback(ip))
        {
            context.Items[ExemptItem] = true;
            await _next(context);
            return;
        }

        var ua = context.Request.Headers.UserAgent.ToString();

        if (string.IsNullOrWhiteSpace(ua))
        {
            if (_opt.BlockEmptyUserAgent)
            {
                await Block(context, ip, "empty-ua", ua);
                return;
            }
        }
        else
        {
            var uaLower = ua.ToLowerInvariant();

            var blocked = _opt.BlockedUserAgents.FirstOrDefault(b => uaLower.Contains(b));
            if (blocked is not null)
            {
                await Block(context, ip, "ua:" + blocked, ua);
                return;
            }

            var bot = SearchBots.FirstOrDefault(b => uaLower.Contains(b.Token));
            if (bot.Token is not null && ip is not null)
            {
                var verified = await VerifyBotAsync(ip, bot.Suffixes);
                if (verified == false)
                {
                    await Block(context, ip, "fake-bot:" + bot.Token, ua);
                    return;
                }
                if (verified == true)
                {
                    context.Items[VerifiedBotItem] = true;
                    context.Items[PartitionKeyItem] = "bot:" + bot.Token;
                }
                // null = DNS lỗi/timeout → coi như người dùng thường
            }
        }

        await _next(context);
    }

    /// <summary>IPv4 giữ nguyên; IPv6 gộp theo /64 để 1 máy không xoay địa chỉ né limit.</summary>
    public static string PartitionKey(IPAddress? ip)
    {
        if (ip is null) return "unknown";
        if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();
        if (ip.AddressFamily != AddressFamily.InterNetworkV6) return ip.ToString();

        var bytes = ip.GetAddressBytes();
        Array.Clear(bytes, 8, 8);
        return new IPAddress(bytes) + "/64";
    }

    /// <returns>true = bot thật, false = giả mạo, null = không xác định được (DNS lỗi).</returns>
    private async Task<bool?> VerifyBotAsync(IPAddress ip, string[] suffixes)
    {
        var cacheKey = "botverify:" + ip;
        if (_cache.TryGetValue(cacheKey, out bool? cached)) return cached;

        bool? result;
        try
        {
            var reverse = await Dns.GetHostEntryAsync(ip).WaitAsync(DnsTimeout);
            var host = reverse.HostName.TrimEnd('.').ToLowerInvariant();

            if (!suffixes.Any(s => host.EndsWith(s, StringComparison.Ordinal)))
            {
                result = false;
            }
            else
            {
                var forward = await Dns.GetHostAddressesAsync(host).WaitAsync(DnsTimeout);
                result = forward.Any(a => a.Equals(ip));
            }
        }
        catch (SocketException ex) when (ex.SocketErrorCode is SocketError.HostNotFound or SocketError.NoData)
        {
            // Không có PTR record → chắc chắn không phải bot search engine
            result = false;
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Bot DNS verification failed for {Ip}", ip);
            result = null;
        }

        _cache.Set(cacheKey, result, result is null ? TimeSpan.FromMinutes(5) : VerifyTtl);
        return result;
    }

    private Task Block(HttpContext context, IPAddress? ip, string reason, string ua)
    {
        _logger.LogWarning("BotGuard blocked {Ip} {Method} {Path} reason={Reason} UA={UserAgent}",
            ip, context.Request.Method, context.Request.Path, reason, ua);

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        context.Response.ContentType = "text/plain; charset=utf-8";
        return context.Response.WriteAsync("Forbidden");
    }
}
