using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IFairGuideRepository
{
    Task<IEnumerable<FairGuideModel>> GetAllActiveAsync(int portalId);
    Task<FairGuideModel?> GetByIdAsync(int id, int portalId);
    Task<IEnumerable<FairGuideMediaModel>> GetMediaAsync(int fairGuideId, int portalId);
}
