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
    public string?  Tags             { get; set; }

    public List<NewsItemViewModel>    RelatedNews    { get; set; } = [];
    public List<TruongCardViewModel>  RelatedSchools { get; set; } = [];

    /// <summary>Sự kiện sắp diễn ra – hiển thị website, sắp xếp theo thời gian bắt đầu gần nhất.</summary>
    public List<EventsCatViewModel>   UpcomingEvents { get; set; } = [];
}