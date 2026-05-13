Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.FairGuide

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.FairGuide", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "NVCMS_Fairguide"

        Public MustOverride Function Fairguide_GetByID(ByVal id As Integer, ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function Fairguide_GetAll(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function Fairguide_Insert(ByVal Title As String, ByVal Avatar As String, ByVal Descreption As String, ByVal Noidung As String, ByVal Ordernumber As Integer, ByVal IsActive As Boolean, ByVal Createddate As DateTime, ByVal sizewidth As Integer, ByVal sizeheight As Integer, ByVal UserId As Integer, ByVal Portalid As Integer) As Integer

        Public MustOverride Sub Fairguide_Delete(ByVal id As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub Fairguide_Update(ByVal id As Integer, ByVal Title As String, ByVal Avatar As String, ByVal Descreption As String, ByVal Noidung As String, ByVal Ordernumber As Integer, ByVal IsActive As Boolean, ByVal Createddate As DateTime, ByVal sizewidth As Integer, ByVal sizeheight As Integer, ByVal UserId As Integer, ByVal Portalid As Integer)
        Public MustOverride Function Fairguide_Find_Count(subtractIds As String, ByVal Title As String, ByVal PortalId As Integer) As Integer
        Public MustOverride Function Fairguide_Find_Index(subtractIds As String, ByVal Title As String, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
#End Region
#Region "FairGuideByMedia"

        Public MustOverride Function FairGuideByMedia_GetByID(ByVal id As Integer, ByVal portalid As Integer) As IDataReader

        Public MustOverride Function FairGuideByMedia_GetAllByFairGuideId(ByVal FairGuideId As Integer, ByVal portalid As Integer) As IDataReader

        Public MustOverride Sub FairGuideByMedia_Insert(ByVal FairGuideId As Integer, ByVal mediaid As Integer, ByVal ordernumber As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub FairGuideByMedia_Delete(ByVal id As Integer, ByVal portalid As Integer)

        Public MustOverride Sub FairGuideByMedia_DeleteByFairGuideId(ByVal FairGuideIdid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub FairGuideByMedia_DeleteByMediaId(ByVal Mediaid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub FairGuideByMedia_Update(ByVal id As Integer, ByVal FairGuideId As Integer, ByVal mediaid As Integer, ByVal ordernumber As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub FairGuideByMedia_UpdateFairGuideId(ByVal FairGuideId As Integer, FairGuideIdnew As Integer)

#End Region
#End Region


    End Class

End Namespace