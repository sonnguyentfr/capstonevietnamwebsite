<%@ Control Language="vb" Inherits="NVCMS.Modules.TinTuc.NhuanButView" ClientIDMode="Static" CodeFile="ListNhuanBut.ascx.vb" AutoEventWireup="false" Explicit="True" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%--<%@ Register Src="~/DesktopModules/TNReport/AdminForm.ascx" TagName="AdminForm" TagPrefix="pu" %>--%>
<script type="text/javascript" src="/static/_admin/js/autoNumeric.js"></script>
<script src="/static/_admin/js/nvcmsinit.js"></script>
<style type="text/css">
    .form-control.currency {
        font-size: 12px;
        color: red;
        height: 28px;
    }

    .table {
        font-size: 12px;
    }
</style>
<div class="nk-content-body">
    <div class="nk-block-head nk-block-head-sm">
        <div class="nk-block-between">
            <div class="nk-block-head-content">
                <h3 class="nk-block-title page-title">Thống kê nhuận bút</h3>
            </div>
            <!-- .nk-block-head-content -->
            <div class="nk-block-head-content">
                <div class="toggle-wrap nk-block-tools-toggle">
                    <a href="#" class="btn btn-icon btn-trigger toggle-expand mr-n1" data-target="pageMenu"><em class="icon ni ni-more-v"></em></a>
                    <div class="toggle-expand-content" data-content="pageMenu">
                        <ul class="nk-block-tools g-3">
                            <li>
                                <asp:LinkButton ID="lbtEdit" runat="server" Font-Bold="true" CssClass="btn btn-info" ToolTip="Sửa nhuận bút"><em class="icon ni ni-edit-alt"></em> Sửa Nhuận bút</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="lbtReport" runat="server" Font-Bold="true" CssClass="btn btn-primary" ToolTip="Tìm kiếm" OnClientClick="return checkvalidate();"><em class="icon ni ni-external"></em> Xuất</asp:LinkButton>
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
                        <div class="col-md-9 col-xs-8 pdf0">
                            Tổng số có: 
                                <asp:Label ID="lbTotalNewsCount" runat="server" ForeColor="Maroon" Font-Bold="true" Text="00"></asp:Label>
                            bản ghi
                        </div>
                        <div class="col-md-1 col-xs-4 ">
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
                                <th colspan="1" rowspan="2" style="width: 100px; text-align: center;" scope="col">Loại tin bài</th>
                                <th colspan="6" rowspan="1" style="text-align: center;" scope="col">TH&Ocirc;NG TIN NHUẬN B&Uacute;T</th>
                                <th colspan="3" rowspan="1" style="text-align: center;" scope="col">NGƯỜI TẠO - T&Aacute;C GIẢ</th>
                                <th colspan="1" rowspan="2" style="width: 20px; text-align: center;" scope="col">Sửa</th>
                                <th colspan="1" rowspan="2" style="width: 20px; text-align: center;" scope="col">Xóa</th>
                            </tr>
                            <tr>
                                <th>Chuy&ecirc;n mục</th>
                                <th>Ng&agrave;y xuất bản</th>
                                <th>Gi&aacute; trị Tin</th>
                                <th>Gi&aacute; trị B&agrave;i</th>
                                <th>Gi&aacute; trị Ảnh</th>
                                <th>Gi&aacute; trị Video</th>
                                <th>Người tạo</th>
                                <th>T&aacute;c giả</th>
                                <th>Bút danh</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptData" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td style="text-align: center;"><%# Container.ItemIndex + 1%></td>
                                        <td>
                                            <a target="_blank" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                                <%#Eval("Title")%> (<em class="icon ni ni-eye"></em><%#Eval("ViewCount")%>)
                                            </a></td>
                                        <td style="width: 30px;"><%#IIf(CBool(DataBinder.Eval(Container.DataItem, "isPR")) = True, "V", "") %></td>
                                        <td style="width: 110px;"><%# BL.FormatLoaiTinBaiHTML(Cint(Eval("NewsKind")))%></td>
                                        <td style="width: 200px;"><%#Eval("CategoryName")%></td>
                                        <td style="width: 100px; text-align: center;" class=""><%#BL.FormatDate(CDate(Eval("PublishedDate")))%></td>
                                        <td style="width: 100px; text-align: center;" class="auto currency"><%#TienTin(CInt(CInt(Eval("Newid"))), 4)%>
                                        </td>
                                        <td style="width: 100px; text-align: center;"><span class="auto currency"><%#TienTin(CInt(Eval("id")), 1)%></span></td>
                                        <td style="width: 100px; text-align: center;" class="auto currency"><%#TienTin(CInt(Eval("id")), 2)%></td>
                                        <td style="width: 100px; text-align: center;" class="auto currency"><%#TienTin(CInt(Eval("id")), 3)%></td>
                                        <td style="width: 100px; text-align: center;"><%# BL.GetNameByUserId(PortalId, CInt(Eval("CreateUser")))%></td>
                                        <td style="width: 100px; text-align: center;"><%# Eval("CategoryName")%>
                                        <td style="width: 100px; text-align: center;"><%# Eval("ButDanh")%>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="cmdEdit" OnClick="cmdEdit" CommandName="cmdEdit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Sửa nhuận bút" runat="server">
                                                                        <em class="icon ni ni-calender-date"></em><span>Sửa</span>
                                            </asp:LinkButton>
                                            <asp:Label ID="lblnewid" runat="server" Text='<%# CInt(Eval("NewId")) %>' Visible="false" /></td>
                                        <td>
                                            <asp:LinkButton ID="cmdXoaNhuan" OnClick="cmdXoaNhuan" CommandName="cmdXoaNhuan" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' ToolTip="Xóa nhuận bút" runat="server">
                                                                        <em class="icon ni ni-remove"></em><span>Xóa</span>
                                            </asp:LinkButton>
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
            <div class="modal fade " id="modal-sua">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span></button>
                            <p class="modal-title">Sửa thông tin nhuận bút</p>
                        </div>
                        <div class="modal-body form-horizontal form-label-left">
                            <div class="form-group">
                                <h4>
                                    <asp:Literal ID="ltrtitle" runat="server"></asp:Literal>
                                </h4>
                            </div>
                            <div class="form-group">
                                <table class="table bordered">
                                    <tr>
                                        <td>Lượt xem:
                                        </td>
                                        <td>
                                            <asp:Literal ID="ltrview" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Ngày đăng: 
                                        </td>
                                        <td>
                                            <asp:Literal ID="ltrngaydang" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tổng tiền: 
                                        </td>
                                        <td>
                                            <asp:Literal ID="ltrtongtiennhuan" runat="server"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Kiểu bài: 
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlnhuanbuttype" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tác giả: 
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlnhuanbutuser" CssClass="form-control"></asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Tiền nhuận bút: 
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtcredit1" CssClass="form-control auto currency" Text="0"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="ln_solid"></div>
                            <div class="form-group">
                                <asp:LinkButton ID="lbtDuyet" runat="server" Text="Cập nhật" CssClass="btn btn-success btn-xs" OnClientClick="return validateformduyet();"></asp:LinkButton>
                                <asp:LinkButton ID="lbtHuy" runat="server" Text="Hủy thao tác" CssClass="btn btn-dark btn-xs"></asp:LinkButton>
                            </div>
                            <div class="ln_solid"></div>
                            <div class="form-group" id="tacgikhac" runat="server" visible="false">
                                <h4>Danh sách tác giả khác</h4>
                                <table class="table table-striped">
                                    <thead>
                                        <tr>
                                            <th>Kiểu</th>
                                            <th style="width: 100px">Tác giả</th>
                                            <th>Tiền</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptTacGiaNhuanBut" runat="server">
                                            <ItemTemplate>
                                                <tr style="<%#ShowNhuanSua(Cint(Eval("id"))) %>">
                                                    <td><%#BL.FormatNhuanButLoaitin(CInt(Eval("Type"))) %></td>
                                                    <td><%#BL.GetButDanh(0, CInt(Eval("UserId"))) %></td>
                                                    <td style="width: 150px">
                                                        <%#Eval("Credit") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="ln_solid"></div>
                            <div class="form-group">
                                <div class="col-md-6 col-sm-6 col-xs-12 col-md-offset-3">
                                    <%----%>
                                    <%--<asp:LinkButton ID="btnDuyetCancel" runat="server" Text="Hủy" CssClass="btn btn-primary"></asp:LinkButton>--%>
                                    <asp:HiddenField ID="hfdduyetid" runat="server" />
                                </div>
                            </div>

                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
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
                <h5 class="nk-block-title">Chuyên mục</h5>
                <div class="nk-block-des">
                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control select2"></asp:DropDownList>
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
<!-- /.box-body -->
<script type="text/javascript">
    function checkvalidate() {
        var txtStartDate = document.getElementById('<%=txtStartDate.ClientID%>').value;
        if (txtStartDate == "") {
            alert("Bạn chưa chọn ngày bắt đầu");
            document.getElementById('<%=txtStartDate.ClientID%>').focus();
            return false;
        }
        var txtEndDate = document.getElementById('<%=txtEndDate.ClientID%>').value;
        if (txtEndDate == "") {
            alert("Bạn chưa chọn ngày kết thúc!");
            document.getElementById('<%=txtEndDate.ClientID%>').focus();
            return false;
        }
    }

    function validateformduyet() {
        var txtcredit1 = document.getElementById('<%=txtcredit1.ClientID%>').value;
        if ((txtcredit1 == "") || (txtcredit1 == 0)) {
            alert("Bạn chưa Nhập tiền nhuận bút");
            document.getElementById('<%=txtcredit1.ClientID%>').focus();
            return false;
        }
    }
</script>
<script type="text/javascript">
    //Duyet xuat ban
    function OpenDialogDuyet() {
        $("#modal-sua").modal();
        //Tien nhuan but
        $(".currency").on({
            keyup: function () {
                formatCurrency($(this));
            },
            blur: function () {
                formatCurrency($(this), "blur");
            }
        });

    }
    function CloseDialogDuyet() {
        $("#modal-sua").removeClass("show");
        $(".modal-backdrop").remove();
        $('body').removeClass('modal-open');
        $('body').css('padding-right', '');
        $("#modal-sua").hide();
        $('.modal-backdrop').css({
            display: 'none'
        });
    };
</script>
