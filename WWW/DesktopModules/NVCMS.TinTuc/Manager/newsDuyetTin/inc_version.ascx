<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" codefile="inc_version.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.newsapprove.NewsApprove_inc_version" %>

<table id="Table1" border="0" width="100%" >
	<tr>
		<td>
		    <table width="100%">
		        <tr>
		            <td valign="top"><asp:label id="lbTitle" CssClass="link_news_hot" runat="server"></asp:label><br/>
		            <asp:Label ID="lbDateText" runat="server" CssClass="HotNews_DateText" text="Ngày cập nhât:"></asp:Label>
			        <asp:label Font-Size="11px" id="lbDatetime" CssClass="HotNews_date" runat="server"></asp:label></td>
		        </tr>
		    </table>
	    </td>
	</tr>
	<tr><td style="border-bottom:dotted #d7d7d7 1px;"><asp:label id="lbContent" CssClass="NewsDetail_content" runat="server"></asp:label></td></tr>
</table>
<script type="text/javascript">
    // Click 2 play
    var storagePath = '<%= StorageFolder %>';
    $('a[title="Play"]').on('click', function () {
        ViewMedia(storagePath + "/" + getDecodeString($(this).attr("href").match(/[^\/\\]+$/)));
        return false;
    });
    $('a[title="Download"]').on('click', function () {
        window.open(storagePath + "/" + getEncodedString($(this).attr("href").match(/[^\/\\]+$/)));
        return false;
    });
    $('a[title="IMAGES"]').on('click', function () {
        window.open($(this).find('img').attr("src"));
        return false;
    });
</script>