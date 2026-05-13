Imports System
Imports System.Data
Imports System.Text
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.TinTuc

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
        Private Const ModuleQualifier As String = ""

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
            _connectionString = Config.GetConnectionString()

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("connectionString")
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
            For i As Integer = 0 To splitedKeyWords.Length - 2

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
        Public Overrides Function NVTest() As String
            Return SqlHelper.ExecuteScalar(ConnectionString, "NV_Test")
        End Function
#Region "categories"
        Public Overrides Sub NV_NewsCategories_add(ByVal categoryname As String, ByVal description As String, ByVal TabID As Integer, TabIdDetail As Integer, ByVal isactive As Integer, ByVal PortalId As Integer, ByVal ParentId As Integer, ByVal OrderNumber As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_add", categoryname, description, TabID, TabIdDetail, isactive, PortalId, ParentId, OrderNumber)
        End Sub
        Public Overrides Sub NV_NewsCategories_delete(ByVal categoryid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsCategories_delete", categoryid)
        End Sub
        Public Overrides Sub NV_NewsCategories_update(ByVal categoryid As Integer, ByVal categoryname As String, ByVal description As String, ByVal TabID As Integer, TabIdDetail As Integer, ByVal isactive As Integer, ByVal PortalId As Integer, ByVal ParentId As Integer, ByVal OrderNumber As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_update", categoryid, categoryname, description, TabID, TabIdDetail, isactive, PortalId, ParentId, OrderNumber)
        End Sub
        Public Overrides Sub NV_NewsCategories_updateOrderNumber(ByVal categoryid As Integer, ByVal OrderNumber As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_updateOrderNumber", categoryid, OrderNumber)
        End Sub
        Public Overrides Function NV_NewsCategories_selectall(ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_selectall", PortalId)
        End Function
        Public Overrides Function NV_NewsCategories_selectallVisible(ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_selectallVisible", PortalId)
        End Function
        Public Overrides Function NV_NewsCategories_selectbyid(ByVal categoryid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_selectbyid", categoryid), IDataReader)
        End Function
        Public Overrides Function NV_NewsCategories_selectByParentId(ByVal Parentid As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_selectbyParentId", Parentid, PortalId)
        End Function
        Public Overrides Function NV_NewsCategories_selectByParentIdExt(ByVal Parentid As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_selectbyParentIdExt", Parentid, PortalId)
        End Function
        Public Overrides Function NV_NewsCategories_selectRandom() As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_SelectRandom"), IDataReader)
        End Function
        Public Overrides Function NV_NewsCategories_selectbyTabID(ByVal tabid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NewsCategories_selectbyTabID", tabid), IDataReader)
        End Function
#End Region
#Region "news"
        Public Overrides Function NV_News_add(ByVal objNews As NV_NewsInfo) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "News_add", objNews.meta_title, objNews.meta_description, objNews.meta_image, objNews.meta_url, objNews.CategoryId, objNews.Title, objNews.ImagePath, objNews.Summary, objNews.keyword, objNews.Content, objNews.isActive, objNews.Hotcat, objNews.Hotsite, objNews.Status, objNews.Unit, objNews.NewsKind, objNews.Type, objNews.TypeUrl, objNews.Links, objNews.Tags, objNews.IsImage, objNews.IsVideo, objNews.IsPhoto, objNews.IsPR, objNews.IsShowBaiMoi, objNews.IsAMP, objNews.IsHienQuangCao, objNews.IsAnNoiDung, objNews.ButDanh, objNews.Note, objNews.SourceInfo, objNews.SourceText, objNews.StorageFolder, objNews.AttachedFiles, objNews.IsEdited, objNews.EditedUser, GetNull(objNews.EditedTime), objNews.Credit, GetNull(objNews.CreateDate), GetNull(objNews.ApprovalRequestDate), GetNull(objNews.ApprovalDate), GetNull(objNews.ReturnedDate), GetNull(objNews.CancelPublishDate), GetNull(objNews.PublishedDate), objNews.UserId, objNews.Tacgia, objNews.LanguageId, objNews.PortalId), Integer)

        End Function
        Public Overrides Sub NV_News_delete(ByVal categoryid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_delete", categoryid)
        End Sub
        Public Overrides Sub NV_News_Approve(ByVal newid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Approve", newid)
        End Sub
        Public Overrides Sub NV_News_update(ByVal objNews As NV_NewsInfo)
            SqlHelper.ExecuteReader(ConnectionString, "News_update", objNews.NewId, objNews.meta_title, objNews.meta_description, objNews.meta_image, objNews.meta_url, objNews.CategoryId, objNews.Title, objNews.ImagePath, objNews.Summary, objNews.keyword, objNews.Content, objNews.isActive, objNews.Hotcat, objNews.Hotsite, objNews.Unit, objNews.NewsKind, objNews.Type, objNews.TypeUrl, objNews.Links, objNews.Tags, objNews.IsImage, objNews.IsVideo, objNews.IsPhoto, objNews.IsPR, objNews.IsShowBaiMoi, objNews.IsAMP, objNews.IsHienQuangCao, objNews.IsAnNoiDung, objNews.ButDanh, objNews.Note, objNews.SourceInfo, objNews.SourceText, objNews.AttachedFiles, objNews.IsEdited, objNews.EditedUser, GetNull(objNews.EditedTime), objNews.Credit, GetNull(objNews.CreateDate), GetNull(objNews.ApprovalRequestDate), GetNull(objNews.ApprovalDate), GetNull(objNews.ReturnedDate), GetNull(objNews.CancelPublishDate), GetNull(objNews.PublishedDate), objNews.PublishedUser, objNews.UserId, objNews.Tacgia, objNews.LanguageId, objNews.PortalId)
        End Sub
        Public Overrides Sub NV_News_updateContent(ByVal newid As Integer, ByVal Content As String)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateContent", newid, Content)
        End Sub
        Public Overrides Sub NV_News_updateStatusDate(ByVal newid As Integer, ByVal Status As Integer, ByVal atDate As DateTime)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateStatus_Date", newid, Status, atDate)
        End Sub
        Public Overrides Sub NV_News_updateStatusNone(ByVal newid As Integer, ByVal Status As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateStatus_None", newid, Status)
        End Sub
        Public Overrides Sub NV_News_updateStatusUser(ByVal newid As Integer, ByVal Status As Integer, ByVal userid As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateStatus_User", newid, Status, userid)
        End Sub
        Public Overrides Sub NV_News_updateStatus(ByVal newid As Integer, ByVal Status As Integer, ByVal userid As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateStatus", newid, Status, userid)
        End Sub
        Public Overrides Sub NV_News_updateVisible(ByVal NewId As Integer, ByVal IsVisible As Boolean)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateVisible", NewId, IsVisible)
        End Sub
        Public Overrides Function NV_News_selectall(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectall", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_selectbyid(ByVal newid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectbyid", newid), IDataReader)
        End Function
        Public Overrides Function NV_News_selectbycategory(ByVal categoryid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectbycategory", categoryid), IDataReader)
        End Function
        Public Overrides Function NV_News_find(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_find", datefrom, dateto, title, categoryid, PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_FindContent(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_FindContent", control, PortalId, datefrom, dateto, key, categoryid), IDataReader)
        End Function
        Public Overrides Function NV_News_findbystatus(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal status As Integer, ByVal UserId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_findbystatus", datefrom, dateto, title, categoryid, PortalId, status, UserId), IDataReader)
        End Function
        Public Overrides Function NV_News_selecthotcat(ByVal categoryid As Integer, Count As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecthotcat", categoryid, Count), IDataReader)
        End Function
        Public Overrides Function NV_News_selecthotsite(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecthotsite", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_selectCustomeNews(ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal Count As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectcustomenews", CategoryId, PortalId, Count), IDataReader)
        End Function
        Public Overrides Function NV_News_select5hotsite(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_select5hotsite", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_selecthotsite(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer) As System.Data.IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecthotsitecount", PortalId, Count, NewsId), IDataReader)
        End Function
        Public Overrides Function NV_News_select3hotsiteByCat(ByVal catId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_select3hotsiteByCat", catId), IDataReader)
        End Function
        Public Overrides Function NV_News_selecthotCatNews(ByVal subtractIds As String, ByVal catId As Integer, count As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecthotcatNews", subtractIds, catId, count), IDataReader)
        End Function
        Public Overrides Function NV_News_selecttopsitenews(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecttopsitenews", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_selecttopcatnews(ByVal PortalId As Integer, ByVal Count As Integer, exceptNewsId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecttopcatnews", PortalId, Count, exceptNewsId), IDataReader)
        End Function
        Public Overrides Function NV_News_selecttopcatbycatid(ByVal categoryid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecttopcatbycatid", categoryid), IDataReader)
        End Function
        Public Overrides Function NV_News_selecttopcatbyphongbanid(ByVal phongbanId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecttopcatbyphongbanid", phongbanId), IDataReader)
        End Function
        Public Overrides Function NV_News_selecttopnewsbycatid(ByVal categoryid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecttopnewsbycatid", categoryid), IDataReader)
        End Function
        Public Overrides Function NV_News_selecttopnormalnews(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selecttopnormalnews", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_select5lastestnews(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_select5lastestnews", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_select6lastestcatnews(ByVal PortalId As Integer, ByVal catId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_select6lastestcatnews", PortalId, catId), IDataReader)
        End Function
        Public Overrides Function NV_News_selectlastestnews(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectlastestnews", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_selectapprovenew(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectapprovenew", startdate, enddate, title, categoryid, PortalId, UserId), IDataReader)
        End Function
        Public Overrides Function NV_News_selectnewsinsamecat(ByVal exceptNewid As Integer, ByVal catid As Integer, ByVal count As Integer, ByVal includeChildrenCat As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectnewsinsamecat", exceptNewid, catid, count, includeChildrenCat), IDataReader)
        End Function
        Public Overrides Function NV_News_selectnewsinsamephongban(ByVal exceptNewid As Integer, ByVal arrPhongBan As String, ByVal count As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectnewsinsamephongban", exceptNewid, arrPhongBan, count), IDataReader)
        End Function
        Public Overrides Function NV_News_selectothertopsitenews(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectothertopsitenews", PortalId), IDataReader)
        End Function
        'TrungNS
        Public Overrides Function AdminFindSourceText_Count(ByVal sourcetext As String, ByVal PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_AdminFindSourceText_Count", sourcetext, PortalId)
        End Function
        Public Overrides Function Select_Count(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal arrPhongBan As String, ByVal isImage As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_Select_Count", subtractIds, CategoryId, PortalId, arrPhongBan, isImage)
        End Function
        Public Overrides Function Select_Index(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal isImage As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_Select_Index", subtractIds, CategoryId, PortalId, PageIndex, PageSize, arrPhongBan, isImage), IDataReader)
        End Function
        Public Overrides Function FindContent_Count(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_FindContent_Count", control, PortalId, datefrom, dateto, key, GetSqlTypeString(key), categoryid, arrPhongBan, uid, isImage, type)
        End Function
        Public Overrides Function FindContent_Index(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_FindContent_Index", control, PortalId, datefrom, dateto, key, GetSqlTypeString(key), categoryid, PageIndex, PageSize, arrPhongBan, uid, isImage, type), IDataReader)
        End Function
        Public Overrides Function FindContentExact_Count(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_FindContentExact_Count", control, PortalId, datefrom, dateto, WrapWordFullText(key), GetSqlTypeString(key), categoryid, arrPhongBan, uid, isImage, type)
        End Function
        Public Overrides Function FindContentExact_Index(ByVal control As Integer, ByVal PortalId As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal categoryid As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal uid As Integer, ByVal isImage As Boolean, ByVal type As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_FindContentExact_Index", control, PortalId, datefrom, dateto, WrapWordFullText(key), GetSqlTypeString(key), categoryid, PageIndex, PageSize, arrPhongBan, uid, isImage, type), IDataReader)
        End Function
        Public Overrides Function Findbystatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_findByStatus_Count", datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan)
        End Function
        Public Overrides Function Findbystatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_findByStatus_Index", datefrom, dateto, title, categoryid, PortalId, Status, UserId, PageIndex, PageSize, arrPhongBan), IDataReader)
        End Function
        Public Overrides Function SelectApproveNews_Count(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, Status As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal isImage As Boolean) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_SelectApproveNews_Count", startdate, enddate, title, categoryid, Status, PortalId, UserId, isImage)
        End Function
        Public Overrides Function SelectApproveNews_Index(ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, Status As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_SelectApproveNews_Index", startdate, enddate, title, categoryid, Status, PortalId, UserId, PageIndex, PageSize, isImage), IDataReader)
        End Function
        Public Overrides Function FindNews_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, isImage As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_findNews_Count", datefrom, dateto, title, categoryid, isImage, PortalId, Status, UserId, arrPhongBan)
        End Function
        Public Overrides Function FindNews_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, isImage As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_findNews_Index", datefrom, dateto, title, categoryid, isImage, PortalId, Status, UserId, PageIndex, PageSize, arrPhongBan), IDataReader)
        End Function
        Public Overrides Function FindImages_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_findImages_Count", datefrom, dateto, title, PortalId, Status, UserId, arrPhongBan)
        End Function
        Public Overrides Function FindImages_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_findImages_Index", datefrom, dateto, title, PortalId, Status, UserId, PageIndex, PageSize, arrPhongBan), IDataReader)
        End Function
        Public Overrides Sub NV_News_updatePublishedDate(ByVal newid As Integer, ByVal publishedDate As DateTime, ByVal userid As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_updatePublishedDate", newid, publishedDate, userid)
        End Sub
        Public Overrides Sub NV_News_updateUsersGet(ByVal newid As Integer, ByVal usersGet As String)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateUsersGet", newid, usersGet)
        End Sub
        Public Overrides Sub NV_News_updateUsersView(ByVal newid As Integer, ByVal usersView As String)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateUsersView", newid, usersView)
        End Sub
        'New LOGIC
        Public Overrides Function SelectApproveNews_CountExt(ByVal UserId As Integer, ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal CreatedUser As Integer, ByVal isImage As Boolean) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_SelectApproveNews_Count_Ext", UserId, startdate, enddate, title, categoryid, PortalId, CreatedUser, isImage)
        End Function
        Public Overrides Function SelectApproveNews_IndexExt(ByVal UserId As Integer, ByVal startdate As Date, ByVal enddate As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal CreatedUser As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_SelectApproveNews_Index_Ext", UserId, startdate, enddate, title, categoryid, PortalId, CreatedUser, PageIndex, PageSize, isImage), IDataReader)
        End Function
        Public Overrides Function Findbystatus_CountExt(ByVal ToUserID As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_findByStatus_Count_Ext", ToUserID, datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan)
        End Function
        Public Overrides Function Findbystatus_IndexExt(ByVal ToUserID As Integer, ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_findByStatus_Index_Ext", ToUserID, datefrom, dateto, title, categoryid, PortalId, Status, UserId, PageIndex, PageSize, arrPhongBan), IDataReader)
        End Function

        Public Overrides Sub NV_News_updateArchiving(ByVal newid As Integer, ByVal isArchived As Boolean, ByVal storagefolder As String)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateArchiving", newid, isArchived, storagefolder)
        End Sub
        Public Overrides Sub NV_News_updateTaping(ByVal newid As Integer, ByVal isTaped As Boolean)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateTaping", newid, isTaped)
        End Sub
        Public Overrides Function FindByPhongBanId_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal arrPhongBan As String, ByVal isImage As Boolean) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_FindByPhongBanId_Count", datefrom, dateto, GetSqlTypeString(key), arrPhongBan, isImage)
        End Function
        Public Overrides Function FindByPhongBanId_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal key As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String, ByVal isImage As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_FindByPhongBanId_Index", datefrom, dateto, GetSqlTypeString(key), PageIndex, PageSize, arrPhongBan, isImage), IDataReader)
        End Function
        Public Overrides Function FindHome_Count(ByVal key As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_FindHome_Count", key)
        End Function
        Public Overrides Function FindHome_Index(ByVal key As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_FindHome_Index", key, PageIndex, PageSize), IDataReader)
        End Function

        'Files handler
        Public Overrides Function FindFiles_Count(ByVal folderID As Integer, ByVal type As Integer, ByVal key As String, ByVal fromDate As DateTime, ByVal toDate As DateTime) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Files_SelectByFolderID_Count", folderID, type, key, fromDate, toDate)
        End Function
        Public Overrides Function FindFiles_Index(ByVal folderID As Integer, ByVal type As Integer, ByVal key As String, ByVal fromDate As DateTime, ByVal toDate As DateTime, ByVal sortDir As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Files_SelectByFolderID_Index", folderID, type, key, fromDate, toDate, sortDir, PageIndex, PageSize), IDataReader)
        End Function

        Public Overrides Function AdminFind_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_AdminFind_Count", datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan, isActive, isImage)
        End Function
        Public Overrides Function AdminFind_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_AdminFind_Index", datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan, isActive, isImage, PageIndex, PageSize), IDataReader)
        End Function

        Public Overrides Function FindDatBai_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal requestedtitle As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_findDatBai_Count", datefrom, dateto, requestedtitle, categoryid, PortalId, UserId, arrPhongBan)
        End Function
        Public Overrides Function FindDatBai_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal requestedtitle As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal arrPhongBan As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_findDatBai_Index", datefrom, dateto, requestedtitle, categoryid, PortalId, UserId, PageIndex, PageSize, arrPhongBan), IDataReader)
        End Function
        Public Overrides Sub NV_News_updateArchiving(ByVal newid As Integer, ByVal Status As Integer, ByVal userid As Integer, ByVal atDate As DateTime)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateStatus_archiving", newid, Status, userid, atDate)
        End Sub
        Public Overrides Sub NV_News_updateLock(ByVal NewId As Integer, ByVal lock As Boolean, ByVal userid As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateLock", NewId, lock, userid)
        End Sub
        Public Overrides Function NV_News_GetLocks(ByVal PortalId As Integer, Optional ByVal newsid As Integer = 0) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_selectLock", PortalId, newsid), IDataReader)
        End Function

        Public Overrides Function NV_News_SelectTopView(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer, fromdate As DateTime, ByVal categoryid As Integer, ByVal arrPhongBan As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_SelectTopView", PortalId, Count, NewsId, fromdate, categoryid, arrPhongBan), IDataReader)
        End Function
        Public Overrides Function NV_News_SelectTopGet(ByVal PortalId As Integer, ByVal Count As Integer, ByVal NewsId As Integer, fromdate As DateTime, ByVal categoryid As Integer, ByVal arrPhongBan As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_SelectTopGet", PortalId, Count, NewsId, fromdate, categoryid, arrPhongBan), IDataReader)
        End Function

        Public Overrides Function FindByPhongBanStatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_findPhongBanStatus_Count", datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan)
        End Function
        Public Overrides Function FindByPhongBanStatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_FindPhongBanStatus_Index", datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan, PageIndex, PageSize), IDataReader)
        End Function
        Public Overrides Sub NV_News_updateProcessUserID(ByVal newid As Integer, ByVal userid As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_updateProcessUserID", newid, userid)
        End Sub
        Public Overrides Function FindNotUse_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal arrExceptPB As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_FindNotUse_Count", datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan, isActive, isImage, arrExceptPB)
        End Function
        Public Overrides Function FindNotUse_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal arrPhongBan As String, isActive As Boolean, isImage As Boolean, ByVal arrExceptPB As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_FindNotUse_Index", datefrom, dateto, title, categoryid, PortalId, Status, UserId, arrPhongBan, isActive, isImage, arrExceptPB, PageIndex, PageSize), IDataReader)
        End Function
        Public Overrides Function addTerm(ByVal vocabularyid As Integer, name As String, des As String, weight As Integer, createduserid As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "AddSimpleTerm", vocabularyid, name, des, weight, createduserid), Integer)
        End Function
        Public Overrides Sub NV_News_IncrementViewCount(ByVal newid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_IncrementViewCount", newid)
        End Sub
        Public Overrides Sub Admin_News_update_Tacgia(ByVal newid As Integer, ByVal tacgia As String)
            SqlHelper.ExecuteReader(ConnectionString, "News_News_Admin_Update_Tacgia", newid, tacgia)
        End Sub
        Public Overrides Sub Admin_News_update_Category(ByVal newid As Integer, ByVal CategoryId As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_News_Admin_Update_Category", newid, CategoryId)
        End Sub
        Public Overrides Sub Admin_News_updateNhuanBut(ByVal newid As Integer, ByVal Credit As Integer)
            SqlHelper.ExecuteReader(ConnectionString, "News_News_Admin_updateNhuanBut", newid, Credit)
        End Sub
        Public Overrides Function User_GetTongView(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_USelectTongViewDate", datefrom, dateto, UserId)
        End Function
#End Region
#Region "show"
        Public Overrides Function Show_BaiMoiNhat(ByVal subtractIds As String, ByVal PortalId As Integer, ByVal Count As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_Show_BaiMoiNhat", subtractIds, PortalId, Count), IDataReader)
        End Function
        Public Overrides Function Show_ShowBaiMoiDanhMuc(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, Count As Integer, isImage As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_Show_BaiMoiDanhMuc", subtractIds, CategoryId, PortalId, Count, isImage), IDataReader)
        End Function
        Public Overrides Function Show_TopViewSite(ByVal PortalId As Integer, SoNgay As Integer, Count As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_ShowSelecttopviewnews", PortalId, SoNgay, Count), IDataReader)
        End Function
        Public Overrides Function Show_SelectTopCatNews(ByVal PortalId As Integer, ByVal Count As Integer, exceptNewsId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_ShowSelectTopCatNews", PortalId, Count, exceptNewsId), IDataReader)
        End Function
        Public Overrides Function Show_SelectTopCatNewsHOT(ByVal subtractIds As String, ByVal CategoryId As Integer, count As Integer, Portalid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_ShowSelectTopCatNewsHOT", subtractIds, CategoryId, count, Portalid), IDataReader)
        End Function
        Public Overrides Function Show_Select_Count(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, languageId As String, ByVal isImage As Boolean) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_News_ShowSelect_Count", subtractIds, CategoryId, PortalId, languageId, isImage)
        End Function
        Public Overrides Function Show_Select_Index(ByVal subtractIds As String, ByVal CategoryId As Integer, ByVal PortalId As Integer, languageId As String, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal isImage As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_ShowSelect_Index", subtractIds, CategoryId, PortalId, languageId, PageIndex, PageSize, isImage), IDataReader)
        End Function
        Public Overrides Function Show_ShowSelectNewsInSameCat(ByVal exceptNewid As Integer, ByVal catid As Integer, ByVal count As Integer, ByVal includeChildrenCat As Boolean) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_Show_SelectNewsInSameCat", exceptNewid, catid, count, includeChildrenCat), IDataReader)
        End Function
        Public Overrides Function Show_YearMonth(ByVal PortalId As Integer, ByVal Year As Integer, Month As Integer, Day As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_News_Show_YearMonth", PortalId, Year, Month, Day), IDataReader)
        End Function
#End Region
#Region "NewsFeedback"

        Public Overrides Function NV_NewsFeedback_GetByID(ByVal NewsFeedbackId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsFeedback_SelectByID", NewsFeedbackId)
        End Function

        '------------------------------------------'
        Public Overrides Function NV_NewsFeedback_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsFeedback_SelectAll")
        End Function

        Public Overrides Function NV_NewsFeedback_GetByNewsId(ByVal NewsId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsFeedback_SelectByNewsId", NewsId)
        End Function

        Public Overrides Function NV_NewsFeedback_GetByPortalId(ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsFeedback_SelectByPortalId", PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Sub NV_NewsFeedback_Insert(ByVal NewsId As Integer, ByVal FullName As String, ByVal Email As String, ByVal CreateDate As DateTime, ByVal PhoneNumber As String, ByVal Title As String, ByVal Content As String, ByVal Address As String, ByVal IPTrack As String, ByVal Status As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsFeedback_Insert", NewsId, FullName, Email, CreateDate, PhoneNumber, Title, Content, Address, IPTrack, Status)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NV_NewsFeedback_Update(ByVal NewsFeedbackId As Integer, ByVal NewsId As Integer, ByVal FullName As String, ByVal Email As String, ByVal CreateDate As DateTime, ByVal PhoneNumber As String, ByVal Title As String, ByVal Content As String, ByVal Address As String, ByVal IPTrack As String, ByVal Status As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsFeedback_Update", NewsFeedbackId, NewsId, FullName, Email, CreateDate, PhoneNumber, Title, Content, Address, IPTrack, Status)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NV_NewsFeedback_Delete(ByVal NewsFeedbackId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsFeedback_Delete", NewsFeedbackId)
        End Sub

        '------------------------------------------'
        Public Overrides Function NV_NewsFeedback_GetByNewsId_Count(ByVal NewsId As Integer, ByVal Status As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NewsFeedback_SelectByNewsID_Count", NewsId, Status)
        End Function
        Public Overrides Function NV_NewsFeedback_GetByNewsId_Index(ByVal NewsId As Integer, ByVal Status As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsFeedback_SelectByNewsID_Index", NewsId, Status, PageIndex, PageSize)
        End Function
#End Region
#Region "NewsStatus"

        Public Overrides Function NV_NewsStatus_GetByID(ByVal NewsStatusId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsStatus_SelectByID", NewsStatusId)
        End Function

        '------------------------------------------'
        Public Overrides Function NV_NewsStatus_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsStatus_SelectAll")
        End Function

        '------------------------------------------'
        Public Overrides Sub NV_NewsStatus_Insert(ByVal StatusName As String, ByVal Description As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsStatus_Insert", StatusName, Description)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NV_NewsStatus_Update(ByVal NewsStatusId As Integer, ByVal StatusName As String, ByVal Description As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsStatus_Update", NewsStatusId, StatusName, Description)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NV_NewsStatus_Delete(ByVal NewsStatusId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsStatus_Delete", NewsStatusId)
        End Sub

        '------------------------------------------'


#End Region
#Region "NV_NewsByCategory"

        Public Overrides Function NV_NewsByCategory_GetByID(ByVal Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByCategoryGet", Id)
        End Function

        '------------------------------------------'
        Public Overrides Function NV_NewsByCategory_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByCategoryGetAll")
        End Function

        '------------------------------------------'
        Public Overrides Function NV_NewsByCategory_Insert(ByVal NewsId As Integer, ByVal CategoryId As Integer, ByVal IsMainCategory As Boolean) As Integer
            SqlHelper.ExecuteScalar(ConnectionString, "NewsByCategoryAdd", NewsId, CategoryId, IsMainCategory)
        End Function

        '------------------------------------------'
        Public Overrides Sub NV_NewsByCategory_Update(ByVal Id As Integer, ByVal NewsId As Integer, ByVal CategoryId As Integer, ByVal IsMainCategory As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByCategoryUpdate", Id, NewsId, CategoryId, IsMainCategory)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NV_NewsByCategory_Delete(ByVal Id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByCategoryDelete", Id)
        End Sub

        Public Overrides Sub NV_NewsByCategory_DeleteByNewsId(ByVal NewsId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByCategoryDeleteByNewsID", NewsId)
        End Sub
        '------------------------------------------'
        Public Overrides Function NV_NewsByCategory_GetByNewsId(ByVal newsId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByCategoryGetByNewsId", newsId)
        End Function

#End Region
#Region "Phan quyen"

        Public Overrides Function Permissions_GetAllUsersByRole(ByVal roleId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NV_GetAllUsersByRole"), roleId), IDataReader)
        End Function

        Public Overrides Function Permissions_GetAllUsersByRoles(ByVal arrRoleId As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NV_GetAllUsersByRoles"), arrRoleId), IDataReader)
        End Function

        Public Overrides Function Permissions_AddUserPermissionByCategories(ByVal userId As Integer, ByVal categoryId As Integer, ByVal roleId As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, GetFullyQualifiedName("NV_GrantPermissonByCategories"), userId, categoryId, roleId), Integer)
        End Function

        Public Overrides Sub Permissions_DeleteUserPermissionByRole(ByVal userId As Integer, ByVal roleId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NV_DeleteAllUserPermission"), userId, roleId)
        End Sub

        Public Overrides Sub Permissions_DeleteUserPermissionByRoleAndCategory(ByVal categoryId As Integer, ByVal roleId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, GetFullyQualifiedName("NV_DeleteUserPermissionByRoleAndCategory"), categoryId, roleId)
        End Sub

        Public Overrides Function Permissions_GetAllCategoriesByUserIdAndRoleid(ByVal userId As Integer, ByVal roleId As Integer, ByVal languageId As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NV_GetAllCategoriesByUserIdAndRoleId"), userId, roleId, languageId), IDataReader)
        End Function

        Public Overrides Function Permissions_GetNotAssignedCategoriesByUserIdAndRoleid(ByVal userId As Integer, ByVal roleId As Integer, ByVal languageId As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NV_GetNotAssignedCategoriesByUserIdAndRoleId"), userId, roleId, languageId), IDataReader)
        End Function

        Public Overrides Function Permissions_GetAllAssignedUsersByRoleIdAndCategoryId(ByVal categoryId As Integer, ByVal roleId As Integer, ByVal languageId As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, GetFullyQualifiedName("NV_GetAllAssignedUsersByRoleIdAndCategoryId"), categoryId, roleId, languageId), IDataReader)
        End Function


#End Region
#Region "NewsByTags"

        Public Overrides Function NewsByTags_GetByTags_Index(ByVal Tags As String, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByTags_SelectByTags_Index", Tags, PortalId, PageIndex, PageSize)
        End Function
        Public Overrides Function NewsByTags_GetByTags_Count(ByVal Tags As String, PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NewsByTags_SelectByTags_Count", Tags, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function NewsByTags_GetByTags(ByVal Tags As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByTags_SelectByTags", Tags)
        End Function
        '------------------------------------------'
        Public Overrides Function NewsByTags_GetByNewId(ByVal NewId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByTags_SelectByNewID", NewId)
        End Function

        '------------------------------------------'
        Public Overrides Function NewsByTags_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByTags_SelectAll")
        End Function
        '------------------------------------------'
        Public Overrides Function NewsByTags_GetByTags(ByVal subtractIds As String, Tags As String, Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByTags_GetByTags", subtractIds, Tags, Count)
        End Function
        '------------------------------------------'

        Public Overrides Function NewsByTags_GetAllAutoComplate() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByTags_SelectAllAutoComplate")
        End Function
        '------------------------------------------'
        Public Overrides Sub NewsByTags_Insert(ByVal NewId As Integer, ByVal Tags As String, TagsTitle As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByTags_Insert", NewId, Tags, TagsTitle, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NewsByTags_DeleteByNewId(ByVal Newid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByTags_DeleteByNewId", Newid)
        End Sub

        '------------------------------------------'


#End Region
#Region "News_Version"
        Public Overrides Function News_Version_GetById(ByVal Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Version_SelectById", Id)
        End Function

        Public Overrides Function News_Version_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Version_SelectAll")
        End Function

        Public Overrides Function News_Version_Insert(ByVal objInfo As NewsVersionInfo) As Integer
            Return CInt(SqlHelper.ExecuteScalar(ConnectionString, "News_Version_Insert", objInfo.CreatedUser, objInfo.NewId, objInfo.CategoryId, objInfo.Title, objInfo.ImagePath, objInfo.Summary, objInfo.Content, objInfo.isActive, objInfo.Hotcat, objInfo.Hotsite, objInfo.Unit, objInfo.NewsKind, objInfo.Type, objInfo.TypeUrl, objInfo.Links, objInfo.Tags, objInfo.IsImage, objInfo.Note, objInfo.SourceInfo, objInfo.StorageFolder, objInfo.AttachedFiles, objInfo.IsEdited, objInfo.EditedUser, GetNull(objInfo.EditedTime), objInfo.Credit, GetNull(objInfo.CreateDate), GetNull(objInfo.ApprovalRequestDate), GetNull(objInfo.ApprovalDate), GetNull(objInfo.ReturnedDate), GetNull(objInfo.CancelPublishDate), GetNull(objInfo.PublishedDate), objInfo.UserId, objInfo.PortalId))
        End Function

        Public Overrides Sub News_Version_Update(ByVal objInfo As NewsVersionInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Version_Update", objInfo.Id, objInfo.CreatedUser, objInfo.NewId, objInfo.CategoryId, objInfo.Title, objInfo.ImagePath, objInfo.Summary, objInfo.Content, objInfo.isActive, objInfo.Hotcat, objInfo.Hotsite, objInfo.Unit, objInfo.NewsKind, objInfo.Type, objInfo.TypeUrl, objInfo.Links, objInfo.Tags, objInfo.IsImage, objInfo.Note, objInfo.SourceInfo, objInfo.StorageFolder, objInfo.AttachedFiles, objInfo.IsEdited, objInfo.EditedUser, GetNull(objInfo.EditedTime), objInfo.Credit, GetNull(objInfo.CreateDate), GetNull(objInfo.ApprovalRequestDate), GetNull(objInfo.ApprovalDate), GetNull(objInfo.ReturnedDate), GetNull(objInfo.CancelPublishDate), GetNull(objInfo.PublishedDate), objInfo.UserId, objInfo.PortalId)
        End Sub

        Public Overrides Sub News_Version_Delete(ByVal Id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Version_Delete", Id)
        End Sub

        Public Overrides Sub News_Version_DeleteByNewsID(ByVal newsID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Version_DeleteByNewsID", newsID)
        End Sub
#End Region
#Region "News_Process"
        Public Overrides Function News_Process_GetById(ByVal ID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Process_SelectById", ID)
        End Function

        Public Overrides Function News_Process_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Process_SelectAll")
        End Function

        Public Overrides Function News_Process_Insert(ByVal objInfo As NewsProcessInfo) As Integer
            Return CInt(SqlHelper.ExecuteScalar(ConnectionString, "News_Process_Insert", objInfo.NewsID, objInfo.StatusID, objInfo.ProcessName, objInfo.Comment, objInfo.ByUser, objInfo.ToUser, objInfo.CreateDate, objInfo.VersionId, objInfo.IPTrack))
        End Function

        Public Overrides Sub News_Process_Update(ByVal objInfo As NewsProcessInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Process_Update", objInfo.ID, objInfo.NewsID, objInfo.StatusID, objInfo.ProcessName, objInfo.Comment, objInfo.ByUser, objInfo.ToUser, objInfo.CreateDate, objInfo.VersionId, objInfo.IPTrack)
        End Sub

        Public Overrides Sub News_Process_Delete(ByVal ID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Process_Delete", ID)
        End Sub

        Public Overrides Function News_Process_GetByNewsId(ByVal newsId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Process_SelectByNewsId", newsId)
        End Function

        Public Overrides Function News_Process_GetCurrentProcess(ByVal newsId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Process_SelectCurrent", newsId)
        End Function

        Public Overrides Function News_Process_GetLastProcessByStatus(ByVal newsId As Integer, ByVal status As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Process_SelectLastProcessByStatus", newsId, status)
        End Function

        Public Overrides Sub News_Process_DeleteByNewsID(ByVal newsID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Process_DeleteByNewsID", newsID)
        End Sub
#End Region
#Region "ViewNews"

        Public Overrides Function NV_ViewNews_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "ViewNews_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function NV_ViewNews_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "ViewNews_SelectAll")
        End Function

        '------------------------------------------'
        Public Overrides Sub NV_ViewNews_Insert(ByVal userid As Integer, ByVal newsid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "ViewNews_Insert", userid, newsid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NV_ViewNews_Update(ByVal id As Integer, ByVal userid As Integer, ByVal newsid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "ViewNews_Update", id, userid, newsid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NV_ViewNews_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "ViewNews_Delete", id)
        End Sub

        '------------------------------------------'
        Public Overrides Function NV_ViewNews_GetByUserId(ByVal userid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "ViewNews_SelectByUser", userid)
        End Function

        Public Overrides Function NV_ViewNews_GetByNewsId(ByVal newsid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "ViewNews_SelectByNewsId", newsid)
        End Function

        Public Overrides Function NV_ViewNews_GetByNewsIdAndUserId(ByVal newsid As Integer, ByVal userid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "ViewNews_SelectByNewsIdAndUserId", newsid, userid)
        End Function
#End Region
#Region "News_Me"
        Public Overrides Function NV_News_Me_add(ByVal categoryid As Integer, ByVal title As String, ByVal imagepath As String, ByVal summary As String, ByVal content As String, ByVal isactive As Integer, ByVal hotcat As Integer, ByVal hotsite As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Exsummary As String, ByVal TypeUrl As String) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "News_Me_add", categoryid, title, imagepath, summary, content, isactive, hotcat, hotsite, PortalId, UserId, Exsummary, TypeUrl), Integer)
        End Function
        Public Overrides Sub NV_News_Me_delete(ByVal categoryid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Me_delete", categoryid)
        End Sub
        Public Overrides Sub NV_News_Me_update(ByVal newid As Integer, ByVal categoryid As Integer, ByVal title As String, ByVal imagepath As String, ByVal summary As String, ByVal content As String, ByVal isactive As Integer, ByVal hotcat As Integer, ByVal hotsite As Integer, ByVal PortalId As Integer, ByVal Exsummary As String, ByVal TypeUrl As String)
            SqlHelper.ExecuteReader(ConnectionString, "News_Me_update", newid, categoryid, title, imagepath, summary, content, isactive, hotcat, hotsite, PortalId, Exsummary, TypeUrl)
        End Sub
        Public Overrides Function NV_News_Me_selectall(ByVal PortalId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_Me_selectall", PortalId), IDataReader)
        End Function
        Public Overrides Function NV_News_Me_selectbyid(ByVal newid As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_Me_selectbyid", newid), IDataReader)
        End Function
        Public Overrides Function News_Me_Findbystatus_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_Me_findByStatus_Count", datefrom, dateto, title, categoryid, PortalId, Status, UserId)
        End Function
        Public Overrides Function News_Me_Findbystatus_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_Me_findByStatus_Index", datefrom, dateto, title, categoryid, PortalId, Status, UserId, PageIndex, PageSize), IDataReader)
        End Function
#End Region
#Region "News_UserWF"

        Public Overrides Function News_UserWF_GetById(ByVal ID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_UserWF_SelectById", ID)
        End Function

        Public Overrides Function News_UserWF_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_UserWF_SelectAll")
        End Function

        Public Overrides Function News_UserWF_Insert(ByVal objInfo As News_UserWFInfo) As Integer
            Return CInt(SqlHelper.ExecuteScalar(ConnectionString, "News_UserWF_Insert", objInfo.TenLuong, objInfo.PhongBan, objInfo.NguoiGui, objInfo.NguoiNhan, objInfo.TrangThaiDich, objInfo.LoaiWF, objInfo.IsDefault, objInfo.MoTa, objInfo.NguoiTao, objInfo.NgayTao, objInfo.IsActive, objInfo.OrderNumber, objInfo.PortalId, objInfo.ModuleId, objInfo.LanguageId, objInfo.IconSmall, objInfo.IconLarge))
        End Function

        Public Overrides Sub News_UserWF_Update(ByVal objInfo As News_UserWFInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_UserWF_Update", objInfo.ID, objInfo.TenLuong, objInfo.PhongBan, objInfo.NguoiGui, objInfo.NguoiNhan, objInfo.TrangThaiDich, objInfo.LoaiWF, objInfo.IsDefault, objInfo.MoTa, objInfo.NguoiTao, objInfo.NgayTao, objInfo.IsActive, objInfo.OrderNumber, objInfo.PortalId, objInfo.ModuleId, objInfo.LanguageId, objInfo.IconSmall, objInfo.IconLarge)
        End Sub

        Public Overrides Sub News_UserWF_Delete(ByVal ID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_UserWF_Delete", ID)
        End Sub

        Public Overrides Function News_UserWF_GetByUserId(ByVal LoaiWF As LoaiWF, ByVal UserId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_UserWF_SelectByUserId", LoaiWF, UserId)
        End Function

        Public Overrides Function News_UserWF_GetByPhongBanId(ByVal LoaiWF As LoaiWF, ByVal phongbanID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_UserWF_SelectByPhongBanId", LoaiWF, phongbanID)
        End Function

        Public Overrides Sub News_UserWF_DeleteByPhongBanId(ByVal phongbanID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_UserWF_DeleteByPhongBanId", phongbanID)
        End Sub
#End Region
#Region "News_UserProcess"

        Public Overrides Function News_UserProcess_GetById(ByVal ID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_UserProcess_SelectById", ID)
        End Function

        Public Overrides Function News_UserProcess_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_UserProcess_SelectAll")
        End Function

        Public Overrides Function News_UserProcess_Insert(ByVal objInfo As News_UserProcessInfo) As Integer
            Return CInt(SqlHelper.ExecuteScalar(ConnectionString, "News_UserProcess_Insert", objInfo.UserID, objInfo.NewsID, objInfo.Status, objInfo.CreatedDate, objInfo.CreatedUser))
        End Function

        Public Overrides Sub News_UserProcess_Update(ByVal objInfo As News_UserProcessInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_UserProcess_Update", objInfo.ID, objInfo.UserID, objInfo.NewsID, objInfo.Status)
        End Sub

        Public Overrides Sub News_UserProcess_Delete(ByVal ID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_UserProcess_Delete", ID)
        End Sub

        Public Overrides Function News_UserProcess_GetByUserId(ByVal UserId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_UserProcess_SelectByUserId", UserId)
        End Function

        Public Overrides Sub News_UserProcess_DeleteByNewsID(ByVal NewsID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_UserProcess_DeleteByNewsID", NewsID)
        End Sub
#End Region
#Region "NewsByMedia"

        Public Overrides Function NewsByMedia_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByMedia_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function NewsByMedia_GetAllByNewid(newid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByMedia_SelectAllByNewId", newid)
        End Function
        '------------------------------------------'
        Public Overrides Sub NewsByMedia_Insert(ByVal newid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByMedia_Insert", newid, mediaid, createdted, userid, portalid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NewsByMedia_Update(ByVal id As Integer, ByVal newid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByMedia_Update", id, newid, mediaid, createdted, userid, portalid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NewsByMedia_UpdateNewId(ByVal newid As Integer, newidnew As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByMedia_UpdateNewId", newid, newidnew)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NewsByMedia_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByMedia_Delete", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NewsByMedia_DeleteByNewId(ByVal newid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByMedia_DeleteByNewId", newid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NewsByMedia_DeleteByMediaId(ByVal Mediaid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByMedia_DeleteByMediaid", Mediaid)
        End Sub
        '------------------------------------------'


#End Region
#Region "NVCMS_MediaItem"

        Public Overrides Function MediaItem_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_MediaItem_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function MediaItem_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_MediaItem_SelectAll", PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function MediaItem_Insert(ByVal title As String, ByVal filename As String, ByVal forder As String, ByVal MediaUrl As String, ByVal Size As Integer, ByVal extension As String, ByVal createddate As DateTime, ByVal userid As Integer, ByVal portalid As Integer) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_MediaItem_Insert", title, filename, forder, MediaUrl, Size, extension, createddate, userid, portalid), Integer)
        End Function
        '------------------------------------------'
        Public Overrides Sub MediaItem_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_MediaItem_Delete", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub MediaItem_UpdateTitle(ByVal id As Integer, ByVal title As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_MediaItem_UpdateTitle", id, title)
        End Sub


#End Region
#Region "News_Template"
        Public Overrides Function News_Template_Get(ByVal PortalID As Integer, ByVal TemplateId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Template_Get", PortalID, TemplateId)
        End Function
        Public Overrides Function News_Template_GetAll(ByVal PortalID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Template_GetAll", PortalID)
        End Function
        Public Overrides Sub News_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Template_Add", TemplateName, FilePath, PortalID)
        End Sub
        Public Overrides Sub News_Template_Delete(ByVal TemplateId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Template_Delete", TemplateId)
        End Sub
        Public Overrides Sub News_Template_Update(ByVal TemplateId As Integer, ByVal TemplateName As String, ByVal FilePath As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Template_Update", TemplateId, TemplateName, FilePath)
        End Sub
#End Region
#Region "News_Settings"

        Public Overrides Function News_Settings_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Settings_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function News_Settings_GetAll(Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Settings_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function News_Settings_GetAllByType(ByVal Type As Integer, Count As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Settings_SelectAllByType", Type, Count, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub News_Settings_Insert(ByVal NewId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Settings_Insert", NewId, OrderNumber, Type, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub News_Settings_Update(ByVal id As Integer, ByVal NewId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Settings_Update", id, NewId, OrderNumber, Type, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub News_Settings_UpdateOrder(ByVal id As Integer, ByVal OrderNumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Settings_UpdateOrder", id, OrderNumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub News_Settings_Delete(ByVal Type As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Settings_Delete", Type, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub News_Settings_DeleteById(ByVal Id As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Settings_DeleteById", Id, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub News_Settings_DeleteByNewId(ByVal NewId As Integer, ByVal Type As Integer, ByVal Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Settings_DeleteByNewId", NewId, Type, Portalid)
        End Sub

        '------------------------------------------'


#End Region
#Region "NewsByShare"

        Public Overrides Function NewsByShare_GetByNewID(ByVal NewId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByShare_SelectAllByNewId", NewId)
        End Function
        Public Overrides Function NewsByShare_GetCountByNewId(ByVal NewId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NewsByShare_SelectCountNewId", NewId)
        End Function
        ''------------------------------------------'
        'Public Overrides Function NewsByShare_GetAll() As IDataReader
        '    Return SqlHelper.ExecuteReader(ConnectionString, "NewsByShare_SelectAll")
        'End Function

        '------------------------------------------'
        Public Overrides Sub NewsByShare_Insert(ByVal NewId As Integer, ByVal LinkShare As String, ByVal CreatedDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByShare_Insert", NewId, LinkShare, CreatedDate)
        End Sub

#End Region
#Region "NewsByView"

        Public Overrides Function NewsByView_GetByNewID(ByVal Newid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsByView_SelectByNewID", Newid)
        End Function

        '------------------------------------------'
        Public Overrides Sub NewsByView_Insert(ByVal NewId As Integer, ByVal ViewCount As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByView_Insert", NewId, ViewCount, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NewsByView_Update(ByVal NewId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByView_Update", NewId)
        End Sub

#End Region
#Region "News_NhuanBut"

        Public Overrides Function News_NhuanBut_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_NhuanBut_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function News_NhuanBut_GetAll(ByVal NewId As Integer, KieuNhuanBut As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_NhuanBut_SelectAll", NewId, KieuNhuanBut)
        End Function
        '------------------------------------------'
        Public Overrides Function News_NhuanBut_GetCount(ByVal NewId As Integer, KieuNhuanBut As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_NhuanBut_SelectCount", NewId, KieuNhuanBut)
        End Function
        '------------------------------------------'
        Public Overrides Function News_NhuanBut_GetTongTien(ByVal NewId As Integer, KieuNhuanBut As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_NhuanBut_SelectTongTien", NewId, KieuNhuanBut)
        End Function
        '------------------------------------------'
        Public Overrides Sub News_NhuanBut_Insert(ByVal NewId As Integer, ByVal Type As Integer, ByVal UserId As Integer, ByVal Credit As Integer, ByVal Createdate As DateTime, ByVal CreateUser As Integer, ByVal UserChamNhuanBut As Integer, ByVal PortalId As Integer, KieuNhuanBut As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_NhuanBut_Insert", NewId, Type, UserId, Credit, Createdate, CreateUser, UserChamNhuanBut, PortalId, KieuNhuanBut)
        End Sub

        '------------------------------------------'
        Public Overrides Sub News_NhuanBut_Update(ByVal id As Integer, ByVal NewId As Integer, ByVal Type As Integer, ByVal UserId As Integer, ByVal Credit As Integer, ByVal Createdate As DateTime, ByVal CreateUser As Integer, ByVal UserChamNhuanBut As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_NhuanBut_Update", id, NewId, Type, UserId, Credit, Createdate, CreateUser, UserChamNhuanBut, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub News_NhuanBut_UpdateNhuan(ByVal id As Integer, ByVal Credit As Integer, ByVal UserChamNhuanBut As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_NhuanBut_UpdateNhuanBut", id, Credit, UserChamNhuanBut)
        End Sub
        '------------------------------------------'
        Public Overrides Sub News_NhuanBut_UpdateNhuanXuatBan(ByVal NewId As Integer, ByVal UserChamNhuanBut As Integer, UserChamNhuanButdate As DateTime, XuatBan As Boolean, KieuNhuanBut As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_NhuanBut_UpdateNhuanButXuatBan", NewId, UserChamNhuanBut, UserChamNhuanButdate, XuatBan, KieuNhuanBut)
        End Sub
        '------------------------------------------'
        Public Overrides Sub News_NhuanBut_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_NhuanBut_Delete", id)
        End Sub
        Public Overrides Function News_NhuanBut_Find_Count(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, KieuNhuanBut As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_NhuanBut_Find_Count", datefrom, dateto, UserId, type, PortalId, KieuNhuanBut)
        End Function
        Public Overrides Function News_NhuanBut_Find_Index(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, KieuNhuanBut As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "News_NhuanBut_Find_Index", datefrom, dateto, UserId, type, PortalId, PageIndex, PageSize, KieuNhuanBut), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function News_NhuanBut_Find_Index_Export(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer, type As Integer, PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, KieuNhuanBut As Integer) As Object
            Return SqlHelper.ExecuteDataset(New SqlDataProvider().ConnectionString, "News_NhuanBut_Find_Index_Export", datefrom, dateto, UserId, type, PortalId, PageIndex, PageSize, KieuNhuanBut).Tables(0)
        End Function
        Public Overrides Function NhuanBut_User_GetTongTien(ByVal datefrom As Date, ByVal dateto As Date, UserId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "News_NhuanBut_USelectTongTienDate", datefrom, dateto, UserId)
        End Function
#End Region
#Region "News_Attach"
        Public Overrides Sub News_AttachByPhongBan_Add(ByVal newsAttachByPhongBan As News_AttachByPhongBanInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_AttachByPhongBan_Add", newsAttachByPhongBan.AttachFileID, newsAttachByPhongBan.PhongBanID)
        End Sub
        Public Overrides Sub News_AttachByPhongBan_DeleteByAttachId(ByVal AttachId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_AttachByPhongBan_DeleteByAttachID", AttachId)
        End Sub
        Public Overrides Sub News_AttachByPhongBan_DeleteByPhongBanId(ByVal PhongBanId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_AttachByPhongBan_DeleteByPhongBanID", PhongBanId)
        End Sub
        Public Overrides Function News_AttachByPhongBan_GetByAttachId(attachId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_AttachByPhongBan_GetByAttachID", attachId)
        End Function

        Public Overrides Function News_Attach_Add(ByVal newsAttach As NewsAttachInfo) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "News_AttachFile_Add", newsAttach.FileName, newsAttach.Description, newsAttach.FileType, newsAttach.FileId, newsAttach.IsPublic, newsAttach.CreatedDate, newsAttach.PortalId), Integer)
        End Function
        Public Overrides Sub News_Attach_Update(ByVal newsAttach As NewsAttachInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_AttachFile_Update", newsAttach.AttachFileID, newsAttach.FileName, newsAttach.Description, newsAttach.FileType, newsAttach.FileId, newsAttach.IsPublic, newsAttach.PortalId)
        End Sub
        Public Overrides Sub News_Attach_UpdateFileName(ByVal AttachId As Integer, FileName As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_AttachFile_UpdateFileName", AttachId, FileName)
        End Sub
        Public Overrides Sub News_Attach_Delete(ByVal AttachId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_AttachFile_Delete", AttachId)
        End Sub
        Public Overrides Sub News_Attach_SwapSort(ByVal FirstId As Integer, ByVal SecondId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_AttachFile_SwapSort", FirstId, SecondId)
        End Sub
        Public Overrides Function News_Attach_Get(ByVal AttachId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_AttachFile_Get", AttachId)
        End Function
        Public Overrides Function News_Attach_GetByNewId(ByVal PortalId As Integer, ByVal NewsId As Integer, ByVal UserId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_AttachFile_GetByNewsId", PortalId, NewsId, UserId)
        End Function
        Public Overrides Function News_Attach_GetMaxId() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_AttachFile_GetMaxID")
        End Function

        Public Overrides Sub NewsByAttach_Add(ByVal newsByAttach As NewsByAttachInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByAttach_Add", newsByAttach.NewsId, newsByAttach.AttachId)
        End Sub
        Public Overrides Sub NewsByAttach_DeleteByAttachId(ByVal AttachId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByAttach_DeleteByAttachId", AttachId)
        End Sub
        Public Overrides Sub NewsByAttach_DeleteByNewsId(ByVal NewsId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsByAttach_DeleteByNewsId", NewsId)
        End Sub
#End Region
#Region "News_Note"
        Public Overrides Function News_Note_GetbyNewId(ByVal NewId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Note_SelectByNewId", NewId)
        End Function
        Public Overrides Function News_Note_GetbyNewIdTop1(ByVal NewId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "News_Note_SelectByNewIdTop1", NewId)
        End Function
        Public Overrides Sub News_Note_Insert(ByVal objInfo As NewsNoteInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "News_Note_Insert", objInfo.NewId, objInfo.Noidung, objInfo.CreatedDate, objInfo.UserId, objInfo.PortalId)
        End Sub

#End Region
#End Region

    End Class
End Namespace