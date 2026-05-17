<%@ Control Language="vb" EnableViewState="false" AutoEventWireup="false" Explicit="true" CodeFile="CapV2_HomeTruongNoiBat.ascx.vb" Inherits="NVCMS.Modules.School.IndexSearch" %>
<style type="text/css">
    .slick_slider .col-lg-2-5 {
        padding: 0px 5px;
    }

    .truongnoibat .slick-arrow {
        position: absolute;
        top: 50%;
        background: unset;
        color: #4e4e4e !important;
        height: 60px;
        width: 60px;
    }

    .truongnoibat .slick-next {
        right: 0px
    }

    .truongnoibat .featured-imagebox .featured-thumbnail::before {
        position: absolute;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        background: rgba(254, 199, 34, 0.1);
        z-index: 1;
        content: "";
    }

    .truongnoibat .featured-content-post .featured-content-post-inner {
        min-height: 215px;
        padding: 20px 10px !important;
        background: #f0f0f0;
    }

        .truongnoibat .featured-content-post .featured-content-post-inner .post-desc {
            font-size: 13px;
        }

        .truongnoibat .featured-content-post .featured-content-post-inner .post-title h3 {
            font-size: 17px;
            font-weight: 600;
            line-height: 22px;
            color: #a81f25;
        }

    .section-title .scrach-btn-box .scrach-btn {
        background: #2e4076;
        border: none;
        font-size: 14px;
        border-radius: 0;
        color: #fff;
        font-weight: 400;
        margin-top: 40px;
        padding: 7px 40px;
        text-transform: uppercase;
        border-radius: 0px 5px;
        /*transform: skew(-20deg);*/
        border: solid 1px transparent;
    }

        .section-title .scrach-btn-box .scrach-btn.active {
            background: #b11116;
        }

        .section-title .scrach-btn-box .scrach-btn:hover {
            background: #b11116;
            box-shadow: 0px 0px 10px 2px #2e407678;
            border: solid 1px #fff;
        }

    .truongnoibat .slick-slide img {
        height: 147px;
        border: solid 1px #c3c3c3;
    }
</style>
<div class="col-lg-12 order-lg-12">
    <div class="section-title text-center  mb-20">
        <h2 class="title-clamp mb-20 uppercase font-bold"><a class="font-color" href="#">TRƯỜNG ĐỐI TÁC</a></h2>
        <p class="fs-20">
            Capstone chỉ hợp tác với các trường được kiểm định cấp khu vực (Kiểm định vàng) tại Mỹ,<br />
            và các trường được kiểm định chính thống tại các quốc gia khác
        </p>
        <div class="bar"></div>
    </div>
</div>
<div class="col-lg-12 order-lg-12">
    <div class="slider-5-items">
        <asp:Repeater ID="rptContent" runat="server">
            <ItemTemplate>
                <div class="featured-imagebox">
                    <div class="featured-thumbnail">
                        <a href='<%# Ultis.FormatLink_School(5585, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>'>
                        <img class="img-fluid lazyload blur-up" data-src="<%#Ultis.FormatThumbImage(Eval("Logo"), 230, 150, "constrain", "middlecenter", "") %>" src="/data/no-photo.png?width=230&height=150&mode=constrain&anchor=middlecenter" alt="<%#Eval("NameofSchool") %>">
                            </a>
                        <div class="ttm-box-post-date">
                            <span class="ttm-entry-date">Đối tác
                            </span>
                        </div>
                    </div>
                    <div class="featured-content">
                        <div class="featured-content-icon_img-block">
                            
                        </div>
                        <div class="featured-content-post-inner">
                            <div class="featured-title">
                                <a href='<%# Ultis.FormatLink_School(5585, CType(DataBinder.Eval(Container.DataItem, "id"), Integer), CType(DataBinder.Eval(Container.DataItem, "NameofSchool"), String)) %>'>
                                    <h3 class="text-limit-3-row"><%#Eval("NameofSchool") %></h3>
                                </a>
                            </div>
                            <div class="post-desc featured-desc">
                                <small> <%--<img class="img-fluid" src="https://duhocnamphong.vn/images/countries/original/us-fi_1554716808.png" alt="image">--%><%#Eval("StateCityName") %>, <%#Eval("CountryName") %></small>
                            </div>

                            <div class="post-desc featured-desc">
                                <ul class="list-unstyled">
                                    <li><b>Cấp bậc: </b><%#Eval("Loaitruongtext") %>
                                       <li>
                                    <li>
                                        <b>Chi phí</b>: <%#Gethocphi(Eval("id")) %>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>
</div>
