using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingClickRepository : MarketingRepository<MarketingMailClick>, IMarketingClickRepository
    {
        public MarketingClickRepository(MarketingDbContext context) : base(context) { }

        public async Task<IEnumerable<MarketingMailClick>> GetByListMailIdAsync(int listMailId)
        {
            return await _dbSet
                .Where(c => c.ListMailId == listMailId)
                .OrderByDescending(c => c.ClickedAt)
                .ToListAsync();
        }
    }
}
