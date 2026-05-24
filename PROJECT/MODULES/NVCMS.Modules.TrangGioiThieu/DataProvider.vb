Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.TrangGioiThieu

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.TrangGioiThieu", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "NVCMS_PageGioiThieu"

        Public MustOverride Function PageGioiThieu_GetByID(ByVal id As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function PageGioiThieu_GetAll(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function PageGioiThieu_GetAllByParentId(ByVal ParentId As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Sub PageGioiThieu_Insert(ByVal TrangDanhMuc As String, ByVal Tieudephu As String, ByVal ImagePath As String, ByVal tomtat As String, ByVal Noidung As String, ByVal Link As String, ByVal ParentId As Integer, ByVal Ordernumber As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub PageGioiThieu_Delete(ByVal id As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub PageGioiThieu_Update(ByVal id As Integer, ByVal TrangDanhMuc As String, ByVal Tieudephu As String, ByVal ImagePath As String, ByVal tomtat As String, ByVal Noidung As String, ByVal Link As String, ByVal ParentId As Integer, ByVal Ordernumber As Integer, ByVal PortalId As Integer)


#End Region
#Region "NVCMS_PageGioiThieu_Template"

        Public MustOverride Function NVCMS_PageGioiThieu_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
        Public MustOverride Function NVCMS_PageGioiThieu_Template_SelectAll(ByVal PortalId As Integer) As IDataReader
        Public MustOverride Sub NVCMS_PageGioiThieu_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_PageGioiThieu_Template_Delete(ByVal Id As Integer, Portalid As Integer)

        Public MustOverride Sub NVCMS_PageGioiThieu_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

#End Region
#Region "NVCMS_PageGioiThieu_Media"

        Public MustOverride Function NVCMS_PageGioiThieu_Media_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function NVCMS_PageGioiThieu_Media_GetAll(ByVal TrangGioiThieuId As Integer) As IDataReader

        Public MustOverride Sub NVCMS_PageGioiThieu_Media_Insert(ByVal TrangGioiThieuId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_PageGioiThieu_Media_Delete(ByVal id As Integer)

        Public MustOverride Sub NVCMS_PageGioiThieu_Media_Update(ByVal id As Integer, ByVal TrangGioiThieuId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_PageGioiThieu_Media_UpdateTitle(ByVal id As Integer, ByVal Title As String, ByVal Descreption As String)

#End Region
#End Region


    End Class

End Namespace