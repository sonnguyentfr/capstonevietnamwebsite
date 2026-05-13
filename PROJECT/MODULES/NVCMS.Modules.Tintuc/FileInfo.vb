'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class V_FileInfo
        Private _FileId As Integer
        Private _PortalId As Integer
        Private _FileName As String
        Private _Extension As String
        Private _Size As Integer
        Private _Width As Integer
        Private _Height As Integer
        Private _ContentType As String
        Private _Folder As String
        Private _FolderID As Integer
        Private _CreatedByUserID As Integer
        Private _CreatedOnDate As DateTime
        Private _LastModifiedByUserID As Integer
        Private _LastModifiedOnDate As DateTime
        '------------------------------------------'
        Public Property FileId() As Integer
            Get
                Return _FileId
            End Get
            Set(ByVal Value As Integer)
                _FileId = Value
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
        Public Property FileName() As String
            Get
                Return _FileName
            End Get
            Set(ByVal Value As String)
                _FileName = Value
            End Set
        End Property
        Public Property Extension() As String
            Get
                Return _Extension
            End Get
            Set(ByVal Value As String)
                _Extension = Value
            End Set
        End Property
        Public Property Size() As Integer
            Get
                Return _Size
            End Get
            Set(ByVal Value As Integer)
                _Size = Value
            End Set
        End Property
        Public Property Width() As Integer
            Get
                Return _Width
            End Get
            Set(ByVal Value As Integer)
                _Width = Value
            End Set
        End Property
        Public Property Height() As Integer
            Get
                Return _Height
            End Get
            Set(ByVal Value As Integer)
                _Height = Value
            End Set
        End Property
        Public Property ContentType() As String
            Get
                Return _ContentType
            End Get
            Set(ByVal Value As String)
                _ContentType = Value
            End Set
        End Property
        Public Property Folder() As String
            Get
                Return _Folder
            End Get
            Set(ByVal Value As String)
                _Folder = Value
            End Set
        End Property
        Public Property FolderID() As Integer
            Get
                Return _FolderID
            End Get
            Set(ByVal Value As Integer)
                _FolderID = Value
            End Set
        End Property
        Public Property CreatedByUserID() As Integer
            Get
                Return _CreatedByUserID
            End Get
            Set(ByVal Value As Integer)
                _CreatedByUserID = Value
            End Set
        End Property
        Public Property CreatedOnDate() As DateTime
            Get
                Return _CreatedOnDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedOnDate = Value
            End Set
        End Property
        Public Property LastModifiedByUserID() As Integer
            Get
                Return _LastModifiedByUserID
            End Get
            Set(ByVal Value As Integer)
                _LastModifiedByUserID = Value
            End Set
        End Property
        Public Property LastModifiedOnDate() As DateTime
            Get
                Return _LastModifiedOnDate
            End Get
            Set(ByVal Value As DateTime)
                _LastModifiedOnDate = Value
            End Set
        End Property
        '------------------------------------------'
    End Class
End Namespace