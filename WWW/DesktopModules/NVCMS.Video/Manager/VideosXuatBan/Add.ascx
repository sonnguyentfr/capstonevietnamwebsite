<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Add.ascx.vb" Inherits="DesktopModules.Video.Manager.Video.Edit" EnableViewState="true" %>
<script src="/Providers/HtmlEditorProviders/DNNConnect.CKE/js/ckeditor/4.15.1/ckeditor.js"></script>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<script type="text/javascript" src="/js/base64.js"></script>
<script src="/js/Common.js" type="text/javascript"></script>
<script type="text/javascript" src="/static/_admin/js/jquery.charactercounter.js"></script>
<link href="/static/_admin/js/jquery.tagsinput/bootstrap-tagsinput.css" rel="stylesheet" />
<script src="/static/_admin/js/jquery.tagsinput/bootstrap-tagsinput.js"></script>
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.Current.ActiveTab.Title %></h3>
        </div>
    </div>
    <!-- .nk-block-between -->
</div>
<div class="nk-block">
    <div class="row">
        <div class="col-md-6 col-lg-6 col-xxl-6">
            <div class="card card-bordered">
                <div class="card-header border-bottom">
                    <ul class="cc_button">
                        <li>
                            <asp:LinkButton ID="lbtSave" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass=" btn btn-sm  btn-outline-primary" OnClientClick="formModified=false; updateFormAttachedMedia(); saveNews(); return false;">
                                <span>Lưu thay đổi</span><em class="icon ni ni-save-fill"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtTralai" Visible="false" ValidationGroup="InputValidate" runat="server" CssClass="btn  btn-sm  btn-warning" OnClientClick="formModified=false; updateFormAttachedMedia(); saveNews(); return false;">
                                <span>Trả lại</span><em class="icon ni ni-send"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtSaveXB" Visible="false" ValidationGroup="InputValidate" runat="server" CssClass="btn  btn-sm  btn-danger" OnClientClick="formModified=false; return checkvalidatexuatban(); updateFormAttachedMedia(); saveNews(); return false; ">
                                <span>Xuất bản ngay</span> <em class="icon ni ni-check-circle"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtCancelTop" runat="server" Font-Bold="True" CssClass="__neo_submit-BTN btn btn-sm  btn-outline-danger">
                                <em class="icon ni ni-arrow-left"></em><span>Thoát</span></asp:LinkButton></li>
                        <li style="float: right;">
                            <asp:LinkButton ID="lbtDeleteTop" runat="server" Font-Bold="True" CssClass="btn btn-sm btn-dark" OnClientClick="formModified=false; return confirm('Bạn có thực sự muốn xóa các tin đã chọn không?');">
                                <span>Xóa bài</span><em class="icon ni ni-trash"></em>
                            </asp:LinkButton></li>
                    </ul>

                </div>
                <div class="card-inner">
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
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtSummary" runat="server" CssClass="form-control form-control-outlined editor-f-18 editor-font" Height="60px" TextMode="MultiLine" ToolTip="Nhập Sapo tin bài" MaxLength="1000"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtSummary.ClientID %>">Tóm tắt</label>
                        </div>
                    </div>
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <h6 class="title">Nội dung</h6>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <textarea id="teContent" width="100%" runat="server" font-size="22px" height="400px" validationgroup="InputValidate"></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtButDanh" runat="server" CssClass="form-control form-control-xl form-control-outlined"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtButDanh.ClientID %>">Bút Danh Tác giả</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <label class="form-label" for="<%=txtTags.ClientID %>">Tag bài viết</label>
                            <asp:TextBox ID="txtTags" runat="server" CssClass="form-control" MaxLength="200" data-role="tagsinput"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <!-- .card -->
        </div>
        <!-- .col -->
        <div class="col-md-6 col-lg-6 col-xxl-6 pl-0">
            <div class="card card-bordered h-100">
                <div class="card-inner">
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">Cấu hình hiện thị</h6>
                            <div class="cauhinhtin align-center flex-wrap">
                                <div class="custom-control custom-control-sm custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkBaiMoiNhat" runat="server">
                                    <label class="custom-control-label" for="<%=chkBaiMoiNhat.ClientID %>">Bài mới</label>
                                </div>
                                <div class="custom-control custom-control-sm custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkHotCat" runat="server">
                                    <label class="custom-control-label" for="<%=chkHotCat.ClientID %>">Nổi bật mục</label>
                                </div>
                                <div class="custom-control custom-control-sm custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="chkHotSite" runat="server">
                                    <label class="custom-control-label" for="<%=chkHotSite.ClientID %>">Nổi bật trang</label>
                                </div>
                                <div class="custom-control custom-control-sm">
                                    <input type="checkbox" class="custom-control-input" id="Checkbox1" runat="server">
                                    <label class="custom-control-label" for="<%=chkHotSite.ClientID %>">Nổi bật trang</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">Cấu hình hiện thị</h6>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <h6 class="overline-title title">Nhuận bút</h6>
                                    <asp:TextBox ID="txtnhuanbut" runat="server" CssClass="form-control auto currency" Text="0"></asp:TextBox>
                                </div>
                                <div class="g">
                                    <h6 class="overline-title title">NGÀY GIỜ XUẤT BẢN</h6>
                                    <asp:TextBox ID="txtPublishedDate" runat="server" CssClass="form-control datetimepicker"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <a href="#" class="btn btn-xs btn-info"><em class="icon ni ni-reports-alt"></em><span>Ảnh đại diện</span></a>
                            <div class="border border-primary p-2">
                                <div id="divImagePath" runat="server"></div>
                                <asp:HiddenField ID="hdfImagePath" runat="server" />
                                <div class="form-group">
                                    <div class="form-control-wrap uploadbtn">
                                        <input id="inptFileImagePath" runat="server" type="file" />
                                    </div>
                                </div>
                            </div>
                            <!-- .nk-tb-list -->
                        </div>
                    </div>
                    <%--<asp:UpdatePanel ID="upcode" runat="server">
                        <ContentTemplate>--%>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:DropDownList ID="ddlkieuvideo" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate" AutoPostBack="true" OnSelectedIndexChanged="ddlkieuvideo_SelectedIndexChanged">
                                <asp:ListItem Value="0">--Chọn hình thức tải Videos--</asp:ListItem>
                                <asp:ListItem Value="1">Sử dụng mã nhúng từ website khác</asp:ListItem>
                                <asp:ListItem Value="2">Youtube</asp:ListItem>
                                <asp:ListItem Value="3">Upload File từ máy tính</asp:ListItem>
                            </asp:DropDownList>
                            <label class="form-label-outlined" for="<%=ddlkieuvideo.ClientID %>">Chọn kiểu Video</label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlkieuvideo" Display="Dynamic" CssClass="NormalRed"
                                ErrorMessage="Chưa chọn kiểu Video" InitialValue="0" ValidationGroup="InputValidate"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-control-wrap" id="Manhhung" runat="server" visible="false">
                            <div class="card-inner">
                                <h6 class="overline-title title">Mã nhúng</h6>
                                <asp:TextBox ID="txtMaNhung" CssClass="form-control" runat="server" Font-Size="14px" TextMode="MultiLine" Height="80px" AutoPostBack="true" OnTextChanged="txtMaNhung_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-control-wrap" id="dyoutube" runat="server" visible="false">
                            <div class="card-inner">
                                <h6 class="overline-title title">Mã youtube</h6>
                                <asp:TextBox ID="txtlinkYotube" CssClass="form-control" runat="server" Font-Size="14px" AutoPostBack="true" OnTextChanged="txtlinkYotube_TextChanged"></asp:TextBox>
                                <br />
                                <i>Ví dụ:</i> www.youtube.com/watch?v=<font style="color: Red; font-weight: bold;">dyQA6a78xYE</font>
                            </div>
                        </div>
                        <div class="form-control-wrap" id="dupload" runat="server" visible="false">
                            <div class="card-inner">
                                <h6 class="overline-title title">Tải Media</h6>
                                <mark><small>Sử dụng file có định dang: *.avi; *.mp4 | Kích thước:< 50 Mb</small></mark>
                                <hr />
                                <asp:FileUpload ID="file_upload" class="btn btn-xs btn-info multi" AllowMultiple="false" runat="server" />
                                <progress id="fileProgress" class="fileProgress" style="display: none"></progress>
                            </div>

                        </div>
                        <div class="form-control-wrap">
                            <asp:Literal ID="ltrviewdemo" runat="server"></asp:Literal>
                            <div id="viewdemo"></div>
                            <asp:HiddenField ID="hdf_linkvideo" runat="server" />
                            <asp:HiddenField ID="hdf_itemid" runat="server" />
                        </div>
                    </div>
                    <%-- </ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
            <!-- .card -->
        </div>

        <!-- .col -->
    </div>
</div>

<script type="text/javascript">
    //Tecontent
    var editor = CKEDITOR.replace('<%=teContent.ClientID %>');
    //upload ảnh đại diện
    window.onload = function () {
        fileUpload = document.getElementById('<%=inptFileImagePath.ClientID%>');
        fileUpload.onchange = function () {
            if (typeof (FileReader) != "undefined") {
                var dvPreviewlogo = document.getElementById('<%=divImagePath.ClientID%>');
                dvPreviewlogo.innerHTML = "";
                var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
                for (var i = 0; i < fileUpload.files.length; i++) {
                    var file = fileUpload.files[i];
                    if (regex.test(file.name.toLowerCase())) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            var img = document.createElement("IMG");
                            img.height = "350";
                            img.src = e.target.result;
                            dvPreviewlogo.appendChild(img);
                        }
                        reader.readAsDataURL(file);
                    } else {
                        alert(file.name + " Tên file không đúng:Không viết tiếng việt, không để dấu cách!.");
                        dvPreviewlogo.innerHTML = "";
                        return false;
                    }
                }
            } else {
                alert("This browser does not support HTML5 FileReader.");
            }
        }
    };
    //Upload Video
    $("#<%=file_upload.ClientID%>").on("change", function () {
        var datamedia = new FormData();
        var fileInput = document.getElementById('<%=file_upload.ClientID%>');
        var itemid = document.getElementById('<%=hdf_itemid.ClientID%>').value;
        if (fileInput.files.length > 1) {
            alert("Bạn chỉ chọn 1 file!");
        } else {
            for (i = 0; i < fileInput.files.length; i++) {
                var sfilename = fileInput.files[i].name;
                datamedia.append(sfilename, fileInput.files[i]);
            }
        }

        uploadMediaToServer(datamedia, itemid);
        $(this).val('');
        $('#<%=file_upload.ClientID%>').val('');
    });
    function uploadMediaToServer(formData, itemid) {
        $.ajax({
            url: '/DesktopModules/NVCMS.Video/Manager/Services/UploadVideo.ashx?itemid=' + itemid,
            type: 'POST',
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            async: false,
            success: OnSuccess,
            xhr: function () {
                var fileXhr = $.ajaxSettings.xhr();
                if (fileXhr.upload) {
                    $("#fileProgress").show();
                    fileXhr.upload.addEventListener("progress", function (e) {
                        if (e.lengthComputable) {
                            $("#fileProgress").attr({
                                value: e.loaded,
                                max: e.total
                            });
                        }
                    }, false);
                }
                return fileXhr;
            }
        });
    }
    function OnSuccess(response) {
        $("#fileProgress").hide();
        document.getElementById('viewdemo').innerHTML = "<video controls='controls' src='" + response + "' style='width:100%' /></video>";
        document.getElementById('<%=hdf_linkvideo.ClientID %>').value = response;
    }
    function checkvalidatexuatban() {
        var res = true;
        var nhuanbut = document.getElementById('<%=txtnhuanbut.ClientID%>').value;
        if (nhuanbut == 0) {
            alert("Bạn vui lòng chậm nhuật bút trước khi xuất bản");
            $('#<%= txtnhuanbut.ClientID%>').focus();
            return false;
        }
        var txtPublishedDate = document.getElementById('<%=txtPublishedDate.ClientID%>').value;
        if (txtPublishedDate == "") {
            alert("Bạn vui lòng chọn ngày giờ xuất bản");
            $('#<%= txtPublishedDate.ClientID%>').focus();
            return false;
        }
        return res;
    }



</script>
