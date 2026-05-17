Imports DiffMatchPatch
Imports NVCMS.Modules.School
Imports NVCMS.Utilities
Namespace DesktopModules.TinTuc.Manager.news

    Public MustInherit Class newsedit
        Inherits Entities.Modules.PortalModuleBase
        Dim _MarketingSchoolController As New MarketingSchoolController
        Dim objMarketingSchoolInfo As New MarketingSchoolInfo
        Dim _diff_match_patch As New diff_match_patch
        Public Property ItemID() As Integer
            Get
                If Not ViewState("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("ItemID"), String))
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
        Public Property PhotoAbPath() As String
            Get
                If Not ViewState.Item("PhotoAbPath") Is Nothing Then
                    Return CType(ViewState.Item("PhotoAbPath"), String)
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("PhotoAbPath", value)
            End Set
        End Property
        Public Property PhotoVirPath() As String
            Get
                If Not ViewState.Item("PhotoVirPath") Is Nothing Then
                    Return CType(ViewState.Item("PhotoVirPath"), String)
                Else
                    Return nvcmsBL.GetImagePath(True, PortalId, True)
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("PhotoVirPath", value)
            End Set
        End Property
        Public Property verid() As Integer
            Get
                If Not ViewState("verid") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("verid"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("verid", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("verid") = Value.ToString
            End Set
        End Property
#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                Try
                    If Request.Item("itemid") <> "" Then
                        ItemID = Request.Item("itemid")
                        objMarketingSchoolInfo = _MarketingSchoolController.Marketing_Truong_GetByID(ItemID)
                        If Not objMarketingSchoolInfo Is Nothing Then
                            With objMarketingSchoolInfo
                                txtNameOfSchool.Text = .NameofSchool
                                ltrNameOfSchool.Text = .NameofSchool
                                txtwebsite.Text = .Website
                                txtloai.Text = .LoaiTruongTen
                                If Not String.IsNullOrEmpty(.Social) Then
                                    Dim cover As String() = .Social.Split(CType(",", Char))
                                    For i As Integer = 0 To cover.Length - 1
                                        Me.txtfacebook.Text = cover(0)
                                        Me.txttiwtter.Text = cover(1)
                                        Me.txtlinkedin.Text = cover(2)
                                        'Me.txtgplus.Text = cover(3)
                                        Me.txtyoutube.Text = cover(4)
                                        Me.txtinstagram.Text = cover(5)
                                    Next
                                End If
                                txtTomtat.Text = .Tomtat
                                Me.VideoLink.Text = .VideoLink
                                If Not .Logo Is Nothing Then
                                    Me.dvPreviewlogo.Visible = True
                                    Me.dvPreviewlogo.InnerHtml = "<img src=""" & .Logo & """  height='120px' />"
                                End If
                                hpfLogo.Value = .Logo
                                'Cover
                                If Not .Conver Is Nothing Then
                                    Me.dvPreviewcover.Visible = True
                                    Me.dvPreviewcover.InnerHtml = "<img src=""" & .Conver & """ style='width: 100%' />"
                                End If
                                '--
                                txtVitri.Text = .Vitri
                                txtnamthanhlap.Text = .Namthanhlap
                                ltrdiachi.Text = .Address
                                ltrQuocGia.Text = "<img src='" & .CountryFlag & "' style='height:20px; margin-right:5px;' />" & .CountryName
                                ltrQuocGiaBang.Text = .StateCityName
                                txtInfo.Text = Server.HtmlDecode(.Info)
                                txtInfoEN.Text = Server.HtmlDecode(.InfoEN)
                                'Diem manh
                                Me.txtDiemManh.Text = Server.HtmlDecode(.Descreption)
                                Me.ltrDiemManh.Text = Server.HtmlDecode(.DescreptionEN)
                                'Kiem dinh
                                ltrkiemdinh.Text = .KiemdinhEN
                                txtKiemDinh.Text = .Kiemdinh
                                'Xep hang
                                ltrXepHang.Text = .TypeofRanking
                                txtXepHang.Text = .TypeofRankingVN
                                'Loai truong
                                ltrLoaitruong.Text = .LoaitruongtextEN
                                txtLoaitruong.Text = .Loaitruongtext
                                '/////////////////////////////////
                                verid = CType(Request.Item("verid"), Integer)
                                If verid > 0 Then
                                    GetChangeTitle(verid)
                                End If
                                BindEdit()
                            End With
                        Else
                            Response.Redirect(NavigateURL(TabId))
                        End If

                    End If
                    PhotoAbPath = nvcmsBL.GetImagePath(False, PortalId, True)
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
        Private Sub lbtUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
            AddNews()
            Response.Redirect(NavigateURL(), True)
        End Sub
        Private Sub AddNews()
            Try
                '1. Update
                If ItemID <> 0 Then
                    Dim objMarketingSchoolInfo As MarketingSchoolInfo = _MarketingSchoolController.Marketing_Truong_GetByID(ItemID)
                    '1.2. Update News
                    objMarketingSchoolInfo = CollectNewsInfo(objMarketingSchoolInfo)
                    _MarketingSchoolController.Marketing_Truong_Update(objMarketingSchoolInfo)
                    '1.3. Save a Version
                    Ultis.Marketing_SaveTruongVersion(objMarketingSchoolInfo, ItemID, UserId)
                End If
                Response.Redirect(NavigateURL(TabId))
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Function CollectNewsInfo(ByVal obj As MarketingSchoolInfo) As MarketingSchoolInfo
            Try
                With obj
                    .Truongid = ItemID
                    .id = ItemID
                    .Tomtat = txtTomtat.Text
                    'Logo
                    Dim strFileName As String = ""
                    Dim strFileNamePath As String = ""
                    If Me.filelogo.PostedFile.FileName <> "" Then
                        strFileName = System.IO.Path.GetFileName(Me.filelogo.PostedFile.FileName)
                        strFileName = DateTime.Now.ToString("yyyyMMddHHmmss") & strFileName.Replace(" ", "-")
                        Me.filelogo.PostedFile.SaveAs(PhotoAbPath & "/" & strFileName)
                        strFileNamePath = GetMediaPath(PhotoVirPath, strFileName)
                    Else
                        strFileNamePath = hpfLogo.Value
                    End If

                    .Logo = strFileNamePath
                    'Cover
                    Dim strFileNameCover As String = ""
                    Dim strFileNameCoverPath As String = ""
                    If Me.filecover.PostedFile.FileName <> "" Then
                        strFileNameCover = System.IO.Path.GetFileName(Me.filecover.PostedFile.FileName)
                        strFileNameCover = DateTime.Now.ToString("yyyyMMddHHmmss") & strFileNameCover.Replace(" ", "-")
                        Me.filecover.PostedFile.SaveAs(PhotoAbPath & "/" & strFileNameCover)
                        strFileNameCoverPath = GetMediaPath(PhotoVirPath, strFileNameCover)
                    Else
                        strFileNameCoverPath = hdfCover.Value
                    End If
                    .Conver = strFileNameCoverPath
                    '---
                    .VideoLink = VideoLink.Text
                    .Descreption = Me.txtDiemManh.Text
                    .Website = txtwebsite.Text
                    '.Email = objMarketingSchoolInfo.Email
                    '.Phone = objMarketingSchoolInfo.Phone
                    'Social
                    Dim Scocial As String = ""
                    Scocial = Me.txtfacebook.Text & "," & Me.txttiwtter.Text & "," & Me.txtlinkedin.Text & ",," & Me.txtyoutube.Text & "," & Me.txtinstagram.Text
                    .Social = Scocial
                    .Kiemdinh = txtKiemDinh.Text
                    .TypeofRankingVN = txtXepHang.Text
                    .Loaitruongtext = txtLoaitruong.Text
                    '.SingleSex = objMarketingSchoolInfo.SingleSex
                    .Info = txtInfo.Text
                    .CreatedDate = DateTime.Now
                    .UserId = UserId
                End With
                Return obj
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                Return Nothing
            End Try
        End Function

#End Region

#Region "Upload"
        Private Function GetUploadPath(ByVal spath As String) As String
            Try
                Return spath.Substring(0, spath.LastIndexOf("/", System.StringComparison.Ordinal))
            Catch ex As Exception
                Return ""
            End Try
        End Function
        Private Function GetMediaPath(ByVal foldername As String, ByVal radupload As String) As String
            If radupload.Length > 0 Then
                Return foldername & "/" & radupload
            Else
                Return ""
            End If
        End Function
#End Region
#Region "Lịch sử chỉnh sửa"
        Public Sub BindEdit()
            rptListHistory.DataSource = _MarketingSchoolController.Marketing_Truong_Version_GetAllByTruong(ItemID)
            rptListHistory.DataBind()
        End Sub
        Public Function GetSelect(veridrequest As Integer, verid As Integer) As String
            Dim sresult As String = ""
            If veridrequest = verid Then
                sresult = "active"
            End If
            Return sresult
        End Function
        Public Sub GetChangeTitle(verid As Integer)
            Dim iitemit As Integer = 0
            Dim arraylistz As New ArrayList
            Dim arraylistNew As New ArrayList
            arraylistz = _MarketingSchoolController.Marketing_Truong_Version_GetAllByTruong(ItemID)
            If arraylistz.Count > 0 Then
                For i As Integer = 0 To arraylistz.Count - 1
                    Dim objVersionz As MarketingSchoolInfo = CType(arraylistz(i), MarketingSchoolInfo)
                    arraylistNew.Add(objVersionz.id)
                Next
            End If
            Dim sssss As String = ""
            For i As Integer = 0 To arraylistNew.Count - 1
                sssss += arraylistNew(i).ToString() & " - "
            Next
            iitemit = arraylistNew.IndexOf(verid)
            If iitemit < arraylistNew.Count - 1 Then
                Dim stitle As String = ""
                Dim stitle2 As String = ""
                Dim objVersion As MarketingSchoolInfo = CType(arraylistz(iitemit), MarketingSchoolInfo)
                If Not objVersion Is Nothing Then
                    With objVersion
                        ltridId.Text = .id & "-" & iitemit
                        stitle = "<h2>" & .NameofSchool & "</h2><h4>Tóm tắt</h4><p><b>" & .Tomtat & "</b></p><h4>Thông tin</h4>" & HtmlBuilderHelper.CleanAllStyleContent(.Info) & "<h4>Điểm mạnh</h4>" & HtmlBuilderHelper.CleanAllStyleContent(.Descreption) & "<h4>Kiểm định</h4><p>" & .Kiemdinh & "</p><h4>Xếp hạng</h4><p>" & .TypeofRankingVN & "</p><h4>Loại trường</h4><p>" & .Loaitruongtext & "</p>"
                        ltrbanTruocDo.Text = Server.HtmlDecode(stitle)
                    End With
                End If
                Dim objVersion2 As MarketingSchoolInfo = CType(arraylistz(iitemit + 1), MarketingSchoolInfo)
                If Not objVersion2 Is Nothing Then
                    With objVersion2
                        ltridId2.Text = .id & "-" & iitemit + 1
                        stitle2 = "<h2>" & .NameofSchool & "</h2><h4>Tóm tắt</h4><p>" & .Tomtat & "</b></p><h4>Thông tin</h4>" & HtmlBuilderHelper.CleanAllStyleContent(.Info) & "<h4>Điểm mạnh</h4>" & HtmlBuilderHelper.CleanAllStyleContent(.Descreption) & "<h4>Kiểm định</h4><p>" & .Kiemdinh & "</p><h4>Xếp hạng</h4><p>" & .TypeofRankingVN & "</p><h4>Loại trường</h4><p>" & .Loaitruongtext & "</p>"
                        Dim diffs = _diff_match_patch.diff_main(stitle2, stitle)
                        _diff_match_patch.diff_cleanupSemantic(diffs)
                        Dim sresult As String = Server.HtmlDecode(_diff_match_patch.diff_prettyHtml(diffs))
                        ltrbanHientai.Text = sresult
                    End With
                End If
            Else
                Dim objVersion As MarketingSchoolInfo
                objVersion = _MarketingSchoolController.Marketing_Truong_Version_GetByID(verid)
                If Not objVersion Is Nothing Then
                    With objVersion
                        ltrbanHientai.Text = .NameofSchool
                        ltrbanTruocDo.Text = "Bài viết vừa khởi tạo không có so sánh"
                    End With
                End If
            End If
        End Sub
#End Region
    End Class
End Namespace