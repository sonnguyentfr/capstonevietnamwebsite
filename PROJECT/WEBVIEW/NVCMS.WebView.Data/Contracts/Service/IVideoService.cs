using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Contracts.Service;

public interface IVideoService
{
    Task<IEnumerable<VideoItemVM>> GetVideosAsync(int portalId, int page, int pageSize);
    Task<VideoDetailVM?> GetVideoAsync(int videoId, int portalId);
}
