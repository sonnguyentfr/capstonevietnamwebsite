Imports System
Imports System.Web
Imports System.Web.UI
Imports DotNetNuke
Imports DotNetNuke.UI.Utilities
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.ShortURL

    Public MustInherit Class inc_list
        Inherits Entities.Modules.PortalModuleBase
        Dim _ShortUrlShareController As New ShortUrlShareController
#Region "Controls"
        Public Property TotalPage() As Integer
            Get
                If Not ViewState.Item("TotalPage") Is Nothing Then
                    Try
                        Return CInt(ViewState.Item("TotalPage"))
                    Catch ex As Exception
                        Return Null.NullInteger
                    End Try
                Else
                    ViewState.Add("TotalPage", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("TotalPage") = Value.ToString
            End Set
        End Property
        Property CurrentPage() As Integer 'Trang hiện tại
            Get
                If Not ViewState.Item("CurrentPage") Is Nothing Then
                    Return CInt(ViewState.Item("CurrentPage"))
                Else
                    ViewState.Add("CurrentPage", "1")
                    Return 1
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("CurrentPage") = value.ToString
            End Set
        End Property
        Property PageSize() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("PageSize") Is Nothing Then
                    Return CInt(ViewState.Item("PageSize"))
                Else
                    ViewState.Add("PageSize", "40")
                    Return 40
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("PageSize") = value.ToString
            End Set
        End Property
        Property TotalRecord() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("TotalRecord") Is Nothing Then
                    Return CInt(ViewState.Item("TotalRecord"))
                Else
                    ViewState.Add("TotalRecord", "0")
                    Return 0
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("TotalRecord") = value.ToString
            End Set
        End Property
        Public Property itemid() As String
            Get
                If Not ViewState.Item("itemid") Is Nothing Then
                    Return ViewState.Item("itemid")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("itemid", value)
            End Set
        End Property

#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then
                    If Request.Item("itemid") <> "" Then
                        itemid = Request.Item("itemid")
                        ltrshortlink.Text = itemid
                        BinddrgDataViewer()
                    End If


                Else
                    Dim sTemp As String = Request("__EVENTARGUMENT")
                    If Not String.IsNullOrEmpty(sTemp) AndAlso sTemp.StartsWith("Page_") Then
                        CurrentPage = Integer.Parse(sTemp.Replace("Page_", ""))
                        'Fill dữ liệu vào grid
                        BinddrgDataViewer()
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
#Region "BindData"

        Private Sub BinddrgDataViewer()
            TotalRecord = _ShortUrlShareController._Find_Count(itemid)
            Dim totalPage As Integer = If(TotalRecord Mod PageSize <> 0, (TotalRecord / PageSize + 1), (TotalRecord / PageSize))
            If totalPage > 1 Then
                vbPaging.TotalPage = totalPage
                vbPaging.bindPages()
                vbPaging.Visible = True
            Else
                vbPaging.Visible = False
            End If
            'Nem vao cache chơi
            Dim cacheName As String = nvcmsBL.cacheShortUrl & itemid & CurrentPage & PageSize
            Dim fromCache As ArrayList = HttpCacheHelper.GetFromCache(cacheName)
            If fromCache Is Nothing Then
                Dim arr As ArrayList = _ShortUrlShareController._Find_Index(itemid, CurrentPage, PageSize)
                If arr IsNot Nothing AndAlso arr.Count() > 0 Then
                    fromCache = arr
                    HttpCacheHelper.SaveToCacheDependency("CapstoneVietNamV2", New String() {"NVCMS_ShortyUrls"}, cacheName, fromCache, TimeSpan.FromDays(30))
                End If
            End If
            drgDataViewer.DataSource = fromCache
            drgDataViewer.DataBind()
            Me.lbTotalNewsFind.Text = TotalRecord
        End Sub
#End Region

    End Class


End Namespace
