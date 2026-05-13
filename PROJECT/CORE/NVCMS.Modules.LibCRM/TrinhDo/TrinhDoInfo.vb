'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.LibCRM
    Public Class TrinhDoInfo
        Private _id As Integer
        Private _Title As String
        Private _TitleEN As String
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
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TitleEN() As String
            Get
                Return _TitleEN
            End Get
            Set(ByVal Value As String)
                _TitleEN = Value
            End Set
        End Property
        '------------------------------------------'
    End Class
End Namespace