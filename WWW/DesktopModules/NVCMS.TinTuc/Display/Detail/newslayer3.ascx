<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="newslayer3.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.newslayer3" %>
<%@ Import Namespace="VLVN.Modules.TinTuc" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<div style="margin:5px;">
<table width="100%">
    <tr>
        <td class="Head" style="border-bottom: 1px #d2d2d2 solid; height:28px; line-height:25px; text-transform:uppercase; font-size:16px;" valign="top">
            <%= PhongBanName%>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lbNoRecordMsg" runat="server" ForeColor="Red" Font-Bold="true" Visible="false" Text="Chưa có dữ liệu"></asp:Label>
            <br />
            <asp:DataGrid ID="drgOtherNews" runat="server" AllowPaging="False" PageSize="10"
                CellPadding="3" AutoGenerateColumns="False" ShowHeader="False" GridLines="None"
                Width="100%">
                <Columns>
                    <asp:TemplateColumn HeaderText="C&#225;c tin kh&#225;c">
                        <ItemTemplate>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <asp:Image ID="imgCat" align="left" BorderColor="#d2d2d2" BorderWidth="1" Width="60" hspace="3"
                                            ImageUrl='<%# DataBinder.Eval(Container, "DataItem.imagepath") %>' runat="server"></asp:Image>
                                        <font style="color:DarkBlue;"><%# BL.FormatLoaiTinBaiText(Eval("NewsKind"))%>:</font>
                                        <img src='<%# Eval("TypeUrl") %>' border="0" alt="" style="vertical-align:bottom;" />
                                        <asp:HyperLink CssClass="link_news_hot" Font-Bold="true" ForeColor='<%# Ultis.FormatViewColor(PortalId,UserId,Eval("UsersView"), Eval("UsersGet")) %>' ID="hplCatTitle" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"title")%>'
                                            NavigateUrl='<%# FormatURL("itemid",DataBinder.Eval(Container.DataItem,"newid"),"catid",DataBinder.Eval(Container.DataItem,"CategoryId")) %>'  />
                                            <%# Ultis.FormatIconGetNews(UserId, Eval("NewId"))%>
                                        <br />
                                        <font style="color:Maroon;">Tác giả:</font><asp:Label ID="lblCreatedInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetCreatedInfo(PortalId,Eval("UserId"),Eval("CoAuthor2"),Eval("CoAuthor2"),Eval("CoAuthor3"),Eval("Createdate")) %>'></asp:Label> | 
                                        <font style="color:Maroon;">Phê duyệt:</font><asp:Label ID="lblApprovalInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetApprovalInfo(PortalId,Eval("ApprovalUser"),Eval("ApprovalDate")) %>'></asp:Label> | 
                                        <font style="color:Maroon;">Duyệt xuất bản:</font><asp:Label ID="lblPublishInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetLDXLInfo(PortalId,Eval("PublishedUser"),Eval("PublishedDate")) %>'></asp:Label>
                                        <br />
                                        <asp:Label ID="lbCatSummary" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"summary")%>' /></td>
                                </tr>
                                <tr>
                                    <td height="5">
                                    </td>
                                </tr>
                                <tr>
                                    <td height="5"  style="border-bottom:dotted #d7d7d7 1px;">
                                        <img src="/images/spacer.gif" width="1" height="1" alt="" /></td>
                                </tr>
                                <tr>
                                    <td height="5">
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>
        </td>
    </tr>
    <tr>
        <td>
            <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="PostBack" AutoPostBack="true" PageLinksPerPage="20" />
        </td>
    </tr>
</table>
</div>
<script type="text/javascript" language="javascript">
    $('a[title="IMAGES"]').on('click', function () {
        window.open($(this).find('img').attr("src"));
        return false;
    });
</script>