namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_ListMail
    public class Marketing_Mail_ListMail
    {
        public int id { get; set; }
        public int? CampaingId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public bool? Status { get; set; }
        public int? sendcount { get; set; }
        public int? RetryCount { get; set; }
        public int? RecipientStatus { get; set; }
        public string? MessageId { get; set; }
        public string? BounceReason { get; set; }
        public string? ComplaintReason { get; set; }
        public DateTime? Datetime { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClickedAt { get; set; }
        public int? UserId { get; set; }
        public int? PortalId { get; set; }
    }
}
