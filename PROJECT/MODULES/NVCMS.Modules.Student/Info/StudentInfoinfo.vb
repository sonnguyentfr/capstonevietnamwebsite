'******************************************
'Author         :SonNguyen
'Created Date   :3/21/2010
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities
Imports NVCMS.Modules.LibCRM

Namespace NVCMS.Modules.Student
    Public Class StudentInfoInfo
        Private _id As Integer
        Private _CODE As String
        Private _VP As Integer
        Private _Type As Integer
        Private _Hotendem As String
        Private _Ten As String
        Private _fullname As String
        Private _Sex As Boolean
        Private _Ngaysinh As DateTime
        Private _kieuNgaysinh As Integer
        Private _Sodienthoai As String
        Private _Email As String
        Private _Diachi As String
        Private _Tinh As Integer
        Private _Huyen As Integer
        Private _EB5 As Boolean
        Private _PermissionUser As String
        Private _AdviserId As Integer
        Private _FollowPhuongThuc As Integer
        Private _FollowKetQua As Integer
        Private _FollowNoiDung As String
        Private _FollowUpStatus As Integer
        Private _FollowUpDateUpdate As DateTime
        Private _FollowUpUser As Integer
        Private _TuVanHocVanmongmuon As String
        Private _TuVanNamdi As String
        Private _TuVanKyhoc As String
        Private _TuVanNganhhoc As String
        Private _TuVanTruongdukien As String
        Private _TuVanQuocgia As String
        Private _TuVanDiadiem As Integer
        Private _TuVanKhanangchitra As Integer
        Private _TuVanKhac As String
        Private _TuVanEditUserId As Integer
        Private _TuVanEditDate As DateTime
        Private _TuVanApproveUserId As Integer
        Private _TuVanApproveDate As DateTime
        Private _HocVanDanghoc As String
        Private _HocVanTruongdanghoc As String
        Private _HocVanDiemtrungbinh As String
        Private _HocVanDiemsobaithichuanhoa As String
        Private _HocVanLuuy As String
        Private _HocVanEditUserId As Integer
        Private _HocVanEditDate As DateTime
        Private _HocVanApproveUserId As Integer
        Private _HocVanApproveDate As DateTime
        Private _Kyhopdong As Boolean
        Private _HinhThuc As Integer
        Private _CreatedDate As DateTime
        Private _UserId As Integer
        Private _PortalId As Integer
        Private _Xoa As Boolean
        Private _isSpy As Boolean
        Private _dongyguithongtin As Boolean
        Private _direct As Boolean
        Private _nhom As Integer
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
        Public Property CODE() As String
            Get
                Return _CODE
            End Get
            Set(ByVal Value As String)
                _CODE = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property VP() As Integer
            Get
                Return _VP
            End Get
            Set(ByVal Value As Integer)
                _VP = Value
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
        Public Property Hotendem() As String
            Get
                Return _Hotendem
            End Get
            Set(ByVal Value As String)
                _Hotendem = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Ten() As String
            Get
                Return _Ten
            End Get
            Set(ByVal Value As String)
                _Ten = Value
            End Set
        End Property

        '------------------------------------------'
        Public ReadOnly Property Fullname() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctlStatus As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctlStatus._Info_GetByID(id)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .Hotendem & " " & .Ten
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public Property Sex() As Boolean
            Get
                Return _Sex
            End Get
            Set(ByVal Value As Boolean)
                _Sex = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Ngaysinh() As DateTime
            Get
                Return _Ngaysinh
            End Get
            Set(ByVal Value As DateTime)
                _Ngaysinh = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property KieuNgaysinh() As Integer
            Get
                Return _kieuNgaysinh
            End Get
            Set(ByVal Value As Integer)
                _kieuNgaysinh = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Sodienthoai() As String
            Get
                Return _Sodienthoai
            End Get
            Set(ByVal Value As String)
                _Sodienthoai = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal Value As String)
                _Email = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Diachi() As String
            Get
                Return _Diachi
            End Get
            Set(ByVal Value As String)
                _Diachi = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Tinh() As Integer
            Get
                Return _Tinh
            End Get
            Set(ByVal Value As Integer)
                _Tinh = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Huyen() As Integer
            Get
                Return _Huyen
            End Get
            Set(ByVal Value As Integer)
                _Huyen = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property EB5() As Boolean
            Get
                Return _EB5
            End Get
            Set(ByVal Value As Boolean)
                _EB5 = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property PermissionUser() As String
            Get
                Return _PermissionUser
            End Get
            Set(ByVal Value As String)
                _PermissionUser = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property AdviserId() As Integer
            Get
                Return _AdviserId
            End Get
            Set(ByVal Value As Integer)
                _AdviserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FollowPhuongThuc() As Integer
            Get
                Return _FollowPhuongThuc
            End Get
            Set(ByVal Value As Integer)
                _FollowPhuongThuc = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FollowKetQua() As Integer
            Get
                Return _FollowKetQua
            End Get
            Set(ByVal Value As Integer)
                _FollowKetQua = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FollowNoiDung() As String
            Get
                Return _FollowNoiDung
            End Get
            Set(ByVal Value As String)
                _FollowNoiDung = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FollowUpStatus() As Integer
            Get
                Return _FollowUpStatus
            End Get
            Set(ByVal Value As Integer)
                _FollowUpStatus = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property FollowUpDateUpdate() As DateTime
            Get
                Return _FollowUpDateUpdate
            End Get
            Set(ByVal Value As DateTime)
                _FollowUpDateUpdate = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property FollowUpUser() As Integer
            Get
                Return _FollowUpUser
            End Get
            Set(ByVal Value As Integer)
                _FollowUpUser = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property TuVanHocVanmongmuon() As String
            Get
                Return _TuVanHocVanmongmuon
            End Get
            Set(ByVal Value As String)
                _TuVanHocVanmongmuon = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanNamdi() As String
            Get
                Return _TuVanNamdi
            End Get
            Set(ByVal Value As String)
                _TuVanNamdi = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanKyhoc() As String
            Get
                Return _TuVanKyhoc
            End Get
            Set(ByVal Value As String)
                _TuVanKyhoc = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanNganhhoc() As String
            Get
                Return _TuVanNganhhoc
            End Get
            Set(ByVal Value As String)
                _TuVanNganhhoc = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanTruongdukien() As String
            Get
                Return _TuVanTruongdukien
            End Get
            Set(ByVal Value As String)
                _TuVanTruongdukien = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanQuocgia() As String
            Get
                Return _TuVanQuocgia
            End Get
            Set(ByVal Value As String)
                _TuVanQuocgia = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanDiadiem() As Integer
            Get
                Return _TuVanDiadiem
            End Get
            Set(ByVal Value As Integer)
                _TuVanDiadiem = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanKhanangchitra() As Integer
            Get
                Return _TuVanKhanangchitra
            End Get
            Set(ByVal Value As Integer)
                _TuVanKhanangchitra = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property TuVanKhac() As String
            Get
                Return _TuVanKhac
            End Get
            Set(ByVal Value As String)
                _TuVanKhac = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property TuVanEditUserId() As Integer
            Get
                Return _TuVanEditUserId
            End Get
            Set(ByVal Value As Integer)
                _TuVanEditUserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanEditDate() As DateTime
            Get
                Return _TuVanEditDate
            End Get
            Set(ByVal Value As DateTime)
                _TuVanEditDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanApproveUserId() As Integer
            Get
                Return _TuVanApproveUserId
            End Get
            Set(ByVal Value As Integer)
                _TuVanApproveUserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property TuVanApproveDate() As DateTime
            Get
                Return _TuVanApproveDate
            End Get
            Set(ByVal Value As DateTime)
                _TuVanApproveDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanDanghoc() As String
            Get
                Return _HocVanDanghoc
            End Get
            Set(ByVal Value As String)
                _HocVanDanghoc = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanTruongdanghoc() As String
            Get
                Return _HocVanTruongdanghoc
            End Get
            Set(ByVal Value As String)
                _HocVanTruongdanghoc = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanDiemtrungbinh() As String
            Get
                Return _HocVanDiemtrungbinh
            End Get
            Set(ByVal Value As String)
                _HocVanDiemtrungbinh = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanDiemsobaithichuanhoa() As String
            Get
                Return _HocVanDiemsobaithichuanhoa
            End Get
            Set(ByVal Value As String)
                _HocVanDiemsobaithichuanhoa = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanLuuy() As String
            Get
                Return _HocVanLuuy
            End Get
            Set(ByVal Value As String)
                _HocVanLuuy = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanEditUserId() As Integer
            Get
                Return _HocVanEditUserId
            End Get
            Set(ByVal Value As Integer)
                _HocVanEditUserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanEditDate() As DateTime
            Get
                Return _HocVanEditDate
            End Get
            Set(ByVal Value As DateTime)
                _HocVanEditDate = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanApproveUserId() As Integer
            Get
                Return _HocVanApproveUserId
            End Get
            Set(ByVal Value As Integer)
                _HocVanApproveUserId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property HocVanApproveDate() As DateTime
            Get
                Return _HocVanApproveDate
            End Get
            Set(ByVal Value As DateTime)
                _HocVanApproveDate = Value
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
        Public Property HinhThuc() As Integer
            Get
                Return _HinhThuc
            End Get
            Set(ByVal Value As Integer)
                _HinhThuc = Value
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
        Public Property UserId() As Integer
            Get
                Return _UserId
            End Get
            Set(ByVal Value As Integer)
                _UserId = Value
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
        Public Property Xoa() As Boolean
            Get
                Return _Xoa
            End Get
            Set(ByVal Value As Boolean)
                _Xoa = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property isSpy() As Boolean
            Get
                Return _isSpy
            End Get
            Set(ByVal Value As Boolean)
                _isSpy = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property dongyguithongtin() As Boolean
            Get
                Return _dongyguithongtin
            End Get
            Set(ByVal Value As Boolean)
                _dongyguithongtin = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property direct() As Boolean
            Get
                Return _direct
            End Get
            Set(ByVal Value As Boolean)
                _direct = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property nhom() As Integer
            Get
                Return _nhom
            End Get
            Set(ByVal Value As Integer)
                _nhom = Value
            End Set
        End Property
        '------------------------------------------'
        Public ReadOnly Property Tags() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    'Dim ctlStatus As New StudentByTagController
                    'Dim arr As ArrayList = ctlStatus.Student_ByTags_GetByStudentID(id)
                    'If Not arr Is Nothing AndAlso arr.Count > 0 Then
                    '    For i As Integer = 0 To arr.Count - 1
                    '        Dim obj As StudentByTagInfo = CType(arr(i), StudentByTagInfo)
                    '        strResult += "<span class='stagsz'>" & st.GetStudentTagName(obj.titlelow) & "</span>"
                    '    Next
                    'End If
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property NhomName() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "NhomName:" & id
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    Dim _FollowUpTrangThaiNhomController As New FollowUpTrangThaiNhomController
                    Dim obj As FollowUpTrangThaiNhomInfo = _FollowUpTrangThaiNhomController.Follow_TrangThaiNhom_GetByID(nhom)
                    If Not obj Is Nothing Then
                        strResult = obj.TenNhom
                    Else
                        strResult = ""
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
    End Class
End Namespace