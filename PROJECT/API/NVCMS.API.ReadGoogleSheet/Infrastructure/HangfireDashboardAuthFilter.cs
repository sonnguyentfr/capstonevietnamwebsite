using Hangfire.Dashboard;
using System.Security.Cryptography;
using System.Text;

namespace NVCMS.API.ReadGoogleSheet.Infrastructure
{
    /// <summary>
    /// IDashboardAuthorizationFilter chỉ làm nhiệm vụ cho qua — việc kiểm tra
    /// cookie/redirect đã được xử lý bởi middleware UseHangfireAuthMiddleware()
    /// đặt TRƯỚC UseHangfireDashboard() trong Program.cs.
    /// </summary>
    public class HangfireDashboardAuthFilter : IDashboardAuthorizationFilter
    {
        internal const string CookieName = "HangfireAuth";

        private readonly string _cookieSecret;

        public HangfireDashboardAuthFilter(string cookieSecret)
        {
            _cookieSecret = cookieSecret;
        }

        // Middleware đã xác thực rồi → luôn cho qua
        public bool Authorize(DashboardContext context) => true;

        /// <summary>Tạo signed cookie: "expiry|HMAC-SHA256(secret, expiry)"</summary>
        internal string BuildCookieValue(int expiryHours = 8)
        {
            var expiry = DateTimeOffset.UtcNow.AddHours(expiryHours).ToUnixTimeSeconds().ToString();
            var sig    = ComputeHmac(_cookieSecret, expiry);
            return $"{expiry}|{sig}";
        }

        /// <summary>Dùng bởi middleware trong Program.cs để kiểm tra cookie.</summary>
        internal static bool IsValidCookieValue(string? value, string secret)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            var parts = value.Split('|');
            if (parts.Length != 2) return false;

            var expiry   = parts[0];
            var sig      = parts[1];
            var expected = ComputeHmac(secret, expiry);

            // So sánh constant-time để tránh timing attack
            var sigBytes      = Encoding.UTF8.GetBytes(sig);
            var expectedBytes = Encoding.UTF8.GetBytes(expected);
            if (sigBytes.Length != expectedBytes.Length) return false;
            if (!CryptographicOperations.FixedTimeEquals(sigBytes, expectedBytes)) return false;

            if (!long.TryParse(expiry, out var expiryTs)) return false;
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds() < expiryTs;
        }

        private static string ComputeHmac(string secret, string message)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secret);
            var msgBytes = Encoding.UTF8.GetBytes(message);
            return Convert.ToHexString(HMACSHA256.HashData(keyBytes, msgBytes)).ToLowerInvariant();
        }
    }
}
