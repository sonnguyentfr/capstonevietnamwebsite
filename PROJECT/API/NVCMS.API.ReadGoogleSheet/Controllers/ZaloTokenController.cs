using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using System.Threading.Tasks;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ZaloTokenController : ControllerBase
    {
        private readonly IZaloTokenService _zaloTokenService;

        public ZaloTokenController(IZaloTokenService zaloTokenService)
        {
            _zaloTokenService = zaloTokenService;
        }
        /// <summary>
        /// get Access Token from Zalo API and save to database
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("get-access-token")]
        public async Task<IActionResult> GetAccessToken([FromForm] string code)
        {
            var result = await _zaloTokenService.GetAndSaveTokenAsync(code);
            return Ok(result);
        }
        /// <summary>
        /// refresh token from Zalo API and save to database
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("get-refresh-token")]
        public async Task<IActionResult> GetRefreshToken()
        {
            var token = await _zaloTokenService.GetLastTokenAsync();
            if (token == null)
                return NotFound("No token found to refresh");   

            var result = await _zaloTokenService.RefreshAndSaveTokenAsync(token.RefreshToken);
            return Ok(result);
        }
        [HttpGet("last")]
        public async Task<ActionResult<Zalo_Token>> GetLastZaloToken()
        {
            var token = await _zaloTokenService.GetLastTokenAsync();
            if (token == null)
                return NotFound();
            return token;
        }
    }
}