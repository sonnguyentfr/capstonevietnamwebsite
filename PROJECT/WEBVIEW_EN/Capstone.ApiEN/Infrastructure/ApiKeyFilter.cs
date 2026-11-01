using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;

namespace Capstone.ApiEN.Infrastructure;

/// <summary>
/// Requires a valid X-Api-Key header when ApiSecurity:ApiKeys is configured.
/// The EN website sends it from ContentApi:ApiKey. With no keys configured (development) every call passes.
/// </summary>
public sealed class ApiKeyFilter(IOptionsMonitor<ApiSecurityOptions> options) : IEndpointFilter
{
    public const string HeaderName = "X-Api-Key";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var keys = options.CurrentValue.ApiKeys;
        if (keys.Length == 0)
            return await next(context);

        var supplied = context.HttpContext.Request.Headers[HeaderName].ToString();
        if (supplied.Length > 0 && keys.Any(k => FixedTimeEquals(k, supplied)))
            return await next(context);

        return Results.Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Missing or invalid API key",
            detail: $"Send a valid key in the {HeaderName} header.");
    }

    private static bool FixedTimeEquals(string expected, string actual)
        => CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(expected), Encoding.UTF8.GetBytes(actual));
}
