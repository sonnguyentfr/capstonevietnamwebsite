<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="DesktopModules.Video.ViewPage.Details" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<%--<%@ Register TagPrefix="vbuzz" TagName="LASTEST" Src="~/DesktopModules/NVCMS.TinTuc/Control/Lastest.ascx" %>--%>
<%@ Register TagPrefix="vbuzz" TagName="RELATED" Src="~/DesktopModules/NVCMS.Video/Control/Related.ascx" %>
<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
	<div class="mt-4">
        <asp:Literal ID="ltContent" runat="server" />
		</div>
        <vbuzz:RELATED runat="server" ID="vbRelated" />
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div class="loading" id="loadizng">Loading&#8230;</div>
    </ProgressTemplate>
</asp:UpdateProgress>

