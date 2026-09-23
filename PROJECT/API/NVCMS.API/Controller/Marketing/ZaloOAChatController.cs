using DotNetNuke.Web.Api;
using NVCMS.API.Marketing.Services.Marketing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NVCMS.API.Controller
{
    /// <summary>
    /// Proxy Zalo OA Chat cho module DNN (DesktopModules/NVCMS.Marketing/Manager/ZaloOAChat).
    /// Route: /DesktopModules/NVCMS/API/ZaloOAChat/{action}
    ///  - Xác thực: user DNN đăng nhập + anti-forgery token (ServicesFramework).
    ///  - Phân quyền: ZaloOAChatPermission (Administrators / SuperUser / role trong "zalooa_chat_roles").
    ///  - Định danh nhân viên (AgentUserId / UserId) luôn lấy từ UserInfo.UserID, không nhận từ client.
    ///  - Chuyển tiếp sang NVCMS.API.ReadGoogleSheet /api/zalo-oa/* và trả nguyên JSON.
    ///  - fromDate / toDate: chuỗi ISO-8601 UTC (vd 2026-09-23T00:00:00Z), chuyển nguyên văn.
    /// </summary>
    [DnnAuthorize]
    [ValidateAntiForgeryToken]
    public class ZaloOAChatController : DnnApiController
    {
        private const string ApiRoot = "/api/zalo-oa";
        private const long MaxUploadBytes = 6 * 1024 * 1024;

        // ── Conversations ───────────────────────────────────────────────────

        [HttpGet]
        public Task<HttpResponseMessage> GetConversations(string keyword = null, string status = null, bool unreadOnly = false,
            int assignedTo = -1, string fromDate = null, string toDate = null, string sortBy = null, string sortDir = null,
            int pageIndex = 0, int pageSize = 20)
        {
            var q = Query(new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["status"] = status,
                ["unreadOnly"] = unreadOnly ? "true" : null,
                ["assignedTo"] = assignedTo.ToString(CultureInfo.InvariantCulture),
                ["fromDate"] = fromDate,
                ["toDate"] = toDate,
                ["sortBy"] = sortBy,
                ["sortDir"] = sortDir,
                ["pageIndex"] = pageIndex.ToString(CultureInfo.InvariantCulture),
                ["pageSize"] = pageSize.ToString(CultureInfo.InvariantCulture)
            });
            return ForwardGet("/conversations" + q);
        }

        [HttpGet]
        public Task<HttpResponseMessage> GetConversation(long id)
            => ForwardGet("/conversations/" + id);

        [HttpGet]
        public Task<HttpResponseMessage> GetMessages(long conversationId, long beforeId = 0, int pageSize = 30)
            => ForwardGet("/conversations/" + conversationId + "/messages" + Query(new Dictionary<string, string>
            {
                ["beforeId"] = beforeId.ToString(CultureInfo.InvariantCulture),
                ["pageSize"] = pageSize.ToString(CultureInfo.InvariantCulture)
            }));

        /// <summary>since: chuỗi ISO-8601 UTC do server trả về trước đó (giữ nguyên, không parse lại theo giờ máy).</summary>
        [HttpGet]
        public Task<HttpResponseMessage> GetMessageChanges(long conversationId, string since)
            => ForwardGet("/conversations/" + conversationId + "/messages/changes" + Query(new Dictionary<string, string>
            {
                ["since"] = since
            }));

        [HttpGet]
        public Task<HttpResponseMessage> GetUnreadSummary(long afterMessageId = 0)
            => ForwardGet("/unread-summary?afterMessageId=" + afterMessageId.ToString(CultureInfo.InvariantCulture));

        [HttpPost]
        public Task<HttpResponseMessage> MarkRead(IdRequest request)
            => ForwardPost("/conversations/" + (request?.Id ?? 0) + "/read", new { userId = UserInfo.UserID });

        [HttpPost]
        public Task<HttpResponseMessage> SetStatus(SetStatusRequest request)
            => ForwardPost("/conversations/" + (request?.Id ?? 0) + "/status", new { status = request?.Status, userId = UserInfo.UserID });

        // ── Customers ───────────────────────────────────────────────────────

        [HttpGet]
        public Task<HttpResponseMessage> GetCustomers(string keyword = null, int isFollower = -1, string fromDate = null,
            string toDate = null, string sortBy = null, string sortDir = null, int pageIndex = 0, int pageSize = 20)
            => ForwardGet("/customers" + Query(new Dictionary<string, string>
            {
                ["keyword"] = keyword,
                ["isFollower"] = isFollower.ToString(CultureInfo.InvariantCulture),
                ["fromDate"] = fromDate,
                ["toDate"] = toDate,
                ["sortBy"] = sortBy,
                ["sortDir"] = sortDir,
                ["pageIndex"] = pageIndex.ToString(CultureInfo.InvariantCulture),
                ["pageSize"] = pageSize.ToString(CultureInfo.InvariantCulture)
            }));

        [HttpGet]
        public Task<HttpResponseMessage> GetCustomer(long id)
            => ForwardGet("/customers/" + id);

        [HttpPost]
        public Task<HttpResponseMessage> SyncCustomer(IdRequest request)
            => ForwardPost("/customers/" + (request?.Id ?? 0) + "/sync", new { });

        // ── Messages ────────────────────────────────────────────────────────

        [HttpPost]
        public Task<HttpResponseMessage> SendMessage(SendMessageRequest request)
        {
            if (request == null)
                return Task.FromResult(Local(HttpStatusCode.BadRequest, "VALIDATION_ERROR", "Thiếu dữ liệu."));

            return ForwardPost("/messages/send", new
            {
                conversationId = request.ConversationId,
                clientMessageId = request.ClientMessageId,
                agentUserId = UserInfo.UserID,
                messageType = request.MessageType,
                text = request.Text,
                imageUrl = request.ImageUrl
            });
        }

        /// <summary>multipart/form-data: conversationId, clientMessageId, caption, file.</summary>
        [HttpPost]
        public async Task<HttpResponseMessage> SendAttachment()
        {
            var denied = CheckPermission();
            if (denied != null) return denied;

            if (!Request.Content.IsMimeMultipartContent())
                return Local(HttpStatusCode.UnsupportedMediaType, "VALIDATION_ERROR", "Yêu cầu phải là multipart/form-data.");
            if (Request.Content.Headers.ContentLength > MaxUploadBytes)
                return Local(HttpStatusCode.RequestEntityTooLarge, "VALIDATION_ERROR", "Tệp quá lớn (tối đa 5MB).");

            var provider = await Request.Content.ReadAsMultipartAsync(new MultipartMemoryStreamProvider());
            string conversationId = null, clientMessageId = null, caption = null, fileName = null, contentType = null;
            byte[] fileBytes = null;

            foreach (var part in provider.Contents)
            {
                var name = (part.Headers.ContentDisposition?.Name ?? "").Trim('"');
                if (!string.IsNullOrEmpty(part.Headers.ContentDisposition?.FileName))
                {
                    fileName = part.Headers.ContentDisposition.FileName.Trim('"');
                    contentType = part.Headers.ContentType?.MediaType ?? "application/octet-stream";
                    fileBytes = await part.ReadAsByteArrayAsync();
                }
                else if (name == "conversationId") conversationId = await part.ReadAsStringAsync();
                else if (name == "clientMessageId") clientMessageId = await part.ReadAsStringAsync();
                else if (name == "caption") caption = await part.ReadAsStringAsync();
            }

            if (fileBytes == null || fileBytes.Length == 0)
                return Local(HttpStatusCode.BadRequest, "VALIDATION_ERROR", "Chưa chọn tệp.");
            if (fileBytes.Length > MaxUploadBytes)
                return Local(HttpStatusCode.RequestEntityTooLarge, "VALIDATION_ERROR", "Tệp quá lớn (tối đa 5MB).");

            var agentUserId = UserInfo.UserID.ToString(CultureInfo.InvariantCulture);
            var bytes = fileBytes;
            var safeName = System.IO.Path.GetFileName(fileName);
            var response = await ZaloOAChatProxy.SendAsync(HttpMethod.Post, ApiRoot + "/messages/send-attachment", () =>
            {
                var form = new MultipartFormDataContent();
                form.Add(new StringContent(conversationId ?? "0"), "conversationId");
                form.Add(new StringContent(clientMessageId ?? Guid.Empty.ToString()), "clientMessageId");
                form.Add(new StringContent(agentUserId), "agentUserId");
                if (!string.IsNullOrEmpty(caption)) form.Add(new StringContent(caption), "caption");
                var file = new ByteArrayContent(bytes);
                file.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                form.Add(file, "file", safeName);
                return form;
            });
            return ToHttp(response);
        }

        [HttpPost]
        public Task<HttpResponseMessage> RetryMessage(IdRequest request)
            => ForwardPost("/messages/" + (request?.Id ?? 0) + "/retry", new { agentUserId = UserInfo.UserID });

        // ── Vận hành ────────────────────────────────────────────────────────

        [HttpGet]
        public Task<HttpResponseMessage> TokenStatus()
            => ForwardGet("/token-status");

        /// <summary>Nhập lịch sử chat từ Zalo - chỉ Administrators / SuperUser.</summary>
        [HttpPost]
        public Task<HttpResponseMessage> ImportHistory(ImportHistoryRequest request)
        {
            if (!ZaloOAChatPermission.IsChatAdmin(UserInfo))
                return Task.FromResult(Local(HttpStatusCode.Forbidden, "FORBIDDEN", "Chỉ quản trị viên được nhập lịch sử."));

            return ForwardPost("/history/import", new
            {
                maxConversations = request?.MaxConversations,
                maxMessagesPerUser = request?.MaxMessagesPerUser,
                requestedByUserId = UserInfo.UserID
            });
        }

        // ── Helpers ─────────────────────────────────────────────────────────

        private HttpResponseMessage CheckPermission()
            => ZaloOAChatPermission.CanUseChat(UserInfo)
                ? null
                : Local(HttpStatusCode.Forbidden, "FORBIDDEN", "Bạn không có quyền sử dụng Zalo OA Chat.");

        private async Task<HttpResponseMessage> ForwardGet(string path)
        {
            var denied = CheckPermission();
            if (denied != null) return denied;
            return ToHttp(await ZaloOAChatProxy.GetAsync(ApiRoot + path));
        }

        private async Task<HttpResponseMessage> ForwardPost(string path, object body)
        {
            var denied = CheckPermission();
            if (denied != null) return denied;
            return ToHttp(await ZaloOAChatProxy.PostJsonAsync(ApiRoot + path, body));
        }

        private HttpResponseMessage ToHttp(ZaloOAProxyResponse r)
        {
            var msg = Request.CreateResponse(r.StatusCode);
            msg.Content = new StringContent(r.Body ?? "{}", Encoding.UTF8, "application/json");
            return msg;
        }

        private HttpResponseMessage Local(HttpStatusCode status, string code, string message)
            => ToHttp(ZaloOAChatProxy.Error(status, code, message));

        private static string Query(Dictionary<string, string> values)
        {
            var parts = values.Where(kv => !string.IsNullOrEmpty(kv.Value))
                .Select(kv => kv.Key + "=" + HttpUtility.UrlEncode(kv.Value));
            var q = string.Join("&", parts);
            return q.Length == 0 ? "" : "?" + q;
        }

        public class IdRequest
        {
            public long Id { get; set; }
        }

        public class SetStatusRequest
        {
            public long Id { get; set; }
            public string Status { get; set; }
        }

        public class SendMessageRequest
        {
            public long ConversationId { get; set; }
            public Guid ClientMessageId { get; set; }
            public string MessageType { get; set; }
            public string Text { get; set; }
            public string ImageUrl { get; set; }
        }

        public class ImportHistoryRequest
        {
            public int? MaxConversations { get; set; }
            public int? MaxMessagesPerUser { get; set; }
        }
    }
}
