<%@ Control Language="VB" AutoEventWireup="false" CodeFile="CapstoneHomeTuVan.ascx.vb" Inherits="NVCMS.Modules.Form.CapstoneHomeTuVan" %>
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

            .form-contact div .form-group select.form-control {
                padding: 5px 10px 5px 15px;
            }

            .form-contact div .form-group .form-control {
                padding: 5px 10px 5px 45px;
                color: #202647;
                background-color: #f2f9fc;
                border: 1px solid #f2f9fc;
                font-size: 13px;
                font-weight: 400;
                height: 35px;
                -webkit-transition: .5s;
                transition: .5s;
                border-radius: 5px;
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
<div class="form-contact">
    <div class="content">
        <h3>Đăng ký tư vấn</h3>
    </div>
    <div class="content">
        <asp:UpdatePanel runat="server" ID="paneAJAXFormTuVanHome">
            <ContentTemplate>
                <div class="row" id="guimailthanhcong" runat="server" visible="false">
                    <div class="col-lg-12 col-sm-12">
                        <h5 style="color: #0f66b1; background: #f9a81b61; padding: 10px 20px; text-shadow: 1px 1px #fff; font-weight: 600; font-size: 25px;"><i class="ti-check"></i>Thông tin của bạn đã được gửi thành công!</h5>
                    </div>
                </div>
                <div class="row" id="commentForm" runat="server">
                    <div class="col-lg-12 col-sm-12">
                        <div class="form-group">
                            <asp:DropDownList ID="ddlHinhthuc" runat="server" CssClass="form-control">
                                <asp:ListItem Value="0" Text="--Chọn hình thức tư vấn--"></asp:ListItem>
                                <asp:ListItem Value="TUVANDUHOC" Text="Tư vấn du học"></asp:ListItem>
                                <asp:ListItem Value="DINHHUONGNGHENGHIEP" Text="Định hướng nghề nghiệp"></asp:ListItem>
                                <asp:ListItem Value="DINHCU" Text="Định cư"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12 col-sm-12">
                        <div class="form-group">
                            <asp:DropDownList ID="ddlvanphong" runat="server" CssClass="form-control">
                                <asp:ListItem Value="KO" Text="--Chọn Văn Phòng Tư vấn --"></asp:ListItem>
                                <asp:ListItem Value="HN" Text="Hà nội"></asp:ListItem>
                                <asp:ListItem Value="HCM" Text="Hồ Chí Minh"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-lg-12 col-sm-12">
                        <div class="form-group">
                            <label><i class="fa fa-user" aria-hidden="true"></i></label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidate" placeholder="* Họ và tên"></asp:TextBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidate" ControlToValidate="txtFullName" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Nhập Họ và tên"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-5 col-sm-5">
                        <div class="form-group">
                            <label><i class="fa fa-phone" aria-hidden="true"></i></label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidate" placeholder="* Số điện thoại"></asp:TextBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidate" ControlToValidate="txtPhone" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Nhập Số điện thoại"></asp:RequiredFieldValidator>

                        </div>
                    </div>
                    <div class="col-lg-7 col-sm-7">
                        <div class="form-group">
                            <label><i class="fa fa-envelope" aria-hidden="true"></i></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidate" placeholder="* Địa chỉ Email"></asp:TextBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidate" ControlToValidate="txtEmail" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Nhập Email"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" ValidationGroup="FormContactInputValidate" CssClass="NormalRed" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ErrorMessage="Email không đúng"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 col-md-12">
                        <div class="form-group">
                            <asp:TextBox ID="txtcontent" runat="server" CssClass="form-control" ValidationGroup="FormContactInputValidate" TextMode="MultiLine" Height="100px"></asp:TextBox>
                            <asp:RequiredFieldValidator ValidationGroup="FormContactInputValidate" ControlToValidate="txtcontent" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Nhập Nội dung"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div class="form-group notes">
                            Capstone cam kết không chia sẻ thông tin của bạn cho bất kỳ bên thứ ba nào.
                       
                        </div>
                        <div class="form-check">
                            <span class="form-check-input">
                                <input id="dnn_CapstoneVNHome_checkme" type="checkbox" name="dnn$CapstoneVNHome$checkme"></span>
                            <label class="form-check-label" for="dnn_CapstoneVNHome_checkme">
                                Tôi đồng ý với các  <a href="terms-of-service.html">điều khoản </a>và <a href="privacy-policy.html">điều kiện bảo mật thông tin</a> của Capstone  
                           
                            </label>
                        </div>
                        <div class="form-check">
                            <span class="form-check-input">
                                <input id="dnn_CapstoneVNHome_chklienhe" type="checkbox" name="dnn$CapstoneVNHome$chklienhe"></span>
                            <label class="form-check-label" for="dnn_CapstoneVNHome_chklienhe">
                                Tôi muốn nhận thông tin du học và các ưu đãi cập nhật từ Capstone, vui lòng liên hệ với tôi qua điện thoại, email hoặc SMS.
                           
                            </label>
                        </div>
                        <div class="form-group">
                            <div id="recaptchaContainer"  class="g-recaptcha" data-sitekey="6Le4ATsUAAAAAIOMvOYTAf1zAX2-tiwiOciVPgQC" data-callback="onRecaptchaSuccess"></div>
                            <asp:Label ID="lblMessage" Font-Bold="true" ForeColor="Red" Font-Size="12px" runat="server" />
                            <%--<asp:Panel ID="Panel1" runat="server"></asp:Panel>--%>
                            <%--<dnn:CaptchaControl ID="ctlCaptcha" CaptchaLength="5" CaptchaWidth="80" CaptchaHeight="30" CssClass="mabaomat" ErrorStyle-CssClass="dnnFormMessage dnnFormError" runat="server" ViewStateMode="Enabled" />--%>
                        </div>
                    </div>
                    <div class="col-lg-12">
                        <div class="queries-btn">
                            <asp:LinkButton ID="btnSend" ValidationGroup="FormContactInputValidate" runat="server" Font-Bold="True" CssClass="default-btn" OnClientClick="return isFormValidTuVan();">
                                <i class="fa fa-paper-plane" aria-hidden="true"></i> <span>Gửi thông tin</span>
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
                
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSend" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="paneAJAXa" runat="server">
            <ProgressTemplate>
                <div class="formloding">
                    <asp:Image ImageUrl="/images/loading1.gif" runat="server" />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
</div>

<script type="text/javascript">
    //function isFormValidTuVan() {
    //    var res = true;
    //    var $captcha = $('#recaptcha'),
    //        response = grecaptcha.getResponse();
    //    if (response.length === 0) {
    //        alert("Bạn vui lòng kiếm tra mã bảo vệ!");
    //        res = false;
    //    }
    //   return res;
    //}
</script>

