'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006

Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.Banner

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.Banner", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "NV_Videos"
#Region "NVCMS_Banner_Vitri"

        Public MustOverride Function _Vitri_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function _Vitri_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Sub _Vitri_Insert(ByVal Title As String, ByVal width As Integer, ByVal height As Integer, ByVal Images As String, ByVal CreatedByUserId As Integer, ByVal CreatedOnDate As DateTime, ByVal LastModifiedByUserId As Integer, ByVal LastModifiedOnDate As DateTime, ByVal ModuleId As Integer, ByVal portalid As Integer)

        Public MustOverride Sub _Vitri_Delete(ByVal id As Integer)

        Public MustOverride Sub _Vitri_Update(ByVal id As Integer, ByVal Title As String, ByVal width As Integer, ByVal height As Integer, Images As String, ByVal LastModifiedByUserId As Integer, ByVal LastModifiedOnDate As DateTime)

#End Region
#Region "NV_BannNVCMS_Banner"

        Public MustOverride Function NVCMS_Banner_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function NVCMS_Banner_GetAll(ByVal PortalId As Integer) As IDataReader

        Public MustOverride Function NVCMS_Banner_GetAllVitri(ByVal PortalId As Integer, vitri As Integer) As IDataReader

        Public MustOverride Sub NVCMS_Banner_Insert(ByVal Title As String, ByVal KieuBanner As Integer, ByVal IMGLink As String, ByVal Vitri As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Visible As Boolean, ByVal CreatedDate As DateTime, ByVal Ordernumber As Integer, ByVal Link As String, ByVal Startdate As DateTime, ByVal enddate As DateTime, ByVal Contact As String)

        Public MustOverride Sub NVCMS_Banner_Delete(ByVal id As Integer)

        Public MustOverride Sub NVCMS_Banner_Update(Id As Integer, ByVal Title As String, ByVal KieuBanner As Integer, ByVal IMGLink As String, ByVal Vitri As Integer, ByVal Height As Integer, ByVal Width As Integer, ByVal PortalId As Integer, ByVal UserId As Integer, ByVal Visible As Boolean, ByVal CreatedDate As DateTime, ByVal Ordernumber As Integer, ByVal Link As String, ByVal Startdate As DateTime, ByVal enddate As DateTime, ByVal Contact As String)

        Public MustOverride Sub NVCMS_Banner_UpdateView(ByVal id As Integer)
        Public MustOverride Sub NVCMS_Banner_UpdateClick(ByVal id As Integer)
        Public MustOverride Sub NVCMS_Banner_UpdateOrder(ByVal id As Integer, ByVal Ordernumber As Integer)
        Public MustOverride Function NVCMS_Banner_GetAllShow(ByVal PortalId As Integer, ByVal vitri As Integer) As IDataReader

#End Region
#Region "NVCMS_Banner_Template"

        Public MustOverride Function NVCMS_Banner_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
        Public MustOverride Function NVCMS_Banner_Template_SelectAll(ByVal PortalId As Integer) As IDataReader
        Public MustOverride Sub NVCMS_Banner_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

        Public MustOverride Sub NVCMS_Banner_Template_Delete(ByVal Id As Integer, Portalid As Integer)

        Public MustOverride Sub NVCMS_Banner_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

#End Region
#Region "NVCMS_Banner_Static"

        Public MustOverride Function NVCMS_Banner_Static_GetAllByBanner(BannerId As Integer, ByVal isclick As Boolean) As IDataReader
        Public MustOverride Sub NVCMS_Banner_Static_Insert(ByVal BannerId As Integer, ByVal IP As String, ByVal Createdate As DateTime, ByVal isclick As Boolean)
        Public MustOverride Function NVCMS_Banner_Static_SeletCount(ByVal datefrom As Date, ByVal dateto As Date, ByVal Bannerid As Integer, ByVal IP As String) As Integer

        Public MustOverride Function NVCMS_Banner_Static_SeletIndex(ByVal datefrom As Date, ByVal dateto As Date, ByVal Bannerid As Integer, ByVal IP As String, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader
        Public MustOverride Function NVCMS_Banner_Static_SeletCountDate(ByVal createdate As Date, ByVal Bannerid As Integer, ByVal IP As String) As Integer
#End Region
#End Region

#End Region


    End Class

End Namespace