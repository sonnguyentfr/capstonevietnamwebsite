Imports System
Imports DotNetNuke.Entities.Controllers

Namespace NVCMS.Modules.School

    Public MustInherit Class SettingNewsCategory
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase


#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    BLStudent.Search_BindDDLLocation(0, ddlQuocGia)
                    Me.ddlQuocGia.Items.Insert(0, New ListItem("--Tất cả--", -1))
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings("ShoolMarketing_Show_QuocGia")) Then
                        Me.ddlQuocGia.SelectedValue = ModuleConfiguration.ModuleSettings("ShoolMarketing_Show_QuocGia").ToString()
                    End If
                    BLSchool.Search_BindLoaiTruongShow(1010, ddlLoaiTruong)
                    Me.ddlLoaiTruong.Items.Insert(0, New ListItem("--Tất cả--", -1))
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings("ShoolMarketing_Show_LoaiTruong")) Then
                        Me.ddlLoaiTruong.SelectedValue = ModuleConfiguration.ModuleSettings("ShoolMarketing_Show_LoaiTruong").ToString()
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
                objModules.UpdateModuleSetting(ModuleId, "ShoolMarketing_Show_QuocGia", Me.ddlQuocGia.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "ShoolMarketing_Show_LoaiTruong", Me.ddlLoaiTruong.SelectedValue)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


#End Region

    End Class
End Namespace