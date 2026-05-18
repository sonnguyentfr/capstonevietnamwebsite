Imports System
Imports System.Text.RegularExpressions

    Public Class TextHelper
        ' Methods
        Public Shared Function CleanHtml(ByVal html As String) As String
            html = Regex.Replace(html, "<[/]?(b|div|font|span|xml|del|ins|[ovwxp]:\w+)[^>]*?>", "", RegexOptions.IgnoreCase)
            html = Regex.Replace(html, "<([^>]*)(?:class|lang|style|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>", RegexOptions.IgnoreCase)
            html = Regex.Replace(html, "<([^>]*)(?:class|lang|style|size|face|[ovwxp]:\w+)=(?:'[^']*'|""[^""]*""|[^\s>]+)([^>]*)>", "<$1$2>", RegexOptions.IgnoreCase)
            Return html
        End Function

        Public Shared Function CleanMsWordHtml(ByVal wordContent As String) As String
            wordContent = Regex.Replace(wordContent, "class=[^\s|>]*", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "style='[^>]*'", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "align=[^\s|>]*", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "<p [^>]*>", "<p>", (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "<b [^>]*>", "<b>", RegexOptions.IgnoreCase)
            wordContent = Regex.Replace(wordContent, "<i [^>]*>", "<i>", RegexOptions.IgnoreCase)
            wordContent = Regex.Replace(wordContent, "<ul [^>]*>", "<ul>", RegexOptions.IgnoreCase)
            wordContent = Regex.Replace(wordContent, "<li [^>]*>", "<li>", RegexOptions.IgnoreCase)
            wordContent = Regex.Replace(wordContent, "<em>", "<i>", (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</em>", "</i>", (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "<\?xml:[^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</?st1:[^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</?[a-z]\:[^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</?span[^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</?div[^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</?font[^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</?pre[^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = Regex.Replace(wordContent, "</?h[1-6][^>]*>", String.Empty, (RegexOptions.IgnorePatternWhitespace Or RegexOptions.IgnoreCase))
            wordContent = wordContent.Replace(ChrW(13), String.Empty)
            wordContent = wordContent.Replace(ChrW(13) & ChrW(10), String.Empty)
            wordContent = wordContent.Replace(ChrW(10), String.Empty)
            wordContent = wordContent.Replace(ChrW(8217), "'")
            wordContent = wordContent.Replace(ChrW(8217), "'")
            wordContent = wordContent.Replace("<P>", String.Empty)
            wordContent = wordContent.Replace("</P>", "<br/>")
            wordContent = wordContent.Replace("<p>", String.Empty)
            wordContent = wordContent.Replace("</p>", "<br/>")
            Return wordContent
        End Function

        Public Shared Function CleanSign(ByVal wordContent As String) As String
            Dim chArray As Char() = New Char() { ChrW(224), ChrW(225), ChrW(7843), ChrW(227), ChrW(7841), ChrW(259), ChrW(7857), ChrW(7855), ChrW(7859), ChrW(7861), ChrW(7863), ChrW(226), ChrW(7847), ChrW(7845), ChrW(7849), ChrW(7851), ChrW(7853), ChrW(273), ChrW(232), ChrW(233), ChrW(7867), ChrW(7869), ChrW(7865), ChrW(234), ChrW(7873), ChrW(7871), ChrW(7875), ChrW(7877), ChrW(7879), ChrW(236), ChrW(237), ChrW(7881), ChrW(297), ChrW(7883), ChrW(7923), ChrW(253), ChrW(7927), ChrW(7929), ChrW(7925), ChrW(242), ChrW(243), ChrW(7887), ChrW(245), ChrW(7885), ChrW(244), ChrW(7891), ChrW(7889), ChrW(7893), ChrW(7895), ChrW(7897), ChrW(417), ChrW(7901), ChrW(7899), ChrW(7903), ChrW(7905), ChrW(7907), ChrW(249), ChrW(250), ChrW(7911), ChrW(361), ChrW(7909), ChrW(432), ChrW(7915), ChrW(7913), ChrW(7917), ChrW(7919), ChrW(7921) }
            Dim chArray2 As Char() = New Char() { "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "a"c, "d"c, "e"c, "e"c, "e"c, "e"c, "e"c, "e"c, "e"c, "e"c, "e"c, "e"c, "e"c, "i"c, "i"c, "i"c, "i"c, "i"c, "y"c, "y"c, "y"c, "y"c, "y"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "o"c, "u"c, "u"c, "u"c, "u"c, "u"c, "u"c, "u"c, "u"c, "u"c, "u"c, "u"c }
            Dim num2 As Integer = (chArray.Length - 1)
            Dim i As Integer = 0
            Do While (i <= num2)
                wordContent = wordContent.Replace(chArray(i), chArray2(i))
                i += 1
            Loop
            Return wordContent
        End Function

    End Class