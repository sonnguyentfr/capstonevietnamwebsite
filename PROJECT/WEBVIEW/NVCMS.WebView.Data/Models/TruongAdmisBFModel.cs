namespace NVCMS.WebView.Data.Models;

public class TruongAdmisBFModel
{
    public int Id { get; set; }
    public int? currency { get; set; }
    public int? Gradesfrom { get; set; }
    public int? Gradesto { get; set; }
    public bool? ESL { get; set; }
    public bool? APCourse { get; set; }
    public bool? IBCourse { get; set; }
    public bool? HonorsCourse { get; set; }
    public bool? TESTToefl { get; set; }
    public int? TESTToeflMin { get; set; }
    public bool? TESTIELTS { get; set; }
    public decimal? TESTIELTSMin { get; set; }
    public int? COSTuti { get; set; }
    public int? COSBook { get; set; }
    public int? COSHealth { get; set; }
    public int? COSRoom { get; set; }
    public bool? ScholarshipInternation { get; set; }
    public string? ScholarshipInternationRang { get; set; }
    public string? ScholarshipInternationRangVN { get; set; }
    public bool? HousingBF { get; set; }
    public bool? HousingHome { get; set; }
    public bool? SummerProgram { get; set; }
    public string? SummerProgramAges { get; set; }
    public string? SummerProgramCOST { get; set; }
    public DateTime? AdmFall { get; set; }
    public DateTime? AdmWinter { get; set; }
    public DateTime? AdmSpring { get; set; }
    public DateTime? AdmSummer { get; set; }
    public bool? AdmRoll { get; set; }
    public int? PortalId { get; set; }
}
