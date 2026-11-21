using System.Net;
using Capstone.View.Options;
using Microsoft.Extensions.Options;

namespace Capstone.View.Middleware;

/// <summary>
/// Site đứng sau Cloudflare → Connection.RemoteIpAddress là IP của Cloudflare, không phải khách.
/// Middleware này (đặt ĐẦU pipeline) thay RemoteIpAddress bằng IP thật trong header
/// CF-Connecting-IP, nhưng CHỈ KHI kết nối đến từ dải IP proxy tin cậy
/// (RateLimiting:TrustedProxyNetworks). Request đi thẳng vào origin thì header bị bỏ qua,
/// nên client không tự đặt header để giả IP / né rate limit được.
/// </summary>
public class TrustedProxyClientIpMiddleware
{
    public const string ProxyIpItem = "rl:proxy-ip";

    private readonly RequestDelegate _next;
    private readonly string _header;
    private readonly System.Net.IPNetwork[] _networks;

    public TrustedProxyClientIpMiddleware(RequestDelegate next,
        ILogger<TrustedProxyClientIpMiddleware> logger, IOptions<RateLimitOptions> opt)
    {
        _next   = next;
        _header = opt.Value.ClientIpHeader ?? string.Empty;

        var list = new List<System.Net.IPNetwork>();
        foreach (var cidr in opt.Value.TrustedProxyNetworks ?? [])
        {
            if (System.Net.IPNetwork.TryParse(cidr, out var network)) list.Add(network);
            else logger.LogWarning("RateLimiting:TrustedProxyNetworks có CIDR không hợp lệ: {Cidr}", cidr);
        }
        _networks = list.ToArray();
    }

    public Task InvokeAsync(HttpContext context)
    {
        var remote = context.Connection.RemoteIpAddress;
        if (remote is not null && _header.Length > 0 && _networks.Length > 0)
        {
            if (remote.IsIPv4MappedToIPv6) remote = remote.MapToIPv4();

            if (IsTrustedProxy(remote)
                && context.Request.Headers.TryGetValue(_header, out var values)
                && IPAddress.TryParse(values.ToString().Trim(), out var client))
            {
                context.Items[ProxyIpItem] = remote;
                context.Connection.RemoteIpAddress = client.IsIPv4MappedToIPv6 ? client.MapToIPv4() : client;
            }
        }

        return _next(context);
    }

    private bool IsTrustedProxy(IPAddress ip)
    {
        foreach (var network in _networks)
            if (network.Contains(ip)) return true;
        return false;
    }
}
