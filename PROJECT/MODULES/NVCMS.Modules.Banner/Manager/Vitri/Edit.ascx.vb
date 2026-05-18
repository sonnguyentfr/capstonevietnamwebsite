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
    Partial Class Edit
        Inherits Entities.Modules.PortalModuleBase
#Region "Propertice"
        Dim ctlAdvBanner As New BannerAdv_VitriController
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
            Dim objAdvBannervitri As BannerAdv_VitriInfo
            objAdvBannervitri = ctlAdvBanner._Vitri_GetByID(id)
            If Not objAdvBannervitri Is Nothing Then
                With objAdvBannervitri
                    Me.Title.Value = .Title
                    Me.txtdai.Value = .width
                    Me.txtCao.Value = .height
                    hpflinkimage.Value = .Images
                    If Not .Images Is Nothing Then
                        If .Images.ToString.Contains("http://") Then
                            Me.dvPreviewlogo.Visible = True
                            Me.dvPreviewlogo.InnerHtml = "<img src=""" & .Images & """ height='100px' />"
                        Else
                            Me.dvPreviewlogo.Visible = True
                            'Me.imgLogo.ImageUrl = .Logo
                            Me.dvPreviewlogo.InnerHtml = "<img src=""" & .Images & """  height='100px' />"
                        End If
                    End If
                End With
            End If
        End Sub

#End Region
#Region "action"
        Private Function addnews() As Boolean
            Try

                Dim strFileName As String = ""
                Dim strFileNamePath As String = ""
                If Me.filelogo.PostedFile.FileName <> "" Then
                    strFileName = System.IO.Path.GetFileName(Me.filelogo.PostedFile.FileName)
                    Me.filelogo.PostedFile.SaveAs(PhotoAbPath & "/" & strFileName)
                    strFileNamePath = GetMediaPath(PhotoVirPath, Me.filelogo.PostedFile.FileName)
                Else
                    strFileNamePath = hpflinkimage.Value
                End If

                If ItemID > 0 Then
                    ctlAdvBanner._Vitri_Update(ItemID, Me.Title.Value, txtdai.Value, txtCao.Value, strFileNamePath, UserId, DateTime.Now)
                Else
                    ctlAdvBanner._Vitri_Insert(Me.Title.Value, txtdai.Value, txtCao.Value, strFileNamePath, UserId, DateTime.Now, UserId, DateTime.Now, ModuleId, PortalContextHelper.CurrentPortal.PortalId)
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