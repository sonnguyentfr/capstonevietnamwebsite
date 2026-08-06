<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Settings.ascx.vb" Inherits="NVCMS.Modules.LadingPage.SettingCustomeDisplay" %>
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

    .table tr td {
        border: solid 1px #ebebeb;
        font-size: 12px;
        padding: 10px !important;
    }

        .table tr td input, .table tr td select {
            padding: 5px;
            border: solid 1px #ebebeb;
        }
</style>
<div class="setting_news">
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table table-bordered">
        <tbody>
            <tr>
                <td>Chọn Trang Hiện thị</td>
                <td>
                    <asp:DropDownList ID="ddlTrangLadingPage" runat="server" Width="400"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Hiện tiêu đề</td>
                <td>
                    <asp:CheckBox ID="chkHienTieude" runat="server" />Hiện tiêu đề vào nội dung</td>
            </tr>
            <tr>
                <td>Hiện danh sách trang con</td>
                <td>
                    <asp:CheckBox ID="chkHienDanhsachSub" runat="server" />Lấy danh sách trang con</td>
            </tr>
            <tr>
                <td>Chọn template Index</td>
                <td>
                    <asp:DropDownList ID="dropTemplate" runat="server" CssClass="form-control select2"></asp:DropDownList></td>
            </tr>
            <tr>
                <td>Chọn template Detail</td>
                <td>
                    <asp:DropDownList ID="dropTemplateDetail" runat="server" CssClass="form-control select2"></asp:DropDownList></td>
            </tr>
        </tbody>
    </table>
</div>
