<%@ Control Language="VB" AutoEventWireup="false" CodeFile="footerjs.ascx.vb" Inherits="Portals__default_Skins_BUH_Control_Footer" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<!-- Main Wrap End-->
<div class="dark-mark"></div>
<!-- Vendor JS-->
<script src="/static/nvcms_vanhoa/js/vendor/modernizr-3.5.0.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery-1.12.4.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/popper.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/bootstrap.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.slicknav.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/owl.carousel.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/slick.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/wow.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/animated.headline.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.magnific-popup.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.ticker.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.vticker-min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.scrollUp.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.nice-select.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.sticky.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/perfect-scrollbar.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/waypoints.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.counterup.min.js"></script>
<script src="/static/nvcms_vanhoa/js/vendor/jquery.theia.sticky.js"></script>
<!-- UltraNews JS -->
<script src="/static/nvcms_vanhoa/js/main.js"></script>
<div id="fb-root"></div>
<script type="text/javascript">
    (function (d, s, id) {
        var js, fjs = d.getElementsByTagName(s)[0];
        if (d.getElementById(id)) return;
        js = d.createElement(s); js.id = id;
        js.src = 'https://connect.facebook.net/vi_VN/sdk.js#xfbml=1&version=v2.12';
        fjs.parentNode.insertBefore(js, fjs);
    }(document, 'script', 'facebook-jssdk'));
    var u = location.href;

    window.fbAsyncInit = function () {
        FB.init({
            appId: '517574561947569',
            cookie: true,
            status: true,
            xfbml: true,
            oauth: true,
            version: 'v11.0'
        });
    };

</script>
<!-- Google Tag Manager (noscript) -->
<noscript>
    <iframe src="https://www.googletagmanager.com/ns.html?id=GTM-PDVPCG8"
        height="0" width="0" style="display: none; visibility: hidden"></iframe>
</noscript>
<script type="text/javascript">
    var addthis_config = addthis_config || {};
    addthis_config.data_track_clickback = false;
</script>
<%--
<script type="text/javascript">
    $(document).ready(function () {
        $('img').each(function () {
            if (!this.complete || typeof this.naturalWidth == "undefined" || this.naturalWidth == 0) {
                this.src = '/DATA/noimage.png';
            }
        });
		$('.quangcaogoogle').fadeIn(0);
    });
</script>--%>