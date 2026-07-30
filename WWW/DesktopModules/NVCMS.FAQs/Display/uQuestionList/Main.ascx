<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Main.ascx.vb" Inherits="NVCMS.Modules.FAQs.uQuestionList" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<asp:Literal ID="ltContent" runat="server" />
<br />
<div class="pagination">
<vbuzz:PAGING ID="vbPaging" runat="server" /></div>