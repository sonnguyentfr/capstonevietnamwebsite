using DotNetNuke.Web.Api;
using NVCMS.API.Marketing.Services.Marketing;
using NVCMS.API.Model;
using NVCMS.Modules.Marketing;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NVCMS.API.Controller
{
    [DnnAuthorize]
    [ValidateAntiForgeryToken]
    public class ZaloReportController : DnnApiController
    {
        private bool HasReportPermission()
        {
            return UserInfo.IsSuperUser
                || UserInfo.IsInRole("Administrators")
                || UserInfo.IsInRole("Manager")
                || UserInfo.IsInRole("Xuat ban")
                || UserInfo.IsInRole("LanhDaoToaSoan");
        }

        /// <summary>GET /DesktopModules/NVCMS/API/ZaloReport/GetDashboard?campaignId=1</summary>
        [HttpGet]
        public HttpResponseMessage GetDashboard(int campaignId)
        {
            if (!HasReportPermission())
            {
                return Request.CreateResponse(
                    HttpStatusCode.Forbidden,
                    ApiResponse<object>.ErrorResponse("Bạn không có quyền truy cập API Report Zalo."));
            }

            if (campaignId <= 0)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    ApiResponse<Marketing_Zalo_CampaignAnalyticsResult>.ErrorResponse(
                        "campaignId phải lớn hơn 0."));
            }

            try
            {
                var data = MarketingZaloReportServices.GetDashboard(campaignId);

                var response = ApiResponse<Marketing_Zalo_CampaignAnalyticsResult>.SuccessResponse(
                    data,
                    "Load dashboard Zalo thành công",
                    data.Details.Count);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Zalo_CampaignAnalyticsResult>.ErrorResponse(ex.Message);
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    response);
            }
        }
    }
}
