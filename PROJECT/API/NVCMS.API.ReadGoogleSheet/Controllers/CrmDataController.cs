using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CrmDataController : ControllerBase
    {
        private readonly ICrmDataService _crmDataService;
        private readonly ILogger<CrmDataController> _logger;

        public CrmDataController(ICrmDataService crmDataService, ILogger<CrmDataController> logger)
        {
            _crmDataService = crmDataService;
            _logger = logger;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportFromGoogleSheet([FromBody] GoogleSheetRequest request)
        {
            try
            {
                var importedCount = await _crmDataService.ImportFromGoogleSheetAsync(
                    request.SpreadsheetId,
                    request.Range,
                    request.eventCat_id);

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { ImportedRecords = importedCount },
                    $"Successfully imported {importedCount} new records",
                    importedCount));
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogError(ex, "Service account file not found");
                return BadRequest(ApiResponse<object>.ErrorResponse("Google service account configuration file not found"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing data from Google Sheet");
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _crmDataService.GetAllAsync();
                return Ok(ApiResponse<object>.SuccessResponse(
                    data,
                    "Data retrieved successfully",
                    data.Count()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving CRM data");
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"An error occurred: {ex.Message}"));
            }
        }
    }
}