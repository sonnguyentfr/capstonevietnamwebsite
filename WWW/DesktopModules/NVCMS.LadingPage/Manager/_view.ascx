<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_view.ascx.vb" Inherits="NVCMS.Modules.LadingPage.inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<style type="text/css">
    .table-ulogs tr td {
        font-size: 12px;
    }

        .table-ulogs tr td.cautraloitd {
            width: 60%
        }

            .table-ulogs tr td.cautraloitd .cautraloi {
                overflow: hidden !important;
                display: -webkit-box !important;
                -webkit-line-clamp: 3 !important;
                -webkit-box-orient: vertical;
            }

    .sub4 {
    }

        .sub4 ul {margin-left:10px;padding-left:10px;
        }

            .sub4 ul li {
                list-style-type: disc;
                padding-bottom: 5px;
            }
</style>

<div class="nk-block nk-block-lg">
    <div class="nk-block-head">
        <div class="nk-block-head-content">
            <h4 class="nk-block-title"><%=PortalSettings.ActiveTab.Title %></h4>
            <div class="nk-block-des">
                <asp:LinkButton ID="lbtAddTop" runat="server" CssClass="btn btn-primary">Thêm mới</asp:LinkButton>
            </div>
            <div class="nk-block-des text-soft">
                <p>
                    Tổng số có: 
                        <asp:Label ID="lbTotalNewsFind" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                    bản ghi.
                </p>
            </div>
        </div>
    </div>
    <div class="nk-block">
        <div class="row g-gs">
            <asp:Repeater ID="drgDataViewer" runat="server" OnItemDataBound="OndrgDataViewer">
                <ItemTemplate>
                    <asp:HiddenField ID="hdfid" runat="server" Value='<%#Eval("id") %>' />
                    <div class="col-sm-6 col-lg-6 col-xxl-6">
                        <div class="card card-bordered h-100">
                            <div class="card-inner">
                                <div class="project">
                                    <div class="project-head">
                                        <asp:HyperLink ID="hplEditTop" CssClass="project-title" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                            <div class="user-avatar sq bg-purple"><span>G</span></div>
                                            <div class="project-info">
                                                <h6 class="title"><%#Eval("TrangDanhMuc") %></h6>
                                                <span class="sub-text"></span>
                                            </div>
                                        </asp:HyperLink>
                                        <div class="drodown">
                                            <a href="#" class="dropdown-toggle btn btn-sm btn-icon btn-trigger mt-n1 mr-n1" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                            <div class="dropdown-menu dropdown-menu-right">
                                                <ul class="link-list-opt no-bdr">
                                                    <li>
                                                        <asp:LinkButton ID="cmdquickview" OnClick="cmdquickview" CommandName="cmdquickview" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Lịch sử bài viết" runat="server">
                                                <em class="icon ni ni-eye"></em><span>Xem nhanh</span>
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:HyperLink ID="hplEdit" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                                        <em class="icon ni ni-edit"></em><span>Sửa</span>
                                                        </asp:HyperLink>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row g-gs">
                                    <asp:Repeater ID="rpttranLadingPagesub" runat="server" OnItemDataBound="OnrpttranLadingPagesub">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdfid2" runat="server" Value='<%#Eval("id") %>' />
                                            <div class="col-sm-12 col-lg-12 col-xxl-12">
                                                <div class="card h-100">
                                                    <div class="card-inner">
                                                        <div class="project">
                                                            <div class="project-head">
                                                                <asp:HyperLink ID="HyperLink1" CssClass="project-title" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                        <div class="user-avatar sq bg-purple"><span>G</span></div>
                                                        <div class="project-info">
                                                            <h6 class="title"><%#Eval("TrangDanhMuc") %></h6>
                                                            <span class="sub-text"></span>
                                                        </div>
                                                                </asp:HyperLink>
                                                                <div class="drodown">
                                                                    <a href="#" class="dropdown-toggle btn btn-sm btn-icon btn-trigger mt-n1 mr-n1" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                                    <div class="dropdown-menu dropdown-menu-right">
                                                                        <ul class="link-list-opt no-bdr">
                                                                            <li>
                                                                                <asp:LinkButton ID="LinkButton1" OnClick="cmdquickview" CommandName="cmdquickview" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Lịch sử bài viết" runat="server">
                                                            <em class="icon ni ni-eye"></em><span>Xem nhanh</span>
                                                                                </asp:LinkButton>
                                                                            </li>
                                                                            <li>
                                                                                <asp:HyperLink ID="HyperLink2" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                                                    <em class="icon ni ni-edit"></em><span>Sửa</span>
                                                                                </asp:HyperLink>
                                                                            </li>
                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row g-gs">
                                                            <asp:Repeater ID="rpttranLadingPagesub2" runat="server" OnItemDataBound="OnrpttranLadingPagesub2">
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdfid3" runat="server" Value='<%#Eval("id") %>' />
                                                                    <div class="col-sm-12 col-lg-12 col-xxl-12">
                                                                        <div class="card card-bordered h-100">
                                                                            <div class="card-inner">
                                                                                <div class="project">
                                                                                    <div class="project-head">
                                                                                        <asp:HyperLink ID="HyperLink1" CssClass="project-title" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                                                    <div class="user-avatar sq bg-purple"><span>G</span></div>
                                                                                    <div class="project-info">
                                                                                        <h6 class="title"><%#Eval("TrangDanhMuc") %></h6>
                                                                                        <span class="sub-text"></span>
                                                                                    </div>
                                                                                        </asp:HyperLink>
                                                                                        <div class="drodown">
                                                                                            <a href="#" class="dropdown-toggle btn btn-sm btn-icon btn-trigger mt-n1 mr-n1" data-toggle="dropdown"><em class="icon ni ni-more-h"></em></a>
                                                                                            <div class="dropdown-menu dropdown-menu-right">
                                                                                                <ul class="link-list-opt no-bdr">
                                                                                                    <li>
                                                                                                        <asp:LinkButton ID="LinkButton1" OnClick="cmdquickview" CommandName="cmdquickview" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Lịch sử bài viết" runat="server">
                                                                                        <em class="icon ni ni-eye"></em><span>Xem nhanh</span>
                                                                                                        </asp:LinkButton>
                                                                                                    </li>
                                                                                                    <li>
                                                                                                        <asp:HyperLink ID="HyperLink2" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server">
                                                                                                                <em class="icon ni ni-edit"></em><span>Sửa</span>
                                                                                                        </asp:HyperLink>
                                                                                                    </li>
                                                                                                </ul>
                                                                                            </div>
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
                                                                                <%--Level 4--%>
                                                                                <div class="sub4">
                                                                                    <ul>
                                                                                        <asp:Repeater ID="rpttranLadingPagesub3" runat="server">
                                                                                            <ItemTemplate>
                                                                                                <li>
                                                                                                    <h6 class="title">
                                                                                                        <asp:HyperLink ID="HyperLink2" NavigateUrl='<%#NavigateURL() & "?view=edit&itemid=" & DataBinder.Eval(Container.DataItem, "id") %>' runat="server"><%#Eval("TrangDanhMuc") %>
                                                                                                        </asp:HyperLink></h6>
                                                                                                </li>
                                                                                            </ItemTemplate>

                                                                                        </asp:Repeater>
                                                                                    </ul>
                                                                                </div>
                                                                            </div>

                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>


        </div>
    </div>
    <!-- .card-preview -->
</div>
<asp:LinkButton ID="lbtAddBottom" runat="server" CssClass="btn btn-primary" Font-Bold="True">Thêm mới</asp:LinkButton>
<!-- /.box-body -->
<%--Đoạn nay xem nhanh--%>
<div class="modal fade" tabindex="-1" id="modal-history">
    <div class="modal-dialog modal-xl modal-dialog-top" role="document">
        <div class="modal-content">
            <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                <em class="icon ni ni-cross"></em>
            </a>
            <div class="modal-header">
                <h5 class="modal-title">Thông tin chi tiết</h5>
            </div>
            <div class="modal-body">
                <asp:Literal ID="ltrcautraloi" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</div>
<%--===================================================--%>