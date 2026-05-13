Imports System
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Entities.Content.Taxonomy
Imports NVCMS.Modules.Video
Imports NVCMS.Modules.TinTuc
Imports DotNetNuke.UI.Utilities
Imports Telerik.Web.UI
Imports System.Collections.Generic
Imports HtmlAgilityPack
Imports NVCMS.CrawlerData
Imports System.IO

Namespace DesktopModules.Video.Manager.Video

    Public MustInherit Class Edit
        Inherits Entities.Modules.PortalModuleBase
        Public StorageFolder As String = String.Empty
        Public MediaPath As String = String.Empty
        Public requestAutoSaveInterval As String = "30000"
        Dim PhotoPhysicPath As String
        Public PhotoVirtualPath As String
        Private ctlVideos As New Videos_Controller
        Private objVideos As New Videos_Info
        Private ReadOnly _VideoProcessController As New VideoProcessController
        Private objProcessInfo As New VideoProcessInfo
        Dim _VideoByMediaController As New VideoByMediaController
        Dim _MediaItemController As New MediaItemController
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
        Public Property PageSize() As Integer
            Get
                If Not ViewState("PageSize") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("PageSize"), String))
                    Catch ex As Exception
                        Return 20
                    End Try
                Else
                    ViewState.Add("PageSize", "20")
                    Return 20
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("PageSize") = Value.ToString
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
                    Return Ultis.GetVideoPath(True, PortalId, True)
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("PhotoVirPath", value)
            End Set
        End Property
#Region "Ajax"
        Private Sub Insert_CauHinhTin_AutoSave(ByVal newsid As Integer, chkconfighotslide As Boolean, chkconfigtinnong As Boolean, chkconfigxuhuongdoc As Boolean)
            Try
                'If chkconfighotslide = True Then
                '    If Ultis.CheckCauHinhTin(newsid, 1, PortalId) = False Then
                '        _NewsSettingsController.Insert(newsid, 0, 1, PortalId)
                '    End If
                'Else
                '    _NewsSettingsController.DeleteByNewId(newsid, 1, PortalId)
                'End If
                'If chkconfigtinnong = True Then
                '    If Ultis.CheckCauHinhTin(newsid, 2, PortalId) = False Then
                '        _NewsSettingsController.Insert(newsid, 0, 2, PortalId)
                '    End If
                'Else
                '    _NewsSettingsController.DeleteByNewId(newsid, 2, PortalId)
                'End If
                'If chkconfigxuhuongdoc = True Then
                '    If Ultis.CheckCauHinhTin(newsid, 3, PortalId) = False Then
                '        _NewsSettingsController.Insert(newsid, 0, 3, PortalId)
                '    End If
                'Else
                '    _NewsSettingsController.DeleteByNewId(newsid, 3, PortalId)
                'End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

#End Region
#Region "Event Handlers"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                requestAutoSaveInterval = PortalController.GetPortalSetting(BL.settingAutoSaveRequestDuration, PortalId, "5000")
                Try

                    ''Vao phat la copy hết đống này vào check quyền
                    lbtSave.Visible = Ultis.ButtonNutLuu(UserId)
                    lbtTralai.Visible = Ultis.ButtonGuiXuatBan(UserId)
                    lbtSaveXB.Visible = Ultis.ButtonXuatBanLuon(UserId)
                    '====================================
                    'Nhuan but
                    'LoadWorkFlow()
                    If Request.Item("itemid") <> "" Then
                        lbtDeleteTop.Visible = True
                        ItemID = Request.Item("itemid")
                        ' Hien thi tin nay
                        objVideos = ctlVideos.GetByID(ItemID, PortalId)

                        If Not objVideos Is Nothing Then
                            With objVideos
                                Me.txtTitle.Text = .Title
                                Me.txtSummary.Text = .Summary
                                txtButDanh.Text = .ButDanh
                                teContent.Value = Server.HtmlDecode(.Content)
                                ddlkieuvideo.SelectedValue = .TypeVideo
                                hdf_linkvideo.Value = .VideoPath
                                Me.txtPublishedDate.Text = .PublishedDate.ToString("dd/MM/yyyy HH:mm")
                                hdf_itemid.Value = .VideoId
                                If .TypeVideo = TypeVideo.manhung Then
                                    Manhhung.Visible = True
                                    ltrviewdemo.Text = .VideoPath
                                End If
                                If .TypeVideo = TypeVideo.youtube Then
                                    dyoutube.Visible = True
                                    Dim strYoutube As String = "<iframe width='100%' height='489' src='https://www.youtube.com/embed/__videoLink__?showinfo=0&amp;ps=docs&amp;autoplay=1&amp;iv_load_policy=3&amp;vq=large&amp;modestbranding=1&amp;nologo=0' frameborder='0' allowfullscreen='1'></iframe>"
                                    strYoutube = strYoutube.Replace("__videoLink__", .VideoPath)
                                    Me.ltrviewdemo.Text = strYoutube
                                End If
                                If .TypeVideo = TypeVideo.uploadfile Then
                                    dupload.Visible = True
                                    Me.ltrviewdemo.Text = "<video controls='controls' src='" & .VideoPath & "' style='width:100%'></video>"
                                End If
                                'Bind noidung for Autosave
                                teContent.Value = Server.HtmlDecode(.Content)
                                divImagePath.InnerHtml = "<img src='" + .ImagePath + "' width='120px'/>"
                                hdfImagePath.Value = .ImagePath
                                'Tags
                                txtTags.Text = .Tags
                                txtnhuanbut.Text = .Credit
                                '6 Lay bang nhuan but ra
                                'upload ảnh
                                'hdf_itemid.Value = .VideoId
                                'lbtXemTruoc.NavigateUrl = "/news/" & ReplaceChuoi.bodautenfile(.Title) & "-" & .VideoId & ".html"
                            End With
                        End If
                    Else
                        AddNews()
                    End If
                    PhotoAbPath = Ultis.GetVideoPath(False, PortalId, True)
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub

        Private Sub AddNews()
            Try
                '1. Update
                If ItemID > 0 Then
                    '1.1. Process: Created User Edit
                    With objProcessInfo
                        .VideoId = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = (New Videos_Controller).GetByID(ItemID, PortalId).Status
                        .ProcessName = BL.msgProcessEditByCreator
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    _VideoProcessController.Insert(objProcessInfo)

                    Dim objVideosInfo As Videos_Info = ctlVideos.GetByID(ItemID, PortalId)
                    '1.2. Update News
                    objVideosInfo = CollectVideoInfo(objVideosInfo)
                    ctlVideos.Update(objVideosInfo)
                    ctlVideos.UpdateNhuanBut(ItemID, txtnhuanbut.Text)
                    '============================================
                Else 'Add New
                    Dim objInfo As Videos_Info = CollectVideoInfo(Nothing)
                    ItemID = ctlVideos.Insert(objInfo)
                    '1. Process: Khởi tạo
                    With objProcessInfo
                        .VideoId = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = NewsStatus.DangBienSoan 'Khởi tạo
                        .ProcessName = BL.msgProcessCreated
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    _VideoProcessController.Insert(objProcessInfo)
                    '2. Save a Version
                    objInfo.VideoId = ItemID
                    'Ultis.Save2Version(objInfo, UserId)
                    '3.Generate Thumbs
                    'Ultis.GenerateThumbs(Server.MapPath(txtImagePath.Value))

                    '4. Cap nhat tac gia vào bằng news
                    'ctlVideos.UpdateTacgia(ItemID, UserId & ";")
                    '5. Cap nhat tac gia vao bang nhuan but
                    'ctlnhuabut.NhuanBut_Insert(ItemID, 1, UserId, 0, DateTime.Now, UserId, UserId, PortalId, 1)
                    '6 Update bangr View
                    Dim ctlVideosByView As New NewsByView
                    ctlVideosByView.NewsByView_Insert(ItemID, 0, PortalId)
                    Response.Redirect(NavigateURL() & "?itemid=" + ItemID.ToString, False)
                End If
                '3. Update News -- Category

                '4. Update Media
                'Ultis.InsertMediaFiles(Server, PortalId, UserId, ItemID, CType(hdf_Category.Value, Integer), tnUpload.FileList)
                '5 update Tag
                'Ultis.UpdateNewsByTags(ItemID, txtTags.Text)

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Function CollectVideoInfo(ByVal obj As Videos_Info) As Videos_Info
            Try
                If obj Is Nothing Then
                    obj = New Videos_Info '
                    obj.Title = "Tiêu đề Video"
                    obj.Createdate = DateTime.Now
                    obj.Status = NewsStatus.DangBienSoan
                    obj.UserId = UserId
                    obj.LanguageId = BL.GetLanguage()
                    obj.PortalId = PortalId
                End If
                With obj
                    .VideoId = ItemID
                    Try
                        .CategoryId = 0
                    Catch ex As Exception
                        ProcessModuleLoadException(Me, ex)
                    End Try
                    .VideoPath = hdf_linkvideo.Value
                    .Status = NewsStatus.DaXuatBan
                    .ButDanh = txtButDanh.Text
                    .Title = Me.txtTitle.Text
                    .TypeVideo = ddlkieuvideo.SelectedValue
                    'Doan nay xu ly viec xoa file video upload, neu ko su dung file upload
                    If ddlkieuvideo.SelectedValue <> TypeVideo.uploadfile Then
                        Dim arrvideobyMedia As ArrayList = _VideoByMediaController._GetAllByvideoid(ItemID)
                        If arrvideobyMedia.Count > 0 Then
                            For Each objVideoByMediaInfo As VideoByMediaInfo In arrvideobyMedia
                                If Not objVideoByMediaInfo Is Nothing Then
                                    With objVideoByMediaInfo
                                        Dim objMediaItemInfo As MediaItemInfo
                                        objMediaItemInfo = _MediaItemController._GetByID(objVideoByMediaInfo.mediaid)
                                        If Not objMediaItemInfo Is Nothing Then
                                            With objMediaItemInfo
                                                'Xoa file vat ly
                                                Dim FileToDelete As String = .forder & "\" & .filename
                                                If System.IO.File.Exists(FileToDelete) = True Then
                                                    System.IO.File.Delete(FileToDelete)
                                                End If
                                                'xoa database
                                                _MediaItemController._Delete(.id)
                                            End With
                                        End If
                                    End With
                                End If

                            Next
                        End If
                        _VideoByMediaController._DeleteByvideoid(ItemID, PortalId)
                    End If
                    If Not String.IsNullOrEmpty(txtSummary.Text.Trim) Then
                        .Summary = Me.txtSummary.Text
                    End If
                    .Content = teContent.Value
                    .isActive = True
                    'Anh dai dien
                    Dim strFileName As String = ""
                    Dim strFileNamePath As String = ""
                    Try
                        If Me.inptFileImagePath.PostedFile.FileName <> "" Then
                            strFileName = System.IO.Path.GetFileName(Me.inptFileImagePath.PostedFile.FileName)
                            Me.inptFileImagePath.PostedFile.SaveAs(PhotoAbPath & "/" & strFileName)
                            strFileNamePath = GetMediaPath(PhotoVirPath, Me.inptFileImagePath.PostedFile.FileName)
                        Else
                            strFileNamePath = hdfImagePath.Value
                        End If
                    Catch ex As Exception
                        ProcessModuleLoadException(Me, ex)
                    End Try
                    .ImagePath = strFileNamePath
                    'Tags
                    .Tags = txtTags.Text 'Tags
                End With

                Return obj
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                Return Nothing
            End Try
        End Function

#End Region
#Region "Button Action"
        ''' <summary>
        ''' Xóa mềm tin bài
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub lbtDeleteTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtDeleteTop.Click
            Try
                If Request.Item("itemid") <> "" Then
                    ItemID = CInt(Request.Item("itemid"))

                    '1. Unlock tin bai
                    Ultis.UnlockNews(ItemID, UserId)
                    '2. Soft delete
                    'ctlVideos.UpdateVisible(ItemID, False)
                    '3. Process: Deleted
                    With objProcessInfo
                        .VideoId = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = ctlVideos.GetByID(ItemID, PortalId).Status
                        .ProcessName = BL.msgProcessXoa
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    _VideoProcessController.Insert(objProcessInfo)
                    '4. Return
                    Response.Redirect(BL.pageDanhSachVideoDaXuatBan)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub lbtCancelTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtCancelTop.Click
            Try
                '1. Unlock tin bai
                Ultis.UnlockNews(ItemID, UserId)
                '2. Return
                Response.Redirect(BL.pageDanhSachVideoDaXuatBan)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        ''' <summary>
        ''' Hàm này hiện không dùng, Khi save => Auto saving
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub lbtSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtSave.Click
            Try
                'Unlock tin bai
                Ultis.UnlockNews(ItemID, UserId)
                If ItemID <> 0 Then
                    'TrungNS: New Process
                    With objProcessInfo
                        .VideoId = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = (New Videos_Controller).GetByID(ItemID, PortalId).Status
                        .ProcessName = BL.msgProcessEditByCreator
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    _VideoProcessController.Insert(objProcessInfo)

                    Dim objVideosInfo As Videos_Info = ctlVideos.GetByID(ItemID, PortalId)
                    objVideosInfo = CollectVideoInfo(objVideosInfo)
                    ctlVideos.Update(objVideosInfo)
                    '1.4 Nem vao cau hinh tin news_setting
                    '1.5 update Tag
                    Ultis.UpdateNewsByTags(ItemID, txtTags.Text)
                Else
                    ' Insert
                    Dim objInfo As Videos_Info = CollectVideoInfo(Nothing)
                    ItemID = ctlVideos.Insert(objInfo)
                    'ctlVideos.UpdateStatus(ItemID, 0, UserId)
                    'Version
                    With objProcessInfo
                        .VideoId = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = NewsStatus.DangBienSoan 'Khởi tạo
                        .ProcessName = BL.msgProcessCreated
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    _VideoProcessController.Insert(objProcessInfo)
                End If
                'Cap nhat bao bang trung gian News -- Category
                'TrungNS: Updated for VOV: AUDIO != VIDEO
                'Ultis.InsertMediaFiles(Server, PortalId, UserId, ItemID, hdf_Category.Value, tnUpload.FileList)
                'ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Thực hiện lưu tin bài thành công!');</script>")
                Response.Redirect(BL.pageDanhSachVideoDaXuatBan)
                'Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub lbtTralai_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtTralai.Click
            Try
                AddNews()
                '2 Cập nhật nhuận bút vào NEWS

                '3. Process 2 Users
                With objProcessInfo
                    .VideoId = ItemID
                    .CreateDate = DateTime.Now
                    .ByUser = UserId
                    .StatusID = NewsStatus.ChoXuatBan
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    .ProcessName = "Thực hiện Gửi Xuất bản => "
                End With
                _VideoProcessController.Insert(objProcessInfo)
                '4. Sending
                Dim ctlUserNews As New News_UserProcessController
                ctlUserNews.DeleteByNewsID(ItemID)
                ctlVideos.UpdateStatus(ItemID, NewsStatus.DangBienSoan, UserId)
                '5. Unlock tin bai
                Ultis.UnlockNews(ItemID, UserId)
                '6 Gui mail cho nhom bien tap
                Dim obbjnews As Videos_Info = ctlVideos.GetByID(ItemID, PortalId)
                If Not obbjnews Is Nothing Then
                    With obbjnews
                        'Ultis.SendMailThongBaoBai(obbjnews, PortalId)
                    End With
                End If
                '7. Return

                Response.Redirect(BL.pageDanhSachVideoDaXuatBan, False)
                'Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub lbtSaveXB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtSaveXB.Click
            Try
                AddNews()
                Dim pubDate As Date = Date.Now
                Try
                    If Not String.IsNullOrEmpty(txtPublishedDate.Text) Then
                        pubDate = Date.ParseExact(txtPublishedDate.Text, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
                    End If
                Catch ex As Exception
                End Try
                ' ctlnhuabut.NhuanBut_UpdateNhuanXuatBan(ItemID, UserId, pubDate, 1, KieuNhuanBut.TinBai)
                '3. Process 2 Users
                With objProcessInfo
                    .VideoId = ItemID
                    .CreateDate = DateTime.Now
                    .ByUser = UserId
                    .StatusID = NewsStatus.DaXuatBan
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    .ProcessName = "Thực hiện Xuất bản ngay => "
                End With
                _VideoProcessController.Insert(objProcessInfo)
                '3. Update nguoi duyet + xb neu User XB ngay.
                ctlVideos.UpdateStatus(ItemID, NewsStatus.ChoXuatBan, UserId)
                ctlVideos.UpdatePublishedDate(ItemID, pubDate, UserId)
                '4. Sending
                ctlVideos.UpdateStatus(ItemID, NewsStatus.DaXuatBan, UserId)
                '5. Unlock tin bai
                ' Ultis.UnlockNews(ItemID, UserId)
                'ZZ Xóa cahce để lên bài luôn
                'ctlVideos.News_Publish_Insert(ItemID, ddlCategory.SelectedValue, Me.txtTitle.Text, savatar, Me.txtSummary.Value, txtKeyword.Text, teContent.Value, True, CType(IIf(Me.chkHotCat.Checked(), 1, 0), Boolean), CType(IIf(Me.chkHotSite.Checked(), 1, 0), Boolean), NewsStatus.DaXuatBan, hdf_Related.Value, tags.Value, False, CType(IIf(Me.chkIsVideo.Checked(), 1, 0), Boolean), CType(IIf(Me.chkIsPhoto.Checked(), 1, 0), Boolean), txtbutdanh.Value, _SourceText.Value, 0, 0, pubDate, UserId, UserId, PortalId, "vi-VN", BL.maxDateV, "", CType(IIf(Me.chkamp.Checked(), 1, 0), Boolean), "", False, CType(IIf(Me.chkhienbaimoi.Checked(), 1, 0), Boolean), CType(IIf(Me.chkshowQC.Checked(), 1, 0), Boolean))
                '7. Return
                Response.Redirect(BL.pageDanhSachVideoDaXuatBan, False)
                '8 Clear cache
                DotNetNuke.Common.Utilities.DataCache.ClearCache()
                DotNetNuke.Entities.Host.ServerController.ClearCachedServers()
                'Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
#Region "Logic Handlers"
        Public Sub ddlkieuvideo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlkieuvideo.SelectedIndexChanged
            If Me.ddlkieuvideo.SelectedValue = TypeVideo.manhung Then
                Me.Manhhung.Visible = True
                Me.dupload.Visible = False
                Me.dyoutube.Visible = False
                Me.ltrviewdemo.Text = ""
            End If
            If Me.ddlkieuvideo.SelectedValue = TypeVideo.youtube Then
                Me.Manhhung.Visible = False
                Me.dupload.Visible = False
                Me.dyoutube.Visible = True
                Me.ltrviewdemo.Text = ""
            End If
            If Me.ddlkieuvideo.SelectedValue = TypeVideo.uploadfile Then
                Me.Manhhung.Visible = False
                Me.dupload.Visible = True
                Me.dyoutube.Visible = False
                Me.ltrviewdemo.Text = ""
            End If
        End Sub
        Protected Sub txtMaNhung_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.ltrviewdemo.Text = ""
            If txtMaNhung.Text <> "" Then
                Me.ltrviewdemo.Text = txtMaNhung.Text
                hdf_linkvideo.Value = txtMaNhung.Text
            End If
        End Sub
        Protected Sub txtlinkYotube_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.ltrviewdemo.Text = ""
            Dim stlinkun As String = ""
            If txtlinkYotube.Text <> "" Then
                If txtlinkYotube.Text.Contains("iframe") Then
                    Dim linksrc As String = Ultis.GetLinkFromIframe(txtlinkYotube.Text)
                    stlinkun = linksrc
                    If linksrc.Contains("https") Or (linksrc.Contains("http")) Then
                        If Ultis.IsValidURL(linksrc) Then
                            stlinkun = Ultis.GetYouTubeVideoIdFromUrl(linksrc)
                        End If
                    End If
                End If
                If txtlinkYotube.Text.Contains("https") Or (txtlinkYotube.Text.Contains("http")) Then
                    If Ultis.IsValidURL(txtlinkYotube.Text) Then
                        stlinkun = Ultis.GetYouTubeVideoIdFromUrl(txtlinkYotube.Text)
                    End If
                Else
                    stlinkun = txtlinkYotube.Text
                End If
                Dim strYoutube As String = "<iframe width='100%' height='489' src='http://www.youtube.com/embed/__videoLink__?showinfo=0&amp;ps=docs&amp;autoplay=1&amp;iv_load_policy=3&amp;vq=large&amp;modestbranding=1&amp;nologo=0' frameborder='0' allowfullscreen='1'></iframe>"
                strYoutube = strYoutube.Replace("__videoLink__", stlinkun)
                Me.ltrviewdemo.Text = strYoutube
                hdf_linkvideo.Value = stlinkun
            End If
        End Sub


#End Region
#Region "Upload anh bai viet"
        Private Sub BindAnhBaiViet(id As Integer)
            'Dim arrMediaNews As New ArrayList
            'Dim _VideoByMediaController As New NewsByMediaController
            'Dim currentMediaByNews = _VideoByMediaController._GetAllByNewId(id)
            'If Not currentMediaByNews Is Nothing AndAlso currentMediaByNews.Count > 0 Then
            '    Me.rptphotoatt.DataSource = currentMediaByNews
            '    Me.rptphotoatt.DataBind()
            'End If
        End Sub
        Protected Sub btnxoaanh(ByVal sender As Object, ByVal e As EventArgs)
            Dim itemidhistory As Integer = Integer.Parse(TryCast(sender, Button).CommandArgument)
            'Xoa file vat ly
            Dim objMedia As MediaItemInfo
            objMedia = _MediaItemController._GetByID(itemidhistory)
            If Not objMedia Is Nothing Then
                With objMedia
                    Dim FileToDelete As String = .forder & "\" & .filename
                    If System.IO.File.Exists(FileToDelete) = True Then
                        System.IO.File.Delete(FileToDelete)
                    End If
                End With
            End If
            'Xoa bang media
            _MediaItemController._Delete(itemidhistory)
            'Xoa Video media
            _VideoByMediaController._DeleteByMediaId(itemidhistory)
            BindAnhBaiViet(ItemID)
        End Sub
        Public Function ChoXoaAnh(id As Integer) As Boolean
            Dim objMedia As MediaItemInfo
            objMedia = _MediaItemController._GetByID(id)
            If Not objMedia Is Nothing Then
                With objMedia
                    Dim objVideos As Videos_Info
                    objVideos = ctlVideos.GetByID(ItemID, PortalId)
                    If Not objVideos Is Nothing Then
                        If objVideos.Content.Contains(.filename) Or objVideos.ImagePath.Contains(.filename) Then
                            Return False
                        Else
                            Return True
                        End If
                    Else
                        Return True
                    End If

                End With
            Else
                Return True
            End If
        End Function
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


    End Class
End Namespace