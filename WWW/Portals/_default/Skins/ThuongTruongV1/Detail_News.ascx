<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/footerjs.ascx" TagPrefix="uc1" TagName="footerjs" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/BreadCrumb.ascx" TagPrefix="uc1" TagName="BreadCrumb" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/CatName.ascx" TagPrefix="uc1" TagName="CatName" %>
<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<!--========== BEGIN #WRAPPER ==========-->
<div id="wrapper" data-color="red">
    <uc1:TopHeader runat="server" ID="TopHeader" />
    <section id="main-section">
        <section class="module tinnong">
            <div class="container">
                <div id="TopPane" runat="server"></div>
            </div>
        </section>
        <section class="module">
            <div class="container">
                <div class="row">
                    <div class='quangcaogoogle' style='text-align: center;'>
                        <!-- PC.970x90 -->
						<ins class="adsbygoogle"
							 style="display:block"
							 data-ad-client="ca-pub-3311450421751656"
							 data-ad-slot="8429530491"
							 data-ad-format="auto"
							 data-full-width-responsive="true"></ins>
						<script>
							 (adsbygoogle = window.adsbygoogle || []).push({});
						</script>
                    </div>
                </div>
                <div class="row no-gutter breadcrumb">
                    <uc1:BreadCrumb runat="server" ID="BreadCrumb" />
                </div>
                <div id="ContentPane" runat="server"></div>
            </div>
        </section>
    </section>
    <uc1:Footer runat="server" ID="Footer" />
</div>
<uc1:footerjs runat="server" ID="footerjs" />
