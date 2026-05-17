<%@ Control Language="VB" AutoEventWireup="false" CodeFile="EventsConfig.ascx.vb" Inherits="DesktopModules.TinTuc.Configurations.NewsConfig" %>
<script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
<style>
  #sortable { list-style-type: none; margin: 0; padding-top: 10px; width: 60%; }
  #sortable li { margin: 0 3px 3px 3px; padding: 0.4em; padding-left: 1.5em; height: 18px; cursor: move;color: #CB2027;}
  #sortable li span { position: absolute; margin-left: -1.3em;}
</style>

<div class="pustyle">
<div class="toolbar-placeholder">
        <div class="toolbarBox toolbarHead">
            <ul class="cc_button">
                <li><asp:linkbutton id="lbtUpdate" runat="server" Font-Bold="True" CssClass="toolbar_btn" OnClientClick="updateFormValues(); return true;"><img src="/images/icons/script_save.png" alt="Thực hiện"/> Cập nhật</asp:linkbutton></li>
                <li><asp:linkbutton id="lbtCancelTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/icons/arrow_rotate_clockwise.png" alt="Thoát" /> Thoát</asp:linkbutton></li>
            </ul>
            <div class="clear"></div>
        </div>
    </div>
</div>

<a id="page-popup" onclick="popupwindow(900,600); return false;" style="text-decoration: underline; cursor: pointer; color: #CB2027;"><img src="/images/paperclip.png"/>Chọn tin:</a>
<br/>
<ul id="sortable">
    <asp:Repeater runat="server" ID="rptSettings">
        <ItemTemplate>
            <li class="ui-state-default" rel="<%# Eval("id")%>">
                <span class="ui-icon ui-icon-arrowthick-2-n-s"></span><%# Eval("Title")%>
                <a class="delRelated" onclick="javascript:delNews(this,<%# Eval("id")%>);" title="Loại bỏ sự kiện này?" style="cursor:pointer;"></a>
            </li>
        </ItemTemplate>
    </asp:Repeater>
</ul>
<asp:HiddenField ID="hdf_Value" runat="server"/>
<script type="text/javascript">
    $(function () {
        $("#sortable").sortable();
        $("#sortable").disableSelection();
    });
    function delNews(sender, id) {
        if (confirm("Bạn có chắc chắn muốn xóa?") == true) {
            //Remove
            $(sender).parent().remove();

            return false;
        }
    }
    function addValue(id, value) {
        if ($('#' + id).val().indexOf(value) == -1) {
            if ($('#' + id).val() == '' || $('#' + id).val() == null)
                $('#' + id).val(value);
            else
                $('#' + id).val($('#' + id).val() + ',' + value);
        }

    }
    //Update all included media
    function updateFormValues() {
        $('#<%= hdf_Value.ClientID %>').val("");
        $("#sortable li").each(function (index) {
            addValue('<%= hdf_Value.ClientID %>', $(this).attr("rel"));
        });
    }
</script>

<script type="text/javascript">
    function popupwindow(w, h) {
        var left = (screen.width / 2) - (w / 2);
        var top = (screen.height / 2) - (h / 2);
        return window.open("/DesktopModules/NV_Events/Manager/Events/Choose.aspx", "Cấu hình tin bài", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=20, left=' + left);
    }
    function HandlePopupResult(result) {
        var arr = new Array();
        arr = result.split(";");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] != null && arr[i] != '') {
                var sid = arr[i].split("|")[0];
                var stitle = Base64.decode(arr[i].split("|")[1]);

                $('#sortable').append('<li class="ui-state-default" rel=' + sid + '><span class="ui-icon ui-icon-arrowthick-2-n-s"></span>' + stitle + ' <a class="delRelated" onclick="javascript:delNews(this,' + sid + ');" title="Loại bỏ tin này?" style="cursor:pointer;"></a></li>');
            }
        }
    }
</script>