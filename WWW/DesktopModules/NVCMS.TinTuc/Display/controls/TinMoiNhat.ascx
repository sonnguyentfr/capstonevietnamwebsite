<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="TinMoiNhat.ascx.vb" Inherits="DesktopModules.TinTuc.Control.IndexLastest" %>
<div class="block-title-1">
    <h3><a href="#">Tin mới</a></h3>
</div>
<div class="sidebar-newsfeed">
    <!-- Begin .newsfeed -->
    <div class="newsfeed-3">
        <ul>
            <asp:Repeater ID="rptLastest" runat="server">
                <ItemTemplate>
                    <li>
                        <div class="item">
                            <div class="item-image">
                                <a class="img-link" href="<%# Ultis.FormatLink(TabId, CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                    <img class="img-responsive img-full" src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 100, 80, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                                </a>
                            </div>
                            <div class="item-content">
                                <h2 class="ellipsis"><a title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" href="<%# Ultis.FormatLink(TabId, CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                    <%# DataBinder.Eval(Container.DataItem, "title")%></a></h1>
                            </div>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
        </ul>
    </div>
    <!-- End .newsfeed -->
</div>
