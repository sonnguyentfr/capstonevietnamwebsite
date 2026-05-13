<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CatName.ascx.vb" Inherits="DesktopModules.TinTuc.Control.BreadCrumb" %>
<div class="module-title">
    <h3 class="title fl">
        <asp:Literal ID="ltrtitlecat" runat="server"></asp:Literal>
    </h3>
    <div class="subtitlecat">
        <ul>
            <asp:Repeater ID="rptsubcat" runat="server">
                <ItemTemplate>
                    <li class="<%#ActiveSubCat(Eval("CategoryId")) %>"><a href="<%# NavigateURL(BL.GetMappingTabIDByCategoryID(Eval("CategoryId"))) %>"><%#Eval("CategoryName") %></a></li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
</div>
