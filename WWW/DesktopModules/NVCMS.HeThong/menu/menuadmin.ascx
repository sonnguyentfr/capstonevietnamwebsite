<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="menuadmin.ascx.vb" Inherits="NVCMS.Modules.HeThong.menuadmin" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<style type="text/css">
    .tdleftMenu {
        width: 110px;
    }

    .dnnLabel {
        width: 110px !important;
    }

    .form-horizontal .control-label {
        padding-top: 8px;
        font-size: 13px;
        font-weight: 400;
    }

    .x_title h2 {
        margin: 5px 0 6px;
        float: left;
        display: block;
        text-overflow: ellipsis;
        overflow: inherit;
        white-space: nowrap;
        font-size: 16px;
    }

    .disable {
        display: none;
    }
</style>
<%--<asp:UpdatePanel ID="updatemenu" runat="server">
    <ContentTemplate>--%>
        
        <div class="form-inline">
            <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                <label for="drlMenu" class="mr-sm-2">Chọn danh mục</label>
                <asp:DropDownList ID="drlMenu" runat="server" DataTextField="Text" DataValueField="Value" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
            </div>
            <telerik:radtoolbar id="tbNavigator" runat="server" skin="Office2010Blue" autopostback="true" enableroundedcorners="true" enableshadows="true"
                usefadeeffect="True">
                        <items>
                            <telerik:RadToolBarButton id="button_save"  AccessKey="s"  Tooltip="Lưu (Alt+S)" Value="save" Text=" Lưu các thay đổi" ImageUrl="/Icons/Sigma/Save_16X16_Gray.png" />
                            <telerik:RadToolBarButton IsSeparator="true" />
                            <telerik:RadToolBarButton id="button_exit" AccessKey="x" Tooltip="Thoát (Alt+X)" Value="exit" Text="Thoát" ImageUrl="/Icons/Sigma/TreeViewHide_16x16_Gray.png" />
                         </items>
                        </telerik:radtoolbar>
        </div>
        <div class="divider"></div>
        <div>
            <telerik:radsplitter id="RadSplitter1" runat="server" width="100%" height="800px" skin="Office2010Blue">
                <telerik:radpane id="RightPane" runat="server" class="tdTree" Width="1000px">
                    <telerik:RadTreeView ID="catMenu" Runat="server" Skin="Outlook"
                        OnContextMenuItemClick="HandleContextClick" OnClientContextMenuItemClicking="onClientContextMenuItemClicking"
                        SingleExpandPath="False" AutoPostBack="false" EnableDragAndDrop="True" OnNodeDrop="HandleDrop" EnableDragAndDropBetweenNodes="true"
                        OnNodeClick="catMenu_NodeClick">
                        <ContextMenus>
                            <telerik:RadTreeViewContextMenu ID="MainContextMenu" runat="server">
                            <Items>
                                <telerik:RadMenuItem Value="addchildren" Text="Thêm chuyên mục con" ImageUrl="/Images/12.gif"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="addsibling" Text="Thêm chuyên mục cùng cấp" ImageUrl="/Images/12.gif"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem IsSeparator="true"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="edit" Text="Sửa chi tiết" ImageUrl="/Images/3Drafts.gif"></telerik:RadMenuItem>
                                    <telerik:RadMenuItem Value="Delete" Text="Xóa nút xử lý" ImageUrl="/Images/7.gif"></telerik:RadMenuItem>
                            </Items>
                            <CollapseAnimation Type="none" />
                        </telerik:RadTreeViewContextMenu>
                    </ContextMenus>
                    </telerik:RadTreeView>
                </telerik:radpane>
                <telerik:radsplitbar id="RadSplitBar3" runat="server"></telerik:radsplitbar>
                <telerik:radpane id="DetailPane" runat="server" Width="22px" Scrolling="None">
                    <telerik:RadSlidingZone ID="SlidingZone2" runat="server" Width="22px" ClickToOpen="true" SlideDirection="Left" DockedPaneId="pnlDetail">
                        <telerik:RadSlidingPane ID="pnlDetail" Title="CHI TIẾT" runat="server" Width="650px" MinWidth="300">
                            <div class="row">
                                <div class="col-md-12 col-xs-12">
                                    <asp:LinkButton ID="lnkUpdateEditParams" CssClass="btn btn-primary btn-xs" runat="server" OnClientClick="CloseEditNodeDialog();">
                                                                    Cập nhật
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="lnkCancelEditParams" CssClass="btn btn-danger btn-xs" runat="server" OnClientClick="CloseEditNodeDialog(); return false;">
                                                                    Hủy thao tác
                                    </asp:LinkButton>
                                </div>
                            </div>
                            <div class="card-body">
                                    <h5 class="card-title">Thông tin Menu *</h5>
                                    <div class="">
                                        <div class="position-relative form-group">
                                            <label for="exampleAddress" class="">Link tới trang</label>
                                            <asp:DropDownList CssClass="input-field" ID="drdLink" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" AutoPostBack="true" Width="100%" />
                                        </div>
                                        <div class="position-relative form-group">
                                            <label for="chkLinkPopup" class="">Cửa sổ mới </label>
                                            <input type="checkbox" class="flat" id="chkLinkPopup" runat="server">
                                        </div>
                                        <div class="position-relative form-group">
                                            <label for="exampleAddress" class=""> Đường dẫn *</label>
                                            <input class="form-control" type="text" id="txtUrl" runat="server">
                                        </div>
                                        <div class="position-relative form-group">
                                            <label for="exampleAddress" class="">Tiêu đề *</label>
                                            <input type="text" class="form-control" placeholder="Tiêu đề menu" id="txtNodeName" runat="server" enableviewstate="true">
                                        </div>
                                        <div class="position-relative form-group">
                                            <label for="chkUnderline" class="">Gạch chân </label>
                                            <input type="checkbox" class="flat" id="chkUnderline" runat="server">
                                        </div>
                                        <div class="position-relative form-group">
                                            <label for="chkUnderline" class="">Mô tả</label>
                                            <input type="text" class="form-control" placeholder="Mãu màu" id="txtBackground" runat="server">
                                        </div>
                                        <div class="position-relative form-group" style="display:none;">
                                            <label for="chkUnderline" class="">Tham biến </label>
                                            <input type="text" class="form-control" placeholder="tham biến" id="txtParam" runat="server">
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkApplyToAllChildren" Text="Áp dụng cho tất cả các con?" runat="server" resourcekey="chkApplyToAllChildren" />
                                                            </label>
                                                        </div>
                                        </div>
                                    </div>
                                    <h5 class="card-title">Phân quyền sử dụng Menu:</h5>
                                    <div class="">
                                        <div class="position-relative form-group">
                                            <label for="exampleAddress" class="">Tất cả:</label>
                                            <asp:CheckBox ID="chkGrantAll" runat="server" AutoPostBack="true" />
                                        </div>
                                        <div class="position-relative form-group" id="trGroups" runat="server">
                                            <label for="exampleAddress" class="">Chọn nhóm quyền:</label>
                                            <asp:DropDownList ID="cboRoleGroups" runat="server" AutoPostBack="True" CssClass="form-control" />
                                        </div>
                                        <div class="position-relative form-group">
                                            <div class="form-group" id="trRoleGrids" runat="server">
                                                    <label for="exampleAddress" class="">Danh sách quyền: </label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <telerik:RadGrid ID="RadGrid1" runat="server"  OnNeedDataSource="RadGrid1_NeedDataSource" ShowHeader="False">
                                                            <mastertableview datakeynames="RoleID">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn UniqueName="RoleID" DataField="RoleID" Visible="false"/>
                                                                    <telerik:GridBoundColumn UniqueName="RoleName" DataField="RoleName" HeaderStyle-Width="70%" ItemStyle-Width="70%" ItemStyle-HorizontalAlign="Left" />
                                                                    <telerik:GridTemplateColumn Groupable="False" UniqueName="TemplateColumn">
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkChooseThisPermission" ToolTip='<%#DataBinder.Eval(Container.DataItem, "RoleID")%>' Checked='<%#DataBinder.Eval(Container.DataItem, "Allow") %>' OnCheckedChanged="chkChooseThisPermission_CheckedChanged" AutoPostBack="true" runat="server" />
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </mastertableview>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>
                                        </div>
                                        <div class="position-relative form-group" id="trUser" runat="server">
                                            <label for="exampleAddress" class="">Chọn người dùng:</label>
                                            <input class="form-control" type="text" id="txtUser" runat="server"> 
                                                        <asp:Label ID="lblErrMsg" runat="server" CssClass="Red"></asp:Label>  
                                                        <asp:LinkButton ID="cmdAddUser" CssClass="btn btn-warning btn-xs" runat="server" Text="Thêm người dùng"></asp:LinkButton> 
                                        </div>
                                        <div class="position-relative form-group">
                                            <div class="form-group" id="trUserGrid" runat="server">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Danh sách: </label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <telerik:RadGrid ID="dgUserPermissions" runat="server"  OnNeedDataSource="dgUserPermissions_NeedDataSource" ShowHeader="False">
                                                            <mastertableview datakeynames="UserId">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn UniqueName="DisplayName" DataField="DisplayName"  HeaderStyle-Width="70%" ItemStyle-Width="70%" ItemStyle-HorizontalAlign="Left" />
                                                                    <telerik:GridBoundColumn UniqueName="UserId" DataField="UserId" Visible="false"/>
                                                                    <telerik:GridTemplateColumn Groupable="False" UniqueName="TemplateColumn" ItemStyle-HorizontalAlign="Right" >
                                                                        <ItemTemplate>
                                                                            <div class="DCommandButtonMenuNoBG">
                                                                                <asp:LinkButton ID="lnkRemoveUser" runat="server" CssClass="CommandButtonMenu" CommandName="RemoveUser" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "userid") %>'>
                                                                                        <asp:Label ID="Label7" runat="server" Text="Xóa" CssClass="CsslblRemoveUser" resourcekey="RemoveUser"></asp:Label>
                                                                                </asp:LinkButton>
                                                                            </div>                                          
                                                                        </ItemTemplate>
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </mastertableview>
                                                        </telerik:RadGrid>
                                                    </div>
                                                </div>
                                        </div>
                                    </div>
                                </div>
                            <!-- Div hiển thị sửa Params cho Menu trong file .xml -->
                            
                            
                            
                            <!-- End Div hiển thị sửa Params cho Menu-->
                        </telerik:RadSlidingPane>
                    </telerik:RadSlidingZone>                            
                </telerik:radpane>
            </telerik:radsplitter>
        </div>


<%--    </ContentTemplate>
</asp:UpdatePanel>--%>
<script type="text/javascript">
    function onClientContextMenuShowing(sender, args) {
        var treeNode = args.get_node();
        treeNode.set_selected(true);
        //enable/disable menu items
        setMenuItemsState(args.get_menu().get_items(), treeNode);
    }

    function onClientContextMenuItemClicking(sender, args) {
        var menuItem = args.get_menuItem();
        var treeNode = args.get_node();
        menuItem.get_menu().hide();

        switch (menuItem.get_value()) {
            case "Delete":
                var result = confirm("Bạn có chắc chắn muốn xóa nút xử lý: " + treeNode.get_text());
                args.set_cancel(!result);
                break;
        }
    }

    //this method disables the appropriate context menu items
    function setMenuItemsState(menuItems, treeNode) {
        for (var i = 0; i < menuItems.get_count(); i++) {
            var menuItem = menuItems.getItem(i);
            switch (menuItem.get_value()) {
                case "Delete":
                    formatMenuItem(menuItem, treeNode, 'Xóa "{0}"');
                    break;
            }
        }
    }

    //formats the Text of the menu item
    function formatMenuItem(menuItem, treeNode, formatString) {
        var nodeValue = treeNode.get_value();
        var newText = String.format(formatString, extractTitleWithoutMails(treeNode));
        menuItem.set_text(newText);
    }

    //removes the brackets with the numbers,e.g. Inbox (30)
    function extractTitleWithoutMails(treeNode) {
        return treeNode.get_text().replace(/\s*\([\d]+\)\s*/ig, "");
    }
</script>


