using System;
using System.ServiceProcess;
using Hangfire;

namespace NVCMS.API.SendMail.WindowsService
{
    public class SendMailService : ServiceBase
    {
        private BackgroundJobServer _hangfireServer;

        public SendMailService() { ServiceName="NVCMS.SendMail"; CanStop=true; CanPauseAndContinue=true; AutoLog=true; }

        protected override void OnStart(string[] args)
        {
            _hangfireServer = new BackgroundJobServer(new BackgroundJobServerOptions
            {
                WorkerCount = Config.AppConfig.MaxConcurrent
            });
        }

        protected override void OnStop()
        {
            _hangfireServer?.Dispose();
        }
    }
}
