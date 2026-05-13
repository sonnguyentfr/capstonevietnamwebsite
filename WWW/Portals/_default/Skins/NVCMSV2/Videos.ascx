<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/js.ascx" TagPrefix="uc1" TagName="js" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>

<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<div class="bg-repeat font-family  color-red scolor-red">
    <!--Background image-->
    <div class="bg-image"></div>
    <!-- ========== WRAPPER ========== -->
    <div class="wrapper">
        <!--Header start-->
        <uc1:TopHeader runat="server" ID="TopHeader" />
        <!--End header-->
        <!--Main menu-->
        <uc1:Menu runat="server" ID="Menu" />
        <div id="tags">
            <div class="container">
                <div class="row">
                    <div class="col-12">
                        <div class="row">
                            <!--Breaking box-->
                            <div class="col-12 col-sm-12 col-md-12 col-lg-12 pl-1 pl-md-2">
                                <div class="breaking-tags">
                                    <a class="item" href="/tags.html?tag=gia+ca+phe+hom+nay">#giá cà phê hôm nay
                                    </a>

                                    <a class="item" href="/tags.html?tag=gia+heo+hoi+hom+nay">#giá heo hơi hôm nay
                                    </a>

                                    <a class="item" href="/tags.html?tag=gia+vang+hom+nay">#giá vàng hôm nay
                                    </a>

                                    <a class="item" href="/tags.html?tag=gia+xang+dau+hom+nay">#giá xăng dầu hôm nay
                                    </a>

                                    <a class="item" href="/tags.html?tag=gia+tieu+hom+nay">#giá tiêu hôm nay
                                    </a>

                                    <a class="item" href="/tags.html?tag=ty+gia+ngoai+te+hom+nay">#Tỷ giá ngoại tệ hôm nay
                                    </a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="ContentPane" runat="server"></div>
        <!--Footer start-->
        <uc1:Footer runat="server" ID="Footer" />
        <!-- End Footer -->
    </div>
    <uc1:js runat="server" ID="js" />
</div>
