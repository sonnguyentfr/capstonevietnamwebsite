namespace NVCMS.WebView.Data.ViewModels;

public class VideoItemVM
{
    public int     VideoId    { get; set; }
    public string  Title      { get; set; } = string.Empty;
    public string? ImagePath  { get; set; }
    public string? Summary    { get; set; }
    public int     TypeVideo  { get; set; }
    public string? VideoPath  { get; set; }
}

public class VideoDetailVM
{
    public int     VideoId    { get; set; }
    public string  Title      { get; set; } = string.Empty;
    public string? ImagePath  { get; set; }
    public string? Summary    { get; set; }
    public string? Content    { get; set; }
    public int     TypeVideo  { get; set; }
    public string? VideoPath  { get; set; }
}
