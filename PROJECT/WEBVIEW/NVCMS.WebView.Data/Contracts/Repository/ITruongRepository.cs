using NVCMS.WebView.Data.Models;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface ITruongRepository
{
    Task<(IEnumerable<TruongModel> Items, int Total)> SearchAsync(TruongSearchFilterViewModel filter);
    Task<IEnumerable<TruongModel>> GetRandomPartnersAsync(int count, int? portalId = null);
    Task<IEnumerable<TruongModel>> GetByCountryAsync(int countryId, string? loai = null, int? portalId = null);
    Task<TruongModel?> GetByIdAsync(int id);
    Task<IEnumerable<TruongModel>> GetByIdsAsync(IEnumerable<int> ids);
    Task<TruongAdmis4YearModel?> GetAdmis4YearAsync(int truongId, int? portalId = null);
    Task<TruongAdmisBFModel?> GetAdmisBFAsync(int truongId, int? portalId = null);
    Task<TruongAdmisESLModel?> GetAdmisESLAsync(int truongId, int? portalId = null);
    Task<IEnumerable<TruongMajorModel>> GetMajorsByTruongAsync(int truongId);
    Task<IEnumerable<TruongMajorModel>> GetAllMajorsAsync();
    Task<IEnumerable<TruongMajorModel>> GetMajorsWithCountAsync(int? quocGiaId, string? loai);
    Task<IEnumerable<(int Id, string Ten, int Count)>> GetCountriesWithCountAsync(bool? isPartner = null);
}
