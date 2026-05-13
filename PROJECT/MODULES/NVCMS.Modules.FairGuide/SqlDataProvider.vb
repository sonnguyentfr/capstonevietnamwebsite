
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke

Namespace NVCMS.Modules.FairGuide

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


#Region "NVCMS_Fairguide"

        Public Overrides Function Fairguide_GetByID(ByVal id As Integer, ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Fairguide_SelectByID", id, PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function Fairguide_GetAll(ByVal PortalId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Fairguide_SelectAll", PortalId)
        End Function

        '------------------------------------------'
        Public Overrides Function Fairguide_Insert(ByVal Title As String, ByVal Avatar As String, ByVal Descreption As String, ByVal Noidung As String, ByVal Ordernumber As Integer, ByVal IsActive As Boolean, ByVal Createddate As DateTime, ByVal sizewidth As Integer, ByVal sizeheight As Integer, ByVal UserId As Integer, ByVal Portalid As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Fairguide_Insert", Title, Avatar, Descreption, Noidung, Ordernumber, IsActive, Createddate, sizewidth, sizeheight, UserId, Portalid)
        End Function

        '------------------------------------------'
        Public Overrides Sub Fairguide_Update(ByVal id As Integer, ByVal Title As String, ByVal Avatar As String, ByVal Descreption As String, ByVal Noidung As String, ByVal Ordernumber As Integer, ByVal IsActive As Boolean, ByVal Createddate As DateTime, ByVal sizewidth As Integer, ByVal sizeheight As Integer, ByVal UserId As Integer, ByVal Portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Update", id, Title, Avatar, Descreption, Noidung, Ordernumber, IsActive, Createddate, sizewidth, sizeheight, UserId, Portalid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Fairguide_Delete(ByVal id As Integer, ByVal PortalId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Delete", id, PortalId)
        End Sub
        '------------------------------------------'
        Public Overrides Function Fairguide_Find_Count(subtractIds As String, ByVal Title As String, ByVal PortalId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "NVCMS_Fairguide_FindCount", subtractIds, Title, PortalId)
        End Function
        '------------------------------------------'
        Public Overrides Function Fairguide_Find_Index(subtractIds As String, ByVal Title As String, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Fairguide_FindIndex", subtractIds, Title, PortalId, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'


#End Region
#Region "NVCMS_Fairguide_Media"

        Public Overrides Function FairGuideByMedia_GetByID(ByVal id As Integer, ByVal portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Fairguide_Media_SelectByID", id, portalid)
        End Function

        '------------------------------------------'
        Public Overrides Function FairGuideByMedia_GetAllByFairGuideId(FairGuideId As Integer, ByVal portalid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NVCMS_Fairguide_Media_SelectAllByFairGuideId", FairGuideId, portalid)
        End Function
        '------------------------------------------'
        Public Overrides Sub FairGuideByMedia_Insert(ByVal FairGuideId As Integer, ByVal mediaid As Integer, ByVal ordernumber As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Media_Insert", FairGuideId, mediaid, ordernumber, createdted, userid, portalid)
        End Sub

        '------------------------------------------'
        Public Overrides Sub FairGuideByMedia_Update(ByVal id As Integer, ByVal FairGuideId As Integer, ByVal mediaid As Integer, ByVal ordernumber As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Media_Update", id, FairGuideId, mediaid, ordernumber, createdted, userid, portalid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub FairGuideByMedia_UpdateFairGuideId(ByVal FairGuideId As Integer, FairGuideIdnew As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Media_UpdateFairGuideId", FairGuideId, FairGuideIdnew)
        End Sub
        '------------------------------------------'
        Public Overrides Sub FairGuideByMedia_Delete(ByVal id As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Media_Delete", id, portalid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub FairGuideByMedia_DeleteByFairGuideId(ByVal FairGuideId As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Media_DeleteByFairGuideId", FairGuideId, portalid)
        End Sub
        '------------------------------------------'
        Public Overrides Sub FairGuideByMedia_DeleteByMediaId(ByVal Mediaid As Integer, ByVal portalid As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NVCMS_Fairguide_Media_DeleteByMediaid", Mediaid, portalid)
        End Sub
        '------------------------------------------'


#End Region
#End Region


    End Class

End Namespace