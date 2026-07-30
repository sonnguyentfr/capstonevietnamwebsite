<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Main.ascx.vb" Inherits="NVCMS.Modules.FormLandingPage.MainCustomeDisplay" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<style type="text/css">
    .ladingformlayout {
        margin: 0 auto;
        position: relative;
        padding: 100px;
    }

        .ladingformlayout .ladi-section-background {
            background-size: cover;
            background-origin: content-box;
            background-position: 50% 0%;
            background-repeat: repeat;
            background-attachment: scroll;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            pointer-events: none;
            overflow: hidden;
        }

        .ladingformlayout .row {
            position: relative;
            height: 100%;
        }

    .queries-area {
        display: none !important
    }

    .formloding {
        text-align: center;
        padding: 10px;
        position: absolute;
        height: 100%;
        width: 100%;
        top: 40%;
    }

    .comment_form {
        position: relative !important;
    }

    .form-contact {
        background-color: #fff;
        -webkit-box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
        box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
        padding: 50px 40px;
        border-radius: 5px;
        border: 1px solid #eee;
        position: relative;
    }

    .form-contact-content {
        background-color: #fff;
        -webkit-box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
        box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
        padding: 40px 30px;
        border-radius: 5px;
        border: 1px solid #eee;
        position: relative;
    }

        .form-contact-content p {
            font-size: 14px;
            padding-bottom: 5px;
            font-family: Roboto Condensed;
            line-height: 19px;
        }

    .form-contact .content {
        text-align: center;
    }

        .form-contact .content h3 {
            font-size: 20px;
            font-weight: 600;
            margin-bottom: 20px;
        }

    .form-contact div .form-group {
        position: relative;
        margin-bottom: 15px;
    }

        .form-contact div .form-group label {
            z-index: 1;
            display: block;
            margin-bottom: 0;
            position: absolute;
            left: 15px;
            color: #107cbe;
            font-size: 22px;
            top: 50%;
            -webkit-transform: translateY(-50%);
            transform: translateY(-50%);
        }

        .form-contact div .form-group select.form-control {
            padding: 8px 10px 8px 45px;
            font-family: Roboto Condensed;
        }

    .form-contact .editModule {
        position: absolute;
        top: 40px;
        right: 40px;
        color: #fff;
        background: #e35f5f;
        padding: 5px;
        border-radius: 14%;
    }

    .form-contact div .form-group .form-control {
        padding: 15px 10px 15px 45px;
        color: #202647;
        background-color: #f2f9fc;
        border: 1px solid #f2f9fc;
        font-size: 14px;
        font-weight: 400;
        height: 40px;
        -webkit-transition: .5s;
        transition: .5s;
        border-radius: 5px;
        font-family: Roboto Condensed;
    }

    .form-contact div .form-group.notes,
    .form-contact div .form-check label {
        font-size: 12px;
        font-style: italic;
    }

    .form-control {
        display: block;
        width: 100%;
        padding: 0.375rem 0.75rem;
        font-size: 1rem;
        font-weight: 400;
        line-height: 1.5;
        color: #212529;
        background-color: #fff;
        background-clip: padding-box;
        border: 1px solid #ced4da;
        -webkit-appearance: none;
        -moz-appearance: none;
        appearance: none;
        border-radius: 0.25rem;
        transition: border-color .15s ease-in-out,box-shadow .15s ease-in-out;
    }

    .form-group span.NormalRed {
        font-weight: normal;
        color: red;
        background: #ffff3752;
        font-size: 12px;
        padding: 3PX 7px;
        margin-top: 7px;
        display: none;
    }

    .form-contact h3 {
        font-size: 40px;
    font-family: 'Roboto Condensed';
    color: red;
    text-align: center;
    padding: 50px;
    }

    .mabaomat {
        float: left;
        width: 100%;
    }

        .mabaomat img {
            width: 90px;
            height: 40px;
            float: left;
            margin-right: 5px;
        }

        .mabaomat div {
            display: none;
        }

        .mabaomat input {
            width: 90px !important;
            font-size: 20px !important;
            height: 40px !important;
            float: left;
            margin-left: 10px;
            border: solid 1px #a54b4b;
            background: #efefef;
            color: #000;
            font-weight: 600;
        }

    .default-btn {
        display: inline-block;
        padding: 10px 35px;
        background-color: #107cbe;
        color: #fff;
        border-radius: 10px;
        -webkit-transition: .5s;
        transition: .5s;
        font-weight: 500;
        width: 100%;
        border: 0px;
        margin: 10px 0px;
    }
</style>
<div class="ladingformlayout" id="SECTION11">
    <div class="ladi-section-background">&nbsp;</div>
    <asp:Literal ID="ltrbackground" runat="server"></asp:Literal>
    <div class="container">
        <div class="row">
            <div class="col-lg-1 col-sm-12"></div>
            <div class="col-lg-4 col-sm-12">
                <div class="form-contact-content">
                    <asp:Literal ID="ltrnoidung" runat="server"></asp:Literal>
                </div>
            </div>

            <div class="col-lg-5 col-sm-12">
                <asp:UpdatePanel ID="udpform" runat="server">
                    <ContentTemplate>
                        <div class="form-contact" id="formreg" runat="server">
                            <asp:HyperLink ID="hplEditMoudle" Target="_blank" CssClass="editModule" runat="server" Visible="false"><i class="fa fa-pencil" aria-hidden="true"></i> Sửa</asp:HyperLink>
                            <asp:Literal ID="ltrtitle" runat="server"></asp:Literal>
                            <div class="content">
                                <div class="form-group" id="type" runat="server" visible="false">
                                    <label><i class="fa fa-map-user" aria-hidden="true"></i></label>
                                    <asp:DropDownList CssClass="form-group" ID="ddlType" runat="server">
                                        <asp:ListItem Text="Chọn vai trò" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Học Sinh / Sinh Viên/...(Student)" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Phụ Huynh (Parents)" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group" id="hovaten" runat="server" visible="false">
                                    <label><i class="fa fa-user" aria-hidden="true"></i></label>
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidate" placeholder="* Họ và tên"></asp:TextBox>
                                </div>
                                <div class="form-group" id="sodienthoai" runat="server" visible="false">
                                    <label><i class="fa fa-phone" aria-hidden="true"></i></label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidate" placeholder="* Số điện thoại"></asp:TextBox>

                                </div>
                                <div class="form-group" id="email" runat="server" visible="false">
                                    <label><i class="fa fa-envelope" aria-hidden="true"></i></label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidate" placeholder="* Địa chỉ Email"></asp:TextBox>
                                </div>
                                <div class="form-group" id="ngaysinh" runat="server" visible="false">
                                    <label><i class="fa fa-calendar" aria-hidden="true"></i></label>
                                    <asp:TextBox ID="txtngaysinh" runat="server" CssClass="form-control datepicker" placeholder="* Ngày sinh"></asp:TextBox>

                                </div>
                                <div class="form-group" id="diachitinh" runat="server" visible="false">
                                    <label><i class="fa fa-map-marker" aria-hidden="true"></i></label>
                                    <asp:DropDownList ID="ddldiachitinh" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group" id="diadiem" runat="server" visible="false">
                                    <asp:DropDownList ID="ddldiadiem" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="form-group" id="ebfive" runat="server" visible="false">
                                    <div class="form-check">
                                        <span class="form-check-input">
                                            <input id="chkebfive" type="checkbox" runat="server"></span>
                                        <label class="form-check-label" for="<%=chkebfive.ClientID %>">Quan tâm Eb5</label>
                                    </div>
                                </div>
                                <div class="form-group" id="yeucautuvan" runat="server" visible="false">
                                    <div class="form-check">
                                        <label><i class="fa fa-pencil" aria-hidden="true"></i></label>
                                        <asp:TextBox ID="txtyeucautuvan" TextMode="MultiLine" runat="server" CssClass="form-control" placeholder="Yêu cầu tư vấn thêm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <%--<asp:Panel ID="Panel1" runat="server"></asp:Panel>--%>
                                    <dnn:CaptchaControl ID="ctlCaptcha" CaptchaLength="3" CaptchaWidth="100" CaptchaHeight="30" CssClass="mabaomat" ErrorStyle-CssClass="dnnFormMessage dnnFormError" runat="server" ViewStateMode="Enabled" />
                                </div>
                                <div class="queries-btn">
                                    <asp:LinkButton ID="btnSend" runat="server" ValidationGroup="FormContactInputValidate" Font-Bold="True" CssClass="default-btn" OnClientClick="return isFormValidTuVan();">
                                <i class="fa fa-paper-plane" aria-hidden="true"></i> <span>Đăng ký</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                        <div class="form-contact" id="foregsuc" runat="server">
                            <h3>Đăng ký thành công</h3>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
                    <ProgressTemplate>
                        <div class="loading" id="loadizng">Loading&#8230;</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="col-lg-2 col-sm-12"></div>

        </div>
    </div>
</div>
<link rel="stylesheet" type="text/css" href="/static/Landing/css/jquery.datetimepicker.min.css" />
<script src='/static/capstonev3/js/vendor/jquery-1.12.4.min.js'></script>
<script src="/static/Landing/js/jquery.datetimepicker.js"></script>

<script type="text/javascript">
    function isFormValidTuVan() {
        //var res = true;
        //var $captcha = $('#recaptcha'),
        //    response = grecaptcha.getResponse();
        //if (response.length === 0) {
        //    alert("Bạn vui lòng kiếm tra mã bảo vệ!");
        //    res = false;
        //}
        //return res;
    }
    $('.datepicker').datetimepicker({
        format: 'd/m/Y',
        timepicker: false
    }); $(".datepicker").keydown(false);
</script>
