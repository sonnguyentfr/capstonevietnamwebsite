using Capstone.View.Options;
using Capstone.View.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Controllers;

/// <summary>Trang /dang-ky-tu-van – đẩy thông tin vào Student_Info (CRM).</summary>
public class TuVanDangKyController : Controller
{
    private const int NamDiRange = 6;

    private readonly ITuVanDangKyService _service;
    private readonly TuVanDangKyMailService _mailService;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IOptions<SiteSettings> _siteOptions;
    private readonly TuVanDangKyOptions _tuVanOptions;
    private readonly ILogger<TuVanDangKyController> _logger;
    private readonly string _recaptchaSiteKey;
    private readonly string _recaptchaSecretKey;

    public TuVanDangKyController(
        ITuVanDangKyService service,
        TuVanDangKyMailService mailService,
        IHttpClientFactory httpFactory,
        IOptions<SiteSettings> siteOptions,
        IOptions<TuVanDangKyOptions> tuVanOptions,
        IConfiguration config,
        ILogger<TuVanDangKyController> logger)
    {
        _service = service;
        _mailService = mailService;
        _httpFactory = httpFactory;
        _siteOptions = siteOptions;
        _tuVanOptions = tuVanOptions.Value;
        _logger = logger;
        _recaptchaSiteKey = config["Google:recaptchav3_sitekey"] ?? string.Empty;
        _recaptchaSecretKey = config["Google:recaptchav3_secretkey"] ?? string.Empty;
    }

    // ── GET /dang-ky-tu-van ───────────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await BuildPageAsync(new TuVanDangKyInputViewModel()));
    }

    // ── POST /dang-ky-tu-van ──────────────────────────────────────────────────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        [Bind(Prefix = "Input")] TuVanDangKyInputViewModel input, CancellationToken ct)
    {
        var recaptchaToken = Request.Form["g-recaptcha-response"].ToString();
        if (!await VerifyRecaptchaAsync(recaptchaToken))
            ModelState.AddModelError(string.Empty, "Xác thực reCAPTCHA thất bại. Vui lòng thử lại.");

        if (!ModelState.IsValid)
            return View(await BuildPageAsync(input));

        TuVanDangKyResult result;
        try
        {
            result = await _service.SubmitAsync(input, _siteOptions.Value.PortalCRMId, _tuVanOptions, ct);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "TuVanDangKy submit failed: Phone={Phone} Email={Email}", input.SoDienThoai, input.Email);
            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi gửi đăng ký. Vui lòng thử lại sau.");
            return View(await BuildPageAsync(input));
        }

        AfterSubmit(result);

        TempData["TuVanName"] = result.HoVaTen;
        return RedirectToAction(nameof(Success));
    }

    // ── POST /dang-ky-tu-van/nhanh (AJAX – form sidebar trang chi tiết) ───────

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Nhanh(TuVanNhanhInputViewModel input, CancellationToken ct)
    {
        var recaptchaToken = Request.Form["g-recaptcha-response"].ToString();
        if (!await VerifyRecaptchaAsync(recaptchaToken))
            return Json(new { success = false, message = "Xác thực reCAPTCHA thất bại. Vui lòng thử lại." });

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .Where(m => !string.IsNullOrWhiteSpace(m))
                .Distinct()
                .ToList();
            return Json(new { success = false, message = string.Join(" ", errors), errors });
        }

        try
        {
            var result = await _service.SubmitAsync(input, _siteOptions.Value.PortalCRMId, _tuVanOptions, ct);
            AfterSubmit(result);
            return Json(new
            {
                success = true,
                message = "Cảm ơn bạn! Capstone đã nhận được thông tin và sẽ liên hệ lại trong thời gian sớm nhất."
            });
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "TuVanNhanh submit failed: Nguon={Nguon} Phone={Phone} Email={Email}",
                input.NguonTrang, input.SoDienThoai, input.Email);
            return Json(new { success = false, message = "Có lỗi xảy ra khi gửi đăng ký. Vui lòng thử lại sau." });
        }
    }

    // ── GET /dang-ky-tu-van/thanh-cong ────────────────────────────────────────

    [HttpGet]
    public IActionResult Success()
    {
        ViewData["TrangDanhMuc"] = "Đăng ký tư vấn thành công";
        return View();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    /// <summary>Ghi log + gửi mail thông báo nội bộ ở background (không làm chậm/fail đăng ký).</summary>
    private void AfterSubmit(TuVanDangKyResult result)
    {
        _logger.LogInformation("TuVanDangKy: StudentId={StudentId} Code={Code} IsExisting={IsExisting} Nguon={Nguon}",
            result.StudentId, result.StudentCode, result.IsExisting, result.NguonTrang);

        var notifyEmails = _tuVanOptions.NotifyEmails;
        _ = Task.Run(async () =>
        {
            try
            {
                await _mailService.SendNotifyAsync(result, notifyEmails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TuVanDangKy notify mail failed: StudentId={StudentId}", result.StudentId);
            }
        }, CancellationToken.None);
    }

    private async Task<TuVanDangKyPageViewModel> BuildPageAsync(TuVanDangKyInputViewModel input)
    {
        var catalogs = await _service.GetCatalogsAsync(_siteOptions.Value.PortalCRMId);
        var popularIds = _tuVanOptions.PopularCountryIds;
        var year = DateTime.Now.Year;

        ViewData["TrangDanhMuc"] = "Đăng ký tư vấn";
        ViewBag.RecaptchaSiteKey = _recaptchaSiteKey;

        return new TuVanDangKyPageViewModel
        {
            Catalogs = catalogs,
            PopularQuocGia = popularIds
                .Select(id => catalogs.QuocGia.FirstOrDefault(x => x.Id == id))
                .OfType<TuVanOption>()
                .ToList(),
            NamDiOptions = Enumerable.Range(year, NamDiRange).ToList(),
            Input = input,
        };
    }

    private async Task<bool> VerifyRecaptchaAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(_recaptchaSecretKey)) return true; // chưa cấu hình → bỏ qua
        if (string.IsNullOrWhiteSpace(token)) return false;
        try
        {
            var client = _httpFactory.CreateClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("secret", _recaptchaSecretKey),
                new KeyValuePair<string, string>("response", token)
            });

            var response = await client.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
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
            return true; // fail-open giống EventRegistration để không chặn khách thật khi Google lỗi
        }
    }
}
