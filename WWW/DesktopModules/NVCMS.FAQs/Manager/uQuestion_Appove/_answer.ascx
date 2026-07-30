<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_answer.ascx.vb" Inherits="BUH.Modules.FAQs.inc_edit" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>





<style type="text/css">
    .upnewstd1 {
        width: 100px;
        font-family: Arial;
        color: Black;
        text-align: right;
        height: 30px;
        font-weight: normal;
        font-size: 12px;
        border-bottom: dotted 1px #dddddd;
        padding-right: 10px;
    }

    .upnewstd2 {
        border-bottom: dotted 1px #dddddd;
        padding: 5px;
    }
</style>
<div class="pustyle">
    <div class="toolbar-placeholder">
        <div class="toolbarBox toolbarHead">
            <ul class="cc_button">
                <li style="padding-left: 20px;">
                    <asp:LinkButton ID="lbDelete" OnClientClick="javascript: return confirm('Bạn có muốn xoá không?');" runat="server" Font-Bold="True" CssClass="StandardButton">
                        <img src="/images/icons/delete.png" alt="Xoá" /> Xoá
                    </asp:LinkButton>
                </li>
                <li style="padding-left: 20px;">
                    <asp:LinkButton ID="lbtCancel" runat="server" ValidationGroup="VBuzzValidation22" Font-Bold="True" CssClass="StandardButton">
                        <img src="/images/icons/arrow_rotate_clockwise.png" alt="Quay lại" /> Thoát
                    </asp:LinkButton>
                </li>
            </ul>
            <div class="clear"></div>
        </div>
    </div>
</div>
<table id="table1" cellpadding="0" cellspacing="0" width="90%" align="center" style="padding: 20px; background: #f3f3f3; border: solid 1px #dddddd;">
    <tr>
        <td colspan="4">
            <asp:Label ID="lbResult" runat="server" CssClass="NormalRed"></asp:Label></td>
    </tr>
    <tr>
        <td class="upnewstd1">Họ và tên: </td>
        <td class="upnewstd2">
            <asp:Label ID="lblHovaTen" runat="server" Font-Bold="true"></asp:Label>
        </td>
        <td class="upnewstd1">Email: </td>
        <td class="upnewstd2">
            <asp:Label ID="lblEmail" Font-Bold="true" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="upnewstd1">Địa chỉ: </td>
        <td class="upnewstd2">
            <asp:Label ID="lblAddress" Font-Bold="true" runat="server"></asp:Label>
        </td>
        <td class="upnewstd1">Số điện thoại: </td>
        <td class="upnewstd2">
            <asp:Label ID="lblMobile" Font-Bold="true" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="upnewstd1">Tiêu đề: </td>
        <td class="upnewstd2" colspan="3">
            <asp:Label ID="lblTite" Font-Bold="true" ForeColor="Red" Font-Size="15px" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="upnewstd1">Câu hỏi: </td>
        <td class="upnewstd2" colspan="3">
            <asp:Label ID="lblQuestion" Font-Bold="true" ForeColor="Red" Font-Size="15px" runat="server"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="upnewstd1">Tên hiện thị trả lời</td>
        <td class="upnewstd2" colspan="3">
            <asp:Label ID="txtuAnswer" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px"></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="upnewstd1">Nội dung trả lời</td>
        <td class="upnewstd2" colspan="3">
            <asp:Label ID="txtTraloi" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="15px"></asp:Label>
        </td>
    </tr>
</table>

