<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Listen.ascx.vb" Inherits="DesktopModules.NV_Videos.Manager.Videos.Listen" %>
<div class="x_content">
    <asp:LinkButton ID="lbtCancel" runat="server" CssClass="btn btn-dark" Font-Bold="true">
                                     <i class="fa fa-save"></i> Quay lại</asp:LinkButton>
    <div class="col-md-7 col-sm-7 col-xs-12" style="border: 0px solid #e5e5e5;">
        <h3 class="prod_title">
            <asp:Literal ID="lblTenBaiHat" runat="server"></asp:Literal></h3>
        <div class="">
            <ul class="list-inline prod_size">
                <li>
                    <button type="button" class="btn btn-default btn-xs">
                        <asp:Literal ID="ltrdate" runat="server"></asp:Literal>
                    </button>
                </li>
            </ul>
        </div>
        <p><strong>
            <asp:Literal ID="ltrtomtat" runat="server"></asp:Literal></strong></p>
        <p>
            <asp:Literal ID="ltrnoidung" runat="server"></asp:Literal></p>
        <div class="product_social">
            <ul class="list-inline">
                <li><a href="#"><i class="fa fa-facebook-square"></i></a>
                </li>
                <li><a href="#"><i class="fa fa-twitter-square"></i></a>
                </li>
                <li><a href="#"><i class="fa fa-envelope-square"></i></a>
                </li>
                <li><a href="#"><i class="fa fa-rss-square"></i></a>
                </li>
            </ul>
        </div>

    </div>
    <div class="col-md-5 col-sm-5 col-xs-12">
        <div class="product-image">
            <h4>Kiểu Video:
                <asp:Literal ID="ltrkieuvideo" runat="server"></asp:Literal>
            </h4>
            <asp:Literal ID="ltrplayVideo" runat="server"></asp:Literal>
        </div>
    </div>

</div>
