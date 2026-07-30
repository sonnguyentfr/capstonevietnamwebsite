Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Entities
Imports DotNetNuke.Entities.Modules
Imports System.Collections
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.Form
Imports Newtonsoft.Json

Namespace NVCMS.Modules.Form
    Public MustInherit Class Defaultz
        Inherits Entities.Modules.PortalModuleBase

        Dim ctlform As New Form_Controller
        Dim settingmail As String = ""
        Dim settingnhanmail As String = ""
        Dim settingmailtitle As String = ""
        Dim isCaptchaValid As Boolean
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Null.IsNull(ModuleConfiguration.ModuleSettings(BL.settingForm_MailOK)) Then
                    settingnhanmail = ModuleConfiguration.ModuleSettings(BL.settingForm_MailOK)
                End If
                'Response.Write(settingnhanmail)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub SendMail(strFrom As String, strTo As String, strCC As String, strBCC As String, strSubject As String, strBody As String)
            Dim strSMTP As String = "smtp.gmail.com:587"
            DotNetNuke.Services.Mail.Mail.SendMail(strFrom, strTo, strCC, strBCC, DotNetNuke.Services.Mail.MailPriority.High, strSubject, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, strBody, "", strSMTP, "1", "cong1034@nvportal.net", "hemygbwbjqemigle", True)
        End Sub
        Public Sub Clearform()
            Me.txtTitle.Value = ""
            Me.txtFullName.Value = ""
            Me.txtPhone.Value = ""
            Me.txtEmail.Value = ""
            Me.txtcontent.Value = ""
        End Sub
        Protected Sub btnClear_Click(sender As Object, e As EventArgs)
            Me.txtTitle.Value = ""
            Me.txtFullName.Value = ""
            Me.txtPhone.Value = ""
            Me.txtEmail.Value = ""
            Me.txtcontent.Value = ""
        End Sub
        Private Sub Save()
            Try
                Dim title = Ultis.ConvertStringNonAttact(txtTitle.Value)
                Dim noidung = Ultis.ConvertStringNonAttact(txtcontent.Value)
                Dim hovaten = Ultis.ConvertStringNonAttact(txtFullName.Value)
                Dim email = Ultis.ConvertStringNonAttact(txtEmail.Value)
                Dim sodienthoai = Ultis.ConvertStringNonAttact(txtPhone.Value)
                ctlform.Form_Insert(1, title, noidung, hovaten, email, sodienthoai, "", 1, DateTime.Now, PortalId)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub btnSend_Click(sender As Object, e As EventArgs)
            'Lay settings

            If Not Null.IsNull(ModuleConfiguration.ModuleSettings(BL.settingForm_MailNhan)) Then
                settingmail = ModuleConfiguration.ModuleSettings(BL.settingForm_MailNhan)
            Else
                settingmail = "cong1034@nvportal.net"
            End If
            If Not Null.IsNull(ModuleConfiguration.ModuleSettings(BL.settingForm_MailNhanTieude)) Then
                settingmailtitle = ModuleConfiguration.ModuleSettings(BL.settingForm_MailNhanTieude)
            End If
            'Lay du lieu
            Dim title = Ultis.ConvertStringNonAttact(txtTitle.Value)
            Dim noidung = Ultis.ConvertStringNonAttact(txtcontent.Value)
            Dim hovaten = Ultis.ConvertStringNonAttact(txtFullName.Value)
            Dim email = Ultis.ConvertStringNonAttact(txtEmail.Value)
            Dim sodienthoai = Ultis.ConvertStringNonAttact(txtPhone.Value)
            '=================
            If ctlCaptcha.IsValid Then
                Save()
                If settingnhanmail = "Guimail" Then
                    Dim sTitle As String = title
                    Dim sBody As String = "<table cellpadding='0' cellspacing='0' width='800px'> " _
                                        & " <tr><td colspan='2' style='height:25px; padding: 5px; text-align: center; background: #1B96DC'>" & title & "</td></tr> " _
                                        & " <tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Họ và tên: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'> " & hovaten & "</td></tr> " _
                                        & " <tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Số điện thoại: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & sodienthoai & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Email: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & email & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Date: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & DateTime.Now.ToString("H:mm - MM/dd/yy ") & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Tiêu đề: </td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & title & "</td></tr>" _
                                        & "<tr><td style='height:25px; width: 100px; padding: 5px; border-width: 1px; border-color:#1B96DC; border-style: solid;'>Nội dung:</td><td style='border-width: 1px; border-color:#1B96DC; border-style: solid;padding: 5px'>" & noidung & "</td></tr></table>"
                    Dim sName As String = "Cổng thông tin điện tử 1034 <cong1034@nvportal.net>"
                    SendMail(sName, settingmail, "", "nguyen@nvportal.net", sTitle, sBody)
                    System.Threading.Thread.Sleep(1000)
                End If
                btnSend.Visible = False
                Me.guimailthanhcong.Visible = True
                Me.commentForm.Visible = False
                Clearform()
            Else
                ClientAPI.RegisterStartUpScript(Me.Page, "showmessage", "<script>alert('Mã bảo vệ không chính xác!');</script>")
            End If
        End Sub

    End Class
End Namespace

