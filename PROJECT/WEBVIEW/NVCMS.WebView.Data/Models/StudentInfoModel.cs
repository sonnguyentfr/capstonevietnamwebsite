namespace NVCMS.WebView.Data.Models;

/// <summary>Projection for Student_Info lookup used in event registration.</summary>
public class StudentInfoModel
{
    public int     Id           { get; set; }
    public string? Code         { get; set; }
    public string? Hotendem     { get; set; }
    public string? Ten          { get; set; }
    public string? Sodienthoai  { get; set; }
    public string? Email        { get; set; }
    public string?   Diachi      { get; set; }
    public DateTime?  Ngaysinh    { get; set; }
    public int?       Tinh        { get; set; }   // NVCMS_DM_DVHanhChinh_Tinh.id
    public int        PortalId    { get; set; }

    public string FullName => $"{Hotendem} {Ten}".Trim();
}
