Imports DotNetNuke.UI.Utilities
Imports Lucene.Net.Analysis.De
Imports NVCMS.Modules.EventsWebsite
Imports NVCMS.Modules.Student

Namespace NVCMS.Modules.Marketing

    Public MustInherit Class ZaloListSdtViewer
        Inherits Entities.Modules.PortalModuleBase

#Region "Controllers"
        Private _campaignCtl As New Marketing_Zalo_Campaign_Controller()
        Private _listSdtCtl As New Marketing_Zalo_ListSdt_Controller()
        Private _eventsCatCtl As New EventsWebsite_CatController()
        Private _eventsCtl As New EventsWebsiteController()
        Private _eventsStudentCtl As New EventsStudentWebsiteController()
        Private _studentInfoController As New StudentInfoController
#End Region

#Region "Helper class"
        Private Class SdtPreviewItem
            Public Property StudentFullname As String = ""
            Public Property SdtRaw As String = ""
            Public Property SdtNormalized As String = ""
            Public Property SdtStatus As String = ""
        End Class
#End Region

#Region "ViewState properties"
        Public ReadOnly Property CampaignId() As Integer
            Get
                Dim val As String = Request.QueryString("campaignId")
                Dim id As Integer = 0
                Integer.TryParse(val, id)
                Return id
            End Get
        End Property

        Public ReadOnly Property BackUrl() As String
            Get
                Return NavigateURL()
            End Get
        End Property

        Private Property VS_EventCatId() As Integer
            Get
                If ViewState("VS_EventCatId") IsNot Nothing Then Return CInt(ViewState("VS_EventCatId"))
                Return 0
            End Get
            Set(ByVal v As Integer)
                ViewState("VS_EventCatId") = v
            End Set
        End Property

        Private Property VS_EventId() As Integer
            Get
                If ViewState("VS_EventId") IsNot Nothing Then Return CInt(ViewState("VS_EventId"))
                Return 0
            End Get
            Set(ByVal v As Integer)
                ViewState("VS_EventId") = v
            End Set
        End Property

        Private Property VS_Checkin() As Integer
            Get
                If ViewState("VS_Checkin") IsNot Nothing Then Return CInt(ViewState("VS_Checkin"))
                Return -1
            End Get
            Set(ByVal v As Integer)
                ViewState("VS_Checkin") = v
            End Set
        End Property
#End Region

#Region "Page Load"
        Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            If CampaignId <= 0 Then
                Response.Redirect(NavigateURL())
                Return
            End If

            If Not IsPostBack Then
                Dim camp = _campaignCtl._GetByID(CampaignId)
                If camp IsNot Nothing Then ltrCampaignTitle.Text = camp.Title

                BindddlEventCat()
                BindddlCheckin()
                BindListSdt()
            End If
        End Sub
#End Region

#Region "Bind helpers"
        Private Sub BindddlEventCat()
            Dim arr As ArrayList = _eventsCatCtl.Events_Cat_GetAll(50)
            ddlEventCat.DataSource = arr
            ddlEventCat.DataTextField = "CatName"
            ddlEventCat.DataValueField = "id"
            ddlEventCat.DataBind()
            ddlEventCat.Items.Insert(0, New ListItem("-- CHỌN SỰ KIỆN --", "0"))
        End Sub

        Private Sub BindddlEvent(ByVal catId As Integer)
            ddlEvent.Items.Clear()
            If catId > 0 Then
                Dim arr As ArrayList = _eventsCtl.Events_GetAllByCat(catId, 50)
                ddlEvent.DataSource = arr
                ddlEvent.DataTextField = "Title"
                ddlEvent.DataValueField = "id"
                ddlEvent.DataBind()
            End If
            ddlEvent.Items.Insert(0, New ListItem("-- TẤT CẢ ĐỊA ĐIỂM --", "0"))
        End Sub

        Private Sub BindddlCheckin()
            ddlCheckin.Items.Clear()
            ddlCheckin.Items.Add(New ListItem("-- TẤT CẢ TRẠNG THÁI --", "-1"))
            ddlCheckin.Items.Add(New ListItem("ĐÃ CHECK-IN", "1"))
            ddlCheckin.Items.Add(New ListItem("KHÔNG CHECK-IN", "0"))
        End Sub

        Private Sub BindListSdt()
            Dim keySearch As String = txtSearch.Text.Trim()
            Dim statusFilter As Integer = CInt(ddlStatusFilter.SelectedValue)
            Dim arr As ArrayList = _listSdtCtl._GetAll(CampaignId, keySearch, statusFilter, 0, 100000)
            rptListSdt.DataSource = arr
            rptListSdt.DataBind()
            ltrTotal.Text = arr.Count.ToString()
        End Sub

        Public Function GetStatusLabel(ByVal s As Integer) As String
            Select Case s
                Case 0 : Return "Chờ gửi"
                Case 1 : Return "Đã gửi"
                Case 2 : Return "Lỗi"
                Case Else : Return "N/A"
            End Select
        End Function

        Public Function GetStatusBadge(ByVal s As Integer) As String
            Select Case s
                Case 0 : Return "badge-warning"
                Case 1 : Return "badge-success"
                Case 2 : Return "badge-danger"
                Case Else : Return "badge-light"
            End Select
        End Function
#End Region

#Region "Chọn sự kiện → load địa điểm"
        Public Sub ddlEventCat_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEventCat.SelectedIndexChanged
            Dim catId As Integer = CInt(ddlEventCat.SelectedValue)
            VS_EventCatId = catId
            BindddlEvent(catId)
            rptPreviewEvent.DataSource = Nothing
            rptPreviewEvent.DataBind()
            lbtImportFromEvent.Visible = False
            ResetCounters()
        End Sub
#End Region

#Region "XEM DANH SÁCH (preview từ sự kiện)"
        Private Sub lbtXemDanhSach_Click(sender As Object, e As EventArgs) Handles lbtXemDanhSach.Click
            Dim catId As Integer = CInt(ddlEventCat.SelectedValue)
            Dim eventId As Integer = CInt(ddlEvent.SelectedValue)
            Dim checkin As Integer = CInt(ddlCheckin.SelectedValue)

            VS_EventCatId = catId
            VS_EventId = eventId
            VS_Checkin = checkin

            If catId = 0 Then
                ClientAPI.RegisterStartUpScript(Me.Page, "err", "<script>alert('Vui lòng chọn sự kiện!');</script>")
                Return
            End If

            ' SĐT đã tồn tại trong campaign
            Dim existingPhones As New List(Of String)()
            Dim arrCurrent As ArrayList = _listSdtCtl._GetAll(CampaignId, "", -1, 0, 100000)
            If arrCurrent IsNot Nothing Then
                For Each item As Marketing_Zalo_ListSdtInfo In arrCurrent
                    If Not String.IsNullOrWhiteSpace(item.Phone) Then existingPhones.Add(item.Phone)
                Next
            End If

            ' Lấy sinh viên theo sự kiện / địa điểm / trạng thái checkin
            Dim arrStudents As ArrayList = _eventsStudentCtl.Events_Student_FindIndexByEvent(
                eventId, catId, checkin, -1, 1, 100000)

            Dim previewList As New List(Of SdtPreviewItem)()
            Dim okCount As Integer = 0
            Dim errCount As Integer = 0
            Dim dupCount As Integer = 0

            If arrStudents IsNot Nothing Then
                For Each sv As EventsStudentWebsiteInfo In arrStudents
                    Dim rawSdt As String = sv.StudentSodienthoai.Trim()
                    Dim pi As New SdtPreviewItem()
                    pi.StudentFullname = sv.StudentFullname
                    pi.SdtRaw = rawSdt

                    If String.IsNullOrWhiteSpace(rawSdt) Then
                        pi.SdtNormalized = ""
                        pi.SdtStatus = "LỖI"
                        errCount += 1
                    Else
                        Dim errMsg As String = String.Empty
                        Dim normalized As String = ZaloPhoneHelper.ValidateAndNormalize(rawSdt, errMsg)
                        If normalized Is Nothing Then
                            pi.SdtNormalized = rawSdt
                            pi.SdtStatus = "LỖI"
                            errCount += 1
                        ElseIf existingPhones.Contains(normalized) Then
                            pi.SdtNormalized = normalized
                            pi.SdtStatus = "TRÙNG"
                            dupCount += 1
                        Else
                            pi.SdtNormalized = normalized
                            pi.SdtStatus = "OK"
                            okCount += 1
                        End If
                    End If

                    previewList.Add(pi)
                Next
            End If

            rptPreviewEvent.DataSource = previewList
            rptPreviewEvent.DataBind()
            ltrOK.Text = okCount.ToString()
            ltrLoi.Text = errCount.ToString()
            ltrTrung.Text = dupCount.ToString()
            lbtImportFromEvent.Visible = (okCount > 0)

            ClientAPI.RegisterStartUpScript(Me.Page, "ok",
                "<script>UpdateSuccess('Tìm thấy " & previewList.Count & " sinh viên, " & okCount & " SĐT hợp lệ');</script>")
        End Sub
#End Region

#Region "THÊM VÀO DANH SÁCH (từ sự kiện)"
        Private Sub lbtImportFromEvent_Click(sender As Object, e As EventArgs) Handles lbtImportFromEvent.Click
            Dim catId As Integer = VS_EventCatId
            Dim eventId As Integer = VS_EventId
            Dim checkin As Integer = VS_Checkin
            If catId = 0 Then Return

            Dim existingPhones As New List(Of String)()
            Dim arrCurrent As ArrayList = _listSdtCtl._GetAll(CampaignId, "", -1, 0, 100000)
            If arrCurrent IsNot Nothing Then
                For Each item As Marketing_Zalo_ListSdtInfo In arrCurrent
                    If Not String.IsNullOrWhiteSpace(item.Phone) Then existingPhones.Add(item.Phone)
                Next
            End If

            Dim arrStudents As ArrayList = _eventsStudentCtl.Events_Student_FindIndexByEvent(
                eventId, catId, checkin, -1, 1, 100000)

            Dim insertCount As Integer = 0
            If arrStudents IsNot Nothing Then
                For Each sv As EventsStudentWebsiteInfo In arrStudents
                    Dim fullname As String = sv.StudentFullname
                    Dim rawSdt As String = sv.StudentSodienthoai
                    If String.IsNullOrWhiteSpace(rawSdt) Then Continue For

                    Dim errMsg As String = String.Empty
                    Dim normalized As String = ZaloPhoneHelper.ValidateAndNormalize(rawSdt, errMsg)
                    If normalized Is Nothing Then Continue For
                    If existingPhones.Contains(normalized) Then Continue For

                    _listSdtCtl._Insert(CampaignId, fullname, rawSdt, normalized, 0, DateTime.Now, UserId, 50)
                    existingPhones.Add(normalized)
                    insertCount += 1
                Next
            End If

            BindListSdt()
            lbtImportFromEvent.Visible = False
            ClientAPI.RegisterStartUpScript(Me.Page, "ok",
                "<script>UpdateSuccess('Đã thêm " & insertCount & " SĐT vào chiến dịch!');</script>")
        End Sub
#End Region

#Region "NHẬP THỦ CÔNG"
        Private Sub lbtImportManual_Click(sender As Object, e As EventArgs) Handles lbtImportManual.Click
            Dim input As String = txtSdt.Text.Trim()
            If String.IsNullOrWhiteSpace(input) Then Return

            Dim existingPhones As New List(Of String)()
            Dim arrCurrent As ArrayList = _listSdtCtl._GetAll(CampaignId, "", -1, 0, 100000)
            If arrCurrent IsNot Nothing Then
                For Each item As Marketing_Zalo_ListSdtInfo In arrCurrent
                    If Not String.IsNullOrWhiteSpace(item.Phone) Then existingPhones.Add(item.Phone)
                Next
            End If

            input = input.Replace(vbCrLf, ";").Replace(vbCr, ";").Replace(vbLf, ";").Replace(",", ";")
            Dim tokens As String() = input.Split(";"c)

            Dim previewList As New List(Of SdtPreviewItem)()
            Dim seenInBatch As New List(Of String)()
            Dim okCount As Integer = 0
            Dim errCount As Integer = 0
            Dim dupCount As Integer = 0
            Dim insertCount As Integer = 0

            For Each token As String In tokens
                Dim rawSdt As String = token.Trim()
                If String.IsNullOrEmpty(rawSdt) Then Continue For


                Dim pi As New SdtPreviewItem()
                pi.SdtRaw = rawSdt

                Dim errMsg As String = String.Empty
                Dim normalized As String = ZaloPhoneHelper.ValidateAndNormalize(rawSdt, errMsg)

                If normalized Is Nothing Then
                    pi.SdtNormalized = rawSdt
                    pi.SdtStatus = "LỖI"
                    errCount += 1
                ElseIf existingPhones.Contains(normalized) OrElse seenInBatch.Contains(normalized) Then
                    pi.SdtNormalized = normalized
                    pi.SdtStatus = "TRÙNG"
                    dupCount += 1
                Else


                    Dim studentname As String = "Khách hàng"
                    Dim objStudent As StudentInfoInfo
                    objStudent = _studentInfoController._Info_GetBySodienthoai(normalized)
                    If Not objStudent Is Nothing Then
                        With objStudent
                            studentname = .Fullname
                        End With
                    End If

                    pi.SdtNormalized = normalized
                    pi.SdtStatus = "OK"
                    okCount += 1
                    seenInBatch.Add(normalized)
                    _listSdtCtl._Insert(CampaignId, studentname, rawSdt, normalized, 0, DateTime.Now, UserId, 50)
                    existingPhones.Add(normalized)
                    insertCount += 1
                End If

                previewList.Add(pi)
            Next

            rptPreviewManual.DataSource = previewList
            rptPreviewManual.DataBind()
            rptPreviewEvent.DataSource = Nothing
            rptPreviewEvent.DataBind()

            ltrOK.Text = okCount.ToString()
            ltrLoi.Text = errCount.ToString()
            ltrTrung.Text = dupCount.ToString()
            txtSdt.Text = ""
            BindListSdt()

            ClientAPI.RegisterStartUpScript(Me.Page, "ok",
                "<script>UpdateSuccess('Đã thêm " & insertCount & " SĐT. Trùng/bỏ qua: " & dupCount & "');</script>")
        End Sub
#End Region

#Region "Tìm kiếm"
        Private Sub lbtSearch_Click(sender As Object, e As EventArgs) Handles lbtSearch.Click
            BindListSdt()
        End Sub
#End Region

#Region "Xóa SĐT"
        Private Sub rptListSdt_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptListSdt.ItemCommand
            If e.CommandName = "DeleteSdt" Then
                Dim id As Integer = CInt(e.CommandArgument)
                _listSdtCtl._Delete(id)
                BindListSdt()
                ClientAPI.RegisterStartUpScript(Me.Page, "ok", "<script>UpdateSuccess('Đã xóa SĐT!');</script>")
            End If
        End Sub

        Private Sub lbtDeleteAll_Click(sender As Object, e As EventArgs) Handles lbtDeleteAll.Click
            _listSdtCtl._DeleteByCampaignId(CampaignId)
            BindListSdt()
            ResetCounters()
            rptPreviewEvent.DataSource = Nothing : rptPreviewEvent.DataBind()
            rptPreviewManual.DataSource = Nothing : rptPreviewManual.DataBind()
            ClientAPI.RegisterStartUpScript(Me.Page, "ok", "<script>UpdateSuccess('Đã xóa toàn bộ SĐT!');</script>")
        End Sub
#End Region

#Region "Private helpers"
        Private Sub ResetCounters()
            ltrOK.Text = "0"
            ltrLoi.Text = "0"
            ltrTrung.Text = "0"
        End Sub
#End Region

    End Class
End Namespace
