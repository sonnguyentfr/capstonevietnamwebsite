using Capstone.View.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Form tư vấn nhanh ở sidebar các trang chi tiết (trường, tin tức).
/// Dùng: @await Component.InvokeAsync("TuVanNhanh", new { nguonTrang = TuVanNguonTrang.Truong, nguonTieuDe = Model.NameofSchool })
/// </summary>
public class TuVanNhanhViewComponent : ViewComponent
{
    private readonly ITuVanDangKyService _service;
    private readonly IOptions<SiteSettings> _siteOptions;
    private readonly TuVanDangKyOptions _tuVanOptions;
    private readonly string _recaptchaSiteKey;

    public TuVanNhanhViewComponent(
        ITuVanDangKyService service,
        IOptions<SiteSettings> siteOptions,
        IOptions<TuVanDangKyOptions> tuVanOptions,
        IConfiguration config)
    {
        _service = service;
        _siteOptions = siteOptions;
        _tuVanOptions = tuVanOptions.Value;
        _recaptchaSiteKey = config["Google:recaptchav3_sitekey"] ?? string.Empty;
    }

    public async Task<IViewComponentResult> InvokeAsync(TuVanNguonTrang nguonTrang, string? nguonTieuDe)
    {
        var catalogs = await _service.GetCatalogsAsync(_siteOptions.Value.PortalCRMId);

        ViewBag.RecaptchaSiteKey = _recaptchaSiteKey;
        ViewBag.PopularQuocGia = _tuVanOptions.PopularCountryIds
            .Select(id => catalogs.QuocGia.FirstOrDefault(x => x.Id == id))
            .OfType<TuVanOption>()
            .ToList();
        ViewBag.Catalogs = catalogs;

        return View(new TuVanNhanhInputViewModel
        {
            NguonTrang = nguonTrang,
            NguonTieuDe = nguonTieuDe,
        });
    }
}
