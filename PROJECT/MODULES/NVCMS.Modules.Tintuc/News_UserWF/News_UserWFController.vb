Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc
    Public Class News_UserWFController
        Public Function Insert(ByVal objInfo As News_UserWFInfo) As Integer
            Return DataProvider.Instance.News_UserWF_Insert(objInfo)
        End Function

        Public Sub Update(ByVal objInfo As News_UserWFInfo)
            DataProvider.Instance.News_UserWF_Update(objInfo)
        End Sub

        Public Sub Delete(ByVal ID As Integer)
            DataProvider.Instance.News_UserWF_Delete(ID)
        End Sub

        Public Function GetById(ByVal ID As Integer) As News_UserWFInfo
            Return CType(CBO.FillObject(Of News_UserWFInfo)(DataProvider.Instance.News_UserWF_GetById(ID), True), News_UserWFInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_UserWF_GetAll(), GetType(News_UserWFInfo))
        End Function

        Public Function GetByUserId(ByVal LoaiWF As LoaiWF, ByVal UserId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_UserWF_GetByUserId(LoaiWF, UserId), GetType(News_UserWFInfo))
        End Function

        Public Function GetByPhongBanId(ByVal LoaiWF As LoaiWF, ByVal phongbanID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_UserWF_GetByPhongBanId(LoaiWF, phongbanID), GetType(News_UserWFInfo))
        End Function

        Public Sub DeleteByPhongBanID(ByVal phongbanID As Integer)
            DataProvider.Instance.News_UserWF_DeleteByPhongBanId(phongbanID)
        End Sub
    End Class
End Namespace
