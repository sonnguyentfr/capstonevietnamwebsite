using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Hiển thị danh sách tin nổi bật được chọn từ bảng News_Settings.
/// Cách dùng trong Razor view:
///   @await Component.InvokeAsync("NewsFeatured", new { count = 8, bigCount = 3 })
///   - count:    tổng số tin lấy về (mặc định 8)
///   - bigCount: số tin hiển thị lớn ở trên (mặc định 3), phần còn lại hiển thị nhỏ
/// </summary>
public class NewsFeaturedViewComponent : ViewComponent
{
    private readonly INewsService _news;
    private readonly IConfiguration _config;

    public NewsFeaturedViewComponent(INewsService news, IConfiguration config)
    {
        _news   = news;
        _config = config;
    }

    public async Task<IViewComponentResult> InvokeAsync(int count = 8, int bigCount = 3)
    {
        var portalId = _config.GetValue<int>("SiteSettings:PortalId");
        var items    = (await _news.GetFeaturedAsync(portalId, count)).ToList();

        if (items.Count == 0)
            return Content(string.Empty);

        ViewData["BigCount"] = Math.Clamp(bigCount, 1, items.Count);
        return View(items);
    }
}
