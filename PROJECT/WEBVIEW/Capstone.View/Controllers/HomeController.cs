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
    public async Task<IActionResult> DangKyTuVan([FromForm] TuVanFormInputViewModel model, CancellationToken ct)
    {

        if (!ModelState.IsValid)
        {
            return Json(new { success = false, message = "Vui lòng nhập đúng thông tin." });
        }

        try
        {
            await _tuVanFormService.SubmitAsync(model, _siteSettings.Value.PortalId, ct);
            return Json(new { success = true, message = "Đăng ký tư vấn thành công. Chúng tôi sẽ liên hệ sớm." });
        }
        catch (InvalidOperationException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception)
        {
            return Json(new { success = false, message = "Có lỗi xảy ra khi đăng ký. Vui lòng thử lại sau." });
        }
    }

    public IActionResult Error() => View();
}
