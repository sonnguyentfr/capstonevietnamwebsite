using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Controllers;

/// <summary>
/// Chạy tay 2 job đồng bộ CRM (thay cho việc bấm Run Now trong DNN Scheduler).
/// Job được đẩy vào Hangfire nên vẫn theo dõi được ở /hangfire.
/// </summary>
[Route("api/crm-sync")]
[ApiController]
[Authorize]
public class CrmSyncJobController : ControllerBase
{
    private readonly IBackgroundJobClient _jobClient;

    public CrmSyncJobController(IBackgroundJobClient jobClient)
    {
        _jobClient = jobClient;
    }

    /// <summary>Google Sheet → student_from_ladipage.</summary>
    [HttpPost("run-import")]
    public IActionResult RunImport()
    {
        var jobId = _jobClient.Enqueue<ImportCrmDataJob>(x => x.Execute(CancellationToken.None));

        return Ok(ApiResponse<object>.SuccessResponse(
            new { jobId }, "Đã đẩy ImportCrmDataJob vào hàng đợi Hangfire"));
    }

    /// <summary>student_from_ladipage → Student_Info / NV_Events_Student + gửi mail.</summary>
    [HttpPost("run-copy-student")]
    public IActionResult RunCopyStudent()
    {
        var jobId = _jobClient.Enqueue<CopyStudentFromLadiJob>(x => x.Execute(CancellationToken.None));

        return Ok(ApiResponse<object>.SuccessResponse(
            new { jobId }, "Đã đẩy CopyStudentFromLadiJob vào hàng đợi Hangfire"));
    }
}
