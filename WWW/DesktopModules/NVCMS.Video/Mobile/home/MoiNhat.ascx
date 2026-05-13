<%@ Control Language="VB" EnableViewState="false" AutoEventWireup="false" CodeFile="Moinhat.ascx.vb" Inherits="NVCMS.Modules.Video.MoiNhat" %>
<div class="videohomemoinhat">
    <div class="title-container no-pt no-pb">
        <h3 class="section-title">Video - Clips</h3>
    </div>
    <ul class="card-list scrollable pr-20 pl-20">
        <asp:Repeater ID="rptMoinhatVideo" runat="server">
            <ItemTemplate>
                <li>
                    <div class="card">
                        <div class="card-image">
                            <a href="<%# Ultis.EventsFormatLink(1194, "d" & (DataBinder.Eval(Container.DataItem, "id")), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                <img class="img-fluid lazy" src="//f.thuongtruong.com.vn/nophoto.png?dpi=150&quality=150&width=280&height=170&mode=crop&anchor=middlecenter" data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "Avatar"), 270, 180, "crop", "middlecenter", "") %>" alt="<%#ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "Title"))%>">
                                <div class="media-caption">
                                    <span class="icon"><i class="fa fa-video-camera"></i></span>
                                </div>
                            </a>
                        </div>
                        <div class="card-content">
                            <div class="entry-title">
                                <a href="<%# Ultis.EventsFormatLink(1194, "d" & (DataBinder.Eval(Container.DataItem, "id")), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                    <%# DataBinder.Eval(Container.DataItem, "Title")%>
                                </a>
                            </div>
                        </div>
                    </div>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</div>
