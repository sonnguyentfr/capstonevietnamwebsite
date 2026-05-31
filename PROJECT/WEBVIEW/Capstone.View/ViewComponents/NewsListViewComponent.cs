using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Hiển thị danh sách tin tức theo CategoryId.
/// Cách dùng trong Razor view:
///   @await Component.InvokeAsync("NewsList", new { categoryId = 5, count = 6 })
/// </summary>
public class NewsListViewComponent : ViewComponent
{
    private readonly INewsService _news;
    private readonly IConfiguration _config;

    public NewsListViewComponent(INewsService news, IConfiguration config)
    {
        _news   = news;
        _config = config;
    }

    public async Task<IViewComponentResult> InvokeAsync(int categoryId, int count = 6)
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
