using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Data
{
    public class MarketingDbContext : DbContext
    {
        public MarketingDbContext(DbContextOptions<MarketingDbContext> options) : base(options) { }

        public DbSet<Marketing_Mail_Campaing>    Campaigns { get; set; }
        public DbSet<Marketing_Mail_ListMail>    ListMails { get; set; }
        public DbSet<MarketingMailListMailUnsub> Unsubs    { get; set; }
        public DbSet<Marketing_Mail_Template>    Templates { get; set; }
        public DbSet<MarketingMailSendLog>       SendLogs  { get; set; }

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
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
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
                e.Property(x => x.Status).HasColumnName("Status");
                e.Property(x => x.sendcount).HasColumnName("sendcount");
                e.Property(x => x.Datetime).HasColumnName("Datetime").HasColumnType("datetime");
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
                e.Property(x => x.created_date).HasColumnName("created_date").HasColumnType("datetime");
                e.Property(x => x.PortalId).HasColumnName("PortalId");
            });

            modelBuilder.Entity<Marketing_Mail_Template>(e =>
            {
                e.ToTable("Marketing_Mail_Template");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.TemplateName).HasColumnName("TemplateName").HasMaxLength(500);
                e.Property(x => x.FilePath).HasColumnName("FilePath").HasMaxLength(50);
                e.Property(x => x.PortalId).HasColumnName("PortalId");
            });

            modelBuilder.Entity<MarketingMailSendLog>(e =>
            {
                e.ToTable("Marketing_Mail_Send_Log");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.CampaignSendId).HasColumnName("CampaignSendId");
                e.Property(x => x.ListMailId).HasColumnName("ListMailId");
                e.Property(x => x.Email).HasColumnName("Email").HasMaxLength(200).IsRequired();
                e.Property(x => x.SesMessageId).HasColumnName("SesMessageId").HasMaxLength(200);
                e.Property(x => x.Status).HasColumnName("Status").HasMaxLength(100).IsRequired();
                e.Property(x => x.ErrorMessage).HasColumnName("ErrorMessage").HasMaxLength(1000);
                e.Property(x => x.SentTime).HasColumnName("SentTime").HasColumnType("datetime");
                e.Property(x => x.DeliveredTime).HasColumnName("DeliveredTime").HasColumnType("datetime");
                e.Property(x => x.OpenedTime).HasColumnName("OpenedTime").HasColumnType("datetime");
                e.Property(x => x.ClickedTime).HasColumnName("ClickedTime").HasColumnType("datetime");
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
            });
        }
    }
}
