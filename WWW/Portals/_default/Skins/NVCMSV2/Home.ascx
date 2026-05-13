<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/footerjs.ascx" TagPrefix="uc2" TagName="footerjs" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Footer.ascx" TagPrefix="uc2" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<uc1:TopHeader runat="server" ID="TopHeader" />
<uc1:Menu runat="server" ID="Menu" />
<%--<section class="my-3 d-lg-block">
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
</section>--%>

<section class="section_top mt-4">
    <div class="container">
        <div class="row">

            <div class="col-md-8 col-lg-8 mt-4 mt-lg-0 order-lg-1">
                <div id="TopPane" runat="server"></div>
            </div>
            <div class="col-md-4 col-lg-4 mt-4 mt-lg-0 order-lg-2">
                <div id="TopLeftPane" runat="server"></div>
            </div>
        </div>
    </div>
</section>
<section class="mt-4 mt-lg-5 quochoi">
    <div class="container">
        <div id="QuocHoiPane" runat="server"></div>
    </div>
</section>
<section class="my-3 mt-4 d-none d-md-block">
    <div class="container text-center">
        <div class="row">
            <div class="col-lg-6 order-lg-2">
                <div id="BannerTopLeft" runat="server"></div>
            </div>
            <div class="col-md-6">
                <div id="BannerTopRight" runat="server"></div>

            </div>
        </div>
    </div>
</section>

<section class="mt-4 mt-lg-5">
    <div class="container">
        <div class="row-custom">
            <div class="col-left-8">
                <div id="ContentPane" runat="server"></div>
                <div id="TopRightPane" runat="server"></div>
            </div>
            <div class="col-right-4 d-none d-md-block">
                <div class="box-banner-sticky">
                    <div id="RightTop1Pane" runat="server"></div>
                    <%--<div class="box-quangcao">
					
                        <div class="box-indiv-tmp-1 text-center">
                            <a class="d-block" href="#">
                                <img src="/static/nvcmsv2/images/banner-36-1.png" alt="300x600">
                            </a>
                        </div>
                        <div class="box-indiv-tmp-1 text-center">
                            <a class="d-block" href="#">
                                <img src="/static/nvcmsv2/images/qc-300x100.png" alt="300x100">
                            </a>
                        </div>
                    </div>--%>
                </div>
            </div>
        </div>
    </div>
</section>

<section class="mt-4 mt-lg-5 covid">
    <div class="container">
        <div id="CoVidPane" runat="server"></div>
    </div>
</section>

<section class="my-3 d-none d-md-block">
    <div class="container text-center">
        <!-- PC.970x90 -->
        <ins class="adsbygoogle"
            style="display: block"
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
        <div class="row-custom">
            <div class="col-left-8">
                <div id="DoanhNghiepPane" runat="server"></div>
            </div>
            <div class="d-none d-md-block col-right-4 bd-left mt-4 mt-md-0">
                <div id="HiepHoiPane" runat="server"></div>

            </div>
        </div>
    </div>
</section>

<section class="mt-4 mt-lg-5">
    <div class="container">
        <div id="DoiSongTieuDungPane" runat="server"></div>
    </div>
</section>

<section class="mt-4 mt-lg-5 gocnhin">
    <div class="container">
        <div class="row-custom">
            <div class="col-left-8">
                <div id="GocNhinPane" runat="server"></div>
            </div>
            <div class="col-right-4 bd-left mt-4 mt-md-0">
                <div id="ChinhSachPhapLuatPane" runat="server"></div>
            </div>
        </div>
    </div>
</section>

<section class="mt-4 mt-lg-5 d-md-none">
    <div class="container">
        <div class="module-tmp-1">
            <div id="HiepHoiMobilePane" runat="server"></div>

        </div>
    </div>
</section>
<section class="my-3 d-none d-md-block">
    <div class="container text-center">
        <!-- PC.970x90 -->
        <ins class="adsbygoogle"
            style="display: block"
            data-ad-client="ca-pub-3311450421751656"
            data-ad-slot="8429530491"
            data-ad-format="auto"
            data-full-width-responsive="true"></ins>
        <script>
            (adsbygoogle = window.adsbygoogle || []).push({});
        </script>
    </div>
</section>
<%--
<section class="mt-4 mt-lg-5">
    <div class="container">
        <div class="row">
            <div class="col-lg-6">
                <div id="VanHoaXaHoiPane" runat="server"></div>
            </div>
            <div class="col-lg-6 mt-4 mt-lg-0">
                <div class="module-tmp-3">
                    <div class="tmp-title-header bd_none d-flex justify-content-between">
                        <h2 class="title-clamp m-0"><a href="#">Media</a></h2>
                    </div>
                    <div class="box-content mt-3">
                        <div class="item-main">
                            <a class="item-image" href="#">
                                <img src="/static/nvcmsv2/images/image-medium-16.png" alt="video"></a>
                            <h3 class="title-clamp-26 title-tt-icon mt-3">
                                <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                                    viewBox="0 0 30.8 23.2" xml:space="preserve">
                                    <g>
                                        <defs>
                                            <rect id="SVGID_1_" width="30.8" height="23.2" />
                                        </defs>
                                        <clipPath id="SVGID_2_">
                                            <use xlink:href="#SVGID_1_" style="overflow: visible;" />
                                        </clipPath>
                                        <path class="st0" d="M30,0H0.8C0.3,0,0,0.4,0,0.7v21.7c0,0.5,0.3,0.7,0.8,0.7H30c0.5,0,0.8-0.2,0.8-0.7V0.7C30.8,0.4,30.5,0,30,0
                                                M21.8,12.1L21.8,12.1l-10.4,5.7c-0.1,0.1-0.2,0.1-0.5,0.1c-0.1,0-0.3,0-0.5-0.1c-0.4-0.2-0.5-0.5-0.5-0.9V6.3c0-0.4,0.1-0.7,0.5-1
                                                c0.3-0.1,0.6-0.1,1,0l10.4,4.8c0.4,0.3,0.6,0.6,0.6,1C22.5,11.5,22.2,11.9,21.8,12.1 M11.7,8L11.7,8l7.2,3.4l-7.2,3.9V8z" />
                                    </g>
                                </svg>
                                <a href="#">Nóng Sài Gòn: Thông tin mới nhất về số bệnh nhân COVID-19 xuất viện ở TPHCM</a>
                            </h3>
                        </div>
                        <div class="mt-4">
                            <div class="slider-2-items">
                                <div class="item">
                                    <div class="bg-white border">
                                        <div class="mask-box">
                                            <a class="item-image" href="#">
                                                <img src="/static/nvcmsv2/images/image-medium-11.png" alt="video"></a>
                                            <div class="tt-icon">
                                                <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                                                    viewBox="0 0 41.5 28.3" xml:space="preserve">
                                                    <g>
                                                        <defs>
                                                            <rect id="SVGID_1_" width="41.5" height="28.3" />
                                                        </defs>
                                                        <clipPath id="SVGID_2_">
                                                            <use xlink:href="#SVGID_1_" style="overflow: visible;" />
                                                        </clipPath>
                                                        <path class="st0" d="M5.5,0.4C4.6,0.4,3.9,1.1,3.9,2v0.6h7.4V2c0-0.9-0.7-1.6-1.6-1.6H5.5z" />
                                                        <path class="st0" d="M16.3,11.7c-1.1,1.1-1.8,2.7-1.8,4.4c0,1.7,0.7,3.3,1.8,4.4c1.1,1.1,2.7,1.8,4.4,1.8c1.7,0,3.3-0.7,4.4-1.8
                                                                c1.1-1.1,1.8-2.7,1.8-4.4c0-1.7-0.7-3.3-1.8-4.4c-1.1-1.1-2.7-1.8-4.4-1.8C19,9.8,17.5,10.5,16.3,11.7" />
                                                        <path class="st0" d="M28.6,3.9C28.1,1.6,26.1,0,23.7,0h-6c-2.4,0-4.3,1.6-4.8,3.9H3.4C1.5,3.9,0,5.4,0,7.2V25
                                                                c0,1.9,1.5,3.4,3.4,3.4h34.7c1.9,0,3.4-1.5,3.4-3.4V7.2c0-1.9-1.5-3.4-3.4-3.4H28.6z M20.8,23.7c-4.2,0-7.6-3.4-7.6-7.6
                                                                c0-4.2,3.4-7.6,7.6-7.6c4.2,0,7.6,3.4,7.6,7.6C28.4,20.3,25,23.7,20.8,23.7 M36.5,7.4h-3.6c-0.4,0-0.6-0.3-0.6-0.6
                                                                c0-0.4,0.3-0.6,0.6-0.6h3.6c0.4,0,0.6,0.3,0.6,0.6C37.1,7.1,36.8,7.4,36.5,7.4" />
                                                    </g>
                                                </svg>
                                                Photo
                                               
                                            </div>
                                        </div>
                                        <h3 class="title-clamp-18 p-2">
                                            <a href="#">10 điểm đến hấp dẫn tại Phú Quốc sẽ đón khách quốc tế từ 20.11</a>
                                        </h3>
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="bg-white border">
                                        <div class="mask-box">
                                            <a class="item-image" href="#">
                                                <img src="/static/nvcmsv2/images/image-medium-12.png" alt="video"></a>
                                            <div class="tt-icon">
                                                <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                                                    viewBox="0 0 30.8 23.2" xml:space="preserve">
                                                    <g>
                                                        <defs>
                                                            <rect id="SVGID_1_" width="30.8" height="23.2" />
                                                        </defs>
                                                        <clipPath id="SVGID_2_">
                                                            <use xlink:href="#SVGID_1_" style="overflow: visible;" />
                                                        </clipPath>
                                                        <path class="st0" d="M30,0H0.8C0.3,0,0,0.4,0,0.7v21.7c0,0.5,0.3,0.7,0.8,0.7H30c0.5,0,0.8-0.2,0.8-0.7V0.7C30.8,0.4,30.5,0,30,0
                                                                M21.8,12.1L21.8,12.1l-10.4,5.7c-0.1,0.1-0.2,0.1-0.5,0.1c-0.1,0-0.3,0-0.5-0.1c-0.4-0.2-0.5-0.5-0.5-0.9V6.3c0-0.4,0.1-0.7,0.5-1
                                                                c0.3-0.1,0.6-0.1,1,0l10.4,4.8c0.4,0.3,0.6,0.6,0.6,1C22.5,11.5,22.2,11.9,21.8,12.1 M11.7,8L11.7,8l7.2,3.4l-7.2,3.9V8z" />
                                                    </g>
                                                </svg>
                                                Video
                                               
                                            </div>
                                        </div>
                                        <h3 class="title-clamp-18 p-2">
                                            <a href="#">Cô giáo vùng cao vượt 80km giao bài tập cho học sinh trong mùa dịch</a>
                                        </h3>
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="bg-white border">
                                        <div class="mask-box">
                                            <a class="item-image" href="#">
                                                <img src="/static/nvcmsv2/images/image-medium-11.png" alt="video"></a>
                                            <div class="tt-icon">
                                                <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                                                    viewBox="0 0 41.5 28.3" xml:space="preserve">
                                                    <g>
                                                        <defs>
                                                            <rect id="SVGID_1_" width="41.5" height="28.3" />
                                                        </defs>
                                                        <clipPath id="SVGID_2_">
                                                            <use xlink:href="#SVGID_1_" style="overflow: visible;" />
                                                        </clipPath>
                                                        <path class="st0" d="M5.5,0.4C4.6,0.4,3.9,1.1,3.9,2v0.6h7.4V2c0-0.9-0.7-1.6-1.6-1.6H5.5z" />
                                                        <path class="st0" d="M16.3,11.7c-1.1,1.1-1.8,2.7-1.8,4.4c0,1.7,0.7,3.3,1.8,4.4c1.1,1.1,2.7,1.8,4.4,1.8c1.7,0,3.3-0.7,4.4-1.8
                                                                c1.1-1.1,1.8-2.7,1.8-4.4c0-1.7-0.7-3.3-1.8-4.4c-1.1-1.1-2.7-1.8-4.4-1.8C19,9.8,17.5,10.5,16.3,11.7" />
                                                        <path class="st0" d="M28.6,3.9C28.1,1.6,26.1,0,23.7,0h-6c-2.4,0-4.3,1.6-4.8,3.9H3.4C1.5,3.9,0,5.4,0,7.2V25
                                                                c0,1.9,1.5,3.4,3.4,3.4h34.7c1.9,0,3.4-1.5,3.4-3.4V7.2c0-1.9-1.5-3.4-3.4-3.4H28.6z M20.8,23.7c-4.2,0-7.6-3.4-7.6-7.6
                                                                c0-4.2,3.4-7.6,7.6-7.6c4.2,0,7.6,3.4,7.6,7.6C28.4,20.3,25,23.7,20.8,23.7 M36.5,7.4h-3.6c-0.4,0-0.6-0.3-0.6-0.6
                                                                c0-0.4,0.3-0.6,0.6-0.6h3.6c0.4,0,0.6,0.3,0.6,0.6C37.1,7.1,36.8,7.4,36.5,7.4" />
                                                    </g>
                                                </svg>
                                                Photo
                                               
                                            </div>
                                        </div>
                                        <h3 class="title-clamp-18 p-2">
                                            <a href="#">10 điểm đến hấp dẫn tại Phú Quốc sẽ đón khách quốc tế từ 20.11</a>
                                        </h3>
                                    </div>
                                </div>
                                <div class="item">
                                    <div class="bg-white border">
                                        <div class="mask-box">
                                            <a class="item-image" href="#">
                                                <img src="/static/nvcmsv2/images/image-medium-12.png" alt="video"></a>
                                            <div class="tt-icon">
                                                <svg version="1.1" id="Layer_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px"
                                                    viewBox="0 0 30.8 23.2" xml:space="preserve">
                                                    <g>
                                                        <defs>
                                                            <rect id="SVGID_1_" width="30.8" height="23.2" />
                                                        </defs>
                                                        <clipPath id="SVGID_2_">
                                                            <use xlink:href="#SVGID_1_" style="overflow: visible;" />
                                                        </clipPath>
                                                        <path class="st0" d="M30,0H0.8C0.3,0,0,0.4,0,0.7v21.7c0,0.5,0.3,0.7,0.8,0.7H30c0.5,0,0.8-0.2,0.8-0.7V0.7C30.8,0.4,30.5,0,30,0
                                                                M21.8,12.1L21.8,12.1l-10.4,5.7c-0.1,0.1-0.2,0.1-0.5,0.1c-0.1,0-0.3,0-0.5-0.1c-0.4-0.2-0.5-0.5-0.5-0.9V6.3c0-0.4,0.1-0.7,0.5-1
                                                                c0.3-0.1,0.6-0.1,1,0l10.4,4.8c0.4,0.3,0.6,0.6,0.6,1C22.5,11.5,22.2,11.9,21.8,12.1 M11.7,8L11.7,8l7.2,3.4l-7.2,3.9V8z" />
                                                    </g>
                                                </svg>
                                                Video
                                               
                                            </div>
                                        </div>
                                        <h3 class="title-clamp-18 p-2">
                                            <a href="#">Cô giáo vùng cao vượt 80km giao bài tập cho học sinh trong mùa dịch</a>
                                        </h3>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
--%>
<section class="mt-4 mt-lg-5">
    <div class="container">
        <div id="KetNoiThuongHieuPane" runat="server"></div>

    </div>
</section>
<uc2:Footer runat="server" ID="Footer" />
<uc2:footerjs runat="server" ID="footerjs" />

