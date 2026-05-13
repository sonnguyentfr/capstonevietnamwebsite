<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Settings.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.SettingNewsCategory" %>
<table cellspacing="1" cellpadding="1" border="0" width="100%">
    <tr>
        <td><asp:Label CssClass="SubHead" ID="Label5" runat="server">Chuyên mục tin</asp:Label></td>
        <td><asp:DropDownList ID="ddlCategory" runat="server" Width="280px"></asp:DropDownList></td>
    </tr>
    <tr>
        <td width="90px"><asp:Label CssClass="SubHead" ID="Label1" runat="server">Trang hiển thị</asp:Label></td>
        <td><asp:TextBox ID="txtDisplayNewsPage" runat="server" Width="40px"></asp:TextBox></td>
    </tr>
</table>
