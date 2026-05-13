Imports System
Imports DotNetNuke
Imports DotNetNuke.Security.Roles
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.Users
Imports DotNetNuke.Entities.Modules

Namespace DesktopModules.TinTuc.Manager.permission
        Partial Public Class permissionSetting
            Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

            Public Overrides Sub LoadSettings()
                Try
                    Dim sLanguage As String = TNFormerStudentHepler.GetCurrentLanguage(Response)
                    Dim arrGroups As ArrayList = RoleController.GetRoleGroups(PortalId)
                    drdRoleGroup.DataSource = arrGroups
                    drdRoleGroup.DataBind()

                    If CType(Settings(TNFormerStudentContants.RoleGroupIdSetting + sLanguage + PortalId.ToString), String) <> "" Then
                        drdRoleGroup.SelectedValue = Convert.ToInt32(Settings(TNFormerStudentContants.RoleGroupIdSetting + sLanguage + PortalId.ToString))
                    End If
                Catch exc As Exception
                    ProcessModuleLoadException(Me, exc)
                End Try
            End Sub

            Public Overrides Sub UpdateSettings()
                Try
                    Dim sLanguage As String = TNFormerStudentHepler.GetCurrentLanguage(Response)
                    Dim ctlModule As ModuleController = New ModuleController
                    ctlModule.UpdateModuleSetting(ModuleId, TNFormerStudentContants.RoleGroupIdSetting + sLanguage + PortalId.ToString, drdRoleGroup.SelectedValue.ToString)

                Catch exc As Exception
                    ProcessModuleLoadException(Me, exc)
                End Try
            End Sub

#End Region
        End Class
End Namespace
