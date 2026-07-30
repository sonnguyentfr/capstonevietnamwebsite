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
Imports NVCMS.Modules.FAQs
Imports Newtonsoft.Json

Namespace NVCMS.Modules.FAQs
    Public MustInherit Class Defaultz
        Inherits Entities.Modules.PortalModuleBase

        Dim ctl As New uQuestion_Controller
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Public Sub Clearform()
            'Me.txtTitle.Value = ""
            Me.txtFullName.Value = ""
            Me.txtPhone.Value = ""
            Me.txtEmail.Value = ""
            Me.txtcontent.Value = ""
        End Sub
        Protected Sub btnClear_Click(sender As Object, e As EventArgs)
            'Me.txtTitle.Value = ""
            Me.txtFullName.Value = ""
            Me.txtPhone.Value = ""
            Me.txtEmail.Value = ""
            Me.txtcontent.Value = ""
        End Sub
        Private Sub Save()
            Try
                'Dim title = Ultis.ConvertStringNonAttact(txtTitle.Value)
                Dim noidung = Ultis.ConvertStringNonAttact(txtcontent.Value)
                Dim hovaten = Ultis.ConvertStringNonAttact(txtFullName.Value)
                Dim email = Ultis.ConvertStringNonAttact(txtEmail.Value)
                Dim sodienthoai = Ultis.ConvertStringNonAttact(txtPhone.Value)
                Dim info As New uQuestion_Info
                With info
                    .UserName = Ultis.ConvertStringNonAttact(txtFullName.Value)
                    .Email = Ultis.ConvertStringNonAttact(txtEmail.Value)
                    .Mobile = Ultis.ConvertStringNonAttact(txtPhone.Value)
                    .Address = Ultis.ConvertStringNonAttact(txtdiachi.Value)
                    .Title = ""
                    .tochuccanhan = Ultis.ConvertStringNonAttact(txttochuc.Value)
                    .Question = Ultis.ConvertStringNonAttact(txtcontent.Value)
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    .Status = "1"
                    .CreatedDate = DateTime.Now
                    .PortalId = PortalId
                End With
                ctl._Insert(info)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub btnSend_Click(sender As Object, e As EventArgs)
            If ctlCaptcha.IsValid Then
                Save()
                btnSend.Visible = False
                Me.guimailthanhcong.Visible = True
                Me.commentForm.Visible = False
                Clearform()
            Else
                errCapchat.Visible = True
            End If
        End Sub

    End Class
End Namespace

