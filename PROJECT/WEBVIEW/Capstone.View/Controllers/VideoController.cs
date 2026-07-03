using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.Controllers;

public class VideoController : Controller
{
    private readonly IVideoService _service;
    private readonly int           _portalId;
    private const    int           PageSize = 40;

    public VideoController(IVideoService service, IConfiguration config)
    {
        _service  = service;
        _portalId = config.GetValue<int>("SiteSettings:PortalId");
    }

    // GET /gioi-thieu/thu-vien-anh-video
    public async Task<IActionResult> Index()
    {
        var items = await _service.GetVideosAsync(_portalId, 1, PageSize);

        ViewData["Title"]           = "Thư viện ảnh & Video";
        ViewData["MetaDescription"] = "Xem thư viện ảnh và video các sự kiện, hội thảo du học của Capstone Vietnam.";
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}/gioi-thieu/thu-vien-anh-video";

        return View(items);
    }

    // GET /gioi-thieu/thu-vien-anh-video/load-more?page=2
    public async Task<IActionResult> LoadMore(int page = 2)
    {
        if (page < 2) return BadRequest();
        var items = await _service.GetVideosAsync(_portalId, page, PageSize);
        return PartialView("_PartialVideoItem", items);
    }

    // GET /gioi-thieu/thu-vien-anh-video/detail/5
    public async Task<IActionResult> Detail(int id)
    {
        var vm = await _service.GetVideoAsync(id, _portalId);
        if (vm is null) return NotFound();
        return Json(vm);
    }
}
