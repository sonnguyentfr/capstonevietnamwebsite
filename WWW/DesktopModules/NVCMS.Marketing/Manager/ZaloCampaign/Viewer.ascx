<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.ZaloCampaignViewer" %>

<div class="nk-content">
    <div class="container-fluid">
        <div class="nk-content-inner">
            <div class="nk-content-body">
                <div class="nk-block-head nk-block-head-sm">
                    <div class="nk-block-between">
                        <div class="nk-block-head-content">
                            <h3 class="nk-block-title page-title">Chiến dịch Zalo</h3>
                            <div class="nk-block-des text-soft">
                                <p>Tổng số: <b><span id="totalRecords">0</span></b> chiến dịch</p>
                            </div>
                        </div>
                        <div class="nk-block-head-content">
                            <ul class="nk-block-tools g-3">
                                <li class="nk-block-tools-opt">
                                    <a href="javascript:void(0);" onclick="openModalAdd()" class="btn btn-primary"><em class="icon ni ni-plus"></em><span>Thêm chiến dịch</span></a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="nk-block">
                    <div class="row g-gs" id="campaignList">
                        <!-- Danh sach campaign Zalo se duoc render qua AJAX -->
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>

<div id="loading" style="display:none; text-align:center; padding:30px;">
    <div class="spinner-border text-primary" role="status"></div>
    <p class="mt-2">Đang tải dữ liệu...</p>
</div>

<!-- Modal them/sua campaign -->
<div class="modal fade zoom" tabindex="-1" id="modalEdit">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <a href="#" class="close" data-dismiss="modal" aria-label="Close"><em class="icon ni ni-cross"></em></a>
            <div class="modal-header">
                <h5 class="modal-title" id="modalTitle">Thêm mới / Chỉnh sửa chiến dịch Zalo</h5>
            </div>
            <div class="modal-body">
                <div class="form-validate is-alter">
                    <input type="hidden" id="hdfId" value="0" />
                    <div class="form-group">
                        <label class="form-label" for="txtTitle">Tiêu đề <span class="text-danger">*</span></label>
                        <div class="form-control-wrap">
                            <input type="text" id="txtTitle" class="form-control" placeholder="Nhập tiêu đề chiến dịch..." required />
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="txtMota">Mô tả</label>
                        <div class="form-control-wrap">
                            <textarea id="txtMota" class="form-control" rows="3" placeholder="Mô tả ngắn về chiến dịch..."></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="selStatus">Trạng thái</label>
                        <div class="form-control-wrap">
                            <select id="selStatus" class="form-control">
                                <option value="0">Nháp</option>
                                <option value="1">Đang hoạt động</option>
                                <option value="2">Đã gửi</option>
                            </select>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <button type="button" id="btnSave" class="btn btn-primary" onclick="saveCampaign()">Lưu</button>
                <button type="button" id="btnDelete" class="btn btn-danger" style="display:none;" onclick="deleteCampaign()">Xoá</button>
                <a href="javascript:void(0);" class="btn btn-secondary" data-dismiss="modal">Hủy</a>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    var moduleId = <%= ModuleId %>;
    var sf = $.ServicesFramework(moduleId);
    var serviceRoot = "/DesktopModules/NVCMS/API/ZaloCampaign/";
    var tabUrl = "<%=NavigateURL()%>";

    $(document).ready(function () {
        loadCampaigns();
    });

    function loadCampaigns() {
        $("#loading").show();
        $.ajax({
            type: "GET",
            url: serviceRoot + "GetAll",
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                $("#loading").hide();
                if (res.Success) {
                    renderCampaigns(res.Data);
                    $("#totalRecords").text(res.TotalRecords);
                } else {
                    NioApp.Toast(res.Message, 'danger', { position: 'top-right' });
                }
            },
            error: function () {
                $("#loading").hide();
                NioApp.Toast('Lỗi khi tải dữ liệu', 'danger', { position: 'top-right' });
            }
        });
    }

    function renderCampaigns(data) {
        var html = '';
        if (!data || data.length === 0) {
            html = '<div class="col-12"><div class="alert alert-info">Chưa có chiến dịch nào. Hãy thêm mới!</div></div>';
        } else {
            $.each(data, function (i, c) {
                var statusBadge = '<span class="badge ' + getStatusBadge(c.Status) + '">' + getStatusLabel(c.Status) + '</span>';
                var listUrl = tabUrl + (tabUrl.indexOf('?') >= 0 ? '&' : '?') + 'view=listSdt&campaignId=' + c.Id;
                var statUrl = tabUrl + (tabUrl.indexOf('?') >= 0 ? '&' : '?') + 'view=static&campaignId=' + c.Id;
                html += '<div class="col-md-4">';
                html += '  <div class="card card-bordered h-100">';
                html += '    <div class="card-inner">';
                html += '      <div class="card-title-group mb-1">';
                html += '        <div class="card-title"><h6 class="title">' + escapeHtml(c.Title) + '</h6></div>';
                html += '        <div class="card-tools">' + statusBadge + '</div>';
                html += '      </div>';
                html += '      <p class="text-soft small mb-2">' + escapeHtml(c.Description || '') + '</p>';
                html += '      <div class="d-flex justify-content-between align-items-center mt-2">';
                html += '        <span class="text-muted small"><em class="icon ni ni-phone"></em> ' + (c.TotalSdt || 0) + ' số ĐT</span>';
                html += '        <div>';
                html += '          <a href="' + listUrl + '" class="btn btn-sm btn-outline-primary mr-1" title="Quản lý số ĐT"><em class="icon ni ni-list"></em></a>';
                html += '          <a href="' + statUrl + '" class="btn btn-sm btn-outline-info mr-1" title="Thống kê chiến dịch"><em class="icon ni ni-bar-chart"></em></a>';
                html += '          <a href="javascript:void(0);" onclick="editCampaign(' + c.Id + ')" class="btn btn-sm btn-outline-warning mr-1" title="Sửa"><em class="icon ni ni-edit"></em></a>';
                html += '          <a href="javascript:void(0);" onclick="confirmDelete(' + c.Id + ')" class="btn btn-sm btn-outline-danger" title="Xoá"><em class="icon ni ni-trash"></em></a>';
                html += '        </div>';
                html += '      </div>';
                html += '    </div>';
                html += '  </div>';
                html += '</div>';
            });
        }
        $("#campaignList").html(html);
    }

    function getStatusLabel(s) {
        switch (parseInt(s)) {
            case 0: return 'Nháp';
            case 1: return 'Hoạt động';
            case 2: return 'Đã gửi';
            default: return 'N/A';
        }
    }

    function getStatusBadge(s) {
        switch (parseInt(s)) {
            case 0: return 'badge-secondary';
            case 1: return 'badge-success';
            case 2: return 'badge-info';
            default: return 'badge-light';
        }
    }

    function openModalAdd() {
        $("#hdfId").val(0);
        $("#txtTitle").val('');
        $("#txtMota").val('');
        $("#selStatus").val(0);
        $("#modalTitle").text('Thêm mới chiến dịch Zalo');
        $("#btnDelete").hide();
        $("#modalEdit").modal('show');
    }

    function editCampaign(id) {
        $.ajax({
            type: "GET",
            url: serviceRoot + "GetByID",
            data: { id: id },
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res.Success) {
                    var c = res.Data;
                    $("#hdfId").val(c.Id);
                    $("#txtTitle").val(c.Title);
                    $("#txtMota").val(c.Description);
                    $("#selStatus").val(c.Status);
                    $("#modalTitle").text('Chỉnh sửa chiến dịch Zalo');
                    $("#btnDelete").show().data("id", c.Id);
                    $("#modalEdit").modal('show');
                }
            }
        });
    }

    function saveCampaign() {
        var id = parseInt($("#hdfId").val()) || 0;
        var title = $.trim($("#txtTitle").val());
        if (!title) { alert('Vui lòng nhập tiêu đề!'); return; }

        var payload = {
            Id: id,
            Title: title,
            Description: $("#txtMota").val(),
            Status: parseInt($("#selStatus").val())
        };

        var url = id > 0 ? serviceRoot + "Update" : serviceRoot + "Insert";
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json",
            data: JSON.stringify(payload),
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res.Success) {
                    $("#modalEdit").modal('hide');
                    loadCampaigns();
                    NioApp.Toast(res.Message || 'Lưu thành công!', 'success', { position: 'top-right' });
                } else {
                    NioApp.Toast(res.Message || 'Có lỗi xảy ra!', 'danger', { position: 'top-right' });
                }
            }
        });
    }

    function confirmDelete(id) {
        if (confirm('Bạn có chắc muốn xóa chiến dịch này? Toàn bộ số điện thoại cũng sẽ bị xóa!')) {
            $.ajax({
                type: "POST",
                url: serviceRoot + "Delete",
                contentType: "application/json",
                data: JSON.stringify({ Id: id }),
                beforeSend: sf.setModuleHeaders,
                success: function (res) {
                    loadCampaigns();
                    NioApp.Toast(res.Message || 'Xóa thành công!', 'success', { position: 'top-right' });
                }
            });
        }
    }

    function deleteCampaign() {
        confirmDelete(parseInt($("#btnDelete").data("id")));
        $("#modalEdit").modal('hide');
    }

    function escapeHtml(s) {
        return String(s).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;');
    }
</script>
