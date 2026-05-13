'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Namespace NVCMS.Modules.TinTuc

    Public Class NV_NewsMeController

        Public Sub Insert(ByVal CategoryId As Integer, ByVal Title As String, ByVal ImagePath As String, ByVal Summary As String, ByVal Content As String, ByVal isActive As Boolean, ByVal Hotcat As Boolean, ByVal Hotsite As Boolean, ByVal Createdate As DateTime, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Status As Integer, ByVal Exsummary As String, ByVal TypeUrl As String)
            DataProvider.Instance.NV_News_Me_add(CategoryId, Title, ImagePath, Summary, Content, isActive, Hotcat, Hotsite, PortalId, UserId, Exsummary, TypeUrl)
        End Sub
        Public Sub Update(ByVal NewId As Integer, ByVal CategoryId As Integer, ByVal Title As String, ByVal ImagePath As String, ByVal Summary As String, ByVal Content As String, ByVal isActive As Boolean, ByVal Hotcat As Boolean, ByVal Hotsite As Boolean, ByVal Createdate As DateTime, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Status As Integer, ByVal Exsummary As String, ByVal TypeUrl As String)
            DataProvider.Instance.NV_News_Me_update(NewId, CategoryId, Title, ImagePath, Summary, Content, isActive, Hotcat, Hotsite, PortalId, Exsummary, TypeUrl)
        End Sub
        Public Sub Delete(ByVal NewId As Integer)
            DataProvider.Instance.NV_News_Me_delete(NewId)
        End Sub
        Public Function GetByID(ByVal NewId As Integer) As NV_NewsInfo
            Return CType(CBO.FillObject(Of NV_NewsInfo)(DataProvider.Instance.NV_News_Me_selectbyid(NewId), True), NV_NewsInfo)
        End Function
        Public Function GetAll(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NV_News_Me_selectall(PortalId), GetType(NV_NewsInfo))
        End Function
        Public Function FindByStatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer) As Integer
            Return DataProvider.Instance.News_Me_Findbystatus_Count(datefrom, dateto, title, categoryid, PortalId, status, UserId)
        End Function
        Public Function FindByStatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.News_Me_Findbystatus_Index(datefrom, dateto, title, categoryid, PortalId, status, UserId, PageIndex, PageSize), GetType(NV_NewsInfo))
        End Function
    End Class
End Namespace