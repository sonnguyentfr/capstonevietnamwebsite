namespace NVCMS.WebView.Data.Models;

public class ShortyUrlModel
{
    public int      Id          { get; set; }
    public string   ShortUrl    { get; set; } = string.Empty;
    public DateTime CreateDate  { get; set; }
    public string   CreatedBy   { get; set; } = string.Empty;
    public string   RealUrl     { get; set; } = string.Empty;
    public string   CreatedUser { get; set; } = string.Empty;
    public int      ShortClicks { get; set; }
}
