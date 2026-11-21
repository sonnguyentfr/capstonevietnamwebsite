using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Controllers;

/// <summary>
/// FB-SYNC 19/11/2026: chạy tay job lấy bài từ Facebook Page (không cần chờ tới lịch).
/// Job được đẩy vào Hangfire nên theo dõi được ở /hangfire.
/// </summary>
[Route("api/facebook-sync")]
[ApiController]
[Authorize]
public class FacebookSyncJobController : ControllerBase
{
    private readonly IBackgroundJobClient _jobClient;

    public FacebookSyncJobController(IBackgroundJobClient jobClient)
    {
        _jobClient = jobClient;
    }

    /// <summary>Facebook Page → Tin tức.</summary>
    [HttpPost("run")]
    public IActionResult Run()
    {
        var jobId = _jobClient.Enqueue<FacebookPageSyncJob>(x => x.Execute(CancellationToken.None));

        return Ok(ApiResponse<object>.SuccessResponse(
            new { jobId }, "Đã đẩy FacebookPageSyncJob vào hàng đợi Hangfire"));
    }
}
