<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Explicit="true" CodeFile="CapV2_HomeTruongDoiTac.ascx.vb" Inherits="NVCMS.Modules.School.IndexSearch" %>
<section class="ttm-row testimonial-section ttm-bgcolor-skincolor bg-img5 ttm-bg ttm-bgimage-yes clearfix">
    <div class="ttm-row-wrapper-bg-layer ttm-bg-layer"></div>
    <div class="">
        <!-- row -->
        <div class="row">
            <div class="col-lg-12">
                <!-- section title -->
                <div class="section-title title-style-center_text">
                    <div class="title-header">
                        <h2 class="title">DANH SÁCH TRƯỜNG ĐỐI TÁC</h2>
                    </div>
                    <div class="heading-seperator"><span></span></div>
                </div>
                <!-- section title end -->
            </div>
        </div>
        <!-- row end -->
        <!-- slick_slider -->
        <div class="row slick_slider" data-slick='{"slidesToShow": 7, "slidesToScroll": 7, "arrows":false, "autoplay":true, "centerMode":false, "centerPadding":0, "infinite":true, "initialSlide":2, "responsive": [{"breakpoint":870,"settings":{"slidesToShow": 1}} , {"breakpoint":525,"settings":{"slidesToShow": 1}}]}'>
            <asp:Repeater ID="rptContent" runat="server">
                <ItemTemplate>
                    <div class="ttm-box-col-wrapper col-lg-12">
                        <!-- testimonials -->
                        <div class="testimonials ttm-testimonial-box-view-style2">
                            <div class="testimonial-content">
                                <div class="testimonial-avatar">
                                    <div class="testimonial-img">
                                        <a href="<%# Ultis.FormatLink_School(4100, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>">
                                            <img src="/static/capstonev3/images/event/event-img-one.jpg" data-src="<%#Ultis.FormatThumbImage(Eval("Logo"), 250, 250, "constrain", "middlecenter", "") %>" alt="<%#Eval("NameofSchool") %>" class="blur-up img-fluid lazyload" /></a>
                                    </div>
                                </div>
                                <div class="testimonial-caption">
                                    <h5>
                                        <a href="<%# Ultis.FormatLink_School(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>"><%#Eval("NameofSchool") %></a>
                                    </h5>
                                </div>
                            </div>
                        </div>
                        <!-- testimonials end -->
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</section>
