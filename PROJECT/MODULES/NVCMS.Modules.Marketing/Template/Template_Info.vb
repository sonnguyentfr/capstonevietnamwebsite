'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Marketing
    Public Class Marketing_Mail_TemplateInfo
        Private _Id As Integer
        Private _TemplateName As String
        Private _FilePath As String
        Private _PortalId As Integer


        '------------------------------------------'
        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal Value As Integer)
                _Id = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TemplateName() As String
            Get
                Return _TemplateName
            End Get
            Set(ByVal Value As String)
                _TemplateName = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FilePath() As String
            Get
                Return _FilePath
            End Get
            Set(ByVal Value As String)
                _FilePath = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property PortalId() As Integer
            Get
                Return _PortalId
            End Get
            Set(ByVal Value As Integer)
                _PortalId = Value
            End Set
        End Property

        '------------------------------------------'
    End Class

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