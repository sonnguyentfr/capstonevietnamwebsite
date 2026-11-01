using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Controllers;

public class SuKienController : Controller
{
    private readonly IEventsService _events;
    private readonly int _portalCrmId;

    public SuKienController(IEventsService events, IOptions<SiteSettings> siteOptions)
    {
        _events = events;
        _portalCrmId = siteOptions.Value.PortalCRMId;
    }

    // GET /su-kien
    public async Task<IActionResult> Index()
    {
        var vm = new EventsPageViewModel
        {
            Upcoming = await _events.GetActiveCatsWithEventsAsync(_portalCrmId),
            Past     = []
        };
        ViewData["TrangDanhMuc"] = "Hội thảo - Sự kiện";
        return View(vm);
    }

    // GET /su-kien/past-paged?page=1&pageSize=24
    [HttpGet]
    public async Task<IActionResult> PastPaged(int page = 1, int pageSize = 24)
    {
        var (items, total) = await _events.GetPastCatsPagedAsync(_portalCrmId, page, pageSize);
        var list = items.Select(c => new
        {
            id       = c.Id,
            slug     = c.Slug,
            catName  = c.CatName,
            avatarUrl= c.AvatarUrl,
            fromDate = c.FromDate?.ToString("dd/MM/yyyy"),
            endDate  = c.EndDate?.ToString("dd/MM/yyyy"),
            desc     = c.Desception,
            linkPr   = c.Link_pr
        });
        return Json(new { items = list, total });
    }

    // GET /su-kien/{slug}-{id}
    public async Task<IActionResult> Detail(int id, string? slug)
    {
        var vm = await _events.GetCatWithEventsAsync(id, _portalCrmId);
        if (vm is null) return NotFound();

        var canonical = SlugHelper.ToSlug(vm.CatName);
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("su-kien-detail", new { slug = canonical, id });

        var description = vm.Desception is { Length: > 0 }
            ? System.Text.RegularExpressions.Regex.Replace(vm.Desception, "<.*?>", "").Trim()
            : vm.CatName;
        if (description.Length > 160) description = description[..157] + "...";

        ViewData["TrangDanhMuc"]           = vm.CatName;
        ViewData["MetaDescription"] = description;
        ViewData["MetaImage"]       = vm.AvatarUrl;
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}/su-kien/{canonical}-{id}";

        return View(vm);
    }
}
