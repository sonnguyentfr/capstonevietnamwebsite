<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="ViewStatic.ascx.vb" Inherits="DesktopModules.NV_Events.Manager.Events.View" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<script src="/DesktopModules/NVCMS.Events/js/jquery.plugin.js"></script>
<script src="/DesktopModules/NVCMS.Events/js/jquery.countdown.js"></script>
<script src="/DesktopModules/NVCMS.Events/js/jquery.countdown-vi.js"></script>
<link rel="stylesheet" href="/DesktopModules/NVCMS.Events/js/jquery.countdown.css">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="nk-block-head nk-block-head-sm">
            <div class="nk-block-between">
                <div class="nk-block-head-content">
                    <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Title %></h3>
                    <div class="nk-block-des text-soft">
                        <p>
                            Tổng số có: 
                        <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                            bản ghi.
                        </p>
                    </div>
                </div>
                <!-- .nk-block-head-content -->
            </div>
            <!-- .nk-block-between -->
        </div>
        <div class="nk-block">
            <div class="card card-bordered card-stretch">
                <div class="card-inner-group">
                    <div class="card-inner position-relative">
                        <div class="card-body">
                            <h4>
                                <asp:Literal ID="ltrtitlte" runat="server"></asp:Literal></h4>
                        </div>
                    <!-- .card-search -->
                    </div>
                    <!-- .card-inner -->
                    <div class="card-inner p-0">
                        <div class="nk-tb-list nk-tb-ulist">
                            <div class="nk-tb-item nk-tb-head">
                                <div class="nk-tb-col nk-tb-col-check">
                                    <div class="custom-control custom-control-sm custom-checkbox notext">
                                        #
                                    </div>
                                </div>
                                <div class="nk-tb-col"><span class="sub-text">Họ và tên</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">Email</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">Số điện thoại</span></div>
                                <div class="nk-tb-col tb-col-md"><span class="sub-text">Khác</span></div>
                                
                            </div>
                            <asp:Repeater ID="drgDataViewer" runat="server">
                                <ItemTemplate>
                                    <!-- .nk-tb-item -->
                                    <div class="nk-tb-item">
                                        <div class="nk-tb-col nk-tb-col-check">
                                            <div class="custom-control custom-control-sm custom-checkbox notext">
                                                <%#Eval("id") %>
                                            </div>
                                        </div>
                                        <div class="nk-tb-col">
                                            <%#Eval("hovaten") %>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <%#Eval("Email") %>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <%#Eval("dienthoai") %>
                                        </div>
                                        <div class="nk-tb-col" style="width: 20%">
                                            Ngày đăng ký: <%#CDate(Eval("Createddate")).ToString("HH:mm - dd/mm/yyy") %><br />
                                            IP:  <%#Eval("ip") %>
                                        </div>
                                        
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <!-- .nk-tb-item -->
                        </div>
                        <!-- .nk-tb-list -->
                    </div>
                    <!-- .card-inner -->
                    <div class="card-inner">
                        <div class="nk-block-between-md g-3">
                            <div class="g">
                                <ul class="pagination justify-content-center justify-content-md-start">
                                    <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" PageLinksPerPage="20" />
                                </ul>
                                <!-- .pagination -->
                            </div>
                            <!-- .pagination-goto -->
                        </div>
                        <!-- .nk-block-between -->
                    </div>
                    <!-- .card-inner -->
                </div>
                <!-- .card-inner-group -->
            </div>
            <!-- .card -->
        </div>

    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>


