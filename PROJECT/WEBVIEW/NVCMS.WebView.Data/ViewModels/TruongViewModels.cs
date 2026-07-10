namespace NVCMS.WebView.Data.ViewModels;

public class TruongCardViewModel
{
    public int Id { get; set; }
    public string? NameofSchool { get; set; }
    public string? Tomtat { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string? Loai { get; set; }
    public string? CountryName { get; set; }
    public int? CountryId { get; set; }
    public bool IsPartner { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string DetailUrl => $"/truong-doi-tac/{Slug}-{Id}";

    /// <summary>Tên bang / thành phố lớn nhất (ThanhPholongannhat)</summary>
    public string? StateName { get; set; }

    /// <summary>Loại trường bằng tiếng Việt (Loaitruongtext)</summary>
    public string? SchoolTypeLabelVN { get; set; }

    /// <summary>Học phí hiển thị (USD/năm). Lấy từ EC* field tuỳ Loai.</summary>
    public int? TuitionDisplay { get; set; }
}

public class TruongSearchFilterViewModel
{
    public string? Ten { get; set; }
    public string? Letter { get; set; }         // lọc theo chữ cái đầu tên trường
    public int? QuocGia { get; set; }           // single-select (backward compat / URL param)
    public List<int> QuocGiaIds { get; set; } = []; // multi-select checkboxes
    public string? Loai { get; set; }
    public int? MajorId { get; set; }            // single-select (backward compat)
    public List<int> MajorIds { get; set; } = []; // multi-select checkboxes
    public int? TuitionMax { get; set; }
    public bool? IsPartner { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}

public class TruongSearchResultViewModel
{
    public IEnumerable<TruongCardViewModel> Items { get; set; } = [];
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);
    public TruongSearchFilterViewModel Filter { get; set; } = new();
    public IEnumerable<QuocGiaViewModel> QuocGiaList { get; set; } = [];
    public IEnumerable<MajorViewModel> MajorList { get; set; } = [];
}

public class QuocGiaViewModel
{
    public int Id { get; set; }
    public string? Ten { get; set; }
    public string? TenEN { get; set; }
    public string? Flag { get; set; }
    public int TruongCount { get; set; }
}

public class MajorViewModel
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? TitleVN { get; set; }
    public int TruongCount { get; set; }
}

public class TruongDetailViewModel
{
    public TruongCardViewModel Card { get; set; } = new();
    public string? NameofSchool { get; set; }
    public string? Tomtat { get; set; }
    public string? TomTatEN { get; set; }
    public string? Info { get; set; }
    public string? InfoEN { get; set; }
    public string? Descreption { get; set; }
    public string? DescreptionEN { get; set; }
    public string? DescreptionWebsite { get; set; }
    public string? Address { get; set; }
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? VideoLink { get; set; }
    public string? Namthanhlap { get; set; }
    public string? Loai { get; set; }
    public string? Loaitruongtext { get; set; }
    public string? Kiemdinh { get; set; }
    public string? KiemdinhEN { get; set; }
    public string? TypeofRanking { get; set; }
    public string? TypeofRankingVN { get; set; }
    public string? ThanhPholongannhat { get; set; }
    public string? ProgramOfered { get; set; }
    public string? ReligiousAffiliation { get; set; }
    public string? LanguageofInstruction { get; set; }
    public bool IsPartner { get; set; }
    public string? LogoUrl { get; set; }
    public string? CoverUrl { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Linkedin { get; set; }
    public string? GPlus { get; set; }
    public string? Youtube { get; set; }
    public string? Instagram { get; set; }
    public string? CountryName { get; set; }
    public IEnumerable<MajorViewModel> Majors { get; set; } = [];
    public TruongAdmis4YearViewModel? Admis4Year { get; set; }
    public TruongAdmisBFViewModel? AdmisBF { get; set; }
    public TruongAdmisESLViewModel? AdmisESL { get; set; }
    public List<NewsItemViewModel> RelatedNews { get; set; } = [];
    public List<TruongCardViewModel> RelatedTruong { get; set; } = [];
}

/// <summary>
/// Dữ liệu truyền vào ViewComponent TruongFilterSidebar.
/// Mỗi Page tạo một instance rồi chỉnh properties cho phù hợp.
/// </summary>
public class TruongFilterSidebarViewModel
{
    // ── CẤU HÌNH HIỂN THỊ ───────────────────────────────────────
    /// <summary>Hiển thị box Quốc gia. Nếu đã chọn sẵn 1 quốc gia (vd. /truong-doi-tac/my) thì ẩn box và set FixedCountryId.</summary>
    public bool ShowCountry { get; set; } = true;
    /// <summary>Nếu != null, quốc gia đã được chọn cố định ở route (không hiển thị box, nhưng truyền vào filter URL).</summary>
    public int? FixedCountryId { get; set; }

    /// <summary>Hiển thị box Bậc học.</summary>
    public bool ShowLoai { get; set; } = true;
    /// <summary>Hiển thị box Học phí.</summary>
    public bool ShowTuition { get; set; } = true;
    /// <summary>Hiển thị box Ngành học.</summary>
    public bool ShowMajor { get; set; } = true;

    // ── DỮ LIỆU ─────────────────────────────────────────────────
    public IEnumerable<QuocGiaViewModel> QuocGiaList { get; set; } = [];
    public IEnumerable<MajorViewModel>   MajorList   { get; set; } = [];

    // ── TRẠNG THÁI FILTER HIỆN TẠI ──────────────────────────────
    public TruongSearchFilterViewModel Filter { get; set; } = new();

    // ── BASE URL để build filter links (vd. "/truong-doi-tac", "/tim-truong") ──
    public string BaseUrl { get; set; } = "/tim-truong";

    // ── Giữ lại Ten khi clear filter ────────────────────────────
    public string? Ten => Filter.Ten;
}

public class TruongAdmis4YearViewModel
{
    public int? TuitionUnder { get; set; }
    public int? TuitionGrad { get; set; }
    public int? TuitionAss { get; set; }
    public int? TuitionESL { get; set; }
    // Chi phí bổ sung
    public int? LivingCost { get; set; }
    public int? InsuranceCost { get; set; }
    public int? OtherCost { get; set; }
    // Học bổng
    public bool? ScholarshipUnder { get; set; }
    public string? ScholarshipUnderRangeVN { get; set; }
    public bool? ScholarshipGrad { get; set; }
    public string? ScholarshipGradRangeVN { get; set; }
    public bool? ScholarshipAss { get; set; }
    public string? ScholarshipAssRangeVN { get; set; }
    public bool? ScholarshipESL { get; set; }
    public string? ScholarshipESLRangeVN { get; set; }
    public string? ScholarshipNoteVN { get; set; }
    public string? FinancialAidNote { get; set; }
    // Tuyển sinh
    public DateTime? FallUnder { get; set; }
    public DateTime? SpringUnder { get; set; }
    public DateTime? FallGrad { get; set; }
    public bool? RollingUnder { get; set; }
    public bool? RollingGrad { get; set; }
    public string? ToefliBTUnder { get; set; }
    public string? IELTSUnder { get; set; }
    public string? DuolingoUnder { get; set; }
    public string? OtherTestUnder { get; set; }
    public string? ApplicationFeeUnder { get; set; }
    // Thống kê
    public string? GraduationRate { get; set; }
    public string? EmploymentRateAfterGraduation { get; set; }
    public string? MostMajor1 { get; set; }
    public string? MostMajor2 { get; set; }
    public string? MostMajor3 { get; set; }
    public string? MostMajor4 { get; set; }
    public string? MostMajor5 { get; set; }
    public bool? OnCampus { get; set; }
    public int? NOSTotalUnder { get; set; }
    public int? NOSInternationalUnder { get; set; }
    public int? NOSVNUnder { get; set; }
}

public class TruongAdmisBFViewModel
{
    public int? GradesFrom { get; set; }
    public int? GradesTo { get; set; }
    public bool? ESL { get; set; }
    public bool? APCourse { get; set; }
    public bool? IBCourse { get; set; }
    public bool? HonorsCourse { get; set; }
    public bool? TESTToefl { get; set; }
    public int? TESTToeflMin { get; set; }
    public bool? TESTIELTS { get; set; }
    public decimal? TESTIELTSMin { get; set; }
    public int? Tuition { get; set; }
    public int? COSRoom { get; set; }
    public bool? ScholarshipInternation { get; set; }
    public string? ScholarshipInternationRangVN { get; set; }
    public bool? HousingBF { get; set; }
    public bool? HousingHome { get; set; }
    public bool? SummerProgram { get; set; }
    public string? SummerProgramAges { get; set; }
    public string? SummerProgramCOST { get; set; }
    public DateTime? AdmFall { get; set; }
    public bool? AdmRoll { get; set; }
}

public class TruongAdmisESLViewModel
{
    public string? TypeOfCourse { get; set; }
    public string? LCName { get; set; }
    public string? LCCost { get; set; }
    public string? Conditional { get; set; }
    public int? RateOfStudent { get; set; }
    public int? NOSTotal { get; set; }
    public int? NOSInternation { get; set; }
    public int? Tuition { get; set; }
    public int? COSRoom { get; set; }
    public bool? Scholarship { get; set; }
    public string? ScholarshipRange { get; set; }
    public bool? HousingOncampus { get; set; }
    public bool? HousingHomeStay { get; set; }
    public bool? HousingApartment { get; set; }
    public DateTime? AdmFall { get; set; }
    public bool? AdmRoll { get; set; }
    public bool? WorkOpp { get; set; }
}

public class MajorSearchViewModel
{
    public IEnumerable<MajorViewModel> Majors { get; set; } = [];
    public string? Filter { get; set; }
    public int? QuocGiaId { get; set; }
    public string? Loai { get; set; }
    public IEnumerable<QuocGiaViewModel> QuocGiaList { get; set; } = [];
}
