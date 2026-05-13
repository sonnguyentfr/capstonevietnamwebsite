Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.TinTuc
    Public Class NewsByAttachInfo
        Private _Id As Integer
        Private _NewsId As Integer
        Private _AttachId As Integer

        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(value As Integer)
                _Id = value
            End Set
        End Property
        Public Property NewsId() As Integer
            Get
                Return _NewsId
            End Get
            Set(value As Integer)
                _NewsId = value
            End Set
        End Property
        Public Property AttachId() As Integer
            Get
                Return _AttachId
            End Get
            Set(value As Integer)
                _AttachId = value
            End Set
        End Property
    End Class
End Namespace
