<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Edit.ascx.vb" Inherits="NVCMS.Modules.BannerAdv.editbanner" %>
<div class="nk-block nk-block-lg">
    <div class="nk-block-head">
        <div class="nk-block-head-content">
            <h4 class="title nk-block-title"><%=PortalSettings.ActiveTab.Description %></h4>
        </div>
    </div>
    <div class="card card-bordered">
        <div class="card-inner">
            <div class="gy-3">
                <div class="row g-3 align-center">
                    <div class="col-lg-7">
                        <div class="row g-3 align-center">
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label class="form-label" for="site-name">Tiêu đề</label>
                                </div>
                            </div>
                            <div class="col-lg-9">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" id="Title" runat="server" required="required" class="form-control">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel ID="upbanneried" runat="server">
                            <ContentTemplate>
                                <div class="row g-3 align-center">
                                    <div class="col-lg-3">
                                        <div class="form-group">
                                            <label class="form-label">Vị trí hiện thị</label>
                                        </div>
                                    </div>
                                    <div class="col-lg-9">
                                        <div class="form-group">
                                            <div class="form-control-wrap">
                                                <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="form-control select2" OnSelectedIndexChanged="ddlvitri_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row g-3 align-center">
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label class="form-label">Dài</label>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="form-group">
                                            <div class="form-control-wrap">
                                                <input type="text" required="required" id="txtdai" runat="server" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-2">
                                        <div class="form-group">
                                            <label class="form-label">Cao</label>
                                        </div>
                                    </div>
                                    <div class="col-lg-4">
                                        <div class="form-group">
                                            <div class="form-control-wrap">
                                                <input type="text" required="required" id="txtCao" runat="server" class="form-control">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row g-3 align-center">
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label class="form-label">Kiểu Banner</label>
                                </div>
                            </div>
                            <div class="col-lg-9">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <asp:DropDownList ID="ddlkieubanner" runat="server" CssClass="form-control select2"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="Anh">
                            <div class="row g-3 align-center">
                                <div class="col-lg-5">
                                    <div class="form-group">
                                        <label class="form-label">Upload ảnh banner</label>
                                    </div>
                                </div>
                                <div class="col-lg-7">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <input id="filelogo" runat="server" type="file" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-12">
                                    <div id="dvPreviewlogo" runat="server"></div>
                                    <asp:HiddenField ID="hpflinkimage" runat="server" />
                                </div>
                                <div class="col-lg-3">
                                    <div class="form-group">
                                        <label class="form-label">Liên kêt</label>
                                        <span class="form-note">Link đến trang</span>
                                    </div>
                                </div>
                                <div class="col-lg-9">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <input type="text" id="txtLink" runat="server" required="required" class="form-control">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="scipt">
                            <div class="row g-3 align-center">
                                <div class="col-lg-5">
                                    <div class="form-group">
                                        <label class="form-label">Code</label>
                                        <span class="form-note">Chèn code quảng cáo ở đây</span>
                                    </div>
                                </div>
                                <div class="col-lg-7">
                                    <div class="form-group">
                                        <div class="form-control-wrap">
                                            <textarea id="txtcode" runat="server" class="form-control" rows="3"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3 align-center">
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label class="form-label">HIện thị từ</label>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type='text' class="form-control date-picker" id="tungay" runat="server" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label class="form-label">đến</label>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type='text' class="form-control date-picker" id="dengay" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3 align-center">
                            <div class="col-lg-5">
                                <div class="form-group">
                                    <label class="form-label">Sắp xếp</label>
                                    <span class="form-note">Thứ tự hiện thị</span>
                                </div>
                            </div>
                            <div class="col-lg-7">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" id="thutu" runat="server" required="required" class="form-control">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3 align-center">
                            <div class="col-lg-5">
                                <div class="form-group">
                                    <label class="form-label" for="site-off">Trạng thái</label>
                                </div>
                            </div>
                            <div class="col-lg-7">
                                <div class="form-group">
                                    <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" name="reg-public" id="chkactive" runat="server">
                                        <label class="custom-control-label" for="<%=chkactive.clientId %>"></label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3">
                            <div class="col-lg-7 offset-lg-5">
                                <div class="form-group mt-2">
                                    <asp:LinkButton ID="lbtUpdate" runat="server" CssClass="btn btn-primary">Cập nhật</asp:LinkButton>
                                    <asp:LinkButton ID="lbtCancel" runat="server" CssClass="btn btn-primary">Hủy Thao tác</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-5">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:Image ID="imgshowvitri" CssClass="anhdaidemvitri" runat="server" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlCategory" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- card -->
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var eSelect = document.getElementById('<%=ddlkieubanner.ClientID%>').value;
        $("#scipt").hide();
        $("#Anh").hide();
        if (eSelect == 1) {
            $("#scipt").hide();
            $("#Anh").show();
        }
        if (eSelect == 3) {
            $("#scipt").show();
            $("#Anh").hide();
        }
        $("#<%=ddlkieubanner.ClientID%>").change(function () {
            $("#<%=ddlkieubanner.ClientID%> option:selected").each(function () {
                if ($(this).attr("value") == "1") {
                    $("#scipt").hide();
                    $("#Anh").show();
                }
                if ($(this).attr("value") == "3") {
                    $("#scipt").show();
                    $("#Anh").hide();
                }
            });
        }).change();
    });
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
<script>
    $(function () {
        $('.datepicker').datetimepicker({

        });
    });
</script>
<style type="text/css">
    .anhdaidemvitri {
        max-width: 100%;
    }

    .inputdate {
        width: 200px;
    }

        .inputdate .datepicker {
            width: 200px;
        }
</style>
