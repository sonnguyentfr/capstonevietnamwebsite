<%@ Control Language="vb" EnableViewState="false" Inherits="DesktopModules.TinTuc.Control.TopHeader" CodeFile="TopHeader.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<header id="header">
    <div class="container">
        <div class="row">
            <div class="tophear">
                <div class="col-md-3 pl-0">
                    <div class="text-center social">
                        <div class="socilalink">
                            <a href="https://www.facebook.com/thuongtruong.com.vn/" target="_blank"><i class="fa fa-facebook"></i></a>
                            <a href="https://youtube.com/" target="_blank"><i class="fa fa-youtube"></i></a>
                        </div>
                        <div class="date">
                            <div class="clock">
                                <div id="time"></div>
                                <div id="date"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 pl-0">
                    <div class="header-logo">
                        <a href="/">
                            <img src="https://thuongtruong-cdn.nvcms.net/nvcms/img/logo.png?v=1.1" alt="Báo điện tử Thương Trường" />
                        </a>
                    </div>
                </div>
                <div class="col-md-3 pl-0"></div>
            </div>
            <nav class="navbar navbar-default" id="mobile-nav">
                <div class="navbar-header">
                    <button type="button" class="navbar-toggle" data-toggle="collapse" id="sidenav-toggle">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <div class="sidenav-header-logo">
                        <a href="/">
                            <img src="https://thuongtruong-cdn.nvcms.net/nvcms/img/mlogo.png?v=1" alt="Tạp chí Thương Trường" />
                        </a>
                    </div>

                    <button type="button" class="search-toggle" data-toggle="collapse" data-target="#search-toggle" aria-expanded="false" aria-controls="search-toggle">
                        <i class="fa fa-search"></i>
                    </button>
                </div>
                <div class="sidenav" data-sidenav data-sidenav-toggle="#sidenav-toggle">
                    <button type="button" class="navbar-toggle active" data-toggle="collapse">
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                        <span class="icon-bar"></span>
                    </button>
                    <div class="sidenav-brand">
                        <div class="sidenav-header-logo">
                            <a href="/">
                                <img src="https://thuongtruong-cdn.nvcms.net/nvcms/img/mlogo.png?v=1" alt="Tạp chí Thương Trường" />
                            </a>
                        </div>
                    </div>
                    <ul class="sidenav-menu">
                        <asp:Literal ID="ltrMenusiderebar" runat="server"></asp:Literal>
                    </ul>
                </div>
                <div class="collapse mobileserch" id="search-toggle">
                    <div class="card card-body">

                        <div class="search-inputm">
                            <input type="search" id="seach-boxm" class="search-bar" placeholder="Tìm kiếm" title="Search">
                        </div>
                        <div class="search-icon-btnm" onclick="doSearchSitem(); return false;">
                            <span style="cursor: pointer"><i class="fa fa-search"></i></span>
                        </div>
                    </div>
                </div>
            </nav>
        </div>

    </div>
    <!--========== BEGIN .NAVBAR #FIXED-NAVBAR ==========-->
    <div class="navbar" id="fixed-navbar">
        <!--========== BEGIN MAIN-MENU .NAVBAR-COLLAPSE COLLAPSE #FIXED-NAVBAR-TOOGLE ==========-->
        <div class="main-menu nav navbar-collapse collapse" id="fixed-navbar-toggle">
            <!--========== BEGIN .CONTAINER ==========-->
            <div class="container">
                <!-- Begin .nav navbar-nav -->
                <uc1:Menu runat="server" ID="Menu" />
                <!--========== END .NAV NAVBAR-NAV ==========-->
            </div>
            <!--========== END .CONTAINER ==========-->
        </div>
        <!--========== END MAIN-MENU .NAVBAR-COLLAPSE COLLAPSE #FIXED-NAVBAR-TOOGLE ==========-->
        <!--========== BEGIN .SECOND-MENU NAVBAR #NAV-BELOW-MAIN ==========-->
        <div class="second-menu navbar" id="nav-below-main">
            <!-- Begin .container -->
            <div class="container">
                <!-- Begin .clock -->
                <div class="breaking-tags">
                    <a class="item" href="/tags.html?tag=gia+ca+phe+hom+nay">#giá cà phê hôm nay
                    </a>
                    <a class="item" href="/tag.html?tag=gia+heo+hoi+hom+nay">#giá heo hơi hôm nay
                    </a>
                    <a class="item" href="/tags.html?tag=gia+vang+hom+nay">#giá vàng hôm nay
                    </a>
                    <a class="item" href="/tags.html?tag=gia+xang+dau+hom+nay">#giá xăng dầu hôm nay
                    </a>
                    <a class="item" href="/tag.html?tag=gia+tieu+hom+nay">#giá tiêu hôm nay
                    </a>
                    <a class="item" href="/tags.html?tag=ty+gia">#Tỷ giá ngoại tệ hôm nay
                    </a>
                    <a class="item" href="/tag.html?tag=gia+gas+hom+nay">#giá gas</a>
                    <a class="item" href="/tag.html?tag=gia+cao+su">#giá cao su</a>
                    <a class="item" href="/tin-tuc-24h">#thương trường 24h</a>
                </div>
                <!-- Begin .collapse navbar-collapse -->
                <div class="collapse navbar-collapse nav-below-main">
                    <!-- Begin .nav navbar-nav -->
                    <ul class="nav navbar-nav">
                        <li>
                            <div class="search-container">
                                <div class="search-icon-btn" onclick="doSearchSite(); return false;">
                                    <span style="cursor: pointer"><i class="fa fa-search"></i></span>
                                </div>
                                <div class="search-input">
                                    <input type="search" id="seach-box" class="search-bar" placeholder="Tìm kiếm" title="Search">
                                </div>
                            </div>
                        </li>
                    </ul>
                    <!-- End .nav navbar-nav -->
                </div>
                <!-- End .collapse navbar-collapse -->

                <!-- End .clock -->
            </div>
            <!-- End .container -->
        </div>
        <!--========== END .SECOND-MENU NAVBAR #NAV-BELOW-MAIN ==========-->
    </div>
</header>
<script type="application/ld+json">
    {
        "@context": "http://schema.org",
        "@type": "WebSite",
        "name": "Tạp chí điện tử Thương trường",
        "alternateName": "tạp chí thương trường, tin tức nhanh nhất, chính trị, xã hội, kinh tế, Thị trường, tài chính, doanh nghiệp, doanh nhân, bất động sản, hàng hoá, tiêu dùng,ngân hàng, thương mại, đầu tư.",
        "url": "https://thuongtruong.com.vn/",
        "potentialAction": {
            "@type": "SearchAction",
            "target": "https://thuongtruong.com.vn/tim-kiem?q={searchKeyword}",
            "query-input": "required name=searchKeyword"
        }
    }
</script>
