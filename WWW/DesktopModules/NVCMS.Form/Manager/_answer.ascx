<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_answer.ascx.vb" Inherits="NVCMS.Modules.Form.inc_edit" %>
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
                                    <label class="form-label" for="<%#lblHovaTen.ClientID %>">Họ và tên</label>
                                    <asp:TextBox ID="lblHovaTen" Enabled="false" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="form-group">
                                    <label class="form-label" for="<%#lblEmail.ClientID %>">Email</label>
                                    <asp:TextBox ID="lblEmail" Enabled="false" runat="server" CssClass="form-control" Font-Size="14px"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#lblMobile.ClientID %>">Số điện thoại</label>
                                    <asp:TextBox ID="lblMobile" Enabled="false" runat="server" CssClass="form-control" Font-Size="14px"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#lblTite.ClientID %>">Tiêu đề</label>
                                    <asp:TextBox ID="lblTite" Enabled="false" runat="server" CssClass="form-control" Font-Size="14px" TextMode="MultiLine"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#lblQuestion.ClientID %>">Nội dung</label>
                                    <div class="form-control-wrap">
                                        <blockquote class="blockquote">
                                            <asp:Literal ID="lblQuestion" runat="server" />
                                            <footer class="blockquote-footer">
                                                Ngày gửi <cite title="Source Title">
                                                    <asp:Literal ID="ltrdate" runat="server"></asp:Literal></cite>
                                            </footer>
                                        </blockquote>
                                    </div>
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
                                <span>Gửi mail trả lời</span><em class="icon ni ni-save-fill"></em>
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
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:TextBox ID="txtuAnswer" Font-Names="Nunito" runat="server" CssClass="form-control form-control-xl form-control-outlined editor-f-22 editor-font" ValidationGroup="InputValidate"></asp:TextBox>
                                    <label class="form-label-outlined" for="<%=txtuAnswer.ClientID %>">Tiêu đề</label>
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
                            <div class="card-title-group align-start pb-3 g-2">
                                <div class="card-title card-title-sm">
                                    <span class="preview-title-lg overline-title">Danh sách các câu trả lời</span>
                                </div>
                            </div>
                            <div id="accordion" class="accordion">
                                <asp:Repeater ID="rpt_formrep" runat="server">
                                    <ItemTemplate>
                                        <div class="accordion-item">
                                            <a href="#" class="accordion-head collapsed" data-toggle="collapse" data-target="#accordion-item-<%#Eval("id") %>">
                                                <h6 class="title"><%#Eval("reptitle") %></h6><em class="icon ni ni-user-circle-fill"></em>  <%#BL.GetButDanh(PortalId, Eval("repuserid")) %> | <em class="icon ni ni-clock-fill"></em>  <%#CDate(Eval("repcreateddate")).ToString("HH:mm - dd/MM/yyy") %>
                                                <span class="accordion-icon"></span>
                                            </a>
                                            <div class="accordion-body collapse" id="accordion-item-<%#Eval("id") %>" data-parent="#accordion">
                                                <div class="accordion-inner">
                                                    <%#Server.HtmlDecode(Eval("repnoidung")) %>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%--<div class="accordion-item">
                                    <a href="#" class="accordion-head" data-toggle="collapse" data-target="#accordion-item-1">
                                        <h6 class="title">What is Dashlite?</h6>
                                        <span class="accordion-icon"></span>
                                    </a>
                                    <div class="accordion-body collapse show" id="accordion-item-1" data-parent="#accordion">
                                        <div class="accordion-inner">
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="accordion-item">
                                    <a href="#" class="accordion-head collapsed" data-toggle="collapse" data-target="#accordion-item-2">
                                        <h6 class="title">What are some of the benefits of receiving my bill electronically?</h6>
                                        <span class="accordion-icon"></span>
                                    </a>
                                    <div class="accordion-body collapse" id="accordion-item-2" data-parent="#accordion">
                                        <div class="accordion-inner">
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="accordion-item">
                                    <a href="#" class="accordion-head collapsed" data-toggle="collapse" data-target="#accordion-item-3">
                                        <h6 class="title">What is the relationship between Dashlite and payment?</h6>
                                        <span class="accordion-icon"></span>
                                    </a>
                                    <div class="accordion-body collapse" id="accordion-item-3" data-parent="#accordion">
                                        <div class="accordion-inner">
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="accordion-item">
                                    <a href="#" class="accordion-head collapsed" data-toggle="collapse" data-target="#accordion-item-4">
                                        <h6 class="title">What are the benefits of using Dashlite?</h6>
                                        <span class="accordion-icon"></span>
                                    </a>
                                    <div class="accordion-body collapse" id="accordion-item-4" data-parent="#accordion">
                                        <div class="accordion-inner">
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
                                            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.</p>
                                        </div>
                                    </div>
                                </div>--%>
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
