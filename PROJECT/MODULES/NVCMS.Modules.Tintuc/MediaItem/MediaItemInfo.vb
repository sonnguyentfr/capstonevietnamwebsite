'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class MediaItemInfo
        Private _id As Integer
        Private _title As String
        Private _filename As String
        Private _forder As String
        Private _MediaUrl As String
        Private _Size As Integer
        Private _extension As String
        Private _createddate As DateTime
        Private _userid As Integer
        Private _portalid As Integer


        '------------------------------------------'
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal Value As Integer)
                _id = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property filename() As String
            Get
                Return _filename
            End Get
            Set(ByVal Value As String)
                _filename = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property forder() As String
            Get
                Return _forder
            End Get
            Set(ByVal Value As String)
                _forder = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property MediaUrl() As String
            Get
                Return _MediaUrl
            End Get
            Set(ByVal Value As String)
                _MediaUrl = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Size() As Integer
            Get
                Return _Size
            End Get
            Set(ByVal Value As Integer)
                _Size = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property extension() As String
            Get
                Return _extension
            End Get
            Set(ByVal Value As String)
                _extension = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property createddate() As DateTime
            Get
                Return _createddate
            End Get
            Set(ByVal Value As DateTime)
                _createddate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property userid() As Integer
            Get
                Return _userid
            End Get
            Set(ByVal Value As Integer)
                _userid = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property portalid() As Integer
            Get
                Return _portalid
            End Get
            Set(ByVal Value As Integer)
                _portalid = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace