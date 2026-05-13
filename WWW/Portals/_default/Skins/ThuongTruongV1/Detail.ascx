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
                            style="display: inline-block; width: 970px; height: 90px"
                            data-ad-client="ca-pub-3311450421751656"
                            data-ad-slot="8429530491"></ins>
                        <script>
                            (adsbygoogle = window.adsbygoogle || []).push({});
                        </script>
                    </div>
                </div>
                <div class="row no-gutter breadcrumb">
                    <uc1:BreadCrumb runat="server" ID="BreadCrumb" />
                </div>
                <div id="ContentPane" runat="server"></div>
                <div class="row newsindex">
                    <!--========== BEGIN .COL-MD-8 ==========-->
                    <div class="col-md-9 pr-0">
                        <uc1:CatName runat="server" id="CatName" />
                        <div id="LeftTopPane" runat="server"></div>
						
                        <!--========== BEGIN .ROW ==========-->
                        <div class="row">
                            <div class="col-xs-12 col-sm-9 col-md-9 ">
								
                                <!--========== BEGIN .NEWS ==========-->
                                <div id="LeftPane" runat="server"></div>
								
                                <!--========== END .NEWS ==========-->
                            </div>
                            <div class="col-xs-12 col-sm-3 col-md-3 pr-0 pl-0">
								<div class='quangcaogoogle no-mobile' style='text-align:center;'>
									<!-- QC Gooogle Index.HotCat -->
								
									<!-- PC.160x600 -->
									<ins class="adsbygoogle"
										 style="display:inline-block;width:160px;height:600px"
										 data-ad-client="ca-pub-3311450421751656"
										 data-ad-slot="9714707914"></ins>
									<script>
										 (adsbygoogle = window.adsbygoogle || []).push({});
									</script>
								</div>
                                <div id="GiuaPane" runat="server"></div><br />
								<div class='sidebar-fixed4 quangcaogoogle' style='text-align:center;'>
									<!-- QC Gooogle Index.HotCat -->
										<!-- PC.160x600 -->
										<ins class="adsbygoogle"
											 style="display:inline-block;width:160px;height:600px"
											 data-ad-client="ca-pub-3311450421751656"
											 data-ad-slot="9714707914"></ins>
										<script>
											 (adsbygoogle = window.adsbygoogle || []).push({});
										</script>
									</div>
                            </div>
                        </div>
                        <!--========== END .ROW ==========-->
                    </div>
                    <!--========== END .COL-MD-8 ==========-->
                    <!--========== BEGIN .COL-MD-4 ==========-->
                    <div class="col-md-3">
                        <div id="RightPane" runat="server"></div>
						
						<div class='sidebar-fixed3 quangcaogoogle' style='text-align:center;'>
							<!-- PC.300x600 -->
							<ins class="adsbygoogle"
								 style="display:inline-block;width:300px;height:600px"
								 data-ad-client="ca-pub-3311450421751656"
								 data-ad-slot="8358212457"></ins>
							<script>
								 (adsbygoogle = window.adsbygoogle || []).push({});
							</script>
							
							<%--<!-- ADOP -->
							<ins class='adsbyadop' _adop_zon = 'a8ca3a54-852c-4fd6-9002-2c8a5b1f60af' _adop_type = 're' style='display:inline-block;width:300px;height:600px;margin-top:10px;' _page_url=''></ins>
							<!------>--%>
						</div>
                    </div>
                    <div class="col-md-12 no-gutter mt-20 videomoinhatz">
                        <div id="VideoPane" runat="server"></div>
                    </div>
                    <!--========== END .COL-MD-4 ==========-->
                </div>
            </div>
        </section>
    </section>
    <uc1:Footer runat="server" ID="Footer" />
</div>
<uc1:footerjs runat="server" ID="footerjs" />
