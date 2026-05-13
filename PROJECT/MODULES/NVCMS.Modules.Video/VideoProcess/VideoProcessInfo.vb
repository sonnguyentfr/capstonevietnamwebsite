'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.Video
    Public Class VideoProcessInfo
        Private _ID As Integer
        Private _VideoId As Integer
        Private _StatusID As Integer
        Private _ProcessName As String
        Private _Comment As String
        Private _ByUser As Integer
        Private _ToUser As Integer
        Private _CreateDate As DateTime
        Private _VersionId As Integer
        Private _IPTrack As String


        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal Value As Integer)
                _ID = Value
            End Set
        End Property

        Public Property VideoId() As Long
            Get
                Return _VideoId
            End Get
            Set(ByVal Value As Long)
                _VideoId = Value
            End Set
        End Property

        Public Property StatusID() As Integer
            Get
                Return _StatusID
            End Get
            Set(ByVal Value As Integer)
                _StatusID = Value
            End Set
        End Property

        Public Property ProcessName() As String
            Get
                Return _ProcessName
            End Get
            Set(ByVal value As String)
                _ProcessName = value
            End Set
        End Property

        Public Property Comment() As String
            Get
                Return _Comment
            End Get
            Set(ByVal Value As String)
                _Comment = Value
            End Set
        End Property

        Public Property ByUser() As Integer
            Get
                Return _ByUser
            End Get
            Set(ByVal Value As Integer)
                _ByUser = Value
            End Set
        End Property

        Public Property ToUser() As Integer
            Get
                Return _ToUser
            End Get
            Set(ByVal Value As Integer)
                _ToUser = Value
            End Set
        End Property

        Public Property CreateDate() As DateTime
            Get
                Return _CreateDate
            End Get
            Set(ByVal Value As DateTime)
                _CreateDate = Value
            End Set
        End Property

        Public Property VersionId() As Integer
            Get
                Return _VersionId
            End Get
            Set(ByVal Value As Integer)
                _VersionId = Value
            End Set
        End Property

        Public Property IPTrack() As String
            Get
                Return _IPTrack
            End Get
            Set(ByVal Value As String)
                _IPTrack = Value
            End Set
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal ID As Integer, ByVal VideoId As Integer, ByVal StatusID As Integer, ByVal ProcessName As String, ByVal Comment As String, ByVal ByUser As Integer, ByVal ToUser As Integer, ByVal CreateDate As DateTime, ByVal VersionId As Integer, ByVal IPTrack As String)
            Me.ID = ID
            Me.VideoId = VideoId
            Me.StatusID = StatusID
            Me.ProcessName = ProcessName
            Me.Comment = Comment
            Me.ByUser = ByUser
            Me.ToUser = ToUser
            Me.CreateDate = CreateDate
            Me.VersionId = VersionId
            Me.IPTrack = IPTrack
        End Sub

    End Class
End Namespace