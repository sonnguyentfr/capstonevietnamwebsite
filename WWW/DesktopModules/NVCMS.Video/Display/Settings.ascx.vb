Imports System
Imports DotNetNuke.Entities.Controllers
Imports ThuongTruong.Modules.TinTuc

Namespace ThuongTruong.Modules.Video

    Public MustInherit Class Settings
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase


#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings(BL.settingList_PageSize)) Then
                        txtList_PageSize.Text = ModuleConfiguration.ModuleSettings(BL.settingList_PageSize).ToString()
                    End If
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings(BL.settingDetails_More)) Then
                        txtDetails_More.Text = ModuleConfiguration.ModuleSettings(BL.settingDetails_More).ToString()
                    End If
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings(BL.settingList_ShowPage)) Then
                        checkList_ShowPage.Checked = Convert.ToBoolean(ModuleConfiguration.ModuleSettings(BL.settingList_ShowPage))
                    End If
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings(BL.settingList_Top)) Then
                        checkList_ShowTop.Checked = Convert.ToBoolean(ModuleConfiguration.ModuleSettings(BL.settingList_Top))
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

                objModules.UpdateModuleSetting(ModuleId, BL.settingList_PageSize, txtList_PageSize.Text)
                objModules.UpdateModuleSetting(ModuleId, BL.settingDetails_More, txtDetails_More.Text)
                objModules.UpdateModuleSetting(ModuleId, BL.settingList_ShowPage, checkList_ShowPage.Checked)
                objModules.UpdateModuleSetting(ModuleId, BL.settingList_Top, checkList_ShowTop.Checked)

                DataCache.ClearCache()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region

    End Class
End Namespace