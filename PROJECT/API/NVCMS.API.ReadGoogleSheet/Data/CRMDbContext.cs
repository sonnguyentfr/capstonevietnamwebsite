using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models;

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

        public DbSet<ZnsTemplate>                ZnsTemplates      { get; set; }
        public DbSet<ZnsTemplateParam>           ZnsTemplateParams  { get; set; }
        public DbSet<ZnsTemplateButton>          ZnsTemplateButtons { get; set; }
        public DbSet<Zalo_Message_Log>           ZaloMessageLogs    { get; set; }
        public DbSet<ZnsSendLog>                 ZnsSendLogs        { get; set; }
        public DbSet<ZnsSendQueue>               ZnsSendQueues      { get; set; }
        public DbSet<Marketing_Zalo_ListSdt>     MarketingZaloListSdts { get; set; }
        public DbSet<NV_Event>                   NV_Events          { get; set; }
        public DbSet<NV_Events_Cat>              NV_EventsCats      { get; set; }
        public DbSet<Student_Info>               StudentInfos       { get; set; }
        public DbSet<NV_Events_Student>          NV_EventsStudents  { get; set; }

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
                e.Property(x => x.SenderEmailId).HasColumnName("SenderEmailId");
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
                e.HasOne(x => x.Template).WithMany(t => t.Params).HasForeignKey(x => x.ZnsTemplateId).OnDelete(DeleteBehavior.Cascade);
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
                e.HasOne(x => x.Template).WithMany(t => t.Buttons).HasForeignKey(x => x.ZnsTemplateId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Zalo_Message_Log>(e =>
            {
                e.ToTable("Zalo_Message_Log");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.Phone).HasColumnName("Phone").HasMaxLength(30);
                e.Property(x => x.FullName).HasColumnName("FullName").HasMaxLength(200);
                e.Property(x => x.TemplateId).HasColumnName("TemplateId");
                e.Property(x => x.TrackingId).HasColumnName("TrackingId").HasMaxLength(200);
                e.Property(x => x.Status).HasColumnName("Status");
                e.Property(x => x.Message).HasColumnName("Message").HasMaxLength(2000);
                e.Property(x => x.RequestJson).HasColumnName("RequestJson");
                e.Property(x => x.ResponseJson).HasColumnName("ResponseJson");
                e.Property(x => x.CreatedTime).HasColumnName("CreatedTime").HasColumnType("datetime");
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
                e.Property(x => x.Type).HasColumnName("Type").HasMaxLength(50);
                e.Property(x => x.CampaignId).HasColumnName("CampaignId");
                e.Property(x => x.EventCatId).HasColumnName("EventCatId");
                e.Property(x => x.EventId).HasColumnName("EventId");
                e.Property(x => x.ContextType).HasColumnName("ContextType").HasMaxLength(100);
                e.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(200);
                e.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                e.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");
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
                e.Ignore(x => x.Type);
                e.Property(x => x.CampaignId).HasColumnName("CampaignId");
                e.Property(x => x.EventCatId).HasColumnName("EventCatId");
                e.Property(x => x.EventId).HasColumnName("EventId");
                e.Property(x => x.ContextType).HasColumnName("ContextType").HasMaxLength(100);
                e.Property(x => x.CreatedBy).HasColumnName("CreatedBy").HasMaxLength(200);
                e.Property(x => x.CreatedAt).HasColumnName("CreatedAt").HasColumnType("datetime");
                e.Property(x => x.UpdatedAt).HasColumnName("UpdatedAt").HasColumnType("datetime");
            });

            modelBuilder.Entity<Marketing_Zalo_ListSdt>(e =>
            {
                e.ToTable("Marketing_Zalo_ListSdt");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id").UseIdentityColumn();
                e.Property(x => x.Marketing_Zalo_CampaignId).HasColumnName("Marketing_Zalo_CampaignId");
                e.Property(x => x.PhoneRaw).HasColumnName("PhoneRaw").HasMaxLength(20);
                e.Property(x => x.Phone).HasColumnName("Phone").HasMaxLength(20).IsRequired();
                e.Property(x => x.Status).HasColumnName("Status");
                e.Property(x => x.SendCount).HasColumnName("SendCount");
                e.Property(x => x.CreatedDate).HasColumnName("CreatedDate").HasColumnType("datetime");
                e.Property(x => x.UserId).HasColumnName("UserId");
                e.Property(x => x.PortalId).HasColumnName("PortalId");
            });

            modelBuilder.Entity<NV_Event>(e =>
            {
                e.ToTable("NV_Events");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.Title).HasColumnName("Title").HasMaxLength(500);
                e.Property(x => x.Diadiem).HasColumnName("Diadiem").HasMaxLength(1000);
                e.Property(x => x.Fromdatetime).HasColumnName("Fromdatetime").HasColumnType("datetime");
                e.Property(x => x.Enddatetime).HasColumnName("Enddatetime").HasColumnType("datetime");
                e.Property(x => x.CatId).HasColumnName("CatId");
                e.Property(x => x.Isactive).HasColumnName("Isactive");
                e.Property(x => x.Portalid).HasColumnName("Portalid");
            });

            modelBuilder.Entity<NV_Events_Cat>(e =>
            {
                e.ToTable("NV_Events_Cat");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.CatName).HasColumnName("CatName").HasMaxLength(500);
                e.Property(x => x.FairOrg).HasColumnName("FairOrg").HasMaxLength(500);
                e.Property(x => x.DateShow).HasColumnName("DateShow").HasMaxLength(100);
                e.Property(x => x.Desception).HasColumnName("Desception").HasMaxLength(2000);
                e.Property(x => x.sendzalo_content).HasColumnName("sendzalo_content").HasMaxLength(200);
                e.Property(x => x.ContentMail).HasColumnName("ContentMail").HasMaxLength(4000);
                e.Property(x => x.FairDiengia).HasColumnName("FairDiengia").HasMaxLength(4000);
                e.Property(x => x.FromDate).HasColumnName("FromDate").HasColumnType("datetime");
                e.Property(x => x.EndDate).HasColumnName("EndDate").HasColumnType("datetime");
                e.Property(x => x.Isactive).HasColumnName("Isactive");
                e.Property(x => x.PortalId).HasColumnName("PortalId");
                e.Property(x => x.is_show_website).HasColumnName("is_show_website");
                e.Property(x => x.Link_pr).HasColumnName("Link_pr");
            });

            modelBuilder.Entity<NV_Events_Student>(e =>
            {
                e.ToTable("NV_Events_Student");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.EventId).HasColumnName("EventId");
                e.Property(x => x.EventCatId).HasColumnName("EventCatId");
                e.Property(x => x.StudentId).HasColumnName("StudentId");
                e.Property(x => x.StudentCode).HasColumnName("StudentCode");
            });

            modelBuilder.Entity<Student_Info>(e =>
            {
                e.ToTable("Student_Info");
                e.HasKey(x => x.Id);
                e.Property(x => x.Id).HasColumnName("Id");
                e.Property(x => x.Code).HasColumnName("Code").HasMaxLength(100);
                e.Property(x => x.Hotendem).HasColumnName("Hotendem").HasMaxLength(200);
                e.Property(x => x.Ten).HasColumnName("Ten").HasMaxLength(100);
                e.Property(x => x.Sodienthoai).HasColumnName("Sodienthoai").HasMaxLength(30);
                e.Property(x => x.PortalId).HasColumnName("PortalId");
            });

            modelBuilder.Entity<student_from_ladipage>(e =>
            {
                // Thiếu mapping này EF lấy tên bảng theo tên DbSet ("CrmDataLadings")
                // → SqlException: Invalid object name 'CrmDataLadings'.
                e.ToTable("student_from_ladipage");
                e.HasKey(x => x.id);
                e.Property(x => x.id).HasColumnName("id").UseIdentityColumn();
                e.Property(x => x.hotendem).HasColumnName("hotendem").HasMaxLength(200);
                e.Property(x => x.ten).HasColumnName("ten").HasMaxLength(50);
                e.Property(x => x.gioi_tinh).HasColumnName("gioi_tinh");
                e.Property(x => x.ngay_sinh).HasColumnName("ngay_sinh").HasColumnType("date");
                e.Property(x => x.so_dien_thoai).HasColumnName("so_dien_thoai").HasMaxLength(30);
                e.Property(x => x.email).HasColumnName("email").HasMaxLength(50);
                e.Property(x => x.truong_dang_hoc).HasColumnName("truong_dang_hoc").HasMaxLength(500);
                e.Property(x => x.event_dia_diem).HasColumnName("event_dia_diem").HasMaxLength(500);
                e.Property(x => x.event_id).HasColumnName("event_id");
                e.Property(x => x.event_dia_diem_id).HasColumnName("event_dia_diem_id");
                e.Property(x => x.source).HasColumnName("source").HasMaxLength(500);
                e.Property(x => x.medium).HasColumnName("medium").HasMaxLength(500);
                e.Property(x => x.link).HasColumnName("link").HasMaxLength(500);
                e.Property(x => x.ladi_page_id).HasColumnName("ladi_page_id").HasMaxLength(500);
                e.Property(x => x.client_ip).HasColumnName("client_ip").HasMaxLength(50);
                e.Property(x => x.thong_tin_khac).HasColumnName("thong_tin_khac");
                e.Property(x => x.is_update_crm).HasColumnName("is_update_crm");
                e.Property(x => x.created_date).HasColumnName("created_date").HasColumnType("datetime");
            });
        }
    }
}
