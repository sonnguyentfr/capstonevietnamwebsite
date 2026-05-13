Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Control
    Partial Class Tinlienquan
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase
        Public Property TotalItem() As Integer
            Get
                If Not ViewState.Item("TotalItem") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState("TotalItem"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("TotalItem", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("TotalItem") = Value.ToString()
            End Set
        End Property
        Public Property Title() As String
            Get
                If Not ViewState.Item("Title") Is Nothing Then
                    Try
                        Return ViewState.Item("Title").ToString()
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("Title", "")
                    Return 0
                End If
            End Get
            Set(ByVal Value As String)
                ViewState.Item("Title") = Value.ToString()
            End Set
        End Property
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
        Public Property CategoryID() As Integer
            Get
                If Not ViewState.Item("CategoryID") Is Nothing Then
                    Try
                        Return Integer.Parse(CType(ViewState.Item("CategoryID"), String))
                    Catch ex As Exception
                        Return 0
                    End Try
                Else
                    ViewState.Add("CategoryID", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CategoryID") = Value.ToString
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
        Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                fbclid = Request.Item("fbclid")
                Dim sUrl1 As String = Request.RawUrl
                Dim sUrl As String = sUrl1.Replace("?fbclid=" & fbclid, "")
                ' Dim sUrl As String = Request.RawUrl
                ItemID = Ultis.GetRequestId(sUrl)
                Dim ctlNews As New NV_NewsController
                ''TIN LIÊN QUAN
                '---
                Dim objNews As NV_NewsInfo = ctlNews.GetByID(ItemID)
                If Not objNews Is Nothing Then
                    With objNews
                        If Not String.IsNullOrEmpty(.Links) Then
                            Dim arrRelated As New ArrayList
                            Dim strArr As String() = objNews.Links.Split(CType(";", Char))
                            For i As Integer = 0 To strArr.Length - 1
                                If IsNumeric(strArr(i)) Then
                                    Dim obj2x As NV_NewsInfo = ctlNews.GetByID(CType(strArr(i), Integer))
                                    If Not obj2x Is Nothing Then
                                        With obj2x
                                            arrRelated.Add(obj2x)
                                        End With
                                    End If
                                End If
                            Next
                            repeateReleated1.DataSource = arrRelated
                            repeateReleated1.DataBind()
                        Else
                            tinlienquan.Visible = False
                        End If
                    End With
                End If

                'ltTitle.Text = objNews.CategoryName
                'hplCat.NavigateUrl = NavigateURL(BL.GetMappingTabIDByCategoryID(objNews.CategoryId))
                'hplCat.ToolTip = objNews.CategoryName
                'Me.drgOtherNews.DataSource = ctlNews.selectnewsinsamecat(ItemID, objNews.CategoryId, 4)
                'Me.drgOtherNews.DataBind()
                'Dim arr2 As ArrayList = ctlNews.selectnewsinsamecat(ItemID, objNews.CategoryId, 10)
                'arr2.RemoveAt(0)
                'arr2.RemoveAt(0)
                'arr2.RemoveAt(0)
                'arr2.RemoveAt(0)
                'drgOtherNews2.DataSource = arr2
                'drgOtherNews2.DataBind()

            End If
        End Sub
    End Class
End Namespace

