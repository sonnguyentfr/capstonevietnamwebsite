using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// ViewComponent dùng chung cho mọi vị trí banner.
/// View được chọn theo vitriid: Views/Shared/Components/Banner/Vitri{vitriid}.cshtml
/// Sử dụng: @await Component.InvokeAsync("Banner", new { vitriid = 1 })
/// </summary>
public class BannerViewComponent : ViewComponent
{
    private readonly IBannerService _bannerService;
    private readonly int _portalId;

    public BannerViewComponent(IBannerService bannerService, IConfiguration config)
    {
        _bannerService = bannerService;
        _portalId      = config.GetValue<int>("SiteSettings:PortalId");
    }

    public async Task<IViewComponentResult> InvokeAsync(int vitriid)
    {
        var banners = await _bannerService.GetAllShowAsync(_portalId, vitriid);
        return View($"Vitri{vitriid}", banners);
    }
}
