Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc
    Public Class NewsAttachController
        Public Sub News_AttachByPhongBan_Add(ByVal newsAttach As News_AttachByPhongBanInfo)
            DataProvider.Instance().News_AttachByPhongBan_Add(newsAttach)
        End Sub
        Public Sub News_AttachByPhongBan_DeleteByAttachId(ByVal AttachId As Integer)
            DataProvider.Instance().News_AttachByPhongBan_DeleteByAttachId(AttachId)
        End Sub
        Public Sub News_AttachByPhongBan_DeleteByPhongBanId(ByVal PhongBanId As Integer)
            DataProvider.Instance().News_AttachByPhongBan_DeleteByPhongBanId(PhongBanId)
        End Sub
        Public Function News_AttachByPhong_GetByAttachID(ByVal AttachId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().News_AttachByPhongBan_GetByAttachId(AttachId), GetType(News_AttachByPhongBanInfo))
        End Function

        Public Function News_Attach_Add(ByVal newsAttach As NewsAttachInfo) As Integer
            Return CType(DataProvider.Instance().News_Attach_Add(newsAttach), Integer)
        End Function
        Public Sub News_Attach_Update(ByVal newsAttach As NewsAttachInfo)
            DataProvider.Instance().News_Attach_Update(newsAttach)
        End Sub
        Public Sub News_Attach_UpdateFileName(ByVal AttachId As Integer, FileName As String)
            DataProvider.Instance().News_Attach_UpdateFileName(AttachId, FileName)
        End Sub
        Public Sub News_Attach_Delete(AttachId As Integer)
            DataProvider.Instance().News_Attach_Delete(AttachId)
        End Sub
        Public Sub News_Attach_SwapSort(ByVal FirstId As Integer, ByVal SecondId As Integer)
            DataProvider.Instance().News_Attach_SwapSort(FirstId, SecondId)
        End Sub
        Public Function News_Attach_Get(ByVal AttachId As Integer) As NewsAttachInfo
            Return CBO.FillObject(Of NewsAttachInfo)(DataProvider.Instance().News_Attach_Get(AttachId), True)
        End Function
        Public Function News_Attach_GetByNewId(ByVal PortalId As Integer, ByVal NewsId As Integer, ByVal UserId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().News_Attach_GetByNewId(PortalId, NewsId, UserId), GetType(NewsAttachInfo))
        End Function
        Public Function News_Attach_GetByNewId(ByVal PortalId As Integer, ByVal NewsId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().News_Attach_GetByNewId(PortalId, NewsId, -1), GetType(NewsAttachInfo))
        End Function
        Public Function News_Attach_GetMaxId() As Integer
            Dim Item As NewsAttachInfo
            Item = CBO.FillObject(Of NewsAttachInfo)(DataProvider.Instance().News_Attach_GetMaxId(), True)
            If IsNothing(Item) Then
                Return 1
            Else
                Return Item.AttachFileID + 1
            End If
        End Function

        Public Sub NewsByAttach_Add(ByVal newsByAttach As NewsByAttachInfo)
            DataProvider.Instance().NewsByAttach_Add(newsByAttach)
        End Sub
        Public Sub NewsByAttach_DeleteByAttachId(ByVal AttachId As Integer)
            DataProvider.Instance().NewsByAttach_DeleteByAttachId(AttachId)
        End Sub
        Public Sub NewsByAttach_DeleteByNewsId(ByVal NewsId As Integer)
            DataProvider.Instance().NewsByAttach_DeleteByNewsId(NewsId)
        End Sub
    End Class
End Namespace
