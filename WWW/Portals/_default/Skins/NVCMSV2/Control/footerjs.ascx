<%@ Control Language="VB" AutoEventWireup="false" CodeFile="footerjs.ascx.vb" Inherits="Portals__default_Skins_BUH_Control_Footer" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>
<%--<script src="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/jquery-3.5.1.min.js"></script>--%>
<dnn:DnnJsInclude ID="Popper" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/popper.min.js" />
<dnn:DnnJsInclude ID="slickjs" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/slick.js" />
<dnn:DnnJsInclude ID="BootstrapJS" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcmsv2/bootstrap/js/bootstrap.min.js" />
<dnn:DnnJsInclude ID="LazyJS" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/jquery.lazy/jquery.lazy.js" />
<dnn:DnnJsInclude ID="LazyPluginJS" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/jquery.lazy/jquery.lazy.plugins.js" />
<dnn:DnnJsInclude ID="Lazysizes" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/lazysizes/lazysizes.min.js?v=1" />
<dnn:DnnJsInclude ID="Lazysizeslugin" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/lazysizes/plugins/blur-up/ls.blur-up.min.js?v=1" />

<dnn:DnnJsInclude ID="mediaelement" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/playvideo/mediaelement-and-player.js?v=1" />
<dnn:DnnJsInclude ID="dailymotion" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/playvideo/renderers/dailymotion.js?v=1" />
<dnn:DnnJsInclude ID="facebook" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/playvideo/renderers/facebook.js?v=1" />
<dnn:DnnJsInclude ID="soundcloud" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/playvideo/renderers/soundcloud.js?v=1" />
<dnn:DnnJsInclude ID="twitch" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/playvideo/renderers/twitch.js?v=1" />
<dnn:DnnJsInclude ID="vimeo" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/playvideo/renderers/vimeo.js?v=1" />
<script src="https://thuongtruong-cdn.nvcms.net/nvcmsv2/js/custom.js?v=1.2"></script>

<%--<script src="https://thuongtruong-cdn.nvcms.net/nvcms/js/functions.js?v=1.12228"></script>
<dnn:DnnJsInclude ID="BootstrapJS" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/js/bootstrap.min.js" />
<dnn:DnnJsInclude ID="JqueryUI" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/js/jquery-ui.min.js" />
<dnn:DnnJsInclude ID="plugins" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/js/plugins.js" />
<dnn:DnnJsInclude ID="pluginskit" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/js/sticky-kit.min.js" />
<dnn:DnnJsInclude ID="lazy" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/js/jquery.lazy/jquery.lazy.min.js" />
<dnn:DnnJsInclude ID="lazyplugin" runat="server" FilePath="https://thuongtruong-cdn.nvcms.net/nvcms/js/jquery.lazy/jquery.lazy.plugins.js" />--%>
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
<script async type="application/javascript"
        src="https://news.google.com/swg/js/v1/swg-basic.js"></script>
