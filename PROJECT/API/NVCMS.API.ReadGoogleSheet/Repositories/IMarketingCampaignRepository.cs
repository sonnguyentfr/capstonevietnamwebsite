using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingCampaignRepository : IRepository<Marketing_Mail_Campaing>
    {
        Task<IEnumerable<Marketing_Mail_Campaing>> GetByPortalIdAsync(int portalId);
        Task<IEnumerable<Marketing_Mail_Campaing>> GetByStatusAsync(int status);
        Task UpdateStatusAsync(int campaignId, int status, DateTime? timestamp = null);
    }
}
