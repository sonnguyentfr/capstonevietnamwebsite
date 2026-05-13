<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/footerjs.ascx" TagPrefix="uc1" TagName="footerjs" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<div id="wrapper" data-color="red">
    <uc1:TopHeader runat="server" ID="TopHeader" />
    <section id="main-section">
        <section class="module">
            <div class="container">
                <div id="TopPane" runat="server"></div>
            </div>
        </section>
        <section class="bannercenter">
            <div class="container">
                <div class="row">
                    <div class="col-sm-6 col-md-6 pl-0">
                        <div id="BannerTopLeft" runat="server"></div>
                    </div>
                    <div class="col-sm-6 col-md-6 pr-0">
                        <div id="BannerTopRight" runat="server"></div>
                    </div>
                </div>
            </div>
        </section>
        <section class="module">
            <div class="container">
                <div id="ContentPane" runat="server"></div>
            </div>
        </section>
        <!--========== END .MODULE ==========-->
        <!--========== BEGIN .MODULE ==========-->
        <section class="module">
            <div class="container">
                <div class="row no-gutter">
                    <!--========== BEGIN .COL-MD-8 ==========-->
                    <div class="col-md-9" style="padding-right: 0px;">
                        <div id="LeftTopPane" runat="server"></div>
                    </div>
                    <!--========== End .COL-MD-8 ==========-->
                    <!--========== BEGIN .COL-MD-4 ==========-->
                    <div class="col-md-3">
                        <div id="RightTop" runat="server"></div>
                    </div>
                    <!--========== END .COL-MD-4 ==========-->
                </div>
            </div>
        </section>
        <section class="module dark" style="background: #ffde7652 !important;border-top: solid 2px #d4000e;border-bottom: solid 2px #d4000e; display:none !important">
            <div class="container">
                <%--<uc1:Videomoinhat runat="server" ID="Videomoinhat" count="8" />--%>
				<div class="col-md-9" style="padding-right: 0px;">
					
				</div>
				<!--========== End .COL-MD-8 ==========-->
				<!--========== BEGIN .COL-MD-4 ==========-->
				<div class="col-md-3">
					<div class='quangcaogoogle' style='text-align:center;'>
						<!-- PC.300x600 -->
						<ins class="adsbygoogle"
							 style="display:inline-block;width:300px;height:600px"
							 data-ad-client="ca-pub-3311450421751656"
							 data-ad-slot="8358212457"></ins>
						<script>
							 (adsbygoogle = window.adsbygoogle || []).push({});
						</script>
						
					</div>
				</div>
            </div>
        </section>
        <!--========== BEGIN .MODULE ==========-->
        <section class="module highlight">
            <div class="container">
                <div class="row no-gutter">
                    <!--========== BEGIN .COL-MD-8 ==========-->
                    <div class="col-md-9">
                        <div class="row">
                            <div class="col-md-12" style="padding: 0px">
                                <div id="BottomPane" runat="server"></div>
                                
                            </div>
                            <div class="col-md-8" style="padding: 0px">
                                <div id="LeftDuoiPane" runat="server"></div>
                            </div>
                            <div class="col-md-4" style="padding-right: 0px">
                                <div id="GiuaDuoiPane" runat="server"></div>
                                <div class="quangcaogoogle no-mobile " style="margin-top: 10px; text-align: center">
                                    <!-- PC.250x250 -->
                                    <ins class="adsbygoogle"
                                        style="display: inline-block; width: 250px; height: 250px"
                                        data-ad-client="ca-pub-3311450421751656"
                                        data-ad-slot="9888899470"></ins>
                                    <script>
                                        (adsbygoogle = window.adsbygoogle || []).push({});
                                    </script>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--========== BEGIN COL-MD-4 ==========-->
                    <div class="col-md-3">
                        <div id="RightDuoiPane" runat="server"></div>
                    </div>
                    <!--========== END .COL-MD-4 ==========-->
                </div>
            </div>
        </section>
        <section class="module ketnoithuonghieuz">
            <!--========== BEGIN.CONTAINER ==========-->
            <div class="container">
                <!--========== BEGIN .ROW ==========-->
                <div class="row no-gutter">
                    <!--========== BEGIN .C0L-MD-8 ==========-->
                    <div class="col-md-12">
                        <div id="KetnoiThuongHieuPane" runat="server"></div>
                    </div>
                </div>
            </div>
        </section>
    </section>
    <uc1:Footer runat="server" ID="Footer" />
</div>
<!--========== END #WRAPPER ==========-->
<!-- External JavaScripts -->
<uc1:footerjs runat="server" ID="footerjs" />


