using NVCMS.WebView.Data.Models;

namespace NVCMS.WebView.Data.Contracts.Repository;

public interface IVideoRepository
{
    Task<IEnumerable<VideoModel>> GetVideosAsync(int portalId, int page, int pageSize);
    Task<VideoModel?> GetVideoByIdAsync(int videoId, int portalId);
}
