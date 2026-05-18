<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="List.ascx.vb" Inherits="NVCMS.Modules.Banner.ListTemplate" %>
<div class="nk-block nk-block-lg">
    <div class="nk-block-head">
        <div class="nk-block-head-content">
            <h4 class="nk-block-title"><%=PortalSettings.ActiveTab.Description %></h4>
            <div class="nk-block-des">
                <asp:LinkButton ID="lbtAddTop" runat="server" CssClass="btn btn-primary">Thêm mới</asp:LinkButton>
            </div>
        </div>
    </div>
    <div class="card card-preview">
        <div class="card-inner">
            <table class="datatable-init nk-tb-list nk-tb-ulist" data-auto-responsive="false">
                <thead>
                    <tr class="nk-tb-item nk-tb-head">
                        <th class="nk-tb-col nk-tb-col-check">
                            <span class="sub-text">#</span>
                        </th>
                        <th class="nk-tb-col"><span class="sub-text">Thông tin</span></th>
                        <th class="nk-tb-col tb-col-mb"><span class="sub-text">File</span></th>
                        <th class="nk-tb-col nk-tb-col-tools text-right"></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="drgViewData" runat="server">
                        <ItemTemplate>
                            <tr class="nk-tb-item">
                                <td class="nk-tb-col nk-tb-col-check">
                                    <%#Eval("id") %>
                                </td>
                                <td class="nk-tb-col">
                                    <div class="user-card">
                                        <div class="user-info">
                                            <span class="tb-lead"><%# Eval("TemplateName")%></span>
                                        </div>
                                    </div>
                                </td>
                                <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                    <span class="tb-amount"><%# PortalSettings.HomeDirectory & "TemplateBanner/" &  Eval("FilePath") %></span>
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
                </tbody>
            </table>
        </div>
    </div>
    <!-- .card-preview -->
</div>
<asp:LinkButton ID="lbtAddBottom" runat="server" CssClass="btn btn-primary" Font-Bold="True">Thêm mới</asp:LinkButton>

