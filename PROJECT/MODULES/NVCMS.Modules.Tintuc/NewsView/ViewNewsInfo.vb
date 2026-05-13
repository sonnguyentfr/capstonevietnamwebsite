'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class ViewNewsInfo
        Private _Id As Integer
        Private _UserId As Integer
        Private _NewsId As Integer

        '------------------------------------------'
        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal Value As Integer)
                _Id = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property UserId() As Integer
            Get
                Return _UserId
            End Get
            Set(ByVal Value As Integer)
                _UserId = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property NewsId() As Integer
            Get
                Return _NewsId
            End Get
            Set(ByVal Value As Integer)
                _NewsId = Value
            End Set
        End Property
        '------------------------------------------'
    End Class
End Namespace