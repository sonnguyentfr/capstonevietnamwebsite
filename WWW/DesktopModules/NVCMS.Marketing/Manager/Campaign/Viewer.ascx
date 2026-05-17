<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.Campaign" %>

<div class="nk-content">
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
                                            <a href="javascript:void(0);" class="btn btn-primary waves-effect waves-light btn-add-new"><span>Thêm mới</span></a>
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
                    <div class="row g-gs" id="campaignList">
                        <!-- Campaign cards will be loaded here via AJAX -->
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
                <h5 class="modal-title" id="modalTitle">Thêm mới / Chỉnh sửa</h5>
            </div>
            <div class="modal-body">
                <div class="form-validate is-alter">
                    <input type="hidden" id="hdfId" value="0" />
                    <div class="form-group">
                        <label class="form-label" for="txtTitle">Tiêu đề</label>
                        <div class="form-control-wrap">
                            <input type="text" id="txtTitle" class="form-control" required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="txtMota">Mô tả</label>
                        <div class="form-control-wrap">
                            <textarea id="txtMota" class="form-control" rows="3"></textarea>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <button type="button" id="btnSave" class="btn btn-primary">Cập nhật</button>
                <a href="javascript:void(0);" type="button" class="btn btn-secondary waves-effect" data-dismiss="modal">Hủy thao tác</a>
                <button type="button" id="btnDelete" class="btn btn-danger" style="display: none;">Xoá</button>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    var moduleId = <%= ModuleId %>;
    var sf = $.ServicesFramework(moduleId);
    var serviceRoot = "/DesktopModules/NVCMS/API/Campaign/";
    var tabUrl = "<%=NavigateURL()%>";

    $(document).ready(function () {
        loadCampaigns();
    });

    // =========================
    // LOAD ALL CAMPAIGNS
    // =========================
    function loadCampaigns() {
        $("#loading").show();
        $.ajax({
            type: "GET",
            url: serviceRoot + "GetAll",
            beforeSend: sf.setModuleHeaders,
            success: function (response) {
                $("#loading").hide();
                if (response.Success) {
                    renderCampaigns(response.Data);
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

    // =========================
    // RENDER CAMPAIGNS
    // =========================
    function renderCampaigns(campaigns) {
        var html = '';
        if (campaigns && campaigns.length > 0) {
            campaigns.forEach(function (item) {
                var emailListUrl = tabUrl + "?view=mail&itemid=" + item.id;
                html += '<div class="col-sm-6 col-lg-4 col-xxl-3">';
                html += '  <div class="card card-bordered h-100">';
                html += '    <div class="card-inner">';
                html += '      <div class="project">';
                html += '        <div class="project-head">';
                html += '          <a href="javascript:void(0);" class="project-title btn-edit" data-id="' + item.id + '">';
                html += '            <div class="project-info">';
                html += '              <h6 class="title">' + (item.Title || '') + '</h6>';
                html += '              <span class="sub-text">' + (item.Description || '') + '</span>';
                html += '            </div>';
                html += '          </a>';
                html += '          <div class="drodown">';
                html += '            <a href="#" class="dropdown-toggle btn btn-sm btn-icon btn-trigger mt-n1 mr-n1" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>';
                html += '            <div class="dropdown-menu dropdown-menu-right">';
                html += '              <ul class="link-list-opt no-bdr">';
                html += '                <li><a href="javascript:void(0);" class="btn-edit" data-id="' + item.id + '"><em class="icon ni ni-edit-fill"></em><span>Sửa</span></a></li>';
                html += '                <li><a href="' + emailListUrl + '"><em class="icon ni ni-emails-fill"></em><span>Danh sách email</span></a></li>';
                html += '                <li><a href="javascript:void(0);" class="btn-delete" data-id="' + item.id + '"><em class="icon ni ni-cross-sm"></em><span>Xóa</span></a></li>';
                html += '              </ul>';
                html += '            </div>';
                html += '          </div>';
                html += '        </div>';
                html += '        <div class="project-meta">';
                html += '          <span class="badge badge-dim badge-light text-gray fw-bold fs-16px"><em class="icon ni ni-clock"></em><span>Số lượng email: <mark>' + (item.soluongemail || 0) + '</mark></span></span>';
                html += '        </div>';
                html += '      </div>';
                html += '    </div>';
                html += '  </div>';
                html += '</div>';
            });
        } else {
            html = '<div class="col-12"><p class="text-center">Chưa có dữ liệu</p></div>';
        }
        $("#campaignList").html(html);
    }

    // =========================
    // ADD NEW
    // =========================
    $(document).on("click", ".btn-add-new", function () {
        $("#hdfId").val("0");
        $("#txtTitle").val("");
        $("#txtMota").val("");
        $("#btnDelete").hide();
        $("#modalTitle").text("Thêm mới Campaign");
        $("#modalEdit").modal("show");
    });

    // =========================
    // EDIT
    // =========================
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
                    $("#txtTitle").val(response.Data.Title || "");
                    $("#txtMota").val(response.Data.Description || "");
                    $("#btnDelete").show();
                    $("#modalTitle").text("Chỉnh sửa Campaign");
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

    // =========================
    // SAVE (INSERT / UPDATE)
    // =========================
    $("#btnSave").click(function (e) {
        e.preventDefault();
        var title = $("#txtTitle").val().trim();
        var mota = $("#txtMota").val().trim();

        if (title == "") {
            NioApp.Toast("Nhập tiêu đề campaign", 'warning', { position: 'top-right' });
            $("#txtTitle").focus();
            return false;
        }

        const id = parseInt($("#hdfId").val());
        const model = {
            id: id,
            Title: title,
            Description: mota,
            UserId: <%= UserId %>,
            PortalId: <%= PortalId %>
        };
        const url = id > 0 ? serviceRoot + "Update" : serviceRoot + "Insert";
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
                    loadCampaigns();
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

    // =========================
    // DELETE IN MODAL
    // =========================
    $("#btnDelete").click(function () {
        Swal.fire({
            title: 'Xác nhận xoá?',
            text: 'Bạn có muốn xoá campaign này không?',
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
                        loadCampaigns();
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

    // =========================
    // DELETE IN DROPDOWN
    // =========================
    $(document).on("click", ".btn-delete", function () {
        const id = $(this).data("id");
        Swal.fire({
            title: 'Xác nhận xoá?',
            text: 'Bạn có muốn xoá campaign này không?',
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
                        loadCampaigns();
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




