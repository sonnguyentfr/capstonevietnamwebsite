<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="DocNhieu.ascx.vb" Inherits="DesktopModules.TinTuc.Control.Lastest" %>
<div class="tmp-title-header d-flex justify-content-between">
    <h2 class="title-clamp m-0"><a href="#">Đọc nhiều</a></h2>
</div>
<div class="mt-4">
    <asp:Repeater ID="rptLastest" runat="server">
        <ItemTemplate>
            <div class="item-most">
                <div class="txt-number"><%# Container.ItemIndex + 1 %></div>
                <div class="txt-content">
                    <div class="title-cate"><%# DataBinder.Eval(Container.DataItem, "CategoryName")%></div>
                    <a title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>"><%# DataBinder.Eval(Container.DataItem, "title")%></a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</div>

