namespace NVCMS.WebView.Data.ViewModels;

public class EventsViewModel
{
    public int       Id            { get; set; }
    public string    Title         { get; set; } = string.Empty;
    public string    TitleEN       { get; set; } = string.Empty;
    public string?   AvatarUrl     { get; set; }
    public string?   Diadiem       { get; set; }
    public string?   DiadiemEN     { get; set; }
    public DateTime? Fromdatetime  { get; set; }
    public DateTime? Enddatetime   { get; set; }
    public string?   Thanhphan     { get; set; }
    public string?   ThanhphanEN   { get; set; }
    public string?   School        { get; set; }
    public string?   Org           { get; set; }
    public int?      Gia           { get; set; }
    public string?   Descreption   { get; set; }
    public string?   DescreptionEN { get; set; }
    public string?   LienheName    { get; set; }
    public string?   LienheEmail   { get; set; }
    public string?   LienheMobile  { get; set; }
    public int       CatId         { get; set; }
    public int?      Ordernumber   { get; set; }
}
