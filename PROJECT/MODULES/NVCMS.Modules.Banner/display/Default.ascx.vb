Imports System
Imports System.IO
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Exceptions
Namespace NVCMS.Modules.Banner
    Partial Class defaul
        Inherits Entities.Modules.PortalModuleBase
#Region "Controls"
        Public Property vitri() As Integer
            Get
                If Not ViewState.Item("vitri") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("vitri")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("vitri", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("vitri") = Value.ToString
            End Set
        End Property
#End Region
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then
                    BindData()
                End If
            Catch ex As Exception
                'Me.imgNews.Visible = False
                ' Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub BindData()
            Dim arrNews As New ArrayList
            Dim ctlNews As New BannerAdvController
            arrNews = ctlNews.GetAllShow(0, vitri)
            Me.drgOtherNews.DataSource = arrNews
            Me.drgOtherNews.DataBind()

        End Sub
        Function GetKieuBanner(ByVal id As Integer) As String
            Dim objVitri As BannerAdvInfo
            Dim ctlvitri As New BannerAdvController
            objVitri = ctlvitri.GetByID(id)
            Return objVitri.KieuBanner
        End Function
        Function GetBanner(ByVal id As Integer) As String
            Dim sresult As String = ""
            Dim objVitri As BannerAdvInfo
            Dim ctlvitri As New BannerAdvController
            objVitri = ctlvitri.GetByID(id)
            If Not objVitri Is Nothing Then
                With objVitri
                    Select Case objVitri.KieuBanner
                        Case "1"
                            If CutstringPhotoExtension(.IMGLink) = "gif" Then
                                sresult = "<a href='/bannerclick/" & .Title & "-" & .id & "' target=_blank alt='" & .Title & "'><img class='img-responsive lazyload' width='" & .Width & "' src='/DATA/noimage.png?width=" & .Width & "&height=" & .Height & "&mode=crop&anchor=middlecenter' data-src='" & .IMGLink.Replace("/DATA", nvcmsBL.filesDomain) & "' alt='" & .Title & "'/></a>"
                            Else
                                sresult = "<a href='/bannerclick/" & .Title & "-" & .id & "' target=_blank alt='" & .Title & "'><img class='img-responsive lazyload' src='/DATA/noimage.png?width=" & .Width & "&height=" & .Height & "&mode=crop&anchor=middlecenter' data-src='" & nvcmsBL.FormatThumbImage(.IMGLink, .Width, .Height, "crop", "middlecenter", "") & "' alt='" & .Title & "'/></a>"
                            End If

                        Case "3"
                            sresult = .IMGLink
                    End Select
                End With
            End If

            Return sresult
        End Function
        Public Function CutstringPhotoExtension(str As String) As String
            Dim extesnsion As String = ""
            extesnsion = Path.GetExtension(str)
            If extesnsion.Length > 1 Then
                Return extesnsion.Remove(0, 1)
            Else
                Return extesnsion
            End If
            Return extesnsion
        End Function
    End Class
End Namespace