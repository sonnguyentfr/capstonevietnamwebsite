<%@ Control Language="vb" Inherits="NVCMS.Modules.TinTuc.NhuanButView" ClientIDMode="Static" CodeFile="ListNhuanButEdit.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%--<%@ Register Src="~/DesktopModules/TNReport/AdminForm.ascx" TagName="AdminForm" TagPrefix="pu" %>--%>
<script type="text/javascript" src="/static/_Admin/build/js/autoNumeric.js"></script>
<script src="/static/_Admin/build/js/newsadmin.js"></script>
<style type="text/css">
    .form-control.currency {
        font-size: 12px;
        color: red;
        height: 28px;
    }

    .table {
        font-size: 12px;
    }
    .khongduview {background: #f5f2a8;
    color: #000;}
    .khongduview a {color:#000;}
    #nhuanbut tr td input:disabled {background: #e0e0e0;
    border: solid 1px #fff;}
</style>
<div class="nk-content-body">
    <div class="nk-block-head nk-block-head-sm">
        <div class="nk-block-between">
            <div class="nk-block-head-content">
                <h3 class="nk-block-title page-title">Sửa nhuận bút</h3>
            </div>
            <!-- .nk-block-head-content -->
            <div class="nk-block-head-content">
                <div class="toggle-wrap nk-block-tools-toggle">
                    <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-more-v"></em></a>
                    <div class="toggle-expand-content" data-content="pageMenu">
                        <ul class="nk-block-tools g-3">
                            <li>
                                <asp:LinkButton ID="lbtEdit" runat="server" Font-Bold="true" CssClass="btn btn-info" ToolTip="Tìm kiếm"><em class="icon ni ni-curve-up-left"></em> Quay lại</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="lbtViewThongke" runat="server" Font-Bold="true" CssClass="btn btn-danger" ToolTip="Tìm kiếm">Thống kê</asp:LinkButton>
                            </li>
                            <li class="nk-block-tools-opt">
                                <a href="#" data-target="addProduct" class="toggle btn btn-icon btn-primary d-md-none"><em class="icon ni ni-plus"></em></a>
                                <a href="#" data-target="addProduct" class="toggle btn btn-primary d-none d-md-inline-flex"><em class="icon ni ni-plus"></em><span>Lọc thông tin</span></a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
            <!-- .nk-block-head-content -->
        </div>
        <!-- .nk-block-between -->
    </div>
    <!-- .nk-block-head -->
    <asp:UpdatePanel runat="server" ID="upnlAttxuatban">
        <ContentTemplate>
            <div class="card card-preview">
                <div class="card-inner">
                    <!-- /.box-header -->
                    <div class="box-header bggray">
                        <div class="col-md-8 col-xs-8 pdf0">
                            Tổng số có: 
                                <asp:Label ID="lbTotalNewsCount" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                            bản ghi
                        </div>
                        <div class="col-md-3 col-xs-4 ">
                            <asp:LinkButton ID="lbtupdatenhuantbut" runat="server" Font-Bold="true" CssClass="btn btn-danger" ToolTip="Tìm kiếm">Cập nhật lại nhuật bút</asp:LinkButton>
                        </div>
                        <div class="col-md-12 col-xs-12 pdf0">
                            <asp:Label runat="server" ID="lblMessage" CssClass="help-lock"></asp:Label>
                        </div>
                    </div>

                    <table class="table table-hover" id="nhuanbut">
                        <thead class="thead-dark thead-light">
                            <tr>
                                <th colspan="1" rowspan="2" style="width: 20px; text-align: center;" scope="col">STT</th>
                                <th colspan="1" rowspan="2" style="width: 300px; text-align: center;" scope="col">B&agrave;i viết</th>
                                <th colspan="1" rowspan="2" style="width: 30px; text-align: center;" scope="col">PR</th>
                                <th colspan="1" rowspan="2" style="width: 50px; text-align: center;" scope="col">Loại</th>
                                <th colspan="6" rowspan="1" style="text-align: center;" scope="col">TH&Ocirc;NG TIN NHUẬN B&Uacute;T</th>
                                <th colspan="2" rowspan="1" style="text-align: center;" scope="col">NGƯỜI TẠO - T&Aacute;C GIẢ</th>
                            </tr>
                            <tr>
                                <th>Chuy&ecirc;n mục</th>
                                <th>Ng&agrave;y xuất bản</th>
                                <th style="width: 30px; text-align: center;" scope="col">Gi&aacute; trị Tin(4)</th>
                                <th style="width: 30px; text-align: center;" scope="col">Gi&aacute; trị B&agrave;i(1)</th>
                                <th style="width: 30px; text-align: center;" scope="col">Gi&aacute; trị Ảnh(2)</th>
                                <th style="width: 30px; text-align: center;" scope="col">Gi&aacute; trị Video(3)</th>
                                <th>Người tạo</th>
                                <th>Bút danh</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptData" runat="server">
                                <ItemTemplate>
                                    <tr id="tr" runat="server" class='<%#BaiItViet(Eval("ViewCount"))%>'>
                                        <td style="text-align: center;"><%# Container.ItemIndex + 1%><br />
                                            <asp:Label ID="lblnewid" runat="server" Text='<%#Eval("id") %>' /></td>
                                        <td>
                                            <a target="_blank" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                                <%#Eval("Title")%> (<font style="color:red;font-weight:600;"><em class="icon ni ni-eye"></em><%#Eval("ViewCount")%>)
                                            </a></td>
                                        <td style="width: 30px;text-align:center"><%#IIf(CBool(DataBinder.Eval(Container.DataItem, "isPR")) = True, "V", "") %></td>
                                        <td style="width: 50px; text-align:center"><%# BL.FormatLoaiTinBaiHTML(Cint(Eval("NewsKind")))%></td>
                                        <td style="width: 100px;"><%#Eval("CategoryName")%></td>
                                        <td style="width: 100px; text-align: center;" class=""><%#BL.FormatDate(CDate(Eval("PublishedDate")))%></td>
                                        <td style="width: 100px; text-align: center;">
                                            <asp:TextBox CssClass="form-control" ID="txtgiatritin" runat="server" Text='<%#TienTin(CInt(Eval("id")), 4)%>' Enabled='<%#LoaiType(CInt(Eval("Type")), 4)%>'></asp:TextBox>
                                            
                                        </td>
                                        <td style="width: 100px; text-align: center;">
                                            <asp:TextBox CssClass="form-control" ID="txtgiatribai" runat="server" Text='<%#TienTin(CInt(Eval("id")), 1)%>' Enabled='<%#LoaiType(CInt(Eval("Type")), 1)%>'></asp:TextBox>
                                            
                                        </td>
                                        <td style="width: 100px; text-align: center;">
                                            <asp:TextBox CssClass="form-control" ID="txtgiatritinanh" runat="server" Text='<%#TienTin(CInt(Eval("id")), 2)%>' Enabled='<%#LoaiType(CInt(Eval("Type")), 2)%>'></asp:TextBox>
                                            
                                        </td>
                                        <td style="width: 100px; text-align: center;">
                                            <asp:TextBox CssClass="form-control" ID="txtgiatrivideo" runat="server" Text='<%#TienTin(CInt(Eval("id")), 3)%>' Enabled='<%#LoaiType(CInt(Eval("Type")), 3)%>'></asp:TextBox>
                                            
                                        </td>
                                        <td style="width: 50px; text-align: center;"><%# BL.GetNameByUserId(PortalId, CInt(Eval("CreateUser")))%></td>
                                        <td style="width: 50px; text-align: center;"><%# Eval("ButDanh")%>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                        </tbody>
                    </table>
                    <div class="box-header bggray">
                        <div class="col-md-10 col-xs-12">
                            <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" PageLinksPerPage="20" />
                        </div>
                        <div class="col-md-2 col-xs-12">
                        </div>
                    </div>
                    <!-- /.box-body -->
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
    <!-- .nk-block -->
    <div class="nk-add-product toggle-slide toggle-slide-right" data-content="addProduct" data-toggle-screen="any" data-toggle-overlay="true" data-toggle-body="true" data-simplebar>
        <div class="nk-block-head">
            <div class="nk-block-head-content">
                <h5 class="nk-block-title">Tác giả</h5>
                <div class="nk-block-des">
                    <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control select2"></asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="nk-block-head">
            <div class="nk-block-head-content">
                <h5 class="nk-block-title">Loại nhuận bút</h5>
                <div class="nk-block-des">
                    <asp:DropDownList ID="ddlKieuNhuanBut" runat="server" CssClass="form-control select2">
                        <asp:ListItem Value="0" Text="Tất cả"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Tin Bài"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Video Clips"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <!-- .nk-block-head -->
        <div class="nk-block">
            <div class="row g-3">
                <div class="col-12">
                    <div class="form-group">
                        <label class="form-label" for="product-title">Ngày xuất bản</label>
                    </div>
                </div>
                <div class="col-mb-6">
                    <div class="form-group">
                        <label class="form-label" for="regular-price">Từ</label>
                        <div class="form-control-wrap">
                            <input type="text" id="txtStartDate" runat="server" class="form-control pull-right datepicker">
                        </div>
                    </div>
                </div>
                <div class="col-mb-6">
                    <div class="form-group">
                        <label class="form-label" for="sale-price">đến</label>
                        <div class="form-control-wrap">
                            <input type="text" id="txtEndDate" runat="server" class="form-control pull-right datepicker">
                        </div>
                    </div>
                </div>
                <div class="col-12">
                    <asp:LinkButton ID="lbtView" runat="server" Font-Bold="true" CssClass="btn btn-danger btn-lg" ToolTip="Tìm kiếm" OnClientClick="return checkvalidate();"><i class="fa fa-search"></i> Xem</asp:LinkButton>

                </div>
            </div>
        </div>
        <!-- .nk-block -->
    </div>
</div>

