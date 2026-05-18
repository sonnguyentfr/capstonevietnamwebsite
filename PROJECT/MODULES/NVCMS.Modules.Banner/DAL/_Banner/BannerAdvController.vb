Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.Banner


    Public Class BannerAdvController

        Public Sub Insert(ByVal Title As String, ByVal KieuBanner As Integer, ByVal IMGLink As String, ByVal Vitri As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Visible As Boolean, ByVal CreatedDate As DateTime, ByVal Ordernumber As Integer, ByVal Link As String, ByVal Startdate As DateTime, ByVal enddate As DateTime, ByVal Contact As String)
            DataProvider.Instance.NVCMS_Banner_Insert(Title, KieuBanner, IMGLink, Vitri, Height, Width, PortalId, UserId, Visible, CreatedDate, Ordernumber, Link, Startdate, enddate, Contact)
        End Sub

        '------------------------------------------'
        Public Sub Update(ByVal id As Integer, ByVal Title As String, ByVal KieuBanner As Integer, ByVal IMGLink As String, ByVal Vitri As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Visible As Boolean, ByVal CreatedDate As DateTime, ByVal Ordernumber As Integer, ByVal Link As String, ByVal Startdate As DateTime, ByVal enddate As DateTime, ByVal Contact As String)
            DataProvider.Instance.NVCMS_Banner_Update(id, Title, KieuBanner, IMGLink, Vitri, Height, Width, PortalId, UserId, Visible, CreatedDate, Ordernumber, Link, Startdate, enddate, Contact)
        End Sub

        '------------------------------------------'
        Public Sub Delete(ByVal id As Integer)
            DataProvider.Instance.NVCMS_Banner_Delete(id)
        End Sub

        '------------------------------------------'
        Public Function GetByID(ByVal id As Integer) As BannerAdvInfo
            Return CType(CBO.FillObject(Of BannerAdvInfo)(DataProvider.Instance.NVCMS_Banner_GetByID(id), True), BannerAdvInfo)
        End Function

        '------------------------------------------'
        Public Function GetAll(ByVal Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NVCMS_Banner_GetAll(Portalid), GetType(BannerAdvInfo))
        End Function
        '------------------------------------------'
        Public Function GetAllVitri(ByVal Portalid As Integer, vitri As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NVCMS_Banner_GetAllVitri(Portalid, vitri), GetType(BannerAdvInfo))
        End Function
        '------------------------------------------'
        Public Sub UpdateOrder(ByVal id As Integer, ByVal OrderNumber As Integer)
            DataProvider.Instance.NVCMS_Banner_UpdateOrder(id, OrderNumber)
        End Sub
        '------------------------------------------'
        Public Sub UpdateClick(ByVal id As Integer)
            DataProvider.Instance.NVCMS_Banner_UpdateClick(id)
        End Sub
        '------------------------------------------'
        Public Sub UpdateView(ByVal id As Integer)
            DataProvider.Instance.NVCMS_Banner_UpdateView(id)
        End Sub
        Public Function GetAllShow(ByVal Portalid As Integer, ByVal vitri As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.NVCMS_Banner_GetAllShow(Portalid, vitri), GetType(BannerAdvInfo))
        End Function

        '------------------------------------------'
    End Class
End Namespace