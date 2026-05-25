using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.ViewModels;

namespace Capstone.View.ViewComponents;

/// <summary>
/// Sidebar b? l?c tr??ng — dùng chung cho m?i trang tìm tr??ng.
///
/// Cách dùng (trong Razor view):
///   @await Component.InvokeAsync("TruongFilterSidebar", new TruongFilterSidebarViewModel { ... })
/// </summary>
public class TruongFilterSidebarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(TruongFilterSidebarViewModel vm) => View(vm);
}
