namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_HangfireLog
    public class MarketingMailHangfireLog
    {
        public long Id { get; set; }
        public int? CampaignId { get; set; }
        public int? BatchNo { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
