<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/BreadCrumb.ascx" TagPrefix="uc1" TagName="BreadCrumb" %>
<%--<%@ Register Src="~/DesktopModules/TinTuc/Control/Home/TinNong.ascx" TagPrefix="uc1" TagName="TinNong" %>--%>
<%--<%@ Register Src="~/DesktopModules/TinTuc/Control/XemTruoc.ascx" TagPrefix="uc1" TagName="XemTruoc" %>--%>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Manager/Xemtruoc/Detail.ascx" TagPrefix="uc1" TagName="Detail" %>

<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<div id="wrapper">
    <uc1:TopHeader runat="server" ID="TopHeader" />
    <div id="trending" class="std">
        <div class="inner">
            <div class="relative">
                <h3 class="title">
                    <span>Tin nóng<i class="fa fa-bolt"></i></span>		</h3>
                <div class="penci-block_content">
                    <%--<uc1:TinNong runat="server" ID="TinNong" />--%>
                </div>
            </div>
            <div class="cl"></div>
        </div>
    </div>
    <%--<uc1:XemTruoc runat="server" ID="XemTruoc" />--%>
    <uc1:Detail runat="server" id="Detail" />
    <div id="ContentPane" runat="server"></div>
    <uc1:Footer runat="server" ID="Footer" />
</div>
