namespace NVCMS.WebView.Data.ViewModels;

public class CheckStudentResult
{
    public bool    Found       { get; set; }
    public int     StudentId   { get; set; }
    public string  StudentCode { get; set; } = string.Empty;
    public string  Hotendem    { get; set; } = string.Empty;
    public string  Ten         { get; set; } = string.Empty;
    public string  FullName    { get; set; } = string.Empty;
    public string  Phone       { get; set; } = string.Empty;
    public string  Email       { get; set; } = string.Empty;
    public string  DiaChi      { get; set; } = string.Empty;
}
