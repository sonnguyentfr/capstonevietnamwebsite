'
' DotNetNuke® - http://www.dotnetnuke.com
' Copyright (c) 2002-2006

Imports DotNetNuke

Namespace NVCMS.Modules.Marketing

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
            objProvider = CType(Framework.Reflection.CreateObject("data", "NVCMS.Modules.Marketing", ""), DataProvider)
        End Sub

        ' return the provider
        Public Shared Shadows Function Instance() As DataProvider
            Return objProvider
        End Function

#End Region

#Region "Abstract methods"

#Region "Marketing_Mail_Account"

        Public MustOverride Function Marketing_Mail_Account_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function Marketing_Mail_Account_GetAll() As IDataReader

        Public MustOverride Sub Marketing_Mail_Account_Insert(ByVal Name As String, ByVal Mail As String, ByVal Password As String, ByVal UserId As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub Marketing_Mail_Account_Delete(ByVal id As Integer)

        Public MustOverride Sub Marketing_Mail_Account_Update(ByVal id As Integer, ByVal Name As String, ByVal Mail As String, ByVal Password As String, ByVal UserId As Integer, ByVal PortalId As Integer)

#End Region
#Region "Marketing_Mail_Campaing"

        Public MustOverride Function Marketing_Mail_Campaing_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function Marketing_Mail_Campaing_GetAll() As IDataReader

        Public MustOverride Sub Marketing_Mail_Campaing_Insert(ByVal Title As String, ByVal Description As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub Marketing_Mail_Campaing_Delete(ByVal id As Integer)

        Public MustOverride Sub Marketing_Mail_Campaing_DeleteCampaing(ByVal CampaingId As Integer)

        Public MustOverride Sub Marketing_Mail_Campaing_Update(ByVal id As Integer, ByVal Title As String, ByVal Description As String, ByVal CreatedDate As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)

#End Region
#Region "Marketing_Mail_ListMail"

        Public MustOverride Function Marketing_Mail_ListMail_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function Marketing_Mail_ListMail_GetAll(CampaingId As Integer) As IDataReader

        Public MustOverride Sub Marketing_Mail_ListMail_Insert(ByVal CampaingId As Integer, ByVal Email As String, ByVal Status As Boolean, ByVal Datetime As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)

        Public MustOverride Sub Marketing_Mail_ListMail_Delete(ByVal id As Integer)

        Public MustOverride Sub Marketing_Mail_ListMail_Update(ByVal id As Integer, ByVal CampaingId As Integer, ByVal Email As String, ByVal Status As Boolean, ByVal Datetime As DateTime, ByVal UserId As Integer, ByVal PortalId As Integer)
        Public MustOverride Sub Marketing_Mail_ListMail_UpdateCountSend(ByVal id As String)

#End Region
#Region "Marketing_Mail_Template"
        Public MustOverride Function Marketing_Mail_Template_GetByID(ByVal Id As Integer, Portalid As Integer) As IDataReader
        Public MustOverride Function Marketing_Mail_Template_SelectAll(ByVal PortalId As Integer) As IDataReader
        Public MustOverride Sub Marketing_Mail_Template_Insert(ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

        Public MustOverride Sub Marketing_Mail_Template_Delete(ByVal Id As Integer, Portalid As Integer)

        Public MustOverride Sub Marketing_Mail_Template_Update(ByVal Id As Integer, ByVal TemplateName As String, ByVal FilePath As String, ByVal PortalId As Integer)

#End Region
#Region "Marketing_Mail_ListMail_Unsub"

        Public MustOverride Function Marketing_Mail_ListMail_Unsub_GetByID(ByVal id As Integer) As IDataReader

        Public MustOverride Function Marketing_Mail_ListMail_Unsub_GetAll() As IDataReader

        Public MustOverride Sub Marketing_Mail_ListMail_Unsub_Insert(ByVal objInfo As Marketing_Mail_ListMailUnsubInfo)

        Public MustOverride Sub Marketing_Mail_ListMail_Unsub_Delete(ByVal id As Integer)

        Public MustOverride Sub Marketing_Mail_ListMail_Unsub_Update(ByVal objInfo As Marketing_Mail_ListMailUnsubInfo)

#End Region
#Region "Marketing_Mail_Campaign_Send"

        Public MustOverride Function Marketing_Mail_Campaign_Send_Insert(ByVal campaignId As Integer, ByVal subject As String, ByVal body As String, ByVal totalRecipient As Integer, ByVal createdDate As DateTime) As Integer

        Public MustOverride Function Marketing_Mail_Campaign_Send_GetByID(ByVal id As Integer) As IDataReader

#End Region
#Region "Marketing_Mail_Send_Log"

        Public MustOverride Function Marketing_Mail_Send_Log_Insert(ByVal campaignSendId As Integer, ByVal listMailId As Integer, ByVal email As String, ByVal createdDate As DateTime) As Integer

        Public MustOverride Function Marketing_Mail_Send_Log_GetByCampaignSendId(ByVal campaignSendId As Integer, ByVal status As String, ByVal email As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal sortBy As String, ByVal sortDirection As String) As IDataReader

        Public MustOverride Function Marketing_Mail_Send_Log_GetStatistics(ByVal campaignSendId As Integer) As IDataReader

        Public MustOverride Function Marketing_Mail_Send_Log_GetStatusDistribution(ByVal campaignSendId As Integer) As IDataReader

#End Region
#Region "Marketing_Static"

        Public MustOverride Function Marketing_Mail_Campaign_Analytics(
            CampaignId As Integer
            ) As IDataReader


#End Region
#End Region


    End Class

End Namespace