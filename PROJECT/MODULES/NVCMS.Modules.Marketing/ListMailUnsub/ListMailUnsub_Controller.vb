'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Mail_ListMailUnsubController
        Public Sub _Insert(ByVal objInfo As Marketing_Mail_ListMailUnsubInfo)
            DataProvider.Instance.Marketing_Mail_ListMail_Unsub_Insert(objInfo)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal objInfo As Marketing_Mail_ListMailUnsubInfo)
            DataProvider.Instance.Marketing_Mail_ListMail_Unsub_Update(objInfo)
        End Sub

        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.Marketing_Mail_ListMail_Unsub_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer) As Marketing_Mail_ListMailUnsubInfo
            Return CType(CBO.FillObject(Of Marketing_Mail_ListMailUnsubInfo)(DataProvider.Instance.Marketing_Mail_ListMail_Unsub_GetByID(id), True), Marketing_Mail_ListMailUnsubInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Mail_ListMail_Unsub_GetAll(), GetType(Marketing_Mail_ListMailUnsubInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace