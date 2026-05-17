<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Explicit="true" CodeFile="Sukiensapdienra.ascx.vb" Inherits="NVCMS.Modules.School.IndexSearch" %>

<div class="col-lg-12 order-lg-12">
    <div class="section-title text-center  mb-20">
        <h2 class="title-clamp mb-20 uppercase font-bold"><a class="font-color" href="#">HỘI THẢO - SỰ KIỆN</a></h2>
        <div class="bar"></div>
    </div>
</div>
<div class="col-lg-12 order-lg-12">
    <div class="slider-2-items">
        <asp:Repeater ID="rptsukien" runat="server" OnItemDataBound="OnItemDataBound">
            <ItemTemplate>
                <div class="item-news bgwhite">
                    <div class="item-image">
                        <a target=_blank href="<%# If(String.IsNullOrEmpty(Eval("link_pr").ToString()), "#", Eval("link_pr")) %>" title="<%#Eval("CatName") %>">
                            <img data-src="<%#Ultis.FormatThumbImage(Eval("Avatar"), 590, 360, "crop", "middlecenter", "") %>" src="/data/no-photo.png?width=540&height=360&mode=crop&anchor=middlecenter" alt="<%#Eval("CatName") %>" class="blur-up lazyload" /></a>
                    </div>
                    <div class="item-content">
                        <h3 class="title-clamp-18 text-black mt-3">
                            <a target=_blank href="<%# If(String.IsNullOrEmpty(Eval("link_pr").ToString()), "#", Eval("link_pr")) %>"><%#Eval("CatName") %></a>
                        </h3>
                        <span class="ttm-entry-date">
                            <time class="entry-date" datetime="<%#Eval("FromDate") %>">
                                <i class="fa fa-clock-o"></i><%#CDate(Eval("FromDate")).ToString("hh:mm dd/MM/yyyy") %> - <%#CDate(Eval("EndDate")).ToString("hh:mm dd/MM/yyyy") %>
                            </time>
                        </span>
                        <asp:HiddenField ID="hdfEventCatId" runat="server" Value='<%#Eval("id") %>' />
                        <ul>
                            <asp:Repeater ID="rptOrders" runat="server">
                                <ItemTemplate>
                                    <li class="ttm-event-meta-item ttm-event-date ttm-event-vanue">Địa điểm: <i class="fa fa-map-marker"></i><%#Eval("Title") %> <%--| <i class="fa fa-clock-o"></i> Thời gian: <%#CDate(Eval("fromdatetime")).ToString("hh:mm dd/MM/yyyy") %> - <%#CDate(Eval("enddatetime")).ToString("hh:mm dd/MM/yyyy") %>--%>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>

                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>
</div>
