<%@ Control Language="vb" AutoEventWireup="false" CodeFile="inc_process.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.newsapprove.Approve_inc_process" %>

<div class="nav-main">
<table class="nav-top nav-noborder" width="100%">
    <tr>
        <td>
            Quá trình xử lý tin bài: <asp:Label ID="lbNews" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label><br />
            Tác giả: <asp:Label ID="lbUserCreated" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00" Font-Italic="true"></asp:Label>
        </td>
    </tr>
</table>
</div>
<br />
<asp:datagrid id="drgDataViewer" DataKeyField ="ID" Width="100%" runat="server" AllowPaging="false" 
    AutoGenerateColumns="False" CssClass="table-bordered">
	<ItemStyle CssClass="TRgrid"></ItemStyle>
	<HeaderStyle Font-Bold="true" ForeColor="White" BackColor="#006699"></HeaderStyle>
	<FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
	<Columns>
        <asp:TemplateColumn HeaderText="Thời gian">
            <ItemTemplate>
                <asp:Label ID="Label6" runat="server" Text='<%# BL.FormatDate(Eval("CreateDate")) %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle Width="100" />
        </asp:TemplateColumn>
		<asp:TemplateColumn HeaderText="Thông tin xử lý">
            <ItemTemplate>
                <asp:Label ID="Label7" runat="server" ForeColor="Maroon" Text='<%# GetUserName(DataBinder.Eval(Container.DataItem,"ByUser")) %>'></asp:Label>: 
                <asp:Label ID="Label6" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"ProcessName") %>'></asp:Label> <br />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Bút phê" ItemStyle-Width="300px">
            <ItemTemplate>
                <asp:Label ID="lblButPhe" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Comment") %>'></asp:Label> <br />
            </ItemTemplate>
        </asp:TemplateColumn>
        <asp:TemplateColumn HeaderText="Phiên bản" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60">
			<ItemTemplate>
				<asp:HyperLink id="Hyperlink2" ToolTip="Xem phiên bản" Target="_blank" Visible='<%# FormatVisible(DataBinder.Eval(Container.DataItem,"VersionId")) %>' NavigateUrl='<%# navigateurl() & "?view=version&ItemID=" & DataBinder.Eval(Container.DataItem,"VersionId") %>' runat="server" >
					<asp:Image id="ViewVersion" ImageUrl="~/images/icons/tag_blue_edit.png" AlternateText="Xem phiên bản" Runat="server" />
				</asp:HyperLink>
			</ItemTemplate>
			<ItemStyle Width="30" />
		</asp:TemplateColumn>
	</Columns>
</asp:datagrid>
<br />
<table cellpadding="2" cellspacing="2" width="100%">
    <tr>
        <td align="left" style="line-height:20px;">
            <asp:LinkButton ID="lbtCancel" CssClass="StandardButton" ToolTip="Hủy bỏ, về lại trang trước" runat="server"><img src="/images/folderup.gif" border="0" /> Quay lại</asp:LinkButton>
        </td>
    </tr>
</table>