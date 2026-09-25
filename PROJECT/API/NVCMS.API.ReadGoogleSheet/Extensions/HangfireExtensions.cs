using Hangfire;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models.Config;

namespace NVCMS.API.ReadGoogleSheet.Infrastructure
{
    public static class HangfireExtensions
    {
        public const string ImportCrmDataJobId = "import-crm-data";
        public const string CopyStudentFromLadiJobId = "copy-student-from-ladi";
        public const string ZaloOAWebhookReprocessJobId = "zalo-oa-webhook-reprocess";

        public static void RegisterRecurringJobs(this WebApplication app)
        {
            var settings = app.Services
                .GetRequiredService<IOptions<HangfireJobSettings>>()
                .Value;

            RegisterZnsRefreshToken(settings);
            RegisterZnsTemplateSync(settings);

            // ── Thay thế 2 job DNN Scheduler ─────────────────────────────────
            // Lưu ý thứ tự: ImportCrmData nạp dữ liệu vào student_from_ladipage,
            // CopyStudentFromLadi đọc bảng đó ra. Mặc định Import chạy phút 00,
            // Copy chạy phút 10 để dữ liệu vừa nạp được xử lý ngay trong cùng giờ.
            RegisterImportCrmData(settings);
            RegisterCopyStudentFromLadi(settings);

            //RegisterZaloOAWebhookReprocess(settings);
        }

        private static void RegisterZaloOAWebhookReprocess(HangfireJobSettings settings)
        {
            var cfg = settings.ZaloOAWebhookReprocess;

            if (!cfg.Enabled)
            {
                RecurringJob.RemoveIfExists(ZaloOAWebhookReprocessJobId);
                return;
            }

            RecurringJob.AddOrUpdate<ZaloOAWebhookReprocessJob>(
                ZaloOAWebhookReprocessJobId,
                x => x.ExecuteAsync(CancellationToken.None),
                cfg.Cron,
                new RecurringJobOptions
                {
                    TimeZone = ResolveTimeZone(cfg.TimeZone)
                });
        }

        private static void RegisterZnsRefreshToken(HangfireJobSettings settings)
        {
            if (!settings.ZnsRefreshToken.Enabled)
                return;

            RecurringJob.AddOrUpdate<ZnsRefreshTokenJob>(
                "zns-refresh-token",
                x => x.Execute(),
                settings.ZnsRefreshToken.Cron,
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                        settings.ZnsRefreshToken.TimeZone)
                });
        }

        private static void RegisterZnsTemplateSync(HangfireJobSettings settings)
        {
            if (!settings.ZnsTemplateSync.Enabled)
                return;

            RecurringJob.AddOrUpdate<ZnsTemplateSyncJob>(
                "zns-template-sync",
                x => x.Execute(CancellationToken.None),
                settings.ZnsTemplateSync.Cron,
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById(
                        settings.ZnsTemplateSync.TimeZone)
                });
        }

        private static void RegisterImportCrmData(HangfireJobSettings settings)
        {
            var cfg = settings.ImportCrmData;

            if (!cfg.Enabled)
            {
                // Gỡ khỏi Hangfire để tắt cờ trong appsettings là dừng hẳn job
                RecurringJob.RemoveIfExists(ImportCrmDataJobId);
                return;
            }

            RecurringJob.AddOrUpdate<ImportCrmDataJob>(
                ImportCrmDataJobId,
                x => x.Execute(CancellationToken.None),
                cfg.Cron,
                new RecurringJobOptions
                {
                    TimeZone = ResolveTimeZone(cfg.TimeZone)
                });
        }

        private static void RegisterCopyStudentFromLadi(HangfireJobSettings settings)
        {
            var cfg = settings.CopyStudentFromLadi;

            if (!cfg.Enabled)
            {
                RecurringJob.RemoveIfExists(CopyStudentFromLadiJobId);
                return;
            }

            RecurringJob.AddOrUpdate<CopyStudentFromLadiJob>(
                CopyStudentFromLadiJobId,
                x => x.Execute(CancellationToken.None),
                cfg.Cron,
                new RecurringJobOptions
                {
                    TimeZone = ResolveTimeZone(cfg.TimeZone)
                });
        }

        private static TimeZoneInfo ResolveTimeZone(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return TimeZoneInfo.Local;

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
                return TimeZoneInfo.Local;
            }
            catch (InvalidTimeZoneException)
            {
                return TimeZoneInfo.Local;
            }
        }
    }
}
