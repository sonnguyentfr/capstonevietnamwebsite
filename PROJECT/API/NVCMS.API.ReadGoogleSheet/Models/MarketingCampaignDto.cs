namespace NVCMS.API.ReadGoogleSheet.Models
{
    // ── New flow DTOs ─────────────────────────────────────────────────────────

    public class SendCampaignBodyRequest
    {
        public int CampaignId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Body    { get; set; } = string.Empty;
    }

    public class SendCampaignResult
    {
        public bool Success          { get; set; }
        public int  CampaignSendId   { get; set; }
        public int  TotalRecipient   { get; set; }
    }

    // ── Legacy DTOs (kept for backward-compat) ────────────────────────────────

    public class CreateCampaignRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Subject { get; set; } = string.Empty;
        public int PortalId { get; set; }
        public int UserId { get; set; }
        public int? TemplateId { get; set; }
        public DateTime? ScheduledAt { get; set; }
    }

    public class CampaignStatusResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public int? Status { get; set; }
        public string StatusLabel => Status switch
        {
            0 => "Draft",
            1 => "Queued",
            2 => "Sending",
            3 => "Completed",
            4 => "Failed",
            5 => "Paused",
            _ => "Unknown"
        };
        public int TotalRecipients { get; set; }
        public int SentCount { get; set; }
        public int FailedCount { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }

    public class UpdateCampaignStatusRequest
    {
        public int CampaignId { get; set; }
        public int Status { get; set; }
    }

    public class CampaignStatisticsResponse
    {
        public int CampaignId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string StatusLabel { get; set; } = string.Empty;
        public int TotalRecipients { get; set; }
        public int SentCount { get; set; }
        public int DeliveredCount { get; set; }
        public int OpenedCount { get; set; }
        public int ClickedCount { get; set; }
        public int BouncedCount { get; set; }
        public int ComplaintCount { get; set; }
        public int UnsubscribedCount { get; set; }
        public int FailedCount { get; set; }
        public double OpenRate => TotalRecipients > 0 ? Math.Round((double)OpenedCount / TotalRecipients * 100, 2) : 0;
        public double ClickRate => TotalRecipients > 0 ? Math.Round((double)ClickedCount / TotalRecipients * 100, 2) : 0;
        public double BounceRate => TotalRecipients > 0 ? Math.Round((double)BouncedCount / TotalRecipients * 100, 2) : 0;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
