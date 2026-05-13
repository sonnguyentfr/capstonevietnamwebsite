'******************************************
'Author         :DuongNQ
'Created Date   :3/21/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
'Imports CAP.Modules.School
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Data
Imports DotNetNuke.Entities
Namespace NVCMS.Modules.Student

    Public Class StudentFromLadipageController

        '------------------------------------------'
        Public Function _Info_GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.StudentFromLadipage_Info_GetAll(), GetType(StudentFromLadipageInfo))
        End Function
        '------------------------------------------'
        Public Function _Info_GetByEventCatId(ByVal event_id As Integer, ByVal is_update_crm As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.StudentFromLadipage_Info_GetByEventCatId(event_id, is_update_crm), GetType(StudentFromLadipageInfo))
        End Function

        Public Sub _Info_Update_Crm(ByVal id As Integer)
            DataProvider.Instance.StudentFromLadipage_Info_Update_Crm(id)
        End Sub


    End Class

End Namespace