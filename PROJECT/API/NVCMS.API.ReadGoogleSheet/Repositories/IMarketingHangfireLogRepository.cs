using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingHangfireLogRepository : IRepository<MarketingMailHangfireLog>
    {
        Task<IEnumerable<MarketingMailHangfireLog>> GetByCampaignIdAsync(int campaignId);
    }
}
