namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Template
    public class Marketing_Mail_Template
    {
        public int Id { get; set; }
        public string? TemplateName { get; set; }
        public string? FilePath { get; set; }
        public int? PortalId { get; set; }
    }
}
