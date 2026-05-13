'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.TinTuc
    Public Class NhuanButInfo
        Private _id As Integer
        Private _NewId As Integer
        Private _Type As Integer
        Private _UserId As Integer
        Private _Credit As Integer
        Private _Createdate As DateTime
        Private _CreateUser As Integer
        Private _UserChamNhuanBut As Integer
        Private _UserChamNhuanButDate As DateTime
        Private _XuatBan As Boolean
        Private _PortalId As Integer
        Private _KieuNhuanBut As Integer
        Public ReadOnly Property Title() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "Title:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.Title
                    Else
                        strResult = "-"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property ButDanh() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "ButDanh:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.ButDanh
                    Else
                        strResult = "-"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property CategoryId() As Integer
            Get
                Dim strCacheKey As String
                strCacheKey = "CategoryId:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.CategoryId
                    Else
                        strResult = 0
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property CategoryName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CategoryName:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.CategoryName
                    Else
                        strResult = 0
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property NewsKind() As Integer
            Get
                Dim strCacheKey As String
                strCacheKey = "NewsKind:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.NewsKind
                    Else
                        strResult = 0
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property isPR() As Boolean
            Get
                Dim strCacheKey As String
                strCacheKey = "isPR:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.IsPR
                    Else
                        strResult = False
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property ViewCount() As Integer
            Get
                Dim strCacheKey As String
                strCacheKey = "ViewCount:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.ViewCount
                    Else
                        strResult = 0
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property View() As Integer
            Get
                Dim strCacheKey As String
                strCacheKey = "View:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.ViewCount
                    Else
                        strResult = 0
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property PublishedDate() As DateTime
            Get
                Dim strCacheKey As String
                strCacheKey = "PublishedDate:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.PublishedDate
                    Else
                        strResult = BL.minDateV
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property CreditTong() As Integer
            Get
                Dim strCacheKey As String
                strCacheKey = "CreditTong:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.Credit
                    Else
                        strResult = 0
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
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
        Public Property NewId() As Integer
            Get
                Return _NewId
            End Get
            Set(ByVal Value As Integer)
                _NewId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Type() As Integer
            Get
                Return _Type
            End Get
            Set(ByVal Value As Integer)
                _Type = Value
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
        Public Property Credit() As Integer
            Get
                Return _Credit
            End Get
            Set(ByVal Value As Integer)
                _Credit = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Createdate() As DateTime
            Get
                Return _Createdate
            End Get
            Set(ByVal Value As DateTime)
                _Createdate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CreateUser() As Integer
            Get
                Return _CreateUser
            End Get
            Set(ByVal Value As Integer)
                _CreateUser = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property UserChamNhuanBut() As Integer
            Get
                Return _UserChamNhuanBut
            End Get
            Set(ByVal Value As Integer)
                _UserChamNhuanBut = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property UserChamNhuanButDate() As DateTime
            Get
                Return _UserChamNhuanButDate
            End Get
            Set(ByVal Value As DateTime)
                _UserChamNhuanButDate = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property XuatBan() As Boolean
            Get
                Return _XuatBan
            End Get
            Set(ByVal Value As Boolean)
                _XuatBan = Value
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
        Public Property KieuNhuanBut() As Integer
            Get
                Return _KieuNhuanBut
            End Get
            Set(ByVal Value As Integer)
                _KieuNhuanBut = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace