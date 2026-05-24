'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.TrangGioiThieu
    Public Class Media_Info
        Private _id As Integer
        Private _TrangGioiThieuId As Integer
        Private _Title As String
        Private _Descreption As String
        Private _MediaLnk As String
        Private _Ordernumber As Integer
        Private _PortalId As Integer


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
        Public Property TrangGioiThieuId() As Integer
            Get
                Return _TrangGioiThieuId
            End Get
            Set(ByVal Value As Integer)
                _TrangGioiThieuId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Descreption() As String
            Get
                Return _Descreption
            End Get
            Set(ByVal Value As String)
                _Descreption = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property MediaLnk() As String
            Get
                Return _MediaLnk
            End Get
            Set(ByVal Value As String)
                _MediaLnk = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Ordernumber() As Integer
            Get
                Return _Ordernumber
            End Get
            Set(ByVal Value As Integer)
                _Ordernumber = Value
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