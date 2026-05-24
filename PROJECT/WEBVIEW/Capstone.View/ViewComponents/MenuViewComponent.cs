using Microsoft.AspNetCore.Mvc;
using NVCMS.WebView.Data.Contracts.Service;

namespace Capstone.View.ViewComponents;

public class MenuViewComponent : ViewComponent
{
    private readonly IMenuService _menuService;

    public MenuViewComponent(IMenuService menuService)
    {
        _menuService = menuService;
    }

    public IViewComponentResult Invoke()
    {
        var menu = _menuService.GetMenu();
        return View(menu);
    }
}
