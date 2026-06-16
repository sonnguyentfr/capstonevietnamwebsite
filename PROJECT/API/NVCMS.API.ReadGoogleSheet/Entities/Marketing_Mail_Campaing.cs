using System.ComponentModel.DataAnnotations.Schema;

namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_Campaing (note: typo in table name preserved)
    public class Marketing_Mail_Campaing
    {
        // ── Columns that exist in DB ──────────────────────────────────────────
        public int id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UserId { get; set; }
        public int? PortalId { get; set; }

        // ── Extended columns (not in DB – runtime only) ───────────────────────
        [NotMapped] public string? Subject { get; set; }
        [NotMapped] public int? TemplateId { get; set; }
        [NotMapped] public int? Status { get; set; }
        [NotMapped] public DateTime? ScheduledAt { get; set; }
        [NotMapped] public DateTime? StartedAt { get; set; }
        [NotMapped] public DateTime? CompletedAt { get; set; }
        [NotMapped] public string? CreatedBy { get; set; }
        [NotMapped] public DateTime? UpdatedDate { get; set; }
    }
}
