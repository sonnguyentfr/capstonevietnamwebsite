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

        
    }
}