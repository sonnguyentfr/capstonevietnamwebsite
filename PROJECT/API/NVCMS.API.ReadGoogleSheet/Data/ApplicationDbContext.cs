using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Zalo_Token> Zalo_Token { get; set; }
        public DbSet<Zalo_Message_Log> Zalo_Message_Log { get; set; }

        public DbSet<ZnsTemplate> ZnsTemplates { get; set; }
        public DbSet<ZnsTemplateParam> ZnsTemplateParams { get; set; }
        public DbSet<ZnsTemplateButton> ZnsTemplateButtons { get; set; }
        public DbSet<ZnsSendLog> ZnsSendLogs { get; set; }
        public DbSet<ZnsSendQueue> ZnsSendQueues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ZnsTemplate>(e =>
            {
                e.ToTable("ZNS_Template");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.TemplateId).HasColumnName("TemplateId");
                e.Property(x => x.TemplateName).HasColumnName("TemplateName").HasMaxLength(500).IsRequired();
                e.Property(x => x.CreatedTime).HasColumnName("CreatedTime");
                e.Property(x => x.Status).HasColumnName("Status").HasMaxLength(100);
                e.Property(x => x.TemplateQuality).HasColumnName("TemplateQuality").HasMaxLength(100);
                e.Property(x => x.TemplateTag).HasColumnName("TemplateTag").HasMaxLength(100);
                e.Property(x => x.Timeout).HasColumnName("Timeout");
                e.Property(x => x.PreviewUrl).HasColumnName("PreviewUrl").HasMaxLength(1000);
                e.Property(x => x.Price).HasColumnName("Price").HasColumnType("decimal(18,2)");
                e.Property(x => x.PriceUid).HasColumnName("PriceUid").HasColumnType("decimal(18,2)");
                e.Property(x => x.PriceSdt).HasColumnName("PriceSdt").HasColumnType("decimal(18,2)");
                e.Property(x => x.ApplyTemplateQuota).HasColumnName("ApplyTemplateQuota");
                e.Property(x => x.Reason).HasColumnName("Reason").HasMaxLength(2000);
                e.Property(x => x.IsActive).HasColumnName("IsActive");
                e.Property(x => x.DetailJson).HasColumnName("DetailJson");
                e.Property(x => x.LastSyncedAt).HasColumnName("LastSyncedAt").HasColumnType("datetime");
                e.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                e.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");

                e.HasIndex(x => x.TemplateId).IsUnique();
                e.HasIndex(x => x.Status);
                e.HasIndex(x => x.IsActive);
                e.HasIndex(x => x.TemplateTag);
            });

            modelBuilder.Entity<ZnsTemplateParam>(e =>
            {
                e.ToTable("ZNS_Template_Param");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.ZnsTemplateId).HasColumnName("ZnsTemplateId");
                e.Property(x => x.ParamName).HasColumnName("ParamName").HasMaxLength(200).IsRequired();
                e.Property(x => x.IsRequired).HasColumnName("IsRequired");
                e.Property(x => x.ParamType).HasColumnName("ParamType").HasMaxLength(50).IsRequired();
                e.Property(x => x.MaxLength).HasColumnName("MaxLength");
                e.Property(x => x.MinLength).HasColumnName("MinLength");
                e.Property(x => x.AcceptNull).HasColumnName("AcceptNull");
                e.Property(x => x.SortOrder).HasColumnName("SortOrder");
                e.Property(x => x.DisplayName).HasColumnName("DisplayName").HasMaxLength(500);
                e.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                e.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");

                e.HasIndex(x => x.ZnsTemplateId);
                e.HasIndex(x => new { x.ZnsTemplateId, x.ParamName }).IsUnique();
                e.HasOne(x => x.Template)
                    .WithMany(t => t.Params)
                    .HasForeignKey(x => x.ZnsTemplateId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ZnsTemplateButton>(e =>
            {
                e.ToTable("ZNS_Template_Button");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.ZnsTemplateId).HasColumnName("ZnsTemplateId");
                e.Property(x => x.ButtonType).HasColumnName("ButtonType");
                e.Property(x => x.Title).HasColumnName("Title").HasMaxLength(500);
                e.Property(x => x.Content).HasColumnName("Content").HasMaxLength(2000);
                e.Property(x => x.SortOrder).HasColumnName("SortOrder");
                e.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                e.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");

                e.HasIndex(x => x.ZnsTemplateId);
                e.HasOne(x => x.Template)
                    .WithMany(t => t.Buttons)
                    .HasForeignKey(x => x.ZnsTemplateId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ZnsSendLog>(e =>
            {
                e.ToTable("ZNS_Send_Log");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.ZnsTemplateId).HasColumnName("ZnsTemplateId");
                e.Property(x => x.ZaloTemplateId).HasColumnName("ZaloTemplateId");
                e.Property(x => x.Phone).HasColumnName("Phone").HasMaxLength(30).IsRequired();
                e.Property(x => x.ParamsJson).HasColumnName("ParamsJson");
                e.Property(x => x.RequestJson).HasColumnName("RequestJson");
                e.Property(x => x.ResponseJson).HasColumnName("ResponseJson");
                e.Property(x => x.Status).HasColumnName("Status").HasMaxLength(50).IsRequired();
                e.Property(x => x.ZaloMessageId).HasColumnName("ZaloMessageId").HasMaxLength(200);
                e.Property(x => x.SentTime).HasColumnName("SentTime").HasColumnType("datetime");
                e.Property(x => x.SendingMode).HasColumnName("SendingMode").HasMaxLength(50);
                e.Property(x => x.RemainingQuota).HasColumnName("RemainingQuota");
                e.Property(x => x.DailyQuota).HasColumnName("DailyQuota");
                e.Property(x => x.ErrorCode).HasColumnName("ErrorCode");
                e.Property(x => x.ErrorMessage).HasColumnName("ErrorMessage").HasMaxLength(2000);
                e.Property(x => x.CampaignId).HasColumnName("CampaignId");
                e.Property(x => x.EventCatId).HasColumnName("EventCatId");
                e.Property(x => x.EventId).HasColumnName("EventId");
                e.Property(x => x.ContextType).HasColumnName("ContextType").HasMaxLength(100);
                e.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(200);
                e.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                e.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");

                e.HasIndex(x => x.ZaloTemplateId);
                e.HasIndex(x => x.Status);
                e.HasIndex(x => x.CreatedAt);
            });

            modelBuilder.Entity<ZnsSendQueue>(e =>
            {
                e.ToTable("ZNS_Send_Queue");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.TemplateId).HasColumnName("TemplateId");
                e.Property(x => x.Phone).HasColumnName("Phone").HasMaxLength(30).IsRequired();
                e.Property(x => x.TemplateDataJson).HasColumnName("TemplateDataJson").IsRequired();
                e.Property(x => x.Status).HasColumnName("Status").HasMaxLength(50).IsRequired();
                e.Property(x => x.RetryCount).HasColumnName("RetryCount");
                e.Property(x => x.ScheduledAt).HasColumnName("ScheduledAt").HasColumnType("datetime");
                e.Property(x => x.StartedAt).HasColumnName("StartedAt").HasColumnType("datetime");
                e.Property(x => x.CompletedAt).HasColumnName("CompletedAt").HasColumnType("datetime");
                e.Property(x => x.ErrorCode).HasColumnName("ErrorCode");
                e.Property(x => x.ErrorMessage).HasColumnName("ErrorMessage").HasMaxLength(2000);
                e.Property(x => x.MsgId).HasColumnName("MsgId").HasMaxLength(200);
                e.Property(x => x.CampaignId).HasColumnName("CampaignId");
                e.Property(x => x.EventCatId).HasColumnName("EventCatId");
                e.Property(x => x.EventId).HasColumnName("EventId");
                e.Property(x => x.ContextType).HasColumnName("ContextType").HasMaxLength(100);
                e.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(200);
                e.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                e.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");

                e.HasIndex(x => x.Status);
                e.HasIndex(x => x.CreatedAt);
            });
        }
    }
}