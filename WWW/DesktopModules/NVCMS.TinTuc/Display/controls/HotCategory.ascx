<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="HotCategory.ascx.vb" Inherits="DesktopModules.TinTuc.Control.HotCategory" %>
<div class="news newsindexhotcat">
    <div class="item">
        <div class="item-image-1">
            <asp:Literal ID="ltrhotimage" runat="server"></asp:Literal>
        </div>
        <div class="item-content">
            <div class="title-left title-style04 underline04">
                <asp:Literal ID="ltrhottitle" runat="server"></asp:Literal>
            </div>
            <br>
            <div class="post-meta-elements">
                <div class="post-meta-date">
                    <i class="fa fa-calendar"></i>
                    <asp:Literal ID="ltrhotdate" runat="server"></asp:Literal>
                </div>
            </div>
            <p>
                <asp:Literal ID="ltrhotsum" runat="server"></asp:Literal>
            </p>
            <div>
                <asp:Literal ID="ltrhotdoctiep" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <div class="news-block">
        <asp:Repeater ID="rptHot" runat="server">
            <ItemTemplate>
                <div class="item-block">
                    <div class="item-image">
                        <a class="img-link" title="<%#ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                            <img class="img-responsive img-full lazy" src="/data/nophoto201-135.png" data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 201, 135, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                        </a>
                    </div>
                    <div class="item-content">
                        <p>
                            <a title="<%#ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                <%# DataBinder.Eval(Container.DataItem, "title")%></a>
                        </p>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>
<asp:Literal ID="ltrllia" runat="server"></asp:Literal>