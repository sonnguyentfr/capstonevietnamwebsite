<%@ WebHandler Language="VB" Class="doi.TrangLadingPage.UploadMedia" %>
Imports System
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Web.Script.Serialization
Imports NVCMS.Modules.LadingPage

Namespace doi.TrangLadingPage
    Public Class UploadMedia : Implements IHttpHandler
        Dim PhotoPhysicPath As String
        Public PhotoVirtualPath As String
        Dim _Media_Controller As New Media_Controller
        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            PhotoPhysicPath = Ultis.GetImagePath(False, PortalSettings.Current.PortalId, True)
            PhotoVirtualPath = Ultis.GetImagePath(True, PortalSettings.Current.PortalId, True)
            Dim itemid As Integer = 0
            Integer.TryParse(context.Request.QueryString("itemid"), itemid)
            Dim tenfile As String = ""
            Dim validFileTypes As String() = {"gif", "jpeg", "jpg", "png"}
            Dim isValidFile As Boolean = False
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
                    If validFileTypes.Contains(sextension) Then
                        'Kiem tra xem file co chua
                        If System.IO.File.Exists(PhotoPhysicPath & "\" & fileName) Then
                        Else
                            'insert vo db
                            Dim filesize As Integer = postedFile.ContentLength
                            If (filesize / 1048576) < 50 Then
                                postedFile.SaveAs(PhotoPhysicPath & "/" & fileName)
                                Dim idmedia As Integer = 0
                                tenfile += "<tr><td><img src='" & Ultis.GetMediaPath(PhotoVirtualPath, fileName) & "' width='100px' style='padding: 0px 10px 5px 0px' align='left' />" & fileName & "</td><td></td></tr>"
                                'chen vao bang product media
                                _Media_Controller._Insert(itemid, fileName, "", Ultis.GetMediaPath(PhotoVirtualPath, fileName), 0, PortalSettings.Current.PortalId)
                            Else
                                context.Response.Write("<script type='text/javascript'>alert('File quá lớn. Kích thước file < 50Mb! ');</script>")
                            End If
                        End If
                    Else
                        context.Response.Write("<script type='text/javascript'>alert('File không đúng định dạng! ');</script>")
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