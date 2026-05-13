<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Setting.ascx.cs" Inherits="DesktopModules.TinTuc.View.Setting" %>
<style type="text/css">
    .setting_news {
        padding: 10px;
    }

        .setting_news table tr td {
            padding: 3px 0px;
        }

        .setting_news .list-radio label, .setting_news .list-checkbox label {
            padding-right: 10px;
        }
</style>
<div class="setting_news">
<%--    <asp:UpdatePanel runat="server">
        <ContentTemplate>--%>
    <table cellspacing="1" cellpadding="3" border="0" width="100%" class="table table-bordered">
        <tbody>
            <tr>
                <td style="width: 120px">Kiểu lấy tin</td>
                <td>
                    <div class="list-radio">
                        <asp:RadioButton Checked="true" AutoPostBack="true" GroupName="GetType" ID="rdGetType_Config" runat="server" Text="Tin cấu hình" OnCheckedChanged="rdGetType_CheckedChanged" />
                        <asp:RadioButton AutoPostBack="true" GroupName="GetType" ID="rdGetType_Cate" runat="server" Text="Tin chuyên mục" OnCheckedChanged="rdGetType_CheckedChanged" />
                    </div>
                </td>
            </tr>
            <tr id="tr_GetType_Config" runat="server" visible="true">
                <td>Tin cấu hình
                </td>
                <td>
                    <div class="list-checkbox">
                        <asp:RadioButtonList RepeatColumns="4" CellPadding="4" ID="checkListNewsConfig" runat="server">
                            <asp:ListItem Value="Slider" Text="Slider" />
                            <asp:ListItem Value="TinNong" Text="Tin Nóng" />
                            <asp:ListItem Value="XuHuongDoc" Text="Xu hướng đọc" />
                            <asp:ListItem Value="TinMoiNhat" Text="Tin Mới" />
                            <asp:ListItem Value="TinDocNhieu" Text="Đọc nhiều" />
                            <asp:ListItem Value="TinAnh" Text="Tin Ảnh" />
                            <asp:ListItem Value="TinVideo" Text="Tin Video" />
                            <asp:ListItem Value="Tin24h" Text="Tin trong ngày" />
                        </asp:RadioButtonList>
                    </div>
                </td>
            </tr>
            <tr id="tr_GetType_Category" runat="server" visible="false">
                <td style="width: 160px">Danh mục tin</td>
                <td>
                    <asp:DropDownList ID="dropCate" runat="server" Width="500px" />
                </td>
            </tr>
            <tr>
                <td>SL tin lấy</td>
                <td>
                    <div class="news-count">
                        <label>Tin TOP</label>
                        <asp:TextBox ID="txtNewsTop" runat="server" Width="50" />
                        <span class="space">&nbsp;</span>
                        <label>Tin mở rộng</label>
                        <asp:TextBox ID="txtNewsMore" runat="server" Width="50" />
                        <p><i style="font-size: 11px">(Nếu chỉ lấy 1 kiểu tin đặt 'Tin mở rộng = 0')</i></p>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Template</td>
                <td>
                    <asp:DropDownList ID="dropTemplate" runat="server" Width="500px" /></td>
            </tr>
            <tr>
                <td>Kích thước ảnh</td>
                <td>
                    <p>
                        <label>Tin TOP</label>
                        <asp:TextBox ID="txtTopWidth" runat="server" Width="60" />
                        <span>x</span>
                        <asp:TextBox ID="txtTopHeight" runat="server" Width="60" />
                        <span style="padding: 0px 10px"></span>
                        <label>Tin mở rộng</label>
                        <asp:TextBox ID="txtMoreWidth" runat="server" Width="60" />
                        <span>x</span>
                        <asp:TextBox ID="txtMoreHeight" runat="server" Width="60" />
                    </p>
                </td>
            </tr>
            <tr>
                <td>Giới hạn tiêu đề</td>
                <td>
                    <asp:TextBox ID="txtSizeTitle" runat="server" Width="60" /></td>
            </tr>
            <tr>
                <td>Giới hạn số từ mô tả</td>
                <td>
                    <asp:TextBox ID="txtSizeDes" runat="server" Width="60" /></td>
            </tr>
        </tbody>
    </table>
    <p>
        <asp:Label ID="lbMessage" runat="server" ForeColor="Red" />
    </p>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</div>
