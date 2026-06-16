using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingSendLogRepository
        : MarketingRepository<MarketingMailSendLog>, IMarketingSendLogRepository
    {
        public MarketingSendLogRepository(MarketingDbContext context) : base(context) { }

        public async Task<IEnumerable<MarketingMailSendLog>> GetQueuedByCampaignSendIdAsync(int campaignSendId)
        {
            return await _dbSet
                .Where(l => l.CampaignSendId == campaignSendId && l.Status == "Queued")
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketingMailSendLog>> GetAllByCampaignSendIdAsync(int campaignSendId)
        {
            return await _dbSet
                .Where(l => l.CampaignSendId == campaignSendId)
                .OrderBy(l => l.Id)
                .ToListAsync();
        }

        public async Task<MarketingMailSendLog?> GetBySesMessageIdAsync(string sesMessageId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(l => l.SesMessageId == sesMessageId);
        }

        public async Task UpdateStatusAsync(int id, string status, string? sesMessageId = null, string? errorMessage = null)
        {
            var record = await _dbSet.FindAsync(id);
            if (record is null) return;

            record.Status = status;
            if (status == "Sent")
                record.SentTime = DateTime.UtcNow;
            if (sesMessageId is not null)
                record.SesMessageId = sesMessageId;
            if (errorMessage is not null)
                record.ErrorMessage = errorMessage;

            await _context.SaveChangesAsync();
        }
    }
}
