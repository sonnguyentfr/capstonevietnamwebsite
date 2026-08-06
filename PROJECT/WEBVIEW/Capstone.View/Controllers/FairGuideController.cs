using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.Controllers;

public class FairGuideController : Controller
{
    private readonly IFairGuideService _service;
    private readonly int               _portalId;

    public FairGuideController(IFairGuideService service, IConfiguration config)
    {
        _service  = service;
        _portalId = config.GetValue<int>("SiteSettings:PortalId");
    }

    // GET /gioi-thieu/fairguide
    public async Task<IActionResult> Index()
    {
        var items = await _service.GetAllAsync(_portalId);

        ViewData["TrangDanhMuc"]        = "FairGuide";
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/gioi-thieu/fairguide";

        return View(items);
    }

    // GET /gioi-thieu/fairguide/{slug}-{id}
    public async Task<IActionResult> Detail(int id, string? slug)
    {
        var vm = await _service.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        if (!string.Equals(slug, vm.Slug, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("fairguide-detail", new { slug = vm.Slug, id });

        var canonical = $"{Request.Scheme}://{Request.Host}/gioi-thieu/fairguide/{vm.Slug}-{vm.Id}";

        ViewData["TrangDanhMuc"]           = vm.Title;
        ViewData["MetaDescription"] = vm.Descreption;
        ViewData["CanonicalUrl"]    = canonical;

        return View(vm);
    }
}
