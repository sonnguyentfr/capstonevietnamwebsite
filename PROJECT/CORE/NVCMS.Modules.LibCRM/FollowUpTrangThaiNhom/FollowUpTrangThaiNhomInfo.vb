'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.LibCRM
    Public Class FollowUpTrangThaiNhomInfo
        Private _id As Integer
        Private _TenNhom As String
        Private _Descreption As String
        Private _Ordernumber As Integer
        Private _Createddate As DateTime
        Private _Userid As Integer
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
        Public Property TenNhom() As String
            Get
                Return _TenNhom
            End Get
            Set(ByVal Value As String)
                _TenNhom = Value
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
        Public Property Ordernumber() As Integer
            Get
                Return _Ordernumber
            End Get
            Set(ByVal Value As Integer)
                _Ordernumber = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Createddate() As DateTime
            Get
                Return _Createddate
            End Get
            Set(ByVal Value As DateTime)
                _Createddate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Userid() As Integer
            Get
                Return _Userid
            End Get
            Set(ByVal Value As Integer)
                _Userid = Value
            End Set
        End Property
        '------------------------------------------'
    End Class
End Namespace