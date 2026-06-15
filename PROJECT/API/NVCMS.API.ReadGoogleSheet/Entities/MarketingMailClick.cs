namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Click
    public class MarketingMailClick
    {
        public long Id { get; set; }
        public int ListMailId { get; set; }
        public string? Url { get; set; }
        public DateTime? ClickedAt { get; set; }
    }
}
