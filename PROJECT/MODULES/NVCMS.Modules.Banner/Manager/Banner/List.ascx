<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="List.ascx.vb" Inherits="NVCMS.Modules.Banner.ListBanner" %>
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
        <div class="card-inner position-relative card-tools-toggle">
            <div class="card-title-group" data-select2-id="13">
                <div class="card-tools" data-select2-id="12">
                    <div class="form-inline flex-nowrap gx-3" data-select2-id="11">
                        <div class="form-wrap w-400px">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select form-control form-control-xl" Width="100%" ValidationGroup="InputValidate" onchange="changeCategory();" DataTextField="CategoryName" DataValueField="CategoryID" data-ui="xl"></asp:DropDownList>
                            <label class="form-label-outlined" for="<%=ddlCategory.ClientID %>">Chọn vị trí</label>
                        </div>
                        <div class="btn-wrap">
                            <span class="d-none d-md-block">
                                <asp:LinkButton ID="lbtFind" runat="server" CssClass="form-control btn btn-round btn-warning"><i class="fa fa-search"></i> Tìm</asp:LinkButton>

                            </span>
                        </div>
                    </div>
                    <!-- .form-inline -->
                </div>
                <!-- .card-tools -->

                <!-- .card-tools -->
            </div>
            <!-- .card-title-group -->

            <!-- .card-search -->
        </div>
        <div class="card-inner">
            <table class="datatable-init nk-tb-list nk-tb-ulist" data-auto-responsive="false">
                <thead>
                    <tr class="nk-tb-item nk-tb-head">
                        <th class="nk-tb-col nk-tb-col-check">
                            <div class="custom-control custom-control-sm custom-checkbox notext">
                                <input type="checkbox" class="custom-control-input" id="uid">
                                <label class="custom-control-label" for="uid"></label>
                            </div>
                        </th>
                        <th class="nk-tb-col"><span class="sub-text">Thông tin</span></th>
                        <th class="nk-tb-col tb-col-mb"><span class="sub-text">Vị trí</span></th>
                        <th class="nk-tb-col tb-col-mb"><span class="sub-text">Kích thước</span></th>
                        <th class="nk-tb-col tb-col-mb"><span class="sub-text">Hiện thị</span></th>
                        <th class="nk-tb-col tb-col-mb"><span class="sub-text">Click</span></th>
                        <th class="nk-tb-col nk-tb-col-tools text-right"></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="drgViewData" runat="server">
                        <ItemTemplate>
                            <tr class="nk-tb-item">
                                <td class="nk-tb-col nk-tb-col-check">
                                    <div class="custom-control custom-control-sm custom-checkbox notext">
                                        <input type="checkbox" class="custom-control-input" id="uid1">
                                        <label class="custom-control-label" for="uid1"></label>
                                    </div>
                                </td>
                                <td class="nk-tb-col">
                                    <div class="user-card">
                                        <div class="news-avatar xs bg-primary">
                                            <a href="<%# Ultis.FormatFullImage(Eval("IMGLink")) %>" data-fancybox data-caption="" class="usernewsimage" data-toggle="tooltip" data-placement="top" title="Xem ảnh lớn">
                                                <asp:Image ImageUrl='<%# Ultis.FormatThumbImage(Eval("IMGLink"), 140, 80, "", "", "", "") %>' AlternateText="" ID="imgNews" runat="server" />
                                            </a>
                                        </div>
                                        <div class="user-info">
                                            <span class="tb-lead"><%# Eval("Title")%></span>
                                        </div>
                                    </div>
                                </td>
                                <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                    <span class="tb-amount"><%# Eval("TenVitri")%></span>
                                </td>
                                <td class="nk-tb-col tb-col-mb" data-order="35040.34">
                                    <span class="tb-amount">Dài: <%# Eval("width") %> x  Cao: <%# Eval("height") %></span>
                                </td>
                                <td class="nk-tb-col tb-col-mb" style="font-size: 20px;" data-order="35040.34">
                                    <%#IIf(CBool(DataBinder.Eval(Container.DataItem, "Visible")) = True, "<em class='icon ni ni-eye'></em>", "<em class='icon ni ni-eye-off'></em>") %>
                                </td>
                                <td class="nk-tb-col tb-col-mb" style="font-size: 20px;" data-order="35040.34">
                                    <span class="tb-amount"><%# Eval("Click")%></span>
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
<asp:LinkButton ID="lbtUpdateOrder" CssClass="btn btn-success" runat="server" Font-Bold="True">Cập nhật thứ tự</asp:LinkButton>

<!-- /.box-body -->
