using System.Text.RegularExpressions;
using Capstone.View.Helpers;
using Microsoft.Extensions.Caching.Memory;
using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class TuVanDangKyService : ITuVanDangKyService
{
    private const int VietnamLocationId = 82;
    private const int CountryParentId   = 0;
    private static readonly TimeSpan CacheTtl = TimeSpan.FromHours(6);

    private readonly ITuVanDangKyRepository _repo;
    private readonly ILocationService _locations;
    private readonly IMemoryCache _cache;

    public TuVanDangKyService(ITuVanDangKyRepository repo, ILocationService locations, IMemoryCache cache)
    {
        _repo      = repo;
        _locations = locations;
        _cache     = cache;
    }

    public async Task<TuVanCatalogs> GetCatalogsAsync(int portalId)
    {
        var key = $"tuvan:catalogs:{portalId}";
        if (_cache.TryGetValue(key, out TuVanCatalogs? cached) && cached is not null)
            return cached;

        var provinces = await _locations.GetProvincesAsync(VietnamLocationId);
        var countries = await _locations.GetProvincesAsync(CountryParentId);

        var result = new TuVanCatalogs
        {
            Provinces     = ToOptions(provinces),
            QuocGia       = ToOptions(countries),
            BacHoc        = await _repo.GetBacHocAsync(portalId),
            KhaNangChiTra = await _repo.GetKhaNangChiTraAsync(portalId),
            NganhHoc      = await _repo.GetNganhHocAsync(),
        };

        _cache.Set(key, result, CacheTtl);
        return result;
    }

    public async Task<TuVanDangKyResult> SubmitAsync(
        TuVanCoBanInputViewModel input, int portalId, TuVanDangKyOptions options, CancellationToken ct = default)
    {
        var catalogs = await GetCatalogsAsync(portalId);
        var full  = input as TuVanDangKyInputViewModel;
        var quick = input as TuVanNhanhInputViewModel;

        // ── Chuẩn hoá ─────────────────────────────────────────────────────────
        var fullName = Regex.Replace(InputCleaner.Text(input.HoVaTen), @"\s+", " ");
        var (hotendem, ten) = SplitName(fullName);

        var email = input.HasEmail ? input.Email!.Trim() : TuVanCoBanInputViewModel.NoEmail;
        var phone = input.HasPhone ? PhoneHelper.Normalize(input.SoDienThoai) : TuVanCoBanInputViewModel.NoPhone;
        var sex   = input.GioiTinh == "1";

        var tinh    = catalogs.Provinces.FirstOrDefault(x => x.Id == input.TinhId);
        var quocGia = Pick(catalogs.QuocGia, input.QuocGiaIds).Take(TuVanCoBanInputViewModel.MaxQuocGia).ToList();
        var bacHoc  = full is null ? [] : Pick(catalogs.BacHoc, full.BacHocIds);
        var chiTra  = full is null ? null : catalogs.KhaNangChiTra.FirstOrDefault(x => x.Id == full.KhaNangChiTra);

        var namDi     = full?.NamDuDinhDi?.ToString() ?? string.Empty;
        var nganhHoc  = full is null ? string.Empty : BuildNganhHoc(catalogs.NganhHoc, full.NganhHocChon, full.NganhHoc);
        var nguon     = quick is null ? string.Empty : BuildNguonTrang(quick);
        var tuVanKhac = BuildTuVanKhac(nguon, InputCleaner.Text(input.TuVanKhac ?? string.Empty));

        // ── Lưu CRM ───────────────────────────────────────────────────────────
        var (studentId, studentCode, isExisting) = await _repo.UpsertAsync(new TuVanUpsertArgs
        {
            Hotendem            = hotendem,
            Ten                 = ten,
            Sex                 = sex,
            Ngaysinh            = input.NgaySinh,
            Sodienthoai         = phone,
            Email               = email,
            Diachi              = tinh?.Title ?? string.Empty,
            TinhId              = tinh?.Id,
            TuVanHocVanmongmuon = JoinIds(bacHoc),
            TuVanNamdi          = namDi,
            TuVanNganhhoc       = nganhHoc,
            TuVanKhanangchitra  = chiTra?.Id ?? 0,
            TuVanQuocgia        = JoinIds(quocGia),
            TuVanKhac           = tuVanKhac,
            HinhThuc            = options.HinhThucWebsiteId,
            CodePrefix          = options.CodePrefix,
            PortalId            = portalId,
        }, ct);

        return new TuVanDangKyResult
        {
            StudentId     = studentId,
            StudentCode   = studentCode,
            IsExisting    = isExisting,
            HoVaTen       = fullName,
            NgaySinh      = input.NgaySinh?.ToString("dd/MM/yyyy") ?? string.Empty,
            GioiTinh      = sex ? "Nam" : "Nữ",
            Email         = email,
            SoDienThoai   = phone,
            Tinh          = tinh?.Title ?? string.Empty,
            BacHoc        = string.Join(", ", bacHoc.Select(x => x.Title)),
            NamDi         = namDi,
            NganhHoc      = nganhHoc,
            KhaNangChiTra = chiTra?.Title ?? string.Empty,
            QuocGia       = string.Join(", ", quocGia.Select(x => x.Title)),
            TuVanKhac     = tuVanKhac,
            NguonTrang    = nguon,
            CreatedAt     = DateTime.Now,
        };
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static IReadOnlyList<TuVanOption> ToOptions(IEnumerable<Models.CapLocationModel> list) =>
        list.Where(x => !string.IsNullOrWhiteSpace(x.Name))
            .Select(x => new TuVanOption(x.Id, x.Name!.Trim()))
            .ToList();

    /// <summary>Chỉ giữ các id có trong danh mục, theo thứ tự danh mục.</summary>
    private static List<TuVanOption> Pick(IReadOnlyList<TuVanOption> catalog, IEnumerable<int> ids)
    {
        var set = ids.ToHashSet();
        return catalog.Where(x => set.Contains(x.Id)).ToList();
    }

    /// <summary>Định dạng CRM: "1,38," (rỗng nếu không chọn).</summary>
    private static string JoinIds(IEnumerable<TuVanOption> items) =>
        string.Concat(items.Select(x => x.Id + ","));

    /// <summary>
    /// Dòng tự động chèn vào "Tư vấn khác" khi khách đăng ký từ trang chi tiết:
    /// "(boot):Khách hàng đang xem thông tin ở trường xxx" / "(boot):Khách hàng đang xem bài viết xxx".
    /// </summary>
    private static string BuildNguonTrang(TuVanNhanhInputViewModel quick)
    {
        var title = InputCleaner.Text(quick.NguonTieuDe ?? string.Empty);
        if (title.Length > 300) title = title[..300];
        if (title.Length == 0) return string.Empty;

        return quick.NguonTrang switch
        {
            TuVanNguonTrang.Truong => $"(boot):Khách hàng đang xem thông tin ở trường {title}",
            TuVanNguonTrang.TinTuc => $"(boot):Khách hàng đang xem bài viết {title}",
            _ => string.Empty,
        };
    }

    /// <summary>
    /// Ghép dòng nguồn (nếu có) + nội dung khách nhập. Giới hạn 900 ký tự vì SP còn
    /// chèn thêm tiền tố ngày khi cập nhật khách cũ (TuVanKhac nvarchar 1000).
    /// </summary>
    private static string BuildTuVanKhac(string nguon, string noiDung)
    {
        var text = string.Join("\n", new[] { nguon, noiDung }.Where(x => x.Length > 0));
        return text.Length <= 900 ? text : text[..900];
    }

    /// <summary>
    /// TuVanNganhhoc (nvarchar 400): ngành tick chọn (chỉ nhận giá trị có trong danh mục)
    /// + ngành tự gõ, ngăn cách bằng ", ".
    /// </summary>
    private static string BuildNganhHoc(IReadOnlyList<string> catalog, IEnumerable<string> chon, string? khac)
    {
        var set = chon.Select(x => x.Trim()).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var items = catalog.Where(set.Contains)
            .Take(TuVanDangKyInputViewModel.MaxNganhHoc)
            .ToList();

        var other = InputCleaner.Text(khac ?? string.Empty);
        if (other.Length > 0 && !items.Contains(other, StringComparer.OrdinalIgnoreCase))
            items.Add(other);

        var joined = string.Join(", ", items);
        return joined.Length <= 400 ? joined : joined[..400];
    }

    /// <summary>"Nguyễn Văn An" → ("Nguyễn Văn", "An").</summary>
    private static (string Hotendem, string Ten) SplitName(string fullName)
    {
        var idx = fullName.LastIndexOf(' ');
        return idx < 0
            ? (string.Empty, fullName)
            : (fullName[..idx], fullName[(idx + 1)..]);
    }
}
