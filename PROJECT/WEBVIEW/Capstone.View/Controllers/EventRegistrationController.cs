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
    private readonly IEventRegistrationService _service;
    private readonly IEventsService            _events;
    private readonly ISiteSettingsHelper       _siteSettings;
    private readonly IHttpClientFactory        _httpFactory;
    private readonly IOptions<SiteSettings>    _siteOptions;
    private readonly ILogger<EventRegistrationController> _logger;

    public EventRegistrationController(
        IEventRegistrationService service,
        IEventsService            events,
        ISiteSettingsHelper       siteSettings,
        IHttpClientFactory        httpFactory,
        IOptions<SiteSettings>    siteOptions,
        ILogger<EventRegistrationController> logger)
    {
        _service      = service;
        _events       = events;
        _siteSettings = siteSettings;
        _httpFactory  = httpFactory;
        _siteOptions  = siteOptions;
        _logger       = logger;
    }

    // ── GET /dang-ky-su-kien?eventCatId=x&eventId=y ──────────────────────────

    [HttpGet]
    public async Task<IActionResult> Index(int eventCatId, int eventId = 0)
    {
        var cat = await _events.GetCatWithEventsAsync(eventCatId, _siteOptions.Value.PortalId);
        if (cat is null) return NotFound();

        // Only allow registration during active window
        var now = DateTime.Now;
        bool isOpen = (!cat.FromDate.HasValue || now >= cat.FromDate.Value.Date)
                   && (!cat.EndDate.HasValue  || now <= cat.EndDate.Value);
        if (!isOpen)
        {
            ViewData["Title"] = "Đăng ký đã đóng";
            return View("Closed");
        }

        // Resolve the pre-selected event location (entry point 1) or default to first
        var selectedEvent = cat.Events.FirstOrDefault(e => e.Id == eventId)
                         ?? cat.Events.FirstOrDefault()
                         ?? new EventsViewModel();

        var vm = new EventRegistrationPageViewModel
        {
            Cat               = cat,
            Event             = selectedEvent,
            PreselectedEventId = eventId,
            Input = new EventRegistrationInputViewModel
            {
                EventCatId = eventCatId,
                EventId    = selectedEvent.Id,
            }
        };

        ViewData["Title"] = $"Đăng ký tham dự - {cat.CatName}";
        return View(vm);
    }

    // ── POST /dang-ky-su-kien ─────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(EventRegistrationInputViewModel input, CancellationToken ct)
    {
        var portalId = _siteOptions.Value.PortalId;

        if (!ModelState.IsValid)
        {
            var cat = await _events.GetCatWithEventsAsync(input.EventCatId, portalId);
            var ev  = cat?.Events.FirstOrDefault(e => e.Id == input.EventId) ?? new EventsViewModel();
            return View(new EventRegistrationPageViewModel
            {
                Cat               = cat ?? new EventsCatViewModel(),
                Event             = ev,
                PreselectedEventId = input.EventId,
                Input             = input,
            });
        }

        var (success, isDuplicate, message, studentId, studentCode) =
            await _service.RegisterAsync(input, portalId, ct);

        if (isDuplicate)
        {
            TempData["RegWarning"] = message;
            _logger.LogWarning(
                "DuplicateRegistration: StudentId={StudentId} EventId={EventId}",
                studentId, input.EventId);
            return RedirectToAction(nameof(Index), new { eventCatId = input.EventCatId, eventId = input.EventId });
        }

        if (!success)
        {
            TempData["RegError"] = message;
            return RedirectToAction(nameof(Index), new { eventCatId = input.EventCatId, eventId = input.EventId });
        }

        _logger.LogInformation(
            "RegistrationSuccess: StudentId={StudentId} EventCatId={EventCatId} EventId={EventId}",
            studentId, input.EventCatId, input.EventId);

        // ── Fire-and-forget to API → Hangfire enqueues the email job ─────────
        _ = Task.Run(() => EnqueueEmailAsync(
            input, portalId, studentId, studentCode, DateTime.Now), CancellationToken.None);

        TempData["RegSuccess"] = message;
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
        int      portalId,
        int      studentId,
        string   studentCode,
        DateTime registeredAt)
    {
        try
        {
            var cat  = await _events.GetCatWithEventsAsync(input.EventCatId, portalId);
            var ev   = cat?.Events.FirstOrDefault(e => e.Id == input.EventId);
            var site = await _siteSettings.GetSettingsAsync(portalId);

            if (cat is null || ev is null) return;

            var eventDate = ev.Fromdatetime.HasValue
                ? ev.Fromdatetime.Value.ToString("dd/MM/yyyy")
                : cat.FromDate?.ToString("dd/MM/yyyy") ?? string.Empty;

            var eventTime = ev.Fromdatetime.HasValue
                ? ev.Fromdatetime.Value.ToString("HH:mm")
                : cat.FromDate?.ToString("HH:mm") ?? string.Empty;

            var adminEmails = (site.Mail.MailList ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Where(EmailHelper.IsValid)
                .ToList();

            var fixedEmail = HttpContext.RequestServices
                .GetRequiredService<IConfiguration>()["Email:FixedEmail"];
            if (EmailHelper.IsValid(fixedEmail))
                adminEmails.Add(fixedEmail!);

            var payload = new
            {
                studentId,
                studentCode,
                studentName      = input.HoVaTen.Trim(),
                studentPhone     = PhoneHelper.Normalize(input.SoDienThoai),
                studentEmail     = input.Email     ?? string.Empty,
                studentAddress   = input.DiaChi    ?? string.Empty,
                eventCatId       = input.EventCatId,
                eventId          = input.EventId,
                eventName        = cat.CatName ?? string.Empty,
                eventLocation    = ev.Diadiem  ?? string.Empty,
                eventDate,
                eventTime,
                registrationTime = registeredAt.ToString("HH:mm dd/MM/yyyy"),
                sendCode         = cat.Sendcode == true,
                importantNotes   = cat.ContentMail,
                companyLogoUrl   = site.Logo.HeaderLogo,
                siteUrl          = site.General.SiteWeb,
                siteName         = site.General.SiteName,
                adminEmails,
            };

            var json     = JsonSerializer.Serialize(payload);
            var content  = new StringContent(json, Encoding.UTF8, "application/json");
            var client   = _httpFactory.CreateClient("ApiClient");
            var response = await client.PostAsync("api/EventRegistrationEmail/enqueue", content);

            if (!response.IsSuccessStatusCode)
                _logger.LogWarning(
                    "EnqueueEmail API returned {Status} for StudentId={StudentId}",
                    (int)response.StatusCode, studentId);
        }
        catch (Exception ex)
        {
            // Email failure must NEVER fail the registration
            _logger.LogError(ex,
                "EnqueueEmailFailure: StudentId={StudentId} EventCatId={EventCatId}",
                studentId, input.EventCatId);
        }
    }
}
