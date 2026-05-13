<%@ WebHandler Language="VB" Class="doi.upload.UploadImage" %>

Imports System
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Web.Script.Serialization
Imports NVCMS.Modules.Video
Imports NVCMS.Modules.TinTuc
Namespace doi.upload


    Public Class UploadImage : Implements IHttpHandler

        Dim PhotoPhysicPath As String
        Public PhotoVirtualPath As String
        Dim _VideoByMediaController As New VideoByMediaController
        Dim _MediaItemController As New MediaItemController
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            PhotoPhysicPath = Ultis.GetVideoPath(False, PortalSettings.Current.PortalId, True)
            PhotoVirtualPath = Ultis.GetVideoPath(True, PortalSettings.Current.PortalId, True)
            Dim itemid As Integer = 0
            Integer.TryParse(context.Request.QueryString("itemid"), itemid)
            Dim tenfile As String = ""
            Dim validFileTypes As String() = {"avi", "mp4"}
            'Check if Request is to Upload the File.
            If context.Request.Files.Count = 1 Then
                For i As Integer = 0 To context.Request.Files.Count - 1
                    Dim postedFile As HttpPostedFile = context.Request.Files(i)
                    'Set the File Name.
                    Dim fileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") & "-" & UserController.Instance.GetCurrentUserInfo().UserID & ReplaceChuoi.bodautenfile(Path.GetFileName(postedFile.FileName))
                    Dim sextension As String = ""
                    Dim extension As String = Path.GetExtension(postedFile.FileName)
                    If extension.Length > 1 Then
                        sextension = extension.Remove(0, 1)
                    End If
                    If validFileTypes.Contains(sextension) Then
                        'Kiem tra xem file co chua
                        If System.IO.File.Exists(PhotoPhysicPath & "\" & fileName) Then
                        Else
                            'insert vo db
                            Dim filesize As Integer = postedFile.ContentLength
                            If (filesize / 1048576) < 350 Then
                                postedFile.SaveAs(PhotoPhysicPath & "/" & fileName)
                                tenfile += Ultis.GetMediaPath(PhotoVirtualPath, fileName)
                                Dim idmedia As Integer = 0
                                idmedia = _MediaItemController._Insert(fileName, fileName, Ultis.GetVideoPath(False, PortalSettings.Current.PortalId, True), Ultis.GetMediaPath(PhotoVirtualPath, fileName), filesize, sextension,0, DateTime.Now, UserController.Instance.GetCurrentUserInfo().UserID, PortalSettings.Current.PortalId)
                                'chen vao bang product media
                                _VideoByMediaController._Insert(itemid, idmedia, DateTime.Now, UserController.Instance.GetCurrentUserInfo().UserID, PortalSettings.Current.PortalId)
                            Else
                                context.Response.Write("<script type='text/javascript'>alert('Kích thước Video phải bé hơn 50Mb');</script>")
                            End If

                        End If
                    Else
                        context.Response.Write("<script type='text/javascript'>alert('Chỉ sử dụng file *.avi hoặc *.mp4');</script>")
                    End If

                Next
                context.Response.StatusCode = CInt(HttpStatusCode.OK)
                context.Response.ContentType = "text/plain"
                context.Response.Write(tenfile)
                context.Response.End()
            Else
                context.Response.Write("<script type='text/javascript'>alert('Bạn vui lòng chỉ chọn 1 video!');</script>")
                context.Response.StatusCode = CInt(HttpStatusCode.OK)
                context.Response.ContentType = "text/plain"
                context.Response.Write(tenfile)
                context.Response.End()
            End If
        End Sub

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
End Namespace