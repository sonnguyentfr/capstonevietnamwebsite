using NVCMS.Modules.Marketing;

namespace NVCMS.API.Marketing.Services.Marketing
{
    public class MarketingZaloReportServices
    {
        /// <summary>
        /// Dashboard phân tích chiến dịch Zalo ZNS theo CampaignId.
        /// Toàn bộ số liệu lấy từ sp_Marketing_Zalo_Campaign_Analytics.
        /// </summary>
        public static Marketing_Zalo_CampaignAnalyticsResult GetDashboard(int campaignId)
        {
            var service = new MarketingZaloAnalyticsService();

            return service.GetCampaignAnalytics(campaignId);
        }
    }
}
