<%@ Control Language="VB" AutoEventWireup="false" CodeFile="permissionSetting.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.permission.permissionSetting" %>
<asp:Label ID="lblRoleGroup" runat="server" resourcekey="lblRoleGroup" Text="Chọn nhóm quyền"></asp:Label>
<asp:DropDownList ID="drdRoleGroup" runat="server" DataValueField="RoleGroupId" DataTextField="RoleGroupName"
    Width="250px">
</asp:DropDownList>