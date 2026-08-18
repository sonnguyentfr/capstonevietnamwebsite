Imports System.Data
Imports NVCMS.Modules.Marketing
Namespace NVCMS.Modules.Marketing
    Public Class MarketingMailAnalyticsService
        Private ReadOnly _dataProvider As DataProvider

        Public Sub New()
            _dataProvider = DataProvider.Instance()
        End Sub

        ''' <summary>
        ''' Xử lý nghiệp vụ và map dữ liệu từ IDataReader vào Models
        ''' </summary>
        Public Function GetCampaignAnalytics(ByVal campaignId As Integer) As Marketing_Mail_CampaignAnalyticsResult
            ' 1. Validate logic nghiệp vụ
            If campaignId <= 0 Then
                Throw New ArgumentException("CampaignId không hợp lệ.")
            End If

            Dim result As New Marketing_Mail_CampaignAnalyticsResult()

            ' 2. Gọi DataProvider để lấy IDataReader từ Store Procedure
            Using reader As IDataReader = _dataProvider.Marketing_Mail_Campaign_Analytics(campaignId)

                ' ==========================================
                ' RESULT SET 1: CAMPAIGN SUMMARY
                ' ==========================================
                If reader.Read() Then
                    Dim summary As New Marketing_Mail_CampaignSummary()
                    summary.CampaignId = GetValue(Of Integer)(reader("CampaignId"))
                    summary.Title = GetValue(Of String)(reader("Title"))
                    summary.Description = GetValue(Of String)(reader("Description"))
                    summary.CreatedDate = GetNullable(Of DateTime)(reader("CreatedDate"))
                    summary.TotalCampaignSend = GetValue(Of Integer)(reader("TotalCampaignSend"))
                    summary.TotalRecipient = GetValue(Of Integer)(reader("TotalRecipient"))
                    summary.TotalSent = GetValue(Of Integer)(reader("TotalSent"))
                    summary.TotalDelivered = GetValue(Of Integer)(reader("TotalDelivered"))
                    summary.TotalOpened = GetValue(Of Integer)(reader("TotalOpened"))
                    summary.TotalClicked = GetValue(Of Integer)(reader("TotalClicked"))
                    summary.TotalBounced = GetValue(Of Integer)(reader("TotalBounced"))
                    summary.TotalComplaint = GetValue(Of Integer)(reader("TotalComplaint"))
                    summary.TotalUnsubscribed = GetValue(Of Integer)(reader("TotalUnsubscribed"))
                    summary.OpenRate = GetValue(Of Decimal)(reader("OpenRate"))
                    summary.ClickRate = GetValue(Of Decimal)(reader("ClickRate"))
                    summary.DeliveryRate = GetValue(Of Decimal)(reader("DeliveryRate"))
                    summary.BounceRate = GetValue(Of Decimal)(reader("BounceRate"))
                    summary.FirstStartedTime = GetNullable(Of DateTime)(reader("FirstStartedTime"))
                    summary.LastCompletedTime = GetNullable(Of DateTime)(reader("LastCompletedTime"))

                    result.Summary = summary
                End If

                ' ==========================================
                ' RESULT SET 2: MAIL SEND DETAIL LOG
                ' ==========================================
                If reader.NextResult() Then
                    While reader.Read()
                        Dim detail As New Marketing_Mail_SendDetailLog()
                        detail.SendLogId = GetValue(Of Long)(reader("SendLogId"))
                        detail.CampaignSendId = GetValue(Of Integer)(reader("CampaignSendId"))
                        detail.ListMailId = GetNullable(Of Integer)(reader("ListMailId"))
                        detail.Email = GetValue(Of String)(reader("Email"))
                        detail.Status = GetValue(Of String)(reader("Status"))
                        detail.SesMessageId = GetValue(Of String)(reader("SesMessageId"))
                        detail.SentTime = GetNullable(Of DateTime)(reader("SentTime"))
                        detail.DeliveredTime = GetNullable(Of DateTime)(reader("DeliveredTime"))
                        detail.OpenedTime = GetNullable(Of DateTime)(reader("OpenedTime"))
                        detail.ClickedTime = GetNullable(Of DateTime)(reader("ClickedTime"))
                        detail.ErrorMessage = GetValue(Of String)(reader("ErrorMessage"))
                        detail.CreatedDate = GetValue(Of DateTime)(reader("CreatedDate"))

                        ' Log metrics thời gian
                        detail.SentToDeliveredSeconds = GetNullable(Of Integer)(reader("SentToDeliveredSeconds"))
                        detail.DeliveredToOpenSeconds = GetNullable(Of Integer)(reader("DeliveredToOpenSeconds"))
                        detail.SentToOpenSeconds = GetNullable(Of Integer)(reader("SentToOpenSeconds"))
                        detail.DeliveredToOpenMinutes = GetNullable(Of Decimal)(reader("DeliveredToOpenMinutes"))
                        detail.SentToOpenMinutes = GetNullable(Of Decimal)(reader("SentToOpenMinutes"))

                        result.Details.Add(detail)
                    End While
                End If

            End Using

            Return result
        End Function

#Region "Helper Methods ép kiểu chống DBNull"
        Private Function GetValue(Of T)(ByVal value As Object) As T
            If value Is DBNull.Value OrElse value Is Nothing Then
                Return CType(Nothing, T)
            End If
            Return CType(Convert.ChangeType(value, GetType(T)), T)
        End Function

        Private Function GetNullable(Of T As Structure)(ByVal value As Object) As Nullable(Of T)
            If value Is DBNull.Value OrElse value Is Nothing Then
                Return Nothing
            End If
            Return CType(Convert.ChangeType(value, GetType(T)), T)
        End Function
#End Region
    End Class
End Namespace