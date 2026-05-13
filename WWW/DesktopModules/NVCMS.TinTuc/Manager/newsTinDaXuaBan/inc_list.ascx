<%@ Control Language="vb" AutoEventWireup="false" CodeFile="inc_list.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.newsapprove.Approve_inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="nk-block-head nk-block-head-sm">
            <disv class="nk-block-between">
                <div class="nk-block-head-content">
                    <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
                    <div class="nk-block-des text-soft">
                        <p>
                            Tổng số có: 
                        <asp:Label ID="lbTotalNewsCount" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                            tin bài.
                        </p>
                    </div>
                </div>
            </disv>
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
                                                                        <div class="col-12">
                                                                            <div class="form-group">
                                                                                <label class="overline-title overline-title-alt">Chuyên mục</label>
                                                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select form-control form-control-sm" AutoPostBack="true"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-12">
                                                                            <div class="form-group">
                                                                                <label class="overline-title overline-title-alt">Tác giả</label>
                                                                                <asp:DropDownList ID="ddlUserPost" runat="server" CssClass="form-select form-control form-control-sm" AutoPostBack="true"></asp:DropDownList>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <div class="form-group">
                                                                                <label class="overline-title overline-title-alt">Từ ngày</label>
                                                                                <asp:TextBox ID="txtStartdate" runat="server" CssClass="form-control form-control-sm form-control-outlined date-picker"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-6">
                                                                            <div class="form-group">
                                                                                <label class="overline-title overline-title-alt">đến ngày</label>
                                                                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control form-control-sm  date-picker"></asp:TextBox>
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
                        Chọn 
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" Enabled="false" />
                        để 
                        <asp:LinkButton Visible="false" ID="lbtApprove" Font-Bold="true" runat="server" CssClass="StandardButton">Phê duyệt</asp:LinkButton>
                        <asp:LinkButton ID="lbtSendBack" Font-Bold="true" CssClass="btn btn-primary btn-dim btn-outline-light" OnClientClick="javascript:return confirm('Trả lại tin bài đã chọn?');" runat="server">
                            <em class="icon ni ni-arrow-up-left"></em><span>Trả lại biên tập</span>
                        </asp:LinkButton>
                    </div>
                    <div class="card-inner p-0">
                        <div class="nk-tb-list nk-tb-ulist">
                            <div class="nk-tb-item nk-tb-head">
                                <div class="nk-tb-col nk-tb-col-check">
                                    <div class="custom-control custom-control-sm custom-checkbox notext">
                                        <input type="checkbox" class="custom-control-input" id="uid">
                                        <label class="custom-control-label" for="uid"></label>
                                    </div>
                                </div>
                                <div class="nk-tb-col"><span class="sub-text">Thông tin</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">Tác giả</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">Danh mục</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">Nhuận bút</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">View</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">Share</span></div>
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
                                <ItemTemplate>
                                    <!-- .nk-tb-item -->
                                    <div class="nk-tb-item">
                                        <div class="nk-tb-col nk-tb-col-check">
                                            <div class="custom-control custom-control-sm custom-checkbox notext">
                                                <input type="checkbox" class="custom-control-input" id="uid1">
                                                <label class="custom-control-label" for="uid1"></label>
                                            </div>
                                        </div>
                                        <div class="nk-tb-col">
                                            <div class="user-card">
                                                <div class="news-avatar xs bg-primary">
                                                    <a href="<%# Ultis.FormatFullImage(Eval("imagepath")) %>" data-fancybox data-caption="" class="usernewsimage" data-toggle="tooltip" data-placement="top" title="Xem ảnh lớn">
                                                        <asp:Image ImageUrl='<%# Ultis.FormatThumbImage(Eval("imagepath"), 120, 70, "", "", "", "") %>' AlternateText="" ID="imgNews" runat="server" />
                                                    </a>
                                                </div>
                                                <div class="user-info">
                                                    <spans class="badge badge-light"><%# BL.FormatLoaiTinBaiHTML(Eval("NewsKind"))%></spans>
                                                    <img src='<%# Eval("TypeUrl") %>' border="0" alt="" style="vertical-align: bottom;" />
                                                    <span class="d-sm-inline tb-lead"><%# Highlight(Eval("Title"), "<span class='highlight'>", "</span>")%></span>
                                                    <asp:Image ID="imgHot" runat="server" ImageUrl="/images/vov/hot.jpg" Visible='<%# iif(CType(eval("HotCat"), Boolean),"True","False") %>' />
                                                    <span id="tinnng" runat="server" visible='<%#IIf(Eval("HotCat"), "True", "False") %>'><em class="icon ni ni-hot text-danger"></em></span>
                                                    <span id="tinanh" runat="server" visible='<%#IIf(Eval("IsImage"), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                                    <span id="video" runat="server" visible='<%#IIf(Eval("IsImage"), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                                    <br />
                                                    <small>
                                                        <font style="color: Maroon;">Tác giả:</font>
                                                        <asp:Label ID="lblCreatedInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetCreatedInfo(PortalId,Eval("UserId"),Eval("Createdate")) %>'></asp:Label>
                                                        | 
                <font style="color: Maroon;">Duyệt:</font>
                                                        <asp:Label ID="lblApprovalInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetApprovalInfo(PortalId,Eval("ApprovalUser"),Eval("ApprovalDate")) %>'></asp:Label>
                                                        | 
                <font style="color: Maroon;">Xuất bản:</font>
                                                        <asp:Label ID="lblPublishInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetPublishedInfo(PortalId,Eval("PublishedUser"),Eval("PublishedDate")) %>'></asp:Label>
                                                    </small>
                                                    <br />
                                                    <small>
                                                        <asp:Label ForeColor="Orange" Font-Size="11px" ID="lblEdittedBy" runat="server" Text='<%# Ultis.FormatEdittedBy(PortalId, DataBinder.Eval(Container.DataItem, "newid")) %>' Visible='<%# Eval("IsEdited") %>'></asp:Label>&nbsp; 
                                                        <asp:LinkButton ID="cmdUnlock" CssClass="btn btn-trigger" ForeColor="Orange" OnClick="GetUnlockNews" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "newid") %>' Visible='<%# Ultis.FormatLockByUser(PortalId, Eval("NewId"), UserId) %>' runat="server" data-toggle="tooltip" data-placement="top" title="" data-original-title="Mở khóa tin bài">
                                                            <em class="icon ni ni-unlock"></em>
                                                        </asp:LinkButton>
                                                    </small>
                                                    <asp:Label ID="lblnewid" runat="server" Text='<%# Eval("newid") %>' Visible="false" />

                                                </div>
                                            </div>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <span class="tb-status text-warning"><%# BL.GetButDanh(PortalId, Eval("Userid"))%></span>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <span class="tb-status text-info"><%# Eval("CategoryName")%></span>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <span class="curency auto"><%# Ultis.GetTienNhuanBut(Eval("NewId"))%></span>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <span class="badge badge-warning"><%# Eval("ViewCount")%> </span>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <span class="badge badge-danger"><a class="text-white" href="<%#NavigateURL() & "?view=share&itemid=" & Eval("NewId") %>" data-toggle="tooltip" data-placement="top" data-original-title="Click vào đây để cập nhật link đã share">
                                                <%# Ultis.CountLinkShare(Eval("Newid"))%></a></span>
                                        </div>
                                        <div class="nk-tb-col nk-tb-col-tools">
                                            <ul class="nk-tb-actions gx-1">
                                                <li class="nk-tb-action-hidden">
                                                    <asp:HyperLink
                                                        ID="hptSua"
                                                        NavigateUrl='<%#PageSuaBai & "?itemid=" & DataBinder.Eval(Container.DataItem, "newid") %>'
                                                        runat="server"
                                                        CssClass="btn btn-trigger btn-icon user-avatar"
                                                        data-toggle="tooltip"
                                                        data-placement="top"
                                                        title=""
                                                        data-original-title="Sửa"
                                                        Visible='<%# Ultis.NewsDaXuatBanCanEdit(Eval("newId"), UserId)  %>'>
                                                        <em class="icon ni ni-edit-fill"></em>
                                                    </asp:HyperLink>
                                                </li>
                                                <li>
                                                    <div class="drodown">
                                                        <a href="#" class="dropdown-toggle btn btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                        <div class="dropdown-menu dropdown-menu-right">
                                                            <ul class="link-list-opt no-bdr">
                                                                <li>
                                                                    <asp:LinkButton ID="cmdSethistory" OnClick="cmdSethistory" CommandName="cmdSethistory" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "newid") %>' ToolTip="Lịch sử bài viết" runat="server">
                                                                        <em class="icon ni ni-shrink"></em><span>Lịch sử</span>
                                                                    </asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:HyperLink ID="hplLichsuSua" NavigateUrl='<%#NavigateURL() & "?view=version&itemid=" & DataBinder.Eval(Container.DataItem, "newid") %>' runat="server" Visible='<%# Eval("CanViewLock") %>'>
                                                                        <em class="icon ni ni-shrink"></em><span>Lịch sử chỉnh sửa</span>
                                                                    </asp:HyperLink>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="cmdButPhe" OnClick="cmdButPhe" CommandName="cmdButPhe" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "newid") %>' Visible='<%# Ultis.NewsNotesShow(PortalId, Eval("NewId")) %>' ToolTip="Thông tin bút phê" runat="server">
                                                                        <em class="icon ni ni-comments"></em><span>Thông tin bút phê</span>
                                                                    </asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <a target="_blank" href='<%#Ultis.FormatLinkadminXemtruoc("/news", CInt(Eval("NewId")), Eval("Title")) %>'>
                                                                        <em class="icon ni ni-eye"></em><span>Xem trước</span>
                                                                    </a>
                                                                </li>
                                                                <li>
                                                                    <asp:HyperLink ID="HyperLink1" NavigateUrl='<%#PageSuaBai & "?itemid=" & DataBinder.Eval(Container.DataItem, "newid") %>' runat="server" Visible='<%# Ultis.NewsDaXuatBanCanEdit(Eval("newId"), UserId)  %>'>
                                                                        <em class="icon ni ni-edit-fill"></em><span>Sửa bài</span>
                                                                    </asp:HyperLink>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="cmdSuaNgayXuatBan" OnClick="cmdSuaNgayXuatBan" CommandName="cmdSuaNgayXuatBan" Visible='<%# Ultis.NewsDaXuatBanCanEditNgayXuatBan(UserId)  %>' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "newid") %>' ToolTip="Sửa ngày xuất bản" runat="server">
                                                                        <em class="icon ni ni-calender-date"></em><span>Sửa ngày xuất bản</span>
                                                                    </asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                    </div>
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
                        </div>
                        <!-- .nk-block-between -->
                    </div>
                    <!-- .card-inner -->
                </div>
                <!-- .card-inner-group -->
            </div>
            <!-- .card -->
        </div>
        <%--Đoạn nay xử ly xem lịch sử bài viết--%>
        <div class="modal fade" tabindex="-1" id="modal-history">
            <div class="modal-dialog modal-xl modal-dialog-top" role="document">
                <div class="modal-content">
                    <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                        <em class="icon ni ni-cross"></em>
                    </a>
                    <div class="modal-header">
                        <h5 class="modal-title">Quá trình xử lý tin bài</h5>
                    </div>
                    <div class="modal-body">
                        <p>
                            <asp:Label ID="lblhNewsTitle" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                        </p>
                        <p>
                            Tác giả:
                        <asp:Label ID="lblhAuthor" runat="server" ForeColor="Maroon" Font-Bold="true" Font-Italic="true"></asp:Label>
                        </p>
                        <asp:DataGrid ID="drgDataViewerHistory" DataKeyField="ID" Width="100%" runat="server" AllowPaging="false"
                            AutoGenerateColumns="False" CssClass="table">
                            <ItemStyle CssClass="TRgrid" Font-Size="13px"></ItemStyle>
                            <HeaderStyle Font-Bold="true" ForeColor="White" BackColor="#006699" Font-Size="12px"></HeaderStyle>
                            <FooterStyle ForeColor="#000066" BackColor="White"></FooterStyle>
                            <Columns>
                                <asp:TemplateColumn HeaderText="Thời gian">
                                    <ItemTemplate>
                                        <asp:Label ID="lbCreatedDate" runat="server" Text='<%# BL.FormatDate(Eval("CreateDate")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="150" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Thông tin xử lý">
                                    <ItemTemplate>
                                        <asp:Label ID="lbByUser" runat="server" ForeColor="Maroon" Text='<%# GetUserName(DataBinder.Eval(Container.DataItem, "ByUser")) %>'></asp:Label>: 
                                        <asp:Label ID="lbProcessName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ProcessName") %>'></asp:Label>
                                        <br />
                                    </ItemTemplate>
                                    <ItemStyle Width="450px" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Bút phê" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblButPhe" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Comment") %>'></asp:Label>
                                        <br />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Phiên bản" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="Hyperlink2" ToolTip="Xem phiên bản" Target="_blank" Visible='<%# FormatVisible(DataBinder.Eval(Container.DataItem, "VersionId")) %>' NavigateUrl='<%# navigateurl() & "?view=version&ItemID=" & DataBinder.Eval(Container.DataItem, "VersionId") %>' runat="server">
                                            <em class="icon ni ni-files-fill"></em>
                                        </asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle Width="50" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderText="Phục hồi" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="cmdRestore" CommandName="cmdRestore" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "VersionId") %>' ToolTip="Phục hồi theo phiên bản này?" ImageUrl="/images/restore.gif" runat="server" Visible='<%# FormatVisible(DataBinder.Eval(Container.DataItem, "VersionId")) %>' />
                                    </ItemTemplate>
                                    <ItemStyle Width="40" />
                                </asp:TemplateColumn>
                            </Columns>
                        </asp:DataGrid>
                    </div>
                </div>
            </div>
        </div>
        <%--Đoạn nay xử ly xem lịch sử bài viết--%>
        <div class="modal fade" tabindex="-1" id="modal-newsnote">
            <div class="modal-dialog modal-xl modal-dialog-top" role="document">
                <div class="modal-content">
                    <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                        <em class="icon ni ni-cross"></em>
                    </a>
                    <div class="modal-header">
                        <h5 class="modal-title">Thông tin trao đổi bài viết</h5>
                    </div>
                    <div class="modal-body">
                        <ul class="newsnote">
                            <asp:Repeater ID="rptNotes" runat="server">
                                <ItemTemplate>
                                    <li>
                                        <b><%#BL.GetButDanh(PortalId, Eval("UserId")) %></b> <small><%#BL.FormatDate(Eval("CreatedDate")) %></small>: <%#Eval("NoiDung") %>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <%--===================================================--%>
        <%--Đoạn nay xử ly sửa ngày xuất bản--%>
        <div class="modal fade" tabindex="-1" id="modal-editngayxuatban">
            <div class="modal-dialog modal-xl modal-dialog-top" role="document">
                <div class="modal-content">
                    <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                        <em class="icon ni ni-cross"></em>
                    </a>
                    <div class="modal-header">
                        <h5 class="modal-title">Sửa thông tin ngày xuất bản tin bài</h5>
                    </div>
                    <div class="modal-body">
                        <p>
                            Tin bài :<asp:Label ID="lbNews" runat="server" ForeColor="Maroon" Font-Bold="true"></asp:Label>
                        </p>
                        <p>
                            Người đăng:
                            <asp:Label ID="lbUserCreated" runat="server" ForeColor="Maroon" Text="00" Font-Italic="true"></asp:Label>
                            -
                            <asp:Label ID="lblCreatedDate" runat="server" ForeColor="Maroon" Text="00" Font-Italic="true"></asp:Label>
                        </p>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Ngày</label>
                                    <asp:TextBox ID="txtNgayXL" runat="server" Width="150px" CssClass="datepicker form-control"></asp:TextBox>
                                </div>
                                <!-- /.form-group -->

                                <!-- /.form-group -->
                            </div>
                            <!-- /.col -->
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Giờ</label>
                                    <asp:DropDownList ID="txtGioXL" runat="server" Width="75px" CssClass="form-control">
                                        <asp:ListItem Value="0" Text="0"></asp:ListItem>
                                        <asp:ListItem Value="1" Text="1"></asp:ListItem>
                                        <asp:ListItem Value="2" Text="2"></asp:ListItem>
                                        <asp:ListItem Value="3" Text="3"></asp:ListItem>
                                        <asp:ListItem Value="4" Text="4"></asp:ListItem>
                                        <asp:ListItem Value="5" Text="5"></asp:ListItem>
                                        <asp:ListItem Value="6" Text="6"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="7"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="8"></asp:ListItem>
                                        <asp:ListItem Value="9" Text="9"></asp:ListItem>
                                        <asp:ListItem Value="10" Text="10"></asp:ListItem>
                                        <asp:ListItem Value="11" Text="11"></asp:ListItem>
                                        <asp:ListItem Value="12" Text="12"></asp:ListItem>
                                        <asp:ListItem Value="13" Text="13"></asp:ListItem>
                                        <asp:ListItem Value="14" Text="14"></asp:ListItem>
                                        <asp:ListItem Value="15" Text="15"></asp:ListItem>
                                        <asp:ListItem Value="16" Text="16"></asp:ListItem>
                                        <asp:ListItem Value="17" Text="17"></asp:ListItem>
                                        <asp:ListItem Value="18" Text="18"></asp:ListItem>
                                        <asp:ListItem Value="19" Text="19"></asp:ListItem>
                                        <asp:ListItem Value="20" Text="20"></asp:ListItem>
                                        <asp:ListItem Value="21" Text="21"></asp:ListItem>
                                        <asp:ListItem Value="22" Text="22"></asp:ListItem>
                                        <asp:ListItem Value="23" Text="23"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <!-- /.form-group -->
                                <div class="form-group">
                                    <label>Phút</label>
                                    <asp:TextBox ID="txtPhutXL" runat="server" Width="60px" CssClass="PhutXL form-control">0</asp:TextBox>
                                    <asp:RangeValidator ID="ranvDependents" runat="server" Display="static" ControlToValidate="txtPhutXL"
                                        ErrorMessage="Phút từ 0 -> 59" Type="Integer" MinimumValue="0" MaximumValue="59" CssClass="NormalRed">
                                    </asp:RangeValidator>
                                </div>
                                <!-- /.form-group -->
                            </div>
                        </div>
                        <asp:LinkButton ID="lbtXBSave" CssClass="btn btn-success pull-left" ToolTip="OK" runat="server">Cập nhật</asp:LinkButton>
                        <asp:LinkButton ID="lbtXBCancel" CssClass="btn btn-default pull-left" ToolTip="Hủy bỏ" runat="server" data-dismiss="modal">Huỷ bỏ</asp:LinkButton>
                        <asp:HiddenField ID="hdf_idkhachhang" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <%--===================================================--%>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="lbtFind" />
        <asp:AsyncPostBackTrigger ControlID="ddlPageSize" />
        <asp:AsyncPostBackTrigger ControlID="ddlCategory" />
    </Triggers>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="PageUpdateProgress">
    <ProgressTemplate>
        <div id="loading">
            <div class="loading">
                <div></div>
                <div></div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
