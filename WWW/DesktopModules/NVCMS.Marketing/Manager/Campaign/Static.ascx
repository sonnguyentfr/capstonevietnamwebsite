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
                                            </div>
                                        </div>
                                        <div class="card-inner p-0">
                                            <div class="nk-tb-list nk-tb-ulist" data-simplebar style="max-height: 500px; overflow-y: auto;">
                                                <!-- Fixed Header -->
                                                <div class="nk-tb-item nk-tb-head">
                                                    <div class="nk-tb-col"><span class="sub-text">Send Log ID</span></div>
                                                    <div class="nk-tb-col"><span class="sub-text">Email</span></div>
                                                    <div class="nk-tb-col tb-col-md"><span class="sub-text">Status</span></div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="sub-text">Thời Gian Gửi</span></div>
                                                    <div class="nk-tb-col tb-col-md"><span class="sub-text">Thời Gian Mở</span></div>
                                                </div>
                                                <!-- Scrollable Table Body -->
                                                <div class="table-scroll-container">
                                                    <div id="tableUnopenedBody"></div>
                                                </div>
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

        $('#unopenedCount').text(
            list.length
        );


        var html = '';


        // --------------------------------------------------------
        // Không có dữ liệu
        // --------------------------------------------------------

        if (list.length === 0) {

            html =
                '<div class="nk-tb-item">' +
                '<div class="nk-tb-col text-center" colspan="5">' +
                'Không có email nào chưa mở.' +
                '</div>' +
                '</div>';

        } else {

            $.each(list, function (idx, item) {

                var sentTimeFormatted = item.SentTime
                    ? new Date(item.SentTime).toLocaleString('vi-VN')
                    : '-';


                html +=
                    '<div class="nk-tb-item">';


                // Send Log ID
                html +=
                    '<div class="nk-tb-col">' +
                    '<span class="tb-lead">#' +
                    (item.SendLogId || '') +
                    '</span>' +
                    '</div>';


                // Email
                html +=
                    '<div class="nk-tb-col">' +
                    '<span class="tb-sub font-weight-bold text-dark">' +
                    (item.Email || '') +
                    '</span>' +
                    '</div>';


                // Status
                html +=
                    '<div class="nk-tb-col tb-col-md">' +
                    '<span class="badge badge-dim badge-warning">' +
                    (item.Status || '') +
                    '</span>' +
                    '</div>';


                // Sent Time
                html +=
                    '<div class="nk-tb-col tb-col-lg">' +
                    '<span class="tb-sub">' +
                    sentTimeFormatted +
                    '</span>' +
                    '</div>';


                // Open status
                html +=
                    '<div class="nk-tb-col tb-col-md">' +
                    '<span class="badge badge-dot badge-danger">' +
                    'Chưa mở' +
                    '</span>' +
                    '</div>';


                html += '</div>';

            });

        }


        $('#tableUnopenedBody').html(
            html
        );

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
</script>
