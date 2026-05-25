namespace NVCMS.WebView.Data.Models;

/// <summary>Map tu table NV_Events (CRMConnection)</summary>
public class EventsModel
{
    public int       Id             { get; set; }
    public string?   Title          { get; set; }
    public string?   TitleEN        { get; set; }
    public string?   CODE           { get; set; }
    public string?   Source         { get; set; }
    public int?      Vanphong       { get; set; }
    public int       CatId          { get; set; }
    public string?   Avatar         { get; set; }
    public string?   Diadiem        { get; set; }
    public string?   DiadiemEN      { get; set; }
    public DateTime? Fromdatetime   { get; set; }
    public DateTime? Enddatetime    { get; set; }
    public string?   Thanhphan      { get; set; }
    public string?   ThanhphanEN    { get; set; }
    public string?   School         { get; set; }
    public string?   Org            { get; set; }
    public int?      Gia            { get; set; }
    public string?   Descreption    { get; set; }
    public string?   DescreptionEN  { get; set; }
    public string?   LienheName     { get; set; }
    public string?   LienheEmail    { get; set; }
    public string?   LienheMobile   { get; set; }
    public string?   LienheAdd      { get; set; }
    public int?      UserId         { get; set; }
    public int      Portalid       { get; set; }
    public DateTime? Createddate    { get; set; }
    public bool?     Isactive       { get; set; }
    public string?   Anhbando       { get; set; }
    public string?   Linkbando      { get; set; }
    public int?      Ordernumber    { get; set; }
}
