'******************************************
' ZNS Template Info
'******************************************
Namespace NVCMS.Modules.Marketing

    Public Class Marketing_ZNS_TemplateInfo
        Public Property Id() As Long
        Public Property TemplateId() As Long
        Public Property TemplateName() As String
        Public Property CreatedTime() As Long
        Public Property Status() As String
        Public Property TemplateQuality() As String
        Public Property TemplateTag() As String
        Public Property Timeout() As Long
        Public Property PreviewUrl() As String
        Public Property Price() As Decimal
        Public Property PriceUid() As Decimal
        Public Property PriceSdt() As Decimal
        Public Property ApplyTemplateQuota() As Boolean
        Public Property Reason() As String
        Public Property IsActive() As Boolean
        Public Property LastSyncedAt() As DateTime
        Public Property CreatedAt() As DateTime
        Public Property UpdatedAt() As DateTime
    End Class

End Namespace