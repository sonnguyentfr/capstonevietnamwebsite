using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface ITuVanDangKyRepository
{
    /// <summary>Cap_CapGiaoduc (IsActive = 1).</summary>
    Task<IReadOnlyList<TuVanOption>> GetBacHocAsync(int portalId);

    /// <summary>Cap_Truong_Major.TitleVN – gợi ý ngành học.</summary>
    Task<IReadOnlyList<string>> GetNganhHocAsync();

    /// <summary>Student_TuVanInfo_ChiTra, sắp xếp theo mức tiền tăng dần.</summary>
    Task<IReadOnlyList<TuVanOption>> GetKhaNangChiTraAsync(int portalId);

    /// <summary>
    /// Gọi WebView_TuVan_Upsert: chưa có thì tạo Student_Info mới,
    /// đã có (trùng SĐT/Email) thì chỉ cập nhật TuVanKhac.
    /// </summary>
    Task<(int StudentId, string StudentCode, bool IsExisting)> UpsertAsync(
        TuVanUpsertArgs args, CancellationToken ct = default);
}

/// <summary>Tham số đã chuẩn hoá truyền vào WebView_TuVan_Upsert.</summary>
public class TuVanUpsertArgs
{
    public string    Hotendem            { get; init; } = string.Empty;
    public string    Ten                 { get; init; } = string.Empty;
    public bool      Sex                 { get; init; }
    public DateTime? Ngaysinh            { get; init; }
    public string    Sodienthoai         { get; init; } = string.Empty;
    public string    Email               { get; init; } = string.Empty;
    public string    Diachi              { get; init; } = string.Empty;
    public int?      TinhId              { get; init; }
    public string    TuVanHocVanmongmuon { get; init; } = string.Empty;
    public string    TuVanNamdi          { get; init; } = string.Empty;
    public string    TuVanNganhhoc       { get; init; } = string.Empty;
    public int       TuVanKhanangchitra  { get; init; }
    public string    TuVanQuocgia        { get; init; } = string.Empty;
    public string    TuVanKhac           { get; init; } = string.Empty;
    public int       HinhThuc            { get; init; }
    public string    CodePrefix          { get; init; } = string.Empty;
    public int       PortalId            { get; init; }
}
