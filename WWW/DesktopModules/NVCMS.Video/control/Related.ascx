<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="Related.ascx.vb" Inherits="DesktopModules.TinTuc.Control.Related" %>


<%--<div class="container mb-30">
    <div class="row">
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="relatedblock-heading">
                <div class="block-title">
                    Video khác
                </div>
                <div class="cl"></div>
            </div>
        </div>
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="loop-metro post-module-1 row">
                <asp:Repeater ID="drgOtherNews" runat="server">
                    <ItemTemplate>
                        <article class="col-lg-4 col-md-6 col-sm-12 mb-30">
                            <div class="post-thumb position-relative">
                                <div class="thumb-overlay img-hover-slide border-radius-5 position-relative" style="background-image: url(<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 240, 160, "crop", "middlecenter", "") %>)">
                                    <a class="img-link" href="<%# Ultis.FormatLink(TabId, CType(DataBinder.Eval(Container.DataItem, "VideoId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>"></a>
                                    <div class="post-content-overlay">
                                        <h6 class="post-title">
                                            <a class="color-white" href="<%# Ultis.FormatLink(TabId, CType(DataBinder.Eval(Container.DataItem, "VideoId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                                <%# DataBinder.Eval(Container.DataItem, "title")%></a>
                                        </h6>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on"><%# BL.FormatDate(DataBinder.Eval(Container.DataItem, "PublishedDate"))%></span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </article>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>
</div>--%>
<script src="/static/nvcms/js/jquery.masonryGrid.js"></script>
<style type="text/css">
    .my-masonry-grid-item {
        background-color: #fdfdfd;
    margin: 0 6px 12px;
    padding: 0px;
    border: solid 1px #9a9a9a;
    position: relative;
    box-shadow: 2px 2px 6px 0px #9e9e9ecf;
    }

        .my-masonry-grid-item:hover {
            background: #e0e0e0;
        }

            .my-masonry-grid-item:hover img {
                opacity: 0.8;
            }

            .my-masonry-grid-item:hover span.cat {
                background: #fff;
            }

                .my-masonry-grid-item:hover span.cat a {
                    color: #000;
                    font-size: 11px;
                }

        .my-masonry-grid-item img {
            max-width: 100%;
        }

        .my-masonry-grid-item span.cat {
            position: absolute;
            right: 5px;
            top: 5px;
            background: #d4000ec9;
            padding: 2px 5px;
            border-radius: 10px;
            color: #fff;
        }

            .my-masonry-grid-item span.cat a {
                color: #fff;
                font-size: 11px;
            }

        .my-masonry-grid-item .post-title {
            padding: 10px 20px;
            
            font-size: 18px;
        }
.my-masonry-grid-item .post-title p {
    overflow: hidden!important;
    display: -webkit-box!important;
    -webkit-line-clamp: 4!important;
    -webkit-box-orient: vertical;
}
.my-masonry-grid-item .post-title h3 {
    margin-bottom:15px;
}
            .my-masonry-grid-item .post-title h3 a {
                color: #262626;font-weight: 600; font-size:20px;line-height: 25px;
            }
            .my-masonry-grid-item span.stt {
            position: absolute;
            right: 5px;
            bottom: 5px;
            color: #ccc;
            font-size:12px;
        }
</style>
<div class="container mt-4">
	<div class="col-lg-12 col-md-12 col-sm-12">
				<div class="tmp-title-header d-flex justify-content-between">
    <h2 class="title-clamp m-0"><a href="#">Video khác</a></h2>
</div>
			</div>
    <div class="my-masonry-grid">
        <asp:Repeater ID="drgOtherNews" runat="server">
                    <ItemTemplate>
                <div class="my-masonry-grid-item">
                    <a href='<%# Ultis.FormatLinkVideo(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "VideoId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>'>
                        <img alt='Video' data-src='<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 312, 220, "crop", "middlecenter", "") %>' class="lazyload" />
                    </a>
                    <div class="post-title">
                        <h3><a href='<%# Ultis.FormatLinkVideo(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "VideoId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>'><%# DataBinder.Eval(Container.DataItem, "title")%></a></h3>
                    </div>
                    <div class="cl"></div>
                </div>
		</ItemTemplate>
                </asp:Repeater>
    </div>
    <div class='quangcaogoogle'>
        <!-- For the demo ad -->
        <ins class="adsbygoogle"
             style="display:block"
             data-ad-format="fluid"
             data-ad-layout-key="-7i+f1-17-4w+dd"
             data-ad-client="ca-pub-3311450421751656"
             data-ad-slot="5706203773"></ins>
        <script>
             (adsbygoogle = window.adsbygoogle || []).push({});
        </script>
    </div>
</div>
<script type="text/javascript">
    $(function () {
        $("div.my-masonry-grid-item:nth-child(10)").after('<div class="my-masonry-grid-item quangcaogoogle"><!-- PC.300x600 --><ins class="adsbygoogle" style="display:inline-block;width:300px;height:600px" data-ad-client="ca-pub-3311450421751656" data-ad-slot="8358212457"></' + 'ins><script>(adsbygoogle = window.adsbygoogle ||[]).push({});</' + 'script></' + 'div>');
        $("div.my-masonry-grid-item:nth-child(25)").after('<div class="my-masonry-grid-item quangcaogoogle"><!-- Mobile Ngang - 300x100 --><ins class="adsbygoogle" style="display:inline-block;width:300px;height:100px" data-ad-client="ca-pub-3311450421751656" data-ad-slot="5452086747"></' + 'ins><script>(adsbygoogle = window.adsbygoogle || []).push({});</' + 'script></' + 'div>');
        $("div.my-masonry-grid-item:nth-child(40)").after('<div class="my-masonry-grid-item quangcaogoogle"><!-- PC.300x600 --><ins class="adsbygoogle" style="display:inline-block;width:300px;height:600px" data-ad-client="ca-pub-3311450421751656" data-ad-slot="8358212457"></' + 'ins><script>(adsbygoogle = window.adsbygoogle ||[]).push({});</' + 'script></' + 'div>');
    });
</script>
<script>
    jQuery(document).ready(function ($) {
        $('.my-masonry-grid').masonryGrid({
            'columns': 4,
        });
    });
</script>

