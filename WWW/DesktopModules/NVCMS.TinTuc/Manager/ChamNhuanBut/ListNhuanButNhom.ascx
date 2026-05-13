<%@ Control Language="vb" Inherits="NVCMS.Modules.TinTuc.NhuanButView" ClientIDMode="Static" CodeFile="ListNhuanButNhom.ascx.vb" AutoEventWireup="false" Explicit="True" %>
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
</style>

<div class="box-body  pdf0 pdr0">
    <div class="col-md-12">
        <!-- /.col -->
        <div class="col-md-2">
            <div class="form-group">
                <label>Tác giả</label>
                <asp:DropDownList ID="ddlUsers" runat="server" AutoPostBack="true" CssClass="form-control select2"></asp:DropDownList>
            </div>
            <!-- /.form-group -->
            <!-- /.form-group -->
        </div>
        <div class="col-md-2">
            <div class="form-group">
                <label>Loại nhuận bút</label>
                <asp:DropDownList ID="ddlKieuNhuanBut" runat="server" CssClass="form-control select2">
                    <asp:ListItem Value="0" Text="Tất cả"></asp:ListItem>
                    <asp:ListItem Value="1" Text="Tin Bài"></asp:ListItem>
                    <asp:ListItem Value="2" Text="Video Clips"></asp:ListItem>
                </asp:DropDownList>
            </div>
            <!-- /.form-group -->
            <!-- /.form-group -->
        </div>
        <div class="col-md-4">
            <div class="form-group">
                <div class="col-md-6 pdf0">
                    <label>Từ ngày </label>
                    <div class="input-group date">
                        <div class="input-group-addon">
                            <i class="fa fa-calendar"></i>
                        </div>
                        <input type="text" id="txtStartDate" runat="server" class="form-control pull-right datepicker">
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
        <div class="col-md-2 pdf0">
            <div class="form-group">
                <asp:LinkButton ID="lbtReport" runat="server" Font-Bold="true" CssClass="btn btn-primary" ToolTip="Tìm kiếm" OnClientClick="return checkvalidate();"><i class="fa fa-search"></i> Xuất</asp:LinkButton>
                <asp:LinkButton ID="lbtReportNhom" runat="server" Font-Bold="true" CssClass="btn btn-primary" ToolTip="Tìm kiếm" OnClientClick="return checkvalidate();"><i class="fa fa-search"></i> Xuất nhoms</asp:LinkButton>
            </div>
        </div>
        <div class="col-md-1 pdf0">
            <div class="form-group">
                <asp:LinkButton ID="lbtView" runat="server" Font-Bold="true" CssClass="btn btn-danger" ToolTip="Tìm kiếm" OnClientClick="return checkvalidate();"><i class="fa fa-search"></i> Xem</asp:LinkButton>
            </div>
        </div>
        <div class="col-md-1 pdf0">
            <div class="form-group">
                <asp:LinkButton ID="lbtViewThongke" runat="server" Font-Bold="true" CssClass="btn btn-danger" ToolTip="Tìm kiếm">Thống kê</asp:LinkButton>
            </div>
        </div>
    </div>
</div>
<!-- /.box-body -->
<asp:UpdatePanel runat="server" ID="upnlAttxuatban">
    <ContentTemplate>
        <div class="col-md-12  pdf0 pdr0">
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

            <table class="table table-striped jambo_table bulk_action table-bordered" id="nhuanbut">
                <thead class="headings">
                    <tr>
                        <th colspan="1" rowspan="2" style="width: 20px; text-align: center;">STT</th>
                        <th colspan="1" rowspan="2" style="width: 300px; text-align: center;">B&agrave;i viết</th>
                        <th colspan="1" rowspan="2" style="width: 30px; text-align: center;">PR</th>
                        <th colspan="1" rowspan="2" style="width: 100px; text-align: center;">Loại tin bài</th>
                        <th colspan="6" rowspan="1" style="text-align: center;">TH&Ocirc;NG TIN NHUẬN B&Uacute;T</th>
                        <th colspan="3" rowspan="1" style="text-align: center;">NGƯỜI TẠO - T&Aacute;C GIẢ</th>
                        <th colspan="1" rowspan="2" style="width: 20px; text-align: center;">Sửa</th>
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
                                <td style="text-align:center;"><%# Container.ItemIndex + 1%></td>
                                <td>

                                    <a target="_blank" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                       <%#Eval("Title")%> (<i class="fa fa-eye"></i><%#Eval("ViewCount")%>)
                                    </a></td>
                                <td style="width: 30px;"><%#IIf(CBool(DataBinder.Eval(Container.DataItem, "isPR")) = True, "V", "") %></td>
                                <td style="width: 110px;"><%# BL.FormatLoaiTinBaiHTMLl(Eval("NewsKind"))%></td>
                                <td style="width: 200px;"><%#Eval("CategoryName")%></td>
                                <td style="width: 100px; text-align: center;" class="auto"><%#BL.FormatDate(Eval("PublishedDate"))%></td>
                                <td style="width: 100px; text-align: center;" class="auto"><%#TienTin(Eval("Newid"), 4)%>
                                   
                                </td>
                                <td style="width: 100px; text-align: center;"><span class="auto currency"><%#TienTin(Eval("Newid"), 1)%></span></td>
                                <td style="width: 100px; text-align: center;" class="auto currency"><%#TienTin(Eval("Newid"), 2)%></td>
                                <td style="width: 100px; text-align: center;" class="auto"><%#TienTin(Eval("Newid"), 3)%></td>
                                <td style="width: 100px; text-align: center;"><%# BL.GetNameByUserId(PortalId, Eval("UserId"))%></td>
                                <td style="width: 100px; text-align: center;"><%# BL.GetNameByUserId(PortalId, Eval("UserId"))%>
                                <td style="width: 100px; text-align: center;"><%# Eval("ButDanh")%>
                                </td>
                                <td>
                                    <asp:Button ID="cmdApprove" CommandArgument='<%#Eval("NewId") %>' CommandName="cmdEdit" Text="[Sửa]" OnClick="cmdEdit" CssClass="btn btn-nho btn-sua"
                                        runat="server" title="Sửa nhuận bút" data-toggle="tooltip" />
                                    <asp:Label ID="lblnewid" runat="server" Text='<%# Eval("NewId") %>' Visible="false" /></td>
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
        <div class="modal fade" id="modal-sua">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">Sửa thông tin nhuận bút</h4>
                    </div>
                    <div class="modal-body form-horizontal form-label-left">
                        <div class="form-group">
                            <h4>
                                <asp:Literal ID="ltrtitle" runat="server"></asp:Literal>
                            </h4>
                        </div>
                        <div class="form-group">
                            <label class="col-md-2 col-sm-3 col-xs-12" for="last-name">
                                Lượt xem:
                            </label>
                            <div class="col-md-1 col-sm-4 col-xs-12">
                                <asp:Literal ID="ltrview" runat="server"></asp:Literal>
                            </div>
                            <label class="col-md-2 col-sm-3 col-xs-12" for="last-name">
                                Ngày đăng: 
                            </label>
                            <div class="col-md-3 col-sm-6 col-xs-12">
                                <asp:Literal ID="ltrngaydang" runat="server"></asp:Literal>
                            </div>
                            <label class="col-md-2 col-sm-3 col-xs-12" for="last-name">
                                Tổng tiền: 
                            </label>
                            <div class="col-md-2 col-sm-6 col-xs-12">
                                <asp:Literal ID="ltrtongtiennhuan" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="ln_solid"></div>
                        <div class="form-group">
                            <div class="col-md-2">
                                <label>Kiểu bài</label>
                                <asp:DropDownList runat="server" ID="ddlnhuanbuttype" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-5">
                                <label>Người nhận</label>
                                <asp:DropDownList runat="server" ID="ddlnhuanbutuser" CssClass="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-5">
                                <label>Tiền nhuận bút</label>
                                <asp:TextBox runat="server" ID="txtcredit1" CssClass="form-control auto currency" Text="0"></asp:TextBox>
                            </div>

                        </div>
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
                                            <tr style="<%#ShowNhuanSua(Eval("id")) %>">
                                                <td><%#BL.FormatNhuanButLoaitin(Eval("Type")) %></td>
                                                <td><%#BL.GetButDanh(Eval("Portalid"), Eval("UserId")) %></td>
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
        $('#modal-sua').modal('hide');
        $('.modal-backdrop').css({
            display: 'none'
        });
    };
</script>
