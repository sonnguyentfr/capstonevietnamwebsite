using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// Trang đăng nhập bảo vệ Hangfire Dashboard.
    /// GET  /hangfire-login  → Hiển thị form login.
    /// POST /hangfire-login  → Nhận JWT token, set cookie, redirect về /hangfire.
    /// </summary>
    [AllowAnonymous]
    public class HangfireLoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<HangfireLoginController> _logger;

        public HangfireLoginController(IConfiguration config, ILogger<HangfireLoginController> logger)
        {
            _config = config;
            _logger = logger;
        }

        // GET /hangfire-login
        [HttpGet("/hangfire-login")]
        public IActionResult Index(string? returnUrl = null)
        {
            return Content(BuildLoginHtml(returnUrl ?? "/hangfire", error: null), "text/html");
        }

        // POST /hangfire-login
        [HttpPost("/hangfire-login")]
        public IActionResult Login([FromForm] string token, [FromForm] string? returnUrl)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var secret      = jwtSettings["Secret"] ?? string.Empty;
            var issuer      = jwtSettings["Issuer"]  ?? string.Empty;
            var audience    = jwtSettings["Audience"] ?? string.Empty;

            if (IsValidToken(token, secret, issuer, audience))
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    SameSite = SameSiteMode.Lax,
                    Secure   = Request.IsHttps,
                    Expires  = DateTimeOffset.UtcNow.AddHours(8)
                };
                Response.Cookies.Append("HangfireToken", token, cookieOptions);

                _logger.LogInformation("Hangfire Dashboard: login thành công từ {IP}",
                    HttpContext.Connection.RemoteIpAddress);

                return Redirect(returnUrl ?? "/hangfire");
            }

            _logger.LogWarning("Hangfire Dashboard: login thất bại từ {IP}",
                HttpContext.Connection.RemoteIpAddress);

            return Content(BuildLoginHtml(returnUrl ?? "/hangfire", error: "Token không hợp lệ hoặc đã hết hạn."), "text/html");
        }

        // GET /hangfire-logout
        [HttpGet("/hangfire-logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("HangfireToken");
            return Redirect("/hangfire-login");
        }

        private static bool IsValidToken(string token, string secret, string issuer, string audience)
        {
            try
            {
                var handler    = new JwtSecurityTokenHandler();
                var key        = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer           = true,
                    ValidateAudience         = true,
                    ValidateLifetime         = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer              = issuer,
                    ValidAudience            = audience,
                    IssuerSigningKey         = key,
                    ClockSkew                = TimeSpan.FromSeconds(30)
                };
                handler.ValidateToken(token, parameters, out _);
                return true;
            }
            catch { return false; }
        }

        private static string BuildLoginHtml(string returnUrl, string? error) => $$"""
            <!DOCTYPE html>
            <html lang="vi">
            <head>
                <meta charset="UTF-8"/>
                <meta name="viewport" content="width=device-width, initial-scale=1"/>
                <title>Hangfire Dashboard – Đăng nhập</title>
                <style>
                    *, *::before, *::after { box-sizing: border-box; margin: 0; padding: 0; }
                    body {
                        font-family: 'Segoe UI', system-ui, sans-serif;
                        background: linear-gradient(135deg, #1a1a2e 0%, #16213e 50%, #0f3460 100%);
                        min-height: 100vh;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                    }
                    .card {
                        background: #fff;
                        border-radius: 12px;
                        padding: 2.5rem 2rem;
                        width: 100%;
                        max-width: 420px;
                        box-shadow: 0 20px 60px rgba(0,0,0,.4);
                    }
                    .logo {
                        text-align: center;
                        margin-bottom: 1.5rem;
                    }
                    .logo svg { width: 48px; height: 48px; }
                    h1 {
                        font-size: 1.4rem;
                        font-weight: 700;
                        color: #1a1a2e;
                        text-align: center;
                        margin-bottom: .25rem;
                    }
                    .subtitle {
                        text-align: center;
                        color: #6b7280;
                        font-size: .875rem;
                        margin-bottom: 1.75rem;
                    }
                    label {
                        display: block;
                        font-size: .8rem;
                        font-weight: 600;
                        color: #374151;
                        margin-bottom: .4rem;
                        letter-spacing: .04em;
                        text-transform: uppercase;
                    }
                    textarea {
                        width: 100%;
                        height: 110px;
                        padding: .65rem .85rem;
                        border: 1.5px solid #d1d5db;
                        border-radius: 8px;
                        font-size: .85rem;
                        font-family: 'Cascadia Code', 'Consolas', monospace;
                        resize: vertical;
                        color: #1f2937;
                        transition: border-color .2s;
                        background: #f9fafb;
                    }
                    textarea:focus {
                        outline: none;
                        border-color: #4f46e5;
                        background: #fff;
                        box-shadow: 0 0 0 3px rgba(79,70,229,.15);
                    }
                    .btn {
                        display: block;
                        width: 100%;
                        margin-top: 1.25rem;
                        padding: .75rem;
                        background: #4f46e5;
                        color: #fff;
                        border: none;
                        border-radius: 8px;
                        font-size: 1rem;
                        font-weight: 600;
                        cursor: pointer;
                        transition: background .2s, transform .1s;
                    }
                    .btn:hover { background: #4338ca; }
                    .btn:active { transform: scale(.98); }
                    .error {
                        background: #fef2f2;
                        border: 1px solid #fca5a5;
                        color: #b91c1c;
                        border-radius: 8px;
                        padding: .65rem .85rem;
                        font-size: .85rem;
                        margin-bottom: 1rem;
                    }
                    .hint {
                        margin-top: 1rem;
                        font-size: .78rem;
                        color: #9ca3af;
                        text-align: center;
                        line-height: 1.5;
                    }
                    .hint a { color: #4f46e5; text-decoration: none; }
                    .hint a:hover { text-decoration: underline; }
                    .badge {
                        display: inline-block;
                        background: #ecfdf5;
                        color: #065f46;
                        border: 1px solid #a7f3d0;
                        border-radius: 999px;
                        font-size: .72rem;
                        font-weight: 600;
                        padding: .15rem .6rem;
                        margin-left: .4rem;
                        vertical-align: middle;
                    }
                </style>
            </head>
            <body>
                <div class="card">
                    <div class="logo">
                        <!-- Hangfire icon (SVG inline) -->
                        <svg viewBox="0 0 64 64" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <rect width="64" height="64" rx="14" fill="#4f46e5"/>
                            <path d="M20 44V20h6v9h12v-9h6v24h-6V34H26v10z" fill="#fff"/>
                        </svg>
                    </div>
                    <h1>Hangfire Dashboard <span class="badge">SECURE</span></h1>
                    <p class="subtitle">Dán JWT Bearer Token để truy cập</p>

                    {{(error is not null ? $"""<div class="error">⚠️ {error}</div>""" : "")}}

                    <form method="post" action="/hangfire-login">
                        <input type="hidden" name="returnUrl" value="{{returnUrl}}"/>
                        <label for="token">Bearer Token</label>
                        <textarea id="token" name="token" placeholder="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." required autocomplete="off" spellcheck="false"></textarea>
                        <button type="submit" class="btn">🔓 Truy cập Dashboard</button>
                    </form>
                    <p class="hint">
                        Token lấy từ <a href="/swagger" target="_blank">Swagger → POST /api/auth/login</a><br/>
                        Token sẽ được lưu vào cookie trong <strong>8 giờ</strong>.
                    </p>
                </div>
            </body>
            </html>
            """;
    }
}
