namespace NVCMS.WebView.Data.Models;

/// <summary>Map từ table NVCMS_LadingPage</summary>
public class NVCMS_LadingPageModel
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public string TrangDanhMuc { get; set; } = string.Empty;
    public string Tieudephu { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string diadiem { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
    public string NoidungFile { get; set; } = string.Empty;
    public string tomtat { get; set; } = string.Empty;
    public bool isActive { get; set; }
    public string Noidung { get; set; } = string.Empty;
    public int PortalId { get; set; }
}
