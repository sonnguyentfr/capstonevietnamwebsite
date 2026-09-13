<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="ListSdt.ascx.vb" Inherits="NVCMS.Modules.Marketing.ZaloListSdtViewer" %>


<!-- Header -->
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title">
                <a href="<%=BackUrl%>" class="btn btn-icon btn-sm btn-outline-light mr-2" title="Quay lại">
                    <em class="icon ni ni-arrow-left"></em>
                </a>
                Danh sách SĐT &mdash; <b>
                    <asp:Literal ID="ltrCampaignTitle" runat="server"></asp:Literal></b>
            </h3>
        </div>
    </div>
</div>

<asp:UpdatePanel runat="server" ID="upnlMain">
    <ContentTemplate>
        <div class="row g-gs">

            <!-- LEFT: Import panel -->
            <div class="col-md-7">
                <div class="card card-preview" id="cardImportPanel" style="position: relative;">

                    <!-- Loading overlay khi THÊM VÀO DANH SÁCH -->
                    <div id="importLoadingOverlay" style="display: none; position: absolute; top: 0; left: 0; width: 100%; height: 100%; background: rgba(255,255,255,0.88); z-index: 10; border-radius: 4px;">
                        <div style="position: absolute; top: 50%; left: 50%; transform: translate(-50%,-50%); text-align: center;">
                            <div class="spinner-border text-success" role="status" style="width: 2.2rem; height: 2.2rem;">
                                <span class="sr-only">Loading...</span>
                            </div>
                            <p class="mt-2 text-success small mb-0"><b>Đang thêm SĐT vào chiến dịch...</b></p>
                        </div>
                    </div>

                    <div class="card-inner">
                        <ul class="nav nav-tabs mt-n3">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#tabEvent">
                                    <em class="icon ni ni-calendar"></em><span>Lọc theo sự kiện</span>
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-toggle="tab" href="#tabManual">
                                    <em class="icon ni ni-edit"></em><span>Nhập thủ công</span>
                                </a>
                            </li>
                        </ul>

                        <div class="tab-content mt-2">

                            <!-- TAB 1: Lọc theo sự kiện -->
                            <div class="tab-pane active" id="tabEvent">
                                <div class="form-group">
                                    <label class="form-label"><b>TÊN SỰ KIỆN:</b></label>
                                    <asp:DropDownList ID="ddlEventCat" runat="server"
                                        CssClass="form-select form-control"
                                        data-search="on"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlEventCat_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="form-label"><b>ĐỊA ĐIỂM (chính xác):</b></label>
                                    <asp:DropDownList ID="ddlEvent" runat="server"
                                        CssClass="form-select form-control"
                                        data-search="on">
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label class="form-label"><b>TRẠNG THÁI CHECK-IN:</b></label>
                                    <asp:DropDownList ID="ddlCheckin" runat="server" CssClass="form-select form-control">
                                    </asp:DropDownList>
                                </div>
                                <asp:LinkButton ID="lbtXemDanhSach" runat="server"
                                    Font-Bold="True"
                                    CssClass="btn btn-outline-primary mr-1"
                                    OnClientClick="return ValidateAndShowLoading();">
                                                    <em class="icon ni ni-eye"></em> XEM DANH SÁCH
                                </asp:LinkButton>
                                <asp:LinkButton ID="lbtImportFromEvent" runat="server"
                                    Font-Bold="True"
                                    CssClass="btn btn-primary"
                                    Visible="false"
                                    OnClientClick="return confirmAndShowImportLoading();">
                                                    <em class="icon ni ni-upload"></em> THÊM VÀO DANH SÁCH
                                </asp:LinkButton>
                            </div>

                            <!-- TAB 2: Nhập thủ công -->
                            <div class="tab-pane" id="tabManual">
                                <p class="text-soft mb-1">Nhập danh sách SĐT, mỗi số một dòng hoặc cách nhau bằng dấu <mark>;</mark></p>
                                <p class="text-info small mb-2">
                                    Hệ thống tự động chuẩn hóa:
                                                    <b>09x→849x</b>, <b>03x→843x</b>, <b>07x→847x</b>, <b>08x→848x</b>
                                </p>
                                <asp:TextBox ID="txtSdt" runat="server"
                                    TextMode="MultiLine"
                                    Height="250px"
                                    CssClass="form-control no-resize"
                                    placeholder="Ví dụ:&#13;&#10;0901234567&#13;&#10;0912345678&#13;&#10;+84987654321">
                                </asp:TextBox>
                                <div class="mt-2">
                                    <asp:LinkButton ID="lbtImportManual" runat="server"
                                        Font-Bold="True"
                                        CssClass="btn btn-primary"
                                        OnClientClick="return ValidateManual();">
                                                        <em class="icon ni ni-upload"></em> CẬP NHẬT VÀO DANH SÁCH
                                    </asp:LinkButton>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>

            <!-- RIGHT: Preview kết quả import -->
            <div class="col-md-5">
                <div class="card card-bordered h-100" style="position: relative;">

                    <!-- Loading overlay cho panel preview -->
                    <div id="previewLoadingOverlay" style="display: none; position: absolute; top: 0; left: 0; width: 100%; height: 100%; background: rgba(255,255,255,0.85); z-index: 10; border-radius: 4px;">
                        <div style="position: absolute; top: 50%; left: 50%; transform: translate(-50%,-50%); text-align: center;">
                            <div class="spinner-border text-primary" role="status" style="width: 2rem; height: 2rem;">
                                <span class="sr-only">Loading...</span>
                            </div>
                            <p class="mt-2 text-primary small mb-0"><b>Đang lấy dữ liệu...</b></p>
                        </div>
                    </div>

                    <div class="card-header border-bottom">
                        <div>
                            <b>Kết quả kiểm tra SĐT</b>
                        </div>
                        <div class="mt-1">
                            <span class="badge badge-success mr-1">Hợp lệ: <b>
                                <asp:Literal ID="ltrOK" runat="server">0</asp:Literal></b>
                            </span>
                            <span class="badge badge-danger mr-1">Lỗi: <b>
                                <asp:Literal ID="ltrLoi" runat="server">0</asp:Literal></b>
                            </span>
                            <span class="badge badge-warning">Trùng: <b>
                                <asp:Literal ID="ltrTrung" runat="server">0</asp:Literal></b>
                            </span>
                        </div>
                    </div>
                    <div class="card-inner p-0">
                        <div data-simplebar style="max-height: 400px; overflow-y: auto;">
                            <div class="nk-tb-list is-compact">
                                <div class="nk-tb-item nk-tb-head">
                                    <div class="nk-tb-col" style="width: 40%"><span>Họ tên</span></div>
                                    <div class="nk-tb-col" style="width: 35%"><span>SĐT</span></div>
                                    <div class="nk-tb-col text-right"><span>Trạng thái</span></div>
                                </div>
                                <!-- Từ sự kiện: SĐT sinh viên -->
                                <asp:Repeater ID="rptPreviewEvent" runat="server">
                                    <ItemTemplate>
                                        <div class="nk-tb-item" style="<%# IIf(Eval("SdtStatus") = "LỖI", "background:#fff3cd;", IIf(Eval("SdtStatus") = "TRÙNG", "background:#e2e3e5;", "")) %>">
                                            <div class="nk-tb-col">
                                                <span class="tb-sub small"><%# Eval("StudentFullname") %></span>
                                            </div>
                                            <div class="nk-tb-col">
                                                <span class="tb-sub small"><b><%# Eval("SdtNormalized") %></b></span>
                                                <span class="text-muted" style="font-size: 11px">(<%# Eval("SdtRaw") %>)</span>
                                            </div>
                                            <div class="nk-tb-col text-right">
                                                <span class="badge <%# IIf(Eval("SdtStatus") = "OK", "badge-success", IIf(Eval("SdtStatus") = "TRÙNG", "badge-warning", "badge-danger")) %>">
                                                    <%# Eval("SdtStatus") %>
                                                </span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <!-- Từ nhập tay: chỉ có SĐT -->
                                <asp:Repeater ID="rptPreviewManual" runat="server">
                                    <ItemTemplate>
                                        <div class="nk-tb-item" style="<%# IIf(Eval("SdtStatus") = "LỖI", "background:#fff3cd;", IIf(Eval("SdtStatus") = "TRÙNG", "background:#e2e3e5;", "")) %>">
                                            <div class="nk-tb-col">
                                                <span class="text-muted small">—</span>
                                            </div>
                                            <div class="nk-tb-col">
                                                <span class="tb-sub small"><b><%# Eval("SdtNormalized") %></b></span>
                                                <span class="text-muted" style="font-size: 11px">(<%# Eval("SdtRaw") %>)</span>
                                            </div>
                                            <div class="nk-tb-col text-right">
                                                <span class="badge <%# IIf(Eval("SdtStatus") = "OK", "badge-success", IIf(Eval("SdtStatus") = "TRÙNG", "badge-warning", "badge-danger")) %>">
                                                    <%# Eval("SdtStatus") %>
                                                </span>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- BOTTOM: Danh sách SĐT đã lưu trong campaign -->
            <div class="col-md-12">
                <div class="card card-bordered">
                    <div class="card-inner">
                        <div class="card-title-group align-start pb-2">
                            <div class="card-title">
                                <h5>DANH SÁCH SĐT TRONG CHIẾN DỊCH</h5>
                                <p class="text-soft">
                                    Tổng số: <b>
                                        <asp:Literal ID="ltrTotal" runat="server">0</asp:Literal></b> số điện thoại
                                </p>
                            </div>
                            <div class="card-tools">
                                <asp:LinkButton ID="lbtDeleteAll" runat="server"
                                    CssClass="btn btn-sm btn-outline-danger"
                                    OnClientClick="return confirm('Xóa TẤT CẢ số điện thoại trong chiến dịch này?');">
                                                    <em class="icon ni ni-trash"></em> Xóa hết
                                </asp:LinkButton>
                            </div>
                        </div>

                        <!-- Filter bar -->
                        <div class="row g-2 mb-3 align-items-end">
                            <div class="col-md-4">
                                <label class="form-label form-label-sm">Tìm SĐT</label>
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Tìm số điện thoại..."></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label form-label-sm">Trạng thái</label>
                                <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="form-control form-control-sm">
                                    <asp:ListItem Value="-1">-- Tất cả --</asp:ListItem>
                                    <asp:ListItem Value="0">Chờ gửi</asp:ListItem>
                                    <asp:ListItem Value="1">Đã gửi</asp:ListItem>
                                    <asp:ListItem Value="2">Lỗi</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-auto">
                                <asp:LinkButton ID="lbtSearch" runat="server" CssClass="btn btn-sm btn-primary">
                                                    <em class="icon ni ni-search"></em> Tìm
                                </asp:LinkButton>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <table class="table table-sm table-hover">
                                <thead class="table-light">
                                    <tr>
                                        <th style="width: 45px">#</th>
                                        <th>Họ và tên</th>
                                        <th>SĐT gốc</th>
                                        <th>SĐT chuẩn hóa (84...)</th>
                                        <th>Trạng thái</th>
                                        <th>Số lần gửi</th>
                                        <th>Ngày thêm</th>
                                        <th style="width: 70px"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptListSdt" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td><%# Container.ItemIndex + 1 %></td>
                                                <td class="text-muted small"><%# Eval("FullName") %></td>
                                                <td class="text-muted small"><%# Eval("PhoneRaw") %></td>
                                                <td><b><%# Eval("Phone") %></b></td>
                                                <td>
                                                    <span class="badge <%# GetStatusBadge(CInt(Eval("Status"))) %>">
                                                        <%# GetStatusLabel(CInt(Eval("Status"))) %>
                                                    </span>
                                                </td>
                                                <td><%# Eval("SendCount") %></td>
                                                <td class="small"><%# String.Format("{0:dd/MM/yyyy}", Eval("CreatedDate")) %></td>
                                                <td>
                                                    <asp:LinkButton ID="btnDeleteSdt"
                                                        CommandArgument='<%# Eval("Id") %>'
                                                        CommandName="DeleteSdt"
                                                        runat="server"
                                                        CssClass="btn btn-sm btn-icon btn-outline-danger"
                                                        OnClientClick="return confirm('Xóa số điện thoại này?');"
                                                        title="Xóa">
                                                                        <em class="icon ni ni-trash"></em>
                                                    </asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Literal ID="ltrEmpty" Visible='<%# rptListSdt.Items.Count = 0 %>' runat="server">
                                                                <tr><td colspan="7" class="text-center text-muted py-3">Chưa có số điện thoại nào.</td></tr>
                                            </asp:Literal>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!-- end row -->
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdateProgress runat="server" ID="upProgress" AssociatedUpdatePanelID="upnlMain" DisplayAfter="0">
    <ProgressTemplate>
        <%-- Trigger JS khi UpdatePanel bắt đầu xử lý --%>
    </ProgressTemplate>
</asp:UpdateProgress>



<script type="text/javascript">
    function ValidateEvent() {
        var ddl = document.getElementById('<%=ddlEventCat.ClientID%>').value;
        if (ddl == 0) {
            alert("Vui lòng chọn sự kiện!");
            return false;
        }
        return true;
    }

    function ValidateAndShowLoading() {
        if (!ValidateEvent()) return false;
        showPreviewLoading(true);
        return true;
    }

    function ValidateManual() {
        var txt = document.getElementById('<%=txtSdt.ClientID%>').value;
        if (txt.trim() == "") {
            alert("Vui lòng nhập danh sách SĐT!");
            return false;
        }
        return true;
    }

    function showPreviewLoading(show) {
        var overlay = document.getElementById('previewLoadingOverlay');
        if (overlay) overlay.style.display = show ? 'block' : 'none';
    }

    function showImportLoading(show) {
        var overlay = document.getElementById('importLoadingOverlay');
        if (overlay) overlay.style.display = show ? 'block' : 'none';
    }

    function confirmAndShowImportLoading() {
        if (!confirm('Thêm toàn bộ SĐT hợp lệ vào chiến dịch?')) return false;
        showImportLoading(true);
        return true;
    }

    // Ẩn loading khi UpdatePanel hoàn thành
    if (typeof Sys !== 'undefined') {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
            showPreviewLoading(false);
            showImportLoading(false);
        });
    }
</script>
