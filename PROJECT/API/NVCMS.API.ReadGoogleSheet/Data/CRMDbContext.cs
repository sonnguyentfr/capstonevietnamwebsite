using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Data
{
    public class CRMDbContext : DbContext
    {
        public CRMDbContext(DbContextOptions<CRMDbContext> options) : base(options) { }

        public DbSet<Marketing_Mail_Account>      MailAccounts   { get; set; }
        public DbSet<Marketing_Mail_Campaing>    Campaigns      { get; set; }
        public DbSet<Marketing_Mail_ListMail>    ListMails      { get; set; }
        public DbSet<MarketingMailListMailUnsub> Unsubs         { get; set; }
        public DbSet<Marketing_Mail_Template>    Templates      { get; set; }
        public DbSet<MarketingMailSendLog>       SendLogs       { get; set; }
        public DbSet<MarketingMailCampaignSend>  CampaignSends  { get; set; }
        public DbSet<student_from_ladipage>      CrmDataLadings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Marketing_Mail_Account>(e =>
            {
                e.ToTable("Marketing_Mail_Account");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.Name).HasColumnName("Name").HasMaxLength(200);
                e.Property(x => x.Mail).HasColumnName("Mail").HasMaxLength(200);
            });

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

            modelBuilder.Entity<MarketingMailCampaignSend>(e =>
            {
                e.ToTable("Marketing_Mail_Campaign_Send");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.CampaignId).HasColumnName("CampaignId");
                e.Property(x => x.TemplateId).HasColumnName("TemplateId");
                e.Property(x => x.Subject).HasColumnName("Subject").HasMaxLength(500).IsRequired();
                e.Property(x => x.Body).HasColumnName("Body");
                e.Property(x => x.Status).HasColumnName("Status");
                e.Property(x => x.TotalRecipient).HasColumnName("TotalRecipient");
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
            });

            modelBuilder.Entity<student_from_ladipage>(entity =>
            {
                entity.ToTable("student_from_ladipage");
                entity.HasKey(e => e.id);
                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.hotendem).HasColumnName("hotendem").HasMaxLength(200);
                entity.Property(e => e.ten).HasColumnName("ten").HasMaxLength(50);
                entity.Property(e => e.gioi_tinh).HasColumnName("gioi_tinh");
                entity.Property(e => e.ngay_sinh).HasColumnName("ngay_sinh").HasColumnType("date");
                entity.Property(e => e.so_dien_thoai).HasColumnName("so_dien_thoai").HasMaxLength(30);
                entity.Property(e => e.email).HasColumnName("email").HasMaxLength(50);
                entity.Property(e => e.truong_dang_hoc).HasColumnName("truong_dang_hoc").HasMaxLength(500);
                entity.Property(e => e.event_dia_diem).HasColumnName("event_dia_diem").HasMaxLength(500);
                entity.Property(e => e.event_id).HasColumnName("event_id");
                entity.Property(e => e.event_dia_diem_id).HasColumnName("event_dia_diem_id");
                entity.Property(e => e.source).HasColumnName("source").HasMaxLength(500);
                entity.Property(e => e.medium).HasColumnName("medium").HasMaxLength(500);
                entity.Property(e => e.link).HasColumnName("link").HasMaxLength(500);
                entity.Property(e => e.ladi_page_id).HasColumnName("ladi_page_id").HasMaxLength(500);
                entity.Property(e => e.client_ip).HasColumnName("client_ip").HasMaxLength(50);
                entity.Property(e => e.thong_tin_khac).HasColumnName("thong_tin_khac").HasMaxLength(50);
                entity.Property(e => e.created_date).HasColumnName("created_date").HasColumnType("datetime");
            });
        }
    }
}
