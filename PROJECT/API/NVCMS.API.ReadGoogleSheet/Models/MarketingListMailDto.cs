namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class AddRecipientsRequest
    {
        public int CampaignId { get; set; }
        public int PortalId { get; set; }
        public int UserId { get; set; }
        public List<RecipientItem> Recipients { get; set; } = [];
    }

    public class RecipientItem
    {
        public string Email { get; set; } = string.Empty;
        public string? FullName { get; set; }
    }

    public class RecipientStatusResponse
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public int? RecipientStatus { get; set; }
        public string StatusLabel => RecipientStatus switch
        {
            0 => "Pending",
            1 => "Sent",
            2 => "Delivered",
            3 => "Opened",
            4 => "Clicked",
            5 => "Bounced",
            6 => "Complaint",
            7 => "Unsubscribed",
            _ => "Unknown"
        };
        public DateTime? SentAt { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? ClickedAt { get; set; }
        public string? BounceReason { get; set; }
    }
}
