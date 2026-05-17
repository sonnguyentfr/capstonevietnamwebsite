Namespace NVCMS.Modules.School

    Public MustInherit Class newsviewerMain
        Inherits Entities.Modules.PortalModuleBase
        Dim _MarketingSchoolController As New MarketingSchoolController
        Public Property fbclid() As String
            Get
                If Not ViewState.Item("fbclid") Is Nothing Then
                    Return ViewState.Item("fbclid")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("fbclid", value)
            End Set
        End Property
        Private setting_type As String
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                fbclid = Request.Item("fbclid")
                Dim sUrl1 As String = Request.RawUrl
                Dim sUrl As String = sUrl1.Replace("?fbclid=" & fbclid, "")
                'Dim sUrl As String = Request.RawUrl
                Dim sId As Integer = Ultis.GetRequestId(sUrl)
                Dim DynamicPage As String
                DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Index.ascx")
                If Not Null.IsNull(Settings(BL.settingView_Type.ToString())) Then
                    setting_type = Settings(BL.settingView_Type.ToString()).ToString()
                End If
                If sId > 0 Then 'Index
                    Dim objSchoolInfo As MarketingSchoolInfo
                    objSchoolInfo = _MarketingSchoolController.Marketing_Truong_GetByID(sId)

                    If Not objSchoolInfo Is Nothing Then
                        With objSchoolInfo
                            If (.Loai = 2) Or (.Loai = 3) Then ' dai hoc
                                DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/DetailUniversity.ascx")
                            End If
                            'If .Loai = 3 Then ' Cao Dang
                            '    DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/DetailCollege.ascx")
                            'End If
                            If .Loai = 5 Then 'Trung ghoc
                                DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/DetailHightSchool.ascx")
                            End If
                        End With
                    End If
                Else
                    If setting_type = "major" Then
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/IndexMajor.ascx")
                    Else
                        DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Index.ascx")
                    End If
                    'DynamicPage = DotNetNuke.Common.ResolveUrl(Me.TemplateSourceDirectory & "/Index.ascx")
                End If
                Dim objModule As Entities.Modules.PortalModuleBase = CType(Me.LoadControl(DynamicPage), DotNetNuke.Entities.Modules.PortalModuleBase)
                If Not objModule Is Nothing Then
                    objModule.ModuleConfiguration = Me.ModuleConfiguration
                    plhNews.Controls.Add(objModule)
                End If

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

    End Class
End Namespace