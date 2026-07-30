Imports System
Imports System.Web
Imports System.Web.UI
Imports DotNetNuke
Namespace BUH.Modules.FAQs

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
                TotalRecord = ctl._Find_Count("", BL.minDateV, BL.maxDateV, KeySearch, 3, PortalId)
                ctlPagingControl.TotalRecords = TotalRecord
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = CurrentPage
                ctlPagingControl.QuerystringParams = Ultis.GenerateQueryStringParameters(Request, BL.qsTimKiem)

                drgDataViewer.DataSource = ctl._Find_Index("", BL.minDateV, BL.maxDateV, KeySearch, 3, PortalId, CurrentPage, PageSize)
                drgDataViewer.DataBind()
                Me.lbTotalNewsFind.Text = TotalRecord
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub lbtDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtDelete.Click
            Dim i As Integer
            Dim chkBox As CheckBox
            Dim ctlNews As New uQuestion_Controller

            For i = 0 To (Me.drgDataViewer.Items.Count - 1)
                Try
                    chkBox = CType(Me.drgDataViewer.Items(i).FindControl("chkItems"), CheckBox)
                    If Not chkBox Is Nothing Then
                        If chkBox.Checked = True Then
                            ' Add product to display product detail
                            ctlNews._Delete(drgDataViewer.DataKeys(i).ToString(), PortalId)
                        End If
                    End If
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            Next
            binddrgDataViewer()
        End Sub
        Protected Sub lbtHuyXB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtHuyXB.Click
            Dim i As Integer
            Dim chkBox As CheckBox
            Dim ctlNews As New uQuestion_Controller
            For i = 0 To (Me.drgDataViewer.Items.Count - 1)
                Try
                    chkBox = CType(Me.drgDataViewer.Items(i).FindControl("chkItems"), CheckBox)
                    If Not chkBox Is Nothing Then
                        If chkBox.Checked = True Then
                            ' Add product to display product detail
                            ctlNews._UpdateStatus(drgDataViewer.DataKeys(i).ToString(), 1, PortalId)
                        End If
                    End If
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            Next
            BinddrgDataViewer()
        End Sub
        Function Highlight(ByVal InputTxt As String, _
                     ByVal StartTag As String, _
                     ByVal EndTag As String) As String

            Dim ResultStr As String = InputTxt
            Dim strArr As String() = KeySearch.Trim.Split(" ")
            For Each word As String In strArr
                ResultStr = Regex.Replace(ResultStr, "\b(" & Regex.Escape(word) & ")\b", StartTag & "$1" & EndTag, RegexOptions.IgnoreCase)
            Next

            Return ResultStr
        End Function
        Protected Sub drgDataViewer_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles drgDataViewer.ItemDataBound
            If e.Item.ItemType = ListItemType.Header Then
                Dim chkUserAll As CheckBox
                'If Not dgrProducts. Is Nothing Then
                chkUserAll = CType(e.Item.FindControl("chkItemsTop"), CheckBox)
                If Not chkUserAll Is Nothing Then
                    chkUserAll.Attributes.Add("OnClick", "javascript:IsCheckBoxSelected(" & drgDataViewer.ClientID & "," & chkUserAll.ClientID & ")")
                End If
                'End If
            End If
        End Sub
    End Class

End Namespace
