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
                    ApiResponse<DashboardResult>.ErrorResponse(
                        "campaignSendId phải lớn hơn 0."));
            }

            try
            {
                var data = MarketingReportServices.GetDashboard(campaignSendId);

                var response = ApiResponse<DashboardResult>.SuccessResponse(
                    data,
                    "Load dashboard thành công",
                    1);

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<DashboardResult>.ErrorResponse(ex.Message);

                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    response);
            }
        }
    //    [HttpGet]
    //    public HttpResponseMessage GetStatus(int campaignId)
    //    {
    //        try
    //        {
    //            var data = MarketingReportServices.GetStatus(campaignId);

    //            return Request.CreateResponse(
    //                HttpStatusCode.OK,
    //                ApiResponse<List<DashboardStatus>>
    //                .SuccessResponse(data, "", data.Count));
    //        }
    //        catch (Exception ex)
    //        {
    //            return Request.CreateResponse(
    //                HttpStatusCode.InternalServerError,
    //                ApiResponse<List<DashboardStatus>>
    //                .ErrorResponse(ex.Message));
    //        }
    //    }
    //    [HttpGet]
    //    public HttpResponseMessage GetTimeline(int campaignId)
    //    {
    //        try
    //        {
    //            var data = MarketingReportServices.GetTimeline(campaignId);

    //            return Request.CreateResponse(
    //                HttpStatusCode.OK,
    //                ApiResponse<List<DashboardTimeline>>
    //                .SuccessResponse(data, "", data.Count));
    //        }
    //        catch (Exception ex)
    //        {
    //            return Request.CreateResponse(
    //                HttpStatusCode.InternalServerError,
    //                ApiResponse<List<DashboardTimeline>>
    //                .ErrorResponse(ex.Message));
    //        }
    //    }
    //    [HttpGet]
    //    public HttpResponseMessage GetDetails(
    //int campaignId,
    //int page = 1,
    //int pageSize = 20,
    //string keyword = "",
    //string status = "")
    //    {
    //        try
    //        {
    //            var data = MarketingReportServices.GetDetails(
    //                campaignId,
    //                page,
    //                pageSize,
    //                keyword,
    //                status);

    //            return Request.CreateResponse(
    //                HttpStatusCode.OK,
    //                ApiResponse<PagedResult<DashboardDetail>>
    //                .SuccessResponse(data, "", data.TotalRecords));
    //        }
    //        catch (Exception ex)
    //        {
    //            return Request.CreateResponse(
    //                HttpStatusCode.InternalServerError,
    //                ApiResponse<PagedResult<DashboardDetail>>
    //                .ErrorResponse(ex.Message));
    //        }
    //    }
    //    [HttpPost]
    //    public HttpResponseMessage Refresh(int campaignId)
    //    {
    //        try
    //        {
    //            MarketingReportServices.Refresh(campaignId);

    //            return Request.CreateResponse(
    //                HttpStatusCode.OK,
    //                ApiResponse<bool>.SuccessResponse(
    //                    true,
    //                    "Refresh thành công",
    //                    1));
    //        }
    //        catch (Exception ex)
    //        {
    //            return Request.CreateResponse(
    //                HttpStatusCode.InternalServerError,
    //                ApiResponse<bool>.ErrorResponse(ex.Message));
    //        }
    //    }
    }
}
