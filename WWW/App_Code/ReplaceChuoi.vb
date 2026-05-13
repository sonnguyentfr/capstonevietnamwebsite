Imports Microsoft.VisualBasic

Public Class ReplaceChuoi
    Private Shared Function UTFConvert(ByVal sContent As [String]) As String
        sContent = sContent.Trim()
        Dim sUTF8Lower As [String] = "a|á|à|ả|ã|ạ|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ|đ|e|é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ|i|í|ì|ỉ|ĩ|ị|o|ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ|u|ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự|y|ý|ỳ|ỷ|ỹ|ỵ"

        Dim sUTF8Upper As [String] = "A|Á|À|Ả|Ã|Ạ|Ă|Ắ|Ằ|Ẳ|Ẵ|Ặ|Â|Ấ|Ầ|Ẩ|Ẫ|Ậ|Đ|E|É|È|Ẻ|Ẽ|Ẹ|Ê|Ế|Ề|Ể|Ễ|Ệ|I|Í|Ì|Ỉ|Ĩ|Ị|O|Ó|Ò|Ỏ|Õ|Ọ|Ô|Ố|Ồ|Ổ|Ỗ|Ộ|Ơ|Ớ|Ờ|Ở|Ỡ|Ợ|U|Ú|Ù|Ủ|Ũ|Ụ|Ư|Ứ|Ừ|Ử|Ữ|Ự|Y|Ý|Ỳ|Ỷ|Ỹ|Ỵ"

        Dim sUCS2Lower As [String] = "a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|a|d|e|e|e|e|e|e|e|e|e|e|e|e|i|i|i|i|i|i|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|o|u|u|u|u|u|u|u|u|u|u|u|u|y|y|y|y|y|y"

        Dim sUCS2Upper As [String] = "A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|A|D|E|E|E|E|E|E|E|E|E|E|E|E|I|I|I|I|I|I|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|O|U|U|U|U|U|U|U|U|U|U|U|U|Y|Y|Y|Y|Y|Y"

        Dim aUTF8Lower As [String]() = sUTF8Lower.Split(New [Char]() {"|"c})

        Dim aUTF8Upper As [String]() = sUTF8Upper.Split(New [Char]() {"|"c})

        Dim aUCS2Lower As [String]() = sUCS2Lower.Split(New [Char]() {"|"c})

        Dim aUCS2Upper As [String]() = sUCS2Upper.Split(New [Char]() {"|"c})

        Dim nLimitChar As Int32

        nLimitChar = aUTF8Lower.GetUpperBound(0)

        For i As Integer = 1 To nLimitChar

            sContent = sContent.Replace(aUTF8Lower(i), aUCS2Lower(i))


            sContent = sContent.Replace(aUTF8Upper(i), aUCS2Upper(i))
        Next
        Dim sUCS2regex As String = "[A-Za-z0-9- ]"
        Dim sEscaped As String = New Regex(sUCS2regex, RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.ExplicitCapture).Replace(sContent, String.Empty)
        If String.IsNullOrEmpty(sEscaped) Then
            Return sContent
        End If
        sEscaped = sEscaped.Replace("[", "\[")
        sEscaped = sEscaped.Replace("]", "\]")
        sEscaped = sEscaped.Replace("^", "\^")
        Dim sEscapedregex As String = "[" & sEscaped & "]"

        Return New Regex(sEscapedregex, RegexOptions.IgnoreCase Or RegexOptions.Multiline Or RegexOptions.ExplicitCapture).Replace(sContent, String.Empty)
    End Function
    Public Shared Function Auto_TagHTML(ByVal source As String) As String
        Try
            Dim result As String
            result = source.ToLower().Replace("""", ",")
            result = result.Replace("""", ",")
            result = result.Replace("""", ",")
            result = result.Replace("<br>", "")
            result = result.Replace("'", ",")
            result = result.Replace(".", ",")
            result = result.Replace("?", ",")
            result = result.Replace("!", ",")
            Do
                result = result.Replace("  ", " ")
            Loop While result.IndexOf("  ") > 0

            Dim aresult As String() = result.Split(" ".ToCharArray())
            Dim sDon_Am As String = "cần|về|quá|vì|bị|do|làm|nhưng|cùng|một|hai|ba|như|sau|không|mà|các|lên|hoặc|giành|này|nhận|ngày|từ|thay|đều|vừa|gì|theo|cho|mới|của|sẽ|trên|và|đang|theo|của|rất|muốn|có|được|với|cả|đến|những|tại|ở|là|của|khi|còn|cũng|vì|có|trong|theo|tại|vào|"
            For i As Integer = 0 To aresult.Length - 1
                If (sDon_Am.IndexOf(aresult(i) & "|") >= 0) Then
                    aresult(i) = ","
                End If
            Next

            result = ""
            For i As Integer = 0 To aresult.Length - 1
                result = result & " " & aresult(i)
            Next
            aresult = result.Split(",".ToCharArray())

            result = ""
            Dim sTmp As String = ""
            For i As Integer = aresult.Length - 1 To 1 Step -1
                sTmp = aresult(i).Trim()
                While sTmp.StartsWith(",")
                    sTmp = sTmp.Remove(0, 1)
                End While
                While sTmp.EndsWith(",")
                    sTmp = sTmp.Remove(sTmp.Length - 1, 1)
                End While
                If sTmp.Trim().Length > 2 Then
                    result = result & ", " & sTmp.Trim()
                End If
            Next
            While result.StartsWith(",")
                result = result.Remove(0, 1)
            End While
            While result.EndsWith(",")
                result = result.Remove(result.Length - 1, 1)
            End While
            Return result.Trim()
        Catch
            Return source
        End Try
    End Function
    Public Shared Function bodau(ByVal sourse As String) As String
        Dim result As String = String.Empty
        result = sourse.ToLower()
        Do
            result = result.Replace("  ", " ")
        Loop While result.IndexOf("  ") > 0
        result = result.Replace("ấ", "a")
        result = result.Replace("ầ", "a")
        result = result.Replace("ẩ", "a")
        result = result.Replace("ẫ", "a")
        result = result.Replace("ậ", "a")
        result = result.Replace("ắ", "a")
        result = result.Replace("ằ", "a")
        result = result.Replace("ẳ", "a")
        result = result.Replace("ẵ", "a")
        result = result.Replace("ặ", "a")
        result = result.Replace("à", "a")
        result = result.Replace("á", "a")
        result = result.Replace("ả", "a")
        result = result.Replace("ã", "a")
        result = result.Replace("ạ", "a")
        result = result.Replace("â", "a")
        result = result.Replace("ă", "a")
        result = result.Replace("ế", "e")
        result = result.Replace("ề", "e")
        result = result.Replace("ể", "e")
        result = result.Replace("ễ", "e")
        result = result.Replace("ệ", "e")
        result = result.Replace("é", "e")
        result = result.Replace("è", "e")
        result = result.Replace("ẻ", "e")
        result = result.Replace("ẽ", "e")
        result = result.Replace("ẹ", "e")
        result = result.Replace("ê", "e")
        result = result.Replace("í", "i")
        result = result.Replace("ì", "i")
        result = result.Replace("ỉ", "i")
        result = result.Replace("ĩ", "i")
        result = result.Replace("ị", "i")
        result = result.Replace("ố", "o")
        result = result.Replace("ồ", "o")
        result = result.Replace("ổ", "o")
        result = result.Replace("ỗ", "o")
        result = result.Replace("ộ", "o")
        result = result.Replace("ớ", "o")
        result = result.Replace("ờ", "o")
        result = result.Replace("ở", "o")
        result = result.Replace("ỡ", "o")
        result = result.Replace("ợ", "o")
        result = result.Replace("ứ", "u")
        result = result.Replace("ừ", "u")
        result = result.Replace("ử", "u")
        result = result.Replace("ữ", "u")
        result = result.Replace("ự", "u")
        result = result.Replace("ý", "y")
        result = result.Replace("ỳ", "y")
        result = result.Replace("ỷ", "y")
        result = result.Replace("ỹ", "y")
        result = result.Replace("ỵ", "y")
        result = result.Replace("đ", "d")
        result = result.Replace("ó", "o")
        result = result.Replace("ò", "o")
        result = result.Replace("ỏ", "o")
        result = result.Replace("õ", "o")
        result = result.Replace("ọ", "o")
        result = result.Replace("ô", "o")
        result = result.Replace("ơ", "o")
        result = result.Replace("ú", "u")
        result = result.Replace("ù", "u")
        result = result.Replace("ủ", "u")
        result = result.Replace("ũ", "u")
        result = result.Replace("ụ", "u")
        result = result.Replace("ư", "u")
        result = result.Replace("""", "")
        result = result.Replace(":", "")
        result = result.Replace("'", "")
        result = result.Replace("''", "")
        result = result.Replace("&", "")
        result = result.Replace(".", "")
        result = result.Replace(",", "")
        result = result.Replace("\", "")
        result = result.Replace("/", "")
        result = result.Replace("=", "")
        result = result.Replace("[", "")
        result = result.Replace("]", "")
        result = result.Replace("(", "")
        result = result.Replace(")", "")
        result = result.Replace("?", "")
        result = result.Replace("%", "")
        result = result.Replace("<", "")
        result = result.Replace(">", "")
        result = result.Replace("/", "")
        result = result.Replace("\", "")
        Return result
    End Function
    Public Shared Function bodau2(ByVal sourse As String) As String
        Dim result As String = String.Empty
        result = sourse.ToLower()
        Do
            result = result.Replace("  ", " ")
        Loop While result.IndexOf("  ") > 0
        result = result.Replace("ấ", "a")
        result = result.Replace("ầ", "a")
        result = result.Replace("ẩ", "a")
        result = result.Replace("ẫ", "a")
        result = result.Replace("ậ", "a")
        result = result.Replace("ắ", "a")
        result = result.Replace("ằ", "a")
        result = result.Replace("ẳ", "a")
        result = result.Replace("ẵ", "a")
        result = result.Replace("ặ", "a")
        result = result.Replace("à", "a")
        result = result.Replace("á", "a")
        result = result.Replace("ả", "a")
        result = result.Replace("ã", "a")
        result = result.Replace("ạ", "a")
        result = result.Replace("â", "a")
        result = result.Replace("ă", "a")
        result = result.Replace("ế", "e")
        result = result.Replace("ề", "e")
        result = result.Replace("ể", "e")
        result = result.Replace("ễ", "e")
        result = result.Replace("ệ", "e")
        result = result.Replace("é", "e")
        result = result.Replace("è", "e")
        result = result.Replace("ẻ", "e")
        result = result.Replace("ẽ", "e")
        result = result.Replace("ẹ", "e")
        result = result.Replace("ê", "e")
        result = result.Replace("í", "i")
        result = result.Replace("ì", "i")
        result = result.Replace("ỉ", "i")
        result = result.Replace("ĩ", "i")
        result = result.Replace("ị", "i")
        result = result.Replace("ố", "o")
        result = result.Replace("ồ", "o")
        result = result.Replace("ổ", "o")
        result = result.Replace("ỗ", "o")
        result = result.Replace("ộ", "o")
        result = result.Replace("ớ", "o")
        result = result.Replace("ờ", "o")
        result = result.Replace("ở", "o")
        result = result.Replace("ỡ", "o")
        result = result.Replace("ợ", "o")
        result = result.Replace("ứ", "u")
        result = result.Replace("ừ", "u")
        result = result.Replace("ử", "u")
        result = result.Replace("ữ", "u")
        result = result.Replace("ự", "u")
        result = result.Replace("ý", "y")
        result = result.Replace("ỳ", "y")
        result = result.Replace("ỷ", "y")
        result = result.Replace("ỹ", "y")
        result = result.Replace("ỵ", "y")
        result = result.Replace("đ", "d")
        result = result.Replace("ó", "o")
        result = result.Replace("ò", "o")
        result = result.Replace("ỏ", "o")
        result = result.Replace("õ", "o")
        result = result.Replace("ọ", "o")
        result = result.Replace("ô", "o")
        result = result.Replace("ơ", "o")
        result = result.Replace("ú", "u")
        result = result.Replace("ù", "u")
        result = result.Replace("ủ", "u")
        result = result.Replace("ũ", "u")
        result = result.Replace("ụ", "u")
        result = result.Replace("ư", "u")
        result = result.Replace("""", "")
        result = result.Replace(":", "")
        result = result.Replace("'", "")
        result = result.Replace("''", "")
        result = result.Replace("&", "")
        result = result.Replace(",", "")
        result = result.Replace("\", "")
        result = result.Replace("/", "")
        result = result.Replace("=", "")
        result = result.Replace("[", "")
        result = result.Replace("]", "")
        result = result.Replace("(", "")
        result = result.Replace(")", "")
        result = result.Replace("?", "")
        result = result.Replace("%", "")
        result = result.Replace("<", "")
        result = result.Replace(">", "")
        result = result.Replace("/", "")
        result = result.Replace("\", "")
        Return result
    End Function
    Public Shared Function bodau3(ByVal sourse As String) As String
        Dim result As String = String.Empty
        result = sourse.ToLower()
        Do
            result = result.Replace("  ", "")
        Loop While result.IndexOf("  ") > 0
        result = result.Replace("ấ", "a")
        result = result.Replace("ầ", "a")
        result = result.Replace("ẩ", "a")
        result = result.Replace("ẫ", "a")
        result = result.Replace("ậ", "a")
        result = result.Replace("ắ", "a")
        result = result.Replace("ằ", "a")
        result = result.Replace("ẳ", "a")
        result = result.Replace("ẵ", "a")
        result = result.Replace("ặ", "a")
        result = result.Replace("à", "a")
        result = result.Replace("á", "a")
        result = result.Replace("ả", "a")
        result = result.Replace("ã", "a")
        result = result.Replace("ạ", "a")
        result = result.Replace("â", "a")
        result = result.Replace("ă", "a")
        result = result.Replace("ế", "e")
        result = result.Replace("ề", "e")
        result = result.Replace("ể", "e")
        result = result.Replace("ễ", "e")
        result = result.Replace("ệ", "e")
        result = result.Replace("é", "e")
        result = result.Replace("è", "e")
        result = result.Replace("ẻ", "e")
        result = result.Replace("ẽ", "e")
        result = result.Replace("ẹ", "e")
        result = result.Replace("ê", "e")
        result = result.Replace("í", "i")
        result = result.Replace("ì", "i")
        result = result.Replace("ỉ", "i")
        result = result.Replace("ĩ", "i")
        result = result.Replace("ị", "i")
        result = result.Replace("ố", "o")
        result = result.Replace("ồ", "o")
        result = result.Replace("ổ", "o")
        result = result.Replace("ỗ", "o")
        result = result.Replace("ộ", "o")
        result = result.Replace("ớ", "o")
        result = result.Replace("ờ", "o")
        result = result.Replace("ở", "o")
        result = result.Replace("ỡ", "o")
        result = result.Replace("ợ", "o")
        result = result.Replace("ứ", "u")
        result = result.Replace("ừ", "u")
        result = result.Replace("ử", "u")
        result = result.Replace("ữ", "u")
        result = result.Replace("ự", "u")
        result = result.Replace("ý", "y")
        result = result.Replace("ỳ", "y")
        result = result.Replace("ỷ", "y")
        result = result.Replace("ỹ", "y")
        result = result.Replace("ỵ", "y")
        result = result.Replace("đ", "d")
        result = result.Replace("ó", "o")
        result = result.Replace("ò", "o")
        result = result.Replace("ỏ", "o")
        result = result.Replace("õ", "o")
        result = result.Replace("ọ", "o")
        result = result.Replace("ô", "o")
        result = result.Replace("ơ", "o")
        result = result.Replace("ú", "u")
        result = result.Replace("ù", "u")
        result = result.Replace("ủ", "u")
        result = result.Replace("ũ", "u")
        result = result.Replace("ụ", "u")
        result = result.Replace("ư", "u")
        result = result.Replace("""", "")
        result = result.Replace(":", "")
        result = result.Replace("'", "")
        result = result.Replace("''", "")
        result = result.Replace("&", "")
        result = result.Replace(",", "")
        result = result.Replace("\", "")
        result = result.Replace("/", "")
        result = result.Replace("=", "")
        result = result.Replace("[", "")
        result = result.Replace("]", "")
        result = result.Replace("(", "")
        result = result.Replace(")", "")
        result = result.Replace("?", "")
        result = result.Replace("%", "")
        result = result.Replace("<", "")
        result = result.Replace(">", "")
        result = result.Replace("/", "")
        result = result.Replace("\", "")
        result = result.Replace(" ", "")
        Return result
    End Function
    Public Shared Function bodautenfile(ByVal sourse As String) As String
        Dim result As String = String.Empty
        result = sourse.ToLower()
        result = bodau2(result)
        result = result.Replace(" ", "-")
        Return result
    End Function
    Public Shared Function StringToArray(ByVal input As String, ByVal separator As String, ByVal type As Type) As Object()
        Dim stringList As String() = input.Split(separator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
        Dim list As Object() = New Object(stringList.Length - 1) {}

        For i As Integer = 0 To stringList.Length - 1
            list(i) = Convert.ChangeType(stringList(i), type)
        Next

        Return list
    End Function
    Public Shared Function titlenews(ByVal sourse As String) As String
        Dim result As String = String.Empty
        result = Regex.Replace(sourse, "[;\/:*?""<>|&']", "")
        Return result
    End Function
    Public Shared Function tags(ByVal sourse As String) As String
        Dim result As String = String.Empty
        result = sourse.ToLower()
        Do
            result = Regex.Replace(sourse, "[;\/:*?""<>|&']", "")
        Loop While result.IndexOf("  ") > 0
        result = result.Replace("ấ", "a")
        result = result.Replace("ầ", "a")
        result = result.Replace("ẩ", "a")
        result = result.Replace("ẫ", "a")
        result = result.Replace("ậ", "a")
        result = result.Replace("ắ", "a")
        result = result.Replace("ằ", "a")
        result = result.Replace("ẳ", "a")
        result = result.Replace("ẵ", "a")
        result = result.Replace("ặ", "a")
        result = result.Replace("à", "a")
        result = result.Replace("á", "a")
        result = result.Replace("ả", "a")
        result = result.Replace("ã", "a")
        result = result.Replace("ạ", "a")
        result = result.Replace("â", "a")
        result = result.Replace("ă", "a")
        result = result.Replace("ế", "e")
        result = result.Replace("ề", "e")
        result = result.Replace("ể", "e")
        result = result.Replace("ễ", "e")
        result = result.Replace("ệ", "e")
        result = result.Replace("é", "e")
        result = result.Replace("è", "e")
        result = result.Replace("ẻ", "e")
        result = result.Replace("ẽ", "e")
        result = result.Replace("ẹ", "e")
        result = result.Replace("ê", "e")
        result = result.Replace("í", "i")
        result = result.Replace("ì", "i")
        result = result.Replace("ỉ", "i")
        result = result.Replace("ĩ", "i")
        result = result.Replace("ị", "i")
        result = result.Replace("ố", "o")
        result = result.Replace("ồ", "o")
        result = result.Replace("ổ", "o")
        result = result.Replace("ỗ", "o")
        result = result.Replace("ộ", "o")
        result = result.Replace("ớ", "o")
        result = result.Replace("ờ", "o")
        result = result.Replace("ở", "o")
        result = result.Replace("ỡ", "o")
        result = result.Replace("ợ", "o")
        result = result.Replace("ứ", "u")
        result = result.Replace("ừ", "u")
        result = result.Replace("ử", "u")
        result = result.Replace("ữ", "u")
        result = result.Replace("ự", "u")
        result = result.Replace("ý", "y")
        result = result.Replace("ỳ", "y")
        result = result.Replace("ỷ", "y")
        result = result.Replace("ỹ", "y")
        result = result.Replace("ỵ", "y")
        result = result.Replace("đ", "d")
        result = result.Replace("ó", "o")
        result = result.Replace("ò", "o")
        result = result.Replace("ỏ", "o")
        result = result.Replace("õ", "o")
        result = result.Replace("ọ", "o")
        result = result.Replace("ô", "o")
        result = result.Replace("ơ", "o")
        result = result.Replace("ú", "u")
        result = result.Replace("ù", "u")
        result = result.Replace("ủ", "u")
        result = result.Replace("ũ", "u")
        result = result.Replace("ụ", "u")
        result = result.Replace("ư", "u")
        result = result.Replace("""", "")
        result = result.Replace(":", "")
        result = result.Replace("'", "")
        result = result.Replace("''", "")
        result = result.Replace("&", "")
        result = result.Replace(",", "")
        result = result.Replace("\", "")
        result = result.Replace("/", "")
        result = result.Replace("=", "")
        result = result.Replace("[", "")
        result = result.Replace("]", "")
        result = result.Replace("(", "")
        result = result.Replace(")", "")
        result = result.Replace("?", "")
        result = result.Replace("%", "")
        result = result.Replace("<", "")
        result = result.Replace(">", "")
        result = result.Replace("/", "")
        result = result.Replace("\", "")
        result = result.Replace(" ", "+")
        Return result
    End Function
End Class
