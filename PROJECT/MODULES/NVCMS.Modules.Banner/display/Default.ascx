<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Default.ascx.vb" Inherits="NVCMS.Modules.Banner.defaul" %>
<div id="BannerAdv" style="text-align: center;">
    <ul>
    <asp:Repeater ID="drgOtherNews" runat="server">
        <ItemTemplate>
            <li style="display: block; padding-bottom: 10px;">
                <%# GetBanner(Eval("id")) %>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul></div>
