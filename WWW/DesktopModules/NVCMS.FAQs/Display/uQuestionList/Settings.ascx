<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" codefile="Settings.ascx.vb" Inherits="BUH.Modules.FAQs.SettingCustomeDisplaySpecial" %>
<style type="text/css">
    .setting_news { padding: 10px; }
    .setting_news table tr td { padding: 3px 0px; }
    .setting_news .list-radio label, .setting_news .list-checkbox label { padding-right: 10px; }
</style>
<div class="setting_news">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <table cellspacing="1" cellpadding="3" border="0" width="100%">
        <tr>
            <td>Kiểu hiện thị: </td>
            <td><asp:DropDownList ID="dropTemplate" runat="server" Width="300px" /></td>
        </tr>
            <tr>
            <td>Số lượng câu hỏi trên trang: </td>
            <td><asp:TextBox ID="txtSL" runat="server" Width="30px" Text="0" Font-Bold="true"></asp:TextBox> </td>
        </tr>
    </table>  
      <p><asp:Label ID="lbMessage" runat="server" ForeColor="Red" /></p>
        </ContentTemplate>
    </asp:UpdatePanel>        
</div>
