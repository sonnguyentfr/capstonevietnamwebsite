namespace NVCMS.WebView.Data.Models;

public class FairGuideModel
{
    public int     Id          { get; set; }
    public string  Title       { get; set; } = string.Empty;
    public string? Avatar      { get; set; }
    public string? Descreption { get; set; }
    public string? Noidung     { get; set; }
    public int?    Ordernumber { get; set; }
    public int     PortalId    { get; set; }
}
