using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Hi?n th? slider tr??ng theo nhóm b?c h?c trên trang ch?.
/// S? d?ng:
///   @await Component.InvokeAsync("TruongSwiper", new { loaiList = new[]{"4Y","2Y"}, viewTitle = "??i h?c & Cao ??ng", pageSize = 12 })
/// </summary>
public class TruongSwiperViewComponent : ViewComponent
{
    private readonly ITruongService _truongService;

    public TruongSwiperViewComponent(ITruongService truongService)
    {
        _truongService = truongService;
    }

    public async Task<IViewComponentResult> InvokeAsync(
        IEnumerable<string> loaiList,
        string viewTitle = "",
        int pageSize = 12)
    {
        var items = await _truongService.GetHomeSwiperAsync(loaiList, pageSize);
        ViewData["ViewTitle"] = viewTitle;
        return View(items);
    }
}
