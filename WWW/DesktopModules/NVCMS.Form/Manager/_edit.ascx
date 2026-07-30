<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_edit.ascx.vb" Inherits="NVCMS.Modules.Form.inc_edit" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
        </div>
    </div>
    <!-- .nk-block-between -->
</div>
<div class="nk-block">
    <div class="row g-gs">
        <div class="col-md-8 col-lg-8 col-xxl-8">
            <div class="card card-bordered">
                <div class="card-header border-bottom">
                    <ul class="cc_button">
                        <li>
                            <asp:LinkButton ID="lbtUpdate" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass=" btn btn-sm  btn-outline-primary" OnClientClick="return checkvalidate();">
                                <span>Lưu thay đổi</span><em class="icon ni ni-save-fill"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lblUpdateXB" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass=" btn btn-sm  btn-outline-primary" OnClientClick="return checkvalidate();">
                                <span>Lưu và xuất bản</span><em class="icon ni ni-save-fill"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtCancel" runat="server" Font-Bold="True" CssClass="__neo_submit-BTN btn btn-sm  btn-outline-danger">
                                <em class="icon ni ni-arrow-left"></em><span>Thoát</span></asp:LinkButton></li>

                        <li style="float: right;">
                            <asp:LinkButton ID="lbDelete" runat="server" Font-Bold="True" CssClass="btn btn-sm btn-dark" OnClientClick="formModified=false; return confirm('Bạn có thực sự muốn xóa các tin đã chọn không?');">
                                <span>Xóa </span><em class="icon ni ni-trash"></em>
                            </asp:LinkButton></li>
                    </ul>

                </div>
                <div class="card-inner">
                    <asp:Label ID="lbResult" runat="server" CssClass="NormalRed"></asp:Label>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtuAnswer" Font-Names="Nunito" runat="server" CssClass="form-control form-control-xl form-control-outlined editor-f-22 editor-font" ValidationGroup="InputValidate"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtuAnswer.ClientID %>">Tên hiện thị trả lời</label>
                            <asp:RequiredFieldValidator ValidationGroup="InputValidate" ControlToValidate="txtuAnswer" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Nhập tên hiện thị trả lời"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator
                                ID="valTitle" runat="server" ControlToValidate="txtuAnswer" ValidationGroup="InputValidate"
                                Display="Dynamic" CssClass="NormalRed" ErrorMessage="Tên hiện thị phải chứa ít nhất 2 ký tự"
                                ForeColor="" ValidationExpression=".{2}.*"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <span class="preview-title-lg overline-title">Nội dung trả lời</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                             <dnn:TextEditor DefaultMode="basic" ID="txtTraloi" Width="100%" Height="500" runat="server" />
                        </div>
                    </div>

                </div>
            </div>
            <!-- .card -->
        </div>
        <!-- .col -->
        <div class="col-md-4 col-lg-4 col-xxl-4">
            <div class="card card-bordered h-100">
                <div class="card-inner">
                    <div class="form-group">
                        <div class="card-head">
                            <h5 class="card-title">Thông tin</h5>
                        </div>
                        <div class="form-group">
                            <label class="form-label" for="<%#txtUsername.ClientID %>">Họ và tên</label>
                            <asp:TextBox ID="txtUsername" Text="0" CssClass="form-control" runat="server" required="required" ValidationGroup="VBuzzValidation"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsername"
                                ForeColor="Red" ErrorMessage="(Nhập họ và tên)" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label class="form-label" for="<%#txtEmail.ClientID %>">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Font-Size="14px" required="required" ValidationGroup="VBuzzValidation"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" SetFocusOnError="true" ValidationGroup="VBuzzValidation" Display="Dynamic" CssClass="error-msg"
                                Text="Địa chỉ Email không đúng!" ValidationExpression="^([0-9a-zA-Z]+[\.]{1})*[0-9a-zA-Z]+@[0-9a-zA-Z]+[\.]{1}[0-9a-zA-Z]+[\.]?[0-9a-zA-Z]+$" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <label class="form-label" for="<%#txtContactName.ClientID %>">Họ và tên</label>
                            <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control" Font-Size="14px"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="form-label" for="<%#txtMobile.ClientID %>">Số điện thoại</label>
                            <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" Font-Size="14px"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label class="form-label" for="<%#txtAddress.ClientID %>">Địa chỉ</label>
                            <div class="form-control-wrap">
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" Font-Size="14px" required="required" ValidationGroup="VBuzzValidation" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="form-label" for="<%#txtCauhoi.ClientID %>">Câu hỏi</label>
                            <div class="form-control-wrap">
                                <asp:TextBox ID="txtCauhoi" runat="server" CssClass="form-control" Font-Size="14px" required="required" ValidationGroup="VBuzzValidation" TextMode="MultiLine"></asp:TextBox>
                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- .card -->
        </div>

        <!-- .col -->
    </div>

</div>



