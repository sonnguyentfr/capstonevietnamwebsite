<%@ Control Language="VB" AutoEventWireup="false" CodeFile="U_NhuanBut.ascx.vb" Inherits="DesktopModules.TinTuc.Control.ThongkeNhuanButUser" %>
<%-- Trang chủ quản trị: Phân tích Marketing từ Google Analytics 4 (cấu hình ở Cấu hình website › Google Analytics).
     Giao diện: DesktopModules/NVCMS.Marketing/Analytics/ga-analytics.js; API: NVCMS.API Controller/Analytics/MarketingAnalyticsController.cs --%>
<link rel="stylesheet" href="<%= AssetUrl("ga-analytics.css") %>" />
<div class="tt-dash mk-dash" data-nvga-screen="marketing" data-module-id="<%= ModuleId %>" data-tab-id="<%= TabId %>" data-api-root="<%= ApiRoot %>">
    <div class="nvc-loading">Đang tải số liệu Google Analytics…</div>
</div>
<script defer src="<%= AssetUrl("ga-analytics.js") %>"></script>
