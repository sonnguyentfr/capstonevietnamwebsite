using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingCampaignRepository : MarketingRepository<Marketing_Mail_Campaing>, IMarketingCampaignRepository
    {
        public MarketingCampaignRepository(MarketingDbContext context) : base(context) { }

        public async Task<IEnumerable<Marketing_Mail_Campaing>> GetByPortalIdAsync(int portalId)
        {
            return await _dbSet
                .Where(c => c.PortalId == portalId)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Marketing_Mail_Campaing>> GetByStatusAsync(int status)
        {
            return await _dbSet
                .Where(c => c.Status == status)
                .OrderBy(c => c.ScheduledAt)
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int campaignId, int status, DateTime? timestamp = null)
        {
            var campaign = await _dbSet.FindAsync(campaignId);
            if (campaign is null) return;

            campaign.Status = status;
            campaign.UpdatedDate = DateTime.UtcNow;

            if (status == 2) campaign.StartedAt = timestamp ?? DateTime.UtcNow;
            if (status == 3 || status == 4) campaign.CompletedAt = timestamp ?? DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
