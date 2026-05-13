<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="settings.ascx.vb" Inherits="NVCMS.Modules.BannerAdv.settings" %>

        <div class="box-body  pdf0 pdr0">
            <div class="col-md-12 col-sm-12 pdf0">
                <div class="form-group">
                    <label>Chọn Vị trí</label>
                    <asp:DropDownList ID="ddlvitri" runat="server"  CssClass="form-control select2"></asp:DropDownList>
                </div>
                <!-- /.form-group -->
                <!-- /.form-group -->
            </div>
            
            <div class="col-md-12">
                <div class="form-group">
                    <label>Chọn Template</label>
                    <asp:DropDownList ID="dropTemplate" runat="server"  CssClass="form-control select2" ></asp:DropDownList>
                </div>
            </div>
        </div>
