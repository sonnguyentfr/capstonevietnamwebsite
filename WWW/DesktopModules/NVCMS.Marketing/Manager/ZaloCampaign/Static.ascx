<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Static.ascx.vb" Inherits="NVCMS.Modules.Marketing.ZaloCampaignStatic" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />

<style>
    /* ============================================================
       Data-viz tokens. Trang nay nam trong admin skin sang mau,
       nen chi dinh nghia bang mau light - khong co dark mode.
       ============================================================ */
    .zns-viz {
        --surface-1: #ffffff;
        --text-primary: #0b0b0b;
        --text-secondary: #52514e;
        --text-muted: #898781;
        --gridline: #e1e0d9;
        --baseline: #c3c2b7;
        /* Status palette (co dinh - luon di kem nhan chu + icon,
           mau khong bao gio la kenh duy nhat mang y nghia) */
        --st-good: #0ca30c; /* Sent       */
        --st-critical: #d03b3b; /* Failed     */
        --st-warning: #fab219; /* Retry      */
        --st-serious: #ec835a; /* Processing */
        --st-neutral: #898781; /* Queued     */
        /* Categorical slots 1-8 (thu tu co dinh, khong xoay vong) */
        --series-1: #2a78d6;
        --series-2: #eb6834;
        --series-3: #1baf7a;
        --series-4: #eda100;
        --series-5: #e87ba4;
        --series-6: #008300;
        --series-7: #4a3aa7;
        --series-8: #e34948;
    }

    .zns-chart-box {
        position: relative;
        height: 300px;
    }

        .zns-chart-box.is-tall {
            height: 380px;
        }

    .zns-table-scroll {
        max-height: 460px;
        overflow-y: auto;
    }

        .zns-table-scroll thead th {
            position: sticky;
            top: 0;
            background: #fff;
            z-index: 5;
        }

    .zns-num {
        font-variant-numeric: tabular-nums;
        text-align: right;
    }

    .zns-kpi-note {
        font-size: 12px;
        color: var(--text-muted);
    }

    .zns-legend {
        display: flex;
        flex-wrap: wrap;
        gap: 6px 16px;
        margin-top: 10px;
    }

        .zns-legend span.item {
            display: inline-flex;
            align-items: center;
            font-size: 12px;
            color: var(--text-secondary);
        }

        .zns-legend i.dot {
            width: 10px;
            height: 10px;
            border-radius: 3px;
            margin-right: 6px;
            display: inline-block;
        }

    .zns-empty {
        padding: 40px 10px;
        text-align: center;
        color: var(--text-muted);
    }
</style>

<div class="nk-body bg-lighter npc-general has-sidebar zns-viz">
    <div class="nk-app-root">
        <div class="nk-main">
            <div class="nk-content">
                <div class="container-fluid">
                    <div class="nk-content-inner">
                        <div class="nk-content-body">

                            <!-- ============ HEADER ============ -->
                            <div class="nk-block-head nk-block-head-sm">
                                <div class="nk-block-between">
                                    <div class="nk-block-head-content">
                                        <h3 class="nk-block-title page-title" id="lblCampaignTitle">Zalo ZNS Dashboard</h3>
                                        <div class="nk-block-des text-soft">
                                            <p id="lblCampaignDesc">Thống kê chất lượng gửi tin ZNS của chiến dịch Zalo</p>
                                        </div>
                                    </div>
                                    <div class="nk-block-head-content">
                                        <ul class="nk-block-tools g-3">
                                            <li>
                                                <a href="<%= BackUrl %>" class="btn btn-outline-light"><em class="icon ni ni-arrow-left"></em><span>Danh sách chiến dịch</span></a>
                                            </li>
                                            <li>
                                                <button type="button" class="btn btn-primary" id="btnExportExcel">
                                                    <em class="icon ni ni-file-xls"></em><span>Xuất Excel</span>
                                                </button>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ KPI HANG 1 ============ -->
                            <div class="nk-block">
                                <div class="row g-gs">
                                    <div class="col-xxl-2 col-md-4 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title">Tổng lượt gửi</h6>
                                                <div class="amount h3 mb-0 zns-num" id="kpiTotalSend">0</div>
                                                <div class="zns-kpi-note" id="kpiSendNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xxl-2 col-md-4 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title">Số ĐT tiếp cận</h6>
                                                <div class="amount h3 mb-0 zns-num" id="kpiPhone">0</div>
                                                <div class="zns-kpi-note" id="kpiPhoneNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xxl-2 col-md-4 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title"><em class="icon ni ni-check-circle"></em> Gửi thành công</h6>
                                                <div class="amount h3 mb-0 zns-num" id="kpiSent">0</div>
                                                <div class="zns-kpi-note" id="kpiSentNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xxl-2 col-md-4 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title"><em class="icon ni ni-cross-circle"></em> Thất bại</h6>
                                                <div class="amount h3 mb-0 zns-num" id="kpiFailed">0</div>
                                                <div class="zns-kpi-note" id="kpiFailedNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xxl-2 col-md-4 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title"><em class="icon ni ni-send"></em> Zalo đã nhận</h6>
                                                <div class="amount h3 mb-0 zns-num" id="kpiDelivered">0</div>
                                                <div class="zns-kpi-note" id="kpiDeliveredNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-xxl-2 col-md-4 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title">Chi phí ước tính</h6>
                                                <div class="amount h3 mb-0 zns-num" id="kpiCost">0</div>
                                                <div class="zns-kpi-note" id="kpiCostNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ KPI HANG 2 ============ -->
                            <div class="nk-block">
                                <div class="row g-gs">
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner py-3">
                                                <h6 class="title">Đang chờ / Gửi lại</h6>
                                                <div class="amount h5 mb-0 zns-num" id="kpiPending">0</div>
                                                <div class="zns-kpi-note" id="kpiPendingNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner py-3">
                                                <h6 class="title">Tổng số lần retry</h6>
                                                <div class="amount h5 mb-0 zns-num" id="kpiRetry">0</div>
                                                <div class="zns-kpi-note">Số lần hệ thống thử gửi lại</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner py-3">
                                                <h6 class="title">Quota Zalo OA còn lại</h6>
                                                <div class="amount h5 mb-0 zns-num" id="kpiQuota">-</div>
                                                <div class="zns-kpi-note" id="kpiQuotaNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner py-3">
                                                <h6 class="title">Thời gian xử lý TB</h6>
                                                <div class="amount h5 mb-0 zns-num" id="kpiSpeed">0s</div>
                                                <div class="zns-kpi-note" id="kpiSpeedNote">&nbsp;</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ CHART: TRANG THAI + DIEN BIEN ============ -->
                            <div class="nk-block">
                                <div class="row g-gs">
                                    <div class="col-lg-4">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title">Phân bố trạng thái gửi</h6>
                                                <p class="zns-kpi-note mb-2">Nguồn: ZNS_Send_Queue</p>
                                                <div class="zns-chart-box">
                                                    <canvas id="chartStatus"></canvas>
                                                </div>
                                                <div class="zns-legend" id="legendStatus"></div>
                                                <div class="table-responsive mt-3">
                                                    <table class="table table-sm is-compact mb-0">
                                                        <thead>
                                                            <tr>
                                                                <th><span class="sub-text">Trạng thái</span></th>
                                                                <th class="zns-num"><span class="sub-text">Số lượt</span></th>
                                                                <th class="zns-num"><span class="sub-text">Tỷ lệ</span></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tblStatus"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-8">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title">Diễn biến gửi theo ngày</h6>
                                                <p class="zns-kpi-note mb-2">Số lượt gửi mỗi ngày, tách theo kết quả</p>
                                                <div class="zns-chart-box is-tall">
                                                    <canvas id="chartTimeline"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ CHART: TEMPLATE ============ -->
                            <div class="nk-block">
                                <div class="card card-bordered">
                                    <div class="card-inner">
                                        <h6 class="title">Hiệu quả theo Template ZNS</h6>
                                        <p class="zns-kpi-note mb-2">Mỗi template là một mẫu tin Zalo đã duyệt - dùng để đánh giá mẫu nào được Zalo chấp nhận tốt nhất</p>
                                        <div class="row g-gs">
                                            <div class="col-lg-6">
                                                <div class="zns-chart-box is-tall">
                                                    <canvas id="chartTemplate"></canvas>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="table-responsive zns-table-scroll">
                                                    <table class="table table-sm is-compact mb-0">
                                                        <thead>
                                                            <tr>
                                                                <th><span class="sub-text">Template</span></th>
                                                                <th class="zns-num"><span class="sub-text">Lượt gửi</span></th>
                                                                <th class="zns-num"><span class="sub-text">SĐT</span></th>
                                                                <th class="zns-num"><span class="sub-text">Thành công</span></th>
                                                                <th class="zns-num"><span class="sub-text">Tỷ lệ</span></th>
                                                                <th class="zns-num"><span class="sub-text">Chi phí (đ)</span></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tblTemplate"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ CHART: SU KIEN + KHUNG GIO ============ -->
                            <div class="nk-block">
                                <div class="row g-gs">
                                    <div class="col-lg-7">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title">Hiệu quả theo Sự kiện / Địa điểm</h6>
                                                <p class="zns-kpi-note mb-2">Nhóm sự kiện: <span id="lblEventCat" class="text-dark">-</span></p>
                                                <div class="zns-chart-box is-tall">
                                                    <canvas id="chartEvent"></canvas>
                                                </div>
                                                <div class="table-responsive mt-3">
                                                    <table class="table table-sm is-compact mb-0">
                                                        <thead>
                                                            <tr>
                                                                <th><span class="sub-text">Sự kiện</span></th>
                                                                <th><span class="sub-text">Địa điểm</span></th>
                                                                <th class="zns-num"><span class="sub-text">Lượt gửi</span></th>
                                                                <th class="zns-num"><span class="sub-text">Thành công</span></th>
                                                                <th class="zns-num"><span class="sub-text">Tỷ lệ</span></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tblEvent"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-5">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <h6 class="title">Phân bố theo khung giờ gửi</h6>
                                                <p class="zns-kpi-note mb-2">Dùng để chọn khung giờ gửi có tỷ lệ Zalo chấp nhận cao</p>
                                                <div class="zns-chart-box is-tall">
                                                    <canvas id="chartHour"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ CHART: NGUYEN NHAN LOI ============ -->
                            <div class="nk-block">
                                <div class="card card-bordered">
                                    <div class="card-inner">
                                        <h6 class="title text-danger"><em class="icon ni ni-alert-circle"></em> Nguyên nhân gửi không thành công</h6>
                                        <p class="zns-kpi-note mb-2">Mã lỗi trả về từ Zalo ZNS API - đây là thông tin cần xử lý để nâng tỷ lệ gửi thành công</p>
                                        <div class="row g-gs">
                                            <div class="col-lg-6">
                                                <div class="zns-chart-box is-tall">
                                                    <canvas id="chartError"></canvas>
                                                </div>
                                            </div>
                                            <div class="col-lg-6">
                                                <div class="table-responsive zns-table-scroll">
                                                    <table class="table table-sm is-compact mb-0">
                                                        <thead>
                                                            <tr>
                                                                <th class="zns-num"><span class="sub-text">Mã lỗi</span></th>
                                                                <th><span class="sub-text">Mô tả</span></th>
                                                                <th class="zns-num"><span class="sub-text">Số lượt</span></th>
                                                                <th class="zns-num"><span class="sub-text">Số ĐT</span></th>
                                                                <th class="zns-num"><span class="sub-text">Tỷ lệ</span></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody id="tblError"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ BANG: SO DIEN THOAI ============ -->
                            <div class="nk-block">
                                <div class="card card-bordered">
                                    <div class="card-inner">
                                        <div class="card-title-group mb-2">
                                            <div class="card-title">
                                                <h6 class="title">Số lượt gửi theo số điện thoại (<span id="phoneCount">0</span> SĐT)</h6>
                                                <p class="zns-kpi-note mb-0">Sắp xếp theo số lượt gửi giảm dần - phát hiện SĐT bị gửi trùng lặp hoặc luôn thất bại</p>
                                            </div>
                                            <div class="card-tools">
                                                <input type="text" class="form-control form-control-sm" id="txtSearchPhone" placeholder="Tìm SĐT / tên..." style="width: 220px;" />
                                            </div>
                                        </div>
                                        <div class="table-responsive zns-table-scroll">
                                            <table class="table table-sm is-compact mb-0">
                                                <thead>
                                                    <tr>
                                                        <th><span class="sub-text">Số điện thoại</span></th>
                                                        <th><span class="sub-text">Họ tên</span></th>
                                                        <th class="zns-num"><span class="sub-text">Lượt gửi</span></th>
                                                        <th class="zns-num"><span class="sub-text">Template</span></th>
                                                        <th class="zns-num"><span class="sub-text">Thành công</span></th>
                                                        <th class="zns-num"><span class="sub-text">Thất bại</span></th>
                                                        <th class="zns-num"><span class="sub-text">Tỷ lệ</span></th>
                                                        <th><span class="sub-text">Trạng thái cuối</span></th>
                                                        <th><span class="sub-text">Lỗi gần nhất</span></th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tblPhone"></tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- ============ BANG: CHI TIET ============ -->
                            <div class="nk-block">
                                <div class="card card-bordered">
                                    <div class="card-inner">
                                        <div class="card-title-group mb-2">
                                            <div class="card-title">
                                                <h6 class="title">Chi tiết từng lượt gửi (<span id="detailCount">0</span>)</h6>
                                            </div>
                                            <div class="card-tools">
                                                <div class="form-inline">
                                                    <select class="form-control form-control-sm mr-2" id="ddlDetailStatus">
                                                        <option value="">-- Tất cả trạng thái --</option>
                                                        <option value="Sent">Gửi thành công</option>
                                                        <option value="Failed">Thất bại</option>
                                                        <option value="Retry">Gửi lại</option>
                                                        <option value="Queued">Chờ gửi</option>
                                                        <option value="Processing">Đang xử lý</option>
                                                    </select>
                                                    <input type="text" class="form-control form-control-sm" id="txtSearchDetail" placeholder="Tìm SĐT / tên / lỗi..." style="width: 220px;" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive zns-table-scroll">
                                            <table class="table table-sm is-compact mb-0">
                                                <thead>
                                                    <tr>
                                                        <th class="zns-num"><span class="sub-text">#</span></th>
                                                        <th><span class="sub-text">Số điện thoại</span></th>
                                                        <th><span class="sub-text">Họ tên</span></th>
                                                        <th><span class="sub-text">Template</span></th>
                                                        <th><span class="sub-text">Trạng thái</span></th>
                                                        <th><span class="sub-text">Zalo nhận</span></th>
                                                        <th><span class="sub-text">Sự kiện</span></th>
                                                        <th><span class="sub-text">Thời điểm gửi</span></th>
                                                        <th class="zns-num"><span class="sub-text">Xử lý (s)</span></th>
                                                        <th><span class="sub-text">Lỗi</span></th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tblDetail"></tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div id="loading" style="display: none;">
    <div class="loading-spinner">
        <div class="spinner-border text-primary" role="status">
            <span class="sr-only">Loading...</span>
        </div>
        <p class="mt-2 loadingtext">Đang tải dữ liệu. Vui lòng đợi trong giây lát...</p>
    </div>
</div>

<script src="https://cdn.jsdelivr.net/npm/xlsx@0.18.5/dist/xlsx.full.min.js"></script>
<script type="text/javascript">
    // ================================================================
    // 1. THAM SO & CAU HINH
    // ================================================================
    var moduleId = <%= ModuleId %>;
    var campaignId = <%= CampaignId %>;
    var sf = (typeof $.ServicesFramework === 'function') ? $.ServicesFramework(moduleId) : null;
    var serviceRoot = "/DesktopModules/NVCMS/API/ZaloReport/";

    // Chart.js v2 va v3+ khac nhau ve cau truc options -> detect runtime
    var CHART_V3 = (typeof Chart !== 'undefined' && Chart.version && parseInt(Chart.version, 10) >= 3);

    // Bang mau: doc tu CSS custom properties (mot noi duy nhat)
    var CSSV = getComputedStyle(document.querySelector('.zns-viz'));
    function token(name) { return (CSSV.getPropertyValue(name) || '').trim(); }

    var C = {
        surface: token('--surface-1') || '#ffffff',
        ink: token('--text-primary') || '#0b0b0b',
        ink2: token('--text-secondary') || '#52514e',
        muted: token('--text-muted') || '#898781',
        grid: token('--gridline') || '#e1e0d9',
        base: token('--baseline') || '#c3c2b7',
        good: token('--st-good') || '#0ca30c',
        critical: token('--st-critical') || '#d03b3b',
        warning: token('--st-warning') || '#fab219',
        serious: token('--st-serious') || '#ec835a',
        neutral: token('--st-neutral') || '#898781'
    };
    // Categorical slots - gan theo THU TU CO DINH, khong bao gio xoay vong
    var SERIES = ['--series-1', '--series-2', '--series-3', '--series-4',
                  '--series-5', '--series-6', '--series-7', '--series-8']
                 .map(function (n) { return token(n); });

    // Mau theo trang thai hang doi (status encoding - luon kem nhan chu)
    var STATUS_COLOR = {
        'Sent': C.good,
        'Failed': C.critical,
        'Retry': C.warning,
        'Processing': C.serious,
        'Queued': C.neutral
    };
    var STATUS_LABEL = {
        'Sent': 'Gửi thành công',
        'Failed': 'Thất bại',
        'Retry': 'Gửi lại',
        'Processing': 'Đang xử lý',
        'Queued': 'Chờ gửi'
    };
    var STATUS_BADGE = {
        'Sent': 'badge-success',
        'Failed': 'badge-danger',
        'Retry': 'badge-warning',
        'Processing': 'badge-warning',
        'Queued': 'badge-light'
    };

    // ================================================================
    // 2. TIEN ICH
    // ================================================================
    function num(v) { return (Number(v) || 0).toLocaleString('vi-VN'); }
    function pct(v) { return (Number(v) || 0).toFixed(2) + '%'; }
    function money(v) { return (Number(v) || 0).toLocaleString('vi-VN') + ' đ'; }
    function esc(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;')
            .replace(/>/g, '&gt;').replace(/"/g, '&quot;');
    }
    function dt(v) { return v ? new Date(v).toLocaleString('vi-VN') : '-'; }
    function d(v) { return v ? new Date(v).toLocaleDateString('vi-VN') : '-'; }
    function cut(s, n) {
        s = String(s == null ? '' : s);
        return s.length > n ? s.substring(0, n) + '…' : s;
    }
    function secs(v) {
        var n = Number(v) || 0;
        return n >= 60 ? (n / 60).toFixed(1) + ' phút' : n.toFixed(1) + ' giây';
    }
    function toast(msg, type) {
        if (typeof NioApp !== 'undefined' && NioApp.Toast) {
            NioApp.Toast(msg, type || 'info', { position: 'top-right' });
        } else { alert(msg); }
    }

    // ================================================================
    // 3. CHART HELPERS (tuong thich Chart.js v2 & v3+)
    // ================================================================

    // Plugin ve nhan gia tri truc tiep tren mark (direct label).
    // Bat buoc vi mot so mau trong bang co do tuong phan < 3:1 so voi nen.
    var directLabel = {
        id: 'znsDirectLabel',
        afterDatasetsDraw: function (chart) {
            var ctx = chart.ctx;
            var opt = (chart.options.plugins && chart.options.plugins.znsDirectLabel)
                   || chart.options.znsDirectLabel;
            if (!opt || !opt.enabled) return;

            ctx.save();
            ctx.font = '600 11px system-ui, -apple-system, "Segoe UI", sans-serif';
            ctx.fillStyle = C.ink2;
            ctx.textAlign = opt.horizontal ? 'left' : 'center';
            ctx.textBaseline = 'middle';

            var metaList = CHART_V3
                ? chart.data.datasets.map(function (_, i) { return chart.getDatasetMeta(i); })
                : chart.data.datasets.map(function (_, i) { return chart.getDatasetMeta(i); });

            metaList.forEach(function (meta, di) {
                if (meta.hidden) return;
                var ds = chart.data.datasets[di];
                meta.data.forEach(function (el, i) {
                    var v = ds.data[i];
                    if (!v) return;                       // bo qua gia tri 0
                    var p = CHART_V3 ? el : el._model;
                    var x = opt.horizontal ? p.x + 6 : p.x;
                    var y = opt.horizontal ? p.y : p.y - 8;
                    ctx.fillText(num(v), x, y);
                });
            });
            ctx.restore();
        }
    };
    if (typeof Chart !== 'undefined') {
        if (Chart.register) { Chart.register(directLabel); }                 // v3+
        else if (Chart.plugins && Chart.plugins.register) { Chart.plugins.register(directLabel); } // v2
    }

    // Sinh options dung cho ca 2 doi Chart.js
    function buildOptions(cfg) {
        cfg = cfg || {};
        var o = { responsive: true, maintainAspectRatio: false };

        var tickFont = { color: C.muted, fontColor: C.muted, fontSize: 11 };
        var gridCfg = { color: C.grid, zeroLineColor: C.base, drawBorder: false };

        if (CHART_V3) {
            o.plugins = {
                legend: cfg.legend === false ? { display: false } : {
                    display: true, position: cfg.legendPosition || 'bottom',
                    labels: { color: C.ink2, boxWidth: 12, usePointStyle: true }
                },
                tooltip: { enabled: true, callbacks: cfg.tooltipCallbacks },
                znsDirectLabel: cfg.directLabel || { enabled: false }
            };
            if (cfg.scales !== false) {
                o.scales = {
                    x: {
                        stacked: !!cfg.stacked, beginAtZero: true,
                        grid: { display: !!cfg.horizontal, color: C.grid, drawBorder: false },
                        ticks: { color: C.muted, font: { size: 11 }, precision: 0 }
                    },
                    y: {
                        stacked: !!cfg.stacked, beginAtZero: true,
                        grid: { display: !cfg.horizontal, color: C.grid, drawBorder: false },
                        ticks: { color: C.muted, font: { size: 11 }, precision: 0 }
                    }
                };
            }
        } else {
            o.legend = cfg.legend === false ? { display: false } : {
                display: true, position: cfg.legendPosition || 'bottom',
                labels: { fontColor: C.ink2, boxWidth: 12, usePointStyle: true }
            };
            o.tooltips = { enabled: true, callbacks: cfg.tooltipCallbacks };
            o.znsDirectLabel = cfg.directLabel || { enabled: false };
            if (cfg.scales !== false) {
                o.scales = {
                    xAxes: [{
                        stacked: !!cfg.stacked,
                        gridLines: $.extend({}, gridCfg, { display: !!cfg.horizontal }),
                        ticks: $.extend({}, tickFont, { beginAtZero: true, precision: 0 })
                    }],
                    yAxes: [{
                        stacked: !!cfg.stacked,
                        gridLines: $.extend({}, gridCfg, { display: !cfg.horizontal }),
                        ticks: $.extend({}, tickFont, { beginAtZero: true, precision: 0 })
                    }]
                };
            }
        }
        return o;
    }

    // Bo goc 4px o dau cot (Chart.js v3 ho tro borderRadius)
    function barStyle(extra) {
        return $.extend({
            borderRadius: 4,
            borderSkipped: false,
            maxBarThickness: 28,
            borderWidth: 2,
            borderColor: C.surface   // khe ho 2px giua cac mang mau
        }, extra || {});
    }

    var charts = {};
    function paint(id, config) {
        var el = document.getElementById(id);
        if (!el) return;
        if (charts[id]) { charts[id].destroy(); }
        charts[id] = new Chart(el.getContext('2d'), config);
    }
    function emptyBox(id, msg) {
        var el = document.getElementById(id);
        if (el && el.parentNode) {
            el.parentNode.innerHTML = '<div class="zns-empty">' + esc(msg) + '</div>';
        }
    }

    // ================================================================
    // 4. TAI DU LIEU
    // ================================================================
    var DATA = null;

    $(document).ready(function () {
        if (!(campaignId > 0)) {
            toast('Thiếu campaignId trên URL.', 'warning');
            return;
        }
        loadDashboard();

        $('#btnExportExcel').on('click', exportExcel);
        $('#txtSearchPhone').on('keyup', function () { renderPhones(); });
        $('#txtSearchDetail, #ddlDetailStatus').on('keyup change', function () { renderDetails(); });
    });

    function loadDashboard() {
        $('#loading').show();

        var opt = {
            url: serviceRoot + 'GetDashboard?campaignId=' + encodeURIComponent(campaignId),
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                $('#loading').hide();
                if (res && res.Success && res.Data) {
                    DATA = res.Data;
                    renderAll();
                } else {
                    toast((res && res.Message) || 'Không tải được dữ liệu dashboard.', 'danger');
                }
            },
            error: function (xhr, status, err) {
                $('#loading').hide();
                console.error('ZaloReport API error:', xhr.responseText, status, err);
                toast('Lỗi kết nối API: ' + err, 'danger');
            }
        };
        if (sf) { opt.beforeSend = function (xhr) { sf.setModuleHeaders(xhr); }; }
        $.ajax(opt);
    }

    function renderAll() {
        renderSummary();
        renderStatus();
        renderTimeline();
        renderTemplates();
        renderEvents();
        renderHours();
        renderErrors();
        renderPhones();
        renderDetails();
    }

    // ================================================================
    // 5. KPI
    // ================================================================
    function renderSummary() {
        var s = DATA.Summary || {};

        $('#lblCampaignTitle').text(s.Title || 'Zalo ZNS Dashboard');
        $('#lblCampaignDesc').text(
            (s.Description || 'Thống kê chất lượng gửi tin ZNS') +
            ' · Tạo ngày ' + d(s.CampaignCreatedDate));

        $('#kpiTotalSend').text(num(s.TotalSend));
        $('#kpiSendNote').text('Từ ' + d(s.FirstQueuedTime) + ' đến ' + d(s.LastCompletedTime));

        $('#kpiPhone').text(num(s.TotalPhoneTargeted));
        $('#kpiPhoneNote').text('TB ' + (Number(s.AvgSendPerPhone) || 0).toFixed(1) +
            ' lượt/SĐT · Danh sách: ' + num(s.TotalPhoneInList));

        $('#kpiSent').text(num(s.TotalSent));
        $('#kpiSentNote').text('Tỷ lệ thành công ' + pct(s.SuccessRate));

        $('#kpiFailed').text(num(s.TotalFailed));
        $('#kpiFailedNote').text('Tỷ lệ thất bại ' + pct(s.FailRate));

        $('#kpiDelivered').text(num(s.TotalDelivered));
        $('#kpiDeliveredNote').text('Zalo từ chối: ' + num(s.TotalRejected) +
            ' · Tỷ lệ nhận ' + pct(s.DeliveryRate));

        $('#kpiCost').text(money(s.EstimatedCostSent));
        $('#kpiCostNote').text('TB ' + money(s.CostPerDelivered) + '/tin nhận được');

        var pending = (Number(s.TotalQueued) || 0) + (Number(s.TotalProcessing) || 0) + (Number(s.TotalRetry) || 0);
        $('#kpiPending').text(num(pending));
        $('#kpiPendingNote').text('Chờ ' + num(s.TotalQueued) + ' · Đang xử lý ' +
            num(s.TotalProcessing) + ' · Retry ' + num(s.TotalRetry));

        $('#kpiRetry').text(num(s.TotalRetryCount));

        if (s.RemainingQuota != null) {
            $('#kpiQuota').text(num(s.RemainingQuota));
            $('#kpiQuotaNote').text('Hạn mức ngày: ' + num(s.DailyQuota));
        } else {
            $('#kpiQuota').text('-');
            $('#kpiQuotaNote').text('Chưa ghi nhận');
        }

        $('#kpiSpeed').text(secs(s.AvgProcessSeconds));
        $('#kpiSpeedNote').text('Chờ hàng đợi TB ' + secs(s.AvgWaitSeconds));
    }

    // ================================================================
    // 6. PHAN BO TRANG THAI (doughnut + legend + bang)
    // ================================================================
    function renderStatus() {
        var rows = DATA.StatusDistribution || [];
        if (!rows.length) { emptyBox('chartStatus', 'Chưa có lượt gửi nào.'); return; }

        var labels = rows.map(function (r) { return r.StatusLabel; });
        var values = rows.map(function (r) { return r.Quantity; });
        var colors = rows.map(function (r) { return STATUS_COLOR[r.Status] || C.neutral; });

        paint('chartStatus', {
            type: 'doughnut',
            data: {
                labels: labels,
                datasets: [{
                    data: values,
                    backgroundColor: colors,
                    borderColor: C.surface,
                    borderWidth: 2          // khe ho 2px giua cac lat cat
                }]
            },
            options: buildOptions({
                legend: false,              // dung legend HTML ben duoi de kem so lieu
                scales: false,
                tooltipCallbacks: {
                    label: function (a, b) {
                        var i = CHART_V3 ? a.dataIndex : a.index;
                        var r = rows[i];
                        return r.StatusLabel + ': ' + num(r.Quantity) + ' (' + pct(r.Percentage) + ')';
                    }
                }
            })
        });

        // Legend + gia tri (mau khong bao gio la kenh duy nhat)
        $('#legendStatus').html(rows.map(function (r) {
            return '<span class="item"><i class="dot" style="background:' +
                (STATUS_COLOR[r.Status] || C.neutral) + '"></i>' +
                esc(r.StatusLabel) + ' · <b>' + num(r.Quantity) + '</b></span>';
        }).join(''));

        $('#tblStatus').html(rows.map(function (r) {
            return '<tr>' +
                '<td><span class="badge badge-dim ' + (STATUS_BADGE[r.Status] || 'badge-light') + '">' +
                    esc(r.StatusLabel) + '</span></td>' +
                '<td class="zns-num">' + num(r.Quantity) + '</td>' +
                '<td class="zns-num">' + pct(r.Percentage) + '</td>' +
            '</tr>';
        }).join(''));
    }

    // ================================================================
    // 7. DIEN BIEN THEO NGAY (line, mot truc duy nhat)
    // ================================================================
    function renderTimeline() {
        var rows = DATA.Timeline || [];
        if (!rows.length) { emptyBox('chartTimeline', 'Chưa có dữ liệu theo ngày.'); return; }

        var labels = rows.map(function (r) { return d(r.SendDate); });

        function line(label, key, color) {
            return {
                label: label,
                data: rows.map(function (r) { return r[key]; }),
                borderColor: color,
                backgroundColor: color,
                borderWidth: 2,
                pointRadius: 4,
                pointHoverRadius: 6,
                pointBorderColor: C.surface,
                pointBorderWidth: 2,
                fill: false,
                lineTension: 0.25,
                tension: 0.25
            };
        }

        paint('chartTimeline', {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    line('Tổng lượt gửi', 'TotalSend', SERIES[0]),
                    line('Gửi thành công', 'Sent', C.good),
                    line('Thất bại', 'Failed', C.critical)
                ]
            },
            options: buildOptions({
                tooltipCallbacks: {
                    label: function (a, b) {
                        var ds = CHART_V3 ? a.dataset.label : b.datasets[a.datasetIndex].label;
                        var v = CHART_V3 ? a.parsed.y : a.yLabel;
                        return ds + ': ' + num(v);
                    }
                }
            })
        });
    }

    // ================================================================
    // 8. THEO TEMPLATE (stacked horizontal bar + bang)
    // ================================================================
    function renderTemplates() {
        var rows = DATA.Templates || [];
        if (!rows.length) { emptyBox('chartTemplate', 'Chưa có template nào được dùng.'); return; }

        var labels = rows.map(function (r) { return cut(r.TemplateName, 34); });

        function seg(label, key, color) {
            return barStyle({
                label: label,
                data: rows.map(function (r) { return r[key]; }),
                backgroundColor: color
            });
        }

        paint('chartTemplate', {
            type: CHART_V3 ? 'bar' : 'horizontalBar',
            data: {
                labels: labels,
                datasets: [
                    seg('Gửi thành công', 'Sent', C.good),
                    seg('Thất bại', 'Failed', C.critical),
                    seg('Gửi lại', 'Retry', C.warning),
                    seg('Chờ gửi', 'Queued', C.neutral)
                ]
            },
            options: $.extend(
                buildOptions({ stacked: true, horizontal: true }),
                CHART_V3 ? { indexAxis: 'y' } : {})
        });

        $('#tblTemplate').html(rows.map(function (r) {
            return '<tr>' +
                '<td><span class="tb-lead">' + esc(cut(r.TemplateName, 40)) + '</span>' +
                    '<div class="zns-kpi-note">#' + r.TemplateId + ' · ' + esc(r.TemplateStatus) + '</div></td>' +
                '<td class="zns-num">' + num(r.TotalSend) + '</td>' +
                '<td class="zns-num">' + num(r.DistinctPhone) + '</td>' +
                '<td class="zns-num">' + num(r.Sent) + '</td>' +
                '<td class="zns-num">' + pct(r.SuccessRate) + '</td>' +
                '<td class="zns-num">' + num(r.EstimatedCost) + '</td>' +
            '</tr>';
        }).join(''));
    }

    // ================================================================
    // 9. THEO SU KIEN / DIA DIEM
    // ================================================================
    function renderEvents() {
        var cats = DATA.EventCats || [];
        $('#lblEventCat').text(cats.map(function (c) {
            return c.EventCatName + ' (' + num(c.TotalSend) + ' lượt)';
        }).join(' · ') || '-');

        var rows = DATA.Events || [];
        if (!rows.length) { emptyBox('chartEvent', 'Chưa có dữ liệu theo sự kiện.'); return; }

        var labels = rows.map(function (r) { return cut(r.EventName, 30); });

        paint('chartEvent', {
            type: 'bar',
            data: {
                labels: labels,
                datasets: [
                    barStyle({ label: 'Gửi thành công', data: rows.map(function (r) { return r.Sent; }), backgroundColor: C.good }),
                    barStyle({ label: 'Thất bại', data: rows.map(function (r) { return r.Failed; }), backgroundColor: C.critical }),
                    barStyle({ label: 'Đang chờ', data: rows.map(function (r) { return r.Pending; }), backgroundColor: C.neutral })
                ]
            },
            options: buildOptions({ stacked: true })
        });

        $('#tblEvent').html(rows.map(function (r) {
            return '<tr>' +
                '<td><span class="tb-lead">' + esc(cut(r.EventName, 38)) + '</span></td>' +
                '<td>' + esc(cut(r.Location, 28)) + '</td>' +
                '<td class="zns-num">' + num(r.TotalSend) + '</td>' +
                '<td class="zns-num">' + num(r.Sent) + '</td>' +
                '<td class="zns-num">' + pct(r.SuccessRate) + '</td>' +
            '</tr>';
        }).join(''));
    }

    // ================================================================
    // 10. PHAN BO THEO KHUNG GIO
    // ================================================================
    function renderHours() {
        var rows = DATA.Hours || [];
        if (!rows.length) { emptyBox('chartHour', 'Chưa có dữ liệu theo giờ.'); return; }

        paint('chartHour', {
            type: 'bar',
            data: {
                labels: rows.map(function (r) { return String(r.SendHour) + 'h'; }),
                datasets: [
                    barStyle({ label: 'Gửi thành công', data: rows.map(function (r) { return r.Sent; }), backgroundColor: C.good }),
                    barStyle({ label: 'Thất bại', data: rows.map(function (r) { return r.Failed; }), backgroundColor: C.critical })
                ]
            },
            options: buildOptions({ stacked: true })
        });
    }

    // ================================================================
    // 11. NGUYEN NHAN LOI
    // ================================================================
    function renderErrors() {
        var rows = (DATA.Errors || []).slice(0, 10);
        if (!rows.length) { emptyBox('chartError', 'Không ghi nhận lỗi nào. '); return; }

        paint('chartError', {
            type: CHART_V3 ? 'bar' : 'horizontalBar',
            data: {
                labels: rows.map(function (r) { return String(r.ErrorCode) + ' · ' + cut(r.ErrorMessage, 28); }),
                datasets: [barStyle({
                    label: 'Số lượt',
                    data: rows.map(function (r) { return r.Quantity; }),
                    backgroundColor: C.critical
                })]
            },
            options: $.extend(
                buildOptions({
                    horizontal: true,
                    legend: false,                        // 1 series -> tieu de da goi ten
                    directLabel: { enabled: true, horizontal: true }
                }),
                CHART_V3 ? { indexAxis: 'y' } : {})
        });

        $('#tblError').html((DATA.Errors || []).map(function (r) {
            return '<tr>' +
                '<td class="zns-num"><span class="badge badge-dim badge-danger">' + r.ErrorCode + '</span></td>' +
                '<td>' + esc(r.ErrorMessage) + '</td>' +
                '<td class="zns-num">' + num(r.Quantity) + '</td>' +
                '<td class="zns-num">' + num(r.DistinctPhone) + '</td>' +
                '<td class="zns-num">' + pct(r.Percentage) + '</td>' +
            '</tr>';
        }).join(''));
    }

    // ================================================================
    // 12. BANG SO DIEN THOAI
    // ================================================================
    function renderPhones() {
        var key = ($('#txtSearchPhone').val() || '').toLowerCase().trim();
        var rows = (DATA.Phones || []).filter(function (r) {
            if (!key) return true;
            return (r.Phone || '').toLowerCase().indexOf(key) >= 0
                || (r.FullName || '').toLowerCase().indexOf(key) >= 0;
        });

        $('#phoneCount').text(num(rows.length));

        if (!rows.length) {
            $('#tblPhone').html('<tr><td colspan="9" class="zns-empty">Không có dữ liệu.</td></tr>');
            return;
        }

        $('#tblPhone').html(rows.map(function (r) {
            var st = r.LastStatus || '';
            return '<tr>' +
                '<td><span class="tb-lead">' + esc(r.Phone || '(trống)') + '</span></td>' +
                '<td>' + esc(cut(r.FullName, 28)) + '</td>' +
                '<td class="zns-num"><b>' + num(r.TotalSend) + '</b></td>' +
                '<td class="zns-num">' + num(r.TemplateCount) + '</td>' +
                '<td class="zns-num">' + num(r.Sent) + '</td>' +
                '<td class="zns-num">' + num(r.Failed) + '</td>' +
                '<td class="zns-num">' + pct(r.SuccessRate) + '</td>' +
                '<td><span class="badge badge-dim ' + (STATUS_BADGE[st] || 'badge-light') + '">' +
                    esc(STATUS_LABEL[st] || st || '-') + '</span></td>' +
                '<td>' + esc(cut(r.LastErrorMessage, 42)) + '</td>' +
            '</tr>';
        }).join(''));
    }

    // ================================================================
    // 13. BANG CHI TIET
    // ================================================================
    function filteredDetails() {
        var key = ($('#txtSearchDetail').val() || '').toLowerCase().trim();
        var st = $('#ddlDetailStatus').val() || '';
        return (DATA.Details || []).filter(function (r) {
            if (st && r.QueueStatus !== st) return false;
            if (!key) return true;
            return (r.Phone || '').toLowerCase().indexOf(key) >= 0
                || (r.FullName || '').toLowerCase().indexOf(key) >= 0
                || (r.ErrorMessage || '').toLowerCase().indexOf(key) >= 0;
        });
    }

    function renderDetails() {
        var rows = filteredDetails();
        $('#detailCount').text(num(rows.length));

        if (!rows.length) {
            $('#tblDetail').html('<tr><td colspan="10" class="zns-empty">Không có dữ liệu.</td></tr>');
            return;
        }

        // Chi render toi da 1000 dong de trang khong bi nang
        var view = rows.slice(0, 1000);
        var html = view.map(function (r, i) {
            var st = r.QueueStatus || '';
            var delivered = (r.DeliveryStatus === 1)
                ? '<span class="badge badge-dim badge-success">Đã nhận</span>'
                : (r.DeliveryStatus == null
                    ? '<span class="text-soft">-</span>'
                    : '<span class="badge badge-dim badge-danger">Từ chối</span>');
            return '<tr>' +
                '<td class="zns-num">' + (i + 1) + '</td>' +
                '<td>' + esc(r.Phone) + '</td>' +
                '<td>' + esc(cut(r.FullName, 24)) + '</td>' +
                '<td>' + esc(cut(r.TemplateName, 28)) + '</td>' +
                '<td><span class="badge badge-dim ' + (STATUS_BADGE[st] || 'badge-light') + '">' +
                    esc(STATUS_LABEL[st] || st) + '</span></td>' +
                '<td>' + delivered + '</td>' +
                '<td>' + esc(cut(r.EventName, 24)) + '</td>' +
                '<td>' + dt(r.CompletedAt || r.StartedAt || r.ScheduledAt) + '</td>' +
                '<td class="zns-num">' + (r.ProcessSeconds == null ? '-' : r.ProcessSeconds) + '</td>' +
                '<td>' + esc(cut(r.ErrorMessage, 40)) + '</td>' +
            '</tr>';
        }).join('');

        if (rows.length > view.length) {
            html += '<tr><td colspan="10" class="zns-empty">Hiển thị ' + num(view.length) +
                '/' + num(rows.length) + ' dòng. Dùng "Xuất Excel" để xem đầy đủ.</td></tr>';
        }
        $('#tblDetail').html(html);
    }

    // ================================================================
    // 14. XUAT EXCEL (nhieu sheet)
    // ================================================================
    function exportExcel() {
        if (!DATA) { toast('Chưa có dữ liệu để xuất.', 'warning'); return; }

        var s = DATA.Summary || {};
        var wb = XLSX.utils.book_new();

        // Sheet 1 - Tong quan
        var summaryRows = [
            { 'Chỉ số': 'Chiến dịch', 'Giá trị': s.Title },
            { 'Chỉ số': 'Ngày tạo', 'Giá trị': d(s.CampaignCreatedDate) },
            { 'Chỉ số': 'SĐT trong danh sách', 'Giá trị': s.TotalPhoneInList },
            { 'Chỉ số': 'Tổng lượt gửi', 'Giá trị': s.TotalSend },
            { 'Chỉ số': 'Số ĐT tiếp cận', 'Giá trị': s.TotalPhoneTargeted },
            { 'Chỉ số': 'Gửi thành công', 'Giá trị': s.TotalSent },
            { 'Chỉ số': 'Thất bại', 'Giá trị': s.TotalFailed },
            { 'Chỉ số': 'Chờ gửi', 'Giá trị': s.TotalQueued },
            { 'Chỉ số': 'Đang xử lý', 'Giá trị': s.TotalProcessing },
            { 'Chỉ số': 'Gửi lại (Retry)', 'Giá trị': s.TotalRetry },
            { 'Chỉ số': 'Tổng số lần retry', 'Giá trị': s.TotalRetryCount },
            { 'Chỉ số': 'Zalo đã nhận', 'Giá trị': s.TotalDelivered },
            { 'Chỉ số': 'Zalo từ chối', 'Giá trị': s.TotalRejected },
            { 'Chỉ số': 'Tỷ lệ thành công (%)', 'Giá trị': s.SuccessRate },
            { 'Chỉ số': 'Tỷ lệ thất bại (%)', 'Giá trị': s.FailRate },
            { 'Chỉ số': 'Tỷ lệ Zalo nhận (%)', 'Giá trị': s.DeliveryRate },
            { 'Chỉ số': 'Số template sử dụng', 'Giá trị': s.TotalTemplateUsed },
            { 'Chỉ số': 'Số sự kiện', 'Giá trị': s.TotalEvent },
            { 'Chỉ số': 'Chờ hàng đợi TB (giây)', 'Giá trị': s.AvgWaitSeconds },
            { 'Chỉ số': 'Xử lý TB (giây)', 'Giá trị': s.AvgProcessSeconds },
            { 'Chỉ số': 'Chi phí ước tính (đ)', 'Giá trị': s.EstimatedCostSent },
            { 'Chỉ số': 'Chi phí / tin nhận (đ)', 'Giá trị': s.CostPerDelivered },
            { 'Chỉ số': 'Quota còn lại', 'Giá trị': s.RemainingQuota },
            { 'Chỉ số': 'Hạn mức ngày', 'Giá trị': s.DailyQuota }
        ];
        addSheet(wb, summaryRows, 'TongQuan');

        addSheet(wb, (DATA.StatusDistribution || []).map(function (r) {
            return { 'Trạng thái': r.StatusLabel, 'Số lượt': r.Quantity, 'Tỷ lệ (%)': r.Percentage };
        }), 'TrangThai');

        addSheet(wb, (DATA.Templates || []).map(function (r) {
            return {
                'TemplateId': r.TemplateId, 'Tên template': r.TemplateName,
                'Trạng thái': r.TemplateStatus, 'Đơn giá': r.Price,
                'Lượt gửi': r.TotalSend, 'Số ĐT': r.DistinctPhone,
                'Thành công': r.Sent, 'Thất bại': r.Failed, 'Retry': r.Retry,
                'Zalo nhận': r.Delivered, 'Tỷ lệ TC (%)': r.SuccessRate,
                'Chi phí (đ)': r.EstimatedCost
            };
        }), 'Template');

        addSheet(wb, (DATA.EventCats || []).map(function (r) {
            return {
                'EventCatId': r.EventCatId, 'Nhóm sự kiện': r.EventCatName,
                'Lượt gửi': r.TotalSend, 'Số ĐT': r.DistinctPhone,
                'Thành công': r.Sent, 'Thất bại': r.Failed,
                'Zalo nhận': r.Delivered, 'Tỷ lệ TC (%)': r.SuccessRate
            };
        }), 'NhomSuKien');

        addSheet(wb, (DATA.Events || []).map(function (r) {
            return {
                'EventId': r.EventId, 'Sự kiện': r.EventName,
                'Nhóm': r.EventCatName, 'Địa điểm': r.Location,
                'Thời gian': d(r.FromDate),
                'Lượt gửi': r.TotalSend, 'Số ĐT': r.DistinctPhone,
                'Thành công': r.Sent, 'Thất bại': r.Failed,
                'Zalo nhận': r.Delivered, 'Tỷ lệ TC (%)': r.SuccessRate
            };
        }), 'SuKien');

        addSheet(wb, (DATA.Phones || []).map(function (r) {
            return {
                'Số điện thoại': r.Phone, 'Họ tên': r.FullName,
                'Lượt gửi': r.TotalSend, 'Số template': r.TemplateCount,
                'Thành công': r.Sent, 'Thất bại': r.Failed, 'Retry': r.Retry,
                'Đang chờ': r.Pending, 'Zalo nhận': r.Delivered,
                'Tỷ lệ TC (%)': r.SuccessRate,
                'Lần gửi đầu': dt(r.FirstSendTime), 'Lần gửi cuối': dt(r.LastSendTime),
                'Trạng thái cuối': STATUS_LABEL[r.LastStatus] || r.LastStatus,
                'Mã lỗi cuối': r.LastErrorCode, 'Lỗi cuối': r.LastErrorMessage
            };
        }), 'TheoSoDienThoai');

        addSheet(wb, (DATA.Timeline || []).map(function (r) {
            return {
                'Ngày': d(r.SendDate), 'Lượt gửi': r.TotalSend,
                'Thành công': r.Sent, 'Thất bại': r.Failed,
                'Đang chờ': r.Pending, 'Zalo nhận': r.Delivered,
                'Tỷ lệ TC (%)': r.SuccessRate
            };
        }), 'TheoNgay');

        addSheet(wb, (DATA.Hours || []).map(function (r) {
            return {
                'Khung giờ': r.SendHour + 'h', 'Lượt gửi': r.TotalSend,
                'Thành công': r.Sent, 'Thất bại': r.Failed, 'Tỷ lệ TC (%)': r.SuccessRate
            };
        }), 'TheoKhungGio');

        addSheet(wb, (DATA.Errors || []).map(function (r) {
            return {
                'Mã lỗi': r.ErrorCode, 'Mô tả': r.ErrorMessage,
                'Số lượt': r.Quantity, 'Số ĐT': r.DistinctPhone,
                'Tỷ lệ (%)': r.Percentage, 'Lần cuối': dt(r.LastOccurredTime)
            };
        }), 'NguyenNhanLoi');

        addSheet(wb, (DATA.Details || []).map(function (r, i) {
            return {
                'STT': i + 1, 'QueueId': r.QueueId, 'LogId': r.LogId,
                'Số điện thoại': r.Phone, 'Họ tên': r.FullName,
                'TemplateId': r.TemplateId, 'Template': r.TemplateName,
                'Trạng thái': STATUS_LABEL[r.QueueStatus] || r.QueueStatus,
                'Log API': r.LogStatus,
                'Zalo nhận': r.DeliveryStatus === 1 ? 'Đã nhận' : (r.DeliveryStatus == null ? '' : 'Từ chối'),
                'Số lần retry': r.RetryCount,
                'Mã lỗi': r.ErrorCode, 'Lỗi': r.ErrorMessage,
                'MsgId': r.MsgId, 'TrackingId': r.TrackingId,
                'Nhóm sự kiện': r.EventCatName, 'Sự kiện': r.EventName,
                'Xếp hàng lúc': dt(r.ScheduledAt), 'Bắt đầu': dt(r.StartedAt),
                'Hoàn tất': dt(r.CompletedAt), 'Zalo gửi lúc': dt(r.SentTime),
                'Chờ (s)': r.WaitSeconds, 'Xử lý (s)': r.ProcessSeconds,
                'Tổng (s)': r.TotalSeconds, 'Đơn giá': r.Price
            };
        }), 'ChiTiet');

        var name = 'ZaloZNS_Campaign' + campaignId + '_' +
            new Date().toISOString().slice(0, 10) + '.xlsx';
        XLSX.writeFile(wb, name);
        toast('Đã xuất file ' + name, 'success');
    }

    function addSheet(wb, rows, name) {
        if (!rows || !rows.length) { rows = [{ 'Thông báo': 'Không có dữ liệu' }]; }
        var ws = XLSX.utils.json_to_sheet(rows);
        var keys = Object.keys(rows[0]);
        ws['!cols'] = keys.map(function (k) { return { wch: Math.min(Math.max(k.length + 4, 12), 40) }; });
        XLSX.utils.book_append_sheet(wb, ws, name);
    }
</script>
