<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.Campaign" %>
<asp:UpdatePanel runat="server" ID="upnlAtt">
    <ContentTemplate>
        <div class="nk-content ">
            <div class="container-fluid">
                <div class="nk-content-inner">
                    <div class="nk-content-body">
                        <div class="nk-block-head nk-block-head-sm">
                            <div class="nk-block-between">
                                <div class="nk-block-head-content">
                                    <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
                                    <div class="nk-block-des text-soft">
                                        <p>
                                            Tổng số có: <b>
                                                <asp:Literal ID="ltrcount" runat="server"></asp:Literal></b> bản ghi
                                        </p>
                                    </div>
                                </div>
                                <!-- .nk-block-head-content -->
                                <div class="nk-block-head-content">
                                    <div class="toggle-wrap nk-block-tools-toggle">
                                        <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-menu-alt-r"></em></a>
                                        <div class="toggle-expand-content" data-content="pageMenu">
                                            <ul class="nk-block-tools g-3">

                                                <li class="nk-block-tools-opt">
                                                    <asp:LinkButton ID="lbtAdd" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary waves-effect waves-light"><span>Thêm mới</span></asp:LinkButton>
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
                        <!-- .nk-block-head -->
                        <div class="nk-block">
                            <div class="row g-gs">
                                <asp:Repeater ID="rptlistacc" runat="server">
                                    <ItemTemplate>
                                        <div class="col-sm-6 col-lg-4 col-xxl-3">
                                            <div class="card card-bordered h-100">
                                                <div class="card-inner">
                                                    <div class="project">
                                                        <div class="project-head">
                                                            <asp:LinkButton ID="LinkButton1" CommandArgument='<%#Eval("Id") %>' CommandName="GetInfo" OnClick="GetInfo" runat="server" title="Sửa thông tin" data-toggle="tooltip" data-placement="top" data-original-title="Sửa thông tin" CssClass="project-title">
                                                                <div class="project-info">
                                                                    <h6 class="title"><%# Eval("Title") %></h6>
                                                                    <span class="sub-text"><%# Eval("Description") %></span>
                                                                </div>
                                                            </asp:LinkButton>
                                                            <div class="drodown">
                                                                <a href="#" class="dropdown-toggle btn btn-sm btn-icon btn-trigger mt-n1 mr-n1" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                                <div class="dropdown-menu dropdown-menu-right">
                                                                    <ul class="link-list-opt no-bdr">

                                                                        <li>
                                                                            <asp:LinkButton ID="GetInfo" CommandArgument='<%#Eval("Id") %>' CommandName="GetInfo" OnClick="GetInfo" runat="server" title="Sửa thông tin" data-toggle="tooltip" data-placement="top" data-original-title="Sửa thông tin">
                                                                        <em class="icon ni ni-edit-fill"></em><span>Sửa</span>
                                                                            </asp:LinkButton>
                                                                            <asp:HyperLink ID="hplEmail" NavigateUrl='<%#NavigateURL() & "?view=mail&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
    <em class="icon ni ni-emails-fill"></em><span>Danh sách email</span>
                                                                            </asp:HyperLink>
                                                                        </li>
                                                                        <li><a href="#"><em class="icon ni ni-cross-sm"></em><span>Xóa</span></a></li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="project-meta">
                                                            <span class="badge badge-dim badge-light text-gray fw-bold fs-16px"><em class="icon ni ni-clock"></em><span>Số lượng email:<mark><%#GetTotalMail(CInt(Eval("id"))) %></mark> </span></span>
                                                        </div>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                        <div class="nk-block-des">
                            <br />
                            <asp:LinkButton ID="lbtAddTop" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary waves-effect waves-light">Thêm mới</asp:LinkButton>
                        </div>
                        <!-- .nk-block -->
                    </div>
                </div>
            </div>
        </div>
        <!-- .card-preview -->
    </ContentTemplate>
    <Triggers>
    </Triggers>
</asp:UpdatePanel>

<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div class="loading" id="loadizng">Loading&#8230;</div>
    </ProgressTemplate>
</asp:UpdateProgress>
<div class="modal fade zoom" tabindex="-1" id="modalEdit">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                <em class="icon ni ni-cross"></em>
            </a>
            <div class="modal-header">
                <h5 class="modal-title">Thêm mới / Chỉnh sửa</h5>
            </div>
            <div class="modal-body">
                <div class="form-validate is-alter">
                    <div class="form-group">
                        <label class="form-label" for="full-name">Tiêu đề</label>
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtTitle" required="" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="email-address">Mô tả</label>
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtMota" required="" runat="server" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer bg-light">
                <asp:LinkButton ID="lbtUpdate" OnClientClick="return checkvalidate();" runat="server" Font-Bold="true" ValidationGroup="InputValidate" CssClass="btn btn-primary">Cập nhật</asp:LinkButton>
                <a href="javascript:void(0);" type="button" class="btn btn-secondary waves-effect" data-dismiss="modal">Hủy thao khác</a>
                <asp:LinkButton ID="lbtDelete" Visible="false" OnClientClick="javascript: return confirm('Bạn có muốn xoá thư mục tin này không?');" ToolTip="Xoá thư mục" runat="server" CssClass="btn btn-danger">Xoá</asp:LinkButton>
            </div>
        </div>
    </div>
</div>
<script type="text/javascript">
    function checkvalidate() {
        var txtCatName = document.getElementById('<%=txtTitle.ClientID%>').value;
        if (txtCatName == "") {
            alert("Nhập tên hiển thị");
            return false;
        }


    }
    function Modalhoatdong() {
        $('#modalEdit').modal('show');
    };
    function ModalFollowUpClose() {
        $('#modalEditl').modal('hide');
        $('.modal-backdrop').css({
            display: 'none'
        });
    };
</script>




