'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Form
    Public Class Form_Info
        Private _id As Integer
        Private _Type As String
        Private _hinhthuc As String
        Private _vanphong As String
        Private _title As String
        Private _noidung As String
        Private _hovaten As String
        Private _email As String
        Private _sodienthoai As String
        Private _diachi As String
        Private _status As String
        Private _creatdate As DateTime
        Private _portalid As Integer
        Private _Formid As Integer
        Private _repuserid As Integer
        Private _repcreateddate As DateTime
        Private _reptitle As String
        Private _repnoidung As String

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
        Public Property Type() As String
            Get
                Return _Type
            End Get
            Set(ByVal Value As String)
                _Type = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property hinhthuc() As String
            Get
                Return _hinhthuc
            End Get
            Set(ByVal Value As String)
                _hinhthuc = Value
            End Set
        End Property
        '------------------------------------------'
        Public ReadOnly Property hinhthucName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "hinhthucName:" & hinhthuc
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    If hinhthuc = "TUVANDUHOC" Then
                        strResult = "Tư vấn du học"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                    If hinhthuc = "DINHHUONGNGHENGHIEP" Then
                        strResult = "Định hướng nghề nghiệp"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                    If hinhthuc = "DINHCU" Then
                        strResult = "Định cư"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public Property vanphong() As String
            Get
                Return _vanphong
            End Get
            Set(ByVal Value As String)
                _vanphong = Value
            End Set
        End Property
        '------------------------------------------'
        Public ReadOnly Property vanphongName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "vanphongName:" & vanphong
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    If vanphong = "HN" Then
                        strResult = "Hà nội"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                    If vanphong = "HCM" Then
                        strResult = "Hồ Chí Minh"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public Property title() As String
            Get
                Return _title
            End Get
            Set(ByVal Value As String)
                _title = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property noidung() As String
            Get
                Return _noidung
            End Get
            Set(ByVal Value As String)
                _noidung = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property hovaten() As String
            Get
                Return _hovaten
            End Get
            Set(ByVal Value As String)
                _hovaten = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property email() As String
            Get
                Return _email
            End Get
            Set(ByVal Value As String)
                _email = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property sodienthoai() As String
            Get
                Return _sodienthoai
            End Get
            Set(ByVal Value As String)
                _sodienthoai = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property diachi() As String
            Get
                Return _diachi
            End Get
            Set(ByVal Value As String)
                _diachi = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property status() As String
            Get
                Return _status
            End Get
            Set(ByVal Value As String)
                _status = Value
            End Set
        End Property
        Public ReadOnly Property StatusName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "StatusName:" & status
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    If status = "VUATIEPNHAN" Then
                        strResult = "vừa tiếp nhận"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                    If status = "DANGXULY" Then
                        strResult = "Đang xử lý"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                    If status = "DATRALOIEMAIL" Then
                        strResult = "Đã trả lời Email"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property TypeName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "TypeName:" & Type
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    If Type = "LIENHE" Then
                        strResult = "Liên hệ"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                    If Type = "HOTRO" Then
                        strResult = "Hỗ trợ"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                    If Type = "TUVAN" Then
                        strResult = "Đăng ký Tư vấn"
                        DataCache.SetCache(strCacheKey, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public Property creatdate() As DateTime
            Get
                Return _creatdate
            End Get
            Set(ByVal Value As DateTime)
                _creatdate = Value
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
        '------------------------------------------'
        Public Property Formid() As Integer
            Get
                Return _Formid
            End Get
            Set(ByVal Value As Integer)
                _Formid = Value
            End Set
        End Property
        Public Property repuserid() As Integer
            Get
                Return _repuserid
            End Get
            Set(ByVal Value As Integer)
                _repuserid = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property repcreateddate() As DateTime
            Get
                Return _repcreateddate
            End Get
            Set(ByVal Value As DateTime)
                _repcreateddate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property reptitle() As String
            Get
                Return _reptitle
            End Get
            Set(ByVal Value As String)
                _reptitle = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property repnoidung() As String
            Get
                Return _repnoidung
            End Get
            Set(ByVal Value As String)
                _repnoidung = Value
            End Set
        End Property
        '------------------------------------------'
    End Class
End Namespace