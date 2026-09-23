using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Models.ZaloOA;
using NVCMS.API.ReadGoogleSheet.Services;
using System.Diagnostics;

namespace NVCMS.API.ReadGoogleSheet.Controllers
{
    /// <summary>
    /// API quản lý chat Zalo OA. Chỉ gọi từ server (proxy DNN NVCMS.API ZaloOAChatController) bằng JWT dịch vụ.
    /// Trình duyệt KHÔNG gọi trực tiếp. Định danh nhân viên (AgentUserId / UserId) do proxy DNN gắn.
    /// Tài liệu: Doc/ZaloOAChat/ZaloOAChat_API.md
    /// </summary>
    [Route("api/zalo-oa")]
    [ApiController]
    [Authorize]
    public class ZaloOAChatController : ControllerBase
    {
        private readonly IZaloOAChatService _chat;
        private readonly IZaloService _token;
        private readonly IBackgroundJobClient _jobs;
        private readonly ILogger<ZaloOAChatController> _logger;

        public ZaloOAChatController(IZaloOAChatService chat, IZaloService token, IBackgroundJobClient jobs, ILogger<ZaloOAChatController> logger)
        {
            _chat = chat;
            _token = token;
            _jobs = jobs;
            _logger = logger;
        }

        // ── Conversations ───────────────────────────────────────────────────

        /// <summary>Danh sách hội thoại (tìm kiếm, lọc trạng thái, chưa đọc, khoảng thời gian, sắp xếp, phân trang).</summary>
        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations([FromQuery] ZaloOAConversationQuery query)
        {
            var r = await _chat.GetConversationsAsync(query);
            return r.Success ? Ok(ApiResponse<List<ZaloOAConversation>>.SuccessResponse(r.Data!.Items, "OK", r.Data.TotalRecords)) : Fail(r);
        }

        [HttpGet("conversations/{id:long}")]
        public async Task<IActionResult> GetConversation(long id) => ToResult(await _chat.GetConversationAsync(id));

        /// <summary>Tin nhắn của hội thoại (cũ → mới). beforeId = Id tin cũ nhất đang hiển thị để tải trang trước.</summary>
        [HttpGet("conversations/{id:long}/messages")]
        public async Task<IActionResult> GetMessages(long id, [FromQuery] long beforeId = 0, [FromQuery] int pageSize = 30)
            => ToResult(await _chat.GetMessagesAsync(id, beforeId, pageSize));

        /// <summary>Polling: tin mới / tin đổi trạng thái kể từ <paramref name="since"/> (UTC ISO-8601).</summary>
        [HttpGet("conversations/{id:long}/messages/changes")]
        public async Task<IActionResult> GetMessageChanges(long id, [FromQuery] DateTime since)
            => ToResult(await _chat.GetMessageChangesAsync(id, since));

        [HttpPost("conversations/{id:long}/read")]
        public async Task<IActionResult> MarkRead(long id, [FromBody] ZaloOAMarkReadRequest request)
            => ToResult(await _chat.MarkReadAsync(id, request.UserId));

        /// <summary>Đóng / mở lại / chờ xử lý hội thoại.</summary>
        [HttpPost("conversations/{id:long}/status")]
        public async Task<IActionResult> SetStatus(long id, [FromBody] ZaloOASetStatusRequest request)
            => ToResult(await _chat.SetStatusAsync(id, request.Status, request.UserId));

        /// <summary>Tổng số chưa đọc + tin khách mới sau afterMessageId (để hiện thông báo trình duyệt).</summary>
        [HttpGet("unread-summary")]
        public async Task<IActionResult> GetUnreadSummary([FromQuery] long afterMessageId = 0)
            => ToResult(await _chat.GetUnreadSummaryAsync(afterMessageId));

        // ── Customers ───────────────────────────────────────────────────────

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers([FromQuery] ZaloOACustomerQuery query)
        {
            var r = await _chat.GetCustomersAsync(query);
            return r.Success ? Ok(ApiResponse<List<ZaloOACustomer>>.SuccessResponse(r.Data!.Items, "OK", r.Data.TotalRecords)) : Fail(r);
        }

        [HttpGet("customers/{id:long}")]
        public async Task<IActionResult> GetCustomer(long id) => ToResult(await _chat.GetCustomerAsync(id));

        /// <summary>Đồng bộ lại hồ sơ khách từ Zalo (user/detail).</summary>
        [HttpPost("customers/{id:long}/sync")]
        public async Task<IActionResult> SyncCustomer(long id, CancellationToken ct) => ToResult(await _chat.SyncCustomerAsync(id, ct));

        // ── Messages ────────────────────────────────────────────────────────

        /// <summary>Gửi tin tư vấn (TEXT hoặc IMAGE theo URL https).</summary>
        [HttpPost("messages/send")]
        public async Task<IActionResult> Send([FromBody] ZaloOASendMessageRequest request, CancellationToken ct)
            => ToResult(await _chat.SendMessageAsync(request, ct));

        /// <summary>Gửi ảnh (jpg/png ≤1MB) hoặc tệp (pdf/doc/docx/csv ≤5MB). multipart/form-data.</summary>
        [HttpPost("messages/send-attachment")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        public async Task<IActionResult> SendAttachment([FromForm] long conversationId, [FromForm] Guid clientMessageId,
            [FromForm] int? agentUserId, [FromForm] string? caption, IFormFile? file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                return Fail(ZaloOAServiceResult<ZaloOAMessage>.Fail(ZaloOAErrorCodes.ValidationError, "Chưa chọn tệp.", 400));

            await using var buffer = new MemoryStream();
            await file.CopyToAsync(buffer, ct);
            buffer.Position = 0;

            return ToResult(await _chat.SendAttachmentAsync(conversationId, clientMessageId, agentUserId,
                buffer, file.FileName, file.ContentType, file.Length, caption, ct));
        }

        [HttpPost("messages/{id:long}/retry")]
        public async Task<IActionResult> Retry(long id, [FromBody] ZaloOARetryMessageRequest request, CancellationToken ct)
            => ToResult(await _chat.RetryMessageAsync(id, request.AgentUserId, ct));

        // ── Vận hành ────────────────────────────────────────────────────────

        /// <summary>Nhập lịch sử chat từ Zalo (chạy nền Hangfire). Trả về Hangfire job id.</summary>
        [HttpPost("history/import")]
        public IActionResult ImportHistory([FromBody] ZaloOAHistoryImportRequest request)
        {
            var jobId = _jobs.Enqueue<ZaloOAHistoryImportJob>(j => j.ExecuteAsync(request, CancellationToken.None));
            _logger.LogInformation("Zalo history import enqueued JobId={JobId} RequestedBy={UserId}", jobId, request.RequestedByUserId);
            return Ok(ApiResponse<object>.SuccessResponse(new { jobId }, "Đã đưa vào hàng đợi nhập lịch sử."));
        }

        /// <summary>Trạng thái Zalo access token (không trả giá trị token).</summary>
        [HttpGet("token-status")]
        public async Task<IActionResult> TokenStatus()
        {
            var status = await _token.GetTokenStatusAsync();
            return status == null
                ? Fail(ZaloOAServiceResult<ZaloTokenStatus>.Fail(ZaloOAErrorCodes.ZaloTokenUnavailable, "Chưa có Zalo token.", 404))
                : Ok(ApiResponse<ZaloTokenStatus>.SuccessResponse(status));
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private string TraceId => Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        private IActionResult ToResult<T>(ZaloOAServiceResult<T> r)
            => r.Success ? Ok(ApiResponse<T>.SuccessResponse(r.Data!, r.Message)) : Fail(r);

        private IActionResult Fail<T>(ZaloOAServiceResult<T> r)
            => StatusCode(r.HttpStatus, ApiResponse<T>.ErrorResponse(r.Message, r.ErrorCode ?? ZaloOAErrorCodes.InternalError, TraceId, r.Data));
    }
}
