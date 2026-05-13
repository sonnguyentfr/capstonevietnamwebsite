'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/28/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.TinTuc
Namespace NVCMS.Modules.School
    Public Class NewsBySchoolInfo
        Private _Id As Integer
        Private _NewId As Integer
        Private _SchoolId As Integer

        Dim _NV_NewsController As New NV_NewsController
        Dim _NV_NewsCategoriesController As New NV_NewsCategoriesController
        Dim _MarketingSchoolController As New MarketingSchoolController
        '------------------------------------------'
        Public Property Id() As Integer
            Get
                Return _Id
            End Get
            Set(ByVal Value As Integer)
                _Id = Value
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
        Public Property SchoolId() As Integer
            Get
                Return _SchoolId
            End Get
            Set(ByVal Value As Integer)
                _SchoolId = Value
            End Set
        End Property
        Public ReadOnly Property SchoolName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "SchoolIdName:" & SchoolId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obj As MarketingSchoolInfo = _MarketingSchoolController.Marketing_Truong_GetByID(SchoolId)
                    If Not obj Is Nothing Then
                        strResult = obj.NameofSchool
                    Else
                        strResult = "-"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property SchoolLogo() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "SchoolIdLogo:" & SchoolId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obj As MarketingSchoolInfo = _MarketingSchoolController.Marketing_Truong_GetByID(SchoolId)
                    If Not obj Is Nothing Then
                        strResult = obj.Logo
                    Else
                        strResult = "-"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public ReadOnly Property NewsTitle() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NewsBySchoolTitle:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obj As NV_NewsInfo = _NV_NewsController.GetByID(NewId)
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
        Public ReadOnly Property NewsSummary() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NewsBySchoolSummary:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim ctl As New NV_NewsController
                    Dim obj As NV_NewsInfo = _NV_NewsController.GetByID(NewId)
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
        Public ReadOnly Property NewsImagePath() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NewsBySchoolImagePath:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obj As NV_NewsInfo = _NV_NewsController.GetByID(NewId)
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
        Public ReadOnly Property NewsCategoryId() As Integer
            Get
                Dim strCacheKey As String
                strCacheKey = "NewsBySchoolCategoryId:" & NewId
                Dim strResult As Integer = 0
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obj As NV_NewsInfo = _NV_NewsController.GetByID(NewId)
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
        Public ReadOnly Property NewsCategoryName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NewsCategoryName:" & NewId
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim obj As NV_NewsInfo = _NV_NewsController.GetByID(NewId)
                    If Not obj Is Nothing Then

                        Dim objcat As NV_NewsCategoriesInfo = _NV_NewsCategoriesController.GetByID(obj.CategoryId)
                        If Not objcat Is Nothing Then
                            With obj
                                strResult = objcat.CategoryName
                            End With
                        End If
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