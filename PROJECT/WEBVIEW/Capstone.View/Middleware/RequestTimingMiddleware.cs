using System.Diagnostics;

namespace Capstone.View.Middleware;

/// <summary>
/// Logs request duration. Warns on requests exceeding SlowThresholdMs.
/// Adds Server-Timing header so Chrome DevTools shows it.
/// </summary>
public class RequestTimingMiddleware
{
    private const int SlowThresholdMs = 500;
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(RequestDelegate next, ILogger<RequestTimingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            await _next(context);
        }
        finally
        {
            sw.Stop();
            var ms     = sw.ElapsedMilliseconds;
            var path   = context.Request.Path;
            var method = context.Request.Method;
            var status = context.Response.StatusCode;

            // Only set header before response body is flushed
            if (!context.Response.HasStarted)
            {
                context.Response.Headers["Server-Timing"] = $"total;dur={ms}";
            }

            if (ms >= SlowThresholdMs)
            {
                _logger.LogWarning(
                    "SLOW REQUEST {Method} {Path} → {Status} in {Ms}ms",
                    method, path, status, ms);
            }
            else
            {
                _logger.LogDebug(
                    "Request {Method} {Path} → {Status} in {Ms}ms",
                    method, path, status, ms);
            }
        }
    }
}
