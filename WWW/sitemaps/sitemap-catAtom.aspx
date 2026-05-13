<%@ Page Language="C#" AutoEventWireup="true" ContentType="text/xml" CodeFile="sitemap-catAtom.aspx.cs" Inherits="feeds" %>

<asp:repeater id="RepeaterRSS" runat="server">
        <HeaderTemplate>
            <rss xmlns:atom="http://www.w3.org/2005/Atom" version="2.0">
                <channel>
                    <title><%#CatName %></title>
                    <description><%#CatSummary %></description>
                    <link><%#CatLink %></link>
                    <copyright>thuongtruong.com.vn - Tạp chí Thương Trường điện tử</copyright>
                    <generator>thuongtruong.com.vn - Tạp chí Thương Trường điện tử</generator>
                    <language>vi-vn</language>
                    <pubDate><%# String.Format("{0:R}", DateTime.Now)%></pubDate>
                    <lastBuildDate><%# String.Format("{0:R}", DateTime.Now)%></lastBuildDate>
                    <managingEditor>toasoanthuongtruong@gmail.com (Tạp chí Thương Trường điện tử)</managingEditor>
                    <webMaster>toasoanthuongtruong@gmail.com (Tạp chí Thương Trường điện tử)</webMaster>
                    <atom:link href="https://thuongtruong.com.vn/sitemap.xml" type="application/rss+xml"/>
                    <image>
                        <url>https://cdn.thuongtruong.com.vn/nvcms/img/logo.png</url>
                        <title><%# DataBinder.Eval(Container.DataItem, "CategoryName") %></title>
                        <link><%#CatLink %></link>
                    </image>
        </HeaderTemplate>
        <ItemTemplate>
            <item>
                <title><![CDATA[<%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "Title")) %>]]></title>
                <link><%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "CategoryId"))), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NewId")), Convert.ToString(DataBinder.Eval(Container.DataItem, "Title"))) %></link>
                <description><![CDATA[<%# RemoveIllegalCharacters(DataBinder.Eval(Container.DataItem, "Summary"))%>]]></description>
                <author><%# RemoveIllegalCharacters(BL.GetButDanh(0,Convert.ToInt32( DataBinder.Eval(Container.DataItem, "UserId"))))%></author>
                <pubDate><%# String.Format("{0:R}", DataBinder.Eval(Container.DataItem, "PublishedDate"))%></pubDate>
                <guid><%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "CategoryId"))), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NewId")), Convert.ToString(DataBinder.Eval(Container.DataItem, "Title"))) %></guid>
                <atom:link href="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "CategoryId"))), Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NewId")), Convert.ToString(DataBinder.Eval(Container.DataItem, "Title"))) %>" rel="self" type="application/rss+xml"/>
                        
            </item>
        </ItemTemplate>
        <FooterTemplate>
            </channel>
            </rss>  
        </FooterTemplate>
    </asp:repeater>
