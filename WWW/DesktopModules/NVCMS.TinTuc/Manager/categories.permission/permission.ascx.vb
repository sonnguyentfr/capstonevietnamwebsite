Imports System
Imports DotNetNuke
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Modules.Users

Namespace DesktopModules.TinTuc.Manager.permission
    Partial Class permission
        Inherits Entities.Modules.PortalModuleBase

#Region "Private Members"

        Private objCtl As New NV_NewsCategoriesController
        Private RoleGroupId As Integer = 0

#End Region

#Region "Private Methods"

        ''' <summary>
        ''' Bind quyền theo nhóm quyền
        ''' </summary>
        ''' <remarks></remarks>
        'Private Sub BindData()
        '    ' Get the portal's roles from the database
        '    Dim objRoles As New RoleController
        '    Dim arrRoles As ArrayList
        '    If RoleGroupId < -1 Then
        '        arrRoles = objRoles.GetPortalRoles(PortalId)
        '    Else
        '        arrRoles = objRoles.GetRolesByGroup(PortalId, RoleGroupId)
        '    End If

        '    If Not arrRoles Is Nothing AndAlso arrRoles.Count > 0 Then
        '        drlRoles.DataSource = arrRoles
        '        drlRoles.DataBind()
        '    End If
        'End Sub
        ''' <summary>
        ''' Đoạn này sửa lại để chọn nhóm quyền cho từng portal
        ''' SonNT
        ''' </summary>
        ''' <param name="RoleGroupId"></param>
        ''' <remarks></remarks>
        Public Sub BindRole(ByVal RoleGroupId As Integer)
            Dim ctl As RoleController = New RoleController
            Dim listOfRole As List(Of RoleInfo) = ctl.GetRoles(PortalId).Where(Function(p) p.RoleGroupID = RoleGroupId).ToList()
            drlRoles.DataSource = listOfRole
            drlRoles.DataTextField = "Description"
            drlRoles.DataValueField = "RoleId"
            drlRoles.DataBind()
        End Sub
        ''' <summary>
        ''' Bind dữ liệu vào combox User theo RoleId của combox Roles
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BindUsersToBombo()
            Dim objRoleController As New RoleController
            Dim iRoleId As Integer = -1
            Try
                iRoleId = Integer.Parse(drlRoles.SelectedValue)
            Catch ex As Exception
            End Try

            Dim arr As ArrayList = objCtl.GetAllUsersByRole(iRoleId)
            radUser.Items.Clear()
            If Not arr Is Nothing AndAlso arr.Count > 0 Then
                radUser.DataSource = arr
                radUser.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Bind dữ liệu vào Duallist dựa theo các quyền đã được phân cho User
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BindDataToDualList()
            Try
                lstAvailable.Items.Clear()
                lstAssigned.Items.Clear()

                Dim iRootCate As Integer = Null.NullInteger

                'Lấy toàn bộ chuyên mục cần phân quyền
                Dim arrNewsCategories As New ArrayList
                arrNewsCategories = objCtl.GetAll(PortalId)
                Dim arrAvailable As New ArrayList
                Dim objNewsCategories As NV_NewsCategoriesInfo
                Dim objNewsCategoriesTemp As NV_NewsCategoriesInfo
                Dim objNewsCategoriesTemp3 As NV_NewsCategoriesInfo
                Dim objNewsCategoriesTemp4 As NV_NewsCategoriesInfo
                If arrNewsCategories.Count > 0 Then
                    For Each objNewsCategories In arrNewsCategories
                        If (objNewsCategories.ParentId = 0) And (objNewsCategories.IsActive = True) Then
                            arrAvailable.Add(objNewsCategories)
                            For Each objNewsCategoriesTemp In arrNewsCategories
                                If objNewsCategoriesTemp.IsActive = True Then
                                    If objNewsCategoriesTemp.ParentId = objNewsCategories.CategoryID Then
                                        If objNewsCategories.IsActive = True Then
                                            objNewsCategoriesTemp.CategoryName = "|---- " & objNewsCategoriesTemp.CategoryName
                                            arrAvailable.Add(objNewsCategoriesTemp)
                                        End If
                                        For Each objNewsCategoriesTemp3 In arrNewsCategories
                                            If objNewsCategoriesTemp3.IsActive = True Then
                                                If objNewsCategoriesTemp3.ParentId = objNewsCategoriesTemp.CategoryID Then
                                                    If objNewsCategoriesTemp3.IsActive = True Then
                                                        objNewsCategoriesTemp3.CategoryName = "|----|---- " & objNewsCategoriesTemp3.CategoryName
                                                        arrAvailable.Add(objNewsCategoriesTemp3)
                                                    End If
                                                    For Each objNewsCategoriesTemp4 In arrNewsCategories
                                                        If objNewsCategoriesTemp4.IsActive = True Then
                                                            If objNewsCategoriesTemp4.ParentId = objNewsCategoriesTemp3.CategoryID Then
                                                                If objNewsCategoriesTemp4.IsActive = True Then
                                                                    objNewsCategoriesTemp4.CategoryName = "|----|----|---- " & objNewsCategoriesTemp4.CategoryName
                                                                    arrAvailable.Add(objNewsCategoriesTemp4)
                                                                End If

                                                            End If
                                                        End If

                                                    Next
                                                End If
                                            End If

                                        Next

                                    End If
                                End If

                            Next
                        End If
                    Next
                End If

                If radUser.SelectedIndex <> -1 AndAlso drlRoles.SelectedIndex <> -1 Then

                    Dim iUserId As Integer = Null.NullInteger
                    iUserId = Integer.Parse(radUser.SelectedValue)


                    'Lấy các chuyên mục đã phân cho người dùng đang chọn
                    Dim arrAssigned As ArrayList = New ArrayList
                    Dim arrTemp As ArrayList = objCtl.GetAllCategoriesByUserIdAndRoleId(iUserId, Integer.Parse(drlRoles.SelectedValue), "")

                    'Đưa các chuyên mục vào phần có và đã gán cho hợp lý
                    'Remove thang da duoc gan va gan nhung thang nay cho arrAssigned
                    If Not arrAvailable Is Nothing AndAlso arrAvailable.Count > 0 Then
                        For Each objItem As NV_NewsCategoriesInfo In arrAvailable
                            For Each objTemp As NV_NewsCategoriesInfo In arrTemp
                                If objItem.CategoryID = objTemp.CategoryID Then
                                    arrAssigned.Add(objItem)
                                End If
                            Next
                        Next
                    End If
                    For Each objItem As NV_NewsCategoriesInfo In arrAssigned
                        arrAvailable.Remove(objItem)
                    Next

                    lstAssigned.DataSource = arrAssigned
                    lstAssigned.DataBind()
                End If

                lstAvailable.DataSource = arrAvailable
                lstAvailable.DataBind()

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub UpdatePermisionOnCategory()
            Dim iUserId As Integer = -1
            If radUser.Items.Count > 0 Then
                iUserId = Integer.Parse(radUser.SelectedValue)
            Else
                Return
            End If
            Dim iCategoryId As Integer
            Dim iRoleId As Integer = Integer.Parse(drlRoles.SelectedValue)
            'Xoa tat ca cac quyen gan cho user truoc tien
            objCtl.DeleteUserPermissionByRole(iUserId, iRoleId)
            'Cập nhật toàn bộ quyền mới được gán
            For Each objItem As ListItem In lstAssigned.Items
                iCategoryId = Integer.Parse(objItem.Value)
                objCtl.AddUserPermissionByCategories(iUserId, iCategoryId, iRoleId)
            Next
            ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Phân quyền chuyên mục thành công!');</script>")
        End Sub

#End Region

#Region "Events Handlers"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    'Load Css cho module
                    'ModuleUltility.SetSCRIPT(Me, "href", AINewsHelper.CSS(PortalId), Server.MapPath(AINewsHelper.CSS(PortalId)), "NewsModuleCSS", "CSS", "LINK", "rel=stylesheet", "type=text/css")
                    'BindData()
                    Dim slanguage As String = TNFormerStudentHepler.GetCurrentLanguage(Response)
                    Dim roleGroupId As Integer = Null.NullInteger
                    If CType(Settings(TNFormerStudentContants.RoleGroupIdSetting + slanguage + PortalId.ToString), String) <> "" Then
                        roleGroupId = Convert.ToInt32(Settings(TNFormerStudentContants.RoleGroupIdSetting + slanguage + PortalId.ToString))
                    End If
                    BindRole(roleGroupId)
                    BindUsersToBombo()
                    BindDataToDualList()
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub drlRoles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drlRoles.SelectedIndexChanged
            Try
                Dim iRoleId As Integer = Integer.Parse(drlRoles.SelectedValue)
                BindUsersToBombo()
                BindDataToDualList()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub radUser_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles radUser.SelectedIndexChanged
            'Remove all items in lstAvailable and lstAssigned
            lstAvailable.Items.Clear()
            lstAssigned.Items.Clear()
            BindDataToDualList()
        End Sub

        Protected Sub lbtAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtAdd.Click
            Try
                Dim objListItem As ListItem
                Dim objList As ArrayList = New ArrayList
                For Each objListItem In lstAvailable.Items
                    objList.Add(objListItem)
                Next
                For Each objListItem In objList
                    If objListItem.Selected Then
                        lstAvailable.Items.Remove(objListItem)
                        lstAssigned.Items.Add(objListItem)
                    End If
                Next
                lstAvailable.ClearSelection()
                lstAssigned.ClearSelection()

                Sort(lstAssigned)

                UpdatePermisionOnCategory()
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Phân quyền chuyên mục thành công!');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub lbtRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtRemove.Click
            Try
                Dim objListItem As ListItem
                Dim objList As ArrayList = New ArrayList
                For Each objListItem In lstAssigned.Items
                    objList.Add(objListItem)
                Next
                For Each objListItem In objList
                    If objListItem.Selected Then
                        lstAssigned.Items.Remove(objListItem)
                        lstAvailable.Items.Add(objListItem)
                    End If
                Next
                lstAvailable.ClearSelection()
                lstAssigned.ClearSelection()
                Sort(lstAvailable)

                UpdatePermisionOnCategory()
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Gỡ bỏ chuyên mục thành công!');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub lbtAddAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtAddAll.Click
            Try
                Dim objListItem As ListItem
                For Each objListItem In lstAvailable.Items
                    lstAssigned.Items.Add(objListItem)
                Next
                lstAvailable.Items.Clear()
                lstAvailable.ClearSelection()
                lstAssigned.ClearSelection()
                Sort(lstAssigned)

                UpdatePermisionOnCategory()
                ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Gỡ bỏ chuyên mục thành công!');</script>")
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub lbtRemoveAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtRemoveAll.Click
            Try
                Dim objListItem As ListItem
                For Each objListItem In lstAssigned.Items
                    lstAvailable.Items.Add(objListItem)
                Next
                lstAssigned.Items.Clear()
                lstAvailable.ClearSelection()
                lstAssigned.ClearSelection()
                Sort(lstAvailable)

                UpdatePermisionOnCategory()
                System.Web.UI.ScriptManager.RegisterClientScriptBlock(Page, GetType(Page), "script", "NotifyError('Gỡ thành công!','Gỡ phân quyền thành công!');", True)
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

#End Region

#Region "Helper"

        ''' <summary>
        ''' This method is used to sort all items in a lists
        ''' </summary>
        ''' <param name="ctlListBox"></param>
        ''' <remarks></remarks>
        Private Sub Sort(ByVal ctlListBox As ListBox)

            Dim arrListItems As New ArrayList
            Dim objListItem As ListItem

            ' store listitems in temp arraylist
            For Each objListItem In ctlListBox.Items
                arrListItems.Add(objListItem)
            Next

            ' sort arraylist based on text value
            arrListItems.Sort(New ListItemComparer)

            ' clear control
            ctlListBox.Items.Clear()

            ' add listitems to control
            For Each objListItem In arrListItems
                ctlListBox.Items.Add(objListItem)
            Next

        End Sub

#End Region

    End Class

    Public Class ListItemComparer
        Implements IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim a As ListItem = CType(x, ListItem)
            Dim b As ListItem = CType(y, ListItem)
            Dim c As New CaseInsensitiveComparer
            Return c.Compare(a.Text, b.Text)
        End Function
    End Class
End Namespace

