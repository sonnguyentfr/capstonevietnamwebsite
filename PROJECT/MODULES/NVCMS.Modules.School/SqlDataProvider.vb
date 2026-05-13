Imports System.Text
Imports DotNetNuke
Imports DotNetNuke.Common.Utilities
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.School

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
#Region "Cap_Marketing_Truong"

        Public Overrides Function Marketing_Truong_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Marketing_Truong_SelectByID", id)
        End Function
        '------------------------------------------'
        Public Overrides Sub Marketing_Truong_Insert(ByVal objInfo As MarketingSchoolInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Marketing_Truong_Insert", objInfo.CODE, objInfo.NameofSchool, objInfo.Address, objInfo.Logo, objInfo.Logodesign, objInfo.LogoLink, objInfo.Conver, objInfo.ConverLink, objInfo.VideoLink, objInfo.Descreption, objInfo.DescreptionEN, objInfo.Namthanhlap, objInfo.Website, objInfo.Email, objInfo.Phone, objInfo.Social, objInfo.ThanhPholongannhat, objInfo.ThanhPholongannhatEN, objInfo.Vitri, objInfo.Loaitruongtext, objInfo.LoaitruongtextEN, objInfo.Kiemdinh, objInfo.KiemdinhEN, objInfo.TypeofRanking, objInfo.TypeofRankingVN, objInfo.Loai, objInfo.ProgramOfered, objInfo.MinimumAgeRequirement, objInfo.MinimumGradeRequirement, objInfo.MinimumGradeRequirementOther, objInfo.SingleSex, objInfo.Indirect, objInfo.OrganizationId, objInfo.Country, objInfo.StateCity, objInfo.Info, objInfo.InfoEN, objInfo.Status, objInfo.CreatedDate, objInfo.isSubAgent)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Truong_Update(ByVal objInfo As MarketingSchoolInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Marketing_Truong_Update", objInfo.id, objInfo.Tomtat, objInfo.TomtatEN, objInfo.DescreptionWebsite, objInfo.DescreptionWebsiteEN, objInfo.Logo, objInfo.LogoLink, objInfo.Conver, objInfo.VideoLink, objInfo.Social)
        End Sub

        '------------------------------------------'
        Public Overrides Sub Marketing_Truong_Delete(ByVal id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Marketing_Truong_Delete", id)
        End Sub
        '------------------------------------------'
        Public Overrides Function Marketing_Truong_Search_Find_Count(ByVal NameofSchool As String, Country As Integer, Loai As Integer, ChiPhi As String, Major As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Cap_Marketing_Truong_Search_Find_Count", NameofSchool, Country, Loai, ChiPhi, Major)
        End Function
        '------------------------------------------'
        Public Overrides Function Marketing_Truong_Search_Find_Index(ByVal NameofSchool As String, Country As Integer, Loai As Integer, ChiPhi As String, Major As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Cap_Marketing_Truong_Search_Find_Index", NameofSchool, Country, Loai, ChiPhi, Major, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function Marketing_Truong_Find_Count(ByVal NameofSchool As String, ByVal Website As String, Loai As Integer, Country As Integer, StateCity As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Cap_Marketing_Truong_Find_Count", NameofSchool, GetSqlTypeString(NameofSchool), Website, Loai, Country, StateCity)
        End Function
        '------------------------------------------'
        Public Overrides Function Marketing_Truong_Find_Index(ByVal NameofSchool As String, ByVal Website As String, Loai As Integer, Country As Integer, StateCity As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Cap_Marketing_Truong_Find_Index", NameofSchool, GetSqlTypeString(NameofSchool), Website, Loai, Country, StateCity, PageIndex, PageSize), IDataReader)
        End Function
        '------------------------------------------'
        Public Overrides Function Marketing_Truong_Find_RandomByLoaiCountry(ByVal Top As Integer, Loai As Integer, Country As Integer) As IDataReader
            Return CType(SqlHelper.ExecuteReader(ConnectionString, "Cap_Marketing_Truong_SelectRanDom", Top, Loai, Country), IDataReader)
        End Function

#End Region

#Region "Cap_Truong_Admis_4Year"

        Public Overrides Function Admis_4Year_GetByTruongID(ByVal Truongid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Truong_Admis_4Year_SelectByTruongID", Truongid)
        End Function
        '------------------------------------------'
#End Region
#Region "Cap_Truong_Admis_BF"

        Public Overrides Function Admis_BF_GetByTruongID(ByVal Truongid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Truong_Admis_BF_SelectByTruongID", Truongid)
        End Function
        '------------------------------------------'
#End Region
#Region "Cap_Truong_TruongMajor"
        Public Overrides Function TruongMajor_GetCountAllByTruong(ByVal TruongId As Integer) As Integer
            Return SqlHelper.ExecuteScalar(ConnectionString, "Cap_Truong_TruongMajor_SelectAllByTruong", TruongId)
        End Function
#End Region
#Region "Cap_Truong_TruongMajor"
        Public Overrides Function Major_GetAll() As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Truong_Major_SelectAll")
        End Function
        Public Overrides Function Major_GetAllByAlphaBet(ByVal key As String) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Truong_Major_SelectAlByAlphaBetl", key)
        End Function
#End Region
#Region "Cap_Marketing_Truong_Version"
        Public Overrides Sub Marketing_Truong_Version_Insert(ByVal objInfo As MarketingSchoolInfo)
            SqlHelper.ExecuteNonQuery(ConnectionString, "Cap_Marketing_Truong_Version_Insert", objInfo.Truongid, objInfo.CODE, objInfo.NameofSchool, objInfo.Tomtat, objInfo.Logo, objInfo.Conver, objInfo.VideoLink, objInfo.Descreption, objInfo.Website, objInfo.Email, objInfo.Phone, objInfo.Social, objInfo.Loaitruongtext, objInfo.Kiemdinh, objInfo.TypeofRankingVN, objInfo.Loai, objInfo.SingleSex, objInfo.Info, objInfo.CreatedDate, objInfo.UserId)
        End Sub
        Public Overrides Function Marketing_Truong_Version_GetAllByTruong(ByVal Truongid As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Marketing_Truong_Version_SelectAllByTruong", Truongid)
        End Function
        Public Overrides Function Marketing_Truong_Version_GetByID(ByVal id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "Cap_Marketing_Truong_Version_SelectByID", id)
        End Function
#End Region
#Region "Cap_NewsBySchool"
        Public Overrides Sub NewsBySchool_Insert(ByVal NewId As Integer, ByVal SchoolId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsBySchool_Insert", NewId, SchoolId)
        End Sub
        Public Overrides Function NewsBySchool_Update(ByVal Id As Integer, ByVal NewId As Integer, ByVal SchoolId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsBySchool_Update", Id, NewId, SchoolId)
        End Function
        Public Overrides Function NewsBySchool_GetByNewID(ByVal NewId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsBySchool_GetByNewID", NewId)
        End Function
        Public Overrides Function NewsBySchool_GetShowNewID(ByVal Count As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsBySchool_GetShowNewID", Count)
        End Function
        Public Overrides Function NewsBySchool_GetByID(ByVal Id As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "NewsBySchool_GetByID", Id)
        End Function
        Public Overrides Sub NewsBySchool_Delete(ByVal Id As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsBySchool_Delete", Id)
        End Sub
        Public Overrides Sub NewsBySchool_DeleteByNewId(ByVal NewId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "NewsBySchool_DeleteByNewId", NewId)
        End Sub
#End Region
#Region "MarketingSchoolTemplate"
        Public Overrides Function MarketingSchoolTemplate_Get(ByVal PortalID As Integer, ByVal TemplateId As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "MarketingSchoolTemplate_Get", PortalID, TemplateId)
        End Function
        Public Overrides Function MarketingSchoolTemplate_GetAll(ByVal PortalID As Integer) As IDataReader
            Return SqlHelper.ExecuteReader(ConnectionString, "MarketingSchoolTemplate_GetAll", PortalID)
        End Function
        Public Overrides Sub MarketingSchoolTemplate_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalID As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "MarketingSchoolTemplate_Add", TemplateName, FilePath, PortalID)
        End Sub
        Public Overrides Sub MarketingSchoolTemplate_Delete(ByVal TemplateId As Integer)
            SqlHelper.ExecuteNonQuery(ConnectionString, "MarketingSchoolTemplate_Delete", TemplateId)
        End Sub
        Public Overrides Sub MarketingSchoolTemplate_Update(ByVal TemplateId As Integer, ByVal TemplateName As String, ByVal FilePath As String)
            SqlHelper.ExecuteNonQuery(ConnectionString, "MarketingSchoolTemplate_Update", TemplateId, TemplateName, FilePath)
        End Sub
#End Region
#End Region


    End Class

End Namespace