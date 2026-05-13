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
    public class AccountController : DnnApiController
    {
        /// <summary>
        /// Lấy tất cả danh sách account
        /// GET: /api/Account/GetAll
        /// </summary>
        /// 
        [HttpGet]
        public HttpResponseMessage GetAll()
        {
            try
            {
                var data = AccountServices.GetAll();
                var response = ApiResponse<List<Marketing_Mail_AccountInfo>>.SuccessResponse(
                    data,
                    "Lấy danh sách account thành công",
                    data.Count
                );

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<List<Marketing_Mail_AccountInfo>>.ErrorResponse(
                    $"Lỗi khi lấy danh sách: {ex.Message}"
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Lấy account theo ID
        /// GET: /api/Account/GetById?id=1
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("ID không hợp lệ");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                var data = AccountServices.GetById(id);

                if (data == null)
                {
                    var notFoundResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse(
                        $"Không tìm thấy account với ID = {id}"
                    );
                    return Request.CreateResponse(HttpStatusCode.NotFound, notFoundResponse);
                }

                var response = ApiResponse<Marketing_Mail_AccountInfo>.SuccessResponse(
                    data,
                    "Lấy thông tin account thành công"
                );

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse(
                    $"Lỗi khi lấy thông tin: {ex.Message}"
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Lấy account theo UserId
        /// GET: /api/Account/GetByUserId?userId=1
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetByUserId(int userId)
        {
            try
            {
                var data = AccountServices.GetByUserId(userId);
                var response = ApiResponse<List<Marketing_Mail_AccountInfo>>.SuccessResponse(
                    data,
                    "Lấy danh sách account theo user thành công",
                    data.Count
                );

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<List<Marketing_Mail_AccountInfo>>.ErrorResponse(
                    $"Lỗi khi lấy danh sách: {ex.Message}"
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Lấy account theo PortalId
        /// GET: /api/Account/GetByPortalId?portalId=1
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetByPortalId(int portalId)
        {
            try
            {
                var data = AccountServices.GetByPortalId(portalId);
                var response = ApiResponse<List<Marketing_Mail_AccountInfo>>.SuccessResponse(
                    data,
                    "Lấy danh sách account theo portal thành công",
                    data.Count
                );

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<List<Marketing_Mail_AccountInfo>>.ErrorResponse(
                    $"Lỗi khi lấy danh sách: {ex.Message}"
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Thêm mới account
        /// POST: /api/Account/Insert
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Insert(Marketing_Mail_AccountInfo model)
        {
            try
            {
                // Validation
                if (string.IsNullOrEmpty(model.Name))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("Tên không được để trống");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (string.IsNullOrEmpty(model.Mail))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("Email không được để trống");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                // Kiểm tra email đã tồn tại
                if (AccountServices.IsEmailExist(model.Mail))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("Email đã tồn tại trong hệ thống");
                    return Request.CreateResponse(HttpStatusCode.Conflict, errorResponse);
                }

                AccountServices.Insert(model);

                var response = ApiResponse<Marketing_Mail_AccountInfo>.SuccessResponse(
                    model,
                    "Tạo account thành công"
                );

                return Request.CreateResponse(HttpStatusCode.Created, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse(
                    $"Lỗi khi tạo account: {ex.Message}"
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Cập nhật account
        /// POST: /api/Account/Update
        /// </summary>
        [HttpPost]
        public HttpResponseMessage Update(Marketing_Mail_AccountInfo model)
        {
            try
            {
                // Validation
                if (model.id <= 0)
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("ID không hợp lệ");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (string.IsNullOrEmpty(model.Name))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("Tên không được để trống");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (string.IsNullOrEmpty(model.Mail))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("Email không được để trống");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                // Kiểm tra account có tồn tại
                if (!AccountServices.IsExist(model.id))
                {
                    var notFoundResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse(
                        $"Không tìm thấy account với ID = {model.id}"
                    );
                    return Request.CreateResponse(HttpStatusCode.NotFound, notFoundResponse);
                }

                // Kiểm tra email đã tồn tại (exclude ID hiện tại)
                if (AccountServices.IsEmailExist(model.Mail, model.id))
                {
                    var errorResponse = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse("Email đã tồn tại trong hệ thống");
                    return Request.CreateResponse(HttpStatusCode.Conflict, errorResponse);
                }

                AccountServices.Update(model);

                var response = ApiResponse<Marketing_Mail_AccountInfo>.SuccessResponse(
                    model,
                    "Cập nhật account thành công"
                );

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<Marketing_Mail_AccountInfo>.ErrorResponse(
                    $"Lỗi khi cập nhật account: {ex.Message}"
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Xóa account
        /// POST: /api/Account/Delete
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

                // Kiểm tra account có tồn tại
                if (!AccountServices.IsExist(id))
                {
                    var notFoundResponse = ApiResponse<bool>.ErrorResponse(
                        $"Không tìm thấy account với ID = {id}"
                    );
                    return Request.CreateResponse(HttpStatusCode.NotFound, notFoundResponse);
                }

                AccountServices.Delete(id);

                var response = ApiResponse<bool>.SuccessResponse(
                    true,
                    "Xóa account thành công"
                );

                return Request.CreateResponse(HttpStatusCode.OK, response);
            }
            catch (Exception ex)
            {
                var response = ApiResponse<bool>.ErrorResponse(
                    $"Lỗi khi xóa account: {ex.Message}"
                );
                return Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
        }

        /// <summary>
        /// Xóa account theo model
        /// POST: /api/Account/DeleteByModel
        /// </summary>
        [HttpPost]
        public HttpResponseMessage DeleteByModel(Marketing_Mail_AccountInfo model)
        {
            return Delete(model.id);
        }
    }
}