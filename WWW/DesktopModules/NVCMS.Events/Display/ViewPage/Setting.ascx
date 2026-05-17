<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Setting.ascx.cs" Inherits="DesktopModules.TinTuc.ViewPage.Setting" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<div class="dnnForm dnnHTMLSettings dnnClear ib-setting">
    <div class="box">
        
        <fieldset class="fieldset">
            <h5>Hiển thị danh sách</h5>
            <div class="dnnFormItem">
                <dnn:Label ID="lbList_PageSize" ControlName="txtList_PageSize" Text="SL Tin / trang" runat="server" />
                <asp:TextBox ID="txtList_PageSize" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lbList_Template" ControlName="cbList_Template" Text="Template" runat="server" />
                <asp:DropDownList ID="cbList_Template" runat="server" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lbList_ImgSize" ControlName="txtList_ImgWidth" Text="Kích thước ảnh" runat="server" />
                <asp:TextBox ID="txtList_ImgWidth" runat="server" Width="90" />
                <span>x</span>
                <asp:TextBox ID="txtList_ImgHeight" runat="server" Width="90" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lbList_SizeDes" ControlName="txtList_SizeDes" Text="Giới hạn ký tự mô tả" runat="server" />
                <asp:TextBox ID="txtList_SizeDes" runat="server" Width="90" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="lbList_ShowPage" ControlName="checkList_ShowPage" Text="" runat="server" />
                <dnn:DnnCheckBox Checked="true" ID="checkList_ShowPage" runat="server" Text="Hiển thị phân trang" />
            </div>
        </fieldset>
        <fieldset class="fieldset">
            <h5>Hiển thị chi tiết</h5>
            <div class="dnnFormItem">
                <dnn:Label ID="lbDetails_Template" ControlName="cbDetails_Template" Text="Template" runat="server" />
                <asp:DropDownList ID="cbDetails_Template" runat="server" />
            </div>
            
            <div class="dnnFormItem" style="display: none;">
                <dnn:Label ID="lbDetails_Other" ControlName="txtDetails_Other" Text="SL tin mới cập nhật" runat="server" />
                <asp:TextBox ID="txtDetails_Other" runat="server" Width="90" />
            </div>
            
        </fieldset>
        <fieldset class="fieldset">
            <h5>Tin cùng chuyên mục</h5>
            <div class="dnnFormItem">
                <dnn:Label ID="Label4" ControlName="cbDetails_Template" Text="Kiểu hiện thị" runat="server" />
                <asp:RadioButton Checked="true" AutoPostBack="true" GroupName="GetType" ID="rdGetType_Fix" runat="server" Text="Cố định" OnCheckedChanged="rdGetType_CheckedChanged" />
                <asp:RadioButton AutoPostBack="true" GroupName="GetType" ID="rdGetType_Scroll" runat="server" Text="Tự động tải" OnCheckedChanged="rdGetType_CheckedChanged" />
            </div>
            <div class="dnnFormItem">
                <dnn:Label ID="Label5" ControlName="txtDetails_More" Text="Số lượng tin: " runat="server" />
                <asp:TextBox ID="txttincungchuyenmucsoluong" runat="server" Width="90" />
            </div>
            <div class="dnnFormItem" id="div_rdGetType_Scroll" runat="server" visible="false">
                <dnn:Label ID="Label6" ControlName="txtDetails_Other" Text="Số trang" runat="server" />
                <asp:TextBox ID="txttincungchuyenmucsotrang" runat="server" Width="90" />
            </div>
            
        </fieldset>
    </div>
</div>
