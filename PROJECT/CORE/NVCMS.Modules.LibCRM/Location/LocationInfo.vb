'******************************************
'Author         :VEPOneGenCode 
'Created Date   :4/23/2008
'Comment        :Lop co so dung cho viec ke thua de  
'               :Thao tac voi da CSDL 
'History        : 
'******************************************
Namespace NVCMS.Modules.LibCRM
    Public Class LibLocationInfo
        Private _id As Integer
        Private _Name As String
        Private _ShortName As String
        Private _currency As String
        Private _currencyName As String
        Private _currencyCode As String
        Private _currencyShowfull As String
        Private _PostCode As String
        Private _ParentId As Integer
        Private _Status As Boolean
        Private _Ordernumber As Integer
        Private _mapLatitude As String
        Private _mapLongitude As String
        Private _Info As String
        Private _PortalId As Integer
        Private _CreatedDate As DateTime


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
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal Value As String)
                _Name = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property ShortName() As String
            Get
                Return _ShortName
            End Get
            Set(ByVal Value As String)
                _ShortName = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property currency() As String
            Get
                Return _currency
            End Get
            Set(ByVal Value As String)
                _currency = Value
            End Set
        End Property
        Public Property currencyName() As String
            Get
                Return _currencyName
            End Get
            Set(ByVal Value As String)
                _currencyName = Value
            End Set
        End Property
        '------------------------------------------'
        Public ReadOnly Property currencyShowfull() As String
            Get
                Dim strResult As String = String.Empty
                If strResult = "" Then
                    Dim ctlStatus As New LibLocationController
                    Dim obj As LibLocationInfo = ctlStatus.Location_GetByID(id, PortalId)
                    If Not obj Is Nothing Then
                        With obj
                            strResult = .Name & " - (" & .currencyName & ": " & .currency & ")"
                        End With
                        'DataCache.SetCache(strCacheKeyt1, strResult)
                    End If
                End If

                Return strResult
            End Get
        End Property
        '------------------------------------------'
        Public Property currencyCode() As String
            Get
                Return _currencyCode
            End Get
            Set(ByVal Value As String)
                _currencyCode = Value
            End Set
        End Property
        Public Property PostCode() As String
            Get
                Return _PostCode
            End Get
            Set(ByVal Value As String)
                _PostCode = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property ParentId() As Integer
            Get
                Return _ParentId
            End Get
            Set(ByVal Value As Integer)
                _ParentId = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Status() As Boolean
            Get
                Return _Status
            End Get
            Set(ByVal Value As Boolean)
                _Status = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property Ordernumber() As Integer
            Get
                Return _Ordernumber
            End Get
            Set(ByVal Value As Integer)
                _Ordernumber = Value
            End Set
        End Property
        '------------------------------------------'
        Public Property mapLatitude() As String
            Get
                Return _mapLatitude
            End Get
            Set(ByVal Value As String)
                _mapLatitude = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property mapLongitude() As String
            Get
                Return _mapLongitude
            End Get
            Set(ByVal Value As String)
                _mapLongitude = Value
            End Set
        End Property

        '------------------------------------------'
        Public Property Info() As String
            Get
                Return _Info
            End Get
            Set(ByVal Value As String)
                _Info = Value
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
        Public Property CreatedDate() As DateTime
            Get
                Return _CreatedDate
            End Get
            Set(ByVal Value As DateTime)
                _CreatedDate = Value
            End Set
        End Property

        '------------------------------------------'
    End Class
End Namespace