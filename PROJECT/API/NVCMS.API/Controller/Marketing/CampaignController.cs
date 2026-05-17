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
    public class CampaignController : DnnApiController
    {
        /// <summary>
        /// Lấy tất cả danh sách campaign
        /// GET: /api/Campaign/GetAll
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var data = CampaignServices.GetAll();
                var response = ApiResponse<List<Marketing_Mail_Campaing_ViewInfo>>.SuccessResponse(
                    data,
                    "Lấy danh sách campaign thành công",
                    data.Count
                );
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<List<Marketing_Mail_Campaing_ViewInfo>>.ErrorResponse(
                    "Lỗi khi lấy danh sách: " + ex.Message
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Lấy campaign theo ID
        /// GET: /api/Campaign/GetById?id=1
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var errorResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse("ID không hợp lệ");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                var data = CampaignServices.GetById(id);

                if (data == null)
                {
                    var notFoundResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse(
                        "Không tìm thấy campaign với ID = " + id
                    );
                    return Request.CreateResponse(HttpStatusCode.NotFound, notFoundResponse);
                }

                var response = ApiResponse<Marketing_Mail_CampaingInfo>.SuccessResponse(
                    data,
                    "Lấy thông tin campaign thành công"
                );
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse(
                    "Lỗi khi lấy thông tin: " + ex.Message
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Thêm mới campaign
        /// POST: /api/Campaign/Insert
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Insert(Marketing_Mail_CampaingInfo model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Title))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse("Tiêu đề không được để trống");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (CampaignServices.IsTitleExist(model.Title))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse("Tiêu đề đã tồn tại trong hệ thống");
                    return Request.CreateResponse(HttpStatusCode.Conflict, errorResponse);
                }

                model.CreatedDate = DateTime.Now;
                CampaignServices.Insert(model);

                var response = ApiResponse<Marketing_Mail_CampaingInfo>.SuccessResponse(
                    model,
                    "Tạo campaign thành công"
                );
                return Request.CreateResponse(HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse(
                    "Lỗi khi tạo campaign: " + ex.Message
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Cập nhật campaign
        /// POST: /api/Campaign/Update
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Update(Marketing_Mail_CampaingInfo model)
        {
            try
            {
                if (model.id <= 0)
                {
                    var errorResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse("ID không hợp lệ");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (string.IsNullOrEmpty(model.Title))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse("Tiêu đề không được để trống");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (!CampaignServices.IsExist(model.id))
                {
                    var notFoundResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse(
                        "Không tìm thấy campaign với ID = " + model.id
                    );
                    return Request.CreateResponse(HttpStatusCode.NotFound, notFoundResponse);
                }

                if (CampaignServices.IsTitleExist(model.Title, model.id))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse("Tiêu đề đã tồn tại trong hệ thống");
                    return Request.CreateResponse(HttpStatusCode.Conflict, errorResponse);
                }
                model.CreatedDate = DateTime.Now;
                CampaignServices.Update(model);

                var response = ApiResponse<Marketing_Mail_CampaingInfo>.SuccessResponse(
                    model,
                    "Cập nhật campaign thành công"
                );
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Mail_CampaingInfo>.ErrorResponse(
                    "Lỗi khi cập nhật campaign: " + ex.Message
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Xóa campaign
        /// POST: /api/Campaign/Delete
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var errorResponse = ApiResponse<bool>.ErrorResponse("ID không hợp lệ");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (!CampaignServices.IsExist(id))
                {
                    var notFoundResponse = ApiResponse<bool>.ErrorResponse(
                        "Không tìm thấy campaign với ID = " + id
                    );
                    return Request.CreateResponse(HttpStatusCode.NotFound, notFoundResponse);
                }

                CampaignServices.Delete(id);

                var response = ApiResponse<bool>.SuccessResponse(true, "Xóa campaign thành công");
                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<bool>.ErrorResponse("Lỗi khi xóa campaign: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }
    }
}