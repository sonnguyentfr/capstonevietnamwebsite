using System.ComponentModel.DataAnnotations;
using NVCMS.WebView.Data.Common;

namespace NVCMS.WebView.Data.ViewModels;

/// <summary>
/// Thông tin cơ bản dùng chung cho form /dang-ky-tu-van và form tư vấn nhanh ở sidebar.
/// </summary>
public class TuVanCoBanInputViewModel : IValidatableObject
{
    /// <summary>Giá trị lưu vào Student_Info.Email khi khách không có email.</summary>
    public const string NoEmail = "NA";

    /// <summary>Giá trị lưu vào Student_Info.Sodienthoai khi khách không có SĐT.</summary>
    public const string NoPhone = "0";

    public const int MaxQuocGia = 10;

    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    [StringLength(150, ErrorMessage = "Họ và tên không được quá 150 ký tự.")]
    [RegularExpression(@"^[\p{L}\s.'-]+$", ErrorMessage = "Họ và tên chỉ được chứa chữ cái.")]
    public string HoVaTen { get; set; } = string.Empty;

    public int? NgaySinhNgay  { get; set; }
    public int? NgaySinhThang { get; set; }
    public int? NgaySinhNam   { get; set; }

    /// <summary>"1" = Nam, "0" = Nữ.</summary>
    [Required(ErrorMessage = "Vui lòng chọn giới tính.")]
    public string? GioiTinh { get; set; }

    /// <summary>Để trống hoặc nhập NA nếu không có email.</summary>
    [StringLength(200)]
    public string? Email { get; set; }

    /// <summary>Để trống hoặc nhập 0 nếu không có số điện thoại.</summary>
    [StringLength(30)]
    public string? SoDienThoai { get; set; }

    /// <summary>Cap_Location.id (ParentId = 82).</summary>
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn tỉnh/thành.")]
    public int TinhId { get; set; }

    /// <summary>Cap_Location.id (ParentId = 0).</summary>
    public List<int> QuocGiaIds { get; set; } = [];

    [StringLength(800, ErrorMessage = "Nội dung tư vấn không được quá 800 ký tự.")]
    public string? TuVanKhac { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý điều khoản bảo mật.")]
    public bool DongYDieuKhoan { get; set; }

    public DateTime? NgaySinh
    {
        get
        {
            if (NgaySinhNgay is > 0 && NgaySinhThang is > 0 && NgaySinhNam is > 1900)
            {
                try { return new DateTime(NgaySinhNam.Value, NgaySinhThang.Value, NgaySinhNgay.Value); }
                catch (ArgumentOutOfRangeException) { return null; }
            }
            return null;
        }
    }

    public bool HasEmail =>
        !string.IsNullOrWhiteSpace(Email) &&
        !string.Equals(Email.Trim(), NoEmail, StringComparison.OrdinalIgnoreCase);

    public bool HasPhone =>
        !string.IsNullOrWhiteSpace(SoDienThoai) && SoDienThoai.Trim() != NoPhone;

    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var dob = NgaySinh;
        if (dob is null)
            yield return new ValidationResult("Vui lòng chọn ngày sinh hợp lệ.", [nameof(NgaySinhNgay)]);
        else if (dob > DateTime.Today)
            yield return new ValidationResult("Ngày sinh không hợp lệ.", [nameof(NgaySinhNgay)]);

        if (!HasEmail && !HasPhone)
            yield return new ValidationResult(
                "Vui lòng nhập Email hoặc Số điện thoại.",
                [nameof(Email), nameof(SoDienThoai)]);

        if (HasEmail && !EmailHelper.IsValid(Email))
            yield return new ValidationResult("Email không đúng định dạng.", [nameof(Email)]);

        if (HasPhone && !PhoneHelper.IsValid(SoDienThoai))
            yield return new ValidationResult("Số điện thoại không hợp lệ.", [nameof(SoDienThoai)]);

        if (QuocGiaIds.Count == 0)
            yield return new ValidationResult("Vui lòng chọn ít nhất một quốc gia.", [nameof(QuocGiaIds)]);
        else if (QuocGiaIds.Count > MaxQuocGia)
            yield return new ValidationResult($"Chỉ chọn tối đa {MaxQuocGia} quốc gia.", [nameof(QuocGiaIds)]);
    }
}

/// <summary>Form đầy đủ /dang-ky-tu-van.</summary>
public class TuVanDangKyInputViewModel : TuVanCoBanInputViewModel
{
    public const int MaxNganhHoc = 10;

    /// <summary>Cap_CapGiaoduc.id – không bắt buộc.</summary>
    public List<int> BacHocIds { get; set; } = [];

    [Required(ErrorMessage = "Vui lòng chọn năm dự định đi học.")]
    public int? NamDuDinhDi { get; set; }

    /// <summary>Ngành tick chọn trong grid (Cap_Truong_Major.TitleVN) – không bắt buộc.</summary>
    public List<string> NganhHocChon { get; set; } = [];

    /// <summary>Ngành tự gõ thêm ngoài danh mục – không bắt buộc.</summary>
    [StringLength(200, ErrorMessage = "Ngành học khác không được quá 200 ký tự.")]
    public string? NganhHoc { get; set; }

    /// <summary>Student_TuVanInfo_ChiTra.id – 0 = chưa chọn.</summary>
    public int KhaNangChiTra { get; set; }

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        foreach (var r in base.Validate(validationContext))
            yield return r;

        if (NganhHocChon.Count > MaxNganhHoc)
            yield return new ValidationResult($"Chỉ chọn tối đa {MaxNganhHoc} ngành học.", [nameof(NganhHocChon)]);
    }
}

/// <summary>Trang chi tiết đang chứa form tư vấn nhanh.</summary>
public enum TuVanNguonTrang
{
    Khac = 0,
    Truong = 1,
    TinTuc = 2,
}

/// <summary>Form tư vấn nhanh ở sidebar trang chi tiết trường / tin tức.</summary>
public class TuVanNhanhInputViewModel : TuVanCoBanInputViewModel
{
    public TuVanNguonTrang NguonTrang { get; set; }

    /// <summary>Tên trường / tiêu đề bài viết khách đang xem.</summary>
    [StringLength(300)]
    public string? NguonTieuDe { get; set; }
}
