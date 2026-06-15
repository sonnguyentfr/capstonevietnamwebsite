using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingUnsubRepository : IRepository<MarketingMailListMailUnsub>
    {
        Task<bool> IsUnsubscribedAsync(string email, int portalId);
        Task<MarketingMailListMailUnsub?> GetByTokenAsync(Guid token);
    }
}
