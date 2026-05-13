<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="categoriesviewer.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.categories.categoriesviewer" %>
<div class="components-preview ">
    <div class="nk-block-head nk-block-head-lg wide-sm">
        <div class="nk-block-head-content">
            <h2 class="nk-block-title fw-normal"><%=PortalSettings.ActiveTab.Title %></h2>
        </div>
        <div class="nk-block-head-content">
            <div class="toggle-wrap nk-block-tools-toggle">
                <asp:LinkButton ID="lbtAddTop" runat="server" CssClass="btn btn-white btn-outline-light"><em class="icon ni ni-download-cloud"></em><span>Thêm mới tin</span></asp:LinkButton>
            </div>
            <!-- .toggle-wrap -->
        </div>
    </div>
    <!-- .nk-block-head -->
    <!-- nk-block -->
    <div class="nk-block nk-block-lg">
        <div class="card card-preview">
            <div class="card-inner">
                <table class="table table-stripped nk-tb-list nk-tb-ulist">
                    <thead>
                        <tr class="nk-tb-item nk-tb-head">
                            <th class="nk-tb-col nk-tb-col-check">
                                <span>#</span>
                            </th>
                            <th class="nk-tb-col"><span class="sub-text">Tên chuyên mục</span></th>
                            <th class="nk-tb-col tb-col-mb"><span class="sub-text">Trạng thái</span></th>
                            <th class="nk-tb-col tb-col-md"><span class="sub-text">TabID</span></th>
                            <th class="nk-tb-col tb-col-md"><span class="sub-text">TabIDDetail</span></th>
                            <th class="nk-tb-col tb-col-lg"><span class="sub-text">Thứ tự</span></th>
                            <th class="nk-tb-col nk-tb-col-tools text-right"></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="drgViewData" runat="server">
                            <ItemTemplate>
                                <tr class="nk-tb-item">
                                    <td class="nk-tb-col nk-tb-col-check">
                                        <span><asp:Label ID="categoryID" Text='<%#Eval("categoryID") %>' runat="server"></asp:Label></span>
                                    </td>
                                    <td class="nk-tb-col">
                                        <div class="user-card">
                                            <div class="user-info">
                                                <span class="d-sm-inline tb-lead"><%#Eval("categoryname") %></span>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                        <span class="tb-amount"><%# iif(DataBinder.Eval(Container.DataItem, "isactive")="True","<em class='icon ni ni-eye'></em>","<em class='icon ni ni-eye-off-fill'></em>") %></span>
                                    </td>
                                    <td class="nk-tb-col tb-col-md">
                                        <span><%#Eval("TabID") %></span>
                                    </td>
                                    <td class="nk-tb-col tb-col-md">
                                        <span><%#Eval("TabIDDetail") %></span>
                                    </td>
                                    <td class="nk-tb-col tb-col-lg" data-order="Email Verified - Kyc Unverified">
                                        <span>
                                            <asp:TextBox ID="txtOrderNumber" Width="40" Text='<%# DataBinder.Eval(Container.DataItem, "OrderNumber") %>' runat="server"></asp:TextBox>
                                        </span>
                                    </td>

                                    <td class="nk-tb-col nk-tb-col-tools">
                                        <ul class="nk-tb-actions gx-1">
                                            <li class="nk-tb-action-hidden">
                                                <asp:HyperLink ID="Hyperlink1" NavigateUrl='<%#NavigateURL() & "?view=edit&ItemID=" & DataBinder.Eval(Container.DataItem, "categoryid") %>' runat="server">
                                                    <em class="icon ni ni-edit-fill"></em><span>Sửa</span>
                                                </asp:HyperLink>
                                            </li>
                                            <%--<li class="nk-tb-action-hidden">
                                                <a href="#" class="btn btn-trigger btn-icon" data-toggle="tooltip" data-placement="top" title="Send Email">
                                                    <em class="icon ni ni-mail-fill"></em>
                                                </a>
                                            </li>
                                            <li class="nk-tb-action-hidden">
                                                <a href="#" class="btn btn-trigger btn-icon" data-toggle="tooltip" data-placement="top" title="Suspend">
                                                    <em class="icon ni ni-user-cross-fill"></em>
                                                </a>
                                            </li>--%>
                                        </ul>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>


                    </tbody>
                </table>
            </div>
        </div>
        <!-- .card-preview -->
    </div>
    <asp:LinkButton ID="lbtAddBottom" runat="server" CssClass="btn btn-primary"><em class="icon ni ni-download-cloud"></em><span>Thêm mới</span></asp:LinkButton>&nbsp;&nbsp;
    <asp:LinkButton ID="lbtUpdateOrder" runat="server" CssClass="btn btn-warning" OnClick="lbtUpdateOrder_Click" ><em class="icon ni ni-sort-line"></em> <span>Cập nhật thứ tự</span></asp:LinkButton>
</div>

