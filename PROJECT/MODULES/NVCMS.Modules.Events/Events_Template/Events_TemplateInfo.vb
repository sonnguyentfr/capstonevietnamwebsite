'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.EventsWebsite
    Public Class Events_TemplateInfo
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
End Namespace