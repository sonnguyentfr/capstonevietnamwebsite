<%@ Control Language="VB" AutoEventWireup="false" CodeFile="tinlienquan.ascx.vb" Inherits="NVCMS.Modules.TinTuc.newsedittinleinquan" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<style>
    .tlq-popup-toolbar {
        display: flex;
        flex-wrap: wrap;
        gap: 8px;
        align-items: flex-end;
        margin-bottom: 8px;
    }

        .tlq-popup-toolbar .tlq-field {
            min-width: 130px;
            margin: 0;
        }

        .tlq-popup-toolbar .tlq-field-title {
            flex: 1 1 260px;
        }

        .tlq-popup-toolbar .tlq-field-date {
            width: 140px;
        }

        .tlq-popup-toolbar label {
            margin-bottom: 4px;
            font-size: 12px;
            font-weight: 600;
        }

        .tlq-popup-toolbar .form-control,
        .tlq-popup-toolbar .select2 {
            height: 32px;
            font-size: 12px;
            padding: 4px 8px;
        }

    .tlq-popup-search {
        width: 34px;
        height: 34px;
        padding: 0;
        border: 0;
        background: transparent;
    }

        .tlq-popup-search img {
            width: 24px;
            height: 24px;
        }
</style>

<ul class="to_do" style="display: none;">
</ul>
<asp:UpdatePanel ID="tinleinquan" runat="server">
    <ContentTemplate>
        <div class="tlq-popup-toolbar">
            <div class="form-group tlq-field tlq-field-title">
                <label>Tiêu đề</label>
                <input class="form-control" id="txtTitle" runat="server" />
            </div>

            <div class="form-group tlq-field">
                <label>Chuyên mục</label>
                <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="form-group tlq-field">
                <label>Tác giả</label>
                <asp:DropDownList ID="ddlUserPost" runat="server" AutoPostBack="true" CssClass="form-control select2"></asp:DropDownList>
            </div>

            <div class="form-group tlq-field tlq-field-date">
                <label>Từ ngày</label>
                <input type="text" id="txtStartdate" runat="server" class="form-control datepicker" />
            </div>

            <div class="form-group tlq-field tlq-field-date">
                <label>Đến ngày</label>
                <input type="text" id="txtEndDate" runat="server" class="form-control datepicker" />
            </div>

            <div class="form-group tlq-field" style="min-width: 34px;">
                <label>&nbsp;</label>
                <asp:ImageButton ID="lbtFind" runat="server" Font-Bold="true" CssClass="tlq-popup-search" ImageUrl="/images/icons/magnifier32.png" ToolTip="Tìm kiếm"></asp:ImageButton>
            </div>
        </div>

        <div class="ln_solid" style="margin: 8px 0;"></div>
        <div class="col-md-12">
            <table id="datatable-checkboxtinlienquan" class="table table-striped table-bordered bulk_action">
                <thead>
                    <tr>
                        <th style="width: 80px; text-align: center;">Ảnh</th>
                        <th>Tiêu đề</th>
                        <th>Danh mục</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rpttinlienquan" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 60px; text-align: center;">
                                    <asp:Image Width="60" ImageUrl='<%# Ultis.FormatThumbImage(CStr(Eval("imagepath")), 60, 60, "", "", "", "")%>'
                                        AlternateText="" ID="imgNews" runat="server" /></td>
                                <td>
                                    <a data-id="<%# Eval("newid") %>" data-title="<%# HtmlUtils.StripPunctuation(CStr(Eval("Title")), True).Replace("'", "")%>"
                                        data-image="<%# Eval("ImagePath") %>" data-sumary="<%# Eval("Summary") %>" data-catid="<%# Eval("CategoryId") %>"
                                        data-link="<%#Ultis.FormatLink(CInt(BL.GetMappingTabIDByCategoryID(CInt(Eval("CategoryId")))), CInt(Eval("NewId")), CStr(Eval("Title"))) %>" class="themvao">
                                        <h5><strong><%# Eval("Title")%></strong></h5>
                                    </a>
                                    <font style="color: Maroon;">Tác giả:</font>
                                    <asp:Label ID="lblCreatedInfo" ForeColor="Maroon"
                                        runat="server" Text='<%# BL.GetCreatedInfo(0, CInt(Eval("UserId")), CDate(Eval("Createdate")))%>'></asp:Label>
                                    | <font style="color: Maroon;">Duyệt xuất bản:</font>
                                    <asp:Label ID="lblPublishInfo"
                                        ForeColor="Maroon" runat="server" Text='<%# BL.GetPublishedInfo(0, CInt(Eval("PublishedUser")), CDate(Eval("PublishedDate")))%>'></asp:Label>
                                </td>
                                <td><%#Eval("CategoryName") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <div class="g">
            <ul class="pagination justify-content-center justify-content-md-start">
                <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="PostBack" PageLinksPerPage="50" />
            </ul>
            <!-- .pagination -->
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ddlCategory" />
        <asp:AsyncPostBackTrigger ControlID="ddlUserPost" />
    </Triggers>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div class="loading">
            <div></div>
            <div></div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<script>

    var txtTitle = document.getElementById('<%=txtTitle.ClientID%>').value;
    var danhmuc = document.getElementById('<%=ddlCategory.ClientID%>').value;
    var uid = document.getElementById('<%=ddlUserPost.ClientID%>').value;
    var from = document.getElementById('<%=txtStartdate.ClientID%>').value;
    var to = document.getElementById('<%=txtEndDate.ClientID%>').value;

</script>

