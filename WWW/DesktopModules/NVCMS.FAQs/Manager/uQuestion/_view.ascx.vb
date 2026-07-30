Imports System
Imports System.Web
Imports System.Web.UI
Imports DotNetNuke
Imports DotNetNuke.UI.Utilities

Namespace NVCMS.Modules.FAQs

    Public MustInherit Class inc_list
        Inherits Entities.Modules.PortalModuleBase
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
                    ViewState.Add("PageSize", "20")
                    Return 20
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
        Property Status() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("Status") Is Nothing Then
                    Return CInt(ViewState.Item("Status"))
                Else
                    ViewState.Add("Status", "0")
                    Return 0
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("Status") = value.ToString
            End Set
        End Property
#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(Me.Parent, Me.lbtFind, Asc(vbCr))
                If Not IsPostBack Then
                    KeySearch = Request.Item("key")
                    Me.txtTitle.Text = Request.Item("key")
                    If IsNumeric(Request.Item("status")) Then
                        Status = Request.Item("status")
                        ddlStatus.SelectedValue = Status
                    End If
                    If IsNumeric(Request.Item("currentpage")) Then
                        CurrentPage = Request.Item("currentpage")
                    End If
                    BinddrgDataViewer()
                    txtTitle.Focus()
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
        Private Sub lbtFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtFind.Click, ddlStatus.SelectedIndexChanged
            Response.Redirect(NavigateURL(TabId) & "?key=" & txtTitle.Text & "&status=" & ddlStatus.SelectedValue)
        End Sub
        Private Sub BinddrgDataViewer()
            Try
                Dim ctl As New uQuestion_Controller
                TotalRecord = ctl._Find_Count("", BL.minDateV, BL.maxDateV, KeySearch, Status, PortalId)
                ctlPagingControl.TotalRecords = TotalRecord
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = CurrentPage
                ctlPagingControl.QuerystringParams = Ultis.GenerateQueryStringParameters(Request, BL.qsTimKiem)

                drgDataViewer.DataSource = ctl._Find_Index("", BL.minDateV, BL.maxDateV, KeySearch, Status, PortalId, CurrentPage, PageSize)
                drgDataViewer.DataBind()
                Me.lbTotalNewsFind.Text = TotalRecord
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub lbtAddBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAddBottom.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub lbtAddTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAddTop.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
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
        ''' <summary>
        ''' Quick View
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub cmdquickview(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim itemidhistory As Integer = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            Dim ctlNews As New uQuestion_Controller
            Dim objhistory As uQuestion_Info = ctlNews._GetByID(itemidhistory, PortalId)
            If Not objhistory Is Nothing Then
                With objhistory
                    Me.lblhAuthor.Text = .UserName
                    ltlngaydang.Text = .CreatedDate.ToString("HH:mm dd/MM/yyy")
                    ltrcauhoi.Text = .Question
                    ltrcautraloi.Text = Server.HtmlDecode(.Traloi)

                End With
            End If
            ClientAPI.RegisterStartUpScript(Me.Page, "OpenDialogHistory", "<script>OpenDialogHistory();</script>")

        End Sub
        Function GetStatus(ByVal statusid As Integer) As String
            Dim ResultStr As String = ""
            If statusid = 1 Then
                ResultStr = "<span class='badge badge-dot badge-danger'>Vừa tiếp nhận</span>"
            End If
            If statusid = 2 Then
                ResultStr = "<span class='badge badge-dot badge-warning'>Đang xử lý</span>"
            End If
            If statusid = 3 Then
                ResultStr = "<span class='badge badge-dot badge-success'>Đã xuất bản</span>"
            End If
            Return ResultStr
        End Function
    End Class

End Namespace
