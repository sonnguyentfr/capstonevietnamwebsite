using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingEventRepository : IRepository<MarketingMailEvent>
    {
        Task<IEnumerable<MarketingMailEvent>> GetByListMailIdAsync(int listMailId);
        Task<IEnumerable<MarketingMailEvent>> GetByEventTypeAsync(string eventType);
    }
}
