using System.Text.Json;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Service;

public class MenuService : IMenuService
{
    private readonly string _webRootPath;
    private List<MenuItemModel>? _cache;

    public MenuService(string webRootPath) => _webRootPath = webRootPath;

    public List<MenuItemModel> GetMenu()
    {
        if (_cache is not null) return _cache;

        var path = Path.Combine(_webRootPath, "data", "menu.json");
        if (!File.Exists(path)) return [];

        var json = File.ReadAllText(path);
        _cache = JsonSerializer.Deserialize<List<MenuItemModel>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];

        return _cache;
    }
}
