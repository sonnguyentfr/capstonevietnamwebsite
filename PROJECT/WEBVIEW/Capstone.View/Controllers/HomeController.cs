using Capstone.View.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _http;
    private readonly ITuVanFormService _tuVanFormService;
    private readonly IOptions<SiteSettings> _siteSettings;

    public HomeController(
        IHttpClientFactory http,
        ITuVanFormService tuVanFormService,
        IOptions<SiteSettings> siteSettings)
    {
        _http = http;
        _tuVanFormService = tuVanFormService;
        _siteSettings = siteSettings;
    }

    public IActionResult Index() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DangKyTuVan(TuVanFormInputViewModel model, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            TempData["TuVanError"] = "Vui lòng nhập đúng thông tin. Cần ít nhất Email hoặc Số điện thoại.";
            return RedirectToAction(nameof(Index));
        }

        await _tuVanFormService.SubmitAsync(model, _siteSettings.Value.PortalId, ct);
        TempData["TuVanSuccess"] = "Đăng ký tư vấn thành công. Chúng tôi sẽ liên hệ sớm.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Error() => View();
}
