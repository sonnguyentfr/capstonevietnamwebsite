<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="KetNoiThuongHieu.ascx.vb" Inherits="NVCMS.Modules.BannerAdv.KetNoiThuongHieu" %>

<div id="ketnoithuonghieu" class="ketnoithuonghieu">
    <div class="module-title">
        <h3 class="title"><span class="bg-1">Kết nối thương hiệu</span></h3>
    </div>
    <div id="footer-slider" class="owl-carousel">
        <asp:Repeater ID="drgOtherNews" runat="server">
            <ItemTemplate>
                <div style="display: block; padding-bottom: 10px;">
                    <%# GetBanner(Eval("id")) %>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</div>

