Imports System.Diagnostics
Imports NVCMS.Modules.TinTuc
Imports NVCMS.Web.Components

Namespace DesktopModules.TinTuc.Control
    Partial Class HotCategory
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

        Public Property CategoryId() As Integer
            Get
                If Not ViewState.Item("CategoryId") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("CategoryId")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("CategoryId", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CategoryId") = Value.ToString
            End Set
        End Property
        Property SubtractIds() As String
            Get
                If Not Session.Item("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString()) Is Nothing Then
                    Return CType(Session.Item("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString()), String)
                Else
                    Session.Add("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString(), "")
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                Session.Item("SubtractIds_" + PortalSettings.ActiveTab.TabID.ToString()) = value.ToString
            End Set
        End Property
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Dim sw As New Stopwatch
                sw.Start()


                CategoryId = BL.GetMappingCategoryIDByTabID(PortalSettings.ActiveTab.TabID)
                'Response.write(CategoryId)
                Dim ctlNews As New NV_NewsController

                Dim cacheName As String = BL.NewsHomeCat & "HOT" & CategoryId & 5
                Dim fromCache As ArrayList = HttpCacheHelper.GetFromCache(cacheName)
                If fromCache Is Nothing Then
                    'fromCache = New List(Of ArticleViewModel)()
                    Dim arrtop As ArrayList = ctlNews.SelectHotCat(CategoryId, 5)
                    If arrtop IsNot Nothing AndAlso arrtop.Count() > 0 Then
                        Dim obj As NV_NewsInfo = CType(arrtop(0), NV_NewsInfo)
                        '1. Hot 1
                        SubtractIds = obj.NewId.ToString()
                        If Not obj Is Nothing Then
                            ltrhotimage.Text = "<a href='" & Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title) & "' title='" & ReplaceChuoi.titlenews(obj.Title) & "'><img class='img-responsive img-full lazy' src='/data/nophoto300-200.png' data-src='" & Ultis.FormatThumbImage(obj.ImagePath, 300, 200, "crop", "middlecenter", "") & "' alt='" & ReplaceChuoi.titlenews(obj.Title) & "' /></a> "
                            ltrhottitle.Text = "<a href='" & Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title) & "' title='" & ReplaceChuoi.titlenews(obj.Title) & "'><strong>" & obj.Title & "</strong></a>"
                            ltrhotdate.Text = BL.FormatDate(obj.PublishedDate)
                            ltrhotsum.Text = obj.Summary
                            ltrhotdoctiep.Text = "<a href='" & Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title) & "' title='" & ReplaceChuoi.titlenews(obj.Title) & "'><span class='read-more'>đọc tiếp</span></a>"
                        End If
                        fromCache = arrtop
                        HttpCacheHelper.SaveToCacheDependency("NVCMSV2", New String() {"News"}, cacheName, fromCache, TimeSpan.FromDays(30))
                    End If
                End If
                rptHot.DataSource = fromCache
                rptHot.DataBind()



                'Dim arrHots As ArrayList = ctlNews.SelectHotCat(CategoryId, 5)
                'If Not arrHots Is Nothing AndAlso arrHots.Count > 0 Then
                '    Dim obj As NV_NewsInfo = CType(arrHots(0), NV_NewsInfo)
                '    '1. Hot 1
                '    SubtractIds = obj.NewId.ToString()
                '    If Not obj Is Nothing Then
                '        ltrhotimage.Text = "<a href='" & Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title) & "' title='" & ReplaceChuoi.titlenews(obj.Title) & "'><img class='img-responsive img-full lazy' src='/data/nophoto300-200.png' data-src='" & Ultis.FormatThumbImage(obj.ImagePath, 300, 200, "crop", "middlecenter", "") & "' alt='" & ReplaceChuoi.titlenews(obj.Title) & "' /></a> "
                '        ltrhottitle.Text = "<a href='" & Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title) & "' title='" & ReplaceChuoi.titlenews(obj.Title) & "'><strong>" & obj.Title & "</strong></a>"
                '        ltrhotdate.Text = BL.FormatDate(obj.PublishedDate)
                '        ltrhotsum.Text = obj.Summary
                '        ltrhotdoctiep.Text = "<a href='" & Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(obj.CategoryId), obj.NewId, obj.Title) & "' title='" & ReplaceChuoi.titlenews(obj.Title) & "'><span class='read-more'>đọc tiếp</span></a>"
                '    End If
                '    arrHots.RemoveAt(0)
                '    Me.rptHot.DataSource = arrHots
                '    Me.rptHot.DataBind()
                'End If
                sw.Stop()
                ltrllia.Text = sw.ElapsedMilliseconds.ToString() & "-ms"
            End If
        End Sub
    End Class
End Namespace

