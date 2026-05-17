<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Setting.ascx.cs" Inherits="NVCMS.Modules.School.SearchSetting" %>
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
                    Kiểu hiển thị
                </td>
                <td>
                    <div class="list-checkbox">
                        <asp:RadioButtonList RepeatColumns="2" CellPadding="4" ID="checkListNewsConfig" runat="server" CssClass="checkListNewsConfig">
                            <asp:ListItem Value="normal" Text="Tìm kiếm" />
                            <asp:ListItem Value="major" Text="Tìm theo ngành học" />
                        </asp:RadioButtonList>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    <p>
        <asp:Label ID="lbMessage" runat="server" ForeColor="Red" />
    </p>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</div>
