Imports DotNetNuke
Imports Microsoft.ApplicationBlocks.Data

Namespace NVCMS.Modules.School

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' An abstract class for the data access layer
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public MustInherit Class DataProvider

#Region "Shared/Static Methods"

        ' singleton reference to the instantiated object 
        Private Shared objProvider As DataProvider = Nothing

        ' constructor
        Shared Sub New()
            CreateProvider()
        End Sub

        ' dynamically create provider
        Private Shared Sub CreateProvider()
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.School", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"
#Region "Cap_Marketing_Truong"
        Public MustOverride Function Marketing_Truong_GetByID(ByVal id As Integer) As IDataReader
        Public MustOverride Sub Marketing_Truong_Insert(ByVal objInfo As MarketingSchoolInfo)
        Public MustOverride Sub Marketing_Truong_Delete(ByVal id As Integer)
        Public MustOverride Sub Marketing_Truong_Update(ByVal objInfo As MarketingSchoolInfo)
        Public MustOverride Function Marketing_Truong_Search_Find_Count(ByVal NameofSchool As String, Country As Integer, Loai As Integer, ChiPhi As String, Major As Integer) As Integer
        Public MustOverride Function Marketing_Truong_Search_Find_Index(ByVal NameofSchool As String, Country As Integer, Loai As Integer, ChiPhi As String, Major As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Marketing_Truong_Find_Count(ByVal NameofSchool As String, ByVal Website As String, Loai As Integer, Country As Integer, StateCity As Integer) As Integer
        Public MustOverride Function Marketing_Truong_Find_Index(ByVal NameofSchool As String, ByVal Website As String, Loai As Integer, Country As Integer, StateCity As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function Marketing_Truong_Find_RandomByLoaiCountry(ByVal Top As Integer, Loai As Integer, Country As Integer) As IDataReader

#End Region

#Region "Cap_Truong_Admis_4Year"
        Public MustOverride Function Admis_4Year_GetByTruongID(ByVal TruongId As Integer) As IDataReader
#End Region
#Region "Cap_Truong_Admis_BF"
        Public MustOverride Function Admis_BF_GetByTruongID(ByVal TruongId As Integer) As IDataReader
#End Region
#Region "Cap_Truong_TruongMajor"
        Public MustOverride Function TruongMajor_GetCountAllByTruong(TruongId As Integer) As Integer
#End Region
#Region "Cap_Truong_Major"
        Public MustOverride Function Major_GetAll() As IDataReader
        Public MustOverride Function Major_GetAllByAlphaBet(ByVal key As String) As IDataReader
#End Region
#Region "Cap_Marketing_Truong_Version"
        Public MustOverride Sub Marketing_Truong_Version_Insert(ByVal objInfo As MarketingSchoolInfo)
        Public MustOverride Function Marketing_Truong_Version_GetAllByTruong(ByVal Truongid As Integer) As IDataReader
        Public MustOverride Function Marketing_Truong_Version_GetByID(ByVal id As Integer) As IDataReader
#End Region
#Region "Cap_NewsBySchool"
        Public MustOverride Sub NewsBySchool_Insert(ByVal NewId As Integer, ByVal SchoolId As Integer)
        Public MustOverride Function NewsBySchool_Update(ByVal Id As Integer, ByVal NewId As Integer, ByVal SchoolId As Integer) As IDataReader
        Public MustOverride Function NewsBySchool_GetByNewID(ByVal NewId As Integer) As IDataReader
        Public MustOverride Function NewsBySchool_GetShowNewID(ByVal Count As Integer) As IDataReader
        Public MustOverride Function NewsBySchool_GetByID(ByVal Id As Integer) As IDataReader
        Public MustOverride Sub NewsBySchool_Delete(ByVal Id As Integer)
        Public MustOverride Sub NewsBySchool_DeleteByNewId(ByVal NewId As Integer)
#End Region
#Region "MarketingSchoolTemplate"
        Public MustOverride Function MarketingSchoolTemplate_Get(ByVal PortalID As Integer, ByVal TemplateId As Integer) As IDataReader
        Public MustOverride Function MarketingSchoolTemplate_GetAll(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Sub MarketingSchoolTemplate_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalID As Integer)
        Public MustOverride Sub MarketingSchoolTemplate_Update(ByVal TemplateId As Integer, ByVal TemplateName As String, ByVal FilePath As String)
        Public MustOverride Sub MarketingSchoolTemplate_Delete(ByVal TemplateId As Integer)
#End Region
#End Region


    End Class

End Namespace