namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Campaing (note: typo in table name preserved)
    public class Marketing_Mail_Campaing
    {
        public int id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Subject { get; set; }
        public int? TemplateId { get; set; }
        public int? Status { get; set; }
        public DateTime? ScheduledAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UserId { get; set; }
        public int? PortalId { get; set; }
    }
}
