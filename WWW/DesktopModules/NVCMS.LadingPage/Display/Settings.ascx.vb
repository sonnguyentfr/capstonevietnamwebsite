Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.LadingPage

    Public MustInherit Class SettingCustomeDisplay
        Inherits DotNetNuke.Entities.Modules.ModuleSettingsBase
        Dim _LadingPage_Controller As New LadingPage_Controller
#Region "Event Handlers"
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then
                    BindTrang()
                    BindTemplate()
                    If CType(ModuleSettings("TrangLadingPage_Id"), String) <> "" Then
                        Me.ddlTrangLadingPage.Items.FindByValue(ModuleSettings("TrangLadingPage_Id")).Selected = True
                    End If
                    If Not Null.IsNull(ModuleSettings("TrangLadingPage_Title")) Then
                        Dim sAllow As String = ModuleSettings("TrangLadingPage_Title").ToString()
                        chkHienTieude.Checked = Convert.ToBoolean(sAllow)
                    End If
                    If Not Null.IsNull(ModuleSettings("TrangLadingPage_ShowSubPage")) Then
                        Dim sAllows As String = ModuleSettings("TrangLadingPage_ShowSubPage").ToString()
                        chkHienDanhsachSub.Checked = Convert.ToBoolean(sAllows)
                    End If
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings("TrangLadingPage_Template")) Then
                        Me.dropTemplate.SelectedValue = ModuleConfiguration.ModuleSettings("TrangLadingPage_Template").ToString()
                    End If
                    If Not Null.IsNull(ModuleConfiguration.ModuleSettings("TrangLadingPage_Template_Detail")) Then
                        Me.dropTemplate.SelectedValue = ModuleConfiguration.ModuleSettings("TrangLadingPage_Template_Detail").ToString()
                    End If
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Private Sub BindTrang()
            Dim arrNewsCategories = _LadingPage_Controller._GetAll(PortalId)
            Dim arrTemp As ArrayList = New ArrayList()
            If arrNewsCategories.Count > 0 Then
                For Each objNewsCategories As LadingPage_Info In arrNewsCategories
                    If objNewsCategories.ParentId = 0 Then
                        arrTemp.Add(objNewsCategories)
                        For Each objNewsCategoriesTemp As LadingPage_Info In arrNewsCategories
                            If objNewsCategoriesTemp.ParentId = objNewsCategories.id Then
                                objNewsCategoriesTemp.TrangDanhMuc = "|----" & objNewsCategoriesTemp.TrangDanhMuc
                                arrTemp.Add(objNewsCategoriesTemp)
                                'Cap 3
                                For Each objNewsCategoriesTemp2 As LadingPage_Info In arrNewsCategories
                                    If objNewsCategoriesTemp2.ParentId = objNewsCategoriesTemp.id Then
                                        objNewsCategoriesTemp2.TrangDanhMuc = "|----|----" & objNewsCategoriesTemp2.TrangDanhMuc
                                        arrTemp.Add(objNewsCategoriesTemp2)
                                        'Cap 3
                                        For Each objNewsCategoriesTemp3 As LadingPage_Info In arrNewsCategories
                                            If objNewsCategoriesTemp3.ParentId = objNewsCategoriesTemp2.id Then
                                                objNewsCategoriesTemp3.TrangDanhMuc = "|----|----" & objNewsCategoriesTemp3.TrangDanhMuc
                                                arrTemp.Add(objNewsCategoriesTemp3)

                                            End If
                                        Next
                                        '------
                                    End If
                                Next
                                '--
                            End If
                        Next
                    End If
                Next
            End If

            Me.ddlTrangLadingPage.DataSource = arrTemp
            Me.ddlTrangLadingPage.DataTextField = "TrangDanhMuc"
            Me.ddlTrangLadingPage.DataValueField = "id"
            Me.ddlTrangLadingPage.DataBind()
            Me.ddlTrangLadingPage.Items.Insert(0, New ListItem("--Chọn Trang--", "0"))
        End Sub
        Private Sub BindTemplate()
            Dim arrTemplate As New ArrayList
            Dim ctltem As New LadingPageTemplateController
            arrTemplate = ctltem._GetAll(0)
            dropTemplate.DataSource = arrTemplate
            dropTemplate.DataTextField = "TemplateName"
            dropTemplate.DataValueField = "FilePath"
            dropTemplate.DataBind()

            dropTemplateDetail.DataSource = arrTemplate
            dropTemplateDetail.DataTextField = "TemplateName"
            dropTemplateDetail.DataValueField = "FilePath"
            dropTemplateDetail.DataBind()
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
                Dim schkHienTieude As String = chkHienTieude.Checked.ToString()

                Dim schkHienDanhsachSub As String = chkHienDanhsachSub.Checked.ToString()
                Dim objModules As New DotNetNuke.Entities.Modules.ModuleController
                objModules.UpdateModuleSetting(ModuleId, "TrangLadingPage_Id", ddlTrangLadingPage.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "TrangLadingPage_Title", schkHienTieude)
                objModules.UpdateModuleSetting(ModuleId, "TrangLadingPage_ShowSubPage", schkHienDanhsachSub)
                objModules.UpdateModuleSetting(ModuleId, "TrangLadingPage_Template", Me.dropTemplate.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "TrangLadingPage_Template_Detail", Me.dropTemplateDetail.SelectedValue)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region


    End Class

End Namespace
