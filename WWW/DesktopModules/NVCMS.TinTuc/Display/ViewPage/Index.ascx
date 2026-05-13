<%@ Control Language="C#" EnableViewState="true" AutoEventWireup="true" CodeFile="Index.ascx.cs" Inherits="DesktopModules.TinTuc.ViewPage.Index" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<%--<asp:Literal ID="ltrllia" runat="server"></asp:Literal>--%>
<asp:Literal ID="ltContent" runat="server" />
<asp:Literal ID="ltSettings" runat="server" />

<div class="clearfix pagination-wp">
    <vbuzz:PAGING ID="vbPaging" runat="server" />
</div>
