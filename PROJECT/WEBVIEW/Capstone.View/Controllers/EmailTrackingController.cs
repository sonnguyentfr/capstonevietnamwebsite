using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NCapstone.View.Helpers;

namespace Capstone.View.Controllers;

/// <summary>
/// Xử lý tracking pixel (open) và click tracking cho email đã gửi.
///
/// GET /api/EmailTracking/open?id={logId}
///   → Trả về 1×1 transparent GIF
///   → UPDATE Marketing_Mail_Send_Log SET OpenedTime=NOW(), Status='Opened' WHERE Id=logId AND OpenedTime IS NULL
///   → UPDATE Marketing_Mail_Campaign_Send SET TotalOpened+=1
///
/// GET /api/EmailTracking/click?id={logId}&amp;url={url}
///   → UPDATE Marketing_Mail_Send_Log SET ClickedTime=NOW(), Status='Clicked' WHERE Id=logId AND ClickedTime IS NULL
///   → UPDATE Marketing_Mail_Campaign_Send SET TotalClicked+=1
///   → 302 Redirect → url
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EmailTrackingController : ControllerBase
{
    // 1×1 transparent GIF (35 bytes)
    private static readonly byte[] _pixel = Convert.FromBase64String(
        "R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");

    private readonly string _crmConnStr;
    private readonly ILogger<EmailTrackingController> _logger;

    public EmailTrackingController(
        IConfiguration config,
        ILogger<EmailTrackingController> logger)
    {
        _crmConnStr = config.GetConnectionString("CRMConnection")
                      ?? throw new InvalidOperationException("CRMConnection not configured");
        _logger = logger;
    }

    /// <summary>Tracking pixel — email client tải ảnh = user đã mở email.</summary>
    [HttpGet("open")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Open([FromQuery] long id)
    {
        if (id > 0)
        {
            try
            {
                await using var conn = new SqlConnection(_crmConnStr);

                // Chỉ update lần đầu mở (OpenedTime IS NULL)
                var rows = await conn.ExecuteAsync(
                    """
                    UPDATE Marketing_Mail_Send_Log
                    SET    OpenedTime = GETDATE(),
                           Status     = @Opened
                    WHERE  Id         = @Id
                      AND  OpenedTime IS NULL
                    """,
                    new { Id = id, Opened = MailSendStatus.Opened });

                if (rows > 0)
                {
                    await conn.ExecuteAsync(
                        """
                        UPDATE cs
                        SET    cs.TotalOpened = ISNULL(cs.TotalOpened, 0) + 1
                        FROM   Marketing_Mail_Campaign_Send cs
                        INNER JOIN Marketing_Mail_Send_Log  sl ON sl.CampaignSendId = cs.Id
                        WHERE  sl.Id = @Id
                        """,
                        new { Id = id });

                    _logger.LogInformation("EmailOpen tracked LogId={Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "EmailOpen track failed LogId={Id}", id);
            }
        }

        return File(_pixel, "image/gif");
    }

    /// <summary>Click tracking — ghi nhận lượt click rồi redirect đến đích.</summary>
    [HttpGet("click")]
    public async Task<IActionResult> Click([FromQuery] long id, [FromQuery] string? url)
    {
        var target = string.IsNullOrWhiteSpace(url)
            ? "/"
            : Uri.UnescapeDataString(url);

        // Validate redirect target — chỉ cho phép URL tương đối hoặc HTTP(S)
        if (Uri.TryCreate(target, UriKind.Absolute, out var targetUri)
            && !string.Equals(targetUri.Scheme, "https", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(targetUri.Scheme, "http",  StringComparison.OrdinalIgnoreCase))
        {
            target = "/";
        }

        if (id > 0)
        {
            try
            {
                await using var conn = new SqlConnection(_crmConnStr);

                // Chỉ update lần đầu click
                var rows = await conn.ExecuteAsync(
                    """
                    UPDATE Marketing_Mail_Send_Log
                    SET    ClickedTime = GETDATE(),
                           Status      = @Clicked
                    WHERE  Id          = @Id
                      AND  ClickedTime IS NULL
                    """,
                    new { Id = id, Clicked = MailSendStatus.Clicked });

                if (rows > 0)
                {
                    await conn.ExecuteAsync(
                        """
                        UPDATE cs
                        SET    cs.TotalClicked = ISNULL(cs.TotalClicked, 0) + 1
                        FROM   Marketing_Mail_Campaign_Send cs
                        INNER JOIN Marketing_Mail_Send_Log  sl ON sl.CampaignSendId = cs.Id
                        WHERE  sl.Id = @Id
                        """,
                        new { Id = id });

                    _logger.LogInformation("EmailClick tracked LogId={Id} Url={Url}", id, target);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "EmailClick track failed LogId={Id}", id);
            }
        }

        return Redirect(target);
    }
}
