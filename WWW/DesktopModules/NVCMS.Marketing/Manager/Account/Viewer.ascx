<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.Account" %>

<div class="nk-content ">
    <div class="container-fluid">
        <div class="nk-content-inner">
            <div class="nk-content-body">
                <div class="nk-block-head nk-block-head-sm">
                    <div class="nk-block-between">
                        <div class="nk-block-head-content">
                            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
                            <div class="nk-block-des text-soft">
                                <p>Tổng số có: <b><span id="totalRecords">0</span></b> bản ghi</p>
                            </div>
                        </div>
                        <!-- .nk-block-head-content -->
                        <div class="nk-block-head-content">
                            <div class="toggle-wrap nk-block-tools-toggle">
                                <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-menu-alt-r"></em></a>
                                <div class="toggle-expand-content" data-content="pageMenu">
                                    <ul class="nk-block-tools g-3">
                                        <li class="nk-block-tools-opt">
                                            <a href="javascript:void(0);" class="btn btn-primary waves-effect waves-light btn-add-new" id="btnAddNew"><span>Thêm mới</span></a>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <!-- .toggle-wrap -->
                        </div>
                        <!-- .nk-block-head-content -->
                    </div>
                    <!-- .nk-block-between -->
                </div>
                <!-- .nk-block-head -->
                <div class="nk-block">
                    <div class="row g-gs" id="accountList">
                        <!-- Account cards will be loaded here via AJAX -->
                    </div>
                </div>
                <div class="nk-block-des">
                    <br />
                    <a href="javascript:void(0);" class="btn btn-primary waves-effect waves-light btn-add-new">Thêm mới</a>
                </div>
                <!-- .nk-block -->
            </div>
        </div>
    </div>
</div>
<!-- .card-preview -->

<%--<div class="loading" id="loading" style="display:none;">Loading&#8230;</div>--%>
<div id="loading" style="display:none;">
    <div class="loading-spinner">   
        <div class="spinner-border text-primary" role="status">
            <span class="sr-only">Loading...</span>
        </div>
        <p class="mt-2 loadingtext">Đang tải dữ liệu. Vui lòng đợi trong giây lát...</p>
    </div>
</div>

<div class="modal fade zoom" tabindex="-1" id="modalEdit">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                <em class="icon ni ni-cross"></em>
            </a>
            <div class="modal-header">
                <h5 class="modal-title" id="modalTitle">Thêm mới / Chính sửa</h5>
            </div>
            <div class="modal-body">
                <div class="form-validate is-alter">
                    <input type="hidden" id="hdfId" value="0" />
                    <div class="form-group">
                        <label class="form-label" for="txtName">Tên gửi mail</label>
                        <div class="form-control-wrap">
                            <input type="text" id="txtName" class="form-control" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="txtMail">Email</label>
                        <div class="form-control-wrap">
                            <input type="email" id="txtMail" class="form-control" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="txtPass">Mật khẩu Email</label>
                        <div class="form-control-wrap">
                            <input type="password" id="txtPass" class="form-control" required />
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <button type="button" id="btnSave" class="btn btn-primary">Cập nhật</button>
                <a href="javascript:void(0);" type="button" class="btn btn-secondary waves-effect" data-dismiss="modal">Hủy thao tác</a>
                <button type="button" id="btnDelete" class="btn btn-danger" style="display:none;">Xoá</button>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    var moduleId = <%= ModuleId %>;
    var sf = $.ServicesFramework(moduleId);
    var serviceRoot = "/DesktopModules/NVCMS/API/Account/";

    $(document).ready(function () {
        loadAccounts();
    });

    function loadAccounts() {
        $("#loading").show();
        $.ajax({
            type: "GET",
            url: serviceRoot + "GetAll",
            beforeSend: sf.setModuleHeaders,
            success: function (response) {
                $("#loading").hide();
                if (response.Success) {
                    renderAccounts(response.Data);
                    $("#totalRecords").text(response.TotalRecords);
                } else {
                    NioApp.Toast(response.Message, 'danger', { position: 'top-right' });
                }
            },
            error: function (xhr, status, error) {
                $("#loading").hide();
                console.error("Error:", error);
                NioApp.Toast("Lỗi khi tải dữ liệu: " + error, 'danger', { position: 'top-right' });
            }
        });
    }

    function renderAccounts(accounts) {
        var html = '';
        if (accounts && accounts.length > 0) {
            accounts.forEach(function (account) {
                html += '<div class="col-sm-6 col-lg-4 col-xxl-3">';
                html += '  <div class="card card-bordered h-100">';
                html += '    <div class="card-inner">';
                html += '      <div class="project">';
                html += '        <div class="project-head">';
                html += '          <a href="javascript:void(0);" class="project-title btn-edit" data-id="' + account.id + '">';
                html += '            <div class="user-avatar sq bg-purple"><span>DD</span></div>';
                html += '            <div class="project-info">';
                html += '              <h6 class="title">' + (account.Name || '') + '</h6>';
                html += '              <span class="sub-text">' + (account.Mail || '') + '</span>';
                html += '            </div>';
                html += '          </a>';
                html += '          <div class="drodown">';
                html += '            <a href="#" class="dropdown-toggle btn btn-sm btn-icon btn-trigger mt-n1 mr-n1" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>';
                html += '            <div class="dropdown-menu dropdown-menu-right">';
                html += '              <ul class="link-list-opt no-bdr">';
                html += '                <li><a href="javascript:void(0);" class="btn-edit" data-id="' + account.id + '"><em class="icon ni ni-edit-fill"></em><span>Sửa</span></a></li>';
                html += '                <li><a href="javascript:void(0);" class="btn-delete" data-id="' + account.id + '"><em class="icon ni ni-cross-sm"></em><span>Xóa</span></a></li>';
                html += '              </ul>';
                html += '            </div>';
                html += '          </div>';
                html += '        </div>';
                html += '      </div>';
                html += '    </div>';
                html += '  </div>';
                html += '</div>';
            });
        } else {
            html = '<div class="col-12"><p class="text-center">Chưa có dữ liệu</p></div>';
        }
        $("#accountList").html(html);
    }

    $(document).on("click", ".btn-add-new", function () {
        $("#hdfId").val("0");
        $("#txtName").val("");
        $("#txtMail").val("");
        $("#txtPass").val("");
        $("#btnDelete").hide();
        $("#modalTitle").text("Thêm mới Account");
        $("#modalEdit").modal("show");
    });

    $(document).on("click", ".btn-edit", function () {
        const id = $(this).data("id");
        $("#loading").show();
        $.ajax({
            type: "GET",
            url: serviceRoot + "GetById?id=" + id,
            beforeSend: sf.setModuleHeaders,
            success: function (response) {
                $("#loading").hide();
                if (response.Success && response.Data) {
                    $("#hdfId").val(response.Data.id);
                    $("#txtName").val(response.Data.Name || "");
                    $("#txtMail").val(response.Data.Mail || "");
                    $("#txtPass").val(response.Data.Password || "");
                    $("#btnDelete").show();
                    $("#modalTitle").text("Chỉnh sửa Account");
                    $("#modalEdit").modal("show");
                } else {
                    NioApp.Toast(response.Message, 'danger', { position: 'top-right' });
                }
            },
            error: function (xhr, status, error) {
                $("#loading").hide();
                console.error("Error:", error);
                NioApp.Toast("Lỗi khi tải thông tin: " + error, 'danger', { position: 'top-right' });
            }
        });
    });

    $("#btnSave").click(function (e) {
        e.preventDefault();
        var name = $("#txtName").val().trim();
        var mail = $("#txtMail").val().trim();
        var pass = $("#txtPass").val().trim();

        if (name == "") {
            NioApp.Toast("Nhập tên hiển thị", 'warning', { position: 'top-right' });
            $("#txtName").focus();
            return false;
        }
        if (mail == "") {
            NioApp.Toast("Nhập địa chỉ email", 'warning', { position: 'top-right' });
            $("#txtMail").focus();
            return false;
        }
        if (pass == "") {
            NioApp.Toast("Nhập mật khẩu email", 'warning', { position: 'top-right' });
            $("#txtPass").focus();
            return false;
        }

        const id = parseInt($("#hdfId").val());
        const model = {
            id: id,
            Name: name,
            Mail: mail,
            Password: pass,
            UserId: <%= UserId %>,
            PortalId: <%= PortalId %>
        };
        const url    = id > 0 ? serviceRoot + "Update" : serviceRoot + "Insert";
        const action = id > 0 ? "Cập nhật" : "Thêm mới";

        $("#loading").show();
        $.ajax({
            type: "POST",
            url: url,
            beforeSend: sf.setModuleHeaders,
            contentType: "application/json",
            data: JSON.stringify(model),
            success: function (response) {
                $("#loading").hide();
                if (response.Success) {
                    $("#modalEdit").modal("hide");
                    UpdateSuccess(action + " thành công!");
                    loadAccounts();
                } else {
                    NioApp.Toast(response.Message, 'danger', { position: 'top-right' });
                }
            },
            error: function (xhr, status, error) {
                $("#loading").hide();
                console.error("Error:", error);
                NioApp.Toast("Lỗi khi " + action.toLowerCase() + ": " + error, 'danger', { position: 'top-right' });
            }
        });
    });

    $("#btnDelete").click(function () {
        Swal.fire({
            title: 'Xác nhận xoá?',
            text: 'Bạn có muốn xoá account này không?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xoá',
            cancelButtonText: 'Huỷ'
        }).then(function (result) {
            if (!result.isConfirmed) return;
            const id = parseInt($("#hdfId").val());
            $("#loading").show();
            $.ajax({
                type: "POST",
                url: serviceRoot + "Delete?id=" + id,
                beforeSend: sf.setModuleHeaders,
                contentType: "application/json",
                success: function (response) {
                    $("#loading").hide();
                    if (response.Success) {
                        $("#modalEdit").modal("hide");
                        UpdateSuccess("Xóa thành công!");
                        loadAccounts();
                    } else {
                        NioApp.Toast(response.Message, 'danger', { position: 'top-right' });
                    }
                },
                error: function (xhr, status, error) {
                    $("#loading").hide();
                    console.error("Error:", error);
                    NioApp.Toast("Lỗi khi xóa: " + error, 'danger', { position: 'top-right' });
                }
            });
        });
    });

    $(document).on("click", ".btn-delete", function () {
        const id = $(this).data("id");
        Swal.fire({
            title: 'Xác nhận xoá?',
            text: 'Bạn có muốn xoá account này không?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xoá',
            cancelButtonText: 'Huỷ'
        }).then(function (result) {
            if (!result.isConfirmed) return;
            $("#loading").show();
            $.ajax({
                type: "POST",
                url: serviceRoot + "Delete?id=" + id,
                beforeSend: sf.setModuleHeaders,
                contentType: "application/json",
                success: function (response) {
                    $("#loading").hide();
                    if (response.Success) {
                        UpdateSuccess("Xóa thành công!");
                        loadAccounts();
                    } else {
                        NioApp.Toast(response.Message, 'danger', { position: 'top-right' });
                    }
                },
                error: function (xhr, status, error) {
                    $("#loading").hide();
                    console.error("Error:", error);
                    NioApp.Toast("Lỗi khi xóa: " + error, 'danger', { position: 'top-right' });
                }
            });
        });
    });
</script>









