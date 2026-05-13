'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class NewsByCategoryInfo
        Private _Id As Integer
        Private _NewsId As Integer
        Private _CategoryId As Integer
        Private _IsMainCategory As Boolean


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
        Public Property NewsId() As Integer
            Get
                Return _NewsId
            End Get
            Set(ByVal Value As Integer)
                _NewsId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CategoryId() As Integer
            Get
                Return _CategoryId
            End Get
            Set(ByVal Value As Integer)
                _CategoryId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IsMainCategory() As Boolean
            Get
                Return _IsMainCategory
            End Get
            Set(ByVal Value As Boolean)
                _IsMainCategory = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace