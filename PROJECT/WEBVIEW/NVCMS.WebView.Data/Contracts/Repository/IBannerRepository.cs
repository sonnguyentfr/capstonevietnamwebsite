using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IBannerRepository
{
    Task<IEnumerable<BannerModel>> GetAllAsync(int portalId);
    Task<IEnumerable<BannerModel>> GetAllShowAsync(int portalId, int vitri);
    Task<IEnumerable<BannerModel>> GetByVitriAsync(int vitri, int portalId);
    Task<BannerModel?> GetByIdAsync(int bannerId);
    Task UpdateClickAsync(int bannerId);
    Task UpdateViewAsync(int bannerId);
}