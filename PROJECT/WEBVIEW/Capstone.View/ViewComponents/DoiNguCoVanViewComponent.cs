using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Hien thi danh sach co van theo CategoryId duoi dang luoi home-courses-item.
/// Cach dung: @await Component.InvokeAsync("DoiNguCoVan", new { categoryId = 198, count = 15 })
/// </summary>
public class DoiNguCoVanViewComponent : ViewComponent
{
    private readonly INewsService _news;
    private readonly int _portalId;

    public DoiNguCoVanViewComponent(INewsService news, IOptions<SiteSettings> settings)
    {
        _news     = news;
        _portalId = settings.Value.PortalId;
    }

    public async Task<IViewComponentResult> InvokeAsync(int categoryId = 198, int count = 15)
    {
        // GetByCategoryIdAsync already caches internally.
        // GetCategoryByIdAsync is also cached — two cached lookups instead of two raw DB calls.
        var paged    = await _news.GetByCategoryIdAsync(categoryId, _portalId, page: 1, pageSize: count);
        var category = await _news.GetCategoryByIdAsync(categoryId);

        ViewData["CategoryId"]   = categoryId;
        ViewData["CategoryName"] = category?.CategoryName ?? string.Empty;
        ViewData["CategorySlug"] = category?.Slug ?? string.Empty;

        return View(paged.Items);
    }
}
