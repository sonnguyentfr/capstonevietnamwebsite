Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.Form

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.Form", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "Form"
#Region "NVCMS_Form"

        Public MustOverride Function Form_GetByID(ByVal id As Integer) As IDataReader
        Public MustOverride Function Form_GetAll() As IDataReader
        Public MustOverride Sub Form_Insert(ByVal objform As Form_Info)
        Public MustOverride Sub Form_Delete(ByVal id As Integer)
        Public MustOverride Sub Form_Update(ByVal objform As Form_Info)
        Public MustOverride Sub Form_Update_Traloi(ByVal id As Integer, ByVal status As String, ByVal repuserid As Integer, ByVal repcreateddate As DateTime, ByVal reptitle As String, ByVal repnoidung As String)
        Public MustOverride Function Form_Find_Count(subtractIds As String, Type As String, datefrom As DateTime, dateto As DateTime, ByVal noidung As String, ByVal Status As String, ByVal PortalId As Integer) As Integer
        Public MustOverride Function Form_Find_Index(subtractIds As String, Type As String, datefrom As DateTime, dateto As DateTime, ByVal noidung As String, ByVal Status As String, ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
#End Region
#Region "Form_rep"
        Public MustOverride Sub Form_Rep_Insert(ByVal FormId As Integer, ByVal repuserid As Integer, ByVal repcreateddate As DateTime, ByVal reptitle As String, ByVal repnoidung As String, ByVal portalid As Integer)
        Public MustOverride Function Form_Rep_GetAll(ByVal FormId As Integer) As IDataReader
#End Region
#End Region

#End Region


    End Class

End Namespace