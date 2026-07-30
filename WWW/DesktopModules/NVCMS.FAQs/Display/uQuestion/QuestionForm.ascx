<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="QuestionForm.ascx.vb" Inherits="NVCMS.Modules.FAQs.inc_edit" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<asp:UpdatePanel ID="up" runat="server">
    <ContentTemplate>
        <div id="writecomment" class="accordion writecomment">
            <p class="comment-info">
                <i class="fa fa-info"></i>
                
                <strong><%=Localization.GetString("titlenote.text", resourceform) %></strong>
                <span><a href="#" id='hideshow'><%=Localization.GetString("datcauhoi.text", resourceform) %></a></span>
            </p>
            <div class="coloralert" style="background: #CA2E1A;" id="alerterror" runat="server" visible="false">
                <i class="fa fa-warning"></i>
                <p>
                    <%=Localization.GetString("khongthanhcong.text", resourceform) %>
                    
                </p>
                <a href="#close-alert"><i class="fa fa-times-circle"></i></a>
            </div>
            <div id="alert" class="coloralert" style="background: #68a117; display: none;" runat="server">
                <i class="fa fa-check"></i>
                <p>
                    <%=Localization.GetString("thanhcong.text", resourceform) %>
                    
                </p>
                <a href="#close-alert"><i class="fa fa-times-circle"></i></a>
            </div>
            <span id="content" style=" width: 100%;" runat="server">
                <p class="contact-form-user">
                    <span><%=Localization.GetString("hoten.text", resourceform) %> <font style="color: red">(*)</font></span>
                    <asp:TextBox runat="server" ID="txtName" EnableViewState="True"></asp:TextBox>
                    <span id="errName" class="error-msg" style="display: none;"><i class="fa fa-ban"></i><%=Localization.GetString("errorhoten.text", resourceform) %></span>
                </p>
                <p class="contact-form-email">
                    <span><%=Localization.GetString("email.text", resourceform) %> <font style="color: red">(*)</font></span>
                    <asp:TextBox runat="server" ID="txtEmail" EnableViewState="True"></asp:TextBox>
                    <span id="erremail" class="error-msg" style="display: none;"><i class="fa fa-ban"></i><%=Localization.GetString("erroremail.text", resourceform) %></span>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" SetFocusOnError="true" ValidationGroup="fr" Display="Dynamic" CssClass="error-msg"
                        Text='<%=Localization.GetString("erroremail.text", resourceform) %>' ValidationExpression="^([0-9a-zA-Z]+[\.]{1})*[0-9a-zA-Z]+@[0-9a-zA-Z]+[\.]{1}[0-9a-zA-Z]+[\.]?[0-9a-zA-Z]+$" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                </p>
                <p class="contact-form-message">
                    <span><%=Localization.GetString("question.text", resourceform) %> <font style="color: red">(*)</font></span>
                    <asp:TextBox runat="server" ID="txtConent" TextMode="MultiLine" Height="80px" EnableViewState="True"></asp:TextBox>
                    <span id="errContent" class="error-msg" style="display: none;"><i class="fa fa-ban"></i><%=Localization.GetString("errornoidung.text", resourceform) %></span>
                </p>
                <p>
                    <dnn:CaptchaControl ID="ctlCaptcha" CaptchaLength="3" CaptchaWidth="80" CaptchaHeight="30" CssClass="Normal" ErrorStyle-CssClass="dnnFormMessage dnnFormError" runat="server" ViewStateMode="Enabled" />
                    <span id="errCapchat" runat="server" class="error-msg" visible="false"><i class="fa fa-ban"></i>Mã kiểm tra không đúng!</span>
                </p>
                <p>
                    <asp:LinkButton ID="lbtUpdate" runat="server" class="button" OnClientClick="return isFormValid();" ValidationGroup="Comment" EnableViewState="True">
                            <%=Localization.GetString("submit.text", resourceform) %>
                    </asp:LinkButton>
                </p>
            </span>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script>
    jQuery(document).ready(function () {
        jQuery('#hideshow').live('click', function (event) {
            jQuery(document.getElementById('<%=content.ClientID%>')).toggle('show');
        });
    });
</script>
<script>
    function isFormValid() {
        var Name = document.getElementById('<%=txtName.ClientID%>').value;
        if (Name == "") {
            document.getElementById('errName').style.display = 'block';
            return false;
        }
        else {
            document.getElementById('errName').style.display = 'none';
        }
        //--------------Kiem tra email
        var Email = document.getElementById('<%=txtEmail.ClientID%>').value;
        if (Email == "") {
            document.getElementById('erremail').style.display = 'block';
            return false;
        }
        else {
            document.getElementById('erremail').style.display = 'none';
        }

        //--------------Kiem tra email
        var Content = document.getElementById('<%=txtConent.ClientID%>').value;
        if (Content == "") {
            document.getElementById('errContent').style.display = 'block';
            return false;
        }
        else {
            document.getElementById('errContent').style.display = 'none';
        }
        document.getElementById('alert').style.display = 'block';
        document.getElementById('content').style.display = 'none';
        //document.getElementById('alertinfo').style.display = 'none';
        return false;
    }

</script>
