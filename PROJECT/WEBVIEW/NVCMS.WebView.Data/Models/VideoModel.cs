namespace NVCMS.WebView.Data.Models;

public class VideoModel
{
    public int      VideoId       { get; set; }
    public int      CategoryId    { get; set; }
    public string   Title         { get; set; } = string.Empty;
    public string?  ImagePath     { get; set; }
    public string?  VideoPath     { get; set; }
    public string?  Summary       { get; set; }
    public string?  Content       { get; set; }
    public int      TypeVideo     { get; set; }
    public bool     IsActive      { get; set; }
    public int      Status        { get; set; }
    public DateTime? Createdate   { get; set; }
    public int      ViewCount     { get; set; }
    public int      PortalId      { get; set; }
}
