<%@ Control Language="VB" AutoEventWireup="false" CodeFile="tinlienquan.ascx.vb" Inherits="NVCMS.Modules.TinTuc.newsedittinleinquan" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<ul class="to_do" style="display:none;">
</ul>
<asp:UpdatePanel ID="tinleinquan" runat="server">
    <ContentTemplate>
        <div class="col-md-12 ">
            <div class="col-md-4">
                <div class="form-group">
                    <label>Tiêu đề</label>
                    <input class="form-control" id="txtTitle" runat="server" style="width: 100%;" />
                </div>
            </div>
            <!-- /.col -->
            <div class="col-md-2 col-sm-12">
                <div class="form-group">
                    <label>Chuyên mục</label>
                    <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="form-control select2"></asp:DropDownList>
                </div>
                <!-- /.form-group -->
                <!-- /.form-group -->
            </div>
            <div class="col-md-2 col-sm-12">
                <div class="form-group">
                    <label>Tác giả</label>
                    <asp:DropDownList ID="ddlUserPost" runat="server" AutoPostBack="true" CssClass="form-control select2"></asp:DropDownList>
                </div>
                <!-- /.form-group -->
                <!-- /.form-group -->
            </div>
            <div class="col-md-3 col-sm-12">
                <div class="form-group">
                    <div class="col-md-6 pdf0">
                        <label>Từ ngày </label>
                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input type="text" id="txtStartdate" runat="server" class="form-control pull-right datepicker">
                        </div>
                    </div>
                    <div class="col-md-5">
                        <label>Đến ngày</label>
                        <div class="input-group date">
                            <div class="input-group-addon">
                                <i class="fa fa-calendar"></i>
                            </div>
                            <input type="text" id="txtEndDate" runat="server" class="form-control pull-right datepicker">
                        </div>
                    </div>
                </div>
                <!-- /.form-group -->
                <!-- /.form-group -->
            </div>
            <div class="col-md-1">
                <label>&nbsp;</label>
                <asp:ImageButton ID="lbtFind" runat="server" Font-Bold="true" CssClass="toolbar_btn" ImageUrl="/images/icons/magnifier32.png" ToolTip="Tìm kiếm"></asp:ImageButton>
            </div>
        </div>
        <!-- /.col -->
        <!-- /.row -->
        <div class="ln_solid"></div>
        <div class="col-md-12">
            <table id="datatable-checkboxtinlienquan" class="table table-striped table-bordered bulk_action">
                <thead>
                    <tr>
                        <th style="width: 80px; text-align: center;">Ảnh</th>
                        <th>Tiêu đề</th>
                        <th>Danh mục</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rpttinlienquan" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 60px; text-align: center;">
                                    <asp:Image Width="60" ImageUrl='<%# Ultis.FormatThumbImage(CStr(Eval("imagepath")), 60, 60, "", "", "", "")%>'
                                        AlternateText="" ID="imgNews" runat="server" /></td>
                                <td>
                                    <a data-id="<%# Eval("newid") %>" data-title="<%# HtmlUtils.StripPunctuation(CStr(Eval("Title")), True).Replace("'", "")%>"
                                        data-image="<%# Eval("ImagePath") %>" data-sumary="<%# Eval("Summary") %>" data-catid="<%# Eval("CategoryId") %>"
                                        data-link="<%#Ultis.FormatLink(CInt(BL.GetMappingTabIDByCategoryID(CInt(Eval("CategoryId")))), CInt(Eval("NewId")), CStr(Eval("Title"))) %>" class="themvao">
                                        <h5><strong><%# Eval("Title")%></strong></h5>
                                    </a>
                                    <font style="color: Maroon;">Tác giả:</font>
                                    <asp:Label ID="lblCreatedInfo" ForeColor="Maroon"
                                        runat="server" Text='<%# BL.GetCreatedInfo(0, CInt(Eval("UserId")), CDate(Eval("Createdate")))%>'></asp:Label>
                                    | <font style="color: Maroon;">Duyệt xuất bản:</font>
                                    <asp:Label ID="lblPublishInfo"
                                        ForeColor="Maroon" runat="server" Text='<%# BL.GetPublishedInfo(0, CInt(Eval("PublishedUser")), CDate(Eval("PublishedDate")))%>'></asp:Label>
                                </td>
                                <td><%#Eval("CategoryName") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <div class="col-md-12"><a class="btn btn-danger" href="#!" onclick="tinlienquanxemtiep()"><span class="load-more">Xem thêm</span></a></div>
        <div id="tinlienquanloadimage" style="display: none;" class="text-center mr-10">
            <img src="/static/nvcms/img/load.gif" height="80px" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlCategory" />
        <asp:AsyncPostBackTrigger ControlID="ddlUserPost" />
    </Triggers>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div class="loading">
            <div></div>
            <div></div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<script>

    var txtTitle = document.getElementById('<%=txtTitle.ClientID%>').value;
    var danhmuc = document.getElementById('<%=ddlCategory.ClientID%>').value;
    var uid = document.getElementById('<%=ddlUserPost.ClientID%>').value;
    var from = document.getElementById('<%=txtStartdate.ClientID%>').value;
    var to = document.getElementById('<%=txtEndDate.ClientID%>').value;
    var tinlienquanclick = 1;
    var tinlienquantotalpage = 10;
    function tinlienquanxemtiep() {
        if (tinlienquanclick == 0 || tinlienquanclick == tinlienquantotalpage) {
            return false;
        }
        else {
            tinlienquanclick += 1;
            tinlienquanloaddata();
        }
    }
    function tinlienquanloaddata() {
        $('div#tinlienquanloadimage').show();
        $.ajax({
            url: "/DesktopModules/NVCMS.TinTuc/Manager/control/tinlienquanLoadMore.aspx?key=" + txtTitle + "&catid=" + danhmuc + "&uid=" + uid + "&from=" + from + "&to=" + to + "&trang=" + tinlienquanclick,
            success: function (data) {
                $('div#tinlienquanloadimage').hide();
                $('#datatable-checkboxtinlienquan tbody').append(data);
            }
        });
    }



</script>

