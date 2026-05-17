<%@ Control Language="vb" EnableViewState="true" AutoEventWireup="false" Explicit="true"
    CodeFile="DetailHightSchool.ascx.vb" Inherits="DesktopModules.TinTuc.Display.News.DetailHighSchool" %>
<style type="text/css">
    .icontitle {
        position: absolute;
        top: 0px;
        right: 0px;
        background: #f7d636db;
        border-radius: 0px 0px 0px 7px;
        padding: 2px 6px;
        color: #fff;
        font-size: 11px;
    }

    .list-school {
    }

        .list-school .postlist {
            padding: 10px 15px;
            background-color: #fff;
            -webkit-box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
            box-shadow: 0 0 1.25rem rgb(108 118 134 / 10%);
            font-size: 13px;
            border: 1px #e5e5e5 solid;
            margin-bottom: 20px;
            position: relative;
            min-height: 390px;
        }

            .list-school .postlist:hover {
                box-shadow: 0 0 1.25rem rgb(108 118 134 / 35%);
                border: 1px #dbdbdb solid;
            }

            .list-school .postlist .post-title {
            }

                .list-school .postlist .post-title h2 {
                    font-size: 20px;
                    font-weight: 500;
                    letter-spacing: -0.8px;
                    padding: 20px 0px 10px 0px;
                }

            .list-school .postlist .post-entry p {
                text-overflow: ellipsis;
                position: relative;
                white-space: nowrap;
                overflow: hidden;
                margin-bottom: 5px;
            }

    .schooldetail h1 {
        font-size: 50px;
        font-weight: 500;
        letter-spacing: 0px;
        margin-bottom: 21px;
        text-shadow: 2px 2px 0px #cfcfcf;
        color: #b11116;
        border-bottom: solid 1px #004c92;
    }

    .word-icon {
        font-family: "Helvetica", sans-serif;
        font-size: 24px;
        font-weight: bold;
        background-color: #0054a6;
        color: white;
        padding: 2px 5px;
        vertical-align: middle;
    }

    .upnewstd1 {
        width: 150px;
    }

    .upnewstd1, .upnewstd2 {
        padding: 8px 0px;
        border-bottom: solid 1px #eee;
    }

    .upnewstd2 {
        padding-left: 10px !important;
        width: 400px !important;
    }

    select, input[type="file"] {
        height: 40px;
    }

    .tienganh {
        background: #81c9f5;
        font-weight: bold;
        color: #fff;
    }

    .ui-corner-all, .ui-corner-top, .ui-corner-left, .ui-corner-tl {
        border-radius: 0px;
    }

    .ui-widget-header {
        background: none;
    }

    .ui-tabs .ui-tabs-nav li a {
        float: left;
        padding: 5px 7px;
        text-decoration: none;
    }

    .ui-tabs .ui-tabs-nav li {
        margin: 0;
    }

    .PartnershipStatus td {
        padding: 5px 10px;
        border: 1px solid #c2c2c2;
        margin-right: 10px;
    }

        .PartnershipStatus td input {
            height: 18px;
            width: 18px;
            margin-right: 5px;
        }

    #sortableschool2 li, #sortableschool li {
        display: block;
        background: none;
        border: solid 1px #8c8c8c;
        min-height: 32px;
    }

        #sortableschool2 li img, #sortableschool li img {
            float: left;
            padding-right: 5px;
        }

    p {
        font-size: 13px;
        line-height: 1.4em;
        margin: 0px;
        padding: 0px;
    }

    .NameofSchool i {
        font-size: 24px;
        padding: 5px;
    }

    .inforbasic p {
        font-size: 13px;
        line-height: 1.8em;
        margin: 2px;
        padding: 2px;
    }


    .tablemaina {
        width: 100%;
        vertical-align: top;
        padding: 0px;
        margin: 0px;
        border: solid 1px #d6d6d6;
    }



        .tablemaina input {
            font-size: 20px;
            color: blue;
            font-weight: bold;
        }

    ::-webkit-input-placeholder { /* WebKit, Blink, Edge */
        color: #808080;
        font-size: 12px;
        font-weight: normal;
    }

    .appdeadline {
        width: 110px;
    }

    .pustyle .StandardButton {
        margin: 5px 10px;
        padding: 8px;
        color: #fff;
    }

    .pustyle .red {
        background-color: #d9534f;
        border-color: #d43f3a;
    }

    .pustyle .blue {
        background-color: #1eac13;
        border-color: #1eac13;
    }

    .toolbarBox {
        background-color: #F8F8F8;
        border: 1px solid #007236;
        padding: 3px 0;
        /* border-radius: 3px; */
        -moz-border-radius: 0px;
        -webkit-border-radius: 0px;
    }

        .toolbarBox ul.cc_button li {
            color: #666666;
            float: left;
            height: auto;
            list-style: none outside none;
            padding: 0px;
            text-align: center;
        }

    .truongmajor {
        border: solid 1px #d6d6d6;
    }

        .truongmajor tr td {
            border: solid 1px #d6d6d6;
            padding: 5px;
            text-align: center;
        }

            .truongmajor tr td:first-child {
                border: solid 1px #d6d6d6;
                padding: 5px;
                text-align: right !important;
                padding-right: 10px;
            }

        .truongmajor th {
            border: solid 1px #d6d6d6;
            padding: 7px;
            text-align: center;
            background: #e4e4e4;
        }

            .truongmajor th span {
                color: red;
                font-size: 20px;
                font-weight: bold;
            }

    #iufnotruong .red {
        background-color: #d9534f;
        border-color: #d43f3a;
    }

    #iufnotruong .StandardButton {
        margin: 5px 10px;
        padding: 8px;
        color: #fff;
    }

    .none {
        display: none;
    }

    .Maincontact {
        border: solid 2px Red !important;
    }

    .fairguide_logo {
        padding: 20px;
        border: solid 3px #242973;
    }

        .fairguide_logo img {
            max-width: 100%;
        }

        .fairguide_logo ul.social {
            text-align: center;
            margin: 0 auto;
        }

            .fairguide_logo ul.social li {
                list-style-type: none;
                display: inline;
                padding: 10px;
            }

                .fairguide_logo ul.social li i {
                    color: #fff;
                    background: #242973;
                    border-radius: 50%;
                    height: 30px;
                    width: 30px;
                    padding: 5px;
                    font-size: 20px;
                }

                    .fairguide_logo ul.social li i:hover {
                        color: #242973;
                        background: #e4e4e4;
                        border-radius: 50%;
                        height: 30px;
                        width: 30px;
                        padding: 5px;
                        font-size: 20px;
                    }

    .fairguide_thongtin {
        text-align: center;
        padding: 20px 0px;
    }

        .fairguide_thongtin p {
            margin-bottom: 5px;
        }

    .fairguide_cover {
    }

        .fairguide_cover img {
            width: 100%;
            max-width: 100%;
        }

    .fairguide_thongtinchung {
        margin: 30px 0px;
    }

        .fairguide_thongtinchung ul {
            margin: 0px;
            padding: 0px;
        }

            .fairguide_thongtinchung ul li {
                list-style-type: none;
                display: inline-block;
                padding: 10px;
                text-align: center;
            }

    .fairguide_tomtatgioithieu {
        position: relative;
    }

        .fairguide_tomtatgioithieu blockquote {
            border: 0px;
            padding: 20px 50px;
            margin: 20px;
            font-size: 17px;
            line-height: 22px;font-style: unset;
        }

            .fairguide_tomtatgioithieu blockquote p {
                font-size: 14px;
                line-height: 23px;
                font-weight: 500;font-style: unset;
            }

            .fairguide_tomtatgioithieu blockquote:before {
                display: block;
                padding-left: 10px;
                content: "\201C";
                font-size: 113px;
                position: absolute;
                left: 13px;
                top: 43px;
            }

    .fairguide_gioithieu p {
        font-size: 17px;
        margin-bottom: 10px;
    }

    .fairguide_tuyensinh {
        padding: 20px;
        margin: 0px 15px;
        border: solid 3px #242973;
    }

        .fairguide_tuyensinh .fairguide_tuyensinhcontent {
            border-right: solid 2px #242973;
            padding: 0px 10px;
            min-height: 200px;
        }

            .fairguide_tuyensinh .fairguide_tuyensinhcontent:last-child {
                border-right: 0px;
            }

    @media (min-width: 320px) and (max-width: 480px) {
        .fairguide_tuyensinh {
            padding: 20px;
            margin: 10 auto;
            border: solid 3px #242973;
            width: 90%;
        }

            .fairguide_tuyensinh .fairguide_tuyensinhcontent {
                border-right: 0px;
                min-height: auto;
                font-size: 13px;
                color: #242973;
            }

        .fairguide_thongtinchung ul li {
            width: 33.333333% !important;
        }

        .list-school .postlist .news-image img {
            width: 100%;
            margin-bottom: 10px;
        }

        .list-school .postlist .post-title h2 {
            font-size: 20px;
        }
    }
</style>
<div class="row">
    <div class="col-lg-12">
        <h1>
            <asp:Literal ID="ltrtentruong" runat="server"></asp:Literal></h1>
    </div>
    <div class="col-lg-4 col-md-6">
        <div class="fairguide_logo">
            <div id="dvPreviewlogo2" runat="server"></div>
            <ul class="social">
                <asp:Literal ID="ltrsocical" runat="server"></asp:Literal>
            </ul>
        </div>
        <div class="fairguide_thongtin">
            <p>
                <strong>Tên trường: </strong>
                <asp:Literal ID="txtTitleview" runat="server"></asp:Literal>
            </p>
            <p>
                <strong>Năm thành lập: </strong>
                <asp:Literal ID="txtnamthanhlap" runat="server"></asp:Literal>
            </p>
            <p>
                <strong>Vị trí: </strong>
                <asp:Literal ID="txtvitri" runat="server"></asp:Literal>
            </p>
            <p>
                <strong>Địa chỉ:</strong><asp:Literal ID="txtdiachia" runat="server"></asp:Literal>
            </p>
            <p>
                <strong>Loại trường: </strong>
                <asp:Literal ID="txtloaitruongtext" runat="server"></asp:Literal>
            </p>
            <p>
                <strong>Chương trình học: </strong>
                <asp:Literal ID="ProgramOffered" runat="server"></asp:Literal>
            </p>
            <p>
                <strong>Website: </strong>
                <asp:Literal ID="txtwebsite" runat="server"></asp:Literal>
            </p>
        </div>
    </div>
    <div class="col-lg-8 col-md-6">
        <div class="fairguide_cover">
            <div id="dvPreviewcover" runat="server" style="margin: 0 auto; text-align: center;"></div>
        </div>
    </div>
    <div class="col-lg-12 col-md-12">
        <div class="fairguide_thongtinchung">
            <ul id="thongtincoicon">
                <asp:Literal ID="ltrthongtinchung" runat="server"></asp:Literal>
            </ul>
        </div>
    </div>
    <div class="col-lg-12 col-md-12">
        <div class="fairguide_tomtatgioithieu">
            <blockquote>
                <asp:Literal ID="txtthongtintomtat" runat="server"></asp:Literal>
            </blockquote>
        </div>
    </div>
    <div class="col-lg-12 col-md-12">
        <div class="fairguide_gioithieu">
            <asp:Literal ID="txtthongtinEN" runat="server"></asp:Literal>
        </div>
    </div>
    <div class="row fairguide_tuyensinh">
        <div class="col-lg-3 col-md-6 fairguide_tuyensinhcontent">
            <p><strong>Hạn nộp hồ sơ: </strong></p>
            <ul>
                <asp:Literal ID="ltrhannophoso" runat="server"></asp:Literal>
            </ul>
            <p>
                <strong>Nhận học sinh từ: </strong>
                <asp:Literal ID="ltrnhanhocsinhtu" runat="server"></asp:Literal>
            </p>

        </div>
        <div class="col-lg-3 col-md-6 fairguide_tuyensinhcontent">
            <p>
                <strong>Yêu cầu tối thiểu tiếng Anh: </strong>
                <asp:Literal ID="ltryeucautienganh" runat="server"></asp:Literal>
            </p>
            <p>
                <strong>Học bổng: </strong>
                <asp:Literal ID="ltrhocbong" runat="server"></asp:Literal>
            </p>
        </div>
        <div class="col-lg-3 col-md-6 fairguide_tuyensinhcontent">
            <p>
                <strong>Chi phí: </strong>
                <asp:Literal ID="ltrchiphi" runat="server"></asp:Literal>
            </p>
        </div>
        <div class="col-lg-3 col-md-6 fairguide_tuyensinhcontent">
            <p>
                <strong>Top 5 trường ĐH hàng đầu học sinh nhập học sau khi tốt nghiệp: </strong>
                <asp:Literal ID="ltrtoptruongdaihoc" runat="server"></asp:Literal>
            </p>

        </div>
    </div>
</div>
<div class="section-title text-center mt-30">
    <h2>Danh sách các trường khác</h2>
    <div class="bar"></div>
</div>
<div class="list-school row">
    <asp:Repeater ID="rptContent" runat="server">
        <ItemTemplate>
            <div class="col-lg-3">
                <div class="postlist">
                    <div class="news-image">
                        <a href='<%# Ultis.FormatLink_School(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>'>
                            <img class="responsive-img" src="<%#Ultis.FormatThumbImage(Eval("Logo"), 310, 209, "constrain", "middlecenter", "") %>" alt="news Image" onerror="LoadImage(this, '/data/noimage.png?width=310&height=209&mode=constrain&anchor=middlecenter')" />
                        </a>
                        <span class="icontitle">
                            <i class="fa fa-clock-o"></i><%#BL.FormatDate(Eval("CreatedDate"))%>
                            <asp:Label Font-Bold="true" ForeColor="#0093ff" ID="lblPartner" runat="server" Text="<i class='fa fa-check-square' aria-hidden='true'></i>"></asp:Label>
                        </span>
                    </div>
                    <div class="post-title">
                        <h2>
                            <a href='<%# Ultis.FormatLink_School(PortalSettings.ActiveTab.TabID, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>'>
                                <%#Eval("NameofSchool") %></a>
                        </h2>
                    </div>
                    <div class="post-entry">
                        <p>Năm thành lập: <%#Eval("Namthanhlap") %></p>
                        <p>Website: <%#Eval("Website") %></p>
                        <p><%# Ultis.SubString(Eval("Descreption"), 20, "..") %></p>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    <div class="cl"></div>
</div>
<script type="text/javascript">
    var countp = $("#thongtincoicon").find('li').length;
    var width2 = 100 / countp;
    $('#thongtincoicon li').css("width", width2 + "%");
</script>
