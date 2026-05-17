<%@ Control Language="C#" AutoEventWireup="true" CodeFile="inc_list.ascx.cs" Inherits="DesktopModules.NV_Events.Manager.template.inc_list" %>
<style type="text/css">
    #gridview table tr td {
        padding: 5px;
        border: solid 1px #e6e6e6;
    }

    #gridview table tr th {
        padding: 10px;
        border: solid 1px #e6e6e6;
        background: #2f92fe;
        font-weight: bold;
        color: white;
        text-align: center;
    }
</style>
<p class="title" style="font-weight: bold; margin-bottom: 5px;">Danh sách Template</p>
<div id="gridview">
    <asp:GridView ID="gridView" runat="server" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
        PageSize="100" EmptyDataText="Dữ liệu trống !"
        OnRowDeleting="gridView_RowDeleting" OnRowDataBound="gridView_RowDataBound">
        <HeaderStyle Height="30px"
            HorizontalAlign="Left" />
        <PagerStyle CssClass="page" />
        <RowStyle CssClass="row-item" />
        <SelectedRowStyle Font-Underline="True" />
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true">
                <ItemStyle Width="25px" />
            </asp:BoundField>
            <asp:BoundField DataField="TemplateName" HeaderText="Name" ItemStyle-Width="200px">
                <ItemStyle Width="200px"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="FilePath" HeaderText="File" />
            <asp:TemplateField HeaderText="" ItemStyle-Width="50px">
                <ItemTemplate>
                    <asp:HyperLink ID="linkEdit" Text="Sửa" runat="server" CssClass="btnEdit" />
                </ItemTemplate>

                <ItemStyle Width="50px"></ItemStyle>
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
<p>
    <asp:LinkButton ID="linkAdd" runat="server" Text="Thêm mới" OnClick="linkAdd_Click" /></p>