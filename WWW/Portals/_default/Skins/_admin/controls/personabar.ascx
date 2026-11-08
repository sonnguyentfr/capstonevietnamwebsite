<%@ Control Language="VB" AutoEventWireup="false" %>
<%--
  PERSONABAR-TOGGLE: skin _admin an Persona Bar cua DNN mac dinh; nut (icon) o chan sidebar (menu.ascx, ngay truoc nut Settings; icon MDI mdi-adjust, font /fonts/materialdesignicons-webfont.*)
  bat / tat Persona Bar. Trang thai luu trong cookie phien "nvpb" (1 = hien) => giu nguyen khi chuyen trang, cho den khi bam lai icon de dong
  (hoac dong trinh duyet). Nut chi hien voi tai khoan co Persona Bar (trang co #personaBar-iframe).
  Man hinh < 1200px: an nut va an luon Persona Bar (sidebar DashLite chuyen sang dang truot, khong co cho cho Persona Bar 80px).
  Chen CSS + script vao <head> o moi request (ca postback) de an ngay tu dau, khong nhay. Dang ky trong Default.ascx / Index.ascx.
--%>
<script runat="server">
    Protected Overrides Sub OnInit(ByVal e As EventArgs)
        MyBase.OnInit(e)
        If Page.Header Is Nothing OrElse Page.Items("nvpb") IsNot Nothing Then Return
        Page.Items("nvpb") = True
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine("<style id='nv-pb-style'>")
        sb.AppendLine("html.nv-pb-off #personaBar-iframe, html.nv-pb-off #personabar-placeholder, html.nv-pb-off .personaBarContainer{visibility:hidden!important;pointer-events:none!important}")
        sb.AppendLine("html.nv-pb-off body.personabar-visible, html.nv-pb-off body.personabar-visible .nk-sidebar{margin-left:0!important}")
        sb.AppendLine("html.nv-pb-on body.personabar-visible .nk-header-fixed{margin-left:80px}")
        sb.AppendLine(".nv-pb-toggle{display:none}")
        sb.AppendLine("@font-face{font-family:'nv-mdi';src:url('/fonts/materialdesignicons-webfont.woff2?v=1.6.50') format('woff2'),url('/fonts/materialdesignicons-webfont.woff?v=1.6.50') format('woff'),url('/fonts/materialdesignicons-webfont.ttf?v=1.6.50') format('truetype');font-weight:normal;font-style:normal}")
        sb.AppendLine(".nv-pb-toggle .mdi{display:inline-block;font:normal normal normal 22px/1 'nv-mdi';text-rendering:auto;-webkit-font-smoothing:antialiased}")
        sb.AppendLine(".nv-pb-toggle .mdi-adjust:before{content:'\F01A'}")
        sb.AppendLine("html.nv-pb-has .nv-pb-toggle{display:list-item}")
        sb.AppendLine("html.nv-pb-on .nv-pb-toggle .nk-menu-link,html.nv-pb-on .nv-pb-toggle .nk-menu-link .icon{color:#6576ff}")
        sb.AppendLine("html.nv-pb-on .nv-pb-toggle .nk-menu-link{background:rgba(101,118,255,.12);border-radius:6px}")
        sb.AppendLine("@media (max-width:1199.98px){")
        sb.AppendLine("  html .nv-pb-toggle,html.nv-pb-has .nv-pb-toggle{display:none!important}")
        sb.AppendLine("  #personaBar-iframe,#personabar-placeholder,.personaBarContainer{display:none!important}")
        sb.AppendLine("  body.personabar-visible,body.personabar-visible .nk-sidebar,body.personabar-visible .nk-header-fixed,html.nv-pb-on body.personabar-visible .nk-header-fixed{margin-left:0!important}")
        sb.AppendLine("}")
        sb.AppendLine("</style>")
        sb.AppendLine("<script>")
        sb.AppendLine("(function(){")
        sb.AppendLine("  var d=document.documentElement;")
        sb.AppendLine("  function isOn(){return /(?:^|;\s*)nvpb=1(?:;|$)/.test(document.cookie);}")
        sb.AppendLine("  function apply(on){")
        sb.AppendLine("    d.classList.remove(on?'nv-pb-off':'nv-pb-on'); d.classList.add(on?'nv-pb-on':'nv-pb-off');")
        sb.AppendLine("    var b=document.getElementById('nvPbToggle'); if(b){var a=b.querySelector('a'); if(a){a.setAttribute('aria-pressed',on?'true':'false'); a.title=on?'Ẩn Persona Bar':'Hiện Persona Bar';}}")
        sb.AppendLine("  }")
        sb.AppendLine("  var cur=isOn();")
        sb.AppendLine("  apply(cur);")
        sb.AppendLine("  window.nvPersonaBar={toggle:function(){")
        sb.AppendLine("    var on=cur=!cur;")
        sb.AppendLine("    document.cookie='nvpb='+(on?'1':'0')+'; path=/; SameSite=Lax'+(location.protocol==='https:'?'; Secure':'');")
        sb.AppendLine("    apply(on);")
        sb.AppendLine("    try{window.dispatchEvent(new Event('resize'));}catch(e){}")
        sb.AppendLine("    return false;")
        sb.AppendLine("  }};")
        sb.AppendLine("  document.addEventListener('DOMContentLoaded',function(){")
        sb.AppendLine("    if(document.getElementById('personaBar-iframe')) d.classList.add('nv-pb-has');")
        sb.AppendLine("    apply(cur);")
        sb.AppendLine("  });")
        sb.AppendLine("})();")
        sb.AppendLine("</scr" & "ipt>")
        Page.Header.Controls.Add(New LiteralControl(sb.ToString()))
    End Sub
</script>
