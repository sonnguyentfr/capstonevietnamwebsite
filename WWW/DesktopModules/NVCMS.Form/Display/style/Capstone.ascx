<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Capstone.ascx.vb" Inherits="NVCMS.Modules.Form.Defaultz" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<script src="https://www.google.com/recaptcha/api.js" async defer></script>
    <script>
        // Function to reset reCAPTCHA
        function renderRecaptcha() {
            if (typeof grecaptcha !== 'undefined' && !document.querySelector('.g-recaptcha iframe')) {
                grecaptcha.render('recaptchaContainer', {
                    'sitekey': '6Le4ATsUAAAAAIOMvOYTAf1zAX2-tiwiOciVPgQC'
                });
            }
        }
        // Initialize reCAPTCHA on page load and after partial postbacks
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(renderRecaptcha);
        window.onload = renderRecaptcha;
    </script>
<script src="/static/nvcms/js/validator.js"></script>
<style>
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

            .form-contact div .form-group .form-control {
                padding: 10px 10px 10px 45px;
                color: #202647;
                background-color: #f2f9fc;
                border: 1px solid #f2f9fc;
                font-size: 14px;
                font-weight: 400;
                height: 45px;
                font-family: poppins,sans-serif;
                -webkit-transition: .5s;
                transition: .5s;
                border-radius: 5px;
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
        PADDING: 3PX 7px;
        margin-top: 7px;
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
<div class="row">
    <div class="col-sm-6">
        <asp:UpdatePanel runat="server" ID="paneAJAX">
            <ContentTemplate>
                <div class="title" style="margin: 20px 0px;">
                    <h3>Liên hệ với chúng tôi!</h3>
                </div>
                <div class="form-contact" id="guimailthanhcong" runat="server" visible="false">
                    <div class="row">
                        <div class="col-12">
                            <h5 style="color: #0f66b1; background: #f9a81b61; padding: 10px 20px; text-shadow: 1px 1px #fff; font-weight: 600; font-size: 25px;"><i class="ti-check">Thông tin của bạn đã được gửi thành công!</i></h5>
                        </div>
                    </div>
                </div>
                <div class="form-contact" id="commentForm" runat="server">
                    <div class="row">
                        <div class="col-12">
                            <div class="form-group">
                                <label><i class="fa fa-user" aria-hidden="true"></i></label>
                                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidateStyleCap" placeholder="* Họ và tên"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidateStyleCap" ControlToValidate="txtFullName" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Nhập Họ và tên"></asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label><i class="fa fa-envelope-o" aria-hidden="true"></i></label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidateStyleCap" placeholder="* Địa chỉ Email"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidateStyleCap" ControlToValidate="txtEmail" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Nhập Email"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationGroup="FormContactInputValidateStyleCap" CssClass="NormalRed" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ErrorMessage="Email không đúng"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label><i class="fa fa-phone" aria-hidden="true"></i></label>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidateStyleCap" placeholder="* Số điện thoại"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidateStyleCap" ControlToValidate="txtPhone" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Nhập Số điện thoại"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-12" style="display: none">
                            <div class="form-group">
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-12">
                            <div class="form-group">
                                <asp:TextBox ID="txtcontent" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidateStyleCap" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidateStyleCap" ControlToValidate="txtcontent" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Nhập Nội dung"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="form-group">
                                Capstone cam kết không chia sẻ thông tin của bạn cho bất kỳ bên thứ ba nào.
                            </div>
                            <div class="form-check">
                                <span class="form-check-input">
                                    <input id="dnn_CapstoneVNHome_checkme" type="checkbox" name="dnn$CapstoneVNHome$checkme"></span>
                                <label class="form-check-label" for="dnn_CapstoneVNHome_checkme">
                                    Tôi đồng ý với các  <a href="#">điều khoản </a>và <a href="#">điều kiện bảo mật thông tin</a> của Capstone  
                                </label>
                            </div>
                            <div class="form-check">
                                <span class="form-check-input">
                                    <input id="dnn_CapstoneVNHome_chklienhe" type="checkbox" name="dnn$CapstoneVNHome$chklienhe"></span>
                                <label class="form-check-label" for="dnn_CapstoneVNHome_chklienhe">
                                    Tôi muốn nhận thông tin du học và các ưu đãi cập nhật từ Capstone, vui lòng liên hệ với tôi qua điện thoại, email hoặc SMS.
                                </label>
                            </div>

                        </div>

                    </div>
                    <div class="form-group">
                        <%--<asp:Panel ID="Panel1" runat="server"></asp:Panel>--%>
                        <%--<dnn:CaptchaControl ID="ctlCaptcha" CaptchaLength="5" CaptchaWidth="80" CaptchaHeight="30" CssClass="mabaomat" ErrorStyle-CssClass="dnnFormMessage dnnFormError" runat="server" ViewStateMode="Enabled" />--%>
                        <div id="recaptchaContainer"  class="g-recaptcha" data-sitekey="6Le4ATsUAAAAAIOMvOYTAf1zAX2-tiwiOciVPgQC" data-callback="onRecaptchaSuccess"></div>
                        <asp:Label ID="lblMessage" Font-Bold="true" ForeColor="Red" Font-Size="12px" runat="server" />
                    </div>
                    <div class="form-group">
                        <asp:LinkButton ID="btnSend" ValidationGroup="FormContactInputValidateStyleCap"  runat="server"  Font-Bold="True" CssClass="default-btn">
                                <i class="fa fa-paper-plane" aria-hidden="true"></i> <span>Gửi thông tin</span>
                        </asp:LinkButton>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="paneAJAXa" runat="server">
            <ProgressTemplate>
                <div class="formloding">
                    <asp:Image ImageUrl="/images/loading1.gif" runat="server" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div class="col-sm-6">
        <div class="bgcontact" style="padding: 30px; border: solid 1px #dedede; box-shadow: 3px 3px 6px 2px #bbbbbb; margin-top: 34px;">
            <p><b>Thông tin liên hệ</b></p>
            <p>
                <b>Văn phòng Hà Nội:</b><br>
                2 Lê Quý Đôn, P. Hai Bà Trưng<br>
                Điện thoại: <%=PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai, PortalSettings.Current.PortalId, Null.NullString) %>
            </p>
            <p>
                <b>Văn phòng TP. Hồ Chí Minh:</b>
                <br>
				22 Trần Quý Khoách, P. Tân Định
				
                <br>
                Điện thoại: <%=PortalController.GetPortalSetting(nvcmsBL.settingPagesitedienthoai1, PortalSettings.Current.PortalId, Null.NullString) %>
            </p>
            <p>
                <b>Email: </b>
                <br>
                <a href="mailto:info@capstonevietnam.comn">info@capstonevietnam.com</a>
            </p>
            <p>
                <a href="https://www.facebook.com/CapstoneVN/" target="_blank">
                    <i class="bx bxl-facebook"></i>: https://www.facebook.com/CapstoneVN/
                </a>
            </p>
            <p>
                <a href="https://www.instagram.com/capstonevn/" target="_blank">
                    <i class="bx bxl-instagram"></i>: https://www.instagram.com/capstonevn/
                </a>
            </p>
            <p>
                <b><img src="https://page.widget.zalo.me/static/images/2.0/Logo.svg" alt="" width='15px'></b><a href="https://zalo.me/capstonevietnam" target="_blank"> https://zalo.me/capstonevietnam</a>
            </p>
            <p>
                <a href="https://twitter.com/capstonevn" target="_blank">
                    <i class="bx bxl-twitter"></i>: https://twitter.com/capstonevn
                </a>
            </p>
            <p>
                <a href="https://www.linkedin.com/company/capstonevietnam" target="_blank">
                    <i class="bx bxl-linkedin"></i>: https://www.linkedin.com/company/capstonevietnam
                </a>
            </p>
            <!--<p>
                <b>Hotline:</b><br>
                +84934644268
            </p>-->
            <p>
                <b>Hotline</b><br>
                <%=PortalController.GetPortalSetting(nvcmsBL.settingPagesitewhatsapp, PortalSettings.Current.PortalId, Null.NullString) %> (Zalo/Whatsapp/Wechat/Viber)
            </p>
            <!--<p>
                <b>Skype: </b>
                <br>
                capstonevietnam OR hang.capstone
            </p>
            <p>
                <a href="https://capstone.edu.vn">www.capstone.edu.vn</a>
            </p>-->
        </div>
    </div>
</div>
<script type="text/javascript">
    function isFormValidStyleCap() {
        //var res = true;
        //var $captcha = $('#recaptcha'),
        //    response = grecaptcha.getResponse();
        //if (response.length === 0) {
        //    alert("Bạn vui lòng kiếm tra mã bảo vệ!");
        //    return false;
        //}
        //return res;
        
    }
</script>
