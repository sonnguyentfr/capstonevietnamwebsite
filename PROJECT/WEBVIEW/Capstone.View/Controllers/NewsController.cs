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

    // GET /cam-nang-su-kien-du-hoc
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

    // GET /{section}/{slug}  — canonical path dựa theo URL hiện tại, không redirect
    public async Task<IActionResult> CategoryBySlug(string slug, int page = 1, int pageSize = 27)
    {
        var category = await _news.GetCategoryBySlugAsync(slug, _portalId);
        if (category == null)
            return NotFound();

        // ================= CANONICAL SLUG CHECK =================
        if (!string.Equals(slug, category.Slug, StringComparison.OrdinalIgnoreCase))
        {
            return RedirectToActionPermanent(nameof(CategoryBySlug), new
            {
                slug = category.Slug,
                page = page > 1 ? page : 1
            });
        }

        var paged = await _news.GetByCategoryIdAsync(category.CategoryID, _portalId, page, pageSize);

        // ================= SEO META =================
        ViewData["Title"] = category.CategoryName;
        ViewData["MetaDescription"] = category.Description ?? category.CategoryName;

        // ================= CANONICAL URL FIX =================
        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.Path}";

        ViewData["CanonicalUrl"] = page > 1
            ? $"{baseUrl}?page={page}"
            : baseUrl;

        ViewData["Category"] = category;
        ViewData["Categories"] = await _news.GetCategoriesWithCountAsync(_portalId);

        return View("Category", paged);
    }

    // GET /tin-tuc/danh-muc/{slug}-{categoryId:int}  ← legacy URL → 301 redirect sang slug-only
    public async Task<IActionResult> Category(int categoryId, string? slug, int page = 1)
    {
        var category = await _news.GetCategoryByIdAsync(categoryId);
        if (category is null) return NotFound();

        return RedirectToRoutePermanent("news-category-slug",
            new { slug = category.Slug, page = page > 1 ? page : (object?)null });
    }

    // Trang danh sách tin tức theo chuyên mục (multi-segment legacy routes)
    // Route 2-seg: /{section}/{slug}-{categoryId}  VD: /gioi-thieu/doi-ngu-227
    // Route 3-seg: /{s1}/{s2}/{slug}-{categoryId}  VD: /thong-tin-du-hoc/du-hoc-my/tin-tuc-244
    public async Task<IActionResult> CategoryPage(int categoryId, int page = 1, int pageSize = 26)
    {
        var category = await _news.GetCategoryByIdAsync(categoryId);
        if (category is null) return NotFound();

        // Redirect sang URL chính tắc slug-only
        return RedirectToRoutePermanent("news-category-slug",
            new { slug = category.Slug, page = page > 1 ? page : (object?)null });
    }

    // GET /tin-tuc/{slug}-{id:int}
    public async Task<IActionResult> Detail(int id, string? slug)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        // Bài thuộc category "doi-ngu" → redirect sang /doi-ngu/{slug}-{id}
        if (string.Equals(vm.CategorySlug, "doi-ngu", StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("doi-ngu-detail", new { slug = vm.Slug, id });

        // Slug canonical redirect
        var canonical = vm.Slug;
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("news-detail", new { slug = canonical, id });

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = NewsUrlBuilder.BuildFullNewsUrl(Request.Scheme, Request.Host.Value, vm);

        return View(vm);
    }

    // GET /{section}/{catSlug}/{slug}-{id}
    public async Task<IActionResult> SectionDetail(int id, string? slug, string? catSlug, string? section)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        var canonical        = vm.Slug;
        var canonicalCatSlug = vm.CategorySlug;
        var sec              = section ?? "cam-nang-su-kien-du-hoc";

        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(catSlug, canonicalCatSlug, StringComparison.OrdinalIgnoreCase))
        {
            // Tìm tên route tương ứng với section
            var routeName = sec + "-detail";
            return RedirectToRoutePermanent(routeName,
                new { catSlug = canonicalCatSlug, slug = canonical, id });
        }

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = NewsUrlBuilder.BuildFullNewsUrl(Request.Scheme, Request.Host.Value, vm);

        return View("Detail", vm);
    }

    // GET /doi-ngu/{slug}-{id}
    public async Task<IActionResult> DoiNguDetail(int id, string? slug)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        var canonical = vm.Slug;
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("doi-ngu-detail", new { slug = canonical, id });

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = NewsUrlBuilder.BuildFullNewsUrl(Request.Scheme, Request.Host.Value, vm);

        return View("Detail", vm);
    }
    
    // GET /cam-nang-su-kien-du-hoc/{catSlug}/{slug}-{id}
    public async Task<IActionResult> CamNangSuKienDetail(int id, string? slug, string? catSlug)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        var canonical = vm.Slug;
        var canonicalCatSlug = vm.CategorySlug;

        // Redirect nếu slug hoặc catSlug sai
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(catSlug, canonicalCatSlug, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("cam-nang-su-kien-du-hoc-detail",
                new { catSlug = canonicalCatSlug, slug = canonical, id });

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = NewsUrlBuilder.BuildFullNewsUrl(Request.Scheme, Request.Host.Value, vm);

        return View("Detail", vm);
    }

    // ── Section /cam-nang-va-tin-tuc ─────────────────────────────────────────

    // GET /cam-nang-va-tin-tuc
    public async Task<IActionResult> CamNangSection()
    {
        var categories = await _news.GetCategoriesWithCountAsync(_portalId);
        ViewData["Title"]    = "Cẩm nang & Tin tức";
        ViewData["Section"]  = "cam-nang-va-tin-tuc";
        return View("Index", categories);
    }

    // GET /cam-nang-va-tin-tuc/{slug}-{categoryId}
    public async Task<IActionResult> CamNangCategory(int categoryId, string? slug, int page = 1, int pageSize = 27)
    {
        var category = await _news.GetCategoryByIdAsync(categoryId);
        if (category is null) return NotFound();

        // Canonical slug redirect
        if (!string.Equals(slug, category.Slug, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("cam-nang-va-tin-tuc-cat",
                new { slug = category.Slug, categoryId, page = page > 1 ? page : (object?)null });

        var paged      = await _news.GetByCategoryIdAsync(categoryId, _portalId, page, pageSize);
        var categories = await _news.GetCategoriesWithCountAsync(_portalId);

        ViewData["Title"]           = category.CategoryName;
        ViewData["MetaDescription"] = category.Description ?? category.CategoryName;
        ViewData["CanonicalUrl"]    = $"{Request.Scheme}://{Request.Host}/cam-nang-va-tin-tuc/{category.Slug}-{categoryId}";
        ViewData["Category"]        = category;
        ViewData["Categories"]      = categories;
        ViewData["Section"]         = "cam-nang-va-tin-tuc";
        ViewData["DetailRoute"]     = "cam-nang-va-tin-tuc-detail";
        ViewData["DetailCatSlug"]   = category.Slug;

        return View("Category", paged);
    }

    // GET /cam-nang-va-tin-tuc/{catSlug}/{slug}-{id}
    public async Task<IActionResult> CamNangDetail(int id, string? slug, string? catSlug)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        // Slug canonical redirect
        var canonical = vm.Slug;
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("cam-nang-va-tin-tuc-detail",
                new { catSlug = catSlug ?? vm.CategorySlug, slug = canonical, id });

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = NewsUrlBuilder.BuildFullNewsUrl(Request.Scheme, Request.Host.Value, vm);

        return View("Detail", vm);
    }

    // ── Section /tuyen-dung ───────────────────────────────────────────────────

    // GET /tuyen-dung/{slug}-{id}
    public async Task<IActionResult> TuyenDungDetail(int id, string? slug)
    {
        var vm = await _news.GetDetailAsync(id, _portalId);
        if (vm is null) return NotFound();

        var canonical = vm.Slug;
        if (!string.Equals(slug, canonical, StringComparison.OrdinalIgnoreCase))
            return RedirectToRoutePermanent("tuyen-dung-detail", new { slug = canonical, id });

        ViewData["Title"]           = vm.MetaTitle ?? vm.Title;
        ViewData["MetaDescription"] = vm.MetaDescription ?? vm.Summary;
        ViewData["MetaImage"]       = vm.MetaImage ?? vm.ImagePath;
        ViewData["CanonicalUrl"]    = NewsUrlBuilder.BuildFullNewsUrl(Request.Scheme, Request.Host.Value, vm);

        return View("Detail", vm);
    }

    // GET /{*slug} — catch-all route
    public async Task<IActionResult> DetailCatchAll(string slug)
    {
        var newsId = _urlService.ExtractNewsId(slug);
        if (newsId is null) return NotFound();

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
}
