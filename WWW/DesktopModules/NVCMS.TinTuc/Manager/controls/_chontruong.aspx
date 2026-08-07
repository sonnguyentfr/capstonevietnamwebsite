<%@ Page Language="VB" AutoEventWireup="false" CodeFile="_chontruong.aspx.vb" Inherits="DesktopModules.TinTuc.Control.ChonTruong" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Chon truong lien quan</title>
    <script src="/Resources/Shared/scripts/jquery/jquery.min.js" type="text/javascript"></script>
    <style>
        body {
            font-family: Nunito,sans-serif;
            font-size: 13px;
            margin: 0;
            padding: 8px;
            background: #f8f9fa
        }

        .search-bar {
            display: flex;
            gap: 6px;
            margin-bottom: 10px
        }

            .search-bar input {
                flex: 1;
                padding: 6px 10px;
                border: 1px solid #ced4da;
                border-radius: 4px;
                font-size: 13px
            }

                .search-bar button, .search-bar input[type=submit] {
                    padding: 6px 14px;
                    background: #0062c5;
                    color: #fff;
                    border: none;
                    border-radius: 4px;
                    cursor: pointer;
                    font-size: 13px
                }

                    .search-bar button:hover {
                        background: #004a99
                    }

        .school-list {
            list-style: none;
            margin: 0;
            padding: 0;
            max-height: 380px;
            overflow-y: auto;
            border: 1px solid #dee2e6;
            border-radius: 4px;
            background: #fff
        }

            .school-list li {
                display: flex;
                align-items: center;
                justify-content: space-between;
                padding: 7px 12px;
                border-bottom: 1px solid #f0f0f0
            }

                .school-list li:last-child {
                    border-bottom: none
                }

                .school-list li:hover {
                    background: #e9f2ff
                }

        .school-name {
            flex: 1;
            font-size: 13px
        }

        .btn-add {
            padding: 3px 10px;
            background: #28a745;
            color: #fff;
            border: none;
            border-radius: 3px;
            cursor: pointer;
            font-size: 12px;
            white-space: nowrap
        }

            .btn-add:hover {
                background: #1e7e34
            }

        .btn-added {
            background: #6c757d !important;
            cursor: default
        }

        .paging {
            margin-top: 8px;
            display: flex;
            align-items: center;
            gap: 6px;
            font-size: 12px
        }

            .paging a {
                padding: 3px 10px;
                border: 1px solid #ced4da;
                background: #fff;
                border-radius: 3px;
                cursor: pointer;
                text-decoration: none;
                color: #333
            }

                .paging a:hover {
                    background: #e9ecef
                }

            .paging span {
                color: #495057
            }

        .footer-bar {
            margin-top: 10px;
            display: flex;
            justify-content: flex-end
        }

        .btn-close-popup {
            padding: 6px 16px;
            background: #dc3545;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 13px
        }

            .btn-close-popup:hover {
                background: #b02a37
            }

        .info-row {
            margin-bottom: 6px;
            font-size: 12px;
            color: #555
        }
    </style>
</head>
<body>
    <form id="frmChonTruong" runat="server">
        <div class="search-bar">
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Nhập tên trường..." />
            <asp:Button ID="btnSearch" runat="server" Text="Tìm" OnClick="btnSearch_Click" />
        </div>
        <div class="info-row">Tìm thấy: <strong>
            <asp:Label ID="lblTotal" runat="server" Text="0"></asp:Label></strong> trường</div>
        <asp:Repeater ID="rptSchools" runat="server">
            <HeaderTemplate>
                <ul class="school-list">
            </HeaderTemplate>
            <ItemTemplate>
                <li>
                    <span class="school-name"><%# Eval("NameofSchool") %></span>
                    <button type="button" class="btn-add"
                        data-id="<%# Eval("id") %>"
                        data-name="<%# Server.HtmlEncode(Eval("NameofSchool").ToString()) %>"
                        onclick="addSchool(this)">
                        + chọn</button>
                </li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater>
        <asp:Literal ID="ltrEmpty" runat="server"></asp:Literal>
        <div class="paging">
            <asp:LinkButton ID="lbtPrev" runat="server" OnClick="lbtPrev_Click">&lsaquo; trước</asp:LinkButton>
            <span>Trang <strong>
                <asp:Label ID="lblCurPage" runat="server" Text="1"></asp:Label></strong>
                / <strong>
                    <asp:Label ID="lblTotalPage" runat="server" Text="1"></asp:Label></strong></span>
            <asp:LinkButton ID="lbtNext" runat="server" OnClick="lbtNext_Click">tiếp &rsaquo;</asp:LinkButton>
        </div>
        <div class="footer-bar">
            <button type="button" class="btn-close-popup" onclick="window.close()">&#x2715; Đóng</button>
        </div>
    </form>
    <script type="text/javascript">
        function addSchool(btn) {
            var id = btn.getAttribute('data-id');
            var name = btn.getAttribute('data-name');
            if (window.opener && typeof window.opener.HandleSchoolResult === 'function') {
                window.opener.HandleSchoolResult(id, name);
            }
            btn.disabled = true;
            btn.className = 'btn-add btn-added';
            btn.textContent = '\u2713 đã chọn';
        }
</script>
</body>
</html>
