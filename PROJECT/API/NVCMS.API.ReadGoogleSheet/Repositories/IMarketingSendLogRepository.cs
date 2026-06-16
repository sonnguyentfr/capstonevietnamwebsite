using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingSendLogRepository : IRepository<MarketingMailSendLog>
    {
        Task<IEnumerable<MarketingMailSendLog>> GetQueuedByCampaignSendIdAsync(int campaignSendId);
        Task<IEnumerable<MarketingMailSendLog>> GetAllByCampaignSendIdAsync(int campaignSendId);
        Task<MarketingMailSendLog?> GetBySesMessageIdAsync(string sesMessageId);
        Task UpdateStatusAsync(int id, string status, string? sesMessageId = null, string? errorMessage = null);
    }
}
