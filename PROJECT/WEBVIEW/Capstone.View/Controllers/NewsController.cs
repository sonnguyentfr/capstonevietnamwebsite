using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.Controllers;

public class NewsController : Controller
{
    private readonly INewsService _news;
    private readonly int _portalId;

    public NewsController(INewsService news, IConfiguration config)
    {
        _news     = news;
        _portalId = config.GetValue<int>("SiteSettings:PortalId");
    }

    // GET /cam-nang-va-tin-tuc
    public async Task<IActionResult> Index()
    {
        var categories = await _news.GetCategoriesWithCountAsync(_portalId);
        ViewData["Title"] = "Cẩm nang & Tin tức";
        return View(categories);
    }

    // GET /tin-tuc?page=1  (tất cả tin tức, phân trang)
    public async Task<IActionResult> All(int page = 1, int pageSize = 27)
    {
        var paged = await _news.GetAllPagedAsync(_portalId, page, pageSize);
        ViewData["Title"]        = "Tất cả tin tức";
        ViewData["CanonicalUrl"] = $"{Request.Scheme}://{Request.Host}/tin-tuc";
        return View(paged);
    }

    // GET /tin-tuc/danh-muc/{slug}-{categoryId:int}?page=1
    public async Task<IActionResult> Category(int categoryId, string? slug, int page = 1, int pageSize = 27)
    {
        var category = await _news.GetCategoryByIdAsync(categoryId);
        if (category is null) return NotFound();

        // Slug canonical redirect
        var canonical = SlugHelper.ToSlug(category.CategoryName);
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("news-category", new { slug = canonical, categoryId, page });

        var paged      = await _news.GetByCategoryIdAsync(categoryId, _portalId, page, pageSize);
        var categories = await _news.GetCategoriesWithCountAsync(_portalId);

        ViewData["Title"]           = category.CategoryName;
        ViewData["MetaDescription"] = category.Description ?? category.CategoryName;
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}/tin-tuc/danh-muc/{canonical}-{categoryId}";
        ViewData["Category"]        = category;
        ViewData["Categories"]      = categories;

        return View(paged);
    }

    // Trang danh sách tin tức theo chuyên mục
    // Route 2-seg: /{section}/{slug}-{categoryId}  VD: /gioi-thieu/doi-ngu-227
    // Route 3-seg: /{s1}/{s2}/{slug}-{categoryId}  VD: /thong-tin-du-hoc/du-hoc-my/tin-tuc-244
    public async Task<IActionResult> CategoryPage(int categoryId, int page = 1, int pageSize = 26)
    {
        var category = await _news.GetCategoryByIdAsync(categoryId);
        if (category is null) return NotFound();

        var paged      = await _news.GetByCategoryIdAsync(categoryId, _portalId, page, pageSize);
        var categories = await _news.GetCategoriesWithCountAsync(_portalId);

        var canonicalPath = Request.Path.Value ?? string.Empty;
        ViewData["Title"]           = category.CategoryName;
        ViewData["MetaDescription"] = category.Description ?? category.CategoryName;
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}{canonicalPath}";
        ViewData["Category"]        = category;
        ViewData["Categories"]      = categories;

        return View("Category", paged);
    }

    // GET /tin-tuc/{slug}-{id:int}
    public async Task<IActionResult> Detail(int id, string? slug)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        // Slug canonical redirect
        var canonical = SlugHelper.ToSlug(vm.Title);
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("news-detail", new { slug = canonical, id });

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}/tin-tuc/{canonical}-{id}";

        return View(vm);
    }
}
