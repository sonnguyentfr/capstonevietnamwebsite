<%@ Control EnableViewState="false" Inherits="DesktopModules.TinTuc.Control.Pages" CodeFile="Pagesadmin.ascx.vb" Language="vb" AutoEventWireup="false" Explicit="True" %>
<div class="card-inner">
    <ul class="pagination justify-content-center justify-content-md-start">
        <li class="page-item"><asp:HyperLink ID="btnfirst" runat="server" CssClass="page-link">Đầu</asp:HyperLink></li>
        <li class="page-item"><asp:HyperLink ID="btnPrevious" runat="server" CssClass="page-link">Trước</asp:HyperLink></li>
        <li class="page-item"><asp:HyperLink ID="btnPg1" runat="server" CssClass="page-link">1</asp:HyperLink></li>
        <li class="page-item"> <asp:HyperLink ID="btnPg2" runat="server" CssClass="page-link">2</asp:HyperLink></li>
        <li class="page-item"><asp:HyperLink ID="btnPg3" runat="server" CssClass="page-link">3</asp:HyperLink></li>
        <li class="page-item"><asp:HyperLink ID="btnPg4" runat="server" CssClass="page-link">4</asp:HyperLink></li>
        <li class="page-item"><asp:HyperLink ID="btnPg5" runat="server" CssClass="page-link">5</asp:HyperLink></li>
        <li class="page-item"><asp:HyperLink ID="btnNext" runat="server" CssClass="page-link">Tiếp</asp:HyperLink></li>
        <li class="page-item"><asp:HyperLink ID="btnLast" runat="server" CssClass="page-link">Cuối</asp:HyperLink></li>
    </ul>
    <!-- .pagination -->
</div>
