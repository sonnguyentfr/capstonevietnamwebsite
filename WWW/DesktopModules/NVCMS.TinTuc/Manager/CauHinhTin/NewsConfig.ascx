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
                        <a class="btn btn-primary" data-toggle="modal" data-target=".bs-example-modal-lg">Chọn Tin</a>
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
        <div id="boxtinlienquan" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">
                            <span aria-hidden="true">×</span>
                        </button>
                        <h4 class="modal-title" id="myModalLabel">Chọn tin liên quan</h4>
                    </div>
                    <div class="modal-body">
                        <uc1:tinlienquan runat="server" ID="tinlienquan" />
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Đóng</button>
                    </div>

                </div>
            </div>
        </div>
        <script type="text/javascript">
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
