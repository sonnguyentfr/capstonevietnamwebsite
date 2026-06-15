using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Simple authentication - Replace with your actual authentication logic
            // In production, use ASP.NET Core Identity or similar
            if (request.Username == "admin" && request.Password == "Admin@123")
            {
                var token = _tokenService.GenerateToken(request.Username);
                
                return Ok(ApiResponse<LoginResponse>.SuccessResponse(new LoginResponse
                {
                    Token = token,
                    Expiration = DateTime.Now.AddHours(24)
                }, "Login successful"));
            }

            return Unauthorized(ApiResponse<LoginResponse>.ErrorResponse("Invalid username or password"));
        }
    }
}