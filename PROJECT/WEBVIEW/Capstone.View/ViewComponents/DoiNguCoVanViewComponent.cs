using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Hien thi danh sach co van theo CategoryId duoi dang luoi home-courses-item.
/// Cach dung trong Razor view:
///   @await Component.InvokeAsync("DoiNguCoVan", new { categoryId = 198, count = 15 })
/// </summary>
public class DoiNguCoVanViewComponent : ViewComponent
{
    private readonly INewsService _news;
    private readonly IConfiguration _config;

    public DoiNguCoVanViewComponent(INewsService news, IConfiguration config)
    {
        _news   = news;
        _config = config;
    }

    public async Task<IViewComponentResult> InvokeAsync(int categoryId = 198, int count = 15)
    {
        var portalId = _config.GetValue<int>("SiteSettings:PortalId");
        var paged    = await _news.GetByCategoryIdAsync(categoryId, portalId, page: 1, pageSize: count);
        var category = await _news.GetCategoryByIdAsync(categoryId);

        ViewData["CategoryId"]   = categoryId;
        ViewData["CategoryName"] = category?.CategoryName ?? string.Empty;
        ViewData["CategorySlug"] = category?.Slug ?? string.Empty;

        return View(paged.Items);
    }
}
