<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_view.ascx.vb" Inherits="DesktopModules.School.Manager.news.newsfind" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>

<style type="text/css">
    .chonportal {
        display: none;
    }

    .dropdown-menu-xl {
        min-width: 600px;
        max-width: 600px;
    }

    .nk-tb-col span.badge {
        margin-right: 4px;
    }
</style>

<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
            <div class="nk-block-des text-soft">
                <p>
                    Tổng số có: 
                        <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                    bản ghi.
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
                                                            <span class="sub-title dropdown-title">Tìm thông tin</span>
                                                            <div class="dropdown">
                                                                <a href="#" class="btn btn-sm btn-icon">
                                                                    <em class="icon ni ni-more-h"></em>
                                                                </a>
                                                            </div>
                                                        </div>
                                                        <div class="dropdown-body dropdown-body-rg">
                                                            <div class="row gx-6 gy-3">
                                                                <div class="col-6">
                                                                    <div class="form-group">
                                                                        <label class="overline-title overline-title-alt">Tên trường</label>
                                                                        <asp:TextBox ID="txttentruong" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-6">
                                                                    <div class="form-group">
                                                                        <label class="overline-title overline-title-alt">Website</label>
                                                                        <asp:TextBox ID="txtwebsite" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-12">
                                                                    <div class="form-group">
                                                                        <label class="overline-title overline-title-alt">Loại trường</label>
                                                                        <asp:DropDownList ID="ddlLoaitruong" runat="server" CssClass="form-select form-control form-control-sm"></asp:DropDownList>
                                                                    </div>
                                                                </div>

                                                                <div class="col-6">
                                                                    <div class="form-group">
                                                                        <label class="overline-title overline-title-alt">Quốc gia</label>
                                                                        <asp:DropDownList ID="ddlQuoicgia" runat="server" CssClass="form-select form-control form-control-sm"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-6">
                                                                    <div class="form-group">
                                                                        <label class="overline-title overline-title-alt">Bang (Tỉnh)</label>
                                                                        <asp:DropDownList ID="ddlQuoicgia_Bang" runat="server" CssClass="form-select form-control form-control-sm"></asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                                <div class="col-12">
                                                                    <div class="form-group">
                                                                        <asp:LinkButton ID="lbtFind" runat="server" Font-Bold="true" CssClass="btn btn-secondary" ToolTip="Tìm kiếm" Text="Tìm thông tin"></asp:LinkButton>
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
                            <input type="text" class="form-control " placeholder="Nhập tên trường" id="txtTitle2" runat="server">
                            <asp:Button ID="lbtFindTitle" runat="server" Font-Bold="true" CssClass="search-submit btn btn-primary" ToolTip="Tìm kiếm" Text="Tìm"></asp:Button>
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
                        <div class="nk-tb-col tb-col-mb"><span class="sub-text">Loại trường</span></div>
                        <div class="nk-tb-col tb-col-mb"><span class="sub-text">Đối tác</span></div>
                        <div class="nk-tb-col tb-col-mb"><span class="sub-text">Quốc gia / Bang</span></div>
                        <div class="nk-tb-col tb-col-mb"><span class="sub-text">Hiện thị</span></div>
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
                            <div class="nk-tb-item " style='<%# GetStatus(Eval("id"))%>'>
                                <div class="nk-tb-col nk-tb-col-check">
                                    <div class="custom-control custom-control-sm custom-checkbox notext">
                                        <%#Eval("id") %>
                                    </div>
                                </div>
                                <div class="nk-tb-col">
                                    <div class="user-card">
                                        <div class="news-avatar xs bg-primary">
                                            <a href="<%# Ultis.FormatFullImage(CStr(Eval("Logo"))) %>" data-fancybox data-caption="" class="usernewsimage" data-toggle="tooltip" data-placement="top" title="Xem ảnh lớn">
                                                <asp:Image ImageUrl='<%# Ultis.FormatThumbImage(CStr(Eval("Logo")), 120, 70, "", "", "", "") %>' AlternateText="" ID="imgNews" runat="server" />
                                            </a>
                                        </div>
                                        <div class="user-info">
                                            <span class="d-sm-inline tb-lead"><%# Highlight(CStr(Eval("NameofSchool")), "<span class='highlight'>", "</span>")%></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="nk-tb-col tb-col-mb">
                                    <span class=""><%# Eval("LoaiTruongTen")%> </span>
                                </div>
                                <div class="nk-tb-col tb-col-mb">
                                    <%# PartnershipStatus(Eval("PartnershipStatus"))%>
                                </div>
                                <div class="nk-tb-col tb-col-mb">
                                    <span class=""><%# Eval("CountryName")%> / <%# Eval("StateCityName")%></span>
                                </div>
                                <div class="nk-tb-col tb-col-mb">
                                    <span class=""><%#IIf(DataBinder.Eval(Container.DataItem, "status") = "True", "Show", "Ẩn") %> </span>
                                </div>
                                <div class="nk-tb-col nk-tb-col-tools">
                                    <ul class="nk-tb-actions gx-1">
                                        <li class="nk-tb-action-hidden" id="nutsua" runat="server">
                                            <asp:HyperLink ID="hplEditnews" runat="server" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & Eval("id") %>' CssClass="btn btn-trigger btn-icon user-avatar" data-toggle="tooltip" data-placement="top" title="" data-original-title="Sửa">
                                                <em class="icon ni ni-edit-fill"></em></asp:HyperLink>
                                        </li>
                                        <li>
                                            <div class="drodown">
                                                <a href="#" class="dropdown-toggle btn btn-icon btn-trigger" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                <div class="dropdown-menu dropdown-menu-right">
                                                    <ul class="link-list-opt no-bdr">
                                                        <li></li>
                                                        <li><a href="#"><em class="icon ni ni-repeat"></em><span>Orders</span></a></li>
                                                        <li><a href="#"><em class="icon ni ni-activity-round"></em><span>Activities</span></a></li>
                                                        <li class="divider"></li>
                                                        <li><a href="#"><em class="icon ni ni-shield-star"></em><span>Reset Pass</span></a></li>
                                                        <li><a href="#"><em class="icon ni ni-na"></em><span>Suspend</span></a></li>
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
                                                        <li><a href="/quan-tri/tin-tuc/them-moi-tin-anh"><em class="icon ni ni-camera"></em><span>Thêm tin ảnh</span></a></li>
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

<%--<asp:HiddenField ID="hdfQuocGia" runat="server" />--%>
<%--<script type="text/javascript">
    $(document).ready(function () {
        $('#<%=ddlQuoicgia.ClientID %>').change(function () {
            var id = $('#<%=ddlQuoicgia.ClientID %>').val();
            document.getElementById('<%=hdfQuocGia.ClientID %>').value = $('#<%=ddlQuoicgia.ClientID %>').val();
            if (id > 0) {
                $('#<%=ddlQuoicgia_Bang.ClientID %>').html("");
                $.getJSON("/Services/QuocGia.ashx?itemid=" + id, function (citys) {
                    $.each(citys, function () {
                        $('#<%=ddlQuoicgia_Bang.ClientID %>').append($("<option></option>").val(this['id']).html(this['Name']));
                    });
                });
            }
        });
    });
</script>--%>
