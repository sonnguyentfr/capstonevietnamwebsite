using System.ComponentModel.DataAnnotations.Schema;

namespace NVCMS.API.ReadGoogleSheet.Entities
{
    // Maps to dbo.Marketing_Mail_ListMail_Unsub
    public class MarketingMailListMailUnsub
    {
        // ── Columns that exist in DB ──────────────────────────────────────────
        public int id { get; set; }
        public string? Email { get; set; }
        public int? reason { get; set; }
        public DateTime? created_date { get; set; }
        public int? PortalId { get; set; }

        // ── Extended columns (not in DB – runtime only) ───────────────────────
        [NotMapped] public Guid? Token { get; set; }
        [NotMapped] public string? IPAddress { get; set; }
        [NotMapped] public string? UserAgent { get; set; }
    }
}
