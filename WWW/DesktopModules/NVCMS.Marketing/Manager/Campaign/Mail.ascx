<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Mail.ascx.vb" Inherits="NVCMS.Modules.Marketing.CamPaingMail" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
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

                <div class="col-md-12 col-lg-12 col-xxl-12">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <h4><asp:Literal ID="lbMessage" runat="server"></asp:Literal></h4>
                            <p><asp:Literal ID="ltedes" runat="server"></asp:Literal></p>
                        </div>
                    </div>
                </div>
                <div class="col-md-7 col-lg-7 col-xxl-7">
                    <div class="card card-preview">
                        <div class="card-inner">
                            <ul class="nav nav-tabs mt-n3">
                                <li class="nav-item">
                                    <a class="nav-link active" data-toggle="tab" href="#tabItem5"><em class="icon ni ni-user"></em><span>Lọc theo sự kiện</span></a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-toggle="tab" href="#tabItem6"><em class="icon ni ni-lock-alt"></em><span>Lấy theo danh sách có sẵn</span></a>
                                </li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane active" id="tabItem5">
                                    <div class="form-group">
                                        <label class="form-label"><b>CHỌN SỰ KIỆN: </b></label>
                                        <asp:DropDownList ID="ddlEventCat" runat="server" CssClass="form-select form-control" data-search="on" placeholder="--chọn sự kiện--" AutoPostBack="true" OnSelectedIndexChanged="ddlEventCat_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label class="form-label"><b>ĐIA ĐIỂM: </b></label>
                                        <asp:DropDownList ID="ddlEvent" runat="server" CssClass="form-select form-control" data-search="on" placeholder="--chọn sự kiện--"></asp:DropDownList>
                                    </div>
                                    <div class="form-group">
                                        <label class="form-label"><b>TRẠNG THÁI: </b></label>
                                        <asp:DropDownList ID="CheckIn" runat="server" CssClass="form-select form-control" data-search="on" placeholder="--trạng thái--"></asp:DropDownList>
                                    </div>
                                    <asp:LinkButton ID="lbtUpdate_Event" OnClientClick="return ValidateEvent();" runat="server" Font-Bold="True" class="btn btn-primary">XEM DANH SÁCH</asp:LinkButton>
                                </div>
                                <div class="tab-pane" id="tabItem6">
                                    <p><strong>Nhập danh sách Email vào đây</strong></p>
                                    <asp:TextBox ID="txtMail" runat="server" TextMode="MultiLine" Height="330px" CssClass="form-control no-resize"></asp:TextBox>
                                    <p>Các Email cách nhau bới dấu <mark>;</mark> (chấm phẩy)</p>
                                    <asp:LinkButton ID="lbtUpdate_TextBox" OnClientClick="return ValidateTexBox();" runat="server" Font-Bold="True" class="btn btn-primary">CẬP NHẬT VÀO DANH SÁCH</asp:LinkButton>
                                </div>

                            </div>
                        </div>
                    </div>
                    <!-- .card -->
                </div>
                <div class="col-md-5 col-lg-5 col-xxl-5">
                    <div class="nk-msg-list">
                        <div class="card card-bordered">
                            <div class="card-header border-bottom">
                                <b>Kết quả Import Email</b> &nbsp;&nbsp;&nbsp;<asp:LinkButton ID="lbtInsertFromEvent" Visible="false"  runat="server" class="btn btn-primary">THÊM VÀO DANH SÁCH</asp:LinkButton>
                                <p>
                                    <b>
                                        <asp:Literal ID="ltrok" runat="server"></asp:Literal></b> email thành công | <b>
                                            <asp:Literal ID="ltrloi" runat="server"></asp:Literal></b> lỗi
                                     | <b>
                                         <asp:Literal ID="ltrtrung" runat="server"></asp:Literal></b> trùng
                                </p>

                            </div>
                            <div class="card-inner">
                                <div class="form-group" data-simplebar style="max-height: 430px">
                                    <div class="nk-tb-list is-compact">
                                        <div class="nk-tb-item nk-tb-head">
                                            <div class="nk-tb-col"><span>Email</span></div>
                                            <div class="nk-tb-col text-right"><span>Status</span></div>
                                        </div>
                                        <asp:Repeater ID="rptlistEmailStudent" runat="server">
                                            <ItemTemplate>
                                                <div class="nk-tb-item" style="<%#IIf(Eval("StudentEmail_status") = "false", "background: #f2ffa6;", "") %>">
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span><%#Eval("StudentFullname") %></span></span>
                                                    </div>
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span>
                                                            <asp:Label id="StudentEmail" Text='<%#Eval("StudentEmail") %>' runat="server"></asp:Label>
                                                            </span></span>
                                                    </div>
                                                    <div class="nk-tb-col text-right">
                                                        <span class="tb-sub tb-amount"><span>
                                                            <%#IIf(Eval("StudentEmail_status") = "false", "LỖI", "OK") %>
                                                        </span></span>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Repeater ID="rptEmailPass" runat="server">
                                            <ItemTemplate>
                                                <div class="nk-tb-item">
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span><%#Eval("Email") %></span></span>
                                                    </div>
                                                    <div class="nk-tb-col text-right">
                                                        <span class="tb-sub tb-amount"><span><%#Eval("status") %></span></span>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Repeater ID="rptEmailLoi" runat="server">
                                            <ItemTemplate>
                                                <div class="nk-tb-item" style="background: #f2ffa6;">
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span><%#Eval("Email") %></span></span>
                                                    </div>
                                                    <div class="nk-tb-col text-right">
                                                        <span class="badge badge-pill badge-danger"><%#Eval("status") %></span>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Repeater ID="rptEmailTrung" runat="server">
                                            <ItemTemplate>
                                                <div class="nk-tb-item" style="background: #c3c3c3;">
                                                    <div class="nk-tb-col">
                                                        <span class="tb-sub"><span><%#Eval("Email") %></span></span>
                                                    </div>
                                                    <div class="nk-tb-col text-right">
                                                        <span class="badge badge-pill badge-warning"><%#Eval("status") %></span>
                                                    </div>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="col-md-12 col-lg-12 col-xxl-12">
                    <div class="card card-bordered">
                        <div class="card-inner">
                            <div class="card-title-group align-start pb-3 g-2">
                                <div class="card-title card-title-sm">
                                    <span class="preview-title-lg overline-title">
                                        <h4>DANH SÁCH EMAIL</h4>
                                    </span>
                                    <p>
                                        Số lượng email: <b>
                                            <asp:Literal ID="lblTotal2" runat="server"></asp:Literal></b>
                                    </p>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <ul class="preview-list">
                                        <asp:Repeater ID="rptlistmail" runat="server">
                                            <ItemTemplate>
                                                <li class="preview-item" style="padding: 0.25rem 0.5rem !important">
                                                    <span class="<%#IIf(Ultis.IsValidEmail(DataBinder.Eval(Container.DataItem, "Email").Trim) = "false", "badge badge-pill badge-danger fs-14px", "badge badge-pill badge-outline-secondary fs-14px") %>">
                                                        <asp:LinkButton ID="GetInfo" CommandArgument='<%#Eval("Id") %>' CommandName="GetInfo" OnClick="GetInfo" runat="server" title="Sửa thông tin" data-toggle="tooltip" data-placement="top" data-original-title="Sửa thông tin">
                                                    <em class="icon ni ni-edit-fill"></em>
                                                        </asp:LinkButton>
                                                        <span><%# Eval("Email")%></span>
                                                        <asp:LinkButton ID="btnDelete" CommandArgument='<%#Eval("Id") %>' CommandName="btnDelete" OnClick="btnDelete" OnClientClick="javascript: return confirm('Bạn có muốn xoá email này không?');" ToolTip="Xoá Email" runat="server">
                                                            <em class="icon ni ni-cross-sm"></em>
                                                        </asp:LinkButton>
                                                        
                                                    </span>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade zoom" tabindex="-1" id="modalEdit">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                        <em class="icon ni ni-cross"></em>
                    </a>
                    <div class="modal-header">
                        <h5 class="modal-title">Thêm mới / Chính sửa</h5>
                    </div>
                    <div class="modal-body">
                        <div class="form-validate is-alter">
                            <div class="form-group">
                                <label class="form-label" for="full-name">Email</label>
                                <div class="form-control-wrap">
                                    <asp:TextBox ID="txtEmail" required="" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer bg-light">
                        <asp:LinkButton ID="lbtUpdateEmail" OnClientClick="return checkvalidate();" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary">Cập nhật</asp:LinkButton>
                        <a href="javascript:void(0);" type="button" class="btn btn-secondary waves-effect" data-dismiss="modal">Hủy thao khác</a>
                        <asp:LinkButton ID="lbtDelete" Visible="false" OnClientClick="javascript: return confirm('Bạn có muốn xoá thư mục tin này không?');" ToolTip="Xoá thư mục" runat="server" CssClass="btn btn-danger">Xoá</asp:LinkButton>
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
        <div class="loading" id="loadizng">Loading&#8230;</div>
    </ProgressTemplate>
</asp:UpdateProgress>

<script type="text/javascript">
    function ValidateEvent() {
        var ddlEventCat = document.getElementById('<%=ddlEventCat.ClientID%>').value;
        if (ddlEventCat == 0) {
            alert("Vui lòng Chọn sự kiện");
            document.getElementById('<%=ddlEventCat.ClientID%>').focus;
            return false;
        }
    }
    function ValidateTexBox() {
        var emailstring = document.getElementById('<%=txtMail.ClientID%>').value;
        if (emailstring == "") {
            alert("Vui lòng nhập danh sách Email");
            document.getElementById('<%=txtMail.ClientID%>').focus;
            return false;
        }
    }
    function checkvalidate() {
        var txtCatName = document.getElementById('<%=txtEmail.ClientID%>').value;
        if (txtCatName == "") {
            alert("Nhập tên hiển thị");
            return false;
        }
    }
    function Modalhoatdong() {
        $('#modalEdit').modal('show');
    };
    function ModalFollowUpClose() {
        $('#modalEditl').modal('hide');
        $('.modal-backdrop').css({
            display: 'none'
        });
    };
</script>




