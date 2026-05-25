using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface ITruongService
{
    Task<TruongSearchResultViewModel> SearchAsync(TruongSearchFilterViewModel filter);
    Task<IEnumerable<TruongCardViewModel>> GetRandomPartnersAsync(int count);
    Task<IEnumerable<TruongCardViewModel>> GetByCountryAsync(int countryId, string? loai = null);
    Task<TruongDetailViewModel?> GetDetailAsync(int id);
    Task<MajorSearchViewModel> GetMajorSearchAsync(string? filter, int? quocGiaId, string? loai);
    Task<IEnumerable<QuocGiaViewModel>> GetCountriesAsync();

    /// <summary>
    /// Lấy danh sách trường cho Home Swiper theo bậc học (Loai).
    /// loaiList: ví dụ ["4Y","2Y"] hoặc ["BF"]
    /// </summary>
    Task<IEnumerable<TruongCardViewModel>> GetHomeSwiperAsync(IEnumerable<string> loaiList, int pageSize = 12);
}
