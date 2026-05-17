Imports System
Imports DotNetNuke
Imports System.IO
Imports NVCMS.Modules.Events
Imports Telerik.Web.UI

Namespace DesktopModules.NV_Events.Manager.Events
    Public MustInherit Class Edit
        Inherits Entities.Modules.PortalModuleBase

        Public Property PhotosAbPath() As String
            Get
                If Not ViewState.Item("PhotosAbPath") Is Nothing Then
                    Return CType(ViewState.Item("PhotosAbPath"), String)
                Else
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("PhotosAbPath", value)
            End Set
        End Property
        Public Property PhotosVirPath() As String
            Get
                If Not ViewState.Item("PhotosVirPath") Is Nothing Then
                    Return CType(ViewState.Item("PhotosVirPath"), String)
                Else
                    Return Ultis.GetImagePath(True, PortalId, True)
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("PhotosVirPath", value)
            End Set
        End Property
        'TrungNS: Sử dụng biến Viewstate lưu ID => Phạm vi trên trang
        'Chỉ cần request 1 lần, và có thể dùng lại ở tất cả hàm khác.
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
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                Try
                    BindFindFormData()
                    BindddlDanhmuc()
                    If IsNumeric(Request.Item("itemid")) Then 'TrungNS: Tránh tấn công
                        ItemID = CType(Request.Item("itemid"), Integer)
                        Dim ctlVideo As New EventsController
                        Dim objInfo As EventsInfo
                        objInfo = ctlVideo.Events_GetByID(ItemID, PortalId)
                        If Not objInfo Is Nothing Then
                            With objInfo 'TrungNS: Sử dụng with => Giảm lượng code, trông ngăn nắp: neaty
                                Me.txtTitle.Text = .Title
                                Me.ddlDanhmuc.SelectedValue = .CatId
                                Me.chkshow.Checked = CType(.Isactive, Boolean)
                                Me.txtStartdate.Text = .fromdatetime.ToShortDateString()
                                Me.ddlGio.SelectedValue = .fromdatetime.ToString("HH")
                                Me.ddlPhut.SelectedValue = .fromdatetime.ToString("mm")
                                Me.txtdiadiem.Text = .diadiem
                                Me.txtthanhphan.Text = .thanhphan
                                Me.txtContactName.Text = .LienheName
                                Me.txtContactMail.Text = .LienheEmail
                                Me.txtContactPhone.Text = .LienheMobile
                                Me.txtContactAdd.Value = .LienheAdd
                                If Not .Avatar Is Nothing Then
                                    Me.dvPreviewlogo.InnerHtml = "<img src=""" & .Avatar & """  height='100px' />"
                                    hpflinkimage.Value = objInfo.Avatar
                                End If
                                If Ultis.Events_CheckHienTrangChu(.id, .Portalid) Then
                                    chkHienTrangChu.Checked = True
                                End If
                                ddlhinhthuc.SelectedValue = .hinhthuc
                                'TrungNS: Cập nhật đường dẫn cũ cho Radasyncupload
                                Me.TextEditor1.Value = .Descreption
                            End With
                        End If
                    Else
                        Me.lbtDeleteBottom.Visible = False
                        Me.txtStartdate.Text = DateTime.Now.ToShortDateString()
                        Me.ddlGio.SelectedValue = DateTime.Now.ToString("HH")
                        Me.ddlPhut.SelectedValue = DateTime.Now.ToString("mm")
                    End If
                    'TrungNS: Khởi tạo đường dẫn mới, ex: Vir/BaiHat/2013/12
                    'AvatarAbPath = Ultis.CreateAvatarDir(PhotosAbPath)
                    'ruMediaImage.TargetFolder = AvatarAbPath


                    PhotosAbPath = Ultis.GetImagePath(False, PortalId, True)
                    lbtUpdateBottom.Attributes.Add("onClick", "javascript:return check();")
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
        Private Function addnews() As Boolean
            Try
                Dim ctl As New EventsController
                Dim iDanhmuc As Integer = CType(Me.ddlDanhmuc.SelectedItem.Value, Integer)
                Dim ihinhthuc As Integer = CType(Me.ddlhinhthuc.SelectedItem.Value, Integer)
                Dim icheckshow As Integer = CInt(IIf(Me.chkshow.Checked(), 1, 0))
                Dim iGio As Integer = CType(Me.ddlGio.SelectedItem.Value, Integer)
                Dim iPhut As Integer = CType(Me.ddlPhut.SelectedItem.Value, Integer)
                Dim sDate As String = Me.txtStartdate.Text & " " & iGio & ":" & iPhut
                Dim startDate As Date = CDate(sDate)
                Dim sdiadiem As String = Me.txtdiadiem.Text
                Dim sthanhphan As String = Me.txtthanhphan.Text
                Dim sLienheName As String = Me.txtContactName.Text
                Dim sLienHeEmail As String = Me.txtContactMail.Text
                Dim sLienHePhone As String = Me.txtContactPhone.Text
                Dim sLienheAdd As String = Me.txtContactAdd.Value
                'upload anh dai dien
                Dim strFileName As String = ""
                Dim strFileNamePath As String = ""
                If Me.filelogo.PostedFile.FileName <> "" Then
                    strFileName = System.IO.Path.GetFileName(Me.filelogo.PostedFile.FileName)
                    Me.filelogo.PostedFile.SaveAs(PhotosAbPath & "/" & strFileName)
                    strFileNamePath = GetMediaPath(PhotosVirPath, Me.filelogo.PostedFile.FileName)
                Else
                    strFileNamePath = hpflinkimage.Value
                End If
                If ItemID > 0 Then
                    ctl.Events_Update(ItemID, Me.txtTitle.Text, iDanhmuc, strFileNamePath, sdiadiem, startDate, DateTime.Now, sthanhphan, 0, Me.TextEditor1.Value, ihinhthuc, sLienheName, sLienHeEmail, sLienHePhone, sLienheAdd, UserId, PortalId, DateTime.Now, icheckshow)
                Else
                    ItemID = ctl.Events_Insert(Me.txtTitle.Text, iDanhmuc, strFileNamePath, sdiadiem, startDate, DateTime.Now, sthanhphan, 0, Me.TextEditor1.Value, ihinhthuc, sLienheName, sLienHeEmail, sLienHePhone, sLienheAdd, UserId, PortalId, DateTime.Now, icheckshow)

                End If
                Insert_CauHinhTin(ItemID)
                Return True
            Catch ex As Exception
                Me.lbResult.Text = "Lỗi cập nhật tin"
                ProcessModuleLoadException(Me, ex)
                Return False
            End Try
        End Function
        Private Sub lbtUpdateBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtUpdateBottom.Click
            Try
                If addnews() Then
                    Response.Redirect(NavigateURL(), True)
                End If
            Catch ex As Exception
                Me.lbResult.Text = "Lỗi cập nhật tin"
                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub
        Private Sub lbtCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Protected Sub lbtDeleteBottom_Click(sender As Object, e As System.EventArgs) Handles lbtDeleteBottom.Click
            Dim ctl As New EventsController
            ctl.Events_Delete(ItemID, PortalId)
            Response.Redirect(NavigateURL(), True)
        End Sub
        Private Sub BindddlDanhmuc()
            Dim ctlNewsCategories As New Events_CatController
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = ctlNewsCategories.Events_Cat_GetAll(PortalId)
            Me.ddlDanhmuc.DataSource = arrNewsCategories
            Me.ddlDanhmuc.DataTextField = "CatName"
            Me.ddlDanhmuc.DataValueField = "id"
            Me.ddlDanhmuc.DataBind()
        End Sub
        Private Function GetMediaPath(ByVal foldername As String, ByVal radupload As RadAsyncUpload) As String
            If radupload.UploadedFiles.Count > 0 Then
                Dim f As UploadedFile = radupload.UploadedFiles(0)
                Return foldername & "/" & f.FileName
            Else
                Return ""
            End If
        End Function
        Private Sub BindFindFormData()
            Dim arrGio As New DataTable
            Dim arrPhut As New DataTable
            '-------------------------------------
            Dim arrGioEnd As New DataTable
            Dim arrPhutEnd As New DataTable
            '------------------------------
            Dim tDataRow As DataRow

            arrGio.Columns.Add(New DataColumn("Name", System.Type.GetType("System.String")))
            arrGio.Columns.Add(New DataColumn("Value", System.Type.GetType("System.String")))

            arrPhut.Columns.Add(New DataColumn("Name", System.Type.GetType("System.String")))
            arrPhut.Columns.Add(New DataColumn("Value", System.Type.GetType("System.String")))
            '----------------------------------------------------
            arrGioEnd.Columns.Add(New DataColumn("Name", System.Type.GetType("System.String")))
            arrGioEnd.Columns.Add(New DataColumn("Value", System.Type.GetType("System.String")))

            arrPhutEnd.Columns.Add(New DataColumn("Name", System.Type.GetType("System.String")))
            arrPhutEnd.Columns.Add(New DataColumn("Value", System.Type.GetType("System.String")))
            '-------------------------------------------------------

            Dim i As Integer
            For i = 0 To 24
                tDataRow = arrGio.NewRow
                tDataRow.Item("Name") = i
                tDataRow.Item("Value") = i
                arrGio.Rows.Add(tDataRow)
            Next
            For i = 0 To 59
                tDataRow = arrPhut.NewRow
                tDataRow.Item("Name") = i
                tDataRow.Item("Value") = i
                arrPhut.Rows.Add(tDataRow)
            Next
            '------------------------------------------
            For i = 0 To 24
                tDataRow = arrGioEnd.NewRow
                tDataRow.Item("Name") = i
                tDataRow.Item("Value") = i
                arrGioEnd.Rows.Add(tDataRow)

            Next
            '------------------------------------------
            For i = 0 To 59
                tDataRow = arrPhutEnd.NewRow
                tDataRow.Item("Name") = i
                tDataRow.Item("Value") = i
                arrPhutEnd.Rows.Add(tDataRow)
            Next
            '------------------------------------------
            Me.ddlGio.DataSource = arrGio
            Me.ddlGio.DataTextField = "Name"
            Me.ddlGio.DataValueField = "Value"
            Me.ddlGio.DataBind()

            Me.ddlPhut.DataSource = arrPhut
            Me.ddlPhut.DataTextField = "Name"
            Me.ddlPhut.DataValueField = "Value"
            Me.ddlPhut.DataBind()
            '------------------------------------------
            Me.ddlGioEnd.DataSource = arrGioEnd
            Me.ddlGioEnd.DataTextField = "Name"
            Me.ddlGioEnd.DataValueField = "Value"
            Me.ddlGioEnd.DataBind()

            Me.ddlPhutend.DataSource = arrPhutEnd
            Me.ddlPhutend.DataTextField = "Name"
            Me.ddlPhutend.DataValueField = "Value"
            Me.ddlPhutend.DataBind()
        End Sub
#End Region
#Region "Insert Trang chu"
        Public Sub Insert_CauHinhTin(ByVal eventId As Integer)
            Try
                Dim sSettings = PortalController.GetPortalSetting("settingsEventsHOME", PortalId, Null.NullString)
                If chkHienTrangChu.Checked = True Then
                    If Ultis.Events_CheckHienTrangChu(eventId, PortalId) = False Then
                        PortalController.UpdatePortalSetting(PortalId, "settingsEventsHOME", eventId & "," & sSettings, True)
                    End If
                Else
                    Dim sSettingsnews = sSettings.Replace(sSettings, eventId & ",")
                    PortalController.UpdatePortalSetting(PortalId, "settingsEventsHOME", sSettingsnews, True)
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
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
        'upload hop dong
#End Region

    End Class
End Namespace