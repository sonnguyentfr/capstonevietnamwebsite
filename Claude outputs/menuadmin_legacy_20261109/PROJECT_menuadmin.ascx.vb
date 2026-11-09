Imports System.IO
Imports System.Xml
Imports DotNetNuke
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services
Imports DotNetNuke.Services.Exceptions.Exceptions

Namespace NVCMS.Modules.HeThong

    Partial Class menuadmin
        Inherits Entities.Modules.PortalModuleBase

#Region "Variable"
        Private mXmlPath As String = Null.NullString 'Lưu đường dẫn tới file xml
        Private RoleGroupId As Integer = -1
#End Region

#Region "Property"
        Property CurrentXML() As String
            Get
                If Not ViewState("CurrentXml") Is Nothing Then
                    Return ViewState("CurrentXml")
                Else
                    ViewState.Add("CurrentXml", "")
                    Return ""
                End If
            End Get
            Set(ByVal value As String)
                ViewState("CurrentXml") = value.ToString
            End Set
        End Property
        Property CurrentValueNode() As String
            Get
                If Not ViewState("CurrentValueNode") Is Nothing Then
                    Return ViewState("CurrentValueNode")
                Else
                    ViewState.Add("CurrentValueNode", Null.NullInteger.ToString)
                    Return Null.NullInteger.ToString
                End If
            End Get
            Set(ByVal value As String)
                ViewState("CurrentValueNode") = value.ToString
            End Set
        End Property
        Property IsFather() As Boolean
            Get
                If Not ViewState.Item("IsFather") Is Nothing Then
                    Return Boolean.Parse(ViewState.Item("IsFather"))
                Else
                    ViewState.Add("IsFather", "False")
                    Return Null.NullBoolean
                End If
            End Get
            Set(ByVal value As Boolean)
                If ViewState.Item("IsFather") Is Nothing Then
                    ViewState.Add("IsFather", value)
                Else
                    ViewState.Item("IsFather") = value
                End If
            End Set
        End Property
        Property IsNewNode() As Boolean
            Get
                If Not ViewState.Item("IsNewNode") Is Nothing Then
                    Return Boolean.Parse(ViewState.Item("IsNewNode"))
                Else
                    ViewState.Add("IsNewNode", "False")
                    Return Null.NullBoolean
                End If
            End Get
            Set(ByVal value As Boolean)
                If ViewState.Item("IsNewNode") Is Nothing Then
                    ViewState.Add("IsNewNode", value)
                Else
                    ViewState.Item("IsNewNode") = value
                End If
            End Set
        End Property
        ''' <summary>
        ''' Dùng để lưu MenuPermission
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property MenuPermissions() As String
            Get
                If Not ViewState.Item("MenuPermissions") Is Nothing Then
                    Return ViewState.Item("MenuPermissions")
                Else
                    ViewState.Add("MenuPermissions", Null.NullString)
                    Return Null.NullString
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item("MenuPermissions") = value
            End Set
        End Property
        ''' <summary>
        ''' Dùng để lưu UserPermission
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property UserPermissions() As String
            Get
                If Not ViewState.Item("UserPermissions") Is Nothing Then
                    Return ViewState.Item("UserPermissions")
                Else
                    ViewState.Add("UserPermissions", Null.NullString)
                    Return Null.NullString
                End If
            End Get
            Set(ByVal value As String)
                ViewState.Item("UserPermissions") = value
            End Set
        End Property
#End Region

#Region "Common function"
        ''' <summary>
        ''' Xóa dữ liệu ở các điều khiển
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ClearControls()
            txtNodeName.Value = String.Empty
            txtUrl.Value = String.Empty
            txtParam.Value = String.Empty
            txtBackground.Value = "Mô tả"
            drdLink.SelectedValue = "-1"
            chkLinkPopup.Checked = False
        End Sub

        ''' <summary>
        ''' Ẩn hiện các điều khiển thiết lập quyền
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GrantAllCheckChange()
            If chkGrantAll.Checked = True Then
                trGroups.Visible = False
                trRoleGrids.Visible = False
                trUser.Visible = False
                trUserGrid.Visible = False
            Else
                trGroups.Visible = True
                trRoleGrids.Visible = True
                trUser.Visible = True
                trUserGrid.Visible = True
            End If
        End Sub
        Private Sub InitializecboFiles()
            drlMenu.Items.Clear()
            Dim arrFile As ArrayList = GetFileList(PortalId, "xml", False, "MenuAdmin")
            drlMenu.DataSource = arrFile
            drlMenu.DataBind()
        End Sub
#End Region

#Region "TreeView events"
        ''' <summary>
        ''' Gán context menu mới cho tất cả các con đa cấp của một nut
        ''' </summary>
        ''' <param name="node"></param>
        ''' <remarks></remarks>
        Private Sub GetAllNodeRecursive(ByVal node As RadTreeNode)
            node.ContextMenuID = "MainContextMenu"
            'Xử lý với các con
            For Each childNode As RadTreeNode In node.Nodes
                GetAllNodeRecursive(childNode)
            Next
        End Sub
        ''' <summary>
        ''' Xử lý sự kiện kéo thả giữa 2 tree
        ''' Lưu ý bên tree thả ra phải kiểm tra nếu trước khi thả mà trên cây chỉ có một node với id=-1 thì phải xóa node này đi
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub HandleDrop(ByVal sender As Object, ByVal e As RadTreeNodeDragDropEventArgs)
            Try
                Dim sourceNode As RadTreeNode = e.SourceDragNode
                Dim destNode As RadTreeNode = e.DestDragNode
                Dim dropPosition As RadTreeViewDropPosition = e.DropPosition

                If Not destNode Is Nothing Then
                    If sourceNode.TreeView.SelectedNodes.Count <= 1 Then
                        'Gán menu context mới
                        GetAllNodeRecursive(sourceNode)
                        If catMenu.GetAllNodes.Count = 1 AndAlso catMenu.Nodes(0).Value.ToString = Null.NullInteger.ToString Then
                            sourceNode.Owner.Nodes.Remove(sourceNode)
                            catMenu.Nodes.Clear()
                            catMenu.Nodes.Add(sourceNode)
                        Else
                            PerformDragAndDrop(dropPosition, sourceNode, destNode)
                        End If
                    ElseIf sourceNode.TreeView.SelectedNodes.Count > 1 Then
                        Dim node As RadTreeNode
                        'Gán menu context mới
                        For Each node In sourceNode.TreeView.SelectedNodes
                            GetAllNodeRecursive(node)
                        Next node

                        If catMenu.GetAllNodes.Count = 1 AndAlso catMenu.Nodes(0).Value.ToString = Null.NullInteger.ToString Then
                            Select Case dropPosition
                                Case RadTreeViewDropPosition.Over, RadTreeViewDropPosition.Above
                                    ' child hoawcj sibling - above
                                    For Each node In sourceNode.TreeView.SelectedNodes
                                        PerformDragAndDrop(dropPosition, node, destNode)
                                    Next node
                                Case RadTreeViewDropPosition.Below
                                    ' sibling - below
                                    Dim count As Integer = sourceNode.TreeView.SelectedNodes.Count
                                    For i As Integer = 0 To count - 1
                                        PerformDragAndDrop(dropPosition, sourceNode.TreeView.SelectedNodes(count - 1 - i), destNode)
                                    Next
                            End Select

                            'Xóa nút gốc
                            Dim rooNode As RadTreeNode = catMenu.FindNodeByValue(Null.NullInteger.ToString)
                            If rooNode.Nodes.Count = 0 Then
                                rooNode.Owner.Nodes.Remove(rooNode)
                            Else
                                For Each node In rooNode.Nodes
                                    catMenu.Nodes.Add(node)
                                Next
                                catMenu.Nodes.Remove(rooNode)
                            End If
                        Else
                            For Each node In sourceNode.TreeView.SelectedNodes
                                PerformDragAndDrop(dropPosition, node, destNode)
                            Next node
                        End If
                    End If

                    'Bỏ chọn tất các node
                    UnselectAllNodes(catMenu)

                    'Chọn Node vừa thêm vào
                    sourceNode.Selected = True
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub 'Handle dropping events
        Private Shared Sub PerformDragAndDrop(ByVal dropPosition As RadTreeViewDropPosition, ByVal sourceNode As RadTreeNode, ByVal destNode As RadTreeNode)
            Select Case dropPosition
                Case RadTreeViewDropPosition.Over
                    ' child
                    If Not sourceNode.IsAncestorOf(destNode) Then
                        sourceNode.Owner.Nodes.Remove(sourceNode)
                        destNode.Nodes.Add(sourceNode)
                    End If
                Case RadTreeViewDropPosition.Above
                    ' sibling - above
                    sourceNode.Owner.Nodes.Remove(sourceNode)
                    destNode.InsertBefore(sourceNode)

                Case RadTreeViewDropPosition.Below
                    ' sibling - below
                    sourceNode.Owner.Nodes.Remove(sourceNode)
                    destNode.InsertAfter(sourceNode)
            End Select
            destNode.Expanded = True
        End Sub
        Protected Sub catMenu_NodeClick(sender As Object, e As RadTreeNodeEventArgs)
            If e Is Nothing Then
                Exit Sub
            End If
            Dim currentNode As RadTreeNode = e.Node
            'Lưu lại để dùng cho nút cập nhật
            CurrentValueNode = currentNode.Value
            'Xem thằng này có phải Root không. Nếu không là Root mới được phép sửa
            If Not (currentNode Is Nothing) And Not currentNode.Value.Equals(Null.NullInteger.ToString) Then
                'Gán lại tên nút
                txtNodeName.Value = currentNode.Text

                'Đánh dấu là sửa chi tiết
                IsNewNode = False

                'Tìm ra Node hiện tại đang được sửa trên treeview
                'Dim objCurNode As XmlNode = xmlDoc.SelectSingleNode("//Node[@Value='" + currentNode.Value + "']")
                Try
                    If Not currentNode.Attributes("TabId") Is Nothing Then
                        drdLink.SelectedValue = currentNode.Attributes("TabId")
                    End If
                Catch ex As Exception
                    'Trường hợp trang này đã bị xóa hoặc đổi thành trang khác

                End Try

                If Not currentNode.Attributes("NavUrl") Is Nothing Then
                    txtUrl.Value = currentNode.Attributes("NavUrl")
                End If
                If Not currentNode.Attributes("Background") Is Nothing Then
                    txtBackground.Value = currentNode.Attributes("Background")
                End If
                If Not currentNode.Attributes("Params") Is Nothing Then
                    txtParam.Value = currentNode.Attributes("Params")
                End If

                If Not currentNode.Target = "" Then
                    chkLinkPopup.Checked = True
                Else
                    chkLinkPopup.Checked = False
                End If

                If Not currentNode.Attributes("MenuPermissions") Is Nothing Then
                    'Lưu permission lên viewstate
                    MenuPermissions = currentNode.Attributes("MenuPermissions")
                Else
                    MenuPermissions = Null.NullString
                End If

                If Not currentNode.Attributes("UserPermissions") Is Nothing Then
                    'Lưu permission lên viewstate
                    UserPermissions = currentNode.Attributes("UserPermissions")
                Else
                    UserPermissions = Null.NullString
                End If

                If Not currentNode.Attributes("GrantAll") Is Nothing Then
                    chkGrantAll.Checked = True
                Else
                    chkGrantAll.Checked = False
                End If

                'Ẩn hiện các điều khiển thiết lập quyền
                GrantAllCheckChange()

                'Check lại permisstion lên grid permistion
                RoleGroupId = Int32.Parse(cboRoleGroups.SelectedValue)
                BindData()

                'Bind lại Grid UserPermission
                BindUsersGrid()
                dgUserPermissions.DataBind()
            End If
        End Sub
        ''' <summary>
        ''' Bỏ chọn tất cả các Node
        ''' </summary>
        ''' <param name="treeView"></param>
        ''' <remarks></remarks>
        Private Sub UnselectAllNodes(ByVal treeView As RadTreeView)
            Dim node As RadTreeNode
            For Each node In treeView.GetAllNodes()
                node.Selected = False
            Next node
        End Sub 'UnselectAllNodes
        ''' <summary>
        ''' Quản lý sự kiệp popup trên cây lưu trên file
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub HandleContextClick(ByVal sender As Object, ByVal e As RadTreeViewContextMenuEventArgs)
            Try
                If e Is Nothing Then
                    Exit Sub
                End If
                Dim currentNode As RadTreeNode = e.Node
                'Lưu lại để dùng cho nút cập nhật
                CurrentValueNode = currentNode.Value

                Select Case e.MenuItem.Value
                    Case "addchildren" 'Thêm chuyên mục con
                        ClearControls()

                        'Là node mới
                        IsNewNode = True

                        'Node này sẽ được làm cha.
                        IsFather = True

                    Case "addsibling" 'Thêm chuyên mục cùng cấp
                        ClearControls()

                        'Là node mới
                        IsNewNode = True

                        'Node này ko được làm cha
                        IsFather = False

                    Case "Delete" 'Xóa chuyên mục này
                        If currentNode Is Nothing Then
                            Exit Sub
                        End If

                        'Xem thằng này có phải Root không. Nếu là root thì chỉ được xóa khi cây có >=2 con
                        If currentNode.Value.Equals(Null.NullInteger.ToString) AndAlso catMenu.Nodes.Count < 2 Then
                            Exit Sub
                        End If

                        'Remove Node trên treeview
                        currentNode.Owner.Nodes.Remove(currentNode)

                        'Lấy lại cấu trúc cây
                        Dim xmlDoc As New XmlDocument
                        xmlDoc.LoadXml(catMenu.GetXml())

                        'Lưu vào file .xml
                        mXmlPath = CurrentXML
                        xmlDoc.Save(Server.MapPath(mXmlPath))

                        If catMenu.Nodes.Count = 0 Then
                            'Tạo cây menu mới
                            Dim root As RadTreeNode = CreateNewNodeForMenu("Root", Null.NullInteger.ToString)
                            catMenu.Nodes.Add(root)
                        End If

                    Case "edit" 'Sửa chi tiết
                        'Xem thằng này có phải Root không. Nếu không là Root mới được phép sửa
                        If Not (currentNode Is Nothing) And Not currentNode.Value.Equals(Null.NullInteger.ToString) Then
                            'Gán lại tên nút
                            txtNodeName.Value = currentNode.Text

                            'Đánh dấu là sửa chi tiết
                            IsNewNode = False

                            'Tìm ra Node hiện tại đang được sửa trên treeview
                            'Dim objCurNode As XmlNode = xmlDoc.SelectSingleNode("//Node[@Value='" + currentNode.Value + "']")
                            Try
                                If Not currentNode.Attributes("TabId") Is Nothing Then
                                    drdLink.SelectedValue = currentNode.Attributes("TabId")
                                End If
                            Catch ex As Exception
                                'Trường hợp trang này đã bị xóa hoặc đổi thành trang khác

                            End Try

                            If Not currentNode.Attributes("NavUrl") Is Nothing Then
                                txtUrl.Value = currentNode.Attributes("NavUrl")
                            End If
                            If Not currentNode.Attributes("Background") Is Nothing Then
                                txtBackground.Value = currentNode.Attributes("Background")
                            End If
                            If Not currentNode.Attributes("Params") Is Nothing Then
                                txtParam.Value = currentNode.Attributes("Params")
                            End If

                            If Not currentNode.Target = "" Then
                                chkLinkPopup.Checked = True
                            Else
                                chkLinkPopup.Checked = False
                            End If

                            If Not currentNode.Attributes("MenuPermissions") Is Nothing Then
                                'Lưu permission lên viewstate
                                MenuPermissions = currentNode.Attributes("MenuPermissions")
                            Else
                                MenuPermissions = Null.NullString
                            End If

                            If Not currentNode.Attributes("UserPermissions") Is Nothing Then
                                'Lưu permission lên viewstate
                                UserPermissions = currentNode.Attributes("UserPermissions")
                            Else
                                UserPermissions = Null.NullString
                            End If

                            If Not currentNode.Attributes("GrantAll") Is Nothing Then
                                chkGrantAll.Checked = True
                            Else
                                chkGrantAll.Checked = False
                            End If

                            'Ẩn hiện các điều khiển thiết lập quyền
                            GrantAllCheckChange()

                            'Check lại permisstion lên grid permistion
                            RoleGroupId = Int32.Parse(cboRoleGroups.SelectedValue)
                            BindData()

                            'Bind lại Grid UserPermission
                            BindUsersGrid()
                            dgUserPermissions.DataBind()
                        End If
                End Select
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

#Region "Toolbar events"
        ''' <summary>
        ''' Handle các sự kiện trên Toolbar
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>

        Private Sub tbNavigator_ButtonClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadToolBarEventArgs) Handles tbNavigator.ButtonClick
            Try
                Dim sAction As String = e.Item.Value
                Select Case sAction
                    Case "save"
                        'Lưu cấu trúc cây vào file .xml với tên là ID của loại Menu
                        mXmlPath = CurrentXML

                        Dim xmlDoc As New System.Xml.XmlDocument
                        xmlDoc.LoadXml(catMenu.GetXml())
                        xmlDoc.Save(Server.MapPath(mXmlPath))
                        'clear cache
                        DotNetNuke.Common.Utilities.DataCache.ClearCache()
                        DotNetNuke.Entities.Host.ServerController.ClearCachedServers()
                    'ToDo: thông báo cập nhật xong ở đây
                    Case "exit"
                        'Trở lại trang trước
                        Response.Redirect(NavigateURL(), True)
                End Select
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

#Region "Tạo cây"
        ''' <summary>
        ''' Sub đệ qui construct cấu trúc cây
        ''' </summary>
        ''' <param name="parent"></param>
        ''' <param name="list"></param>
        ''' <remarks></remarks>
        Public Sub CreateChildTree(ByVal parent As RadTreeNode, ByVal list As ArrayList)
            Dim childList As New List(Of MenuInfo)
            For Each obj As MenuInfo In list
                If obj.ParentId = Convert.ToInt32(parent.Value) Then
                    childList.Add(obj)
                End If
            Next
            For Each obj As MenuInfo In childList
                Dim node As RadTreeNode = CreateNewNode(obj)
                parent.Nodes.Add(node)
                node.DataItem = obj
                CreateChildTree(node, list)
            Next
        End Sub

        ''' <summary>
        ''' Tạo một nút mới cho trường hợp dựng nút cho cây gốc
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateNewNode(ByVal obj As MenuInfo) As RadTreeNode
            Dim node As New RadTreeNode(obj.Name, obj.Id.ToString)
            node.Attributes.Add("TabId", obj.TabId.ToString)
            node.Expanded = False
            node.ToolTip = obj.Id.ToString
            node.Attributes("Title") = obj.Title

            'Phần dành cho liên kết và tham biến
            node.Attributes.Add("NavUrl", obj.Url.Trim)
            If obj.Params <> Null.NullString Then
                node.Attributes.Add("Params", obj.Params.Trim)
            End If
            Return node
        End Function

        ''' <summary>
        ''' Tạo một nút mới cho trường hợp tạo nút cho menu
        ''' </summary>
        ''' <param name="text"></param>
        ''' <param name="value"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CreateNewNodeForMenu(ByVal text As String, ByVal value As String) As RadTreeNode
            Dim node As New RadTreeNode(text, value)
            node.Expanded = False
            node.ToolTip = value
            Return node
        End Function
        ''' <summary>
        ''' Tạo cây Menu đang được chỉnh sửa
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindMenu()
            Try
                catMenu.Nodes.Clear()
                'Nếu tồn tại file chứa cấu trúc cây
                If Not String.IsNullOrEmpty(CurrentXML) AndAlso File.Exists(Server.MapPath(CurrentXML)) Then
                    mXmlPath = CurrentXML
                    catMenu.LoadContentFile(mXmlPath)
                Else
                    'Tạo cây menu mới
                    Dim root As RadTreeNode = CreateNewNodeForMenu("Root", Null.NullInteger.ToString)
                    catMenu.Nodes.Add(root)
                End If
                For Each node As RadTreeNode In catMenu.GetAllNodes
                    node.ContextMenuID = "MainContextMenu"
                Next
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

#Region "Events"
        Private Function GetTabs(ByVal includeURL As Boolean) As List(Of TabInfo)
            Dim noneSpecified As String = "<" + DotNetNuke.Services.Localization.Localization.GetString("None_Specified") + ">"
            Dim tabs As List(Of TabInfo) = TabController.GetPortalTabs(PortalId, Null.NullInteger, True, noneSpecified, True, False, includeURL, True, True)
            Return tabs
        End Function

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    InitializecboFiles()
                    BindGroups()
                    CurrentXML = "/Portals/" & PortalId.ToString & "/MenuAdmin/" & drlMenu.SelectedItem.Text
                    'Fill dữ liệu cho drdLink
                    drdLink.DataSource = GetTabs(True)
                    drdLink.DataBind()

                    'Construct menu
                    BindMenu()
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Tạo url tương ứng liên kết đến trang này
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub drdLink_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drdLink.SelectedIndexChanged
            'Dim sDomain As String = HttpContext.Current.Request.ServerVariables("SERVER_NAME")
            'Dim currentNode As RadTreeNode = catMenu.FindNodeByValue(CurrentValueNode)
            'Dim sCategoryUrl As String = "c" & currentNode.Value
            'Dim seo As String = BL.ConvertTiengVietCoDauThanhKhongDau(txtNodeName.Text.Trim)
            'While True
            '    Dim parentNode As RadTreeNode = currentNode.ParentNode
            '    If Not parentNode Is Nothing Then
            '        seo = BL.ConvertTiengVietCoDauThanhKhongDau(parentNode.Text) + "/" + seo
            '        currentNode = parentNode
            '    Else
            '        Exit While
            '    End If
            'End While
            ''Code cũ
            ''Dim str As String = "http://" & sDomain & "/" & seo & "_t" & drdLink.SelectedValue.ToString

            ''Code mới: Sửa link thành không có domain

            'Dim str As String = "/" & seo & ".html" ' "_t" & drdLink.SelectedValue.ToString

            'If Not IsFather Then
            '    str = str & sCategoryUrl
            'End If
            Dim sUrl = NavigateURL(Integer.Parse(drdLink.SelectedValue))
            txtUrl.Value = sUrl
            Dim obj As TabInfo = TabController.Instance.GetTab(Integer.Parse(drdLink.SelectedValue), PortalId)
            txtNodeName.Value = obj.Title
            txtBackground.Value = obj.Title
        End Sub

        Public Function GenerateSEOForCatId(ByVal node As RadTreeNode) As String
            Dim sTemp As String = String.Empty
            sTemp = nvcmsBL.ConvertTiengVietCoDauThanhKhongDau(node.Text)
            While True
                Dim parentNode As RadTreeNode = node.ParentNode
                If Not parentNode Is Nothing Then
                    sTemp = nvcmsBL.ConvertTiengVietCoDauThanhKhongDau(parentNode.Text) + "/" + sTemp
                    node = parentNode
                Else
                    Exit While
                End If
            End While

            Return sTemp
        End Function

        ''' <summary>
        ''' Cập nhật thông tin vào trực tiếp Treeview menu
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub lnkUpdateEditParams_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUpdateEditParams.Click
            Try
                If String.IsNullOrEmpty(CurrentXML) Then
                    Exit Sub
                End If

                'Tìm ra Node hiện tại đang được chọn trên treeview
                Dim currentNode As RadTreeNode = catMenu.FindNodeByValue(CurrentValueNode)

                Dim xmlDoc As New System.Xml.XmlDocument
                mXmlPath = CurrentXML
                'Trường hợp sửa thông tin của Node đã có
                If IsNewNode = False Then
                    If currentNode Is Nothing Then
                        Exit Sub
                    End If

                    currentNode.Text = txtNodeName.Value
                    'Kiểm tra các Attributes. Nếu không có thì thêm mới. Rồi gán giá trị cho các Attributes
                    If currentNode.Attributes("TabId") Is Nothing Then
                        currentNode.Attributes.Add("TabId", drdLink.SelectedValue)
                    Else
                        currentNode.Attributes("TabId") = drdLink.SelectedValue
                    End If
                    If currentNode.Attributes("NavUrl") Is Nothing Then
                        currentNode.Attributes.Add("NavUrl", txtUrl.Value)
                    Else
                        currentNode.Attributes("NavUrl") = txtUrl.Value
                    End If
                    If currentNode.Attributes("Background") Is Nothing Then
                        currentNode.Attributes.Add("Background", txtBackground.Value)
                    Else
                        currentNode.Attributes("Background") = txtBackground.Value
                    End If
                    If currentNode.Attributes("Params") Is Nothing Then
                        currentNode.Attributes.Add("Params", txtParam.Value)
                    Else
                        currentNode.Attributes("Params") = txtParam.Value
                    End If
                    currentNode.Attributes("Title") = GenerateSEOForCatId(currentNode)

                    If chkLinkPopup.Checked Then
                        currentNode.Target = "_blank"
                    Else
                        currentNode.Target = "_self"
                    End If

                    'Nếu được gán tất quyền
                    If chkGrantAll.Checked = True Then
                        If currentNode.Attributes("GrantAll") Is Nothing Then
                            currentNode.Attributes.Add("GrantAll", "1")
                        Else
                            currentNode.Attributes("GrantAll") = "1"
                        End If
                        'Bỏ đi 2 thuộc tính phân quyền còn lại vì thằng này đã bao trùm hết cả
                        If Not currentNode.Attributes("MenuPermissions") Is Nothing Then
                            currentNode.Attributes.Remove("MenuPermissions")
                        End If
                        If Not currentNode.Attributes("UserPermissions") Is Nothing Then
                            currentNode.Attributes.Remove("UserPermissions")
                        End If
                    Else
                        'Bỏ thuộc tính GrantAll - cho tất cả người dùng được phép view
                        If Not currentNode.Attributes("GrantAll") Is Nothing Then
                            currentNode.Attributes.Remove("GrantAll")
                        End If
                        If currentNode.Attributes("MenuPermissions") Is Nothing Then
                            currentNode.Attributes.Add("MenuPermissions", MenuPermissions)
                        Else
                            currentNode.Attributes("MenuPermissions") = MenuPermissions
                        End If
                        If currentNode.Attributes("UserPermissions") Is Nothing Then
                            currentNode.Attributes.Add("UserPermissions", UserPermissions)
                        Else
                            currentNode.Attributes("UserPermissions") = UserPermissions
                        End If
                    End If
                Else 'Trường hợp tạo Node mới
                    Dim sValue As String = String.Empty
                    sValue = Guid.NewGuid.ToString 'Giá trị duy nhất để phân biệt giữa các node

                    Dim objNewNode As RadTreeNode = CreateNewNodeForMenu(txtNodeName.Value, sValue)
                    objNewNode.Attributes.Add("TabId", drdLink.SelectedValue)
                    objNewNode.Attributes.Add("NavUrl", txtUrl.Value)
                    objNewNode.Attributes.Add("Background", txtUrl.Value)
                    objNewNode.Attributes.Add("Params", txtParam.Value)
                    If chkLinkPopup.Checked Then
                        objNewNode.Target = "_blank"
                    Else
                        objNewNode.Target = "_self"
                    End If

                    If chkGrantAll.Checked = True Then
                        objNewNode.Attributes.Add("GrantAll", "1")
                    Else
                        objNewNode.Attributes.Add("MenuPermissions", MenuPermissions)
                        objNewNode.Attributes.Add("UserPermissions", UserPermissions)
                    End If

                    If IsFather = True Then
                        'Thêm con
                        If currentNode Is Nothing Then
                            catMenu.Nodes.Add(objNewNode)
                            catMenu.ExpandAllNodes()
                        Else
                            currentNode.Nodes.Add(objNewNode)
                            currentNode.ExpandChildNodes()
                        End If
                    Else
                        'Thêm Node cùng cấp
                        currentNode.Owner.Nodes.Add(objNewNode)
                    End If
                End If

                'Nếu tồn tại nút gốc --> Xóa nút gốc
                Dim rooNode As RadTreeNode = catMenu.FindNodeByValue(Null.NullInteger.ToString)
                If Not rooNode Is Nothing Then
                    If rooNode.Nodes.Count = 0 Then
                        rooNode.Owner.Nodes.Remove(rooNode)
                    Else
                        For Each node In rooNode.Nodes
                            catMenu.Nodes.Add(node)
                        Next
                        catMenu.Nodes.Remove(rooNode)
                    End If
                End If

                'Lấy lại cấu trúc cây
                xmlDoc.LoadXml(catMenu.GetXml())

                'Lưu vào file .xml
                xmlDoc.Save(Server.MapPath(mXmlPath))
                'clear cache
                DotNetNuke.Common.Utilities.DataCache.ClearCache()
                DotNetNuke.Entities.Host.ServerController.ClearCachedServers()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub
#End Region

#Region "Bind dữ liệu cho rolegroup và role"

        Private Sub BindData()
            Try
                ' Get the portal's roles from the database
                Dim objRoles As New RoleController
                Dim arrRoles As ArrayList
                Dim arrRolesExt As New ArrayList
                If RoleGroupId < -1 Then
                    arrRoles = objRoles.GetPortalRoles(PortalId)
                Else
                    arrRoles = objRoles.GetRolesByGroup(PortalId, RoleGroupId)
                End If
                'Copy toàn bộ Info của arrRoles sang arrRolesExt với property mới: Allow

                For Each obj As RoleInfo In arrRoles
                    Dim objRoleExt As New RoleExtendInfo
                    objRoleExt = objRoleExt.CopyDataFromObjRole(obj)
                    Dim arrChar As Char() = {","}
                    If ("," + MenuPermissions.Trim(arrChar) + ",").Contains("," + obj.RoleID.ToString + ",") Then
                        objRoleExt.Allow = True
                    Else
                        objRoleExt.Allow = False
                    End If
                    arrRolesExt.Add(objRoleExt)
                Next

                RadGrid1.AllowPaging = False
                RadGrid1.AutoGenerateColumns = False
                RadGrid1.MasterTableView.ShowHeadersWhenNoRecords = True
                RadGrid1.DataSource = arrRolesExt
                RadGrid1.DataBind()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub BindGroups()
            Dim liItem As ListItem
            Dim arrGroups As ArrayList = RoleController.GetRoleGroups(PortalId)

            If arrGroups.Count > 0 Then
                cboRoleGroups.Items.Clear()
                cboRoleGroups.Items.Add(New ListItem("All roles", "-1")) 'Localization.GetString("AllRoles"), "-2"))

                liItem = New ListItem("Global roles", "-1") 'Localization.GetString("GlobalRoles"), "-1")
                If RoleGroupId < 0 Then
                    liItem.Selected = True
                End If
                cboRoleGroups.Items.Add(liItem)

                For Each roleGroup As RoleGroupInfo In arrGroups
                    liItem = New ListItem(roleGroup.RoleGroupName, roleGroup.RoleGroupID.ToString)
                    If RoleGroupId = roleGroup.RoleGroupID Then
                        liItem.Selected = True
                    End If
                    cboRoleGroups.Items.Add(liItem)
                Next
            Else
                RoleGroupId = -2
            End If

            BindData()
        End Sub

        Protected Sub RadGrid1_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles RadGrid1.NeedDataSource
            Try
                BindData()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub cboRoleGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboRoleGroups.SelectedIndexChanged
            Try
                RoleGroupId = Int32.Parse(cboRoleGroups.SelectedValue)
                BindData()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub RadGrid1_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.ItemCommand
            Try
                If e.CommandName = "ChooseThisPermission" Then
                    'Xóa menu trong cơ sở dữ liệu
                    Dim sRoleId As String = e.CommandArgument.ToString
                    'Kiểm tra xem là check hay không check và lưu hay xóa trong AIMenuPermission
                    Dim chkChoose As CheckBox = e.Item.FindControl("chkChooseThisPermission")
                    Dim arrChar As Char() = {","}
                    If chkChoose.Checked Then
                        If Not ("," + MenuPermissions.Trim(arrChar) + ",").Contains("," + sRoleId + ",") Then
                            MenuPermissions = nvcmsBL.AppendPatternToString(MenuPermissions, sRoleId, ",", False)
                        End If
                    Else
                        If ("," + MenuPermissions.Trim(arrChar) + ",").Contains("," + sRoleId + ",") Then
                            MenuPermissions = nvcmsBL.RemovePatternFromString(MenuPermissions, sRoleId, ",")
                        End If
                    End If
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub chkChooseThisPermission_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
            Dim chk As CheckBox = CType(sender, CheckBox)
            If chk.Checked = True Then
                MenuPermissions = nvcmsBL.AppendPatternToString(MenuPermissions, chk.ToolTip, ",", False)
            Else
                MenuPermissions = nvcmsBL.RemovePatternFromString(MenuPermissions, chk.ToolTip, ",")
            End If
        End Sub

        Private Sub cmdAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddUser.Click
            If txtUser.Value <> "" Then
                ' verify username
                Dim objUser As UserInfo = UserController.GetUserByName(PortalSettings.PortalId, txtUser.Value)
                If Not objUser Is Nothing Then
                    lblErrMsg.Visible = False
                    SetPermissionForUser(objUser)
                Else
                    ' user does not exist
                    lblErrMsg.Visible = True
                    lblErrMsg.Text = "Not found" ' Localization.GetString("FoundNoUser", LocalResourceFile)
                End If
            End If
        End Sub

        Private Sub dgUserPermissions_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles dgUserPermissions.ItemCommand
            Try
                If e.CommandName = "RemoveUser" Then
                    UserPermissions = nvcmsBL.RemovePatternFromString(UserPermissions, e.CommandArgument.ToString, ",")
                    BindUsersGrid()
                    dgUserPermissions.DataBind()
                End If
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Protected Sub dgUserPermissions_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles dgUserPermissions.NeedDataSource
            Try
                BindUsersGrid()
            Catch ex As Exception
                ProcessModuleLoadException(Me, ex)
            End Try
        End Sub

        Private Sub BindUsersGrid()
            Dim arrUsers As New ArrayList
            Dim sUserId As String() = UserPermissions.Split(",")
            For Each sId As String In sUserId
                If Not Null.IsNull(sId) Then
                    Dim userid As Integer = Integer.Parse(sId)
                    Dim objUserInfo As UserInfo = UserController.GetUserById(PortalSettings.PortalId, userid)
                    arrUsers.Add(objUserInfo)
                End If
            Next

            dgUserPermissions.AllowPaging = False
            dgUserPermissions.AutoGenerateColumns = False
            dgUserPermissions.MasterTableView.ShowHeadersWhenNoRecords = True
            dgUserPermissions.DataSource = arrUsers


        End Sub

        Private Sub SetPermissionForUser(ByVal userinfo As UserInfo)
            Dim arrChar As Char() = {","}

            If Not ("," + UserPermissions.Trim(arrChar) + ",").Contains("," + userinfo.UserID.ToString + ",") Then
                UserPermissions = nvcmsBL.AppendPatternToString(UserPermissions, userinfo.UserID.ToString, ",", False)

                'Binh lại UserDatagrid
                BindUsersGrid()
                dgUserPermissions.DataBind()
            End If
        End Sub

        Private Sub chkGrantAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGrantAll.CheckedChanged
            GrantAllCheckChange()
        End Sub

        Private Sub drlMenu_SelectedIndexChanged(sender As Object, e As EventArgs) Handles drlMenu.SelectedIndexChanged
            CurrentXML = "/Portals/" & PortalId.ToString & "/MenuAdmin/" & drlMenu.SelectedItem.Text
            'Fill dữ liệu cho drdLink
            drdLink.DataSource = GetTabs(True)
            drdLink.DataBind()

            'Construct menu
            BindMenu()
        End Sub

#Region "Class RoleExt"

        ''' <summary>
        ''' Class mở rộng của RoleInfo 
        ''' Thêm trường thuộc tính mới là Allow --> thể hiện quyền view trên một node trên menu đối với từng User
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RoleExtendInfo
            Inherits RoleInfo

            Private _allow As Boolean
            Public Property Allow() As Boolean
                Get
                    Return _allow
                End Get
                Set(ByVal value As Boolean)
                    _allow = value
                End Set
            End Property

            Public Function CopyDataFromObjRole(ByVal roleInfo As RoleInfo) As RoleExtendInfo
                Dim roleExt As New RoleExtendInfo
                roleExt.RoleID = roleInfo.RoleID
                roleExt.RoleName = roleInfo.RoleName

                'Dim type As Type = roleInfo.GetType()
                'Dim typeExt As Type = roleExt.GetType()
                'Dim pr As PropertyInfo() = type.GetProperties()
                'Dim prExt As PropertyInfo() = typeExt.GetProperties()
                'Dim str As String
                'For i As Integer = 0 To pr.Length - 1
                '    str = pr(i).Name
                '    Dim pType As Type = pr(i).PropertyType
                '    If pr(i).GetValue(roleInfo, Nothing) Is Nothing Then
                '        If pType Is GetType(String) Then
                '            prExt(i + 1).SetValue(roleExt, "", Nothing)
                '        ElseIf pType Is GetType(Int32) Then
                '            prExt(i + 1).SetValue(roleExt, 0, Nothing)
                '        ElseIf pType Is GetType(Long) Then
                '            prExt(i + 1).SetValue(roleExt, 0, Nothing)
                '        ElseIf pType Is GetType(Boolean) Then
                '            prExt(i + 1).SetValue(roleExt, False, Nothing)
                '        ElseIf pType Is GetType(DateTime) Then
                '            prExt(i + 1).SetValue(roleExt, DateTime.MinValue, Nothing)
                '        End If
                '    Else
                '        If pType Is GetType(String) Then
                '            prExt(i + 1).SetValue(roleExt, pr(i).GetValue(roleInfo, Nothing).ToString(), Nothing)
                '        ElseIf pType Is GetType(Int32) Then
                '            prExt(i + 1).SetValue(roleExt, Int32.Parse(pr(i).GetValue(roleInfo, Nothing)), Nothing)
                '        ElseIf pType Is GetType(Long) Then
                '            prExt(i + 1).SetValue(roleExt, Int64.Parse(pr(i).GetValue(roleInfo, Nothing)), Nothing)
                '        ElseIf pType Is GetType(Boolean) Then
                '            prExt(i + 1).SetValue(roleExt, Boolean.Parse(pr(i).GetValue(roleInfo, Nothing)), Nothing)
                '        ElseIf pType Is GetType(DateTime) Then
                '            prExt(i + 1).SetValue(roleExt, DateTime.Parse(pr(i).GetValue(roleInfo, Nothing)), Nothing)
                '        End If
                '    End If
                'Next
                Return roleExt
            End Function

        End Class

#End Region

#End Region

    End Class
End Namespace