Imports NVCMS.Modules.TinTuc
Imports NVCMS.Web.Components

Namespace DesktopModules.TinTuc.Control
    Partial Class IndexLastest
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Public count As Integer = 1
        Public Property ItemID() As Integer
            Get
                If Not ViewState.Item("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState.Item("ItemID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("ItemID") = Value.ToString
            End Set
        End Property
        Public Property fbclid() As String
            Get
                If Not ViewState.Item("fbclid") Is Nothing Then
                    Return ViewState.Item("fbclid")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("fbclid", value)
            End Set
        End Property
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                'Dim sUrl As String = Request.RawUrl
                'ItemID = Ultis.GetRequestId(sUrl)
                fbclid = Request.Item("fbclid")
                Dim sUrl1 As String = Request.RawUrl
                Dim sUrl As String = sUrl1.Replace("?fbclid=" & fbclid, "")
                'Dim sUrl As String = Request.RawUrl
                'ItemID = Ultis.GetRequestId(sUrl)
                Dim ctlNews As New NV_NewsController
                Dim cacheName As String = BL.NewsHomeCat & "MoiNhat" & count
                Dim fromCache As ArrayList = HttpCacheHelper.GetFromCache(cacheName)
                If fromCache Is Nothing Then
                    'fromCache = New List(Of ArticleViewModel)()
                    Dim arrtop As ArrayList = ctlNews.ShowBaiMoiNhat("", 0, count)
                    If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                        fromCache = arrtop
                        HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News"}, cacheName, fromCache, TimeSpan.FromDays(30))
                    End If
                End If
                rptLastest.DataSource = fromCache
                rptLastest.DataBind()
            End If
        End Sub
    End Class
End Namespace

