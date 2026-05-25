namespace NVCMS.WebView.Data.Models;

/// <summary>Map tu table NV_Events_Cat (CRMConnection)</summary>
public class EventsCatModel
{
    public int       Id              { get; set; }
    public string?   CatName         { get; set; }
    public string?   CatNameEN       { get; set; }
    public int?      Marketing       { get; set; }
    public bool?     Chonnhieu       { get; set; }
    public string?   Code            { get; set; }
    public string?   Source          { get; set; }
    public string?   Email           { get; set; }
    public string?   DateShow        { get; set; }
    public DateTime? FromDate        { get; set; }
    public DateTime? EndDate         { get; set; }
    public string?   Avatar          { get; set; }
    public string?   Desception      { get; set; }
    public string?   DesceptionEN    { get; set; }
    public string?   Contentx        { get; set; }
    public string?   ContentxEN      { get; set; }
    public string?   ContentMail     { get; set; }
    public DateTime? CreatedDate     { get; set; }
    public int?      UserId          { get; set; }
    public int      PortalId        { get; set; }
    public bool?     Isactive        { get; set; }
    public int?      Ordernumber     { get; set; }
    public string?   FairSchool      { get; set; }
    public string?   FairDiengia     { get; set; }
    public string?   FairTestimonial { get; set; }
    public string?   FairDonviTaiTro { get; set; }
    public string?   FairOrg         { get; set; }
    public int?      TabID           { get; set; }
    public bool?     Sendmail        { get; set; }
    public bool?     Sendcode        { get; set; }
    public string?   TitleMail       { get; set; }
    public string?   Link_pr         { get; set; }
    public string?   Link_data_google_sheet       { get; set; }
    public string?   Link_data_google_sheet_range { get; set; }
    public bool      Is_show_website { get; set; }
}
