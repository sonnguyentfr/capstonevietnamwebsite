namespace NVCMS.WebView.Data.ViewModels;

public class FairGuideDetailVM
{
    public int    Id          { get; set; }
    public string Title       { get; set; } = string.Empty;
    public string? Avatar     { get; set; }
    public string? Descreption { get; set; }
    public string? Noidung    { get; set; }
    public string Slug        { get; set; } = string.Empty;
    public List<FairGuideMediaVM> Media { get; set; } = new();
}
