using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.Controllers;

/// <summary>
/// Routes:
///   /thong-tin-du-hoc/{countrySlug}
///   /thong-tin-du-hoc/{countrySlug}/{pageSlug}           (GioiThieu page OR news category)
///   /thong-tin-du-hoc/{countrySlug}/{pageSlug}/{**rest}  (news detail – handled by catch-all)
///   /thong-tin-du-hoc/{countrySlug}/danh-sach-truong
/// </summary>
public class ThongTinDuHocController : Controller
{
    private readonly ITruongService    _truongService;
    private readonly IGioiThieuService _gioiThieuService;
    private readonly INewsService      _newsService;
    private readonly int               _portalId;

    // countrySlug → GioiThieu page id (landing pages)
    private static readonly Dictionary<string, int> CountryPageIdMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "du-hoc-my",      408 },
            { "du-hoc-anh",     413 },
            { "du-hoc-canada",  412 },
            { "du-hoc-uc",      414 },
            { "du-hoc-ireland", 438 },
            { "du-hoc-thuy-si", 441 },
        };

    // "countrySlug/pageSlug" → GioiThieu page id (sub-pages)
    private static readonly Dictionary<string, int> SubPageIdMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "du-hoc-my/he-thong-giao-duc",       434 },
            { "du-hoc-my/visa",                    430 },
            { "du-hoc-anh/he-thong-giao-duc",      436 },
            { "du-hoc-anh/visa",                   432 },
            { "du-hoc-canada/he-thong-giao-duc",   435 },
            { "du-hoc-canada/visa",                431 },
            { "du-hoc-uc/he-thong-giao-duc",       437 },
            { "du-hoc-uc/visa",                    433 },
            { "du-hoc-ireland/he-thong-giao-duc",  440 },
            { "du-hoc-ireland/visa",               439 },
            { "du-hoc-thuy-si/he-thong-giao-duc",  443 },
            { "du-hoc-thuy-si/visa",               442 },
        };

    // "countrySlug/pageSlug" → news categoryId (tin tức theo nước)
    private static readonly Dictionary<string, int> NewsCategoryMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "du-hoc-my/tin-tuc-du-hoc-my",           234 },
            { "du-hoc-anh/tin-tuc-du-hoc-anh",         218 },
            { "du-hoc-canada/tin-tuc-du-hoc-canada",   219 },
            { "du-hoc-uc/tin-tuc-du-hoc-uc",           220 },
            { "du-hoc-uc/tin-tuc-du-hoc-ireland",      221 },
            { "du-hoc-thuy-si/tin-tuc-du-hoc-thuy-si", 222 },
        };

    private static readonly Dictionary<string, (int Id, string Ten)> CountryMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "du-hoc-my",          (38,  "Mỹ") },
            { "du-hoc-anh",         (28,  "Anh") },
            { "du-hoc-canada",      (3,   "Canada") },
            { "du-hoc-uc",          (1,   "Úc") },
            { "du-hoc-ireland",     (401, "Ireland") },
            { "du-hoc-thuy-si",     (23,  "Thụy Sĩ") },
            { "du-hoc-new-zealand", (99,  "New Zealand") },
        };

    private static readonly Dictionary<int, string> TruongSlugMap = new()
    {
        { 38,  "my" },
        { 28,  "anh" },
        { 3,   "canada" },
        { 1,   "uc" },
        { 401, "ireland" },
        { 23,  "thuy-si" },
        { 99,  "new-zealand" },
    };

    public ThongTinDuHocController(
        ITruongService         truongService,
        IGioiThieuService      gioiThieuService,
        INewsService           newsService,
        IOptions<SiteSettings> settings,
        IConfiguration         config)
    {
        _truongService    = truongService;
        _gioiThieuService = gioiThieuService;
        _newsService      = newsService;
        _portalId         = config.GetValue<int>("SiteSettings:PortalId");
    }

    // GET /thong-tin-du-hoc/{countrySlug}
    public async Task<IActionResult> CountryPage(string countrySlug)
    {
        if (!CountryPageIdMap.TryGetValue(countrySlug, out var pageId))
            return NotFound();

        var vm = await _gioiThieuService.GetByIdAsync(pageId, _portalId);
        if (vm is null) return NotFound();

        ViewData["Title"]        = vm.TrangDanhMuc;
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/thong-tin-du-hoc/{countrySlug}";
        ViewData["MenuGroupUrl"] = $"/thong-tin-du-hoc/{countrySlug}";

        return View("~/Views/ThongTinDuHoc/GioiThieuPage.cshtml", vm);
    }

    // GET /thong-tin-du-hoc/{countrySlug}/{pageSlug}  — GioiThieu page or news category list
    public async Task<IActionResult> SubPage(string countrySlug, string pageSlug)
    {
        var key = $"{countrySlug}/{pageSlug}";

        // 1. GioiThieu static page
        if (SubPageIdMap.TryGetValue(key, out var pageId))
        {
            var vm = await _gioiThieuService.GetByIdAsync(pageId, _portalId);
            if (vm is null) return NotFound();

            ViewData["Title"]        = vm.TrangDanhMuc;
            ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/thong-tin-du-hoc/{key}";
            ViewData["MenuGroupUrl"] = $"/thong-tin-du-hoc/{countrySlug}";

            return View("~/Views/ThongTinDuHoc/GioiThieuPage.cshtml", vm);
        }

        // 2. News category list page
        if (NewsCategoryMap.TryGetValue(key, out var categoryId))
        {
            var category = await _newsService.GetCategoryByIdAsync(categoryId);
            if (category is null) return NotFound();

            var page  = int.TryParse(Request.Query["page"], out var p) && p > 0 ? p : 1;
            var paged = await _newsService.GetByCategoryIdAsync(categoryId, _portalId, page, 27);

            ViewData["Title"]            = category.CategoryName;
            ViewData["MetaDescription"]  = category.Description ?? category.CategoryName;
            ViewData["CanonicalUrl"]     = $"{Request.Scheme}://{Request.Host}/thong-tin-du-hoc/{key}";
            ViewData["Category"]         = category;
            ViewData["Categories"]       = await _newsService.GetCategoriesWithCountAsync(_portalId);

            return View("~/Views/News/Category.cshtml", paged);
        }

        return NotFound();
    }

    // GET /thong-tin-du-hoc/{countrySlug}/danh-sach-truong
    public async Task<IActionResult> DanhSachTruong(string countrySlug, TruongSearchFilterViewModel filter)
    {
        if (!CountryMap.TryGetValue(countrySlug, out var country))
            return NotFound();

        filter.QuocGia   = country.Id;
        filter.IsPartner = true;
        if (filter.PageSize <= 0) filter.PageSize = 12;

        var vm = await _truongService.SearchAsync(filter);

        ViewData["Title"]   = $"Danh sách trường du học {country.Ten}";
        ViewBag.CountryName = country.Ten;
        ViewBag.CountrySlug = TruongSlugMap.TryGetValue(country.Id, out var ts) ? ts : countrySlug;
        ViewBag.CountryId   = country.Id;
        ViewBag.BaseAction  = $"/thong-tin-du-hoc/{countrySlug}/danh-sach-truong";

        return View("~/Views/Truong/QuocGia.cshtml", vm);
    }
}
