'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Marketing
    Public Class DashboardSummary

        Public Property TotalRecipient As Integer

        Public Property Queued As Integer

        Public Property Sending As Integer

        Public Property Sent As Integer

        Public Property Delivered As Integer

        Public Property Opened As Integer

        Public Property Clicked As Integer

        Public Property Bounce As Integer

        Public Property Complaint As Integer

        Public Property Unsubscribe As Integer

        Public Property AvgOpenSeconds As Integer

        Public Property FastestOpen As Integer

        Public Property SlowestOpen As Integer

        Public Property OpenRate As Decimal

        Public Property ClickRate As Decimal

        Public Property BounceRate As Decimal

        Public Property ComplaintRate As Decimal

        Public Property CTR As Decimal

    End Class
End Namespace