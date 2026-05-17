<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Explicit="true" CodeFile="Index.ascx.vb" Inherits="NVCMS.Modules.School.IndexSearch" %>
<style type="text/css">
    .slider-6-items .slick-slide {
    margin: 0 5px;background:#fff;text-align:center;
    min-height:200px;
}
    .slider-6-items .slick-slide h3 {font-size: 17px;
    font-weight: 500;}
</style>
<div class="col-lg-12 order-lg-12">
    <div class="section-title text-center  mb-20">
        <h2 class="title-clamp mb-20 uppercase font-bold"><a class="font-color" href="/cac-truong-doi-tac">DANH SÁCH TRƯỜNG ĐỐI TÁC</a></h2>
        <div class="bar"></div>
    </div>
</div>
<div class="col-lg-12 order-lg-12">
    <div class="module-tmp-2">
        <div class="slider-6-items">
            <asp:Repeater ID="rptContent" runat="server">
                <ItemTemplate>
                    <div class="item-news">
                        <div class="item-image">
                            <a href="<%# Ultis.FormatLink_School(4100, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>">
                                <img src="<%#Ultis.FormatThumbImage(Eval("Logo"), 230, 120, "constrain", "middlecenter", "") %>" alt="<%#Eval("NameofSchool") %>" class="blur-up lazyload" /></a>
                        </div>
                        <h3 class="mt-3 text-limit-4-row">
                            <a href="<%# Ultis.FormatLink_School(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>"><%#Eval("NameofSchool") %></a>
                        </h3>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
