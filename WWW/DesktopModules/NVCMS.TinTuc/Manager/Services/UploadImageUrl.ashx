<%@ WebHandler Language="VB" Class="doi.upload.UploadImageUrl" %>

Imports System
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Web
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.Hethong
Namespace doi.upload

    ''' <summary>
    ''' Tai anh tu mot duong dan (dan tu Google Docs / web vao noi dung bai) ve may chu va dua vao danh sach anh, tep cua bai,
    ''' giong UploadImage.ashx (cung bang media, cung dinh dang tra ve). Chi cho nguoi dung da dang nhap, chi nhan http/https,
    ''' chan dia chi noi bo, toi da 10 MB, chi nhan anh jpg/png/gif/webp/bmp (kiem tra byte dau).
    ''' </summary>
    Public Class UploadImageUrl : Implements IHttpHandler

        Private Const MAX_BYTES As Integer = 10 * 1024 * 1024
        Dim PhotoPhysicPath As String
        Public PhotoVirtualPath As String
        Dim ctlMediaNews As New NewsByMediaController
        Dim ctlMedia As New MediaItemController
        Public Shared PortalId As Integer = PortalSettings.Current.PortalId

        Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
            context.Response.ContentType = "text/plain"
            Try
                If Not String.Equals(context.Request.HttpMethod, "POST", StringComparison.OrdinalIgnoreCase) Then Fail(context, 405) : Return
                Dim userId As Integer = DotNetNuke.Entities.Users.UserController.Instance.GetCurrentUserInfo().UserID
                If userId <= 0 Then Fail(context, 401) : Return
                'Chong CSRF: form o trang khac khong the tu dat header nay (khong phu thuoc host, dung duoc sau reverse proxy)
                If Not String.Equals(context.Request.Headers("X-Requested-With"), "XMLHttpRequest", StringComparison.OrdinalIgnoreCase) Then Fail(context, 403) : Return
                Dim itemid As Integer = 0
                Integer.TryParse(context.Request.QueryString("itemid"), itemid)
                If itemid <= 0 Then Fail(context, 400) : Return

                Dim bytes As Byte() = Download(context.Request.Form("url"))
                If bytes Is Nothing Then Fail(context, 400) : Return
                Dim ext As String = DetectExt(bytes)
                If ext = "" Then Fail(context, 400) : Return

                PhotoPhysicPath = Ultis.GetImagePath(False, PortalId, True)
                PhotoVirtualPath = Ultis.GetImagePath(True, PortalId, True)
                Dim fileName As String = DateTime.Now.ToString("yyyyMMddHHmmss") & "-" & userId & "-url" & Guid.NewGuid().ToString("N").Substring(0, 8) & "." & ext
                File.WriteAllBytes(PhotoPhysicPath & "/" & fileName, bytes)
                Dim mediaPath As String = Ultis.GetMediaPath(PhotoVirtualPath, fileName)
                Dim li As String = "<li class='anh-daupload'><div class='anh-khunganh'><a data-fancybox data-caption='' href='" & Ultis.GetBackround(ext, mediaPath) & "'><img src='" & Ultis.GetBackround(ext, mediaPath) & "' /></a><input " & Ultis.Enableanh(ext) & " type='checkbox' data-img='" & mediaPath & "' class='anh-addToAvatar' data-toggle='tooltip' data-placement='top' title='Đặt làm ảnh đại diện'/></div><div class='anh-thongtin'><a class='anh-addToContent btn' data-title='" & mediaPath & "' data-img='" & mediaPath & "' data-toggle='tooltip' data-placement='top' title='Chèn vào bài viết'><em class='icon ni ni-download'></em></a><a class='anh-addToContent2 btn' data-title='" & mediaPath & "' data-img='" & mediaPath & "' data-toggle='tooltip' data-placement='top' title='Chèn ảnh gốc vào bài viết'><em class='icon ni ni-camera'></em></a><a class='anh-addToContentLink btn' data-title='" & mediaPath & "' data-img='" & mediaPath & "' data-toggle='tooltip' data-placement='top' title='Chèn link vào text'><em class='icon ni ni-link'></em></a></div><div style='clear:both'></div></li>"
                Dim idmedia As Integer = ctlMedia._Insert(fileName, fileName, Ultis.GetImagePath(False, PortalId, True), mediaPath, bytes.Length, ext, DateTime.Now, userId, PortalId)
                ctlMediaNews._Insert(itemid, idmedia, DateTime.Now, userId, PortalId)

                context.Response.StatusCode = CInt(HttpStatusCode.OK)
                context.Response.Write(li)
            Catch ex As Exception
                Fail(context, 500)
            End Try
        End Sub

        Private Shared Sub Fail(ByVal context As HttpContext, ByVal code As Integer)
            context.Response.StatusCode = code
            context.Response.Write("")
        End Sub

        ''' <summary>Tai ve, theo toi da 3 lan chuyen huong, moi buoc deu kiem tra lai dia chi dich.</summary>
        Private Shared Function Download(ByVal url As String) As Byte()
            Dim current As Uri = Nothing
            If String.IsNullOrWhiteSpace(url) OrElse Not Uri.TryCreate(url.Trim(), UriKind.Absolute, current) Then Return Nothing
            ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol Or SecurityProtocolType.Tls12
            Try
                For hop As Integer = 0 To 3
                    If Not IsSafeUri(current) Then Return Nothing
                    Dim req As HttpWebRequest = CType(WebRequest.Create(current), HttpWebRequest)
                    req.AllowAutoRedirect = False
                    req.Timeout = 15000
                    req.ReadWriteTimeout = 15000
                    req.UserAgent = "Mozilla/5.0 (NVCMS image import)"
                    req.Accept = "image/*"
                    Using resp As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                        Dim code As Integer = CInt(resp.StatusCode)
                        If code >= 300 AndAlso code < 400 Then
                            Dim loc As String = resp.Headers("Location")
                            Dim nxt As Uri = Nothing
                            If String.IsNullOrEmpty(loc) OrElse Not Uri.TryCreate(current, loc, nxt) Then Return Nothing
                            current = nxt
                            Continue For
                        End If
                        If code <> 200 Then Return Nothing
                        If resp.ContentLength > MAX_BYTES Then Return Nothing
                        Using st As Stream = resp.GetResponseStream()
                            Using ms As New MemoryStream()
                                Dim buf(8191) As Byte
                                Dim n As Integer = st.Read(buf, 0, buf.Length)
                                While n > 0
                                    ms.Write(buf, 0, n)
                                    If ms.Length > MAX_BYTES Then Return Nothing
                                    n = st.Read(buf, 0, buf.Length)
                                End While
                                Return ms.ToArray()
                            End Using
                        End Using
                    End Using
                Next
            Catch ex As Exception
                Return Nothing
            End Try
            Return Nothing
        End Function

        Private Shared Function IsSafeUri(ByVal u As Uri) As Boolean
            If u.Scheme <> Uri.UriSchemeHttp AndAlso u.Scheme <> Uri.UriSchemeHttps Then Return False
            If u.Port <> 80 AndAlso u.Port <> 443 Then Return False
            If Not String.IsNullOrEmpty(u.UserInfo) Then Return False
            Dim addrs As IPAddress() = Nothing
            Try
                addrs = Dns.GetHostAddresses(u.DnsSafeHost)
            Catch ex As Exception
                Return False
            End Try
            If addrs Is Nothing OrElse addrs.Length = 0 Then Return False
            For Each a As IPAddress In addrs
                If Not IsPublicAddress(a) Then Return False
            Next
            Return True
        End Function

        Private Shared Function IsPublicAddress(ByVal ip As IPAddress) As Boolean
            If IPAddress.IsLoopback(ip) Then Return False
            Dim b As Byte() = ip.GetAddressBytes()
            If ip.AddressFamily = AddressFamily.InterNetworkV6 Then
                ' ::ffff:a.b.c.d -> kiem tra nhu IPv4
                Dim mapped As Boolean = True
                For i As Integer = 0 To 9
                    If b(i) <> 0 Then mapped = False
                Next
                If mapped AndAlso b(10) = 255 AndAlso b(11) = 255 Then
                    Return IsPublicAddress(New IPAddress(New Byte() {b(12), b(13), b(14), b(15)}))
                End If
                If ip.IsIPv6LinkLocal OrElse ip.IsIPv6SiteLocal OrElse ip.IsIPv6Multicast Then Return False
                If (b(0) And &HFE) = &HFC Then Return False
                Return True
            End If
            If ip.AddressFamily <> AddressFamily.InterNetwork Then Return False
            If b(0) = 0 OrElse b(0) = 10 OrElse b(0) = 127 Then Return False
            If b(0) = 169 AndAlso b(1) = 254 Then Return False
            If b(0) = 172 AndAlso b(1) >= 16 AndAlso b(1) <= 31 Then Return False
            If b(0) = 192 AndAlso b(1) = 168 Then Return False
            If b(0) = 100 AndAlso b(1) >= 64 AndAlso b(1) <= 127 Then Return False
            If b(0) >= 224 Then Return False
            Return True
        End Function

        ''' <summary>Nhan dang anh theo byte dau (khong tin Content-Type).</summary>
        Private Shared Function DetectExt(ByVal b As Byte()) As String
            If b.Length < 12 Then Return ""
            If b(0) = &HFF AndAlso b(1) = &HD8 AndAlso b(2) = &HFF Then Return "jpg"
            If b(0) = &H89 AndAlso b(1) = &H50 AndAlso b(2) = &H4E AndAlso b(3) = &H47 Then Return "png"
            If b(0) = &H47 AndAlso b(1) = &H49 AndAlso b(2) = &H46 AndAlso b(3) = &H38 Then Return "gif"
            If b(0) = &H52 AndAlso b(1) = &H49 AndAlso b(2) = &H46 AndAlso b(3) = &H46 AndAlso b(8) = &H57 AndAlso b(9) = &H45 AndAlso b(10) = &H42 AndAlso b(11) = &H50 Then Return "webp"
            If b(0) = &H42 AndAlso b(1) = &H4D Then Return "bmp"
            Return ""
        End Function

        Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property

    End Class
End Namespace
