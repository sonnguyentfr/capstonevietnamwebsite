<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_edit.ascx.vb" Inherits="NVCMS.Modules.FAQs.inc_edit" %>
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
<asp:UpdatePanel ID="udpContent" runat="server">
    <ContentTemplate>
        <div class="nk-block">
            <div class="row g-gs">
                <div class="col-md-4 col-lg-4 col-xxl-4">
                    <div class="card card-bordered h-100">
                        <div class="card-inner">
                            <div class="form-group">
                                <div class="card-head">
                                    <h5 class="card-title">Thông tin</h5>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtCauhoi.ClientID %>">Câu hỏi</label>
                                    <asp:TextBox ID="txtCauhoi" runat="server" Font-Size="14px" CssClass="form-control" required="required" TextMode="MultiLine"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCauhoi"
                                        ForeColor="Red" ErrorMessage="(Nhập câu hỏi)" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtMota.ClientID %>">Mô tả nội dung</label>
                                    <asp:TextBox ID="txtMota" runat="server" CssClass="form-control" required="required" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#ddlStatus.ClientID %>">Trạng thái</label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtOrdernumber.ClientID %>">Thứ tự</label>
                                    <asp:TextBox ID="txtOrdernumber" runat="server" CssClass="form-control" Width="90px" Text="0" TextMode="Number"></asp:TextBox>
                                </div>


                            </div>
                        </div>
                    </div>
                    <!-- .card -->
                </div>
                <div class="col-md-8 col-lg-8 col-xxl-8">
                    <div class="card card-bordered">
                        <div class="card-header border-bottom">
                            <ul class="cc_button">
                                <li>
                                    <a href="#" class="btn btn-dim btn-primary">
                                        <asp:Literal ID="ltrstatus" runat="server"></asp:Literal></a>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lbtUpdate" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass=" btn btn-sm  btn-outline-primary" OnClientClick="return checkvalidate();">
                                <span>Cập nhật</span><em class="icon ni ni-save-fill"></em>
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
                            <asp:Literal ID="lbResult" runat="server"></asp:Literal>
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
            </div>

        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbtUpdate" />
    </Triggers>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="PageUpdateProgress">
    <ProgressTemplate>
        <div id="loading">
            <div class="loading">
                <div></div>
                <div></div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>

<%--=======================================--%>


