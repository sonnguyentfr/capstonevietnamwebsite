Imports System.IO
Imports System.Xml
Imports DotNetNuke.Services.OutputCache

Namespace DesktopModules.TinTuc.Control
    Partial Class TopHeader
        Inherits Entities.Modules.PortalModuleBase

#Region "Event Handlers"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    'ltrMenusiderebar.Text = GetMenuSiderbar()
                End If
            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Public Function GetMenuSiderbar() As String
            Dim sLanguage As String = BL.GetLanguage()

            'Lấy cache
            Dim cache As Hashtable = DataCache.GetCache(Of Hashtable)("MenusSideBarMenuCacheSetting" + sLanguage)
            If (cache Is Nothing) Then
                cache = New Hashtable
            End If

            'Nếu đối tượng đang xét chưa đăng ký cache mà chương trình có sử dụng cache thì đăng ký cho nó
            If Not cache.ContainsKey("MenusSideBarMenuCacheSetting" + sLanguage) Then
                If File.Exists(PortalSettings.HomeDirectoryMapPath & "\MenusXML\" & "MainMenu.xml") Then
                    'Lấy đường dẫn file menu
                    Dim mXmlPath As String = PortalSettings.HomeDirectory & "MenusXML/" & "MainMenu.xml"

                    'Đọc file
                    Dim xmlDoc As New System.Xml.XmlDocument
                    xmlDoc.Load(Server.MapPath(mXmlPath))
                    Dim objRoot As XmlElement = xmlDoc.DocumentElement

                    'Lưu file này lên cache-->Chỉ để sử dụng cho trường hợp tìm mnid của tin tức hay sản phẩm
                    cache.Item("MenusSideBarMenuCacheSetting" + sLanguage + ModuleId.ToString) = xmlDoc

                    'Tạo menu
                    Dim name As String = String.Empty
                    Dim link As String = String.Empty
                    Dim mnid As String = String.Empty
                    Dim iTabid As Integer = Null.NullInteger
                    Dim sParam As String = String.Empty
                    Dim iTaget As String = String.Empty
                    Dim sbMenu As New StringBuilder()
                    Dim arrMainCateInfo As XmlNodeList = objRoot.ChildNodes
                    For Each objMenusInfo As XmlNode In arrMainCateInfo
                        name = objMenusInfo.Attributes("Text").Value
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
                            sbMenu.AppendFormat("<li><a class='{0}' href='{1}' target={2}>{3}</a></li>", MenuActive(iTabid), link, iTaget, name)
                        Else
                            sbMenu.AppendFormat("<li class='{0}'>", MenuActive(iTabid))
                            sbMenu.AppendFormat("<a href='{0}'>{1}</a><div class='icon-sub-menu' data-sidenav-dropdown-toggle><span class='sidenav-dropdown-icon show' data-sidenav-dropdown-icon></span><span class='sidenav-dropdown-icon up-icon' data-sidenav-dropdown-icon></span></div>", link, name)
                            sbMenu.AppendFormat("<ul class='sidenav-dropdown' data-sidenav-dropdown>")
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
                                    sbMenu.AppendFormat("<li><a href='{0}'>{1}</a></li>", link, name)
                                Else

                                End If
                            Next
                            sbMenu.AppendFormat("</ul></li>")
                        End If
                    Next

                    'Set lên cache
                    cache.Item("MenusSideBarMenuCacheSetting" + sLanguage) = sbMenu.ToString()
                End If
                'If (DotNetNuke.Common.Globals.PerformanceSetting <> PerformanceSettings.NoCaching) Then
                '    DataCache.SetCache("MenusCacheSetting" + sLanguage, cache)
                'End If
            End If

            Return cache.Item("MenusSideBarMenuCacheSetting" + sLanguage)
        End Function
        Public Function MenuActive(ByVal tab As Integer) As String
            Dim mytab As String = PortalSettings.ActiveTab.TabID.ToString
            Dim mytabsub As String = PortalSettings.ActiveTab.ParentId.ToString
            If tab = mytab Or tab = mytabsub Then
                Return "active"
            End If

            Return ""
        End Function
#End Region

    End Class
End Namespace