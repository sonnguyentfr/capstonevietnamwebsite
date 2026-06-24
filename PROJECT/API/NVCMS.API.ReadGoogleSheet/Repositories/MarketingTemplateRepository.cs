using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MarketingTemplateRepository : MarketingRepository<Marketing_Mail_Template>, IMarketingTemplateRepository
    {
        public MarketingTemplateRepository(CRMDbContext context) : base(context) { }

        public async Task<IEnumerable<Marketing_Mail_Template>> GetByPortalIdAsync(int portalId)
        {
            return await _dbSet
                .Where(t => t.PortalId == portalId)
                .OrderBy(t => t.TemplateName)
                .ToListAsync();
        }
    }
}
