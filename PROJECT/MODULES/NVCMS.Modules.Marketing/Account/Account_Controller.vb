Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Mail_AccountController

        Public Sub _Insert(ByVal Name As String, ByVal Mail As String, ByVal Password As String, ByVal UserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_Account_Insert(Name, Mail, Password, UserId, PortalId)
        End Sub

        '------------------------------------------'
        Public Sub _Update(ByVal id As Integer, ByVal Name As String, ByVal Mail As String, ByVal Password As String, ByVal UserId As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Marketing_Mail_Account_Update(id, Name, Mail, Password, UserId, PortalId)
        End Sub
        '------------------------------------------'
        Public Sub _Delete(ByVal id As Integer)
            DataProvider.Instance.Marketing_Mail_Account_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function _GetByID(ByVal id As Integer) As Marketing_Mail_AccountInfo
            Return CType(CBO.FillObject(Of Marketing_Mail_AccountInfo)(DataProvider.Instance.Marketing_Mail_Account_GetByID(id), True), Marketing_Mail_AccountInfo)
        End Function

        '------------------------------------------'
        Public Function _GetAll() As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Marketing_Mail_Account_GetAll(), GetType(Marketing_Mail_AccountInfo))
        End Function

        '------------------------------------------'
    End Class

End Namespace