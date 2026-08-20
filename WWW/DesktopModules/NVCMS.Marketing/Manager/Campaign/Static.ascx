<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Static.ascx.vb" Inherits="NVCMS.Modules.Marketing.CamPaingMailStatic" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />

<style>
    /* Custom Scrollable Table Container */
    .table-scroll-container {
        max-height: 500px;
        overflow-y: auto;
        overflow-x: hidden;
    }

        /* Custom Scrollbar Styling */
        .table-scroll-container::-webkit-scrollbar {
            width: 8px;
        }

        .table-scroll-container::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 4px;
        }

        .table-scroll-container::-webkit-scrollbar-thumb {
            background: #888;
            border-radius: 4px;
        }

            .table-scroll-container::-webkit-scrollbar-thumb:hover {
                background: #555;
            }

    /* Ensure table header stays visible */
    .nk-tb-head {
        position: sticky;
        top: 0;
        background: #fff;
        z-index: 10;
    }
</style>
<asp:Label ID="lblblblb" runat="server"></asp:Label>
<div class="nk-body bg-lighter npc-general has-sidebar">

    <div class="nk-app-root">
        <div class="nk-main">
            <!-- Content Body -->
            <div class="nk-content">
                <div class="container-fluid">
                    <div class="nk-content-inner">
                        <div class="nk-content-body">

                            <!-- Header Title -->
                            <div class="nk-block-head nk-block-head-sm">
                                <div class="nk-block-between">
                                    <div class="nk-block-head-content">
                                        <h3 class="nk-block-title page-title" id="lblCampaignTitle">Campaign Dashboard</h3>
                                        <div class="nk-block-des text-soft">
                                            <p id="lblCampaignDesc">Thống kê hiệu quả chiến dịch Email Marketing</p>
                                        </div>
                                    </div>
                                    <div class="nk-block-head-content">
                                        <div class="toggle-wrap nk-block-tools-toggle">
                                            <%--<button type="button" class="btn btn-primary" onclick="exportUnopenedToExcel()">
                                                <em class="icon ni ni-file-xls"></em><span>Xuất Excel Mail Chưa Mở</span>
                                            </button>--%>
                                            <button type="button" class="btn btn-primary" id="btnExportExcelClient">
                                                <em class="icon ni ni-file-xls"></em><span>Xuất Excel Mail Chưa Mở</span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- .nk-block-head -->

                            <!-- KPI Summary Cards -->
                            <div class="nk-block">
                                <div class="row g-gx-14 g-gy-3">
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-2">
                                                    <div class="card-title">
                                                        <h6 class="title">Tổng Nhận (Recipient)</h6>
                                                    </div>
                                                </div>
                                                <div class="align-end flex-between font-weight-bold">
                                                    <div class="amount h3 mb-0" id="kpiTotalRecipient">0</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-2">
                                                    <div class="card-title">
                                                        <h6 class="title text-primary">Đã Mở (Opened)</h6>
                                                    </div>
                                                </div>
                                                <div class="align-end flex-between font-weight-bold">
                                                    <div class="amount h3 mb-0 text-primary" id="kpiTotalOpened">0</div>
                                                    <div class="sub-text" id="kpiOpenRate">0%</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-2">
                                                    <div class="card-title">
                                                        <h6 class="title text-warning">Chưa Mở (Unopened)</h6>
                                                    </div>
                                                </div>
                                                <div class="align-end flex-between font-weight-bold">
                                                    <div class="amount h3 mb-0 text-warning" id="kpiTotalUnopened">0</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-3 col-6">
                                        <div class="card card-bordered">
                                            <div class="card-inner">
                                                <div class="card-title-group align-start mb-2">
                                                    <div class="card-title">
                                                        <h6 class="title text-danger">Bounced / Unsub</h6>
                                                    </div>
                                                </div>
                                                <div class="align-end flex-between font-weight-bold">
                                                    <div class="amount h3 mb-0 text-danger" id="kpiTotalBounced">0</div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- .nk-block -->

                            <!-- Charts Section -->
                            <div class="nk-block">
                                <div class="row g-gx-14 g-gy-3">
                                    <!-- Doughnut Chart: Tỷ lệ Mở / Chưa Mở -->
                                    <div class="col-lg-5">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <div class="card-title-group">
                                                    <div class="card-title">
                                                        <h6 class="title">Tỷ Lệ Mở Mail (Open Rate)</h6>
                                                    </div>
                                                </div>
                                                <div class="nk-ck-sm my-4">
                                                    <canvas class="chart-canvas" id="openRateDoughnutChart"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Bar Chart: Phân tích chi tiết -->
                                    <div class="col-lg-7">
                                        <div class="card card-bordered h-100">
                                            <div class="card-inner">
                                                <div class="card-title-group">
                                                    <div class="card-title">
                                                        <h6 class="title">Biểu Đồ Trạng Thái Chi Tiết</h6>
                                                    </div>
                                                </div>
                                                <div class="nk-ck-sm my-4">
                                                    <canvas class="chart-canvas" id="statusBarChart"></canvas>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- .nk-block -->

                            <!-- Table Details: Danh sách Email Chưa Mở -->
                            <div class="nk-block">
                                <div class="card card-bordered card-stretch">
                                    <div class="card-inner-group">
                                        <div class="card-inner position-relative card-tools-toggle">
                                            <div class="card-title-group">
                                                <div class="card-tools">
                                                    <h5 class="title text-warning">
                                                        <em class="icon ni ni-mail-fill"></em>Danh Sách Email Chưa Mở 
                                                            (<span id="unopenedCount">0</span>)
                                                    </h5>
                                                </div>
                                                <div class="card-tools me-n1">
                                                    <ul class="btn-toolbar gx-1">
                                                        <li>
                                                            <button type="button" class="btn btn-primary" id="btnResendSelected" onclick="showResendModal()" disabled>
                                                                <em class="icon ni ni-send"></em><span>Resend Selected</span>
                                                            </button>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-inner p-0">
                                            <div style="max-height: 500px; overflow-y: auto;">
                                                <table class="table table-tranx is-compact">
                                                    <thead class="tb-tnx-head" style="position:sticky;top:0;background:#fff;z-index:10;">
                                                        <tr>
                                                            <th style="width:40px;">
                                                                <div class="custom-control custom-control-sm custom-checkbox notext">
                                                                    <input type="checkbox" class="custom-control-input" id="selectAllUnopened" onclick="toggleSelectAll(this)">
                                                                    <label class="custom-control-label" for="selectAllUnopened"></label>
                                                                </div>
                                                            </th>
                                                            <th><span class="sub-text">Send Log ID</span></th>
                                                            <th><span class="sub-text">Email</span></th>
                                                            <th><span class="sub-text">Status</span></th>
                                                            <th><span class="sub-text">Thời Gian Gửi</span></th>
                                                            <th><span class="sub-text">Thời Gian Mở</span></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tableUnopenedBody"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- .nk-block -->

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
<!-- Dashlite JS & Dependencies -->


<!-- Thư viện Export Excel Client-side (SheetJS) -->
<script src="https://cdn.jsdelivr.net/npm/xlsx@0.18.5/dist/xlsx.full.min.js"></script>

<script src="https://cdn.jsdelivr.net/npm/xlsx@0.18.5/dist/xlsx.full.min.js"></script>
<script>
    // ============================================================
    // 1. URL PARAMETERS
    // ============================================================

    var urlParams = new URLSearchParams(window.location.search);

    var sendId = urlParams.get('sendid') || urlParams.get('itemid');

    console.log("sendId:", sendId);

    if (!sendId) {
        console.warn("Không tìm thấy sendid hoặc itemid trên URL!");
    }

    var moduleId = <%= ModuleId %>;

    console.log("moduleId:", moduleId);


    // ============================================================
    // 2. DOTNETNUKE SERVICES FRAMEWORK
    // ============================================================

    var sf = null;

    if (typeof $.ServicesFramework === 'function') {
        sf = $.ServicesFramework(moduleId);
    }

    console.log("sf:", sf);


    // ============================================================
    // 3. SERVICE ROOT
    // ============================================================

    var serviceRoot = "/DesktopModules/NVCMS/API/Report/";


    // ============================================================
    // 4. GLOBAL VARIABLES
    // ============================================================

    var globalUnopenedList = [];
    var globalCampaignTitle = "Campaign";


    // ============================================================
    // 5. DOCUMENT READY
    // ============================================================

    $(document).ready(function () {

        var sendIdNum = parseInt(sendId, 10);

        if (sendIdNum > 0) {

            console.log("Loading dashboard. CampaignSendId:", sendIdNum);

            loadDashboardData(sendIdNum);

        } else {

            console.warn("sendId không hợp lệ hoặc <= 0:", sendId);

        }


        // Export Excel
        $('#btnExportExcelClient').on('click', function () {
            exportUnopenedToExcel();
        });

    });


    // ============================================================
    // 6. LOAD DASHBOARD DATA
    // ============================================================

    function loadDashboardData(campaignSendId) {
        $("#loading").show();
        var ajaxOptions = {
            url: serviceRoot + "GetDashboard?campaignSendId=" + encodeURIComponent(campaignSendId),
            type: 'GET',
            dataType: 'json',

            success: function (res) {
                $("#loading").hide();
                console.log("Dashboard API response:", res);

                if (res && res.Success && res.Data) {

                    var summary = res.Data.Summary || {};
                    var details = res.Data.Details || [];

                    // ------------------------------------------------
                    // Campaign title
                    // ------------------------------------------------

                    globalCampaignTitle = summary.Title || "Campaign";

                    $('#lblCampaignTitle').text(
                        summary.Title || "Campaign Dashboard"
                    );


                    // ------------------------------------------------
                    // KPI
                    // ------------------------------------------------

                    var totalRecipient = Number(summary.TotalRecipient) || 0;
                    var totalOpened = Number(summary.TotalOpened) || 0;
                    var totalBounced = Number(summary.TotalBounced) || 0;
                    var totalUnsubscribed = Number(summary.TotalUnsubscribed) || 0;
                    var totalClicked = Number(summary.TotalClicked) || 0;


                    var unopenedCount = totalRecipient - totalOpened;

                    if (unopenedCount < 0) {
                        unopenedCount = 0;
                    }


                    $('#kpiTotalRecipient').text(
                        totalRecipient.toLocaleString()
                    );

                    $('#kpiTotalOpened').text(
                        totalOpened.toLocaleString()
                    );

                    $('#kpiTotalUnopened').text(
                        unopenedCount.toLocaleString()
                    );

                    $('#kpiTotalBounced').text(
                        (totalBounced + totalUnsubscribed).toLocaleString()
                    );


                    // ------------------------------------------------
                    // Open rate
                    // ------------------------------------------------

                    var openRatePercent = totalRecipient > 0
                        ? ((totalOpened / totalRecipient) * 100).toFixed(2)
                        : "0.00";

                    $('#kpiOpenRate').text(
                        openRatePercent + '%'
                    );


                    // ------------------------------------------------
                    // Render charts
                    // ------------------------------------------------

                    if (typeof renderCharts === 'function') {

                        renderCharts(
                            totalOpened,
                            unopenedCount,
                            totalBounced,
                            totalClicked
                        );

                    }


                    // ------------------------------------------------
                    // Filter unopened emails
                    // ------------------------------------------------

                    globalUnopenedList = details.filter(function (item) {

                        return item.OpenedTime === null ||
                            typeof item.OpenedTime === 'undefined';

                    });


                    console.log(
                        "Unopened emails:",
                        globalUnopenedList.length
                    );


                    // ------------------------------------------------
                    // Render unopened table
                    // ------------------------------------------------

                    if (typeof renderUnopenedTable === 'function') {

                        renderUnopenedTable(
                            globalUnopenedList
                        );

                    }

                } else {

                    var message = res && res.Message
                        ? res.Message
                        : "Không thể tải dữ liệu dashboard.";

                    showError(message);

                }

            },

            error: function (xhr, status, error) {
                $("#loading").hide();
                console.error(
                    "Chi tiết lỗi API:",
                    xhr.responseText
                );

                console.error(
                    "Status:",
                    status
                );

                console.error(
                    "Error:",
                    error
                );

                showError(
                    "Lỗi kết nối API: " + error
                );

            }
        };


        // ========================================================
        // DotNetNuke ServicesFramework headers
        // ========================================================

        if (sf) {

            ajaxOptions.beforeSend = function (xhr) {

                sf.setModuleHeaders(xhr);

            };

        }


        $.ajax(ajaxOptions);
    }


    // ============================================================
    // 7. SHOW ERROR
    // ============================================================

    function showError(message) {

        if (typeof NioApp !== 'undefined' &&
            NioApp.Toast) {

            NioApp.Toast(
                message,
                'error'
            );

        } else {

            alert(message);

        }

    }


    // ============================================================
    // 8. RENDER CHARTS
    // ============================================================

    function renderCharts(
        opened,
        unopened,
        bounced,
        clicked
    ) {

        // --------------------------------------------------------
        // Chart 1: Doughnut
        // --------------------------------------------------------

        var doughnutElement = document.getElementById(
            'openRateDoughnutChart'
        );

        if (doughnutElement) {

            var ctxDoughnut = doughnutElement.getContext('2d');

            new Chart(ctxDoughnut, {

                type: 'doughnut',

                data: {

                    labels: [
                        'Đã Mở',
                        'Chưa Mở'
                    ],

                    datasets: [{

                        data: [
                            opened,
                            unopened
                        ],

                        backgroundColor: [
                            '#1ee0ac',
                            '#f4bd0e'
                        ],

                        borderWidth: 2,

                        borderColor: '#ffffff'

                    }]

                },

                options: {

                    legend: {
                        position: 'bottom'
                    },

                    responsive: true,

                    maintainAspectRatio: false

                }

            });

        }


        // --------------------------------------------------------
        // Chart 2: Bar
        // --------------------------------------------------------

        var barElement = document.getElementById(
            'statusBarChart'
        );

        if (barElement) {

            var ctxBar = barElement.getContext('2d');

            new Chart(ctxBar, {

                type: 'bar',

                data: {

                    labels: [
                        'Đã Mở',
                        'Chưa Mở',
                        'Clicked',
                        'Bounced'
                    ],

                    datasets: [{

                        label: 'Số Lượng Mail',

                        data: [
                            opened,
                            unopened,
                            clicked,
                            bounced
                        ],

                        backgroundColor: [
                            '#1ee0ac',
                            '#f4bd0e',
                            '#09c2de',
                            '#e85347'
                        ]

                    }]

                },

                options: {

                    legend: {
                        display: false
                    },

                    responsive: true,

                    maintainAspectRatio: false,

                    scales: {

                        yAxes: [{

                            ticks: {
                                beginAtZero: true
                            }

                        }]

                    }

                }

            });

        }

    }


    // ============================================================
    // 9. RENDER UNOPENED TABLE
    // ============================================================

    function renderUnopenedTable(list) {

        list = list || [];

        $('#unopenedCount').text(list.length);

        var html = '';

        if (list.length === 0) {

            html = '<tr><td colspan="6" class="text-center">Không có email nào chưa mở.</td></tr>';

        } else {

            $.each(list, function (idx, item) {

                var sentTimeFormatted = item.SentTime
                    ? new Date(item.SentTime).toLocaleString('vi-VN')
                    : '-';

                html += '<tr>';

                // Checkbox
                html += '<td>' +
                    '<div class="custom-control custom-control-sm custom-checkbox notext">' +
                    '<input type="checkbox" class="custom-control-input email-checkbox" ' +
                    'id="email_' + item.SendLogId + '" ' +
                    'data-id="' + item.SendLogId + '" ' +
                    'data-email="' + (item.Email || '') + '" ' +
                    'data-listmailid="' + (item.ListMailId || '') + '" ' +
                    'onchange="updateSelectedEmails()">' +
                    '<label class="custom-control-label" for="email_' + item.SendLogId + '"></label>' +
                    '</div>' +
                    '</td>';

                // Send Log ID
                html += '<td><span class="tb-lead">#' + (item.SendLogId || '') + '</span></td>';

                // Email
                html += '<td><span class="tb-sub font-weight-bold text-dark">' + (item.Email || '') + '</span></td>';

                // Status
                html += '<td><span class="badge badge-dim badge-warning">' + (item.Status || '') + '</span></td>';

                // Sent Time
                html += '<td><span class="tb-sub">' + sentTimeFormatted + '</span></td>';

                // Open status
                html += '<td><span class="badge badge-dot badge-danger">Chưa mở</span></td>';

                html += '</tr>';

            });

        }

        $('#tableUnopenedBody').html(html);

    }


    // ============================================================
    // 10. EXPORT UNOPENED EMAILS TO EXCEL
    // ============================================================

    function exportUnopenedToExcel() {

        if (
            !globalUnopenedList ||
            globalUnopenedList.length === 0
        ) {

            if (
                typeof NioApp !== 'undefined' &&
                NioApp.Toast
            ) {

                NioApp.Toast(
                    'Không có dữ liệu mail chưa mở để xuất Excel!',
                    'warning'
                );

            } else {

                alert(
                    'Không có dữ liệu mail chưa mở để xuất Excel!'
                );

            }

            return;

        }


        // --------------------------------------------------------
        // Convert data
        // --------------------------------------------------------

        var excelData = globalUnopenedList.map(
            function (item, index) {

                return {

                    "STT":
                        index + 1,

                    "Send Log ID":
                        item.SendLogId || '',

                    "Campaign Send ID":
                        item.CampaignSendId || '',

                    "List Mail ID":
                        item.ListMailId || 0,

                    "Email":
                        item.Email || '',

                    "Trạng Thái":
                        item.Status || '',

                    "Thời Gian Gửi":
                        item.SentTime
                            ? new Date(
                                item.SentTime
                            ).toLocaleString('vi-VN')
                            : '',

                    "Thời Gian Tạo":
                        item.CreatedDate
                            ? new Date(
                                item.CreatedDate
                            ).toLocaleString('vi-VN')
                            : ''

                };

            }
        );


        // --------------------------------------------------------
        // Create worksheet
        // --------------------------------------------------------

        var worksheet =
            XLSX.utils.json_to_sheet(
                excelData
            );


        // --------------------------------------------------------
        // Column widths
        // --------------------------------------------------------

        worksheet['!cols'] = [

            { wch: 6 },   // STT
            { wch: 12 },  // Send Log ID
            { wch: 18 },  // Campaign Send ID
            { wch: 12 },  // List Mail ID
            { wch: 30 },  // Email
            { wch: 15 },  // Status
            { wch: 22 },  // Sent Time
            { wch: 22 }   // Created Date

        ];


        // --------------------------------------------------------
        // Create workbook
        // --------------------------------------------------------

        var workbook =
            XLSX.utils.book_new();


        XLSX.utils.book_append_sheet(
            workbook,
            worksheet,
            "Emails_Chua_Mo"
        );


        // --------------------------------------------------------
        // File name
        // --------------------------------------------------------

        var fileName =
            "Unopened_Emails_" +
            new Date()
                .toISOString()
                .slice(0, 10) +
            ".xlsx";


        // --------------------------------------------------------
        // Download
        // --------------------------------------------------------

        XLSX.writeFile(
            workbook,
            fileName
        );

    }

    // ============================================================
    // RESEND FUNCTIONALITY
    // ============================================================

    var selectedEmails = [];

    function toggleSelectAll(checkbox) {
        var checkboxes = document.querySelectorAll('.email-checkbox');
        checkboxes.forEach(function (cb) {
            cb.checked = checkbox.checked;
        });
        updateSelectedEmails();
    }

    function updateSelectedEmails() {
        selectedEmails = [];
        var checkboxes = document.querySelectorAll('.email-checkbox:checked');
        checkboxes.forEach(function (cb) {
            selectedEmails.push({
                id: cb.getAttribute('data-id'),
                email: cb.getAttribute('data-email'),
                listMailId: cb.getAttribute('data-listmailid')
            });
        });

        // Update button state and count
        var btnResend = document.getElementById('btnResendSelected');
        if (selectedEmails.length > 0) {
            btnResend.disabled = false;
            btnResend.innerHTML = '<em class="icon ni ni-send"></em><span>Resend Selected (' + selectedEmails.length + ')</span>';
        } else {
            btnResend.disabled = true;
            btnResend.innerHTML = '<em class="icon ni ni-send"></em><span>Resend Selected</span>';
        }

        // Update hidden field
        document.getElementById('<%= hdnSelectedEmails.ClientID %>').value = JSON.stringify(selectedEmails);
    }

    function showResendModal() {
        if (selectedEmails.length === 0) {
            alert('Vui lòng chọn ít nhất một email để resend.');
            return;
        }

        // Show modal
        $('#resendModal').modal('show');

        // Update selected count in modal
        document.getElementById('selectedEmailCount').innerText = selectedEmails.length;
    }

</script>

<!-- Resend Modal -->
<div class="modal fade" id="resendModal" tabindex="-1" role="dialog" aria-labelledby="resendModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-xl" role="document" style="max-width: 90%;">
        <div class="modal-content">
            <div class="modal-header bg-primary text-white">
                <h5 class="modal-title" id="resendModalLabel">
                    <em class="icon ni ni-send"></em>Resend Email Campaign
                </h5>
                <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel runat="server" ID="upnlResend">
                    <ContentTemplate>
                        <!-- Result Message Area -->
                        <asp:Panel ID="pnlResendResult" runat="server" Visible="false" CssClass="mb-3">
                            <div class="alert alert-dismissible fade show" role="alert" id="resendResultAlert">
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <asp:Literal ID="ltrResendResult" runat="server"></asp:Literal>
                            </div>
                        </asp:Panel>

                        <div class="row">
                            <!-- Left Column: Form Fields -->
                            <div class="col-md-6">
                                <div class="alert alert-info mb-3">
                                    <em class="icon ni ni-info"></em>
                                    Bạn đã chọn <strong><span id="selectedEmailCount">0</span></strong> email để resend.
                                </div>

                                <div class="alert alert-warning mb-4">
                                    <em class="icon ni ni-alert"></em>
                                    <strong>Lưu ý:</strong> Mỗi email chỉ được resend tối đa <strong><%= MaxResendCount %></strong> lần. 
                                    Email đã vượt quá giới hạn sẽ được bỏ qua.
                                </div>

                                <div class="form-group mb-3">
                                    <label class="form-label"><b>CHỌN TEMPLATE: </b><span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlResendTemplate" runat="server" CssClass="form-select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlResendTemplate_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="form-group mb-3">
                                    <label class="form-label"><b>CHỌN EMAIL GỬI ĐI: </b><span class="text-danger">*</span></label>
                                    <asp:DropDownList ID="ddlResendEmail" runat="server" CssClass="form-select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlResendEmail_SelectedIndexChanged"></asp:DropDownList>
                                </div>

                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group mb-3">
                                            <label class="form-label"><b>CHỌN SỰ KIỆN: </b></label>
                                            <asp:DropDownList ID="ddlResendEventCat" runat="server" CssClass="form-select form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlResendEventCat_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group mb-3">
                                            <label class="form-label"><b>ĐIA ĐIỂM: </b></label>
                                            <asp:DropDownList ID="ddlResendEvent" runat="server" CssClass="form-select form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group mb-3">
                                    <label class="form-label"><b>TIÊU ĐỀ MAIL: </b><span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtResendTitleMail" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                                <div class="form-group mb-3">
                                    <label class="form-label"><b>NỘI DUNG HIỂN THỊ VIEW MAIL: </b></label>
                                    <asp:TextBox ID="txtResendContentView" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>

                                <div class="form-group mb-3">
                                    <label class="form-label"><b>Email Sender Name: </b></label>
                                    <div class="form-control-plaintext">
                                        <asp:Literal ID="ltrResendEmailName" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>

                            <!-- Right Column: Preview Template -->
                            <div class="col-md-6">
                                <div class="card card-bordered h-100">
                                    <div class="card-header bg-light">
                                        <h6 class="title mb-0">
                                            <em class="icon ni ni-eye"></em>Preview Template
                                        </h6>
                                    </div>
                                    <div class="card-inner" style="max-height: 600px; overflow-y: auto; background-color: #f8f9fa;">
                                        <asp:Literal ID="ltrResendEmailPreview" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <asp:HiddenField ID="hdnSelectedEmails" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                    <em class="icon ni ni-cross"></em>Đóng
                </button>
                <asp:LinkButton ID="btnConfirmResend" runat="server" CssClass="btn btn-primary" OnClick="btnConfirmResend_Click">
                    <em class="icon ni ni-send"></em> Xác Nhận Resend
                </asp:LinkButton>
            </div>
        </div>
    </div>
</div>

<asp:UpdateProgress runat="server" ID="UpdateProgressResend" AssociatedUpdatePanelID="upnlResend">
    <ProgressTemplate>
        <div style="top: 0; left: 0; width: 100vw; height: 100vh; padding: 20% 45%; background: #00000030; position: fixed; z-index: 9999;">
            <div class="spinner-border text-primary" role="status" style="width: 10rem !important; height: 10rem !important;">
                <span class="sr-only">Loading...</span>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

