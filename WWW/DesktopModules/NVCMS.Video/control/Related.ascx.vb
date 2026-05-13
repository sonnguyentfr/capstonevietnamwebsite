Imports NVCMS.Modules.Video

Namespace DesktopModules.TinTuc.Control
    Partial Class Related
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
                'Dim sUrl As String = Request.RawUrl
                ItemID = Ultis.GetRequestId(sUrl)
                Dim ctlNews As New Videos_Controller
                Dim objNews As Videos_Info = ctlNews.GetByID(ItemID, 0)
                If Not objNews Is Nothing Then
                    With objNews
                        'Dim arr2 As ArrayList = ctlNews.ShowSelectNewsInSameCat(ItemID, .CategoryId, 9)
                        Dim arr2 As ArrayList = ctlNews.Find_Show_Index(0, 1, 18)
                        If Not arr2 Is Nothing AndAlso arr2.Count > 0 Then
                            If DataCache.GetCache(BL.NewsHomeCat & CategoryID & "Tinlienquan") Is Nothing Then
                                DataCache.SetCache(BL.NewsHomeCat & CategoryID & "Tinlienquan", arr2, Nothing, DateTime.Now.AddSeconds(10), TimeSpan.Zero)
                                drgOtherNews.DataSource = arr2
                                drgOtherNews.DataBind()
                            Else
                                drgOtherNews.DataSource = DataCache.GetCache(BL.NewsHomeCat & CategoryID & "Tinlienquan")
                                drgOtherNews.DataBind()
                            End If
                        End If

                    End With
                End If


            End If
        End Sub

    End Class
End Namespace

