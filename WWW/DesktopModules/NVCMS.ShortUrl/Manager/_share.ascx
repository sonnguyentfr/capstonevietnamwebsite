<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_share.ascx.vb" Inherits="NVCMS.Modules.ShortURL.inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register Src="~/controls/Pagesadmin.ascx" TagPrefix="uc1" TagName="Pages" %>

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
</style>
<div class="nk-block nk-block-lg">
    <asp:UpdatePanel ID="UpTrinhDo" runat="server">
        <ContentTemplate>
            <div class="nk-block-head nk-block-head-lg">
                <div class="nk-block-head nk-block-head-sm">
                    <div class="nk-block-between">
                        <div class="nk-block-head-content">
                            <h3 class="nk-block-title page-title">Danh sách link truy cập đến: <strong class="color-red"><asp:Literal ID="ltrshortlink" runat="server"></asp:Literal></strong></h3>
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
                        <table class="datatable-trinhdo nk-tb-list nk-tb-ulist" data-auto-responsive="true">
                            <thead>
                                <tr class="nk-tb-item nk-tb-head">
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">Redirect</span></th>
                                    <th class="nk-tb-col tb-col-mb"><span class="sub-text">Click</span></th>
                                    
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="drgDataViewer" runat="server">
                                    <ItemTemplate>
                                        <tr class="nk-tb-item">
                                            
                                            <td class="nk-tb-col tb-col-md">
                                                <%#Eval("LinkShare") %>
                                            </td>
                                            
                                            <td class="nk-tb-col tb-col-md">
                                                <%#Eval("count") %>
                                            </td>
                                            
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                        <uc1:Pages runat="server" ID="vbPaging" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
