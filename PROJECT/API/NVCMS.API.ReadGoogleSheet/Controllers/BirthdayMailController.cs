using System.Text.RegularExpressions;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.Config;
using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// BIRTHDAY-MAIL 02/11/2026: thư chúc mừng sinh nhật.
    ///
    /// Công khai (khách bấm trong thư — KHÔNG cần đăng nhập):
    ///   GET  /api/BirthdayMail/open?t={token}          ảnh 1x1 — mỗi lần tải = +1 lượt mở (OpenCount, FirstOpenAt, LastOpenAt)
    ///   GET  /api/BirthdayMail/click?t={token}&amp;l={linkId}  +1 click cho người nhận và link đó rồi 302 tới link đã lưu trong DB
    ///                                                  (không nhận URL từ query => không bị lợi dụng chuyển hướng sang trang lạ)
    /// CRM gọi (JWT như các API khác — cap_api_user):
    ///   POST /api/BirthdayMail/preview                 { portalId, templateId, subject, bodyHtml, fullName } -> { subject, html, items }
    ///   POST /api/BirthdayMail/test                    + { email, requestedBy } -> gửi thư thử "[THỬ] ..." (không tính thống kê)
    ///   POST /api/BirthdayMail/run-now                 { portalId, triggeredBy } -> Hangfire enqueue BirthdayMailJob.RunNow
    ///   GET  /api/BirthdayMail/default-template        -> { subject, html } thiệp mặc định (để Marketing sao chép làm mẫu mới)
    ///   GET  /api/BirthdayMail/status                  -> cấu hình kỹ thuật (job bật?, cron, tracking công khai?)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BirthdayMailController : ControllerBase
    {
        private static readonly byte[] Pixel = Convert.FromBase64String("R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7");
        private static readonly Regex RxToken = new("^[0-9a-f]{32}$", RegexOptions.Compiled);

        private readonly IBirthdayMailRepository _repo;
        private readonly BirthdayMailService _service;
        private readonly IBackgroundJobClient _jobs;
        private readonly BirthdayMailSettings _settings;
        private readonly HangfireJobSettings _hangfire;
        private readonly ILogger<BirthdayMailController> _logger;

        public BirthdayMailController(IBirthdayMailRepository repo, BirthdayMailService service, IBackgroundJobClient jobs,
            IOptions<BirthdayMailSettings> settings, IOptions<HangfireJobSettings> hangfire, ILogger<BirthdayMailController> logger)
        {
            _repo = repo;
            _service = service;
            _jobs = jobs;
            _settings = settings.Value;
            _hangfire = hangfire.Value;
            _logger = logger;
        }

        // ── tracking công khai ──────────────────────────────────────────────

        [HttpGet("open")]
        [AllowAnonymous]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Open([FromQuery] string? t)
        {
            if (IsToken(t))
            {
                try { await _repo.TrackOpenAsync(t!, BirthdayMailService.VietnamNow); }
                catch (Exception ex) { _logger.LogWarning(ex, "BirthdayMail open lỗi"); }
            }
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate, max-age=0";
            return File(Pixel, "image/gif");
        }

        [HttpGet("click")]
        [AllowAnonymous]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> Click([FromQuery] string? t, [FromQuery] int l)
        {
            string? url = null;
            if (l > 0)
            {
                try { url = await _repo.TrackClickAsync(IsToken(t) ? t! : new string('0', 32), l, BirthdayMailService.VietnamNow); }
                catch (Exception ex) { _logger.LogWarning(ex, "BirthdayMail click lỗi (link {Link})", l); }
            }
            if (!Uri.TryCreate(url, UriKind.Absolute, out var u) || (u.Scheme != Uri.UriSchemeHttps && u.Scheme != Uri.UriSchemeHttp))
                url = string.IsNullOrWhiteSpace(_settings.WebsiteUrl) ? "https://capstonevietnam.com" : _settings.WebsiteUrl;
            return Redirect(url!);
        }

        // ── CRM ─────────────────────────────────────────────────────────────

        [HttpPost("preview")]
        [Authorize]
        public async Task<IActionResult> Preview([FromBody] BirthdayPreviewRequest req)
        {
            try
            {
                var (subject, html, items) = await _service.PreviewAsync(req);
                return Ok(ApiResponse<object>.SuccessResponse(new { subject, html, items }));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is FileNotFoundException)
            {
                return Ok(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("test")]
        [Authorize]
        public async Task<IActionResult> Test([FromBody] BirthdayTestRequest req)
        {
            try
            {
                var subject = await _service.SendTestAsync(req);
                return Ok(ApiResponse<object>.SuccessResponse(new { subject, tracking = _service.TrackingIsPublic }, "Đã gửi thư thử tới " + req.Email + "."));
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException || ex is FileNotFoundException)
            {
                return Ok(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpPost("run-now")]
        [Authorize]
        public IActionResult RunNow([FromBody] BirthdayRunNowRequest req)
        {
            var by = string.IsNullOrWhiteSpace(req.TriggeredBy) ? "CRM" : req.TriggeredBy.Trim();
            if (by.Length > 200) by = by[..200];
            var jobId = _jobs.Enqueue<BirthdayMailJob>(j => j.RunNow(req.PortalId, by, CancellationToken.None));
            _logger.LogInformation("BirthdayMail RunNow portal {Portal} bởi {By} -> job {Job}", req.PortalId, by, jobId);
            return Ok(ApiResponse<object>.SuccessResponse(new { jobId }, "Đã đưa vào hàng đợi gửi. Kết quả hiện trong Thống kê sau ít phút."));
        }

        [HttpGet("default-template")]
        [Authorize]
        public async Task<IActionResult> DefaultTemplate()
        {
            try
            {
                var (subject, html) = await _service.GetDefaultTemplateAsync();
                return Ok(ApiResponse<object>.SuccessResponse(new { subject, html }));
            }
            catch (FileNotFoundException ex)
            {
                return Ok(ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        [HttpGet("status")]
        [Authorize]
        public IActionResult Status()
        {
            var j = _hangfire.BirthdayMail;
            return Ok(ApiResponse<object>.SuccessResponse(new
            {
                jobEnabled = j.Enabled,
                cron = j.Cron,
                timeZone = j.TimeZone,
                trackingBase = _service.TrackingBase,
                trackingPublic = _service.TrackingIsPublic,
                latestSendTime = _settings.LatestSendTime,
                vietnamNow = BirthdayMailService.VietnamNow.ToString("yyyy-MM-dd HH:mm:ss")
            }));
        }

        private static bool IsToken(string? t) => t != null && RxToken.IsMatch(t);
    }
}
