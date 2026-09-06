<%@ Control Language="VB" AutoEventWireup="false" CodeFile="NewsConfig.ascx.vb" Inherits="DesktopModules.TinTuc.Configurations.NewsConfigPC" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Manager/controls/tinlienquan.ascx" TagPrefix="uc1" TagName="tinlienquan" %>
<%--<script src="//cdn.thuongtruong.com.vn/_Admin/vendors/ipad/jquery.mobile-1.4.0-alpha.2.min.js"></script>
<script src="//cdn.thuongtruong.com.vn/_Admin/vendors/ipad/jquery-ui.min.js"></script>
<script src="//cdn.thuongtruong.com.vn/_Admin/vendors/ipad/jquery.ui.touch-punch.min.js"></script>--%>
<%--<asp:UpdatePanel ID="upCongif" runat="server">
    <ContentTemplate>--%>
        <div class="nav-tabs-custom">
            <ul class="nav nav-tabs">
                <li class="active"><a href="#activity" data-toggle="tab">Chọn danh sách Tin chọn</a></li>
                <li><a href="#tags" data-toggle="tab">Nhập TAGS hiện thị trên top</a></li>
            </ul>
            <div class="tab-content">
                <div class="active tab-pane" id="activity">
                    Loại cấu hình: 
                    <asp:DropDownList runat="server" ID="drlSettings" AutoPostBack="True" OnSelectedIndexChanged="drlSettings_SelectedIndexChanged">
                        <asp:ListItem Value="1" Text="Tin Nổi Bật"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Tin Nóng Top"></asp:ListItem>
                        <asp:ListItem Value="3" Text="Xu Hướng Đọc"></asp:ListItem>
                    </asp:DropDownList>
                    <div id="select" runat="server">
                        <a class="btn btn-primary" href="javascript:void(0);" onclick="openTinLienQuanDrawer();">Chọn Tin</a>
                    </div>
                    <div class="list-lq col-sm-12" id="divrelated">
                        <ul data-role="listview" data-inset="true" data-theme="d" id="sortable" class="to_do">
                            <asp:Repeater runat="server" ID="rptSettings">
                                <ItemTemplate>
                                    <li class="tinlienquanli" title="Xóa" data-id="<%# Eval("NewId")%>"><strong><%# Eval("Title")%> <%# Eval("NewId")%></strong><span class="removeSelected"><em class="icon ni ni-trash-alt"></em></span>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <asp:LinkButton ID="lbtUpdateOrder" CssClass="btn btn-primary" runat="server" OnClientClick="updateFormValues(); return true;"><i class="fa fa-save"></i> Cập nhật</asp:LinkButton>
                    <asp:HiddenField ID="hdf_Value" runat="server" />
                </div>
                <div class="tab-pane" id="tags">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <input type="text" value="" data-role="tagsinput" runat="server" id="settingsTAGS" class="form-control" />
                            <i>Các từ khóa cách nhau dấu <font style="color: red; font-weight: bold; font-size: 19px;">PHẨY (,)</font></i>
                        </div>
                    </div>
                    <p>
                        <asp:LinkButton ID="lbtUpdate" CssClass="btn btn-primary" runat="server"><i class="fa fa-save"></i> Cập nhật</asp:LinkButton>
                    </p>
                </div>
                <!-- /.tab-pane -->
            </div>
            <!-- /.tab-content -->
        </div>

        <%--== Chèn tin liên quan vào bài ==--%>
<style type="text/css">
    .tlq-drawer-overlay {
        position: fixed;
        inset: 0;
        background: rgba(0, 0, 0, 0.35);
        z-index: 1050;
        display: none;
    }

    .tlq-drawer {
        position: fixed;
        top: 0;
        right: 0;
        height: 100vh;
        width: min(920px, 92vw);
        background: #fff;
        box-shadow: -6px 0 24px rgba(0, 0, 0, 0.15);
        z-index: 1060;
        transform: translateX(100%);
        transition: transform .25s ease;
        display: flex;
        flex-direction: column;
    }

    .tlq-drawer.open {
        transform: translateX(0);
    }

    .tlq-drawer-header,
    .tlq-drawer-footer {
        padding: 10px 14px;
        border-bottom: 1px solid #eef0f2;
    }

    .tlq-drawer-footer {
        border-top: 1px solid #eef0f2;
        border-bottom: 0;
        text-align: right;
    }

    .tlq-drawer-body {
        padding: 10px 14px;
        overflow: auto;
        flex: 1;
    }

    .tlq-drawer-title {
        margin: 0;
        font-size: 16px;
        font-weight: 600;
    }
</style>

<div id="boxtinlienquanOverlay" class="tlq-drawer-overlay" onclick="closeTinLienQuanDrawer();"></div>
<div id="boxtinlienquan" class="tlq-drawer" role="dialog" aria-hidden="true" aria-labelledby="myModalLabel">
    <div class="tlq-drawer-header d-flex align-items-center justify-content-between">
        <h4 class="tlq-drawer-title" id="myModalLabel">Chọn tin liên quan</h4>
        <button type="button" class="close" onclick="closeTinLienQuanDrawer();" aria-label="Close"><span aria-hidden="true">×</span></button>
    </div>
    <div class="tlq-drawer-body">
        <uc1:tinlienquan runat="server" ID="tinlienquan" />
    </div>
    <div class="tlq-drawer-footer">
        <button type="button" class="btn btn-default" onclick="closeTinLienQuanDrawer();">Đóng</button>
    </div>
</div>
<script type="text/javascript">
    function openTinLienQuanDrawer() {
        $('#boxtinlienquanOverlay').show();
        $('#boxtinlienquan').addClass('open').attr('aria-hidden', 'false');
        $('body').css('overflow', 'hidden');
    }

    function closeTinLienQuanDrawer() {
        $('#boxtinlienquan').removeClass('open').attr('aria-hidden', 'true');
        $('#boxtinlienquanOverlay').hide();
        $('body').css('overflow', '');
    }

    $(document).ready(function () {
        $("#sortable").sortable({
            items: "li:not(.ui-li-divider)"
        });
        $("#sortable").sortable();
        $("#sortable").disableSelection();
        $("#sortable").bind("sortstop", function (event, ui) {
            $('#sortable').listview('refresh');
        });
    });
    var arrList = [];
    $(document).ready(function () {
        $("body").on("click", '.themvao', function () {
            var id = $(this).attr("data-id");
            var title = $(this).attr("data-title");
            var image = $(this).attr("data-image");
            var summary = $(this).attr("data-sumary");
            var cat = $(this).attr("data-catid");
            var link = $(this).attr("data-link");

            var checkExistID = arrList.findIndex(x=>x.id == id);
            if (checkExistID == -1) {
                arrList.push({ id: id, title: title, image: image, summary: summary, cat: cat, link: link });
                $('ul.to_do').append('<li class="tinlienquanli" title="Xóa" data-id="' + id + '"><a href="javascript:void(0);"><strong>' + title + '</strong></a><span class="removeSelected"><i class="fa fa-close "></i></span></li>');
            }
            //remove
            $('.removeSelected').off();
            $('.removeSelected').on('click', function () {
                var _thisLI = $(this).closest("li");
                var _thisID = $(_thisLI).attr("data-id");
                var checkExistID = arrList.findIndex(x=>x.id == _thisID);
                if (checkExistID > -1) {
                    arrList.splice(checkExistID, 1);
                }
                $(_thisLI).remove();
            });
        });
        //Mr Dòi phệt thằng này ra ngoài
        //Xử ly đống tin liên quan đã có 
        //remove
        $('.removeSelected').off();
        $('.removeSelected').on('click', function () {
            var _thisLI = $(this).closest("li");

            var _thisID = $(_thisLI).attr("data-id");
            console.log(_thisID);
            var checkExistID = arrList.findIndex(x=>x.id == _thisID);
            if (checkExistID > -1) {
                arrList.splice(checkExistID, 1);
            }
            $(_thisLI).remove();
        });

    });
    function updateFormValues() {
        $('#<%= hdf_Value.ClientID %>').val("");
        $("#sortable li").each(function (index) {
            addValue('<%= hdf_Value.ClientID %>', $(this).attr("data-id"));
});
}
function addValue(id, value) {
    if ($('#' + id).val().indexOf(value) == -1) {
        if ($('#' + id).val() == '' || $('#' + id).val() == null)
            $('#' + id).val(value);
        else
            $('#' + id).val($('#' + id).val() + ',' + value);
    }

}
</script>
        <%--== Chèn tin liên quan vào bài ==--%>
<%--    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="PageUpdateProgress">
    <ProgressTemplate>
        <div class="loading">
            <div></div>
            <div></div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
