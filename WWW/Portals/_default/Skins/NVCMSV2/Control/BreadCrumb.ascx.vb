Imports System.IO
Imports System.Xml
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Control
    Partial Class BreadCrumb
        Inherits Entities.Modules.PortalModuleBase
        Public Property ItemID() As Integer
            Get
                If Not ViewState.Item("ItemID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState.Item("ItemID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("ItemID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("ItemID") = Value.ToString
            End Set
        End Property
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
#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    fbclid = Request.Item("fbclid")
                    Dim sUrl1 As String = Request.RawUrl
                    Dim sUrl As String = sUrl1.Replace("?fbclid=" & fbclid, "")
                    'Dim sUrl As String = Request.RawUrl
                    ItemID = Ultis.GetRequestId(sUrl)
                    Dim ctlNews As New NV_NewsController
                    Dim objNews As NV_NewsInfo = ctlNews.GetByID(ItemID)
                    If Not objNews Is Nothing Then
                        With objNews
                            ltrbreadcrumb.Visible = True
                            Dim strsresult As String = ""
                            strsresult = "<span id='dnn_BreadCrumb_lblBreadCrumb' itemprop='breadcrumb' itemscope='' itemtype='https://schema.org/breadcrumb'><span itemscope='' itemtype='http://schema.org/BreadcrumbList'>"
                            strsresult += "<span itemprop='itemListElement' itemscope='' itemtype='http://schema.org/ListItem'><a href='https://thuongtruong.com.vn/' class='item' itemprop='item'><span itemprop='name'>Thương Trường</span></a><meta itemprop='position' content='1'></span>"
                            strsresult += "<i class='fa fa-1x fa-angle-right'></i>"

                            Dim ctlcat As New NV_NewsCategoriesController
                            Dim objcat As NV_NewsCategoriesInfo
                            objcat = ctlcat.GetByID(.CategoryId)
                            If Not objcat Is Nothing Then
                                With objcat
                                    If objcat.ParentId > 0 Then
                                        'Cat chat
                                        Dim objcatParent As NV_NewsCategoriesInfo = ctlcat.GetByID(.ParentId)
                                        strsresult += "<span itemprop='itemListElement' itemscope='' itemtype='http://schema.org/ListItem'><a href='" & NavigateURL(objcatParent.TabID) & "' class='item' itemprop='item'><span itemprop='name'>" & objcatParent.CategoryName & "</span></a><meta itemprop='position' content='2'></span>"
                                        strsresult += "<i class='fa fa-1x fa-angle-right'></i>"
                                        'cat con
                                        strsresult += "<span itemprop='itemListElement' itemscope='' itemtype='http://schema.org/ListItem'><a href='" & NavigateURL(objcat.TabID) & "' class='item' itemprop='item'><span itemprop='name'>" & objcat.CategoryName & "</span></a><meta itemprop='position' content='3'></span>"
                                        strsresult += "<i class='fa fa-1x fa-angle-right'></i>"
                                    Else
                                        strsresult += "<span itemprop='itemListElement' itemscope='' itemtype='http://schema.org/ListItem'><a href='" & NavigateURL(objcat.TabID) & "' class='item' itemprop='item'><span itemprop='name'>" & objcat.CategoryName & "</span></a><meta itemprop='position' content='2'></span>"
                                    End If
                                End With
                            End If
                            strsresult += "</span></span>"
                            ltrbreadcrumb.Text = strsresult
                        End With
                    Else
                        BreadCrumb.Visible = True
                    End If
                End If
            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
#End Region

    End Class
End Namespace