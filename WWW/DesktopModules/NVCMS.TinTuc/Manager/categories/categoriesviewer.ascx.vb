Imports System
Imports DotNetNuke
Imports DotNetNuke.UI.Utilities
Imports NVCMS.Modules.Hethong
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Manager.categories

    Public MustInherit Class categoriesviewer
        Inherits Entities.Modules.PortalModuleBase

#Region "Controls"
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            '
            If Not IsPostBack Then
                Try
                    BindGridData()
                Catch ex As Exception
                    ProcessModuleLoadException(Me, ex)
                End Try
            End If
        End Sub
#End Region

        Private Sub BindGridData()
            Dim ctlNewsCategory As New NV_NewsCategoriesController
            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = ctlNewsCategory.GetAll(PortalId)
            Dim arrTemp As New ArrayList
            Dim objNewsCategories As NV_NewsCategoriesInfo
            Dim objNewsCategoriesTemp As NV_NewsCategoriesInfo
            Dim objNewsCategoriesTemp3 As NV_NewsCategoriesInfo

            If arrNewsCategories.Count > 0 Then
                For Each objNewsCategories In arrNewsCategories
                    If objNewsCategories.ParentId = 0 Then
                        arrTemp.Add(objNewsCategories)
                        For Each objNewsCategoriesTemp In arrNewsCategories
                            If objNewsCategoriesTemp.ParentId = objNewsCategories.CategoryID Then
                                objNewsCategoriesTemp.CategoryName = "|----" & objNewsCategoriesTemp.CategoryName
                                arrTemp.Add(objNewsCategoriesTemp)

                                For Each objNewsCategoriesTemp3 In arrNewsCategories
                                    If objNewsCategoriesTemp3.ParentId = objNewsCategoriesTemp.CategoryID Then
                                        objNewsCategoriesTemp3.CategoryName = "|--------" & objNewsCategoriesTemp3.CategoryName
                                        arrTemp.Add(objNewsCategoriesTemp3)
                                    End If
                                Next

                            End If
                        Next
                    End If
                Next
            End If

            Me.drgViewData.DataSource = arrTemp
            Me.drgViewData.DataBind()
        End Sub

        Private Sub lbtAddBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAddBottom.Click, lbtAddTop.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#Region "action"
        Protected Sub lbtUpdateOrder_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim ctlNewsCategories As New NV_NewsCategoriesController
            For Each repeaterItem As RepeaterItem In drgViewData.Items
                If repeaterItem.ItemType = ListItemType.Item Or repeaterItem.ItemType = ListItemType.AlternatingItem Then
                    Dim txtOrderNumber As String = (TryCast(repeaterItem.FindControl("txtOrderNumber"), TextBox)).Text.Trim()
                    Dim catid As Integer = (TryCast(repeaterItem.FindControl("categoryID"), Label)).Text.Trim()
                    ctlNewsCategories.UpdateOrderNumber(catid, txtOrderNumber)
                End If
            Next
            ClientAPI.RegisterStartUpScript(Me.Page, "UpdateSuccess", "<script>UpdateSuccess('Cập nhật thứ tự thành công');</script>")
            BindGridData()
        End Sub
#End Region
    End Class

End Namespace
