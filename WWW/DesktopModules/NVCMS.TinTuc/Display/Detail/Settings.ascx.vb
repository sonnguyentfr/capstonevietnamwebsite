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
                    BindddlCategories()
                    Dim iCat As Integer = HostController.Instance.GetInteger("IndexCategorySetting_" + PortalSettings.ActiveTab.TabID.ToString(), 0)
                    If iCat > 0 Then
                        Me.ddlCategory.Items.FindByValue(iCat.ToString()).Selected = True
                    End If
                    If IsNumeric(ModuleSettings("NVNewsDisplayPageSetting")) Then
                        Me.txtDisplayNewsPage.Text = CType(ModuleSettings("NVNewsDisplayPageSetting"), String)
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

                If IsNumeric(Me.txtDisplayNewsPage.Text) Then
                    objModules.UpdateModuleSetting(ModuleId, "NVNewsDisplayPageSetting", Me.txtDisplayNewsPage.Text)
                End If
                HostController.Instance.Update("IndexCategorySetting_" + PortalSettings.ActiveTab.TabID.ToString(), Me.ddlCategory.SelectedValue, False)
                DataCache.ClearCache()
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub BindddlCategories()
            Dim ctlNewsCategories As New NV_NewsCategoriesController
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = ctlNewsCategories.GetAll(PortalId)
            Dim arrTemp As New ArrayList
            Dim objNewsCategories As NV_NewsCategoriesInfo
            Dim objNewsCategoriesTemp As NV_NewsCategoriesInfo

            If arrNewsCategories.Count > 0 Then
                For Each objNewsCategories In arrNewsCategories
                    If objNewsCategories.ParentId = 0 Then
                        arrTemp.Add(objNewsCategories)
                        For Each objNewsCategoriesTemp In arrNewsCategories
                            If objNewsCategoriesTemp.ParentId = objNewsCategories.CategoryID Then
                                objNewsCategoriesTemp.CategoryName = "--" & objNewsCategoriesTemp.CategoryName
                                arrTemp.Add(objNewsCategoriesTemp)
                            End If
                        Next
                    End If
                Next
            End If
            Me.ddlCategory.DataSource = arrTemp
            Me.ddlCategory.DataTextField = "CategoryName"
            Me.ddlCategory.DataValueField = "CategoryId"
            Me.ddlCategory.DataBind()
            Me.ddlCategory.Items.Insert(0, New ListItem("--Tất cả thư mục--", 0))
        End Sub

#End Region

    End Class
End Namespace