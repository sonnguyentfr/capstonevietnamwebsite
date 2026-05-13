Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :SonNguyen 
'Created Date   :23/07/2016
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.FairGuide

    Public Class FairGuide_Controller

        Public Function Insert(ByVal Title As String, ByVal Avatar As String, ByVal Descreption As String, ByVal Noidung As String, ByVal Ordernumber As Integer, ByVal IsActive As Boolean, ByVal Createddate As DateTime, ByVal sizewidth As Integer, ByVal sizeheight As Integer, ByVal UserId As Integer, ByVal Portalid As Integer) As Integer
            Return CType(DataProvider.Instance.Fairguide_Insert(Title, Avatar, Descreption, Noidung, Ordernumber, IsActive, Createddate, sizewidth, sizeheight, UserId, Portalid), Integer)
        End Function
        '------------------------------------------'
        Public Sub Update(ByVal id As Integer, ByVal Title As String, ByVal Avatar As String, ByVal Descreption As String, ByVal Noidung As String, ByVal Ordernumber As Integer, ByVal IsActive As Boolean, ByVal Createddate As DateTime, ByVal sizewidth As Integer, ByVal sizeheight As Integer, ByVal UserId As Integer, ByVal Portalid As Integer)
            DataProvider.Instance.Fairguide_Update(id, Title, Avatar, Descreption, Noidung, Ordernumber, IsActive, Createddate, sizewidth, sizeheight, UserId, Portalid)
        End Sub

        '------------------------------------------'
        Public Sub Delete(ByVal id As Integer, ByVal PortalId As Integer)
            DataProvider.Instance.Fairguide_Delete(id, PortalId)
        End Sub

        '------------------------------------------'
        Public Function GetByID(ByVal id As Integer, ByVal PortalId As Integer) As FairGuide_Info
            Return CType(CBO.FillObject(Of FairGuide_Info)(DataProvider.Instance.Fairguide_GetByID(id, PortalId), True), FairGuide_Info)
        End Function

        '------------------------------------------'
        Public Function GetAll(ByVal PortalId As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Fairguide_GetAll(PortalId), GetType(FairGuide_Info))
        End Function
        '------------------------------------------'
        Public Function Find_Count(subtractIds As String, ByVal Title As String, ByVal PortalId As Integer) As Integer
            Return DataProvider.Instance.Fairguide_Find_Count(subtractIds, Title, PortalId)
        End Function
        '------------------------------------------'
        Public Function Find_Index(subtractIds As String, ByVal Title As String, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Fairguide_Find_Index(subtractIds, Title, PortalId, PageIndex, PageSize), GetType(FairGuide_Info))
        End Function
        '------------------------------------------'
    End Class

End Namespace

