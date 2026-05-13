
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke

Namespace NVCMS.Modules.Video

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
            _connectionString = Config.GetConnectionString("SiteSqlServer")

            If _connectionString = "" Then
                ' Use connection string specified in provider
                _connectionString = objProvider.Attributes("SiteSqlServer")
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

#Region "Public Methods"
#Region "videosclip"
        '------------------------------------------'
        Public Overrides Function Videos_Insert(ByVal objVideo As Videos_Info) As Integer
            Return CType(SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Video_Insert", objVideo.CategoryId, objVideo.Title, objVideo.ImagePath, objVideo.VideoPath, objVideo.Summary, objVideo.Content, objVideo.TypeVideo, objVideo.isActive, GetNull(objVideo.Createdate), objVideo.Status, objVideo.UserId, objVideo.LanguageId, objVideo.PortalId), Integer)
        End Function
        Public Overrides Function Videos_GetByID(ByVal VideoId As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_SelectByID", VideoId, PortalId)
        End Function
        Public Overrides Function Admin_Find_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Video_AdminFind_Count", datefrom, dateto, title, categoryid, PortalId, Status, UserId)
        End Function
        Public Overrides Function Admin_Find_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal sapxep As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_AdminFind_Index", datefrom, dateto, title, categoryid, PortalId, Status, UserId, PageIndex, PageSize, sapxep), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Sub Videos_Update(ByVal objVideo As Videos_Info)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_update", objVideo.VideoId, objVideo.CategoryId, objVideo.Title, objVideo.ImagePath, objVideo.VideoPath, objVideo.Summary, objVideo.Content, objVideo.TypeVideo, objVideo.isActive, objVideo.Hotcat, objVideo.Hotsite, objVideo.Status, objVideo.Tags, objVideo.IsShowBaiMoi, objVideo.ButDanh, objVideo.IsEdited, objVideo.EditedUser, GetNull(objVideo.EditedTime), objVideo.VoteCount, objVideo.ViewCount, objVideo.Credit, GetNull(objVideo.Createdate), GetNull(objVideo.ApprovalRequestDate), GetNull(objVideo.ApprovalDate), objVideo.ApprovalUser, GetNull(objVideo.ReturnedDate), objVideo.ReturnedUser, GetNull(objVideo.CancelPublishDate), GetNull(objVideo.CancelPublishUser), GetNull(objVideo.PublishedDate), objVideo.PublishedUser, objVideo.UserId, objVideo.Tacgia, objVideo.LanguageId, objVideo.PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Videos_UpdateStatus(ByVal VideoId As Integer, Status As Integer, UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_UpdateStatus", VideoId, Status, UserId)
        End Sub
        ''------------------------------------------'
        Public Overrides Sub Videos_UpdatePublishedDate(ByVal VideoId As Integer, PublicDate As DateTime, UserId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_UpdatePublishedDate", VideoId, PublicDate, UserId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Videos_Find_Show_Count(ByVal PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Video_Show_Find_Count", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Videos_Find_Show_Index(ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Show_Find_Index", PortalId, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'

#End Region
#Region "NVCMS_VideoByMedia"

        Public Overrides Function VideoByMedia_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_VideoByMedia_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function VideoByMedia_GetAllByvideoid(videoid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_VideoByMedia_SelectAllByvideoid", videoid)
        End Function
        '------------------------------------------'
        Public Overrides Sub VideoByMedia_Insert(ByVal videoid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_VideoByMedia_Insert", videoid, mediaid, createdted, userid, portalid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub VideoByMedia_Update(ByVal id As Integer, ByVal videoid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_VideoByMedia_Update", id, videoid, mediaid, createdted, userid, portalid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub VideoByMedia_Updatevideoid(ByVal videoid As Integer, videoidnew As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_VideoByMedia_Updatevideoid", videoid, videoidnew)
        End Sub
        '------------------------------------------'
        Public Overrides Sub VideoByMedia_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_VideoByMedia_Delete", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub VideoByMedia_DeleteByvideoid(ByVal videoid As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_VideoByMedia_DeleteByvideoid", videoid, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub VideoByMedia_DeleteByMediaId(ByVal Mediaid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_VideoByMedia_DeleteByMediaid", Mediaid)
        End Sub
        '------------------------------------------'


#End Region
#Region "Video_Process"
        Public Overrides Function Video_Process_GetById(ByVal ID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Process_SelectById", ID)
        End Function

        Public Overrides Function Video_Process_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Process_SelectAll")
        End Function

        Public Overrides Function Video_Process_Insert(ByVal objInfo As VideoProcessInfo) As Integer
            Return CInt(SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Video_Process_Insert", objInfo.VideoId, objInfo.StatusID, objInfo.ProcessName, objInfo.Comment, objInfo.ByUser, objInfo.ToUser, objInfo.CreateDate, objInfo.VersionId, objInfo.IPTrack))
        End Function

        Public Overrides Sub Video_Process_Update(ByVal objInfo As VideoProcessInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Process_Update", objInfo.ID, objInfo.VideoId, objInfo.StatusID, objInfo.ProcessName, objInfo.Comment, objInfo.ByUser, objInfo.ToUser, objInfo.CreateDate, objInfo.VersionId, objInfo.IPTrack)
        End Sub

        Public Overrides Sub Video_Process_Delete(ByVal ID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Process_Delete", ID)
        End Sub

        Public Overrides Function Video_Process_GetByNewsId(ByVal newsId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Process_SelectByVideoId", newsId)
        End Function

        Public Overrides Function Video_Process_GetCurrentProcess(ByVal newsId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Process_SelectCurrent", newsId)
        End Function

        Public Overrides Function Video_Process_GetLastProcessByStatus(ByVal newsId As Integer, ByVal status As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Process_SelectLastProcessByStatus", newsId, status)
        End Function

        Public Overrides Sub Video_Process_DeleteByNewsID(ByVal newsID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Process_DeleteByNewsID", newsID)
        End Sub
#End Region
#Region "Video_Settings"

        Public Overrides Function Video_Settings_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Settings_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function Video_Settings_GetAll(Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Settings_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function Video_Settings_GetAllByType(ByVal Type As Integer, Count As Integer, PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Settings_SelectAllByType", Type, Count, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub Video_Settings_Insert(ByVal VideoId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Settings_Insert", VideoId, OrderNumber, Type, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Video_Settings_Update(ByVal id As Integer, ByVal VideoId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Settings_Update", id, VideoId, OrderNumber, Type, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Video_Settings_UpdateOrder(ByVal id As Integer, ByVal OrderNumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Settings_UpdateOrder", id, OrderNumber)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Video_Settings_Delete(ByVal Type As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Settings_Delete", Type, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Video_Settings_DeleteByVideoId(ByVal VideoId As Integer, ByVal Type As Integer, ByVal Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Settings_DeleteByVideoId", VideoId, Type, Portalid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Video_Settings_DeleteById(ByVal Id As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Settings_DeleteById", Id, PortalId)
        End Sub
        '------------------------------------------'


#End Region
#Region "NVCMS_Video_Template"
        Public Overrides Function Video_Template_Get(ByVal PortalID As Integer, ByVal TemplateId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Template_Get", PortalID, TemplateId)
        End Function
        Public Overrides Function Video_Template_GetAll(ByVal PortalID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Video_Template_GetAll", PortalID)
        End Function
        Public Overrides Sub Video_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Template_Add", TemplateName, FilePath, PortalID)
        End Sub
        Public Overrides Sub Video_Template_Delete(ByVal TemplateId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Template_Delete", TemplateId)
        End Sub
        Public Overrides Sub Video_Template_Update(ByVal TemplateId As Integer, ByVal TemplateName As String, ByVal FilePath As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Video_Template_Update", TemplateId, TemplateName, FilePath)
        End Sub
#End Region
#End Region


    End Class

End Namespace