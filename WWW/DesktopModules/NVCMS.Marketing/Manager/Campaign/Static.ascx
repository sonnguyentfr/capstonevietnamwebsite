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
        window.addEventListener('load', loadEmailPreview, false);
    } else if (window.attachEvent) {
        window.attachEvent('onload', loadEmailPreview);
    }

    // Reload after UpdatePanel postback
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function() {
        loadEmailPreview();
    });
</script>
