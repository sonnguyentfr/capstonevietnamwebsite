<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_edit.ascx.vb" Inherits="NVCMS.Modules.LadingPage.inc_edit" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
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
                        <label class="form-label" for="<%#txtTrangDanhMuc.ClientID %>">Tên Trang giới thiệu</label>
                        <asp:TextBox ID="txtTrangDanhMuc" runat="server" Font-Size="14px" CssClass="form-control" required="required"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtTrangDanhMuc"
                            ForeColor="Red" ErrorMessage="Nhập tên trang" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="<%#txtOrdernumber.ClientID %>">Sắp xếp</label>
                        <asp:TextBox ID="txtOrdernumber" runat="server" Font-Size="14px" CssClass="form-control" required="required" Width="100px" TextMode="Number" Text="0"></asp:TextBox>
                    </div>
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <span class="preview-title-lg overline-title">Tóm tắt giới thiệu</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txttomtat" runat="server" CssClass="form-control" TextMode="MultiLine" Height="100px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <span class="preview-title-lg overline-title">Thông tin Thời gian địa điểm</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <dnn:TextEditor DefaultMode="basic" ID="txtdiadiem" Width="100%" Height="200px" runat="server" />
                        </div>
                    </div>
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <span class="preview-title-lg overline-title">Nội dung Landing Page</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <dnn:TextEditor DefaultMode="basic" ID="txtNoiDung" Width="100%" Height="800px" runat="server" />
                        </div>
                    </div>
                </div>

            </div>
            <!-- .card -->
        </div>
        <div class="col-md-4 col-lg-4 col-xxl-4">
            <div class="card card-bordered">
                <div class="card-inner">
                    <div class="form-group">
                        <label class="form-label" for="<%#txttieudephu.ClientID %>">Tiêu đề phụ (nếu có)</label>
                        <asp:TextBox ID="txttieudephu" runat="server" Font-Size="14px" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="default-04">Link Liên kết</label>
                        <div class="form-control-wrap">
                            <div class="form-icon form-icon-left">
                                <em class="icon ni ni-link"></em>
                            </div>
                            <asp:TextBox ID="txtLink" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="form-label" for="default-04">Danh mục cha</label>
                        <div class="form-control-wrap">
                            <asp:DropDownList ID="ddlParent" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                    </div>
                    <div class="form-group">
                        <label class="form-label" for="default-textarea">Ảnh đại diện</label>
                        <div class="form-control-wrap">
                            <input id="filelogo" runat="server" type="file" />
                        </div>
                        <div class="form-control-wrap">
                            <div id="dvPreviewlogo" runat="server"></div>
                            <asp:HiddenField ID="hpflinkimage" runat="server" />
                        </div>
                    </div>

                </div>
            </div>
            <div class="card card-bordered">
                <div class="card-inner">
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <span class="preview-title-lg overline-title">File LandingPage</span>
                            <span style="color: red">Nếu Upload File thì không cần chèn nội dung LANDING PAGE</span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div id="dvLandingPreview" runat="server" style="margin-bottom:6px;">
                            <asp:Literal ID="noidungfile" runat="server"></asp:Literal>
                            <asp:LinkButton ID="lbDeleteLanding" runat="server" CssClass="btn btn-sm btn-outline-danger ms-2"
                                OnClientClick="return confirm('Bạn có muốn xóa file landing này không?');">
                                <em class="icon ni ni-trash"></em> Xóa file
                            </asp:LinkButton>
                        </div>
                        <span style="color: blue">Chỉ upload file đuôi .html | .htm</span>
                        <div class="form-control-wrap">
                            <input id="filelanding" runat="server" type="file" accept=".html,.htm" onchange="validateFileLanding(this)" />
                        </div>
                        <asp:HiddenField ID="hdfLandingFile" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>
<asp:HiddenField ID="hdf_itemid" runat="server" Value="0" />
<script type="text/javascript">
    function validateFileLanding(input) {
        if (input.value === '') return true;
        var ext = input.value.split('.').pop().toLowerCase();
        if (ext !== 'html' && ext !== 'htm') {
            alert('Chỉ được upload file có đuôi .html hoặc .htm!');
            input.value = '';
            return false;
        }
        return true;
    }
    window.onload = function () {
        fileUpload = document.getElementById('<%=filelogo.ClientID%>');
        fileUpload.onchange = function () {
        if (typeof (FileReader) != "undefined") {
        var dvPreviewlogo = document.getElementById('<%=dvPreviewlogo.ClientID%>');
        dvPreviewlogo.innerHTML = "";
        var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
        for (var i = 0; i < fileUpload.files.length; i++) {
        var file = fileUpload.files[i];
        if (regex.test(file.name.toLowerCase())) {
        var reader = new FileReader();
        reader.onload = function (e) {
        var img = document.createElement("IMG");
        img.height = "100";
        img.src = e.target.result;
        dvPreviewlogo.appendChild(img);
        }
        reader.readAsDataURL(file);
        } else {
        alert(file.name + " file không đúng định dạng");
        dvPreviewlogo.innerHTML = "";
        return false;
        }
        }
        } else {
        alert("Trình duyệt của bạn không hỗ trợ Upload");
        }
        }
    };
</script>



