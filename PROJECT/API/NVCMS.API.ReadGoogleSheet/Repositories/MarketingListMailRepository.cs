using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingListMailRepository
        : MarketingRepository<Marketing_Mail_ListMail>, IMarketingListMailRepository
    {
        public MarketingListMailRepository(CRMDbContext context) : base(context) { }

        public async Task<IEnumerable<Marketing_Mail_ListMail>> GetByCampaignIdAsync(int campaignId)
        {
            return await _dbSet
                .Where(l => l.CampaingId == campaignId)
                .ToListAsync();
        }
    }
}
