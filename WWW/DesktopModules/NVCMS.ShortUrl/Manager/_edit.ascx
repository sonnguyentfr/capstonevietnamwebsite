<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="_edit.ascx.vb" Inherits="NVCMS.Modules.ShortURL.inc_edit" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<%@ Register Src="~/controls/Pagesadmin.ascx" TagPrefix="uc1" TagName="Pages" %>

<style type="text/css">
    .location {
        padding: 5px;
    }

        .location .project {
        }

            .location .project .project-head {
            }

                .location .project .project-head .user-avatar {
                    background: none;
                    border-radius: 0px;
                    width: unset;
                }

                    .location .project .project-head .user-avatar img {
                        border-radius: 0px;
                        max-height: 40px;
                    }

    .toggle-slide-right.content-active {
        transform: unset;
    }

    .toggle-slide.content-active {
        position: relative;
    }
</style>
<div class="nk-block nk-block-lg">
    <div class="card-inner">
        <div class="form-group">
            <label class="form-label" for="full-name">Short Link <strong>(không dùng dấu / )</strong></label>
            <div class="form-control-wrap">
                <asp:TextBox ID="txtshorturl" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="InputValidate" ControlToValidate="txtshorturl" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Vui lòng nhập short Link"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator
                    ID="valTitle" runat="server" ControlToValidate="txtshorturl" ValidationGroup="InputValidate"
                    Display="Dynamic" CssClass="NormalRed" ErrorMessage="Vui lòng nhập shortLink"
                    ForeColor="" ValidationExpression=".{1}.*"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <label class="form-label" for="email-address">Link Gốc</label>
            <div class="form-control-wrap">
                <asp:TextBox ID="txtrealurl" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ValidationGroup="InputValidate" ControlToValidate="txtrealurl" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Vui lòng nhập Link đầy đủ"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator
                    ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtrealurl" ValidationGroup="InputValidate"
                    Display="Dynamic" CssClass="NormalRed" ErrorMessage="Vui lòng nhập Link đầy đủ"
                    ForeColor="" ValidationExpression=".{3}.*"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="form-group">
            <asp:LinkButton ID="lbtXBSave" CssClass="btn btn-success btn-sm pull-left" ToolTip="OK" runat="server" ValidationGroup="InputValidate">Cập nhật</asp:LinkButton>
            <asp:LinkButton ID="lbtXBCancel" CssClass="btn btn-warning btn-sm pull-left" ToolTip="Hủy bỏ" runat="server" data-dismiss="modal">Huỷ bỏ</asp:LinkButton>
            <asp:LinkButton ID="lbtXoa" CssClass="btn btn-dark btn-sm pull-left" OnClientClick="formModified=false; return confirm('Bạn có thực sự muốn xóa không? Xóa là mất hết mọi thứ');" ToolTip="Xóa shortlink" runat="server" data-dismiss="modal">Xóa Link</asp:LinkButton>
            <asp:HiddenField ID="hdf_idkhachhang" runat="server" />
        </div>
    </div>
</div>
