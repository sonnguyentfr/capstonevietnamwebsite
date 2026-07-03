using NVCMS.WebView.Data.Common;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Contracts.Service;
using NVCMS.WebView.Data.ViewModels;

namespace NVCMS.WebView.Data.Service;

public class VideoService : IVideoService
{
    private readonly IVideoRepository  _repo;
    private readonly ContentUrlRewriter _rewriter;

    public VideoService(IVideoRepository repo, ContentUrlRewriter rewriter)
    {
        _repo     = repo;
        _rewriter = rewriter;
    }

    public async Task<IEnumerable<VideoItemVM>> GetVideosAsync(int portalId, int page, int pageSize)
    {
        var list = await _repo.GetVideosAsync(portalId, page, pageSize);
        return list.Select(m => new VideoItemVM
        {
            VideoId   = m.VideoId,
            Title     = m.Title,
            ImagePath = _rewriter.ResolveUrl(m.ImagePath),
            Summary   = m.Summary,
            TypeVideo = m.TypeVideo,
            VideoPath = m.VideoPath
        });
    }

    public async Task<VideoDetailVM?> GetVideoAsync(int videoId, int portalId)
    {
        var m = await _repo.GetVideoByIdAsync(videoId, portalId);
        if (m is null) return null;
        return new VideoDetailVM
        {
            VideoId   = m.VideoId,
            Title     = m.Title,
            ImagePath = _rewriter.ResolveUrl(m.ImagePath),
            Summary   = m.Summary,
            Content   = _rewriter.ResolveHtml(m.Content),
            TypeVideo = m.TypeVideo,
            VideoPath = m.VideoPath
        };
    }
}
