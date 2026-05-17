<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Edit.ascx.vb" Inherits="DesktopModules.NV_Events.Manager.Events.Edit" %>
<script src="/Providers/HtmlEditorProviders/DNNConnect.CKE/js/ckeditor/4.15.1/ckeditor.js"></script>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
        </div>
    </div>
    <!-- .nk-block-between -->
</div>
<div class="nk-block">
    <div class="row g-gs">
        <div class="col-md-9 col-lg-9 col-xxl-9">
            <div class="card card-bordered">
                <div class="card-header border-bottom">
                    <ul class="cc_button">
                        <li>
                            <asp:LinkButton ID="lbtUpdateBottom" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass=" btn btn-sm  btn-outline-primary" OnClientClick="return checkvalidate();">
                                <span>Lưu thay đổi</span><em class="icon ni ni-save-fill"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtCancel" runat="server" Font-Bold="True" CssClass="__neo_submit-BTN btn btn-sm  btn-outline-danger">
                                <em class="icon ni ni-arrow-left"></em><span>Thoát</span></asp:LinkButton></li>

                        <li style="float: right;">
                            <asp:LinkButton ID="lbtDeleteBottom" runat="server" Font-Bold="True" CssClass="btn btn-sm btn-dark" OnClientClick="formModified=false; return confirm('Bạn có thực sự muốn xóa các tin đã chọn không?');">
                                <span>Xóa bài</span><em class="icon ni ni-trash"></em>
                            </asp:LinkButton></li>
                    </ul>

                </div>
                <div class="card-inner">
                    <asp:Label ID="lbResult" runat="server" CssClass="NormalRed"></asp:Label>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtTitle" Font-Names="Nunito" runat="server" CssClass="form-control form-control-xl form-control-outlined editor-f-22 editor-font" ValidationGroup="InputValidate"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtTitle.ClientID %>">Nhập tiêu đề</label>
                            <asp:RequiredFieldValidator ValidationGroup="InputValidate" ControlToValidate="txtTitle" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Nhập tiêu đề cho bài viết"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator
                                ID="valTitle" runat="server" ControlToValidate="txtTitle" ValidationGroup="InputValidate"
                                Display="Dynamic" CssClass="NormalRed" ErrorMessage="Tiêu đề phải chứa ít nhất 3 ký tự"
                                ForeColor="" ValidationExpression=".{3}.*"></asp:RegularExpressionValidator>
                            <div id="seotitle" class="chuanseo col-sm-12">
                            </div>
                        </div>
                    </div>
                    <div class="row gy-4 align-center">
                        <div class="col-lg-6">
                            <span class="preview-title-lg overline-title">Ngày Giờ Diễn Ra </span>
                            <div class="row gy-4 align-center">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <asp:DropDownList ID="ddlGio" Font-Names="Nunito" runat="server" CssClass="form-select form-control " Width="100%" ValidationGroup="InputValidate"></asp:DropDownList>
                                            <label class="form-label-outlined" for="<%=ddlGio.ClientID %>">Giờ</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <asp:DropDownList ID="ddlPhut" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate"></asp:DropDownList>
                                            <label class="form-label-outlined" for="<%=ddlPhut.ClientID %>">Phút</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <div class="form-icon form-icon-right">
                                                <em class="icon ni ni-calendar-alt"></em>
                                            </div>
                                            <asp:TextBox ID="txtStartdate" Font-Names="Nunito" runat="server" CssClass="form-control form-control-outlined datepicker" ValidationGroup="InputValidate"></asp:TextBox>
                                            <label class="form-label-outlined" for="<%=txtStartdate.ClientID %>">Ngày</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 displaynone">
                            <span class="preview-title-lg overline-title">Ngày Giờ Kết thúc </span>
                            <div class="row gy-4 align-center">
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <asp:DropDownList ID="ddlGioEnd" Font-Names="Nunito" runat="server" CssClass="form-select form-control " Width="100%" ValidationGroup="InputValidate"></asp:DropDownList>
                                            <label class="form-label-outlined" for="<%=ddlGioEnd.ClientID %>">Giờ</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <asp:DropDownList ID="ddlPhutend" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate"></asp:DropDownList>
                                            <label class="form-label-outlined" for="<%=ddlPhutend.ClientID %>">Phút</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <div class="form-icon form-icon-right">
                                                <em class="icon ni ni-calendar-alt"></em>
                                            </div>
                                            <asp:TextBox ID="txtEnddate" Font-Names="Nunito" runat="server" CssClass="form-control form-control-outlined datepicker" ValidationGroup="InputValidate"></asp:TextBox>
                                            <label class="form-label-outlined" for="<%=txtEnddate.ClientID %>">Ngày</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row gy-4 align-center">
                        <div class="col-lg-6">
                            <span class="preview-title-lg overline-title">Địa điểm </span>
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:TextBox ID="txtdiadiem" runat="server" CssClass="form-control form-control-outlined editor-f-18 editor-font" Height="60px" TextMode="MultiLine" ToolTip="Nhập Sapo tin bài" MaxLength="1000"></asp:TextBox>
                                    <label class="form-label-outlined" for="<%=txtdiadiem.ClientID %>">Địa điểm</label>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <span class="preview-title-lg overline-title">Thành phần tham dự </span>
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <asp:TextBox ID="txtthanhphan" runat="server" CssClass="form-control form-control-outlined editor-f-18 editor-font" Height="60px" TextMode="MultiLine" ToolTip="Nhập Sapo tin bài" MaxLength="1000"></asp:TextBox>
                                    <label class="form-label-outlined" for="<%=txtthanhphan.ClientID %>">Thành phần tham dự </label>
                                </div>
                            </div>

                        </div>
                    </div>

                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <span class="preview-title-lg overline-title">Nội dung sự kiện </span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <textarea id="TextEditor1" width="100%" runat="server" font-size="22px" height="400px" validationgroup="InputValidate"></textarea>
                        </div>
                    </div>

                </div>
            </div>
            <!-- .card -->
        </div>
        <!-- .col -->
        <div class="col-md-3 col-lg-3 col-xxl-3">
            <div class="card card-bordered h-100">
                <div class="card-inner">
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:DropDownList ID="ddlDanhmuc" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate"></asp:DropDownList>
                            <label class="form-label-outlined" for="<%=ddlDanhmuc.ClientID %>">Chọn Chuyên mục</label>
                            <asp:RequiredFieldValidator ID="valCategory" runat="server" ControlToValidate="ddlDanhmuc" Display="Dynamic" CssClass="NormalRed"
                                ErrorMessage="Chưa chọn chuyên mục" InitialValue="0" ValidationGroup="InputValidate"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:DropDownList ID="ddlhinhthuc" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate">
                                <asp:ListItem Text="--Chọn Hình thức tổ chức--"  Value="0"></asp:ListItem>
                                <asp:ListItem Text="Online"  Value="1"></asp:ListItem>
                                <asp:ListItem Text="Offline"  Value="2"></asp:ListItem>
                            </asp:DropDownList>
                            <label class="form-label-outlined" for="<%=ddlhinhthuc.ClientID %>">Hình Thức tổ chức</label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlhinhthuc" Display="Dynamic" CssClass="NormalRed"
                                ErrorMessage="Chưa chọn hình thức tổ chức" InitialValue="0" ValidationGroup="InputValidate"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">Cấu hình hiện thị</h6>
                            <div class="cauhinhtin g-2 align-center flex-wrap ">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkshow" runat="server" checked>
                                        <label class="custom-control-label" for="<%=chkshow.ClientID %>">Hiện thị</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkHienTrangChu" runat="server">
                                        <label class="custom-control-label" for="<%=chkHienTrangChu.ClientID %>">Hiện trang chủ</label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <a href="#" class="btn btn-xs btn-info"><em class="icon ni ni-reports-alt"></em><span>Ảnh đại diện</span></a>
                                <div class="border border-primary p-2">
                                    <div id="dvPreviewlogo" runat="server"></div>
                                    <asp:HiddenField ID="hpflinkimage" runat="server" />
                                </div>
                                <div class="border border-primary p-2">
                                    <input id="filelogo" runat="server" type="file" />
                                </div>

                                <!-- .nk-tb-list -->
                            </div>
                        </div>
                    </div>
                    <div class="card card-bordered h-100">
                        <div class="card-inner">
                            <div class="card-head">
                                <h5 class="card-title">Liên hệ đăng ký</h5>
                            </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtCost.ClientID %>">Tiền vé</label>
                                    <asp:TextBox ID="txtCost" Text="0" CssClass="form-control" runat="server" TextMode="Number"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtContactName.ClientID %>">Họ và tên</label>
                                    <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control" Font-Size="14px"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtContactMail.ClientID %>">Email</label>
                                    <asp:TextBox ID="txtContactMail" runat="server" CssClass="form-control" Font-Size="14px"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtContactPhone.ClientID %>">Số điện thoại</label>
                                    <asp:TextBox ID="txtContactPhone" runat="server" CssClass="form-control" Font-Size="14px"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="<%#txtContactAdd.ClientID %>">Địa chỉ</label>
                                    <div class="form-control-wrap">
                                        <textarea class="form-control form-control-sm" placeholder="Địa chỉ đặt vé" ID="txtContactAdd" runat="server"></textarea>
                                    </div>
                                </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- .card -->
        </div>

        <!-- .col -->
    </div>

</div>
<script type="text/javascript">
    //upload anh
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
                            img.width = "300";
                            img.src = e.target.result;
                            dvPreviewlogo.appendChild(img);
                        }
                        reader.readAsDataURL(file);
                    } else {
                        alert(file.name + " is not a valid image file.");
                        dvPreviewlogo.innerHTML = "";
                        return false;
                    }
                }
            } else {
                alert("This browser does not support HTML5 FileReader.");
            }
        }
    };
    //---
    var editor = CKEDITOR.replace('<%=TextEditor1.ClientID %>');
    elEditor = editor.getData();
    function checkvalidate() {
        var Title = document.getElementById('<%=txtTitle.ClientID%>').value;
        var diadiem = document.getElementById('<%=txtdiadiem.ClientID%>').value;
        var ddlhinhthuc = document.getElementById('<%=ddlhinhthuc.ClientID%>').value;
        if (Title == "") {
            alert("Nhập tiêu đề");
            return false;
        }
        if (diadiem == "") {
            alert("Bạn chưa nhập địa điểm tổ chức");
            return false;
        }
        if (ddlhinhthuc == 0) {
            alert("Bạn chưa chọn hình thức tổ chức");
            document.getElementById('ddlhinhthuc').focus();
            return false;
        }
    }

</script>



