
Imports System.IO
Imports System.Web.Services
Imports DotNetNuke.Entities.Modules
Namespace NVCMS.Modules.Tintuc

    Partial Class LoadMore
        Inherits DotNetNuke.Framework.CDefault
#Region "Properties"
        Public Property TotalPage() As Integer
            Get
                If Not ViewState.Item("TotalPage") Is Nothing Then
                    Try
                        Return CInt(ViewState.Item("TotalPage"))
                    Catch ex As Exception
                        Return Null.NullInteger
                    End Try
                Else
                    ViewState.Add("TotalPage", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("TotalPage") = Value.ToString
            End Set
        End Property
        Property CurrentPage() As Integer 'Trang hiện tại
            Get
                If Not ViewState.Item("CurrentPage") Is Nothing Then
                    Return CInt(ViewState.Item("CurrentPage"))
                Else
                    ViewState.Add("CurrentPage", "1")
                    Return 1
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("CurrentPage") = value.ToString
            End Set
        End Property
        Property PageSize() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("PageSize") Is Nothing Then
                    Return CInt(ViewState.Item("PageSize"))
                Else
                    ViewState.Add("PageSize", "10")
                    Return 10
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("PageSize") = value.ToString
            End Set
        End Property
        Property TotalRecord() As Integer 'Số bản ghi trên trang
            Get
                If Not ViewState.Item("TotalRecord") Is Nothing Then
                    Return CInt(ViewState.Item("TotalRecord"))
                Else
                    ViewState.Add("TotalRecord", "0")
                    Return 0
                End If
            End Get
            Set(ByVal value As Integer)
                ViewState.Item("TotalRecord") = value.ToString
            End Set
        End Property
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
#End Region
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            Try
                Dim iPage As Integer = 6
                Dim pageid As Integer = CType(Request.QueryString("pageid"), Integer)
                Dim catid As Integer = CType(Request.QueryString("catid"), Integer)
                Dim objController As New NV_NewsController
                Dim arrList As ArrayList = objController.SelectIndex("", catid, 0, pageid, iPage, "", False)
                'Response.Write(arrList.Count)
                Dim sbd = New StringBuilder
                'Dim _template = "<div class=""col-md-4 col-sm-12 col-xs-12""><div class=""photo""><a href=""{0}""><img class=""img-responsive"" src=""{1}"" /></a></div><div class=""title""><a href=""{2}"">{3}</a></div></div>"
                'Dim _template = "<div class='item'>" _
                '                & "<a class='post-thumb' href='{0}' title='{1}'>" _
                '                    & "<img class='lazy' src='/static/nvcms/img/assets/lazy-empty.png' data-src='{2}' alt='{3}' />" _
                '                & "</a>" _
                '                & "<div class='post-content'>" _
                '                    & "<div class='category blue-text'>{4}</div>" _
                '                    & "<div class='meta'>{5}</div>" _
                '                    & "<div class='entry-title'>" _
                '                        & "<a href='{6}' title='{7}'>{8}</a>" _
                '                    & "</div>" _
                '                & "</div>" _
                '            & "</div>"
                Dim _template = "<div class='item-news mt-3 pt-3 border-top {0}'><div class='start-date'><span class='pe-1'>{1}</div><h5 class='title-clamp-20'><a href='{2}'>{3}</a></h5><div class='row mt-3'><div class='col-5 col-lg-4 pe-0 pe-md-2 side-bar-img'><a href='{4}'><img src='{5}' class='lazy'></a></div><div class='col-7 col-lg-8 txt-dec'><a href='{6}'>{7}</a></div></div></div>"
                For Each obj As NV_NewsInfo In arrList
                    sbd.Append(String.Format(_template,
                                             Showhotcat(obj.NewId),
                                             BL.FormatDate(obj.PublishedDate),
                                             Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(obj.CategoryId, Integer)), CType(obj.NewId, Integer), CType(obj.Title, String)),
                                             obj.Title,
                                             Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(obj.CategoryId, Integer)), CType(obj.NewId, Integer), CType(obj.Title, String)),
                                             Ultis.FormatThumbImage(obj.ImagePath, 240, 160, "crop", "middlecenter", ""),
                                             Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(obj.CategoryId, Integer)), CType(obj.NewId, Integer), CType(obj.Title, String)),
                                             obj.Summary
                                             )
                                    )
                Next
                sbd.AppendLine("<!-- Trong bai viet --><div style='margin: 15px 0px;padding: 5px 5px;border: dashed 2px #e8e8e8;'><div class='middle_code_post'><div class='middle_code_post-inside'><ins class='adsbygoogle' style='display: block; text-align: center;' data-ad-layout='in-article' data-ad-format='fluid' data-ad-client='ca-pub-3311450421751656' data-ad-slot='2677822351'></ins><script>(adsbygoogle = window.adsbygoogle || []).push({});</script></div></div></div>")
                ltrContent.Text = sbd.ToString()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Public Function Showhotcat(id As Integer) As String
            Dim ctl As New NV_NewsController
            Dim obj As NV_NewsInfo '
            Dim sresult As String = ""
            obj = ctl.GetByID(id)
            If Not obj Is Nothing Then
                With obj
                    If .Hotcat Then
                        sresult = "hotcat"
                    End If
                End With
            End If
            Return sresult
        End Function
    End Class
End Namespace