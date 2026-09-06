Imports System
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Content.Taxonomy
Imports NVCMS.Modules.TinTuc
Imports DotNetNuke.UI.Utilities
Imports Telerik.Web.UI
Imports System.Collections.Generic
Imports DotNetNuke.Services.FileSystem
Imports System.IO
Imports NVCMS.Modules.Users

Namespace NVCMS.Modules.TinTuc


    Public MustInherit Class newsedittinleinquan
        Inherits Entities.Modules.PortalModuleBase
#Region "Properties"
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
                    ViewState.Add("PageSize", "50")
                    Return 5
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
        Public Property KeySearch() As String
            Get
                If Not ViewState.Item("KeySearch") Is Nothing Then
                    Return ViewState.Item("KeySearch")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("KeySearch", value)
            End Set
        End Property
        Public Property Datefrom() As Date
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return CType(ViewState.Item("Datefrom"), Date)
                Else
                    Return BL.minDateV
                End If
            End Get
            Set(ByVal value As Date)
                ViewState.Add("Datefrom", value)
            End Set
        End Property
        Public Property DateTo() As Date
            Get
                If Not ViewState.Item("todate") Is Nothing Then
                    Return CType(ViewState.Item("todate"), Date)
                Else
                    Return BL.maxDateV
                End If
            End Get
            Set(ByVal value As Date)
                ViewState.Add("todate", value)
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
        Public Property CreatedUser() As Integer
            Get
                If Not ViewState.Item("CreatedUser") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("CreatedUser")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("CreatedUser", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CreatedUser") = Value.ToString
            End Set
        End Property
        Public Property ItemID() As Int64
            Get
                If Not ViewState("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(ViewState("ItemID"))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Int64)
                ViewState("ItemID") = Value.ToString
            End Set
        End Property

#End Region
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                'DotNetNuke.Framework.AJAX.RegisterScriptManager()

                'DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(Me.Parent, Me.lbtFind, Asc(vbCr))
                If Not IsPostBack Then
                    KeySearch = Request.Item("key")
                    Me.txtTitle.Value = Request.Item("key")
                    BindddlCategories()
                    If IsNumeric(Request.Item("catid")) Then
                        CategoryId = Request.Item("catid")
                        ddlCategory.SelectedValue = CategoryId
                    End If
                    CategoryId = ddlCategory.SelectedValue
                    BindDllUserPost()
                    If IsNumeric(Request.Item("uid")) Then
                        CreatedUser = Request.Item("uid")
                        ddlUserPost.SelectedValue = CreatedUser
                    End If
                    If IsDate(Request.Item("from")) Then
                        Datefrom = Request.Item("from")
                        If Not Datefrom = BL.minDateV Then
                            Me.txtStartdate.Value = Datefrom
                        End If
                    End If
                    If IsDate(Request.Item("to")) Then
                        DateTo = Request.Item("to")
                        If Not DateTo = BL.maxDateV Then
                            Me.txtEndDate.Value = DateTo
                        End If
                    End If
                    If Not Request.QueryString("trang") Is Nothing Then
                        CurrentPage = Integer.Parse(Request.QueryString("trang"))
                    End If
                    Dim requestedUrl As String = DirectCast(HttpContext.Current.Items()("UrlRewrite:OriginalUrl"), String)
                    If CurrentPage > 1 Then
                        requestedUrl += "?trang=" + CurrentPage.ToString
                    End If
                    BinddrgDataViewer()
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
        Private Sub lbtFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtFind.Click, ddlUserPost.SelectedIndexChanged, ddlCategory.SelectedIndexChanged
            Try
                KeySearch = txtTitle.Value
                If IsDate(txtStartdate.Value) Then
                    Datefrom = txtStartdate.Value
                End If
                If IsDate(txtEndDate.Value) Then
                    DateTo = txtEndDate.Value
                End If
                CategoryId = ddlCategory.SelectedValue
                CreatedUser = ddlUserPost.SelectedValue
                BinddrgDataViewer()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BinddrgDataViewer()
            Try
                Dim ctl As New NV_NewsController

                KeySearch = txtTitle.Value
                If IsDate(txtStartdate.Value) Then
                    Datefrom = txtStartdate.Value
                End If
                If IsDate(txtEndDate.Value) Then
                    DateTo = txtEndDate.Value
                End If
                CategoryId = ddlCategory.SelectedValue
                CreatedUser = ddlUserPost.SelectedValue

                TotalRecord = ctl.FindNews_Count(Datefrom, DateTo, KeySearch, CategoryId, False, PortalId, NewsStatus.DaXuatBan, CreatedUser, "")
                ctlPagingControl.TotalRecords = TotalRecord
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = CurrentPage
                ctlPagingControl.TabID = TabId
                ctlPagingControl.QuerystringParams = String.Empty

                rpttinlienquan.DataSource = ctl.FindByPhongBanStatus_Index(Datefrom, DateTo, KeySearch, CategoryId, 0, NewsStatus.DaXuatBan, CreatedUser, "", CurrentPage, PageSize)
                rpttinlienquan.DataBind()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindDllUserPost()
            Try
                Dim ctl As New PhongBanNguoiDungController
                Dim arr As ArrayList = ctl.GetPhongBanNguoiDung(0, -1)
                Me.ddlUserPost.DataSource = arr
                Me.ddlUserPost.DataTextField = "DisplayName"
                Me.ddlUserPost.DataValueField = "UserID"
                Me.ddlUserPost.DataBind()
                Me.ddlUserPost.Items.Insert(0, New ListItem("--Tất cả người dùng--", "0"))
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindddlCategories()
            Try
                Dim ctlNewsCategories As New NV_NewsCategoriesController
                Dim arrNewsCategories As New ArrayList
                arrNewsCategories = ctlNewsCategories.GetAll(0)
                Dim arrTemp As New ArrayList
                Dim objNewsCategories As NV_NewsCategoriesInfo
                Dim objNewsCategoriesTemp As NV_NewsCategoriesInfo

                If arrNewsCategories.Count > 0 Then
                    For Each objNewsCategories In arrNewsCategories
                        If objNewsCategories.ParentId = 0 Then
                            arrTemp.Add(objNewsCategories)
                            For Each objNewsCategoriesTemp In arrNewsCategories
                                If objNewsCategoriesTemp.ParentId = objNewsCategories.CategoryID Then
                                    objNewsCategoriesTemp.CategoryName = "--" & objNewsCategoriesTemp.CategoryName
                                    arrTemp.Add(objNewsCategoriesTemp)
                                End If
                            Next
                        End If
                    Next
                End If

                Me.ddlCategory.DataSource = arrTemp
                Me.ddlCategory.DataTextField = "CategoryName"
                Me.ddlCategory.DataValueField = "CategoryId"
                Me.ddlCategory.DataBind()
                Me.ddlCategory.Items.Insert(0, New ListItem("--Tất cả thư mục--", 0))
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

    End Class
End Namespace