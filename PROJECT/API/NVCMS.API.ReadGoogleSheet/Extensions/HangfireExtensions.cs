using Hangfire;
using Microsoft.Extensions.Options;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models.Config;

namespace NVCMS.API.ReadGoogleSheet.Infrastructure
{
    public static class HangfireExtensions
    {
        public static void RegisterRecurringJobs(this WebApplication app)
        {
            var settings = app.Services
                .GetRequiredService<IOptions<HangfireJobSettings>>()
                .Value;

            RegisterZnsRefreshToken(settings);
            RegisterZnsTemplateSync(settings);
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
    }
}