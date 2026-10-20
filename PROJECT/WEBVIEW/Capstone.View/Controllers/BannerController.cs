using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.Controllers;

/// <summary>
/// Thống kê click banner (thay cho bannerclick.aspx của site cũ).
///
/// POST /api/Banner/click/{id}
///   → gọi từ site.js bằng navigator.sendBeacon khi click thẻ a[data-banner-id]
///   → EXEC NVCMS_Banner_UpdateClick @BannerId
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BannerController : ControllerBase
{
    private readonly IBannerService _bannerService;
    private readonly ILogger<BannerController> _logger;

    public BannerController(IBannerService bannerService, ILogger<BannerController> logger)
    {
        _bannerService = bannerService;
        _logger = logger;
    }

    [HttpPost("click/{id:int}")]
    [EnableRateLimiting("form")]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Click(int id)
    {
        if (id <= 0) return NoContent();

        try
        {
            await _bannerService.UpdateClickAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "BannerClick track failed BannerId={Id}", id);
        }

        return NoContent();
    }
}
