<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Setting.ascx.cs" Inherits="DesktopModules.Video.ViewPage.Setting" %>
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
            <div class="dnnFormItem">
                <dnn:Label ID="lbDetails_Cmt" ControlName="txtDetails_Cmt" Text="SL bình luận / trang" runat="server" />
                <asp:TextBox ID="txtDetails_Cmt" runat="server" Width="90" />
            </div>
            <div runat="server" id="div_AllowCmt" class="dnnFormItem">
                <dnn:Label ID="lbDetails_Allow" ControlName="checkDetails_Cmt" Text="Cho phép bình luận" runat="server" />
                <asp:CheckBox ID="checkDetails_Cmt" runat="server" />
            </div>
            <div runat="server" id="div_AllowCmtLogin" class="dnnFormItem">
                <dnn:Label ID="lbDetails_AllowLogin" ControlName="checkDetails_Cmt" Text="Đăng nhập bình luận" runat="server" />
                <asp:CheckBox ID="checkDetails_CmtLogin" runat="server" />
            </div>
            <div runat="server" id="div_AllowCmtFB" class="dnnFormItem">
                <dnn:Label ID="lbDetails_AllowFB" ControlName="checkDetails_CmtFB" Text="Cho phép bình luận Facebook" runat="server" />
                <dnn:DnnCheckBox ID="checkDetails_CmtFb" runat="server" OnCheckedChanged="checkDetails_CmtFb_CheckedChanged" AutoPostBack="true" />
            </div>
            <div runat="server" id="div_AllowCmtFBadmin" class="dnnFormItem">
                <dnn:Label ID="Label1" ControlName="txtDetails_CmtFidApp" Text="ID Facebook Apps" runat="server" />
                <asp:TextBox ID="txt_commentFBId" runat="server" Width="400" />

                <dnn:Label ID="lblAllowCmtFBadmin" ControlName="txtDetails_Cmt" Text="ID Facebook admin" runat="server" />
                <asp:TextBox ID="txt_commentFBadmin" runat="server" Width="400" TextMode="MultiLine" Height="30px" />
                Nhiều tài khoản cách nhau dấu ;
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
