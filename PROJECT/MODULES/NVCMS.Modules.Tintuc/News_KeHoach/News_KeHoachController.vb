Imports DotNetNuke.Data
Imports DotNetNuke.Common.Utilities

Namespace Vbuzz.Modules.TinTuc

    Public Class News_KeHoachController

        Public Function Insert(ByVal objInfo As News_KeHoachInfo) As Integer
            Return DataProvider.Instance.News_KeHoach_Insert(objInfo)
        End Function

        Public Sub Update(ByVal objInfo As News_KeHoachInfo)
            DataProvider.Instance.News_KeHoach_Update(objInfo)
        End Sub

        Public Sub Delete(ByVal Id As Integer)
            DataProvider.Instance.News_KeHoach_Delete(Id)
        End Sub

        Public Function GetById(ByVal Id As Integer) As News_KeHoachInfo
            Return CType(CBO.FillObject(DataProvider.Instance.News_KeHoach_GetById(Id), GetType(News_KeHoachInfo)), News_KeHoachInfo)
        End Function

        Public Function GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_KeHoach_GetAll(), GetType(News_KeHoachInfo))
        End Function

        Public Function FindKH_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal tieude As String, ByVal UserId As Integer, ByVal phongbanid As Integer) As Integer
            Return DataProvider.Instance.FindKH_Count(datefrom, dateto, tieude, UserId, phongbanid)
        End Function
        Public Function FindKH_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal tieude As String, ByVal UserId As Integer, ByVal phongbanid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.FindKH_Index(datefrom, dateto, tieude, UserId, phongbanid, PageIndex, PageSize), GetType(News_KeHoachInfo))
        End Function


    End Class

End Namespace