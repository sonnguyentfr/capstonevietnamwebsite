<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/footerjs.ascx" TagPrefix="uc2" TagName="footerjs" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Footer.ascx" TagPrefix="uc2" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/CatName.ascx" TagPrefix="uc1" TagName="CatName" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/banner_detail.ascx" TagPrefix="uc1" TagName="banner_detail" %>
<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<uc1:TopHeader runat="server" ID="TopHeader" />
<uc1:Menu runat="server" ID="Menu" />
<section class="mt-4 mb-10">
    <div class="container">
        <div class='quangcaogoogle' style='text-align: center;'>
            <!-- PC Ngang NewsIndex -->
            <ins class="adsbygoogle"
                style="display: block"
                data-ad-client="ca-pub-3311450421751656"
                data-ad-slot="1244174619"
                data-ad-format="auto"
                data-full-width-responsive="true"></ins>
            <script>
                (adsbygoogle = window.adsbygoogle || []).push({});
            </script>

        </div>
    </div>
</section>
<section class="mt-4 mt-lg-5 quochoi">
    <div class="container">
        
        <uc1:banner_detail runat="server" ID="banner_detail" />
    </div>
</section>
<section class="mt-4 mb-10">
    <div class="container">
        <div class='quangcaogoogle' style='text-align: center;'>
            <!-- PC Ngang NewsIndex -->
            <ins class="adsbygoogle"
                style="display: block"
                data-ad-client="ca-pub-3311450421751656"
                data-ad-slot="1244174619"
                data-ad-format="auto"
                data-full-width-responsive="true"></ins>
            <script>
                (adsbygoogle = window.adsbygoogle || []).push({});
            </script>

        </div>
    </div>
</section>
<div id="ContentPane" runat="server"></div>
<uc2:Footer runat="server" ID="Footer" />
<uc2:footerjs runat="server" ID="footerjs" />
