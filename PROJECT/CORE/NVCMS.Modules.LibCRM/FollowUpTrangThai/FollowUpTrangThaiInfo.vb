'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.LibCRM
    Public Class FollowUpTrangThaiInfo
        Private _id As Integer
        Private _Title As String
        Private _ParentId As Integer
        Private _isShow As Boolean
        Private _isActive As Boolean
        Private _Kyhopdong As Boolean
        Private _UserId As Integer
        Private _CreatedDate As DateTime
        Private _Student_NhomId As Integer
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
        Public Property ParentId() As Integer
            Get
                Return _ParentId
            End Get
            Set(ByVal Value As Integer)
                _ParentId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property isShow() As Boolean
            Get
                Return _isShow
            End Get
            Set(ByVal Value As Boolean)
                _isShow = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property isActive() As Boolean
            Get
                Return _isActive
            End Get
            Set(ByVal Value As Boolean)
                _isActive = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Kyhopdong() As Boolean
            Get
                Return _Kyhopdong
            End Get
            Set(ByVal Value As Boolean)
                _Kyhopdong = Value
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
        Public Property CreatedDate() As DateTime
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Student_NhomId() As Integer
            Get
                Return _Student_NhomId
            End Get
            Set(ByVal Value As Integer)
                _Student_NhomId = Value
            End Set
        End Property

        '------------------------------------------'
        Public ReadOnly Property NhomName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NhomName:" & Student_NhomId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim _FollowUpTrangThaiNhomController As New FollowUpTrangThaiNhomController
                    Dim obj As FollowUpTrangThaiNhomInfo = _FollowUpTrangThaiNhomController.Follow_TrangThaiNhom_GetByID(Student_NhomId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = obj.TenNhom
                        End With
                    Else
                        strResult = "-"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
    End Class
End Namespace