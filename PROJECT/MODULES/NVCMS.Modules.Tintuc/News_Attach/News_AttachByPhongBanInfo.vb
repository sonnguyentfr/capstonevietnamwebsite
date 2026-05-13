Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.TinTuc

Namespace NVCMS.Modules.TinTuc
    Public Class News_AttachByPhongBanInfo
        Private _Id As Integer
        Private _AttachFileID As Integer
        Private _PhongBanID As Integer

        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(value As Integer)
                _Id = value
            End Set
        End Property
        Public Property AttachFileID() As Integer
            Get
                Return _AttachFileID
            End Get
            Set(value As Integer)
                _AttachFileID = value
            End Set
        End Property
        Public Property PhongBanID() As Integer
            Get
                Return _PhongBanID
            End Get
            Set(value As Integer)
                _PhongBanID = value
            End Set
        End Property
    End Class
End Namespace
