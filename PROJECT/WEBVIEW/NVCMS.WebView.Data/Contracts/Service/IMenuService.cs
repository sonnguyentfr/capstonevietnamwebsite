using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface IMenuService
{
    List<MenuItemModel> GetMenu();
}
