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

        var enqueueResult = await _sendService.EnqueueAsync(request, cancellationToken);

        return Ok(new
        {
            success = enqueueResult.Success,
            message = enqueueResult.Message,
            data = new
            {
                totalRecipients = enqueueResult.TotalRecipients,
                items = enqueueResult.Items,
                templateId = request.TemplateId,
                campaignId = request.CampaignId,
                eventCatId = request.EventCatId,
                eventId = request.EventId,
                contextType = request.ContextType,
                createdBy = request.CreatedBy
            }
        });
    }
}
