using DotNetNuke.Web.Api;
using NVCMS.API.Marketing.Services.Marketing;
using NVCMS.API.Model;
using NVCMS.Modules.Marketing;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;
namespace NVCMS.API.Controller
{
    [DnnAuthorize]
    [ValidateAntiForgeryToken]
    public class ReportController : DnnApiController
    {
        private bool HasReportPermission()
        {
            return UserInfo.IsSuperUser
                || UserInfo.IsInRole("Administrators")
                || UserInfo.IsInRole("Manager")
                || UserInfo.IsInRole("Xuat ban")
                || UserInfo.IsInRole("LanhDaoToaSoan");
        }
        [HttpGet]
        public HttpResponseMessage GetDashboard(int campaignSendId)
        {
            if (!HasReportPermission())
            {
                return Request.CreateResponse(
                    HttpStatusCode.Forbidden,
                    "Bạn không có quyền truy cập API Report.");
            }
            if (campaignSendId <= 0)
            {
                return Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    ApiResponse<Marketing_Mail_CampaignAnalyticsResult>.ErrorResponse(
                        "campaignSendId phải lớn hơn 0."));
            }

            try
            {
                var data = MarketingReportServices.GetDashboard(campaignSendId);

                var response = ApiResponse<Marketing_Mail_CampaignAnalyticsResult>.SuccessResponse(
                    data,
                    "Load dashboard thành công",
                    1);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Mail_CampaignAnalyticsResult>.ErrorResponse(ex.Message);
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    response);
            }
        }

    }
}
