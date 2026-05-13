<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Index.ascx.vb"
    Inherits="NVCMS.Modules.Video.Index" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<div class="row no-gutter videoindex">
    <div id="truyenhinhinternkhac">
        <asp:Repeater ID="rptMoinhatVideoz" runat="server">
            <ItemTemplate>
                <div class="col-md-3 item">
                    <div class="big-gallery">
                        <img class="img-responsive img-full lazy" src="/data/noimage.png?width=291&height=185" data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "Avatar"), 291, 185, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" />
                        <a title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "Title"))%>" href="<%# Ultis.EventsFormatLink(1194, "d" & DataBinder.Eval(Container.DataItem, "id"), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                            <span class="play-icon"></span></a>
                        <h3>
                            <a title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "Title"))%>" href="<%# Ultis.EventsFormatLink(1194, "d" & DataBinder.Eval(Container.DataItem, "id"), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                <%# DataBinder.Eval(Container.DataItem, "Title")%>
                            </a>
                        </h3>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    </div>
<div class="row no-gutter videoindex">
    <div class="clearfix pagination-wp">
        <ul class="pagination pull-left">
            <vbuzz:PAGING ID="vbPaging" runat="server" />
        </ul>
        <div class="cl"></div>
    </div>
</div>
