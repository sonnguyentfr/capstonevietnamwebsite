using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingListMailRepository : MarketingRepository<Marketing_Mail_ListMail>, IMarketingListMailRepository
    {
        public MarketingListMailRepository(MarketingDbContext context) : base(context) { }

        public async Task<IEnumerable<Marketing_Mail_ListMail>> GetByCampaignIdAsync(int campaignId)
        {
            return await _dbSet
                .Where(l => l.CampaingId == campaignId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Marketing_Mail_ListMail>> GetPendingByCampaignIdAsync(int campaignId)
        {
            return await _dbSet
                .Where(l => l.CampaingId == campaignId && l.RecipientStatus == 0)
                .ToListAsync();
        }

        public async Task<Marketing_Mail_ListMail?> GetByMessageIdAsync(string messageId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(l => l.MessageId == messageId);
        }

        public async Task UpdateRecipientStatusAsync(int id, int status, DateTime? timestamp = null)
        {
            var record = await _dbSet.FindAsync(id);
            if (record is null) return;

            record.RecipientStatus = status;
            var now = timestamp ?? DateTime.UtcNow;

            switch (status)
            {
                case 1: record.SentAt      = now; break;
                case 2: record.DeliveredAt = now; break;
                case 3: record.OpenedAt    = now; break;
                case 4: record.ClickedAt   = now; break;
            }

            await _context.SaveChangesAsync();
        }

        public async Task IncrementSendCountAsync(int id)
        {
            var record = await _dbSet.FindAsync(id);
            if (record is null) return;

            record.sendcount = (record.sendcount ?? 0) + 1;
            record.RetryCount = (record.RetryCount ?? 0) + 1;
            await _context.SaveChangesAsync();
        }

        public async Task<(int Total, int Sent, int Failed)> GetCampaignCountsAsync(int campaignId)
        {
            var query = _dbSet.Where(l => l.CampaingId == campaignId);
            var total  = await query.CountAsync();
            var sent   = await query.CountAsync(l => l.RecipientStatus >= 1 && l.RecipientStatus <= 4);
            var failed = await query.CountAsync(l => l.RecipientStatus == 5 || l.RecipientStatus == 6);
            return (total, sent, failed);
        }
    }
}
