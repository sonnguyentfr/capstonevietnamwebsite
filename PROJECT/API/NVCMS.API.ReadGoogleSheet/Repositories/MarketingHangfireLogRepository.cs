using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingHangfireLogRepository : MarketingRepository<MarketingMailHangfireLog>, IMarketingHangfireLogRepository
    {
        public MarketingHangfireLogRepository(MarketingDbContext context) : base(context) { }

        public async Task<IEnumerable<MarketingMailHangfireLog>> GetByCampaignIdAsync(int campaignId)
        {
            return await _dbSet
                .Where(l => l.CampaignId == campaignId)
                .OrderByDescending(l => l.CreatedDate)
                .ToListAsync();
        }
    }
}
