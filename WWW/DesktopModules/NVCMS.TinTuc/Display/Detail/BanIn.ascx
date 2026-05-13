<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" Explicit="true"
    CodeFile="BanIn.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.Detail" %>
<script src="//cdn.thuongtruong.com.vn/thuongtruong/js/jquery.swipebox.js"></script>
<link rel="stylesheet" href="//cdn.thuongtruong.com.vn/thuongtruong/css/swipebox.css">

<div class="std">
    <div class="inner">
        <div class="row mt-15">
            <div class="col-xs-12 auto-padding-right">
                <article class="mb-15">
                    <h1 class="post-title title-color">
                        <asp:Literal ID="lbTitle" runat="server"></asp:Literal></h1>
                    <div class="row mt-10">
                        <div class="col-xs-12">
                            <div class="post-description">
                                <div class="website-typo">
                                    <asp:HyperLink ID="hpltcatlink" runat="server" CssClass="white-color"></asp:HyperLink>
                                </div>
                                <h2 class="post-sapo">
                                    <strong class="red-color"><a class="red-color" href="http://thuongtruong.com.vn/">Thương Trường | </a></strong>
                                    <asp:Literal ID="lbSummary" runat="server" Text=""></asp:Literal></h2>
                            </div>
                            <div class="post-tools">
                                <div class="pull-left inline-block mt-5">
                                </div>
                                <div class="pull-left inline-block mt-5">
                                </div>
                                <div class="pull-left mt-5 ml-10">
                                    &nbsp; <i class="fa fa-clock-o"></i>
                                    <asp:Literal ID="lbPublishedDate" runat="server" Text="0"></asp:Literal>
                                </div>
                                <div class="pull-right">
                                </div>
                                <div class="cl"></div>
                            </div>
                            <div class="post-content" id="NewsDetailX">
                                <uc:Tinlienquan runat="server" ID="Tinlienquan" />
                                <asp:Label ID="lbContent" runat="server"></asp:Label>
                                <div class="contact-box mb-15 pd-10" style="background-color: #f3f3f3">
                                    Bạn đang đọc bài viết
                                    <asp:HyperLink ID="hptcontact" runat="server" CssClass="red-color"></asp:HyperLink>
                                    tại chuyên mục 
                                <asp:HyperLink ID="hptcontact2" runat="server" CssClass="red-color"></asp:HyperLink>
                                    của <a href="http://thuongtruong.com.vn/">Tạp chí Điện tử Thương Trường</a>. Mọi thông tin góp ý và chia sẻ, xin vui lòng liên hệ SĐT: <a href="#">0913398394</a>  hoặc gửi về hòm thư <a href="#">toasoanthuongtruong@gmail.com</a>
                                </div>
                            </div>

                            <div class="related-tags">
                                <asp:Literal ID="ltrTags" runat="server"></asp:Literal>
                                <div class="cl"></div>
                            </div>
                        </div>
                    </div>
                </article>
                <div class="cl"></div>
            </div>

        </div>
    </div>
</div>
<div id="limit-right-scroll"></div>

<script type="text/javascript">
    ; (function ($) {
        $('.newsslidephoto').swipebox();
    })(jQuery);
</script>

