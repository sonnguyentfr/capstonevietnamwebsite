<%@ Control Language="vb" AutoEventWireup="false" CodeFile="inc_list.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.adminnews.adminnews_inc_list" %>
<%@ Import Namespace="NVCMS.Modules.TinTuc" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<script type="text/javascript" src="/static/_Admin/build/js/autoNumeric.js"></script>
<script type="text/javascript">
    jQuery(function ($) {
        $('.auto').autoNumeric('init', { dGroup: '3', aSep: '.', aDec: ',', aSign: '₫ ', vMin: '0', vMax: '100000000', wEmpty: 'zero', wEmpty: 'sign' });
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
        <div class="nk-block-head nk-block-head-sm">
            <div class="nk-block-between">
                <div class="nk-block-head-content">
                    <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Title %></h3>
                    <div class="nk-block-des text-soft">
                        <p>
                            Tổng số có: 
                                <asp:Label ID="lbTotalNewsCount" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                            tin bài. | Thời gian từ: <strong><asp:Literal ID="thoigiantu" runat="server"></asp:Literal></strong> -> đến  <strong><asp:Literal ID="thoigianden" runat="server"></asp:Literal></strong>
                        </p>
                    </div>
                </div>
                <!-- .nk-block-head-content -->
            </div>
            <!-- .nk-block-between -->
        </div>
        <!-- .nk-block-head -->
        <div class="nk-block" id="tblSearch" runat="server">
            <div class="card card-bordered card-stretch">
                <div class="card-inner-group">
                    <div class="card-inner position-relative card-tools-toggle">
                        <div class="row gy-4">
                            <div class="col-lg-4 col-sm-6">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <asp:DropDownList id="ddlUserPost" runat="server" CssClass="form-select form-control form-control-xl" data-ui="xl"></asp:DropDownList>
                                        <label class="form-label-outlined" for="<%=ddlUserPost.ClientID %>">Chọn tác giả</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-sm-6">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <div class="form-icon form-icon-right">
                                            <em class="icon ni ni-calendar-alt"></em>
                                        </div>
                                        <input type="text" id="txtStartdate" runat="server" class="form-control form-control-xl form-control-outlined datepicker" autocomplete="off">
                                        <label class="form-label-outlined" for="<%=txtStartdate.ClientID %>">Từ ngày</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3 col-sm-6">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <div class="form-icon form-icon-right">
                                            <em class="icon ni ni-calendar-alt"></em>
                                        </div>
                                        <input type="text" id="txtEndDate" runat="server" class="form-control form-control-xl form-control-outlined datepicker" autocomplete="off">
                                        <label class="form-label-outlined" for="<%=txtEndDate.ClientID %>">đến ngày</label>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-2 col-sm-6">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <asp:LinkButton ID="lbtFind" runat="server" Font-Bold="true" CssClass="btn btn-primary" ToolTip="Tìm kiếm"><i class="fa fa-search"></i> Tìm kiếm</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            
                        </div>
                        <!-- .card-search -->
                    </div>
                    <!-- .card-inner -->
                    <div class="card-inner p-0">
                        <div class="nk-tb-list nk-tb-ulist">
                            <div class="nk-tb-item nk-tb-head">
                                <div class="nk-tb-col"><span class="sub-text">Tác giả</span></div>
                                <div class="nk-tb-col tb-col-mb"><span class="sub-text">TỔNG BÀI</span></div>
                                <div class="nk-tb-col tb-col-md"><span class="sub-text">Tin tức tổng hợp</span></div>
                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Bài tổng hợp</span></div>
                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Tin sản xuất</span></div>
                                <div class="nk-tb-col tb-col-md"><span class="sub-text">Bài sản xuất</span></div>
                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Bài Phản ánh</span></div>
                                <div class="nk-tb-col tb-col-md"><span class="sub-text">Bài Phỏng vấn</span></div>
                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Phóng sự điều tra</span></div>
                                <div class="nk-tb-col tb-col-md"><span class="sub-text">Bài PR</span></div>
                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Tin Dẫn Nguồn</span></div>
                                <div class="nk-tb-col tb-col-lg"><span class="sub-text">Tiền nhuận bút</span></div>
                            </div>
                            <!-- .nk-tb-item -->
                            
                            <asp:Literal ID="ltrnhuanbut" runat="server"></asp:Literal>
                            
                        </div>
                        <!-- .nk-tb-list -->
                    </div>
                    <!-- .card-inner -->
                </div>
                <!-- .card-inner-group -->
            </div>
            <!-- .card -->
        </div>
        <!-- .nk-block -->
        
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
