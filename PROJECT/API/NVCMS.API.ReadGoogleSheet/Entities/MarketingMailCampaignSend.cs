namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Campaign_Send
    public class MarketingMailCampaignSend
    {
        public int Id { get; set; }
        public int? CampaignId { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Status { get; set; }         // Queued | Processing | Completed | Failed
        public DateTime? CreatedDate { get; set; }
        public DateTime? StartedTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public int? TotalRecipient { get; set; }
        public int? TotalSent { get; set; }
        public int? TotalDelivered { get; set; }
        public int? TotalOpened { get; set; }
        public int? TotalClicked { get; set; }
        public int? TotalBounced { get; set; }
        public int? TotalComplaint { get; set; }
    }
}
