using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IOrganizationRepository
{
    Task<IEnumerable<OrganizationModel>> GetByIdsAsync(IEnumerable<int> ids);
}
