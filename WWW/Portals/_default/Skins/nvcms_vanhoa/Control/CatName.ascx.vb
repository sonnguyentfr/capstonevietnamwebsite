Imports System.IO
Imports System.Xml
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Web.Components

Namespace DesktopModules.TinTuc.Control
    Partial Class BreadCrumb
        Inherits Entities.Modules.PortalModuleBase
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
        Public Property CategoryId() As Integer
            Get
                If Not ViewState.Item("CategoryId") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("CategoryId")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("CategoryId", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CategoryId") = Value.ToString
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
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    CategoryId = BL.GetMappingCategoryIDByTabID(PortalSettings.ActiveTab.TabID)
                    ' Response.Write(CategoryId)
                    If CategoryId > 0 Then
                        'Lay danh mục con ra
                        Dim ctlCategory As New NV_NewsCategoriesController
                        'Dim objCategory As NV_NewsCategoriesInfo
                        'objCategory = ctlCategory.GetByID(CategoryId)

                        Dim cacheName As String = BL.NewsDetailCache & "Danhmuc" & CategoryId
                        Dim objCategory As NV_NewsCategoriesInfo = HttpCacheHelper.GetFromCache(cacheName)
                        If objCategory Is Nothing Then

                            objCategory = New NV_NewsCategoriesInfo()
                            Dim objNewsFromDB As NV_NewsCategoriesInfo = ctlCategory.GetByID(CategoryId)
                            If Not objNewsFromDB Is Nothing Then
                                objCategory = objNewsFromDB
                                HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"NewsCategories"}, cacheName, objCategory, TimeSpan.FromDays(30))
                            End If
                        End If

                        Me.ltrtitlecat.Text = objCategory.CategoryName
                        If Not objCategory Is Nothing Then
                            With objCategory
                                If .ParentId > 0 Then
                                    Dim arrChildrenCat As ArrayList = ctlCategory.GetByParentId(.ParentId, 0)
                                    If Not arrChildrenCat Is Nothing AndAlso arrChildrenCat.Count > 0 Then
                                        If DataCache.GetCache(BL.NewsHomeCat & CategoryId & CurrentPage & "Subcat") Is Nothing Then
                                            DataCache.SetCache(BL.NewsHomeCat & CategoryId & CurrentPage & "Subcat", arrChildrenCat, Nothing, DateTime.Now.AddSeconds(10), TimeSpan.Zero)
                                            'Lay subcat
                                            rptsubcat.DataSource = arrChildrenCat
                                            rptsubcat.DataBind()
                                            '--
                                        Else
                                            'Lay subcat
                                            rptsubcat.DataSource = DataCache.GetCache(BL.NewsHomeCat & CategoryId & CurrentPage & "Subcat")
                                            rptsubcat.DataBind()
                                            '----
                                        End If
                                        'rptCat.DataSource = arrChildrenCat
                                        'rptCat.DataBind()
                                    End If
                                Else
                                    Dim arrChildrenCat As ArrayList = ctlCategory.GetByParentId(CategoryId, 0)
                                    If Not arrChildrenCat Is Nothing AndAlso arrChildrenCat.Count > 0 Then
                                        If DataCache.GetCache(BL.NewsHomeCat & CategoryId & CurrentPage & "Subcat") Is Nothing Then
                                            DataCache.SetCache(BL.NewsHomeCat & CategoryId & CurrentPage & "Subcat", arrChildrenCat, Nothing, DateTime.Now.AddSeconds(10), TimeSpan.Zero)
                                            'Lay subcat
                                            rptsubcat.DataSource = arrChildrenCat
                                            rptsubcat.DataBind()
                                            '--
                                        Else
                                            'Lay subcat
                                            rptsubcat.DataSource = DataCache.GetCache(BL.NewsHomeCat & CategoryId & CurrentPage & "Subcat")
                                            rptsubcat.DataBind()
                                        End If
                                        'rptCat.DataSource = arrChildrenCat
                                        'rptCat.DataBind()
                                    End If
                                End If


                            End With
                        End If
                    Else
                        'Response.Redirect("/")
                    End If
                End If
            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Public Function ActiveSubCat(id As Integer) As String
            Dim ctlcat As New NV_NewsCategoriesController
            Dim objcat As NV_NewsCategoriesInfo
            objcat = ctlcat.GetByID(id)
            If Not objcat Is Nothing Then
                With objcat
                    If .CategoryID = BL.GetMappingCategoryIDByTabID(PortalSettings.ActiveTab.TabID) Then
                        Return "active"
                    Else
                        Return ""
                    End If
                End With
            Else
                Return ""
            End If
        End Function
#End Region

    End Class
End Namespace