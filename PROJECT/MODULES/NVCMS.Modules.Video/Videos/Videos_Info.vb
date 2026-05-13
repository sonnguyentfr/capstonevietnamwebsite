'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.Video

    Public Class Videos_Info
        Private _VideoId As Integer
        Private _CategoryId As Integer
        Private _Title As String
        Private _ImagePath As String
        Private _VideoPath As String
        Private _Summary As String
        Private _Content As String
        Private _TypeVideo As Integer
        Private _isActive As Boolean
        Private _Hotcat As Boolean
        Private _Hotsite As Boolean
        Private _Status As Integer
        Private _Tags As String
        Private _IsShowBaiMoi As Boolean
        Private _ButDanh As String
        Private _IsEdited As Boolean
        Private _EditedUser As Integer
        Private _EditedTime As DateTime
        Private _VoteCount As Integer
        Private _ViewCount As Integer
        Private _Credit As Integer
        Private _Createdate As DateTime
        Private _ApprovalRequestDate As DateTime
        Private _ApprovalDate As DateTime
        Private _ApprovalUser As Integer
        Private _ReturnedDate As DateTime
        Private _ReturnedUser As Integer
        Private _CancelPublishDate As DateTime
        Private _CancelPublishUser As Integer
        Private _PublishedDate As DateTime
        Private _PublishedUser As Integer
        Private _UserId As Integer
        Private _Tacgia As String
        Private _LanguageId As String
        Private _PortalId As Integer


        '------------------------------------------'
        Public Property VideoId() As Integer
            Get
                Return _VideoId
            End Get
            Set(ByVal Value As Integer)
                _VideoId = Value
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
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ImagePath() As String
            Get
                Return _ImagePath
            End Get
            Set(ByVal Value As String)
                _ImagePath = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property VideoPath() As String
            Get
                Return _VideoPath
            End Get
            Set(ByVal Value As String)
                _VideoPath = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Summary() As String
            Get
                Return _Summary
            End Get
            Set(ByVal Value As String)
                _Summary = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Content() As String
            Get
                Return _Content
            End Get
            Set(ByVal Value As String)
                _Content = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TypeVideo() As Integer
            Get
                Return _TypeVideo
            End Get
            Set(ByVal Value As Integer)
                _TypeVideo = Value
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
        Public Property Hotcat() As Boolean
            Get
                Return _Hotcat
            End Get
            Set(ByVal Value As Boolean)
                _Hotcat = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Hotsite() As Boolean
            Get
                Return _Hotsite
            End Get
            Set(ByVal Value As Boolean)
                _Hotsite = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Status() As Integer
            Get
                Return _Status
            End Get
            Set(ByVal Value As Integer)
                _Status = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Tags() As String
            Get
                Return _Tags
            End Get
            Set(ByVal Value As String)
                _Tags = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IsShowBaiMoi() As Boolean
            Get
                Return _IsShowBaiMoi
            End Get
            Set(ByVal Value As Boolean)
                _IsShowBaiMoi = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ButDanh() As String
            Get
                Return _ButDanh
            End Get
            Set(ByVal Value As String)
                _ButDanh = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property IsEdited() As Boolean
            Get
                Return _IsEdited
            End Get
            Set(ByVal Value As Boolean)
                _IsEdited = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property EditedUser() As Integer
            Get
                Return _EditedUser
            End Get
            Set(ByVal Value As Integer)
                _EditedUser = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property EditedTime() As DateTime
            Get
                Return _EditedTime
            End Get
            Set(ByVal Value As DateTime)
                _EditedTime = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property VoteCount() As Integer
            Get
                Return _VoteCount
            End Get
            Set(ByVal Value As Integer)
                _VoteCount = Value
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
        Public Property ApprovalRequestDate() As DateTime
            Get
                Return _ApprovalRequestDate
            End Get
            Set(ByVal Value As DateTime)
                _ApprovalRequestDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ApprovalDate() As DateTime
            Get
                Return _ApprovalDate
            End Get
            Set(ByVal Value As DateTime)
                _ApprovalDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ApprovalUser() As Integer
            Get
                Return _ApprovalUser
            End Get
            Set(ByVal Value As Integer)
                _ApprovalUser = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ReturnedDate() As DateTime
            Get
                Return _ReturnedDate
            End Get
            Set(ByVal Value As DateTime)
                _ReturnedDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ReturnedUser() As Integer
            Get
                Return _ReturnedUser
            End Get
            Set(ByVal Value As Integer)
                _ReturnedUser = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CancelPublishDate() As DateTime
            Get
                Return _CancelPublishDate
            End Get
            Set(ByVal Value As DateTime)
                _CancelPublishDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CancelPublishUser() As Integer
            Get
                Return _CancelPublishUser
            End Get
            Set(ByVal Value As Integer)
                _CancelPublishUser = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property PublishedDate() As DateTime
            Get
                Return _PublishedDate
            End Get
            Set(ByVal Value As DateTime)
                _PublishedDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property PublishedUser() As Integer
            Get
                Return _PublishedUser
            End Get
            Set(ByVal Value As Integer)
                _PublishedUser = Value
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
        Public Property Tacgia() As String
            Get
                Return _Tacgia
            End Get
            Set(ByVal Value As String)
                _Tacgia = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property LanguageId() As String
            Get
                Return _LanguageId
            End Get
            Set(ByVal Value As String)
                _LanguageId = Value
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
        '------------------------------------------'
        Public ReadOnly Property CanViewLock() As Boolean
            Get
                Select Case IsEdited
                    Case True
                        Return False
                    Case Else
                        Return True
                End Select
            End Get
        End Property
        '------------------------------------------'
        Public ReadOnly Property StatusName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "StatusName:" & Status
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctlStatus As New NV_NewsStatusController
                    Dim obj As NV_NewsStatusInfo = ctlStatus.NV_NewsStatus_GetByID(Status)
                    If Not obj Is Nothing Then
                        strResult = obj.StatusName
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property CategoryName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "CategoryName:" & CategoryId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsCategoriesController
                    Dim obj As NV_NewsCategoriesInfo = ctl.GetByID(CategoryId)
                    If Not obj Is Nothing Then
                        strResult = obj.CategoryName
                    Else
                        strResult = "-"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property videourl() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "videourl:" & VideoId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim _Videos_Controller As New Videos_Controller
                    Dim objVideotop As Videos_Info = _Videos_Controller.GetByID(VideoId, PortalId)
                    If Not objVideotop Is Nothing Then
                        With objVideotop
                            If .TypeVideo = 2 Then
                                strResult = "https://www.youtube.com/watch?v=" & .VideoPath
                            End If
                            If .TypeVideo = 3 Then
                                strResult = .VideoPath.Replace("/DATA", BL.filesDomain)
                            End If
                        End With
                    Else
                        strResult = "-"
                    End If
                End If

                Return strResult
            End Get
        End Property

    End Class
End Namespace