using System.ComponentModel.DataAnnotations;

namespace NVCMS.WebView.Data.ViewModels;

public class TuVanFormInputViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Vui lòng chọn hình thức tư vấn.")]
    public string HinhThuc { get; set; } = "TUVANDUHOC";

    [Required(ErrorMessage = "Vui lòng chọn văn phòng.")]
    public string VanPhong { get; set; } = "HN";

    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    public string HoVaTen { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string? Email { get; set; }

    [RegularExpression(@"^[0-9+\s().-]{8,20}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
    public string? SoDienThoai { get; set; }

    public string? NoiDung { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý điều khoản bảo mật.")]
    public bool DongYDieuKhoan { get; set; }

    public bool NhanThongTin { get; set; } = true;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(SoDienThoai))
        {
            yield return new ValidationResult(
                "Vui lòng nhập Email hoặc Số điện thoại.",
                [nameof(Email), nameof(SoDienThoai)]);
        }
    }
}