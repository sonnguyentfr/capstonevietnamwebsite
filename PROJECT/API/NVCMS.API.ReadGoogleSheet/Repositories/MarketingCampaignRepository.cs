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

        /// <summary>
        /// DB không có cột Status trên bảng Marketing_Mail_Campaing.
        /// Trả về tất cả campaign theo portalId — caller tự lọc theo nghiệp vụ.
        /// </summary>
        public async Task<IEnumerable<Marketing_Mail_Campaing>> GetByStatusAsync(int status)
        {
            // Bảng Marketing_Mail_Campaing không có cột Status.
            // Trả toàn bộ để CampaignSchedulerJob tự quyết định qua CampaignSend.
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Bảng Marketing_Mail_Campaing không có cột Status/UpdatedDate/StartedAt/CompletedAt.
        /// Method này là no-op để tránh lỗi runtime — trạng thái được quản lý qua Marketing_Mail_Campaign_Send.
        /// </summary>
        public Task UpdateStatusAsync(int campaignId, int status, DateTime? timestamp = null)
        {
            return Task.CompletedTask;
        }
    }
}
