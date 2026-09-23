Imports DotNetNuke.Framework
Imports NVCMS.API.Marketing.Services.Marketing

Namespace NVCMS.Modules.Marketing
    ''' <summary>
    ''' Zalo OA Chat - giao diện CSKH. Dữ liệu tải qua AJAX tới /DesktopModules/NVCMS/API/ZaloOAChat/*
    ''' (NVCMS.API ZaloOAChatController → NVCMS.API.ReadGoogleSheet /api/zalo-oa/*).
    ''' Quyền: ZaloOAChatPermission (Administrators / SuperUser / role trong appSettings "zalooa_chat_roles").
    ''' </summary>
    Public MustInherit Class ZaloOAChatViewer
        Inherits Entities.Modules.PortalModuleBase

        Protected CanUseChat As Boolean
        Protected IsChatAdmin As Boolean

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                CanUseChat = ZaloOAChatPermission.CanUseChat(UserInfo)
                IsChatAdmin = ZaloOAChatPermission.IsChatAdmin(UserInfo)
                If CanUseChat Then
                    ServicesFramework.Instance.RequestAjaxScriptSupport()
                    ServicesFramework.Instance.RequestAjaxAntiForgerySupport()
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        ''' <summary>Đổi query string khi file JS/CSS thay đổi để trình duyệt không dùng bản cache cũ.</summary>
        Protected Function AssetUrl(ByVal relativePath As String) As String
            Dim url As String = Me.TemplateSourceDirectory & "/" & relativePath
            Try
                Dim path As String = Server.MapPath(url)
                If System.IO.File.Exists(path) Then
                    url &= "?v=" & System.IO.File.GetLastWriteTimeUtc(path).Ticks.ToString()
                End If
            Catch
            End Try
            Return url
        End Function

        Protected Function JsString(ByVal value As String) As String
            Return HttpUtility.JavaScriptStringEncode(If(value, ""), True)
        End Function

    End Class
End Namespace
