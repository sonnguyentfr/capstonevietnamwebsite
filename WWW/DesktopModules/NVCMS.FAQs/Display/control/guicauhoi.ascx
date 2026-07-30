<%@ Control Language="VB" AutoEventWireup="false" CodeFile="guicauhoi.ascx.vb" Inherits="NVCMS.Modules.FAQs.Defaultz" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<script src="/static/cong1034/js/validations.js"></script>
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
</style>
<asp:UpdatePanel runat="server" ID="paneAJAX">
    <ContentTemplate>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="form-contact" id="guimailthanhcong" runat="server" visible="false">
                    <h5 style="color: #0f66b1; background: #f9a81b61; padding: 10px 20px; text-shadow: 1px 1px #fff; font-weight: 600; font-size: 25px;"><i class="ti-check">Thông tin của bạn đã được gửi thành công!</i></h5>
                </div>
            </div>
        </div>
        <div class="form-contact comment_form guicauhoi" id="commentForm" runat="server">
            <div class="row">
                <div class="col-lg-4 col-md-4">
                    <h6 class="text-center">Thông tin người gửi</h6>
                    <div class="form-group">
                        <input class="form-control" id="txtFullName" runat="server" placeholder="Họ và tên *">
                        <span id="errName" class="error-msg" style="display: none;"><i class="fa fa-ban mr-5"></i>Bạn chưa nhập Họ và tên</span>
                    </div>
                    <div class="form-group">
                        <input class="form-control" id="txttochuc" runat="server" placeholder="Tổ chức, cá nhân *">
                    </div>
                    <div class="form-group">
                        <input class="form-control" id="txtdiachi" runat="server" placeholder="Địa chỉ">
                    </div>
                    <div class="form-group">
                        <input class="form-control" id="txtPhone" runat="server" placeholder="Điện thoại di động *">
                        <span id="errdienthoai" class="error-msg" style="display: none;"><i class="fa fa-ban mr-5"></i>Bạn chưa nhập Điện thoại liên hệ</span>
                    </div>
                    <div class="form-group">
                        <input class="form-control" id="txtEmail" runat="server" placeholder="Email *">
                        <span id="erremail" class="error-msg" style="display: none;"><i class="fa fa-ban mr-5"></i>Bạn chưa nhập Email hoặc Email không đúng!</span>
                    </div>
                </div>
                <div class="col-lg-8 col-md-8">
                    <h6 class="text-center">Nội dung</h6>
                    <div class="form-group">
                        <textarea class="form-control w-100" id="txtcontent" runat="server" cols="30" rows="9" placeholder="Nội dung *"></textarea>
                        <span id="errnoidung" class="error-msg" style="display: none;"><i class="fa fa-ban mr-5"></i>Bạn vui lòng nhập nội dung liên hệ!</span>
                    </div>
                    <div class="form-group">
                        <dnn:CaptchaControl ID="ctlCaptcha" CaptchaLength="3" CaptchaWidth="80" CaptchaHeight="30" CssClass="mabaomat" ErrorStyle-CssClass="dnnFormMessage dnnFormError" runat="server" ViewStateMode="Enabled" />
                        <span id="errCapchat" runat="server" class="error-msg" visible="false"><i class="fa fa-ban"></i>Mã kiểm tra không đúng!</span>
                    </div>
                    <div class="form-group">
                        <asp:Button CssClass="button button-contactForm" ID="btnSend" OnClientClick="return Validate();" runat="server" Text="Gửi câu hỏi" OnClick="btnSend_Click" />
                    </div>
                </div>
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
<script type="text/javascript">

    function Validate() {
        var res = true;
        var txtFullName = document.getElementById('<%=txtFullName.ClientID %>').value;
        if (txtFullName == "") {
            document.getElementById('errName').style.display = 'block';
            $('#<%= txtFullName.ClientID%>').focus();
            return false;
        }
        else {
            document.getElementById('errName').style.display = 'none';
        }
        //=================================
        var txtPhone = document.getElementById('<%=txtPhone.ClientID %>').value;
        if (txtPhone == "") {
            document.getElementById('errdienthoai').style.display = 'block';
            $('#<%= txtPhone.ClientID%>').focus();
            return false;
        }
        else {
            document.getElementById('errdienthoai').style.display = 'none';
        }
        //=================================
        var txtEmail = document.getElementById('<%=txtEmail.ClientID %>').value;
        if (txtEmail == "") {
            document.getElementById('erremail').style.display = 'block';
            $('#<%= txtEmail.ClientID%>').focus();
            return false;
        }
        else {
            document.getElementById('erremail').style.display = 'none';
        }
        //=================================
        if (isEmail(txtEmail) == false) {
            document.getElementById('erremail').style.display = 'block';
            $('#<%= txtEmail.ClientID%>').focus();
            return false;
        }
        else {
            document.getElementById('erremail').style.display = 'none';
        }
        //=================================
        var txtcontent = document.getElementById('<%=txtcontent.ClientID %>').value;
        if (txtcontent == "") {
            document.getElementById('errnoidung').style.display = 'block';
            $('#<%= txtcontent.ClientID%>').focus();
            return false;
        }
        else {
            document.getElementById('errnoidung').style.display = 'none';
        }
        //=================================

        return res;
    }

</script>
