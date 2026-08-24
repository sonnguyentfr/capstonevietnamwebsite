<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Edit.ascx.vb" Inherits="NVCMS.Modules.Marketing.inc_edit" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<script src="/static/_Admin/js/ace/ace.js"></script>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Title %></h3>
        </div>
    </div>
    <!-- .nk-block-between -->
</div>
<div class="nk-block">
    <div class="row g-gs">

        <div class="col-md-12 col-lg-12 col-xxl-12">
            <div class="card card-bordered">
                <div class="card-inner">
                    <asp:Literal ID="lbMessage" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
        <div class="col-md-8 col-lg-8 col-xxl-8">
            <div class="card card-bordered">
                <div class="card-header border-bottom">
                    <ul class="cc_button">
                        <li>
                            <asp:LinkButton ID="lbtUpdate" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass=" btn btn-sm  btn-outline-primary" OnClientClick="return checkvalidate();">
                                <span>Cập nhật</span><em class="icon ni ni-save-fill"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtCancel" runat="server" Font-Bold="True" CssClass="__neo_submit-BTN btn btn-sm  btn-outline-danger">
                                <em class="icon ni ni-arrow-left"></em><span>Thoát</span></asp:LinkButton></li>

                        <li style="float: right;">
                            <asp:LinkButton ID="lbDelete" runat="server" Font-Bold="True" CssClass="btn btn-sm btn-dark" OnClientClick="formModified=false; return confirm('Bạn có thực sự muốn xóa các tin đã chọn không?');">
                                <span>Xóa </span><em class="icon ni ni-trash"></em>
                            </asp:LinkButton></li>
                    </ul>
                </div>
                <div class="card-inner">
                    <div class="form-group">
                        <label class="form-label" for="<%#txtTrangDanhMuc.ClientID %>">Tiêu đề</label>
                        <asp:TextBox ID="txtTrangDanhMuc" runat="server" Font-Size="14px" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtTrangDanhMuc"
                            ForeColor="Red" ErrorMessage="Nhập tiêu đề" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="<%#txtTrangDanhMuc.ClientID %>">Đường dẫn File </label>
                        <asp:TextBox ID="txtFilePath" runat="server" Font-Size="14px" CssClass="form-control" required="required"></asp:TextBox>
                    </div>
                </div>

            </div>
            <!-- .card -->
        </div>
        <div class="col-md-4 col-lg-4 col-xxl-4">
            <div class="card card-bordered">
                <div class="card-inner">
                    <div class="form-group">
                        <label class="form-label"><strong>Danh sách TOKEN</strong></label>
                        <ul>
                            <li><b>[__HOTENLOT__] </b>: Họ và tên đệm</li>
                            <li><b>[__TEN__] </b>: Tên</li>
                            <li><b>[__TRIENLAM__] </b>: Tên triển lãm</li>
                            <li><b>[__TRIENLAMDIADIEM__]</b> : Địa điểm tổ chức triển lãm</li>
                            <li><b>[__TRIENLAMBATDAU__] </b>: Thời gian bắt đầutriển lãm</li>
                            <li><b>[__TRIENLAMKETHUC__] </b>: Thời gian Kết thúc triển lãm</li>
                            <li><b>[__TRIENLAMNOIDUNG__] </b>: Nội dung triển lãm</li>
                            <li><b>[URL] </b>: Địa chỉ liên kết đến chi tiết tin bài</li>
                        </ul>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-md-12 col-lg-12 col-xxl-12">
            <div class="card card-bordered">
                <div class="card-inner">
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <span class="preview-title-lg overline-title">Nội dung gửi mail</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <dnn:TextEditor DefaultMode="basic" ID="txtValue" Width="100%" Height="1000px" runat="server" />
                            <%--<textarea id="txtValue" runat="server" cols="10" rows="10" class="codetemplate form-control" style="height: 400px;"></textarea>
                            <asp:HiddenField ID="hdf_textcode" runat="server" />--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>
<asp:HiddenField ID="hdf_itemid" runat="server" Value="0" />
<%--<style type="text/css">
    .ace_editor {
        height: 500px;
    }
</style>
<script>
    $(document).ready(function () {
        // Javascript editor
        var HeadScript = ace.edit("<%=txtValue.ClientID%>");
        HeadScript.setTheme("ace/theme/monokai");
        HeadScript.getSession().setMode("ace/mode/html");
        HeadScript.setShowPrintMargin(false);
        HeadScript.getSession().on('change', function (e) {
            $('#<%=hdf_textcode.ClientID%>').val(HeadScript.getValue());
        });
    });
</script>--%>



