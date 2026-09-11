Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Zalo_ListSdtInfo
        Public Property Id() As Integer
        Public Property Marketing_Zalo_CampaignId() As Integer
        Public Property PhoneRaw() As String
        Public Property Phone() As String
        Public Property Status() As Integer
        Public Property SendCount() As Integer
        Public Property TotalRecords() As Integer
        Public Property CreatedDate() As DateTime
        Public Property UserId() As Integer
        Public Property PortalId() As Integer

        Public ReadOnly Property StatusLabel() As String
            Get
                Select Case Status
                    Case 0 : Return "Cho gui"
                    Case 1 : Return "Da gui"
                    Case 2 : Return "Loi"
                    Case Else : Return "Khong xac dinh"
                End Select
            End Get
        End Property

        Public ReadOnly Property StatusCssClass() As String
            Get
                Select Case Status
                    Case 0 : Return "badge-warning"
                    Case 1 : Return "badge-success"
                    Case 2 : Return "badge-danger"
                    Case Else : Return "badge-light"
                End Select
            End Get
        End Property
    End Class

    Public Class Marketing_Zalo_ListSdt_BulkResult
        Public Property InsertCount() As Integer
        Public Property DupCount() As Integer
    End Class

End Namespace
