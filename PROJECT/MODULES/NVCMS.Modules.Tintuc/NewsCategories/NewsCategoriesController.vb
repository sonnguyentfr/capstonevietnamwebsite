'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users

Namespace NVCMS.Modules.TinTuc

    Public Class NV_NewsCategoriesController

        Public Sub Insert(ByVal CategoryName As String, ByVal Description As String, ByVal TabID As Integer, TabIdDetail As Integer, ByVal IsActive As Boolean, ByVal CreateDate As DateTime, ByVal PortalId As Integer, ByVal ParentId As Integer, ByVal OrderNumber As Integer)
            DataProvider.Instance.NV_NewsCategories_add(CategoryName, Description, TabID, TabIdDetail, IsActive, PortalId, ParentId, OrderNumber)
        End Sub

        '------------------------------------------'
        Public Sub Update(ByVal CategoryID As Integer, ByVal CategoryName As String, ByVal Description As String, ByVal TabID As Integer, TabIdDetail As Integer, ByVal IsActive As Boolean, ByVal CreateDate As DateTime, ByVal PortalId As Integer, ByVal ParentId As Integer, ByVal OrderNumber As Integer)
            DataProvider.Instance.NV_NewsCategories_update(CategoryID, CategoryName, Description, TabID, TabIdDetail, IsActive, PortalId, ParentId, OrderNumber)
        End Sub

        Public Sub UpdateOrderNumber(ByVal CategoryID As Integer, ByVal OrderNumber As Integer)
            DataProvider.Instance.NV_NewsCategories_updateOrderNumber(CategoryID, OrderNumber)
        End Sub

        '------------------------------------------'
        Public Sub Delete(ByVal CategoryID As Integer)
            DataProvider.Instance.NV_NewsCategories_delete(CategoryID)
        End Sub

        '------------------------------------------'
        Public Function GetByID(ByVal CategoryID As Integer) As NV_NewsCategoriesInfo
            Return CType(CBO.FillObject(Of NV_NewsCategoriesInfo)(DataProvider.Instance.NV_NewsCategories_selectbyid(CategoryID), True), NV_NewsCategoriesInfo)
        End Function

        '------------------------------------------'
        Public Function GetAll(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsCategories_selectall(PortalId), GetType(NV_NewsCategoriesInfo))
        End Function

        Public Function GetByParentId(ByVal Parentid As Integer, ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsCategories_selectByParentId(Parentid, PortalId), GetType(NV_NewsCategoriesInfo))
        End Function

        Public Function GetByParentIdExt(ByVal Parentid As Integer, ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsCategories_selectByParentIdExt(Parentid, PortalId), GetType(NV_NewsCategoriesInfo))
        End Function

        '------------------------------------------'
        Public Function GetAllVisible(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_NewsCategories_selectallVisible(PortalId), GetType(NV_NewsCategoriesInfo))
        End Function
        Public Function GetRandom() As NV_NewsCategoriesInfo
            Return CType(CBO.FillObject(Of NV_NewsCategoriesInfo)(DataProvider.Instance.NV_NewsCategories_selectRandom(), True), NV_NewsCategoriesInfo)
        End Function
        Public Function GetByTabID(ByVal tabID As Integer) As NV_NewsCategoriesInfo
            Return CType(CBO.FillObject(Of NV_NewsCategoriesInfo)(DataProvider.Instance.NV_NewsCategories_selectbyTabID(tabID), True), NV_NewsCategoriesInfo)
        End Function
#Region "Phan quyen"

        Public Function GetAllUsersByRole(ByVal roleId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().Permissions_GetAllUsersByRole(roleId), GetType(UserInfo))
        End Function

        Public Function GetAllUsersByRoles(ByVal arrRoleId As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().Permissions_GetAllUsersByRoles(arrRoleId), GetType(UserInfo))
        End Function

        Public Function AddUserPermissionByCategories(ByVal userId As Integer, ByVal categoryId As Integer, ByVal roleId As Integer) As Integer
            Return CType(DataProvider.Instance().Permissions_AddUserPermissionByCategories(userId, categoryId, roleId), Integer)
        End Function

        Public Sub DeleteUserPermissionByRole(ByVal userId As Integer, ByVal roleId As Integer)
            DataProvider.Instance().Permissions_DeleteUserPermissionByRole(userId, roleId)
        End Sub

        Public Sub DeleteUserPermissionByRoleAndCategory(ByVal categoryId As Integer, ByVal roleId As Integer)
            DataProvider.Instance().Permissions_DeleteUserPermissionByRoleAndCategory(categoryId, roleId)
        End Sub

        Public Function GetAllCategoriesByUserIdAndRoleId(ByVal userId As Integer, ByVal roleId As Integer, ByVal languageId As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().Permissions_GetAllCategoriesByUserIdAndRoleId(userId, roleId, languageId), GetType(NV_NewsCategoriesInfo))
        End Function

        Public Function GetNotAssignedCategoriesByUserIdAndRoleId(ByVal userId As Integer, ByVal roleId As Integer, ByVal languageId As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().Permissions_GetNotAssignedCategoriesByUserIdAndRoleId(userId, roleId, languageId), GetType(NV_NewsCategoriesInfo))
        End Function

        Public Function GetAllAssignedUsersByRoleIdAndCategoryId(ByVal categoryId As Integer, ByVal roleId As Integer, ByVal languageId As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().Permissions_GetAllAssignedUsersByRoleIdAndCategoryId(categoryId, roleId, languageId), GetType(UserInfo))
        End Function


#End Region
    End Class

End Namespace