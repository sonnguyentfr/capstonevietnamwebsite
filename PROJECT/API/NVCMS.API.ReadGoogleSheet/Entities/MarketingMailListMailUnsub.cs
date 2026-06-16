namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_ListMail_Unsub
    public class MarketingMailListMailUnsub
    {
        public int id { get; set; }
        public string? Email { get; set; }
        public int? reason { get; set; }
        public Guid? Token { get; set; }
        public DateTime? created_date { get; set; }
        public int? PortalId { get; set; }
        public string? IPAddress { get; set; }
    }
}
