using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface IBannerService
{
    Task<IEnumerable<BannerViewModel>> GetAllAsync(int portalId);
    Task<IEnumerable<BannerViewModel>> GetAllShowAsync(int portalId, int vitri);
    Task<IEnumerable<BannerViewModel>> GetByVitriAsync(int vitri, int portalId);
    Task<BannerViewModel?> GetByIdAsync(int bannerId);
    Task UpdateClickAsync(int bannerId);
    Task UpdateViewAsync(int bannerId);
}