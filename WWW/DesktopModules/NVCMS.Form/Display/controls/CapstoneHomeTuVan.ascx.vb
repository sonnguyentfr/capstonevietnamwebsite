Imports System.IO
Imports System.Net
Imports DotNetNuke.UI.Utilities
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Namespace NVCMS.Modules.Form
    Public MustInherit Class CapstoneHomeTuVan
        Inherits Entities.Modules.PortalModuleBase

        Dim ctlform As New Form_Controller
        Dim settingmail As String = ""
        'Dim settingnhanmail As String = PortalController.GetPortalSetting(nvcmsBL.settingPagesiteemail, PortalSettings.Current.PortalId, Null.NullString)
        Dim settingmailtitle As String = ""
        Dim isCaptchaValid As Boolean
        Private Const SecretKey As String = "6Le4ATsUAAAAAFdNwBf9pcSldyKbNm42cYdV8uLc"
        'Private ctrlGoogleReCaptcha As New GoogleReCaptcha.GoogleReCaptcha()
        'Protected Overrides Sub CreateChildControls()
        '    MyBase.CreateChildControls()
        '    ctrlGoogleReCaptcha.PublicKey = PortalController.GetPortalSetting(nvcmsBL.settingPageGooogleCapcha, PortalSettings.Current.PortalId, Null.NullString)
        '    ctrlGoogleReCaptcha.PrivateKey = PortalController.GetPortalSetting(nvcmsBL.settingPageGooogleCapchaSecret, PortalSettings.Current.PortalId, Null.NullString)
        '    Me.Panel1.Controls.Add(ctrlGoogleReCaptcha)
        'End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(Me.Parent, Me.btnSend, Asc(vbCr))
            If Not IsPostBack Then
                Try
                    lblMessage.Text = ""
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
        Private Sub SendMail(strFrom As String, strTo As String, strCC As String, strBCC As String, strSubject As String, strBody As String)
            Dim strSMTP As String = "smtp.gmail.com:587"
            DotNetNuke.Services.Mail.Mail.SendMail(strFrom, strTo, strCC, strBCC, DotNetNuke.Services.Mail.MailPriority.High, strSubject, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, strBody, "", strSMTP, "1", "no-reply@capstonevietnam.com", "whepagxgukpyhaav", True)
        End Sub
        Public Sub Clearform()
            'Me.txtTitle.Text = ""
            Me.txtFullName.Text = ""
            Me.txtPhone.Text = ""
            Me.txtEmail.Text = ""
            Me.txtcontent.Text = ""
        End Sub
        Protected Sub btnSend_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSend.Click
            Try
                'Lay du lieu
                Dim title = ddlHinhthuc.SelectedIndex.ToString()
                Dim noidung As String = Ultis.ConvertStringNonAttact(txtcontent.Text)
                Dim hovaten As String = Ultis.ConvertStringNonAttact(txtFullName.Text)
                Dim email As String = Ultis.ConvertStringNonAttact(txtEmail.Text)
                Dim sodienthoai As String = Ultis.ConvertStringNonAttact(txtPhone.Text)
                Dim hinhthuc As String = ddlHinhthuc.SelectedValue.ToString()
                Dim vanphong As String = ddlvanphong.SelectedValue.ToString()
                '=================
                Dim recaptchaResponse As String = Request.Form("g-recaptcha-response")
                If String.IsNullOrEmpty(recaptchaResponse) Then
                    lblMessage.Text = "Vui lòng nhập mã bảo vệ!"
                    Return
                End If
                Dim isValid As Boolean = VerifyRecaptcha(recaptchaResponse)

                If isValid Then
                    'Luu vao database
                    Ultis.FormInsert("TUVAN", hinhthuc, vanphong, title, noidung, hovaten, email, sodienthoai, "", "VUATIEPNHAN", PortalId)
                    Dim sTitle As String = title
                    Dim sBody As String = "<table cellpadding='0' cellspacing='0' width='800px'> " _
                                        & " <tr><td colspan='2' style='height:25px; padding: 5px; text-align: center; background: #1B96DC'>" & title & "</td></tr> " _
                                        & " <tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Họ và tên: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'> " & hovaten.ToString() & "</td></tr> " _
                                        & " <tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Số điện thoại: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & sodienthoai & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Email: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & email & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Ngày đăng ký: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & DateTime.Now.ToString("H:mm - MM/dd/yy ") & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Hình thức: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & hinhthuc & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Văn phòng: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & vanphong & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Nội dung:</td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & noidung & "</td></tr></table>"
                    Dim sName As String = "Capstone Vietnam <no-reply@capstonevietnam.com>"
                    'SendMail(sName, "it@capstonevietnam.com", "", "", sTitle, sBody)
                    'System.Threading.Thread.Sleep(1000)
                    btnSend.Visible = False
                    Me.guimailthanhcong.Visible = True
                    Me.commentForm.Visible = False
                    Clearform()
                Else
                    ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Mã bảo vệ không chính xác!');</script>")

                    lblMessage.ForeColor = Drawing.Color.Red
                    lblMessage.Text = "reCAPTCHA validation failed. Please try again."
                    'ClientAPI.RegisterStartUpScript(Me.Page, "resetRecaptcha", "<script>resetRecaptcha();</script>")
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Function VerifyRecaptcha(recaptchaResponse As String) As Boolean
            Dim url As String = "https://www.google.com/recaptcha/api/siteverify"
            Dim postData As String = String.Format("secret={0}&response={1}", SecretKey, recaptchaResponse)

            Try
                ' Create a web request to the reCAPTCHA API
                Dim request As WebRequest = WebRequest.Create(url)
                request.Method = "POST"
                request.ContentType = "application/x-www-form-urlencoded"
                Dim byteArray As Byte() = System.Text.Encoding.UTF8.GetBytes(postData)
                request.ContentLength = byteArray.Length

                ' Send the request
                Using dataStream As Stream = request.GetRequestStream()
                    dataStream.Write(byteArray, 0, byteArray.Length)
                End Using

                ' Get the response
                Dim response As WebResponse = request.GetResponse()
                Using dataStream As Stream = response.GetResponseStream()
                    Using reader As New StreamReader(dataStream)
                        Dim responseFromServer As String = reader.ReadToEnd()
                        ' Deserialize the JSON response
                        Dim result = JsonConvert.DeserializeObject(Of RecaptchaResult)(responseFromServer)
                        Return result.success
                    End Using
                End Using
            Catch ex As Exception
                ' Log or handle exceptions as needed
                Return False
            End Try
        End Function
    End Class
    Public Class RecaptchaResult
        Public Property success As Boolean
        Public Property challenge_ts As String
        Public Property hostname As String
    End Class
End Namespace

