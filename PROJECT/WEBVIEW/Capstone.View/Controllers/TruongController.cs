using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Controllers;

public class TruongController : Controller
{
    private readonly ITruongService _truongService;
    private readonly int _portalId;

    private static readonly Dictionary<string, int> CountrySlugMap = new(StringComparer.OrdinalIgnoreCase)
    {
        {"my",          38},
        {"usa",         38},
        {"uc",          1},
        {"australia",   1},
        {"canada",      3},
        {"anh",         28},
        {"uk",          28},
        {"ireland",     401},
        {"thuy-si",     23},
        {"switzerland", 23},
        {"new-zealand", 99},
    };

    public TruongController(ITruongService truongService, IOptions<SiteSettings> settings)
    {
        _truongService = truongService;
        _portalId = settings.Value.PortalId;
    }

    // Parse comma-separated or repeated query param values into List<int>
    // Supports: ?majorids=62,56  OR  ?majorids=62&majorids=56
    private static List<int> ParseIntList(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return [];
        return raw.Split(',', StringSplitOptions.RemoveEmptyEntries)
                  .Select(s => int.TryParse(s.Trim(), out var n) ? (int?)n : null)
                  .Where(n => n.HasValue)
                  .Select(n => n!.Value)
                  .ToList();
    }

    private static void NormalizeFilter(TruongSearchFilterViewModel filter, string? majorids, string? quocgiaids)
    {
        if (!string.IsNullOrWhiteSpace(majorids))
            filter.MajorIds = ParseIntList(majorids);
        if (!string.IsNullOrWhiteSpace(quocgiaids))
            filter.QuocGiaIds = ParseIntList(quocgiaids);
    }

    // GET /truong-doi-tac
    public async Task<IActionResult> Index(TruongSearchFilterViewModel filter, string? majorids, string? quocgiaids)
    {
        NormalizeFilter(filter, majorids, quocgiaids);
        filter.IsPartner = true;
        var vm = await _truongService.SearchAsync(filter);
        ViewData["TrangDanhMuc"] = "Trường đối tác";
        return View(vm);
    }

    // GET /tim-truong  (public search, no partner filter)
    public async Task<IActionResult> TimTruong(TruongSearchFilterViewModel filter, string? majorids, string? quocgiaids)
    {
        NormalizeFilter(filter, majorids, quocgiaids);
        var vm = await _truongService.SearchAsync(filter);
        ViewData["TrangDanhMuc"] = "Tìm trường";
        return View(vm);
    }

    // GET /truong-doi-tac/{countrySlug}
    public async Task<IActionResult> QuocGia(string countrySlug, TruongSearchFilterViewModel filter, string? majorids, string? quocgiaids)
    {
        if (!CountrySlugMap.TryGetValue(countrySlug, out var countryId))
            return NotFound();

        NormalizeFilter(filter, majorids, quocgiaids);
        filter.QuocGia   = countryId;
        filter.IsPartner = true;
        if (filter.PageSize <= 0) filter.PageSize = 20;

        var vm       = await _truongService.SearchAsync(filter);
        var countries = await _truongService.GetCountriesAsync();
        var countryName = countries.FirstOrDefault(c => c.Id == countryId)?.Ten ?? countrySlug;

        ViewData["TrangDanhMuc"]       = $"Trường đối tác tại {countryName}";
        ViewBag.CountrySlug     = countrySlug;
        ViewBag.CountryName     = countryName;
        ViewBag.CountryId       = countryId;
        ViewBag.Countries       = countries;
        return View(vm);
    }

    // GET /truong-doi-tac/{slug}-{id}
    public async Task<IActionResult> Detail(string slug, int id)
    {
        var vm = await _truongService.GetDetailAsync(id);
        if (vm is null) return NotFound();

        var canonical = SlugHelper.ToSlug(vm.NameofSchool ?? string.Empty);
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("truong-detail", new { slug = canonical, id });

        ViewData["TrangDanhMuc"] = vm.NameofSchool;
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/truong-doi-tac/{canonical}-{id}";
        return View(vm);
    }

    // GET /tim-nganh-hoc
    public async Task<IActionResult> TimNganhHoc(string? filter, int? quocGia, string? loai)
    {
        var vm = await _truongService.GetMajorSearchAsync(filter, quocGia, loai);
        ViewData["TrangDanhMuc"] = "Tìm ngành học";
        return View(vm);
    }

    // AJAX: GET /truong-doi-tac/search-json
    public async Task<IActionResult> SearchJson(TruongSearchFilterViewModel filter, string? majorids, string? quocgiaids)
    {
        NormalizeFilter(filter, majorids, quocgiaids);
        var vm = await _truongService.SearchAsync(filter);
        return Json(new
        {
            items = vm.Items,
            total = vm.Total,
            page = vm.Page,
            totalPages = vm.TotalPages
        });
    }
}
