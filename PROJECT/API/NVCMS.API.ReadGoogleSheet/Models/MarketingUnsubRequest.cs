namespace NVCMS.API.ReadGoogleSheet.Models
{
    public class UnsubscribeRequest
    {
        public string Email { get; set; } = string.Empty;
        public Guid Token { get; set; }
        public int? Reason { get; set; }            // 1=NotInterested 2=TooFrequent 3=NeverSignedUp 4=Other
        public int PortalId { get; set; }
    }
}
