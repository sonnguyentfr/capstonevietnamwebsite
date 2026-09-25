using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Services;
using System.Threading.Tasks;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ZaloController : ControllerBase
    {
        private readonly IZaloService _zaloService;
        private readonly IZnsSendService _znsSendService;

        public ZaloController(IZaloService zaloTokenService, IZnsSendService znsSendService)
        {
            _zaloService = zaloTokenService;
            _znsSendService = znsSendService;
        }
        /// <summary>
        /// get Access Token from Zalo API and save to database.
        /// Không trả giá trị token về client - chỉ trả trạng thái.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("get-access-token")]
        public async Task<IActionResult> GetAccessToken([FromForm] string code)
        {
            var result = await _zaloService.GetAndSaveTokenAsync(code);
            return Ok(ToSafeResult(result));
        }
        /// <summary>
        /// refresh token from Zalo API and save to database.
        /// Không trả giá trị token về client - chỉ trả trạng thái.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("get-refresh-token")]
        public async Task<IActionResult> GetRefreshToken()
        {
            var token = await _zaloService.GetLastTokenAsync();
            if (token == null)
                return NotFound("No token found to refresh");

            var result = await _zaloService.RefreshAndSaveTokenAsync(token.RefreshToken);
            return Ok(ToSafeResult(result));
        }
        /// <summary>
        /// Trạng thái token mới nhất (Id, CreatedAt, ExpiresAt, IsExpired, IsEncrypted) - KHÔNG kèm giá trị token.
        /// </summary>
        [HttpGet("last")]
        public async Task<ActionResult<ZaloTokenStatus>> GetLastZaloToken()
        {
            var status = await _zaloService.GetTokenStatusAsync();
            if (status == null)
                return NotFound();
            return status;
        }
        /// <summary>
        /// Gửi tin nhắn Zalo theo template - MÀN HÌNH THỐNG KÊ SỰ KIỆN
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("send-event-success")]
        public async Task<IActionResult> Send_DangKySuKien_ThanhCong([FromBody] ZaloMessageRequest<ZaloMessage_DangKyThanhCongSK_Request> request)
        {
            var result = await _zaloService.SendTemplateAsync(request);

            return Ok(result);
        }

        private static object ToSafeResult(ZaloTokenResponse? result) => new
        {
            success = !string.IsNullOrWhiteSpace(result?.access_token),
            expires_in = result?.expires_in,
            error = result?.error,
            error_name = result?.error_name,
            error_description = result?.error_description
        };
        /// <summary>
        /// Gửi tin nhắn Zalo theo template - MÀN HÌNH THỐNG KÊ SỰ KIỆN (chạy qua job Hangfire).
        /// Mỗi NV_Events_Student hợp lệ → 1 ZnsSendQueue + 1 ZnsSendJob; dòng không hợp lệ trả về trong skipped.
        /// </summary>
        [HttpPost("send-event-job")]
        public async Task<IActionResult> Send_DangKySuKien_Job([FromBody] ZnsEventStudentSendRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid request"));

            var result = await _znsSendService.EnqueueEventStudentsAsync(request, cancellationToken);

            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                data = new
                {
                    totalRequested = result.TotalRequested,
                    totalQueued = result.TotalQueued,
                    items = result.Items,
                    skipped = result.Skipped
                }
            });
        }

    }
}
