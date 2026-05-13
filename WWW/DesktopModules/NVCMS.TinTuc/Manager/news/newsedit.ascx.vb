Imports System.IO
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.UI.Utilities
Imports HtmlAgilityPack
Imports NVCMS.CrawlerData
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Manager.news

    Public MustInherit Class newsedit
        Inherits Entities.Modules.PortalModuleBase
        Implements IClientAPICallbackEventHandler

        Public StorageFolder As String = String.Empty
        Public MediaPath As String = String.Empty
        Public requestAutoSaveInterval As String = "30000"
        Dim PhotoPhysicPath As String
        Public PhotoVirtualPath As String
        Private ReadOnly ctlNews As New NV_NewsController
        Private objNews As New NV_NewsInfo
        Private ReadOnly ctlProcess As New NewsProcessController
        Private objProcessInfo As New NewsProcessInfo
        Private objWF As New News_UserWFInfo
        Private objNewsNote As New NewsNoteInfo
        Dim _NewsNoteController As New NewsNoteController
        Dim ctlMediaNews As New NewsByMediaController
        Dim ctlMedia As New MediaItemController
        Dim _NewsSettingsController As New NewsSettingsController
        Dim ctlnhuabut As New NhuanButController
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
        Public Property status() As Integer
            Get
                If Not ViewState("status") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("status"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("status", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("status") = Value.ToString
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
#Region "Ajax"
        Public Property ajItemID() As Int64
            Get
                If Not Session("ajItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(Session("ajItemID"))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    Session.Add("ajItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Int64)
                Session("ajItemID") = Value.ToString
            End Set
        End Property
        Public Function RaiseClientAPICallbackEvent(ByVal eventArgument As String) As String Implements DotNetNuke.UI.Utilities.IClientAPICallbackEventHandler.RaiseClientAPICallbackEvent
            Try
                Dim cvl As String = eventArgument
                If cvl.Contains("~!@|") Then
                    Dim arr As String() = cvl.Split("~!@|")

                    Dim iID As Integer = AutoSave(arr(0), arr(1).Substring(3), arr(2).Substring(3), arr(3).Substring(3), arr(4).Substring(3), arr(5).Substring(3), arr(6).Substring(3), arr(7).Substring(3), arr(8).Substring(3), arr(9).Substring(3), arr(10).Substring(3), arr(11).Substring(3), arr(12).Substring(3), arr(13).Substring(3), arr(14).Substring(3), arr(15).Substring(3), arr(16).Substring(3), arr(17).Substring(3), arr(18).Substring(3), arr(19).Substring(3), arr(20).Substring(3), arr(21).Substring(3), arr(22).Substring(3), arr(23).Substring(3), arr(24).Substring(3), arr(25).Substring(3), arr(26).Substring(3), arr(27).Substring(3), arr(28).Substring(3), arr(29).Substring(3), arr(30).Substring(3), arr(31).Substring(3), arr(32).Substring(3), arr(33).Substring(3), arr(34).Substring(3))

                    Return Server.HtmlDecode(iID)
                Else

                    Dim arr As String() = cvl.Split("|")
                    Dim curpage As Integer = Integer.Parse(arr(0))
                    Dim source As Integer = Integer.Parse(arr(1))
                    Dim type As Integer = Integer.Parse(arr(2))
                    Dim key As String = arr(3).Trim

                    Return Ultis.GetFilesByFolder(curpage, source, type, key, PageSize).GetXml
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

            Return ""
        End Function
        Private Function AutoSave(ByVal sID As String, ByVal metatitle64 As String, ByVal metadesc64 As String, ByVal metaslug64 As String, ByVal title64 As String, ByVal loaitinbai As String, ByVal tinnongchuyenmuc As Boolean, ByVal tinnongsite As Boolean, ByVal chuyenmucchinh As Integer, ByVal chuyenmucphu As String, ByVal tomtat64 As String, ByVal he As String, ByVal noidung64 As String, ByVal nguontin As String, ByVal dongtg As String, ByVal luuy64 As String, ByVal links64 As String, ByVal anhdd64 As String, mediaList64 As String, imgList64 As String, isVideo As Boolean, isPhoto As Boolean, isPr As Boolean, isShowBaiMoi As Boolean, isAMP As Boolean, isHienQuangCao As Boolean, isAnNoiDung As Boolean, isAnLink As Boolean, keyword64 As String, butdanh64 As String, SourceText64 As String, chkconfighotslide As Boolean, chkconfigtinnong As Boolean, chkconfigxuhuongdoc As Boolean, sTags As String) As Integer
            Try
                'Title = "" van luu.
                'If String.IsNullOrEmpty(title64) Then
                '    Return 0
                'End If

                Dim b As Byte() = Convert.FromBase64String(title64)
                Dim title As String = System.Text.Encoding.UTF8.GetString(b)
                If String.IsNullOrEmpty(title) Then title = "Tin nháp_" + UserInfo.Username + "_" + Date.Now.ToString("dd-MM-yyyy HH:mm")
                b = Convert.FromBase64String(tomtat64)
                Dim tomtat As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(noidung64)
                Dim noidung As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(luuy64)
                Dim luuy As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(links64)
                Dim links As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(anhdd64)
                Dim anhdd As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(mediaList64)
                Dim mediaList As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(imgList64)
                Dim imgList As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(keyword64)
                Dim keyword As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(sTags)
                Dim sTagsz As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(butdanh64)
                Dim butdanh As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(SourceText64)
                Dim SourceText As String = System.Text.Encoding.UTF8.GetString(b)

                b = Convert.FromBase64String(metatitle64)
                Dim metatitle As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(metadesc64)
                Dim metadesc As String = System.Text.Encoding.UTF8.GetString(b)
                b = Convert.FromBase64String(metaslug64)
                Dim metaslug As String = System.Text.Encoding.UTF8.GetString(b)
                If IsNumeric(sID) Then
                    ajItemID = sID
                    objNews = ctlNews.GetByID(ajItemID)
                End If

                With objNews
                    .NewId = ajItemID
                    .meta_title = metatitle
                    .meta_description = metadesc
                    .meta_url = metaslug
                    .CategoryId = chuyenmucchinh
                    .Title = title
                    .Summary = tomtat
                    .Content = noidung
                    .isActive = True
                    .Hotcat = tinnongchuyenmuc
                    .Status = NewsStatus.DangBienSoan
                    .Hotsite = tinnongsite
                    .IsVideo = isVideo
                    .IsPhoto = isPhoto
                    .IsPR = isPr
                    .IsShowBaiMoi = isShowBaiMoi
                    .IsAMP = isAMP
                    .IsHienQuangCao = isHienQuangCao
                    .IsAnNoiDung = isAnNoiDung
                    '.IsAnLink = isAnLink
                    '.SourceInfo
                    '.Unit
                    .Type = BL.GetLoaiTinBai(mediaList, imgList)
                    .TypeUrl = BL.FormatTheLoai(BL.GetLoaiTinBai(mediaList, imgList))
                    .NewsKind = CType(loaitinbai, Integer)
                    .AttachedFiles = "" ' tnUpload.FileList
                    .Note = luuy
                    .Links = links
                    .ImagePath = anhdd
                    .keyword = keyword
                    .ButDanh = butdanh
                    .SourceText = SourceText
                    .Tags = sTagsz
                    'ajItemID, chuyenmucchinh, title, loaitinbai, tomtat, noidung, 1, tinnongchuyenmuc, tinnongsite, PortalId, tnUpload.FileList, he, Null.NullDate, "", False, tnUpload.FileList, "", UserId, DateTime.Now
                    'chuyenmucchinh, title, loaitinbai, tomtat, noidung, 1, tinnongchuyenmuc, tinnongsite, DateTime.Now, PortalId, UserId, tnUpload.FileList, he, Null.NullDate, "", False, tnUpload.FileList, "", -1, Null.NullDate
                End With
                If ajItemID <> 0 Then
                    ctlNews.Update(objNews)
                Else
                    objNews.CreateDate = DateTime.Now
                    objNews.PortalId = PortalId
                    objNews.UserId = UserId
                    objNews.IsImage = False
                    objNews.Unit = BL.GetPhongBanIdByUserId(PortalId, UserId)

                    ajItemID = ctlNews.Insert(objNews)
                    ctlNews.UpdateStatus(ajItemID, 0, UserId)
                    'Process
                    With objProcessInfo
                        .NewsID = ajItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = NewsStatus.DangBienSoan
                        .ProcessName = BL.msgProcessCreated
                    End With
                    ctlProcess.Insert(objProcessInfo)
                End If

                Insert_Categories_AutoSave(ajItemID, chuyenmucchinh, chuyenmucphu)
                Insert_CauHinhTin_AutoSave(ajItemID, chkconfighotslide, chkconfigtinnong, chkconfigxuhuongdoc)
                Ultis.UpdateNewsByTags(ajItemID, sTagsz)
                'Lưu lịch sử
                Ultis.Save2Version(objNews, UserId)
                'Ultis.InsertMediaFiles(Server, PortalId, UserId, ajItemID, chuyenmucchinh, mediaList)
                Return ajItemID
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                Return 0
            End Try
        End Function
        Private Sub Insert_Categories_AutoSave(ByVal newsid As Integer, ByVal chuyenmucchinh As Integer, ByVal chuyenmucphu As String)
            Try
                'TrungNS: News belonging to multiple categories
                NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_DeleteByNewsId(newsid)
                NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_Insert(newsid, chuyenmucchinh, True)
                Dim objInfo As New NewsByCategoryInfo
                If Not String.IsNullOrEmpty(chuyenmucphu) Then
                    Dim arr As String() = chuyenmucphu.Split(CType(",", Char))
                    For Each c As String In arr
                        If IsNumeric(c) Then
                            objInfo.NewsId = newsid
                            objInfo.CategoryId = Integer.Parse(c)
                            NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_Insert(newsid, Integer.Parse(c), False)
                        End If
                    Next
                End If

                'NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_DeleteByNewsId(newsid)
                'Try
                '    NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_Insert(newsid, CType(hdf_Category.Value, Integer), True)
                'Catch ex As Exception
                'End Try

                'Dim objInfo As New NewsByCategoryInfo
                'For Each node As Telerik.Web.UI.RadTreeNode In radTreeCategory.CheckedNodes
                '    If node.Checked = True Then
                '        objInfo.NewsId = newsid
                '        objInfo.CategoryId = Integer.Parse(node.Value)
                '        NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_Insert(newsid, CType(node.Value, Integer), False)
                '    End If
                'Next

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub Insert_CauHinhTin_AutoSave(ByVal newsid As Integer, chkconfighotslide As Boolean, chkconfigtinnong As Boolean, chkconfigxuhuongdoc As Boolean)
            Try
                If chkconfighotslide = True Then
                    If Ultis.CheckCauHinhTin(newsid, 1, PortalId) = False Then
                        _NewsSettingsController.Insert(newsid, 0, 1, PortalId)
                    End If
                Else
                    _NewsSettingsController.DeleteByNewId(newsid, 1, PortalId)
                End If
                If chkconfigtinnong = True Then
                    If Ultis.CheckCauHinhTin(newsid, 2, PortalId) = False Then
                        _NewsSettingsController.Insert(newsid, 0, 2, PortalId)
                    End If
                Else
                    _NewsSettingsController.DeleteByNewId(newsid, 2, PortalId)
                End If
                If chkconfigxuhuongdoc = True Then
                    If Ultis.CheckCauHinhTin(newsid, 3, PortalId) = False Then
                        _NewsSettingsController.Insert(newsid, 0, 3, PortalId)
                    End If
                Else
                    _NewsSettingsController.DeleteByNewId(newsid, 3, PortalId)
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

#End Region
#Region "Event Handlers"
        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Try
                'Client API
                Dim ClientCallBackRef As String = ClientAPI.GetCallbackEventReference(Me, "arr", "updateSuccess", "''", "updateError")

                Dim AjaxJavaScript As String = " <script type=""text/javascript"">"
                AjaxJavaScript += "function AutoSave(arr) {" & ClientCallBackRef & "}"
                AjaxJavaScript += " </script>"

                If Not Page.IsStartupScriptRegistered("AjaxSave") Then
                    Page.RegisterStartupScript("AjaxSave", AjaxJavaScript)
                End If

                'Fetch Files from DALET
                ClientCallBackRef = ClientAPI.GetCallbackEventReference(Me, "curpage", "onFetchSuccess", "''", "onFetchError")

                AjaxJavaScript = " <script type=""text/javascript"">"
                AjaxJavaScript += "function FetchFiles(curpage) {" & ClientCallBackRef & "}"
                AjaxJavaScript += " </script>"

                If Not Page.IsStartupScriptRegistered("AjaxFetching") Then
                    Page.RegisterStartupScript("AjaxFetching", AjaxJavaScript)
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                requestAutoSaveInterval = PortalController.GetPortalSetting(BL.settingAutoSaveRequestDuration, PortalId, "5000")
                Try
                    ''Vao phat la copy hết đống này vào check quyền
                    lbtSave.Visible = Ultis.ButtonNutLuu(UserId)
                    'lbtSaveGuiBientao.Visible = Ultis.ButtonGuiTienTap(UserId)
                    lbtSaveGuiXB.Visible = Ultis.ButtonGuiXuatBan(UserId)
                    lbtSaveXB.Visible = Ultis.ButtonXuatBanLuon(UserId)
                    '====================================
                    BindddlCategories()
                    BindRadTreeCategory()
                    'Nhuan but
                    BindNhuanButUser()
                    BindNhuanButKieuBai()
                    'LoadWorkFlow()
                    If Request.Item("catid") <> "" Then
                        ddlCategory.SelectedValue = Request.Item("catid")
                    End If
                    If Request.Item("itemid") <> "" Then
                        lbtDeleteTop.Visible = True
                        ItemID = Request.Item("itemid")
                        ajItemID = ItemID
                        ' Hien thi tin nay
                        objNews = ctlNews.GetByID(ItemID)
                        'NẾU BÀI ĐÃ LOCK -> OUT
                        'If Not Ultis.FormatVisibleByStatus(PortalId, objNews.IsEditable, ItemID) Then
                        '    Response.Redirect(BL.pageDanhSachTin)
                        'Else
                        '    Ultis.LockNews(ItemID, UserId)
                        'End If
                        If Not objNews Is Nothing Then
                            With objNews
                                If UserId = 1 Or UserId = 4 Then
                                    Dechoxemnoidung.Visible = True
                                End If
                                If (.Status = NewsStatus.BiTraLai) Or (.Notes <> "") Then
                                    ClientAPI.RegisterStartUpScript(Me.Page, "NewsNotesToast", "<script> NewsNotesToast('" & .Notes & "')</script>")
                                End If
                                status = .Status
                                Me.txtTitle.Text = .Title
                                Me.txtmeta_title.Text = .meta_title
                                Me.txtmeta_description.Text = .meta_description
                                Me.txtmeta_url.Text = .meta_url
                                Me.txtSummary.Text = .Summary
                                txtkeyword.Text = .keyword
                                txtSource.Text = .SourceText
                                txtButDanh.Text = .ButDanh
                                teContent.Value = Server.HtmlDecode(.Content)
                                If .Hotcat Then
                                    Me.chkHotCat.Checked = True
                                Else
                                    Me.chkHotCat.Checked = False
                                End If
                                If .Hotsite Then
                                    Me.chkHotSite.Checked = True
                                Else
                                    Me.chkHotSite.Checked = False
                                End If
                                If .IsVideo Then
                                    Me.chkVideo.Checked = True
                                Else
                                    Me.chkVideo.Checked = False
                                End If
                                If .IsPhoto Then
                                    Me.chkPhoto.Checked = True
                                Else
                                    Me.chkPhoto.Checked = False
                                End If
                                If .IsPR Then
                                    Me.chkPR.Checked = True
                                Else
                                    Me.chkPR.Checked = False
                                End If
                                If .IsShowBaiMoi Then
                                    Me.chkBaiMoiNhat.Checked = True
                                Else
                                    Me.chkBaiMoiNhat.Checked = False
                                End If
                                If .IsAMP Then
                                    Me.chkAMP.Checked = True
                                Else
                                    Me.chkAMP.Checked = False
                                End If
                                If .IsHienQuangCao Then
                                    Me.chkQuangCao.Checked = True
                                Else
                                    Me.chkQuangCao.Checked = False
                                End If
                                If .IsAnNoiDung Then
                                    Me.chkAnNoiDung.Checked = True
                                Else
                                    Me.chkAnNoiDung.Checked = False
                                End If
                                'If .IsAnLink Then
                                '    Me.chkisAnLink.Checked = True
                                'Else
                                '    Me.chkisAnLink.Checked = False
                                'End If
                                ddlImage.SelectedValue = CType(.NewsKind, String)
                                Try
                                    Me.ddlCategory.Items.FindByValue(CType(.CategoryId, String)).Selected = True
                                Catch ex As Exception
                                End Try

                                'Chuyen muc phu
                                ReconfigNodeChecked()

                                Dim sFiles As String = .AttachedFiles
                                If Not String.IsNullOrEmpty(sFiles) AndAlso sFiles <> "<files></files>" Then
                                    'tnUpload.FileList = .AttachedFiles
                                End If
                                'Bind noidung for Autosave
                                hdf_nodung.Value = Server.HtmlDecode(.Content)
                                BindButPhe()

                                imgDD.InnerHtml = "<img src='" + .ImagePath + "' width='120px'/>"
                                txtImagePath.Value = .ImagePath
                                txtCredit.Text = .Credit.ToString()
                                'Tin lien quan
                                If Not String.IsNullOrEmpty(.Links) Then
                                    hdf_Related.Value = .Links
                                    Dim arrRelated As New ArrayList

                                    Dim strArr As String() = .Links.Split(CType(";", Char))
                                    For i As Integer = 0 To strArr.Length - 1
                                        If IsNumeric(strArr(i)) Then
                                            Dim obj As NV_NewsInfo = ctlNews.GetByID(strArr(i))
                                            arrRelated.Add(obj)
                                        End If
                                    Next
                                    rptRelated.DataSource = arrRelated
                                    rptRelated.DataBind()
                                End If
                                'Tags
                                txtTags.Text = .Tags
                                'Doan nay lay cau hinh tin bai News_Settings
                                If Ultis.CheckCauHinhTin(.NewId, 1, PortalId) Then
                                    chkconfighotslide.Checked = True
                                End If
                                If Ultis.CheckCauHinhTin(.NewId, 2, PortalId) Then
                                    chkconfigtinnong.Checked = True
                                End If
                                If Ultis.CheckCauHinhTin(.NewId, 3, PortalId) Then
                                    chkconfigxuhuongdoc.Checked = True
                                End If
                                StorageFolder = BL.GetStorageFolder(objNews, PortalId)
                                '6 Lay bang nhuan but ra
                                BindNhuanBut(.NewId)
                                'upload ảnh
                                BindAnhBaiViet(.NewId)
                                hdf_itemid.Value = .NewId
                                lbtXemTruoc.NavigateUrl = "/news/" & ReplaceChuoi.bodautenfile(.Title) & "-" & .NewId & ".html"
                            End With

                        End If
                    Else

                        lbtDeleteTop.Visible = False
                        'lbtUyNhiemXB.Visible = False
                        AddNews()

                    End If
                    If Not BL.IsAdminGroup(UserInfo) Then
                        drlSource.Items.RemoveAt(0)
                    End If
                    If Not BL.IsXBGroup(UserInfo) Then
                        txtCredit.Visible = False
                    End If
                    hdf_Category.Value = ddlCategory.SelectedValue
                    'hdf_WF.Value = ddlWFTop.SelectedValue
                    Try
                        'hdf_WF_Text.Value = ddlWFTop.SelectedItem.Text
                    Catch ex As Exception
                        hdf_WF_Text.Value = ""
                    End Try

                    MediaPath = Ultis.GetUploadPath(True, PortalId, True)
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub

        Private Sub AddNews()
            Try
                '1. Update
                If ItemID <> 0 Then
                    '1.1. Process: Created User Edit
                    With objProcessInfo
                        .NewsID = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = (New NV_NewsController).GetByID(ItemID).Status
                        .ProcessName = BL.msgProcessEditByCreator
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    ctlProcess.Insert(objProcessInfo)

                    Dim objNewsInfo As NV_NewsInfo = ctlNews.GetByID(ItemID)
                    'If objNewsInfo.ImagePath <> txtImagePath.Value Then
                    '    'Generate Thumbs
                    '    Ultis.GenerateThumbs(Server.MapPath(txtImagePath.Value))
                    'End If
                    '1.2. Update News
                    objNewsInfo = CollectNewsInfo(objNewsInfo)
                    ctlNews.Update(objNewsInfo)
                    '1.3. Save a Version
                    Ultis.Save2Version(objNewsInfo, UserId)

                    'SEND MAIL & SMS
                    'Dim lstUsers As New List(Of Entities.Users.UserInfo)
                    'lstUsers.Add(UserController.GetUserById(PortalId, 145)) 'Mr Cuongvx
                    'If chkSendMail.Checked Then
                    '    SendEmails(lstUsers)
                    'End If
                    'If chkSendSMS.Checked Then
                    '    Send_SMSToUsers(lstUsers)
                    'End If
                    InsertNews_Category(ItemID)
                    '1.4 Nem vao cau hinh tin news_setting
                    Insert_CauHinhTin(ItemID)
                    '1.5 Update tac gia va nhuan but
                    'ctlnhuabut.NhuanBut_Insert(ItemID, 1, UserId, 0, DateTime.Now, UserId, UserId, PortalId, 1)
                    '============================================
                Else 'Add New
                    Dim objInfo As NV_NewsInfo = CollectNewsInfo(Nothing)
                    ItemID = ctlNews.Insert(objInfo)
                    '1. Process: Khởi tạo
                    With objProcessInfo
                        .NewsID = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = NewsStatus.DangBienSoan 'Khởi tạo
                        .ProcessName = BL.msgProcessCreated
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    ctlProcess.Insert(objProcessInfo)
                    '2. Save a Version
                    objInfo.NewId = ItemID
                    Ultis.Save2Version(objInfo, UserId)
                    '3.Generate Thumbs
                    'Ultis.GenerateThumbs(Server.MapPath(txtImagePath.Value))
                    InsertNews_Category(ItemID)
                    '4. Cap nhat tac gia vào bằng news
                    ctlNews.UpdateTacgia(ItemID, UserId & ";")
                    '5. Cap nhat tac gia vao bang nhuan but
                    ctlnhuabut.NhuanBut_Insert(ItemID, 1, UserId, 0, DateTime.Now, UserId, UserId, PortalId, 1)
                    '6 Update bangr View
                    Dim ctlNewsByView As New NewsByView
                    ctlNewsByView.NewsByView_Insert(ItemID, 0, PortalId)
                    Response.Redirect(NavigateURL() & "?itemid=" + ItemID.ToString, False)
                End If
                '3. Update News -- Category

                '4. Update Media
                'Ultis.InsertMediaFiles(Server, PortalId, UserId, ItemID, CType(hdf_Category.Value, Integer), tnUpload.FileList)
                '5 update Tag
                Ultis.UpdateNewsByTags(ItemID, txtTags.Text)

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Function CollectNewsInfo(ByVal obj As NV_NewsInfo) As NV_NewsInfo
            Try
                If obj Is Nothing Then
                    obj = New NV_NewsInfo
                    obj.CreateDate = DateTime.Now
                    obj.Status = NewsStatus.DangBienSoan
                    obj.UserId = UserId
                    obj.PortalId = PortalId
                    obj.IsImage = False
                    obj.Unit = BL.GetPhongBanIdByUserId(PortalId, UserId)
                    obj.StorageFolder = Ultis.GetStorageFolder()
                End If

                With obj
                    .NewId = ItemID
                    .meta_title = Me.txtmeta_title.Text
                    .meta_description = Me.txtmeta_description.Text
                    .meta_url = Me.txtmeta_url.Text
                    Try
                        .CategoryId = CType(hdf_Category.Value, Integer)
                    Catch ex As Exception
                        ProcessModuleLoadException(Me, ex)
                    End Try
                    .Hotcat = CType(IIf(Me.chkHotCat.Checked(), 1, 0), Boolean)
                    .Status = NewsStatus.DangBienSoan
                    .Hotsite = CType(IIf(Me.chkHotSite.Checked(), 1, 0), Boolean)
                    .Status = NewsStatus.DangBienSoan
                    .IsVideo = CType(IIf(Me.chkVideo.Checked(), 1, 0), Boolean)
                    .IsPhoto = CType(IIf(Me.chkPhoto.Checked(), 1, 0), Boolean)
                    .IsShowBaiMoi = CType(IIf(Me.chkBaiMoiNhat.Checked(), 1, 0), Boolean)
                    .IsAMP = CType(IIf(Me.chkAMP.Checked(), 1, 0), Boolean)
                    .IsHienQuangCao = CType(IIf(Me.chkQuangCao.Checked(), 1, 0), Boolean)
                    .IsAnNoiDung = CType(IIf(Me.chkAnNoiDung.Checked(), 1, 0), Boolean)
                    '.IsAnLink = CType(IIf(Me.chkisAnLink.Checked(), 1, 0), Boolean)
                    .ButDanh = txtButDanh.Text
                    .Title = Me.txtTitle.Text
                    If Not String.IsNullOrEmpty(txtSummary.Text.Trim) Then
                        .Summary = Me.txtSummary.Text
                    End If
                    .Content = teContent.Value
                    .NewsKind = CType(ddlImage.SelectedValue, Integer)
                    .AttachedFiles = "" 'tnUpload.FileList
                    .isActive = True
                    .Type = BL.GetLoaiTinBai(hdf_list_files.Value, hdf_IMG_files.Value)
                    .TypeUrl = BL.FormatTheLoai(BL.GetLoaiTinBai(hdf_list_files.Value, hdf_IMG_files.Value))
                    .ImagePath = txtImagePath.Value
                    .Links = hdf_Related.Value 'Tin lien quan
                    .Note = ""
                    .SourceText = txtSource.Text
                    .keyword = txtkeyword.Text
                    Dim sCredit As String = Regex.Replace(txtCredit.Text, "[^\d]", "")
                    If Not String.IsNullOrEmpty(sCredit) Then
                        .Credit = CType(sCredit, Integer)
                    Else
                        .Credit = 0
                    End If
                    .Tags = txtTags.Text 'Tags
                End With

                Return obj
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                Return Nothing
            End Try
        End Function
        Private Sub Insert_CauHinhTin(ByVal newsid As Integer)
            Try
                If chkconfighotslide.Checked = True Then
                    If Ultis.CheckCauHinhTin(newsid, 1, PortalId) = False Then
                        _NewsSettingsController.Insert(newsid, 0, 1, PortalId)
                    End If
                Else
                    _NewsSettingsController.DeleteByNewId(newsid, 1, PortalId)
                End If
                If chkconfigtinnong.Checked = True Then
                    If Ultis.CheckCauHinhTin(newsid, 2, PortalId) = False Then
                        _NewsSettingsController.Insert(newsid, 0, 2, PortalId)
                    End If
                Else
                    _NewsSettingsController.DeleteByNewId(newsid, 2, PortalId)
                End If
                If chkconfigxuhuongdoc.Checked = True Then
                    If Ultis.CheckCauHinhTin(newsid, 3, PortalId) = False Then
                        _NewsSettingsController.Insert(newsid, 0, 3, PortalId)
                    End If
                Else
                    _NewsSettingsController.DeleteByNewId(newsid, 3, PortalId)
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
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
                    'Ultis.UnlockNews(ItemID, UserId)
                    '2. Soft delete
                    ctlNews.UpdateVisible(ItemID, False)
                    '3. Process: Deleted
                    With objProcessInfo
                        .NewsID = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = ctlNews.GetByID(ItemID).Status
                        .ProcessName = BL.msgProcessXoa
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    ctlProcess.Insert(objProcessInfo)
                    '4. Return
                    Response.Redirect(BL.pageDanhSachTin)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub lbtCancelTop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtCancelTop.Click
            Try
                If ajItemID > 0 AndAlso ItemID = 0 Then
                    ItemID = ajItemID
                End If
                '1. Unlock tin bai
                'Ultis.UnlockNews(ItemID, UserId)
                If status = NewsStatus.BiTraLai Then
                    Response.Redirect(BL.pageDanhSachTinBiTraLai)
                Else
                    Response.Redirect(BL.pageDanhSachTin)
                End If
                '2. Return

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
        Protected Sub lbtSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtSave.Click, lbtSave2.Click
            Try

                'For the new Insert
                If ajItemID > 0 AndAlso ItemID = 0 Then
                    ItemID = ajItemID
                End If

                'Unlock tin bai
                'Ultis.UnlockNews(ItemID, UserId)

                If ItemID <> 0 Then
                    'TrungNS: New Process
                    With objProcessInfo
                        .NewsID = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = (New NV_NewsController).GetByID(ItemID).Status
                        .ProcessName = BL.msgProcessEditByCreator
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    ctlProcess.Insert(objProcessInfo)

                    Dim objNewsInfo As NV_NewsInfo = ctlNews.GetByID(ItemID)
                    objNewsInfo = CollectNewsInfo(objNewsInfo)
                    ctlNews.Update(objNewsInfo)
                    '1.4 Nem vao cau hinh tin news_setting
                    Insert_CauHinhTin(ItemID)
                    '1.5 update Tag
                    Ultis.UpdateNewsByTags(ItemID, txtTags.Text)
                Else
                    ' Insert
                    Dim objInfo As NV_NewsInfo = CollectNewsInfo(Nothing)
                    ItemID = ctlNews.Insert(objInfo)
                    ctlNews.UpdateStatus(ItemID, 0, UserId)
                    'Version
                    With objProcessInfo
                        .NewsID = ItemID
                        .CreateDate = DateTime.Now
                        .ByUser = UserId
                        .StatusID = NewsStatus.DangBienSoan 'Khởi tạo
                        .ProcessName = BL.msgProcessCreated
                        .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    End With
                    ctlProcess.Insert(objProcessInfo)
                End If

                'Cap nhat bao bang trung gian News -- Category
                InsertNews_Category(ItemID)
                'TrungNS: Updated for VOV: AUDIO != VIDEO
                'Ultis.InsertMediaFiles(Server, PortalId, UserId, ItemID, hdf_Category.Value, tnUpload.FileList)
                'ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Thực hiện lưu tin bài thành công!');</script>")
                Response.Redirect(NavigateURL() & "?itemid=" + ItemID.ToString, False)
                'Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        ''' <summary>
        ''' Gửi biên tập
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub lbtSaveGuiBientao_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtSaveGuiBientao.Click
            Try
                AddNews()
                '2 Cập nhật nhuận bút vào NEWS

                '3. Process 2 Users
                With objProcessInfo
                    .NewsID = ItemID
                    .CreateDate = DateTime.Now
                    .ByUser = UserId
                    .StatusID = NewsStatus.ChoPheDuyet
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    .ProcessName = "Thực hiện Gửi Biên tập => "
                End With
                ctlProcess.Insert(objProcessInfo)
                '4. Sending
                Dim strToUsers As String = objWF.NguoiNhan
                Dim ctlUserNews As New News_UserProcessController
                Dim objUserNews As New News_UserProcessInfo
                ctlUserNews.DeleteByNewsID(ItemID)
                ctlNews.UpdateStatus(ItemID, NewsStatus.ChoPheDuyet, UserId)
                '5. Unlock tin bai
                'Ultis.UnlockNews(ItemID, UserId)
                '6 Gui mail cho nhom bien tap
                Dim obbjnews As NV_NewsInfo = ctlNews.GetByID(ItemID)
                If Not obbjnews Is Nothing Then
                    With obbjnews
                        Ultis.SendMailThongBaoBai(obbjnews, PortalId)
                    End With
                End If
                '7. Return
                Response.Redirect(BL.pageDanhSachTin, False)
                'Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub lbtSaveGuiXB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtSaveGuiXB.Click
            Try
                AddNews()
                '2 Cập nhật nhuận bút vào NEWS

                '3. Process 2 Users
                With objProcessInfo
                    .NewsID = ItemID
                    .CreateDate = DateTime.Now
                    .ByUser = UserId
                    .StatusID = NewsStatus.ChoXuatBan
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    .ProcessName = "Thực hiện Gửi Xuất bản => "
                End With
                ctlProcess.Insert(objProcessInfo)
                '4. Sending
                Dim ctlUserNews As New News_UserProcessController
                ctlUserNews.DeleteByNewsID(ItemID)
                ctlNews.UpdateStatus(ItemID, NewsStatus.ChoXuatBan, UserId)
                '5. Unlock tin bai
                'Ultis.UnlockNews(ItemID, UserId)
                '6 Gui mail cho nhom bien tap
                Dim obbjnews As NV_NewsInfo = ctlNews.GetByID(ItemID)
                If Not obbjnews Is Nothing Then
                    With obbjnews
                        Ultis.SendMailThongBaoBai(obbjnews, PortalId)
                    End With
                End If
                '7. Return

                Response.Redirect(BL.pageDanhSachTin, False)
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
                    .NewsID = ItemID
                    .CreateDate = DateTime.Now
                    .ByUser = UserId
                    .StatusID = NewsStatus.DaXuatBan
                    .IPTrack = Request.ServerVariables("REMOTE_ADDR")
                    .ProcessName = "Thực hiện Xuất bản ngay => "
                End With
                ctlProcess.Insert(objProcessInfo)
                '3. Update nguoi duyet + xb neu User XB ngay.
                ctlNews.UpdateStatus(ItemID, NewsStatus.ChoXuatBan, UserId)
                ctlNews.UpdatePublishedDate(ItemID, pubDate, UserId)
                '4. Sending
                Dim ctlUserNews As New News_UserProcessController
                ctlUserNews.DeleteByNewsID(ItemID)
                ctlNews.UpdateStatus(ItemID, NewsStatus.DaXuatBan, UserId)
                '5. Unlock tin bai
                'Ultis.UnlockNews(ItemID, UserId)
                'ZZ Xóa cahce để lên bài luôn

                '6. Insert vao bang News_Publish
                Dim savatar As String = "/no-image.png"
                If txtImagePath.Value <> "" Then
                    savatar = txtImagePath.Value.Replace("/DATA", BL.filesDomain)
                End If
                'ctlNews.News_Publish_Insert(ItemID, ddlCategory.SelectedValue, Me.txtTitle.Text, savatar, Me.txtSummary.Value, txtKeyword.Text, teContent.Value, True, CType(IIf(Me.chkHotCat.Checked(), 1, 0), Boolean), CType(IIf(Me.chkHotSite.Checked(), 1, 0), Boolean), NewsStatus.DaXuatBan, hdf_Related.Value, tags.Value, False, CType(IIf(Me.chkIsVideo.Checked(), 1, 0), Boolean), CType(IIf(Me.chkIsPhoto.Checked(), 1, 0), Boolean), txtbutdanh.Value, _SourceText.Value, 0, 0, pubDate, UserId, UserId, PortalId, "vi-VN", BL.maxDateV, "", CType(IIf(Me.chkamp.Checked(), 1, 0), Boolean), "", False, CType(IIf(Me.chkhienbaimoi.Checked(), 1, 0), Boolean), CType(IIf(Me.chkshowQC.Checked(), 1, 0), Boolean))
                '7. Return
                Response.Redirect(BL.pageDanhSachTin, False)
                '8 Clear cache
                DotNetNuke.Common.Utilities.DataCache.ClearCache()
                DotNetNuke.Entities.Host.ServerController.ClearCachedServers()
                Ultis.RecycleApplicationPool("THUONGTRUONG PC")
                'Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
#Region "Logic Handlers"
        Private Sub InsertNews_Category(ByVal newsid As Integer)
            Try
                'TrungNS: News belonging to multiple categories
                NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_DeleteByNewsId(newsid)
                Try
                    NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_Insert(newsid, CType(hdf_Category.Value, Integer), True)
                Catch ex As Exception
                End Try
                'Chuyen mục phu
                Dim slistUnit As String = hdf_subCategories.Value
                If slistUnit.Length > 0 Then
                    Dim slistUnitList As String() = slistUnit.Split(CType(",", Char))
                    For i As Integer = 0 To slistUnitList.Length - 1
                        If IsNumeric(slistUnitList(i)) Then
                            NVCMS.Modules.TinTuc.DataProvider.Instance.NV_NewsByCategory_Insert(newsid, CType(slistUnitList(i), Integer), False)
                        End If
                    Next

                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindddlCategories()
            'Kiểm tra 2 trường hợp
            '1. Tạo mới --> Liệt kê các chuyên mục theo user hiện tại
            '2. Sửa     --> Liệt kê các chuyên mục theo user đã tạo bài đó
            Dim iUserId As Integer = UserId
            If ItemID <> 0 Then
                Dim objNewsCtl As New NV_NewsController
                Dim objNewsInfo As NV_NewsInfo = objNewsCtl.GetByID(ItemID)
                iUserId = objNewsInfo.UserId
            End If

            Dim arrResult As New ArrayList
            Dim ctlNewsCategories As New NV_NewsCategoriesController
            Dim arrAllCategory As New ArrayList
            arrAllCategory = ctlNewsCategories.GetAll(PortalId)

            If UserId = 1 Or UserInfo.IsInRole("Administrators") Or iUserId = 1 Then
                arrResult = arrAllCategory
            Else
                Dim objRoleCtl As New RoleController
                Dim iRoleId As Integer = objRoleCtl.GetRoleByName(PortalId, "Bien Tap").RoleID
                Dim arrTemp As ArrayList = ctlNewsCategories.GetAllCategoriesByUserIdAndRoleId(iUserId, iRoleId, "")
                For Each objTemp As NV_NewsCategoriesInfo In arrAllCategory
                    For Each objTemp1 As NV_NewsCategoriesInfo In arrTemp
                        If objTemp.CategoryID = objTemp1.CategoryID Then
                            arrResult.Add(objTemp)
                        End If
                    Next
                Next
            End If

            Dim arrTempDecentName As New ArrayList
            Dim objNewsCategories As NV_NewsCategoriesInfo
            Dim objNewsCategoriesTemp As NV_NewsCategoriesInfo
            If Not arrResult Is Nothing AndAlso arrResult.Count > 0 Then
                For Each objNewsCategories In arrResult
                    If objNewsCategories.ParentId = 0 Then
                        arrTempDecentName.Add(objNewsCategories)
                        For Each objNewsCategoriesTemp In arrResult
                            If objNewsCategoriesTemp.ParentId = objNewsCategories.CategoryID Then
                                objNewsCategoriesTemp.CategoryName = "|---" & objNewsCategoriesTemp.CategoryName
                                arrTempDecentName.Add(objNewsCategoriesTemp)
                            End If
                        Next
                    End If
                Next
            End If

            Me.ddlCategory.DataSource = arrTempDecentName
            Me.ddlCategory.DataBind()
            ddlCategory.Items.Insert(0, New ListItem("- Chọn chuyên mục -", "0"))
        End Sub
        Private Sub Send_SMSToUsers(ByVal arrUser As List(Of Entities.Users.UserInfo))
            Dim sContent As String = "(TTT) - Tin bài mới, "
            sContent += "tiêu đề: " + txtTitle.Text + ", " + "người gửi: " + UserInfo.DisplayName + ", " + "thời gian: " + DateTime.Now.ToString("HH:mm dd/MM/yyyy")
            For Each obj As Entities.Users.UserInfo In arrUser
                Dim sPhoneNo As String = obj.Profile.GetPropertyValue("Cell")
                If Not String.IsNullOrEmpty(sPhoneNo) Then
                    Ultis.Send_SMS(UserId, PortalId, ModuleId, sPhoneNo, sContent, DateTime.Now)
                End If
            Next
        End Sub
        Private Sub BindRadTreeCategory()
            Dim ctl As New NV_NewsCategoriesController
            Dim arr As ArrayList = ctl.GetAll(PortalId)
            Dim sresult As String = ""
            If arr.Count > 0 Then
                For i As Integer = 0 To arr.Count - 1
                    Dim objcourse As NV_NewsCategoriesInfo = CType(arr(i), NV_NewsCategoriesInfo)
                    Dim arrUnit As New ArrayList
                    If objcourse.IsActive = True Then
                        arrUnit = ctl.GetByParentId(objcourse.CategoryID, PortalId)
                        If arrUnit.Count > 0 Then
                            sresult += "<li id='" & objcourse.CategoryID & "'>" & objcourse.CategoryName
                            sresult += "<ul>"
                            For i2 As Integer = 0 To arrUnit.Count - 1

                                Dim objUnit As NV_NewsCategoriesInfo = CType(arrUnit(i2), NV_NewsCategoriesInfo)
                                If objUnit.IsActive = True Then
                                    sresult += "<li id=" & objUnit.CategoryID & ">" & objUnit.CategoryName & "</li>"
                                End If
                            Next
                            sresult += "</ul>"
                            sresult += "</li>"
                        End If
                    End If

                Next
            End If
            ltrCourseUnit.Text = sresult
        End Sub
        Private Sub ReconfigNodeChecked()
            Dim sresult As String = "<script type='text/javascript'>jQuery(function ($) {"
            Dim arr As ArrayList = (New NV_NewsController).NewsByCategory_GetByNewsId(ItemID)
            'For Each node As RadTreeNode In radTreeCategory.GetAllNodes
            Dim sIdcatselected As String = ""
            If arr.Count > 0 Then
                For Each obj As NewsByCategoryInfo In arr
                    sresult += "$('#checkbox-tree').jstree().select_node(" & obj.CategoryId & ", true);"
                    sIdcatselected += obj.CategoryId & ","
                Next
                sresult += "})</script>"
                ltrscriptSubcat.Text = sresult
                hdf_subCategories.Value = sIdcatselected.Substring(0, sIdcatselected.Length - 1)
            End If

        End Sub

#End Region
#Region "but phe"
        Private Sub BindButPhe()
            'But phe
            Dim ctl As New NewsProcessController
            Dim arrResult As New ArrayList
            Dim arrTemp As ArrayList = _NewsNoteController.News_Note_GetByNewId(ItemID)
            rptNotes.DataSource = arrTemp
            rptNotes.DataBind()
        End Sub
        Protected Sub lbtSendNewsNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtSendNewsNote.Click
            Try
                With objNewsNote
                    .NewId = ItemID
                    .Noidung = txtNoteNews.Text
                    .CreatedDate = DateTime.Now
                    .UserId = UserId
                    .PortalId = PortalId
                End With
                _NewsNoteController.News_Note_Insert(objNewsNote)
                txtNoteNews.Text = ""
                BindButPhe()
            Catch ex As Exception

            End Try
        End Sub
#End Region
#Region "Upload anh bai viet"
        Private Sub BindAnhBaiViet(id As Integer)
            Dim arrMediaNews As New ArrayList
            Dim ctlMediaNews As New NewsByMediaController
            Dim currentMediaByNews = ctlMediaNews._GetAllByNewId(id)
            If Not currentMediaByNews Is Nothing AndAlso currentMediaByNews.Count > 0 Then
                Me.rptphotoatt.DataSource = currentMediaByNews
                Me.rptphotoatt.DataBind()
            End If
        End Sub
        Protected Sub btnxoaanh(ByVal sender As Object, ByVal e As EventArgs)
            Dim itemidhistory As Integer = Integer.Parse(TryCast(sender, Button).CommandArgument)
            'Xoa file vat ly
            Dim objMedia As MediaItemInfo
            objMedia = ctlMedia._GetByID(itemidhistory)
            If Not objMedia Is Nothing Then
                With objMedia
                    Dim FileToDelete As String = .forder & "\" & .filename
                    If System.IO.File.Exists(FileToDelete) = True Then
                        System.IO.File.Delete(FileToDelete)
                    End If
                End With
            End If
            'Xoa bang media
            ctlMedia._Delete(itemidhistory)
            'Xoa Video media
            ctlMediaNews._DeleteByMediaId(itemidhistory)
            BindAnhBaiViet(ItemID)
        End Sub
        Public Function ChoXoaAnh(id As Integer) As Boolean
            Dim objMedia As MediaItemInfo
            objMedia = ctlMedia._GetByID(id)
            If Not objMedia Is Nothing Then
                With objMedia
                    Dim objNews As NV_NewsInfo
                    objNews = ctlNews.GetByID(ItemID)
                    If Not objNews Is Nothing Then
                        If objNews.Content.Contains(.filename) Or objNews.ImagePath.Contains(.filename) Then
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

#End Region
#Region "Nhuan but"
        Private Sub BindNhuanButUser()
            Dim arr As ArrayList = UserController.GetUsers(PortalId)
            Dim arrnew As New ArrayList
            If (arr.Count > 0) Then
                For i As Integer = 0 To arr.Count - 1
                    Dim obj As UserInfo = CType(arr(i), UserInfo)

                    If obj.Membership.Approved = True Then
                        arrnew.Add(obj)
                    End If
                Next
            End If
            Me.ddlnhuanbutuser.DataSource = arrnew
            Me.ddlnhuanbutuser.DataTextField = "DisplayName"
            Me.ddlnhuanbutuser.DataValueField = "UserID"
            Me.ddlnhuanbutuser.DataBind()
            Me.ddlnhuanbutuser.Items.Insert(0, New ListItem("Tác giả", "-1"))
        End Sub
        Private Sub BindNhuanButKieuBai()
            Me.ddlnhuanbuttype.Items.Insert(0, New ListItem("Chọn", "0"))
            Me.ddlnhuanbuttype.Items.Insert(1, New ListItem("Bài", "1"))
            Me.ddlnhuanbuttype.Items.Insert(2, New ListItem("Ảnh", "2"))
            Me.ddlnhuanbuttype.Items.Insert(3, New ListItem("Video", "3"))
            Me.ddlnhuanbuttype.Items.Insert(4, New ListItem("Tin", "4"))
            Me.ddlnhuanbuttype.DataBind()
        End Sub
        Public Function hiennhuanbut() As String
            If Ultis.ButtonXuatBanLuon(UserId) = True Then
                Return ""
            Else
                Return "display:none;"
            End If
        End Function
        Public Sub BindNhuanBut(newid As Integer)
            Dim arrnhuan As New ArrayList
            arrnhuan = ctlnhuabut.NhuanBut_GetAll(newid, KieuNhuanBut.TinBai)
            If Not arrnhuan Is Nothing AndAlso arrnhuan.Count > 0 Then
                Me.rptTacGiaNhuanBut.DataSource = arrnhuan
                Me.rptTacGiaNhuanBut.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Neu la them moi thi moi bat dau chay cai nay
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub lbtaddtacgia_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtaddtacgia.Click
            Try
                If ItemID > 0 Then
                    '1. Add vào bảng News
                    Dim tacgia As String = ""
                    Dim objnew As New NV_NewsInfo
                    objnew = ctlNews.GetByID(ItemID)
                    If Not objnew Is Nothing Then
                        With objnew
                            tacgia = .Tacgia
                        End With
                    End If
                    tacgia += ddlnhuanbutuser.SelectedValue & ";"
                    ctlNews.UpdateTacgia(ItemID, tacgia)
                    '2. Cap nhat tac gia vao bang nhuan but
                    Dim ctlnhuabut As New NhuanButController
                    ctlnhuabut.NhuanBut_Insert(ItemID, ddlnhuanbuttype.SelectedValue, ddlnhuanbutuser.SelectedValue, Regex.Replace(txtcredit1.Text, "[^\d]", ""), DateTime.Now, UserId, UserId, PortalId, KieuNhuanBut.TinBai)
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Thêm tác giả thành công!');</script>")
                    'Them nhuan but vao bai viet
                    ctlNews.UpdateNhuanBut(ItemID, hdf_nhuanbut.Value)
                    'Load dâta
                    BindNhuanBut(ItemID)
                    Me.txtcredit1.Text = 0
                    Me.ddlnhuanbutuser.SelectedValue = -1
                    Me.ddlnhuanbuttype.SelectedValue = 0
                End If

                'Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub cmdXoanhuan(sender As Object, e As EventArgs)
            Try
                Dim idnhuan As Integer = Integer.Parse(TryCast(sender, Button).CommandArgument)
                ''1. Update lai bang Tac gia trong News
                Dim objnhuanbut As NhuanButInfo
                objnhuanbut = ctlnhuabut.NhuanBut_GetByID(idnhuan)
                Dim tacgia As String = ""
                Dim objnew As New NV_NewsInfo
                objnew = ctlNews.GetByID(ItemID)
                If Not objnew Is Nothing Then
                    With objnew
                        tacgia = .Tacgia.Replace(objnhuanbut.UserId & ";", "")
                        ''1. Update lai bang Tac gia trong News
                        ctlNews.UpdateTacgia(ItemID, tacgia)
                        ''1.2 update nhuan but vao news
                        Dim nhuanmoi As Integer = 0
                        nhuanmoi = .Credit - objnhuanbut.Credit
                        txtCredit.Text = nhuanmoi
                        If nhuanmoi > 0 Then
                            ctlNews.UpdateNhuanBut(ItemID, nhuanmoi)
                            txtCredit.Text = nhuanmoi.ToString()
                        Else
                            ctlNews.UpdateNhuanBut(ItemID, 0)
                        End If
                    End With
                End If
                '2 xoa bang nhuan but
                ctlnhuabut.NhuanBut_Delete(idnhuan)
                'cuoi. Rebind
                BindNhuanBut(ItemID)
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Xóa tác giả thành công!');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Protected Sub cmdUpdateNhuan(sender As Object, e As EventArgs)
            Try
                Dim idnhuan As Integer = Integer.Parse(TryCast(sender, Button).CommandArgument)
                Dim item As RepeaterItem = TryCast((TryCast(sender, Button)).NamingContainer, RepeaterItem)
                Dim nhuanbut As Integer = (TryCast(item.FindControl("tiennhuanbut"), TextBox)).Text
                ctlnhuabut.NhuanBut_UpdateNhuan(idnhuan, nhuanbut, UserId)
                '2 Cap nhật nhuận bút vào news
                ctlNews.UpdateNhuanBut(ItemID, Ultis.GetTienNhuanBut(ItemID))
                '3. Rebind
                BindNhuanBut(ItemID)
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Chấm nhuận bút thành công!');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub


#End Region
#Region "Crawler"
        Protected Sub txtLinkCrawler_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                Dim count As Integer = 0
                count = ctlNews.AdminFindSourceText_Count(txtLinkCrawler.Text, PortalId)
                If count > 0 Then
                    ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Link này đã được lấy trước đó!','');</script>")
                Else
                    If Ultis.IsValidURL(txtLinkCrawler.Text) Then
                        ctlMediaNews._DeleteByNewId(ItemID)
                        Dim Url = txtLinkCrawler.Text
                        Dim doc As New HtmlDocument()
                        Dim web As New HtmlWeb()
                        web.UseCookies = True
                        'doc = web.Load(obj.URL)
                        doc = web.Load(txtLinkCrawler.Text)
                        Dim title = Ultis.HtmlAgi(txtLinkCrawler.Text, "og:title").ToString

                        txtTitle.Text = Server.HtmlDecode(title.ToString())
                        txtSummary.Text = Server.HtmlDecode(Ultis.HtmlAgi(txtLinkCrawler.Text, "og:description").ToString)
                        txtSource.Text = txtLinkCrawler.Text
                        ddlImage.SelectedValue = 22
                        Dim dataCrawler As CrawlerDataModel = Crawler_ArticleServices.CrawlingArticleByUrl(Url)
                        If Not dataCrawler Is Nothing Then
                            Try
                                teContent.Value = DownloadImageInContent(ItemID, dataCrawler.Content)
                            Catch ex As Exception
                                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Chịu rồi - Không lấy được nội dung!','');</script>")
                                'ProcessModuleLoadException(Me, ex)
                            End Try

                        End If

                        txtLinkCrawler.Text = ""
                    Else
                        ClientAPI.RegisterStartUpScript(Me.Page, "CanhbaoLinksai", "<script>CanhbaoLinksai();</script>")
                    End If
                End If
            Catch exc As Exception
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateError", "<script>UpdateError('Chịu rồi!','');</script>")
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Function DownloadImageInContent(ByVal ItemID As Integer, ByVal stringHtml As String) As String
            Try
                PhotoPhysicPath = Ultis.GetImagePath(False, PortalSettings.Current.PortalId, True)
                PhotoVirtualPath = Ultis.GetImagePath(True, PortalSettings.Current.PortalId, True)
                Dim fullpath As String = PhotoPhysicPath & "\"
                Dim articleModel = ctlNews.GetByID(ItemID)
                If articleModel IsNot Nothing Then
                    If String.IsNullOrEmpty(stringHtml) Then Return Nothing
                    Dim doc = New HtmlDocument()
                    doc.LoadHtml(stringHtml)
                    Dim listImageNodes = doc.DocumentNode.SelectNodes("//img")
                    If listImageNodes IsNot Nothing Then
                        For i As Integer = 0 To listImageNodes.Count - 1
                            Dim imageNode = listImageNodes(i)
                            Dim imageSrc = imageNode.Attributes("src").Value
                            If Not String.IsNullOrEmpty(imageSrc) Then
                                Dim regexhttp = Regex.Match(imageSrc, "http").Captures
                                Dim hasHttp = False
                                If regexhttp.Count > 1 Then imageSrc = imageSrc.Substring(imageSrc.LastIndexOf("http", StringComparison.Ordinal))
                                If imageSrc.Contains("http") OrElse imageSrc.Contains("https") Then hasHttp = True
                                If hasHttp Then
                                    Dim imagenamenews2 As String = Ultis.CrawlerGetNameNameL(imageSrc)
                                    Dim extension = Path.GetExtension(Path.GetFileName(imagenamenews2))
                                    If extension.Contains("?") Then
                                        extension = extension.Substring(0, extension.IndexOf("?", StringComparison.Ordinal))
                                    End If
                                    Dim sextension = extension.Remove(0, 1)
                                    Dim imagenamenews As String = UserId & "-" & i & "-" & DateTime.Now.ToString("yyyyMMddHHss") & extension

                                    My.Computer.Network.DownloadFile(imageSrc, fullpath & imagenamenews)
                                    Dim filezie = New FileInfo(Path.Combine(fullpath, Path.GetFileName(imagenamenews))).Length

                                    stringHtml = stringHtml.Replace(imageSrc, PhotoVirtualPath & "/" & imagenamenews)

                                    Dim idmedia As Integer = 0
                                    idmedia = ctlMedia._Insert(imagenamenews, imagenamenews, Ultis.GetImagePath(False, PortalSettings.Current.PortalId, True), Ultis.GetMediaPath(PhotoVirtualPath, imagenamenews), filezie, sextension, DateTime.Now, UserId, PortalId)
                                    'chen vao bang product media
                                    ctlMediaNews._Insert(ItemID, idmedia, DateTime.Now, UserController.Instance.GetCurrentUserInfo().UserID, PortalSettings.Current.PortalId)
                                    'Else
                                    '    imageNode.Remove()
                                    'End If

                                End If
                            Else
                                imageNode.Remove()
                            End If
                        Next
                    End If

                    Dim outhtml = Regex.Replace(stringHtml, "\<[\/]*a[^\>]*\>", "")
                    BindAnhBaiViet(ItemID)
                    Return outhtml
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

            Return stringHtml
        End Function


#End Region
    End Class
End Namespace
