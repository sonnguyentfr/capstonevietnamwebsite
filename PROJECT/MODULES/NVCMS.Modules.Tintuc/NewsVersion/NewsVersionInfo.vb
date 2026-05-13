'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/21/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.TinTuc
    Public Class NewsVersionInfo
        Private _Id As Integer
        Private _CreatedUser As Integer
        Private _NewId As Integer
        Private _CategoryId As Integer
        Private _Title As String
        Private _ImagePath As String
        Private _Summary As String
        Private _Content As String
        Private _isActive As Boolean
        Private _Hotcat As Boolean
        Private _Hotsite As Boolean
        Private _Status As Integer
        Private _Unit As Integer
        Private _NewsKind As Integer
        Private _Type As Integer
        Private _TypeUrl As String
        Private _Links As String
        Private _Tags As String
        Private _IsImage As Boolean
        Private _Note As String
        Private _SourceInfo As Integer
        Private _StorageFolder As String
        Private _AttachedFiles As String
        Private _IsEdited As Boolean
        Private _EditedUser As Integer
        Private _EditedTime As DateTime
        Private _VoteCount As Integer
        Private _ViewCount As Integer
        Private _IsArchived As Boolean
        Private _ArchivedDate As DateTime
        Private _Credit As Integer
        Private _CreateDate As DateTime
        Private _ApprovalRequestDate As DateTime
        Private _ApprovalDate As DateTime
        Private _ApprovalUser As Integer
        Private _ReturnedDate As DateTime
        Private _ReturnedUser As Integer
        Private _CancelPublishDate As DateTime
        Private _CancelPublishUser As Integer
        Private _PublishedDate As DateTime
        Private _PublishedUser As Integer
        Private _PortalId As Integer
        Private _UserId As Integer

        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal Value As Integer)
                _Id = Value
            End Set
        End Property
        Public Property CreatedUser() As Integer
            Get
                Return _CreatedUser
            End Get
            Set(ByVal Value As Integer)
                _CreatedUser = Value
            End Set
        End Property

        Public Property NewId() As Integer
            Get
                Return _NewId
            End Get
            Set(ByVal Value As Integer)
                _NewId = Value
            End Set
        End Property
        Public Property CategoryId() As Integer
            Get
                Return _CategoryId
            End Get
            Set(ByVal Value As Integer)
                _CategoryId = Value
            End Set
        End Property
        Public Property Title() As String
            Get
                Return _Title
            End Get
            Set(ByVal Value As String)
                _Title = Value
            End Set
        End Property
        Public Property ImagePath() As String
            Get
                Return _ImagePath
            End Get
            Set(ByVal Value As String)
                _ImagePath = Value
            End Set
        End Property
        Public Property Summary() As String
            Get
                Return _Summary
            End Get
            Set(ByVal Value As String)
                _Summary = Value
            End Set
        End Property
        Public Property Content() As String
            Get
                Return _Content
            End Get
            Set(ByVal Value As String)
                _Content = Value
            End Set
        End Property
        Public Property isActive() As Boolean
            Get
                Return _isActive
            End Get
            Set(ByVal Value As Boolean)
                _isActive = Value
            End Set
        End Property
        Public Property Hotcat() As Boolean
            Get
                Return _Hotcat
            End Get
            Set(ByVal Value As Boolean)
                _Hotcat = Value
            End Set
        End Property
        Public Property Hotsite() As Boolean
            Get
                Return _Hotsite
            End Get
            Set(ByVal Value As Boolean)
                _Hotsite = Value
            End Set
        End Property
        Public Property PortalId() As Integer
            Get
                Return _PortalId
            End Get
            Set(ByVal Value As Integer)
                _PortalId = Value
            End Set
        End Property
        Public Property UserId() As Integer
            Get
                Return _UserId
            End Get
            Set(ByVal Value As Integer)
                _UserId = Value
            End Set
        End Property
        Public Property Status() As Integer
            Get
                Return _Status
            End Get
            Set(ByVal Value As Integer)
                _Status = Value
            End Set
        End Property
        Public Property Note() As String
            Get
                Return _Note
            End Get
            Set(ByVal value As String)
                _Note = value
            End Set
        End Property
        Public Property TypeUrl() As String
            Get
                Return _TypeUrl
            End Get
            Set(ByVal Value As String)
                _TypeUrl = Value
            End Set
        End Property
        Public Property CreateDate() As DateTime
            Get
                Return _CreateDate
            End Get
            Set(ByVal Value As DateTime)
                _CreateDate = Value
            End Set
        End Property
        Public Property ApprovalRequestDate() As DateTime
            Get
                Return _ApprovalRequestDate
            End Get
            Set(ByVal Value As DateTime)
                _ApprovalRequestDate = Value
            End Set
        End Property
        Public Property ApprovalDate() As DateTime
            Get
                Return _ApprovalDate
            End Get
            Set(ByVal Value As DateTime)
                _ApprovalDate = Value
            End Set
        End Property
        Public Property ApprovalUser() As Integer
            Get
                Return _ApprovalUser
            End Get
            Set(ByVal Value As Integer)
                _ApprovalUser = Value
            End Set
        End Property
        Public Property ReturnedDate() As DateTime
            Get
                Return _ReturnedDate
            End Get
            Set(ByVal Value As DateTime)
                _ReturnedDate = Value
            End Set
        End Property
        Public Property ReturnedUser() As Integer
            Get
                Return _ReturnedUser
            End Get
            Set(ByVal Value As Integer)
                _ReturnedUser = Value
            End Set
        End Property
        Public Property CancelPublishDate() As DateTime
            Get
                Return _CancelPublishDate
            End Get
            Set(ByVal Value As DateTime)
                _CancelPublishDate = Value
            End Set
        End Property
        Public Property CancelPublishUser() As Integer
            Get
                Return _CancelPublishUser
            End Get
            Set(ByVal Value As Integer)
                _CancelPublishUser = Value
            End Set
        End Property
        Public Property PublishedDate() As DateTime
            Get
                Return _PublishedDate
            End Get
            Set(ByVal value As DateTime)
                _PublishedDate = value
            End Set
        End Property
        Public Property PublishedUser() As Integer
            Get
                Return _PublishedUser
            End Get
            Set(ByVal Value As Integer)
                _PublishedUser = Value
            End Set
        End Property
        Public Property SourceInfo() As Integer
            Get
                Return _SourceInfo
            End Get
            Set(ByVal Value As Integer)
                _SourceInfo = Value
            End Set
        End Property
        Public Property Unit() As Integer
            Get
                Return _Unit
            End Get
            Set(ByVal Value As Integer)
                _Unit = Value
            End Set
        End Property
        Public Property Type() As Integer
            Get
                Return _Type
            End Get
            Set(ByVal value As Integer)
                _Type = value
            End Set
        End Property
        Public Property NewsKind() As Integer
            Get
                Return _NewsKind
            End Get
            Set(ByVal value As Integer)
                _NewsKind = value
            End Set
        End Property
        Public Property Tags() As String
            Get
                Return _Tags
            End Get
            Set(ByVal value As String)
                _Tags = value
            End Set
        End Property
        Public Property IsImage() As Boolean
            Get
                Return _IsImage
            End Get
            Set(ByVal value As Boolean)
                _IsImage = value
            End Set
        End Property
        Public Property IsEdited() As Boolean
            Get
                Return _IsEdited
            End Get
            Set(ByVal value As Boolean)
                _IsEdited = value
            End Set
        End Property
        Public Property StorageFolder() As String
            Get
                Return _StorageFolder
            End Get
            Set(ByVal value As String)
                _StorageFolder = value
            End Set
        End Property
        Public Property AttachedFiles() As String
            Get
                Return _AttachedFiles
            End Get
            Set(ByVal value As String)
                _AttachedFiles = value
            End Set
        End Property
        Public Property EditedUser() As Integer
            Get
                Return _EditedUser
            End Get
            Set(ByVal Value As Integer)
                _EditedUser = Value
            End Set
        End Property
        Public Property EditedTime() As DateTime
            Get
                Return _EditedTime
            End Get
            Set(ByVal value As DateTime)
                _EditedTime = value
            End Set
        End Property
        Public Property Credit() As Integer
            Get
                Return _Credit
            End Get
            Set(ByVal Value As Integer)
                _Credit = Value
            End Set
        End Property
        Public Property VoteCount() As Integer
            Get
                Return _VoteCount
            End Get
            Set(ByVal Value As Integer)
                _VoteCount = Value
            End Set
        End Property
        Public Property ViewCount() As Integer
            Get
                Return _ViewCount
            End Get
            Set(ByVal Value As Integer)
                _ViewCount = Value
            End Set
        End Property
        Public Property Links() As String
            Get
                Return _Links
            End Get
            Set(ByVal value As String)
                _Links = value
            End Set
        End Property
        Public Property IsArchived() As Boolean
            Get
                Return _IsArchived
            End Get
            Set(ByVal value As Boolean)
                _IsArchived = value
            End Set
        End Property
        Public Property ArchivedDate() As DateTime
            Get
                Return _ArchivedDate
            End Get
            Set(ByVal value As DateTime)
                _ArchivedDate = value
            End Set
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
                        strResult = "Tin ảnh"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
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
        Public ReadOnly Property IsEditable() As Boolean
            Get
                ' 0: dang bien soan,1 cho phe duyet, 2 da phe duyet, 3 tra lai, 4 Huy phe duyet 
                Select Case Status
                    Case NewsStatus.DangBienSoan, NewsStatus.BiTraLai, NewsStatus.HuyXuatBan
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property
        Public ReadOnly Property IsActionable() As Boolean
            Get
                ' 0: dang bien soan,1 cho phe duyet, 2 da phe duyet, 3 tra lai, 4 Huy phe duyet 
                Select Case Status
                    Case 1, 4 ' Tin bai: CHO PHE DUYET -> Ko cho sua
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property
        Public ReadOnly Property IsApprovalState() As Boolean
            Get
                Select Case Status
                    Case NewsStatus.ChoPheDuyet, NewsStatus.BiTraLai, NewsStatus.HuyXuatBan ' Tin bai: CHO PHE DUYET -> Ko cho sua
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property
        Public ReadOnly Property IsPublishedState() As Boolean
            Get
                Select Case Status
                    Case NewsStatus.ChoXuatBan
                        Return True
                    Case Else
                        Return False
                End Select
            End Get
        End Property
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
    End Class
End Namespace