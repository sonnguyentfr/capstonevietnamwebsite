Imports System
Imports DotNetNuke
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Display.Search

    Public MustInherit Class inc_SerachResult
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
        Property CurrentPage() As Int32 'Trang hiện tại
            Get
                If Not ViewState.Item("CurrentPage") Is Nothing Then
                    Return Int32.Parse(CType(ViewState.Item("CurrentPage"), String))
                Else
                    ViewState.Add("CurrentPage", "1")
                    Return 1
                End If
            End Get
            Set(ByVal value As Int32)
                ViewState.Item("CurrentPage") = value.ToString
            End Set
        End Property
        Property PageSize() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("PageSize") Is Nothing Then
                    Return CInt(ViewState.Item("PageSize"))
                Else
                    ViewState.Add("PageSize", "30")
                    Return 30
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
        Public Property Datefrom() As String
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return ViewState.Item("Datefrom")
                Else
                    Return BL.minDateV
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("Datefrom", value)
            End Set
        End Property
        Public Property DateTo() As String
            Get
                If Not ViewState.Item("todate") Is Nothing Then
                    Return ViewState.Item("todate")
                Else
                    Return BL.maxDateV
                End If
            End Get
            Set(ByVal value As String)
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
#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                'DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(Me.btnSearch, Asc(vbCr))
                If Not IsPostBack Then
                    Dim pSecurity As New PortalSecurity
                    KeySearch = pSecurity.InputFilter(Ultis.GetSafeRawUrl(Request.Item("q")), PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup Or PortalSecurity.FilterFlag.NoSQL Or PortalSecurity.FilterFlag.NoAngleBrackets)
                    Me.txtSearch.Text = pSecurity.InputFilter(Request.Item("q"), PortalSecurity.FilterFlag.NoScripting Or PortalSecurity.FilterFlag.NoMarkup)
                    If Ultis.isValidNumber(Request.Item("page")) Then
                        CurrentPage = CType(Request.Item("page"), Integer)
                    End If

                    If Not String.IsNullOrEmpty(KeySearch) Then
                        LoadData()
                    End If
                End If
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try
        End Sub
        Public Sub LoadData()
            Dim ctl As New NV_NewsController
            Try

                TotalRecord = ctl.FindContent_Count(1, 0, CType(Datefrom, Date), CType(DateTo, Date), KeySearch, CategoryId, "", 0, False, 0)
                If TotalRecord Mod 30 > 0 Then
                    vbPaging.TotalPage = TotalRecord \ 30 + 1
                Else
                    vbPaging.TotalPage = TotalRecord \ 30
                End If
                vbPaging.bindPages()

                drgNews.DataSource = ctl.FindContent_Index(1, 0, CType(Datefrom, Date), CType(DateTo, Date), KeySearch, CategoryId, CurrentPage, PageSize, "", 0, False, 0)
                drgNews.DataBind()

                txtSearch.Focus()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try
        End Sub
        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
            Response.Redirect(CType((NavigateURL(TabId) & "?q=" & txtSearch.Text), String))
        End Sub
        Function Highlight(ByVal InputTxt As String,
                    ByVal StartTag As String,
                    ByVal EndTag As String) As String

            Dim ResultStr As String = InputTxt
            Dim strArr As String() = KeySearch.Trim.Split(" ")
            For Each word As String In strArr
                ResultStr = Regex.Replace(ResultStr, "\b(" & Regex.Escape(word) & ")\b", StartTag & "$1" & EndTag, RegexOptions.IgnoreCase)
            Next
            Return ResultStr
        End Function

#End Region
    End Class
End Namespace