using NVCMS.API.SendMail.Interfaces;
using NVCMS.API.SendMail.Repositories;
using NVCMS.API.SendMail.Services;
namespace NVCMS.API.SendMail.Worker
{
    public static class ServiceContainer
    {
        public static MailDispatchWorker CreateWorker()
        {
            return new MailDispatchWorker(
                new MailQueueRepository(),
                new MailKitSenderService(),
                new ExponentialBackoffRetryEngine(),
                new TokenBucketRateLimiter(),
                new CampaignRepository());
        }
        public static ICampaignQueueService CreateCampaignQueueService()
        {
            return new CampaignQueueService(
                new CampaignRepository(),
                new MailQueueRepository(),
                new UnsubscribeRepository());
        }
    }
}
