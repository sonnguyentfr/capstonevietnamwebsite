<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="SubMenu_News.ascx.vb" Inherits="DesktopModules.TinTuc.Control.Lastest" %>
<div class="tab-content">
    <div class="tab-pane show active" id="news-0" role="tabpanel">
        <div class="row">
            <asp:Repeater ID="rptLastest" runat="server">
                <ItemTemplate>
                    <div class="col-3 post-module-1">
                        <div class="post-thumb d-flex border-radius-5 img-hover-scale mb-15">
                            <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                <img src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 263, 164, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                            </a>
                            <span class="top-right-icon background2">
                                <i class="mdi mdi-audiotrack"></i>
                            </span>
                        </div>
                        <div class="post-content media-body">
                            
                                <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                    <h6 class="post-title mb-10 text-limit-2-row"><%# DataBinder.Eval(Container.DataItem, "title")%></h6> </a>
                            <div class="entry-meta meta-1 font-x-small color-grey">
                                <span class="post-on">25 April</span>
                                <span class="hit-count has-dot">126k Views</span>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
