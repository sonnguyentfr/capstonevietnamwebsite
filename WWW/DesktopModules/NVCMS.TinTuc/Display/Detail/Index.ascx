<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Explicit="true" CodeFile="Index.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.Index" %>
<%@ Register TagPrefix="vbuzz" TagName="PAGING" Src="~/Controls/Pages.ascx" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/controls/HotCategory.ascx" TagPrefix="uc" TagName="HotCategory" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/controls/TinMoiNhat.ascx" TagPrefix="uc2" TagName="MoiNhat" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/controls/DocNhieu.ascx" TagPrefix="uc2" TagName="DocNhieu" %>
<%@ Register Src="~/DesktopModules/NVCMS.TinTuc/Display/controls/XuHuongdoc.ascx" TagPrefix="uc" TagName="XuHuongdoc" %>
<%--<%@ Register Src="~/DesktopModules/NVCMS.Video/Display/control/home/Videomoinhat.ascx" TagPrefix="uc" TagName="Videomoinhat" %>--%>
<script type="text/javascript">$(location).attr('href', '/')</script>
<div class="row newsindex">
    <!--========== BEGIN .COL-MD-8 ==========-->
    <div class="col-md-9 pr-0">
        <div class="module-title">
            <h3 class="title fl">
                <asp:Literal ID="ltrtitlecat" runat="server"></asp:Literal>
            </h3>
            <div class="subtitlecat">
                <ul>
                    <asp:Repeater ID="rptsubcat" runat="server">
                        <ItemTemplate>
                            <li class="<%#ActiveSubCat(Eval("CategoryId")) %>"><a href="<%# NavigateURL(BL.GetMappingTabIDByCategoryID(Eval("CategoryId"))) %>"><%#Eval("CategoryName") %></a></li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </div>
        <uc:HotCategory runat="server" ID="HotCategory" />
        <!--========== BEGIN .ROW ==========-->
        <div class="row">
            <div class="col-xs-12 col-sm-9 col-md-9 ">
                <!--========== BEGIN .NEWS ==========-->
                <div class="news listnews">
                    <asp:Repeater runat="server" ID="rptContent">
                        <ItemTemplate>
                            <div class="item">
                                <div class="item-image-3">
                                    <a class="img-link" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                                        <img class="img-responsive img-full lazy" src="/data/nophoto240-160.png" data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 240, 160, "crop", "middlecenter", "") %>" alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" />
                                        <%--<img class="img-responsive img-full lazy"
                                            src="https://f.thuongtruong.com.vn/nophoto240-160.png"
                                            data-src="<%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 240, 160, "crop", "middlecenter", "") %>"
                                            srcset="
			                                 <%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 240, 160, "crop", "middlecenter", "") %> 1920w,
			                                 <%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 140, 120, "crop", "middlecenter", "") %> 600w,
			                                 <%# Ultis.FormatThumbImage(DataBinder.Eval(Container.DataItem, "ImagePath"), 140, 120, "crop", "middlecenter", "") %> 320w"
                                            sizes="(min-width: 1920px) 1920px, 100vw"
                                            alt="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" />--%>
                                    </a>
                                </div>
                                <div class="item-content">
                                    <div class="title-left title-style04 underline04">
                                        <h3><a href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" title="<%# ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>">
                                            <strong><%# DataBinder.Eval(Container.DataItem, "title")%></strong>

                                        </a></h3>
                                    </div>
                                    <div class="post-meta-elements">
                                        <div class="post-meta-date">
                                            <i class="fa fa-calendar"></i><%# BL.FormatDate(DataBinder.Eval(Container.DataItem, "PublishedDate"))%>
                                        </div>
                                    </div>
                                    <p><%# DataBinder.Eval(Container.DataItem, "summary")%> </p>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="clearfix pagination-wp">
                    <ul class="pagination pull-left">
                        <vbuzz:PAGING ID="vbPaging" runat="server" />
                    </ul>
                    <div class="cl"></div>
                </div>
                <!--========== END .NEWS ==========-->
            </div>
            <div class="col-xs-12 col-sm-3 col-md-3 pr-0 pl-0">
                <div class='quangcaogoogle' style='text-align:center;'>
                    <!-- QC Gooogle Index.HotCat -->
                    
						<!-- PC.160x600 -->
						<ins class="adsbygoogle"
							 style="display:inline-block;width:160px;height:600px"
							 data-ad-client="ca-pub-3311450421751656"
							 data-ad-slot="9714707914"></ins>
						<script>
							 (adsbygoogle = window.adsbygoogle || []).push({});
						</script>
                </div>
				
                <asp:Repeater ID="rptCat" runat="server" OnItemDataBound="rptCatItemDataBound">
                    <ItemTemplate>
                        <div class="style5" id="ListCat" runat="server" visible='<%# Checkhien(Eval("CategoryId"))%>'>
                            <div class="title-style02">
                                <h3>
                                    <a href="<%# NavigateURL(BL.GetMappingTabIDByCategoryID(Eval("CategoryId"))) %>"><strong><%#Eval("CategoryName") %></strong></a></h3>
                            </div>
                            <div class="sidebar-scroll">
                                <div class="scroll-item">
                                    <asp:Repeater ID="rptListNewsCatHot" runat="server">
                                        <ItemTemplate>
                                            <div class="item">
                                                <div class="item-content-1">
                                                    <h3>
                                                        <a title="<%#ReplaceChuoi.titlenews(DataBinder.Eval(Container.DataItem, "title"))%>" href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>">
                                                            <i class="fa fa-angle-double-right"></i><%# DataBinder.Eval(Container.DataItem, "title")%></a>
                                                    </h3>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="CatId" runat="server" Value='<%# Eval("CategoryId") %>' />
                    </ItemTemplate>
                </asp:Repeater>
				<div class='sidebar-fixed4 quangcaogoogle' style='text-align:center;'>
					<!-- QC Gooogle Index.HotCat -->
						<!-- PC.160x600 -->
						<ins class="adsbygoogle"
							 style="display:inline-block;width:160px;height:600px"
							 data-ad-client="ca-pub-3311450421751656"
							 data-ad-slot="9714707914"></ins>
						<script>
							 (adsbygoogle = window.adsbygoogle || []).push({});
						</script>
					</div>
            </div>
        </div>
        <!--========== END .ROW ==========-->
    </div>
    <!--========== END .COL-MD-8 ==========-->
    <!--========== BEGIN .COL-MD-4 ==========-->
    <div class="col-md-3">
        <uc2:MoiNhat runat="server" ID="MoiNhat" count="8" />
		
        <uc2:DocNhieu runat="server" ID="DocNhieu" count="8" />
        <uc:XuHuongdoc runat="server" ID="XuHuongdoc" />
        
		<div class='sidebar-fixed3 quangcaogoogle' style='text-align:center;'>
			<!-- PC.300x600 -->
			<ins class="adsbygoogle"
				 style="display:inline-block;width:300px;height:600px"
				 data-ad-client="ca-pub-3311450421751656"
				 data-ad-slot="8358212457"></ins>
			<script>
				 (adsbygoogle = window.adsbygoogle || []).push({});
			</script>
			
		</div>
    </div>
    <div class="col-md-12 no-gutter mt-20 videomoinhatz">
        <%--<uc:Videomoinhat runat="server" ID="Videomoinhat" count="8" />--%>
    </div>
    <!--========== END .COL-MD-4 ==========-->
</div>

