'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Data

Namespace NVCMS.Modules.Video

    Public Class Videos_Controller
        Public Function Insert(ByVal objInfo As Videos_Info) As Integer
            Return CType(DataProvider.Instance.Videos_Insert(objInfo), Integer)
        End Function
        Public Function GetByID(ByVal id As Integer, ByVal PortalId As Integer) As Videos_Info
            Return CType(CBO.FillObject(Of Videos_Info)(DataProvider.Instance.Videos_GetByID(id, PortalId), True), Videos_Info)
        End Function
        Public Function Find_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer) As Integer
            Return DataProvider.Instance.Admin_Find_Count(datefrom, dateto, title, categoryid, PortalId, status, UserId)
        End Function
        Public Function Find_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal sapxep As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Admin_Find_Index(datefrom, dateto, title, categoryid, PortalId, status, UserId, PageIndex, PageSize, sapxep), GetType(Videos_Info))
        End Function
        Public Sub Update(ByVal objInfo As Videos_Info)
            DataProvider.Instance.Videos_Update(objInfo)
        End Sub
        ''------------------------------------------'
        Public Sub UpdateStatus(ByVal VideoId As Integer, Status As Integer, Userid As Integer)
            DataProvider.Instance.Videos_UpdateStatus(VideoId, Status, Userid)
        End Sub
        ''------------------------------------------'
        Public Sub UpdatePublishedDate(ByVal id As Integer, PublicDate As DateTime, Userid As Integer)
            DataProvider.Instance.Videos_UpdatePublishedDate(id, PublicDate, Userid)
        End Sub
        Public Function Find_Show_Count(ByVal PortalId As Integer) As Integer
            Return DataProvider.Instance.Videos_Find_Show_Count(PortalId)
        End Function
        Public Function Find_Show_Index(ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Videos_Find_Show_Index(PortalId, PageIndex, PageSize), GetType(Videos_Info))
        End Function
        ''------------------------------------------'

    End Class

End Namespace