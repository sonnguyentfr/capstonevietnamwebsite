<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="Tinlienquan.ascx.vb" Inherits="DesktopModules.TinTuc.Control.Tinlienquan" %>
<div class="module-related-news mt-5" id="tinlienquan" runat="server">
    <div class="tmp-title-header ps-2 bd_none d-flex justify-content-between">
        <h2 class="title-clamp m-0"><a href="#">Tin liên quan</a></h2>
    </div>
    <div class="list-items-related">
        <asp:Repeater ID="repeateReleated1" runat="server">
            <ItemTemplate>
        <div class="item-related">
            <div class="start-date"><span class="pe-1"><%# CDate(Eval("PublishedDate")).ToString("HH:mm")%></span> <%# CDate(Eval("PublishedDate")).ToString("dd-MM-yyyy")%></div>
            <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>"><%# DataBinder.Eval(Container.DataItem, "title")%></a>
        </div>
                </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
