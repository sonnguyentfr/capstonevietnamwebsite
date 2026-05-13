<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Edit.ascx.vb" Inherits="NVCMS.Modules.BannerAdv.edit" %>
<div class="nk-block nk-block-lg">
    <div class="nk-block-head">
        <div class="nk-block-head-content">
            <h4 class="title nk-block-title">Thêm mới vị trí banner</h4>
        </div>
    </div>
    <div class="card card-preview">
        <div class="card-inner">
            <div class="preview-block">
                <div class="row gy-4">
                    <div class="col-lg-6 col-sm-6">
                        <div class="row gy-4">
                            <div class="col-lg-12 col-sm-12">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" class="form-control form-control-xl form-control-outlined" id="Title" runat="server">
                                        <label class="form-label-outlined" for="<%=Title.clientid %>">Tên vị trí</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" class="form-control form-control-xl form-control-outlined" id="txtdai" runat="server" oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*?)\..*/g, '$1');">
                                        <label class="form-label-outlined" for="<%=txtdai.ClientId %>">Dài</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-6 col-sm-6">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" class="form-control form-control-xl form-control-outlined" id="txtCao" runat="server" oninput="this.value = this.value.replace(/[^0-9.]/g, '').replace(/(\..*?)\..*/g, '$1');">
                                        <label class="form-label-outlined" for="<%=txtCao.ClientId %>">Cao</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12">
                                <div class="form-group">
                                    <span class="preview-title-lg overline-title">Ảnh minh họa</span>
                                    <div class="form-control-wrap">
                                        <input id="filelogo" runat="server" type="file" />
                                    </div>
                                    <div class="form-control-wrap">
                                        <div id="dvPreviewlogo" runat="server"></div>
                                        <asp:HiddenField ID="hpflinkimage" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-12 col-sm-12">
                                <div class="form-group">
                                    <asp:LinkButton ID="lbtUpdate" runat="server" CssClass="btn btn-success">Cập nhật</asp:LinkButton>
                                    <asp:LinkButton ID="lbtCancel" runat="server" CssClass="btn btn-primary">Hủy Thao tác</asp:LinkButton>
                                    <button class="btn btn-primary" type="reset">Xóa</button>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- .card-preview -->

    <!-- .code-block -->
</div>

<script type="text/javascript">
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

</script>
