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

    [StringLength(2000, ErrorMessage = "Nội dung không được quá 2000 ký tự.")]
    public string? NoiDung { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn đồng ý điều khoản.")]
    public string? DongYDieuKhoanTuvan { get; set; }

    public string? NhanThongTin { get; set; }

    public string? RecaptchaToken { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        // Phải có ít nhất Email hoặc SĐT
        if (string.IsNullOrWhiteSpace(Email) && string.IsNullOrWhiteSpace(SoDienThoai))
        {
            yield return new ValidationResult(
                "Vui lòng nhập Email hoặc Số điện thoại.",
                [nameof(Email), nameof(SoDienThoai)]);
        }

        // Validate email format nếu có
        if (!string.IsNullOrWhiteSpace(Email))
        {
            var emailTrimmed = Email.Trim();
            if (!IsValidEmail(emailTrimmed))
            {
                yield return new ValidationResult(
                    "Email không đúng định dạng.",
                    [nameof(Email)]);
            }
        }

        // Validate phone format nếu có
        if (!string.IsNullOrWhiteSpace(SoDienThoai))
        {
            var phoneTrimmed = SoDienThoai.Trim().Replace(" ", "").Replace("-", "").Replace(".", "");
            if (!IsValidVietnamesePhone(phoneTrimmed))
            {
                yield return new ValidationResult(
                    "Số điện thoại không đúng định dạng Việt Nam.",
                    [nameof(SoDienThoai)]);
            }
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidVietnamesePhone(string phone)
    {
        // Remove country code if present
        phone = phone.TrimStart('+');
        if (phone.StartsWith("84"))
            phone = "0" + phone[2..];

        // Vietnamese phone: starts with 0, followed by 9 digits
        // Mobile: 03, 05, 07, 08, 09 (10 digits total)
        // Landline: 02 (10 digits total)
        return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^0[2-9][0-9]{8}$");
    }
}
