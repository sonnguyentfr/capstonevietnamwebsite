<%@ Control Language="vb" EnableViewState="false" Inherits="DesktopModules.TinTuc.Control.HeaderNews" CodeFile="Menu.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%--<ul class="nav navbar-nav">
    <asp:Literal ID="ltrMenu" runat="server"></asp:Literal>
</ul>--%>
<section id="wrap-scroll" class="wrap-main-nav">
    <div class="mb-header d-lg-none d-flex align-items-center justify-content-between w-100">
        <a class="navbar-brand logo-absolute" href="/">
            <img src="https://thuongtruong-cdn.nvcms.net/nvcmsv2/images/logo-red.png?v=1" alt="Tạp chí điện tử Thương Trường"></a>
        <div class="btn-group ms-auto me-4">
            <span class="btn-dropdown" data-bs-toggle="collapse" data-bs-target="#collapseSearch" aria-expanded="false" aria-controls="collapseSearch">
                <i class="fa fa-search" style="color:#fff" aria-hidden="true"></i>
            </span>
        </div>
        <a href="#" class="navbar-toggler btn_toggle collapsed" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="ic"><span></span><span></span><span></span></span>
        </a>
    </div>

    <div class="box-all-nav">
        <nav class="navbar navbar-expand-lg">
            <div class="w-100">
                <div class="collapse navbar-collapse tt-border-bottom" id="navbarNav">
                    <div class="container">
                        <ul class="navbar-nav justify-content-between align-items-center lh-1 w-100">
                            <li class="nav-item d-none d-lg-block">
                                <a class="nav-link text-uppercase active" href="/">
                                    <img src="/static/nvcmsv2/images/ic-home.png" alt=""></a>
                            </li>
                            <asp:Literal ID="ltrMenu" runat="server"></asp:Literal>
                            
                        </ul>

                        <div class="d-lg-none ft-s-arl">
                            <div class="d-flex justify-content-center mt-3">
                                <a class="me-2" href="#">
                                    <i class="fa fa-facebook-square" aria-hidden="true"></i></a>
                                <a class="" href="#">
                                    <i  class="fa fa-youtube-play" aria-hidden="true"></i></a>
                            </div>

                            <div class="row p-2 bg-red my-3">
                                <div class="d-flex justify-content-between">
                                    <a class="text-white" href="#">Giới thiệu</a>
                                    <a class="text-white" href="#">Liên hệ</a>
                                </div>
                            </div>
                            <div class="txt-red text-center mb-5">Mọi hình thức sao chép phải được sự chấp thuận bằng văn bản của Tạp chí Thương Trường</div>
                        </div>
                    </div>
                </div>
            </div>
        </nav>

        <div class="sub-nav-folder mt-lg-3 mb-2">
            <div class="container">
                <div class="nav-folder-home">
                    <div class="d-flex">
                        <div class="heading-tags pb-lg-1">
                            <a href="/tags.html?tag=gia+ca+phe+hom+nay" class="active">Giá cà phê hôm nay</a>
                            <a href="/tags.html?tag=gia+heo+hoi+hom+nay">Giá heo hơi hôm nay</a>
                            <a href="/tags.html?tag=gia+vang+hom+nay">Giá vàng hôm nay</a>
                            <a href="/tags.html?tag=gia+xang+dau+hom+nay">Giá xăng hôm nay</a>
                            <a href="/tags.html?tag=gia+tieu+hom+nay">Giá tiêu hôm nay</a>
                            <a href="/tags.html?tag=ty+gia">Giá ngoại tệ hôm nay</a>
                            <a href="/tags.html?tag=gia+gas+hom+nay">Giá gas</a>
                            <a href="/tag.html?tag=gia+cao+su">Giá cao su</a>
                            <a href="/tin-tuc-24h">Thương trường 24h</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- search mobile -->
    <div class="sp-box-search collapse" id="collapseSearch">
        <div class="search">
            <div class="input-group">
                <input type="text" class="form-control" placeholder="Nội dung cần tìm..." id="seach-boxm" onkeypress="return checkKeypressSearchTopm(event)">
                <div class="icon-btn-search" onclick="doSearchSitem(); return false;">
                    <i class="fa fa-search" aria-hidden="true"></i>
                </div>
            </div>
        </div>
    </div>
</section>
