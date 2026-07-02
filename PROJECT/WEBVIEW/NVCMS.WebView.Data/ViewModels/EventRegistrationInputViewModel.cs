using System.ComponentModel.DataAnnotations;

namespace NVCMS.WebView.Data.ViewModels;

public class EventRegistrationInputViewModel
{
    [Required]
    public int EventCatId { get; set; }

    [Required]
    public int EventId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
    public string HoVaTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    public string SoDienThoai { get; set; } = string.Empty;

    public string? Email { get; set; }

    public string? DiaChi { get; set; }

    [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý điều khoản bảo mật.")]
    public bool DongYDieuKhoan { get; set; }
}
