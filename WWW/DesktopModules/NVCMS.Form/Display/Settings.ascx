<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Settings.ascx.vb" Inherits="NVCMS.Modules.Form.SettingCustomeDisplay" %>
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
                <td>Gửi mail đến quản trị
                </td>
                <td>
                    <div class="list-radio">
                        <asp:RadioButton Checked="true" AutoPostBack="true" GroupName="GetType" ID="rd_KhongGui" runat="server" Text="Không" OnCheckedChanged="rdGetType_CheckedChanged" />
                        <asp:RadioButton AutoPostBack="true" GroupName="GetType" ID="rd_Gui" runat="server" Text="Có" OnCheckedChanged="rdGetType_CheckedChanged" />
                    </div>
                </td>
            </tr>
            <tr id="tr_nhanmail" runat="server" visible="true">
                <td>NHẬN Email: Danh sách mail<br />
                    <i>Các địa chỉ email cách nhau dấu , (phẩy): mai@nvportal.net,mail2@nvportal.net</i>
                </td>
                <td>
                    <asp:TextBox ID="txtemailnhan" runat="server" TextMode="MultiLine" Width="100%" Height="40px"  /></td>
            </tr>
            <tr id="tr_nhanmail2" runat="server" visible="true">
                <td>NHẬN EMAIL: Tiêu đề</td>
                <td>
                    <asp:TextBox ID="txttieudemail" Text="0" runat="server" /></td>
            </tr>
            <tr>
                <td>Kiểu hiện thị</td>
                <td>
                    <asp:DropDownList ID="ddlDisplayStyle" runat="server" Width="400">
                        <asp:ListItem Value="0">Chọn kiểu hiện thị</asp:ListItem>
                        <asp:ListItem Value="Capstone.ascx">Capstone</asp:ListItem>
                        <asp:ListItem Value="defaultEN.ascx">Mạc đinh EN</asp:ListItem>
                        
                    </asp:DropDownList></td>
            </tr>
        </tbody>
    </table>
</div>
