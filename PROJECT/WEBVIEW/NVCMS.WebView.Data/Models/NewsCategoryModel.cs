namespace NVCMS.WebView.Data.Models;

/// <summary>Map từ table NV_NewsCategories</summary>
public class NewsCategoryModel
{
    public int      CategoryID   { get; set; }
    public int      ParentId     { get; set; }
    public string   CategoryName { get; set; } = string.Empty;
    public string?  Description  { get; set; }
    public bool     IsActive     { get; set; }
    public int      OrderNumber  { get; set; }
    public int      PortalId     { get; set; }
    public int      TabID        { get; set; }
    public int      TabIdDetail  { get; set; }
    public string?  Slug         { get; set; }
}