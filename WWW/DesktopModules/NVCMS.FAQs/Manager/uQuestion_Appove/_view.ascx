<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_view.ascx.vb" Inherits="BUH.Modules.FAQs.inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<script type="text/javascript" language="JavaScript" src="/js/NVCorp.js"></script>
<table cellpadding="0" cellspacing="0" width="100%" align="center" style="border: solid 1px #eee; padding: 10px; font-size: 14px;">
    <tr>
        <td style="width: 90px;">Tìm câu hỏi</td>
        <td>
            <asp:TextBox ID="txtTitle" Width="97%" BorderWidth="1px" Font-Size="14px" runat="server"></asp:TextBox>
        </td>
        <td style="width: 280px;">Trạng thái:
            <asp:DropDownList ID="ddlStatus" Width="200px" runat="server" Enabled="false">
                <asp:ListItem Value="3">Đã xuất bản</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td style="width: 60px;">
            <asp:ImageButton ID="lbtFind" runat="server" Font-Bold="true" CssClass="toolbar_btn" ImageUrl="/images/icons/magnifier32.png" ToolTip="Tìm kiếm"></asp:ImageButton>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="border-top: solid 1px #ccc; padding-top: 10px;">Tổng số có :<asp:Label ID="lbTotalNewsFind" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
            câu hỏi được xuất bản
        </td>
    </tr>
</table>
<table cellpadding="0" cellspacing="0" width="100%" align="center" style="border: solid 1px #eee; border-top: 0px; padding: 10px;">
    <tr>
        <td>
            Chọn
            <asp:CheckBox ID="CheckBox1" runat="server" />
            để 
            <asp:LinkButton ID="lbtDelete" runat="server" font-Bold="True" CssClass="StandardButton"><img src="/images/icons/delete.png" alt="Xoá" />Xóa</asp:LinkButton>
            Hoặc
            <asp:LinkButton ID="lbtHuyXB" runat="server" font-Bold="True" CssClass="StandardButton"><img src="/images/icons/arrow_redo.png" alt="Hủy Xuất Bản" />Hủy Xuất Bản</asp:LinkButton>
        </td>
    </tr>
</table>
<asp:UpdatePanel ID="udpContent" runat="server">
    <ContentTemplate>
        <table cellpadding="0" cellspacing="0" width="100%" align="center">
            <tr>
                <td>
                    <asp:DataGrid ID="drgDataViewer" DataKeyField="id" runat="server" Width="100%"
                        AutoGenerateColumns="False" CssClass="table-bordered">
                        <HeaderStyle Font-Bold="true" ForeColor="White" BackColor="#006699"></HeaderStyle>
                        <ItemStyle CssClass="TRgrid"></ItemStyle>
                        <Columns>
                            <asp:BoundColumn DataField="id" HeaderText="ID" ItemStyle-Width="15px"></asp:BoundColumn>
                            <asp:TemplateColumn>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkItemsTop" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkItems" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="15px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Thông tin">
                                <ItemTemplate>
                                    Họ và Tên: <b><%# Eval("UserName")%></b><br />
                                    Email: <b><%# Eval("Email")%></b>
                                </ItemTemplate>
                                <ItemStyle Width="220px" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Câu hỏi">
                                <ItemTemplate>
                                    <p style="color: #d43604; font-size: 14px; font-weight: bold;"><%# Highlight(Eval("Question"), "<span class='highlight'>", "</span>")%></p>
                                    <hr />
                                    
                                    <p><%#Server.HtmlDecode(Eval("Traloi"))%></p>
                                    <p>
                                        Tên hiện thị: <b><%#Eval("UAnswer")%></b>&nbsp;&nbsp;|&nbsp;&nbsp;
                                        Ngày: <b><%# BL.FormatDate(Eval("PublichDate"))%></b>&nbsp;&nbsp;|&nbsp;&nbsp;
                                        <asp:HyperLink ID="cmdEdit" ImageUrl="~/images/register.gif" NavigateUrl='<%# NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id")%>'
                                        runat="server" />
                                    </p>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="Xem">
                                <ItemTemplate>
                                    <asp:HyperLink ID="cmdEdit" ImageUrl="~/images/register.gif" NavigateUrl='<%# NavigateURL() & "?view=answer&answer=" & DataBinder.Eval(Container.DataItem,"id") %>'
                                        runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="40px" />
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td>
                    <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" PageLinksPerPage="20" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbtFind" />
        <asp:AsyncPostBackTrigger ControlID="ddlStatus" />
    </Triggers>
</asp:UpdatePanel>

