using System.ComponentModel.DataAnnotations;

namespace NVCMS.WebView.Data.ViewModels;

public class EventRegistrationInputViewModel
{
    [Required]
    public int EventCatId { get; set; }

    [Required]
    public int EventId { get; set; }

    // ── Who are you ────────────────────────────────────────────────────
    public string? BanLa { get; set; }

    // ── Contact ────────────────────────────────────────────────────────
    public string? Email { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    public string SoDienThoai { get; set; } = string.Empty;

    // ── Name (separate) ───────────────────────────────────────────────
    [Required(ErrorMessage = "Vui lòng nhập họ và tên đệm.")]
    public string Hotendem { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập tên.")]
    public string Ten { get; set; } = string.Empty;

    // ── Personal info ─────────────────────────────────────────────────
    public string? GioiTinh { get; set; }

    public int? NgaySinhNgay  { get; set; }
    public int? NgaySinhThang { get; set; }
    public int? NgaySinhNam   { get; set; }

    public string? TinhThanh { get; set; }

    // ── Consultation ──────────────────────────────────────────────────
    public string? TuVan { get; set; }
    public bool    QuanTamDinhCuMy { get; set; }

    // ── Terms ─────────────────────────────────────────────────────────
    [Range(typeof(bool), "true", "true", ErrorMessage = "Bạn phải đồng ý điều khoản bảo mật.")]
    public bool DongYDieuKhoan { get; set; }

    // ── Computed helpers ──────────────────────────────────────────────
    public string HoVaTen => $"{Hotendem} {Ten}".Trim();

    public DateTime? NgaySinh
    {
        get
        {
            if (NgaySinhNgay is > 0 && NgaySinhThang is > 0 && NgaySinhNam is > 1900)
            {
                try { return new DateTime(NgaySinhNam.Value, NgaySinhThang.Value, NgaySinhNgay.Value); }
                catch { return null; }
            }
            return null;
        }
    }
}
