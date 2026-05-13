Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.TinTuc

Namespace NVCMS.Modules.TinTuc
    Public Class NewsAttachInfo
        Private _AttachFileID As Integer
        Private _FileName As String
        Private _Description As String
        Private _FileType As String
        Private _FileId As Integer
        Private _IsPublic As Boolean
        Private _Sort As Integer
        Private _CreatedDate As Date
        Private _PortalId As Integer

        Public Property AttachFileID() As Integer
            Get
                Return _AttachFileID
            End Get
            Set(value As Integer)
                _AttachFileID = value
            End Set
        End Property
        Public Property FileName() As String
            Get
                Return _FileName
            End Get
            Set(value As String)
                _FileName = value
            End Set
        End Property
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(value As String)
                _Description = value
            End Set
        End Property
        Public Property FileType() As String
            Get
                Return _FileType
            End Get
            Set(value As String)
                _FileType = value
            End Set
        End Property
        Public Property FileId() As Integer
            Get
                Return _FileId
            End Get
            Set(value As Integer)
                _FileId = value
            End Set
        End Property
        Public Property IsPublic() As Boolean
            Get
                Return _IsPublic
            End Get
            Set(value As Boolean)
                _IsPublic = value
            End Set
        End Property
        Public Property Sort() As Integer
            Get
                Return _Sort
            End Get
            Set(value As Integer)
                _Sort = value
            End Set
        End Property
        Public Property CreatedDate() As Date
            Get
                Return _CreatedDate
            End Get
            Set(value As Date)
                _CreatedDate = value
            End Set
        End Property
        Public Property PortalId() As Integer
            Get
                Return _PortalId
            End Get
            Set(value As Integer)
                _PortalId = value
            End Set
        End Property
    End Class
End Namespace