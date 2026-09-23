using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Services;
using System.Text;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// Nhận webhook Zalo OA. Endpoint PUBLIC (Zalo gọi trực tiếp) - xác thực bằng X-ZEvent-Signature.
    /// Đăng ký trên Zalo Developers: https://api-data.capstonevietnam.com/api/zalo-oa/webhook
    /// Zalo yêu cầu HTTP 200 trong 2 giây nên chỉ lưu event rồi xử lý nền (Hangfire).
    /// </summary>
    [Route("api/zalo-oa/webhook")]
    [ApiController]
    [AllowAnonymous]
    public class ZaloOAWebhookController : ControllerBase
    {
        private const int MaxBodyBytes = 1024 * 1024;
        public const string SignatureHeader = "X-ZEvent-Signature";

        private readonly IZaloOAWebhookService _webhook;

        public ZaloOAWebhookController(IZaloOAWebhookService webhook)
        {
            _webhook = webhook;
        }

        /// <summary>Kiểm tra endpoint còn sống (không xử lý gì).</summary>
        [HttpGet]
        public IActionResult Ping() => Ok(new { status = "ok" });

        /// <summary>Zalo POST event (application/json).</summary>
        [HttpPost]
        [RequestSizeLimit(MaxBodyBytes)]
        public async Task<IActionResult> Receive(CancellationToken cancellationToken)
        {
            string rawBody;
            using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
                rawBody = await reader.ReadToEndAsync(cancellationToken);

            var result = await _webhook.ReceiveAsync(rawBody, Request.Headers[SignatureHeader].ToString(), cancellationToken);

            return result.Outcome switch
            {
                ZaloOAWebhookReceiveOutcome.Accepted => Ok(new { status = "received" }),
                ZaloOAWebhookReceiveOutcome.Duplicate => Ok(new { status = "duplicate" }),
                ZaloOAWebhookReceiveOutcome.InvalidPayload => BadRequest(new { status = "invalid_payload" }),
                ZaloOAWebhookReceiveOutcome.InvalidSignature => Unauthorized(new { status = "invalid_signature" }),
                _ => StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = "not_configured" })
            };
        }
    }
}
