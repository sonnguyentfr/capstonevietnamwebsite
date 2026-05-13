'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.Video
    Public Class VideoByMediaInfo
        Private _id As Integer
        Private _newid As Integer
        Private _mediaid As Integer
        Private _createdted As DateTime
        Private _userid As Integer
        Private _portalid As Integer
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
        Public Property newid() As Integer
            Get
                Return _newid
            End Get
            Set(ByVal Value As Integer)
                _newid = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property mediaid() As Integer
            Get
                Return _mediaid
            End Get
            Set(ByVal Value As Integer)
                _mediaid = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property createdted() As DateTime
            Get
                Return _createdted
            End Get
            Set(ByVal Value As DateTime)
                _createdted = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property userid() As Integer
            Get
                Return _userid
            End Get
            Set(ByVal Value As Integer)
                _userid = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property portalid() As Integer
            Get
                Return _portalid
            End Get
            Set(ByVal Value As Integer)
                _portalid = Value
            End Set
        End Property
        'Viet them class chho nay
        Public ReadOnly Property ImageExtension() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "ImageExtensionNews:" & mediaid
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctlStatus As New MediaItemController
                    Dim obj As MediaItemInfo = ctlStatus._GetByID(mediaid)
                    If Not obj Is Nothing Then
                        strResult = obj.extension
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property MediaTitle() As String
            Get
                'Dim strCacheKey As String
                'strCacheKey = "MediaTitle" & mediaid
                Dim strResult As String = String.Empty
                'strResult = DataCache.GetCache(strCacheKey)
                'If strResult = "" Then
                Dim ctlStatus As New MediaItemController
                Dim obj As MediaItemInfo = ctlStatus._GetByID(mediaid)
                If Not obj Is Nothing Then
                    strResult = obj.title
                    'DataCache.SetCache(strCacheKey, strResult)
                End If
                'End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public ReadOnly Property ImageName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "ImageNameNews:" & mediaid
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctlStatus As New MediaItemController
                    Dim obj As MediaItemInfo = ctlStatus._GetByID(mediaid)
                    If Not obj Is Nothing Then
                        strResult = obj.filename
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public ReadOnly Property ImageFull() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "ImageFullNews:" & mediaid
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctlStatus As New MediaItemController
                    Dim obj As MediaItemInfo = ctlStatus._GetByID(mediaid)
                    If Not obj Is Nothing Then
                        strResult = obj.MediaUrl
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public ReadOnly Property ImageFullPhysic() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "ImageFullPhysic:" & mediaid
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctlStatus As New MediaItemController
                    Dim obj As MediaItemInfo = ctlStatus._GetByID(mediaid)
                    If Not obj Is Nothing Then
                        strResult = obj.forder
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
    End Class
End Namespace