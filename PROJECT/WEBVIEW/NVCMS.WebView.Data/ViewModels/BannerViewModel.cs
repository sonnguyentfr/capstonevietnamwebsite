namespace NVCMS.WebView.Data.ViewModels;

public class BannerViewModel
{
    public int     Id         { get; set; }
    public string  Title      { get; set; } = string.Empty;
    public int     KieuBanner { get; set; }
    public string? IMGLink    { get; set; }
    public string? Link       { get; set; }
    public int     Vitri      { get; set; }
    public int     Height     { get; set; }
    public int     Width      { get; set; }
}