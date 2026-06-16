using System.ComponentModel.DataAnnotations.Schema;

namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_ListMail
    public class Marketing_Mail_ListMail
    {
        // ── Columns that exist in DB ──────────────────────────────────────────
        public int id { get; set; }
        public int? CampaingId { get; set; }
        public string? Email { get; set; }
        public bool? Status { get; set; }
        public int? sendcount { get; set; }
        public DateTime? Datetime { get; set; }
        public int? UserId { get; set; }
        public int? PortalId { get; set; }

        // ── Extended columns (not in DB – runtime only) ───────────────────────
        [NotMapped] public string? FullName { get; set; }
        [NotMapped] public string? MessageId { get; set; }
        [NotMapped] public DateTime? SentAt { get; set; }
        [NotMapped] public DateTime? DeliveredAt { get; set; }
        [NotMapped] public DateTime? OpenedAt { get; set; }
        [NotMapped] public DateTime? ClickedAt { get; set; }
        [NotMapped] public string? BounceReason { get; set; }
        [NotMapped] public string? ComplaintReason { get; set; }
        [NotMapped] public int? RetryCount { get; set; }
        [NotMapped] public int? RecipientStatus { get; set; }
    }
}
