using Capstone.View.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Capstone.View.Controllers;

public class MailUnsubscribeController : Controller
{
    private readonly string _crmConnStr;
    private readonly ILogger<MailUnsubscribeController> _logger;

    public MailUnsubscribeController(
        IConfiguration config,
        ILogger<MailUnsubscribeController> logger)
    {
        _crmConnStr = config.GetConnectionString("CRMConnection")
                      ?? throw new InvalidOperationException("CRMConnection not configured");
        _logger = logger;
    }

    // GET /mail-unsubscribe?email={encrypted}
    [HttpGet]
    public IActionResult Index([FromQuery] string? email)
    {
        var decoded = DecryptEmail(email);
        ViewBag.Email   = decoded;
        ViewBag.Success = false;
        ViewBag.Error   = TempData["Error"] as string;
        return View();
    }

    // POST /mail-unsubscribe
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        [FromQuery]  string? email,
        [FromForm]   string? reason)
    {
        var decoded = DecryptEmail(email);

        if (string.IsNullOrWhiteSpace(reason))
        {
            ViewBag.Email      = decoded;
            ViewBag.Success    = false;
            ViewBag.ErrorMsg   = "Vui lòng chọn lý do hủy đăng ký.";
            return View();
        }

        try
        {
            await using var conn = new SqlConnection(_crmConnStr);
            await conn.ExecuteAsync(
                """
                IF NOT EXISTS (
                    SELECT 1 FROM [Marketing_Mail_ListMail_Unsub]
                    WHERE Email = @Email
                )
                INSERT INTO [Marketing_Mail_ListMail_Unsub] (Email, reason, created_date, PortalId)
                VALUES (@Email, @Reason, GETDATE(), 0)
                """,
                new { Email = decoded, Reason = reason });

            _logger.LogInformation("Unsubscribed {Email} reason={Reason}", decoded, reason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unsub failed for {Email}", decoded);
            ViewBag.Email    = decoded;
            ViewBag.Success  = false;
            ViewBag.ErrorMsg = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            return View();
        }

        ViewBag.Email   = decoded;
        ViewBag.Success = true;
        return View();
    }

    // ── helper ───────────────────────────────────────────────────────────────
    private static string DecryptEmail(string? encrypted)
    {
        if (string.IsNullOrWhiteSpace(encrypted)) return string.Empty;
        try   { return CdnSrcTagHelper.Decrypt(encrypted); }
        catch { return string.Empty; }
    }
}
