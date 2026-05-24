
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke

Namespace NVCMS.Modules.TrangGioiThieu

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
#Region "NVCMS_PageGioiThieu"

        Public Overrides Function PageGioiThieu_GetByID(ByVal id As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_PageGioiThieu_SelectByID", id, PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function PageGioiThieu_GetAll(ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_PageGioiThieu_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function PageGioiThieu_GetAllByParentId(ByVal ParentId As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_PageGioiThieu_SelectAllByParentId", ParentId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub PageGioiThieu_Insert(ByVal TrangDanhMuc As String, ByVal Tieudephu As String, ByVal ImagePath As String, ByVal tomtat As String, ByVal Noidung As String, ByVal Link As String, ByVal ParentId As Integer, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Insert", TrangDanhMuc, Tieudephu, ImagePath, tomtat, Noidung, Link, ParentId, Ordernumber, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub PageGioiThieu_Update(ByVal id As Integer, ByVal TrangDanhMuc As String, ByVal Tieudephu As String, ByVal ImagePath As String, ByVal tomtat As String, ByVal Noidung As String, ByVal Link As String, ByVal ParentId As Integer, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Update", id, TrangDanhMuc, Tieudephu, ImagePath, tomtat, Noidung, Link, ParentId, Ordernumber, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub PageGioiThieu_Delete(ByVal id As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Delete", id, PortalId)
        End Sub

        '------------------------------------------'


#End Region
#Region "NVCMS_PageGioiThieu_Template"

        Public Overrides Function NVCMS_PageGioiThieu_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_PageGioiThieu_Template_SelectByID", Id, Portalid)
        End Function

        '------------------------------------------'
        Public Overrides Function NVCMS_PageGioiThieu_Template_SelectAll(ByVal Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_PageGioiThieu_Template_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Sub NVCMS_PageGioiThieu_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Template_Insert", TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_PageGioiThieu_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Template_Update", Id, TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_PageGioiThieu_Template_Delete(ByVal Id As Integer, Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Template_Delete", Id, Portalid)
        End Sub

        '------------------------------------------'


#End Region
#Region "NVCMS_PageGioiThieu_Media"

        Public Overrides Function NVCMS_PageGioiThieu_Media_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_PageGioiThieu_Media_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function NVCMS_PageGioiThieu_Media_GetAll(ByVal TrangGioiThieuId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_PageGioiThieu_Media_SelectAll", TrangGioiThieuId)
        End Function

        '------------------------------------------'
        Public Overrides Sub NVCMS_PageGioiThieu_Media_Insert(ByVal TrangGioiThieuId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Media_Insert", TrangGioiThieuId, Title, Descreption, MediaLnk, Ordernumber, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_PageGioiThieu_Media_Update(ByVal id As Integer, ByVal TrangGioiThieuId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Media_Update", id, TrangGioiThieuId, Title, Descreption, MediaLnk, Ordernumber, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NVCMS_PageGioiThieu_Media_UpdateTitle(ByVal id As Integer, ByVal Title As String, ByVal Descreption As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Media_UpdateTitle", id, Title, Descreption)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NVCMS_PageGioiThieu_Media_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_PageGioiThieu_Media_Delete", id)
        End Sub

        '------------------------------------------'


#End Region
#End Region


    End Class

End Namespace