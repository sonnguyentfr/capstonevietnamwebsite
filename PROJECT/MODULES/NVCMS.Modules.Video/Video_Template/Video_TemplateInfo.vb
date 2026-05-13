Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.Video

Namespace NVCMS.Modules.Video
    Public Class Video_TemplateInfo
        Private _TemplateID As Integer
        Private _TemplateName As String
        Private _FilePath As String
        Private _PortalID As Integer
        Public Property TemplateID() As Integer
            Get
                Return _TemplateID
            End Get
            Set(ByVal Value As Integer)
                _TemplateID = Value
            End Set
        End Property
        Public Property TemplateName() As String
            Get
                Return _TemplateName
            End Get
            Set(ByVal Value As String)
                _TemplateName = Value
            End Set
        End Property
        Public Property FilePath() As String
            Get
                Return _FilePath
            End Get
            Set(ByVal Value As String)
                _FilePath = Value
            End Set
        End Property
        Public Property PortalID() As Integer
            Get
                Return _PortalID
            End Get
            Set(value As Integer)
                _PortalID = value
            End Set
        End Property
    End Class
End Namespace
