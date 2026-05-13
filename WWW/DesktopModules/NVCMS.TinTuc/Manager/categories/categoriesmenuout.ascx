<%@ Control Language="vb" AutoEventWireup="false" Explicit ="true" codefile="categoriesmenuout.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.categories.categoriesmenuout" %>
<DIV id="lftNav">
	<asp:DataGrid id="drgMenu" runat="server" ShowHeader="False" CellPadding="0" AutoGenerateColumns="False"
		Width="100%" GridLines="None">
		<Columns>
			<asp:TemplateColumn HeaderText="Menu">
				<ItemTemplate>
					<asp:HyperLink CssClass="new" id="hplMenu" text='<%# DataBinder.Eval(Container.DataItem,"categoryname")%>' NavigateUrl='<%# FormatURL("catid",DataBinder.Eval(Container.DataItem,"categoryid")) %>' runat="server"/>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
</DIV>
