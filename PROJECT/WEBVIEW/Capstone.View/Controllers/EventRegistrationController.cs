using Capstone.View.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.SiteSettings;
using NVCMS.WebView.Data.ViewModels;
using System.Text;
using System.Text.Json;

namespace Capstone.View.Controllers;

public class EventRegistrationController : Controller
{
    private const int VietnamLocationId = 82;

    private readonly IEventRegistrationService _service;
    private readonly IEventsService _events;
    private readonly ILocationService _locations;
    private readonly ISiteSettingsHelper _siteSettings;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IOptions<SiteSettings> _siteOptions;
    private readonly ILogger<EventRegistrationController> _logger;
    private readonly string _googlerecaptchav3_sitekey;
    private readonly string _googlerecaptchav3_secretkey;
    private readonly string? _fixedEmail;

    public EventRegistrationController(
        IEventRegistrationService service,
        IEventsService events,
        ILocationService locations,
        ISiteSettingsHelper siteSettings,
        IHttpClientFactory httpFactory,
        IOptions<SiteSettings> siteOptions,
        IConfiguration config,
        ILogger<EventRegistrationController> logger)
    {
        _service = service;
        _events = events;
        _locations = locations;
        _siteSettings = siteSettings;
        _httpFactory = httpFactory;
        _siteOptions = siteOptions;
        _logger = logger;
        _googlerecaptchav3_sitekey = config["Google:recaptchav3_sitekey"] ?? string.Empty;
        _googlerecaptchav3_secretkey = config["Google:recaptchav3_secretkey"] ?? string.Empty;
        _fixedEmail = config["Email:FixedEmail"];
    }

    // ── GET /dang-ky-su-kien?eventCatId=x&eventId=y ──────────────────────────

    [HttpGet]
    public async Task<IActionResult> Index(int eventCatId, int eventId = 0)
    {
        var cat = await _events.GetCatWithEventsAsync(eventCatId, 50);
        if (cat is null) return NotFound();

        // Only allow registration during active window
        var now = DateTime.Now;
        bool isOpen = (!cat.EndDate.HasValue || now <= cat.EndDate.Value);
        if (!isOpen)
        {
            ViewData["Title"] = "Đăng ký đã đóng";
            return View("Closed");
        }

        // Resolve the pre-selected event location (entry point 1) or default to first
        var selectedEvent = cat.Events.FirstOrDefault(e => e.Id == eventId)
                         ?? cat.Events.FirstOrDefault()
                         ?? new EventsViewModel();

        var provinces = await _locations.GetProvincesAsync(VietnamLocationId);

        var vm = new EventRegistrationPageViewModel
        {
            Cat = cat,
            Event = selectedEvent,
            PreselectedEventId = eventId,
            Provinces = provinces,
            Input = new EventRegistrationInputViewModel
            {
                EventCatId = eventCatId,
                EventId = selectedEvent.Id,
            }
        };

        ViewData["Title"] = $"Đăng ký tham dự - {cat.CatName}";

        var site = await _siteSettings.GetSettingsAsync(_siteOptions.Value.PortalCRMId);
        //ViewBag.RecaptchaSiteKey = site.Google.CaptchaKey ?? string.Empty;
        ViewBag.RecaptchaSiteKey = _googlerecaptchav3_sitekey ?? string.Empty;

        return View(vm);
    }

    // ── POST /dang-ky-su-kien ─────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(EventRegistrationInputViewModel input, CancellationToken ct)
    {
        var portalId = _siteOptions.Value.PortalCRMId;

        // ── reCAPTCHA v3 verification ─────────────────────────────────────────
        var recaptchaToken = Request.Form["g-recaptcha-response"].ToString();
        if (!await VerifyRecaptchaAsync(recaptchaToken, portalId))
        {
            ModelState.AddModelError(string.Empty, "Xác thực reCAPTCHA thất bại. Vui lòng thử lại.");
        }

        if (!ModelState.IsValid)
        {
            var cat = await _events.GetCatWithEventsAsync(input.EventCatId, portalId);
            var ev = cat?.Events.FirstOrDefault(e => e.Id == input.EventId) ?? new EventsViewModel();
            var site2 = await _siteSettings.GetSettingsAsync(portalId);
            var provinces = await _locations.GetProvincesAsync(VietnamLocationId);
            //ViewBag.RecaptchaSiteKey = site2.Google.CaptchaKey ?? string.Empty;
            ViewBag.RecaptchaSiteKey = _googlerecaptchav3_sitekey ?? string.Empty;

            return View(new EventRegistrationPageViewModel
            {
                Cat = cat ?? new EventsCatViewModel(),
                Event = ev,
                PreselectedEventId = input.EventId,
                Provinces = provinces,
                Input = input,
            });
        }

        var (success, isDuplicate, message, studentId, studentCode) =
            await _service.RegisterAsync(input, portalId, ct);

        if (!success)
        {
            TempData["RegError"] = message;
            return RedirectToAction(nameof(Index), new { eventCatId = input.EventCatId, eventId = input.EventId });
        }

        _logger.LogInformation(
            "{Event}: StudentId={StudentId} EventCatId={EventCatId} EventId={EventId} IsDuplicate={IsDuplicate}",
            isDuplicate ? "DuplicateReRegistration" : "RegistrationSuccess",
            studentId, input.EventCatId, input.EventId, isDuplicate);

        // ── Fire-and-forget to API → Hangfire enqueues the email job ─────────
        // Only send if Sendmail == true on the event category
        var catForMail = await _events.GetCatWithEventsAsync(input.EventCatId, portalId);
        if (catForMail?.Sendmail == true)
        {
            // Capture all request-scoped values NOW — before Task.Run — to avoid
            // accessing disposed HttpContext inside the background thread.
            var site = await _siteSettings.GetSettingsAsync(portalId);
            var adminEmails = (site.Mail.MailList ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(EmailHelper.IsValid)
                .ToList();

            // BCC on the customer email: FixedEmail + cat.Email
            var bccEmails = new List<string>();
            if (EmailHelper.IsValid(_fixedEmail))
                bccEmails.Add(_fixedEmail!);
            if (EmailHelper.IsValid(catForMail.Email))
                bccEmails.Add(catForMail.Email!);

            if (EmailHelper.IsValid(_fixedEmail))
                adminEmails.Add(_fixedEmail!);

            _ = Task.Run(() => EnqueueEmailAsync(
                input, portalId, studentId, studentCode, DateTime.Now,
                catForMail, site, adminEmails, bccEmails), CancellationToken.None);
        }

        TempData["RegSuccess"]   = message;
        TempData["IsDuplicate"]  = isDuplicate;
        return RedirectToAction(nameof(Success));
    }

    // ── GET /dang-ky-su-kien/thanh-cong ──────────────────────────────────────

    [HttpGet("dang-ky-su-kien/thanh-cong")]
    public IActionResult Success()
    {
        ViewData["Title"] = "Đăng ký thành công";
        return View();
    }

    // ── GET /EventRegistration/CheckStudent?phone=x&email=y ──────────────────

    [HttpGet]
    public async Task<IActionResult> CheckStudent(string? phone, string? email)
    {
        var result = await _service.CheckStudentAsync(phone, email);
        return Json(result);
    }

    // ── Internal: POST payload to API, API enqueues into existing Hangfire ────

    private async Task EnqueueEmailAsync(
        EventRegistrationInputViewModel input,
        int portalId,
        int studentId,
        string studentCode,
        DateTime registeredAt,
        EventsCatViewModel cat,
        NVCMS.WebView.Data.SiteSettings.WebSiteSettings site,
        List<string> adminEmails,
        List<string> bccEmails)
    {
        try
        {
            var ev = cat.Events.FirstOrDefault(e => e.Id == input.EventId);
            if (ev is null) return;

            var eventDate = ev.Fromdatetime.HasValue
                ? ev.Fromdatetime.Value.ToString("dd/MM/yyyy")
                : cat.FromDate?.ToString("dd/MM/yyyy") ?? string.Empty;

            var eventTime = ev.Fromdatetime.HasValue
                ? ev.Fromdatetime.Value.ToString("HH:mm")
                : cat.FromDate?.ToString("HH:mm") ?? string.Empty;

            var payload = new
            {
                studentId,
                studentCode,
                studentName    = input.HoVaTen.Trim(),
                studentPhone   = PhoneHelper.Normalize(input.SoDienThoai),
                studentEmail   = input.Email ?? string.Empty,
                studentAddress = input.TinhThanh ?? string.Empty,
                eventCatId     = input.EventCatId,
                eventId        = input.EventId,
                eventName      = cat.CatName ?? string.Empty,
                eventLocation  = ev.Diadiem ?? string.Empty,
                eventDate,
                eventTime,
                registrationTime = registeredAt.ToString("HH:mm dd/MM/yyyy"),
                sendCode         = cat.Sendcode == true,
                importantNotes   = cat.ContentMail,
                companyLogoUrl   = site.Logo.HeaderLogo,
                siteUrl          = site.General.SiteWeb,
                siteName         = site.General.SiteName,
                adminEmails,
                bccEmails,
            };

            var json    = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var client  = _httpFactory.CreateClient("ApiClient");
            var response = await client.PostAsync("api/EventRegistrationEmail/enqueue", content);

            if (!response.IsSuccessStatusCode)
                _logger.LogWarning(
                    "EnqueueEmail API returned {Status} for StudentId={StudentId}",
                    (int)response.StatusCode, studentId);
            else
                _logger.LogInformation(
                    "EnqueueEmail OK: StudentId={StudentId} EventCatId={EventCatId}",
                    studentId, input.EventCatId);
        }
        catch (Exception ex)
        {
            // Email failure must NEVER fail the registration
            _logger.LogError(ex,
                "EnqueueEmailFailure: StudentId={StudentId} EventCatId={EventCatId}",
                studentId, input.EventCatId);
        }
    }

    // ── reCAPTCHA v3 verification ─────────────────────────────────────────────

    private async Task<bool> VerifyRecaptchaAsync(string token, int portalId)
    {
        if (string.IsNullOrWhiteSpace(token)) return false;
        try
        {
            var site = await _siteSettings.GetSettingsAsync(portalId);
            //var secret = site.Google.CaptchaSecret;
            if (string.IsNullOrWhiteSpace(_googlerecaptchav3_secretkey)) return true; // not configured → skip

            var client = _httpFactory.CreateClient();
            //var response = await client.PostAsync(
            //    $"https://www.google.com/recaptcha/api/siteverify?secret={Uri.EscapeDataString(_googlerecaptchav3_secretkey)}&response={Uri.EscapeDataString(token)}",
            //    null);

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("secret", _googlerecaptchav3_secretkey),
                    new KeyValuePair<string, string>("response", token)
                });

            var response = await client.PostAsync(
                "https://www.google.com/recaptcha/api/siteverify",
                content);

            if (!response.IsSuccessStatusCode) return false;

            var json = await response.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var success = doc.RootElement.TryGetProperty("success", out var s) && s.GetBoolean();
            var score = doc.RootElement.TryGetProperty("score", out var sc) ? sc.GetDouble() : 0.5;
            return success && score >= 0.5;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RecaptchaVerifyError");
            return true; // fail-open to avoid blocking legit users on API errors
        }
    }
}
