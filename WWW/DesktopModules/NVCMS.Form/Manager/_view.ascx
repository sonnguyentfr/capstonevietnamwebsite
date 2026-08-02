<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_view.ascx.vb" Inherits="NVCMS.Modules.Form.inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register Src="~/controls/Pagesadmin.ascx" TagPrefix="uc1" TagName="Pagesadmin" %>
<%--================================================================--%>
<style type="text/css">
    .nk-tb-col {
        font-size: 12px;
    }
</style>
<div class="nk-block nk-block-lg">
    <div class="nk-block-head">
        <div class="nk-block-head-content">
            <h4 class="nk-block-title"><%=PortalSettings.ActiveTab.Title %></h4>
            <div class="nk-block-des text-soft">
                <p>
                    Tổng số có: 
                        <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                    tin bài.
                </p>
            </div>
        </div>
    </div>
    <div class="card card-preview">
        <div class="card-inner position-relative card-tools-toggle">
            <!-- .card-title-group -->
            <div class="form-validate">
                <div class="row g-gs">
                    <div class="col-md-2">
                        <div class="form-group">
                            <label class="form-label" for="fv-email">Kiểu</label>
                            <div class="form-control-wrap">
                                <asp:DropDownList ID="ddltype" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate">
                                    <asp:ListItem Value="0">Tất cả</asp:ListItem>
                                    <asp:ListItem Value="TUVAN">Đăng ký tư vấn</asp:ListItem>
                                    <asp:ListItem Value="LIENHE">Liên hệ</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="fv-full-name">Nội dung</label>
                            <div class="form-control-wrap">
                                <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <label class="form-label" for="fv-email">Trạng thái</label>
                            <div class="form-control-wrap">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate">
                                    <asp:ListItem Value="0">Tất cả</asp:ListItem>
                                    <asp:ListItem Value="VUATIEPNHAN">Vừa tiếp nhận</asp:ListItem>
                                    <asp:ListItem Value="DATRALOI">Đã trả lời</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-2">
                        <div class="form-group">
                            <br />
                            <asp:LinkButton ID="lbtFind" runat="server" CssClass="btn btn-primary">Tìm thông tin</asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
            <!-- .card-search -->
        </div>
        <div class="card card-bordered card-preview">
            <asp:UpdatePanel ID="udpContent" runat="server">
                <ContentTemplate>
                    <table class="table table-ulogs" data-auto-responsive="false">
                        <thead class="thead-light">
                            <tr class="nk-tb-item nk-tb-head">
                                <th class="nk-tb-col nk-tb-col-check">
                                    <div class="custom-control custom-control-sm custom-checkbox notext">
                                        <input type="checkbox" class="custom-control-input" id="uid">
                                        <label class="custom-control-label" for="uid"></label>
                                    </div>
                                </th>
                                <th class="nk-tb-col"><span class="sub-text">Thông tin</span></th>

                                <th class="nk-tb-col tb-col-mb"><span class="sub-text">Câu hỏi</span></th>
                                <th class="nk-tb-col tb-col-mb"><span class="sub-text">Trạng thái</span></th>
                                <th class="nk-tb-col tb-col-mb"><span class="sub-text">Kiểu</span></th>
                                <th class="nk-tb-col nk-tb-col-tools text-right"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="drgDataViewer" runat="server">
                                <ItemTemplate>
                                    <tr class="nk-tb-item">
                                        <td class="nk-tb-col nk-tb-col-check">
                                            <div class="custom-control custom-control-sm custom-checkbox notext">
                                                <input type="checkbox" class="custom-control-input" id="uid1">
                                                <label class="custom-control-label" for="uid1"></label>
                                            </div>
                                        </td>
                                        <td class="nk-tb-col">
                                            <strong><mark><%#Eval("TypeName") %>-><%#Eval("hinhthucName") %> -> <%#Eval("vanphongName") %></mark></strong>
                                            <p>Họ và Tên: <b><%# Eval("hovaten")%></b></p>
                                            <p>Email: <b><%# Eval("Email")%></b></p>
                                            <p>Điện thoại: <b><%# Eval("sodienthoai")%></b></p>
                                        </td>

                                        <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                            <%# Highlight(Eval("noidung"), "<span class='highlight'>", "</span>")%>
                                            <br />
                                            <small>Ngày: <%# BL.FormatDate(Eval("creatdate"))%></small>
                                        </td>
                                        <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                            <span class="badge badge-dot badge-warning"><%#Eval("statusname")%></span>
                                        </td>
                                        <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                            <span class="badge badge-dot badge-warning"><%#Eval("typename")%></span>
                                        </td>
                                        <td class="nk-tb-col nk-tb-col-tools">
                                            <ul class="nk-tb-actions gx-1">
                                                <li class="nk-tb-action-hidden">
                                                    <asp:HyperLink ID="Hyperlink2" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                        <em class="icon ni ni-edit-fill"></em><span>Sửa</span>
                                                    </asp:HyperLink>
                                                </li>
                                                <li>
                                                    <div class="drodown">
                                                        <a href="#" class="dropdown-toggle btn btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                        <div class="dropdown-menu dropdown-menu-right">
                                                            <ul class="link-list-opt no-bdr">
                                                                <li>
                                                                    <asp:HyperLink ID="Hyperlink1" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                        <em class="icon ni ni-pen"></em><span>Sửa</span>
                                                                    </asp:HyperLink>
                                                                </li>
                                                                <li>
                                                                    <asp:HyperLink ID="cmdEdit" NavigateUrl='<%#NavigateURL() & "?view=answer&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                        <em class="icon ni ni-pen"></em><span>Trả lời</span>
                                                                    </asp:HyperLink>
                                                                </li>

                                                            </ul>
                                                        </div>
                                                    </div>
                                                </li>
                                            </ul>
                                        </td>
                                    </tr>
                                    <!-- .nk-tb-item  -->
                                </ItemTemplate>
                            </asp:Repeater>
                            <!-- .nk-tb-item  -->
                            <tr>
                                <td colspan="6">
                                    <%--<dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" PageLinksPerPage="20" />--%>
                                    <uc1:Pagesadmin runat="server" ID="vbPaging" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lbtFind" />
                    <asp:AsyncPostBackTrigger ControlID="ddlStatus" />
                </Triggers>
            </asp:UpdatePanel>

        </div>
    </div>
    <!-- .card-preview -->
</div>

<!-- /.box-body -->
