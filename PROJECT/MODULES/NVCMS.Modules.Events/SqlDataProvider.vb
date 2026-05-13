Imports System.Text
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.EventsWebsite

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' SQL Server implementation of the abstract DataProvider class
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class SqlDataProvider

        Inherits DataProvider

#Region "Private Members"

        Private Const ProviderType As String = "data"
        Private Const ModuleQualifier As String = "NVPortal_"

        Private _providerConfiguration As Framework.Providers.ProviderConfiguration = Framework.Providers.ProviderConfiguration.GetProviderConfiguration(ProviderType)
        Private _connectionString As String
        Private _providerPath As String
        Private _objectQualifier As String
        Private _databaseOwner As String

#End Region

#Region "Constructors"

        Public Sub New()

            ' Read the configuration specific information for this provider
            Dim objProvider As Framework.Providers.Provider = CType(_providerConfiguration.Providers(_providerConfiguration.DefaultProvider), Framework.Providers.Provider)

            ' Read the attributes for this provider
            'Get Connection string from web.config
            _connectionString = Config.GetConnectionString("SiteSqlServerV1")

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("SiteSqlServerV1")
            End If

            _providerPath = objProvider.Attributes("providerPath")

            _objectQualifier = objProvider.Attributes("objectQualifier")
            If _objectQualifier <> "" And _objectQualifier.EndsWith("_") = False Then
                _objectQualifier += "_"
            End If

            _databaseOwner = objProvider.Attributes("databaseOwner")
            If _databaseOwner <> "" And _databaseOwner.EndsWith(".") = False Then
                _databaseOwner += "."
            End If

        End Sub

#End Region

#Region "Properties"

        Public ReadOnly Property ConnectionString() As String
            Get
                Return _connectionString
            End Get
        End Property

        Public ReadOnly Property ProviderPath() As String
            Get
                Return _providerPath
            End Get
        End Property

        Public ReadOnly Property ObjectQualifier() As String
            Get
                Return _objectQualifier
            End Get
        End Property

        Public ReadOnly Property DatabaseOwner() As String
            Get
                Return _databaseOwner
            End Get
        End Property

#End Region

#Region "Private Methods"

        Private Function GetFullyQualifiedName(ByVal name As String) As String
            Return DatabaseOwner & ObjectQualifier & ModuleQualifier & name
        End Function

        Private Function GetNull(ByVal Field As Object) As Object
            Return DotNetNuke.Common.Utilities.Null.GetNull(Field, DBNull.Value)
        End Function

#End Region
#Region "Common Function"
        Public Function GetSqlTypeString(ByVal keyword As String) As SqlTypes.SqlString
            Dim _keywords As String = String.Empty
            If keyword <> Null.NullString AndAlso keyword <> String.Empty Then
                _keywords = New SqlTypes.SqlString(FullTextSearchFormat(keyword))
            End If
            Return _keywords
        End Function
        Public Function FullTextSearchFormat(ByVal keywords As String) As String
            If keywords Is Nothing OrElse keywords = String.Empty Then
                Return String.Empty
            End If

            Dim sbKeyWordsFilter As New StringBuilder()
            Dim splitedKeyWords As String() = keywords.Trim().Split(" "c)
            For i As Integer = 0 To splitedKeyWords.Length - 1
                'The first key words
                sbKeyWordsFilter.Append("""")
                sbKeyWordsFilter.Append(splitedKeyWords(i))
                sbKeyWordsFilter.Append("*"" & ")
            Next

            'The last key word
            sbKeyWordsFilter.Append("""")
            sbKeyWordsFilter.Append(splitedKeyWords(splitedKeyWords.Length - 1))
            sbKeyWordsFilter.Append("*""")

            Return sbKeyWordsFilter.ToString()
        End Function
        Public Function WrapWordFullText(word As String) As String
            If String.IsNullOrEmpty(word) Then
                Return String.Empty
            Else
                Return """" & word & "*"""
            End If
        End Function
#End Region
#Region "Public Methods"
#Region "Events_Cat"
        Public Overrides Function Events_Cat_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectByID", id, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetByTabID(ByVal tabid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_GetByTabID", tabid)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShow(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAllShow", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShowPastCount(ByVal CatName As String, Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Cat_SelectAllPast_Count", CatName, GetSqlTypeString(CatName), Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShowPast(ByVal CatName As String, Portalid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAllPast", CatName, GetSqlTypeString(CatName), Portalid, PageIndex, PageSize)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShowOnline(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAllOnline", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Cat_GetAllShowOnlineViewWebsite(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Cat_SelectAllOnlineViewWebsite", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Cat_Insert(ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, code As String, Source As String, Email As String, ByVal DateShow As String, FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, TabId As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_Insert", CatName, CatNameEN, Marketing, chonnhieu, code, Source, Email, DateShow, FromDate, EndDate, Avatar, Desception, DesceptionEN, Contentx, ContentxEN, ContentMail, CreatedDate, UserId, PortalId, Isactive, Ordernumber, TabId, sendmail, sendCode, titleMail)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Events_Cat_Update(ByVal id As Integer, ByVal CatName As String, CatNameEN As String, Marketing As Integer, chonnhieu As Boolean, Source As String, Email As String, ByVal DateShow As String, ByVal FromDate As DateTime, EndDate As DateTime, ByVal Avatar As String, ByVal Desception As String, ByVal DesceptionEN As String, ByVal Contentx As String, ByVal ContentxEN As String, ContentMail As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Isactive As Boolean, Ordernumber As Integer, TabId As Integer, sendmail As Boolean, sendCode As Boolean, titleMail As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_Update", id, CatName, CatNameEN, Marketing, chonnhieu, Source, Email, DateShow, FromDate, EndDate, Avatar, Desception, DesceptionEN, Contentx, ContentxEN, ContentMail, CreatedDate, UserId, PortalId, Isactive, Ordernumber, TabId, sendmail, sendCode, titleMail)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateOrdernumber(ByVal id As Integer, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateOrdernumber", id, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairSchool(ByVal id As Integer, Portalid As Integer, FairSchool As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairSchool", id, Portalid, FairSchool)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairOrg(ByVal id As Integer, Portalid As Integer, FairOrg As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairOrg", id, Portalid, FairOrg)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairDiengia(ByVal id As Integer, Portalid As Integer, FairDiengia As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairDiengia", id, Portalid, FairDiengia)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairTestimonial(ByVal id As Integer, Portalid As Integer, FairTestimonial As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairTestimonial", id, Portalid, FairTestimonial)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_UpdateFairDonviTaiTro(ByVal id As Integer, Portalid As Integer, FairDonviTaiTro As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_UpdateFairDonviTaiTro", id, Portalid, FairDonviTaiTro)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Cat_Delete(ByVal id As Integer, PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Cat_Delete", id, PortalId)
        End Sub

        '------------------------------------------'


#End Region
#Region "NV_Events"

        Public Overrides Function Events_GetByID(ByVal id As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectByID", id, PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function Events_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllByCat(CatId As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllByCat", CatId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllShowByCat(CatId As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllShowByCat", CatId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Insert(ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, Org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Insert", Title, TitleEN, CODE, Source, Vanphong, CatId, Avatar, diadiem, diadiemEN, fromdatetime, enddatetime, thanhphan, thanhphanEN, School, Org, Gia, Descreption, DescreptionEN, LienheName, LienheEmail, LienheMobile, LienheAdd, UserId, Portalid, Createddate, Isactive, anhbando, linkbando, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Update(ByVal id As Integer, ByVal Title As String, ByVal TitleEN As String, ByVal CODE As String, ByVal Source As String, ByVal Vanphong As Integer, ByVal CatId As Integer, ByVal Avatar As String, ByVal diadiem As String, ByVal diadiemEN As String, ByVal fromdatetime As DateTime, ByVal enddatetime As DateTime, ByVal thanhphan As String, ByVal thanhphanEN As String, ByVal School As String, Org As String, ByVal Gia As Integer, ByVal Descreption As String, ByVal DescreptionEN As String, ByVal LienheName As String, ByVal LienheEmail As String, ByVal LienheMobile As String, ByVal LienheAdd As String, ByVal UserId As Integer, ByVal Portalid As Integer, ByVal Createddate As DateTime, ByVal Isactive As Boolean, ByVal anhbando As String, ByVal linkbando As String, Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Update", id, Title, TitleEN, CODE, Source, Vanphong, CatId, Avatar, diadiem, diadiemEN, fromdatetime, enddatetime, thanhphan, thanhphanEN, School, Org, Gia, Descreption, DescreptionEN, LienheName, LienheEmail, LienheMobile, LienheAdd, UserId, Portalid, Createddate, Isactive, anhbando, linkbando, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Delete(ByVal id As Integer, PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Delete", id, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Events_Find_Count(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Find_Count", subtractIds, datefrom, dateto, title, CatId, PortalId, Isactive, UserId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Find_Index(ByVal subtractIds As String, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, CatId As Integer, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Find_Index", subtractIds, datefrom, dateto, title, CatId, PortalId, Isactive, UserId, PageIndex, PageSize), IDataReader)
        End Function
        Public Overrides Function Events_FindShow_Count(ByVal subtractIds As String, PortalId As Integer, ByVal Isactive As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_FindShow_Count", subtractIds, PortalId, Isactive)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_FindShow_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_FindShow_Index", subtractIds, PortalId, Isactive, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_FindShowPast_Count(ByVal subtractIds As String, PortalId As Integer, ByVal Isactive As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_FindShowPast_Count", subtractIds, PortalId, Isactive)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_FindShowPast_Index(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Isactive As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_FindShowPast_Index", subtractIds, PortalId, Isactive, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllShow(PortalId As Integer, Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllShow", PortalId, Count)
        End Function
        Public Overrides Function Events_GetAllShowEnd(PortalId As Integer, Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllShowEnd", PortalId, Count)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_GetAllOnline(PortalId As Integer, Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_SelectAllOnline", PortalId, Count)
        End Function
#End Region
#Region "NVCMS_Events_Template"
        Public Overrides Function NVCMS_Events_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Events_Template_SelectByID", Id, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function NVCMS_Events_Template_SelectAll(ByVal Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Events_Template_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Sub NVCMS_Events_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Events_Template_Insert", TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_Events_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Events_Template_Update", Id, TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_Events_Template_Delete(ByVal Id As Integer, Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Events_Template_Delete", Id, Portalid)
        End Sub

        '------------------------------------------'
#End Region
#Region "NV_Events_Student"

        Public Overrides Function Events_Student_GetAllByEvent(ByVal EventId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEvent", EventId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetAllByStudent(ByVal StudentId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByStudent", StudentId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetAllByEventCheckIn(ByVal EventId As Integer, Source As String, checkin As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCheckIn", EventId, Source, checkin)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCheckInbySource(ByVal EventId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCheckInbySource", EventId, CheckIn, Source)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Student_Insert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, ByVal Source As Integer, ByVal Nguon As String, ByVal CreatedDate As DateTime, ByVal PortalId As Integer, Nguontutao As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_Insert", EventId, EventCatId, StudentId, StudentCode, Source, Nguon, CreatedDate, PortalId, Nguontutao)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateCheckIn(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateCheckIn", EventId, EventCatId, StudentId, Nguoidikem, Checkin, CheckInDate, UserId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateThamdu(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Thamdu As Boolean, ByVal ThamduDateUpdate As DateTime, ByVal ThamduUserUpdate As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateThamdu", EventId, EventCatId, StudentId, Thamdu, ThamduDateUpdate, ThamduUserUpdate)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateCheckInAfterFair(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal Checkin As Boolean, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateCheckInAfterFair", EventId, EventCatId, StudentId, Checkin, CheckInDate, UserId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateStudentNguon(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal Nguon As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateStudentNguon", EventId, StudentId, Nguon)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateStudentNguonTutao(ByVal EventId As Integer, ByVal StudentId As Integer, ByVal NguonTutao As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateStudentNguonTutao", EventId, StudentId, NguonTutao)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Events_Student_UpdateCheckInInsert(ByVal EventId As Integer, EventCatId As Integer, ByVal StudentId As Integer, ByVal StudentCode As String, Source As Integer, Nguoidikem As Integer, ByVal Checkin As Boolean, ByVal CreatedDate As DateTime, Portalid As Integer, ByVal CheckInDate As DateTime, ByVal UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_UpdateCheckInInsert", EventId, StudentId, EventCatId, StudentCode, Source, Nguoidikem, Checkin, CreatedDate, Portalid, CheckInDate, UserId)
        End Sub
        Public Overrides Function Events_Student_GetById(ByVal NewsFeedbackId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectById", NewsFeedbackId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetAllByEventCat(ByVal EventCatId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCat", EventCatId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetCountByEventCat(Datetime As DateTime, ByVal EventCatId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_CountByEventCat", Datetime, EventCatId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_GetCountByEvent(Datetime As DateTime, ByVal EventId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_CountByEvent", Datetime, EventId)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatCheckInbySource(ByVal EventCatId As Integer, CheckIn As Boolean, Source As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatCheckInbySource", EventCatId, CheckIn, Source)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatbySource(ByVal EventCatId As Integer, Source As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatbySource", EventCatId, Source)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatbyNguon(ByVal EventCatId As Integer, CheckIn As Integer, Nguon As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatbyNguon", EventCatId, CheckIn, Nguon)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventCatbyNguonTutao(ByVal EventCatId As Integer, CheckIn As Integer, NguonTutao As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventCatbyNguonTutao", EventCatId, CheckIn, NguonTutao)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectAllByEventbyNguon(ByVal EventId As Integer, CheckIn As Integer, Nguon As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectAllByEventbyNguon", EventId, CheckIn, Nguon)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_FindCountByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_FindCountByEvent", EventId, EventCatId, Checkin, Source)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_FindCountThamDuByEvent(EventId As Integer, EventCatId As Integer, Thamdu As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Events_Student_FindCountThamDuByEvent", EventId, EventCatId, Thamdu)
        End Function
        '------------------------------------------'
        Public Overrides Function Events_Student_FindIndexByEvent(EventId As Integer, EventCatId As Integer, Checkin As Integer, Source As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_FindIndexByEvent", EventId, EventCatId, Checkin, Source, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Sub Events_Student_DeleteStudentEventId(ByVal EventId As Integer, StudentId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NV_Events_Student_DeleteStudentEventId", EventId, StudentId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Events_Student_SelectByEventstudentid(ByVal EventId As Integer, StudentId As Integer)
            Return SqlHelper.ExecuteReader(ConnectionString, "NV_Events_Student_SelectByEventstudentid", EventId, StudentId)
        End Function
        '------------------------------------------'

#End Region
#End Region


    End Class

End Namespace