Imports System
Imports DotNetNuke.Entities.Controllers
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Display.News

    Public MustInherit Class SettingNewsCategory
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase


#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    BindDdlStatus()
                    If IsNumeric(ModuleSettings("VideoAdminVideoStatus")) Then
                        Me.ddlStatus.SelectedValue = CType(ModuleSettings("VideoAdminVideoStatus"), Integer)
                    End If

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

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
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController

                objModules.UpdateModuleSetting(ModuleId, "VideoAdminVideoStatus", Me.ddlStatus.SelectedValue)
                DataCache.ClearCache()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub BindDdlStatus()
            Dim ctlStatus As New NV_NewsStatusController
            Me.ddlStatus.DataSource = ctlStatus.NV_NewsStatus_GetAll
            Me.ddlStatus.DataTextField = "StatusName"
            Me.ddlStatus.DataValueField = "NewsStatusId"
            Me.ddlStatus.DataBind()
            Me.ddlStatus.Items.Insert(0, New ListItem(" - Tất cả - ", "-1"))
        End Sub
#End Region

    End Class
End Namespace