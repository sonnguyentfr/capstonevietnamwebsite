using Hangfire;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Jobs;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// Called by Capstone.View (fire-and-forget HTTP POST) after a successful registration.
    /// Enqueues the email job onto the existing Hangfire server.
    /// No JWT auth required — the endpoint is called internally from within the same network.
    /// Expose only on internal routes if needed.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventRegistrationEmailController : ControllerBase
    {
        private readonly IBackgroundJobClient _jobs;
        private readonly ILogger<EventRegistrationEmailController> _logger;

        public EventRegistrationEmailController(
            IBackgroundJobClient jobs,
            ILogger<EventRegistrationEmailController> logger)
        {
            _jobs   = jobs;
            _logger = logger;
        }

        /// <summary>
        /// Enqueues the confirmation + admin email job.
        /// Always returns 200 immediately — email is processed in the background.
        /// </summary>
        [HttpPost("enqueue")]
        public IActionResult Enqueue([FromBody] EventRegistrationEmailRequest request)
        {
            if (request is null)
                return BadRequest();

            _jobs.Enqueue<EventRegistrationEmailJob>(j => j.SendAsync(request));

            _logger.LogInformation(
                "EventRegistrationEmail enqueued: StudentId={StudentId} EventCatId={EventCatId}",
                request.StudentId, request.EventCatId);

            return Ok();
        }
    }
}
