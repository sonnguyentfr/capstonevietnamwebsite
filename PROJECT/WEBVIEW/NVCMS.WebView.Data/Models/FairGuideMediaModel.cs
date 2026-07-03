namespace NVCMS.WebView.Data.Models;

public class FairGuideMediaModel
{
    public int    Id        { get; set; }
    public string Title     { get; set; } = string.Empty;
    public string FileName  { get; set; } = string.Empty;
    public string MediaUrl  { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public int    OrderNumber { get; set; }
}
