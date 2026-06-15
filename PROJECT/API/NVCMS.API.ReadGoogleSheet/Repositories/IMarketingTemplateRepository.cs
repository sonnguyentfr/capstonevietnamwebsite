using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMarketingTemplateRepository : IRepository<Marketing_Mail_Template>
    {
        Task<IEnumerable<Marketing_Mail_Template>> GetByPortalIdAsync(int portalId);
    }
}
