using Dapper;
using Microsoft.Data.SqlClient;
using NVCMS.WebView.Data.Contracts.Repository;
using NVCMS.WebView.Data.Models;
using System.Data;

namespace NVCMS.WebView.Data.Repository;

public class VideoRepository : IVideoRepository
{
    private readonly string _connectionString;

    public VideoRepository(string connectionString) =>
        _connectionString = connectionString;

    private SqlConnection CreateConn() => new(_connectionString);

    public async Task<IEnumerable<VideoModel>> GetVideosAsync(int portalId, int page, int pageSize)
    {
        const string sql = @"
            SELECT VideoId, CategoryId, Title, ImagePath, VideoPath,
                   Summary, TypeVideo, IsActive, Status, Createdate, ViewCount, PortalId
            FROM   NVCMS_Video
            WHERE  IsActive = 1
              AND  Status   = 2
              AND  PortalId = @PortalId
            ORDER BY
                CASE WHEN Createdate IS NULL THEN 1 ELSE 0 END,
                Createdate DESC,
                VideoId    DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        await using var conn = CreateConn();
        return await conn.QueryAsync<VideoModel>(sql, new
        {
            PortalId = portalId,
            Offset = (page - 1) * pageSize,
            PageSize = pageSize
        });
        //await using var conn = CreateConn();

        //return await conn.QueryAsync<VideoModel>(
        //    "WebView_NVCMS_Video_SelectIndexView",
        //    new
        //    {
        //        PortalId = portalId,
        //        Page = page,
        //        PageSize = pageSize
        //    },
        //    commandType: CommandType.StoredProcedure);
    }

    public async Task<VideoModel?> GetVideoByIdAsync(int videoId, int portalId)
    {
        await using var conn = CreateConn();
        return await conn.QueryFirstOrDefaultAsync<VideoModel>(
            "WebView_NVCMS_Video_SelectById",
            new { videoId = videoId, PortalId = portalId },
            commandType: CommandType.StoredProcedure);
    }
}
