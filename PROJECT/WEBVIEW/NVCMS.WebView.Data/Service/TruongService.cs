using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class TruongService : ITruongService
{
    private readonly ITruongRepository _repo;
    private readonly ContentUrlRewriter _rewriter;

    // Country Id mapping — matches Cap_Location.LocationId in CapstoneVietnam_old
    // 1=Úc, 3=Canada, 23=Thụy Sĩ, 28=Anh, 38=Mỹ, 99=New Zealand, 401=Ireland
    private static readonly Dictionary<int, string> CountryNames = new()
    {
        {1,  "Úc"},
        {3,  "Canada"},
        {23, "Thụy Sĩ"},
        {28, "Anh"},
        {38, "Mỹ"},
        {99, "New Zealand"},
        {401,"Ireland"},
    };

    public TruongService(ITruongRepository repo, ContentUrlRewriter rewriter)
    {
        _repo = repo;
        _rewriter = rewriter;
    }

    public async Task<TruongSearchResultViewModel> SearchAsync(TruongSearchFilterViewModel filter)
    {
        // Merge single-select backward-compat fields into the list variants
        if (filter.QuocGia.HasValue && !filter.QuocGiaIds.Contains(filter.QuocGia.Value))
            filter.QuocGiaIds.Add(filter.QuocGia.Value);
        if (filter.MajorId.HasValue && !filter.MajorIds.Contains(filter.MajorId.Value))
            filter.MajorIds.Add(filter.MajorId.Value);

        // Cap_Truong.Loai stores raw integer IDs ("2","10",...).
        // When the filter carries a normalized code ("4Y","2Y",...) expand it to
        // all matching raw IDs and fan-out the query, then merge + re-page.
        var rawIds = LoaiCodeToRawIds(filter.Loai);

        // Build all combinations: for each country (or no country filter) × each rawLoai
        var countryList = filter.QuocGiaIds.Count > 0 ? filter.QuocGiaIds : new List<int> { 0 };
        IEnumerable<TruongModel> items;
        int total;

        bool multiQuery = rawIds.Count > 1 || filter.QuocGiaIds.Count > 1;

        if (multiQuery)
        {
            var tasks = new List<Task<(IEnumerable<TruongModel> Items, int Total)>>();
            foreach (var cid in countryList)
            {
                foreach (var rawId in (rawIds.Count > 0 ? rawIds : new List<string> { "" }))
                {
                    tasks.Add(_repo.SearchAsync(new TruongSearchFilterViewModel
                    {
                        Ten = filter.Ten,
                        QuocGia = cid > 0 ? cid : null,
                        Loai = string.IsNullOrEmpty(rawId) ? null : rawId,
                        MajorId = filter.MajorIds.Count == 1 ? filter.MajorIds[0] : filter.MajorId,
                        TuitionMax = filter.TuitionMax,
                        IsPartner = filter.IsPartner,
                        Page = 1,
                        PageSize = 10_000
                    }));
                }
            }
            var results = await Task.WhenAll(tasks);
            var allItems = results.SelectMany(r => r.Items).DistinctBy(t => t.Id).ToList();

            // Multi-select major filter (in-memory after normalization)
            if (filter.MajorIds.Count > 1)
            {
                // major filtering handled by repo for single, here we re-filter
            }

            total = allItems.Count;
            items = allItems
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);
        }
        else
        {
            var repoFilter = new TruongSearchFilterViewModel
            {
                Ten = filter.Ten,
                QuocGia = filter.QuocGiaIds.Count == 1 ? filter.QuocGiaIds[0] : filter.QuocGia,
                Loai = rawIds.Count == 1 ? rawIds[0] : (rawIds.Count == 0 ? null : filter.Loai),
                MajorId = filter.MajorIds.Count == 1 ? filter.MajorIds[0] : filter.MajorId,
                TuitionMax = filter.TuitionMax,
                IsPartner = filter.IsPartner,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
            (items, total) = await _repo.SearchAsync(repoFilter);
        }

        var majors = await _repo.GetAllMajorsAsync();
        var countries = await _repo.GetCountriesWithCountAsync(filter.IsPartner == true ? true : null);
        return new TruongSearchResultViewModel
        {
            Items = items.Select(MapCard),
            Total = total,
            Page = filter.Page,
            PageSize = filter.PageSize,
            Filter = filter,
            MajorList = majors.Select(m => new MajorViewModel { Id = m.Id, Title = m.Title, TitleVN = m.TitleVN }),
            QuocGiaList = countries.Select(c => new QuocGiaViewModel { Id = c.Id, Ten = c.Ten, TruongCount = c.Count })
        };
    }

    public async Task<IEnumerable<TruongCardViewModel>> GetRandomPartnersAsync(int count)
    {
        var items = await _repo.GetRandomPartnersAsync(count);
        return items.Select(MapCard);
    }

    public async Task<IEnumerable<TruongCardViewModel>> GetByCountryAsync(int countryId, string? loai = null)
    {
        // Fetch all records for the country without a loai filter at the DB level,
        // because the DB stores raw integer IDs (e.g. "2", "10") while the UI
        // passes normalized codes (e.g. "4Y"). Filter in-memory after MapCard
        // normalizes the Loai field so the comparison is always code-to-code.
        var items = await _repo.GetByCountryAsync(countryId, null);
        var cards = items.Select(MapCard);
        if (!string.IsNullOrWhiteSpace(loai))
            cards = cards.Where(c => c.Loai == loai);
        return cards;
    }

    public async Task<TruongDetailViewModel?> GetDetailAsync(int id)
    {
        var t = await _repo.GetByIdAsync(id);
        if (t is null) return null;

        var majors = await _repo.GetMajorsByTruongAsync(id);

        var loaiCode = NormalizeLoai(t.Loai);
        TruongAdmis4YearViewModel? vm4y = null;
        TruongAdmisBFViewModel? vmBF = null;
        TruongAdmisESLViewModel? vmESL = null;

        if (loaiCode is "4Y" or "2Y" or "GR")
        {
            var a4 = await _repo.GetAdmis4YearAsync(id);
            if (a4 is not null) vm4y = Map4Year(a4);
        }
        if (loaiCode is "BF" or "HS")
        {
            var aBF = await _repo.GetAdmisBFAsync(id);
            if (aBF is not null) vmBF = MapBF(aBF);
        }
        if (loaiCode is "ESL")
        {
            var aESL = await _repo.GetAdmisESLAsync(id);
            if (aESL is not null) vmESL = MapESL(aESL);
        }
        // Fallback: load all if loai unknown
        if (vm4y is null && vmBF is null && vmESL is null)
        {
            var a4 = await _repo.GetAdmis4YearAsync(id);
            if (a4 is not null) vm4y = Map4Year(a4);
            var aBF = await _repo.GetAdmisBFAsync(id);
            if (aBF is not null) vmBF = MapBF(aBF);
            var aESL = await _repo.GetAdmisESLAsync(id);
            if (aESL is not null) vmESL = MapESL(aESL);
        }
        var social = (t.Social ?? "").Split(',', StringSplitOptions.TrimEntries);
        return new TruongDetailViewModel
        {
            Card = MapCard(t),
            NameofSchool = t.NameofSchool,
            Tomtat = t.Tomtat,
            TomTatEN = t.TomTatEN,
            Info = t.Info,
            InfoEN = t.InfoEN,
            Descreption = _rewriter.ResolveHtml(t.Descreption),
            DescreptionEN = _rewriter.ResolveHtml(t.DescreptionEN),
            DescreptionWebsite = t.DescreptionWebsite,
            Address = t.Address,
            Website = t.Website,
            Email = t.Email,
            Phone = t.Phone,
            VideoLink = t.VideoLink,
            Namthanhlap = t.Namthanhlap,
            Loai = loaiCode,
            Loaitruongtext = t.Loaitruongtext,
            Kiemdinh = t.Kiemdinh,
            KiemdinhEN = t.KiemdinhEN,
            TypeofRanking = t.TypeofRanking,
            TypeofRankingVN = t.TypeofRankingVN,
            ThanhPholongannhat = t.ThanhPholongannhat,
            ProgramOfered = t.ProgramOfered,
            ReligiousAffiliation = t.ReligiousAffiliation,
            LanguageofInstruction = t.LanguageofInstruction,
            IsPartner = t.isPartner ?? false,
            LogoUrl = _rewriter.ResolveUrl(t.Logo),
            CoverUrl = _rewriter.ResolveUrl(t.Conver),
            CountryName = CountryNames.TryGetValue(t.Country ?? 0, out var cn) ? cn : null,
            Majors = majors.Select(m => new MajorViewModel { Id = m.Id, Title = m.Title, TitleVN = m.TitleVN }),
            Admis4Year = vm4y,
            AdmisBF = vmBF,
            AdmisESL = vmESL,
            Facebook = social.ElementAtOrDefault(0),
            Twitter = social.ElementAtOrDefault(1),
            Linkedin = social.ElementAtOrDefault(2),
            GPlus = social.ElementAtOrDefault(3),
            Youtube = social.ElementAtOrDefault(4),
            Instagram = social.ElementAtOrDefault(5)
        };
    }

    public async Task<MajorSearchViewModel> GetMajorSearchAsync(string? filter, int? quocGiaId, string? loai)
    {
        // Convert code to first matching raw ID for the majors query
        var rawLoaiIds = LoaiCodeToRawIds(loai);
        var rawLoai = rawLoaiIds.Count == 1 ? rawLoaiIds[0] : (rawLoaiIds.Count == 0 ? null : rawLoaiIds[0]);
        var majors = await _repo.GetMajorsWithCountAsync(quocGiaId, rawLoai);
        var countries = await _repo.GetCountriesWithCountAsync();

        var list = majors.Select(m => new MajorViewModel { Id = m.Id, Title = m.Title, TitleVN = m.TitleVN })
                         .AsEnumerable();
        if (!string.IsNullOrWhiteSpace(filter))
            list = list.Where(m =>
                (m.TitleVN ?? string.Empty).Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                (m.Title ?? string.Empty).Contains(filter, StringComparison.OrdinalIgnoreCase));

        return new MajorSearchViewModel
        {
            Majors = list,
            Filter = filter,
            QuocGiaId = quocGiaId,
            Loai = loai,
            QuocGiaList = countries.Select(c => new QuocGiaViewModel { Id = c.Id, Ten = c.Ten, TruongCount = c.Count })
        };
    }

    public async Task<IEnumerable<QuocGiaViewModel>> GetCountriesAsync()
    {
        var list = await _repo.GetCountriesWithCountAsync();
        return list.Select(c => new QuocGiaViewModel { Id = c.Id, Ten = c.Ten, TruongCount = c.Count });
    }

    public async Task<IEnumerable<TruongCardViewModel>> GetHomeSwiperAsync(IEnumerable<string> loaiList, int pageSize = 12)
    {
        // Fetch all partner schools without a Loai filter at the DB level,
        // because the DB stores raw integer IDs ("2","10",...) while loaiList
        // carries normalized codes ("4Y","2Y",...).
        // Filter in-memory after MapCard normalizes Loai — same pattern as GetByCountryAsync.
        var loaiSet = new HashSet<string>(loaiList, StringComparer.OrdinalIgnoreCase);

        var (items, _) = await _repo.SearchAsync(new TruongSearchFilterViewModel
        {
            IsPartner = true,
            Page = 1,
            PageSize = 10_000   // fetch all partner schools, filter in-memory
        });

        return items
            .Select(MapCard)
            .Where(c => loaiSet.Count == 0 || loaiSet.Contains(c.Loai ?? string.Empty))
            .DistinctBy(t => t.Id)
            .Take(pageSize);
    }

    private TruongCardViewModel MapCard(TruongModel t)
    {
        var loaiCode = NormalizeLoai(t.Loai);
        return new()
        {
            Id = t.Id,
            NameofSchool = t.NameofSchool,
            Tomtat = t.Tomtat,
            LogoUrl = _rewriter.ResolveUrl(t.Logo),
            CoverUrl = _rewriter.ResolveUrl(t.Conver),
            Loai = loaiCode,
            CountryId = t.Country,
            CountryName = CountryNames.TryGetValue(t.Country ?? 0, out var cn) ? cn : null,
            IsPartner = t.isPartner ?? false,
            Slug = SlugHelper.ToSlug(t.NameofSchool ?? string.Empty),
            StateName = string.IsNullOrWhiteSpace(t.ThanhPholongannhat) ? null : t.ThanhPholongannhat,
            SchoolTypeLabelVN = string.IsNullOrWhiteSpace(t.Loaitruongtext) ? LoaiToLabel(loaiCode) : t.Loaitruongtext,
            TuitionDisplay = TuitionFromEcFields(t, loaiCode)
        };
    }

    private static int? TuitionFromEcFields(TruongModel t, string loaiCode)
    {
        // EC* fields store -1 as "no data", treat as null
        static int? Valid(int? v) => v is null or <= 0 ? null : v;
        return loaiCode switch
        {
            "4Y" or "GR" => Valid(t.ECUnder) ?? Valid(t.ECgrad),
            "2Y" => Valid(t.ECass) ?? Valid(t.ECUnder),
            "BF" or "HS" => Valid(t.ECHighschool),
            "ESL" => Valid(t.ECESL),
            _ => Valid(t.ECUnder) ?? Valid(t.ECHighschool) ?? Valid(t.ECass)
        };
    }

    /// <summary>
    /// Cap_Truong.Loai lưu INTEGER ID từ bảng Cap_Loaitruong.
    /// Hàm này map integer ID (dạng string) sang code nội bộ.
    /// </summary>
    private static string NormalizeLoai(string? rawLoai)
    {
        return rawLoai switch
        {
            "1" => "ESL",  // ESL
            "11" => "ESL",  // Language & Culture Institute
            "2" => "4Y",   // University
            "10" => "4Y",   // Liberal Art College
            "3" => "2Y",   // College / Community College
            "6" => "2Y",   // College
            "4" => "BF",   // Secondary School
            "5" => "BF",   // Primary - Secondary Education
            "9" => "GR",   // Graduate
            "7" => "ESL",  // Summer & Winter program
            "8" => "ESL",  // Summer Program
            // Nếu đã là code string thì giữ nguyên (backward compat)
            "4Y" or "2Y" or "GR" or "BF" or "HS" or "ESL" => rawLoai,
            _ => rawLoai ?? string.Empty
        };
    }

    private static string LoaiToLabel(string? loai) => loai switch
    {
        "4Y" => "Đại học",
        "2Y" => "Cao đẳng",
        "GR" => "Sau đại học",
        "BF" => "Trung học",
        "HS" => "Trung học",
        "ESL" => "Anh ngữ ESL",
        _ => "Trường đối tác"
    };

    /// <summary>
    /// Reverse-map của NormalizeLoai: trả về danh sách raw integer ID (dạng string)
    /// tương ứng với một code nội bộ, để truyền vào WHERE Loai IN (...) ở DB.
    /// Nếu đầu vào đã là raw ID (hoặc null/empty) thì trả về list chứa chính nó.
    /// </summary>
    private static List<string> LoaiCodeToRawIds(string? code) => code switch
    {
        "4Y" => ["2", "10"],
        "2Y" => ["3", "6"],
        "BF" or "HS" => ["4", "5"],
        "GR" => ["9"],
        "ESL" => ["1", "7", "8", "11"],
        _ => string.IsNullOrWhiteSpace(code) ? [] : [code]
    };

    private static TruongAdmis4YearViewModel Map4Year(TruongAdmis4YearModel a)
    {
        var majors = (a.MostMajor ?? "")
            .Split(',', StringSplitOptions.TrimEntries);

        return new TruongAdmis4YearViewModel
        {
            TuitionUnder = a.COSTuitionfeeUnder,
            TuitionGrad = a.COSTuitionfeeGrad,
            TuitionAss = a.COSTuitionfeeAss,
            TuitionESL = a.COSTuitionfeeESL,

            ScholarshipUnder = a.ScholarshipUnder,
            ScholarshipUnderRangeVN = a.ScholarshipUnderRangeVN,
            ScholarshipGrad = a.ScholarshipGrad,
            ScholarshipGradRangeVN = a.ScholarshipGradRangeVN,
            ScholarshipAss = a.ScholarshipAss,
            ScholarshipAssRangeVN = a.ScholarshipAssRangeVN,
            ScholarshipESL = a.ScholarshipESL,
            ScholarshipESLRangeVN = a.ScholarshipESLRangeVN,
            ScholarshipNoteVN = a.ScholarshipNoteVN,

            FallUnder = a.FallUnder,
            SpringUnder = a.SpringUnder,
            FallGrad = a.FallGrad,
            RollingUnder = a.RollingUnder,
            RollingGrad = a.RollingGrad,

            ToefliBTUnder = a.ToefliBTUnder,
            IELTSUnder = a.IELTSUnder,

            GraduationRate = a.GraduationRate,
            EmploymentRateAfterGraduation = a.EmploymentRateAfterGraduation,

            MostMajor1 = majors.ElementAtOrDefault(0),
            MostMajor2 = majors.ElementAtOrDefault(1),
            MostMajor3 = majors.ElementAtOrDefault(2),
            MostMajor4 = majors.ElementAtOrDefault(3),
            MostMajor5 = majors.ElementAtOrDefault(4),

            OnCampus = a.OnCampus,
            NOSTotalUnder = a.NOSTotalUnder,
            NOSInternationalUnder = a.NOSInternationalUnder,
            NOSVNUnder = a.NOSVNUnder
        };
    }

    private static TruongAdmisBFViewModel MapBF(TruongAdmisBFModel a) => new()
    {
        GradesFrom = a.Gradesfrom,
        GradesTo = a.Gradesto,
        ESL = a.ESL,
        APCourse = a.APCourse,
        IBCourse = a.IBCourse,
        HonorsCourse = a.HonorsCourse,
        TESTToefl = a.TESTToefl,
        TESTToeflMin = a.TESTToeflMin,
        TESTIELTS = a.TESTIELTS,
        TESTIELTSMin = a.TESTIELTSMin,
        Tuition = a.COSTuti,
        COSRoom = a.COSRoom,
        ScholarshipInternation = a.ScholarshipInternation,
        ScholarshipInternationRangVN = a.ScholarshipInternationRangVN,
        HousingBF = a.HousingBF,
        HousingHome = a.HousingHome,
        SummerProgram = a.SummerProgram,
        SummerProgramAges = a.SummerProgramAges,
        SummerProgramCOST = a.SummerProgramCOST,
        AdmFall = a.AdmFall,
        AdmRoll = a.AdmRoll
    };

    private static TruongAdmisESLViewModel MapESL(TruongAdmisESLModel a) => new()
    {
        TypeOfCourse = a.TypeOfCourse,
        LCName = a.LCName,
        LCCost = a.LCCost,
        Conditional = a.Conditional,
        RateOfStudent = a.RateOfStudent,
        NOSTotal = a.NOSTotal,
        NOSInternation = a.NOSInternation,
        Tuition = a.COSTuti,
        COSRoom = a.COSRoom,
        Scholarship = a.Scholarship,
        ScholarshipRange = a.ScholarshipRange,
        HousingOncampus = a.HousingOncampus,
        HousingHomeStay = a.HousingHomeStay,
        HousingApartment = a.HousingApartment,
        AdmFall = a.AdmFall,
        AdmRoll = a.AdmRoll,
        WorkOpp = a.WorkOpp
    };
}
