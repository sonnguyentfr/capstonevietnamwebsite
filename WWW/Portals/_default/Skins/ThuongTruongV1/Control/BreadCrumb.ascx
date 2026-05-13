<%@ Control Language="VB" AutoEventWireup="false" CodeFile="BreadCrumb.ascx.vb" Inherits="DesktopModules.TinTuc.Control.BreadCrumb" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<dnn:BREADCRUMB ID="BreadCrumb" runat="server" UseTitle="true" CssClass="item" RootLevel="-1" Separator="&lt;i class=&quot;fa fa-1x fa-angle-right&quot;&gt;&lt;/i&gt; " Visible="false" />
<asp:Literal ID="ltrbreadcrumb" runat="server" Visible="false"></asp:Literal>
