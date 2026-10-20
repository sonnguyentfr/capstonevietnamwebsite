using Capstone.View.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.Controllers;

/// <summary>
/// Thống kê banner (thay cho bannerclick.aspx của site cũ). Gọi từ site.js bằng navigator.sendBeacon.
///
/// POST /api/Banner/click/{id}       → khi click thẻ a[data-banner-id]    → EXEC NVCMS_Banner_UpdateClick (cột Click)
/// POST /api/Banner/view?ids=1,2,3   → khi banner [data-banner-view] hiện  → EXEC NVCMS_Banner_UpdateView  (cột sView)
///
/// Dùng policy "track" riêng: KHÔNG dùng chung "form" (3 lần/phút) — vừa bị 429 khi test click,
/// vừa ăn mất quota của form đăng ký tư vấn.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BannerController : ControllerBase
{
    private const int MaxViewIds = 20;

    private readonly IBannerService _bannerService;
    private readonly ILogger<BannerController> _logger;

    public BannerController(IBannerService bannerService, ILogger<BannerController> logger)
    {
        _bannerService = bannerService;
        _logger = logger;
    }

    [HttpPost("click/{id:int}")]
    [EnableRateLimiting(RateLimitingSetup.TrackPolicy)]
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

    [HttpPost("view")]
    [EnableRateLimiting(RateLimitingSetup.TrackPolicy)]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Impression([FromQuery] string? ids)
    {
        var list = (ids ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(s => int.TryParse(s, out var v) ? v : 0)
            .Where(v => v > 0)
            .Distinct()
            .Take(MaxViewIds)
            .ToList();

        if (list.Count == 0) return NoContent();

        try
        {
            await _bannerService.UpdateViewAsync(list);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "BannerView track failed BannerIds={Ids}", string.Join(',', list));
        }

        return NoContent();
    }
}
