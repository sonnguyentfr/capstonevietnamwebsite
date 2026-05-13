
Partial Class Portals__default_Skins_BUH_Control_Headersxxx
    Inherits System.Web.UI.UserControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If Not Page.IsPostBack Then
                Dim strlinkcssjs As String
                strlinkcssjs = "<meta http-equiv='REFRESH' content='1800' />" _
                    & vbCrLf & "<meta name='copyright' content='Báo điện tử Thương Trường' />" _
                    & vbCrLf & "<meta charset='utf-8'><meta http-equiv='x-ua-compatible' content='ie=edge'>" _
                    & vbCrLf & "<meta property='fb:app_id' content='517574561947569'/>" _
                    & vbCrLf & "<meta property='fb:admins' content='1188617780' />" _
                    & vbCrLf & "<meta property='fb:pages' content='1135091826577788' />" _
                    & vbCrLf & "<meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'>" _
                    & vbCrLf & "<meta http-equiv='X-UA-Compatible' content='IE=EmulateIE7' />" _
                    & vbCrLf & "<!-- Bootstrap -->" _
                    & vbCrLf & "<link rel='origin' href='//cdn.thuongtruong.com.vn' />" _
                    & vbCrLf & "<link rel='origin' href='//f.thuongtruong.com.vn' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//oss.maxcdn.com' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//fortawesome.github.io' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='/staticxx.facebook.com' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//www.adobe.com/' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//www.google.com' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//fontawesome.io' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//connect.facebook.net' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//www.google-analytics.com' />" _
                    & vbCrLf & "<link rel='dns-prefetch' href='//s7.addthis.com' />" _
                    & vbCrLf & "<!--[if lt IE 9]>" _
                    & vbCrLf & "<script src='https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js' type='text/javascript'></script>" _
                    & vbCrLf & "<script src='https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js' type='text/javascript'></script>" _
                    & vbCrLf & "<script src='http://html5shiv.googlecode.com/svn/trunk/html5.js'></script>" _
                    & vbCrLf & "<![endif]-->" _
                    & vbCrLf & "<link href='https://fonts.googleapis.com/css?family=Roboto+Condensed%7CRoboto+Slab:300,400,700%7CRoboto:300,400,500,700' rel='stylesheet'>" _
                    & vbCrLf & "<script async src='https://pagead2.googlesyndication.com/pagead/js/adsbygoogle.js'></script>" _
                    & vbCrLf & "<link rel='stylesheet' href='//cdn.thuongtruong.com.vn/nvcms_vanhoa/css/style.css?v=1.1.4'>" _
                    & vbCrLf & "<link rel='stylesheet' href='//cdn.thuongtruong.com.vn/nvcms_vanhoa/css/widgets.css?v=1.1.4'>" _
                    & vbCrLf & "<link rel='stylesheet' href='//cdn.thuongtruong.com.vn/nvcms_vanhoa/css/color.css?v=1.1.4'>" _
                    & vbCrLf & "<link rel='stylesheet' href='//cdn.thuongtruong.com.vn/nvcms_vanhoa/css/responsive.css?v=1.1.4'>" _
                    & vbCrLf & "<meta name='dmca-site-verification' content='cmQzMlo5eTUrTTFYeXZVazNodjJMNWVVblluMDVOeHRRSzhMMDFjUDVmZz01' />" _
                    & vbCrLf & "<link href='/fonts/font-awesome.min.css?v=1.1.4' rel='stylesheet' />" _
                    & vbCrLf & "<link href='/Portals/_default/Skins/nvcms_vanhoa/Control/fontawesome-all.min.css?v=1.1.4' rel='stylesheet' /><link href='/Portals/_default/Skins/nvcms_vanhoa/Control/fontcss.css?v=1.1.4' rel='stylesheet' />" _
                    & vbCrLf & ""
                Dim htmlHeaderTags2 = ""
                Dim htmlHeaderCtrl2 As New LiteralControl()
                htmlHeaderTags2 = strlinkcssjs
                htmlHeaderCtrl2.Text = htmlHeaderTags2.ToString()
                Page.Header.Controls.Add(htmlHeaderCtrl2)
            End If
        Catch exc As Exception        'Module failed to load
        End Try
    End Sub
End Class
