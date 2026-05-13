<%@ WebHandler Language="VB" Class="doi.upload.UploadImage" %>

Imports System
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Web.Script.Serialization
Imports NVCMS.Modules.TinTuc
Namespace doi.upload


    Public Class UploadImage : Implements IHttpHandler

        Dim PhotoPhysicPath As String
        Public PhotoVirtualPath As String
        Dim ctlMediaNews As New NewsByMediaController
        Dim ctlMedia As New MediaItemController
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            PhotoPhysicPath = Ultis.GetImagePath(False, PortalSettings.Current.PortalId, True)
            PhotoVirtualPath = Ultis.GetImagePath(True, PortalSettings.Current.PortalId, True)
            Dim itemid As Integer = 0
            Integer.TryParse(context.Request.QueryString("itemid"), itemid)
            Dim tenfile As String = ""
            Dim duration As Integer = 0
            'Check if Request is to Upload the File.
            If context.Request.Files.Count > 0 Then
                For i As Integer = 0 To context.Request.Files.Count - 1
                    Dim postedFile As HttpPostedFile = context.Request.Files(i)
                    'Set the File Name.
                    Dim fileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") & "-" & UserController.Instance.GetCurrentUserInfo().UserID & ReplaceChuoi.bodautenfile(Path.GetFileName(postedFile.FileName))
                    Dim sextension As String = ""
                    Dim extension As String = Path.GetExtension(postedFile.FileName)
                    If extension.Length > 1 Then
                        sextension = extension.Remove(0, 1)
                    End If
                    'Kiem tra xem file co chua
                    If System.IO.File.Exists(PhotoPhysicPath & "\" & fileName) Then
                    Else
                        postedFile.SaveAs(PhotoPhysicPath & "/" & fileName)
                        tenfile += "<li class='anh-daupload'><div class='anh-khunganh'><a data-fancybox data-caption='' href='" & Ultis.GetBackround(sextension, Ultis.GetMediaPath(PhotoVirtualPath, fileName)) & "'><img src='" & Ultis.GetBackround(sextension, Ultis.GetMediaPath(PhotoVirtualPath, fileName)) & "' /></a><input " & Ultis.Enableanh(sextension) & " type='checkbox' data-img='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' class='anh-addToAvatar' data-toggle='tooltip' data-placement='top' title='Đặt làm ảnh đại diện'/></div><div class='anh-thongtin'><a class='anh-addToContent btn' data-title='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' data-img='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' data-toggle='tooltip' data-placement='top' title='Chèn vào bài viết'><em class='icon ni ni-download'></em></a><a class='anh-addToContent2 btn' data-title='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' data-img='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' data-toggle='tooltip' data-placement='top' title='Chèn ảnh gốc vào bài viết'><em class='icon ni ni-camera'></em></a><a class='anh-addToContentLink btn' data-title='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' data-img='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' data-toggle='tooltip' data-placement='top' title='Chèn link vào text'><em class='icon ni ni-link'></em></a></div><div style='clear:both'></div></li>"
                        'check xem có phải video ko
                        'If sextension = "mp4" Then
                        '    duration = CType(Ultis.GetVideoDurationSecond(Ultis.GetImagePath(False, PortalSettings.Current.PortalId, True) & "\" & fileName), Integer)
                        'End If
                        'insert vo db
                        Dim filesize As Integer = postedFile.ContentLength
                        Dim idmedia As Integer = 0
                        idmedia = ctlMedia._Insert(fileName, fileName, Ultis.GetImagePath(False, PortalSettings.Current.PortalId, True), Ultis.GetMediaPath(PhotoVirtualPath, fileName), filesize, sextension, 0, DateTime.Now, UserController.Instance.GetCurrentUserInfo().UserID, PortalSettings.Current.PortalId)
                        'update Duration
                        Dim objMedia As MediaItemInfo = ctlMedia._GetByID(idmedia)
                        
                        'chen vao bang product media
                        ctlMediaNews._Insert(itemid, idmedia, DateTime.Now, UserController.Instance.GetCurrentUserInfo().UserID, PortalSettings.Current.PortalId)
                    End If
                    'Save the File in Folder.

                Next

                'context.Response.ContentType = "text/plain"
                'context.Response.Write(tenfile)
                'context.Response.End()
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