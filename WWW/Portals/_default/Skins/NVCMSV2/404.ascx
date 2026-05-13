<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/footerjs.ascx" TagPrefix="uc2" TagName="footerjs" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Footer.ascx" TagPrefix="uc2" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<meta name="robots" content="noindex, nofollow">
<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<uc1:TopHeader runat="server" ID="TopHeader" />
<uc1:Menu runat="server" ID="Menu" />
<section class="my-3 d-lg-block">
    <div class="container text-center">
        <div class='quangcaogoogle' style='text-align: center; width:100%'>
			<!-- PC Ngang NewsIndex -->
			<ins class="adsbygoogle"
				 style="display:block"
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
		
        <div id="ContentPane" runat="server"></div>
    </div>
</section>
<section class="section_top mt-4">
    <div class="container">
        <div class="row">
            <div class="col-lg-6 order-lg-2">
                <div id="TopPane" runat="server"></div>
            </div>
            <div class="col-md-6 col-lg-3 mt-4 mt-lg-0 order-lg-1">
                <div id="TopLeftPane" runat="server"></div>
            </div>
            <div class="col-md-6 col-lg-3 mt-4 mt-lg-0 order-lg-3">
                <div id="TopRightPane" runat="server"></div>
				
            </div>
        </div>
    </div>
</section>
<section class="my-3 d-none d-md-block">
    <div class="container text-center">
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
</section>
<section class="my-3 d-none d-md-block">
    <div class="container text-center">
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
</section>
<section class="mt-4 mt-lg-5">
    <div class="container">
        <div id="KetNoiThuongHieuPane" runat="server"></div>
        
    </div>
</section>
<uc2:Footer runat="server" ID="Footer" />
<uc2:footerjs runat="server" ID="footerjs" />

