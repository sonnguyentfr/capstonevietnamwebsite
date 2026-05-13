<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="bannerclick.aspx.vb" Inherits="NVCMS.Modules.BannerAdv.bannerclick" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <div style="width:600px; margin:30px auto; text-align:center">
        <img src="https://thuongtruong.com.vn/static/nvcms/img/logo.png" />
        <p style="font-size:16px;padding:20px;"><strong>Bạn đang muốn link đến Trang <font style="color:red"><asp:Label ID="lblName" runat="server"></asp:Label></font></strong></p>
        <h2 style="font-size:24px;padding:0px; margin:0px;">HỆ THỐNG ĐANG CHUYỂN....</h2>
        <img src="/static/connecting.gif" alt="" width="600px" />
    </div>
	<!-- NVCMS Traffic -->
<script>
  var _paq = window._paq = window._paq || [];
  /* tracker methods like "setCustomDimension" should be called before "trackPageView" */
  _paq.push(['trackPageView']);
  _paq.push(['enableLinkTracking']);
  (function() {
    var u="//traffic.nvcms.net/";
    _paq.push(['setTrackerUrl', u+'matomo.php']);
    _paq.push(['setSiteId', '2']);
    var d=document, g=d.createElement('script'), s=d.getElementsByTagName('script')[0];
    g.async=true; g.src=u+'matomo.js'; s.parentNode.insertBefore(g,s);
  })();
</script>
<!-- End NVCMS Traffic Code -->
</body>
</html>
