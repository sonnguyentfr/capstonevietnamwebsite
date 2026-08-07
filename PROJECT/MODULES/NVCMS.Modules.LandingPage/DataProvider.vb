Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.LadingPage

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.LadingPage", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "NVCMS_LadingPage"

        Public MustOverride Function LadingPage_GetByID(ByVal id As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function LadingPage_GetAll(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function LadingPage_GetAllByParentId(ByVal ParentId As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Sub LadingPage_Insert(ByVal obj As LadingPage_Info)

        Public MustOverride Sub LadingPage_Delete(ByVal id As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub LadingPage_Update(ByVal obj As LadingPage_Info)


#End Region
#Region "NVCMS_LadingPage_Template"

        Public MustOverride Function NVCMS_LadingPage_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
        Public MustOverride Function NVCMS_LadingPage_Template_SelectAll(ByVal PortalId As Integer) As IDataReader
        Public MustOverride Sub NVCMS_LadingPage_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_LadingPage_Template_Delete(ByVal Id As Integer, Portalid As Integer)

        Public MustOverride Sub NVCMS_LadingPage_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

#End Region
#Region "NVCMS_LadingPage_Media"

        Public MustOverride Function NVCMS_LadingPage_Media_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function NVCMS_LadingPage_Media_GetAll(ByVal TrangLadingPageId As Integer) As IDataReader

        Public MustOverride Sub NVCMS_LadingPage_Media_Insert(ByVal TrangLadingPageId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_LadingPage_Media_Delete(ByVal id As Integer)

        Public MustOverride Sub NVCMS_LadingPage_Media_Update(ByVal id As Integer, ByVal TrangLadingPageId As Integer, ByVal Title As String, ByVal Descreption As String, ByVal MediaLnk As String, ByVal Ordernumber As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_LadingPage_Media_UpdateTitle(ByVal id As Integer, ByVal Title As String, ByVal Descreption As String)

#End Region
#End Region


    End Class

End Namespace