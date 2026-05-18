<%@ Control Language="VB" AutoEventWireup="false" CodeFile="header.ascx.vb" Inherits="DesktopModules.TinTuc.Controls.Headersss" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<style>
    .color-red {
        color: red !important;
    }
</style>
<div class="nk-header nk-header-fixed nk-header-fluid is-light">
    <div class="container-fluid">
        <div class="nk-header-wrap">
            <div class="nk-menu-trigger d-xl-none ml-n1">
                <a href="#" class="nk-nav-toggle nk-quick-nav-icon" data-target="sidebarMenu"><em class="icon ni ni-menu"></em></a>
            </div>
            <div class="nk-header-brand d-xl-none">
                <a href="/" class="logo-link">
                    <img class="logo-light logo-img" src="/static/_admin/images/logo.png" srcset="/static/_admin/images/logo2x.png 2x" alt="logo">
                    <img class="logo-dark logo-img" src="/static/_admin/images/logo-dark.png?v=1" srcset="/static/_admin/images/logo-dark2x.png 2x" alt="logo-dark">
                </a>
            </div>
            <!-- .nk-header-brand -->
            <div class="nk-header-search ml-3 ml-xl-0">
                <nav>
                    <ul class="breadcrumb">
                        <dnn:BREADCRUMB ID="dnnBreadcrumb" runat="server" UseTitle="true" CssClass="breadcrumbLink" RootLevel="0" Separator="&lt;img src=&quot;/Portals/_default/Skins/Xcillion/Images/breadcrumb-arrow.png&quot; alt=&quot;breadcrumb separator&quot;&gt;" HideWithNoBreadCrumb="true" />
                    </ul>
                </nav>
            </div>
            <!-- .nk-header-news -->
            <div class="nk-header-tools">
                <ul class="nk-quick-nav">
                    <li class="">Bạn đang ở website:
                        <strong class="color-red">
                            <asp:Literal ID="ltrCurrentWebite" runat="server"></asp:Literal></strong>
                    </li>
                    <li class="dropdown chats-dropdown hide-mb-xs">
                        <a href="#" class="dropdown-toggle mr-n1" data-toggle="dropdown">
                            <div class="user-toggle">
                                thay đổi
                            </div>
                        </a>
                        <div class="dropdown-menu dropdown-menu-xl dropdown-menu-right">
                            <div class="dropdown-head">
                                <span class="sub-title nk-dropdown-title">Chọn Website Quản trị</span>
                            </div>
                            <div class="dropdown-body">
                                <div class="nk-notification">
                                    <div class="form-group" style="padding: 0px 10px;">
                                        <asp:DropDownList
                                            ID="ddlPortal"
                                            runat="server"
                                            AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlPortal_SelectedIndexChanged" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <!-- .nk-notification -->
                            </div>
                            <!-- .nk-dropdown-body -->
                            <div class="dropdown-foot center">
                                &nbsp;
                            </div>
                        </div>
                    </li>
                    <li class="dropdown chats-dropdown hide-mb-xs">
                        <asp:HyperLink NavigateUrl="/quan-tri/video-cap-cao/cho-xuat-ban" ID="hplvideochoxuatban" CssClass="dropdown-toggle nk-quick-nav-icon" data-toggle="dropdown" title="Video chờ xuất bản" runat="server" Visible="false">
                            <div class="icon-status icon-status-na">
                                <em class="icon ni ni-video"></em><span class="icon-status-count">
                                    <asp:Literal ID="ltrvideochoxuatban" runat="server"></asp:Literal></span>
                            </div>
                        </asp:HyperLink>
                        <div class="dropdown-menu dropdown-menu-xl dropdown-menu-right">
                            <div class="dropdown-head">
                                <span class="sub-title nk-dropdown-title">Video chờ xuất bản</span>
                            </div>
                            <div class="dropdown-body">
                                <div class="nk-notification">
                                    <asp:Repeater ID="rptvideochoxuatban" runat="server">
                                        <ItemTemplate>
                                            <div class="nk-notification-item dropdown-inner">
                                                <div class="nk-notification-icon">
                                                    <em class="icon icon-circle bg-warning-dim ni ni-curve-down-right"></em>
                                                </div>
                                                <div class="nk-notification-content">
                                                    <div class="nk-notification-text">
                                                        <asp:HyperLink ID="hptSua" NavigateUrl='<%# "/quan-tri/video-cap-cao/cho-xuat-ban?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "videoid") %>' runat="server" data-toggle="tooltip" data-placement="top" title="" data-original-title="Sửa" Visible='<%# Eval("CanViewLock") %>'>
                                                        <%# Eval("Title")%>
                                                        </asp:HyperLink>
                                                    </div>
                                                    <%--<div class="nk-notification-time"><%#Ultis.ToRelativeDate(Eval("ApprovalRequestDate")) %></div>--%>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <!-- .nk-notification -->
                            </div>
                            <!-- .nk-dropdown-body -->
                            <div class="dropdown-foot center">
                                <a href="/quan-tri/video-cap-cao/cho-xuat-ban">Xem tất cả</a>
                            </div>
                        </div>
                    </li>
                    <li class="dropdown chats-dropdown hide-mb-xs">
                        <asp:HyperLink NavigateUrl="/quan-tri/quan-tri-tin-tuc-cap-cao/duyet-tin-bai" ID="hplchopheduyet" CssClass="dropdown-toggle nk-quick-nav-icon" data-toggle="dropdown" title="Chờ Biên Tập" runat="server" Visible="false">
                            <div class="icon-status icon-status-na">
                                <em class="icon ni ni-edit"></em><span class="icon-status-count">
                                    <asp:Literal ID="ltrchobientap" runat="server"></asp:Literal></span>
                            </div>
                        </asp:HyperLink>
                        <div class="dropdown-menu dropdown-menu-xl dropdown-menu-right">
                            <div class="dropdown-head">
                                <span class="sub-title nk-dropdown-title">Bài chờ Phê duyệt</span>
                            </div>
                            <div class="dropdown-body">
                                <div class="nk-notification">
                                    <asp:Repeater ID="rptchopheduyet" runat="server">
                                        <ItemTemplate>
                                            <div class="nk-notification-item dropdown-inner">
                                                <div class="nk-notification-icon">
                                                    <em class="icon icon-circle bg-warning-dim ni ni-curve-down-right"></em>
                                                </div>
                                                <div class="nk-notification-content">
                                                    <div class="nk-notification-text">
                                                        <asp:HyperLink ID="hptSua" NavigateUrl='<%# "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-duyet-bien-tap?itemid=" & DataBinder.Eval(Container.DataItem, "newid") %>' runat="server" data-toggle="tooltip" data-placement="top" title="" data-original-title="Sửa" Visible='<%# Eval("CanViewLock") %>'>
                                                        <%# Eval("Title")%>
                                                        </asp:HyperLink>
                                                    </div>
                                                    <div class="nk-notification-time"><%#Ultis.ToRelativeDate(Eval("ApprovalRequestDate")) %></div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <!-- .nk-notification -->
                            </div>
                            <!-- .nk-dropdown-body -->
                            <div class="dropdown-foot center">
                                <a href="/quan-tri/quan-tri-tin-tuc-cap-cao/duyet-tin-bai">Xem tất cả</a>
                            </div>
                        </div>
                    </li>
                    <li class="dropdown notification-dropdown">
                        <asp:HyperLink NavigateUrl="/quan-tri/quan-tri-tin-tuc-cap-cao/duyet-xuat-ban" ID="hplchoxuatban" CssClass="dropdown-toggle nk-quick-nav-icon" data-toggle="dropdown" title="Chờ Biên Tập" runat="server" Visible="false">
                            <div class="icon-status icon-status-info">
                                <em class="icon ni ni-bell"></em><span class="icon-status-count">
                                    <asp:Literal ID="ltrchoxuatban" runat="server"></asp:Literal></span>
                            </div>
                        </asp:HyperLink>
                        <div class="dropdown-menu dropdown-menu-xl dropdown-menu-right">
                            <div class="dropdown-head">
                                <span class="sub-title nk-dropdown-title">Bài chờ xuất bản</span>
                            </div>
                            <div class="dropdown-body">
                                <div class="nk-notification">
                                    <asp:Repeater ID="rptchoxuatban" runat="server">
                                        <ItemTemplate>
                                            <div class="nk-notification-item dropdown-inner">
                                                <div class="nk-notification-icon">
                                                    <em class="icon icon-circle bg-warning-dim ni ni-curve-down-right"></em>
                                                </div>
                                                <div class="nk-notification-content">
                                                    <div class="nk-notification-text">
                                                        <asp:HyperLink ID="hptSua" NavigateUrl='<%# "/quan-tri/quan-tri-tin-tuc-cap-cao/cap-nhat/sua-duyet-xuat-ban?itemid=" & DataBinder.Eval(Container.DataItem, "newid") %>' runat="server" data-toggle="tooltip" data-placement="top" title="" data-original-title="Sửa" Visible='<%# Eval("CanViewLock") %>'>
                                                        <%# Eval("Title")%>
                                                        </asp:HyperLink>
                                                    </div>
                                                    <div class="nk-notification-time"><%#Ultis.ToRelativeDate(Eval("ApprovalRequestDate")) %></div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                                <!-- .nk-notification -->
                            </div>
                            <!-- .nk-dropdown-body -->
                            <div class="dropdown-foot center">
                                <a href="/quan-tri/quan-tri-tin-tuc-cap-cao/duyet-xuat-ban">xem tất cả</a>
                            </div>
                        </div>
                    </li>
                    <li class="dropdown user-dropdown">
                        <a href="#" class="dropdown-toggle mr-n1" data-toggle="dropdown">
                            <div class="user-toggle">
                                <div class="user-avatar sm">
                                    <em class="icon ni ni-user-alt"></em>
                                </div>
                            </div>
                        </a>
                        <div class="dropdown-menu dropdown-menu-md dropdown-menu-right">
                            <div class="dropdown-inner user-card-wrap bg-lighter">
                                <div class="user-card">
                                    <div class="user-avatar">
                                        <span>
                                            <asp:Image ID="imgAvtar" runat="server" /></span>
                                    </div>
                                    <div class="user-info">
                                        <span class="lead-text">
                                            <asp:Literal ID="ltrname" runat="server"></asp:Literal></span>
                                        <span class="sub-text">
                                            <asp:Literal ID="ltremail" runat="server"></asp:Literal></span>
                                    </div>
                                </div>
                            </div>
                            <div class="dropdown-inner">
                                <ul class="link-list">
                                    <li><a href="/quan-tri/nguoi-dung/cap-nhat-thong-tin"><em class="icon ni ni-user-alt"></em><span>Sửa thông tin</span></a></li>
                                    <%--<li><a href="#"><em class="icon ni ni-setting-alt"></em><span>Account Setting</span></a></li>
                                    <li><a href="html/user-profile-activity.html"><em class="icon ni ni-activity-alt"></em><span>Login Activity</span></a></li>
                                    <li><a class="dark-switch" href="#"><em class="icon ni ni-moon"></em><span>Dark Mode</span></a></li>--%>
                                </ul>
                            </div>
                            <div class="dropdown-inner">
                                <ul class="link-list">
                                    <li><a href="/dang-nhap/ctl/Logoff"><em class="icon ni ni-signout"></em><span>Thoát</span></a></li>
                                </ul>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <!-- .nk-header-wrap -->
    </div>
    <!-- .container-fliud -->
</div>
<script>
    var currentTime = new Date()
    // returns the month (from 0 to 11)
    var month = currentTime.getMonth() + 1
    // returns the day of the month (from 1 to 31)
    var day = currentTime.getDate()
    // returns the year (four digits)
    var year = currentTime.getFullYear()
    // document.getElementById("pinggoogle").innerHTML = "<a href='https://www.google.com/webmasters/tools/ping?sitemap=https://thuongtruong.com.vn/sitemaps/newslist/"+ year + "-" + month + "-" + day+".xml' target=_blank>ping google</a>";
</script>
