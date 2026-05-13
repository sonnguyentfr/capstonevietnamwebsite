'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NewsByShareController
        Public Sub _Insert(ByVal NewId As Integer, ByVal LinkShare As String, ByVal CreatedDate As DateTime)
            DataProvider.Instance.NewsByShare_Insert(NewId, LinkShare, CreatedDate)
        End Sub

        ''------------------------------------------'
        'Public Sub hare_Update(ByVal objInfo As hareInfo)
        '    DataProvider.Instance.NewsByShare_Update(id, NewId, LinkShare, CreatedDate, UserId, Count)
        'End Sub

        ''------------------------------------------'
        'Public Sub hare_Delete(ByVal id As Integer)
        '    DataProvider.Instance.NewsByShare_Delete(id)
        'End Sub

        '------------------------------------------'
        Public Function _GetByNewID(ByVal Newid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NewsByShare_GetByNewID(Newid), GetType(NewsByShareInfo))
        End Function
        ''------------------------------------------'
        Public Function _GetCountbyNewId(NewId As Integer) As Integer
            Return DataProvider.Instance.NewsByShare_GetCountByNewId(NewId)
        End Function
        ''------------------------------------------'
        'Public Function hare_GetAll() As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance.hare_GetAll(), GetType(NewsByShareInfo))
        'End Function

        '------------------------------------------'
    End Class

End Namespace