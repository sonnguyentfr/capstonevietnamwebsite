using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Controllers;

[Route("api/zns")]
[ApiController]
[Authorize]
public class ZnsController : ControllerBase
{
    private readonly IZnsSendService _sendService;

    public ZnsController(IZnsSendService sendService)
    {
        _sendService = sendService;
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromBody] ZnsSendRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request"));

        var result = await _sendService.SendNowAsync(request, cancellationToken);

        if (!result.Success)
            return BadRequest(new { success = false, errorCode = result.ErrorCode, message = result.Message });

        return Ok(new
        {
            success = true,
            message = result.Message,
            data = new
            {
                templateId = request.TemplateId,
                msgId = result.MsgId,
                sentTime = result.SentTime,
                sendingMode = result.SendingMode,
                quota = new
                {
                    remainingQuota = result.RemainingQuota,
                    dailyQuota = result.DailyQuota
                }
            }
        });
    }
}
