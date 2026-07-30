<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="faqtraloi.ascx.vb" Inherits="NVCMS.Modules.FAQs.inc_list" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke" Namespace="DotNetNuke.UI.WebControls" %>
<div class="row">
    <div class="col-lg-12 col-md-12 col-sm-12">
        <div class="danhsachcautraloi" id="dscautraloi">
            <asp:Repeater ID="drgDataViewer" runat="server">
                <ItemTemplate>
                    <div class="danhsachcautraloi-item justify-content-between pb-20 mb-20">
                        <p class="comment">
                            <%# Eval("UserName")%> | <i class="ti-timer mr-5"></i><%# BL.FormatDate(Eval("CreatedDate"))%>
                        </p>
                        <p class="cauhoi">
                            <span>Hỏi đáp</span> <%# Eval("Question")%>
                        </p>
                        <p class="traloi">
                            <span>Trả lời</span> <a href="#" data-toggle="collapse" data-target="#cautraloi-<%# Eval("id")%>">Xem chi tiết</a>
                        </p>
                        <div class="accordion-body collapse" id="cautraloi-<%# Eval("id")%>" data-parent="#dscautraloi">
                            <%# Server.HtmlDecode(DataBinder.Eval(Container.DataItem, "Traloi")) %>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <dnn:PagingControl ID="ctlPagingControl" runat="server" EnableViewState="true" Mode="URL" PageLinksPerPage="20" />
        </div>
    </div>
</div>
