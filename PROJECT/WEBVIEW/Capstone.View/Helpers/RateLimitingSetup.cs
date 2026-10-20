using System.Globalization;
using System.Threading.RateLimiting;
using Capstone.View.Middleware;
using Capstone.View.Options;
using Microsoft.AspNetCore.RateLimiting;

namespace Capstone.View.Helpers;

/// <summary>
/// Rate limit theo IP (khoá do BotGuardMiddleware gán).
///  - Global: mọi request động, sliding window + giới hạn đồng thời.
///  - Policy "search" / "detail" / "form": gắn bằng [EnableRateLimiting] trên controller/action,
///    áp dụng THÊM vào global.
/// </summary>
public static class RateLimitingSetup
{
    public const string SearchPolicy = "search";
    public const string DetailPolicy = "detail";
    public const string FormPolicy   = "form";
    public const string TrackPolicy  = "track";

    public static IServiceCollection AddSiteRateLimiting(this IServiceCollection services, IConfiguration config)
    {
        var section = config.GetSection(RateLimitOptions.SectionName);
        services.Configure<RateLimitOptions>(section);
        var opt = section.Get<RateLimitOptions>() ?? new RateLimitOptions();

        services.AddRateLimiter(o =>
        {
            o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            o.GlobalLimiter = PartitionedRateLimiter.CreateChained(
                PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
                {
                    if (IsExempt(ctx)) return RateLimitPartition.GetNoLimiter("exempt");
                    var limit = IsVerifiedBot(ctx) ? opt.VerifiedBotPerMinute : opt.GlobalPerMinute;
                    return SlidingWindow("g:" + Key(ctx), limit);
                }),
                PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
                {
                    if (IsExempt(ctx) || IsVerifiedBot(ctx)) return RateLimitPartition.GetNoLimiter("exempt");
                    return RateLimitPartition.GetConcurrencyLimiter("c:" + Key(ctx), _ => new ConcurrencyLimiterOptions
                    {
                        PermitLimit          = opt.ConcurrentPerIp,
                        QueueLimit           = opt.ConcurrentPerIp,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
                }));

            o.AddPolicy(SearchPolicy, ctx => EndpointLimiter(ctx, "s:", opt.SearchPerMinute, opt));
            o.AddPolicy(DetailPolicy, ctx => EndpointLimiter(ctx, "d:", opt.DetailPerMinute, opt));
            o.AddPolicy(FormPolicy, ctx => IsExempt(ctx)
                ? RateLimitPartition.GetNoLimiter("exempt")
                : SlidingWindow("f:" + Key(ctx), opt.FormPerMinute));
            o.AddPolicy(TrackPolicy, ctx => IsExempt(ctx)
                ? RateLimitPartition.GetNoLimiter("exempt")
                : SlidingWindow("t:" + Key(ctx), opt.TrackPerMinute));

            o.OnRejected = async (ctx, ct) =>
            {
                var http = ctx.HttpContext;
                if (ctx.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    http.Response.Headers.RetryAfter = ((int)Math.Ceiling(retryAfter.TotalSeconds)).ToString(CultureInfo.InvariantCulture);
                else
                    http.Response.Headers.RetryAfter = "60";

                http.RequestServices.GetRequiredService<ILoggerFactory>()
                    .CreateLogger("RateLimit")
                    .LogWarning("Rate limited {Key} {Method} {Path} UA={UserAgent}",
                        Key(http), http.Request.Method, http.Request.Path, http.Request.Headers.UserAgent.ToString());

                http.Response.ContentType = "text/plain; charset=utf-8";
                await http.Response.WriteAsync("Bạn thao tác quá nhanh, vui lòng thử lại sau ít phút.", ct);
            };
        });

        return services;
    }

    private static RateLimitPartition<string> EndpointLimiter(HttpContext ctx, string prefix, int limit, RateLimitOptions opt)
    {
        if (IsExempt(ctx)) return RateLimitPartition.GetNoLimiter("exempt");
        if (IsVerifiedBot(ctx)) limit = opt.VerifiedBotPerMinute;
        return SlidingWindow(prefix + Key(ctx), limit);
    }

    private static RateLimitPartition<string> SlidingWindow(string key, int limit) =>
        RateLimitPartition.GetSlidingWindowLimiter(key, _ => new SlidingWindowRateLimiterOptions
        {
            PermitLimit       = limit,
            Window            = TimeSpan.FromMinutes(1),
            SegmentsPerWindow = 6,
            QueueLimit        = 0
        });

    private static string Key(HttpContext ctx) =>
        ctx.Items[BotGuardMiddleware.PartitionKeyItem] as string
        ?? BotGuardMiddleware.PartitionKey(ctx.Connection.RemoteIpAddress);

    private static bool IsExempt(HttpContext ctx) => ctx.Items.ContainsKey(BotGuardMiddleware.ExemptItem);

    private static bool IsVerifiedBot(HttpContext ctx) => ctx.Items.ContainsKey(BotGuardMiddleware.VerifiedBotItem);
}
