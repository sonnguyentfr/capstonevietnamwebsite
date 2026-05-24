namespace NVCMS.WebView.Data.Models;

/// <summary>Map từ table BannerAdv (DNN module NVCMS.Banner)</summary>
public class BannerModel
{
    public int      Id          { get; set; }
    public string   Title       { get; set; } = string.Empty;
    public int      KieuBanner  { get; set; }   // 1=Ảnh, 3=Code
    public string?  IMGLink     { get; set; }
    public string?  Link        { get; set; }
    public int      Vitri       { get; set; }
    public int      Height      { get; set; }
    public int      Width       { get; set; }
    public bool     Visible     { get; set; }
    public int      Ordernumber { get; set; }
    public DateTime Startdate   { get; set; }
    public DateTime Enddate     { get; set; }
    public int      PortalId    { get; set; }
    public int      Click       { get; set; }
}