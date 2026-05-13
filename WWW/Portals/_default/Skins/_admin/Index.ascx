<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/menu.ascx" TagPrefix="uc1" TagName="menu" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/header.ascx" TagPrefix="uc1" TagName="header" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/footer.ascx" TagPrefix="uc1" TagName="footer" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/index/U_NhuanBut.ascx" TagPrefix="uc1" TagName="U_NhuanBut" %>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css">
<link rel="stylesheet" href="/static/_admin/assets/css/dashlite.css?ver=2.3.0">
<link id="skin-default" rel="stylesheet" href="/static/_admin/assets/css/theme.css?ver=2.3.0">
<link href="/static/_admin/assets/css/jquery.fancybox.min.css" rel="stylesheet" />
<link href="/static/_admin/js/datepicker/jquery.datetimepicker.min.css" rel="stylesheet" />
<script src="/static/_admin/assets/js/bundle.js?ver=2.3.0"></script>
<div class="nk-body ui-rounder npc-default has-sidebar ">
    <div class="nk-app-root">
        <div class="nk-sidebar" data-content="sidebarMenu">
            <uc1:menu runat="server" ID="menu" />
        </div>
        <!-- main @s -->
        <div class="nk-main ">
            <!-- wrap @s -->
            <div class="nk-wrap ">
                <!-- main header @s -->
                <uc1:header runat="server" ID="header" />
                <!-- main header @e -->
                <!-- content @s -->
                <div class="nk-content ">
                    <div class="container-fluid">
                        <div class="nk-content-inner">
                            <div class="nk-content-body">
                                <div class="nk-block-head nk-block-head-sm">
                                    <div class="nk-block-between">
                                        <div class="nk-block-head-content">
                                            <h3 class="nk-block-title page-title">NVCMS Dashboard</h3>
                                            <div class="nk-block-des text-soft">
                                                <p>Bảng thống kê thông tin NVCMS</p>
                                            </div>
                                        </div>
                                        <!-- .nk-block-head-content -->
                                    </div>
                                    <!-- .nk-block-between -->
                                </div>
                                <!-- .nk-block-head -->
                                <uc1:U_NhuanBut runat="server" ID="U_NhuanBut" />
                                <!-- .nk-block -->
                            </div>
                        </div>
                    </div>
                </div>
                <!-- content @e -->
            </div>
            <!-- wrap @e -->
        </div>
        <!-- main @e -->
    </div>
    <script src="/static/_admin/assets/js/scripts.js?ver=2.3.1"></script>
    <script src="/static/_admin/assets/js/jquery.fancybox.min.js"></script>
    <%--datepicker--%>
    <script src="/static/_admin/js/datepicker/jquery.datetimepicker.full.min.js"></script>
    <script src="/static/_admin/js/autoNumeric.js"></script>
    <script src="/static/_admin/js/nvcmsinit.js"></script>
</div>
<uc1:footer runat="server" ID="footer" />
