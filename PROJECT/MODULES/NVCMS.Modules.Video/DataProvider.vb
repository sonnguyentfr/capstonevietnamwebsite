Imports System
Imports DotNetNuke

Namespace NVCMS.Modules.Video

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.Video", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"
#Region "videosclip"

        Public MustOverride Function Videos_Insert(ByVal objNews As Videos_Info) As Integer
        Public MustOverride Function Videos_GetByID(ByVal id As Integer, ByVal PortalId As Integer) As IDataReader
        Public MustOverride Function Admin_Find_Count(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer) As Integer
        Public MustOverride Function Admin_Find_Index(ByVal datefrom As Date, ByVal dateto As Date, ByVal title As String, ByVal categoryid As Integer, ByVal PortalId As Integer, ByVal Status As Integer, ByVal UserId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal sapxep As String) As IDataReader
        Public MustOverride Sub Videos_Update(ByVal objNews As Videos_Info)
        Public MustOverride Sub Videos_UpdateStatus(ByVal VideoId As Integer, Status As Integer, UserId As Integer)
        Public MustOverride Sub Videos_UpdatePublishedDate(ByVal id As Integer, PublicDate As DateTime, UserId As Integer)

        Public MustOverride Function Videos_Find_Show_Count(ByVal PortalId As Integer) As Integer
        Public MustOverride Function Videos_Find_Show_Index(ByVal PortalId As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer) As IDataReader


#End Region
#Region "NVCMS_VideoByMedia"

        Public MustOverride Function VideoByMedia_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function VideoByMedia_GetAllByvideoid(ByVal videoid As Integer) As IDataReader
        Public MustOverride Sub VideoByMedia_Insert(ByVal videoid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub VideoByMedia_Delete(ByVal id As Integer)

        Public MustOverride Sub VideoByMedia_DeleteByvideoid(ByVal videoidid As Integer, ByVal Portalid As Integer)

        Public MustOverride Sub VideoByMedia_DeleteByMediaId(ByVal Mediaid As Integer)

        Public MustOverride Sub VideoByMedia_Update(ByVal id As Integer, ByVal videoid As Integer, ByVal mediaid As Integer, ByVal createdted As DateTime, ByVal userid As Integer, ByVal portalid As Integer)

        Public MustOverride Sub VideoByMedia_Updatevideoid(ByVal videoid As Integer, videoidnew As Integer)

#End Region
#Region "Video_Process"
        Public MustOverride Function Video_Process_GetById(ByVal ID As Integer) As IDataReader

        Public MustOverride Function Video_Process_GetAll() As IDataReader

        Public MustOverride Function Video_Process_Insert(ByVal objInfo As VideoProcessInfo) As Integer

        Public MustOverride Sub Video_Process_Delete(ByVal ID As Integer)

        Public MustOverride Sub Video_Process_Update(ByVal objInfo As VideoProcessInfo)

        Public MustOverride Function Video_Process_GetByNewsId(ByVal newsId As Integer) As IDataReader

        Public MustOverride Function Video_Process_GetCurrentProcess(ByVal newsId As Integer) As IDataReader

        Public MustOverride Function Video_Process_GetLastProcessByStatus(ByVal newsId As Integer, ByVal status As Integer) As IDataReader

        Public MustOverride Sub Video_Process_DeleteByNewsID(ByVal newsID As Integer)
#End Region
#Region "Video_Settings"

        Public MustOverride Function Video_Settings_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function Video_Settings_GetAll(PortalId As Integer) As IDataReader

        Public MustOverride Function Video_Settings_GetAllByType(Type As Integer, Count As Integer, PortalId As Integer) As IDataReader

        Public MustOverride Sub Video_Settings_Insert(ByVal VideoId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub Video_Settings_Delete(Type As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub Video_Settings_DeleteByVideoId(ByVal VideoId As Integer, Type As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub Video_Settings_DeleteById(ByVal Id As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub Video_Settings_Update(ByVal id As Integer, ByVal VideoId As Integer, ByVal OrderNumber As Integer, ByVal Type As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub Video_Settings_UpdateOrder(ByVal id As Integer, ByVal OrderNumber As Integer)

#End Region
#Region "Video_Template"
        Public MustOverride Function Video_Template_Get(ByVal PortalID As Integer, ByVal TemplateId As Integer) As IDataReader
        Public MustOverride Function Video_Template_GetAll(ByVal PortalID As Integer) As IDataReader
        Public MustOverride Sub Video_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalID As Integer)
        Public MustOverride Sub Video_Template_Update(ByVal TemplateId As Integer, ByVal TemplateName As String, ByVal FilePath As String)
        Public MustOverride Sub Video_Template_Delete(ByVal TemplateId As Integer)
#End Region
#End Region


    End Class

End Namespace