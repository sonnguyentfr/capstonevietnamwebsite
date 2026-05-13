<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CatName.ascx.vb" Inherits="DesktopModules.TinTuc.Control.BreadCrumb" %>
<h2 class="title-clamp m-0">
    <asp:Literal ID="ltrtitlecat" runat="server"></asp:Literal></h2>
<div class="sub-menu d-flex text-nowrap scroll-menu">
    <asp:Repeater ID="rptsubcat" runat="server">
        <ItemTemplate>
            <a class="menu-link" href='<%# NavigateURL(BL.GetMappingTabIDByCategoryID(Convert.ToInt32(Eval("CategoryId")))) %>'><%# Eval("CategoryName") %></a>
        </ItemTemplate>
    </asp:Repeater>
</div>
