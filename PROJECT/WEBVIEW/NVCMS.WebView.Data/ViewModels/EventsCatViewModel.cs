namespace NVCMS.WebView.Data.ViewModels;


public class EventsCatViewModel
{
    public int Id { get; set; }
    public string CatName { get; set; } = string.Empty;
    public string CatNameEN { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Desception { get; set; }
    public string? DesceptionEN { get; set; }
    public string? Contentx { get; set; }
    public string? ContentxEN { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? DateShow { get; set; }
    public string? FairSchool { get; set; }
    public string? FairDiengia { get; set; }
    public string? FairTestimonial { get; set; }
    public string? FairDonviTaiTro { get; set; }
    public string? FairOrg { get; set; }
    public string? Email { get; set; }
    public string? Link_pr { get; set; }
    public int? TabID { get; set; }
    public int? Ordernumber { get; set; }
    public bool Is_show_website { get; set; }
    public int PortalId { get; set; }

    /// <summary>Gửi email xác nhận cho user sau khi đăng ký (map từ NV_Events_Cat.Sendmail).</summary>
    public bool? Sendmail { get; set; }
    /// <summary>Gửi mã QR/barcode kèm email xác nhận (map từ NV_Events_Cat.Sendcode).</summary>
    public bool? Sendcode { get; set; }
    /// <summary>Nội dung ghi chú quan trọng gửi kèm email (map từ NV_Events_Cat.ContentMail).</summary>
    public string? ContentMail { get; set; }

    public IEnumerable<EventsViewModel> Events { get; set; } = [];

    /// <summary>Danh sách trường tham gia toàn sự kiện (parse từ FairSchool)</summary>
    public IEnumerable<TruongCardViewModel> Schools { get; set; } = [];

    /// <summary>Đơn vị tổ chức toàn sự kiện (parse từ FairOrg)</summary>
    public IEnumerable<OrgCardViewModel> Orgs { get; set; } = [];
}
