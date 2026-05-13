<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="XuHuongdoc.ascx.vb" Inherits="DesktopModules.TinTuc.Control.Home.HotHome" %>
<div class="style5">
    <div class="title-style02">
        <h3>
            <strong>Xu hướng đọc</strong></h3>
    </div>
    <div class="sidebar-scroll">
        <div class="scroll-item">
            <asp:Repeater ID="rptHot" runat="server">
                <ItemTemplate>
                    <div class="item">
                        <div class="item-content-1">
                            <h3>
                                <a title="<%#ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                    <i class="fa fa-angle-double-right"></i><%# DataBinder.Eval(Container.DataItem, "title")%></a>
                            </h3>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
