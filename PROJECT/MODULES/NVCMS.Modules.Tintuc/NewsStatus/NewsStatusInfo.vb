'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class NV_NewsStatusInfo
        Private _NewsStatusId As Integer
        Private _StatusName As String
        Private _Description As String


        '------------------------------------------'
        Public Property NewsStatusId() As Integer
            Get
                Return _NewsStatusId
            End Get
            Set(ByVal Value As Integer)
                _NewsStatusId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property StatusName() As String
            Get
                Return _StatusName
            End Get
            Set(ByVal Value As String)
                _StatusName = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal Value As String)
                _Description = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace