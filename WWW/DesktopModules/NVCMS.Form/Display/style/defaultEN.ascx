<%@ Control Language="VB" AutoEventWireup="false" CodeFile="defaultEN.ascx.vb" Inherits="NVCMS.Modules.Form.Defaultz" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<script src="/static/khoavn/js/validations.js"></script>
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
        <div class="form-contact" id="guimailthanhcong" runat="server" visible="false">
            <div class="row">
                <div class="col-12">
                    <h5 style="color: #0f66b1; background: #f9a81b61; padding: 10px 20px; text-shadow: 1px 1px #fff; font-weight: 600; font-size: 25px;"><i class="ti-check">Thank you!</i></h5>
                </div>
            </div>
        </div>
        <div class="form-contact comment_form" id="commentForm" runat="server">
            <div class="row">
                <div class="col-12">
                    <div class="form-group">
                        <input class="form-control" id="txtFullName" runat="server" placeholder="Fullname *">
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <input class="form-control" id="txtEmail" runat="server" placeholder="Email *">
                    </div>
                </div>
                <div class="col-sm-6">
                    <div class="form-group">
                        <input class="form-control" id="txtPhone" runat="server" placeholder="Phone number *">
                    </div>
                </div>
                <div class="col-sm-12">
                    <div class="form-group">
                        <input class="form-control"  id="txtTitle" runat="server" placeholder="Title *">
                    </div>
                </div>
                <div class="col-12">
                    <div class="form-group">
                        <textarea class="form-control w-100" id="txtcontent" runat="server" cols="30" rows="9" placeholder="Content *"></textarea>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <dnn:CaptchaControl ID="ctlCaptcha" CaptchaLength="3" CaptchaWidth="100" CaptchaHeight="30" CssClass="mabaomat" ErrorStyle-CssClass="dnnFormMessage dnnFormError" runat="server" ViewStateMode="Enabled" />
            </div>
            <div class="form-group">
                <asp:Button CssClass="ttm-btn ttm-btn-size-md ttm-btn-shape-square ttm-btn-style-border ttm-btn-color-dark mb-15" ID="btnSend" OnClientClick="return Validate();" runat="server" Text="Send" OnClick="btnSend_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress id="paneAJAXa" runat="server">
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
        if (txtFullName == '') {
            alert('Input fullname');
            $('#<%= txtFullName.ClientID%>').focus();
            return false;
        }
        var txtEmail = document.getElementById('<%=txtEmail.ClientID %>').value;
        if (txtEmail == '') {
            alert('Input email.');
            $('#<%= txtEmail.ClientID%>').focus();
            return false;
        }
        if (isEmail(txtEmail) == false) {
            alert('Please check email');
            $('#<%= txtEmail.ClientID%>').focus();
            return false;
        }
        var txtTitle = document.getElementById('<%=txtTitle.ClientID %>').value;
        if (txtTitle == '') {
            alert('Input Title.');
            $('#<%= txtTitle.ClientID%>').focus();
            return false;
        }
        var txtcontent = document.getElementById('<%=txtcontent.ClientID %>').value;
        if (txtcontent == '') {
            alert('Input Content.');
            $('#<%= txtcontent.ClientID%>').focus();
            return false;
        }

        return res;
    }

</script>
