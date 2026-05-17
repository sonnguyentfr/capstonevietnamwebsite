<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="DesktopModules.NV_Events.Manager.Event_Cat.categoriesviewer" %>

<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
            <div class="nk-block-des text-soft">
                <p>
                    Tổng số có
                    <asp:Literal ID="ltrcount" runat="server"></asp:Literal>
                    bản ghi.
                </p>
            </div>
        </div>
        <!-- .nk-block-head-content -->
        <div class="nk-block-head-content">
            <div class="toggle-wrap nk-block-tools-toggle">
                <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-menu-alt-r"></em></a>
                <div class="toggle-expand-content" data-content="pageMenu">
                    <ul class="nk-block-tools g-3">
                        <li>
                            <div class="drodown">
                                <a href="#" class="dropdown-toggle btn btn-white btn-dim btn-outline-light" data-toggle="dropdown"><em class="d-none d-sm-inline icon ni ni-filter-alt"></em><span>Filtered By</span><em class="dd-indc icon ni ni-chevron-right"></em></a>
                                <div class="dropdown-menu dropdown-menu-right">
                                    <ul class="link-list-opt no-bdr">
                                        <li><a href="#"><span>Open</span></a></li>
                                        <li><a href="#"><span>Closed</span></a></li>
                                        <li><a href="#"><span>Onhold</span></a></li>
                                    </ul>
                                </div>
                            </div>
                        </li>
                        <li class="nk-block-tools-opt">
                            <asp:LinkButton ID="lbtAddTop" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary"><em class="icon ni ni-plus"></em>Thêm mới</asp:LinkButton>
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
<!-- .nk-block-head -->
<div class="nk-block">
    <div class="card card-bordered card-stretch">
        <div class="card-inner-group">
            <div class="card-inner p-0">
                <table id="datatableeventcat" class="nk-tb-list nk-tb-ulist table table-stripped">
                    <thead>
                        <tr class="nk-tb-item nk-tb-head">
                            <th class="nk-tb-col"><span class="sub-text">Thông tin</span></th>
                            <th class="nk-tb-col text-right" style="width: 500px"></th>
                        </tr>
                        <!-- .nk-tb-item -->
                    </thead>
                    <tbody>
                        <asp:Repeater ID="drgViewData" runat="server" OnItemDataBound="OnItemDataBound">
                            <ItemTemplate>
                                <tr class="nk-tb-item">
                                    <td class="nk-tb-col" style="padding: 10px;">
                                        <asp:HyperLink ID="cmdEdit" NavigateUrl='<%# NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' CssClass="project-title" runat="server">
                                            <div class="project-info">
                                                 <h5 class="title text-danger"> <%#Eval("CatName") %> </h6>
                                                <h5 class="title"> <%#Eval("CatNameEN") %> </h5>
                                            </div>
                                        </asp:HyperLink>
                                        <hr />
                                        <div class="project-progress">
                                            <div class="project-progress-details">
                                                <div class="project-progress-task"><em class="icon ni ni-check-round-cut"></em><span><%#CheckIn(Eval("Id")) %> check-in</span></div>
                                                <div class="project-progress-percent"><%#TyLeCheckIn(Eval("Id")) %>%</div>
                                            </div>
                                            <div class="progress progress-pill progress-md bg-light">
                                                <div class="progress-bar" style="background: #e85347 !important" data-progress="<%#TyLeCheckIn2(Eval("Id")) %>"></div>
                                            </div>
                                        </div>
                                        <div class="project-meta">
                                            <ul class="project-users g-1">
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="https://capstonevietnam.com/fairs/register/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Xem hiện thị trên web">
                                                            <span><em class="icon ni ni-eye fs-20px"></em></span></a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="/fair/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Xem hiện thị trên CRM">
                                                            <span><em class="icon ni ni-eye fs-20px"></em></span></a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="/fair/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Danh sách trường">
                                                            <span><em class="icon ni ni-home fs-20px"></em></span></a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="/fair/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Danh sách tổ chức">
                                                            <span>
                                                                <em class="icon ni ni-home-fill fs-20px"></em>
                                                            </span></a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="/fair/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Hình ảnh sự kiện">
                                                            <span><em class="icon ni ni-img fs-20px"></em></span></a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="/fair/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Danh sách diễn giả">
                                                            <span><em class="icon ni ni-account-setting-alt fs-20px"></em></span></a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="/fair/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Testimonial">
                                                            <span><em class="icon ni ni-comments fs-20px"></em></span></a>
                                                    </div>
                                                </li>
                                                <li>
                                                    <div class="user-avatar bg-light sm">
                                                        <a href="/fair/e-<%# DataBinder.Eval(Container.DataItem, "id") %>.html" target="_blank" data-toggle="tooltip" data-placement="top" data-original-title="Nhà tài trợ">
                                                            <span><em class="icon ni ni-sign-dollar fs-20px"></em></span></a>
                                                    </div>
                                                </li>
                                            </ul>
                                        </div>
                                    </td>
                                    <td class="nk-tb-col">
                                        <asp:HiddenField ID="hdfcatid" runat="server" Value='<%# Eval("id") %>' />
                                        <div class="timeline">
                                            <ul class="timeline-list">
                                                <asp:Repeater ID="rptEventinCat" runat="server">
                                                    <ItemTemplate>
                                                        <li class="timeline-item">
                                                            <div class="timeline-status bg-pink"></div>
                                                            <div class="timeline-date">
                                                                <%# CDate(DataBinder.Eval(Container.DataItem, "fromdatetime")).ToString("dd/MM/yyy") %>
                                                            </div>
                                                            <div class="timeline-data" style="width:100%">
                                                                <h6 class="timeline-title"><%# Eval("Title")%></h6>
                                                                <div class="timeline-des">
                                                                    <span class="time">
                                                                        <em class="icon ni ni-clock"></em><%# CDate(DataBinder.Eval(Container.DataItem, "fromdatetime")).ToString("HH:mm") %> &nbsp;&nbsp;
                                                                        <em class="icon ni ni-map-pin"></em><%# DataBinder.Eval(Container.DataItem, "diadiem") %></span>
                                                                </div>
                                                                <div class="project-progress">
                                                                    <div class="project-progress-details">
                                                                        <div class="project-progress-task"><em class="icon ni ni-check-round-cut"></em><span><%#CheckIn_Event(Eval("Id"), Eval("CatId")) %> check-in</span></div>
                                                                        <div class="project-progress-percent"><%#TyLeCheckIn_Event(Eval("Id"), Eval("CatId")) %>%</div>
                                                                    </div>
                                                                    <div class="progress progress-pill progress-md bg-light">
                                                                        <div class="progress-bar" style="background: #e85347 !important" data-progress="<%#TyLeCheckIn2_Event(Eval("Id"), Eval("CatId")) %>"></div>
                                                                    </div>
                                                                </div>
                                                                
                                                                <div class="timeline-des">
                                                                    <%# CoutDowntime(CInt(DataBinder.Eval(Container.DataItem, "id")))%>
                                                                    <div id="defaultCountdown<%# DataBinder.Eval(Container.DataItem, "id")%>"></div>
                                                                </div><br /><hr />
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>

                                            </ul>
                                        </div>
                                        <%--<table id="datatableeventcat" class="table table-striped  table-colored table-hover table-bordered" style="margin-bottom: 0px;">
                                            <tbody>
                                                <asp:Repeater ID="rptEventinCat" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <b><%# Eval("Title")%></b> <code><%#CountSchoolEvent(Eval("id")) %></code>
                                                                <asp:HyperLink ID="cmdEdit" NavigateUrl='<%# NavigateURL() & "?view=eventedit&itemid=" & DataBinder.Eval(Container.DataItem, "id") & "&catid=" & DataBinder.Eval(Container.DataItem, "CatId") %>'
                                                                    runat="server" title="Xem / Sửa thông tin khách hàng" data-toggle="tooltip" data-placement="top" data-original-title="Xem / Sửa thông tin khách hàng"><code><i class="fa fa-pencil-square-o"></i></code></asp:HyperLink>
                                                                <p class="text-muted fs-13px" style="padding: 5px 0px; margin: 0px;">
                                                                    <em class="icon ni ni-map-pin"></em><%# DataBinder.Eval(Container.DataItem, "diadiem") %>
                                                                </p>
                                                                <p class="text-muted fs-13px" style="padding: 0px; margin: 0px;">
                                                                    <em class="icon ni ni-clock"></em><%# BL.FormatDate(CDate(DataBinder.Eval(Container.DataItem, "fromdatetime"))) %>
                                                                </p>
                                                            </td>
                                                            <td style="text-align: center; width: 200px">
                                                                <%# CoutDowntime(CInt(DataBinder.Eval(Container.DataItem, "id")))%>
                                                                <div id="defaultCountdown<%# DataBinder.Eval(Container.DataItem, "id")%>"></div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:HyperLink ID="cmdaddevent1" NavigateUrl='<%# NavigateURL() & "?view=eventadd" & "&catid=" & DataBinder.Eval(Container.DataItem, "id") %>'
                                                            runat="server" title="Thêm địa điểm" data-toggle="tooltip" data-placement="top" data-original-title="Thêm địa điểm"><code><em class="icon ni ni-plus-round-fill fs-20px"></em></code></asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>--%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <!-- .nk-tb-item -->
                    </tbody>
                </table>
                <!-- .nk-tb-list -->
            </div>
            <!-- .card-inner -->
        </div>
        <!-- .card-inner-group -->
    </div>
    <!-- .card -->
</div>
<!-- .nk-block -->
<script src="https://crm.capstone.edu.vn/static/admin/assets/js/jquery.plugin.js"></script>
<script src='https://crm.capstone.edu.vn/static/admin/assets/js/jquery.countdown.js' type='text/javascript'></script>
<script type="text/javascript">

    $(document).ready(function () {
        $('#datatableeventcat').dataTable({
            "order": [],
            "lengthChange": false,
            "sort": false,
            "columnDefs": [{
                "targets": 'no-sort',
                "orderable": false,
            }],
            responsive: {
                details: true
            }
        });
    });
    TableManageButtons.init();
</script>

