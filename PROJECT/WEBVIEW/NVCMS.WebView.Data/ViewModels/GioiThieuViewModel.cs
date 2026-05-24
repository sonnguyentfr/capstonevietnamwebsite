using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.ViewModels;

public class GioiThieuViewModel
{
    public int    Id           { get; set; }
    public string TrangDanhMuc { get; set; } = string.Empty;
    public string Tieudephu    { get; set; } = string.Empty;
    public string ImagePath    { get; set; } = string.Empty;
    public string Tomtat       { get; set; } = string.Empty;
    public string Noidung      { get; set; } = string.Empty;
    public string Link         { get; set; } = string.Empty;
    public int    ParentId     { get; set; }
    public int    Ordernumber  { get; set; }
    public int    PortalId     { get; set; }

    public List<MenuItemModel> SidebarMenu { get; set; } = [];
}
