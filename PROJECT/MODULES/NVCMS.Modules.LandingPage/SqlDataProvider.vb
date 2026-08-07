
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.LadingPage

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
#Region "NVCMS_LadingPage"

        Public Overrides Function LadingPage_GetByID(ByVal id As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_LadingPage_SelectByID", id, PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function LadingPage_GetAll(ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_LadingPage_SelectAll", PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function LadingPage_GetAllByParentId(ByVal ParentId As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_LadingPage_SelectAllByParentId", ParentId, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Sub LadingPage_Insert(ByVal obj As LadingPage_Info)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Insert", obj.TrangDanhMuc, obj.Tieudephu, obj.ImagePath, obj.diadiem, obj.tomtat, obj.Noidung, obj.NoidungFile, obj.Link, obj.ParentId, obj.Ordernumber, obj.isActive, obj.PortalId, obj.CreatedDate, obj.UserId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub LadingPage_Update(ByVal obj As LadingPage_Info)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Update", obj.id, obj.TrangDanhMuc, obj.Tieudephu, obj.ImagePath, obj.diadiem, obj.tomtat, obj.Noidung, obj.NoidungFile, obj.Link, obj.ParentId, obj.Ordernumber, obj.isActive, obj.ModifyDate, obj.UserIdModify)
        End Sub

        '------------------------------------------'
        Public Overrides Sub LadingPage_Delete(ByVal id As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Delete", id, PortalId)
        End Sub

        '------------------------------------------'


#End Region
#Region "NVCMS_LadingPage_Template"

        Public Overrides Function NVCMS_LadingPage_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_LadingPage_Template_SelectByID", Id, Portalid)
        End Function

        '------------------------------------------'
        Public Overrides Function NVCMS_LadingPage_Template_SelectAll(ByVal Portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_LadingPage_Template_SelectAll", Portalid)
        End Function
        '------------------------------------------'
        Public Overrides Sub NVCMS_LadingPage_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Template_Insert", TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_LadingPage_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Template_Update", Id, TemplateName, FilePath, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_LadingPage_Template_Delete(ByVal Id As Integer, Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Template_Delete", Id, Portalid)
        End Sub

        '------------------------------------------'


#End Region
#Region "NVCMS_LadingPage_Media"

        Public Overrides Function NVCMS_LadingPage_Media_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_LadingPage_Media_SelectByID", id)
        End Function

        '------------------------------------------'
        Public Overrides Function NVCMS_LadingPage_Media_GetAll(ByVal TrangLadingPageId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_LadingPage_Media_SelectAll", TrangLadingPageId)
        End Function

        '------------------------------------------'
        Public Overrides Sub NVCMS_LadingPage_Media_Insert(ByVal TrangLadingPageId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Media_Insert", TrangLadingPageId, Title, Descreption, MediaLnk, Ordernumber, PortalId)
        End Sub

        '------------------------------------------'
        Public Overrides Sub NVCMS_LadingPage_Media_Update(ByVal id As Integer, ByVal TrangLadingPageId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Media_Update", id, TrangLadingPageId, Title, Descreption, MediaLnk, Ordernumber, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NVCMS_LadingPage_Media_UpdateTitle(ByVal id As Integer, ByVal Title As String, ByVal Descreption As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Media_UpdateTitle", id, Title, Descreption)
        End Sub
        '------------------------------------------'
        Public Overrides Sub NVCMS_LadingPage_Media_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_LadingPage_Media_Delete", id)
        End Sub

        '------------------------------------------'


#End Region
#End Region


    End Class

End Namespace