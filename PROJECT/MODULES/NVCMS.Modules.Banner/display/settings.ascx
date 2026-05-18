<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Settings.ascx.vb" Inherits="NVCMS.Modules.Banner.settings" %>
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
<table cellspacing="1" cellpadding="3" border="0" width="100%" class="table table-bordered">
    <tbody>
        <tr>
            <td style="width: 120px">Chọn Vị trí</td>
            <td>
                <asp:DropDownList ID="ddlvitri" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">Chọn Template</td>
            <td>
                <asp:DropDownList ID="dropTemplate" runat="server" CssClass="form-control select2"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td style="width: 120px">Hiện thị tiêu đề</td>
            <td>
                <asp:CheckBox ID="chkshowtieude" runat="server" Checked="true" />
            </td>
        </tr>
        <tr>
            <td style="width: 120px">Hiện thị Mô tả</td>
            <td>
                <asp:CheckBox ID="chkmota" runat="server" Checked="true" />
            </td>
        </tr>
    </tbody>
</table>
