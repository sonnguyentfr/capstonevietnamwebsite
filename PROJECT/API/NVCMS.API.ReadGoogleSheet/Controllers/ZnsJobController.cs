using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Controllers;

[Route("api/zns")]
[ApiController]
[Authorize]
public class ZnsJobController : ControllerBase
{
    private readonly IZnsSendService _sendService;

    public ZnsJobController(IZnsSendService sendService)
    {
        _sendService = sendService;
    }

    [HttpPost("send-job")]
    [Authorize]
    public async Task<IActionResult> SendJob([FromBody] ZnsSendRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request"));

        var enqueueResult = await _sendService.EnqueueAsync(request, cancellationToken);

        return Ok(new
        {
            success = enqueueResult.Success,
            message = enqueueResult.Message,
            data = new
            {
                totalRecipients = enqueueResult.TotalRecipients,
                items = enqueueResult.Items
            }
        });
    }
}
