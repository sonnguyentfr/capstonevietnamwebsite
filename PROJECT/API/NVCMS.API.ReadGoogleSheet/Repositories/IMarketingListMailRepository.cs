using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingListMailRepository : IRepository<Marketing_Mail_ListMail>
    {
        Task<IEnumerable<Marketing_Mail_ListMail>> GetByCampaignIdAsync(int campaignId);
    }
}
