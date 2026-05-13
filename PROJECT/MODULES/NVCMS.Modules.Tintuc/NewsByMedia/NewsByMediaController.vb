'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/27/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NewsByMediaController

        Public Sub _Insert(ByVal newid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            DataProvider.Instance.NewsByMedia_Insert(newid, mediaid, createdted, userid, portalid)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal newid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            DataProvider.Instance.NewsByMedia_Update(id, newid, mediaid, createdted, userid, portalid)
        End Sub
        '------------------------------------------'
        Public Sub _UpdateNewId(ByVal newid As Integer, newidnew As Integer)
            DataProvider.Instance.NewsByMedia_UpdateNewId(newid, newidnew)
        End Sub
        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.NewsByMedia_Delete(id)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteByNewId(ByVal Newid As Integer)
            DataProvider.Instance.NewsByMedia_DeleteByNewId(Newid)
        End Sub
        '------------------------------------------'
        Public Sub _DeleteByMediaId(ByVal Mediaid As Integer)
            DataProvider.Instance.NewsByMedia_DeleteByMediaId(Mediaid)
        End Sub
        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer) As NewsByMediaInfo
            Return CType(CBO.FillObject(Of NewsByMediaInfo)(DataProvider.Instance.NewsByMedia_GetByID(id), True), NewsByMediaInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAllByNewId(ByVal newid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsByMedia_GetAllByNewid(newid), GetType(NewsByMediaInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace
