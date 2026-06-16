using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Infrastructure;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// Trang đăng nhập bảo vệ Hangfire Dashboard.
    /// GET  /hangfire-login  → Hiển thị form username/password.
    /// POST /hangfire-login  → Kiểm tra thông tin, set signed cookie, redirect về /hangfire.
    /// GET  /hangfire-logout → Xóa cookie, redirect về trang login.
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
            => Content(BuildLoginHtml(returnUrl ?? "/hangfire", error: null), "text/html");

        // POST /hangfire-login
        [HttpPost("/hangfire-login")]
        public IActionResult Login(
            [FromForm] string username,
            [FromForm] string password,
            [FromForm] string? returnUrl)
        {
            var cfg            = _config.GetSection("HangfireDashboard");
            var validUser      = cfg["Username"] ?? string.Empty;
            var validPassword  = cfg["Password"] ?? string.Empty;
            var cookieSecret   = cfg["CookieSecret"] ?? string.Empty;

            if (username == validUser && password == validPassword)
            {
                var filter       = new HangfireDashboardAuthFilter(cookieSecret);
                var cookieValue  = filter.BuildCookieValue(expiryHours: 8);

                Response.Cookies.Append(HangfireDashboardAuthFilter.CookieName, cookieValue,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Lax,
                        Secure   = Request.IsHttps,
                        Expires  = DateTimeOffset.UtcNow.AddHours(8)
                    });

                _logger.LogInformation("Hangfire Dashboard: login thành công – user={User} IP={IP}",
                    username, HttpContext.Connection.RemoteIpAddress);

                return Redirect(returnUrl ?? "/hangfire");
            }

            _logger.LogWarning("Hangfire Dashboard: login thất bại – user={User} IP={IP}",
                username, HttpContext.Connection.RemoteIpAddress);

            return Content(
                BuildLoginHtml(returnUrl ?? "/hangfire", error: "Tên đăng nhập hoặc mật khẩu không đúng."),
                "text/html");
        }

        // GET /hangfire-logout
        [HttpGet("/hangfire-logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(HangfireDashboardAuthFilter.CookieName);
            _logger.LogInformation("Hangfire Dashboard: logout từ {IP}", HttpContext.Connection.RemoteIpAddress);
            return Redirect("/hangfire-login");
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
                        max-width: 400px;
                        box-shadow: 0 20px 60px rgba(0,0,0,.4);
                    }
                    .logo { text-align: center; margin-bottom: 1.5rem; }
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
                    .field { margin-bottom: 1.1rem; }
                    label {
                        display: block;
                        font-size: .8rem;
                        font-weight: 600;
                        color: #374151;
                        margin-bottom: .4rem;
                        letter-spacing: .04em;
                        text-transform: uppercase;
                    }
                    input[type=text], input[type=password] {
                        width: 100%;
                        padding: .65rem .85rem;
                        border: 1.5px solid #d1d5db;
                        border-radius: 8px;
                        font-size: .95rem;
                        color: #1f2937;
                        background: #f9fafb;
                        transition: border-color .2s, box-shadow .2s;
                    }
                    input[type=text]:focus, input[type=password]:focus {
                        outline: none;
                        border-color: #4f46e5;
                        background: #fff;
                        box-shadow: 0 0 0 3px rgba(79,70,229,.15);
                    }
                    .btn {
                        display: block;
                        width: 100%;
                        margin-top: 1.5rem;
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
                    .btn:hover  { background: #4338ca; }
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
                        <svg viewBox="0 0 64 64" fill="none" xmlns="http://www.w3.org/2000/svg">
                            <rect width="64" height="64" rx="14" fill="#4f46e5"/>
                            <path d="M20 44V20h6v9h12v-9h6v24h-6V34H26v10z" fill="#fff"/>
                        </svg>
                    </div>
                    <h1>Hangfire Dashboard <span class="badge">SECURE</span></h1>
                    <p class="subtitle">Đăng nhập để theo dõi jobs</p>

                    {{(error is not null ? $"""<div class="error">⚠️ {error}</div>""" : "")}}

                    <form method="post" action="/hangfire-login">
                        <input type="hidden" name="returnUrl" value="{{returnUrl}}"/>

                        <div class="field">
                            <label for="username">Tên đăng nhập</label>
                            <input id="username" name="username" type="text"
                                   placeholder="admin" required autocomplete="username"/>
                        </div>
                        <div class="field">
                            <label for="password">Mật khẩu</label>
                            <input id="password" name="password" type="password"
                                   placeholder="••••••••" required autocomplete="current-password"/>
                        </div>

                        <button type="submit" class="btn">🔓 Đăng nhập</button>
                    </form>
                </div>
            </body>
            </html>
            """;
    }
}

