<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Static.ascx.vb" Inherits="NVCMS.Modules.Marketing.CamPaingMailStatic" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />

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
                                            <div class="nk-tb-list nk-tb-ulist">
                                                <div class="nk-tb-item nk-tb-head">
                                                    <div class="nk-tb-col"><span class="sub-text">Send Log ID</span></div>
                                                    <div class="nk-tb-col"><span class="sub-text">Email</span></div>
                                                    <div class="nk-tb-col tb-col-md"><span class="sub-text">Status</span></div>
                                                    <div class="nk-tb-col tb-col-lg"><span class="sub-text">Thời Gian Gửi</span></div>
                                                    <div class="nk-tb-col tb-col-md"><span class="sub-text">Thời Gian Mở</span></div>
                                                </div>
                                                <!-- Dynamic Rows Inserted via JS -->
                                                <div id="tableUnopenedBody"></div>
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

<!-- Dashlite JS & Dependencies -->


<!-- Thư viện Export Excel Client-side (SheetJS) -->
<script src="https://cdn.jsdelivr.net/npm/xlsx@0.18.5/dist/xlsx.full.min.js"></script>

<script src="https://cdn.jsdelivr.net/npm/xlsx@0.18.5/dist/xlsx.full.min.js"></script>
<script>
    // 1. Khởi tạo đối tượng đọc URL Parameters chuẩn của trình duyệt
    var urlParams = new URLSearchParams(window.location.search);

    // 2. Lấy ID và kiểm tra xem có giá trị không
    var sendId = urlParams.get('sendid') || urlParams.get('itemid');
    console.log("sendId", sendId);

    // Nếu không có sendId, cảnh báo
    if (!sendId) {
        console.warn("Không tìm thấy sendid hoặc itemid trên URL!");
        // sendId = 1263; // Set cứng ID test nếu cần
    }

    var moduleId = <%= ModuleId %>;
    console.log("moduleId", moduleId);

    var sf = (typeof $.ServicesFramework === 'function') ? $.ServicesFramework(moduleId) : null;
    console.log("sf", sf);

    var serviceRoot = "/DesktopModules/NVCMS/API/Report/";

    // 3. Sử dụng cú pháp jQuery Ready chuẩn
    $(document).ready(function () {
        // Chuyển sendId về số nguyên để so sánh chuẩn xác
        var sendIdNum = parseInt(sendId, 10);
        if (sendIdNum > 0) {
            console.log("aaaaaaaaa"); // ✅ Đã fix lỗi cconsole thành console
            loadDashboardData(sendIdNum);
        } else {
            console.warn("sendId không hợp lệ hoặc <= 0:", sendId);
        }
    });

    function loadDashboardData() {
        var reqHeaders = {};
        if (sf) {
            reqHeaders["ModuleId"] = sf.getModuleId();
            reqHeaders["TabId"] = sf.getTabId();
            reqHeaders["RequestVerificationToken"] = sf.getAntiForgeryValue();
        }

        $.ajax({
            url: serviceRoot + "GetDashboard?campaignSendId=" + id,
            type: 'GET',
            dataType: 'json',
            beforeSend: sf.setModuleHeaders,
            success: function (res) {
                if (res && res.Success && res.Data) {
                    var summary = res.Data.Summary;
                    var details = res.Data.Details || [];

                    $('#lblCampaignTitle').text(summary.Title || "Campaign Dashboard");
                    $('#kpiTotalRecipient').text(summary.TotalRecipient.toLocaleString());
                    $('#kpiTotalOpened').text(summary.TotalOpened.toLocaleString());

                    var unopenedCount = summary.TotalRecipient - summary.TotalOpened;
                    if (unopenedCount < 0) unopenedCount = 0;
                    $('#kpiTotalUnopened').text(unopenedCount.toLocaleString());
                    $('#kpiTotalBounced').text((summary.TotalBounced + summary.TotalUnsubscribed).toLocaleString());

                    var openRatePercent = summary.TotalRecipient > 0
                        ? ((summary.TotalOpened / summary.TotalRecipient) * 100).toFixed(2)
                        : 0;
                    $('#kpiOpenRate').text(openRatePercent + '%');

                    // Gọi hàm render Charts
                    if (typeof renderCharts === "function") {
                        renderCharts(summary.TotalOpened, unopenedCount, summary.TotalBounced, summary.TotalClicked);
                    }

                    globalUnopenedList = details.filter(function (item) {
                        return item.OpenedTime === null;
                    });

                    // Gọi hàm render Table
                    if (typeof renderUnopenedTable === "function") {
                        renderUnopenedTable(globalUnopenedList);
                    }
                } else {
                    if (typeof NioApp !== 'undefined') {
                        NioApp.Toast('Không thể tải dữ liệu: ' + res.Message, 'error');
                    } else {
                        alert('Không thể tải dữ liệu: ' + res.Message);
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error("Chi tiết lỗi API: ", xhr.responseText);
                if (typeof NioApp !== 'undefined') {
                    NioApp.Toast('Lỗi kết nối API: ' + error, 'error');
                } else {
                    alert('Lỗi kết nối API: ' + error);
                }
            }
        });
    } oApp.Toa    st("Lỗi khi tải dữ liệu:     " + error, 'danger', { position: 'top-right' });
            }
            });
    }

    // Render Cha    rt.js
    function renderCharts(opened, unopened, bounced, clicked) {
        // Chart 1: Do    ughnut Chart
        var ctxDoughnut = document.getElementById('open    RateDoughnutChart').getCo    ntext('2d');
        new Chart(ctxDoughnut, {
            type: 'doughnut',
            data: {
                labels: ['Đã     Mở', 'Chưa Mở'],
                datasets: [{
                    data: [opened, unopened],
                    backgroundColor: ['#1ee    0ac', '#f4bd0e'],
                    borderWidth: 2,
                    borderColor: '#ffffff'
                }]
            },
            options: {
                legend: { position: 'bottom' },
                responsive: true,
                maintainAspectRatio: false
            }
        });

        // Chart 2: Bar Chart
        var ctxBar = document.get    ElementById('statusBarCha    rt').getContext('2d');
        new Chart(ctxBar, {
            type: 'bar',
            data: {
                labels: ['Đã Mở', 'Chưa Mở', 'Cl    icked', 'Bounced'],
                datasets: [{
                    label: 'Số Lượng Mail',
                    data: [opened, unopened, cli    cked, bounced],
                    backgroundColor: ['#1ee    0ac', '#f4bd0e', '#09c2de', '#e85347']
                }]
            },
            options: {
                legend: { display: false },
                responsive: true,
                maintainAspectRatio: false,
                scales: {
                    yAxes: [{ ticks: { beginAtZero: tr    ue } }]
                }
            }
        });
    }

    // Render Table Data (Ope    nedTime is null)
    func    tion renderUnopenedTable(list) {
        $('#unopenedCount').text(list.length);
        var html = '';

        if (list.length === 0) {
            html = '<div c    lass="nk-tb-item"><div     class="nk-tb-col text-center" colspan="5">Không có e    mail nào chưa mở.</div></div>';
        } else {
            $.each(list, function (idx, item) {
                var sentTimeFormatted = item.SentTime ? new Date(it    em.SentTime).toLocaleString('vi-VN') : '-';

                html += '<div class="nk-tb-item">';
                html += '  <div class="nk-tb-col"><span class="tb-lead">#' + item.SendLogId + '</span></div>';
                html += '  <div c    lass="nk-tb-col"><span class="tb-sub font-weight-bold text-dark">' + item.Email + '</span></div>';
                html += '  <div class="n    k-tb-col tb-col-md"><span class="badge badge-dim badge-warning">' + item.Status + '</span></div>';
                html += '  <div class="nk-tb-col tb-col-lg"><span class="tb-sub">' + sentTimeFormatted + '</span></div>';
                html += '      <div class="nk-tb-col tb-col-md    "><span class    ="badge badge    -dot badge-danger">Chưa mở</span></div>';
                html += '</div>';
            });
        }

        $('#tableUnopenedBody').h    tml(html);
    }

    // Fast Client-side Export Excel function
    fun    ction exportUnopenedToExcel() {
        if (!globalUnopenedList || globalUnopenedList.length === 0) {
            NioApp.Toast('Không có dữ liệu mail chưa mở để xuất Exc    el!', 'warning');
            return;
        }

        // Map da    ta to Clean JSON structure     for Excel
        var excelData = globalUnopen    edList.map(function (item) {
            return {
                "Send Log ID": item.SendLogId,
                "Campaign Send ID": item.Campa    ignSendId,
                "List Mail ID": item.ListMailId,
                "Email": item.Email,
                "Trạng Thái": item.Status,
                "Thời Gian Gửi": item.SentTime ? new Date(item.SentTime).toLocaleString('vi-VN') : '',
                "    Thời Gian Tạo": item.CreatedDate ? new Date(item.CreatedDate).toLocaleString('vi-VN') : ''
            };
        });

        // Create Worksheet
        var worksheet = XLSX.utils.json_to_sheet(excelData);
        var workbook = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workboo    k, worksheet, "Unopened_Emails");

        // Auto     fit column widths
        worksheet['!cols'] = [
            { wch: 12 }, { wch: 18 }, { wch: 15 },
            { wch: 30 }, { wch: 12 }, { wch: 22 }, { wch: 22 }
        ];

        // Save     File
        var filename = "Danh_Sach_    Mail_Ch    ua_Mo_" + new Date().toISOString().slice(0, 10) + ".xlsx";
        XLSX.writeFile(workbook, filename);
    }
    //Export Excel
    // Biến lưu danh sách     Mail Chưa Mở sau khi fetch API xong
    var globalUnopenedList = [];
    var globalCampaignTitle = "Campaign";

    $(document).ready(function () {
        loadDashboardData();

        // Bắt     sự kiện click nút Export Excel Cl    ient-sid    e
        $('#btnExportExcelClient').on('click', function () {
            exportUnopenedToExcel();
        });
    });

    function loadDashboardData() {
        $.ajax({
            url: servic    eRoot,
            type: 'GET',
            dataType: 'jso    n',
            success: function (res) {
                if (res && res.Success && res.Data) {
                    var summary = res.Data.Summary;
                    var details = r    es.Data.Details || [];

            globalCampaignTitl    e = summary.Title || "Campaign";

            // Lọc mail chưa mở (Ope    nedTime === null)
            globalUnopenedL    ist = details.filter(func    tion(item) {
                return     item.OpenedTime === null;
            });

            // Render     UI & Cha    rts...
            renderUnopenedTable(globalUnopened    List);
        }
            }
            });
    }

    // HÀM XUẤT EXCEL CỰC NHANH BẰNG JQUERY + SHEETJS
    f    unction exportUnopenedToExcel() {
        if (!globalUnopenedList |    | globalUnopened    List.length === 0) {
            alert("Không có dữ liệu mail chưa mở để xuất E    xcel!");
            return;
        }

        // 1. Chuyển đổi dữ liệu JSO    N thành Array cho đẹp Cột     Excel
        var excelData = glo    balUnopenedList.map(function (item, index) {
            return {
                "STT": index + 1,
                "Send Log ID": item.Se    ndLogId,
                "List Mail ID": it    em.ListMailId || 0,
                "Email": item.Email,
                "Trạng Thái": item.Status,
                "Thời Gian Gửi": item.SentTime ? new Date(item.SentTime).toLocaleString('vi-VN') : '',
                "Thời Gian T    ạo": item.CreatedDate ? new Date(ite    m.CreatedDate).toLocaleString('vi-VN') : ''
            };
        });

        // 2. Tạo Worksheet từ J    SON
        var worksheet = XLSX.ut    ils.json_to_sheet(excelData);

        // 3. Set độ rộng các cột cho đẹp    
        worksheet['!cols'] = [
            { wch: 6 },  // STT
            { wch: 12 }, // Send Log ID
            { wch: 12 }, // List Mail ID
            { wch: 30 }, // Email
            { wch: 15 }, // Status
            { wch: 22 },     // Sent Time
            { wch: 22 }  // Creat    ed Date
        ];

        // 4. Tạo Workbook và ghi ra file
        var work    book = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(workbook, worksheet, "Emails_Chua_Mo");

        // 5. Downl    oad file .xlsx
        var fileName = "U    nop    ened_Emails_" + new Date().toISOString().slice(0, 10) + ".xlsx";
        XLSX.writeFile(workbook, fileName);
    }
</script>
