<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.SendMailMail" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />
<style type="text/css">
    .unsubmail {
        text-decoration: line-through !important
    }

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
    <!-- .nk-block-between -->
</div>
<asp:UpdatePanel runat="server" ID="upnlAtt">
    <ContentTemplate>
        <div class="nk-block">
            <div class="row g-gs">
                <div class="col-md-4 col-lg-4 col-xxl-4">
                    <div class="card card-preview">
                        <div class="card-inner">
                            <div class="form-group">
                                <label class="form-label"><b>CHỌN SỰ KIỆN: </b></label>
                                <asp:DropDownList ID="ddlEventCat" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--chọn sự kiện--" AutoPostBack="true" OnSelectedIndexChanged="ddlEventCat_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="form-label"><b>ĐIA ĐIỂM: </b></label>
                                <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--chọn sự kiện--"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label class="form-label"><b>CHỌN CHIẾN DỊCH: </b></label>
                                <asp:DropDownList ID="ddlcampaing" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--Chọn chiến dịch--" AutoPostBack="true" OnSelectedIndexChanged="ddlcampaing_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="card-inner ">
                            <div class="card-header border-bottom">
                                Danh sách có: <mark>
                                    <asp:Literal ID="ltrok" runat="server"></asp:Literal></mark> email 
                            </div>
                            <div class="card-inner card-bordered">
                                <div class="form-group" data-simplebar style="max-height: 430px">
                                    <div class="nk-tb-list is-compact">
                                        <div class="nk-tb-item nk-tb-head">
                                            <div class="nk-tb-col"><span>#</span></div>
                                            <div class="nk-tb-col"><span>Email</span></div>
                                            <div class="nk-tb-col text-right"><span>Send</span></div>
                                        </div>
                                        <asp:Repeater ID="rptlistEmailStudent" runat="server">
                                            <ItemTemplate>
                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span>
                                                            <%# DataBinder.Eval(Container, "ItemIndex", "") + 1%>
                                                            <asp:Label ID="lblid" Text='<%#Eval("id") %>' runat="server" Visible="false"></asp:Label>
                                                        </span></span>
                                                    </div>
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub <%#IIf(CBool(DataBinder.Eval(Container.DataItem, "isUnsub")) = True, "", "unsubmail") %>"><span>
                                                            <asp:Label ID="StudentEmail" Text='<%#Eval("Email") %>' runat="server"></asp:Label>
                                                        </span></span>
                                                    </div>
                                                    <div class="nk-tb-col text-right">
                                                        <span class="tb-sub tb-amount"><span>
                                                            <%#Eval("sendcount")%>
                                                        </span></span>
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
                                <label class="form-label"><b>CHỌN Email gửi đi: </b></label>
                                <asp:DropDownList ID="ddlEmail" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--chọn email--" AutoPostBack="true" OnSelectedIndexChanged="ddlEmail_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <h3 class="text-danger">
                                    <asp:Literal ID="ltrtitlemail" runat="server"></asp:Literal></h3>
                            </div>
                            <div class="form-group">
                                <label class="form-label"><b>TIÊU ĐỀ MAIL: </b></label>
                                <asp:TextBox ID="txtTitleMail" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="form-label"><b>NỘI DUNG HIỂN THỊ VIEW MAIL: </b></label>
                                <asp:TextBox ID="txtcontentview" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:LinkButton ID="lbtSendMail" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary waves-effect waves-light">GỬI MAIL</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- .card -->
                <div class="col-md-8 col-lg-8 col-xxl-8">
                    <div class="card card-preview">
                        <div class="card-header border-bottom">
                            <div class="form-group">
                                <label class="form-label"><b>CHỌN TEMPLATE: </b></label>
                                <asp:DropDownList ID="ddltemplate" runat="server" CssClass="form-select form-control select2-ddl" data-search="on" placeholder="--Chọn tempate--" AutoPostBack="true" OnSelectedIndexChanged="ddltemplate_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="card-inner card-bordered">
                            <div class="card-inner" data-simplebar style="max-height: 720px">
                                <asp:Literal ID="ltrEmailMau" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </ContentTemplate>
    <Triggers>
    </Triggers>
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




