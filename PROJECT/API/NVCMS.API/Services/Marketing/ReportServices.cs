using NVCMS.Modules.Marketing;
namespace NVCMS.API.Marketing.Services.Marketing
{
    public class MarketingReportServices
    {
        public static Marketing_Mail_CampaignAnalyticsResult GetDashboard(int campaignSendId)
        {
            var controller = new MarketingMailAnalyticsService();

            return controller.GetCampaignAnalytics(campaignSendId);
        }
    
    }
}
