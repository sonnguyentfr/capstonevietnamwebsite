<%@ Control Language="VB" AutoEventWireup="false" CodeFile="footerjs.ascx.vb" Inherits="Portals__default_Skins_BUH_Control_Footer" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<script src="/static/nvcms/js/functions.js?v=1.12226"></script>
<dnn:DnnJsInclude ID="BootstrapJS" runat="server" FilePath="/static/nvcms/js/bootstrap.min.js" />
<dnn:DnnJsInclude ID="JqueryUI" runat="server" FilePath="/static/nvcms/js/jquery-ui.min.js" />
<dnn:DnnJsInclude ID="plugins" runat="server" FilePath="/static/nvcms/js/plugins.js" />
<dnn:DnnJsInclude ID="pluginskit" runat="server" FilePath="/static/nvcms/js/sticky-kit.min.js" />
<dnn:DnnJsInclude ID="lazy" runat="server" FilePath="/static/nvcms/js/jquery.lazy/jquery.lazy.min.js" />
<dnn:DnnJsInclude ID="lazyplugin" runat="server" FilePath="/static/nvcms/js/jquery.lazy/jquery.lazy.plugins.js" />
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
<script type="application/javascript">
(function(w,d,p,c){var r='ptag',o='script',s=function(u){var a=d.createElement(o),
m=d.getElementsByTagName(o)[0];a.async=1;a.src=u;m.parentNode.insertBefore(a,m);};
w[r]=w[r]||function(){(w[r].q = w[r].q || []).push(arguments)};s(p);s(c);})
(window, document, '//tag.adbro.me/tags/ptag.js', '//tag.adbro.me/configs/1gzhvseu.js');
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
