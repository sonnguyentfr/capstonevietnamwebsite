<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/headercss.ascx" TagPrefix="uc1" TagName="headercss" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/js.ascx" TagPrefix="uc1" TagName="js" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/menu.ascx" TagPrefix="uc1" TagName="menu" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/footer.ascx" TagPrefix="uc1" TagName="footer" %>
<%@ Register Src="~/Portals/_default/Skins/_admin/controls/header.ascx" TagPrefix="uc1" TagName="header" %>

<uc1:headercss runat="server" ID="headercss" />

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
                                <div id="ContentPane" runat="server"></div>
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
    <uc1:js runat="server" ID="js" />
</div>
<uc1:footer runat="server" ID="footer" />
