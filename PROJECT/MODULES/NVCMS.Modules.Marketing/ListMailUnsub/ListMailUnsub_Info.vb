'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Imports DotNetNuke.Common.Utilities

Namespace NVCMS.Modules.Marketing
    Public Class Marketing_Mail_ListMailUnsubInfo
        Private _id As Integer
        Private _Email As String
        Private _reason As Integer
        Private _created_date As DateTime
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
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal Value As String)
                _Email = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property reason() As Integer
            Get
                Return _reason
            End Get
            Set(ByVal Value As Integer)
                _reason = Value
            End Set
        End Property
        Public ReadOnly Property reasonname() As String
            Get
                Dim strCacheKey As String
                strCacheKey = "reasonname:" & reason
                Dim strResult As String = String.Empty
                strResult = DataCache.GetCache(strCacheKey)
                If strResult = "" Then
                    If reason = 1 Then
                        strResult = "I no longer want to receive these emails / Tôi không còn muốn nhận các email này"
                    End If
                    If reason = 2 Then
                        strResult = "I never signed up for this mailing list / Tôi chưa bao giờ đăng ký nhận danh sách gửi thư này"
                    End If
                    If reason = 3 Then
                        strResult = "The emails are inappropriate / Các email không phù hợp"
                    End If
                    If reason = 4 Then
                        strResult = "The emails are spam and should be reported / Các email là thư rác và nên được báo cáo"
                    End If
                    If reason = 5 Then
                        strResult = "Other (fill in reason below) / Lý do khác (điền lý do vào dưới đây)"
                    End If
                    DataCache.SetCache(strCacheKey, strResult)
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public Property created_date() As DateTime
            Get
                Return _created_date
            End Get
            Set(ByVal Value As DateTime)
                _created_date = Value
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