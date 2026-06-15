using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingCampaignSendRepository
        : MarketingRepository<MarketingMailCampaignSend>, IMarketingCampaignSendRepository
    {
        public MarketingCampaignSendRepository(MarketingDbContext context) : base(context) { }

        public new async Task<MarketingMailCampaignSend?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task UpdateStatusAsync(int id, string status)
        {
            var record = await _dbSet.FindAsync(id);
            if (record is null) return;
            record.Status = status;
            await _context.SaveChangesAsync();
        }

        public async Task IncrementCounterAsync(int id, string counterName)
        {
            var record = await _dbSet.FindAsync(id);
            if (record is null) return;

            switch (counterName)
            {
                case "TotalSent":      record.TotalSent      = (record.TotalSent      ?? 0) + 1; break;
                case "TotalDelivered": record.TotalDelivered = (record.TotalDelivered ?? 0) + 1; break;
                case "TotalOpened":   record.TotalOpened    = (record.TotalOpened    ?? 0) + 1; break;
                case "TotalClicked":  record.TotalClicked   = (record.TotalClicked   ?? 0) + 1; break;
                case "TotalBounced":  record.TotalBounced   = (record.TotalBounced   ?? 0) + 1; break;
                case "TotalComplaint":record.TotalComplaint = (record.TotalComplaint ?? 0) + 1; break;
            }
            await _context.SaveChangesAsync();
        }
    }
}
