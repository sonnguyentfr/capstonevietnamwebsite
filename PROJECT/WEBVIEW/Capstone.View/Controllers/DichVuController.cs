using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.Controllers;

public class DichVuController : Controller
{
    private readonly IGioiThieuService _gioiThieuService;
    private readonly int               _portalId;

    // slug → GioiThieu page id
    private static readonly Dictionary<string, int> PageIdMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "",                  417 }, // /tu-van-dinh-cu
            { "dinh-cu-my-eb5",   444 },
            { "dinh-cu-my-eb3",   445 },
            { "dinh-cu-canada",   446 },
            { "dinh-cu-uc",       447 },
        };

    public DichVuController(IGioiThieuService gioiThieuService, IConfiguration config)
    {
        _gioiThieuService = gioiThieuService;
        _portalId         = config.GetValue<int>("SiteSettings:PortalId");
    }

    // /dich-vu
    public IActionResult Index() => View();

    // /tu-van-dinh-cu
    public async Task<IActionResult> TuVanDinhCu()
        => await RenderPage("", "/tu-van-dinh-cu");

    // /tu-van-dinh-cu/{pageSlug}
    public async Task<IActionResult> TuVanDinhCuSubPage(string pageSlug)
        => await RenderPage(pageSlug, $"/tu-van-dinh-cu/{pageSlug}");

    // /dich-vu/*
    public IActionResult TuVanDuHocCacNuoc() => View();
    public IActionResult TuVanDuHocTruongTop() => View();
    public IActionResult TuVanDuHocCaoHoc() => View();
    public IActionResult TuVanNganhNghe() => View();
    public IActionResult TuVanVisa() => View();
    public IActionResult ChuyenTienDuHoc() => View();
    public IActionResult TimNha() => View();

    private async Task<IActionResult> RenderPage(string key, string canonicalPath)
    {
        if (!PageIdMap.TryGetValue(key, out var pageId))
            return NotFound();

        var vm = await _gioiThieuService.GetByIdAsync(pageId, _portalId);
        if (vm is null) return NotFound();

        ViewData["TrangDanhMuc"]        = vm.TrangDanhMuc;
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}{canonicalPath}";
        ViewData["MenuGroupUrl"] = "/tu-van-dinh-cu";

        return View("~/Views/ThongTinDuHoc/GioiThieuPage.cshtml", vm);
    }
}
