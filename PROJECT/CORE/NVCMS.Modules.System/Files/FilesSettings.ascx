<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="FilesSettings.ascx.vb" Inherits="FilesSettings" %>
<asp:UpdatePanel runat="server" ID="uptpanefils">
    <ContentTemplate>

        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtSXCTVirtual" class="">Virtual Directory Mạng SXCT</label>
                    <asp:TextBox ID="txtSXCTVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtSXCTPhysical" class="">Thư mục vật lý</label>
                    <asp:TextBox ID="txtSXCTPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtAnhLuuTruVirtual" class="">Virtual Directory ảnh trong bài <span class="required">*</span></label>
                    <asp:TextBox ID="txtAnhLuuTruVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtAnhLuuTruPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtAnhLuuTruPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtFlashVirtual" class="">Virtual Directory Flash <span class="required">*</span></label>
                    <asp:TextBox ID="txtFlashVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtFlashPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtFlashPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtMediaPathVirtual" class="">Virtual Directory upload <span class="required">*</span></label>
                    <asp:TextBox ID="txtMediaPathVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtMediaPathPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtMediaPathPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtDocumentLuuTruVirtual" class="">Virtual Directory Document <span class="required">*</span></label>
                    <asp:TextBox ID="txtDocumentLuuTruVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtDocumentLuuTruPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtDocumentLuuTruPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtMediaLuuTruVirtual" class="">Virtual Directory Media <span class="required">*</span></label>
                    <asp:TextBox ID="txtMediaLuuTruVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtMediaLuuTruPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtMediaLuuTruPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtBaiHatVirtual" class="">Virtual Directory Media Audio <span class="required">*</span></label>
                    <asp:TextBox ID="txtBaiHatVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtBaiHatPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtBaiHatPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtSanPhamTruVirtual" class="">Virtual Directory Sản phẩm <span class="required">*</span></label>
                    <asp:TextBox ID="txtSanPhamTruVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtSanPhaTruPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtSanPhaTruPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtVideoVirtual" class="">Virtual Directory Media Video <span class="required">*</span></label>
                    <asp:TextBox ID="txtVideoVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtVideoPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtVideoPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtBackupPathVirtual" class="">Virtual Directory BACKUP <span class="required">*</span></label>
                    <asp:TextBox ID="txtBackupPathVirtual" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col s12 m6 l6">
                <div class="position-relative form-group">
                    <label for="txtBackupPathPhysical" class="">Thư mục vật lý <span class="required">*</span></label>
                    <asp:TextBox ID="txtBackupPathPhysical" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="form-row">
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <label for="txtSXCTVirtual" class="">Thời gian gửi request xác định Tin chờ duyệt/xuất bản: <span class="required">*</span></label>
                    <asp:TextBox ID="txtAlertRequestDuration" runat="server" Width="90%" AutoPostBack="True" CssClass="form-control"></asp:TextBox> ms
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <label for="txtAutoSaveRequestDuration" class=""> Thời gian Autosave: <span class="required">*</span></label>
                    <asp:TextBox ID="txtAutoSaveRequestDuration" runat="server" CssClass="form-control"></asp:TextBox>ms
                </div>
            </div>
            <div class="col-md-4">
                <div class="position-relative form-group">
                    <label for="txtDataRequestDuration" class=""> Khoảng thời gian lấy dữ liệu mới (trang chủ): <span class="required">*</span></label>
                    <asp:TextBox ID="txtDataRequestDuration" runat="server" CssClass="form-control"></asp:TextBox> ms
                </div>
            </div>
            
        </div>
        <div class="form-row">
            <div class="col s12 m6 l6">
                <asp:LinkButton ID="lbtUpdate" runat="server" Font-Bold="True" class="btn btn-success">Cập nhật</asp:LinkButton>
                <asp:LinkButton ID="lbtCancelTop" runat="server" Font-Bold="True" CssClass="btn btn-primary"> Thoát</asp:LinkButton>
            </div>
            <div class="col s12 m6 l6">
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress runat="server" ID="UpdateProgress1">
    <ProgressTemplate>
        <div class="loading" id="loadizng">Loading&#8230;</div>
    </ProgressTemplate>
</asp:UpdateProgress>
<table width="100%" cellpadding="2" cellspacing="2" border="1" class="table-bordered">
    <tr style="display: none;">
        <td></td>
        <td>&nbsp;
            
        </td>
    </tr>
    <tr style="display: none;">
        <td>Virtual Directory FTP:</td>
        <td>
            <asp:TextBox ID="txtFTPVirtual" runat="server" Width="200px" AutoPostBack="True"></asp:TextBox>
            &nbsp;Thư mục vật lý:
            <asp:TextBox ID="txtFTPPhysical" runat="server" CssClass="form-control col-md-7 col-xs-12"></asp:TextBox>
        </td>
    </tr>
    <tr style="display: none;">
        <td>Thư mục kết xuất Dalet:</td>
        <td>
            <asp:TextBox ID="txtDalet" runat="server" Width="350px"></asp:TextBox>
            &nbsp;
            <asp:CheckBox ID="chkDalet2XML" runat="server" Checked="false" Text="Xuất XML?" />
        </td>
    </tr>
    <tr style="display: none;">
        <td>Thư mục kết xuất Netia:</td>
        <td>
            <asp:TextBox ID="txtNetia" runat="server" Width="350px"></asp:TextBox>
            &nbsp;
            <asp:CheckBox ID="chkNetia2XML" runat="server" Checked="false" Text="Xuất XML?" />
        </td>
    </tr>
    <tr style="display: none;">
        <td>Thư mục kết xuất mở rộng 1:</td>
        <td>
            <asp:TextBox ID="txtMultiMediaCopyPath1" runat="server" Width="350px"></asp:TextBox>
        </td>
    </tr>
    <tr style="display: none;">
        <td>Thư mục kết xuất mở rộng 2:</td>
        <td>
            <asp:TextBox ID="txtMultiMediaCopyPath2" runat="server" Width="350px"></asp:TextBox>
        </td>
    </tr>
    <tr style="display: none;">
        <td>Thư mục kết xuất mở rộng 3:</td>
        <td>
            <asp:TextBox ID="txtMultiMediaCopyPath3" runat="server" Width="350px"></asp:TextBox>
        </td>
    </tr>
</table>


