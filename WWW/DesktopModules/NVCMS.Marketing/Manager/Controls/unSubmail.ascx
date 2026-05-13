<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="unSubmail.ascx.vb" Inherits="NVCMS.Modules.Marketing.unSubMail" %>
<style type="text/css">
    .news-details {
        padding: 1em;
        border: 1em solid transparent;
        background: linear-gradient(white, white) padding-box, repeating-linear-gradient(-45deg, red 0, red 12.5%, transparent 0, transparent 25%, #58a 0, #58a 37.5%, transparent 0, transparent 50%) 0 / 5em 5em;
    }

        .news-details table tr td {
            padding: 5px;
        }

            .news-details table tr td label {
                padding-left: 10px;
            }

    strong {
        font-weight: 600
    }

    p {
        margin-block: 6px;
    }

    h3 {
        margin-block: 20px;
    }
</style>

<asp:UpdatePanel ID="updatepanelusub" runat="server">
    <ContentTemplate>
        <div class="news-details" id="form" runat="server">
            <h3><asp:Literal ID="ltremail" runat="server"></asp:Literal>   Unsubscribe (Huỷ đăng ký)</h3>
            <p>You will no longer receive email marketing from this list. (Từ nay, quý khách sẽ không còn nhận được các email tiếp thị từ danh sách này.)</p>
            <p><strong>If you have a moment, please let us know why you unsubscribed (Nếu quý khách có một chút thời gian, xin vui lòng cho chúng tôi biết lý do tại sao quý khách hủy đăng ký):</strong></p>
            <asp:RadioButtonList ID="rdblistreason" runat="server">
                <asp:ListItem Value="1" Text=" I no longer want to receive these emails / Tôi không còn muốn nhận các email này"></asp:ListItem>
                <asp:ListItem Value="2" Text=" I never signed up for this mailing list / Tôi chưa bao giờ đăng ký nhận danh sách gửi thư này "></asp:ListItem>
                <asp:ListItem Value="3" Text=" The emails are inappropriate / Các email không phù hợp"></asp:ListItem>
                <%--<asp:ListItem Value="4" Text=" The emails are spam and should be reported / Các email là thư rác và nên được báo cáo"></asp:ListItem>--%>
                <asp:ListItem Value="5" Text=" Other/ Lý do khác"></asp:ListItem>
            </asp:RadioButtonList>
            <%--<p><strong>Unsubscribing from list. (Quý khách đang hủy đăng ký khỏi danh sách.)</strong></p>
            <p>
                By clicking Unsubscribe, you’ll no longer receive marketing emails. (Bằng cách nhấp vào "Hủy đăng ký", quý khách sẽ không còn nhận được các email tiếp thị)
            </p>--%>
            <p>
                <asp:Label ID="lblerror" runat="server" Font-Bold="true" ForeColor="red"></asp:Label></p>
				<asp:LinkButton ID="lblunscu" runat="server" CssClass="default-btn"><i class="fa fa-paper-plane" aria-hidden="true" OnClientClick="return validateUnsubscribeForm()"></i> &nbsp;&nbsp;&nbsp;Bỏ đăng ký nhận mail</asp:LinkButton>
        </div>
        <div class="news-details" id="thanhcong" runat="server" visible="false">
            <h2 style="color:red; font-weight:600">Unsubscribe Successful (Hủy đăng ký thành công)</h2>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div style="top: 0; left: 0; width: 100vw; height: 100vh; padding: 20% 45%; background: #00000030; position: fixed;">
            <div class="spinner-border text-danger" role="status" style="width: 10rem !important; height: 10rem !important;">
                <span class="sr-only">Loading...</span>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<script>
    function validateUnsubscribeForm() {
        const reasonList = document.querySelector('input[name="rdblistreason"]:checked');
        if (!reasonList) {
            alert("Please select a reason for unsubscribing.");
            return false; // Prevents form submission
        }
        return true; // Allows form submission
    }
</script>
