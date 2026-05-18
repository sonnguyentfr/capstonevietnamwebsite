<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_edit.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.news.newsedit" EnableViewState="true" %>
<script src="/Providers/HtmlEditorProviders/DNNConnect.CKE/js/ckeditor/4.15.1/ckeditor.js"></script>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css" />
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<style type="text/css">
    .chonportal {
        display: none;
    }

    .user-card-s2 .user-info {
        margin: 10px 0px !important;
        width: 100%;
    }

    .custom-file, .custom-file-input, .custom-file-label, .custom-file-label::after {
        height: 30px;
    }

    .custom-file-label {
        padding: 5px;
        font-size: 12px;
    }

        .custom-file-label::after {
            content: "Chọn file";
            padding: 5px 29px;
        }

    .user-avatar.lg {
        height: 120px;
        width: 120px;
        font-size: 28px;
        font-weight: 400;
        border-radius: 0px;
        margin-bottom: 10px !important;
    }

    .anhcover img {
        max-width: 100%;
        max-height: 300px
    }

    ul.thongtindiachi {
    }

        ul.thongtindiachi li {
            padding: 5px 0px;
            margin-bottom: 5px;
            border-bottom: solid 1px #f9f9f9;
            font-size: 13px;
        }

            ul.thongtindiachi li em {
                font-size: 15px;
                margin-right: 5px;
            }

    [data-simplebar] {
        position: unset;
    }

    ul.timeline-list {
        width: 100%;
        overflow: scroll hidden;
    }

        ul.timeline-list::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            background-color: #F5F5F5;
        }

        ul.timeline-list::-webkit-scrollbar {
            width: 6px;
            height: 6px;
            background-color: #F5F5F5;
        }

        ul.timeline-list::-webkit-scrollbar-thumb {
            background-color: #000000;
        }

    .timeline-item {
        display: table-cell;
        padding-bottom: 0.5rem;
    }

        .timeline-item a {
            position: relative;
            top: -13px;
        }

    .timeline-date {
        width: 150px;
        font-size: 13px;
    }

        .timeline-date .icon {
            vertical-align: middle;
            color: #8094ae;
            display: inline-block;
            position: relative;
            margin-right: 0px;
            right: auto;
            top: -1px;
        }

    .timeline-item.active a {
        color: red;
        font-weight: 600;
    }
</style>
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between g-3">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title">
                <asp:TextBox ID="txtNameOfSchool" runat="server" CssClass="form-control form-control-lg" Enabled="false"></asp:TextBox></h3>
        </div>
        <div class="nk-block-head-content">
            <asp:LinkButton ID="lbtUpdate" runat="server" ValidationGroup="InputValidateSchool" CssClass="btn btn-outline-light bg-white d-none d-sm-inline-flex"><em class="icon ni ni-save"></em><span>Lưu thông tin</span></asp:LinkButton>
            <a href="/quan-tri/doi-tac/danh-sach-truong" class="btn btn-outline-light bg-white d-none d-sm-inline-flex"><em class="icon ni ni-arrow-left"></em><span>Back</span></a>

        </div>
    </div>
</div>
<div class="nk-block">
    <div class="row g-gs">
        <div class="col-md-9 col-lg-9 col-xxl-9">
            <div class="card card-bordered">
                <div class="card-aside-wrap">
                    <div class="card-content">
                        <ul class="nav nav-tabs nav-tabs-mb-icon nav-tabs-card">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#thongtin"><em class="icon ni ni-user-circle"></em><span>Thông tin</span></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-toggle="tab" href="#lichsuchinhsua"><em class="icon ni ni-repeat"></em><span>Lịch sử chỉnh sửa</span></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-toggle="tab" href="#Admission"><em class="icon ni ni-file-text"></em><span>Admission</span></a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" href="#"><em class="icon ni ni-bell"></em><span>Major</span></a>
                            </li>
                            <li class="nav-item nav-item-trigger d-xxl-none">
                                <a href="#" class="toggle btn btn-icon btn-trigger" data-target="userAside"><em class="icon ni ni-user-list-fill"></em></a>
                            </li>
                        </ul>
                        <!-- .nav-tabs -->
                        <div class="card-inner">
                            <div class="tab-content">
                                <div class="tab-pane active" id="thongtin">
                                    <div class="nk-block">
                                        <div class="nk-block-head">
                                            <h5 class="title">Thông tin về Trường</h5>
                                        </div>
                                    </div>
                                    <!-- .nk-block -->
                                    <div class="row g-4">
                                        <div class="col-lg-12">
                                            <div class="form-group">
                                                <label class="form-label" for="full-name-1">Tóm tắt:</label>
                                                <div class="form-control-wrap">
                                                    <asp:TextBox ID="txtTomtat" runat="server" TextMode="MultiLine" CssClass="form-control" ValidationGroup="InputValidateSchool"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ValidationGroup="InputValidateSchool" ControlToValidate="txtTomtat" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Nhập tóm tắt cho trường"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator
                                                        ID="valTitle" runat="server" ControlToValidate="txtTomtat" ValidationGroup="InputValidateSchool"
                                                        Display="Dynamic" CssClass="NormalRed" ErrorMessage="Tiêu đề phải chứa ít nhất 3 ký tự"
                                                        ForeColor="" ValidationExpression=".{3}.*"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <%--Thông tin--%>
                                        <div class="col-lg-12">
                                            <div class="nk-block-head">
                                                <h5 class="title">Thông tin về Trường</h5>
                                            </div>
                                            <div class="bq-note">
                                                <div class="bq-note-item">
                                                    <div class="bq-note-text">
                                                        <asp:Literal ID="txtInfoEN" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-12">
                                            <div class="form-group">
                                                <div class="form-control-wrap">
                                                    <dnn:TextEditor DefaultMode="basic" ID="txtInfo" Width="100%" Height="400px" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--Điểm mạnh--%>
                                        <div class="col-lg-12" style="display:none">
                                            <div class="nk-block-head">
                                                <h5 class="title">Điểm mạnh</h5>
                                            </div>
                                            <div class="bq-note">
                                                <div class="bq-note-item">
                                                    <div class="bq-note-text">
                                                        <asp:Literal ID="ltrDiemManh" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-12"  style="display:none">
                                            <div class="form-group">

                                                <div class="form-control-wrap">
                                                    <dnn:TextEditor DefaultMode="basic" ID="txtDiemManh" Width="100%" Height="400px" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--Kiểm định--%>
                                        <div class="col-lg-12"  style="display:none">
                                            <div class="nk-block-head">
                                                <h5 class="title">Kiểm định</h5>
                                            </div>
                                            <div class="bq-note">
                                                <div class="bq-note-item">
                                                    <div class="bq-note-text">
                                                        <asp:Literal ID="ltrkiemdinh" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-12"  style="display:none">
                                            <div class="form-group">
                                                <div class="form-control-wrap">
                                                    <asp:TextBox ID="txtKiemDinh" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%--Xếp hạng--%>
                                        <div class="col-lg-12"  style="display:none">
                                            <div class="nk-block-head">
                                                <h5 class="title">Xếp hạng</h5>
                                            </div>
                                            <div class="bq-note">
                                                <div class="bq-note-item">
                                                    <div class="bq-note-text">
                                                        <asp:Literal ID="ltrXepHang" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-12"  style="display:none">
                                            <div class="form-group">
                                                <div class="form-control-wrap">
                                                    <asp:TextBox ID="txtXepHang" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <%--Loaj trường--%>
                                        <div class="col-lg-12">
                                            <div class="nk-block-head" style="display:none">
                                                <h5 class="title">Loại Trường</h5>
                                            </div>
                                            <div class="bq-note">
                                                <div class="bq-note-item">
                                                    <div class="bq-note-text">
                                                        <asp:Literal ID="ltrLoaitruong" runat="server"></asp:Literal>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-12" style="display:none">
                                            <div class="form-group">
                                                <div class="form-control-wrap">
                                                    <asp:TextBox ID="txtLoaitruong" runat="server" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>


                                    </div>
                                    <!-- .nk-block -->
                                </div>
                                <div class="tab-pane" id="lichsuchinhsua">
                                    <div class="nk-block">
                                        <div class="nk-block-head">
                                            <h5 class="title">Lịch sử chỉnh sửa</h5>
                                        </div>
                                        <div class="timeline">
                                            <ul class="timeline-list data-simplebar">
                                                <asp:Repeater ID="rptListHistory" runat="server">
                                                    <ItemTemplate>
                                                        <li class="timeline-item <%#GetSelect(ID, Eval("Id")) %>">
                                                            <div class="timeline-status bg-primary"></div>
                                                            <div class="timeline-date">
                                                                <a href="/quan-tri/doi-tac/danh-sach-truong?view=edit&itemid=<%#Eval("TruongId") %>&verid=<%#Eval("Id") %>">
                                                                    <em class="icon ni ni-alarm-alt"></em><%#BL.FormatDate(Eval("CreatedDate")) %>
                                                                    <br />
                                                                    <em class="icon ni ni-users-fill"></em><%#BL.GetButDanh(PortalId, Eval("UserId")) %>
                                                                    <br />
                                                                    # <%#Eval("Id") %></a>
                                                            </div>
                                                        </li>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ul>
                                        </div>
                                        <div class="row gy-4">
                                            <div class="col-sm-6">
                                                <h4>Bản đang xem
                                            <asp:Literal ID="ltridId2" runat="server"></asp:Literal></h4>
                                                <asp:Literal ID="ltrbanTruocDo" runat="server"></asp:Literal>

                                            </div>
                                            <div class="col-sm-6">
                                                <h4>Bản cũ
                                            <asp:Literal ID="ltridId" runat="server"></asp:Literal></h4>
                                                <asp:Literal ID="ltrbanHientai" runat="server"></asp:Literal>
                                            </div>

                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane" id="Admission">
                                    <div class="nk-block">
                                        <div class="nk-block-head">
                                            <h5 class="title">Admission</h5>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <!-- .nk-block -->
                        </div>
                        <!-- .card-inner -->
                    </div>
                    <!-- .card-content -->

                    <!-- .card-aside -->
                </div>
                <!-- .card-aside-wrap -->
            </div>
        </div>
        <div class="col-md-3 col-lg-3 col-xxl-3">
            <div class="card card-bordered">

                <div class="simplebar-content" style="padding: 0px;">
                    <div class="card-inner">
                        <div class="user-card user-card-s2">
                            <div class="user-avatar lg bg-primary">
                                <div id="dvPreviewlogo" runat="server"></div>
                                <asp:HiddenField ID="hpfLogo" runat="server" />
                            </div>
                            <div class="form-control-wrap">
                                <div class="custom-file">
                                    <input type="file" class="custom-file-input" id="filelogo" runat="server">
                                    <label class="custom-file-label" for="<%=filelogo.ClientID %>">Chọn logo</label>
                                </div>
                            </div>
                            <div class="user-info">
                                <div class="badge badge-outline-light badge-pill ucap">
                                    <asp:Literal ID="txtloai" runat="server"></asp:Literal>
                                </div>
                                <h5>
                                    <asp:Literal ID="ltrNameOfSchool" runat="server"></asp:Literal></h5>
                                <asp:TextBox ID="txtwebsite" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                            <div class="user-info">
                            </div>
                        </div>
                    </div>
                    <div class="card-inner">
                        <ul class="thongtindiachi">
                            <li>
                                <strong><em class="icon ni ni-trend-up"></em>Năm thành lập:</strong>
                                <asp:Literal ID="txtnamthanhlap" runat="server"></asp:Literal>
                            </li>
                            <li>
                                <strong><em class="icon ni ni-map-pin"></em>Vị trí:</strong>
                                <asp:Literal ID="txtVitri" runat="server"></asp:Literal>
                            </li>
                            <li>
                                <strong><em class="icon ni ni-map-pin"></em>Địa chỉ:</strong>
                                <asp:Literal ID="ltrdiachi" runat="server"></asp:Literal>
                            </li>
                            <li>
                                <strong><em class="icon ni ni-dribbble-round"></em>Quốc gia:</strong>
                                <asp:Literal ID="ltrQuocGia" runat="server"></asp:Literal>
                            </li>
                            <li>
                                <strong><em class="icon ni ni-location"></em>Bang/Tỉnh:</strong>
                                <asp:Literal ID="ltrQuocGiaBang" runat="server"></asp:Literal>
                            </li>
                        </ul>
                    </div>
                    <div class="card-inner">
                        <div class="overline-title-alt mb-2">Ảnh Cover</div>
                        <div class="anhcover">
                            <div class="anhcover" id="dvPreviewcover" runat="server"></div>
                            <asp:HiddenField ID="hdfCover" runat="server" />
                        </div>
                        <div class="form-control-wrap">
                            <div class="custom-file">
                                <input type="file" class="custom-file-input" id="filecover" runat="server">
                                <label class="custom-file-label" for="<%=filecover.ClientID %>">Chọn Ảnh</label>
                            </div>
                        </div>
                    </div>
                    <!-- .card-inner -->
                    <div class="card-inner card-inner-sm">
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <div class="form-icon form-icon-left">
                                    <em class="icon ni ni-facebook-circle"></em>
                                </div>
                                <asp:TextBox ID="txtfacebook" runat="server" CssClass="form-control" placeholder="Link Facebook"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <div class="form-icon form-icon-left">
                                    <em class="icon ni ni-twitter"></em>
                                </div>
                                <asp:TextBox ID="txttiwtter" runat="server" CssClass="form-control" placeholder="Link twitter"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <div class="form-icon form-icon-left">
                                    <em class="icon ni ni-linkedin"></em>
                                </div>
                                <asp:TextBox ID="txtlinkedin" runat="server" CssClass="form-control" placeholder="Link linkedin"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <div class="form-icon form-icon-left">
                                    <em class="icon ni ni-youtube-round"></em>
                                </div>
                                <asp:TextBox ID="txtyoutube" runat="server" CssClass="form-control" placeholder="Link Youtube"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <div class="form-icon form-icon-left">
                                    <em class="icon ni ni-instagram"></em>
                                </div>
                                <asp:TextBox ID="txtinstagram" runat="server" CssClass="form-control" placeholder="Link instagram"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="form-label" for="default-03">Link Video Ngoài</label>
                            <div class="form-control-wrap">
                                <div class="form-icon form-icon-left">
                                    <em class="icon ni ni-video"></em>
                                </div>
                                <asp:TextBox ID="VideoLink" runat="server" CssClass="form-control" placeholder="Link Videos"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- .card-inner -->

                </div>

            </div>
        </div>
    </div>

    <!-- .card -->
</div>
<script type="text/javascript">
    //Upload Logo
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
        //Cover
        fileUpload1 = document.getElementById('<%=filecover.ClientID%>');
        fileUpload1.onchange = function () {
            if (typeof (FileReader) != "undefined") {
                var dvPreviewcover = document.getElementById('<%=dvPreviewcover.ClientID%>');
                dvPreviewcover.innerHTML = "";
                var regex2 = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp|.svg)$/;
                for (var i = 0; i < fileUpload1.files.length; i++) {
                    var file2 = fileUpload1.files[i];
                    if (regex2.test(file2.name.toLowerCase())) {
                        var reader2 = new FileReader();
                        reader2.onload = function (e) {
                            var img2 = document.createElement("IMG");
                            img2.src = e.target.result;
                            dvPreviewcover.appendChild(img2);
                        }
                        reader2.readAsDataURL(file2);
                    } else {
                        alert(file2.name + " is not a valid image file.");
                        dvPreviewcover.innerHTML = "";
                        return false;
                    }
                }
            } else {
                alert("Trình duyệt của bạn không hỗ trợ Upload");
            }
        }
    };

</script>
