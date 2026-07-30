<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_view.ascx.vb" Inherits="NVCMS.Modules.FAQs.inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<style type="text/css">
    .table-ulogs tr td {
        font-size: 12px;
    }

        .table-ulogs tr td.cautraloitd {
            width: 60%
        }

            .table-ulogs tr td.cautraloitd .cautraloi {
                overflow: hidden !important;
                display: -webkit-box !important;
                -webkit-line-clamp: 3 !important;
                -webkit-box-orient: vertical;
            }
</style>
<div class="nk-block nk-block-lg">
    <div class="nk-block-head">
        <div class="nk-block-head-content">
            <h4 class="nk-block-title"><%=PortalSettings.ActiveTab.Title %></h4>
            <div class="nk-block-des">
                <asp:LinkButton ID="lbtAddTop" runat="server" CssClass="btn btn-primary">Thêm mới</asp:LinkButton>
            </div>
            <div class="nk-block-des text-soft">
                <p>
                    Tổng số có: 
                        <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                    bản ghi.
                </p>
            </div>
        </div>
    </div>
    <div class="card card-preview">
        <div class="card-inner position-relative card-tools-toggle">
            <!-- .card-title-group -->
            <div class="form-validate">
                <div class="row g-gs">
                    <div class="col-md-6">
                        <div class="form-group">
                            <label class="form-label" for="fv-full-name">Câu hỏi</label>
                            <div class="form-control-wrap">
                                <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-group">
                            <label class="form-label" for="fv-email">Trạng thái</label>
                            <div class="form-control-wrap">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate">
                                    <asp:ListItem Value="0">Tất cả</asp:ListItem>
                                    <asp:ListItem Value="1">Vừa tiếp nhận</asp:ListItem>
                                    <asp:ListItem Value="3">Đã xuất bản</asp:ListItem>
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
                                <th class="nk-tb-col nk-tb-col-check">#
                                </th>
                                <th class="nk-tb-col"><span class="sub-text">Thông tin</span></th>

                                <th class="nk-tb-col tb-col-mb"><span class="sub-text">Trả lời</span></th>
                                <th class="nk-tb-col tb-col-mb"><span class="sub-text">Trạng thái</span></th>
                                <th class="nk-tb-col nk-tb-col-tools text-right"></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="drgDataViewer" runat="server">
                                <ItemTemplate>
                                    <tr class="nk-tb-item">
                                        <td class="nk-tb-col nk-tb-col-check">
                                            <%#Eval("id") %>
                                        </td>
                                        <td class="nk-tb-col">
                                            <%-- <p style="color: #d43604; font-size: 17px; font-weight: bold;"><%# Highlight(Eval("CauHoi"), "<span class='highlight'>", "</span>")%>"</p>
                                            <small><%# Eval("Mota")%></small>--%>
                                            <p>Họ và Tên: <b><%# Eval("UserName")%></b></p>
                                            <p>Email: <b><%# Eval("Email")%></b></p>
                                        </td>
                                        <td class="nk-tb-col tb-col-mb cautraloitd">
                                            Câu hỏi: <%# Highlight(Eval("Question"), "<span class='highlight'>", "</span>")%><br />
                                            <small>Ngày: <%# BL.FormatDate(Eval("CreatedDate"))%></small>
                                            <div class="cautraloi"><%# Server.HtmlDecode(DataBinder.Eval(Container.DataItem, "Traloi")) %></div>
                                            <asp:LinkButton ID="cmdquickview" OnClick="cmdquickview" CommandName="cmdquickview" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Lịch sử bài viết" runat="server">
                                                <strong><em class="icon ni ni-shrink"></em><span>Xem nhanh</span></strong>
                                            </asp:LinkButton>
                                        </td>
                                        <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                            <%#GetStatus(Eval("status"))%>
                                        </td>
                                        <td class="nk-tb-col nk-tb-col-tools">
                                            <ul class="nk-tb-actions gx-1">
                                                <li class="nk-tb-action-hidden">
                                                    <asp:HyperLink ID="Hyperlink2" NavigateUrl='<%#NavigateURL() & "?view=answer&answer=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                        <em class="icon ni ni-edit-fill"></em><span>Trả lời</span>
                                                    </asp:HyperLink>
                                                </li>
                                                <li>
                                                    <div class="drodown">
                                                        <a href="#" class="dropdown-toggle btn btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                        <div class="dropdown-menu dropdown-menu-right">
                                                            <ul class="link-list-opt no-bdr">
                                                                <li>
                                                                    <asp:HyperLink ID="Hyperlink1" NavigateUrl='<%#NavigateURL() & "?view=edit&questionid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                                        <em class="icon ni ni-edit-fill"></em><span>Sửa</span>
                                                                    </asp:HyperLink>
                                                                </li>
                                                                <li>
                                                                    <asp:HyperLink ID="Hyperlink3" NavigateUrl='<%#NavigateURL() & "?view=answer&answer=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                        <em class="icon ni ni-edit-fill"></em><span>Trả lời</span>
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
                                    <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" PageLinksPerPage="20" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <%--Đoạn nay xem nhanh--%>
                    <div class="modal fade" tabindex="-1" id="modal-history">
                        <div class="modal-dialog modal-xl modal-dialog-top" role="document">
                            <div class="modal-content">
                                <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                                    <em class="icon ni ni-cross"></em>
                                </a>
                                <div class="modal-header">
                                    <h5 class="modal-title">Thông tin chi tiết</h5>
                                </div>
                                <div class="modal-body">
                                    <h4 class="text-danger">
                                        <asp:Literal ID="ltrcauhoi" runat="server"></asp:Literal></h4>
                                    <p>
                                        Tác giả:
                                        <asp:Literal ID="lblhAuthor" runat="server"></asp:Literal>
                                        Ngày đăng:
                                        <asp:Literal ID="ltlngaydang" runat="server"></asp:Literal>
                                    </p><hr />
                                    <h4 class="text-danger">
                                        Trả lời</h4>
                                    <p>
                                        <asp:Literal ID="ltrcautraloi" runat="server"></asp:Literal>
                                    </p>

                                </div>
                            </div>
                        </div>
                    </div>
                    <%--===================================================--%>
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
<asp:LinkButton ID="lbtAddBottom" runat="server" CssClass="btn btn-primary" Font-Bold="True">Thêm mới</asp:LinkButton>
<!-- /.box-body -->
