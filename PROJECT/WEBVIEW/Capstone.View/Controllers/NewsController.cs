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

    // GET /tin-tuc?page=1  (tất cả tin tức, phân trang)
    public async Task<IActionResult> All(int page = 1, int pageSize = 27)
    {
        var paged = await _news.GetAllPagedAsync(_portalId, page, pageSize);
        ViewData["Title"]        = "Tất cả tin tức";
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/tin-tuc";
        return View(paged);
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

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
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

            ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
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

            ViewData["Title"] = category.CategoryName;
            ViewData["MetaDescription"] = category.Description ?? category.CategoryName;
            ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/{category.Slug}";
            ViewData["Category"] = category;
            ViewData["Categories"] = await _news.GetCategoriesWithCountAsync(_portalId);

            return View("Category", paged);
        }
    }
}
