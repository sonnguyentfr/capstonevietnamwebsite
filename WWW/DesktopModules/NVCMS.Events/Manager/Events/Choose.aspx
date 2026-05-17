<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Choose.aspx.vb" Inherits="DesktopModules.TinTuc.Control.Choose" %>

<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" lang="en-US">
<head runat="server">
    <title>Chọn Sự kiện hiển thị Trang chủ</title>
    <script src="/Resources/Shared/scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <link href="/Portals/_default/Skins/Admin/AdminSkin.css" rel="stylesheet" type="text/css" />
    <script src="/js/Common.js" type="text/javascript"></script>
    <script src="/js/base64.js" type="text/javascript"></script>
    <link rel="stylesheet" href="/js/jquery-ui-1.10.2.custom/development-bundle/themes/base/jquery.ui.all.css" />
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.core.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.widget.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.mouse.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.draggable.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.position.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.resizable.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.button.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="/js/general.js" type="text/javascript"></script>
    <script src="/js/jquery-ui-1.10.2.custom/development-bundle/ui/jquery.ui.datepicker.js" type="text/javascript"></script>
    <script src="/js/jquery.ui.datepicker-vi.js" type="text/javascript"></script>
    <script src="/DesktopModules/NV_Events/js/jquery.plugin.js"></script>
<script src="/DesktopModules/NV_Events/js/jquery.countdown.js"></script>
<script src="/DesktopModules/NV_Events/js/jquery.countdown-vi.js"></script>
<link rel="stylesheet" href="/DesktopModules/NV_Events/js/jquery.countdown.css">
    <style type="text/css" media="screen">
        .square-thumb {
            width: 60px;
            height: 60px;
        }
    </style>
    <style type="text/css">
        .list-lq {
            padding: 10px 0px 5px 20px;
        }

        ul {
            list-style: none;
        }

        .list-lq ul li {
            line-height: 20px;
            background: url('/img/arrow-tlq.png') no-repeat 0 8px;
            padding-left: 15px;
        }

            .list-lq ul li a:hover {
                color: #f59206;
            }
    </style>
    <script language="javascript" type="text/javascript">
        function CloseMySelf(sender) {
            try {
                //window.opener.HandlePopupResult(sender.getAttribute("result"));
            }
            catch (err) { }
            window.close();
            return false;
        }

        function setVal(id, title) {
            //addValue(id + "|" + title);
            $('.list-lq ul').append('<li><a href="#">' + Base64.decode(title) + '</a></li>');
            window.opener.HandlePopupResult(id + "|" + title);
        }

        function removeVal(sender) {
            alert($(sender).parent().html());
        }
        function addValue(value) {
            if ($('#divResult').attr("result").indexOf(value) == -1) {
                if ($('#divResult').attr("result") == '' || $('#divResult').attr("result") == null)
                    $('#divResult').attr("result", value);
                else
                    $('#divResult').attr("result", $('#divResult').attr("result") + ';' + value);
            }

        }
        function removeValue(value) {
            var arrTemp = $('#divResult').attr("result");
            var sResult = "";
            var arr = new Array();
            arr = arrTemp.split(";");
            for (var i = 0; i < arr.length; i++) {
                if (arr[i] && arr[i] != value) {
                    sResult += ";" + arr[i];
                }
            }
            $('#divResult').attr("result", sResult.substring(1, sResult.length));

        }
    </script>
</head>
<body>
    <form id="formChoose" runat="server">
        <div id="content" style="padding: 5px;">
            <div class="nav-main">
                <table width="100%" class="nav-top nav-noborder">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" CssClass="SubHead" runat="server">Tiêu đề</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTitle" runat="server" Width="100%"></asp:TextBox>
                        </td>
                        <td align="center" width="100px">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="lbtFind" runat="server" Font-Bold="true" CssClass="toolbar_btn"
                                            ImageUrl="/images/icons/magnifier32.png" ToolTip="Tìm kiếm"></asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="120px">
                            <asp:Label ID="Label10" CssClass="SubHead" runat="server">Chuyên mục</asp:Label>
                        </td>
                        <td>
                            <table style="line-height: 25px;" width="90%">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlCategory" runat="server" Width="250px" AutoPostBack="true"></asp:DropDownList>
                                    </td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" CssClass="SubHead" runat="server">Ngày đăng</asp:Label>
                        </td>
                        <td>
                            <table style="line-height: 25px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" runat="server">Từ ngày</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtStartdate" runat="server" Width="96px" CssClass="date"></asp:TextBox>
                                    </td>
                                    <td style="width: 20px;"></td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server">Đến ngày</asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndDate" runat="server" Width="96px" CssClass="date"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <table style="background-color: #d2d2d2; height: 20px; line-height: 20px;" cellpadding="2"
                            cellspacing="2" width="100%">
                            <tr>
                                <td>Tổng số có:
                                <asp:Label ID="lbTotalNewsCount" runat="server" ForeColor="Maroon" Font-Bold="true"
                                    Text="00"></asp:Label>
                                    tin bài
                                </td>
                                <td align="right">Hiển thị
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True">
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>100</asp:ListItem>
                                </asp:DropDownList>
                                    tin / trang
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="list-lq">
                            <ul></ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <a id="divResult" href="#" result="" onclick="return CloseMySelf(this);" class="StandardButton">
                                        <img src="/images/icons/arrow_redo.png" alt="Huỷ xuất bản" />
                                        Đóng cửa sổ</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:DataGrid ID="drgDataViewer" DataKeyField="id" Width="100%" runat="server"
                AutoGenerateColumns="False" CssClass="table-bordered">
                <ItemStyle CssClass="TRgrid"></ItemStyle>
                <HeaderStyle Font-Bold="true" ForeColor="White" BackColor="#006699"></HeaderStyle>
                <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
                <Columns>
                    <asp:TemplateColumn Visible="False">
                        <HeaderTemplate>
                            <asp:CheckBox ID="chkItemsTop" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkItems" runat="server" />
                        </ItemTemplate>
                        <ItemStyle Width="15" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="P/B">
                        <ItemTemplate>
                            <a title='<%# DataBinder.Eval(Container.DataItem, "Title") %>'>
                                <asp:Image Width="60" ImageUrl='<%# Ultis.FormatThumbImage(CStr(Eval("Avatar")), 60, 0, "", "", "", "")%>'
                                    AlternateText="" ID="imgNews" runat="server" />
                            </a>
                        </ItemTemplate>
                        <ItemStyle Width="60" />
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Tin bài">
                        <ItemTemplate>
                            <a href="javascript:setVal(<%# Eval("id") %>,Base64.encode('<%# HtmlUtils.StripPunctuation(CStr(Eval("Title")), True).Replace("'", "")%>'))">
                                <%# Highlight(CStr(Eval("Title")), "<span class='highlight'>", "</span>")%>
                            </a>

                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Tình trạng">
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# CoutDowntime(CInt(DataBinder.Eval(Container.DataItem, "id")))%>'></asp:Label>
                            <div id="defaultCountdown<%# DataBinder.Eval(Container.DataItem, "id")%>"></div>
                        </ItemTemplate>
                        <ItemStyle Width="300" />
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="Chuyên mục">
                        <ItemTemplate>
                            <%# NVVideosGet.GetCatName(CInt(Eval("CatId")))%>
                        </ItemTemplate>
                        <ItemStyle Width="120px" />
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
            <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="Postback" pagelinksperpage="20" />
        </div>
    </form>
</body>
<script language="javascript" type="text/javascript">
    $("tr.TRgrid").hover(function () {
        $(this).addClass("TRgrid-Hover");
    }, function () {
        $(this).removeClass("TRgrid-Hover");
    });
    $(function () {
        $(".date").datepicker({
            dateFormat: 'dd/mm/yy',
            showOn: 'button',
            buttonImage: "/Portals/_default/Skins/Admin/Images/83.png",
            buttonText: "Nhập ngày tháng",
            buttonImageOnly: true,
            changeYear: true,
            numberOfMonths: 3,
            showWeek: true,
            firstDay: 1
        });
    });
</script>
</html>
