<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="View.ascx.vb" Inherits="NVCMS.Modules.Video.View" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<asp:UpdatePanel ID="udpContent" runat="server">
    <ContentTemplate>
        <div class="nk-block-head nk-block-head-sm">
            <div class="nk-block-between">
                <div class="nk-block-head-content">
                    <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
                    <div class="nk-block-des text-soft">
                        <p>
                            Tổng số có: 
                        <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                            tin bài.
                        </p>
                    </div>
                </div>
                <!-- .nk-block-head-content -->
                
                <!-- .nk-block-head-content -->
            </div>
            <!-- .nk-block-between -->
        </div>
        <div class="nk-block">
            <div class="card card-bordered card-stretch">
                <div class="card-inner-group">
                    <div class="card-inner position-relative card-tools-toggle">
                        <div class="card-title-group">
                            <div class="card-tools">
                            </div>
                            <!-- .card-tools -->
                            <div class="card-tools mr-n1">
                                <ul class="btn-toolbar gx-1">
                                    <li>
                                        <a href="#" class="btn btn-icon search-toggle toggle-search" data-target="search"><em class="icon ni ni-search"></em></a>
                                    </li>
                                    <!-- li -->
                                    <li class="btn-toolbar-sep"></li>
                                    <!-- li -->
                                    <li>
                                        <div class="toggle-wrap">
                                            <a href="#" class="btn btn-icon btn-trigger toggle" data-target="cardTools"><em class="icon ni ni-menu-right"></em></a>
                                            <div class="toggle-content" data-content="cardTools">
                                                <ul class="btn-toolbar gx-1">
                                                    <li class="toggle-close">
                                                        <a href="#" class="btn btn-icon btn-trigger toggle" data-target="cardTools"><em class="icon ni ni-arrow-left"></em></a>
                                                    </li>
                                                    <!-- li -->
                                                    <li>
                                                        <div class="dropdown">
                                                            <a href="#" class="btn btn-trigger btn-icon dropdown-toggle" data-toggle="dropdown">
                                                                <div class="dot dot-primary"></div>
                                                                <em class="icon ni ni-filter-alt"></em>
                                                            </a>
                                                            <div class="filter-wg dropdown-menu dropdown-menu-xl dropdown-menu-right">
                                                                <div class="dropdown-head">
                                                                    <span class="sub-title dropdown-title">Tìm bài viết</span>
                                                                    <div class="dropdown">
                                                                        <a href="#" class="btn btn-sm btn-icon">
                                                                            <em class="icon ni ni-more-h"></em>
                                                                        </a>
                                                                    </div>
                                                                </div>
                                                                <div class="dropdown-body dropdown-body-rg">
                                                                    <div class="row gx-6 gy-3">
                                                                        <div class="col-12">
                                                                            <div class="form-group">
                                                                                <label class="overline-title overline-title-alt">Tiêu đề</label>
                                                                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <div class="form-group">
                                                                                <label class="overline-title overline-title-alt">Từ ngày</label>
                                                                                <asp:TextBox ID="txtStartdate" runat="server" CssClass="form-control form-control-sm form-control-outlined datepicker"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <div class="form-group">
                                                                                <label class="overline-title overline-title-alt">đến ngày</label>
                                                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control form-control-sm datepicker"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-12">
                                                                            <div class="form-group">
                                                                                <asp:Button ID="lbtFind" runat="server" Font-Bold="true" CssClass="btn btn-secondary" ToolTip="Tìm kiếm" Text="Tìm bài viết"></asp:Button>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="dropdown-foot between">
                                                                    <a class="clickable" href="#">Xóa tìm kiếm</a>
                                                                </div>
                                                            </div>
                                                            <!-- .filter-wg -->
                                                        </div>
                                                        <!-- .dropdown -->
                                                    </li>
                                                    <!-- li -->
                                                    <li>
                                                        <div class="dropdown">
                                                            <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="True" CssClass="d-sm-inline form-control form-select link-check">
                                                                <asp:ListItem>30</asp:ListItem>
                                                                <asp:ListItem>50</asp:ListItem>
                                                                <asp:ListItem>100</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <!-- .dropdown -->
                                                    </li>
                                                    <!-- li -->
                                                </ul>
                                                <!-- .btn-toolbar -->
                                            </div>
                                            <!-- .toggle-content -->
                                        </div>
                                        <!-- .toggle-wrap -->
                                    </li>
                                    <!-- li -->
                                </ul>
                                <!-- .btn-toolbar -->
                            </div>
                            <!-- .card-tools -->
                        </div>
                        <!-- .card-title-group -->
                        <div class="card-search search-wrap" data-search="search">
                            <div class="card-body">
                                <div class="search-content">
                                    <a href="#" class="search-back btn btn-icon toggle-search" data-target="search"><em class="icon ni ni-arrow-left"></em></a>
                                    <input type="text" class="form-control " placeholder="Nhập tiêu đề bài viết" id="txtTitle2" runat="server">
                                    <asp:Button ID="lbtFindTitle" runat="server" Font-Bold="true" CssClass="search-submit btn btn-primary" ToolTip="Tìm kiếm" Text="Tìm bài viết"></asp:Button>
                                </div>
                            </div>
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
                                <div class="nk-tb-col"><span class="sub-text">Thông tin</span></div>
                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Trạng thái</span></div>
                                <div class="nk-tb-col nk-tb-col-tools">
                                    <ul class="nk-tb-actions gx-1 my-n1">
                                        <li>
                                            <div class="drodown">
                                                <a href="#" class="btn btn-icon btn-trigger mr-n1"><em class="icon ni ni-more-h"></em></a>
                                            </div>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <asp:Repeater ID="drgDataViewer" runat="server">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <!-- .nk-tb-item -->
                                    <div class="nk-tb-item">
                                        <div class="nk-tb-col nk-tb-col-check">
                                            <div class="custom-control custom-control-sm custom-checkbox notext">
                                                <%#Eval("VideoId") %>
                                            </div>
                                        </div>
                                        <div class="nk-tb-col">
                                            <div class="user-card">
                                                <div class="news-avatar xs bg-primary">
                                                    <a href="<%# Ultis.FormatFullImage(CStr(Eval("ImagePath"))) %>" data-fancybox data-caption="" class="usernewsimage" data-toggle="tooltip" data-placement="top" title="Xem ảnh lớn">
                                                        <asp:Image ImageUrl='<%# Ultis.FormatThumbImage(CStr(Eval("ImagePath")), 120, 70, "", "", "", "") %>' AlternateText="" ID="imgNews" runat="server" />
                                                    </a>
                                                </div>
                                                <div class="user-info">
                                                    <span class="d-sm-inline tb-lead"><%# Highlight(CStr(Eval("Title")), "<span class='highlight'>", "</span>")%></span>
                                                    <br />
                                                    <small>Ngày tạo: <%# DataBinder.Eval(Container.DataItem, "Createdate") %></small>
                                                    <asp:Label ID="lblnewid" runat="server" Text='<%# Eval("videoid") %>' Visible="false" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="nk-tb-col tb-col-lg" id="viewcount2" runat="server">
                                            <span class="badge badge-warning"><%# Eval("StatusName")%></span>
                                        </div>

                                        <div class="nk-tb-col nk-tb-col-tools">
                                            <ul class="nk-tb-actions gx-1">
                                                <li class="nk-tb-action-hidden" >
                                                    <a href="<%#NavigateURL() & "?view=edit&itemid=" & Eval("videoid") %>" class="btn btn-trigger btn-icon user-avatar" data-toggle="tooltip" data-placement="top" title="" data-original-title="Sửa">
                                                        <em class="icon ni ni-edit-fill"></em>
                                                    </a>
                                                </li>
                                                <li class="nk-tb-action-hidden">
                                                    <asp:LinkButton ID="cmdCancelRequest" CssClass="btn btn-trigger btn-icon user-avatar" OnClick="cmdCancelRequest" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "videoid") %>' Visible='<%# Ultis.VideoFormatTrieuHoi(CInt(Eval("Status"))) %>' runat="server" data-toggle="tooltip" data-placement="top" title="" data-original-title="Trả lại tác giả"><em class="icon ni ni-curve-up-left"></em></asp:LinkButton>
                                                </li>
                                                
                                            </ul>
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
                                    <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" pagelinksperpage="20" />
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
        <asp:AsyncPostBackTrigger ControlID="lbtFind" />
        <asp:AsyncPostBackTrigger ControlID="ddlPageSize" />
    </Triggers>
</asp:UpdatePanel>
