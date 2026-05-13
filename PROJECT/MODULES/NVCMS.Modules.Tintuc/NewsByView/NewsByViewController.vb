'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NewsByView
        Public Sub NewsByView_Insert(ByVal NewId As Integer, ByVal ViewCount As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.NewsByView_Insert(NewId, ViewCount, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub NewsByView_Update(ByVal NewId As Integer)
            DataProvider.Instance.NewsByView_Update(NewId)
        End Sub

        '------------------------------------------'
        'Public Function NewsByView_GetByNewID(ByVal Newid As Integer) As ArrayList
        '    Return CBO.FillCollection(DataProvider.Instance.NewsByView_GetByNewID(Newid), GetType(NewsByViewInfo))
        'End Function
        Public Function NewsByView_GetByNewID(ByVal NewId As Integer) As NewsByViewInfo
            Return CType(CBO.FillObject(Of NewsByViewInfo)(DataProvider.Instance.NewsByView_GetByNewID(NewId), True), NewsByViewInfo)
        End Function

        '------------------------------------------'
    End Class

End Namespace