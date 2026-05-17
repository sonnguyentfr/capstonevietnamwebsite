'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************

Imports NVCMS.Modules.Student

Namespace NVCMS.Modules.EventsWebsite
    Public Class EventsStudentWebsiteInfo
        Private _id As Integer
        Private _EventId As Integer
        Private _EventCatId As Integer
        Private _StudentId As Integer
        Private _StudentCode As String
        Private _Source As Integer
        Private _Nguon As String
        Private _Nguoidikem As Integer
        Private _Checkin As Boolean
        Private _CreatedDate As DateTime
        Private _PortalId As Integer
        Private _CheckInDate As DateTime
        Private _UserId As Integer
        Private _Thamdu As Boolean
        Private _ThamduUserUpdate As Integer
        Private _ThamduDateUpdate As DateTime
        Private _Nguontutao As String
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
        Public Property EventId() As Integer
            Get
                Return _EventId
            End Get
            Set(ByVal Value As Integer)
                _EventId = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property EventCatId() As Integer
            Get
                Return _EventCatId
            End Get
            Set(ByVal Value As Integer)
                _EventCatId = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property StudentId() As Integer
            Get
                Return _StudentId
            End Get
            Set(ByVal Value As Integer)
                _StudentId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property StudentCode() As String
            Get
                Return _StudentCode
            End Get
            Set(ByVal Value As String)
                _StudentCode = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Source() As Integer
            Get
                Return _Source
            End Get
            Set(ByVal Value As Integer)
                _Source = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Nguon() As String
            Get
                Return _Nguon
            End Get
            Set(ByVal Value As String)
                _Nguon = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Nguontutao() As String
            Get
                Return _Nguontutao
            End Get
            Set(ByVal Value As String)
                _Nguontutao = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Nguoidikem() As Integer
            Get
                Return _Nguoidikem
            End Get
            Set(ByVal Value As Integer)
                _Nguoidikem = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Checkin() As Boolean
            Get
                Return _Checkin
            End Get
            Set(ByVal Value As Boolean)
                _Checkin = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Thamdu() As Boolean
            Get
                Return _Thamdu
            End Get
            Set(ByVal Value As Boolean)
                _Thamdu = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ThamduUserUpdate() As Integer
            Get
                Return _ThamduUserUpdate
            End Get
            Set(ByVal Value As Integer)
                _ThamduUserUpdate = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ThamduDateUpdate() As DateTime
            Get
                Return _ThamduDateUpdate
            End Get
            Set(ByVal Value As DateTime)
                _ThamduDateUpdate = Value
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
        Public Property PortalId() As Integer
            Get
                Return _PortalId
            End Get
            Set(ByVal Value As Integer)
                _PortalId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property CheckInDate() As DateTime
            Get
                Return _CheckInDate
            End Get
            Set(ByVal Value As DateTime)
                _CheckInDate = Value
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
        Public ReadOnly Property StudentGender() As Boolean
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .Sex
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public ReadOnly Property StudentSexIcon() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = IIf(CBool(.Sex) = True, "<i class=""ion-male""></i>", "<i class=""ion-female""></i>")
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        Public ReadOnly Property StudentFullname() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .Hotendem & " " & .Ten
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        Public ReadOnly Property StudentEmail() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .Email
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        Public ReadOnly Property StudentEmail_status() As Boolean
            Get
                Dim strResult As Boolean = False
                Dim ctl As New StudentInfoController
                Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                If Not obj Is Nothing Then
                    With obj
                        strResult = SuKienZ.IsValidEmail(.Email.Trim())
                    End With
                End If

                Return strResult
            End Get
        End Property
        Public ReadOnly Property StudentSodienthoai() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .Sodienthoai
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        'Public ReadOnly Property StudentDiachi() As String
        '    Get
        '        Dim strResult As String = String.Empty
        '        If strResult = "" Then
        '            Dim ctl As New StudentInfoController
        '            Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
        '            If Not obj Is Nothing Then
        '                With obj
        '                    strResult = st.GetLocationName(.Tinh)
        '                End With
        '            End If
        '        End If
        '        Return strResult
        '    End Get
        'End Property
        Public ReadOnly Property StudentNgaySinh() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = CDate(.Ngaysinh).ToString("dd/MM/yyyy")
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        Public ReadOnly Property StudentDanghoc() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .HocVanDanghoc
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        Public ReadOnly Property StudentDanghocTruong() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .HocVanTruongdanghoc
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property

        'Public ReadOnly Property StudentFollowUpStatus() As String
        '    Get
        '        Dim strResult As String = String.Empty
        '        If strResult = "" Then
        '            Dim ctl As New StudentInfoController
        '            Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
        '            If Not obj Is Nothing Then
        '                With obj
        '                    strResult = st.FollowStatusList(.FollowUpStatus)
        '                End With
        '            End If
        '        End If
        '        Return strResult
        '    End Get
        'End Property
        'Public ReadOnly Property StudentFollowUpPhuongThuc() As String
        '    Get
        '        Dim strResult As String = String.Empty
        '        If strResult = "" Then
        '            Dim ctl As New StudentInfoController
        '            Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
        '            If Not obj Is Nothing Then
        '                With obj
        '                    strResult = st.FollowGetPhuongThucKQ(st.ReplaceString(.FollowPhuongThuc))
        '                End With
        '            End If
        '        End If
        '        Return strResult
        '    End Get
        'End Property
        Public ReadOnly Property StudentFollowUpDate() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = CDate(.FollowUpDateUpdate).ToString("HH:mm - dd/MM/yyyy")
                        End With
                    End If
                End If
                Return strResult
            End Get
        End Property
        Public ReadOnly Property StudentFollowUpNoiDung() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctl As New StudentInfoController
                    Dim obj As StudentInfoInfo = ctl._Info_GetByID(StudentId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .FollowNoiDung
                        End With
                    End If
                End If

                Return strResult
            End Get
        End Property
    End Class
End Namespace