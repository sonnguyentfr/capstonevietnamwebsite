'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.EventsWebsite
    Public Class EventsInfo
        Private _id As Integer
        Private _Title As String
        Private _TitleEN As String
        Private _CODE As String
        Private _Source As String
        Private _Vanphong As Integer
        Private _CatId As Integer
        Private _Avatar As String
        Private _diadiem As String
        Private _diadiemEN As String
        Private _fromdatetime As DateTime
        Private _enddatetime As DateTime
        Private _thanhphan As String
        Private _thanhphanEN As String
        Private _School As String
        Private _Org As String
        Private _Gia As Integer
        Private _Descreption As String
        Private _DescreptionEN As String
        Private _LienheName As String
        Private _LienheEmail As String
        Private _LienheMobile As String
        Private _LienheAdd As String
        Private _UserId As Integer
        Private _Portalid As Integer
        Private _Createddate As DateTime
        Private _Isactive As Boolean
        Private _anhbando As String
        Private _linkbando As String
        Private _Ordernumber As Integer

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
        Public Property Ordernumber() As Integer
            Get
                Return _Ordernumber
            End Get
            Set(ByVal Value As Integer)
                _Ordernumber = Value
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
        Public Property CODE() As String
            Get
                Return _CODE
            End Get
            Set(ByVal Value As String)
                _CODE = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Source() As String
            Get
                Return _Source
            End Get
            Set(ByVal Value As String)
                _Source = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Vanphong() As Integer
            Get
                Return _Vanphong
            End Get
            Set(ByVal Value As Integer)
                _Vanphong = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CatId() As Integer
            Get
                Return _CatId
            End Get
            Set(ByVal Value As Integer)
                _CatId = Value
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
        Public Property diadiem() As String
            Get
                Return _diadiem
            End Get
            Set(ByVal Value As String)
                _diadiem = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property diadiemEN() As String
            Get
                Return _diadiemEN
            End Get
            Set(ByVal Value As String)
                _diadiemEN = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property fromdatetime() As DateTime
            Get
                Return _fromdatetime
            End Get
            Set(ByVal Value As DateTime)
                _fromdatetime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property enddatetime() As DateTime
            Get
                Return _enddatetime
            End Get
            Set(ByVal Value As DateTime)
                _enddatetime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property thanhphan() As String
            Get
                Return _thanhphan
            End Get
            Set(ByVal Value As String)
                _thanhphan = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property thanhphanEN() As String
            Get
                Return _thanhphanEN
            End Get
            Set(ByVal Value As String)
                _thanhphanEN = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property School() As String
            Get
                Return _School
            End Get
            Set(ByVal Value As String)
                _School = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Org() As String
            Get
                Return _Org
            End Get
            Set(ByVal Value As String)
                _Org = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Gia() As Integer
            Get
                Return _Gia
            End Get
            Set(ByVal Value As Integer)
                _Gia = Value
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
        Public Property DescreptionEN() As String
            Get
                Return _DescreptionEN
            End Get
            Set(ByVal Value As String)
                _DescreptionEN = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property LienheName() As String
            Get
                Return _LienheName
            End Get
            Set(ByVal Value As String)
                _LienheName = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LienheEmail() As String
            Get
                Return _LienheEmail
            End Get
            Set(ByVal Value As String)
                _LienheEmail = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LienheMobile() As String
            Get
                Return _LienheMobile
            End Get
            Set(ByVal Value As String)
                _LienheMobile = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LienheAdd() As String
            Get
                Return _LienheAdd
            End Get
            Set(ByVal Value As String)
                _LienheAdd = Value
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
        Public Property Createddate() As DateTime
            Get
                Return _Createddate
            End Get
            Set(ByVal Value As DateTime)
                _Createddate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Isactive() As Boolean
            Get
                Return _Isactive
            End Get
            Set(ByVal Value As Boolean)
                _Isactive = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property anhbando() As String
            Get
                Return _anhbando
            End Get
            Set(ByVal Value As String)
                _anhbando = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property linkbando() As String
            Get
                Return _linkbando
            End Get
            Set(ByVal Value As String)
                _linkbando = Value
            End Set
        End Property

        '------------------------------------------'
        Public ReadOnly Property CatEventNamea() As String
            Get
                'Dim strCacheKey As String
                'strCacheKey = "CatEventNamea:" & CatId
                Dim strResult As String = String.Empty
                'strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New EventsWebsite_CatController
                    Dim obj As Events_CatInfo = ctl.Events_Cat_GetByID(CatId, 50)
                    If Not obj Is Nothing Then
                        strResult = "[:" & obj.CatName & ":]" & "- " & Title
                    Else
                        strResult = ""
                    End If
                    ' DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property CatEventName() As String
            Get
                'Dim strCacheKey As String
                'strCacheKey = "CatEventNamea:" & CatId
                Dim strResult As String = String.Empty
                'strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New EventsWebsite_CatController
                    Dim obj As Events_CatInfo = ctl.Events_Cat_GetByID(CatId, 50)
                    If Not obj Is Nothing Then
                        strResult = obj.CatName
                    Else
                        strResult = ""
                    End If
                    ' DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
    End Class
End Namespace