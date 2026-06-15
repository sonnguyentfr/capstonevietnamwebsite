using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingCampaignSendRepository : IRepository<MarketingMailCampaignSend>
    {
        Task<MarketingMailCampaignSend?> GetByIdAsync(int id);
        Task UpdateStatusAsync(int id, string status);
        Task IncrementCounterAsync(int id, string counterName);
    }
}
