<%@ Control Language="vb" AutoEventWireup="false"  Explicit="true" codefile="inc_viewimg.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.newsapprove.inc_view" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>

<div class="pustyle">
<div class="toolbar-placeholder">
        <div class="toolbarBox toolbarHead">
            <ul class="cc_button">
                <li><asp:DropDownList ID="ddlWFTop" runat="server" DataTextField="TenLuong" DataValueField="ID"></asp:DropDownList></li>
                <li><asp:linkbutton id="lbtApproveTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/icons/page_go.png" alt="Thực hiện"/> Thực hiện</asp:linkbutton></li>
                <li><asp:linkbutton id="lbtEditTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/edit.gif" alt="Sửa tin" /> Sửa ảnh</asp:linkbutton></li>
                <li><asp:linkbutton id="lbtReturnTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/icons/arrow_redo.png" alt="Trả lại" /> Trả lại</asp:linkbutton></li>
                <li><asp:linkbutton id="lbtCancelTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/icons/arrow_rotate_clockwise.png" alt="Quay lại" /> Thoát</asp:linkbutton></li>
            </ul>
            <div class="clear"></div>
        </div>
    </div>
</div>
<div id="tblContent" style="margin: 0 auto;">
<table width="100%" cellpadding="2" cellspacing="2" border="0" class="tblBriefInfo" style="background-color:#d2d2d2; padding:5px;">
    <tr>
        <td style="width:120px;" class="w-label">Tiêu đề album: </td>
        <td><asp:Label ID="lbTitle" runat="server" Text="" Font-Bold="true"></asp:Label><asp:Image ID="imgHot" runat="server" ImageUrl="/images/vov/hot.jpg"/></td>
        <td rowspan="3" align="right"><asp:Image BorderWidth="1" BorderColor="#d2d2d2" Width="100" ID="imgNews" runat="server" /></td>
    </tr>
    <tr>
        <td class="w-label">Chuyên mục chính:</td>
        <td><asp:Label ID="lbCagegoryName" runat="server" Text="" ForeColor="Maroon"></asp:Label></td>
    </tr>
    <tr>
        <td class="w-label">Tác giả: </td>
        <td><asp:Label ID="lbUserName" runat="server" Text="" ForeColor="Maroon"></asp:Label></td>
    </tr>
</table>
<table id="tableContent" width="100%" cellpadding="2" cellspacing="2" border="0" style="padding-top:5px; margin: 0 auto;text-align:justify;">
    <tr>
        <td align="center" style="text-align: justify;">Mô tả:<asp:Label ID="lbSummary" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="Label1" runat="server" ForeColor="Maroon" Font-Bold="true" Text="Danh sách ảnh"></asp:Label>
            <asp:UpdatePanel ID="udpContent" runat="server">
                <ContentTemplate>
                    <asp:DataList id="grdFiles" resourcekey="grdUpload" runat="server" DataKeyField="Id" AutoGenerateColumns="False" 
                    CssClass="table-bordered" Width="100%" RepeatColumns="3">
				        <HeaderStyle Font-Bold="true" ForeColor="White" BackColor="#006699"></HeaderStyle>
				        <ItemTemplate>
				            <table width="100%">
				                <tr>
				                    <td width="125px">
				                        <asp:Image ImageUrl='<%# Ultis.FormatThumbImage(Server,Ctype(Container.DataItem.MediaUrl, String),127) %>' runat="server" id="Image1" Width="120px" AlternateText="Media Type Image"></asp:Image>
				                    </td>
				                    <td valign="top">
				                        <b> Mô tả:</b><br/>
				                        <%# Ctype(Container.DataItem.Description, String) %>
				                    </td>
				                </tr>
				            </table>
				        </ItemTemplate>
			        </asp:DataList>
                </ContentTemplate>
        </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="2" cellspacing="2" class="table-bordered" style="background-color:#f0f0f0;">
                <tr>
                    <td class="w-label" style="width:100px;">Tin liên quan: </td>
                    <td>
                        <div class="list-lq">
                            <ul>
                                <asp:Repeater runat="server" ID="rptRelated">
                                    <ItemTemplate>
                                        <li>
                                            <a href='<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem,"CategoryId"), Integer)),CType(DataBinder.Eval(Container.DataItem,"NewId"), Integer),CType(DataBinder.Eval(Container.DataItem,"Title"), String)) %>'> <%# Eval("Title")%> </a>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="w-label">Tags: </td>
                    <td>
                        <asp:Repeater runat="server" ID="rptTags">
                            <ItemTemplate>
                                <a href="#"><%# Eval("Name") %></a>
                            </ItemTemplate>
                            <SeparatorTemplate>, </SeparatorTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>    
        </td>
    </tr>
    <tr>
        <td>
            <dnn:SectionHead ID="dshSearch" runat="server" Text="Bút phê" Section="trButPhe" IncludeRule="True" IsExpanded="False" />
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr id="trButPhe" runat="server">
                    <td class="subhead" style="width:100px;">Bút phê</td>
                    <td><asp:TextBox ID="txtButPhe" runat="server" TextMode="MultiLine" Rows="3" Width="100%"></asp:TextBox> </td>
                </tr>
            </table>        
        </td>
    </tr>
</table>
</div>