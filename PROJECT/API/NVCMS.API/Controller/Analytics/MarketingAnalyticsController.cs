using DotNetNuke.Web.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NVCMS.API.Analytics;
using NVCMS.API.Model;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace NVCMS.API.Controller
{
    /// <summary>
    /// Phân tích Marketing (Google Analytics 4) - trang chủ quản trị (skin _admin/controls/index/U_NhuanBut.ascx).
    /// /DesktopModules/NVCMS/API/MarketingAnalytics/{Init|Overview|Content|Article|Acquisition|Campaigns|Campaign|Audience}.
    /// Khóa GA4 chỉ ở server (Cấu hình website › Google Analytics). JSON trả về dạng camelCase.
    /// </summary>
    [DnnAuthorize]
    [ValidateAntiForgeryToken]
    public class MarketingAnalyticsController : DnnApiController
    {
        private static readonly JsonSerializerSettings Camel = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

        private static HttpResponseMessage Json(HttpStatusCode status, ApiResponse<object> body) =>
            new HttpResponseMessage(status) { Content = new StringContent(JsonConvert.SerializeObject(body, Camel), Encoding.UTF8, "application/json") };

        private bool HasPermission()
        {
            return UserInfo.IsSuperUser
                || UserInfo.IsInRole("Administrators")
                || UserInfo.IsInRole("Manager")
                || UserInfo.IsInRole("Marketing")
                || UserInfo.IsInRole("Xuat ban")
                || UserInfo.IsInRole("LanhDaoToaSoan");
        }

        private HttpResponseMessage Run(Func<MarketingService, object> action)
        {
            if (!HasPermission())
                return Json(HttpStatusCode.Forbidden, ApiResponse<object>.ErrorResponse("Bạn không có quyền xem Phân tích Marketing."));
            try
            {
                object data = action(new MarketingService(PortalSettings.PortalId));
                return Json(HttpStatusCode.OK, ApiResponse<object>.SuccessResponse(data));
            }
            catch (AnalyticsException ex)
            {
                return Json(HttpStatusCode.BadRequest, ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return Json(HttpStatusCode.BadRequest, ApiResponse<object>.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                DotNetNuke.Services.Exceptions.Exceptions.LogException(ex);
                return Json(HttpStatusCode.InternalServerError, ApiResponse<object>.ErrorResponse("Có lỗi xảy ra. Chi tiết đã được ghi log."));
            }
        }

        [HttpGet] public HttpResponseMessage Init() => Run(s => s.Init());

        [HttpGet]
        public HttpResponseMessage Overview(string range = "28d", DateTime? from = null, DateTime? to = null) => Run(s => s.Overview(range, from, to));

        /// <summary>Bảng bài / trang: group = article | page; q = lọc tiêu đề; segment = star | potential | optimize | normal | low; trend = up | down | new.</summary>
        [HttpGet]
        public HttpResponseMessage Content(string range = "28d", DateTime? from = null, DateTime? to = null, string group = "article", string q = null,
                                           string segment = null, string trend = null, int categoryId = 0, string sort = "views", string dir = "desc", int page = 1, int pageSize = 50)
            => Run(s => s.ContentList(range, from, to, group, q, segment, trend, categoryId, sort, dir, page, pageSize));

        /// <summary>Chi tiết một bài (key = "n{NewId}") hoặc một trang (key = đường dẫn).</summary>
        [HttpGet]
        public HttpResponseMessage Article(string key, string range = "28d", DateTime? from = null, DateTime? to = null) => Run(s => s.Article(range, from, to, key));

        /// <summary>dim = channel | sourceMedium | source | medium | campaign | content | term | landing.</summary>
        [HttpGet]
        public HttpResponseMessage Acquisition(string dim = "channel", string range = "28d", DateTime? from = null, DateTime? to = null) => Run(s => s.Acquisition(range, from, to, dim));

        [HttpGet]
        public HttpResponseMessage Campaigns(string range = "28d", DateTime? from = null, DateTime? to = null) => Run(s => s.Campaigns(range, from, to));

        [HttpGet]
        public HttpResponseMessage Campaign(string name, string range = "28d", DateTime? from = null, DateTime? to = null) => Run(s => s.Campaign(range, from, to, name));

        [HttpGet]
        public HttpResponseMessage Audience(string range = "28d", DateTime? from = null, DateTime? to = null) => Run(s => s.Audience(range, from, to));
    }
}
