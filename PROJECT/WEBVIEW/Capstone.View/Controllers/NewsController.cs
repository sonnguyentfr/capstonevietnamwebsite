using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Common;

namespace Capstone.View.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _news;
    private readonly INewsUrlService _urlService;
    private readonly int _portalId;

    public NewsController(INewsService news, INewsUrlService urlService, IConfiguration config)
    {
        _news       = news;
        _urlService = urlService;
        _portalId   = config.GetValue<int>("SiteSettings:PortalId");
    }

    // GET /tin-tuc?tag=...&page=1
    public async Task<IActionResult> Tag(string tag, int page = 1, int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(tag)) return RedirectToAction(nameof(All));

        var paged = await _news.GetByTagAsync(tag.Trim(), _portalId, page, pageSize);

        ViewData["TrangDanhMuc"]        = $"#{tag}";
        ViewData["Tag"]          = tag;
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/tin-tuc/tag?tag={Uri.EscapeDataString(tag)}";

        return View(paged);
    }

    // GET /tin-tuc?page=1  (tất cả tin tức, phân trang)
    // GET /tin-tuc?tag=...  → 301 redirect sang /tin-tuc/tag?tag=...
    public async Task<IActionResult> All(int page = 1, int pageSize = 27)
    {
        var tag = Request.Query["tag"].ToString();
        if (!string.IsNullOrWhiteSpace(tag))
        {
            var qs = page > 1
                ? $"/tin-tuc/tag?tag={Uri.EscapeDataString(tag)}&page={page}"
                : $"/tin-tuc/tag?tag={Uri.EscapeDataString(tag)}";
            return RedirectPermanent(qs);
        }
        var paged = await _news.GetAllPagedAsync(_portalId, page, pageSize);
        ViewData["TrangDanhMuc"]        = "Tất cả tin tức";
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/tin-tuc";
        return View(paged);
    }

    // GET /tu-van-dinh-cu/tin-tuc-dinh-cu?page=1
    public async Task<IActionResult> TinTucDinhCu(int page = 1, int pageSize = 27)
        => await CategoryById(202, page, pageSize, "/tu-van-dinh-cu/tin-tuc-dinh-cu");

    // GET /tu-van-dinh-cu/tin-tuc-dau-tu?page=1
    public async Task<IActionResult> TinTucDauTu(int page = 1, int pageSize = 27)
        => await CategoryById(196, page, pageSize, "/tu-van-dinh-cu/tin-tuc-dau-tu");

    private async Task<IActionResult> CategoryById(int categoryId, int page, int pageSize, string canonicalPath)
    {
        var cat = await _news.GetCategoryByIdAsync(categoryId);
        if (cat is null) return NotFound();

        var paged = await _news.GetByCategoryIdAsync(categoryId, _portalId, page, pageSize);

        ViewData["TrangDanhMuc"]           = cat.CategoryName;
        ViewData["MetaDescription"] = cat.Description ?? cat.CategoryName;
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}{canonicalPath}";
        ViewData["BaseUrl"]         = canonicalPath;
        ViewData["Category"]        = cat;
        ViewData["Categories"]      = await _news.GetCategoriesWithCountAsync(_portalId);

        return View("Category", paged);
    }

    // GET /tu-van-dinh-cu/{slug}-{id:int}
    public async Task<IActionResult> DetailDinhCu(int id, string? slug)
        => await DetailByCategory(id, slug, 202, "tu-van-dinh-cu", "news-dinh-cu-detail");

    // GET /tu-van-dau-tu/{slug}-{id:int}
    public async Task<IActionResult> DetailDauTu(int id, string? slug)
        => await DetailByCategory(id, slug, 196, "tu-van-dau-tu", "news-dau-tu-detail");

    private async Task<IActionResult> DetailByCategory(
        int id, string? slug, int expectedCategoryId, string categoryPath, string routeName)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        // Canonical slug redirect
        if (!string.Equals(slug, vm.Slug, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent(routeName, new { slug = vm.Slug, id });

        // Ensure news belongs to expected category
        if (vm.CategoryId != expectedCategoryId) return NotFound();

        ViewData["TrangDanhMuc"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}/{categoryPath}/{vm.Slug}-{id}";

        // Override CategorySlug so breadcrumb/related links use canonical path
        vm.CategorySlug = categoryPath;

        return View("Detail", vm);
    }

    // GET /tin-tuc/{slug}-{id:int}
    public async Task<IActionResult> Detail(int id, string? slug)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        // FullSlug canonical redirect
        var canonical = vm.Slug;
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("news-detail", new { slug = canonical, id });

        ViewData["TrangDanhMuc"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = NewsUrlBuilder.BuildFullNewsUrl(Request.Scheme, Request.Host.Value, vm);

        return View(vm);
    }

    // GET /{**path} — catch-all route for category or detail
    public async Task<IActionResult> DetailCatchAll(string path)
    {
        if (string.IsNullOrEmpty(path)) return NotFound();

        // Try extract newsId from path (e.g., "cam-nang/article-123")
        var newsId = _urlService.ExtractNewsId(path);
        
        if (newsId.HasValue)
        {
            // This is a detail URL
            var news = await _news.GetByIdAsync(newsId.Value, _portalId);
            if (news is null) return NotFound();

            var canonicalUrl = await _urlService.BuildCanonicalUrl(news, Request.Scheme, Request.Host.Value);
            if (canonicalUrl is null) return NotFound();

            var requestUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";
            if (!_urlService.IsCanonical(requestUrl, canonicalUrl))
                return RedirectPermanent(canonicalUrl);

            var vm = await _news.GetDetailAsync(newsId.Value, _portalId);
            if (vm is null) return NotFound();

            ViewData["TrangDanhMuc"]           = vm.MetaTitle ?? vm.Title;
            ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
            ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
            ViewData["CanonicalUrl"]    = canonicalUrl;

            return View("Detail", vm);
        }
        else
        {
            // Try as category by FullSlug
            var category = await _news.GetCategoryByFullSlugAsync(path, _portalId);
            if (category == null) return NotFound();

            var page = int.TryParse(Request.Query["page"], out var p) && p > 0 ? p : 1;
            var paged = await _news.GetByCategoryIdAsync(category.CategoryID, _portalId, page, 27);

            ViewData["TrangDanhMuc"] = category.CategoryName;
            ViewData["MetaDescription"] = category.Description ?? category.CategoryName;
            ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/{category.Slug}";
            ViewData["Category"] = category;
            ViewData["Categories"] = await _news.GetCategoriesWithCountAsync(_portalId);

            return View("Category", paged);
        }
    }
}
