using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface ITuVanDangKyService
{
    /// <summary>Tỉnh, bậc học, khả năng chi trả, quốc gia (cache).</summary>
    Task<TuVanCatalogs> GetCatalogsAsync(int portalId);

    /// <summary>
    /// Lưu đăng ký tư vấn vào Student_Info (CRM).
    /// Nhận form đầy đủ (TuVanDangKyInputViewModel) hoặc form nhanh (TuVanNhanhInputViewModel).
    /// </summary>
    Task<TuVanDangKyResult> SubmitAsync(
        TuVanCoBanInputViewModel input, int portalId, TuVanDangKyOptions options, CancellationToken ct = default);
}

/// <summary>Cấu hình section "TuVan" trong appsettings.</summary>
public class TuVanDangKyOptions
{
    public const string SectionName = "TuVan";

    /// <summary>Danh sách email nhận thông báo, ngăn cách bằng dấu phẩy.</summary>
    public string NotifyEmails { get; set; } = "info@capstonevietnam.com";

    /// <summary>Tiền tố mã khách hàng.</summary>
    public string CodePrefix { get; set; } = "WEB";

    /// <summary>Student_Code_Hinhthuc.id của nguồn "Website".</summary>
    public int HinhThucWebsiteId { get; set; } = 7;

    /// <summary>Cap_Location.id các quốc gia hiển thị nhanh.</summary>
    public List<int> PopularCountryIds { get; set; } = [];
}
