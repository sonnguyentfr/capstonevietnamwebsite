'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class NewsByViewInfo
        Private _Id As Integer
        Private _NewId As Integer
        Private _ViewCount As Integer
        Private _PortalId As Integer


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
        Public Property NewId() As Integer
            Get
                Return _NewId
            End Get
            Set(ByVal Value As Integer)
                _NewId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ViewCount() As Integer
            Get
                Return _ViewCount
            End Get
            Set(ByVal Value As Integer)
                _ViewCount = Value
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