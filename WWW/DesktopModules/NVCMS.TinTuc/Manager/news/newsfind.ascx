<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="newsfind.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.news.newsfind" %>
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
                <div class="nk-block-head-content">
                    <div class="toggle-wrap nk-block-tools-toggle">
                        <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-menu-alt-r"></em></a>
                        <div class="toggle-expand-content" data-content="pageMenu">
                            <ul class="nk-block-tools g-3">
                                <li><a href="/quan-tri/tin-tuc/them-moi" class="btn btn-white btn-outline-light"><em class="icon ni ni-download-cloud"></em><span>Thêm mới tin</span></a></li>
                                <li class="nk-block-tools-opt">
                                    <div class="drodown">
                                        <a href="#" class="dropdown-toggle btn btn-icon btn-primary" data-toggle="dropdown" aria-expanded="false"><em class="icon ni ni-plus"></em></a>
                                        <div class="dropdown-menu dropdown-menu-right" style="">
                                            <ul class="link-list-opt no-bdr">
                                                <li><a href="/quan-tri/tin-tuc/them-moi"><em class="icon ni ni-file-docs"></em><span>Thêm tin mới</span></a></li>
                                                <li><a href="#"><em class="icon ni ni-video"></em><span>Thêm Tin Ảnh</span></a></li>
                                                <li><a href="#"><em class="icon ni ni-camera"></em><span>Thêm Tin Video</span></a></li>
                                            </ul>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <!-- .toggle-wrap -->
                </div>
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
                                <%--<div class="form-inline flex-nowrap gx-3">
                            <div class="form-wrap w-150px">
                                <select class="form-select form-select-sm select2-hidden-accessible" data-search="off" data-placeholder="Bulk Action" data-select2-id="1" tabindex="-1" aria-hidden="true">
                                    <option value="" data-select2-id="3">Bulk Action</option>
                                    <option value="email">Send Email</option>
                                    <option value="group">Change Group</option>
                                    <option value="suspend">Suspend User</option>
                                    <option value="delete">Delete User</option>
                                </select>
                            </div>
                            <div class="btn-wrap">
                                <span class="d-none d-md-block">
                                    <button class="btn btn-dim btn-outline-light disabled">Apply</button></span>
                                <span class="d-md-none">
                                    <button class="btn btn-dim btn-outline-light btn-icon disabled"><em class="icon ni ni-arrow-right"></em></button>
                                </span>
                            </div>
                        </div>--%>
                                <!-- .form-inline -->
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
                    <div class="card-inner p-0">
                        Chọn 
                        <asp:CheckBox ID="CheckBox1" runat="server" Checked="true" Enabled="false" />
                        để 
                        
                        <asp:LinkButton ID="lbtDelete" Visible="false" Font-Bold="true" CssClass="btn btn-dark btn-dim btn-outline-light" OnClientClick="javascript:return confirm('Trả lại tin bài đã chọn?');" runat="server">
                            <em class="icon ni ni-property-remove"></em><span>Xóa sạch khỏi CSDL</span>
                        </asp:LinkButton>

                    </div>
                    <!-- .card-inner -->
                    <div class="card-inner p-0">
                        <div class="nk-tb-list nk-tb-ulist" id="datatabledangsoanthao">
                            <div class="nk-tb-item nk-tb-head">
                                <div class="nk-tb-col nk-tb-col-check">
                                    <div class="custom-control custom-control-sm custom-checkbox notext" id="chonxoaz" runat="server" visible="<%# ShowPageDangSoanThao()%>">
                                        <asp:CheckBox ID="chkHeader" runat="server" CssClass="" />
                                    </div>
                                </div>
                                <div class="nk-tb-col nk-tb-col-check">
                                    <div class="custom-control custom-control-sm custom-checkbox notext">
                                        #
                                    </div>
                                </div>
                                <div class="nk-tb-col"><span class="sub-text">Thông tin</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">Danh mục</span></div>
                                <div class="nk-tb-col tb-col-md" id="chiaser" runat="server" visible="false"><span class="sub-text">Chia sẻ</span></div>
                                <div class="nk-tb-col tb-col-lg" id="viewcount" runat="server" visible="false"><span class="sub-text">View</span></div>
                                <div class="nk-tb-col tb-col-lg" id="nhuanbut" runat="server" visible="false"><span class="sub-text">Nhuận bút</span></div>
                                <div class="nk-tb-col nk-tb-col-tools">
                                    <ul class="nk-tb-actions gx-1 my-n1">
                                        <li>
                                            <div class="drodown">
                                                <a href="#" class="btn btn-icon btn-trigger mr-n1"><em class="icon ni ni-more-h"></em></a>
                                                <%--<div class="dropdown-menu dropdown-menu-right">
                                                    <ul class="link-list-opt no-bdr">
                                                        <li><a href="#"><em class="icon ni ni-mail"></em><span>Send Email to All</span></a></li>
                                                        <li><a href="#"><em class="icon ni ni-na"></em><span>Suspend Selected</span></a></li>
                                                        <li><a href="#"><em class="icon ni ni-trash"></em><span>Remove Seleted</span></a></li>
                                                        <li><a href="#"><em class="icon ni ni-shield-star"></em><span>Reset Password</span></a></li>
                                                    </ul>
                                                </div>--%>
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
                                            <div class="custom-control custom-control-sm custom-checkbox notext" id="chonxoa" runat="server" visible="<%# ShowPageDangSoanThao()%>">
                                                <asp:CheckBox ID="chkRow" ClientIDMode="Static" runat="server" CssClass="" />
                                            </div>
                                        </div>
                                        <div class="nk-tb-col nk-tb-col-check">
                                            <div class="custom-control custom-control-sm custom-checkbox notext">
                                                <%#Eval("NewId") %>
                                            </div>
                                        </div>
                                        <div class="nk-tb-col">
                                            <div class="user-card">
                                                <div class="news-avatar xs bg-primary">
                                                    <a href="<%# Ultis.FormatFullImage(CStr(Eval("imagepath"))) %>" data-fancybox data-caption="" class="usernewsimage" data-toggle="tooltip" data-placement="top" title="Xem ảnh lớn">
                                                        <asp:Image ImageUrl='<%# Ultis.FormatThumbImage(CStr(Eval("imagepath")), 120, 70, "", "", "", "") %>' AlternateText="" ID="imgNews" runat="server" />
                                                    </a>
                                                </div>
                                                <div class="user-info">
                                                    <spans class="badge badge-light"><%# BL.FormatLoaiTinBaiHTML(CInt(Eval("NewsKind")))%></spans>
                                                    <img src='<%# Eval("TypeUrl") %>' border="0" alt="" style="vertical-align: bottom;" />
                                                    <span class="d-sm-inline tb-lead"><%# Highlight(CStr(Eval("Title")), "<span class='highlight'>", "</span>")%></span>
                                                    <span id="tinnng" runat="server" visible='<%#IIf(CBool(Eval("HotCat")), "True", "False") %>'><em class="icon ni ni-hot text-danger"></em></span>
                                                    <span id="tinanh" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                                    <span id="video" runat="server" visible='<%#IIf(CBool(Eval("IsImage")), "True", "False") %>'><em class="icon ni ni-camera"></em></span>
                                                    <br />
                                                    <small>Ngày tạo: <%# DataBinder.Eval(Container.DataItem, "CreateDate") %>
                                                        <br />
                                                    </small>
                                                    <small>
                                                        <asp:Label ForeColor="Orange" Font-Size="11px" ID="lblEdittedBy" runat="server" Text='<%# Ultis.FormatEdittedBy(PortalId, CInt(Eval("newid"))) %>' Visible='<%#CBool(Eval("IsEdited")) %>'></asp:Label>&nbsp; 
                                                        <asp:LinkButton ID="cmdUnlock" CssClass="btn btn-trigger" ForeColor="Orange" OnClick="GetUnlockNews" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "newid") %>' Visible='<%# Ultis.FormatLockByUserDangoanThao(PortalId, CInt(Eval("NewId")), UserId) %>' runat="server" data-toggle="tooltip" data-placement="top" title="" data-original-title="Mở khóa tin bài">
                                                            <em class="icon ni ni-unlock"></em>
                                                        </asp:LinkButton>
                                                    </small>
                                                    <small id="baidataxuatbanr" runat="server" visible="<%# ShowPageDaXuatBan()%>">
                                                        <font style="color: Maroon;">Tác giả:</font>
                                                        <asp:Label ID="lblCreatedInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetCreatedInfo(PortalId, CInt(Eval("UserId")), CDate(Eval("Createdate"))) %>'></asp:Label>
                                                        | 
                <font style="color: Maroon;">Duyệt:</font>
                                                        <asp:Label ID="lblApprovalInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetApprovalInfo(PortalId, CInt(Eval("ApprovalUser")), CDate(Eval("ApprovalDate"))) %>'></asp:Label>
                                                        | 
                <font style="color: Maroon;">Xuất bản:</font>
                                                        <asp:Label ID="lblPublishInfo" ForeColor="Maroon" runat="server" Text='<%# BL.GetPublishedInfo(PortalId, CInt(Eval("PublishedUser")), CDate(Eval("PublishedDate"))) %>'></asp:Label>
                                                    </small>
                                                    <small class="text-danger bg-pink-dim">
                                                        <asp:Literal ID="ltrNotes" runat="server" Text='<%# Ultis.NewsNotes(PortalId, CInt(DataBinder.Eval(Container.DataItem, "newid"))) %>'></asp:Literal>
                                                        <asp:LinkButton ID="lbtShowAllNotes" OnClick="cmdShowAllNote" CommandName="cmdShowAllNote" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "newid") %>' Visible='<%# Ultis.NewsNotesShow(PortalId, Eval("NewId")) %>' ToolTip="Xem tất cả lời nhắn" runat="server">
                                                            <em class="icon ni ni-chat-fill"></em><span>xem tất cả</span>
                                                        </asp:LinkButton>
                                                    </small>
                                                    <br />
                                                    <small>
                                                        <asp:Label ForeColor="Orange" Font-Size="11px" ID="Label1" runat="server" Text='<%# Ultis.FormatReturndBy(PortalId, CInt(DataBinder.Eval(Container.DataItem, "newid"))) %>' Visible='<%# Ultis.UButtonBiTraLai(CInt(Eval("NewId"))) %>'></asp:Label>&nbsp; 
                                                    </small>
                                                    <asp:Label ID="lblnewid" runat="server" Text='<%# Eval("newid") %>' Visible="false" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="nk-tb-col tb-col-mb">
                                            <span class=""><%# Eval("CategoryName")%> </span>
                                        </div>
                                        <div class="nk-tb-col tb-col-md" id="chiaser2" runat="server" visible="<%# ShowPageDaXuatBan()%>">
                                            <span class="badge badge-danger"><a class="text-white" href="<%#NavigateURL() & "?view=share&itemid=" & CInt(Eval("NewId")) %>" data-toggle="tooltip" data-placement="top" data-original-title="Click vào đây để cập nhật link đã share">
                                                <%# Ultis.CountLinkShare(CInt(Eval("Newid")))%></a></span>
                                        </div>
                                        <div class="nk-tb-col tb-col-lg" id="viewcount2" runat="server" visible="<%# ShowPageDaXuatBan()%>">
                                            <span class="badge badge-warning"><%# Eval("ViewCount")%></span>
                                        </div>
                                        <div class="nk-tb-col tb-col-lg" id="nhuanbut2" runat="server" visible="<%# ShowPageDaXuatBan()%>">
                                            <span class="curency auto"><%# Ultis.GetTienNhuanBut(CInt(Eval("NewId")))%></span>
                                        </div>

                                        <div class="nk-tb-col nk-tb-col-tools">
                                            <ul class="nk-tb-actions gx-1">
                                                <li class="nk-tb-action-hidden" id="nutsua" runat="server" visible='<%#Ultis.UButtonEdit(CInt(Eval("NewId"))) %>'>
                                                    <a href="/quan-tri/tin-tuc/them-moi?itemid=<%#Eval("NewId") %>" class="btn btn-trigger btn-icon user-avatar" data-toggle="tooltip" data-placement="top" title="" data-original-title="Sửa">
                                                        <em class="icon ni ni-edit-fill"></em>
                                                    </a>
                                                </li>
                                                <li class="nk-tb-action-hidden" id="nuttrieuhoi" runat="server" visible='<%#Ultis.UButtonTrieuHoi(CInt(Eval("NewId"))) %>'>
                                                    <asp:LinkButton ID="cmdCancelRequest" CssClass="btn btn-trigger btn-icon user-avatar" OnClick="cmdCancelRequest" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "newid") %>' Visible='<%# Ultis.FormatTrieuHoi(PortalId, CInt(Eval("NewId")), CInt(Eval("Status")), CInt(Eval("UserId")), CInt(Eval("ApprovalUser"))) %>' runat="server" data-toggle="tooltip" data-placement="top" title="" data-original-title="Thu hồi bài viết"><em class="icon ni ni-curve-up-left"></em></asp:LinkButton>
                                                </li>
                                                <li>
                                                    <div class="drodown">
                                                        <a href="#" class="dropdown-toggle btn btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                        <div class="dropdown-menu dropdown-menu-right">
                                                            <ul class="link-list-opt no-bdr">
                                                                <li><a target="_blank" href='<%#Ultis.FormatLinkadminXemtruoc("/news", CInt(Eval("NewId")), CStr(Eval("Title"))) %>'><em class="icon ni ni-eye"></em><span>Xem trước</span></a></li>
                                                                <%--<li><a href="#"><em class="icon ni ni-repeat"></em><span>Orders</span></a></li>
                                                                <li><a href="#"><em class="icon ni ni-activity-round"></em><span>Activities</span></a></li>
                                                                <li class="divider"></li>
                                                                <li><a href="#"><em class="icon ni ni-shield-star"></em><span>Reset Pass</span></a></li>
                                                                <li><a href="#"><em class="icon ni ni-na"></em><span>Suspend</span></a></li>--%>
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
                                    <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" PageLinksPerPage="20" />
                                </ul>
                                <!-- .pagination -->
                            </div>
                            <div class="g">
                                <div class="nk-block-head-content">
                                    <div class="toggle-wrap nk-block-tools-toggle">
                                        <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-more-v"></em></a>
                                        <div class="toggle-expand-content" data-content="pageMenu">
                                            <ul class="nk-block-tools g-3">
                                                <li class="nk-block-tools-opt"><a href="/quan-tri/tin-tuc/them-moi" class="btn btn-primary"><em class="icon ni ni-save"></em><span>Thêm mới tin</span></a></li>
                                                <li>
                                                    <div class="drodown">
                                                        <a href="#" class="dropdown-toggle btn btn-primary btn-dim btn-outline-light" data-toggle="dropdown"><em class="icon ni ni-list-thumb-alt"></em><span>Tác vụ</span><em class="dd-indc icon ni ni-chevron-right"></em></a>
                                                        <div class="dropdown-menu dropdown-menu-right">
                                                            <ul class="link-list-opt no-bdr">
                                                                <li><a href="/quan-tri/tin-tuc/them-moi"><em class="icon ni ni-file-docs"></em><span>Thêm tin mới</span></a></li>
                                                                <li><a href="#"><em class="icon ni ni-video"></em><span>Thêm Video</span></a></li>
                                                                <li><a href="#"><em class="icon ni ni-camera"></em><span>Thêm Video</span></a></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </li>

                                            </ul>
                                        </div>
                                    </div>
                                </div>
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
        <%--Đoạn nay xử ly xem Lời nhắn--%>
        <div class="modal fade" tabindex="-1" id="modal-newsnote">
            <div class="modal-dialog modal-xl modal-dialog-top" role="document">
                <div class="modal-content">
                    <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                        <em class="icon ni ni-cross"></em>
                    </a>
                    <div class="modal-header">
                        <h5 class="modal-title">Thông tin lời nhắn</h5>
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
<script type="text/javascript">
    $(function () {
        $("#datatabledangsoanthao [id*=chkHeader]").click(function () {
            if ($(this).is(":checked")) {
                $("#datatabledangsoanthao [id*=chkRow]").attr("checked", "checked");
            } else {
                $("#datatabledangsoanthao [id*=chkRow]").removeAttr("checked");
            }
        });
        $("#datatabledangsoanthao [id*=chkRow]").click(function () {
            if ($("#datatabledangsoanthao [id*=chkRow]").length == $("#datatabledangsoanthao [id*=chkRow]:checked").length) {
                $("#datatabledangsoanthao [id*=chkHeader]").attr("checked", "checked");
            } else {
                $("#datatabledangsoanthao [id*=chkHeader]").removeAttr("checked");
            }
        });
    });
</script>
