Imports System.IO
Imports System.Xml
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Security.Permissions
Imports NVCMS.Modules.TinTuc
Namespace DesktopModules.TinTuc.Control
    Partial Class MenuAdmin
        Inherits PortalModuleBase
        Dim ctlnews As New NV_NewsController
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
        Public Property KeySearch() As String
            Get
                If Not ViewState.Item("KeySearch") Is Nothing Then
                    Return ViewState.Item("KeySearch")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("KeySearch", value)
            End Set
        End Property
        Public Property Datefrom() As String
            Get
                If Not ViewState.Item("Datefrom") Is Nothing Then
                    Return ViewState.Item("Datefrom")
                Else
                    Return "01/01/2010"
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
                    Return "01/01/2100"
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Add("todate", value)
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
        Public Property CreatedUser() As Integer
            Get
                If Not ViewState.Item("CreatedUser") Is Nothing Then
                    Dim x As Integer = 0
                    Try : x = CInt(ViewState.Item("CreatedUser")) : Catch ex As Exception : x = 0 : End Try
                    Return x
                Else
                    ViewState.Add("CreatedUser", "0")
                    Return 0
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("CreatedUser") = Value.ToString
            End Set
        End Property
        Public Property Status() As Integer
            Get
                If Not ViewState.Item("Status") Is Nothing Then
                    Dim x As Integer = -1
                    Try : x = CInt(ViewState.Item("Status")) : Catch ex As Exception : x = -1 : End Try
                    Return x
                Else
                    ViewState.Add("Status", "-1")
                    Return -1
                End If
            End Get
            Set(ByVal Value As Integer)
                ViewState.Item("Status") = Value.ToString
            End Set
        End Property
#End Region
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                ltrmenu.Text = GetTopMenu()
                ltrmenusub.Text = GetMenuSub()
                'Lay thong tin user
                Dim obju As UserInfo
                Dim ctluser As New UserController
                obju = ctluser.GetUser(PortalId, UserId)
                If Not obju Is Nothing Then
                    With obju
                        imgAvtar.ImageUrl = .Profile.GetPropertyValue("Avatar")
                        imgAvtar2.ImageUrl = .Profile.GetPropertyValue("Avatar")
                        ltrname.Text = BL.GetButDanh(PortalId, UserId)
                        ltremail.Text = .Email
                    End With
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
        Public Function CanViewControl(ByVal itabid As Integer) As String
            Try
                If UserId = 1 Or UserInfo.IsInRole("Administrators") Then
                    Return "display: block"
                End If
                Dim objTab As Entities.Tabs.TabInfo = New Entities.Tabs.TabController().GetTab(itabid, PortalId, True)
                For i = 0 To objTab.TabPermissions.Count - 1
                    Dim o As TabPermissionInfo = objTab.TabPermissions(i)
                    If UserId = o.UserID AndAlso o.PermissionKey = "VIEW" Then
                        Return "display: block"
                    ElseIf UserInfo.IsInRole(o.RoleName) Then
                        Return "display: block"
                    End If
                Next
                Return "display: none"

                If Not DotNetNuke.Security.Permissions.TabPermissionController.CanViewPage(New DotNetNuke.Entities.Tabs.TabController().GetTab(itabid, PortalId, True)) Then
                    Return False
                Else
                    Return True
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function
        Public Function GetTopMenu() As String
            Dim sLanguage As String = "vi-VN"
            'Lấy cache
            Dim cache As Hashtable = DataCache.GetCache(Of Hashtable)("Menu" + sLanguage)
            If (cache Is Nothing) Then
                cache = New Hashtable
            End If
            'Nếu đối tượng đang xét chưa đăng ký cache mà chương trình có sử dụng cache thì đăng ký cho nó
            If Not cache.ContainsKey("MenusTopMenuCacheSetting" + sLanguage) Then
                If File.Exists(PortalSettings.HomeDirectoryMapPath & "\MenuAdmin\" & "menu.xml") Then
                    'Lấy đường dẫn file menu
                    Dim mXmlPath As String = PortalSettings.HomeDirectory & "MenuAdmin/" & "menu.xml"
                    'Đọc file
                    Dim xmlDoc As New System.Xml.XmlDocument
                    xmlDoc.Load(Server.MapPath(mXmlPath))
                    Dim objRoot As XmlElement = xmlDoc.DocumentElement

                    'Lưu file này lên cache-->Chỉ để sử dụng cho trường hợp tìm mnid của tin tức hay sản phẩm
                    cache.Item("MenusTopMenuXMLDocCacheSetting" + sLanguage + ModuleId.ToString) = xmlDoc

                    'Tạo menu
                    Dim name As String = String.Empty
                    Dim mota As String = String.Empty
                    Dim link As String = String.Empty
                    Dim mnid As String = String.Empty
                    Dim iTabid As Integer = Null.NullInteger
                    Dim sParam As String = String.Empty
                    Dim iTaget As String = String.Empty
                    Dim sbMenu As New StringBuilder()
                    Dim arrMainCateInfo As XmlNodeList = objRoot.ChildNodes
                    For Each objMenusInfo As XmlNode In arrMainCateInfo
                        name = objMenusInfo.Attributes("Text").Value
                        mota = objMenusInfo.Attributes("Background").Value
                        link = objMenusInfo.Attributes("NavUrl").Value
                        mnid = objMenusInfo.Attributes("Value").Value
                        iTabid = Integer.Parse(objMenusInfo.Attributes("TabId").Value)
                        iTaget = objMenusInfo.Attributes("Target").Value
                        If objMenusInfo.Attributes("Params") Is Nothing Then
                            sParam = ""
                        ElseIf objMenusInfo.Attributes("Params").Value.IndexOf("=") > 0 Then
                            sParam = objMenusInfo.Attributes("Params").Value.ToString
                        Else
                            sParam = objMenusInfo.Attributes("Params").Value.ToString + "=" + objMenusInfo.Attributes("Value").Value.ToString
                        End If
                        'Dim mainMenu As New mainMenu(name, link, mnid, iTabid, sParam)
                        If objMenusInfo.ChildNodes.Count = 0 Then
                            sbMenu.AppendFormat("<li class='nk-menu-item {0}' style='{1}'><a href='#' data-toggle='tooltip' data-placement='right' title='{2}' class='nk-menu-link nk-menu-switch' data-target='{3}'>{4}</a></li>", MenuActive(iTabid), CanViewControl(iTabid), mota, mnid, name)
                        Else
                            sbMenu.AppendFormat("<li class='nk-menu-item {0}' style='{1}'><a href='#' data-toggle='tooltip' data-placement='right' title='{2}' class='nk-menu-link nk-menu-switch' data-target='{3}'>{4}</a></li>", MenuActive(iTabid), CanViewControl(iTabid), mota, mnid, name)
                        End If
                    Next

                    'Set lên cache
                    cache.Item("MenusTopMenuCacheSetting" + sLanguage) = sbMenu.ToString()
                End If
                'If DotNetNuke.Common.Globals.PerformanceSetting <> PerformanceSettings.NoCaching Then
                '    DataCache.SetCache("MenusCacheSetting" + sLanguage, cache)
                'End If
            End If

            Return cache.Item("MenusTopMenuCacheSetting" + sLanguage)
        End Function
        Public Function GetMenuSub() As String
            Dim sLanguage As String = "vi-VN"
            'Lấy cache
            Dim cache As Hashtable = DataCache.GetCache(Of Hashtable)("Menu" + sLanguage)
            If (cache Is Nothing) Then
                cache = New Hashtable
            End If
            'Nếu đối tượng đang xét chưa đăng ký cache mà chương trình có sử dụng cache thì đăng ký cho nó
            If Not cache.ContainsKey("MenusTopMenuCacheSetting" + sLanguage) Then
                If File.Exists(PortalSettings.HomeDirectoryMapPath & "\MenuAdmin\" & "menu.xml") Then
                    'Lấy đường dẫn file menu
                    Dim mXmlPath As String = PortalSettings.HomeDirectory & "MenuAdmin/" & "menu.xml"
                    'Đọc file
                    Dim xmlDoc As New System.Xml.XmlDocument
                    xmlDoc.Load(Server.MapPath(mXmlPath))
                    Dim objRoot As XmlElement = xmlDoc.DocumentElement

                    'Lưu file này lên cache-->Chỉ để sử dụng cho trường hợp tìm mnid của tin tức hay sản phẩm
                    cache.Item("MenusTopMenuXMLDocCacheSetting" + sLanguage + ModuleId.ToString) = xmlDoc

                    'Tạo menu
                    Dim name As String = String.Empty
                    Dim mota As String = String.Empty
                    Dim link As String = String.Empty
                    Dim mnid As String = String.Empty
                    Dim iTabid As Integer = Null.NullInteger
                    Dim sParam As String = String.Empty
                    Dim iTaget As String = String.Empty
                    Dim sbMenu As New StringBuilder()
                    Dim arrMainCateInfo As XmlNodeList = objRoot.ChildNodes
                    For Each objMenusInfo As XmlNode In arrMainCateInfo
                        name = objMenusInfo.Attributes("Text").Value
                        mota = objMenusInfo.Attributes("Background").Value
                        link = objMenusInfo.Attributes("NavUrl").Value
                        mnid = objMenusInfo.Attributes("Value").Value
                        iTabid = Integer.Parse(objMenusInfo.Attributes("TabId").Value)
                        iTaget = objMenusInfo.Attributes("Target").Value
                        If objMenusInfo.Attributes("Params") Is Nothing Then
                            sParam = ""
                        ElseIf objMenusInfo.Attributes("Params").Value.IndexOf("=") > 0 Then
                            sParam = objMenusInfo.Attributes("Params").Value.ToString
                        Else
                            sParam = objMenusInfo.Attributes("Params").Value.ToString + "=" + objMenusInfo.Attributes("Value").Value.ToString
                        End If
                        'Dim mainMenu As New mainMenu(name, link, mnid, iTabid, sParam)
                        If objMenusInfo.ChildNodes.Count = 0 Then
                        Else
                            sbMenu.AppendFormat("<div class='nk-menu-content {0}' data-content='{1}'>", MenuActiveSub(iTabid), mnid)
                            sbMenu.AppendFormat("<h5 class='title'>{0}</h5>", mota)
                            sbMenu.AppendFormat("<ul Class='nk-menu'>")
                            'Cap 2 
                            Dim arrSubCateInfo As XmlNodeList = objMenusInfo.ChildNodes
                            For i As Integer = 0 To arrSubCateInfo.Count - 1
                                Dim objAISubMenuInfo As XmlNode = arrSubCateInfo(i)

                                name = objAISubMenuInfo.Attributes("Text").Value
                                link = objAISubMenuInfo.Attributes("NavUrl").Value
                                mnid = objMenusInfo.Attributes("Value").Value
                                iTabid = Integer.Parse(objAISubMenuInfo.Attributes("TabId").Value)
                                If objAISubMenuInfo.Attributes("Params") Is Nothing Then
                                    sParam = ""
                                ElseIf objAISubMenuInfo.Attributes("Params").Value.IndexOf("=") > 0 Then
                                    sParam = objAISubMenuInfo.Attributes("Params").Value.ToString
                                Else
                                    sParam = objAISubMenuInfo.Attributes("Params").Value.ToString + "=" + objAISubMenuInfo.Attributes("Value").Value.ToString
                                End If
                                'Dim subMenu As New subMenu(name, link, mnid, iTabid, sParam)
                                If objAISubMenuInfo.ChildNodes.Count = 0 Then
                                    sbMenu.AppendFormat("<li style='{0}' class='nk-menu-item {1}'><a href='{2}' class='nk-menu-link'>{3} {4}</a></li>", CanViewControl(iTabid), MenuActive(iTabid), link, name, ThongKeBaiViet(iTabid))
                                    'Else
                                    '    If i > 0 Then
                                    '        'sbMenu.AppendFormat("<li class='has-sub'></li>")
                                    '    End If
                                    '    sbMenu.AppendFormat("<li><a href=""{0}"">{1} <span class='fa fa-chevron-down'></span></a>", link, name)
                                    '    'Cap 3
                                    '    Dim arrLeafCateInfo As XmlNodeList = objAISubMenuInfo.ChildNodes
                                    '    sbMenu.AppendFormat("<ul class='nav child_menu'>")
                                    '    For Each objAILeafMenuInfo As XmlNode In arrLeafCateInfo
                                    '        name = objAILeafMenuInfo.Attributes("Text").Value
                                    '        link = objAILeafMenuInfo.Attributes("NavUrl").Value
                                    '        mnid = objMenusInfo.Attributes("Value").Value
                                    '        iTabid = Integer.Parse(objAILeafMenuInfo.Attributes("TabId").Value)
                                    '        If objAILeafMenuInfo.Attributes("Params") Is Nothing Then
                                    '            sParam = ""
                                    '        ElseIf objAILeafMenuInfo.Attributes("Params").Value.IndexOf("=") > 0 Then
                                    '            sParam = objAILeafMenuInfo.Attributes("Params").Value.ToString
                                    '        Else
                                    '            sParam = objAILeafMenuInfo.Attributes("Params").Value.ToString + "=" + objAILeafMenuInfo.Attributes("Value").Value.ToString
                                    '        End If
                                    '        sbMenu.AppendFormat("<li><a href=""{0}"">{1}</a></li>", link, name)
                                    '        sbMenu.AppendFormat("</li>")
                                    '    Next
                                    '    sbMenu.AppendFormat("</ul>")
                                End If
                            Next
                            '---
                            sbMenu.AppendFormat("</ul>")
                            sbMenu.AppendFormat("</div>")
                        End If
                    Next

                    'Set lên cache
                    cache.Item("MenusTopMenuCacheSetting" + sLanguage) = sbMenu.ToString()
                End If
                'If DotNetNuke.Common.Globals.PerformanceSetting <> PerformanceSettings.NoCaching Then
                '    DataCache.SetCache("MenusCacheSetting" + sLanguage, cache)
                'End If
            End If

            Return cache.Item("MenusTopMenuCacheSetting" + sLanguage)
        End Function
        Public Function MenuActive(ByVal tab As Integer) As String
            Dim mytab As String = PortalSettings.ActiveTab.TabID.ToString
            Dim mytabsub As String = PortalSettings.ActiveTab.ParentId.ToString
            If tab = mytab Or tab = mytabsub Then
                Return "active"
            End If
            Return ""
        End Function
        Public Function ThongKeBaiViet(ByVal tab As Integer) As String
            Dim mytab As String = PortalSettings.ActiveTab.TabID.ToString
            Dim status As Integer = 0
            Dim CatPermission As String = "0"
            If UserId = 1 Or UserInfo.IsInRole("Administrators") Or UserInfo.IsInRole("ThukyToaSoan") Or UserInfo.IsInRole("Manager") Then
                CatPermission = "0"
            Else
                If UserInfo.IsInRole("Phe duyet") Then
                    CatPermission = Ultis.GetCaterogyIdPheDuyet(UserId, PortalId)
                Else
                    If UserInfo.IsInRole("Xuat ban") Then
                        CatPermission = Ultis.GetCaterogyIdXuatBan(UserId, PortalId)
                    End If
                End If
            End If
            Dim TotalRecord As Integer = 0
            Dim result As String = ""
            'Phong vien
            If tab = BL.pageDanhSachTinId Then
                status = NewsStatus.DangBienSoan
                TotalRecord = ctlnews.FindNews_Count(Datefrom, DateTo, KeySearch, 0, False, PortalId, status, UserId, "")
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            If tab = BL.pageDanhSachTinChoPheDuyetId Then
                status = NewsStatus.ChoPheDuyet
                TotalRecord = ctlnews.FindNews_Count(Datefrom, DateTo, KeySearch, 0, False, PortalId, status, UserId, "")
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            If tab = BL.pageDanhSachTinChoXuatBanId Then
                status = NewsStatus.ChoXuatBan
                TotalRecord = ctlnews.FindNews_Count(Datefrom, DateTo, KeySearch, 0, False, PortalId, status, UserId, "")
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            If tab = BL.pageDanhSachTinBiTraLaiId Then
                status = NewsStatus.BiTraLai
                TotalRecord = ctlnews.FindNews_Count(Datefrom, DateTo, KeySearch, 0, False, PortalId, status, UserId, "")
                result = "<span style='color:red'>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            If tab = BL.pageDanhSachTinXuatBanId Then
                status = NewsStatus.DaXuatBan
                TotalRecord = ctlnews.FindNews_Count(Datefrom, DateTo, KeySearch, 0, False, PortalId, status, UserId, "")
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            'Bien tap vien va Xuat ban
            If tab = BL.pagePheDuyetId Then
                status = NewsStatus.ChoPheDuyet
                TotalRecord = ctlnews.SelectApproveNews_Count(Datefrom, DateTo, KeySearch, CategoryId, status, PortalId, CreatedUser, False) 'Hien tai: Load tat ca - Anh + text
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            If tab = BL.pagePheDuyetXBId Then
                status = NewsStatus.ChoXuatBan
                TotalRecord = ctlnews.SelectApproveNews_Count(Datefrom, DateTo, KeySearch, CategoryId, status, PortalId, CreatedUser, False) 'Hien tai: Load tat ca - Anh + text
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            If tab = BL.pageDaXuatBanId Then
                status = NewsStatus.DaXuatBan
                TotalRecord = ctlnews.SelectApproveNews_Count(Datefrom, DateTo, KeySearch, CategoryId, status, PortalId, CreatedUser, False) 'Hien tai: Load tat ca - Anh + text
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            If tab = BL.pageHuyXuatBanId Then
                status = NewsStatus.HuyXuatBan
                TotalRecord = ctlnews.SelectApproveNews_Count(Datefrom, DateTo, KeySearch, CategoryId, status, PortalId, CreatedUser, False) 'Hien tai: Load tat ca - Anh + text
                result = "<span>&nbsp;(" & TotalRecord.ToString() & ")</span>"
            End If
            Return result
        End Function
        Public Function MenuActiveSub(ByVal tab As Integer) As String
            Dim mytab As String = PortalSettings.ActiveTab.TabID.ToString
            Dim mytabsub As String = PortalSettings.ActiveTab.ParentId.ToString
            If tab = mytab Or tab = mytabsub Then
                Return "menu-active"
            End If

            Return ""
        End Function
    End Class
End Namespace
