using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Capstone.View.Options;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Hiển thị danh sách tin nổi bật được chọn từ bảng News_Settings.
/// Cách dùng: @await Component.InvokeAsync("NewsFeatured", new { count = 8, bigCount = 3 })
/// </summary>
public class NewsFeaturedViewComponent : ViewComponent
{
    private readonly INewsService _news;
    private readonly int _portalId;

    public NewsFeaturedViewComponent(INewsService news, IOptions<SiteSettings> settings)
    {
        _news     = news;
        _portalId = settings.Value.PortalId;
    }

    public async Task<IViewComponentResult> InvokeAsync(int count = 8, int bigCount = 3)
    {
        var items = (await _news.GetFeaturedAsync(_portalId, count)).ToList();
        if (items.Count == 0) return Content(string.Empty);
        ViewData["BigCount"] = Math.Clamp(bigCount, 1, items.Count);
        return View(items);
    }
}

