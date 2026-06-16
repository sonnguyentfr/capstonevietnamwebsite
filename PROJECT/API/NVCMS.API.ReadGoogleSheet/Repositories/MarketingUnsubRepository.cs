using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingUnsubRepository : MarketingRepository<MarketingMailListMailUnsub>, IMarketingUnsubRepository
    {
        public MarketingUnsubRepository(MarketingDbContext context) : base(context) { }

        public async Task<bool> IsUnsubscribedAsync(string email, int portalId)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email && u.PortalId == portalId);
        }
    }
}
