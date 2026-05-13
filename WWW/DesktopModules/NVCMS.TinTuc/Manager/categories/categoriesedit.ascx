<%@ Control Language="vb" AutoEventWireup="false" Explicit ="true"  codefile="categoriesedit.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.categories.categoriesedit" %>

<div class="x_content">
    <br>
    <div id="demo-form2" class="form-horizontal form-label-left">

        <div class="form-group">
            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="txtCategoryName">
                Tên danh mục <span class="required">*</span>
            </label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control col-md-5 col-xs-12" Width="400px" required="required" ValidationGroup="InputValidate"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCategoryName"
                    Display="Dynamic" ErrorMessage="Nhập tên thư mục" ValidationGroup="InputValidate"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="txtCategoryName">
                Tên tiếng anh <span class="required">*</span>
            </label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <asp:TextBox ID="txtEnglishName" runat="server" CssClass="form-control col-md-5 col-xs-12" Width="400px"></asp:TextBox>
            </div>
        </div>
        <div class="form-group" style="display: none;">
            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="txtCategoryName">
                Ảnh đại diện <span class="required">*</span>
            </label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <asp:Image ID="imgAvatar1" runat="server" /><br />
                <input type="text" name="Image" id="imgAvatar" style="width: 500px" runat="server" class="form-control col-md-7 col-xs-12" />
                <br />
                <br />
                <input type="button" value="Chọn Ảnh" onclick="BrowseImages();" />
                <input type="button" value="Xóa" onclick="RemoveImages();" />
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="last-name">
                Thư mục cha <span class="required">*</span>
            </label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <asp:DropDownList ID="ddlParentCategory" runat="server" CssClass="form-control col-md-7 col-xs-12" Width="400px"></asp:DropDownList>
            </div>
        </div>
        <div class="form-group">
            <label for="middle-name" class="control-label col-md-3 col-sm-3 col-xs-12">Mô tả</label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <textarea id="txtdescription" runat="server" class="form-control col-md-7 col-xs-12" rows="5"></textarea>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-md-3 col-sm-3 col-xs-12">Trạng thái</label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <div class="checkbox">
                    <label>
                        <input type="checkbox" class="flat" checked="checked" id="chkIsActive" runat="server">
                        Hiện thị
                           
                    </label>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="last-name">
                Trang hiện thị <span class="required">*</span>
            </label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <asp:DropDownList ID="drlTabID" runat="server" CssClass="form-control col-md-7 col-xs-12" DataValueField="TabID" DataTextField="IndentedTabName" Width="400px"></asp:DropDownList>
                <asp:Literal ID="ltrpagePC" runat="server" Visible="false"></asp:Literal>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-md-3 col-sm-3 col-xs-12" for="last-name">
                Trang hiện thị chi tiết <span class="required">*</span>
            </label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <asp:DropDownList ID="drlTabIDM" runat="server" CssClass="form-control col-md-7 col-xs-12" Width="400px"></asp:DropDownList>
                <asp:Literal ID="ltrpageMobile" runat="server" Visible="false"></asp:Literal>
            </div>
        </div>
        <div class="form-group">
            <label class="control-label col-md-3 col-sm-3 col-xs-12">
                Sắp xếp
            </label>
            <div class="col-md-6 col-sm-6 col-xs-12">
                <input type="text" id="txtOrderNumber" runat="server" required="required" class="form-control col-md-7 col-xs-12" style="width: 80px" value="0">
            </div>
        </div>
        <div class="ln_solid"></div>
        <div class="form-group">
            <div class="col-md-6 col-sm-6 col-xs-12 col-md-offset-3">
                <asp:LinkButton ID="lbtUpdate" ValidationGroup="VBuzzValidation" runat="server" Font-Bold="True" CssClass="btn btn-success">
                        Cập nhật
                </asp:LinkButton>
                <asp:LinkButton ID="lbtDelete" OnClientClick="javascript: return confirm('Bạn có muốn xoá không?');" runat="server" Font-Bold="True" CssClass="btn btn-primary">
                         Xoá
                </asp:LinkButton>
                <asp:LinkButton ID="lbtCancel" runat="server" ValidationGroup="VBuzzValidation22" Font-Bold="True" CssClass="btn btn-primary">
                        Hủy Thao tác
                </asp:LinkButton>
            </div>
        </div>

    </div>
</div>