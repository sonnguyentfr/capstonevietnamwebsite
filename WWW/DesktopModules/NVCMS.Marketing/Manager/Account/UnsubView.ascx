<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="UnsubView.ascx.vb" Inherits="NVCMS.Modules.Marketing.Unsublist" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<style type="text/css">
    .location {
        padding: 5px;
    }

        .location .project {
        }

            .location .project .project-head {
            }

                .location .project .project-head .user-avatar {
                    background: none;
                    border-radius: 0px;
                    width: unset;
                }

                    .location .project .project-head .user-avatar img {
                        border-radius: 0px;
                        max-height: 40px;
                    }

    .toggle-slide-right.content-active {
        transform: unset;
    }

    .toggle-slide.content-active {
        position: relative;
    }

    .dataTables_filter {
        display: none !important
    }
</style>
<div class="nk-block nk-block-lg">
    <asp:UpdatePanel ID="UpTrinhDo" runat="server">
        <ContentTemplate>
            <div class="nk-block-head nk-block-head-lg">
                <div class="nk-block-head nk-block-head-sm">
                    <div class="nk-block-between">
                        <div class="nk-block-head-content">
                            <h3 class="nk-block-title page-title">Danh sách Mail Unsubscribe</h3>
                            <div class="nk-block-des text-soft">
                                <p>
                                    Tổng số có
                                    <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                                    bản ghi..
                                </p>
                            </div>
                        </div>
                    </div>
                    <!-- .nk-block-between -->
                </div>
                <div class="card card-preview">
                    <div class="card-inner">
                        <table class="datatable-trinhdo nk-tb-list nk-tb-ulist" data-auto-responsive="true" id="tabledatadanhmuc">
                            <thead>
                                <tr class="nk-tb-item nk-tb-head">
                                    <th class="nk-tb-col select-filter"><span class="sub-text">Email</span></th>
                                    <th class="nk-tb-col select-filter"><span class="sub-text">Lý do</span></th>
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">Ngày tạo</span></th>
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">#</span></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="drgDataViewer" runat="server">
                                    <ItemTemplate>
                                        <tr class="nk-tb-item">
                                            <td class="nk-tb-col">
                                                <div class="user-card">
                                                    <div class="user-info">
                                                        <strong><%#Eval("email") %></strong>
                                                    </div>
                                                </div>
                                            </td>
                                            <td class="nk-tb-col tb-col-md">
                                                <%#Eval("reasonname") %>
                                            </td>
                                            <td class="nk-tb-col tb-col-md">
                                                <%#BL.FormatDate(Eval("created_date")) %>
                                            </td>
                                            <td class="nk-tb-col tb-col-md">
                                                <asp:LinkButton ID="btnDelete" CommandArgument='<%#Eval("Id") %>' CommandName="btnDelete" OnClick="btnDelete" OnClientClick="javascript: return confirm('Bạn có muốn xoá email này không?');" ToolTip="Xoá Email" runat="server">
                                                    <em class="icon ni ni-cross-sm"></em>
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        datatableSearch("#tabledatadanhmuc");
    });
</script>
