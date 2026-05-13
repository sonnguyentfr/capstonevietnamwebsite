Imports DotNetNuke.Common.Utilities

'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Namespace NVCMS.Modules.LibCRM

    Public Class Lib_Events_CatController
        Private Sub Clearchace()
            DataCache.ClearCache("Events_Static")
        End Sub
        Public Sub Events_Cat_Insert(ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, Code As String, Source As String, Email As String, ByVal Dateshow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, Tabid As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)
            DataProvider.Instance.Events_Cat_Insert(CatName, CatNameEN, chonnhieu, Marketing, Code, Source, Email, Dateshow, FromDate, EndDate, Avatar, Desception, DesceptionEN, Contentx, ContentxEN, ContentMail, CreatedDate, UserId, PortalId, Isactive, Ordernumber, Tabid, sendmail, sendCode, titleMail)
        End Sub

        '------------------------------------------'
        Public Sub Events_Cat_Update(ByVal id As Integer, ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, Source As String, Email As String, ByVal Dateshow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, Tabid As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)
            DataProvider.Instance.Events_Cat_Update(id, CatName, CatNameEN, Marketing, chonnhieu, Source, Email, Dateshow, FromDate, EndDate, Avatar, Desception, DesceptionEN, Contentx, ContentxEN, ContentMail, CreatedDate, UserId, PortalId, Isactive, Ordernumber, Tabid, sendmail, sendCode, titleMail)
            Clearchace()
        End Sub
        '------------------------------------------'
        Public Sub Events_Cat_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
            DataProvider.Instance.Events_Cat_UpdateOrdernumber(id, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Sub Events_Cat_UpdateFairSchool(ByVal id As Integer, Portalid As Integer, FairSchool As String)
            DataProvider.Instance.Events_Cat_UpdateFairSchool(id, Portalid, FairSchool)
            Clearchace()
        End Sub
        '------------------------------------------'
        Public Sub Events_Cat_UpdateFairOrg(ByVal id As Integer, Portalid As Integer, FairOrg As String)
            DataProvider.Instance.Events_Cat_UpdateFairOrg(id, Portalid, FairOrg)
            Clearchace()
        End Sub
        '------------------------------------------'
        Public Sub Events_Cat_UpdateFairDiengia(ByVal id As Integer, Portalid As Integer, FairDiengia As String)
            DataProvider.Instance.Events_Cat_UpdateFairDiengia(id, Portalid, FairDiengia)
            Clearchace()
        End Sub
        '------------------------------------------'
        Public Sub Events_Cat_UpdateFairTestimonial(ByVal id As Integer, Portalid As Integer, FairTestimonial As String)
            DataProvider.Instance.Events_Cat_UpdateFairTestimonial(id, Portalid, FairTestimonial)
            Clearchace()
        End Sub
        '------------------------------------------'
        Public Sub Events_Cat_UpdateFairDonviTaiTro(ByVal id As Integer, Portalid As Integer, FairDonviTaiTro As String)
            DataProvider.Instance.Events_Cat_UpdateFairDonviTaiTro(id, Portalid, FairDonviTaiTro)
            Clearchace()
        End Sub
        '------------------------------------------'
        Public Sub Events_Cat_Delete(ByVal id As Integer, Portalid As Integer)
            DataProvider.Instance.Events_Cat_Delete(id, Portalid)
        End Sub

        '------------------------------------------'
        Public Function Events_Cat_GetByID(ByVal id As Integer, Portalid As Integer) As Lib_Events_CatInfo
            Return CType(CBO.FillObject(Of Lib_Events_CatInfo)(DataProvider.Instance.Events_Cat_GetByID(id, Portalid), True), Lib_Events_CatInfo)
        End Function
        '------------------------------------------'
        Public Function Events_Cat_GetByTabID(ByVal tabid As Integer) As Lib_Events_CatInfo
            Return CType(CBO.FillObject(Of Lib_Events_CatInfo)(DataProvider.Instance.Events_Cat_GetByTabID(tabid), True), Lib_Events_CatInfo)
        End Function

        '------------------------------------------'
        Public Function Events_Cat_GetAll(Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Cat_GetAll(Portalid), GetType(Lib_Events_CatInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Cat_GetAllShow(Portalid As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance.Events_Cat_GetAllShow(Portalid), GetType(Lib_Events_CatInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Cat_GetAllShowOnline(Portalid As Integer) As ArrayList
            Dim stringcache = "Events_Static" & "Online" & Portalid
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Events_Cat_GetAllShowOnline(Portalid), GetType(Lib_Events_CatInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)

            'Return CBO.FillCollection(DataProvider.Instance.Events_Cat_GetAllShowOnline(Portalid), GetType(Lib_Events_CatInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Cat_GetAllShowPastCount(ByVal CatName As String, Portalid As Integer) As Integer
            Dim stringcache = "Events_Static" & "DaDienRa_Count" & CatName & Portalid
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = DataProvider.Instance.Events_Cat_GetAllShowPastCount(CatName, Portalid)
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)

            'Return CBO.FillCollection(DataProvider.Instance.Events_Cat_GetAllShowPast(CatName, Portalid, PageIndex, PageSize), GetType(Lib_Events_CatInfo))
        End Function
        '------------------------------------------'
        Public Function Events_Cat_GetAllShowPast(ByVal CatName As String, Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As ArrayList
            Dim stringcache = "Events_Static" & "DaDienRa" & CatName & Portalid & PageIndex & PageSize
            If DataCache.GetCache(stringcache) Is Nothing Then
                Dim arrtop = CBO.FillCollection(DataProvider.Instance.Events_Cat_GetAllShowPast(CatName, Portalid, PageIndex, PageSize), GetType(Lib_Events_CatInfo))
                DataCache.SetCache(stringcache, arrtop)
            End If
            Return DataCache.GetCache(stringcache)

            'Return CBO.FillCollection(DataProvider.Instance.Events_Cat_GetAllShowPast(CatName, Portalid, PageIndex, PageSize), GetType(Lib_Events_CatInfo))
        End Function
        '------------------------------------------'
    End Class

End Namespace