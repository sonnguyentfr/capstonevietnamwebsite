<%@ Control Language="vb" AutoEventWireup="false" CodeFile="viewdetail.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.adminnews_inc_list" %>
<%@ Import Namespace="NVCMS.Modules.TinTuc" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<script type="text/javascript" src="/static/_Admin/build/js/autoNumeric.js"></script>
<script type="text/javascript">
    jQuery(function ($) {
        $('.auto').autoNumeric('init', { dGroup: '3', aSep: '.', aDec: ',', aSign: '₫ ', vMin: '0', vMax: '1000000', wEmpty: 'zero', wEmpty: 'sign' });
    });
</script>
<style type="text/css">
    .table tr td a.titlte {
        font-weight: 600;
        color: #434343;
    }

    .divsearch {
        padding-top: 30px;
    }

        .divsearch .toolbar_btn {
            font-weight: bold;
            font-size: 19px;
            background: #1abb9c;
            padding: 6px 10px;
            color: #fff;
            border: solid 1px #888888;
        }
</style>
<asp:UpdatePanel runat="server" ID="upnlAtt">
    <ContentTemplate>
        <div id="tblSearch" runat="server">
            <div class="box-body  pdf0 pdr0">
                <div class="col-md-12">
                    <!-- /.col -->
                    <div class="col-md-3 col-sm-12 pdf0">
                        <div class="form-group">
                            <label>Tác giả:</label>
                            <asp:DropDownList ID="ddlUserPost" runat="server" AutoPostBack="true" CssClass="form-control select2"></asp:DropDownList>
                        </div>
                        <!-- /.form-group -->
                        <!-- /.form-group -->
                    </div>
                    <div class="col-md-5 col-sm-12 pdf0">
                        <div class="form-group">
                            <div class="col-md-6 pdf0">
                                <label>Từ ngày </label>
                                <div class="input-group date">
                                    <div class="input-group-addon">
                                        <i class="fa fa-calendar"></i>
                                    </div>
                                    <input type="text" id="txtStartdate" runat="server" class="form-control pull-right datepicker">
                                </div>
                            </div>
                            <div class="col-md-5 pdf0">
                                <label>Đến ngày</label>
                                <div class="input-group date">
                                    <div class="input-group-addon">
                                        <i class="fa fa-calendar"></i>
                                    </div>
                                    <input type="text" id="txtEndDate" runat="server" class="form-control pull-right datepicker">
                                </div>
                            </div>
                        </div>
                        <!-- /.form-group -->
                        <!-- /.form-group -->
                    </div>
                    <div class="col-md-2 divsearch">
                        <label>&nbsp;</label>
                        <asp:LinkButton ID="lbtFind" runat="server" Font-Bold="true" CssClass="toolbar_btn" ToolTip="Tìm kiếm"><i class="fa fa-search"></i> Tìm kiếm</asp:LinkButton>
                    </div>
                </div>
                <!-- /.col -->
                <!-- /.row -->
            </div>
            <!-- /.box-body -->

            <div class="col-md-12  pdf0 pdr0">
                <!-- /.box-header -->
                <div class="box-header bggray">
                    <div class="col-md-9 col-xs-8 pdf0">
                        Tổng số có: 
                                <asp:Label ID="lbTotalNewsCount" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                        tin bài
                    </div>
                </div>
                <table border="1" style="width: 100%; padding: 0px; margin: 0px;" cellpadding="0" cellspacing="0" class='table table-striped jambo_table bulk_action table-bordered'>

                    <tr>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Tác giả</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">TỔNG BÀI</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Tin tức tổng hợp</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px; width: 200px;">Bài tổng hợp</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Tin sản xuất</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Bài sản xuất</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Bài Phản ánh</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Bài Phỏng vấn</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Phóng sự điều tra</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Bài Pr</th>
                        <th style="border: solid 1px #d2d2d2; text-align: center; padding: 5px;">Tin dẫn nguồn</th>
                    </tr>
                    <tbody class="dsfdsfsdfdf">
                        <asp:Literal ID="ltrnhuanbut" runat="server"></asp:Literal>
                    </tbody>
                </table>
            </div>

        </div>        
    </ContentTemplate>
    <Triggers>
    </Triggers>
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
