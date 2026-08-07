Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.LadingPage

Namespace NVCMS.Modules.LadingPage
    Public Class LadingPageTemplateInfo
        Private _id As Integer
        Private _TemplateName As String
        Private _FilePath As String
        Private _PortalID As Integer
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal Value As Integer)
                _id = Value
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
