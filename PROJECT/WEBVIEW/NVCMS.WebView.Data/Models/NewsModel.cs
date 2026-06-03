namespace NVCMS.WebView.Data.Models;

/// <summary>Map từ table NV_News (DNN module NVCMS.TinTuc)</summary>
public class NewsModel
{
    public int      NewId            { get; set; }
    public int      CategoryId       { get; set; }
    public string   Title            { get; set; } = string.Empty;
    public string?  ImagePath        { get; set; }
    public string?  Summary          { get; set; }
    public string?  Content          { get; set; }
    public string?  Keyword          { get; set; }
    public string?  Tags             { get; set; }
    public string?  SourceText       { get; set; }
    public string?  Tacgia           { get; set; }
    public string?  MetaTitle        { get; set; }
    public string?  MetaDescription  { get; set; }
    public string?  MetaImage        { get; set; }
    public bool     IsActive         { get; set; }
    public bool     Hotcat           { get; set; }
    public bool     Hotsite          { get; set; }
    public int      ViewCount        { get; set; }
    public int      PortalId         { get; set; }
    public int      UserId           { get; set; }
    public DateTime PublishedDate    { get; set; }
    public DateTime CreateDate       { get; set; }
    public string?  MetaUrl          { get; set; }
}