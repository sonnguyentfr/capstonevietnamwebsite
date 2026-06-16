namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_ListMail
    public class Marketing_Mail_ListMail
    {
        public int       id         { get; set; }
        public int?      CampaingId { get; set; }
        public string?   Email      { get; set; }
        public bool?     Status     { get; set; }
        public int?      sendcount  { get; set; }
        public DateTime? Datetime   { get; set; }
        public int?      UserId     { get; set; }
        public int?      PortalId   { get; set; }
    }
}

