<%@ Control Language="VB" AutoEventWireup="false" CodeFile="U_NhuanBut.ascx.vb" Inherits="DesktopModules.TinTuc.Control.ThongkeNhuanButUser" %>
<style type="text/css">
    .nk-top-products .item {
        border-bottom: 1px solid #dbdfea;
    }
    .table.tknhuabut tr td {
        padding: 2px 5px;
        font-size: 12px;
    }
</style>
<script type="text/javascript" src="/static/_Admin/build/js/autoNumeric.js"></script>
<script type="text/javascript">
    jQuery(function ($) {
        $('.auto').autoNumeric('init', { dGroup: '3', aSep: '.', aDec: ',', aSign: '₫ ', vMin: '0', vMax: '100000000', wEmpty: 'zero', wEmpty: 'sign' });
    });
</script>
<div class="nk-block">
    <div class="row g-gs">
    <div class="col-xxl-12 col-lg-12">
        <div class="card card-full overflow-hidden">
            <div class="nk-ecwg nk-ecwg4 h-100">
                <div class="card-inner flex-grow-1">
                    <div class="card-title-group mb-4">
                        <div class="card-title">
                            <h6 class="title">Lượt truy cập</h6>
                        </div>
                        <div class="card-tools">
                        </div>
                    </div>
                    <div class="data-group">
                        <iframe width="100%" height="300" src="https://traffic.nvcms.net/index.php?module=Widgetize&action=iframe&forceView=1&viewDataTable=graphEvolution&disableLink=0&widget=1&moduleToWidgetize=VisitsSummary&actionToWidgetize=getEvolutionGraph&idSite=2&period=day&date=today&disableLink=1&widget=1&language=vi&token_auth=ee2303cb6cde55d25184f17c8079709a" scrolling="no" frameborder="0" marginheight="0" marginwidth="0"></iframe>
                    </div>
                </div>
                <!-- .card-inner -->
                <%--<div class="card-inner card-inner-md bg-light">
                        <div class="card-note">
                            <em class="icon ni ni-info-fill"></em>
                            <span>Traffic channels have beed generating the most traffics over past days.</span>
                        </div>
                    </div>--%>
            </div>
        </div>
        <!-- .card -->
    </div>
    <!-- ============================================-->
    <div class="col-xxl-5 col-md-6">
        <div class="card">
            <div class="card-inner">
                <div class="card-title-group mb-2">
                    <div class="card-title">
                        <h6 class="title">Bài vừa xuất bản lên trang</h6>
                    </div>
                </div>
                <ul class="nk-top-products">
                    <asp:Repeater ID="rptvuaxuatban" runat="server">
                        <ItemTemplate>
                            <li class="item">
                                <div class="info">
                                    <div class="title">
                                        <a target="_blank" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                            <small><%#Eval("Title") %></small></a>
                                    </div>
                                </div>
                                <div class="total">
                                    <span class="badge badge-danger"><%# BL.GetButDanh(PortalId, Eval("UserId"))%></span>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>


                </ul>
            </div>
            <!-- .card-inner -->
        </div>
        <!-- .card -->
    </div>
    <!-- .col -->
    <div class="col-xxl-3 col-lg-6">
        <div class="card">
            <div class="card-inner">
                <div class="card-title-group mb-2">
                    <div class="card-title">
                        <h6 class="title">Số người đang xem</h6>
                    </div>
                </div>
                <div class="data-group">
                    <div id="widgetIframe">
                        <iframe width="100%" height="220" src="https://traffic.nvcms.net/index.php?module=Widgetize&action=iframe&disableLink=0&widget=1&moduleToWidgetize=Live&actionToWidgetize=getSimpleLastVisitCount&idSite=2&period=day&date=yesterday&disableLink=1&widget=1&language=vi&token_auth=ee2303cb6cde55d25184f17c8079709a" scrolling="no" frameborder="0" marginheight="0" marginwidth="0"></iframe>
                    </div>
                </div>
            </div>

            <!-- .card-inner -->
        </div>
        <div class="card">
            <div class="card-inner">
                <div class="card-title-group mb-2">
                    <div class="card-title">
                        <h6 class="title">Google Trend Keyword</h6>
                    </div>
                </div>
                <script type="text/javascript" src="https://ssl.gstatic.com/trends_nrtr/2578_RC01/embed_loader.js"></script>
                <script type="text/javascript"> trends.embed.renderWidget("dailytrends", "", { "geo": "VN", "guestPath": "https://trends.google.com:443/trends/embed/" }); </script>
            </div>
        </div>
        <!-- .card -->
    </div>
    <!-- .col -->
    <div class="col-xxl-4 col-lg-6">
        <div class="card">
            <div class="card-inner">
                <div class="card-title-group mb-2">
                    <div class="card-title">
                        <h6 class="title">Bài đang được xem</h6>
                    </div>
                </div>
                <div class="data-group">
                    <iframe width="100%" height="690" src="https://traffic.nvcms.net/index.php?module=Widgetize&action=iframe&forceView=1&viewDataTable=VisitorLog&small=1&disableLink=0&widget=1&moduleToWidgetize=Live&actionToWidgetize=getLastVisitsDetails&idSite=2&period=range&date=last7&disableLink=1&widget=1&language=vi&token_auth=ee2303cb6cde55d25184f17c8079709a" scrolling="yes" frameborder="0" marginheight="0" marginwidth="0"></iframe>
                </div>
            </div>

            <!-- .card-inner -->
        </div>
    </div>
    <div class="col-xxl-4 col-md-6">
        <div class="card is-dark h-100">
            <div class="nk-ecwg nk-ecwg1">
                <div class="card-inner">
                    <div class="card-title-group">
                        <div class="card-title">
                            <h6 class="title">Tổng tiền nhuận bút <%=Datefrom %>-<%=DateTo() %></h6>
                        </div>
                    </div>
                    <div class="data">
                        <div class="amount auto">
                            <asp:Literal ID="ltrnhuanbutorg" runat="server"></asp:Literal>
                            vnđ
                        </div>
                        <div class="info" style="display: none">
                            <strong class="auto">
                                <asp:Literal ID="ltrnhuanbutthucnhanorg" runat="server"></asp:Literal></strong> vnđ (thực nhận /300 view)
                        </div>
                    </div>
                    <div class="data">
                        <h6 class="sub-title">Tống lượt view <%=Datefrom %>-<%=DateTo() %></h6>
                        <div class="data-group">
                            <div class="amount">
                                <asp:Literal ID="ltrview" runat="server"></asp:Literal>
                            </div>
                            <div class="info text-right">
                                <span class="change up text-danger "><em class="icon ni ni-arrow-long-up"></em>
                                    <asp:Literal ID="ltrviewtyle" runat="server"></asp:Literal>%</span><br>
                                <span>đạt chuẩn > 300/1 bài</span>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- .card-inner -->
                <div class="nk-ecwg1-ck">
                    <canvas class="ecommerce-line-chart-s1" id="totalSales"></canvas>
                </div>
            </div>
            <!-- .nk-ecwg -->
        </div>
    </div>

    <!-- .col -->
    <div class="col-xxl-4 col-md-6">
        <div class="card h-100">
            <div class="nk-ecwg nk-ecwg2">
                <div class="card-inner">
                    <div class="card-title-group mt-n1">
                        <div class="card-title">
                            <h6 class="title">Thống kê lượt view theo ngày</h6>
                        </div>
                    </div>
                    <div class="data">
                        <div class="data-group">
                            <div class="amount ">
                                Tổng: <span class="auto">
                                    <asp:Literal ID="ltrview2" runat="server"></asp:Literal></span>
                            </div>
                            <div class="info text-right">
                            </div>
                        </div>
                    </div>
                    <h6 class="sub-title">Orders over time</h6>
                </div>
                <!-- .card-inner -->
                <div class="nk-ecwg2-ck">
                    <canvas class="ecommerce-line-chart-s1" id="totalOrders"></canvas>
                </div>
            </div>
            <!-- .nk-ecwg -->
        </div>
        <!-- .card -->
    </div>
    <!-- .col -->
    <div class="col-xxl-4">
        <div class="row g-gs">
            <div class="col-xxl-12 col-md-6">
                <div class="card">
                    <div class="nk-ecwg nk-ecwg3">
                        <div class="card-inner pb-0">
                            <div class="card-title-group">
                                <div class="card-title">
                                    <h6 class="title">Orders</h6>
                                </div>
                            </div>
                            <div class="data">
                                <div class="data-group">
                                    <div class="amount">329</div>
                                    <div class="info text-right">
                                        <span class="change up text-danger"><em class="icon ni ni-arrow-long-up"></em>4.63%</span><br>
                                        <span>vs. last week</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- .card-inner -->
                        <div class="nk-ecwg3-ck">
                            <canvas class="ecommerce-bar-chart-s1" id="averargeOrder"></canvas>
                        </div>
                    </div>
                    <!-- .nk-ecwg -->
                </div>
                <!-- .card -->
            </div>
            <!-- .col -->
            <div class="col-xxl-12 col-md-6">
                <div class="card">
                    <div class="nk-ecwg nk-ecwg3">
                        <div class="card-inner pb-0">
                            <div class="card-title-group">
                                <div class="card-title">
                                    <h6 class="title">Customers</h6>
                                </div>
                            </div>
                            <div class="data">
                                <div class="data-group">
                                    <div class="amount">194</div>
                                    <div class="info text-right">
                                        <span class="change up text-danger"><em class="icon ni ni-arrow-long-up"></em>4.63%</span><br>
                                        <span>vs. last week</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- .card-inner -->
                        <div class="nk-ecwg3-ck">
                            <canvas class="ecommerce-line-chart-s1" id="totalCustomers"></canvas>
                        </div>
                    </div>
                    <!-- .nk-ecwg -->
                </div>
                <!-- .card -->
            </div>
            <!-- .col -->
        </div>
        <!-- .row -->
    </div>
    <!-- .col -->
    <div class="col-xxl-8">
        <div class="card card-full">
            <div class="card-inner">
                <div class="card-title-group">
                    <div class="card-title">
                        <h6 class="title">Bài vừa xuất bản của bạn</h6>
                    </div>
                </div>
            </div>
            <div class="nk-tb-list mt-n2">
                <div class="nk-tb-item nk-tb-head">
                    <div class="nk-tb-col"><span>#</span></div>
                    <div class="nk-tb-col tb-col-sm"><span>Thông tin</span></div>
                    <div class="nk-tb-col tb-col-md"><span>Danh mục</span></div>
                    <div class="nk-tb-col"><span>Nhuận bút</span></div>
                    <div class="nk-tb-col"><span class="d-none d-sm-inline">View</span></div>
                </div>
                <asp:Repeater ID="rptbaivuaxuatban" runat="server">
                    <ItemTemplate>
                        <div class="nk-tb-item">
                            <div class="nk-tb-col">
                                <span class="tb-lead"><a target="_blank" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">#<%#Eval("NewId") %></a></span>
                            </div>
                            <div class="nk-tb-col tb-col-sm">
                                <div class="user-card">
                                    <div class="user-name">
                                        <span class="badge badge-light"><%# BL.FormatLoaiTinBaiHTML(CInt(Eval("NewsKind")))%></span>
                                        <img src='<%# Eval("TypeUrl") %>' border="0" alt="" style="vertical-align: bottom;" />
                                        <span class="d-sm-inline tb-lead">
                                            <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" target="_blank">
                                                <%# Eval("Title")%></a></span>
                                        <span id="tinnng" runat="server" visible='<%#IIf(CBool(Eval("HotCat")), "True", "False") %>'><em class="icon ni ni-hot text-danger"></em></span>
                                        <span id="tinanh" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                        <span id="video" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                        <br />
                                        <small>
                                            <font style="color: Maroon;">Duyệt:</font>
                                            <asp:Label ID="lblApprovalInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetApprovalInfo(PortalId, CInt(Eval("ApprovalUser")), CDate(Eval("ApprovalDate"))) %>'></asp:Label>
                                            | 
                                            <font style="color: Maroon;">Xuất bản:</font>
                                            <asp:Label ID="lblPublishInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetPublishedInfo(PortalId, CInt(Eval("PublishedUser")), CDate(Eval("PublishedDate"))) %>'></asp:Label>
                                        </small>
                                    </div>
                                </div>
                            </div>
                            <div class="nk-tb-col tb-col-md">
                                <span class="tb-sub"><%# Eval("CategoryName")%></span>
                            </div>
                            <div class="nk-tb-col">
                                <span class="tb-sub tb-amount auto"><%# Ultis.GetTienNhuanBut(CInt(Eval("NewId")))%> </span><span></span>
                            </div>
                            <div class="nk-tb-col">
                                <span class="badge badge-danger"><%# Eval("ViewCount")%></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <!-- .card -->
    </div>
    <div class="col-xxl-4 col-md-6">
        <div class="card h-100">
            <div class="card-inner">
                <div class="card-title-group mb-2">
                    <div class="card-title">
                        <h6 class="title">Bài View thấp nhất của bạn</h6>
                    </div>
                </div>
                <ul class="nk-top-products">
                    <asp:Repeater ID="rptViewThap" runat="server">
                        <ItemTemplate>
                            <li class="item">
                                <div class="info">
                                    <div class="title">
                                        <a target="_blank" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                            <small><%#Eval("Title") %></small></a>
                                    </div>
                                </div>
                                <div class="total">
                                    <div class="amount"><span class="badge badge-danger"><%# Eval("ViewCount")%></span></div>
                                </div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>


                </ul>
            </div>
            <!-- .card-inner -->
        </div>
        <!-- .card -->
    </div>
    <!-- .col  Thống kê chung báo-->
    <div class="col-xxl-6">
        <div class="card card-full">
            <div class="card-inner">
                <div class="card-title-group">
                    <div class="card-title">
                        <h6 class="title">Bài view Thấp </h6>
                    </div>
                </div>
            </div>
            <div class="nk-tb-list mt-n2">
                <div class="nk-tb-item nk-tb-head">
                    <div class="nk-tb-col"><span>#</span></div>
                    <div class="nk-tb-col"><span>Thông tin</span></div>
                    <div class="nk-tb-col"><span class="d-none d-sm-inline">View</span></div>
                </div>
                <asp:Repeater ID="rptViewThapChung" runat="server">
                    <ItemTemplate>
                        <div class="nk-tb-item">
                            <div class="nk-tb-col">
                                <span class="tb-lead"><%# Container.ItemIndex + 1 %></span>
                            </div>
                            <div class="nk-tb-col">
                                <div class="user-card">
                                    <div class="user-name">
                                        <span class="badge badge-light"><%# BL.FormatLoaiTinBaiHTML(CInt(Eval("NewsKind")))%></span>
                                        <img src='<%# Eval("TypeUrl") %>' border="0" alt="" style="vertical-align: bottom;" />
                                        <span class="d-sm-inline tb-lead">
                                            <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" target="_blank">
                                                <%# Eval("Title")%></a></span>
                                        <span id="tinnng" runat="server" visible='<%#IIf(CBool(Eval("HotCat")), "True", "False") %>'><em class="icon ni ni-hot text-danger"></em></span>
                                        <span id="tinanh" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                        <span id="video" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <div class="nk-tb-col">
                                <span class="badge badge-danger"><%# Eval("ViewCount")%></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <!-- .card -->
    </div>
    <div class="col-xxl-6">
        <div class="card card-full">
            <div class="card-inner">
                <div class="card-title-group">
                    <div class="card-title">
                        <h6 class="title">Bài view Cao </h6>
                    </div>
                </div>
            </div>
            <div class="nk-tb-list mt-n2">
                <div class="nk-tb-item nk-tb-head">
                    <div class="nk-tb-col"><span>#</span></div>
                    <div class="nk-tb-col"><span>Thông tin</span></div>
                    <div class="nk-tb-col"><span class="d-none d-sm-inline">View</span></div>
                </div>
                <asp:Repeater ID="rptViewThapCao" runat="server">
                    <ItemTemplate>
                        <div class="nk-tb-item">
                            <div class="nk-tb-col">
                                <span class="tb-lead"><%# Container.ItemIndex + 1 %></span>
                            </div>
                            <div class="nk-tb-col">
                                <div class="user-card">
                                    <div class="user-name">
                                        <span class="badge badge-light"><%# BL.FormatLoaiTinBaiHTML(CInt(Eval("NewsKind")))%></span>
                                        <img src='<%# Eval("TypeUrl") %>' border="0" alt="" style="vertical-align: bottom;" />
                                        <span class="d-sm-inline tb-lead">
                                            <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" target="_blank">
                                                <%# Eval("Title")%></a></span>
                                        <span id="tinnng" runat="server" visible='<%#IIf(CBool(Eval("HotCat")), "True", "False") %>'><em class="icon ni ni-hot text-danger"></em></span>
                                        <span id="tinanh" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                        <span id="video" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <div class="nk-tb-col">
                                <span class="badge badge-danger"><%# Eval("ViewCount")%></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
        <!-- .card -->
    </div>
    <!-- ============================================-->

    <!-- .col -->

    <!-- .col -->

</div>
<!-- .row -->
</div>
<asp:Literal ID="ltrnhuanbutthoengay" runat="server"></asp:Literal>
<asp:Literal ID="ltrnviewthoengay" runat="server"></asp:Literal>
<%--<script type="text/html">
    var totalSales = {
            labels: ["01/01", "02/01", "03 Jan", "04 Jan", "05 Jan", "06 Jan", "07 Jan", "08 Jan", "09 Jan", "10 Jan", "11 Jan", "12 Jan", "13 Jan", "14 Jan", "15 Jan", "16 Jan", "17 Jan", "18 Jan", "19 Jan", "20 Jan", "21 Jan", "22 Jan", "23 Jan", "24 Jan", "25 Jan", "26 Jan", "27 Jan", "28 Jan", "29 Jan", "30 Jan"],
            dataUnit: ' vnđ',
            lineTension: .3,
            datasets: [{
                label: "Tổng tiền",
                color: "#0fac81",
                background: NioApp.hexRGB('#0fac81', .25),
                data: [130, 105, 125, 115, 110, 95, 131, 110, 115, 120, 111, 97, 113, 107, 122, 100, 85, 110, 130, 107, 90, 105, 123, 115, 100, 117, 125, 95, 137, 101]
            }]
        };
</script>--%>
<script type="text/javascript">
    !(function (NioApp, $) {
        "use strict";
        var totalCustomers = {
            labels: ["01 Jan", "02 Jan", "03 Jan", "04 Jan", "05 Jan", "06 Jan", "07 Jan", "08 Jan", "09 Jan", "10 Jan", "11 Jan", "12 Jan", "13 Jan", "14 Jan", "15 Jan", "16 Jan", "17 Jan", "18 Jan", "19 Jan", "20 Jan", "21 Jan", "22 Jan", "23 Jan", "24 Jan", "25 Jan", "26 Jan", "27 Jan", "28 Jan", "29 Jan", "30 Jan"],
            dataUnit: 'Customers',
            lineTension: .3,
            datasets: [{
                label: "Customers",
                color: "#008080",
                background: NioApp.hexRGB('#008080', .25),
                data: [92, 105, 125, 85, 110, 106, 131, 105, 110, 115, 135, 105, 120, 85, 122, 100, 125, 110, 120, 125, 85, 105, 123, 115, 90, 117, 125, 100, 95, 65]
            }]
        };

        function ecommerceLineS1(selector, set_data) {
            var $selector = (selector) ? $(selector) : $('.ecommerce-line-chart-s1');
            $selector.each(function () {
                var $self = $(this), _self_id = $self.attr('id'), _get_data = (typeof set_data === 'undefined') ? eval(_self_id) : set_data;
                var selectCanvas = document.getElementById(_self_id).getContext("2d");

                var chart_data = [];
                for (var i = 0; i < _get_data.datasets.length; i++) {
                    chart_data.push({
                        label: _get_data.datasets[i].label,
                        tension: _get_data.lineTension,
                        backgroundColor: _get_data.datasets[i].background,
                        borderWidth: 2,
                        borderColor: _get_data.datasets[i].color,
                        pointBorderColor: 'transparent',
                        pointBackgroundColor: 'transparent',
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: _get_data.datasets[i].color,
                        pointBorderWidth: 2,
                        pointHoverRadius: 4,
                        pointHoverBorderWidth: 2,
                        pointRadius: 4,
                        pointHitRadius: 4,
                        data: _get_data.datasets[i].data,
                    });
                }
                var chart = new Chart(selectCanvas, {
                    type: 'line',
                    data: {
                        labels: _get_data.labels,
                        datasets: chart_data,
                    },
                    options: {
                        legend: {
                            display: (_get_data.legend) ? _get_data.legend : false,
                            rtl: NioApp.State.isRTL,
                            labels: {
                                boxWidth: 12,
                                padding: 20,
                                fontColor: '#6783b8',
                            }
                        },
                        maintainAspectRatio: false,
                        tooltips: {
                            enabled: true,
                            rtl: NioApp.State.isRTL,
                            callbacks: {
                                title: function (tooltipItem, data) {
                                    return data['labels'][tooltipItem[0]['index']];
                                },
                                label: function (tooltipItem, data) {
                                    return data.datasets[tooltipItem.datasetIndex]['data'][tooltipItem['index']] + ' ' + _get_data.dataUnit;
                                }
                            },
                            backgroundColor: '#1c2b46',
                            titleFontSize: 10,
                            titleFontColor: '#fff',
                            titleMarginBottom: 4,
                            bodyFontColor: '#fff',
                            bodyFontSize: 10,
                            bodySpacing: 4,
                            yPadding: 6,
                            xPadding: 6,
                            footerMarginTop: 0,
                            displayColors: false
                        },
                        scales: {
                            yAxes: [{
                                display: false,
                                ticks: {
                                    beginAtZero: true,
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    padding: 0
                                },
                                gridLines: {
                                    color: NioApp.hexRGB("#526484", .2),
                                    tickMarkLength: 0,
                                    zeroLineColor: NioApp.hexRGB("#526484", .2)
                                },
                            }],
                            xAxes: [{
                                display: false,
                                ticks: {
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    source: 'auto',
                                    padding: 0,
                                    reverse: NioApp.State.isRTL
                                },
                                gridLines: {
                                    color: "transparent",
                                    tickMarkLength: 0,
                                    zeroLineColor: NioApp.hexRGB("#526484", .2),
                                    offsetGridLines: true,
                                }
                            }]
                        }
                    }
                });
            })
        }
        // init chart
        NioApp.coms.docReady.push(function () { ecommerceLineS1(); });

        var storeVisitors = {
            labels: ["01 Jan", "02 Jan", "03 Jan", "04 Jan", "05 Jan", "06 Jan", "07 Jan", "08 Jan", "09 Jan", "10 Jan", "11 Jan", "12 Jan", "13 Jan", "14 Jan", "15 Jan", "16 Jan", "17 Jan", "18 Jan", "19 Jan", "20 Jan", "21 Jan", "22 Jan", "23 Jan", "24 Jan", "25 Jan", "26 Jan", "27 Jan", "28 Jan", "29 Jan", "30 Jan"],
            dataUnit: 'Customer',
            lineTension: .1,
            datasets: [{
                label: "Current Month",
                color: "#0fac81",
                dash: 0,
                background: "transparent",
                data: [4110, 4220, 4810, 5480, 4600, 5670, 6660, 4830, 5590, 5730, 4790, 4950, 5100, 5800, 5950, 5850, 5950, 4450, 4900, 8000, 7200, 7250, 7900, 8950, 6300, 7200, 7250, 7650, 6950, 4750]
            }]
        };

        function ecommerceLineS2(selector, set_data) {
            var $selector = (selector) ? $(selector) : $('.ecommerce-line-chart-s2');
            $selector.each(function () {
                var $self = $(this), _self_id = $self.attr('id'), _get_data = (typeof set_data === 'undefined') ? eval(_self_id) : set_data;
                var selectCanvas = document.getElementById(_self_id).getContext("2d");

                var chart_data = [];
                for (var i = 0; i < _get_data.datasets.length; i++) {
                    chart_data.push({
                        label: _get_data.datasets[i].label,
                        tension: _get_data.lineTension,
                        backgroundColor: _get_data.datasets[i].background,
                        borderWidth: 2,
                        borderDash: _get_data.datasets[i].dash,
                        borderColor: _get_data.datasets[i].color,
                        pointBorderColor: 'transparent',
                        pointBackgroundColor: 'transparent',
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: _get_data.datasets[i].color,
                        pointBorderWidth: 2,
                        pointHoverRadius: 4,
                        pointHoverBorderWidth: 2,
                        pointRadius: 4,
                        pointHitRadius: 4,
                        data: _get_data.datasets[i].data,
                    });
                }
                var chart = new Chart(selectCanvas, {
                    type: 'line',
                    data: {
                        labels: _get_data.labels,
                        datasets: chart_data,
                    },
                    options: {
                        legend: {
                            display: (_get_data.legend) ? _get_data.legend : false,
                            rtl: NioApp.State.isRTL,
                            labels: {
                                boxWidth: 12,
                                padding: 20,
                                fontColor: '#6783b8',
                            }
                        },
                        maintainAspectRatio: false,
                        tooltips: {
                            enabled: true,
                            rtl: NioApp.State.isRTL,
                            callbacks: {
                                title: function (tooltipItem, data) {
                                    return data['labels'][tooltipItem[0]['index']];
                                },
                                label: function (tooltipItem, data) {
                                    return data.datasets[tooltipItem.datasetIndex]['data'][tooltipItem['index']];
                                }
                            },
                            backgroundColor: '#1c2b46',
                            titleFontSize: 13,
                            titleFontColor: '#fff',
                            titleMarginBottom: 6,
                            bodyFontColor: '#fff',
                            bodyFontSize: 12,
                            bodySpacing: 4,
                            yPadding: 10,
                            xPadding: 10,
                            footerMarginTop: 0,
                            displayColors: false
                        },
                        scales: {
                            yAxes: [{
                                display: true,
                                position: NioApp.State.isRTL ? "right" : "left",
                                ticks: {
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    padding: 8,
                                    stepSize: 2400,
                                    display: false
                                },
                                gridLines: {
                                    color: NioApp.hexRGB("#526484", .2),
                                    tickMarkLength: 0,
                                    zeroLineColor: NioApp.hexRGB("#526484", .2),
                                },
                            }],
                            xAxes: [{
                                display: false,
                                ticks: {
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    source: 'auto',
                                    padding: 0,
                                    reverse: NioApp.State.isRTL
                                },
                                gridLines: {
                                    color: "transparent",
                                    tickMarkLength: 0,
                                    zeroLineColor: 'transparent',
                                    offsetGridLines: true,
                                }
                            }]
                        }
                    }
                });
            })
        }
        // init chart
        NioApp.coms.docReady.push(function () { ecommerceLineS2(); });

        var todayOrders = {
            labels: ["12AM - 02AM", "02AM - 04AM", "04AM - 06AM", "06AM - 08AM", "08AM - 10AM", "10AM - 12PM", "12PM - 02PM", "02PM - 04PM", "04PM - 06PM", "06PM - 08PM", "08PM - 10PM", "10PM - 12PM"],
            dataUnit: 'Orders',
            lineTension: .3,
            datasets: [{
                label: "Orders",
                color: "#0fac81",
                background: "transparent",
                data: [92, 105, 125, 85, 110, 106, 131, 105, 110, 131, 105, 110]
            }]
        };

        var todayRevenue = {
            labels: ["12AM - 02AM", "02AM - 04AM", "04AM - 06AM", "06AM - 08AM", "08AM - 10AM", "10AM - 12PM", "12PM - 02PM", "02PM - 04PM", "04PM - 06PM", "06PM - 08PM", "08PM - 10PM", "10PM - 12PM"],
            dataUnit: 'Orders',
            lineTension: .3,
            datasets: [{
                label: "Revenue",
                color: "#816bff",
                background: "transparent",
                data: [92, 105, 125, 85, 110, 106, 131, 105, 110, 131, 105, 110]
            }]
        };

        var todayCustomers = {
            labels: ["12AM - 02AM", "02AM - 04AM", "04AM - 06AM", "06AM - 08AM", "08AM - 10AM", "10AM - 12PM", "12PM - 02PM", "02PM - 04PM", "04PM - 06PM", "06PM - 08PM", "08PM - 10PM", "10PM - 12PM"],
            dataUnit: 'Customers',
            lineTension: .3,
            datasets: [{
                label: "Customers",
                color: "#ffa353",
                background: "transparent",
                data: [92, 105, 125, 85, 110, 106, 131, 105, 110, 131, 105, 110]
            }]
        };

        var todayVisitors = {
            labels: ["12AM - 02AM", "02AM - 04AM", "04AM - 06AM", "06AM - 08AM", "08AM - 10AM", "10AM - 12PM", "12PM - 02PM", "02PM - 04PM", "04PM - 06PM", "06PM - 08PM", "08PM - 10PM", "10PM - 12PM"],
            dataUnit: 'Users',
            lineTension: .3,
            datasets: [{
                label: "Visitors",
                color: "#ff63a5",
                background: "transparent",
                data: [92, 105, 125, 85, 110, 106, 131, 105, 110, 131, 105, 110]
            }]
        };

        function ecommerceLineS3(selector, set_data) {
            var $selector = (selector) ? $(selector) : $('.ecommerce-line-chart-s3');
            $selector.each(function () {
                var $self = $(this), _self_id = $self.attr('id'), _get_data = (typeof set_data === 'undefined') ? eval(_self_id) : set_data;
                var selectCanvas = document.getElementById(_self_id).getContext("2d");

                var chart_data = [];
                for (var i = 0; i < _get_data.datasets.length; i++) {
                    chart_data.push({
                        label: _get_data.datasets[i].label,
                        tension: _get_data.lineTension,
                        backgroundColor: _get_data.datasets[i].background,
                        borderWidth: 2,
                        borderColor: _get_data.datasets[i].color,
                        pointBorderColor: 'transparent',
                        pointBackgroundColor: 'transparent',
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: _get_data.datasets[i].color,
                        pointBorderWidth: 2,
                        pointHoverRadius: 4,
                        pointHoverBorderWidth: 2,
                        pointRadius: 4,
                        pointHitRadius: 4,
                        data: _get_data.datasets[i].data,
                    });
                }
                var chart = new Chart(selectCanvas, {
                    type: 'line',
                    data: {
                        labels: _get_data.labels,
                        datasets: chart_data,
                    },
                    options: {
                        legend: {
                            display: (_get_data.legend) ? _get_data.legend : false,
                            rtl: NioApp.State.isRTL,
                            labels: {
                                boxWidth: 12,
                                padding: 20,
                                fontColor: '#6783b8',
                            }
                        },
                        maintainAspectRatio: false,
                        tooltips: {
                            enabled: true,
                            rtl: NioApp.State.isRTL,
                            callbacks: {
                                title: function (tooltipItem, data) {
                                    return false;
                                },
                                label: function (tooltipItem, data) {
                                    return data.datasets[tooltipItem.datasetIndex]['data'][tooltipItem['index']] + ' ' + _get_data.dataUnit;
                                }
                            },
                            backgroundColor: '#1c2b46',
                            titleFontSize: 8,
                            titleFontColor: '#fff',
                            titleMarginBottom: 4,
                            bodyFontColor: '#fff',
                            bodyFontSize: 8,
                            bodySpacing: 4,
                            yPadding: 6,
                            xPadding: 6,
                            footerMarginTop: 0,
                            displayColors: false
                        },
                        scales: {
                            yAxes: [{
                                display: false,
                                ticks: {
                                    beginAtZero: false,
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    padding: 0
                                },
                                gridLines: {
                                    color: NioApp.hexRGB("#526484", .2),
                                    tickMarkLength: 0,
                                    zeroLineColor: NioApp.hexRGB("#526484", .2)
                                },
                            }],
                            xAxes: [{
                                display: false,
                                ticks: {
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    source: 'auto',
                                    padding: 0,
                                    reverse: NioApp.State.isRTL
                                },
                                gridLines: {
                                    color: "transparent",
                                    tickMarkLength: 0,
                                    zeroLineColor: NioApp.hexRGB("#526484", .2),
                                    offsetGridLines: true,
                                }
                            }]
                        }
                    }
                });
            })
        }
        // init chart
        NioApp.coms.docReady.push(function () { ecommerceLineS3(); });

        var salesStatistics = {
            labels: ["01 Jan", "02 Jan", "03 Jan", "04 Jan", "05 Jan", "06 Jan", "07 Jan", "08 Jan", "09 Jan", "10 Jan", "11 Jan", "12 Jan", "13 Jan", "14 Jan", "15 Jan", "16 Jan", "17 Jan", "18 Jan", "19 Jan", "20 Jan", "21 Jan", "22 Jan", "23 Jan", "24 Jan", "25 Jan", "26 Jan", "27 Jan", "28 Jan", "29 Jan", "30 Jan"],
            dataUnit: 'Customer',
            lineTension: .4,
            datasets: [{
                label: "Total orders",
                color: "#0fac81",
                dash: 0,
                background: NioApp.hexRGB('#0fac81', .15),
                data: [3710, 4820, 4810, 5480, 5300, 5670, 6660, 4830, 5590, 5730, 4790, 4950, 5100, 5800, 5950, 5850, 5950, 4450, 4900, 8000, 7200, 7250, 7900, 8950, 6300, 7200, 7250, 7650, 6950, 4750]
            }, {
                label: "Canceled orders",
                color: "#eb6459",
                dash: [5],
                background: "transparent",
                data: [110, 220, 810, 480, 600, 670, 660, 830, 590, 730, 790, 950, 100, 800, 950, 850, 950, 450, 900, 0, 200, 250, 900, 950, 300, 200, 250, 650, 950, 750]
            }]
        };

        function ecommerceLineS4(selector, set_data) {
            var $selector = (selector) ? $(selector) : $('.ecommerce-line-chart-s4');
            $selector.each(function () {
                var $self = $(this), _self_id = $self.attr('id'), _get_data = (typeof set_data === 'undefined') ? eval(_self_id) : set_data;
                var selectCanvas = document.getElementById(_self_id).getContext("2d");

                var chart_data = [];
                for (var i = 0; i < _get_data.datasets.length; i++) {
                    chart_data.push({
                        label: _get_data.datasets[i].label,
                        tension: _get_data.lineTension,
                        backgroundColor: _get_data.datasets[i].background,
                        borderWidth: 2,
                        borderDash: _get_data.datasets[i].dash,
                        borderColor: _get_data.datasets[i].color,
                        pointBorderColor: 'transparent',
                        pointBackgroundColor: 'transparent',
                        pointHoverBackgroundColor: "#fff",
                        pointHoverBorderColor: _get_data.datasets[i].color,
                        pointBorderWidth: 2,
                        pointHoverRadius: 4,
                        pointHoverBorderWidth: 2,
                        pointRadius: 4,
                        pointHitRadius: 4,
                        data: _get_data.datasets[i].data,
                    });
                }
                var chart = new Chart(selectCanvas, {
                    type: 'line',
                    data: {
                        labels: _get_data.labels,
                        datasets: chart_data,
                    },
                    options: {
                        legend: {
                            display: (_get_data.legend) ? _get_data.legend : false,
                            rtl: NioApp.State.isRTL,
                            labels: {
                                boxWidth: 12,
                                padding: 20,
                                fontColor: '#6783b8',
                            }
                        },
                        maintainAspectRatio: false,
                        tooltips: {
                            enabled: true,
                            rtl: NioApp.State.isRTL,
                            callbacks: {
                                title: function (tooltipItem, data) {
                                    return data['labels'][tooltipItem[0]['index']];
                                },
                                label: function (tooltipItem, data) {
                                    return data.datasets[tooltipItem.datasetIndex]['data'][tooltipItem['index']];
                                }
                            },
                            backgroundColor: '#1c2b46',
                            titleFontSize: 13,
                            titleFontColor: '#fff',
                            titleMarginBottom: 6,
                            bodyFontColor: '#fff',
                            bodyFontSize: 12,
                            bodySpacing: 4,
                            yPadding: 10,
                            xPadding: 10,
                            footerMarginTop: 0,
                            displayColors: false
                        },
                        scales: {
                            yAxes: [{
                                display: true,
                                stacked: (_get_data.stacked) ? _get_data.stacked : false,
                                position: NioApp.State.isRTL ? "right" : "left",
                                ticks: {
                                    beginAtZero: true,
                                    fontSize: 11,
                                    fontColor: '#9eaecf',
                                    padding: 10,
                                    callback: function (value, index, values) {
                                        return '$ ' + value;
                                    },
                                    min: 0,
                                    stepSize: 3000
                                },
                                gridLines: {
                                    color: NioApp.hexRGB("#526484", .2),
                                    tickMarkLength: 0,
                                    zeroLineColor: NioApp.hexRGB("#526484", .2)
                                },

                            }],
                            xAxes: [{
                                display: false,
                                stacked: (_get_data.stacked) ? _get_data.stacked : false,
                                ticks: {
                                    fontSize: 9,
                                    fontColor: '#9eaecf',
                                    source: 'auto',
                                    padding: 10,
                                    reverse: NioApp.State.isRTL
                                },
                                gridLines: {
                                    color: "transparent",
                                    tickMarkLength: 0,
                                    zeroLineColor: 'transparent',
                                },
                            }]
                        }
                    }
                });
            })
        }
        // init chart
        NioApp.coms.docReady.push(function () { ecommerceLineS4(); });


        var averargeOrder = {
            labels: ["01 Jan", "02 Jan", "03 Jan", "04 Jan", "05 Jan", "06 Jan", "07 Jan", "08 Jan", "09 Jan", "10 Jan", "11 Jan", "12 Jan", "13 Jan", "14 Jan", "15 Jan", "16 Jan", "17 Jan", "18 Jan", "19 Jan", "20 Jan", "21 Jan", "22 Jan", "23 Jan", "24 Jan", "25 Jan", "26 Jan", "27 Jan", "28 Jan", "29 Jan", "30 Jan"],
            dataUnit: 'Customer',
            lineTension: .1,
            datasets: [{
                label: "Active Users",
                color: "#0fac81",
                background: "#0fac81",
                data: [1110, 1220, 1310, 980, 900, 770, 1060, 830, 690, 730, 790, 950, 1100, 800, 1250, 850, 950, 450, 900, 1000, 1200, 1250, 900, 950, 1300, 1200, 1250, 650, 950, 750]
            }]
        };

        function ecommerceBarS1(selector, set_data) {
            var $selector = (selector) ? $(selector) : $('.ecommerce-bar-chart-s1');
            $selector.each(function () {
                var $self = $(this), _self_id = $self.attr('id'), _get_data = (typeof set_data === 'undefined') ? eval(_self_id) : set_data;
                var selectCanvas = document.getElementById(_self_id).getContext("2d");

                var chart_data = [];
                for (var i = 0; i < _get_data.datasets.length; i++) {
                    chart_data.push({
                        label: _get_data.datasets[i].label,
                        tension: _get_data.lineTension,
                        backgroundColor: _get_data.datasets[i].background,
                        borderWidth: 2,
                        borderColor: _get_data.datasets[i].color,
                        data: _get_data.datasets[i].data,
                        barPercentage: .7,
                        categoryPercentage: .7
                    });
                }
                var chart = new Chart(selectCanvas, {
                    type: 'bar',
                    data: {
                        labels: _get_data.labels,
                        datasets: chart_data,
                    },
                    options: {
                        legend: {
                            display: (_get_data.legend) ? _get_data.legend : false,
                            rtl: NioApp.State.isRTL,
                            labels: {
                                boxWidth: 12,
                                padding: 20,
                                fontColor: '#6783b8',
                            }
                        },
                        maintainAspectRatio: false,
                        tooltips: {
                            enabled: true,
                            rtl: NioApp.State.isRTL,
                            callbacks: {
                                title: function (tooltipItem, data) {
                                    return false; //data['labels'][tooltipItem[0]['index']];
                                },
                                label: function (tooltipItem, data) {
                                    return data.datasets[tooltipItem.datasetIndex]['data'][tooltipItem['index']];
                                }
                            },
                            backgroundColor: '#1c2b46',
                            titleFontSize: 9,
                            titleFontColor: '#fff',
                            titleMarginBottom: 6,
                            bodyFontColor: '#fff',
                            bodyFontSize: 9,
                            bodySpacing: 4,
                            yPadding: 6,
                            xPadding: 6,
                            footerMarginTop: 0,
                            displayColors: false
                        },
                        scales: {
                            yAxes: [{
                                display: true,
                                position: NioApp.State.isRTL ? "right" : "left",
                                ticks: {
                                    beginAtZero: false,
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    padding: 0,
                                    display: false,
                                    stepSize: 100
                                },
                                gridLines: {
                                    color: NioApp.hexRGB("#526484", .2),
                                    tickMarkLength: 0,
                                    zeroLineColor: NioApp.hexRGB("#526484", .2),
                                },
                            }],
                            xAxes: [{
                                display: false,
                                ticks: {
                                    fontSize: 12,
                                    fontColor: '#9eaecf',
                                    source: 'auto',
                                    padding: 0,
                                    reverse: NioApp.State.isRTL
                                },
                                gridLines: {
                                    color: "transparent",
                                    tickMarkLength: 0,
                                    zeroLineColor: 'transparent',
                                    offsetGridLines: true,
                                }
                            }]
                        }
                    }
                });
            })
        }
        // init chart
        NioApp.coms.docReady.push(function () { ecommerceBarS1(); });


        var trafficSources = {
            labels: ["Organic Search", "Social Media", "Referrals", "Others"],
            dataUnit: 'Customer',
            legend: false,
            datasets: [{
                borderColor: "#fff",
                background: ["#0fac81", "#e85347", "#ffa9ce", "#f9db7b"],
                data: [4305, 859, 482, 138]
            }]
        };
        var orderStatistics = {
            labels: ["Completed", "Canclled", "Processing"],
            dataUnit: 'Customer',
            legend: false,
            datasets: [{
                borderColor: "#fff",
                background: ["#0fac81", "#e85347", "#816bff"],
                data: [4305, 259, 682]
            }]
        };

        function ecommerceDoughnutS1(selector, set_data) {
            var $selector = (selector) ? $(selector) : $('.ecommerce-doughnut-s1');
            $selector.each(function () {
                var $self = $(this), _self_id = $self.attr('id'), _get_data = (typeof set_data === 'undefined') ? eval(_self_id) : set_data;
                var selectCanvas = document.getElementById(_self_id).getContext("2d");

                var chart_data = [];
                for (var i = 0; i < _get_data.datasets.length; i++) {
                    chart_data.push({
                        backgroundColor: _get_data.datasets[i].background,
                        borderWidth: 2,
                        borderColor: _get_data.datasets[i].borderColor,
                        hoverBorderColor: _get_data.datasets[i].borderColor,
                        data: _get_data.datasets[i].data,
                    });
                }
                var chart = new Chart(selectCanvas, {
                    type: 'doughnut',
                    data: {
                        labels: _get_data.labels,
                        datasets: chart_data,
                    },
                    options: {
                        legend: {
                            display: (_get_data.legend) ? _get_data.legend : false,
                            rtl: NioApp.State.isRTL,
                            labels: {
                                boxWidth: 12,
                                padding: 20,
                                fontColor: '#6783b8',
                            }
                        },
                        rotation: -1.5,
                        cutoutPercentage: 70,
                        maintainAspectRatio: false,
                        tooltips: {
                            enabled: true,
                            rtl: NioApp.State.isRTL,
                            callbacks: {
                                title: function (tooltipItem, data) {
                                    return data['labels'][tooltipItem[0]['index']];
                                },
                                label: function (tooltipItem, data) {
                                    return data.datasets[tooltipItem.datasetIndex]['data'][tooltipItem['index']] + ' ' + _get_data.dataUnit;
                                }
                            },
                            backgroundColor: '#1c2b46',
                            titleFontSize: 13,
                            titleFontColor: '#fff',
                            titleMarginBottom: 6,
                            bodyFontColor: '#fff',
                            bodyFontSize: 12,
                            bodySpacing: 4,
                            yPadding: 10,
                            xPadding: 10,
                            footerMarginTop: 0,
                            displayColors: false
                        },
                    }
                });
            })
        }
        // init chart
        NioApp.coms.docReady.push(function () { ecommerceDoughnutS1(); });

    })(NioApp, jQuery);
</script>
