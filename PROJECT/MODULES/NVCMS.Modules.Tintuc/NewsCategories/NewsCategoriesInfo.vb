'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.TinTuc
    Public Class NV_NewsCategoriesInfo
        Private _CategoryID As Integer
        Private _CategoryName As String
        Private _Description As String
        Private _TabID As Integer
        Private _TabIdDetail As Integer
        Private _IsActive As Boolean
        Private _CreateDate As DateTime
        Private _PortalId As Integer
        Private _ParentId As Integer
        Private _OrderNumber As Integer


        '------------------------------------------'
        Public Property CategoryID() As Integer
            Get
                Return _CategoryID
            End Get
            Set(ByVal Value As Integer)
                _CategoryID = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CategoryName() As String
            Get
                Return _CategoryName
            End Get
            Set(ByVal Value As String)
                _CategoryName = Value
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
        Public Property TabID() As Integer
            Get
                Return _TabID
            End Get
            Set(ByVal Value As Integer)
                _TabID = Value
            End Set
        End Property
        Public Property TabIdDetail() As Integer
            Get
                Return _TabIdDetail
            End Get
            Set(ByVal Value As Integer)
                _TabIdDetail = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property IsActive() As Boolean
            Get
                Return _IsActive
            End Get
            Set(ByVal Value As Boolean)
                _IsActive = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CreateDate() As DateTime
            Get
                Return _CreateDate
            End Get
            Set(ByVal Value As DateTime)
                _CreateDate = Value
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
        Public Property ParentId() As Integer
            Get
                Return _ParentId
            End Get
            Set(ByVal Value As Integer)
                _ParentId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property OrderNumber() As Integer
            Get
                Return _OrderNumber
            End Get
            Set(ByVal Value As Integer)
                _OrderNumber = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace