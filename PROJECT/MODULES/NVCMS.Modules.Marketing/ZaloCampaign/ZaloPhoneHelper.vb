Imports System.Text.RegularExpressions

Namespace NVCMS.Modules.Marketing

    ''' <summary>
    ''' Xu ly va chuan hoa so dien thoai Viet Nam cho Zalo
    ''' Chuyen tu 09x/03x/07x/08x -> 849x/843x/847x/848x
    ''' </summary>
    Public Class ZaloPhoneHelper

        ''' <summary>
        ''' Chuyen so dien thoai sang dinh dang quoc te 84xxx
        ''' </summary>
        Public Shared Function NormalizePhone(ByVal raw As String) As String
            If String.IsNullOrWhiteSpace(raw) Then Return String.Empty

            ' Bo trang, dau gach, dau cach, dau cham
            Dim phone As String = Regex.Replace(raw.Trim(), "[.\-\s\(\)]", "")

            ' Bo tien to + hoac 00
            If phone.StartsWith("+") Then phone = phone.Substring(1)
            If phone.StartsWith("00") Then phone = phone.Substring(2)

            ' Neu da la 84xxxxxxxxx (10-11 chu so) -> giu nguyen
            If phone.StartsWith("84") AndAlso phone.Length >= 11 AndAlso phone.Length <= 12 Then
                Return phone
            End If

            ' Chuyen 0xxxxxxxxx (10 chu so) -> 84xxxxxxxxx
            If phone.StartsWith("0") AndAlso phone.Length = 10 Then
                Return "84" & phone.Substring(1)
            End If

            ' Truong hop 9 chu so (thieu so 0 dau)
            If phone.Length = 9 AndAlso IsDigitsOnly(phone) Then
                Return "84" & phone
            End If

            Return phone
        End Function

        ''' <summary>
        ''' Kiem tra so dien thoai Viet Nam hop le (sau khi chuan hoa)
        ''' </summary>
        Public Shared Function IsValidPhone(ByVal phone As String) As Boolean
            If String.IsNullOrWhiteSpace(phone) Then Return False
            If Not IsDigitsOnly(phone) Then Return False

            ' Dang quoc te: 84 + 9 chu so = 11 chu so
            If phone.StartsWith("84") AndAlso phone.Length = 11 Then
                Dim prefix As String = phone.Substring(0, 4)
                Return IsValidVietnamPrefix(prefix)
            End If

            ' Dang noi dia: 0 + 9 chu so = 10 chu so
            If phone.StartsWith("0") AndAlso phone.Length = 10 Then
                Dim prefix As String = "84" & phone.Substring(1, 2)
                Return IsValidVietnamPrefix(prefix)
            End If

            Return False
        End Function

        ''' <summary>
        ''' Kiem tra va chuan hoa, tra ve Nothing neu sdt khong hop le
        ''' </summary>
        Public Shared Function ValidateAndNormalize(ByVal raw As String, ByRef errorMsg As String) As String
            errorMsg = String.Empty
            If String.IsNullOrWhiteSpace(raw) Then
                errorMsg = "So dien thoai khong duoc de trong"
                Return Nothing
            End If

            Dim normalized As String = NormalizePhone(raw)

            If Not IsValidPhone(normalized) Then
                errorMsg = String.Format("So dien thoai '{0}' khong hop le hoac khong phai mang Viet Nam", raw.Trim())
                Return Nothing
            End If

            Return normalized
        End Function

        ''' <summary>
        ''' Xu ly danh sach sdt (nhieu dong hoac cach nhau dau phay)
        ''' Tra ve (danhSachHopLe, danhSachLoi)
        ''' </summary>
        Public Shared Sub ProcessPhoneList(ByVal input As String,
                                           ByRef validList As List(Of String),
                                           ByRef invalidList As List(Of String))
            validList = New List(Of String)()
            invalidList = New List(Of String)()

            If String.IsNullOrWhiteSpace(input) Then Return

            ' Split theo dong moi va dau phay
            Dim separators As Char() = {","c, vbLf(0), vbCr(0)}
            Dim tokens As String() = input.Split(separators, StringSplitOptions.RemoveEmptyEntries)

            Dim seen As New HashSet(Of String)()
            For Each token As String In tokens
                Dim raw As String = token.Trim()
                If String.IsNullOrEmpty(raw) Then Continue For

                Dim errMsg As String = String.Empty
                Dim norm As String = ValidateAndNormalize(raw, errMsg)

                If norm Is Nothing Then
                    invalidList.Add(String.Format("{0}: {1}", raw, errMsg))
                ElseIf seen.Contains(norm) Then
                    ' trung nhau trong cung batch - bo qua
                Else
                    seen.Add(norm)
                    validList.Add(norm)
                End If
            Next
        End Sub

        Private Shared Function IsDigitsOnly(ByVal s As String) As Boolean
            For Each c As Char In s
                If Not Char.IsDigit(c) Then Return False
            Next
            Return True
        End Function

        ''' <summary>
        ''' Kiem tra prefix 4 so (84xx) co thuoc mang VN khong
        ''' </summary>
        Private Shared Function IsValidVietnamPrefix(ByVal prefix4 As String) As Boolean
            ' Danh sach dau so hop le cua VN (cap nhat 2024)
            Dim validPrefixes As String() = {
                "8432", "8433", "8434", "8435",
                "8436", "8437", "8438", "8439",
                "8486",
                "8496", "8497", "8498",
                "8481", "8482", "8483", "8484", "8485",
                "8488",
                "8491", "8494",
                "8470", "8476", "8477", "8478", "8479",
                "8489",
                "8490", "8493",
                "8456", "8458",
                "8492",
                "8459",
                "8499",
                "8487"
            }
            Return Array.IndexOf(validPrefixes, prefix4) >= 0
        End Function

    End Class

End Namespace

