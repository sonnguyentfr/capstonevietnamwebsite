namespace NVCMS.WebView.Data.ViewModels;

public class CategoryViewModel
{
    public int      CategoryID   { get; set; }
    public int      ParentId     { get; set; }
    public string   CategoryName { get; set; } = string.Empty;
    public string   Slug         { get; set; } = string.Empty;
    public string?  Description  { get; set; }
    public int      NewsCount    { get; set; }
    public List<CategoryViewModel> Children { get; set; } = [];
}