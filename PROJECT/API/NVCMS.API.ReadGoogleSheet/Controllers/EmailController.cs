using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request)
        {
            try
            {
                await _emailService.SendEmailAsync(
                    request.FromEmail,
                    request.ToEmail,
                    request.Subject,
                    request.Body,
                    request.CcEmail,
                    request.BccEmail);

                return Ok(ApiResponse<object>.SuccessResponse(
                    new { Message = "Email sent successfully" },
                    "Email sent"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return StatusCode(500, ApiResponse<object>.ErrorResponse($"Failed to send email: {ex.Message}"));
            }
        }
    }

    public class SendEmailRequest
    {
        public string FromEmail { get; set; } = string.Empty;
        public string ToEmail { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? CcEmail { get; set; }
        public string? BccEmail { get; set; }
    }
}