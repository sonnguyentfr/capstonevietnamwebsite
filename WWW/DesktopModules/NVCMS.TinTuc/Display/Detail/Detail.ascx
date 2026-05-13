<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" Explicit="true"
    CodeFile="Detail.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.Detail" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/Controls/Tinlienquan.ascx" TagPrefix="uc" TagName="Tinlienquan" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/DocNhieu.ascx" TagPrefix="uc" TagName="DocNhieu" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/DocNhieuMobile.ascx" TagPrefix="uc" TagName="DocNhieuMobile" %>
<%@ Register TagPrefix="uc" TagName="RELATED" Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/Related.ascx" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/display/Controls/TinMoiNhat.ascx" TagPrefix="uc" TagName="MoiNhat" %>
<%--<%@ Register Src="~/DesktopModules/NVCMS.Video/control/home/TruyenHinh.ascx" TagPrefix="uc" TagName="TruyenHinh" %>--%>
<%@ Register Src="~/DesktopModules/NVCMS.Banner/display/Default.ascx" TagPrefix="uc" TagName="Default" %>
<%@ Register Src="~/Portals/_default/Skins/NVCMSV2/Control/BreadCrumb.ascx" TagPrefix="uc" TagName="BreadCrumb" %>

<%--<%@ Register Src="~/DesktopModules/NVCMS.Video/control/home/Videomoinhat.ascx" TagPrefix="uc" TagName="Videomoinhat" %>--%>

<style type="text/css">
    .meta-social .at-share-btn-elements {
        max-width: 44px;
        margin: 0 auto
    }

    .meta-social .at-resp-share-element .at-share-btn {
        margin-bottom: 10px
    }

    .clearfix::after {
        visibility: hidden;
        display: block;
        font-size: 0;
        content: " ";
        clear: both;
        height: 0
    }   
</style>
<%--<asp:Literal ID="ltrllia" runat="server"></asp:Literal>--%>
<section class="mt-4 mt-lg-5">
    <div class="container">
        <div class="tmp-header-2">
            <h2 class="title-clamp m-0" id="titlebreadcrum"><asp:Literal ID="ltrtitlecat" runat="server"></asp:Literal></h2>
            <div class="sub-menu sp-hid-not-active d-flex text-nowrap scroll-menu">
                <uc:BreadCrumb runat="server" ID="BreadCrumb" />
            </div>
        </div>
        <div class="row-custom mt-3">
            <div class="col-left-8">
                <div class="content-detail">
                    <div class="row mb-4">
                        <div class="col-md-6 txt-date">
                            <asp:Literal ID="lbPublishedDate" runat="server" Text="10:00 PM PDT 9/7/2013"></asp:Literal>
                        </div>
                        <div class="col-md-6 text-md-end txt-social">
                            <%--<script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5d406e7063f2119e"></script>
                            <div class="addthis_inline_share_toolbox"></div>--%>
                        </div>
                    </div>
                    <h1 class="title-clamp-40 text-center">
                        <asp:Literal ID="lbTitle" runat="server"></asp:Literal></h1>
                    <%--<div class="main-tag my-4"><span><asp:Literal ID="ltrbutdanh" runat="server"></asp:Literal></span></div>--%>
                    <div class="content-inner">
                        <p class="title-clamp-20 lh-17 text-center mb-4 fw-bold">
                            <asp:Literal ID="lbSummary" runat="server" Text=""></asp:Literal>
                        </p>
                        <asp:Literal ID="lbContent" runat="server"></asp:Literal>
                    </div>
                    <div class="text-end mb-5">
                        <div>
                            <asp:Literal ID="ltrlinkdan" runat="server"></asp:Literal>
                        </div>
                        <div class="text-muted mt-3 d-flex justify-content-end align-items-center" style="display:none;">
                            <span class="me-2">Theo dõi Thương Trường trên</span>
                            <a href="https://news.google.com/publications/CAAiEHbIDWe7fn4-oh8cG1cnnfwqFAgKIhB2yA1nu35-PqIfHBtXJ538?hl=vi&gl=VN&ceid=VN%3Avi" target="_blank">
                                <img src="/static/nvcmsv2/images/gg-news.png" alt=""></a>
                        </div>
                    </div>
                </div>
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
                <uc:Tinlienquan runat="server" ID="Tinlienquan" />
				<asp:Literal ID="ltrBaiPR" runat="server"></asp:Literal>
                <!-- box most-view sp -->
                <!-- box tags -->
                <div class="box-tags d-flex flex-wrap align-items-center mt-5">
                    <asp:Literal ID="ltrTags" runat="server"></asp:Literal>
                </div>
				<div class="quangcaogoogle"></div>
					<ins class="adsbygoogle"
						 style="display:block"
						 data-ad-format="autorelaxed"
						 data-ad-client="ca-pub-3311450421751656"
						 data-ad-slot="8439838971"></ins>
					<script>
						 (adsbygoogle = window.adsbygoogle || []).push({});
					</script>
                <div class="quangcaogoogle"></div>
					
                <!-- box comment -->
                <div class="box-comment my-5">
                    <div class="cm-form">
                        <asp:UpdatePanel ID="updatesubmitcomment" runat="server">
                            <ContentTemplate>
                                <div class="row g-3">
                                    <div class="col-12">
                                        <textarea class="form-control placeholder-italic" id="txtContent" runat="server" rows="3" placeholder="Ý kiến của bạn"></textarea>
                                    </div>
                                    <div class="col-md-6">
                                        <input type="text" class="form-control" id="txtName" runat="server" placeholder="Họ tên " aria-label="name">
                                    </div>
                                    <div class="col-md-6">
                                        <input type="text" class="form-control" id="txtEmail" runat="server" placeholder="Email" aria-label="Email">
                                    </div>

                                    <div class="col-12">
                                        <div class="row g-3 flex-row-reverse">
                                            <div class="col-12 col-lg-6">
                                                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                                            </div>

                                            <div class="col-12 col-lg-6">
                                                <asp:LinkButton ID="lbtUpdate" ForeColor="White" runat="server" class="btn btn-danger" OnClientClick="return isFormValid();this.disabled = true; this.value = 'please wait ..';" ValidationGroup="Comment" EnableViewState="True">
                                                                    <strong>&nbsp;&nbsp;Gửi bình luận&nbsp;&nbsp;</strong>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <!-- Comment -->
                    <div class="show-comment mt-4">
                        <%--<div class="head-box mt-5">
                            <div class="row">
                                <div class="col-lg-5 fs-20">Bình luận (20)</div>
                                <div class="col-lg-7 fs-16 text-lg-end">
                                    <a class="pe-2" href="#">Sắp xếp theo lượt thích</a>
                                    <span>|</span>
                                    <a class="ps-2 active" href="#">Sắp xếp theo ngày</a>
                                </div>
                            </div>
                        </div>--%>
                        <div class="list-chat">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <asp:Repeater runat="server" ID="rptComment">
                                        <ItemTemplate>
                                            <div class="item-comment mt-4">
                                                <div class="txt-name"><%#DataBinder.Eval(Container.DataItem, "FullName") %></div>
                                                <div class="txt-time mt-1"><%#BL.FormatDate(Eval("CreateDate")) %></div>
                                                <div class="txt-content my-3">
                                                    <%# Server.HtmlDecode(Eval("Content"))%>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <vbuzz:PAGING ID="vbPaging" runat="server" />
                        </div>
                    </div>
                </div>

                <!-- box most-view sp -->
				
				<uc:DocNhieuMobile runat="server" ID="DocNhieuMobile" count="8" />
                

                <!-- list module-tmp-5 -->
                <uc:RELATED runat="server" ID="vbRelated" />

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
    $('[data-fancybox="anhtrongbaiviet"]').fancybox({
        afterLoad: function (instance, current) {
            var pixelRatio = window.devicePixelRatio || 1;

            if (pixelRatio > 1.5) {
                current.width = current.width / pixelRatio;
                current.height = current.height / pixelRatio;
            }
        }
    });
    var addthis_config = addthis_config || {};
    addthis_config.data_track_addressbar = false;
    addthis_config.data_track_clickback = false;
</script>
<script type="text/javascript">
    $(document).ready(function () {
		document.getElementById("titlebreadcrum").scrollIntoView();
        $('#<%=txtContent.ClientId%>').blur(function () {
            if ($(this).val() != "") {
                $("#infocomment").show();
            }
            else {
                $("#infocomment").hide();
            }
        });
        $('#<%=txtContent.ClientId%>').focus(function () {
            $("#infocomment").show();
        });
        //Cat comment dai
        var maxLength = 200;
        $(".show-read-more").each(function () {
            var myStr = $(this).text();
            if ($.trim(myStr).length > maxLength) {
                var newStr = myStr.substring(0, maxLength);
                var removedStr = myStr.substring(maxLength, $.trim(myStr).length);
                $(this).empty().html(newStr);
                $(this).append('<a href="javascript:void(0);" class="read-more"> ...<i class="fa fa-plus-square-o"></i></a>');
                $(this).append('<span class="more-text">' + removedStr + '</span>');
            }
        });
        $(".read-more").click(function () {
            $(this).siblings(".more-text").contents().unwrap();
            $(this).append('<a href="javascript:void(0);" class="read-less">read less...</a>');
            $(this).remove();

        });
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
                
            });
        }
    });
    function isFormValid() {
        var choten = document.getElementById('<%=txtName.ClientID%>').value;
        var Email = document.getElementById('<%=txtEmail.ClientID%>').value;
        var txtContent = document.getElementById('<%=txtContent.ClientID%>').value;
        if (txtContent == "") {
            alert("Bạn chưa nhập nội dung");
            document.getElementById('<%=txtContent.ClientID%>').focus();
            return false;
        }
        if (choten == "") {
            alert("Bạn chưa nhập tên");
            document.getElementById('<%=txtName.ClientID%>').focus();
            return false;
        }
        if (Email == "") {
            alert("Bạn chưa nhập Email");
            document.getElementById('<%=txtEmail.ClientID%>').focus();
            return false;
        }
        else {
            if (validateEmail(Email) == false) {
                alert("Kiểm tra định dạng Email");
                document.getElementById('<%=txtEmail.ClientID%>').focus();
                return false;
            }
        }
        var $captcha = $('#recaptcha'),
            response = grecaptcha.getResponse();
        if (response.length === 0) {
            alert("Bạn vui lòng kiếm tra mã bảo vệ!");
            return false;
        }

    }

</script>
