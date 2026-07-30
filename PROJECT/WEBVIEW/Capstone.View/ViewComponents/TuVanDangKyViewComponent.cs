using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.ViewComponents;

public class TuVanDangKyViewComponent : ViewComponent
{
    private readonly IConfiguration _config;

    public TuVanDangKyViewComponent(IConfiguration config)
    {
        _config = config;
    }

    public IViewComponentResult Invoke()
    {
        ViewBag.RecaptchaSiteKey = _config["Google:recaptchav3_sitekey"];
        return View(new TuVanFormInputViewModel());
    }
}