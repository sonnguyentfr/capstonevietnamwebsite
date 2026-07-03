using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface IFairGuideService
{
    Task<IEnumerable<FairGuideItemVM>> GetAllAsync(int portalId);
    Task<FairGuideDetailVM?> GetDetailAsync(int id, int portalId);
}
