
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke

Namespace NVCMS.Modules.Banner

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

#Region "Public Methods"
#Region "NVCMS_Banner_Vitri"

        Public Overrides Function _Vitri_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_Vitri_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function _Vitri_GetAll(PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_Vitri_SelectAll", PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Sub _Vitri_Insert(ByVal Title As String, ByVal width As Integer, ByVal height As Integer, ByVal Images As String, ByVal CreatedByUserId As Integer, ByVal CreatedOnDate As DateTime, ByVal LastModifiedByUserId As Integer, ByVal LastModifiedOnDate As DateTime, ByVal ModuleId As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Vitri_Insert", Title, width, height, Images, CreatedByUserId, CreatedOnDate, LastModifiedByUserId, LastModifiedOnDate, ModuleId, portalid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub _Vitri_Update(ByVal id As Integer, ByVal Title As String, ByVal width As Integer, ByVal height As Integer, ByVal Images As String, ByVal LastModifiedByUserId As Integer, ByVal LastModifiedOnDate As DateTime)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Vitri_Update", id, Title, width, height, Images, LastModifiedByUserId, LastModifiedOnDate)
        End Sub

        '------------------------------------------'
        Public Overrides Sub _Vitri_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Vitri_Delete", id)
        End Sub

        '------------------------------------------'


#End Region
#Region "NVCMS_Banner"

        Public Overrides Function NVCMS_Banner_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_GetAll(ByVal Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_GetAllVitri(ByVal Portalid As Integer, Vitri As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_SelectAllVitri", Portalid, Vitri)
        End Function
        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_Insert(ByVal Title As String, ByVal KieuBanner As Integer, ByVal IMGLink As String, ByVal Vitri As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Visible As Boolean, ByVal CreatedDate As DateTime, ByVal Ordernumber As Integer, ByVal Link As String, ByVal Startdate As DateTime, ByVal enddate As DateTime, ByVal Contact As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Insert", Title, KieuBanner, IMGLink, Vitri, Height, Width, PortalId, UserId, Visible, CreatedDate, Ordernumber, Link, Startdate, enddate, Contact)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_Update(ByVal id As Integer, ByVal Title As String, ByVal KieuBanner As Integer, ByVal IMGLink As String, ByVal Vitri As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Visible As Boolean, ByVal CreatedDate As DateTime, ByVal Ordernumber As Integer, ByVal Link As String, ByVal Startdate As DateTime, ByVal enddate As DateTime, ByVal Contact As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Update", id, Title, KieuBanner, IMGLink, Vitri, Height, Width, PortalId, UserId, Visible, CreatedDate, Ordernumber, Link, Startdate, enddate, Contact)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Delete", id)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_UpdateView(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_UpdateView", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_UpdateClick(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_UpdateClick", id)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_UpdateOrder(ByVal id As Integer, ByVal Ordernumber As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_UpdateOrder", id, Ordernumber)
        End Sub
        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_GetAllShow(ByVal Portalid As Integer, ByVal Vitri As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_SelectAllShow", Portalid, Vitri)
        End Function

        '------------------------------------------'
#End Region
#Region "NVCMS_Banner_Template"

        Public Overrides Function NVCMS_Banner_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_Template_SelectByID", Id, Portalid)
        End Function

        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_Template_SelectAll(ByVal Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_Template_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Template_Insert", TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Template_Update", Id, TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_Template_Delete(ByVal Id As Integer, Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Template_Delete", Id, Portalid)
        End Sub

        '------------------------------------------'


#End Region
#Region "NVCMS_Banner_Static"
        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_Static_GetAllByBanner(bannerid As Integer, isclick As Boolean) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_Static_SelectAllByBanner", bannerid, isclick)
        End Function

        '------------------------------------------'
        Public Overrides Sub NVCMS_Banner_Static_Insert(ByVal BannerId As Integer, ByVal IP As String, ByVal Createdate As DateTime, ByVal isclick As Boolean)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Banner_Static_Insert", BannerId, IP, Createdate, isclick)
        End Sub
        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_Static_SeletCount(ByVal datefrom As Date, ByVal dateto As Date, ByVal bannerid As Integer, ByVal Ip As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Banner_Static_SeletCount", datefrom, dateto, bannerid, Ip)
        End Function
        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_Static_SeletIndex(ByVal datefrom As Date, ByVal dateto As Date, ByVal bannerid As Integer, ByVal Ip As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Banner_Static_SeletIndex", datefrom, dateto, bannerid, Ip, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function NVCMS_Banner_Static_SeletCountDate(ByVal Createdate As Date, ByVal bannerid As Integer, ByVal Ip As String) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Banner_Static_SeletCountDate", Createdate, bannerid, Ip)
        End Function

#End Region
#End Region


    End Class

End Namespace