using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Data
{
    public class MarketingDbContext : DbContext
    {
        public MarketingDbContext(DbContextOptions<MarketingDbContext> options) : base(options) { }

        public DbSet<Marketing_Mail_Campaing>    Campaigns    { get; set; }
        public DbSet<Marketing_Mail_ListMail>    ListMails    { get; set; }
        public DbSet<MarketingMailListMailUnsub> Unsubs       { get; set; }
        public DbSet<Marketing_Mail_Template>    Templates    { get; set; }
        public DbSet<MarketingMailClick>         Clicks       { get; set; }
        public DbSet<MarketingMailHangfireLog>   HangfireLogs { get; set; }
        public DbSet<MarketingMailCampaignSend>  CampaignSends { get; set; }
        public DbSet<MarketingMailSendLog>       SendLogs     { get; set; }
        public DbSet<MarketingMailEvent>         Events       { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Marketing_Mail_Campaing>(e =>
            {
                e.ToTable("Marketing_Mail_Campaing");
                e.HasKey(x => x.id);
                e.Property(x => x.id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Title).HasColumnName("Title").HasMaxLength(500).IsRequired();
                e.Property(x => x.Description).HasColumnName("Description").HasMaxLength(500);
                e.Property(x => x.Subject).HasColumnName("Subject").HasMaxLength(500);
                e.Property(x => x.TemplateId).HasColumnName("TemplateId");
                e.Property(x => x.Status).HasColumnName("Status");
                e.Property(x => x.ScheduledAt).HasColumnName("ScheduledAt").HasColumnType("datetime");
                e.Property(x => x.StartedAt).HasColumnName("StartedAt").HasColumnType("datetime");
                e.Property(x => x.CompletedAt).HasColumnName("CompletedAt").HasColumnType("datetime");
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
                e.Property(x => x.UpdatedDate).HasColumnName("UpdatedDate").HasColumnType("datetime");
                e.Property(x => x.UserId).HasColumnName("UserId");
                e.Property(x => x.PortalId).HasColumnName("PortalId");
            });

            modelBuilder.Entity<Marketing_Mail_ListMail>(e =>
            {
                e.ToTable("Marketing_Mail_ListMail");
                e.HasKey(x => x.id);
                e.Property(x => x.id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.CampaingId).HasColumnName("CampaingId");
                e.Property(x => x.Email).HasColumnName("Email").HasMaxLength(100);
                e.Property(x => x.FullName).HasColumnName("FullName").HasMaxLength(255);
                e.Property(x => x.Status).HasColumnName("Status");
                e.Property(x => x.sendcount).HasColumnName("sendcount");
                e.Property(x => x.RetryCount).HasColumnName("RetryCount");
                e.Property(x => x.RecipientStatus).HasColumnName("RecipientStatus");
                e.Property(x => x.MessageId).HasColumnName("MessageId").HasMaxLength(255);
                e.Property(x => x.BounceReason).HasColumnName("BounceReason");
                e.Property(x => x.ComplaintReason).HasColumnName("ComplaintReason");
                e.Property(x => x.Datetime).HasColumnName("Datetime").HasColumnType("datetime");
                e.Property(x => x.SentAt).HasColumnName("SentAt").HasColumnType("datetime");
                e.Property(x => x.DeliveredAt).HasColumnName("DeliveredAt").HasColumnType("datetime");
                e.Property(x => x.OpenedAt).HasColumnName("OpenedAt").HasColumnType("datetime");
                e.Property(x => x.ClickedAt).HasColumnName("ClickedAt").HasColumnType("datetime");
                e.Property(x => x.UserId).HasColumnName("UserId");
                e.Property(x => x.PortalId).HasColumnName("PortalId");
            });

            modelBuilder.Entity<MarketingMailListMailUnsub>(e =>
            {
                e.ToTable("Marketing_Mail_ListMail_Unsub");
                e.HasKey(x => x.id);
                e.Property(x => x.id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.Email).HasColumnName("Email").HasMaxLength(500);
                e.Property(x => x.reason).HasColumnName("reason");
                e.Property(x => x.Token).HasColumnName("Token");
                e.Property(x => x.created_date).HasColumnName("created_date").HasColumnType("datetime");
                e.Property(x => x.PortalId).HasColumnName("PortalId");
                e.Property(x => x.IPAddress).HasColumnName("IPAddress").HasMaxLength(50);
            });

            modelBuilder.Entity<Marketing_Mail_Template>(e =>
            {
                e.ToTable("Marketing_Mail_Template");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.TemplateName).HasColumnName("TemplateName").HasMaxLength(500);
                e.Property(x => x.FilePath).HasColumnName("FilePath").HasMaxLength(500);
                e.Property(x => x.HtmlContent).HasColumnName("HtmlContent");
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
                e.Property(x => x.PortalId).HasColumnName("PortalId");
            });

            modelBuilder.Entity<MarketingMailClick>(e =>
            {
                e.ToTable("Marketing_Mail_Click");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.ListMailId).HasColumnName("ListMailId");
                e.Property(x => x.Url).HasColumnName("Url").HasMaxLength(2000);
                e.Property(x => x.ClickedAt).HasColumnName("ClickedAt").HasColumnType("datetime");
            });

            modelBuilder.Entity<MarketingMailHangfireLog>(e =>
            {
                e.ToTable("Marketing_Mail_HangfireLog");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.CampaignId).HasColumnName("CampaignId");
                e.Property(x => x.BatchNo).HasColumnName("BatchNo");
                e.Property(x => x.Status).HasColumnName("Status").HasMaxLength(50);
                e.Property(x => x.Message).HasColumnName("Message");
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
            });

            modelBuilder.Entity<MarketingMailCampaignSend>(e =>
            {
                e.ToTable("Marketing_Mail_Campaign_Send");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.CampaignId).HasColumnName("CampaignId");
                e.Property(x => x.Subject).HasColumnName("Subject").HasMaxLength(500);
                e.Property(x => x.Body).HasColumnName("Body");
                e.Property(x => x.Status).HasColumnName("Status").HasMaxLength(50);
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
                e.Property(x => x.StartedTime).HasColumnName("StartedTime").HasColumnType("datetime");
                e.Property(x => x.CompletedTime).HasColumnName("CompletedTime").HasColumnType("datetime");
                e.Property(x => x.TotalRecipient).HasColumnName("TotalRecipient");
                e.Property(x => x.TotalSent).HasColumnName("TotalSent");
                e.Property(x => x.TotalDelivered).HasColumnName("TotalDelivered");
                e.Property(x => x.TotalOpened).HasColumnName("TotalOpened");
                e.Property(x => x.TotalClicked).HasColumnName("TotalClicked");
                e.Property(x => x.TotalBounced).HasColumnName("TotalBounced");
                e.Property(x => x.TotalComplaint).HasColumnName("TotalComplaint");
            });

            modelBuilder.Entity<MarketingMailSendLog>(e =>
            {
                e.ToTable("Marketing_Mail_Send_Log");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.CampaignSendId).HasColumnName("CampaignSendId");
                e.Property(x => x.ListMailId).HasColumnName("ListMailId");
                e.Property(x => x.Email).HasColumnName("Email").HasMaxLength(255);
                e.Property(x => x.Status).HasColumnName("Status").HasMaxLength(50);
                e.Property(x => x.SentTime).HasColumnName("SentTime").HasColumnType("datetime");
                e.Property(x => x.SesMessageId).HasColumnName("SesMessageId").HasMaxLength(255);
                e.Property(x => x.ErrorMessage).HasColumnName("ErrorMessage");
            });

            modelBuilder.Entity<MarketingMailEvent>(e =>
            {
                e.ToTable("Marketing_Mail_Event");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.CampaignSendId).HasColumnName("CampaignSendId");
                e.Property(x => x.ListMailId).HasColumnName("ListMailId");
                e.Property(x => x.SesMessageId).HasColumnName("SesMessageId").HasMaxLength(255);
                e.Property(x => x.EventType).HasColumnName("EventType").HasMaxLength(50);
                e.Property(x => x.Payload).HasColumnName("Payload");
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
            });
        }
    }
}
