Imports DotNetNuke
Imports System.Data

Namespace NVCMS.Modules.Marketing

    Public Class Mail_Campaign_SendController
        Private _dataProvider As DataProvider = DataProvider.Instance()

        Public Function GetByID(ByVal id As Integer) As Campaign_SendInfo
            Dim obj As New Campaign_SendInfo
            Dim dr As IDataReader = Nothing

            Try
                dr = _dataProvider.Marketing_Mail_Campaign_Send_GetByID(id)
                If dr.Read() Then
                    obj = FillCampaignSend(dr)
                End If
            Finally
                If Not dr Is Nothing AndAlso Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try

            Return obj
        End Function

        Public Function GetSendLogs(ByVal campaignSendId As Integer, Optional ByVal status As String = Nothing, Optional ByVal email As String = Nothing, Optional ByVal pageIndex As Integer = 0, Optional ByVal pageSize As Integer = 50, Optional ByVal sortBy As String = "CreatedDate", Optional ByVal sortDirection As String = "DESC") As SendLogResult
            Dim result As New SendLogResult()
            Dim dr As IDataReader = Nothing

            Try
                dr = _dataProvider.Marketing_Mail_Send_Log_GetByCampaignSendId(campaignSendId, status, email, pageIndex, pageSize, sortBy, sortDirection)

                While dr.Read()
                    Dim logInfo As New Mail_Send_LogInfo With {
                        .Id = CType(dr("Id"), Int16),
                        .campaignSendId = CInt(dr("CampaignSendId")),
                        .ListMailId = CInt(dr("ListMailId")),
                        .email = dr("Email").ToString(),
                        .SesMessageId = If(IsDBNull(dr("SesMessageId")), "", dr("SesMessageId").ToString()),
                        .status = dr("Status").ToString(),
                        .ErrorMessage = If(IsDBNull(dr("ErrorMessage")), "", dr("ErrorMessage").ToString()),
                        .CreatedDate = CDate(dr("CreatedDate"))
                    }

                    If Not IsDBNull(dr("SentTime")) Then logInfo.SentTime = CDate(dr("SentTime"))
                    If Not IsDBNull(dr("DeliveredTime")) Then logInfo.DeliveredTime = CDate(dr("DeliveredTime"))
                    If Not IsDBNull(dr("OpenedTime")) Then logInfo.OpenedTime = CDate(dr("OpenedTime"))
                    If Not IsDBNull(dr("ClickedTime")) Then logInfo.ClickedTime = CDate(dr("ClickedTime"))

                    result.Logs.Add(logInfo)

                    If result.TotalCount = 0 AndAlso Not IsDBNull(dr("TotalCount")) Then
                        result.TotalCount = CInt(dr("TotalCount"))
                    End If
                End While
            Finally
                If Not dr Is Nothing AndAlso Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try

            Return result
        End Function

        Public Function GetStatistics(ByVal campaignSendId As Integer) As CampaignStatistics
            Dim stats As New CampaignStatistics()
            Dim dr As IDataReader = Nothing

            Try
                dr = _dataProvider.Marketing_Mail_Send_Log_GetStatistics(campaignSendId)
                If dr.Read() Then
                    stats.TotalRecipients = CInt(dr("TotalRecipients"))
                    stats.CountSent = If(IsDBNull(dr("CountSent")), 0, CInt(dr("CountSent")))
                    stats.CountDelivered = If(IsDBNull(dr("CountDelivered")), 0, CInt(dr("CountDelivered")))
                    stats.CountOpened = If(IsDBNull(dr("CountOpened")), 0, CInt(dr("CountOpened")))
                    stats.CountClicked = If(IsDBNull(dr("CountClicked")), 0, CInt(dr("CountClicked")))
                    stats.CountBounced = If(IsDBNull(dr("CountBounced")), 0, CInt(dr("CountBounced")))
                    stats.CountComplaint = If(IsDBNull(dr("CountComplaint")), 0, CInt(dr("CountComplaint")))
                    stats.CountUnsubscribed = If(IsDBNull(dr("CountUnsubscribed")), 0, CInt(dr("CountUnsubscribed")))
                    stats.CountFailed = If(IsDBNull(dr("CountFailed")), 0, CInt(dr("CountFailed")))

                    If Not IsDBNull(dr("FirstSentTime")) Then stats.FirstSentTime = CDate(dr("FirstSentTime"))
                    If Not IsDBNull(dr("LastSentTime")) Then stats.LastSentTime = CDate(dr("LastSentTime"))
                    If Not IsDBNull(dr("AvgTimeToOpenSeconds")) Then stats.AvgTimeToOpenSeconds = CInt(dr("AvgTimeToOpenSeconds"))

                    If stats.CountDelivered > 0 Then
                        stats.OpenRate = Math.Round((stats.CountOpened / stats.CountDelivered) * 100, 2)
                        stats.ClickRate = Math.Round((stats.CountClicked / stats.CountDelivered) * 100, 2)
                        stats.UnsubscribeRate = Math.Round((stats.CountUnsubscribed / stats.CountDelivered) * 100, 2)
                    End If

                    If stats.CountSent > 0 Then
                        stats.BounceRate = Math.Round((stats.CountBounced / stats.CountSent) * 100, 2)
                    End If
                End If
            Finally
                If Not dr Is Nothing AndAlso Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try

            Return stats
        End Function

        Public Function GetStatusDistribution(ByVal campaignSendId As Integer) As List(Of StatusDistribution)
            Dim result As New List(Of StatusDistribution)()
            Dim dr As IDataReader = Nothing

            Try
                dr = _dataProvider.Marketing_Mail_Send_Log_GetStatusDistribution(campaignSendId)
                While dr.Read()
                    Dim dist As New StatusDistribution With {
                        .Status = dr("Status").ToString(),
                        .Count = CInt(dr("Count")),
                        .Percentage = CDec(dr("Percentage"))
                    }
                    result.Add(dist)
                End While
            Finally
                If Not dr Is Nothing AndAlso Not dr.IsClosed Then
                    dr.Close()
                End If
            End Try

            Return result
        End Function

        Private Function FillCampaignSend(ByVal dr As IDataReader) As Campaign_SendInfo
            Dim obj As New Campaign_SendInfo With {
                .Id = CInt(dr("Id")),
                .CampaignId = CInt(dr("CampaignId")),
                .TemplateId = If(IsDBNull(dr("TemplateId")), 0, CInt(dr("TemplateId"))),
                .Subject = dr("Subject").ToString(),
                .Body = If(IsDBNull(dr("Body")), "", dr("Body").ToString()),
                .Status = CInt(dr("Status")),
                .TotalRecipient = CInt(dr("TotalRecipient")),
                .TotalSent = CInt(dr("TotalSent")),
                .TotalDelivered = CInt(dr("TotalDelivered")),
                .TotalOpened = CInt(dr("TotalOpened")),
                .TotalClicked = CInt(dr("TotalClicked")),
                .TotalBounced = CInt(dr("TotalBounced")),
                .TotalComplaint = CInt(dr("TotalComplaint")),
                .TotalUnsubscribed = CInt(dr("TotalUnsubscribed")),
                .CreatedDate = CDate(dr("CreatedDate"))
            }

            If Not IsDBNull(dr("ScheduleTime")) Then obj.ScheduleTime = CDate(dr("ScheduleTime"))
            If Not IsDBNull(dr("StartedTime")) Then obj.StartedTime = CDate(dr("StartedTime"))
            If Not IsDBNull(dr("CompletedTime")) Then obj.CompletedTime = CDate(dr("CompletedTime"))

            Return obj
        End Function

    End Class

    Public Class SendLogResult
        Public Property Logs As New List(Of Mail_Send_LogInfo)()
        Public Property TotalCount As Integer = 0
    End Class

    Public Class CampaignStatistics
        Public Property TotalRecipients As Integer
        Public Property CountSent As Integer
        Public Property CountDelivered As Integer
        Public Property CountOpened As Integer
        Public Property CountClicked As Integer
        Public Property CountBounced As Integer
        Public Property CountComplaint As Integer
        Public Property CountUnsubscribed As Integer
        Public Property CountFailed As Integer

        Public Property OpenRate As Decimal
        Public Property ClickRate As Decimal
        Public Property BounceRate As Decimal
        Public Property UnsubscribeRate As Decimal

        Public Property FirstSentTime As DateTime?
        Public Property LastSentTime As DateTime?
        Public Property AvgTimeToOpenSeconds As Integer?
    End Class

    Public Class StatusDistribution
        Public Property Status As String
        Public Property Count As Integer
        Public Property Percentage As Decimal
    End Class

End Namespace
