namespace NVCMS.WebView.Data.ViewModels;

public class NewsItemViewModel
{
    public int      NewId              { get; set; }
    public int      CategoryId         { get; set; }
    public string   Title              { get; set; } = string.Empty;
    public string?  ImagePath          { get; set; }
    public string?  Summary            { get; set; }
    public string?  Tacgia             { get; set; }
    public string?  Tags               { get; set; }
    public DateTime PublishedDate      { get; set; }
    public string   Slug               { get; set; } = string.Empty;
    public string   CategorySlug       { get; set; } = string.Empty;
    public string   CategoryName       { get; set; } = string.Empty;
    /// <summary>FullSlug của category cha (dùng để build section URL chính xác)</summary>
    public string?  CategoryParentSlug { get; set; }
}