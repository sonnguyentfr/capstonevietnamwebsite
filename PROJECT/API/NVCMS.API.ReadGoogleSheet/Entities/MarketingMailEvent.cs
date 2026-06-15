namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Event
    public class MarketingMailEvent
    {
        public long Id { get; set; }
        public int? CampaignSendId { get; set; }
        public int? ListMailId { get; set; }
        public string? SesMessageId { get; set; }
        public string? EventType { get; set; }      // Delivery | Open | Click | Bounce | Complaint
        public string? Payload { get; set; }        // raw JSON từ SNS
        public DateTime? CreatedDate { get; set; }
    }
}
