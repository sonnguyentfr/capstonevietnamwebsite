<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Static.ascx.vb" Inherits="NVCMS.Modules.Marketing.CamPaingMailStatic" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />

<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title">Email Campaign Analytics</h3>
            <div class="nk-block-des text-soft">
                <p><asp:Literal ID="ltCampaignTitle" runat="server"></asp:Literal></p>
            </div>
        </div>
    </div>
</div>

<asp:UpdatePanel runat="server" ID="upnlMain">
    <ContentTemplate>
        <!-- KPI Cards -->
        <div class="nk-block">
            <div class="row g-gs">
                <!-- Total Recipients -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Total Recipients</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalRecipients" runat="server" Text="0"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Total Sent -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Sent</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalSent" runat="server" Text="0"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Total Delivered -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Delivered</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalDelivered" runat="server" Text="0"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Total Opened -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Opened</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalOpened" runat="server" Text="0"></asp:Literal></span>
                                    <span class="sub-title"><asp:Literal ID="ltOpenRate" runat="server"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Total Clicked -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Clicked</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalClicked" runat="server" Text="0"></asp:Literal></span>
                                    <span class="sub-title"><asp:Literal ID="ltClickRate" runat="server"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Total Bounced -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Bounced</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalBounced" runat="server" Text="0"></asp:Literal></span>
                                    <span class="sub-title"><asp:Literal ID="ltBounceRate" runat="server"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Total Complaint -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Complaint</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalComplaint" runat="server" Text="0"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Total Unsubscribed -->
                <div class="col-xxl-3 col-md-6">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start mb-2">
                                <div class="card-title">
                                    <h6 class="title">Unsubscribed</h6>
                                </div>
                            </div>
                            <div class="align-end flex-sm-wrap g-4 flex-md-nowrap">
                                <div class="nk-sale-data">
                                    <span class="amount"><asp:Literal ID="ltTotalUnsubscribed" runat="server" Text="0"></asp:Literal></span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Engagement charts: calculated from Send/Open/Click timestamps, not only Status. -->
        <div class="nk-block">
            <div class="alert alert-info"><strong>Tracking note:</strong> Open and click figures are calculated from event times. A blank Delivered Time means delivery is unknown, not that the email failed.</div>
            <asp:HiddenField ID="hdnAnalyticsJson" runat="server" />
            <div class="row g-gs">
                <div class="col-lg-4"><div class="card card-bordered h-100"><div class="card-inner"><h5 class="card-title">Engagement funnel</h5><p class="text-soft small">Sent to opened to clicked</p><canvas id="emailFunnelChart" height="250"></canvas></div></div></div>
                <div class="col-lg-8"><div class="card card-bordered h-100"><div class="card-inner"><h5 class="card-title">Send and open activity by hour</h5><p class="text-soft small">Useful for selecting a better sending time.</p><canvas id="emailActivityChart" height="250"></canvas></div></div></div>
            </div>
            <div class="row g-gs mt-1">
                <div class="col-lg-4"><div class="card card-bordered h-100"><div class="card-inner"><h5 class="card-title">Technical status</h5><p class="text-soft small">Status returned by the sending provider.</p><canvas id="emailStatusChart" height="220"></canvas></div></div></div>
                <div class="col-lg-8"><div class="card card-bordered h-100"><div class="card-inner"><h5 class="card-title">Audience quality</h5><div class="row text-center mt-3"><div class="col-4 border-right"><span class="amount d-block h3"><asp:Literal ID="ltUniqueRecipients" runat="server" Text="0"></asp:Literal></span><span class="text-soft">Unique recipients</span></div><div class="col-4 border-right"><span class="amount d-block h3"><asp:Literal ID="ltUniqueOpened" runat="server" Text="0"></asp:Literal></span><span class="text-soft">Unique opened</span></div><div class="col-4"><span class="amount d-block h3"><asp:Literal ID="ltAverageOpenDelay" runat="server" Text="-"></asp:Literal></span><span class="text-soft">Average time to open</span></div></div></div></div></div>
            </div>
        </div>

        <!-- Email Preview Section -->
        <div class="nk-block">
            <div class="card card-bordered">
                <div class="card-inner">
                    <h5 class="card-title">Email Preview</h5>
                    <div class="form-group">
                        <label class="form-label">Subject</label>
                        <div class="form-control-wrap">
                            <asp:Literal ID="ltEmailSubject" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label">Email Content</label>
                        <div class="form-control-wrap" style="border: 1px solid #dbdfea; padding: 15px; background: #fff;">
                            <iframe id="emailPreviewFrame" style="width: 100%; min-height: 400px; border: none;"></iframe>
                            <asp:HiddenField ID="hdnEmailBody" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Status Distribution -->
        <div class="nk-block" id="divStatusDistribution" runat="server" Visible="false">
            <div class="card card-bordered">
                <div class="card-inner">
                    <h5 class="card-title">Status Distribution</h5>
                    <asp:Repeater ID="rptStatusDistribution" runat="server">
                        <HeaderTemplate>
                            <table class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>Status</th>
                                        <th>Count</th>
                                        <th>Percentage</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Status") %></td>
                                <td><%# Eval("Count") %></td>
                                <td><%# Eval("Percentage") %>%</td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                                </tbody>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>

        <!-- Recipients List -->
        <div class="nk-block">
            <div class="card card-bordered">
                <div class="card-inner">
                    <h5 class="card-title">Recipients</h5>

                    <!-- Filter Controls -->
                    <div class="row g-3 mb-3">
                        <div class="col-md-3">
                            <label class="form-label">Filter by Status</label>
                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlStatusFilter_SelectedIndexChanged">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                <asp:ListItem Text="Sent" Value="Sent"></asp:ListItem>
                                <asp:ListItem Text="Delivered" Value="Delivered"></asp:ListItem>
                                <asp:ListItem Text="Opened" Value="Opened"></asp:ListItem>
                                <asp:ListItem Text="Clicked" Value="Clicked"></asp:ListItem>
                                <asp:ListItem Text="Bounced" Value="Bounced"></asp:ListItem>
                                <asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
                                <asp:ListItem Text="Complaint" Value="Complaint"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label">Search by Email</label>
                            <div class="form-control-wrap">
                                <asp:TextBox ID="txtEmailSearch" runat="server" CssClass="form-control" placeholder="Enter email to search..."></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">&nbsp;</label>
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                        </div>
                    </div>

                    <!-- GridView -->
                    <asp:GridView ID="gvRecipients" runat="server" CssClass="table table-bordered table-hover" 
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="50" 
                        OnPageIndexChanging="gvRecipients_PageIndexChanging"
                        EmptyDataText="No recipients found.">
                        <Columns>
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="Status" HeaderText="Status" />
                            <asp:BoundField DataField="SentTime" HeaderText="Sent Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:BoundField DataField="DeliveredTime" HeaderText="Delivered Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:BoundField DataField="OpenedTime" HeaderText="Opened Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:BoundField DataField="ClickedTime" HeaderText="Clicked Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:BoundField DataField="ErrorMessage" HeaderText="Error" />
                        </Columns>
                        <PagerSettings Mode="NumericFirstLast" />
                    </asp:GridView>

                    <div class="mt-3">
                        <asp:Literal ID="ltPagingInfo" runat="server"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div class="loading" id="loadizng">Loading&#8230;</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<script type="text/javascript">
    // Lightweight canvas charts: no CDN or third-party script required.
    function getEmailAnalytics() {
        var source = document.getElementById('<%= hdnAnalyticsJson.ClientID %>');
        if (!source || !source.value) return null;
        try { return JSON.parse(source.value); } catch (e) { return null; }
    }

    // The page is initially rendered by WebForms, then refreshed from the
    // dashboard API so its figures always match the send-log statistics.
    function loadDashboardFromApi() {
        var query = new URLSearchParams(window.location.search);
        var sendId = query.get('sendid') || query.get('itemid');
        console.log('Loading dashboard for campaign send ID:', sendId);
        if (!sendId || !window.jQuery || !jQuery.ServicesFramework) return;

        var serviceFramework = jQuery.ServicesFramework(<%= ModuleId %>);
        jQuery.ajax({
            url: '/DesktopModules/NVCMS/API/Report/GetDashboard?campaignSendId=' + encodeURIComponent(sendId),
            type: 'GET',
            beforeSend: serviceFramework.setModuleHeaders
        }).done(function (response) {
            if (!response || !readDashboardValue(response, 'success') || !readDashboardValue(response, 'data')) return;
            renderDashboard(readDashboardValue(response, 'data'));
        }).fail(function (xhr) {
            // Keep the server-rendered values visible if the API is temporarily unavailable.
            if (window.console) console.warn('Unable to refresh campaign dashboard.', xhr.status);
        });
    }

    function renderDashboard(data) {
        var summary = readDashboardValue(data, 'summary') || {};
        setDashboardText('<%= ltTotalRecipients.ClientID %>', readDashboardValue(summary, 'totalRecipient'));
        setDashboardText('<%= ltTotalSent.ClientID %>', readDashboardValue(summary, 'sent'));
        setDashboardText('<%= ltTotalDelivered.ClientID %>', readDashboardValue(summary, 'delivered'));
        setDashboardText('<%= ltTotalOpened.ClientID %>', readDashboardValue(summary, 'opened'));
        setDashboardText('<%= ltTotalClicked.ClientID %>', readDashboardValue(summary, 'clicked'));
        setDashboardText('<%= ltTotalBounced.ClientID %>', readDashboardValue(summary, 'bounce'));
        setDashboardText('<%= ltTotalComplaint.ClientID %>', readDashboardValue(summary, 'complaint'));
        setDashboardText('<%= ltTotalUnsubscribed.ClientID %>', readDashboardValue(summary, 'unsubscribe'));
        setDashboardText('<%= ltOpenRate.ClientID %>', formatRate(readDashboardValue(summary, 'openRate'), 'Open Rate'));
        setDashboardText('<%= ltClickRate.ClientID %>', formatRate(readDashboardValue(summary, 'clickRate'), 'Click Rate'));
        setDashboardText('<%= ltBounceRate.ClientID %>', formatRate(readDashboardValue(summary, 'bounceRate'), 'Bounce Rate'));

        var analytics = {
            funnel: { labels: ['Sent', 'Opened', 'Clicked'], values: [readDashboardValue(summary, 'sent') || 0, readDashboardValue(summary, 'opened') || 0, readDashboardValue(summary, 'clicked') || 0] },
            activity: { sent: makeHourlySeries(readDashboardValue(data, 'sentTimeline')), opened: makeHourlySeries(readDashboardValue(data, 'openTimeline')) },
            status: {
                labels: (readDashboardValue(data, 'status') || []).map(function (x) { return readDashboardValue(x, 'status') || ''; }),
                values: (readDashboardValue(data, 'status') || []).map(function (x) { return readDashboardValue(x, 'total') || 0; })
            }
        };
        var source = document.getElementById('<%= hdnAnalyticsJson.ClientID %>');
        if (source) source.value = JSON.stringify(analytics);
        drawEmailCharts();
    }

    function setDashboardText(id, value) {
        var element = document.getElementById(id);
        if (element) element.textContent = value === undefined || value === null ? '0' : value;
    }

    function formatRate(value, label) {
        return (Number(value || 0)).toFixed(2) + '% ' + label;
    }

    function readDashboardValue(object, property) {
        if (!object) return undefined;
        return object[property] !== undefined ? object[property] : object[property.charAt(0).toUpperCase() + property.slice(1)];
    }

    function makeHourlySeries(points) {
        var series = [], i;
        for (i = 0; i < 24; i++) series.push(0);
        (points || []).forEach(function (point) {
            var hour = Number(readDashboardValue(point, 'gio'));
            if (hour >= 0 && hour < 24) series[hour] += Number(readDashboardValue(point, 'total') || 0);
        });
        return series;
    }

    function chartCanvas(id) {
        var canvas = document.getElementById(id);
        if (!canvas) return null;
        var width = canvas.clientWidth || 360, height = canvas.height || 220;
        canvas.width = width; canvas.height = height;
        return { x: canvas.getContext('2d'), w: width, h: height };
    }

    function drawEmailCharts() {
        var data = getEmailAnalytics();
        if (!data) return;
        var colors = ['#6576ff', '#1ee0ac', '#f4bd0e', '#e85347', '#816bff', '#09c2de'], graph = chartCanvas('emailFunnelChart');
        if (graph) {
            var max = Math.max.apply(null, data.funnel.values.concat([1])); graph.x.font = '12px Arial'; graph.x.textBaseline = 'middle';
            data.funnel.labels.forEach(function (label, i) { var y = 28 + i * 62, barW = Math.max(4, (graph.w - 125) * data.funnel.values[i] / max); graph.x.fillStyle = '#526484'; graph.x.fillText(label, 8, y + 15); graph.x.fillStyle = colors[i]; graph.x.fillRect(75, y, barW, 30); graph.x.fillStyle = '#364a63'; graph.x.fillText(data.funnel.values[i], 82 + barW, y + 15); });
        }
        graph = chartCanvas('emailActivityChart');
        if (graph) {
            var pad = { l: 32, r: 12, t: 22, b: 28 }, maxValue = Math.max.apply(null, data.activity.sent.concat(data.activity.opened).concat([1])), cw = graph.w - pad.l - pad.r, ch = graph.h - pad.t - pad.b, step = cw / 24;
            graph.x.strokeStyle = '#e5e9f2'; graph.x.fillStyle = '#8094ae'; graph.x.font = '11px Arial';
            for (var n = 0; n <= 4; n++) { var gy = pad.t + ch - ch * n / 4; graph.x.beginPath(); graph.x.moveTo(pad.l, gy); graph.x.lineTo(graph.w - pad.r, gy); graph.x.stroke(); graph.x.fillText(Math.round(maxValue * n / 4), 2, gy + 3); }
            [['sent', colors[0]], ['opened', colors[1]]].forEach(function (series) { graph.x.strokeStyle = series[1]; graph.x.lineWidth = 2; graph.x.beginPath(); data.activity[series[0]].forEach(function (value, i) { var px = pad.l + step * (i + .5), py = pad.t + ch - (value / maxValue * ch); i ? graph.x.lineTo(px, py) : graph.x.moveTo(px, py); }); graph.x.stroke(); });
            graph.x.fillStyle = colors[0]; graph.x.fillRect(graph.w - 120, 8, 10, 10); graph.x.fillStyle = '#526484'; graph.x.fillText('Sent', graph.w - 106, 14); graph.x.fillStyle = colors[1]; graph.x.fillRect(graph.w - 65, 8, 10, 10); graph.x.fillStyle = '#526484'; graph.x.fillText('Opened', graph.w - 51, 14);
            for (var h = 0; h < 24; h += 3) graph.x.fillText(('0' + h).slice(-2), pad.l + step * h, graph.h - 8);
        }
        graph = chartCanvas('emailStatusChart');
        if (graph) {
            var total = data.status.values.reduce(function(a, b) { return a + b; }, 0), start = -Math.PI / 2, cx = graph.w / 2, cy = graph.h / 2 - 5, radius = Math.min(graph.w, graph.h) / 3;
            data.status.values.forEach(function (value, i) { var end = start + (total ? value / total * Math.PI * 2 : 0); graph.x.beginPath(); graph.x.moveTo(cx, cy); graph.x.arc(cx, cy, radius, start, end); graph.x.closePath(); graph.x.fillStyle = colors[i % colors.length]; graph.x.fill(); start = end; });
            graph.x.font = '12px Arial'; data.status.labels.forEach(function (label, i) { var y = graph.h - 35 + i * 16; graph.x.fillStyle = colors[i % colors.length]; graph.x.fillRect(8, y, 10, 10); graph.x.fillStyle = '#526484'; graph.x.fillText(label + ' (' + data.status.values[i] + ')', 23, y + 9); });
        }
    }

    function loadEmailPreview() {
        var iframe = document.getElementById('emailPreviewFrame');
        var hdnBody = document.getElementById('<%= hdnEmailBody.ClientID %>');
        if (iframe && hdnBody) {
            var iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
            iframeDoc.open();
            iframeDoc.write(hdnBody.value);
            iframeDoc.close();
        }
    }

    // Load preview after page load
    if (window.addEventListener) {
        window.addEventListener('load', function () { loadEmailPreview(); drawEmailCharts(); loadDashboardFromApi(); }, false);
    } else if (window.attachEvent) {
        window.attachEvent('onload', loadEmailPreview);
    }

    // Reload after UpdatePanel postback
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function() {
        loadEmailPreview();
        drawEmailCharts();
        loadDashboardFromApi();
    });
</script>
