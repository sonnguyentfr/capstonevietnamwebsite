Imports NVCMS.Modules.TinTuc
Namespace DesktopModules.TinTuc.Control
    Partial Class Lastest
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Public count As Integer = 1
        Public CategoryId As Integer
        Public Sub New()
        End Sub
        Public Sub New(_CategoryId As Integer)
            CategoryId = _CategoryId
        End Sub
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
                Dim arrnew As New ArrayList
                arrnew = ctlNews.SelectIndex("", CategoryId, 0, 1, count, "", False)
                Dim strcache = BL.NewsHomeCat & "menu" & CategoryId & count
                If DataCache.GetCache(strcache) Is Nothing Then
                    DataCache.SetCache(strcache, arrnew, Nothing, DateTime.Now.AddSeconds(10), TimeSpan.Zero)
                    rptLastest.DataSource = arrnew
                    rptLastest.DataBind()
                Else
                    rptLastest.DataSource = DataCache.GetCache(strcache)
                    rptLastest.DataBind()
                End If
            End If
        End Sub
    End Class
End Namespace

