Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.HeThong
Namespace NVCMS.Modules.Banner
    Partial Class editbanner
        Inherits Entities.Modules.PortalModuleBase
#Region "Propertice"
        Dim ctlAdvBanner As New BannerAdvController
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
#End Region
#Region "pageLoad"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    BindVitri()
                    BindKieuQuangcao()
                    If Request.Item("itemid") <> "" Then
                        ItemID = CInt(Request.Item("itemid"))
                        BindData(ItemID)
                    End If
                    PhotoAbPath = nvcmsBL.GetImagePath(False, PortalId, True)
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region
#Region "Bind dataa"
        Private Sub BindData(id As Integer)
            Dim objAdvBannervitri As BannerAdvInfo
            objAdvBannervitri = ctlAdvBanner.GetByID(id)
            If Not objAdvBannervitri Is Nothing Then
                With objAdvBannervitri
                    Me.Title.Value = .Title
                    Me.Descreption.Value = .Contact
                    ddlkieubanner.SelectedValue = .KieuBanner
                    If .KieuBanner = 1 Then
                        hpflinkimage.Value = .IMGLink
                        If Not .IMGLink Is Nothing Then
                            If .IMGLink.ToString.Contains("http://") Then
                                Me.dvPreviewlogo.Visible = True
                                Me.dvPreviewlogo.InnerHtml = "<img src=""" & .IMGLink & """ height='100px' />"
                            Else
                                Me.dvPreviewlogo.Visible = True
                                'Me.imgLogo.ImageUrl = .Logo
                                Me.dvPreviewlogo.InnerHtml = "<img src=""" & .IMGLink & """  height='100px' />"
                            End If
                        End If
                        Me.txtLink.Value = .Link
                    End If
                    If .KieuBanner = 3 Then
                        Me.txtcode.Value = .IMGLink
                    End If
                    Me.txtdai.Value = .Width
                    Me.txtCao.Value = .Height
                    tungay.Value = .Startdate.ToShortDateString()
                    ddlCategory.SelectedValue = .Vitri
                    If .Vitri > 0 Then
                        BindImageVitri(.Vitri)
                    End If
                    dengay.Value = .enddate.ToShortDateString()
                    Me.thutu.Value = .Ordernumber
                    chkactive.Checked = .Visible
                End With
            End If
        End Sub
        Private Sub BindVitri()
            Dim ctlVideos As New BannerAdv_VitriController
            Me.ddlCategory.DataSource = ctlVideos._Vitri_GetAll(PortalId)
            Me.ddlCategory.DataTextField = "Title"
            Me.ddlCategory.DataValueField = "id"
            Me.ddlCategory.DataBind()
            Me.ddlCategory.Items.Insert(0, New ListItem("--Chọn vị trí--", "0"))
        End Sub
        Public Sub SetKichThuoc(id As Integer)
            Dim ctl As New BannerAdv_VitriController
            Dim objInfo As BannerAdv_VitriInfo
            objInfo = ctl._Vitri_GetByID(id)
            If Not objInfo Is Nothing Then
                With objInfo
                    Me.txtCao.Value = .height
                    Me.txtdai.Value = .width
                End With
            End If
        End Sub
        Public Sub BindImageVitri(id As Integer)
            Dim ctl As New BannerAdv_VitriController
            Dim objInfo As BannerAdv_VitriInfo
            objInfo = ctl._Vitri_GetByID(id)
            If Not objInfo Is Nothing Then
                With objInfo
                    Me.imgshowvitri.ImageUrl = .Images
                End With
            End If
        End Sub
        Public Sub ddlvitri_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCategory.SelectedIndexChanged
            BindImageVitri(ddlCategory.SelectedValue)
            SetKichThuoc(ddlCategory.SelectedValue)
        End Sub
        ''' <summary>
        ''' Kieu banner
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' 
        Private Sub BindKieuQuangcao()
            Dim items As ListItem() = New ListItem(1) {}
            items(0) = New ListItem("- Ảnh -", "1")
            items(1) = New ListItem("- Code -", "3")
            ddlkieubanner.Items.AddRange(items)
            ddlkieubanner.DataBind()

        End Sub
#End Region
#Region "action"
        Private Function addnews() As Boolean
            Try

                Dim strFileName As String = ""
                Dim strFileNamePath As String = ""
                If ddlkieubanner.SelectedValue = 1 Then
                    If Me.filelogo.PostedFile.FileName <> "" Then
                        strFileName = System.IO.Path.GetFileName(Me.filelogo.PostedFile.FileName)
                        Me.filelogo.PostedFile.SaveAs(PhotoAbPath & "/" & strFileName)
                        strFileNamePath = GetMediaPath(PhotoVirPath, Me.filelogo.PostedFile.FileName)
                    Else
                        strFileNamePath = hpflinkimage.Value
                    End If
                End If
                If ddlkieubanner.SelectedValue = 3 Then
                    strFileNamePath = txtcode.Value
                End If

                Dim sStart As DateTime = nvcmsBL.minDateV
                If (Me.tungay.Value <> "") And (IsDate(Me.dengay.Value)) Then
                    sStart = Me.tungay.Value
                End If
                Dim sEnd As DateTime = nvcmsBL.maxDateV
                If (Me.dengay.Value <> "") And (IsDate(Me.dengay.Value)) Then
                    sEnd = Me.dengay.Value
                End If
                Dim iIsActive As Integer = CInt(IIf(Me.chkactive.Checked(), 1, 0))
                If ItemID > 0 Then
                    ctlAdvBanner.Update(ItemID, Me.Title.Value, ddlkieubanner.SelectedValue, strFileNamePath, ddlCategory.SelectedValue, txtCao.Value, txtdai.Value, PortalId, UserId, iIsActive, DateTime.Now, IIf(IsNumeric(Me.thutu.Value), Me.thutu.Value, 0), txtLink.Value, sStart, sEnd, Me.Descreption.Value)
                Else
                    ctlAdvBanner.Insert(Me.Title.Value, ddlkieubanner.SelectedValue, strFileNamePath, ddlCategory.SelectedValue, txtCao.Value, txtdai.Value, PortalId, UserId, iIsActive, DateTime.Now, IIf(IsNumeric(Me.thutu.Value), Me.thutu.Value, 0), txtLink.Value, sStart, sEnd, Me.Descreption.Value)
                End If
                Return True
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
                Return False
            End Try
        End Function
        Private Sub lbtCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub lbtUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtUpdate.Click
            If addnews() Then
                BindData(ItemID)
                Response.Redirect(NavigateURL(), True)
            End If
        End Sub
        Private Sub clearcontroldata()
            Me.Title.Value = ""
            Me.Descreption.Value = ""
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