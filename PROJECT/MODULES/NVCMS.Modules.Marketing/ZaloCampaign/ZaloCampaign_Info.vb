Namespace NVCMS.Modules.Marketing

    Public Class Marketing_Zalo_CampaignInfo
        Public Property Id() As Integer
        Public Property Title() As String
        Public Property Description() As String
        Public Property Status() As Integer
        Public Property TotalSdt() As Integer
        Public Property CreatedDate() As DateTime
        Public Property UserId() As Integer
        Public Property PortalId() As Integer

        Public ReadOnly Property StatusLabel() As String
            Get
                Select Case Status
                    Case 0 : Return "Nhap"
                    Case 1 : Return "Dang hoat dong"
                    Case 2 : Return "Da gui"
                    Case Else : Return "Khong xac dinh"
                End Select
            End Get
        End Property

        Public ReadOnly Property StatusCssClass() As String
            Get
                Select Case Status
                    Case 0 : Return "badge-secondary"
                    Case 1 : Return "badge-success"
                    Case 2 : Return "badge-info"
                    Case Else : Return "badge-light"
                End Select
            End Get
        End Property
    End Class

End Namespace
