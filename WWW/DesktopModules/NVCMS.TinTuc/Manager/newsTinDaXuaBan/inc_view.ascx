<%@ Control Language="vb" AutoEventWireup="false"  Explicit="true" codefile="inc_view.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.newsapprove.Approve_inc_view" %>
<%@ Import Namespace="VLVN.Modules.TinTuc" %>
<%@ Register TagPrefix="dnn" TagName="SectionHead" Src="~/controls/SectionHeadControl.ascx" %>

<div class="pustyle">
<div class="toolbar-placeholder">
        <div class="toolbarBox toolbarHead">
            <ul class="cc_button">
                <li style="padding-left:20px;"><asp:linkbutton id="lbtApproveTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/icons/page_go.png" alt="Thực hiện"/> Thực hiện => </asp:linkbutton></li>
                <li><asp:DropDownList ID="ddlWFTop" runat="server" DataTextField="TenLuong" DataValueField="ID"></asp:DropDownList></li>
                <li style="padding-left:20px;"><asp:linkbutton id="lbtEditTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/edit.gif" alt="Sửa tin" /> Sửa tin</asp:linkbutton></li>
                <li style="padding-left:20px;"><asp:linkbutton id="lbtReturnTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/icons/arrow_redo.png" alt="Trả lại" /> Trả lại</asp:linkbutton></li>
                <li style="padding-left:20px;"><asp:linkbutton id="lbtCancelTop" runat="server" Font-Bold="True" CssClass="toolbar_btn"><img src="/images/icons/arrow_rotate_clockwise.png" alt="Quay lại" /> Thoát</asp:linkbutton></li>
                <li style="padding-left:20px;">
                    <asp:linkbutton id="lbtPrint" CssClass="toolbar_btn" runat="server" Font-Bold="True" OnClientClick="printContent(); return false;">
                        <img src="/images/icons/printer.png" alt="Lưu thay đổi" /> In tin bài
                    </asp:linkbutton>
                </li>
            </ul>
            <div class="clear"></div>
        </div>
    </div>
</div>
<div id="tblContent" style="margin: 0 auto;width:800px;">
<table width="100%" cellpadding="2" cellspacing="2" border="0" class="tblBriefInfo" style="background-color:#d2d2d2; padding:5px;">
    <tr>
        <td style="width:120px;" class="w-label">Tiêu đề: </td>
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
        <td align="center" style="text-align: justify;"><asp:Label ID="lbSummary" runat="server" Text=""></asp:Label></td>
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
            <dnn:SectionHead ID="dshSearch" runat="server" Text="Bút phê" Section="tblButPhe" IncludeRule="True" IsExpanded="False" />
            <table cellpadding="0" cellspacing="0" width="100%" id="tblButPhe" runat="server">
                <tr id="trButPhe" runat="server">
                    <td class="w-label" style="width:100px;">Danh sách: </td>
                    <td class="w-control">
                        <asp:datagrid id="drgDataViewer" DataKeyField ="ID" Width="100%" runat="server" AllowPaging="false" 
                            AutoGenerateColumns="False" CssClass="table-bordered" ShowHeader="false">
	                        <ItemStyle CssClass="TRgrid"></ItemStyle>
	                        <HeaderStyle Font-Bold="true" ForeColor="White" BackColor="#006699"></HeaderStyle>
	                        <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
	                        <Columns>
	                            <asp:TemplateColumn>
	                                <ItemTemplate>
	                                    <asp:Label ID="Label7" runat="server" Text='<%# BL.GetUserName(PortalId,DataBinder.Eval(Container.DataItem,"ByUser")) %>'></asp:Label>
                                        (<asp:Label ID="Label6" runat="server" Text='<%# BL.FormatDate(DataBinder.Eval(Container.DataItem,"CreateDate")) %>'></asp:Label>): 
	                                </ItemTemplate>
                                    <ItemStyle Width="130"></ItemStyle>
	                            </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Bút phê">
                                    <ItemTemplate>
                                        <asp:Label ID="lblButPhe" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Comment").ToString().Replace(ControlChars.NewLine,"<br>") %>'></asp:Label> <br />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
	                        </Columns>
                        </asp:datagrid>
                    </td>
                </tr>
                <tr>
                    <td class="w-label" style="width:100px;">Nội dung:</td>
                    <td class="w-control"><asp:TextBox ID="txtButPhe" runat="server" TextMode="MultiLine" Rows="2" Width="100%"></asp:TextBox></td>
                </tr>
            </table>        
        </td>
    </tr>
</table>
</div>
<script type="text/javascript">
    // Click 2 play
    var storagePath = '<%= StorageFolder %>';
    $('a[title="Play"]').on('click', function () {
        ViewMedia(storagePath + "/" + getDecodeString($(this).attr("href").match(/[^\/\\]+$/)));
        return false;
    });
    $('a[title="Download"]').on('click', function () {
        window.open(storagePath + "/" + getEncodedString($(this).attr("href").match(/[^\/\\]+$/)));
        return false;
    });
    $('a[title="IMAGES"]').on('click', function () {
        window.open($(this).find('img').attr("src"));
        return false;
    });
</script>