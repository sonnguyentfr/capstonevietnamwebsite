<%@ Control Language="VB" AutoEventWireup="false" CodeFile="menu.ascx.vb" Inherits="DesktopModules.TinTuc.Control.MenuAdmin" %>
<div class="nk-sidebar-bar">
    <div class="nk-apps-brand">
        <a href="/" class="logo-link">
            <img class="logo-light logo-img" src="/static/_admin/images/logo-small.png" srcset="/static/_admin/images/logo-small2x.png 2x" alt="logo">
            <img class="logo-dark logo-img" src="/static/_admin/images/logo-dark-small.png" srcset="/static/_admin/images/logo-dark-small2x.png 2x" alt="logo-dark">
        </a>
    </div>
    <div class="nk-sidebar-element">
        <div class="nk-sidebar-body">
            <div class="nk-sidebar-content" data-simplebar>
                <div class="nk-sidebar-menu">
                    <!-- Menu -->
                    <ul class="nk-menu apps-menu">
                        <asp:Literal ID="ltrmenu" runat="server"></asp:Literal>
                    </ul>
                </div>
                <div class="nk-sidebar-footer">
                    <ul class="nk-menu nk-menu-md apps-menu">
                        <li class="nk-menu-item">
                            <a href="#" class="nk-menu-link" title="Settings">
                                <span class="nk-menu-icon"><em class="icon ni ni-setting"></em></span>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="nk-sidebar-profile nk-sidebar-profile-fixed dropdown">
                <a href="#" data-toggle="dropdown" data-offset="50,-50">
                    <div class="user-avatar">
                        <span><asp:Image ID="imgAvtar" runat="server" /></span>
                    </div>
                </a>
                <div class="dropdown-menu dropdown-menu-md ml-4">
                    <div class="dropdown-inner user-card-wrap d-none d-md-block">
                        <div class="user-card">
                            <div class="user-avatar">
                                <span><asp:Image ID="imgAvtar2" runat="server" /></span>
                            </div>
                            <div class="user-info">
                                <span class="lead-text"><asp:Literal ID="ltrname" runat="server"></asp:Literal></span>
                                <span class="sub-text text-soft"><asp:Literal ID="ltremail" runat="server"></asp:Literal></span>
                            </div>
                        </div>
                    </div>
                    <div class="dropdown-inner">
                        <ul class="link-list">
                            <li><a href="/quan-tri/nguoi-dung/cap-nhat-thong-tin"><em class="icon ni ni-user-alt"></em><span>Sửa thông tin</span></a></li>
                            <li><a href="#"><em class="icon ni ni-setting-alt"></em><span>Account Setting</span></a></li>
                            <li><a href="#"><em class="icon ni ni-activity-alt"></em><span>Login Activity</span></a></li>
                        </ul>
                    </div>
                    <div class="dropdown-inner">
                        <ul class="link-list">
                            <li><a href="/dang-nhap/ctl/Logoff"><em class="icon ni ni-signout"></em><span>Thoát</span></a></li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="nk-sidebar-main is-light">
    <div class="nk-sidebar-inner" data-simplebar>
        <asp:Literal ID="ltrmenusub" runat="server"></asp:Literal>
    </div>
</div>
<script type="text/javascript">
    //$(document).ready(
    //    function () {
    //        $("li.nk-menu-item a.nk-menu-link").click(function () {
    //            $("div.nk-sidebar-main").show("slow");
    //        });

    //    });
    $(document).on('click', function (e) {
        if ($(e.target).closest('li.nk-menu-item a.nk-menu-link').length) {
            $("div.nk-sidebar-main").show("slow");
        } else if (!$(e.target).closest('#theDiv').length) {
            $("div.nk-sidebar-main").hide();
        }
    });
</script>