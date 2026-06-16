using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingSendLogRepository : IRepository<MarketingMailSendLog>
    {
        Task<IEnumerable<MarketingMailSendLog>> GetQueuedByCampaignIdAsync(int campaignId);
        Task<IEnumerable<MarketingMailSendLog>> GetAllByCampaignIdAsync(int campaignId);
        Task<MarketingMailSendLog?> GetBySesMessageIdAsync(string sesMessageId);
        Task UpdateStatusAsync(long id, string status, string? sesMessageId = null, string? errorMessage = null);
    }
}
