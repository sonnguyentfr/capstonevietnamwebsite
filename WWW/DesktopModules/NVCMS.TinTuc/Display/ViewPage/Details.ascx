<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Details.ascx.cs" Inherits="DesktopModules.TinTuc.ViewPage.Details" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<%--<%@ Register TagPrefix="vbuzz" TagName="LASTEST" Src="~/DesktopModules/NVCMS.TinTuc/Control/Lastest.ascx" %>--%>
<%@ Register TagPrefix="vbuzz" TagName="RELATED" Src="~/DesktopModules/NVCMS.TinTuc/Display/Controls/Related.ascx" %>
<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
        <div class="news-details">
            <asp:Literal ID="ltContent" runat="server" />
        </div>
        <!-- tag -->
        <asp:Panel ID="panelTag" Visible="false" runat="server">
            <div id="list_tags">
                <span class="title-tag">Tags </span>
                <asp:Repeater runat="server" ID="rptTags">
                    <ItemTemplate>
                        <a href='<%# DotNetNuke.Common.Globals.NavigateURL(BL.tabTags) + "?tag=" + Eval("TermID") %>'>
                            <%# Eval("Name") %></a>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        ,
                    </SeparatorTemplate>
                </asp:Repeater>
            </div>
            <!-- End tag-->
        </asp:Panel>
        
        <asp:Panel runat="server" ID="panelComment" Visible="false">
            <!-- Comment list -->
            <div id="box_comment" class="comment">
                <h3 class="title-cm"><b>
                    <asp:Literal ID="lbliCount" runat="server" /></b> Bình luận</h3>
                <ul class="list-comment">
                    <asp:Repeater runat="server" ID="rptComment">
                        <ItemTemplate>
                            <li>
                                <p class="info-cmt">
                                    <span class="name"><%#DataBinder.Eval(Container.DataItem, "FullName") %></span>
                                    <span class="time"><%# ((DateTime)DataBinder.Eval(Container.DataItem, "CreateDate")).ToShortDateString() %></span>
                                </p>
                                <p class="title-cmt"><%#DataBinder.Eval(Container.DataItem, "Title") %></p>
                                <div class="content-cmt"><%# DataBinder.Eval(Container.DataItem, "Content")%></div>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <vbuzz:PAGING ID="vbPaging" runat="server" />
                <div class="meta-cm">
                    <a href="#" id="btnLoadFormComment" class="ykiencuaban">Gửi ý kiến của bạn</a>
                </div>
            </div>
            <!-- End Comment  list -->
            <!-- Form comment-->
            <asp:UpdateProgress runat="server">
                <ProgressTemplate>
                    <asp:Image ImageUrl="~/images/icon_wait.gif" runat="server" Height="30px" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel runat="server" ID="upformcoment">
                <ContentTemplate>
                    <div class="form-comment clearfix" id="form-comment" style="display: none;">
                        <table cellpadding="4" border="0" style="width: 100%">
                            <tr>
                                <td style="width: 100px">
                                    <asp:Label ID="lbFullName" runat="server" Text="Tên của bạn" />
                                    <span class='required'>(*)</span>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtName" EnableViewState="True" Width="350px" placeholder="Tên của bạn" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbEmail" runat="server" Text="Email" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" Width="350px" EnableViewState="true"  placeholder="Email của bạn"/></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbTitle" runat="server" Text="Tiêu đề" />
                                    <span class='required'>(*)</span>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTitle" runat="server" Width="350px" EnableViewState="true" placeholder="Tiêu đề" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbContent" runat="server" Text="Nội dung" />
                                    <span class='required'>(*)</span></td>
                                <td>
                                    <asp:TextBox ID="txtContent" runat="server" CssClass="form-control" TextMode="MultiLine"
                                        Rows="4" EnableViewState="True" Width="350px" placeholder="Nội dung" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbCaptcha" runat="server" Text="Mã bảo vệ" />
                                    <span class='required'>(*)</span>
                                </td>
                                <td>
                                    <div style="height: 80px">
                                        <dnn:CaptchaControl ID="ctlCaptcha" CaptchaLength="3" CaptchaWidth="80" CaptchaHeight="30"
                                            CssClass="Normal" ErrorStyle-CssClass="dnnFormMessage dnnFormError" runat="server"
                                            ViewStateMode="Enabled" />
                                    </div>
                                    <div class="clear"></div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:LinkButton ID="linkSendComment" CssClass="button btn-send" OnClientClick="return isFormValid();" runat="server" Text="Gủi bình luận" OnClick="linkSendComment_Click" />
                                    <a class="button" id="btn_close" href="#">Đóng</a>
                                </td>
                            </tr>
                        </table>
                        <p>
                            <asp:Label ID="lbMessage" runat="server" />
                        </p>
                    </div>
                    <!-- End form comment-->
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel runat="server" ID="updateloingcoment" Visible="false">
                <ContentTemplate>
                    <div class="form-commentlogin clearfix" id="form-comment" style="display: none;">
                        <a href="/dang-nhap">Đăng Nhập để bình luận</a>
                    </div>
                    <!-- End form comment-->
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <div id="box_more" class="news-lastest">
            <!-- Tin moi cap nhap -->
            <%--<vbuzz:LASTEST runat="server" ID="vbLastest" />--%>
            <!-- End Tin moi cap nhap -->
        </div>
        <div id="box_other" class="news-lastest">
            <!-- Tin khac -->
            <vbuzz:RELATED runat="server" ID="vbRelated" />
            <!-- End Tin moi cap nhap -->
        </div>
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div class="loading" id="loadizng">Loading&#8230;</div>
    </ProgressTemplate>
</asp:UpdateProgress>
<%--<script type="text/javascript" src="/js/inverts.js"></script>--%>
<script type="text/javascript" lang="javascript">
    function download(fileId) {
        window.open("/controls/downloadfile.aspx?fileid=" + fileId);
    }
    $(document).ready(function () {
        //Button control
        //$('#link_invert').click(function () {
        //    if($('#wrapper').hasClass('invert'))
        //        $('#wrapper').removeClass('invert');
        //    else 
        //        $('#wrapper').addClass('invert'); 
        //});

        //Thay the related news
        var related_text = $('#list_related').html();
        $('#box_related').html(related_text);
        $('#list_related').html('');

        //Thay the tags
        var tag_text = $('#list_tags').html();
        $('#box_tags').html(tag_text);
        $('#list_tags').html('');
    });
    function Reset() {
        $(':input').val('');
        return false;
    }
    $('#btnLoadFormComment').click(function () {
        $("#form-comment").toggle();
        return false;
    });
    $('#btn_close').click(function () {
        $('#form-comment').slideUp('fast');
        return false;
    });
    function closeFrom() {
        Reset();
        $('#form-comment').slideUp('fast');
    }
    function scrollToAnchor(aid) {
        var aTag = $("a[name='" + aid + "']");
        $('html,body').animate({ scrollTop: aTag.offset().top }, 'slow');
    }
    function show_page(page) {
        scrollToAnchor('content-page');
        $('#list_page_content li a').removeClass('active');
        $('#list_page_content li #link_page_' + page).addClass('active');
        $('#list_box_content .box-content-item').hide();
        if (page > 0)
            $('#list_box_content #content_page').html($('#list_box_content #box_content_' + page).html());
        else {
            $('#list_box_content #content_page').html('');
            $('#list_box_content #box_content_' + page).show();
        }
        //$('#list_box_content #box_content_' + page).show();
        return false;
    }
    function isFormValid() {
        var res = true;
        //Tiêu đề        
        if (required($("#<%=txtName.ClientID %>").val()) == false) {
            sendError('<%=txtName.ClientID %>', 'Bạn phải nhập tên.');
            res = false;
        }
        else {
            clearError('<%=txtName.ClientID %>');
        }
        //Nội dung       
        if (required($("#<%=txtContent.ClientID %>").val()) == false) {
            sendError('<%=txtContent.ClientID %>', 'Bạn phải nhập nội dung.');
            res = false;
        }
        else {
            clearError('<%=txtContent.ClientID %>');
        }

        return res;
    }
    function sendError(controlID, txtError) {
        if ($("#errlbl_" + controlID).html() == null) {
            $("#" + controlID).after("<p id='errlbl_" + controlID + "' class='AICommentErrorLabel'>" + txtError + "<font style='color:red;'> (*)</font>" + "</p>");
        }
        else {
            $("#errlbl_" + controlID).text(txtError);
        }
    }
    function clearError(controlID) {
        $("#errlbl_" + controlID).remove();
    }
    function checkEnter(event) {
        var keyCode;
        if (window.event) {
            keyCode = event.keyCode;  // voi IE
        }
        else {
            keyCode = event.which;     // voi Firefox
        }
        if (keyCode == 13) {
            $("#btnSendComment").click();
        }
    }
</script>

<script type="text/javascript">
    function share_zing() { var u = location.href; window.open("http://link.apps.zing.vn/share?u=" + encodeURIComponent(u)); }
    function share_linkhay() { var u = location.href; window.open("http://linkhay.com/submit?url=" + encodeURIComponent(u)); }
    function share_twitter() { var u = location.href; t = document.title; window.open("http://twitter.com/home?status=" + encodeURIComponent(u)); }
    function share_facebook() { var u = location.href; t = document.title; window.open("http://www.facebook.com/share.php?u=" + encodeURIComponent(u) + "&t=" + encodeURIComponent(t)); }
    function share_google() { var u = location.href; t = document.title; window.open("http://www.google.com/bookmarks/mark?op=edit&bkmk=" + encodeURIComponent(u) + "&title=" + t + "&annotation=" + t); }
    function print_page() {
        <%--var u = '<%=Ultis.FormatLink(BL.tabPrint,ItemID,"tin-bai") %>';
        window.open(u, '_blank', 'width=800, height=700');--%>
    }

    var addthis_config = { "data_track_addressbar": true };
</script>