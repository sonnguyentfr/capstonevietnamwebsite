using System;
using System.ServiceProcess;
using Hangfire;
using Hangfire.SqlServer;
using NVCMS.API.SendMail.Config;
using NVCMS.API.SendMail.Jobs;
using NVCMS.API.SendMail.WindowsService;
using NVCMS.API.SendMail.Worker;

namespace NVCMS.API.SendMail
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Cấu hình Hangfire với SQL Server
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(AppConfig.ConnectionString, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout       = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout   = TimeSpan.FromMinutes(5),
                    QueuePollInterval            = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks           = true
                });

            // Job recurring mỗi 1 phút quét queue và enqueue
            RecurringJob.AddOrUpdate<MailSendJob>(
                "enqueue-pending-mails",
                j => j.EnqueuePendingMailsAsync(),
                Cron.Minutely);

            bool console = Array.IndexOf(args, "--console") >= 0 || Environment.UserInteractive;
            if (console)
            {
                Console.Title = "NVCMS.API.SendMail (Hangfire)";
                Console.WriteLine("Press [Q] to quit...");

                using (var server = new BackgroundJobServer(new BackgroundJobServerOptions
                {
                    WorkerCount = AppConfig.MaxConcurrent
                }))
                {
                    Console.WriteLine("Hangfire Server started.");
                    while (Console.ReadKey(true).Key != ConsoleKey.Q) { }
                }
            }
            else
            {
                ServiceBase.Run(new SendMailService());
            }
        }
    }
}
