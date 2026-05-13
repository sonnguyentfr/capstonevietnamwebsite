<%@ Control Language="VB" AutoEventWireup="false" CodeFile="permission.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.permission.permission" %>
<asp:UpdatePanel ID="pnlAjax" runat="server">
    <ContentTemplate>
        <div class="components-preview  mx-auto">
            <div class="nk-block-head nk-block-head-lg wide-sm">
                <div class="nk-block-head-content">
                    <h2 class="nk-block-title fw-normal"><%=PortalSettings.ActiveTab.Title %></h2>
                </div>
            </div>
            <!-- .nk-block-head -->
            <div class="nk-block">
                <div class="card card-preview">
                    <div class="card-inner">
                        <div class="row gy-4">
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="form-label">Chọn nhóm quyền</label>
                                    <div class="form-control-wrap">
                                        <asp:DropDownList ID="drlRoles" runat="server" CssClass="form-control " AutoPostBack="true" />

                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="form-label">Danh mục chưa phân quyền</label>
                                    <div class="form-control-wrap">
                                        <div class="form-control-select-multiple">
                                            <asp:ListBox ID="lstAvailable" runat="server" CssClass="custom-select" Width="100%" Height="600" DataTextField="CategoryName" DataValueField="CategoryID" SelectionMode="Multiple"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <div class="form-group">
                                    <div class="x_content nutchuyen">
                                        <asp:LinkButton ID="lbtAdd" runat="server" CssClass="nutxoaz">
                        <em class="icon ni ni-arrow-right"></em>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbtRemove" runat="server" CssClass="nutxoaz">
                        <em class="icon ni ni-arrow-left"></em>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbtAddAll" runat="server" CssClass="nutxoaz">
                        <em class="icon ni ni-arrow-to-right"></em>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="lbtRemoveAll" runat="server" CssClass="nutxoaz">
                        <em class="icon ni ni-arrow-to-left"></em>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-group">
                                    <label class="form-label" for="default-07">Danh sách tài khoản</label>
                                    <div class="form-control-wrap">
                                        <asp:DropDownList ID="radUser" runat="server" Width="250" CssClass="form-control" Filter="Contains" DataTextField="Username" DataValueField="UserId" AutoPostBack="true" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="form-label" for="default-07">Danh mục đã được phân quyền</label>
                                    <div class="form-control-wrap">
                                        <div class="form-control-select-multiple">
                                            <asp:ListBox ID="lstAssigned" runat="server" CssClass="custom-select" Width="100%" Height="600" DataTextField="CategoryName" SelectionMode="Multiple" DataValueField="CategoryID"></asp:ListBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- .code-block -->
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<style type="text/css">
    .nutchuyen {
        padding-top: 253px;
        text-align: center;
    }

        .nutchuyen .nutxoaz {
            display: block;
            /* margin: 10px 0px; */
            text-align: center;
            background: #e6e6e6;
            width: 40px;
            height: 40px;
            margin: 10px auto;
            border-radius: 50%;
            padding: 5px;
            border: solid 1px #7b7b7b;
            box-shadow: 0px 0px 3px 0px dimgrey;
        }

            .nutchuyen .nutxoaz:hover {
                background: #f5dc3c;
                border: solid 1px #2f2f2f;
                box-shadow: 0px 0px 9px 1px dimgrey;
            }

            .nutchuyen .nutxoaz em {
                font-size: 30px;
            }
</style>
