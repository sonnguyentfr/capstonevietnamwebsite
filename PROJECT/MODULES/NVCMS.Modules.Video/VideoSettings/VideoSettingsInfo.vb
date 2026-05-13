'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Video
    Public Class VideoSettingsInfo
        Private _id As Integer
        Private _Title As Integer
        Private _CategoryId As Integer
        Private _CategoryName As String
        Private _ImagePath As String
        Private _Summary As String
        Private _PublishedDate As DateTime
        Private _VideoId As Integer
        Private _OrderNumber As Integer
        Private _Type As Integer
        Private _PortalId As Integer


        '------------------------------------------'
        Public Property id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal Value As Integer)
                _id = Value
            End Set
        End Property
        Public ReadOnly Property Title() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "VideoSettingsTitle:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New Videos_Controller
                    Dim obj As Videos_Info = ctl.GetByID(VideoId, PortalId)
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
        Public ReadOnly Property Summary() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "VideoSettingsSummary:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New Videos_Controller
                    Dim obj As Videos_Info = ctl.GetByID(VideoId, PortalId)
                    If Not obj Is Nothing Then
                        strResult = obj.Summary
                    Else
                        strResult = "-"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property ImagePath() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "VideoSettingsImagePath:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New Videos_Controller
                    Dim obj As Videos_Info = ctl.GetByID(VideoId, PortalId)
                    If Not obj Is Nothing Then
                        strResult = obj.ImagePath
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
                Dim strResult As Integer = 0
                Dim ctl As New Videos_Controller
                Dim obj As Videos_Info = ctl.GetByID(VideoId, PortalId)
                If Not obj Is Nothing Then
                    strResult = obj.CategoryId
                Else
                    strResult = 0
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property PublishedDate() As DateTime
            Get
                Dim strResult As DateTime = DateTime.Now
                Dim ctl As New Videos_Controller
                Dim obj As Videos_Info = ctl.GetByID(VideoId, PortalId)
                If Not obj Is Nothing Then
                    strResult = obj.PublishedDate
                Else
                    strResult = DateTime.Now
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property CategoryName() As String
            Get
                Dim strResult As String = ""
                Dim ctl As New Videos_Controller
                Dim obj As Videos_Info = ctl.GetByID(VideoId, PortalId)
                If Not obj Is Nothing Then
                    strResult = obj.CategoryName
                Else
                    strResult = ""
                End If

                Return strResult
            End Get
        End Property
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
        Public Property OrderNumber() As Integer
            Get
                Return _OrderNumber
            End Get
            Set(ByVal Value As Integer)
                _OrderNumber = Value
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
        Public Property PortalId() As Integer
            Get
                Return _PortalId
            End Get
            Set(ByVal Value As Integer)
                _PortalId = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace