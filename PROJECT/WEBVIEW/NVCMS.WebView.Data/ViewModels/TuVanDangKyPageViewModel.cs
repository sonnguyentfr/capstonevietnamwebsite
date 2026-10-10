namespace NVCMS.WebView.Data.ViewModels;

/// <summary>Một mục danh mục (id + tên) dùng cho select/checkbox.</summary>
public record TuVanOption(int Id, string Title);

/// <summary>Các danh mục CRM dùng cho form đăng ký tư vấn.</summary>
public class TuVanCatalogs
{
    public IReadOnlyList<TuVanOption> Provinces    { get; init; } = [];
    public IReadOnlyList<TuVanOption> BacHoc       { get; init; } = [];
    public IReadOnlyList<TuVanOption> KhaNangChiTra { get; init; } = [];
    public IReadOnlyList<TuVanOption> QuocGia      { get; init; } = [];

    /// <summary>Gợi ý ngành học (Cap_Truong_Major.TitleVN) – vẫn cho tự gõ.</summary>
    public IReadOnlyList<string>      NganhHoc     { get; init; } = [];
}

public class TuVanDangKyPageViewModel
{
    public TuVanCatalogs Catalogs { get; set; } = new();

    /// <summary>Quốc gia hiển thị nhanh ở đầu danh sách.</summary>
    public IReadOnlyList<TuVanOption> PopularQuocGia { get; set; } = [];

    public IReadOnlyList<int> NamDiOptions { get; set; } = [];

    public TuVanDangKyInputViewModel Input { get; set; } = new();
}

/// <summary>Kết quả lưu CRM + thông tin đã quy đổi sang tên để gửi mail thông báo.</summary>
public class TuVanDangKyResult
{
    public int    StudentId   { get; init; }
    public string StudentCode { get; init; } = string.Empty;

    /// <summary>true = SĐT/Email đã có trên CRM, chỉ cập nhật "Tư vấn khác".</summary>
    public bool   IsExisting  { get; init; }

    public string HoVaTen     { get; init; } = string.Empty;
    public string NgaySinh    { get; init; } = string.Empty;
    public string GioiTinh    { get; init; } = string.Empty;
    public string Email       { get; init; } = string.Empty;
    public string SoDienThoai { get; init; } = string.Empty;
    public string Tinh        { get; init; } = string.Empty;
    public string BacHoc      { get; init; } = string.Empty;
    public string NamDi       { get; init; } = string.Empty;
    public string NganhHoc    { get; init; } = string.Empty;
    public string KhaNangChiTra { get; init; } = string.Empty;
    public string QuocGia     { get; init; } = string.Empty;
    public string TuVanKhac   { get; init; } = string.Empty;

    /// <summary>Trang khách đang xem khi đăng ký (form nhanh), rỗng với form đầy đủ.</summary>
    public string NguonTrang  { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
}
