namespace NVCMS.API.ReadGoogleSheet.Models
{
    // BIRTHDAY-MAIL 02/11/2026: dòng dữ liệu đọc từ proc NVCore_BirthdayMail_* (DB CRM, Dapper map theo tên cột).

    public class BirthdayMailConfigRow
    {
        public int PortalId { get; set; }
        public bool Enabled { get; set; }
        public string SendTime { get; set; } = "08:00";
        public int? TemplateId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
        public string BccEmail { get; set; } = string.Empty;
        public string HeroImageUrl { get; set; } = string.Empty;
        public bool OnlyConsented { get; set; }
        public int MaxPerDay { get; set; } = 2000;
    }

    public class BirthdayMailTemplateRow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string BodyHtml { get; set; } = string.Empty;
    }

    /// <summary>ItemType: 1 Tin tức · 2 Sự kiện.</summary>
    public class BirthdayMailItemRow
    {
        public int Id { get; set; }
        public byte ItemType { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string MetaText { get; set; } = string.Empty;
        public string ButtonText { get; set; } = string.Empty;
        public int SortOrder { get; set; }
    }

    public class BirthdayRecipientRow
    {
        public int StudentId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Hotendem { get; set; } = string.Empty;
        public string Ten { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public bool? Sex { get; set; }
        public DateTime? BirthDate { get; set; }
        public int Age { get; set; }
    }

    public class BirthdayRunBeginRow
    {
        public int RunId { get; set; }
        /// <summary>new · resume · done · busy</summary>
        public string Action { get; set; } = string.Empty;
    }

    public class BirthdayRunInfoRow
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public int Total { get; set; }
        public int Sent { get; set; }
        public int Failed { get; set; }
    }

    /// <summary>Kết quả 1 lượt gửi (job / gửi ngay).</summary>
    public class BirthdayRunResult
    {
        public int PortalId { get; set; }
        public int RunId { get; set; }
        public string Action { get; set; } = string.Empty;
        public int Candidates { get; set; }
        public int Sent { get; set; }
        public int Failed { get; set; }
        public int Skipped { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    // ── Yêu cầu từ CRM (JWT) ────────────────────────────────────────────────

    public class BirthdayPreviewRequest
    {
        public int PortalId { get; set; }
        /// <summary>&gt; 0: lấy mẫu trong DB. 0 + BodyHtml trống: thiệp mặc định. BodyHtml có nội dung: xem trước bản đang soạn.</summary>
        public int TemplateId { get; set; }
        public string? Subject { get; set; }
        public string? BodyHtml { get; set; }
        /// <summary>Tên hiển thị thử (trống = "Nguyễn Minh Anh").</summary>
        public string? FullName { get; set; }
    }

    public class BirthdayTestRequest : BirthdayPreviewRequest
    {
        public string Email { get; set; } = string.Empty;
        public string? RequestedBy { get; set; }
    }

    public class BirthdayRunNowRequest
    {
        public int PortalId { get; set; }
        public string? TriggeredBy { get; set; }
    }
}
