<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/DesktopModules/TinTuc/Display/News/BanIn.ascx" TagPrefix="uc1" TagName="BanIn" %>
<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<div>
    <div id="wrapper">
        <div id="main-content">
            <uc1:BanIn runat="server" id="BanIn" />
            <div id="ContentPane" runat="server"></div>
        </div>
       
    </div>
</div>
