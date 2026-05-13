<%@ Control Language="vb" AutoEventWireup="false" Explicit="True" Inherits="DotNetNuke.UI.Skins.Skin" %>
<%@ Register Src="~/Portals/_default/Skins/nvcms_vanhoa/Control/HeaderCSS.ascx" TagPrefix="uc1" TagName="HeaderCSS" %>
<%@ Register Src="~/Portals/_default/Skins/nvcms_vanhoa/Control/footerjs.ascx" TagPrefix="uc1" TagName="footerjs" %>
<%@ Register Src="~/Portals/_default/Skins/nvcms_vanhoa/Control/Footer.ascx" TagPrefix="uc1" TagName="Footer" %>
<%@ Register Src="~/Portals/_default/Skins/nvcms_vanhoa/Control/TopHeader.ascx" TagPrefix="uc1" TagName="TopHeader" %>
<uc1:HeaderCSS runat="server" ID="HeaderCSS" />
<script runat="server">
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim fbclid = Request.Item("fbclid")
        Dim sUrl1 As String = Request.RawUrl
        Dim sUrl As String = sUrl1.Replace("?fbclid=" & fbclid, "")
        Dim ItemID = Ultis.GetRequestId(sUrl)
        If ItemID > 0 Then
            LeftTopPane.Visible = False
        End If
    End Sub
</script>
<script>window.location.href = "https://thuongtruong.com.vn";
</script>
<div class="main-wrap" style="display:none;">
    <!-- Main Wrap Start -->
    <uc1:TopHeader runat="server" ID="TopHeader" />
    <main class="position-relative">
        <!--Search Form-->
        <div class="main-search-form transition-02s">
            <div class="container">
                <div class="pt-50 pb-50 main-search-form-cover">
                    <div class="row mb-20">
                        <div class="col-12">
                            <div class="search-form position-relative">
                                <div class="search-form-icon"><i class="ti-search"></i></div>
                                <label>
                                    <input type="text" class="search_field" placeholder="Enter keywords for search..." value="" name="s">
                                </label>
                                <div class="search-switch">
                                    <ul class="list-inline">
                                        <li class="list-inline-item"><a href="#" class="active">Articles</a></li>
                                        <li class="list-inline-item"><a href="#">Authors</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 font-small suggested-area">
                            <p class="d-inline font-small suggested"><strong>Suggested:</strong></p>
                            <ul class="list-inline d-inline-block">
                                <li class="list-inline-item"><a href="#">Covid-19</a></li>
                                <li class="list-inline-item"><a href="#">Health</a></li>
                                <li class="list-inline-item"><a href="#">WFH</a></li>
                                <li class="list-inline-item"><a href="#">UltraNet</a></li>
                                <li class="list-inline-item"><a href="#">Hospital</a></li>
                                <li class="list-inline-item"><a href="#">Policies</a></li>
                                <li class="list-inline-item"><a href="#">Energy</a></li>
                                <li class="list-inline-item"><a href="#">Business</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!--Featured post Start-->
        <div class="home-featured">
            <div id="LeftTopPane" runat="server"></div>
        </div>
        <%--<div class="recent-area pt-50">
            <div class="container">
                <div class="widgets-post-carausel-1 mb-60">
                    <div class="post-carausel-1 border-radius-10 bg-white">
                        <div class="row no-gutters">
                            <div class="col col-1-5 background6 editor-picked-left d-none d-lg-block">
                                <div class="editor-picked">
                                    <h4>Tin đọc nhiều</h4>
                                    <p class="font-medium color-grey mt-20 mb-30">Tin bài mục <strong>Văn hóa - Giải trí </strong>được bạn đọc quan tâm</p>
                                    <a href="#" class="read-more">Read More</a>
                                    <div class="post-carausel-1-arrow"></div>
                                </div>
                            </div>
                            <div class="col col-4-5 col-md-12">
                                <div class="post-carausel-1-items row">
                                    <div class="slider-single col">
                                        <h6 class="post-title pr-5 pl-5 mb-10 text-limit-2-row"><a href="single.html">The Writer’s Dilemma — For Money or Love of the Game?</a></h6>
                                        <div class="img-hover-scale border-radius-5 hover-box-shadow">
                                            <span class="top-right-icon background2">
                                                <i class="mdi mdi-audiotrack"></i>
                                            </span>
                                            <a href="single.html">
                                                <img class="border-radius-5" src="/static/nvcms_vanhoa/imgs/thumbnail-9.jpg" alt="post-slider"></a>
                                        </div>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on">02 Jan</span>
                                            <span class="hit-count has-dot">23k Views</span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                    <!--end slider single-->
                                    <div class="slider-single col">
                                        <h6 class="post-title pr-5 pl-5 mb-10 text-limit-2-row"><a href="single.html">Conceptual Art: A Beginner’s Guide</a></h6>
                                        <div class="img-hover-scale border-radius-5 hover-box-shadow">
                                            <a href="single.html">
                                                <img class="border-radius-5" src="/static/nvcms_vanhoa/imgs/thumbnail-3.jpg" alt="post-slider"></a>
                                        </div>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on">14 Feb</span>
                                            <span class="hit-count has-dot">59k Views</span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                    <!--end slider single-->
                                    <div class="slider-single col">
                                        <h6 class="post-title pr-5 pl-5 mb-10 text-limit-2-row"><a href="single.html">One of the All-Time Cartooning Greats</a></h6>
                                        <div class="img-hover-scale border-radius-5 hover-box-shadow">
                                            <span class="top-right-icon background10">
                                                <i class="mdi mdi-camera-alt"></i>
                                            </span>
                                            <a href="single.html">
                                                <img class="border-radius-5" src="/static/nvcms_vanhoa/imgs/thumbnail-4.jpg" alt="post-slider">
                                            </a>
                                        </div>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on">25 Feb</span>
                                            <span class="hit-count has-dot">72k Views</span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                    <!--end slider single-->
                                    <div class="slider-single col">
                                        <h6 class="post-title pr-5 pl-5 mb-10 text-limit-2-row"><a href="single.html">9 Things I Love About Shaving My Head During Quarantine</a></h6>
                                        <div class="img-hover-scale border-radius-5 hover-box-shadow">
                                            <a href="single.html">
                                                <img class="border-radius-5" src="/static/nvcms_vanhoa/imgs/thumbnail-5.jpg" alt="post-slider"></a>
                                        </div>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on">15 May</span>
                                            <span class="hit-count has-dot">159k Views</span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                    <!--end slider single-->
                                    <div class="slider-single col">
                                        <h6 class="post-title pr-5 pl-5 mb-10 text-limit-2-row"><a href="single.html">Could He Still Love Me Without My Eating Disorder?</a></h6>
                                        <div class="img-hover-scale border-radius-5 hover-box-shadow">
                                            <span class="top-right-icon background3">
                                                <i class="mdi mdi-videocam"></i>
                                            </span>
                                            <a href="single.html">
                                                <img class="border-radius-5" src="/static/nvcms_vanhoa/imgs/thumbnail-6.jpg" alt="post-slider"></a>
                                        </div>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on">05 April</span>
                                            <span class="hit-count has-dot">35k Views</span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                    <!--end slider single-->
                                    <div class="slider-single col">
                                        <h6 class="post-title pr-5 pl-5 mb-10 text-limit-2-row"><a href="single.html">The Quiet Prejudice of ‘You Are Not Fat, You Have Fat’</a></h6>
                                        <div class="img-hover-scale border-radius-5 hover-box-shadow">
                                            <a href="single.html">
                                                <img class="border-radius-5" src="/static/nvcms_vanhoa/imgs/thumbnail-7.jpg" alt="post-slider"></a>
                                        </div>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on">14 April</span>
                                            <span class="hit-count has-dot">30k Views</span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                    <!--end slider single-->
                                    <div class="slider-single col">
                                        <h6 class="post-title pr-5 pl-5 mb-10 text-limit-2-row">
                                            <a href="single.html">Take The Damn Body Compliments</a></h6>
                                        <div class="img-hover-scale border-radius-5 hover-box-shadow">
                                            <a href="single.html">
                                                <img class="border-radius-5" src="/static/nvcms_vanhoa/imgs/thumbnail-8.jpg" alt="post-slider"></a>
                                        </div>
                                        <div class="entry-meta meta-1 font-small color-grey mt-10 pr-5 pl-5">
                                            <span class="post-on">25 April</span>
                                            <span class="hit-count has-dot">26k Views</span>
                                            <a class="float-right" href="#"><i class="ti-heart"></i></a>
                                        </div>
                                    </div>
                                    <!--end slider single-->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>

        <!--  Recent Articles start -->
        <div class="recent-area background12">
            <div class="container">
                <div class="row">
                    <div class="col-lg-9 col-md-12 pt-50 pb-50 ">
						<div id="LeftPane" runat="server"></div>
                    </div>
                    <div class="col-lg-3 col-md-12 col-sm-12 primary-sidebar sticky-sidebar pt-50 pb-50 " style="background:#fff;">
                        <div class="widget-area">
                            <div id="RightTopPane" runat="server"></div>
                            <div class='quangcaogoogle' style='text-align: center;'>
                                <!-- PC.300x600 -->
                                <ins class="adsbygoogle"
                                    style="display: inline-block; width: 300px; height: 600px"
                                    data-ad-client="ca-pub-3311450421751656"
                                    data-ad-slot="8358212457"></ins>
                                <script>
                                    (adsbygoogle = window.adsbygoogle || []).push({});
                                </script>
                                <!------>
                            </div>
                            <div id="RightBottonPane" runat="server"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="DienAnhPane" runat="server"></div>
        <div class='quangcaogoogle pt-50' style='text-align: center;'>
            <!-- PC.970x90 -->
            <ins class="adsbygoogle"
                style="display: inline-block; width: 970px; height: 90px"
                data-ad-client="ca-pub-3311450421751656"
                data-ad-slot="8429530491"></ins>
            <script>
                (adsbygoogle = window.adsbygoogle || []).push({});
            </script>
        </div>
        <div class="pt-50 pb-50 background-white">
            <div id="ContentPane" runat="server"></div>
        </div>
        <!--Recent Articles End -->
    </main>
    <uc1:Footer runat="server" ID="Footer" />
</div>
<uc1:footerjs runat="server" ID="footerjs" />
