Namespace NVCMS.Modules.Marketing

    Public MustInherit Class TemplateView
        Inherits Entities.Modules.PortalModuleBase

#Region "Controls"
        Dim _Marketing_Mail_TemplateController As New Marketing_Mail_TemplateController
        Public Property ItemID() As Integer
            Get
                If Not ViewState("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("ItemID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState("ItemID") = Value.ToString
            End Set
        End Property
#End Region

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not IsPostBack Then
                    BindGridData()
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region
        Private Sub BindGridData()

            Dim arrNewsCategories As New ArrayList
            arrNewsCategories = _Marketing_Mail_TemplateController._GetAll(PortalId)
            Me.drgViewData.DataSource = arrNewsCategories
            Me.drgViewData.DataBind()
        End Sub

#Region "edit insert"

        Private Sub lbtAddBottom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbtAdd.Click, lbtAddTop.Click
            Try
                Response.Redirect(NavigateURL() & "?view=add", True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region
    End Class

End Namespace
