<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.ZaloSendViewer" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />
<style type="text/css">

    .select2-container--default .select2-selection--single .select2-selection__rendered {
        color: #444;
        line-height: 28px;
        padding: 1px 5px 4px !important;
    }
</style>
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Title %></h3>
        </div>
    </div>
</div>

<asp:UpdatePanel runat="server" ID="upnlZaloSend">
    <ContentTemplate>
        <div class="nk-block">
            <div class="row g-gs">
                <div class="col-md-4 col-lg-4 col-xxl-4">
                    <div class="card card-preview">
                        <div class="card-inner">
                            <div class="form-group">
                                <label class="form-label"><b>CHỌN SỰ KIỆN: </b></label>
                                <asp:DropDownList ID="ddlEventCat" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--chọn sự kiện--" AutoPostBack="true" OnSelectedIndexChanged="ddlEventCat_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlEventCat" InitialValue="0" ErrorMessage="Vui lòng chọn Sự kiện." CssClass="text-danger small" Display="Dynamic" ValidationGroup="SendZalo" />
                            </div>
                            <div class="form-group">
                                <label class="form-label"><b>ĐIA ĐIỂM: </b></label>
                                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--chọn sự kiện--"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlEvent" InitialValue="0" ErrorMessage="Vui lòng chọn Địa điểm." CssClass="text-danger small" Display="Dynamic" ValidationGroup="SendZalo" />

                            </div>
                        </div>

                        <div class="card-inner">
                            <div class="form-group">
                                <label class="form-label"><b>CHỌN CHIẾN DỊCH ZALO:</b></label>
                                <asp:DropDownList ID="ddlCampaign" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--Chọn chiến dịch--" AutoPostBack="true" OnSelectedIndexChanged="ddlCampaign_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvCampaign" runat="server" ControlToValidate="ddlCampaign" InitialValue="0" ErrorMessage="Vui lòng chọn chiến dịch Zalo." CssClass="text-danger small" Display="Dynamic" ValidationGroup="SendZalo" />
                            </div>
                        </div>
                        <div class="card-inner ">
                            <div class="card-header border-bottom">
                                Danh sách có: <mark><asp:Literal ID="ltrTotalPhone" runat="server">0</asp:Literal></mark> số điện thoại
                            </div>
                            <div class="card-inner card-bordered">
                                <div class="form-group" data-simplebar style="max-height: 430px">
                                    <div class="nk-tb-list is-compact">
                                        <div class="nk-tb-item nk-tb-head">
                                            <div class="nk-tb-col"><span>#</span></div>
                                            <div class="nk-tb-col"><span>Số điện thoại</span></div>
                                            <div class="nk-tb-col text-right"><span>Send</span></div>
                                        </div>
                                        <asp:Repeater ID="rptListPhone" runat="server">
                                            <ItemTemplate>
                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span><%# DataBinder.Eval(Container, "ItemIndex", "") + 1%></span></span>
                                                    </div>
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span><%# Eval("Phone") %></span></span>
                                                    </div>
                                                    <div class="nk-tb-col text-right">
                                                        <span class="tb-sub tb-amount"><span><%# Eval("SendCount") %></span></span>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="card-inner">
                            <div class="form-group">
                                <label class="form-label"><b>PREVIEW URL:</b></label>
                                <asp:TextBox ID="txtPreviewUrl" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="lbtSendZalo" runat="server" Font-Bold="true" CssClass="btn btn-primary waves-effect waves-light" CausesValidation="true" ValidationGroup="SendZalo">GỬI ZALO ZNS</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-8 col-lg-8 col-xxl-8">
                    <div class="card card-preview">
                        <div class="card-header border-bottom">
                            <div class="form-group">
                                <label class="form-label"><b>CHỌN TEMPLATE ZNS:</b></label>
                                <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--Chọn template ZNS--" AutoPostBack="true" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlTemplate" InitialValue="0" ErrorMessage="Vui lòng chọn template ZNS." CssClass="text-danger small" Display="Dynamic" ValidationGroup="SendZalo" />
                            </div>
                        </div>
                        <div class="card-inner card-bordered">
                            <div class="card-inner" data-simplebar style="max-height:calc(100vh - 180px)">
                                <iframe id="ifrPreview" runat="server" style="width:100%;min-height:500px;border:1px solid #eee;" src="about:blank"></iframe>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div style="top: 0; left: 0; width: 100vw; height: 100vh; padding: 20% 45%; background: #00000030; position: fixed;">
            <div class="spinner-border text-danger" role="status" style="width: 10rem !important; height: 10rem !important;">
                <span class="sr-only">Loading...</span>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<script type="text/javascript" src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
<script type="text/javascript">
    function initSelect2() {
        $('.select2-ddl').each(function () {
            var placeholder = $(this).attr('placeholder') || '--Chọn--';
            $(this).select2({
                placeholder: placeholder,
                allowClear: true,
                width: '100%',
                language: {
                    noResults: function () { return "Không tìm thấy kết quả"; },
                    searching: function () { return "Đang tìm..."; }
                }
            }).on('change', function () {
                // Trigger ASP.NET postback on change for AutoPostBack dropdowns
                var ddlId = $(this).attr('id');
                if (typeof __doPostBack !== 'undefined') {
                    __doPostBack(ddlId, '');
                }
            });
        });
    }

    $(document).ready(function () {
        initSelect2();
    });

    // Re-initialize after UpdatePanel async postback
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        initSelect2();
    });
</script>