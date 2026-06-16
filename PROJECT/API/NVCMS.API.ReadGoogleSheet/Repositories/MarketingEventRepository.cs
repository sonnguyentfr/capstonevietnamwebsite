using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingEventRepository : MarketingRepository<MarketingMailEvent>, IMarketingEventRepository
    {
        public MarketingEventRepository(MarketingDbContext context) : base(context) { }

        public async Task<IEnumerable<MarketingMailEvent>> GetByListMailIdAsync(int listMailId)
        {
            return await _dbSet
                .Where(e => e.ListMailId == listMailId)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketingMailEvent>> GetByCampaignSendIdAsync(int campaignSendId)
        {
            return await _dbSet
                .Where(e => e.CampaignSendId == campaignSendId)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketingMailEvent>> GetByEventTypeAsync(string eventType)
        {
            return await _dbSet
                .Where(e => e.EventType == eventType)
                .OrderByDescending(e => e.CreatedDate)
                .ToListAsync();
        }
    }
}
