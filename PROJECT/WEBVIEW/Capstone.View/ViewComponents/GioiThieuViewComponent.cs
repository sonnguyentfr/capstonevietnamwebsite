using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

/// <summary>
/// ViewComponent lấy nội dung trang giới thiệu theo id.
/// Sử dụng: @await Component.InvokeAsync("GioiThieu", new { id = 5 })
/// </summary>
public class GioiThieuViewComponent : ViewComponent
{
    private readonly IGioiThieuService _service;
    private readonly IMenuService      _menuService;
    private readonly int               _portalId;

    public GioiThieuViewComponent(
        IGioiThieuService service,
        IMenuService      menuService,
        IConfiguration    config)
    {
        _service     = service;
        _menuService = menuService;
        _portalId    = config.GetValue<int>("SiteSettings:PortalId");
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        var data = await _service.GetByIdAsync(id, _portalId);
        if (data is not null)
        {
            var allMenu      = _menuService.GetMenu();
            var capstoneGroup = allMenu.FirstOrDefault(m => m.Url == "/gioi-thieu");
            data.SidebarMenu = capstoneGroup?.Children ?? [];
        }
        return View(data);
    }
}
