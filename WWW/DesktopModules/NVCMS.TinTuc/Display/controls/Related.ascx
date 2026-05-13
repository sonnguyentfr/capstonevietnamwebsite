<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="Related.ascx.vb" Inherits="DesktopModules.TinTuc.Control.Related" %>
<div class="relatedblock-heading">
    <div class="block-title head-box">
        <asp:HyperLink ID="hplCat" runat="server" CssClass="white-color fs-20">
            Tin bài khác
            <asp:Label ID="lblbl" runat="server"></asp:Label>
        </asp:HyperLink>
    </div>
    <div class="cl"></div>
</div>
<div class="module-tmp-4 mt-5">
    <asp:Repeater ID="drgOtherNews" runat="server">
        <ItemTemplate>
            <div class="item-news mt-3 pt-3 border-top">
                <div class="start-date">
                    <%# BL.FormatDate(DataBinder.Eval(Container.DataItem, "PublishedDate"))%>
                </div>
                <h5 class="title-clamp-20">
                    <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                        <%# DataBinder.Eval(Container.DataItem, "title")%></a>
                </h5>
                <div class="row mt-3">
                    <div class="col-5 col-lg-4 pe-0 pe-md-2 side-bar-img">
                        <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                            <img class="img-responsive img-full lazy" src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 240, 160, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" />
                        </a>
                    </div>
                    <div class="col-7 col-lg-8 txt-dec">
                        <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                            <%# DataBinder.Eval(Container.DataItem, "summary")%></a>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div id="displayMobileMoiNhatResults"></div>
    <div id="loadimageje1mobilemoinhat" style="display: none; background: url(/static/nvcms/img/preloader.gif) no-repeat 22px; height: 40px"
        class="text-center mr-10">
    </div>
</div>
<asp:HiddenField ID="hdfcatid" runat="server" />
<script type="text/javascript">
    var pageIndex = 2;
    var pageCount;
    var catid = document.getElementById('<%=hdfcatid.ClientID%>').value;
    $(window).scroll(function () {
        var htop = parseInt(jQuery("footer.tt_footer").offset().top - 1100);
        if ($(window).scrollTop() >= htop) {
            if (pageIndex < 15) {
                //alert("htop" + htop + "w" + $(window).scrollTop() + "page index: " + pageIndex);
                GetRecords();
            }
        }
    });
    function GetRecords() {
        pageIndex++;
        $('div#loadimageje1mobilemoinhat').show();
        $.ajax({
            url: "/DesktopModules/NVCMS.TinTuc/display/controls/IndexCattLoadMore.aspx?pageid=" + pageIndex + "&catid=" + catid,
            success: function (data) {
                $('div#loadimageje1mobilemoinhat').hide();
                $('#displayMobileMoiNhatResults').append(data);
            }
        });
    }



</script>
