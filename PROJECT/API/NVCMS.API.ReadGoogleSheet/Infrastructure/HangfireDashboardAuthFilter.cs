using Hangfire.Dashboard;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NVCMS.API.ReadGoogleSheet.Infrastructure
{
    /// <summary>
    /// Cho phép truy cập Hangfire Dashboard nếu:
    ///   - Request đến từ localhost (dev), HOẶC
    ///   - Cookie "HangfireToken" chứa JWT hợp lệ (được set bởi /hangfire-login).
    /// </summary>
    public class HangfireDashboardAuthFilter : IDashboardAuthorizationFilter
    {
        private readonly string _jwtSecret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public HangfireDashboardAuthFilter(string jwtSecret, string jwtIssuer, string jwtAudience)
        {
            _jwtSecret   = jwtSecret;
            _jwtIssuer   = jwtIssuer;
            _jwtAudience = jwtAudience;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // ── 1. Localhost luôn được phép (dev) ─────────────────────────────
            if (IsLocalRequest(httpContext))
                return true;

            // ── 2. Kiểm tra Bearer token trong header Authorization ────────────
            var authHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader?.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) == true)
            {
                var token = authHeader["Bearer ".Length..].Trim();
                if (ValidateToken(token))
                    return true;
            }

            // ── 3. Kiểm tra cookie HangfireToken (sau khi login qua /hangfire-login) ──
            var cookieToken = httpContext.Request.Cookies["HangfireToken"];
            if (!string.IsNullOrWhiteSpace(cookieToken) && ValidateToken(cookieToken))
                return true;

            // ── 4. Redirect về trang login thân thiện ─────────────────────────
            httpContext.Response.Redirect("/hangfire-login");
            return false;
        }

        private bool ValidateToken(string token)
        {
            try
            {
                var handler    = new JwtSecurityTokenHandler();
                var key        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = _jwtIssuer,
                    ValidAudience            = _jwtAudience,
                    IssuerSigningKey         = key,
                    ClockSkew                = TimeSpan.FromSeconds(30)
                };

                handler.ValidateToken(token, parameters, out _);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsLocalRequest(HttpContext context)
        {
            var connection = context.Connection;
            if (connection.RemoteIpAddress is null)
                return true; // không xác định được → giả định local khi dev

            if (connection.LocalIpAddress is not null)
                return connection.RemoteIpAddress.Equals(connection.LocalIpAddress);

            return System.Net.IPAddress.IsLoopback(connection.RemoteIpAddress);
        }
    }
}
