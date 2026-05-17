Imports NVCMS.Modules.BannerAdv
Imports NVCMS.Modules.LadingPage

Namespace NVCMS.Modules.ShortURL
    Public MustInherit Class Redirect
        Inherits Entities.Modules.PortalModuleBase
        Dim _ShortUrlController As New ShortUrlController
        Dim _ShortUrlShareController As New ShortUrlShareController
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim sUrl1 As String = Request.RawUrl

                'Dim sUrl As String = ""
                If sUrl1.Contains("?") Then
                    sUrl1 = sUrl1.Substring(0, sUrl1.IndexOf("?"))
                ElseIf sUrl1.Contains("/fairs/register/") Then
                    Me.Response.Clear()
                    Me.Response.Status = "301 Moved Permanently"
                    Me.Response.AddHeader("Location", "https://crm.capstone.edu.vn" & sUrl1)
                    Me.Response.[End]()
                End If
                If sUrl1.Contains("bannerclick") Then
                    Dim srequest As String = sUrl1.ToString().Substring(sUrl1.LastIndexOf("/", StringComparison.Ordinal) + 1)
                    Dim sId = GetRequestIdz(srequest).Trim()
                    Dim ctlAdvbaner As New BannerAdvController
                    If sId > 0 Then
                        Dim ctlstatic As New BannerAdv_StaticController
                        ctlstatic._Insert(sId, Request.ServerVariables("REMOTE_ADDR"), DateTime.Now, True)
                        'update vao bang banner
                        ctlAdvbaner.UpdateClick(sId)
                        Dim objInfo As BannerAdvInfo = ctlAdvbaner.GetByID(sId)
                        If Not objInfo Is Nothing Then
                            With objInfo
                                If (.Link <> "") Then
                                    Response.AddHeader("REFRESH", "5; URL=" & .Link)
                                    Response.Redirect(.Link, True)
                                Else
                                    Response.Redirect("/")
                                End If

                            End With
                        Else
                            Response.Redirect("/")
                        End If
                    End If
                Else

                    Dim srequest As String = sUrl1.ToString().Substring(sUrl1.LastIndexOf("/", StringComparison.Ordinal) + 1) 'sUrl1.Replace("/", "")  'Me.Request.Url.ToString().Substring(sUrl.LastIndexOf("/", StringComparison.Ordinal) + 1)
                    Dim cacheName As String = nvcmsBL.cacheShortUrl & srequest
                    'Dim oShortUrlcache As ShortUrl_Info = HttpCacheHelper.GetFromCache(cacheName)
                    Dim oShortUrlcache As ShortUrl_Info = DataCache.GetCache(cacheName)
                    If oShortUrlcache Is Nothing Then
                        oShortUrlcache = New ShortUrl_Info()
                        With oShortUrlcache
                            Dim oShortUrl As ShortUrl_Info = _ShortUrlController._Redirect(srequest)
                            If Not oShortUrl Is Nothing Then
                                oShortUrlcache = oShortUrl
                                'HttpCacheHelper.SaveToCacheDependency("CapstoneVietNamV2", New String() {"NVCMS_ShortyUrls"}, cacheName, oShortUrl, TimeSpan.FromDays(30))
                                DataCache.SetCache(cacheName, oShortUrl)
                                '--
                                Me.Response.Clear()
                                Me.Response.Status = "301 Moved Permanently"
                                Me.Response.AddHeader("Location", oShortUrl.real_url)
                                Me.Response.[End]()
                            End If
                        End With
                    End If
                    'Dim oShortUrl As ShortUrl_Info
                    'oShortUrl = _ShortUrlController._Redirect(srequest)
                    If oShortUrlcache IsNot Nothing AndAlso Not String.IsNullOrEmpty(oShortUrlcache.real_url) Then
                        With oShortUrlcache
                            'Tinh luot click
                            _ShortUrlController._Update_Click(.short_url)
                            'Luu vet
                            If Request IsNot Nothing Then
                                Dim host = Request.Url.Host
                                If Not Request.UrlReferrer Is Nothing Then
                                    Dim refererUrl = Request.UrlReferrer

                                    _ShortUrlShareController._Insert(.short_url, refererUrl.ToString(), DateTime.Now, HttpContext.Current.Request.UserHostAddress)

                                End If
                            End If
                            '--
                            Me.Response.Clear()
                            Me.Response.Status = "301 Moved Permanently"
                            Me.Response.AddHeader("Location", oShortUrlcache.real_url)
                            Me.Response.[End]()
                        End With

                    End If
                End If
                'Response.Write(sUrl1)
                '                Response.Write("<br />" & GetRequestIdz(srequest))
            Catch ex As Exception
                'ProcessModuleLoadException(Me, ex)
            End Try
        End Sub



        Public Shared Function RemovelinkParam(surl As String, paraname As String) As String
            Dim iStart As Integer = 0
            Dim iEnd As Integer = surl.LastIndexOf(paraname, System.StringComparison.Ordinal)
            'Dim iEnd As Integer = surl.Length
            If iEnd > 0 Then
                Dim iLength As Integer = iEnd - iStart - 1
                Return CType(surl.Substring(iStart + 1, iLength), String)
            Else
                Return ""
            End If
        End Function
        Public Shared Function GetRequestIdz(surl As String) As String
            Dim iStart As Integer = surl.LastIndexOf("-", System.StringComparison.Ordinal)
            Dim iEnd As Integer = surl.Length
            If iEnd > 0 Then
                Dim iLength As Integer = iEnd - iStart - 1
                Return CType(surl.Substring(iStart + 1, iLength), String)
            Else
                Return ""
            End If
        End Function
    End Class
End Namespace
