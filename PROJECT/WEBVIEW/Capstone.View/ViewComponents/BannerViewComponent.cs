using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

public class BannerViewComponent : ViewComponent
{
    private readonly IBannerService _bannerService;
    private readonly int _portalId;

    public BannerViewComponent(IBannerService bannerService, IOptions<SiteSettings> settings)
    {
        _bannerService = bannerService;
        _portalId      = settings.Value.PortalId;
    }

    public async Task<IViewComponentResult> InvokeAsync(int vitriid)
    {
        var banners = await _bannerService.GetAllShowAsync(_portalId, vitriid);
        return View($"Vitri{vitriid}", banners);
    }
}
