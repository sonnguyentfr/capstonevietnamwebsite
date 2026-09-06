using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Capstone.View.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CacheController : ControllerBase
{
    private const string ApiKeyHeaderName = "X-Cache-Api-Key";

    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CacheController> _logger;

    public CacheController(
        IMemoryCache memoryCache,
        IConfiguration configuration,
        ILogger<CacheController> logger)
    {
        _memoryCache = memoryCache;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("clear-all")]
    [HttpGet("clear-all")]
    public IActionResult ClearAll([FromQuery] string? key = null)
    {
        var expectedKey = _configuration["CacheInvalidation:ApiKey"];
        if (string.IsNullOrWhiteSpace(expectedKey))
        {
            _logger.LogError("CacheInvalidation:ApiKey is not configured.");
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                success = false,
                message = "Cache API key is not configured."
            });
        }

        var providedKey = Request.Headers[ApiKeyHeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(providedKey))
        {
            providedKey = key;
        }

        if (!string.Equals(providedKey, expectedKey, StringComparison.Ordinal))
        {
            _logger.LogWarning("Unauthorized cache clear request from {RemoteIp}.", HttpContext.Connection.RemoteIpAddress);
            return Unauthorized(new
            {
                success = false,
                message = "Invalid API key."
            });
        }

        if (_memoryCache is MemoryCache concreteMemoryCache)
        {
            concreteMemoryCache.Compact(1.0);
            _logger.LogInformation("All IMemoryCache entries were cleared by {RemoteIp}.", HttpContext.Connection.RemoteIpAddress);

            return Ok(new
            {
                success = true,
                message = "All memory cache entries have been cleared.",
                clearedAtUtc = DateTime.UtcNow
            });
        }

        _logger.LogWarning("IMemoryCache implementation does not support full clear.");
        return StatusCode(StatusCodes.Status501NotImplemented, new
        {
            success = false,
            message = "Current cache provider does not support clearing all entries."
        });
    }
}
