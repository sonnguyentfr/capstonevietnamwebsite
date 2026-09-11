using DotNetNuke.Web.Api;
using NVCMS.API.Marketing.Services.Marketing;
using NVCMS.API.Model;
using NVCMS.Modules.Marketing;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NVCMS.API.Controller
{
    [DnnAuthorize]
    [ValidateAntiForgeryToken]
    public class ZaloCampaignController : DnnApiController
    {
        // ============================================================
        // CAMPAIGN ENDPOINTS
        // ============================================================

        /// <summary>GET /api/ZaloCampaign/GetAll</summary>
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var data = ZaloCampaignServices.GetAll(PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<List<Marketing_Zalo_CampaignInfo>>.SuccessResponse(data, "OK", data.Count));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse("Lỗi: " + ex.Message));
            }
        }

        /// <summary>GET /api/ZaloCampaign/GetByID?id=1</summary>
        [HttpGet]
        public HttpResponseMessage GetByID(int id)
        {
            try
            {
                if (id <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("ID không hợp lệ"));

                var data = ZaloCampaignServices.GetById(id);
                if (data == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound,
                        ApiResponse<object>.ErrorResponse("Không tìm thấy campaign"));

                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<Marketing_Zalo_CampaignInfo>.SuccessResponse(data, "OK"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>POST /api/ZaloCampaign/Insert</summary>
        [HttpPost]
        public HttpResponseMessage Insert(Marketing_Zalo_CampaignInfo model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model?.Title))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("Tiêu đề không được để trống"));

                model.UserId = UserInfo.UserID;
                model.PortalId = PortalSettings.PortalId;
                int newId = ZaloCampaignServices.Insert(model);
                model.Id = newId;

                return Request.CreateResponse(HttpStatusCode.Created,
                    ApiResponse<Marketing_Zalo_CampaignInfo>.SuccessResponse(model, "Tạo chiến dịch thành công"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>POST /api/ZaloCampaign/Update</summary>
        [HttpPost]
        public HttpResponseMessage Update(Marketing_Zalo_CampaignInfo model)
        {
            try
            {
                if (model?.Id <= 0 || string.IsNullOrWhiteSpace(model?.Title))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("Dữ liệu không hợp lệ"));

                model.UserId = UserInfo.UserID;
                model.PortalId = PortalSettings.PortalId;
                ZaloCampaignServices.Update(model);

                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<Marketing_Zalo_CampaignInfo>.SuccessResponse(model, "Cập nhật thành công"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>POST /api/ZaloCampaign/Delete  body: { Id: 1 }</summary>
        [HttpPost]
        public HttpResponseMessage Delete(Marketing_Zalo_CampaignInfo model)
        {
            try
            {
                if (model?.Id <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("ID không hợp lệ"));

                ZaloCampaignServices.Delete(model.Id);
                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<object>.SuccessResponse(null, "Xóa thành công"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        // ============================================================
        // PHONE LIST ENDPOINTS
        // ============================================================

        /// <summary>
        /// GET /api/ZaloCampaign/GetSdtList?campaignId=1&amp;keySearch=&amp;status=-1&amp;pageIndex=0&amp;pageSize=50
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetSdtList(int campaignId, string keySearch = "", int status = -1, int pageIndex = 0, int pageSize = 50)
        {
            try
            {
                var data = ZaloListSdtServices.GetAll(campaignId, keySearch, status, pageIndex, pageSize);
                int total = data.Count > 0 ? data[0].TotalRecords : 0;
                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<List<Marketing_Zalo_ListSdtInfo>>.SuccessResponse(data, "OK", total));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>POST /api/ZaloCampaign/AddPhone  body: { CampaignId, PhoneRaw }</summary>
        [HttpPost]
        public HttpResponseMessage AddPhone(AddPhoneRequest req)
        {
            try
            {
                if (req?.CampaignId <= 0 || string.IsNullOrWhiteSpace(req?.PhoneRaw))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("Dữ liệu không hợp lệ"));

                string errorMsg;
                int result = ZaloListSdtServices.AddPhone(req.CampaignId, req.PhoneRaw, UserInfo.UserID, PortalSettings.PortalId, out errorMsg);

                if (result == -2)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse(errorMsg));

                if (result == -1)
                    return Request.CreateResponse(HttpStatusCode.Conflict,
                        ApiResponse<object>.ErrorResponse("Số điện thoại đã tồn tại trong chiến dịch này"));

                var normalized = ZaloListSdtServices.NormalizePhone(req.PhoneRaw);
                return Request.CreateResponse(HttpStatusCode.Created,
                    ApiResponse<object>.SuccessResponse(new { Id = result, Phone = normalized }, "Thêm SĐT thành công"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// POST /api/ZaloCampaign/ValidatePhoneList  
        /// body: { PhoneList: "0901234567\n0912345678\n..." }
        /// Preview danh sách mà không lưu DB
        /// </summary>
        [HttpPost]
        public HttpResponseMessage ValidatePhoneList(ValidatePhoneRequest req)
        {
            try
            {
                var result = ZaloListSdtServices.ValidatePhoneList(req?.PhoneList ?? "");
                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<ZaloPhoneValidateResult>.SuccessResponse(result, "OK"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>
        /// POST /api/ZaloCampaign/AddPhoneBulk  
        /// body: { CampaignId, PhoneList: "0901234567\n0912345678\n..." }
        /// </summary>
        [HttpPost]
        public HttpResponseMessage AddPhoneBulk(AddPhoneBulkRequest req)
        {
            try
            {
                if (req?.CampaignId <= 0 || string.IsNullOrWhiteSpace(req?.PhoneList))
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("Dữ liệu không hợp lệ"));

                var result = ZaloListSdtServices.AddBulk(req.CampaignId, req.PhoneList, UserInfo.UserID, PortalSettings.PortalId);
                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<Marketing_Zalo_ListSdt_BulkResult>.SuccessResponse(result,
                        $"Đã thêm {result.InsertCount} SĐT. Trùng/bỏ qua: {result.DupCount}"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>POST /api/ZaloCampaign/DeletePhone  body: { Id }</summary>
        [HttpPost]
        public HttpResponseMessage DeletePhone(DeletePhoneRequest req)
        {
            try
            {
                if (req?.Id <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("ID không hợp lệ"));
                ZaloListSdtServices.Delete(req.Id);
                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<object>.SuccessResponse(null, "Xóa thành công"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }

        /// <summary>POST /api/ZaloCampaign/DeleteAllPhone  body: { CampaignId }</summary>
        [HttpPost]
        public HttpResponseMessage DeleteAllPhone(DeleteAllPhoneRequest req)
        {
            try
            {
                if (req?.CampaignId <= 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        ApiResponse<object>.ErrorResponse("CampaignId không hợp lệ"));
                ZaloListSdtServices.DeleteByCampaignId(req.CampaignId);
                return Request.CreateResponse(HttpStatusCode.OK,
                    ApiResponse<object>.SuccessResponse(null, "Đã xóa toàn bộ SĐT"));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,
                    ApiResponse<object>.ErrorResponse(ex.Message));
            }
        }
    }

    // ============================================================
    // Request Models
    // ============================================================
    public class AddPhoneRequest
    {
        public int CampaignId { get; set; }
        public string PhoneRaw { get; set; }
    }

    public class AddPhoneBulkRequest
    {
        public int CampaignId { get; set; }
        public string PhoneList { get; set; }
    }

    public class ValidatePhoneRequest
    {
        public string PhoneList { get; set; }
    }

    public class DeletePhoneRequest
    {
        public int Id { get; set; }
    }

    public class DeleteAllPhoneRequest
    {
        public int CampaignId { get; set; }
    }
}
