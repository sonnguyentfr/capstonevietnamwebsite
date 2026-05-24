Imports System
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.Hethong
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Manager.newsapprove

    Public MustInherit Class Approve_inc_list
        Inherits Entities.Modules.PortalModuleBase
        Dim _NewsNoteController As New NewsNoteController
        Public PageSuaBai As String = "#"
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
        Public Property Datefrom() As String
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return ViewState.Item("Datefrom")
                Else
                    Return "01/01/2010"
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
                    Return "01/01/2100"
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
        Public Property Status() As Integer
            Get
                If Not ViewState.Item("Status") Is Nothing Then
                    Dim x As Integer = -1
                    Try : x = CInt(ViewState.Item("Status")) : Catch ex As Exception : x = -1 : End Try
                    Return x
                Else
                    ViewState.Add("Status", "-1")
                    Return -1
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("Status") = Value.ToString
            End Set
        End Property
        Public Property ItemID() As Integer
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
            Set(ByVal Value As Integer)
                ViewState("ItemID") = Value.ToString
            End Set
        End Property
#End Region
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                'Kiem tra xem dang o trang nao
                Dim activetab As Integer = PortalSettings.ActiveTab.TabID
                If activetab = BL.pagePheDuyetId Then
                    PageSuaBai = BL.pageSuaPheDuyet

                End If
                If activetab = BL.pagePheDuyetXBId Then
                    PageSuaBai = BL.pageSuaPheDuyetXB
                End If
                If activetab = BL.pageDaXuatBanId Then
                    PageSuaBai = BL.pageDaXuatBanSua

                End If
                DotNetNuke.UI.Utilities.ClientAPI.RegisterKeyCapture(Me.Parent, Me.lbtFind, Asc(vbCr))
                If Not IsPostBack Then
                    KeySearch = Request.Item("key")
                    Me.txtTitle.Text = Request.Item("key")
                    Me.txtTitle2.Value = Request.Item("key")
                    If IsDate(Request.Item("from")) Then
                        Datefrom = Request.Item("from")
                        If Not Datefrom = BL.minDateV Then
                            Me.txtStartdate.Text = Datefrom
                        End If
                    End If
                    If IsDate(Request.Item("to")) Then
                        DateTo = Request.Item("to")
                        If Not DateTo = BL.maxDateV Then
                            Me.txtEndDate.Text = DateTo
                        End If
                    End If
                    BindddlCategories()
                    If IsNumeric(Request.Item("catid")) Then
                        CategoryId = Request.Item("catid")
                        ddlCategory.SelectedValue = CategoryId
                    End If
                    BindDllUserPost()
                    If IsNumeric(Request.Item("uid")) Then
                        CreatedUser = Request.Item("uid")
                        ddlUserPost.SelectedValue = CreatedUser
                    End If
                    If IsNumeric(Request.Item("pageNo")) Then
                        PageSize = Request.Item("pageNo")
                        ddlPageSize.SelectedValue = PageSize
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
        Private Sub BinddrgDataViewer()
            Try
                Dim ctl As New NV_NewsController
                TotalRecord = ctl.FindByPhongBanStatus_Count(Datefrom, DateTo, KeySearch, CategoryId, PortalId, NewsStatus.DaXuatBan, CreatedUser, "")
                ctlPagingControl.TotalRecords = TotalRecord
                ctlPagingControl.PageSize = PageSize
                ctlPagingControl.CurrentPage = CurrentPage
                ctlPagingControl.TabID = TabId
                ctlPagingControl.QuerystringParams = Ultis.GenerateQueryStringParameters(Request, BL.qsTimKiem)

                Dim arrResult As New ArrayList
                Dim arrChinhThong As New ArrayList
                Dim arrNhoXL As New ArrayList
                Dim arr As ArrayList = ctl.FindByPhongBanStatus_Index(Datefrom, DateTo, KeySearch, CategoryId, PortalId, NewsStatus.DaXuatBan, CreatedUser, "", CurrentPage, PageSize)
                drgDataViewer.DataSource = arr
                drgDataViewer.DataBind()

                Me.lbTotalNewsCount.Text = CType(TotalRecord, String)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindddlCategories()
            Dim ctlNewsCategories As New NV_NewsCategoriesController
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = ctlNewsCategories.GetAll(PortalId)
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
            Me.ddlCategory.Items.Insert(0, New ListItem("--Tất cả chuyên mục--", "0"))
        End Sub
        Private Sub BindDllUserPost()
            'Me.ddlUserPost.DataSource = UserController.GetUsers(PortalId)
            'Me.ddlUserPost.DataTextField = "Username"
            'Me.ddlUserPost.DataValueField = "UserID"
            'Me.ddlUserPost.DataBind()
            'Me.ddlUserPost.Items.Insert(0, New ListItem("-- Chọn tác giả --", "0"))
            Dim arrU As New ArrayList
            arrU = UserController.GetUsers(PortalId)
            Dim arrUNews As New ArrayList
            If arrU.Count > 0 Then
                For i As Integer = 0 To arrU.Count - 1
                    Dim obju As UserInfo = CType(arrU(i), UserInfo)
                    With obju
                        If .IsDeleted = False Then
                            If .Membership.Approved = True Then
                                arrUNews.Add(obju)
                            End If

                        End If
                    End With

                Next
            End If
            Me.ddlUserPost.DataSource = arrUNews
            Me.ddlUserPost.DataTextField = "DisplayName"
            Me.ddlUserPost.DataValueField = "UserID"
            Me.ddlUserPost.DataBind()
            Me.ddlUserPost.Items.Insert(0, New ListItem("--Tất cả--", "0"))
        End Sub
        Private Sub lbtFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtFind.Click, ddlPageSize.SelectedIndexChanged, ddlCategory.SelectedIndexChanged, ddlUserPost.SelectedIndexChanged
            Try
                Response.Redirect(NavigateURL(TabId) & "?key=" & txtTitle.Text & "&catid=" & ddlCategory.SelectedValue & "&uid=" & ddlUserPost.SelectedValue & "&from=" & Me.txtStartdate.Text & "&to=" & Me.txtEndDate.Text & "&pageNo=" & ddlPageSize.SelectedValue)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub lbtFindTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtFindTitle.Click, ddlPageSize.SelectedIndexChanged, ddlCategory.SelectedIndexChanged
            Response.Redirect(NavigateURL(TabId) & "?key=" & txtTitle2.Value & "&catid=" & ddlCategory.SelectedValue & "&from=" & Me.txtStartdate.Text & "&to=" & Me.txtEndDate.Text & "&pageNo=" & ddlPageSize.SelectedValue)
        End Sub
        'Private Sub drgDataViewer_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles drgDataViewer.ItemCommand
        '    Try
        '        Select Case e.CommandName
        '            Case "cmdDelete"
        '            Case "cmdApprove"
        '                Dim ctlNews As New NV_NewsController
        '                Dim ctlProcess As New NewsProcessController
        '                Dim objProcessInfo As New NewsProcessInfo

        '                Dim newsid As Integer = Me.drgDataViewer.DataKeys(e.Item.ItemIndex)
        '                ctlNews.UpdateStatus(newsid, NewsStatus.ChoXuatBan, UserId)
        '                'Version
        '                With objProcessInfo
        '                    .NewsID = newsid
        '                    .CreateDate = DateTime.Now
        '                    .ByUser = UserId
        '                    .StatusID = NewsStatus.ChoXuatBan
        '                    .ProcessName = BL.msgProcessPheDuyet
        '                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
        '                End With
        '                ctlProcess.Insert(objProcessInfo)
        '                'Rebind
        '                BinddrgDataViewer()
        '            Case "cmdUnlock"
        '                '1. Unlock
        '                Dim newid As Integer = Integer.Parse(e.CommandArgument)
        '                Ultis.UnlockNews(newid, UserId)
        '                '2. Process
        '                Dim ctlNews As New NV_NewsController
        '                Dim ctlProcess As New NewsProcessController
        '                Dim objProcessInfo As New NewsProcessInfo
        '                With objProcessInfo
        '                    .NewsID = newid
        '                    .CreateDate = DateTime.Now
        '                    .ByUser = UserId
        '                    .StatusID = ctlNews.GetByID(newid).Status
        '                    .ProcessName = BL.msgProcessUnlockNews
        '                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
        '                End With
        '                ctlProcess.Insert(objProcessInfo)
        '                '3. Rebind
        '                BinddrgDataViewer()
        '        End Select
        '    Catch ex As Exception
        '        ProcessModuleLoadException(Me, ex)
        '    End Try
        'End Sub
        'Protected Sub drgDataViewer_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles drgDataViewer.ItemDataBound
        '    If e.Item.ItemType = ListItemType.Header Then
        '        Dim chkUserAll As CheckBox
        '        'If Not dgrProducts. Is Nothing Then
        '        chkUserAll = CType(e.Item.FindControl("chkItemsTop"), CheckBox)
        '        If Not chkUserAll Is Nothing Then
        '            chkUserAll.Attributes.Add("OnClick", "javascript:IsCheckBoxSelected(" & drgDataViewer.ClientID & "," & chkUserAll.ClientID & ")")
        '        End If
        '        'End If
        '    End If
        'End Sub
        'Protected Sub lbtApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtApprove.Click
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
        '                    ctlNews.UpdateStatus(newsid, NewsStatus.ChoXuatBan, UserId)
        '                    'Clear news
        '                    Dim ctlUserNews As New News_UserProcessController
        '                    ctlUserNews.DeleteByNewsID(newsid)
        '                    'Process
        '                    With objProcessInfo
        '                        .NewsID = newsid
        '                        .CreateDate = DateTime.Now
        '                        .ByUser = UserId
        '                        .StatusID = NewsStatus.ChoXuatBan
        '                        .ProcessName = BL.msgProcessPheDuyet
        '                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
        '                    End With
        '                    ctlProcess.Insert(objProcessInfo)
        '                End If
        '            End If
        '        Catch ex As Exception
        '            ProcessModuleLoadException(Me, ex)
        '        End Try
        '    Next

        '    'CurrentPage = 1
        '    BinddrgDataViewer()
        'End Sub
        'Protected Sub lbtSendBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtSendBack.Click
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
        '                    ctlNews.UpdateStatus(newsid, NewsStatus.BiTraLai, UserId)
        '                    'Clear news
        '                    Dim ctlUserNews As New News_UserProcessController
        '                    ctlUserNews.DeleteByNewsID(newsid)
        '                    'Process
        '                    With objProcessInfo
        '                        .NewsID = newsid
        '                        .CreateDate = DateTime.Now
        '                        .ByUser = UserId
        '                        .StatusID = NewsStatus.BiTraLai 'Bi tra lai
        '                        .ProcessName = BL.msgProcessTraLai
        '                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
        '                    End With
        '                    ctlProcess.Insert(objProcessInfo)
        '                End If
        '            End If
        '        Catch ex As Exception
        '            ProcessModuleLoadException(Me, ex)
        '        End Try
        '    Next

        '    'CurrentPage = 1
        '    BinddrgDataViewer()
        'End Sub
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
#Region "Mo khoa tin bai"
        Protected Sub GetUnlockNews(sender As Object, e As EventArgs)
            Try
                Dim newid As Integer = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
                Ultis.UnlockNews(newid, UserId)
                '2. Process
                Dim ctlNews As New NV_NewsController
                Dim ctlProcess As New NewsProcessController
                Dim objProcessInfo As New NewsProcessInfo
                With objProcessInfo
                    .NewsID = newid
                    .CreateDate = DateTime.Now
                    .ByUser = UserId
                    .StatusID = ctlNews.GetByID(newid).Status
                    .ProcessName = BL.msgProcessUnlockNews
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                End With
                ctlProcess.Insert(objProcessInfo)
                '3. Rebind
                'Xoa cache
                'Dim cacheName As String = "AdminTinCuaToi" & KeySearch & Datefrom & DateTo & CategoryId & Status & UserId & CurrentPage
                'HttpCacheHelper.RemoveCache(cacheName)
                BinddrgDataViewer()
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Mở khóa tin bài thành công!');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
#Region "history"
        Protected Sub cmdSethistory(ByVal sender As Object, ByVal e As System.EventArgs)
            '1. Unlock
            Dim itemidhistory As Integer = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            Dim ctlNews As New NV_NewsController
            Dim objhistory As NV_NewsInfo = ctlNews.GetByID(itemidhistory)

            Me.lblhNewsTitle.Text = itemidhistory
            lblhNewsTitle.Text = objhistory.Title
            lblhAuthor.Text = BL.GetUserName(PortalId, objhistory.UserId)
            Dim ctlhh As New NewsProcessController
            Dim ds As ArrayList
            ds = ctlhh.GetByNewsId(itemidhistory)

            Me.drgDataViewerHistory.DataSource = ds
            Me.drgDataViewerHistory.DataBind()
            ClientAPI.RegisterStartUpScript(Me.Page, "OpenDialogHistory", "<script>OpenDialogHistory();</script>")

        End Sub
        Public Function FormatVisible(ByVal id As Object) As String
            If IsNumeric(id) AndAlso id > 0 Then
                Return "True"
            Else
                Return "False"
            End If
        End Function
        Public Function GetUserName(ByVal userid As Integer) As String
            Return "(" + BL.GetNameByUserId(PortalId, userid) + ")"
        End Function
#End Region
#Region "Sửa ngày xuất bản"
        Protected Sub cmdSuaNgayXuatBan(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                '1. Unlock
                ItemID = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
                Dim ctlNews As New NV_NewsController
                Dim objhistory As NV_NewsInfo = ctlNews.GetByID(ItemID)
                If Not objhistory Is Nothing Then
                    With objhistory
                        Me.lbNews.Text = .Title
                        lbUserCreated.Text = BL.GetNameByUserId(PortalId, .UserId)
                        lblCreatedDate.Text = .CreateDate.ToString("HH:mm dd/MM/yyyy")
                        If Not Null.IsNull(.PublishedDate) Then
                            txtNgayXL.Text = .PublishedDate.ToString("dd/MM/yyyy")
                            txtGioXL.SelectedValue = .PublishedDate.Hour
                            txtPhutXL.Text = .PublishedDate.Minute
                        End If
                        ClientAPI.RegisterStartUpScript(Me.Page, "OpenDialogSuaNgayXuatBan", "<script>OpenDialogSuaNgayXuatBan();</script>")
                    End With
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub
        Protected Sub lbtXBSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtXBSave.Click
            Try
                Dim ngayXB As DateTime = DateTime.Parse(txtNgayXL.Text)
                Dim publishedDate As DateTime = New DateTime(ngayXB.Year, ngayXB.Month, ngayXB.Day, txtGioXL.SelectedValue, txtPhutXL.Text, 0)
                Dim ctlNews As New NV_NewsController
                Dim ctlProcess As New NewsProcessController
                Dim objProcessInfo As New NewsProcessInfo
                'Dim itemidxb As Integer = Integer.Parse(hdf_idkhachhang.Value)
                ctlNews.UpdatePublishedDate(ItemID, publishedDate, UserId)
                'Cap nhat lai bang News_PUblish
                ctlNews.UpdatePublishedDate(ItemID, publishedDate, UserId)
                'Process
                With objProcessInfo
                    .NewsID = ItemID
                    .CreateDate = DateTime.Now
                    .ByUser = UserId
                    .StatusID = NewsStatus.DaXuatBan
                    .ProcessName = BL.msgProcessXuatBan
                    .Comment = String.Empty
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                End With
                ctlProcess.Insert(objProcessInfo)
                BinddrgDataViewer()
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật ngày xuất bản thành công!');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Cập nhật t/g xuất bản thất bại');</script>")
            End Try
            ClientAPI.RegisterStartUpScript(Me.Page, "CloseDialogSuaNgayXuatBan", "<script>CloseDialogSuaNgayXuatBan();</script>")
        End Sub
        Protected Sub lbtXBCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtXBCancel.Click
            ItemID = 0
            ClientAPI.RegisterStartUpScript(Me.Page, "CloseDialog", "<script>CloseDialog();</script>")
        End Sub
#End Region
#Region "Thông tin bút phê"
        Protected Sub cmdButPhe(ByVal sender As Object, ByVal e As System.EventArgs)
            '1. Unlock
            Dim itemidhistory As Integer = Integer.Parse(TryCast(sender, LinkButton).CommandArgument)
            Dim arrNewsNote As New ArrayList
            arrNewsNote = _NewsNoteController.News_Note_GetByNewId(itemidhistory)
            rptNotes.DataSource = arrNewsNote
            rptNotes.DataBind()
            ClientAPI.RegisterStartUpScript(Me.Page, "OpenDialogNewsNotes", "<script>OpenDialogNewsNotes();</script>")

        End Sub
#End Region
    End Class
End Namespace