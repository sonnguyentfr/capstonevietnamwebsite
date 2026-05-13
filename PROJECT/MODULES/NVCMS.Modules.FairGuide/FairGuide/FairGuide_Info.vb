'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.FairGuide
    Public Class FairGuide_Info
        Private _id As Integer
        Private _Title As String
        Private _Avatar As String
        Private _Descreption As String
        Private _Noidung As String
        Private _Ordernumber As Integer
        Private _IsActive As Boolean
        Private _Createddate As DateTime
        Private _sizewidth As Integer
        Private _sizeheight As Integer
        Private _UserId As Integer
        Private _Portalid As Integer


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
        Public Property Avatar() As String
            Get
                Return _Avatar
            End Get
            Set(ByVal Value As String)
                _Avatar = Value
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
        Public Property Noidung() As String
            Get
                Return _Noidung
            End Get
            Set(ByVal Value As String)
                _Noidung = Value
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
        Public Property IsActive() As Boolean
            Get
                Return _IsActive
            End Get
            Set(ByVal Value As Boolean)
                _IsActive = Value
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
        Public Property sizewidth() As Integer
            Get
                Return _sizewidth
            End Get
            Set(ByVal Value As Integer)
                _sizewidth = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property sizeheight() As Integer
            Get
                Return _sizeheight
            End Get
            Set(ByVal Value As Integer)
                _sizeheight = Value
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
        Public Property Portalid() As Integer
            Get
                Return _Portalid
            End Get
            Set(ByVal Value As Integer)
                _Portalid = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace