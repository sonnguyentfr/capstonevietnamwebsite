namespace NVCMS.API.ReadGoogleSheet.Models
{
    // ── New flow DTOs ─────────────────────────────────────────────────────────

    public class SendCampaignBodyRequest
    {
        public int    CampaignId      { get; set; }
        public int TemplateId { get; set; }
        public string Subject         { get; set; } = string.Empty;
        public string Body            { get; set; } = string.Empty;
        public int    EmailAccountId  { get; set; }
    }

    public class SendCampaignResult
    {
        public bool   Success        { get; set; }
        public int    CampaignId     { get; set; }
        public int    TotalRecipient { get; set; }
    }

    // ── Campaign DTOs ─────────────────────────────────────────────────────────

    public class CreateCampaignRequest
    {
        public string  Title       { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int     PortalId    { get; set; }
        public int     UserId      { get; set; }
    }

    public class CampaignStatusResponse
    {
        public int     Id    { get; set; }
        public string  Title { get; set; } = string.Empty;
    }

    public class CampaignStatisticsResponse
    {
        public int    CampaignId        { get; set; }
        public string Title             { get; set; } = string.Empty;
        public string StatusLabel       { get; set; } = string.Empty;
        public int    TotalRecipients   { get; set; }
        public int    SentCount         { get; set; }
        public int    DeliveredCount    { get; set; }
        public int    OpenedCount       { get; set; }
        public int    ClickedCount      { get; set; }
        public int    BouncedCount      { get; set; }
        public int    ComplaintCount    { get; set; }
        public int    UnsubscribedCount { get; set; }
        public int    FailedCount       { get; set; }
        public double OpenRate  => TotalRecipients > 0 ? Math.Round((double)OpenedCount  / TotalRecipients * 100, 2) : 0;
        public double ClickRate => TotalRecipients > 0 ? Math.Round((double)ClickedCount / TotalRecipients * 100, 2) : 0;
        public double BounceRate => TotalRecipients > 0 ? Math.Round((double)BouncedCount / TotalRecipients * 100, 2) : 0;
    }

    // ── Gửi mail xác nhận cho danh sách NV_Events_Student (màn hình thống kê sự kiện) ──
    public class SendEventDetailStaticRequest
    {
        /// <summary>Danh sách NV_Events_Student.Id</summary>
        [System.ComponentModel.DataAnnotations.Required, System.ComponentModel.DataAnnotations.MinLength(1)]
        public List<int> Ids { get; set; } = [];

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int EventId { get; set; }

        [System.ComponentModel.DataAnnotations.Range(1, int.MaxValue)]
        public int EventCatId { get; set; }

        /// <summary>Marketing_Mail_Account.Id; 0 = dùng sender mặc định (SesSettings.FromEmail).</summary>
        public int EmailAccountId { get; set; }

        /// <summary>Để trống = "Xác nhận đăng ký tham dự - {CatName}".</summary>
        public string? Subject { get; set; }
    }

    public class SendEventDetailStaticSkipped
    {
        public int     EventStudentId { get; set; }
        public string? Email          { get; set; }
        public string  Reason         { get; set; } = string.Empty;
    }

    public class SendEventDetailStaticResult
    {
        public bool   Success        { get; set; }
        public string Message        { get; set; } = string.Empty;
        public int    CampaignSendId { get; set; }
        public int    TotalRequested { get; set; }
        public int    TotalRecipient { get; set; }
        public string? JobId         { get; set; }
        public List<SendEventDetailStaticSkipped> Skipped { get; set; } = [];
    }

    /// <summary>1 người nhận trong job: Send_Log.Id ↔ NV_Events_Student.Id.</summary>
    public class EventDetailStaticRecipient
    {
        public long LogId          { get; set; }
        public int  EventStudentId { get; set; }
        public int  StudentId      { get; set; }
    }
}
