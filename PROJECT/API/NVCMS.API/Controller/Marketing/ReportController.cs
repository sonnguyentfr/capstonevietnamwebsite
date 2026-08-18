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
    public class ReportController : DnnApiController
    {
        /// <summary>
        /// Dashboard Campaign
        /// GET:
        /// /DesktopModules/NVCMS/API/Report/GetDashboard?campaignSendId=1256
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetDashboard(int campaignSendId)
        {
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
