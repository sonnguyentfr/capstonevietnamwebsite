using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

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
    private readonly IBirthDateBackfillService _birthDateBackfill;

    public CrmSyncJobController(IBackgroundJobClient jobClient, IBirthDateBackfillService birthDateBackfill)
    {
        _jobClient = jobClient;
        _birthDateBackfill = birthDateBackfill;
    }

    /// <summary>
    /// Sửa ngày sinh bị lỗi parse MM/dd của job import cũ, đọc lại từ Google Sheet.
    /// Mặc định CHẠY THỬ (apply=false): chỉ trả danh sách sẽ sửa. apply=true mới ghi DB.
    /// Ví dụ: POST /api/crm-sync/backfill-birthdate?eventCatIds=197&amp;eventCatIds=194&amp;apply=false
    /// </summary>
    [HttpPost("backfill-birthdate")]
    public async Task<IActionResult> BackfillBirthDate(
        [FromQuery] int[] eventCatIds, [FromQuery] bool apply = false, CancellationToken ct = default)
    {
        if (eventCatIds is not { Length: > 0 })
            return BadRequest(ApiResponse<object>.ErrorResponse("Cần truyền ít nhất 1 eventCatIds"));

        var result = await _birthDateBackfill.RunAsync(eventCatIds.Distinct().ToArray(), apply, ct);

        var msg = apply
            ? $"Đã sửa {result.LadipageUpdated} dòng student_from_ladipage, {result.StudentInfoUpdated} Student_Info"
            : $"CHẠY THỬ: sẽ sửa {result.LadipageUpdated} dòng student_from_ladipage, {result.StudentInfoUpdated} Student_Info";

        return Ok(ApiResponse<BirthDateBackfillResult>.SuccessResponse(result, msg));
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
