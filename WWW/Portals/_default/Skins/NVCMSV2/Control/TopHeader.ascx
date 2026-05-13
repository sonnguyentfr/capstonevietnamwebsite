<%@ Control Language="vb" EnableViewState="false" Inherits="DesktopModules.TinTuc.Control.TopHeader" CodeFile="TopHeader.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%--<%@ Register Src="~/Portals/_default/Skins/ThuongTruongV1/Control/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>--%>
<header class="d-none d-lg-block">
    <div class="main-header">
        <div class="container">
            <div class="row align-items-center">
                <div class="col-3">
                    <div class="d-flex">
                        <a class="me-1" href="https://www.facebook.com/thuongtruong.com.vn" target=_blank>
                            <i style="color:#fff" class="fa fa-facebook-square" aria-hidden="true"></i></a>
                        <a class="" href="#">
                            <i style="color:#fff" class="fa fa-youtube-play" aria-hidden="true"></i></a>
                    </div>
                    <div class="text-size-s text-white mt-1">
                        <span id="date"></span> - <span id="time"></span>
                        
                    </div>
                </div>
                <div class="col-6 text-center">
                    <a class="" href="/">
                        <img src="https://thuongtruong-cdn.nvcms.net/nvcmsv2/images/logo-red.png?v=1" alt="Tạp chí điện tử Thương Trường" class="hlogo"></a>
                </div>
                <div class="col-3 text-size-s d-flex justify-content-end">
                    <div class="btn-group me-3">
                        <div class="p-0">
                            <div class="search">
                                <div class="input-group">
                                    <input type="search" class="form-control" placeholder="Nội dung cần tìm..." id="seach-box" onkeypress="return checkKeypressSearchTop(event)" >
                                    <div class="icon-btn-search" onclick="doSearchSite(); return false;">
                                        <i class="fa fa-search" style="color:red" aria-hidden="true"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
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
