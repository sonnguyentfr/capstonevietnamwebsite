namespace NVCMS.WebView.Data.Models;

public class TruongAdmisESLModel
{
    public int Id { get; set; }
    public int? currency { get; set; }
    public string? TypeOfCourse { get; set; }
    public string? LCName { get; set; }
    public string? LCLenght { get; set; }
    public string? LCCost { get; set; }
    public string? Conditional { get; set; }
    public int? RateOfStudent { get; set; }
    public int? NOSTotal { get; set; }
    public int? NOSInternation { get; set; }
    public int? NOSVietnames { get; set; }
    public bool? WorkOpp { get; set; }
    public int? COSTuti { get; set; }
    public int? COSBook { get; set; }
    public int? COSHealth { get; set; }
    public int? COSRoom { get; set; }
    public int? COSTrans { get; set; }
    public bool? Scholarship { get; set; }
    public string? ScholarshipRange { get; set; }
    public bool? Financial { get; set; }
    public string? FinancialRange { get; set; }
    public bool? HousingOncampus { get; set; }
    public bool? HousingHomeStay { get; set; }
    public bool? HousingApartment { get; set; }
    public DateTime? AdmFall { get; set; }
    public DateTime? AdmWinter { get; set; }
    public DateTime? AdmSpring { get; set; }
    public DateTime? AdmSummer { get; set; }
    public bool? AdmRoll { get; set; }
    public int? PortalId { get; set; }
}
