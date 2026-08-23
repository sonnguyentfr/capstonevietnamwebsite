using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Controllers;

[Route("api/zns/templates")]
[ApiController]
[Authorize]
public class ZnsTemplateController : ControllerBase
{
    private readonly IZnsTemplateService _templateService;

    public ZnsTemplateController(IZnsTemplateService templateService)
    {
        _templateService = templateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTemplates([FromQuery] bool onlyActive = true)
    {
        var data = await _templateService.GetTemplatesAsync(onlyActive);
        return Ok(ApiResponse<List<ZnsTemplate>>.SuccessResponse(data, "Success", data.Count));
    }

    [HttpGet("{templateId:long}")]
    public async Task<IActionResult> GetTemplate(long templateId)
    {
        var data = await _templateService.GetTemplateAsync(templateId);
        if (data is null)
            return NotFound(ApiResponse<object>.ErrorResponse("Template not found"));

        return Ok(ApiResponse<ZnsTemplate>.SuccessResponse(data));
    }

    [HttpPost("sync")]
    public async Task<IActionResult> SyncTemplates(CancellationToken cancellationToken)
    {
        var changed = await _templateService.SyncTemplatesAsync(cancellationToken);
        return Ok(ApiResponse<object>.SuccessResponse(new { changed }, "Synced"));
    }
}
