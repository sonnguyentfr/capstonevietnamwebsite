using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingSendLogRepository
        : MarketingRepository<MarketingMailSendLog>, IMarketingSendLogRepository
    {
        public MarketingSendLogRepository(CRMDbContext context) : base(context) { }

        public async Task<IEnumerable<MarketingMailSendLog>> GetQueuedByCampaignIdAsync(int campaignId)
        {
            return await _dbSet
                .Where(l => l.CampaignSendId == campaignId
                         && l.Status == MailSendStatus.Queued)
                .ToListAsync();
        }

        public async Task<IEnumerable<MarketingMailSendLog>> GetAllByCampaignIdAsync(int campaignId)
        {
            return await _dbSet
                .Where(l => l.CampaignSendId == campaignId)
                .OrderBy(l => l.Id)
                .ToListAsync();
        }

        public async Task<MarketingMailSendLog?> GetBySesMessageIdAsync(string sesMessageId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(l => l.SesMessageId == sesMessageId);
        }

        public async Task UpdateStatusAsync(long id, string status,
            string? sesMessageId = null, string? errorMessage = null)
        {
            var record = await _dbSet.FindAsync(id);
            if (record is null) return;

            record.Status = status;

            var now = DateTime.UtcNow;
            switch (status)
            {
                case MailSendStatus.Sent:       record.SentTime       = now; break;
                case MailSendStatus.Delivered:  record.DeliveredTime  = now; break;
                case MailSendStatus.Opened:     record.OpenedTime     = now; break;
                case MailSendStatus.Clicked:    record.ClickedTime    = now; break;
            }

            if (sesMessageId is not null) record.SesMessageId = sesMessageId;
            if (errorMessage  is not null) record.ErrorMessage  = errorMessage;

            await _context.SaveChangesAsync();
        }
    }
}
