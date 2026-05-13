'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.TinTuc
    Public Class NewsByTagsInfo
        Private _id As Integer
        Private _NewId As Integer
        Private _Tags As String
        Private _TagsTitle As String
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
        Public Property Tags() As String
            Get
                Return _Tags
            End Get
            Set(ByVal Value As String)
                _Tags = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property TagsTitle() As String
            Get
                Return _TagsTitle
            End Get
            Set(ByVal Value As String)
                _TagsTitle = Value
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
        Public ReadOnly Property CategoryId() As Integer
            Get
                Dim strResult As Integer = 0
                Dim ctl As New NV_NewsController
                Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                If Not obj Is Nothing Then
                    strResult = obj.CategoryId
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
                        strResult = ""
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
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
                        strResult = ""
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property ImagePath() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "ImagePath:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.ImagePath
                    Else
                        strResult = ""
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property PublishedDate() As DateTime
            Get
                Dim strCacheKey As String
                strCacheKey = "PublishedDate:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = ctl.GetByID(NewId)
                    If Not obj Is Nothing Then
                        strResult = obj.PublishedDate
                    Else
                        strResult = DateTime.Now
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
    End Class
End Namespace