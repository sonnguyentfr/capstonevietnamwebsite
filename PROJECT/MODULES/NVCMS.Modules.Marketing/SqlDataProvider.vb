Imports System.Text
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.Marketing

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
#Region "Marketing_Mail_Account"

        Public Overrides Function Marketing_Mail_Account_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Account_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_Account_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Account_SelectAll")
        End Function

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Account_Insert(ByVal Name As String, ByVal Mail As String, ByVal Password As String, ByVal UserId As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Account_Insert", Name, Mail, Password, UserId, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Account_Update(ByVal id As Integer, ByVal Name As String, ByVal Mail As String, ByVal Password As String, ByVal UserId As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Account_Update", id, Name, Mail, Password, UserId, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Account_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Account_Delete", id)
        End Sub

        '------------------------------------------'


#End Region
#Region "Marketing_Mail_Campaing"

        Public Overrides Function Marketing_Mail_Campaing_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Campaing_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_Campaing_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Campaing_SelectAll")
        End Function

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Campaing_Insert(ByVal Title As String, ByVal Description As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Campaing_Insert", Title, Description, CreatedDate, UserId, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Campaing_Update(ByVal id As Integer, ByVal Title As String, ByVal Description As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Campaing_Update", id, Title, Description, CreatedDate, UserId, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Campaing_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Campaing_Delete", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Campaing_DeleteCampaing(ByVal CampaingId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_DeleteCampaing", CampaingId)
        End Sub
        '------------------------------------------'
#End Region
#Region "Marketing_Mail_ListMail"

        Public Overrides Function Marketing_Mail_ListMail_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_ListMail_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_ListMail_GetAll(CampaingId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_ListMail_SelectAll", CampaingId)
        End Function

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_ListMail_Insert(ByVal CampaingId As Integer, ByVal Email As String, ByVal Status As Boolean, ByVal Datetime As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_Insert", CampaingId, Email, Status, Datetime, UserId, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_ListMail_Update(ByVal id As Integer, ByVal CampaingId As Integer, ByVal Email As String, ByVal Status As Boolean, ByVal Datetime As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_Update", id, CampaingId, Email, Status, Datetime, UserId, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_ListMail_UpdateCountSend(ByVal id As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_UpdateCountSend", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_ListMail_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_Delete", id)
        End Sub

        '------------------------------------------'
#End Region
#Region "Marketing_Mail_Template"
        Public Overrides Function Marketing_Mail_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Template_SelectByID", Id, Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function Marketing_Mail_Template_SelectAll(ByVal Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Template_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Template_Insert", TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Template_Update", Id, TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_Template_Delete(ByVal Id As Integer, Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_Template_Delete", Id, Portalid)
        End Sub

        '------------------------------------------'
#End Region
#Region "Marketing_Mail_ListMail_Unsub"

        Public Overrides Function Marketing_Mail_ListMail_Unsub_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_ListMail_Unsub_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_ListMail_Unsub_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_ListMail_Unsub_SelectAll")
        End Function

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_ListMail_Unsub_Insert(ByVal objInfo As Marketing_Mail_ListMailUnsubInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_Unsub_Insert", objInfo.Email, objInfo.reason, objInfo.created_date, objInfo.PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_ListMail_Unsub_Update(ByVal objInfo As Marketing_Mail_ListMailUnsubInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_Unsub_Update", objInfo.id, objInfo.Email, objInfo.reason, objInfo.created_date, objInfo.PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Mail_ListMail_Unsub_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Marketing_Mail_ListMail_Unsub_Delete", id)
        End Sub

        '------------------------------------------'


#End Region
#Region "Marketing_Mail_Campaign_Send"

        Public Overrides Function Marketing_Mail_Campaign_Send_Insert(ByVal campaignId As Integer, ByVal subject As String, ByVal body As String, ByVal totalRecipient As Integer, ByVal createdDate As DateTime) As Integer
            Return CInt(SqlHelper.ExecuteScalar(ConnectionString, "Marketing_Mail_Campaign_Send_Insert", campaignId, subject, body, totalRecipient, createdDate))
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_Campaign_Send_GetByID(ByVal id As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Campaign_Send_GetByID", id), IDataReader)
        End Function

        '------------------------------------------'

#End Region
#Region "Marketing_Mail_Send_Log"

        Public Overrides Function Marketing_Mail_Send_Log_Insert(ByVal campaignSendId As Integer, ByVal listMailId As Integer, ByVal email As String, ByVal createdDate As DateTime) As Integer
            Return CInt(SqlHelper.ExecuteScalar(ConnectionString, "Marketing_Mail_Send_Log_Insert", campaignSendId, listMailId, email, createdDate))
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_Send_Log_GetByCampaignSendId(ByVal campaignSendId As Integer, ByVal status As String, ByVal email As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal sortBy As String, ByVal sortDirection As String) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Send_Log_GetByCampaignSendId", campaignSendId, status, email, pageIndex, pageSize, sortBy, sortDirection), IDataReader)
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_Send_Log_GetStatistics(ByVal campaignSendId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Send_Log_GetStatistics", campaignSendId), IDataReader)
        End Function

        '------------------------------------------'
        Public Overrides Function Marketing_Mail_Send_Log_GetStatusDistribution(ByVal campaignSendId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Send_Log_GetStatusDistribution", campaignSendId), IDataReader)
        End Function

        '------------------------------------------'

#End Region
#Region "Marketing_Static"

        Public Overrides Function MarketingMailDashboard(CampaignId As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Marketing_Mail_Report_Dashboard", CampaignId), IDataReader)
        End Function

        '------------------------------------------'

#End Region
#End Region


    End Class

End Namespace