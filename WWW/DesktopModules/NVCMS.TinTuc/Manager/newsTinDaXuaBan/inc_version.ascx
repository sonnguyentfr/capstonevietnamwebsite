<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="inc_version.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.newsapprove.NewsApprove_inc_version" %>
<script src="/static/_admin/js/diff_match_patch.js"></script>
<style type="text/css">
    ul.timeline-list {
        width: 100%;
        overflow: scroll hidden;
    }

        ul.timeline-list::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            background-color: #F5F5F5;
        }

        ul.timeline-list::-webkit-scrollbar {
            width: 6px;
            height: 6px;
            background-color: #F5F5F5;
        }

        ul.timeline-list::-webkit-scrollbar-thumb {
            background-color: #000000;
        }

    .timeline-item {
        display: table-cell;padding-bottom: 0.5rem;
    }

        .timeline-item a {
            position: relative;
            top: -13px;
        }

    .timeline-date {
        width: 150px;
        font-size: 13px;
    }

        .timeline-date .icon {
            vertical-align: middle;
            color: #8094ae;
            display: inline-block;
            position: relative;
            margin-right: 0px;
            right: auto;
            top: -1px;
        }

    .timeline-item.active a {
        color: red;
        font-weight: 600;
    }
</style>
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title">Lịch sửa chỉnh sửa bài viết:
                <asp:Label ID="lbTitle" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
            </h3>
            <div class="nk-block-des text-soft">
                <p>
                    Tổng số có: 
                        <asp:Label ID="lbTotalNewsCount" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                    bản ghi
                </p>
            </div>
        </div>
    </div>
    <!-- .nk-block-between -->
</div>
<script type="text/javascript">
    var dmp = new diff_match_patch();
    function launch(text1, text2) {
        //var text1 = document.getElementById('text1').value;
        //var text2 = document.getElementById('text2').value;
        dmp.Diff_Timeout = parseFloat(document.getElementById('timeout').value);
        dmp.Diff_EditCost = parseFloat(document.getElementById('editcost').value);

        var ms_start = (new Date()).getTime();
        var d = dmp.diff_main(text1, text2);
        var ms_end = (new Date()).getTime();

        if (document.getElementById('semantic').checked) {
            dmp.diff_cleanupSemantic(d);
        }
        if (document.getElementById('efficiency').checked) {
            dmp.diff_cleanupEfficiency(d);
        }
        var ds = dmp.diff_prettyHtml(d);
        document.getElementById('outputdiv').innerHTML = ds + '<BR>Time: ' + (ms_end - ms_start) / 1000 + 's';
    }
</script>
<div class="nk-block">
    <div class="card card-bordered card-stretch">
        <div class="card-inner-group">
            <div class="card-inner">
                <div class="row gy-4">
                    <div class="col-sm-12">
                        <div class="timeline">
                            <h6 class="timeline-head">Lịch sử sửa bài:</h6>
                            <ul class="timeline-list data-simplebar">
                                <asp:Repeater ID="rptListHistory" runat="server">
                                    <ItemTemplate>
                                        <li class="timeline-item <%#GetSelect(verid, Eval("Id")) %>">
                                            <div class="timeline-status bg-primary"></div>
                                            <div class="timeline-date">
                                                <a href="/quan-tri/quan-tri-tin-tuc-cap-cao/bai-da-xuat-ban?view=version&itemid=<%#Eval("NewId") %>&verid=<%#Eval("Id") %>">
                                                    <em class="icon ni ni-alarm-alt"></em><%#BL.FormatDate(Eval("CreateDate")) %>
                                                    <br />
                                                    <em class="icon ni ni-users-fill"></em><%#BL.GetButDanh(PortalId, Eval("CreatedUser")) %>
                                                    <br />
                                                    # <%#Eval("Id") %></a>
                                            </div>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                    <div class="col-sm-12">
                        <div class="row gy-4">
                            <div class="col-sm-6">
                                <h4>Bản đang xem <asp:Literal ID="ltridId" runat="server"></asp:Literal></h4>
                                <asp:Literal ID="ltrbanHientai" runat="server"></asp:Literal>
                            </div>
                            <div class="col-sm-6">
                                <h4>Bản cũ <asp:Literal ID="ltridId2" runat="server"></asp:Literal></h4>
                                <asp:Literal ID="ltrbanTruocDo" runat="server"></asp:Literal>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- .card-inner-group -->
    </div>
    <!-- .card -->
</div>
<asp:HiddenField ID="hdfNewId" runat="server" />

