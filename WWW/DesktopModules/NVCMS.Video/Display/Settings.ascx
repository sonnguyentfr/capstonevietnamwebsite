<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Settings.ascx.vb" Inherits="ThuongTruong.Modules.Video.Settings" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls" Assembly="DotNetNuke.Web" %>
<div class="dnnForm dnnHTMLSettings dnnClear ib-setting">
    <div class="box">
    <fieldset class="fieldset">
        <legend>Hiển thị danh sách video</legend>
         <div class="dnnFormItem">
            <dnn:Label ID="lbList_ShowTop" ControlName="checkList_ShowTop" Text="" runat="server" />
            <dnn:DnnCheckBox ID="checkList_ShowTop" runat="server" Text="Hiển thị TOP Video" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lbList_PageSize" ControlName="txtList_PageSize" Text="SL video / trang" runat="server" />
            <dnn:DnnTextBox ID="txtList_PageSize" runat="server" />
        </div>
        <div class="dnnFormItem">
            <dnn:Label ID="lbList_ShowPage" ControlName="checkList_ShowPage" Text="" runat="server" />
            <dnn:DnnCheckBox ID="checkList_ShowPage" runat="server" Text="Hiển thị phân trang" />
        </div>
    </fieldset>
    <fieldset class="fieldset">
        <legend>Hiển thị chi tiết tin video</legend>
        <div class="dnnFormItem">
            <dnn:Label ID="lbDetails_More" ControlName="txtDetails_More" Text="SL video liên quan" runat="server" />
            <dnn:DnnTextBox ID="txtDetails_More" runat="server" Width="30" />
        </div>    
    </fieldset>
   </div>
</div>