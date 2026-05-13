Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports NVCMS.Modules.TinTuc

Namespace DesktopModules.TinTuc.Control
    Partial Class HeaderNews
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
                    ltrMenu.Text = GetTopMenu()
                    Dim mytab As String = PortalSettings.ActiveTab.TabID.ToString
                End If
            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
        Public Function GetTopMenu() As String

            Dim sLanguage As String = BL.GetLanguage()

            'Lấy cache
            Dim cache As Hashtable = DataCache.GetCache(Of Hashtable)("MenusTopMenuCacheSettingV2" + sLanguage)
            If (cache Is Nothing) Then
                cache = New Hashtable
            End If

            'Nếu đối tượng đang xét chưa đăng ký cache mà chương trình có sử dụng cache thì đăng ký cho nó
            If Not cache.ContainsKey("MenusTopMenuCacheSettingV2" + sLanguage) Then
                If File.Exists(PortalSettings.HomeDirectoryMapPath & "\MenusXML\" & "MainMenu.xml") Then
                    'Lấy đường dẫn file menu
                    Dim mXmlPath As String = PortalSettings.HomeDirectory & "MenusXML/" & "MainMenu.xml"

                    'Đọc file
                    Dim xmlDoc As New System.Xml.XmlDocument
                    xmlDoc.Load(Server.MapPath(mXmlPath))
                    Dim objRoot As XmlElement = xmlDoc.DocumentElement

                    'Lưu file này lên cache-->Chỉ để sử dụng cho trường hợp tìm mnid của tin tức hay sản phẩm
                    cache.Item("MenusTopMenuCacheSettingV2" + sLanguage + ModuleId.ToString) = xmlDoc

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
                            sbMenu.AppendFormat("<li class='{0} nav-item'><div class='d-flex align-items-center'><span class='icon-cate'><i class='fas fa-newspaper'></i></span><a class='text-uppercase nav-link' href='{1}'>{2}</a><span class='btn-arrow-sub'><i class='fa fa-arrow-down' aria-hidden='true'></i></span></div></li>", MenuActive(iTabid), link, name)
                        Else
                            sbMenu.AppendFormat("<li class='{0} nav-item'><div class='d-flex align-items-center'><span class='icon-cate'><i class='fas fa-newspaper'></i></span><a class='text-uppercase nav-link' href='{1}'>{2}</a><span class='btn-arrow-sub'><i class='fa fa-arrow-down' aria-hidden='true'></i></span></div>", MenuActive(iTabid), link, name)
                            sbMenu.AppendFormat("<ul class='sub'>")
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
                                    sbMenu.AppendFormat("<li><a href=""{0}"">{1}</a></li>", link, name)
                                Else
                                    'If i > 0 Then
                                    '    'sbMenu.AppendFormat("<li class='has-sub'></li>")
                                    'End If
                                    sbMenu.AppendFormat("<li class='dropdown-submenu'><a class='dropdown-item dropdown-toggle' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false' href='{0}'>{1}</a>", link, name)
                                    'Cap 3
                                    Dim arrLeafCateInfo As XmlNodeList = objAISubMenuInfo.ChildNodes
                                    sbMenu.AppendFormat("<ul class='dropdown-menu'>")
                                    For Each objAILeafMenuInfo As XmlNode In arrLeafCateInfo
                                        name = objAILeafMenuInfo.Attributes("Text").Value
                                        link = objAILeafMenuInfo.Attributes("NavUrl").Value
                                        mnid = objMenusInfo.Attributes("Value").Value
                                        iTabid = Integer.Parse(objAILeafMenuInfo.Attributes("TabId").Value)
                                        If objAILeafMenuInfo.Attributes("Params") Is Nothing Then
                                            sParam = ""
                                        ElseIf objAILeafMenuInfo.Attributes("Params").Value.IndexOf("=") > 0 Then
                                            sParam = objAILeafMenuInfo.Attributes("Params").Value.ToString
                                        Else
                                            sParam = objAILeafMenuInfo.Attributes("Params").Value.ToString + "=" + objAILeafMenuInfo.Attributes("Value").Value.ToString
                                        End If
                                        sbMenu.AppendFormat("<li><a class='dropdown-item' href=""{0}"">{1}</a></li>", link, name)
                                        'Dim leafMenu As New leafMenu(name, link, mnid, iTabid, sParam)

                                    Next

                                    sbMenu.AppendFormat("</ul></li>")
                                End If
                            Next
                            sbMenu.AppendFormat("</ul></li>")
                        End If
                    Next

                    'Set lên cache
                    cache.Item("MenusTopMenuCacheSettingV2" + sLanguage) = sbMenu.ToString()
                End If
                If (DotNetNuke.Common.Globals.PerformanceSettings.HeavyCaching <> PerformanceSettings.NoCaching) Then
                    DataCache.SetCache("MenusTopMenuCacheSettingV2" + sLanguage, cache)
                End If
            End If

            Return cache.Item("MenusTopMenuCacheSettingV2" + sLanguage)

        End Function
        Public Function MenuActive(ByVal tab As Integer) As String

            Dim result As String = ""
            Dim mytab As String = PortalSettings.ActiveTab.TabID.ToString
            Dim mytabsub As String = PortalSettings.ActiveTab.ParentId.ToString
            fbclid = Request.Item("fbclid")
            Dim sUrl1 As String = Request.RawUrl
            Dim sUrl As String = sUrl1.Replace("?fbclid=" & fbclid, "")
            'Dim sUrl As String = Request.RawUrl
            ItemID = Ultis.GetRequestId(sUrl)
			if ItemID > 0 then
				Dim ctlNews As New NV_NewsController
				Dim objNews As NV_NewsInfo = ctlNews.GetByID(ItemID)
				If Not objNews Is Nothing Then
					With objNews
						Dim ctlcat As New NV_NewsCategoriesController
						Dim objcat As NV_NewsCategoriesInfo
						objcat = ctlcat.GetByID(.CategoryId)

						If (tab = BL.GetMappingTabIDByCategoryID(objcat.CategoryID)) Or (tab = BL.GetMappingTabIDByCategoryID(objcat.ParentId)) Then
							result = ""
						End If
					End With
				Else
					If tab = mytab Or tab = mytabsub Then
						result = ""
					End If
				End If
			end if
            
            Return result
        End Function

#End Region

    End Class
End Namespace