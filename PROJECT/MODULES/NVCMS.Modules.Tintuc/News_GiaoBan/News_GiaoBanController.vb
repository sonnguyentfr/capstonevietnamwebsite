Imports DotNetNuke.Data
Imports DotNetNuke.Common.Utilities

Namespace Vbuzz.Modules.TinTuc

    Public Class News_GiaoBanController

        Public Function Insert(ByVal objInfo As News_GiaoBanInfo) As Integer
            Return DataProvider.Instance.News_GiaoBan_Insert(objInfo)
        End Function

        Public Sub Update(ByVal objInfo As News_GiaoBanInfo)
            DataProvider.Instance.News_GiaoBan_Update(objInfo)
        End Sub

        Public Sub Delete(ByVal Id As Integer)
            DataProvider.Instance.News_GiaoBan_Delete(Id)
        End Sub

        Public Function GetById(ByVal Id As Integer) As News_GiaoBanInfo
            Return CType(CBO.FillObject(DataProvider.Instance.News_GiaoBan_GetById(Id), GetType(News_GiaoBanInfo)), News_GiaoBanInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_GiaoBan_GetAll(), GetType(News_GiaoBanInfo))
        End Function

        Public Function FindGB_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal tieude As String, ByVal ModuleId As Integer) As Integer
            Return DataProvider.Instance.FindGB_Count(datefrom, dateto, tieude, ModuleId)
        End Function
        Public Function FindGB_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal tieude As String, ByVal ModuleId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindGB_Index(datefrom, dateto, tieude, ModuleId, PageIndex, PageSize), GetType(News_GiaoBanInfo))
        End Function
    End Class

End Namespace