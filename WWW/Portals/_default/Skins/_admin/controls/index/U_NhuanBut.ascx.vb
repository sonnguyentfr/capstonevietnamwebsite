Imports System.IO
Imports System.Xml
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Permissions
Imports NVCMS.Modules.TinTuc
Namespace DesktopModules.TinTuc.Control
    Partial Class ThongkeNhuanButUser
        Inherits PortalModuleBase
        Dim ctlnews As New NV_NewsController
        Dim ctlnewsview As New NewsByView
        Dim ctlnhuanbut As New NhuanButController
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
                    ViewState.Add("PageSize", "20")
                    Return 20
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
        Public Property Datefrom() As String
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return ViewState.Item("Datefrom")
                Else
                    Return "01/" & DateTime.Now.ToString("MM") & "/" & DateTime.Now.ToString("yyyy")
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("Datefrom", value)
            End Set
        End Property
        Public Property DateTo() As String
            Get
                If Not ViewState.Item("todate") Is Nothing Then
                    Return ViewState.Item("todate")
                Else
                    Return DateTime.Now.ToString("dd/MM/yyyy")
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("todate", value)
            End Set
        End Property
        Private month As Integer = DateTime.Now.Month
        Private year As Integer = DateTime.Now.Year
        Private lastDay As Integer = DateTime.Now.Day


#End Region
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                GetThongTinNhuanButView()
                ltrnhuanbutthoengay.Text = GetBieuDoNhuanBut()
                ltrnviewthoengay.Text = GetBieuDoViewBai()
                GetBaiVuaXuatBan()
                GetBaiViewThap()
                GetBaiVuaXuatBanToanTrang()

            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub GetThongTinNhuanButView()
            Try
                Dim nhuanbuttheobai As Integer = ctlnhuanbut.NhuanBut_User_GetTongTien(Datefrom, DateTo, UserId)
                ltrnhuanbutorg.Text = nhuanbuttheobai.ToString()


                Dim nhuanbutthucnhan As Integer = 0
                Dim arr As New ArrayList
                arr = ctlnhuanbut.NhuanBut_Find_Index(Datefrom, DateTo, UserId, 0, PortalId, 1, 10000, 0)
                If arr.Count > 0 Then
                    For i As Integer = 0 To arr.Count - 1
                        Dim objnhuanbut As NhuanButInfo = CType(arr(i), NhuanButInfo)
                        If Not objnhuanbut Is Nothing Then
                            With objnhuanbut
                                Dim objnews As NewsByViewInfo = ctlnewsview.NewsByView_GetByNewID(.NewId)
                                If Not objnews Is Nothing Then
                                    With objnews
                                        If IsNumeric(.ViewCount) Then
                                            nhuanbutthucnhan += objnhuanbut.Credit * objnews.ViewCount / 250
                                        End If
                                    End With
                                End If
                            End With
                        End If
                    Next
                End If
                ltrnhuanbutthucnhanorg.Text = nhuanbutthucnhan
                'tinh so view
                Dim soviewthat As Integer = 0
                Dim soviewtinhnhuan As Integer = 0
                Dim viewbai As Integer = 0
                Dim arrnews As New ArrayList
                Dim TotalRecord = ctlnews.FindByStatus_Count(Datefrom, DateTo, "", 0, PortalId, NewsStatus.DaXuatBan, UserId, "")
                arrnews = ctlnews.FindByStatus_Index(Datefrom, DateTo, "", 0, PortalId, NewsStatus.DaXuatBan, UserId, 1, TotalRecord, "")
                If TotalRecord > 0 Then
                    For i As Integer = 0 To TotalRecord - 1
                        Dim objnews As NV_NewsInfo = CType(arrnews(i), NV_NewsInfo)
                        If Not objnews Is Nothing Then
                            With objnews
                                soviewthat += objnews.ViewCount
                                If .ViewCount >= 250 Then
                                    viewbai += 1
                                End If
                            End With
                        End If
                    Next
                    ltrview.Text = "<span class='auto'>" & soviewthat.ToString() & "</span>/<span class='auto'>" & TotalRecord.ToString() & "</span> bài"
                    ltrview2.Text = soviewthat.ToString()
                    soviewtinhnhuan = viewbai * 100 / TotalRecord
                    ltrviewtyle.Text = soviewtinhnhuan.ToString()

                End If
            Catch ex As Exception

            End Try
        End Sub
        Private Function GetBieuDoNhuanBut() As String
            Dim ketqua As String = "<script type='text/javascript'>" _
                        & vbCrLf & "var totalSales = {" _
                                & vbCrLf & "labels: [__NGAY___]," _
                                & vbCrLf & "dataUnit: "" vnđ""," _
                                & vbCrLf & "lineTension: .3," _
                                & vbCrLf & "datasets: [{" _
                                    & vbCrLf & "label: ""Tổng tiền""," _
                                    & vbCrLf & "color: ""#0fac81""," _
                                    & vbCrLf & "background: NioApp.hexRGB('#0fac81', .25)," _
                                    & vbCrLf & "data: [__TienNhuan__]" _
                                & vbCrLf & "}]" _
                            & vbCrLf & "};" _
                    & vbCrLf & "</script>"
            Dim thang As String = ""
            Dim tiennhuan As String = 0
            Dim startdate As DateTime = New Date(year, month, 1).ToString("yyyy-MM-dd HH:mm:ss")
            Dim enddate As DateTime = New Date(year, month, lastDay).ToString("yyyy-MM-dd HH:mm:ss")
            For Each Day As DateTime In Enumerable.Range(0, (enddate - startdate).Days + 1).Select(Function(i) startdate.AddDays(i))
                'strthongtkeanca += "{ 'y': " & ctl.Events_Student_GetCountByEventCat(Day, EventCatId) & ", 'x': " & ConvertDateEpoch(Day) & " },"
                thang += """" & Day.ToString("dd/MM") & ""","
                tiennhuan += ctlnhuanbut.NhuanBut_User_GetTongTien(Day, Day, UserId) & ","
            Next Day

            If thang.Length > 0 Then
                thang = thang.Remove(thang.Length - 1)
            End If
            If thang.Length > 0 Then
                tiennhuan = tiennhuan.Remove(tiennhuan.Length - 1)
            End If
            ketqua = ketqua.Replace("__NGAY___", thang)
            ketqua = ketqua.Replace("__TienNhuan__", tiennhuan)
            Return ketqua.ToString()
        End Function
        Private Function GetBieuDoViewBai() As String
            Try
                Dim ketqua As String = "<script type='text/javascript'>" _
                        & vbCrLf & "var totalOrders = {" _
                                & vbCrLf & "labels: [__NGAY___]," _
                                & vbCrLf & "dataUnit: "" ""," _
                                & vbCrLf & "lineTension: .3," _
                                & vbCrLf & "datasets: [{" _
                                    & vbCrLf & "label: ""Orders""," _
                                    & vbCrLf & "color: ""#0fac81""," _
                                    & vbCrLf & "background: NioApp.hexRGB('#0fac81', .25)," _
                                    & vbCrLf & "data: [__SoView__]" _
                                & vbCrLf & "}]" _
                            & vbCrLf & "};" _
                    & vbCrLf & "</script>"
                Dim thang As String = ""
                Dim soviewmoingay As String = 0
                Dim startdate As DateTime = New Date(year, month, 1).ToString("yyyy-MM-dd HH:mm:ss")
                Dim enddate As DateTime = New Date(year, month, lastDay).ToString("yyyy-MM-dd HH:mm:ss")
                For Each Day As DateTime In Enumerable.Range(0, (enddate - startdate).Days + 1).Select(Function(i) startdate.AddDays(i))
                    thang += """" & Day.ToString("dd/MM") & ""","
                    'Tin so view moi ngay

                    soviewmoingay += ctlnews.User_GetTongView(Day, Day, UserId) & ","
                Next Day

                If thang.Length > 0 Then
                    thang = thang.Remove(thang.Length - 1)
                End If
                If thang.Length > 0 Then
                    soviewmoingay = soviewmoingay.Remove(soviewmoingay.Length - 1)
                End If
                ketqua = ketqua.Replace("__NGAY___", thang)
                ketqua = ketqua.Replace("__SoView__", soviewmoingay)
                Return ketqua.ToString()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Function
        Private Sub GetBaiVuaXuatBan()
            Try
                rptbaivuaxuatban.DataSource = ctlnews.FindNews_Index(Datefrom, DateTo, "", 0, 0, PortalId, NewsStatus.DaXuatBan, UserId, 1, 10, "DATE_ASC")
                rptbaivuaxuatban.DataBind()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub GetBaiVuaXuatBanToanTrang()
            Try
                rptvuaxuatban.DataSource = ctlnews.FindNews_Index(Datefrom, DateTo, "", 0, 0, PortalId, NewsStatus.DaXuatBan, 0, 1, 17, "DATE_ASC")
                rptvuaxuatban.DataBind()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Private Sub GetBaiViewThap()
            Try
                rptViewThap.DataSource = ctlnews.FindNews_Index(Datefrom, DateTo, "", 0, 0, PortalId, NewsStatus.DaXuatBan, UserId, 1, 12, "VIEW_ASC")
                rptViewThap.DataBind()

                rptViewThapChung.DataSource = ctlnews.FindNews_Index(Datefrom, DateTo, "", 0, 0, PortalId, NewsStatus.DaXuatBan, 0, 1, 12, "VIEW_ASC")
                rptViewThapChung.DataBind()

                rptViewThapCao.DataSource = ctlnews.FindNews_Index(Datefrom, DateTo, "", 0, 0, PortalId, NewsStatus.DaXuatBan, 0, 1, 12, "VIEW_DESC")
                rptViewThapCao.DataBind()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try

        End Sub

    End Class
End Namespace
