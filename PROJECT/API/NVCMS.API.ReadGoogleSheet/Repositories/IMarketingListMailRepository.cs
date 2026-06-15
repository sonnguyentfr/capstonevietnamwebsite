using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingListMailRepository : IRepository<Marketing_Mail_ListMail>
    {
        Task<IEnumerable<Marketing_Mail_ListMail>> GetByCampaignIdAsync(int campaignId);
        Task<IEnumerable<Marketing_Mail_ListMail>> GetPendingByCampaignIdAsync(int campaignId);
        Task<Marketing_Mail_ListMail?> GetByMessageIdAsync(string messageId);
        Task UpdateRecipientStatusAsync(int id, int status, DateTime? timestamp = null);
        Task IncrementSendCountAsync(int id);
        Task<(int Total, int Sent, int Failed)> GetCampaignCountsAsync(int campaignId);
    }
}
