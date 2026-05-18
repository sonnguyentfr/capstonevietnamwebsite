<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="KetNoiThuongHieu.ascx.vb" Inherits="NVCMS.Modules.Banner.KetNoiThuongHieu" %>
<div class="container">
    <div class="row">
        <h4 class="presstitle"><%=title %></h4>
        <div class="wrapper doitacb press">
            <div class="wrapper">
                <ul class="recruitbottom owl-carousel">
                    <asp:Repeater ID="drgOtherNews" runat="server">
                        <ItemTemplate>
                            <li class="item">
                                <%# GetBanner(Eval("id")) %>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
    </div>
</div>