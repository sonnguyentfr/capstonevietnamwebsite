<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="SearchResult.ascx.vb" Inherits="DesktopModules.TinTuc.Display.Search.inc_SerachResult" %>
<%@ Register Src="~/controls/Pages.ascx" TagPrefix="uc1" TagName="Pages" %>
<style type="text/css">
    .ketquatimkiem .header_search_content.on {
        opacity: 1;
        -webkit-transform: scale(1);
        -ms-transform: scale(1);
        transform: scale(1);
    }

    .ketquatimkiem .header_search_content {
        position: relative;
        top: 100%;
        right: auto;
        width: 100%;
        background: #fff;
        border-top: 3px solid;
        opacity: 0;
        padding: 10px;
        -webkit-transform: scale(0);
        -ms-transform: scale(0);
        transform: scale(0);
        -webkit-transition: all ease .3s;
        -ms-transition: all ease .3s;
        transition: all ease .3s;
        -webkit-box-shadow: 0 3px 5px rgb(0 0 0 / 10%);
        -ms-box-shadow: 0 3px 5px rgba(0,0,0,.1);
        box-shadow: 0 3px 5px rgb(0 0 0 / 10%);
        z-index: 9;
    }

    .ketquatimkiem .header_search_content {
        margin-bottom: 10px;
    }

        .ketquatimkiem .header_search_content input.search_query {
            width: calc(100% - 56px);
            font-family: inherit;
            -webkit-transition: border linear .2s,box-shadow linear .2s;
            -moz-transition: border linear .2s,box-shadow linear .2s;
            -o-transition: border linear .2s,box-shadow linear .2s;
            transition: border linear .2s,box-shadow linear .2s;
            -webkit-border-radius: 0;
            -moz-border-radius: 0;
            border-radius: 0;
            vertical-align: middle;
            color: #8093a8;
            padding: 10px 15px;
            border-radius: 0;
            font-weight: 400;
            background-color: #fff;
            text-transform: inherit;
            border: 1px solid rgba(0, 43, 92, 0.08);
            font-size: 15px;
            outline: none;
            line-height: inherit;
            letter-spacing: 0px;
        }

    .ketquatimkiem a.close-search {
        font-size: 18px;
        width: 54px;
        border: 0;
        border-radius: 0;
        height: 47px;
        text-align: center;
        display: block;
        color: #fff;
        float: right;
        background-color: #ff4f01;
        padding-top: 14px;
    }

    .ketquatimkiem .nsl-5-content {
        display: flex;
        margin-bottom: 15px;
        padding-bottom: 15px;
        border-bottom: dashed 1px #ccc;
        width: 100%;
    }

        .ketquatimkiem .nsl-5-content .nsl-5-left {
            width: 200px;
        }

        .ketquatimkiem .nsl-5-content .nsl-5-right {
            width: calc(100% - 210px);
        }

            .ketquatimkiem .nsl-5-content .nsl-5-right a {
                font-size: 16px;
                font-weight: 600;
            }

    .highlight {
        text-decoration: none;
        color: black;
        background: yellow;
    }
</style>
<div class="ketquatimkiem">
    <div class="header_search_content on">
        <div id="searchbox" class="ml-auto">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="search_query" onkeypress="return checkKeypressSearchTopDetail(event)"></asp:TextBox>
            <asp:LinkButton class="btn close-search" ID="btnSearch" runat="server" OnClientClick="doSearchSiteDetail(); return false;"><i class="fa fa-search"></i></asp:LinkButton>
        </div>
    </div>
</div>
<div class="module-tmp-4 mt-5">
    <asp:Repeater runat="server" ID="drgNews">
        <ItemTemplate>
            <div class="item-news mt-3 pt-3 border-top">
                <div class="start-date">
                    <span class="pe-1"><%# BL.FormatDate(DataBinder.Eval(Container.DataItem, "PublishedDate"))%></span>
                </div>
                <h5 class="title-clamp-20">
                    <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                        <%# Highlight(DataBinder.Eval(Container.DataItem, "Title"), "<font class='highlight'>", "</font>")%></a>
                </h5>
                <div class="row mt-3">
                    <div class="col-5 col-lg-4 pe-0 pe-md-2 side-bar-img">
                        <a class="lazy" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                            <img class="img-responsive img-full lazy" src="/data/nophoto240-160.png" data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 240, 160, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" />
                        </a>
                    </div>
                    <div class="col-7 col-lg-8 txt-dec">
                        <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                            <%# Highlight(DataBinder.Eval(Container.DataItem, "Summary"), "<font class='highlight'>", "</font>")%>
                        </a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

</div>

<div class="clearfix pagination-wp">
    <uc1:Pages runat="server" ID="vbPaging" />
</div>
<script type="text/javascript">
    function doSearchSiteDetail() {
        var e = "";
        "" != (e = document.getElementById("<%=txtSearch.ClientId%>").value).toString() ? window.open("/tim-kiem?q=" + encodeURI(e), "_self") : alert("Nhập từ khóa tìm kiếm");
    }
    function checkKeypressSearchTopDetail(e) {
        var kC = (window.event) ? event.keyCode : e.keyCode;  // MSIE : Firefox
        if (kC == 13) {
            doSearchSiteDetail();
            return false;
        }
    }

</script>
