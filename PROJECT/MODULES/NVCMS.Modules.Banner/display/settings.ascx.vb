Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions

Namespace NVCMS.Modules.Banner
    Public MustInherit Class settings
        Inherits ModuleSettingsBase
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
#End Region
#Region "pageLoad"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    BindVitri()
                    BindTemplate()
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings("NVCMSBannerVitriSetting")) Then
                        Me.ddlvitri.SelectedValue = ModuleConfiguration.ModuleSettings("NVCMSBannerVitriSetting").ToString()
                    End If
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings("NVCMSBannerTemplateSetting")) Then
                        Me.dropTemplate.SelectedValue = ModuleConfiguration.ModuleSettings("NVCMSBannerTemplateSetting").ToString()
                    End If
                    If Not Null.IsNull(ModuleSettings("NVCMSBannerShowTitleSetting")) Then
                        Dim sAllow As String = ModuleSettings("NVCMSBannerShowTitleSetting").ToString()
                        chkshowtieude.Checked = Convert.ToBoolean(sAllow)
                    End If
                    If Not Null.IsNull(ModuleSettings("NVCMSBannerShowMotaSetting")) Then
                        Dim sAllow As String = ModuleSettings("NVCMSBannerShowMotaSetting").ToString()
                        chkmota.Checked = Convert.ToBoolean(sAllow)
                    End If
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region
#Region "Bind dataa"
        Private Sub BindVitri()
            Dim ctlVideos As New BannerAdv_VitriController
            Me.ddlvitri.DataSource = ctlVideos._Vitri_GetAll(PortalId)
            Me.ddlvitri.DataTextField = "Title"
            Me.ddlvitri.DataValueField = "id"
            Me.ddlvitri.DataBind()
            Me.ddlvitri.Items.Insert(0, New ListItem("--Chọn vị trí--", "0"))
        End Sub
        'Public Sub BindImageVitri(id As Integer)
        '    Dim ctl As New BannerAdv_VitriController
        '    Dim objInfo As BannerAdv_VitriInfo
        '    objInfo = ctl._Vitri_GetByID(id)
        '    If Not objInfo Is Nothing Then
        '        With objInfo
        '            Me.imgshowvitri.ImageUrl = .Images
        '        End With
        '    End If
        'End Sub
        'Public Sub ddlvitri_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlvitri.SelectedIndexChanged
        '    BindImageVitri(ddlvitri.SelectedValue)
        'End Sub
        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[cnurse]	10/22/2004	created
        ''' </history>
        ''' -----------------------------------------------------------------------------
        ''' 
        Private Sub BindTemplate()
            Dim arrTemplate As New ArrayList
            Dim ctltem As New TemplateController
            arrTemplate = ctltem._GetAll(0)
            dropTemplate.DataSource = arrTemplate
            dropTemplate.DataTextField = "TemplateName"
            dropTemplate.DataValueField = "FilePath"
            dropTemplate.DataBind()
        End Sub
#End Region
#Region "action"
        Public Overrides Sub UpdateSettings()
            Try
                Dim schktieude As String = chkshowtieude.Checked.ToString()
                Dim schkmota As String = chkmota.Checked.ToString()

                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                objModules.UpdateModuleSetting(ModuleId, "NVCMSBannerVitriSetting", Me.ddlvitri.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "NVCMSBannerTemplateSetting", Me.dropTemplate.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "NVCMSBannerShowTitleSetting", schktieude)
                objModules.UpdateModuleSetting(ModuleId, "NVCMSBannerShowMotaSetting", schkmota)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region

    End Class
End Namespace