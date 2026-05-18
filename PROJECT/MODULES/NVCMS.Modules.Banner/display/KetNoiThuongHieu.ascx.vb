Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Services.Exceptions
Namespace NVCMS.Modules.Banner
    Partial Class KetNoiThuongHieu
        Inherits Entities.Modules.PortalModuleBase
#Region "Controls"
        Public Property vitri() As Integer
            Get
                If Not ViewState("vitri") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("vitri"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("vitri", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("vitri") = Value.ToString
            End Set
        End Property
        Public Property Portal() As Integer
            Get
                If Not ViewState("Portal") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("Portal"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("Portal", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("Portal") = Value.ToString
            End Set
        End Property
        Public Property title() As String
            Get
                If Not ViewState.Item("title") Is Nothing Then
                    Return ViewState.Item("title")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("title", value)
            End Set
        End Property
#End Region

#Region "Event Handlers"
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
            arrNews = ctlNews.GetAllShow(Portal, vitri)
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
                            sresult = "<a href='/bannerclick/" & .Title & "-" & .id & "' target=_blank alt='" & .Title & "'><img class='img-responsive lazyload' src='/DATA/noimage.png' data-src='" & nvcmsBL.FormatThumbImage(.IMGLink, .Width, .Height, "crop", "middlecenter", "") & "' alt='" & .Title & "'/></a>"
                        Case "3"
                            sresult = .IMGLink
                    End Select
                End With
            End If

            Return sresult
        End Function

#End Region



    End Class

End Namespace