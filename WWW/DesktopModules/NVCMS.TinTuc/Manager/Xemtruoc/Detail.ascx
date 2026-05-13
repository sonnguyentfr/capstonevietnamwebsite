<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" Explicit="true"
    CodeFile="Detail.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.AdminxemTruoc" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/Controls/Tinlienquan.ascx" TagPrefix="uc" TagName="Tinlienquan" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/Controls/DocNhieu.ascx" TagPrefix="uc" TagName="DocNhieu" %>
<%@ Register TagPrefix="uc" TagName="RELATED" Src="~/DesktopModules/NVCMS.TinTuc/Display/Controls/Related.ascx" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/Controls/TinMoiNhat.ascx" TagPrefix="uc" TagName="MoiNhat" %>
<%--<%@ Register Src="~/DesktopModules/NVCMS.Video/control/home/TruyenHinh.ascx" TagPrefix="uc" TagName="TruyenHinh" %>--%>
<%@ Register Src="~/DesktopModules/NVCMS.Banner/display/Default.ascx" TagPrefix="uc" TagName="Default" %>
<%--<%@ Register Src="~/DesktopModules/NVCMS.Video/control/home/Videomoinhat.ascx" TagPrefix="uc" TagName="Videomoinhat" %>--%>

<div class="row no-gutter newsdetail">
    <!--========== BEGIN .COL-MD-8 ==========-->
    <div class="col-md-9">
        <div class="row">
            <div class="col-xs-12 col-sm-9 col-md-9">
                <div class="post post-full clearfix">
                    <div class="entry-main">
                        <div class="entry-title">
                            <h4 class="entry-title">
                                <asp:Literal ID="lbTitle" runat="server"></asp:Literal></h4>
                        </div>

                        <div class="post-meta-elements">
                            <div class="post-meta-author">
                                <i class="fa fa-eye"></i>
                                <asp:Literal ID="ltrviewcount" runat="server" Text="10:00 PM PDT 9/7/2013"></asp:Literal>
                            </div>
                            <div class="post-meta-date">
                                <i class="fa fa-calendar"></i>
                                <asp:Literal ID="lbPublishedDate" runat="server" Text="10:00 PM PDT 9/7/2013"></asp:Literal>
                            </div>
                            <div class="post-meta-comments">
                                <i class="fa fa-comment-o"></i><a href="#">
                                    <asp:Literal ID="ltrbinhluan" runat="server"></asp:Literal>
                                    bình luận</a>
                            </div>
                            <div class="post-meta-socialshare">
                                <script type="text/javascript">
                                    document.write("<div class='fb-like' data-size='small' data-href='" + window.location.href + "' data-layout='button_count' data-action='like' data-size='small' data-show-faces='false' data-share='false'></div>");
                                </script>
                            </div>
                            <div class="post-meta-social">
                                <script type="text/javascript" src="//s7.addthis.com/js/300/addthis_widget.js#pubid=ra-5ee6ffdc16441c27"></script>
                                <div class="addthis_inline_share_toolbox"></div>

                            </div>
                        </div>
                        <div class="entry-content">
                            <p>
                                <strong>
                                    <asp:Literal ID="lbSummary" runat="server" Text=""></asp:Literal>
                                </strong>
                            </p>
                            <uc:Tinlienquan runat="server" ID="Tinlienquan" />
                            <asp:Literal ID="lbContent" runat="server"></asp:Literal>
                            <asp:Literal ID="ltrlinkdan" runat="server"></asp:Literal>
                        </div>
                        <div class="related-tags">
                            <asp:Literal ID="ltrTags" runat="server"></asp:Literal>
                            <div class="cl"></div>
                        </div>
                    </div>
                </div>
                <div class="form-reply-section">
                    <div class="comment-title title-style01">
                        <h4>Bình luận bài viết</h4>
                    </div>
                    <div class="form-reply ui-form" action="#" method="post">
                        <div class="row no-gutter">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <input value="" placeholder="Họ tên của bạn" id="txtName" runat="server" type="text" class="form-control" name="data[name]" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <input id="txtEmail" runat="server" placeholder="Phản hồi khi cần thiết" class="form-control" name="data[email]" />
                            </div>
                        </div>
                        <div class="row no-gutter">
                            <div class="col-md-12">
                                <textarea data-minlength="10" id="txtContent" runat="server" placeholder="Cho chúng tôi biết ý kiến của bạn" rows="3" data-error="Bình luận cần có 10 ký tự trở lên" class="form-control comment" style="overflow: hidden; word-wrap: break-word; resize: horizontal; height: 74px;"></textarea>
                            </div>
                        </div>
                        <div class="row no-gutter">
                            <div class="col-md-12">
                                <asp:Panel ID="Panel1" runat="server"></asp:Panel>
                            </div>
                        </div>
                        <div class="row no-gutter">
                            <div class="col-md-12">
                                <asp:LinkButton ID="lbtUpdate" ForeColor="White" runat="server" class="btn btn-primary btn-black" OnClientClick="return isFormValid();this.disabled = true; this.value = 'please wait ..';" ValidationGroup="Comment" EnableViewState="True">
                                                                    <strong>&nbsp;&nbsp;Gửi bình luận&nbsp;&nbsp;</strong>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="comment-section">
                    <!-- Begin .title-style01 -->
                    <div class="comment-title title-style01">
                        <h4>Bình luận</h4>
                    </div>
                    <!-- End .title-style01 -->
                    <ul class="comments-list">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Repeater runat="server" ID="rptComment">
                                    <ItemTemplate>
                                        <li>
                                            <div class="comment clearfix">
                                                <div class="avatar">
                                                </div>
                                                <div class="comment-content">
                                                    <div class="comment-title">
                                                        <h5 class="comment-author"><strong><i class="fa fa-user"></i>&nbsp;<%#DataBinder.Eval(Container.DataItem, "FullName") %></strong></h5>
                                                        <div class="comment-date"><i class="fa fa-clock-o"></i><span class="day"><%#Ultis.ToRelativeDate(Eval("CreateDate")) %></span></div>
                                                    </div>
                                                    <p><%# Server.HtmlDecode(Eval("Content"))%></p>
                                                    <div class="panel-like" style="display:none;">
                                                        <i class="fa fa-thumbs-up"></i>
                                                        <asp:Button ID="btlike" CommandArgument='<%#Eval("NewsFeedbackId") %>' CommandName="btnlike" Text="Thích" OnClick="btnlike" CssClass="combtn blue" runat="server" />
                                                        <i class="fa fa-thumbs-down"></i>
                                                        <asp:Button ID="btdislike" CommandArgument='<%#Eval("NewsFeedbackId") %>' CommandName="btdislike" Text="Không Thích" OnClick="btdislike" CssClass="combtn" runat="server" />
                                                        <i class="fa fa-flag"></i>
                                                        <asp:Button ID="btReport" CommandArgument='<%#Eval("NewsFeedbackId") %>' CommandName="btReport" Text="Báo xấu" OnClick="btReport" CssClass="combtn red" runat="server" />
                                                        <asp:Label ID="lblNewsFeedbackId" Text='<%#Eval("NewsFeedbackId") %>' Visible="false" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ul>
                    <div class="clearfix pagination-wp">
                        <ul class="pagination pull-left">
                            <vbuzz:PAGING ID="vbPaging" runat="server" />
                        </ul>
                        <div class="cl"></div>
                    </div>
                </div>
                <uc:RELATED runat="server" ID="vbRelated" />
            </div>
            <div class="col-xs-12 col-sm-3 col-md-3">
                <div class='sidebar-weather'>
                    <uc:Default runat="server" ID="Default" vitri="6" />
                    <uc:Default runat="server" ID="Default1" vitri="7" />
                    <uc:Default runat="server" ID="Default2" vitri="8" />
                </div>
            </div>
        </div>
    </div>
    <div class="col-md-3">
        <uc:MoiNhat runat="server" ID="MoiNhat" count="8" />
        <uc:DocNhieu runat="server" ID="DocNhieu" count="8" />
    </div>
    <div class="col-md-12 mt-20">
        <%--<uc:Videomoinhat runat="server" ID="Videomoinhat" count="8" />--%>
    </div>
</div>
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
<script type="text/javascript">
    $(window).load(function () {
        $('img').each(function () {
            if (!this.complete || typeof this.naturalWidth == "undefined" || this.naturalWidth == 0) {
                this.src = '/DATA/noimage.png';
            }
        });
    });
</script>
