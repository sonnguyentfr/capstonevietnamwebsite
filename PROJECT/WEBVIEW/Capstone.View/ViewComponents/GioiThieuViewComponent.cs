using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;

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

    public async Task<IViewComponentResult> InvokeAsync(int id, string menuGroupUrl = "/gioi-thieu")
    {
        var data = await _service.GetByIdAsync(id, _portalId);
        if (data is not null)
        {
            var allMenu = _menuService.GetMenu();
            data.SidebarMenu = FindMenuChildren(allMenu, menuGroupUrl);
        }
        return View(data);
    }

    /// <summary>
    /// Tìm children của node có Url == groupUrl, tìm đệ quy qua toàn bộ cây menu.
    /// </summary>
    private static List<MenuItemModel> FindMenuChildren(
        IEnumerable<MenuItemModel> nodes, string groupUrl)
    {
        foreach (var node in nodes)
        {
            if (string.Equals(node.Url, groupUrl, StringComparison.OrdinalIgnoreCase))
                return node.Children;

            if (node.Children.Count > 0)
            {
                var found = FindMenuChildren(node.Children, groupUrl);
                if (found.Count > 0) return found;
            }
        }
        return [];
    }
}
