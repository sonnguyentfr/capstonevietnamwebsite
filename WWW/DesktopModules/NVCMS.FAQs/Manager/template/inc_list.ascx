<%@ Control Language="C#" AutoEventWireup="true" CodeFile="inc_list.ascx.cs" Inherits="DesktopModules.NV_Events.Manager.template.inc_list" %>
<p class="title" style="font-weight: bold; margin-bottom: 5px;">Danh sách Template</p>
<div id="gridview">
    <asp:GridView ID="gridView" runat="server" Width="100%" AutoGenerateColumns="False"
        CellPadding="5" EnableModelValidation="True" GridLines="None" AllowPaging="True"
        BackColor="White" BorderColor="White" BorderStyle="Ridge" 
        BorderWidth="2px" CellSpacing="1"
        PageSize="20" EmptyDataText="Dữ liệu trống !" 
        onrowdeleting="gridView_RowDeleting" OnRowDataBound="gridView_RowDataBound">        
        <EditRowStyle BackColor="#333399" />
        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
        <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="#E7E7FF" Height="30px"
        HorizontalAlign="Left" />
        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" CssClass="page" />
        <RowStyle BackColor="#DEDFDE" ForeColor="Black" CssClass="row-item" />
        <SelectedRowStyle BackColor="#990000" Font-Bold="True" ForeColor="White" Font-Underline="True" />
        <Columns>
        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true">
            <ItemStyle Width="25px" />
        </asp:BoundField>
        <asp:BoundField DataField="TemplateName" HeaderText="Name" ItemStyle-Width="200px">
        </asp:BoundField>
        <asp:BoundField DataField="FilePath" HeaderText="File" />     
        <asp:TemplateField HeaderText="" ItemStyle-Width="50px">
            <ItemTemplate>
                <asp:HyperLink ID="linkEdit" Text="Sửa" runat="server" CssClass="btnEdit" />
            </ItemTemplate>
        </asp:TemplateField>             
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
            <asp:LinkButton ID="linkDelete" runat="server" CausesValidation="false" Text="Xóa" CssClass="btnDelete"
                CommandName="Delete" OnClientClick="return confirm('Bạn có chắc chắn muốn xóa ?')">
            </asp:LinkButton>
            </ItemTemplate>
            <ItemStyle Width="50px" />
        </asp:TemplateField>
        </Columns>
    </asp:GridView>    
    </div>
    <!-- End #gridView -->
<p><asp:LinkButton ID="linkAdd" runat="server" Text="Thêm mới" OnClick="linkAdd_Click" /></p>
