<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/footerjs.ascx" TagPrefix="uc2" TagName="footerjs" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Footer.ascx" TagPrefix="uc2" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/CatName.ascx" TagPrefix="uc1" TagName="CatName" %>

<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<uc1:TopHeader runat="server" ID="TopHeader" />
<uc1:Menu runat="server" ID="Menu" />
<section class="my-3 d-lg-block">
    <div class="container text-center">
        <div class='quangcaogoogle' style='text-align: center;'>
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
<section class="mt-4 mt-lg-4">
    <div class="container">
        <div class="tmp-header-2 bd_none">
            <uc1:CatName runat="server" ID="CatName" />

        </div>
        <div class="row-custom mt-3">
            <div class="col-left-8">
                <div class="module-tmp-1">
                    <div id="ContentPane" runat="server"></div>
                    <!-- banner QC -->
                    <section class="my-4 d-none d-md-block">
                        <div class="container text-center">
                            <ins class="adsbygoogle"
                                style="display: block; text-align: center;"
                                data-ad-layout="in-article"
                                data-ad-format="fluid"
                                data-ad-client="ca-pub-3311450421751656"
                                data-ad-slot="1792804105"></ins>
                            <script>
                                (adsbygoogle = window.adsbygoogle || []).push({});
                            </script>
                        </div>
                    </section>
                </div>
            </div>
            <!-- col right -->
            <div class="col-right-4 d-none d-md-block">
                <div id="RightPane" runat="server"></div>
                <div class="box-banner-sticky">
                    <div class="box-quangcao">
                        <div class="box-indiv-tmp-1 text-center">
                            <!-- PC.300x600 -->
                            <ins class="adsbygoogle"
                                style="display: inline-block; width: 300px; height: 600px"
                                data-ad-client="ca-pub-3311450421751656"
                                data-ad-slot="8358212457"></ins>
                            <script>
                                (adsbygoogle = window.adsbygoogle || []).push({});
                            </script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<uc2:Footer runat="server" ID="Footer" />
<uc2:footerjs runat="server" ID="footerjs" />
