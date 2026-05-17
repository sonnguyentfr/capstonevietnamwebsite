<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="DesktopModules.TinTuc.ViewPage.Details" %>
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
        <div id="box_other" class="news-lastest">
            <div class="relatedblock-heading">
                <div class="block-title">
                    <asp:HyperLink ID="hplCat" runat="server" CssClass="white-color">
                        <%= Localization.GetSafeJSString("sukienkhac.text", Ultis.resourceevents) %>
                    </asp:HyperLink>
                </div>
                <div class="cl"></div>
            </div>
            <div class="otherevent-content">
                <div class="row">
                    <div class="col-md-12">
                        <asp:Repeater ID="drgOtherNews" runat="server">
                            <ItemTemplate>
                                <article class="col-lg-12 mb-10 pb-10">
                                    <div class="post-thumb">
                                        <div class="khungbao"></div>
                                        <div class="khungnen">
                                            <span class="day"><%# Convert.ToDateTime(Eval("fromdatetime")).ToString("dd")%></span>
                                            <span class="date"><%# Convert.ToDateTime(Eval("fromdatetime")).ToString("MM")%>/<%# Convert.ToDateTime(Eval("fromdatetime")).ToString("yyyy")%></span>
                                        </div>
                                    </div>
                                    <div class="post-content-image pr-10 nomobile">
                                        <a href='<%# Ultis.EventsFormatLink(PortalSettings.ActiveTab.TabID, Convert.ToString(Eval("Id")),Convert.ToString(Eval("Title"))) %>' title="<%# ReplaceChuoi.titlenews(Convert.ToString(Eval("Title")))%>">
                                            <img src="/data/no-photo.png?width=200&height=140&mode=crop&anchor=middlecenter" data-src="<%# Ultis.FormatThumbImage(Convert.ToString(Eval("Avatar")), 200, 140, "crop", "middlecenter", "") %>" class="lazy position-relative" alt="<%# ReplaceChuoi.titlenews(Convert.ToString(Eval("Title")))%>" /></a>
                                    </div>
                                    <div class="post-content">
                                        <h3 class="post-title text-limit-3-row">
                                            <a href="<%# Ultis.EventsFormatLink(PortalSettings.ActiveTab.TabID, Convert.ToString(Eval("Id")),Convert.ToString(Eval("Title"))) %>" title="<%# ReplaceChuoi.titlenews(Convert.ToString(Eval("Title")))%>">
                                                <%# Eval("title")%>
                                            </a>
                                        </h3>
                                        <p>
                                            <i class="ti-timer mr-5"></i><%= Localization.GetSafeJSString("thoigian.text", Ultis.resourceevents) %>: <%# Convert.ToDateTime(Eval("fromdatetime")).ToString("HH:mm - dd/MM/yyyy")%>
                                        </p>
                                        <p>
                                            <i class="ti-location-pin"></i><%= Localization.GetSafeJSString("diadiem.text", Ultis.resourceevents) %>: <%# Eval("diadiem")%>
                                        </p>
                                    </div>
                                </article>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </div>
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
<script type="text/javascript" lang="javascript">
    function download(fileId) {
        window.open("/DesktopModules/NVCMS.TinTuc/Display/controls/downloadfile.aspx?fileid=" + fileId);
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
    var addthis_config = { "data_track_addressbar": true };
</script>
