namespace NVCMS.WebView.Data.ViewModels;

public class NewsDetailViewModel
{
    public int      NewId            { get; set; }
    public string   Title            { get; set; } = string.Empty;
    public string?  ImagePath        { get; set; }
    public string?  Content          { get; set; }
    public string?  Summary          { get; set; }
    public string?  Tacgia           { get; set; }
    public string?  SourceText       { get; set; }
    public string?  MetaTitle        { get; set; }
    public string?  MetaDescription  { get; set; }
    public string?  MetaImage        { get; set; }
    public int      ViewCount        { get; set; }
    public DateTime PublishedDate    { get; set; }
    public int      CategoryId       { get; set; }
    public string   CategoryName     { get; set; } = string.Empty;
    public string   CategorySlug     { get; set; } = string.Empty;
    public string   Slug             { get; set; } = string.Empty;

    public List<NewsItemViewModel> RelatedNews { get; set; } = [];
}