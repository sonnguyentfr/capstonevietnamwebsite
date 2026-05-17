Imports BUH.Modules.Users
Imports BUH.Modules.Events

Namespace DesktopModules.TinTuc.Control
    Partial Class Choose
        Inherits CDefault

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
        Private Property Datefrom() As Date
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
#Region "Handlers"
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                'DotNetNuke.Framework.AJAX.RegisterScriptManager()
                If Request.IsAuthenticated Then
                    DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(Me.Parent, Me.lbtFind, Asc(vbCr))
                    If Not IsPostBack Then
                        BindddlCategories()
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
                Else
                    'Response.Redirect("/")
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BinddrgDataViewer()
            Try
                Dim ctl As New EventsController
                TotalRecord = ctl.Events_Find_Count("", Datefrom, DateTo, KeySearch, CategoryId, PortalSettings.PortalId, 1, 0)
                ctlPagingControl.TotalRecords = TotalRecord
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = CurrentPage
                ctlPagingControl.TabID = -1
                ctlPagingControl.QuerystringParams = Ultis.GenerateQueryStringParameters(Request, BL.qsTimKiem)

                drgDataViewer.DataSource = ctl.Events_Find_Index("", Datefrom, DateTo, KeySearch, CategoryId, PortalSettings.PortalId, 1, 0, CurrentPage, PageSize)
                drgDataViewer.DataBind()

                Me.lbTotalNewsCount.Text = TotalRecord.ToString()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindddlCategories()
            Dim ctlVideosCategories As New Events_CatController
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = ctlVideosCategories.Events_Cat_GetAll(PortalSettings.PortalId)
            Me.ddlCategory.DataSource = arrNewsCategories
            Me.ddlCategory.DataTextField = "CatName"
            Me.ddlCategory.DataValueField = "Id"
            Me.ddlCategory.DataBind()
            Me.ddlCategory.Items.Insert(0, New ListItem("--Tất cả chuyên mục--", 0))
        End Sub
        Private Sub lbtFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtFind.Click, ddlPageSize.SelectedIndexChanged, ddlCategory.SelectedIndexChanged
            Try
                KeySearch = txtTitle.Text
                If IsDate(txtStartdate.Text) Then
                    Datefrom = txtStartdate.Text
                End If
                If IsDate(txtEndDate.Text) Then
                    DateTo = txtEndDate.Text
                End If
                CategoryId = ddlCategory.SelectedValue
                PageSize = ddlPageSize.SelectedValue

                BinddrgDataViewer()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub drgDataViewer_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles drgDataViewer.ItemDataBound
            If e.Item.ItemType = ListItemType.Header Then
                Dim chkUserAll As CheckBox
                chkUserAll = CType(e.Item.FindControl("chkItemsTop"), CheckBox)
                If Not chkUserAll Is Nothing Then
                    'chkUserAll.Attributes.Add("OnClick", "javascript:IsCheckBoxSelected(" & drgDataViewer.ClientID & "," & chkUserAll.ClientID & ")")
                End If
            End If
        End Sub
        'Protected Sub lbtCancelPublish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtCancelPublish.Click
        '    Dim i As Integer
        '    Dim chkBox As CheckBox
        '    Dim ctlNews As New NV_NewsController
        '    Dim ctlProcess As New NewsProcessController
        '    Dim objProcessInfo As New NewsProcessInfo

        '    For i = 0 To (Me.drgDataViewer.Items.Count - 1)
        '        Try
        '            chkBox = CType(Me.drgDataViewer.Items(i).FindControl("chkItems"), CheckBox)
        '            If Not chkBox Is Nothing Then
        '                If chkBox.Checked = True Then
        '                    Dim newsid As Integer = drgDataViewer.DataKeys(i)
        '                    '1. Update Status

        '                End If
        '            End If
        '        Catch ex As Exception
        '            ProcessModuleLoadException(Me, ex)
        '        End Try
        '    Next

        '    BinddrgDataViewer()
        'End Sub
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
#End Region
#Region "Countdown"
        Public Function GetdateTime(id As Integer) As String
            Dim obj As EventsInfo
            Dim ctl As New EventsController
            obj = ctl.Events_GetByID(id, PortalSettings.PortalId)
            Return obj.fromdatetime.ToString("HH:mm") & " " & obj.fromdatetime.ToString("dddd") & " - " & obj.fromdatetime.ToString("d/MM/yyyy")

        End Function
        Public Function CoutDowntime(id As Integer) As String
            Dim obj As EventsInfo
            Dim ctl As New EventsController
            obj = ctl.Events_GetByID(id, PortalSettings.PortalId)
            Dim strFlv As String = "<script type='text/javascript'> " _
            & "$(function () {" _
            & "var austDay = new Date();" _
            & "austDay = new Date(__videoLink__);" _
            & "$('#defaultCountdown____id___').countdown({until: austDay});" _
            & "$('#year').text(austDay.getFullYear());" _
            & "});" _
            & "</script>"
            strFlv = strFlv.Replace("__videoLink__", obj.fromdatetime.ToString("yyyy") & "," & CInt(obj.fromdatetime.ToString("MM")) - 1 & "," & obj.fromdatetime.ToString("dd") & "," & obj.fromdatetime.ToString("HH") & "," & obj.fromdatetime.ToString("mm"))
            strFlv = strFlv.Replace("____id___", obj.id)
            Return strFlv
        End Function
#End Region
    End Class
End Namespace