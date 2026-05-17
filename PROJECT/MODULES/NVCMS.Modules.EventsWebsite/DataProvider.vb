'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006

Imports DotNetNuke

Namespace NVCMS.Modules.EventsWebsite

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' An abstract class for the data access layer
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.EventsWebsite", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "Events"
#Region "Events_Cat"

        Public MustOverride Function Events_Cat_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetByTabID(ByVal tabid As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAllShow(PortalId As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAllShowPastCount(ByVal CatName As String, Portalid As Integer) As Integer
        Public MustOverride Function Events_Cat_GetAllShowPast(ByVal CatName As String, Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Function Events_Cat_GetAllShowOnline(PortalId As Integer) As IDataReader
        Public MustOverride Function Events_Cat_GetAllShowOnlineViewWebsite(PortalId As Integer) As IDataReader

        Public MustOverride Sub Events_Cat_Insert(ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, code As String, Source As String, Email As String, ByVal DateShow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, Tabid As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)

        Public MustOverride Sub Events_Cat_Delete(ByVal id As Integer, PortalId As Integer)

        Public MustOverride Sub Events_Cat_Update(ByVal id As Integer, ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, Source As String, Email As String, ByVal DateShow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, TabId As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)

        Public MustOverride Sub Events_Cat_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)

        Public MustOverride Sub Events_Cat_UpdateFairSchool(ByVal id As Integer, Portalid As Integer, FairSchool As String)

        Public MustOverride Sub Events_Cat_UpdateFairOrg(ByVal id As Integer, Portalid As Integer, FairOrg As String)

        Public MustOverride Sub Events_Cat_UpdateFairDiengia(ByVal id As Integer, Portalid As Integer, FairDiengia As String)

        Public MustOverride Sub Events_Cat_UpdateFairTestimonial(ByVal id As Integer, Portalid As Integer, FairTestimonial As String)

        Public MustOverride Sub Events_Cat_UpdateFairDonviTaiTro(ByVal id As Integer, Portalid As Integer, FairDonviTaiTro As String)


#End Region
#Region "Events"
        Public MustOverride Function Events_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Function Events_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function Events_GetAllByCat(CatId As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Function Events_GetAllShowByCat(CatId As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Sub Events_Insert(ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)

        Public MustOverride Sub Events_Update(ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)

        Public MustOverride Sub Events_Delete(ByVal id As Integer, PortalId As Integer)
        Public MustOverride Function Events_Find_Count(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer) As Integer
        Public MustOverride Function Events_Find_Index(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Events_FindShow_Count(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer) As Integer
        Public MustOverride Function Events_FindShow_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Events_FindShowPast_Count(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer) As Integer
        Public MustOverride Function Events_FindShowPast_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Events_GetAllShow(PortalId As Integer, Count As Integer) As IDataReader

        Public MustOverride Function Events_GetAllOnline(PortalId As Integer, Count As Integer) As IDataReader

        Public MustOverride Function Events_GetAllShowEnd(PortalId As Integer, Count As Integer) As IDataReader
#End Region
#Region "NVCMS_Events_Template"
        Public MustOverride Function NVCMS_Events_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
        Public MustOverride Function NVCMS_Events_Template_SelectAll(ByVal PortalId As Integer) As IDataReader
        Public MustOverride Sub NVCMS_Events_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_Events_Template_Delete(ByVal Id As Integer, Portalid As Integer)

        Public MustOverride Sub NVCMS_Events_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

#End Region
#Region "NV_Events_Student"

        Public MustOverride Function Events_Student_GetAllByEvent(EventId As Integer) As IDataReader

        Public MustOverride Function Events_Student_GetAllByStudent(StudentId As Integer) As IDataReader

        Public MustOverride Function Events_Student_GetAllByEventCheckIn(EventId As Integer, Source As String, checkin As Integer) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCheckInbySource(EventId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader

        Public MustOverride Sub Events_Student_Insert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, ByVal Source As Integer, ByVal Nguon As String, ByVal CreatedDate As DateTime, ByVal PortalId As Integer, NguonTutao As String)

        Public MustOverride Sub Events_Student_UpdateCheckIn(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)

        Public MustOverride Sub Events_Student_UpdateThamdu(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Thamdu As Boolean, ByVal ThamduDateUpdate As DateTime, ByVal ThamduUserUpdate As Integer)

        Public MustOverride Sub Events_Student_UpdateCheckInAfterFair(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)

        Public MustOverride Sub Events_Student_UpdateStudentNguon(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal Nguon As String)
        Public MustOverride Sub Events_Student_UpdateStudentNguonTutao(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal NguonTutao As String)

        Public MustOverride Sub Events_Student_UpdateCheckInInsert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, Source As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CreatedDate As DateTime, Portalid As Integer, ByVal CheckInDate As DateTime, ByVal UserId As Integer)

        Public MustOverride Function Events_Student_GetById(id As Integer) As IDataReader
        Public MustOverride Function Events_Student_GetAllByEventCat(EventCatId As Integer) As IDataReader

        Public MustOverride Function Events_Student_GetCountByEventCat(datetime As DateTime, EventCatId As Integer) As Integer

        Public MustOverride Function Events_Student_GetCountByEvent(datetime As DateTime, EventId As Integer) As Integer

        Public MustOverride Function Events_Student_SelectAllByEventCatCheckInbySource(EventCatId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCatbySource(EventId As Integer, Source As Integer) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCatbyNguon(EventCatId As Integer, CheckIn As Integer, Nguon As String) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventCatbyNguonTutao(EventCatId As Integer, CheckIn As Integer, NguonTutao As String) As IDataReader

        Public MustOverride Function Events_Student_SelectAllByEventbyNguon(EventId As Integer, CheckIn As Integer, Nguon As String) As IDataReader

        Public MustOverride Function Events_Student_FindCountByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer) As Integer

        Public MustOverride Function Events_Student_FindCountThamDuByEvent(EventId As Integer, EventCatId As Integer, Thamdu As Integer) As Integer

        Public MustOverride Function Events_Student_FindIndexByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader

        Public MustOverride Sub Events_Student_DeleteStudentEventId(EventId As Integer, StudentId As Integer)

        Public MustOverride Function Events_Student_SelectByEventstudentid(EventId As Integer, StudentId As Integer)
#End Region
#End Region

#End Region


    End Class

End Namespace