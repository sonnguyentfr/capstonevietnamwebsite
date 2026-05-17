<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" codefile="Settings.ascx.vb" Inherits="NVCMS.Modules.School.SettingCustomeDisplay" %>
<table id="Table1" cellspacing="1" cellpadding="1" border="0" width="100%" >
	<tr>
		<td>
			<asp:Label CssClass="SubHead" id="Label1" runat="server">Trang hiển thị</asp:Label></td>
		<td>
            <asp:TextBox ID="txtDisplayNewsPage" runat="server" Width="40px"></asp:TextBox></td>
	</tr>
    <tr>
        <td>
            <asp:Label CssClass="SubHead" ID="Label2" runat="server">Số lượng tin</asp:Label></td>
        <td>
            Dòng
            <asp:TextBox ID="txtDisplayRow" runat="server" Width="40px">1</asp:TextBox>
            Cột
            <asp:TextBox ID="txtDisplayCol" runat="server" Width="40px">1</asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="SubHead" ID="Label7" runat="server">Kiểu hiển thị</asp:Label></td>
        <td>
            <asp:DropDownList id="ddlDisplayStyle" runat="server" Width="280px">
                <asp:ListItem Value="CapV2_HomeTruongNoiBat.ascx">Trường nổi bật</asp:ListItem>
                <asp:ListItem Value="CapV2_HomeTruongDoiTac.ascx">Trường đối tác</asp:ListItem>
                
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="SubHead" ID="Label3" runat="server">Ảnh</asp:Label></td>
        <td>
            <asp:RadioButtonList ID="rbtDisplayImage" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="NoImage">Kh&#244;ng ảnh</asp:ListItem>
                <asp:ListItem Value="HaveImage">C&#243; ảnh</asp:ListItem>
            </asp:RadioButtonList></td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="SubHead" ID="Label4" runat="server">Kích thước ảnh</asp:Label></td>
        <td>
            Chiều dài<asp:TextBox ID="txtImageWidth" runat="server" Width="40px">150</asp:TextBox>Chiều
            rộng
            <asp:TextBox ID="txtImageHeight" runat="server" Width="40px"></asp:TextBox></td>
    </tr>
    <tr>
        <td>
            <asp:Label CssClass="SubHead" ID="Label6" runat="server">Chữ chạy</asp:Label></td>
        <td>
        <asp:RadioButtonList ID="rbtMarquee" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True" Value="0">Không chạy</asp:ListItem>
                <asp:ListItem Value="X">Chiều ngang</asp:ListItem>
                <asp:ListItem Value="Y">Chiều dọc</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr>
		<td>
			<asp:Label CssClass="SubHead" id="Label8" runat="server">Trong khoảng thời gian</asp:Label></td>
		<td>
            <asp:TextBox ID="txtDuration" runat="server" Width="40px">30</asp:TextBox> (ngày)
        </td>
	</tr>
</TABLE>