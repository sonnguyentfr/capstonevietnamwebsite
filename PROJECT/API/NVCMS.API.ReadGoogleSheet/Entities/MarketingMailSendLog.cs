namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Send_Log
    public class MarketingMailSendLog
    {
        public int Id { get; set; }
        public int? CampaignSendId { get; set; }
        public int? ListMailId { get; set; }
        public string? Email { get; set; }
        public string? Status { get; set; }         // Queued | Sent | Failed
        public DateTime? SentTime { get; set; }
        public string? SesMessageId { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
