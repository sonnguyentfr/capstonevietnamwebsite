<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Setting.ascx.cs" Inherits="NVCMS.Modules.School.Setting" %>
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
        .checkListNewsConfig tr td { border:0px; border-right:1px dashed #ccc; padding-left:10px !important;}
</style>
<div class="setting_news">

    <table cellspacing="1" cellpadding="3" border="0" width="100%" class="table table-bordered">
        <tbody>
            <tr>
                <td width="200px">
                    Nội dung hiển thị
                </td>
                <td>
                    <div class="list-checkbox">
                        <asp:RadioButtonList RepeatColumns="2" CellPadding="4" ID="checkListNewsConfig" runat="server" CssClass="checkListNewsConfig">
                            <asp:ListItem Value="TruongDoiTac" Text="Trường đối tác" />
                            <asp:ListItem Value="TruongNoBat" Text="Trường nổi bật" />
                        </asp:RadioButtonList>
                    </div>
                </td>
            </tr>
            <tr>
                <td>Số bản ghi</td>
                <td>
                    <div class="news-count">
                        <label>TOP</label>
                        <asp:TextBox ID="txtNewsTop" runat="server" Width="50" Text="0" />
                        <span class="space">&nbsp;</span>
                        <label>More</label>
                        <asp:TextBox ID="txtNewsMore" runat="server" Width="50" Text="0" />
                        <p><i style="font-size: 11px">(Nếu chỉ lấy 1 kiểu tin đặt 'More = 0')</i></p>
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
                        <asp:TextBox ID="txtTopWidth" runat="server" Width="60" Text="0" />
                        <span>x</span>
                        <asp:TextBox ID="txtTopHeight" runat="server" Width="60" Text="0" />
                        <span style="padding: 0px 10px"></span>
                        <label>Tin mở rộng</label>
                        <asp:TextBox ID="txtMoreWidth" runat="server" Width="60" Text="0" />
                        <span>x</span>
                        <asp:TextBox ID="txtMoreHeight" runat="server" Width="60" Text="0" />
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
