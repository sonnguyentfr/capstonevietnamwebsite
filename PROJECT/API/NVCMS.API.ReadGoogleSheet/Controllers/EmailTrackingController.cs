using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using NVCMS.API.ReadGoogleSheet.Common;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// Xử lý tracking pixel và click tracking cho email đã gửi qua Marketing_Mail_Send_Log.
    ///
    /// GET /api/EmailTracking/open?id={logId}
    ///   → Trả về 1x1 transparent GIF
    ///   → UPDATE Marketing_Mail_Send_Log SET OpenedTime=NOW() WHERE Id=logId AND OpenedTime IS NULL
    ///   → UPDATE Marketing_Mail_Campaign_Send SET TotalOpened=TotalOpened+1 WHERE Id=campaignSendId
    ///
    /// GET /api/EmailTracking/click?id={logId}&url={url}
    ///   → UPDATE Marketing_Mail_Send_Log SET ClickedTime=NOW() WHERE Id=logId AND ClickedTime IS NULL
    ///   → UPDATE Marketing_Mail_Campaign_Send SET TotalClicked=TotalClicked+1 WHERE Id=campaignSendId
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

        public EmailTrackingController(IConfiguration config,
            ILogger<EmailTrackingController> logger)
        {
            _crmConnStr = config.GetConnectionString("DefaultCRMConnection")
                          ?? throw new InvalidOperationException("DefaultCRMConnection not configured");
            _logger = logger;
        }

        /// <summary>Tracking pixel — email client load ảnh = user đã mở email.</summary>
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
                    var rows = await conn.ExecuteAsync("""
                        UPDATE Marketing_Mail_Send_Log
                        SET    Status     = CASE WHEN Status = @Sent THEN @Sent ELSE Status END,
                               OpenedTime = GETDATE()
                        WHERE  Id         = @Id
                          AND  OpenedTime IS NULL
                        """,
                        new { Id = id, Sent = MailSendStatus.Sent });

                    if (rows > 0)
                    {
                        // Tăng TotalOpened trên Campaign_Send tương ứng
                        await conn.ExecuteAsync("""
                            UPDATE cs
                            SET    cs.TotalOpened = cs.TotalOpened + 1
                            FROM   Marketing_Mail_Campaign_Send cs
                            INNER JOIN Marketing_Mail_Send_Log sl ON sl.CampaignSendId = cs.Id
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

        /// <summary>Click tracking — redirect user đến đích, ghi nhận lượt click.</summary>
        [HttpGet("click")]
        public async Task<IActionResult> Click([FromQuery] long id, [FromQuery] string? url)
        {
            var target = string.IsNullOrWhiteSpace(url) ? "/" : Uri.UnescapeDataString(url);

            if (id > 0)
            {
                try
                {
                    await using var conn = new SqlConnection(_crmConnStr);

                    var rows = await conn.ExecuteAsync("""
                        UPDATE Marketing_Mail_Send_Log
                        SET    ClickedTime = GETDATE()
                        WHERE  Id           = @Id
                          AND  ClickedTime  IS NULL
                        """,
                        new { Id = id });

                    if (rows > 0)
                    {
                        await conn.ExecuteAsync("""
                            UPDATE cs
                            SET    cs.TotalClicked = cs.TotalClicked + 1
                            FROM   Marketing_Mail_Campaign_Send cs
                            INNER JOIN Marketing_Mail_Send_Log sl ON sl.CampaignSendId = cs.Id
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
}
