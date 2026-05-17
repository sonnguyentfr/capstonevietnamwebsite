<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_view.ascx.vb" Inherits="NVCMS.Modules.ShortURL.inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register Src="~/controls/Pagesadmin.ascx" TagPrefix="uc1" TagName="Pages" %>

<style type="text/css">
    .location {
        padding: 5px;
    }

        .location .project {
        }

            .location .project .project-head {
            }

                .location .project .project-head .user-avatar {
                    background: none;
                    border-radius: 0px;
                    width: unset;
                }

                    .location .project .project-head .user-avatar img {
                        border-radius: 0px;
                        max-height: 40px;
                    }

    .toggle-slide-right.content-active {
        transform: unset;
    }

    .toggle-slide.content-active {
        position: relative;
    }
</style>
<div class="nk-block nk-block-lg">
    <asp:UpdatePanel ID="UpTrinhDo" runat="server">
        <ContentTemplate>
            <div class="nk-block-head nk-block-head-lg">
                <div class="card card-preview">
                    <div class="card-inner position-relative card-tools-toggle">
                        <!-- .card-title-group -->
                        <div class="form-validate">
                            <div class="row g-gs">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <label class="form-label" for="fv-full-name">Tiêu đề</label>
                                        <div class="form-control-wrap">
                                            <asp:TextBox ID="txtTitle" CssClass="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label class="form-label" for="fv-full-name">&nbsp;</label>
                                        <asp:DropDownList ID="ddlAction" runat="server" CssClass="drodown form-control-select form-control">
                                            <asp:ListItem Value="" Text="Lọc tìm kiếm"></asp:ListItem>
                                            <asp:ListItem Value="NAME" Text="Tên ShortLink A->Z"></asp:ListItem>
                                            <asp:ListItem Value="NAME_DESC" Text="Tên ShortLink Z->A"></asp:ListItem>
                                            <asp:ListItem Value="CLICK" Text="Click Thấp->Cao"></asp:ListItem>
                                            <asp:ListItem Value="CLICK_DESC" Text="Click Cao->Thấp"></asp:ListItem>
                                            <asp:ListItem Value="VIEW" Text="Redirect Thấp->Cao"></asp:ListItem>
                                            <asp:ListItem Value="VIEW_DESC" Text="Redirect Cao->Thấp"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <div class="form-group">
                                        <label class="form-label" for="fv-full-name">&nbsp;</label>
                                        <asp:LinkButton ID="lbtFind" runat="server" CssClass="btn btn-primary form-control">Tìm thông tin</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- .card-search -->
                    </div>

                </div>
                <div class="nk-block-head nk-block-head-sm">
                    <div class="nk-block-between">
                        <div class="nk-block-head-content">
                            <h3 class="nk-block-title page-title">Danh sách ShortLink</h3>
                            <div class="nk-block-des text-soft">
                                <p>
                                    Tổng số có
                                    <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                                    bản ghi..
                                </p>
                            </div>
                        </div>
                        <!-- .nk-block-head-content -->
                        <div class="nk-block-head-content">
                            <div class="toggle-wrap nk-block-tools-toggle">
                                <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-menu-alt-r"></em></a>
                                <div class="toggle-expand-content" data-content="pageMenu">
                                    <ul class="nk-block-tools g-3">
                                        <li class="nk-block-tools-opt">
                                            <asp:LinkButton ID="lbtAddNews" CssClass="btn btn-primary" ToolTip="Thêm mới" runat="server"><em class="icon ni ni-plus"></em><span>Thêm mới</span></asp:LinkButton>
                                        </li>
                                        <li class="nk-block-tools-opt">
                                            <asp:LinkButton ID="lbtRemoveCache" CssClass="btn btn-danger" ToolTip="Xóa cache" runat="server"><em class="icon ni ni-plus"></em><span>Xóa cache</span></asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <!-- .toggle-wrap -->
                        </div>
                        <!-- .nk-block-head-content -->
                    </div>
                    <!-- .nk-block-between -->
                </div>
                <div class="card card-preview">
                    <div class="card-inner">
                        <table class="datatable-trinhdo nk-tb-list nk-tb-ulist" data-auto-responsive="true">
                            <thead>
                                <tr class="nk-tb-item nk-tb-head">
                                    <th class="nk-tb-col"><span class="sub-text">ShortLink</span></th>
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">Real Link</span></th>
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">Ngày tạo</span></th>
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">Click</span></th>
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">Redirect</span></th>
                                    <th class="nk-tb-col nk-tb-col-tools text-right"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="drgDataViewer" runat="server">
                                    <ItemTemplate>
                                        <tr class="nk-tb-item">
                                            <td class="nk-tb-col">
                                                <div class="user-card">
                                                    <div class="user-info">
                                                        <a  href="<%#NavigateURL() & "?view=edit&itemid=" & Eval("id") %>" data-toggle="tooltip" data-placement="top" data-original-title="Sửa thông tin">
                                                        <strong><%#Eval("short_url") %></strong></a>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="nk-tb-col tb-col-md">
                                                <%#Eval("real_url") %>
                                            </td>
                                            <td class="nk-tb-col tb-col-md">
                                                <%#BL.FormatDate(Eval("create_date")) %>
                                            </td>
                                            <td class="nk-tb-col tb-col-md">
                                                <%#Eval("short_clicks") %>
                                            </td>
                                            <td class="nk-tb-col tb-col-md">
                                                <span class="badge badge-danger">
                                                    <a class="text-white" href="<%#NavigateURL() & "?view=share&itemid=" & Eval("short_url") %>" data-toggle="tooltip" data-placement="top" data-original-title="Click vào đây để cập nhật link đã share">
                                                        <%#Eval("countChats") %></a></span>
                                            </td>
                                            <td class="nk-tb-col nk-tb-col-tools">
                                                <ul class="nk-tb-actions gx-1">
                                                    <li class="nk-tb-action-hidden">
                                                        <%--<asp:LinkButton ID="cmdEdit" OnClick="cmdEdit" CommandName="cmdEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' runat="server" data-target="addInfo" data-toggle="tooltip" data-placement="top" title="Sửa thông tin">
                                                        <em class="icon ni ni-edit-fill"></em>
                                                        </asp:LinkButton>--%>
                                                        <a  href="<%#NavigateURL() & "?view=edit&itemid=" & Eval("id") %>" data-toggle="tooltip" data-placement="top" data-original-title="Sửa thông tin">
                                                            <em class="icon ni ni-edit-fill"></em></a></span>
                                                    </li>
                                                    <li class="nk-tb-action-hidden">
                                                        <a href="<%#Eval("real_url") %>" target="_blank" class="btn btn-trigger btn-icon" data-toggle="tooltip" data-placement="top" title="Xem nhanh">
                                                            <em class="icon ni ni-focus"></em>
                                                        </a>
                                                    </li>
                                                </ul>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <uc1:Pages runat="server" ID="vbPaging" />
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
