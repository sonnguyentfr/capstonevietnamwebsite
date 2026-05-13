<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="share.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.news.newsedit" EnableViewState="true" %>
<meta name="viewport" content="width=device-width, initial-scale=1" />
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script src="/Providers/HtmlEditorProviders/DNNConnect.CKE/js/ckeditor/4.12.1/ckeditor.js"></script>
<script src="/Providers/HtmlEditorProviders/DNNConnect.CKE/js/ckeditor/4.12.1/config.js?v=1"></script>
<script src="/static/_Admin/build/js/newsadmin.js"></script>
<script type="text/javascript" src="/static/_Admin/build/js/autoNumeric.js"></script>
<script type="text/javascript" src="/static/_Admin/build/js/base64.js"></script>
<script type="text/javascript" src="/static/_Admin/build/js/jquery.cookie.js"></script>
<script src="/static/_Admin/vendors/autocomplate/jquery.fcbkcomplete.js" type="text/javascript"></script>
<link href="/static/_Admin/vendors/autocomplate/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    jQuery(function ($) {
        $('.auto').autoNumeric('init', { dGroup: '3', aSep: '.', aDec: ',', aSign: '₫ ', vMin: '0', vMax: '1000000', wEmpty: 'zero', wEmpty: 'sign' });
    });
</script>
<asp:UpdatePanel ID="upnewsbyshare" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="x_panel">
                    <div class="x_title">
                        <h2>Bài viết: <asp:Literal ID="title" runat="server"></asp:Literal> </h2>
                        <ul class="nav navbar-right panel_toolbox">
                            <li><a class="collapse-link"><i class="fa fa-chevron-up"></i></a>
                            </li>
                        </ul>
                        <div class="clearfix"></div>
                    </div>
                    <div class="">
                        <h4>DANH SÁCH CÁC LINK ĐÃ SHARE</h4><asp:LinkButton ID="lbnThoat" runat="server" CssClass="btn btn-danger btn-sm"><i class="fa fa-gg"></i> Thoát</asp:LinkButton>
                        <table class="table table-striped jambo_table bulk_action table-bordered">
                            <thead class="headings">
                                <tr>
                                    <th>#</th>
                                    <th>Link Share</th>
                                    <th style="width: 80px; text-align: center;">Ngày tạo</th>
                                    <th style="width: 80px; text-align: center;">Count</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="drgDataViewer" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td style="width: 80px;"></td>
                                            <td>
                                        <asp:HyperLink ID="hplTitle" runat="server" Font-Underline="false" Font-Bold="true" Font-Size="15px" Target="_blank"
                                            NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "LinkShare") %>'>
                                                <%# Eval("LinkShare")%>
                                        </asp:HyperLink>
                                                <asp:HyperLink ID="HyperLink1" CssClass="btn btn-info btn-xs" data-toggle="tooltip" data-placement="top" data-original-title="Xem trước bài viết" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "LinkShare") %>' Target="_blank" runat="server">
                                        <i class="fa fa-eye"></i> Xem
                                                </asp:HyperLink>
                                            </td>
                                            <td style="width: 120px; text-align: center;">
                                                <%# BL.FormatDate(Eval("CreatedDate"))%>
                                            </td>
                                            <td style="width: 80px; text-align: center;">
                                                <%# Eval("Count")%>
                                            </td>

                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>

                        </table>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="PageUpdateProgress">
    <ProgressTemplate>
        <div id="loading">
            <div class="loading">
                <div></div>
                <div></div>
            </div>
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>




