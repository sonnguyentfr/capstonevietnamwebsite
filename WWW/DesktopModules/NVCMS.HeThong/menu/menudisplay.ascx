<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="menudisplay.ascx.vb" Inherits="NVCMS.Modules.HeThong.menudisplay" %>
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

<asp:UpdatePanel ID="updatemenu" runat="server">

    <ContentTemplate>
        <div class="row">
            <div class="col-md-12">
                <div class="x_panel">
                    <div class="x_content">
                        <div class="row">
                            <div class="col-sm-12 form-horizontal">
                                <label class="control-label col-md-4 col-sm-4 col-xs-12">Chọn menu:</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="drlMenu" runat="server" DataTextField="Text" DataValueField="Value" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-sm-6">
                                    <telerik:radtoolbar id="tbNavigator" runat="server" skin="Office2010Blue" autopostback="true" enableroundedcorners="true" enableshadows="true"
                                        usefadeeffect="True">
                        <items>
                            <telerik:RadToolBarButton id="button_save" AccessKey="s"  Tooltip="Lưu (Alt+S)" Value="save" Text=" Lưu các thay đổi" ImageUrl="/Icons/Sigma/Save_16X16_Gray.png" />
                            <telerik:RadToolBarButton IsSeparator="true" />
                            <telerik:RadToolBarButton id="button_exit" AccessKey="x" Tooltip="Thoát (Alt+X)" Value="exit" Text="Thoát" ImageUrl="/Icons/Sigma/TreeViewHide_16x16_Gray.png" />
                         </items>
                        </telerik:radtoolbar>
                                </div>


                            </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-12">
                                <telerik:radsplitter id="RadSplitter1" runat="server" width="100%" height="650px" skin="Office2010Blue">
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
                            <!-- Div hiển thị sửa Params cho Menu trong file .xml -->
                            <div class="row">
                                <div class="col-md-12 col-xs-12">
                                    <div class="x_panel">
                                        <div class="x_title">
                                            <h2>* Thông tin loại Menu:</h2>
                                            <ul class="nav navbar-right panel_toolbox">
                                                <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                                                </li>
                                            </ul>
                                            <div class="clearfix"></div>
                                        </div>
                                        <div class="x_content">
                                            <div class="form-horizontal form-label-left input_mask">
                                                <div class="form-group">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Link tới trang</label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <asp:DropDownList CssClass="form-control" ID="drdLink" runat="server" DataTextField="IndentedTabName" DataValueField="TabId" AutoPostBack="true" Width="100%" />

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Cửa sổ mới </label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <div class="checkbox">
                                                            <label>
                                                                <input type="checkbox" class="flat" id="chkLinkPopup" runat="server">
                                                                Có
                                                       
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">
                                                        Đường dẫn</span>
                                                    </label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <input class="form-control" type="text" id="txtUrl" runat="server">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Tiêu đề</label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <input type="text" class="form-control" placeholder="Tiêu đề menu" id="txtNodeName" runat="server" enableviewstate="true">
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Gạch chân </label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <div class="checkbox">
                                                            <label>
                                                                <input type="checkbox" class="flat" id="chkUnderline" runat="server">
                                                                Có
                                                       
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group disable">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Màu nền </label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <input type="text" class="form-control" placeholder="Mãu màu" id="txtBackground" runat="server">
                                                    </div>
                                                </div>
                                                <div class="form-group disable">
                                                    <label class="control-label col-md-3 col-sm-3 col-xs-12">Tham biến </label>
                                                    <div class="col-md-9 col-sm-9 col-xs-12">
                                                        <input type="text" class="form-control" placeholder="tham biến" id="txtParam" runat="server">
                                                        <div class="checkbox">
                                                            <label>
                                                                <asp:CheckBox ID="chkApplyToAllChildren" Text="Áp dụng cho tất cả các con?" runat="server" resourcekey="chkApplyToAllChildren" />
                                                            </label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <!-- End Div hiển thị sửa Params cho Menu-->
                        </telerik:RadSlidingPane>
                    </telerik:RadSlidingZone>                            
                </telerik:radpane>
            </telerik:radsplitter>
                            </div>
                            <!-- /CONTENT MAIL -->
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
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
        for (var i = 0; i < menuItems.get_count() ; i++) {
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


