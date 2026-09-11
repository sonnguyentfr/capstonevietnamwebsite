<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="ListSdt.ascx.vb" Inherits="NVCMS.Modules.Marketing.ZaloListSdtViewer" %>

<div class="nk-content">
    <div class="container-fluid">
        <div class="nk-content-inner">
            <div class="nk-content-body">

                <!-- Header -->
                <div class="nk-block-head nk-block-head-sm">
                    <div class="nk-block-between">
                        <div class="nk-block-head-content">
                            <h3 class="nk-block-title page-title">
                                <a href="<%=BackUrl%>" class="btn btn-icon btn-sm btn-outline-light mr-2" title="Quay lại">
                                    <em class="icon ni ni-arrow-left"></em>
                                </a>
                                Danh sách SĐT - <span id="spnCampaignTitle">...</span>
                            </h3>
                            <div class="nk-block-des text-soft">
                                Tổng: <b><span id="spnTotal">0</span></b> số &nbsp;|&nbsp;
                                Hợp lệ đã thêm: <b><span id="spnInserted">0</span></b> &nbsp;|&nbsp;
                                Trùng bỏ qua: <b><span id="spnDup">0</span></b>
                            </div>
                        </div>
                        <div class="nk-block-head-content">
                            <ul class="nk-block-tools g-2">
                                <li>
                                    <a href="javascript:void(0);" onclick="openAddSingle()" class="btn btn-sm btn-outline-primary">
                                        <em class="icon ni ni-plus"></em> Thêm 1 SĐT
                                    </a>
                                </li>
                                <li>
                                    <a href="javascript:void(0);" onclick="openAddBulk()" class="btn btn-sm btn-primary">
                                        <em class="icon ni ni-upload"></em> Thêm nhiều SĐT
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>

                <!-- Filter bar -->
                <div class="card card-bordered mb-3">
                    <div class="card-inner py-2">
                        <div class="row g-2 align-items-center">
                            <div class="col-md-4">
                                <input type="text" id="txtSearch" class="form-control form-control-sm" placeholder="Tìm SĐT..." />
                            </div>
                            <div class="col-md-3">
                                <select id="selStatusFilter" class="form-control form-control-sm">
                                    <option value="-1">-- Tất cả trạng thái --</option>
                                    <option value="0">Chờ gửi</option>
                                    <option value="1">Đã gửi</option>
                                    <option value="2">Lỗi</option>
                                </select>
                            </div>
                            <div class="col-auto">
                                <button class="btn btn-sm btn-primary" onclick="loadSdtList(0)"><em class="icon ni ni-search"></em> Tìm</button>
                                <button class="btn btn-sm btn-outline-danger ml-1" onclick="confirmDeleteAll()"><em class="icon ni ni-trash"></em> Xóa hết</button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Table -->
                <div class="nk-block">
                    <div class="card card-bordered">
                        <div class="card-inner p-0">
                            <div class="table-responsive">
                                <table class="table table-sm table-hover" id="tblSdt">
                                    <thead class="table-light">
                                        <tr>
                                            <th style="width:50px">#</th>
                                            <th>SĐT gốc</th>
                                            <th>SĐT chuẩn hóa (84...)</th>
                                            <th>Trạng thái</th>
                                            <th>Số lần gửi</th>
                                            <th>Ngày thêm</th>
                                            <th style="width:80px">Thao tác</th>
                                        </tr>
                                    </thead>
                                    <tbody id="sdtBody">
                                        <tr><td colspan="7" class="text-center">Đang tải...</td></tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <!-- Paging -->
                    <div class="d-flex justify-content-between align-items-center mt-2" id="pagingArea" style="display:none!important;">
                        <div class="text-muted small">Hiển thị <span id="spnFrom">0</span>-<span id="spnTo">0</span> / <span id="spnTotal2">0</span></div>
                        <ul class="pagination pagination-sm" id="paging"></ul>
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>

<!-- Modal thêm 1 SĐT -->
<div class="modal fade" tabindex="-1" id="modalSingle">
    <div class="modal-dialog modal-sm" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Thêm số điện thoại</h5>
                <a href="#" class="close" data-dismiss="modal"><em class="icon ni ni-cross"></em></a>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label class="form-label">Số điện thoại <span class="text-danger">*</span></label>
                    <input type="text" id="txtPhone" class="form-control" placeholder="Vd: 0901234567" />
                    <div class="form-note text-warning" id="phoneNote" style="display:none;"></div>
                    <div class="form-note text-soft mt-1">Hỗ trợ: 09x, 03x, 07x, 08x, +849x, 849x...</div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <button class="btn btn-primary" onclick="saveSinglePhone()">Thêm</button>
                <a href="javascript:void(0);" class="btn btn-secondary" data-dismiss="modal">Hủy</a>
            </div>
        </div>
    </div>
</div>

<!-- Modal thêm nhiều SĐT -->
<div class="modal fade" tabindex="-1" id="modalBulk">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Thêm nhiều số điện thoại</h5>
                <a href="#" class="close" data-dismiss="modal"><em class="icon ni ni-cross"></em></a>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label class="form-label">Danh sách SĐT <span class="text-danger">*</span></label>
                    <textarea id="txtBulkPhones" class="form-control" rows="10"
                        placeholder="Nhập mỗi SĐT trên 1 dòng, hoặc cách nhau bằng dấu phẩy.&#10;Ví dụ:&#10;0901234567&#10;0912345678&#10;+84987654321"></textarea>
                    <div class="form-note text-soft mt-1">
                        Hệ thống sẽ tự động:
                        <ul class="mt-1 mb-0" style="padding-left:16px;">
                            <li>Chuẩn hóa định dạng: <b>09x → 849x</b>, <b>03x → 843x</b>, <b>07x → 847x</b>, <b>08x → 848x</b></li>
                            <li>Bỏ qua số trùng và số không hợp lệ</li>
                        </ul>
                    </div>
                </div>
                <div id="bulkPreview" style="display:none;">
                    <hr />
                    <div class="row g-2">
                        <div class="col-4">
                            <div class="info-card info-card-bordered">
                                <div class="info-card-inner">
                                    <div class="info-card-name text-success">Hợp lệ</div>
                                    <div class="info-card-count" id="preValidCount">0</div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="info-card info-card-bordered">
                                <div class="info-card-inner">
                                    <div class="info-card-name text-danger">Không hợp lệ</div>
                                    <div class="info-card-count" id="preInvalidCount">0</div>
                                </div>
                            </div>
                        </div>
                        <div class="col-4">
                            <div class="info-card info-card-bordered">
                                <div class="info-card-inner">
                                    <div class="info-card-name text-warning">Trùng trong batch</div>
                                    <div class="info-card-count" id="preDupCount">0</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div id="preInvalidList" class="mt-2" style="display:none;">
                        <p class="text-danger small mb-1">Số không hợp lệ:</p>
                        <ul id="ulInvalid" class="text-danger small" style="max-height:100px;overflow-y:auto;"></ul>
                    </div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <button class="btn btn-outline-info" onclick="previewBulk()"><em class="icon ni ni-eye"></em> Kiểm tra trước</button>
                <button class="btn btn-primary" onclick="saveBulkPhones()"><em class="icon ni ni-upload"></em> Thêm vào danh sách</button>
                <a href="javascript:void(0);" class="btn btn-secondary" data-dismiss="modal">Hủy</a>
            </div>
        </div>
    </div>
</div>

<script type="text/javascript">
    var moduleId = <%= ModuleId %>;
    var sf = $.ServicesFramework(moduleId);
    var serviceRoot = "/DesktopModules/NVCMS/API/ZaloCampaign/";
    var campaignId = parseInt("<%=CampaignId%>") || 0;
    var currentPage = 0;
    var pageSize = 50;
    var totalRecordsGlobal = 0;

    $(document).ready(function () {
        if (campaignId > 0) {
            loadCampaignInfo();
            loadSdtList(0);
        } else {
            $("#sdtBody").html('<tr><td colspan="7" class="text-center text-danger">Không tìm thấy chiến dịch!</td></tr>');
        }

        $("#txtSearch").keypress(function (e) {
            if (e.which === 13) loadSdtList(0);
        });
    });

    function loadCampaignInfo() {
        $.ajax({
            type: "GET", url: serviceRoot + "GetByID",
            data: { id: campaignId },
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res.Success && res.Data) {
                    $("#spnCampaignTitle").text(res.Data.Title);
                }
            }
        });
    }

    function loadSdtList(page) {
        currentPage = page;
        $.ajax({
            type: "GET", url: serviceRoot + "GetSdtList",
            data: {
                campaignId: campaignId,
                keySearch: $("#txtSearch").val(),
                status: $("#selStatusFilter").val(),
                pageIndex: page,
                pageSize: pageSize
            },
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res.Success) {
                    renderSdtTable(res.Data, page);
                    totalRecordsGlobal = res.TotalRecords;
                    $("#spnTotal").text(res.TotalRecords);
                    renderPaging(res.TotalRecords, page);
                } else {
                    NioApp.Toast(res.Message, 'danger', { position: 'top-right' });
                }
            }
        });
    }

    function renderSdtTable(data, page) {
        if (!data || data.length === 0) {
            $("#sdtBody").html('<tr><td colspan="7" class="text-center text-muted">Chưa có số điện thoại nào.</td></tr>');
            return;
        }
        var html = '';
        $.each(data, function (i, r) {
            var statusBadge = '<span class="badge ' + getSdtStatusBadge(r.Status) + '">' + getSdtStatusLabel(r.Status) + '</span>';
            var dt = r.CreatedDate ? new Date(parseInt(r.CreatedDate.replace(/\/Date\((\d+)\)\//, '$1'))).toLocaleDateString('vi-VN') : '';
            html += '<tr>';
            html += '<td>' + (page * pageSize + i + 1) + '</td>';
            html += '<td>' + escapeHtml(r.PhoneRaw || r.Phone) + '</td>';
            html += '<td><b>' + escapeHtml(r.Phone) + '</b></td>';
            html += '<td>' + statusBadge + '</td>';
            html += '<td>' + (r.SendCount || 0) + '</td>';
            html += '<td>' + dt + '</td>';
            html += '<td><a href="javascript:void(0);" onclick="deleteSdt(' + r.Id + ')" class="btn btn-sm btn-icon btn-outline-danger" title="Xóa"><em class="icon ni ni-trash"></em></a></td>';
            html += '</tr>';
        });
        $("#sdtBody").html(html);
    }

    function renderPaging(total, current) {
        var pages = Math.ceil(total / pageSize);
        if (pages <= 1) { $("#pagingArea").hide(); return; }
        $("#pagingArea").show();
        var html = '';
        if (current > 0) html += '<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="loadSdtList(' + (current - 1) + ')">‹</a></li>';
        for (var p = 0; p < pages; p++) {
            html += '<li class="page-item' + (p === current ? ' active' : '') + '"><a class="page-link" href="javascript:void(0);" onclick="loadSdtList(' + p + ')">' + (p + 1) + '</a></li>';
        }
        if (current < pages - 1) html += '<li class="page-item"><a class="page-link" href="javascript:void(0);" onclick="loadSdtList(' + (current + 1) + ')">›</a></li>';
        $("#paging").html(html);
        var from = current * pageSize + 1;
        var to = Math.min((current + 1) * pageSize, total);
        $("#spnFrom").text(from); $("#spnTo").text(to); $("#spnTotal2").text(total);
    }

    // ===== Single phone =====
    function openAddSingle() {
        $("#txtPhone").val(''); $("#phoneNote").hide();
        $("#modalSingle").modal('show');
        setTimeout(function () { $("#txtPhone").focus(); }, 400);
    }

    function saveSinglePhone() {
        var phone = $.trim($("#txtPhone").val());
        if (!phone) { alert('Nhập SĐT!'); return; }
        $.ajax({
            type: "POST", url: serviceRoot + "AddPhone",
            contentType: "application/json",
            data: JSON.stringify({ CampaignId: campaignId, PhoneRaw: phone }),
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res.Success) {
                    $("#modalSingle").modal('hide');
                    loadSdtList(0);
                    var msg = 'Đã thêm: ' + res.Data.Phone;
                    NioApp.Toast(msg, 'success', { position: 'top-right' });
                } else {
                    $("#phoneNote").text(res.Message).show();
                }
            }
        });
    }

    // ===== Bulk phone =====
    function openAddBulk() {
        $("#txtBulkPhones").val(''); $("#bulkPreview").hide();
        $("#modalBulk").modal('show');
    }

    function previewBulk() {
        var raw = $("#txtBulkPhones").val();
        $.ajax({
            type: "POST", url: serviceRoot + "ValidatePhoneList",
            contentType: "application/json",
            data: JSON.stringify({ PhoneList: raw }),
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res.Success) {
                    $("#preValidCount").text(res.Data.ValidCount);
                    $("#preInvalidCount").text(res.Data.InvalidCount);
                    $("#preDupCount").text(res.Data.DupCount);
                    if (res.Data.InvalidList && res.Data.InvalidList.length > 0) {
                        var ul = '';
                        $.each(res.Data.InvalidList, function (i, v) { ul += '<li>' + escapeHtml(v) + '</li>'; });
                        $("#ulInvalid").html(ul); $("#preInvalidList").show();
                    } else { $("#preInvalidList").hide(); }
                    $("#bulkPreview").show();
                }
            }
        });
    }

    function saveBulkPhones() {
        var raw = $("#txtBulkPhones").val();
        if (!raw.trim()) { alert('Nhập danh sách SĐT!'); return; }
        $.ajax({
            type: "POST", url: serviceRoot + "AddPhoneBulk",
            contentType: "application/json",
            data: JSON.stringify({ CampaignId: campaignId, PhoneList: raw }),
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res.Success) {
                    $("#modalBulk").modal('hide');
                    loadSdtList(0);
                    $("#spnInserted").text(res.Data.InsertCount);
                    $("#spnDup").text(res.Data.DupCount);
                    NioApp.Toast('Đã thêm ' + res.Data.InsertCount + ' SĐT. Trùng bỏ qua: ' + res.Data.DupCount, 'success', { position: 'top-right' });
                } else {
                    NioApp.Toast(res.Message, 'danger', { position: 'top-right' });
                }
            }
        });
    }

    function deleteSdt(id) {
        if (!confirm('Xóa số điện thoại này?')) return;
        $.ajax({
            type: "POST", url: serviceRoot + "DeletePhone",
            contentType: "application/json",
            data: JSON.stringify({ Id: id }),
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                loadSdtList(currentPage);
                NioApp.Toast('Đã xóa!', 'success', { position: 'top-right' });
            }
        });
    }

    function confirmDeleteAll() {
        if (!confirm('Xóa TẤT CẢ số điện thoại trong chiến dịch này?')) return;
        $.ajax({
            type: "POST", url: serviceRoot + "DeleteAllPhone",
            contentType: "application/json",
            data: JSON.stringify({ CampaignId: campaignId }),
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                loadSdtList(0);
                NioApp.Toast('Đã xóa hết!', 'success', { position: 'top-right' });
            }
        });
    }

    function getSdtStatusLabel(s) {
        switch (parseInt(s)) { case 0: return 'Chờ gửi'; case 1: return 'Đã gửi'; case 2: return 'Lỗi'; default: return 'N/A'; }
    }
    function getSdtStatusBadge(s) {
        switch (parseInt(s)) { case 0: return 'badge-warning'; case 1: return 'badge-success'; case 2: return 'badge-danger'; default: return 'badge-light'; }
    }
    function escapeHtml(s) { return String(s).replace(/&/g,'&amp;').replace(/</g,'&lt;').replace(/>/g,'&gt;'); }
</script>
