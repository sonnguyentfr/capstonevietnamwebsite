<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Explicit="true" CodeFile="Tags.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.Tags" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/Controls/Tinlienquan.ascx" TagPrefix="uc" TagName="Tinlienquan" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/DocNhieu.ascx" TagPrefix="uc" TagName="DocNhieu" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/DocNhieuMobile.ascx" TagPrefix="uc" TagName="DocNhieuMobile" %>
<%@ Register TagPrefix="uc" TagName="RELATED" Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/Related.ascx" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/TinMoiNhat.ascx" TagPrefix="uc" TagName="MoiNhat" %>
<%--<%@ Register Src="~/DesktopModules/NVCMS.Video/control/home/TruyenHinh.ascx" TagPrefix="uc" TagName="TruyenHinh" %>--%>
<%@ Register Src="~/DesktopModules/NVCMS.Banner/display/Default.ascx" TagPrefix="uc" TagName="Default" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/BreadCrumb.ascx" TagPrefix="uc" TagName="BreadCrumb" %>
<section class="mt-4 mt-lg-5">
    <div class="container">
        <div class="tmp-header-2">
            <h2 class="title-clamp m-0">
                <asp:Literal ID="lblTags" runat="server"></asp:Literal></h2>
            <div class="sub-menu sp-hid-not-active d-flex text-nowrap scroll-menu">
            </div>
        </div>
        <div class="row-custom mt-3">
            <div class="col-left-8">
                <div class="module-tmp-4 mt-5" id="khongco" runat="server" visible="false">
                    Không có dữ liệu!
                </div>
                <div class="module-tmp-4 mt-5" id="codata" runat="server" visible="false">
                    <asp:Repeater runat="server" ID="rptContent">
                        <ItemTemplate>
                            <div class="item-news mt-3 pt-3 border-top">
                                <div class="start-date">
                                    <%# BL.FormatDate(DataBinder.Eval(Container.DataItem, "PublishedDate"))%>
                                </div>
                                <h5 class="title-clamp-20">
                                    <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>"><%# DataBinder.Eval(Container.DataItem, "title")%></a>
                                </h5>
                                <div class="row mt-3">
                                    <div class="col-5 col-lg-4 pe-0 pe-md-2 side-bar-img">
                                        <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                            <img src="/data/nophoto240-160.png" data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 293, 174, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" class="lazy" />
                                        </a>
                                    </div>
                                    <div class="col-7 col-lg-8 txt-dec">
                                        <a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>"><%# DataBinder.Eval(Container.DataItem, "summary")%></a>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>
                <vbuzz:PAGING ID="vbPaging" runat="server" />
                <!-- quang cao -->
                <section class="my-3 d-none d-lg-block">
                    <div class="text-center">
                        <ins class="adsbygoogle"
                            style="display: block; text-align: center;"
                            data-ad-layout="in-article"
                            data-ad-format="fluid"
                            data-ad-client="ca-pub-3311450421751656"
                            data-ad-slot="1792804105"></ins>
                        <script>
                            (adsbygoogle = window.adsbygoogle || []).push({});
                        </script>
                    </div>
                </section>
                
            </div>

            <!-- col right -->
            <div class="col-right-4 d-none d-md-block">
                <div class="box-indiv-tmp-1 text-center">
                    <!-- PC.300x600 -->
                    <ins class="adsbygoogle"
                        style="display: inline-block; width: 300px; height: 600px"
                        data-ad-client="ca-pub-3311450421751656"
                        data-ad-slot="8358212457"></ins>
                    <script>
                        (adsbygoogle = window.adsbygoogle || []).push({});
                    </script>
                </div>
                <div class="most-view mb-4 mb-lg-5">
                    <uc:DocNhieu runat="server" ID="DocNhieu" count="8" />
                </div>
                
                <div class="box-banner-sticky">
                    <div class="box-quangcao">
                        <div class="box-indiv-tmp-1 text-center">
                            <!-- PC.300x600 -->
                            <ins class="adsbygoogle"
                                style="display: inline-block; width: 300px; height: 600px"
                                data-ad-client="ca-pub-3311450421751656"
                                data-ad-slot="8358212457"></ins>
                            <script>
                                (adsbygoogle = window.adsbygoogle || []).push({});
                            </script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
<script type="text/javascript">
    $(document).ready(function () {
        //chen quang cao
        var countp = $(".content-inner").find('p').length;
        if (countp > 6 && countp < 15) {
            $('.content-inner').each(function () {
                var $this = $(this);
                var vitriqc = countp - 4;
                $this.children('p:nth-child(5)').append('<!-- Trong bai viet --><div class="quangcaogoogle"><div class="middle_code_post"><div class="middle_code_post-inside"><ins class="adsbygoogle" style="display: block; text-align: center;"data-ad-layout="in-article"data-ad-format="fluid"data-ad-client="ca-pub-3311450421751656"data-ad-slot="2677822351"></ins><script>(adsbygoogle = window.adsbygoogle || []).push({});</' + 'script></' + 'div></' + 'div></' + 'div>');
            });
        }
        if (countp > 15) {
            $('.content-inner').each(function () {
                var $this = $(this);
                var vitriqc = countp - 4;
                $this.children('p:nth-child(5)').append('<!-- Trong bai viet --><div class="quangcaogoogle"><div class="middle_code_post"><div class="middle_code_post-inside"><ins class="adsbygoogle" style="display: block; text-align: center;"data-ad-layout="in-article"data-ad-format="fluid"data-ad-client="ca-pub-3311450421751656"data-ad-slot="2677822351"></ins><script>(adsbygoogle = window.adsbygoogle || []).push({});</' + 'script></' + 'div></' + 'div></' + 'div>');
                //$this.children('p:nth-child(14)').append('<!-- TTM TrongBai2 --><div class="quangcaogoogle"><div class="middle_code_post"><img src="https://f.thuongtruong.com.vn/IMAGES/2021/09/13/hoinachtt1.png" alt= "" /></' + 'div>');
            });
        }
    });


</script>
