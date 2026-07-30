<%@ Control Language="VB" AutoEventWireup="false" CodeFile="cauhoithuonggap.ascx.vb" Inherits="NVCMS.Modules.FAQs.MainCustomeDisplaySpecial" %>
<div class="row">
    <div class="col-lg-4 col-md-4">
        <div class="dashboard-menu cauhoithuonggap">
            <ul class="nav flex-column" role="tablist">
                <asp:Repeater ID="rptcauhoithuonggap" runat="server">
                    <ItemTemplate>
                        <li class="nav-item">
                            <a class="nav-link <%# GetItemClass(Container.ItemIndex) %>" id="dashboard-tab" data-toggle="tab" href="#cauhoithuonggap<%#Eval("id") %>" role="tab" aria-controls="cauhoithuonggap<%#Eval("id") %>" aria-selected="false">
                                <i class="ti-comment-alt mr-5"></i><%#Eval("CauHoi") %></a>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
    <div class="col-lg-8 col-md-8">
        <div class="tab-content dashboard-content cauhoithuonggap">
            <asp:Repeater ID="rptcauhoithuonggap2" runat="server">
                <ItemTemplate>
                    <div class="tab-pane fade <%# GetItemClass2(Container.ItemIndex) %>" id="cauhoithuonggap<%#Eval("id") %>" role="tabpanel" aria-labelledby="cauhoithuonggap<%#Eval("id") %>-tab">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="mb-0"><%#Eval("Cauhoi") %></h5>
                            </div>
                            <div class="card-body">
                                <%#Server.HtmlDecode(Eval("Traloi")) %>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>
</div>
