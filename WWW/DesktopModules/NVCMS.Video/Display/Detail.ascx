<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Detail.ascx.vb" Inherits="NVCMS.Modules.Video.Detail" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>

<div class="row no-gutter videodetaildetail">
    <div class="col-sm-8 col-md-8">
        <!-- Begin .video-full -->
        <div class="video-full">
            <div class="video-container">
                <asp:Literal ID="ltrplayvideo" runat="Server"></asp:Literal>
            </div>
        </div>
        <!-- End .video-full -->
    </div>
    <!-- End .col-md-8 -->
    <!-- Begin .col-md-4 -->
    <div class="col-sm-4 col-md-4">
        <!-- Begin .video-post_content -->
        <div class="video-post_content">
            <div class="title-left title-style04 underline04">
                <h3>
                    <asp:Literal ID="lblTitle" runat="server"></asp:Literal></h3>
            </div>
            <ul>
                <li><i class="fa fa-calendar"></i><span>
                    <asp:Literal ID="lblNgaycapnhat" runat="server"></asp:Literal></span></li>
            </ul>
            <div class="content">
                <asp:Literal ID="lblMota" runat="server"></asp:Literal>
                <asp:Literal ID="lblnoidung" runat="server"></asp:Literal>
            </div>
            <ul class="social-links list-inline">
                <li><a href="#" class="facebook"><i class="fa fa-facebook"></i></a></li>
                <li><a href="#" class="youtube"><i class="fa fa-youtube"></i></a></li>
                <li><a href="#" class="twitter"><i class="fa fa-twitter"></i></a></li>
                <li><a href="#" class="instagram"><i class="fa fa-instagram"></i></a></li>
                <li><a href="#" class="linkedin"><i class="fa fa-linkedin"></i></a></li>
            </ul>
        </div>
        <!-- End .video-post_content -->
    </div>
    <!-- End .col-md-4 -->
    <div class="clear"></div>

    <div class="title-style01">
        <h3><strong>Video Clips</strong> khác</h3>
    </div>
    <!-- End .title-style01 -->
    <!-- Begin .gallery-slider owl-carousel -->
    <div id="truyenhinhinternkhac">
        <asp:Repeater ID="rptMoinhatVideo" runat="server">
            <ItemTemplate>
                <div class="col-md-3 item">
                    <div class="big-gallery">
                        <img class="img-responsive img-full lazy" src="/data/noimage.png?width=291&height=185" data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "Avatar"), 291, 185, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" />
                        <a title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "Title"))%>" href="<%# Ultis.EventsFormatLink(1194, "d" & DataBinder.Eval(Container.DataItem, "id"), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                            <span class="play-icon"></span></a>
                        <h3>
                            <a title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "Title"))%>" href="<%# Ultis.EventsFormatLink(1194, "d" & DataBinder.Eval(Container.DataItem, "id"), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                <%# DataBinder.Eval(Container.DataItem, "Title")%>
                            </a>
                        </h3>
                    </div>

                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>
    <div class="clearfix pagination-wp">
        <ul class="pagination pull-left">
            <vbuzz:PAGING ID="vbPaging" runat="server" />
        </ul>
        <div class="cl"></div>
    </div>
</div>
