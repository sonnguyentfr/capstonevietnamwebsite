
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke

Namespace NVCMS.Modules.Form

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

#Region "NVCMS_Form"

        Public Overrides Function Form_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Form_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function Form_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Form_SelectAll")
        End Function

        '------------------------------------------'
        Public Overrides Sub Form_Insert(ByVal objform As Form_Info)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Form_Insert", objform.Type, objform.hinhthuc, objform.vanphong, objform.title, objform.noidung, objform.hovaten, objform.email, objform.sodienthoai, objform.diachi, objform.status, objform.creatdate, objform.portalid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Form_Update(ByVal objform As Form_Info)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Form_Update", objform.id, objform.Type, objform.hinhthuc, objform.vanphong, objform.title, objform.noidung, objform.hovaten, objform.email, objform.sodienthoai, objform.diachi, objform.status, objform.creatdate, objform.portalid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Form_Update_Traloi(ByVal id As Integer, ByVal status As String, ByVal repuserid As Integer, ByVal repcreateddate As DateTime, ByVal reptitle As String, ByVal repnoidung As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Form_Update_Traloi", id, status, repuserid, repcreateddate, reptitle, repnoidung)
        End Sub
        '------------------------------------------'
        Public Overrides Sub Form_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Form_Delete", id)
        End Sub
        '------------------------------------------'
        Public Overrides Function Form_Find_Count(subtractIds As String, ByVal Type As String, datefrom As DateTime, dateto As DateTime, ByVal noidung As String, ByVal Status As String, ByVal PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Form_FindCount", subtractIds, Type, datefrom, dateto, noidung, Status, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Form_Find_Index(subtractIds As String, Type As String, datefrom As DateTime, dateto As DateTime, ByVal noidung As String, ByVal Status As String, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Form_FindIndex", subtractIds, Type, datefrom, dateto, noidung, Status, PortalId, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'


#End Region
#Region "Form_rep"
        '------------------------------------------'
        Public Overrides Function Form_Rep_GetAll(ByVal FormId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Form_Rep_SelectAll", FormId)
        End Function

        '------------------------------------------'
        Public Overrides Sub Form_Rep_Insert(ByVal FormId As Integer, ByVal repuserid As Integer, ByVal repcreateddate As DateTime, ByVal reptitle As String, ByVal repnoidung As String, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Form_Rep_Insert", FormId, repuserid, repcreateddate, reptitle, repnoidung, portalid)
        End Sub
#End Region
#End Region


    End Class

End Namespace