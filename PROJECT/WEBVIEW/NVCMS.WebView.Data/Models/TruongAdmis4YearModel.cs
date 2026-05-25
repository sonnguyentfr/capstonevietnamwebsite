namespace NVCMS.WebView.Data.Models;

public class TruongAdmis4YearModel
{
    public int Id { get; set; }
    public int? currency { get; set; }
    public int? COSTuitionfeeESL { get; set; }
    public int? COSTuitionfeeUnder { get; set; }
    public int? COSTuitionfeeGrad { get; set; }
    public int? COSTuitionfeeAss { get; set; }
    public bool? ScholarshipESL { get; set; }
    public string? ScholarshipESLRange { get; set; }
    public string? ScholarshipESLRangeVN { get; set; }
    public bool? ScholarshipUnder { get; set; }
    public string? ScholarshipUnderRange { get; set; }
    public string? ScholarshipUnderRangeVN { get; set; }
    public bool? ScholarshipGrad { get; set; }
    public string? ScholarshipGradRange { get; set; }
    public string? ScholarshipGradRangeVN { get; set; }
    public bool? ScholarshipAss { get; set; }
    public string? ScholarshipAssRange { get; set; }
    public string? ScholarshipAssRangeVN { get; set; }
    public string? ScholarshipNote { get; set; }
    public string? ScholarshipNoteVN { get; set; }
    public DateTime? FallUnder { get; set; }
    public DateTime? FallGrad { get; set; }
    public DateTime? FallAss { get; set; }
    public DateTime? FallESL { get; set; }
    public DateTime? WinterUnder { get; set; }
    public DateTime? SpringUnder { get; set; }
    public DateTime? SummerUnder { get; set; }
    public bool? RollingUnder { get; set; }
    public bool? RollingGrad { get; set; }
    public bool? RollingAss { get; set; }
    public bool? RollingESL { get; set; }
    public string? ToefliBTUnder { get; set; }
    public string? IELTSUnder { get; set; }
    public string? GraduationRate { get; set; }
    public string? EmploymentRateAfterGraduation { get; set; }
    public string? MostMajor { get; set; }
    public bool? OnCampus { get; set; }
    public int? NOSTotalUnder { get; set; }
    public int? NOSInternationalUnder { get; set; }
    public int? NOSVNUnder { get; set; }
    public int? PortalId { get; set; }
}
