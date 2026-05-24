namespace NVCMS.WebView.Data.Models;

public class MenuItemModel
{
    public string Title    { get; set; } = string.Empty;
    public string Url      { get; set; } = string.Empty;
    public List<MenuItemModel> Children { get; set; } = [];
}
