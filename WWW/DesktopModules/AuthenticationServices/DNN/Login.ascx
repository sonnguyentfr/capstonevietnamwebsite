<%@ Control Language="C#" Inherits="DotNetNuke.Modules.Admin.Authentication.DNN.Login" AutoEventWireup="false" CodeBehind="Login.ascx.cs" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.UI.WebControls.Internal" Assembly="DotNetNuke.Web" %>
<style type="text/css">
    .form-check-input {display: ruby;}
</style>
<div class="card card-bordered">
    <div class="card-inner card-inner-lg">
        <div class="nk-block-head">
            <div class="nk-block-head-content">
                <h4 class="nk-block-title">Đăng nhập</h4>
                <div class="nk-block-des">
                    <p>Nhập thông tin tài khoản - mật khẩu.</p>
                </div>
            </div>
        </div>
        <div>
            <div class="form-group">
                <div class="form-label-group">
                    <label class="form-label" for="default-01">Tài khoản</label>
                </div>
                <asp:TextBox ID="txtUsername" runat="server" placeholder="Tài khoản" CssClass="form-control form-control-lg" />
            </div>
            <div class="form-group">
                <div class="form-label-group">
                    <label class="form-label" for="password">Mật khẩu</label>
                </div>
                <div class="form-control-wrap">
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" placeholder="Mật khẩu" CssClass="form-control form-control-lg" />
                </div>
            </div>
            <div class="form-group">
                <div class="form-label-group">
                    <label class="form-label" for="password">Con Số May Mắn</label>
                </div>
                <div class="form-control-wrap">
                    <asp:TextBox ID="txtsomayman" runat="server" placeholder="Nhập con số may mắn vào đây" autocomplete="off" CssClass="form-control form-control-lg" />
                </div>
            </div>
            <div class="form-group">
                <div class="form-label-group" style="margin: 10px;padding: 7px;">
                    <asp:CheckBox ID="chkCookie" resourcekey="Remember" runat="server" CssClass="form-check-input" />
                </div>
            </div>
            <div class="form-group" id="divCaptcha2" runat="server" visible="false">
                <dnn:CaptchaControl ID="CaptchaControl1" CaptchaWidth="130" CaptchaHeight="40" runat="server" ErrorStyle-CssClass="dnnFormMessage dnnFormError dnnCaptcha" ViewStateMode="Disabled" />
            </div>
            <div class="form-group">
                <asp:LinkButton ID="cmdLogin" resourcekey="cmdLogin" CssClass="btn btn-lg btn-primary btn-block" Text="Đăng nhập" runat="server" CausesValidation="false" />
            </div>
             <div class="form-group">
                <asp:HyperLink ID="passwordLink" runat="server" CssClass="link link-primary link-sm" resourcekey="cmdPassword" ViewStateMode="Disabled" />
            </div>
        </div>
        <div class="form-note-s2 text-center pt-4">
            <asp:HyperLink ID="registerLink" runat="server" CssClass="" resourcekey="cmdRegister" ViewStateMode="Disabled" Visible="false" />
        </div>

    </div>
</div>
<div class="dnnForm dnnLoginService dnnClear" id="cu" runat="server" visible="false">
    <div class="dnnFormItem">
        <div class="dnnLabel">
            <asp:Label ID="plUsername" AssociatedControlID="txtUsername" runat="server" CssClass="dnnFormLabel" />
        </div>

    </div>
    <div class="dnnFormItem">
        <div class="dnnLabel">
            <asp:Label ID="plPassword" AssociatedControlID="txtPassword" runat="server" resourcekey="Password" CssClass="dnnFormLabel" ViewStateMode="Disabled" />
        </div>

    </div>
    <div class="dnnFormItem" id="divCaptcha1" runat="server" visible="false">
        <asp:Label ID="plCaptcha" AssociatedControlID="ctlCaptcha" runat="server" resourcekey="Captcha" CssClass="dnnFormLabel" />
    </div>
    <div class="dnnFormItem dnnCaptcha" id="divCaptcha2z" runat="server" visible="false">
        <dnn:CaptchaControl ID="ctlCaptcha" CaptchaWidth="130" CaptchaHeight="40" runat="server" ErrorStyle-CssClass="dnnFormMessage dnnFormError dnnCaptcha" ViewStateMode="Disabled" />
    </div>
    <div class="dnnFormItem">
        <asp:Label ID="lblLoginRememberMe" runat="server" CssClass="dnnFormLabel" />
        <span class="dnnLoginRememberMe"></span>
    </div>
    <div class="dnnFormItem">
        <asp:Label ID="lblLogin" runat="server" AssociatedControlID="cmdLogin" CssClass="dnnFormLabel" ViewStateMode="Disabled" />

        <asp:HyperLink ID="cancelLink" runat="server" CssClass="dnnSecondaryAction" resourcekey="cmdCancel" CausesValidation="false" />
    </div>
    <div class="dnnFormItem">
        <span class="dnnFormLabel">&nbsp;</span>
        <div class="dnnLoginActions">
            <ul class="dnnActions dnnClear">
            </ul>
        </div>
    </div>
</div>
<dnn:DnnScriptBlock runat="server">
    <script type="text/javascript">
        /*globals jQuery, window, Sys */
        (function ($, Sys) {
            const disabledActionClass = "dnnDisabledAction";
            const actionLinks = $('a[id^="dnn_ctr<%=ModuleId > Null.NullInteger ? ModuleId.ToString() : ""%>_Login_Login_DNN"]');
            function isActionDisabled($el) {
                return $el && $el.hasClass(disabledActionClass);
            }
            function disableAction($el) {
                if ($el == null || $el.hasClass(disabledActionClass)) {
                    return;
                }
                $el.addClass(disabledActionClass);
            }
            function enableAction($el) {
                if ($el == null) {
                    return;
                }
                $el.removeClass(disabledActionClass);
            }
            function setUpLogin() {
                $.each(actionLinks || [], function (index, action) {
                    var $action = $(action);
                    $action.click(function () {
                        var $el = $(this);
                        if (isActionDisabled($el)) {
                            return false;
                        }
                        disableAction($el);
                    });
                });
            }

            $(document).ready(function () {
                $(document).on('keydown', '.form-group', function (e) {
                    if ($(e.target).is('input:text,input:password') && e.keyCode === 13) {
                        var $loginButton = $('#dnn_ctr<%=ModuleId > Null.NullInteger ? ModuleId.ToString() : ""%>_Login_Login_DNN_cmdLogin');
                        var username = document.getElementById('<%=txtUsername.ClientID%>').value;
                        if (username == "") {
                            alert("Bạn chưa nhập tên tài khoản đăng nhập");
                            document.getElementById('<%=txtUsername.ClientID%>').focus();
                            return false;
                        }
                        var usernamepass = document.getElementById('<%=txtPassword.ClientID%>').value;
                        if (usernamepass == "") {
                            alert("Bạn chưa nhập mật khẩu");
                            document.getElementById('<%=txtPassword.ClientID%>').focus();
                            return false;
                        }
                        if (isActionDisabled($loginButton)) {
                            return false;
                        }
                        disableAction($loginButton);
                        window.setTimeout(function () { eval($loginButton.attr('href')); }, 100);
                        e.preventDefault();
                        return false;
                    }
                });
                setUpLogin();
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                    $.each(actionLinks || [], function (index, item) {
                        enableAction($(item));
                    });
                    setUpLogin();
                });
            });
        }(jQuery, window.Sys));
    </script>
</dnn:DnnScriptBlock>
