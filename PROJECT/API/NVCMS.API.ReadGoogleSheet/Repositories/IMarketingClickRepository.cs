using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingClickRepository : IRepository<MarketingMailClick>
    {
        Task<IEnumerable<MarketingMailClick>> GetByListMailIdAsync(int listMailId);
    }
}
