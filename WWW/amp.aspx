<%@ Page Language="C#" AutoEventWireup="true" CodeFile="amp.aspx.cs" Inherits="feeds" %>
<%@ Register TagPrefix="dnn" TagName="BREADCRUMB" Src="~/Admin/Skins/BreadCrumb.ascx" %>
<!doctype html>
<html amp lang="vi">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,minimum-scale=1,maximum-scale=1,initial-scale=1">
    <meta name="mobile-web-app-capable" content="yes">
    <meta name="theme-color" content="#1e1e1e">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="google-site-verification" content="0puVjKfWCMhTxZejTqrwxRMMfPlk1XNT1kCgYIzGcHc" />
    <meta name="google-site-verification" content="ahnTjZ-WDo7TPqfHzMT1TmQ2JQ29aNMf3rkEsurF_6M" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Roboto:300,500">
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Roboto+Slab:100,300,400,700&display=swap">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.6.3/css/font-awesome.min.css">
    <script async custom-element="amp-install-serviceworker" src="https://cdn.ampproject.org/v0/amp-install-serviceworker-0.1.js"></script>
    <script async custom-element="amp-social-share" src="https://cdn.ampproject.org/v0/amp-social-share-0.1.js"></script>
    <script async custom-element="amp-iframe" src="https://cdn.ampproject.org/v0/amp-iframe-0.1.js"></script>
    <script async custom-element="amp-carousel" src="https://cdn.ampproject.org/v0/amp-carousel-0.1.js"></script>
    <script async custom-element="amp-sidebar" src="https://cdn.ampproject.org/v0/amp-sidebar-0.1.js"></script>
    <script async custom-element="amp-accordion" src="https://cdn.ampproject.org/v0/amp-accordion-0.1.js"></script>
    <script async custom-element="amp-youtube" src="https://cdn.ampproject.org/v0/amp-youtube-0.1.js"></script>
    <script async custom-element="amp-analytics" src="https://cdn.ampproject.org/v0/amp-analytics-0.1.js"></script>
    <script async custom-element="amp-twitter" src="https://cdn.ampproject.org/v0/amp-twitter-0.1.js"></script>
    <script async custom-element="amp-ad" src="https://cdn.ampproject.org/v0/amp-ad-0.1.js"></script>
    <script async custom-element="amp-video" src="https://cdn.ampproject.org/v0/amp-video-0.1.js"></script>
    <script async src="https://cdn.ampproject.org/v0.js"></script>
    <style amp-boilerplate>
        body {
            -webkit-animation: -amp-start 8s steps(1,end) 0s 1 normal both;
            -moz-animation: -amp-start 8s steps(1,end) 0s 1 normal both;
            -ms-animation: -amp-start 8s steps(1,end) 0s 1 normal both;
            animation: -amp-start 8s steps(1,end) 0s 1 normal both;
        }

        @-webkit-keyframes -amp-start {
            from {
                visibility: hidden;
            }

            to {
                visibility: visible;
            }
        }

        @-moz-keyframes -amp-start {
            from {
                visibility: hidden;
            }

            to {
                visibility: visible;
            }
        }

        @-ms-keyframes -amp-start {
            from {
                visibility: hidden;
            }

            to {
                visibility: visible;
            }
        }

        @-o-keyframes -amp-start {
            from {
                visibility: hidden;
            }

            to {
                visibility: visible;
            }
        }

        @keyframes -amp-start {
            from {
                visibility: hidden;
            }

            to {
                visibility: visible;
            }
        }
    </style>
    <noscript>
        <style amp-boilerplate>
            body {
                -webkit-animation: none;
                -moz-animation: none;
                -ms-animation: none;
                animation: none;
            }
        </style>
    </noscript>
    <style amp-custom>
        figure {
            margin: 0;
        }

        * {
            box-sizing: border-box;
        }

        button {
            background: 0 0;
            border: none;
        }

        a {
            text-decoration: none;
        }

        :focus {
            outline: 0;
        }

        ul {
            padding-left: 20px;
        }

        html {
            font-size: 62.5%;
            box-sizing: border-box;
        }

        body {
            font-size: 1.3rem;
            line-height: 1.8;
            -webkit-font-smoothing: antialiased;
            color: #818181;
        }

        .font-1, html {
            font-family: Roboto,serif;
            font-weight: 300;
        }

        .text-center {
            text-align: center;
        }

        .margin-0 {
            margin: 0;
        }

        .margin-top-0 {
            margin-top: 0;
        }

        .margin-bottom-0 {
            margin-bottom: 0;
        }

        .minus-margin-top-bottom-15 {
            margin-top: -15px;
            margin-bottom: -15px;
        }

        .space {
            height: 10px;
        }

        .space-2 {
            height: 20px;
        }

        .space-3 {
            height: 30px;
        }

        .divider {
            margin: 13px 0;
        }

        .divider-30 {
            margin: 30px 0;
        }

        .divider.colored {
            height: 1px;
            background: rgba(0,0,0,.12);
        }

        .divider-30.colored {
            height: 1px;
            background: rgba(0,0,0,.12);
        }

        .pull-left {
            float: left;
        }

        .pull-right {
            float: right;
        }

        .clearfix:after, .clearfix:before {
            display: table;
            content: "";
            line-height: 0;
        }

        .clearfix:after {
            clear: both;
        }

        h2 {
            margin-bottom: 7.5px;
        }

        p {
            margin: 7.5px 0 0;
        }

        small {
            font-size: 1rem;
            line-height: 1;
        }

        b, strong {
            font-weight: 500;
        }

        h1, h2, h3, h4, h5, h6 {
            font-weight: 500;
            color: #414141;
        }

        h1 {
            font-size: 2.7rem;
        }

        h2 {
            font-size: 1.9rem;
        }

        h3 {
            font-size: 1.7rem;
        }

        h4 {
            font-size: 1.5rem;
        }

        h5 {
            font-size: 1.3rem;
        }

        h6 {
            font-size: 1rem;
        }

        .primary-color, a {
            color: #5782C9;
        }

        .secondary-color {
            color: #442672;
        }

        .light-color {
            color: #FFF;
        }

        .light-color-2 {
            color: rgba(255,255,255,.54);
        }

        .dark-color {
            color: #333030;
        }

        .ocean-color {
            color: #2b90d9;
        }

        .grass-color {
            color: #3ac569;
        }

        .salmon-color {
            color: #ff7473;
        }

        .sun-color {
            color: #feee7d;
        }

        .alge-color {
            color: #79a8a9;
        }

        .flower-color {
            color: #353866;
        }

        .primary-bg {
            background-color: #ed1b24;
        }

        .secondary-bg {
            background-color: #442672;
        }

        .light-bg {
            background-color: #fff;
        }

        .dark-bg {
            background-color: #333030;
        }

        .ocean-bg {
            background-color: #2b90d9;
        }

        .grass-bg {
            background-color: #3ac569;
        }

        .salmon-bg {
            background-color: #ff7473;
        }

        .sun-bg {
            background-color: #feee7d;
        }

        .alge-bg {
            background-color: #79a8a9;
        }

        .flower-bg {
            background-color: #353866;
        }

        .circle {
            border-radius: 50%;
        }

        [dir=rtl] .pull-left {
            float: right;
        }

        [dir=rtl] .pull-right {
            float: left;
        }

        body {
            text-align: left;
        }

            body[dir=rtl] {
                text-align: right;
            }

        .text-center {
            text-align: center;
        }

        code {
            padding: .2rem .4rem;
            font-size: 90%;
            color: #bd4147;
            background-color: #f7f7f9;
            border-radius: .25rem;
        }

        .topheader {
            background: #ed1b24;
            padding: 5px;
            color: #fff;
            clear: both;
            display: block;
            margin: 0 auto;
            height: 33px;
        }

            .topheader .topsoclai a.social-item {
                color: #fff;
                padding: 0 5px;
                font-size: 15px;
            }

        header {
            position: relative;
            min-height: 55px;
            padding: 0 5px;
            background: #ffffff;
            box-shadow: 0 0 4px #69181c;
        }

            header .fa {
                color: #ed1c24;
                opacity: .87;
                font-size: 17px;
                line-height: 56px;
                height: 55px;
                padding: 0 15px;
                margin: 0;
            }

        #logo {
            height: 35px;
            line-height: 61px;
            display: inline-block;
            padding-top: 12px;
        }

        #mainSideBar {
            min-width: 300px;
            padding-bottom: 30px;
        }

            #mainSideBar > div:not(.divider) {
                padding: 17px 20px;
            }

            #mainSideBar figure {
                width: 300px;
                max-width: 100%;
                padding: 20px;
                position: relative;
            }

            #mainSideBar button {
                position: absolute;
                right: 20px;
                top: 20px;
            }

            #mainSideBar amp-img {
                margin-bottom: 5px;
            }

            #mainSideBar h3, #mainSideBar h5 {
                margin: 0;
                line-height: 1.5;
            }

        #menu {
            margin-top: 15px;
        }

            #menu div {
                padding: 0;
            }

            #menu a, #menu h6 {
                color: inherit;
                font-size: 1.3rem;
                font-weight: 300;
                padding: 0;
                border: none;
            }

            #menu a, #menu span {
                padding: 14px 20px 14px 53px;
                display: block;
                color: #000;
                font-weight: 500;
                position: relative;
                -webkit-transition: all ease-in-out .2s;
                transition: all ease-in-out .2s;
            }

            #menu section[expanded] > h6 span {
                background-color: rgba(0,0,0,.06);
                color: #5782C9;
            }

            #menu h6 span:after {
                position: absolute;
                right: 20px;
                top: 0;
                font-family: FontAwesome;
                font-size: 12px;
                line-height: 47px;
                content: '\f0dd';
            }

            #mainSideBar li i, #menu i {
                font-size: 1.7rem;
                position: absolute;
                left: 20px;
            }

        .social-ball {
            font-size: 1.6rem;
            display: inline-block;
            text-align: center;
            line-height: 30px;
            height: 30px;
            width: 30px;
            border-radius: 50%;
            color: #FFF;
            margin-right: 5px;
        }

            .social-ball.fa-facebook {
                background-color: #4867AA;
            }

            .social-ball.fa-twitter {
                background-color: #00ACED;
            }

            .social-ball.fa-linkedin {
                background-color: #0177B5;
            }

            .social-ball.fa-behance {
                background-color: #010103;
            }

            .social-ball.fa-dribbble {
                background-color: #E04C86;
            }

        [class*=col-] {
            margin-bottom: 30px;
        }

        .container-fluid {
            padding-right: 15px;
            padding-left: 15px;
            margin-right: auto;
            margin-left: auto;
        }

        .row {
            margin-right: -15px;
            margin-left: -15px;
        }

            .row:after, .row:before {
                display: table;
                content: " ";
            }

            .row:after {
                clear: both;
            }

        .container-full, .container-full [class*=col-] {
            padding-left: 0;
            padding-right: 0;
        }

            .container-full .row {
                margin-left: 0;
                margin-right: 0;
            }

        .no-gap [class*=col-] {
            padding-right: 0;
            padding-left: 0;
            margin-bottom: 0;
        }

        .no-gap.row {
            margin-right: 0;
            margin-left: 0;
        }

        .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9, .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {
            position: relative;
            min-height: 1px;
            padding-right: 15px;
            padding-left: 15px;
        }

        .col-xs-1, .col-xs-10, .col-xs-11, .col-xs-12, .col-xs-2, .col-xs-3, .col-xs-4, .col-xs-5, .col-xs-6, .col-xs-7, .col-xs-8, .col-xs-9 {
            float: left;
        }

        .col-xs-12 {
            width: 100%;
        }

        .col-xs-11 {
            width: 91.66666667%;
        }

        .col-xs-10 {
            width: 83.33333333%;
        }

        .col-xs-9 {
            width: 75%;
        }

        .col-xs-8 {
            width: 66.66666667%;
        }

        .col-xs-7 {
            width: 58.33333333%;
        }

        .col-xs-6 {
            width: 50%;
        }

        .col-xs-5 {
            width: 41.66666667%;
        }

        .col-xs-4 {
            width: 33.33333333%;
        }

        .col-xs-3 {
            width: 25%;
        }

        .col-xs-2 {
            width: 16.66666667%;
        }

        .col-xs-1 {
            width: 8.33333333%;
        }

        @media (min-width:768px) {
            .col-sm-1, .col-sm-10, .col-sm-11, .col-sm-12, .col-sm-2, .col-sm-3, .col-sm-4, .col-sm-5, .col-sm-6, .col-sm-7, .col-sm-8, .col-sm-9 {
                float: left;
            }

            .col-sm-12 {
                width: 100%;
            }

            .col-sm-11 {
                width: 91.66666667%;
            }

            .col-sm-10 {
                width: 83.33333333%;
            }

            .col-sm-9 {
                width: 75%;
            }

            .col-sm-8 {
                width: 66.66666667%;
            }

            .col-sm-7 {
                width: 58.33333333%;
            }

            .col-sm-6 {
                width: 50%;
            }

            .col-sm-5 {
                width: 41.66666667%;
            }

            .col-sm-4 {
                width: 33.33333333%;
            }

            .col-sm-3 {
                width: 25%;
            }

            .col-sm-2 {
                width: 16.66666667%;
            }

            .col-sm-1 {
                width: 8.33333333%;
            }
        }

        .blog-item h1 {
            font-size: 26px;
            line-height: 1.2;
            color: #000;
            font-weight: 700;
        }

        .blog-item .preview {
            display: block;
            margin-bottom: 15px;
        }

        .subtitle {
            font-size: 1.2rem;
        }

        .blog-sidebar-box {
            margin-bottom: 30px;
        }

        .photo-row {
            margin: 15px -2.5px 0;
        }

            .photo-row a {
                width: 33.33333333333333%;
                padding: 0 2.5px;
                float: left;
                display: block;
            }

        .media-list {
            list-style: none;
            padding: 0;
            border-bottom: 1px solid rgba(0,0,0,.06);
        }

            .media-list a {
                position: relative;
                display: block;
            }

            .media-list i {
                position: absolute;
                right: 5px;
                top: 50%;
                line-height: 10px;
                margin-top: -17px;
                display: block;
            }

            .media-list amp-img {
                display: block;
            }

            .media-list div {
                margin-left: 80px;
                margin-right: 13px;
            }

            .media-list li {
                border-bottom: 1px solid rgba(0,0,0,.06);
                padding-bottom: 8px;
                margin-bottom: 10px;
            }

            .media-list h4 {
                line-height: 18px;
                font-size: 13px;
            }

        .bordered-list {
            padding-left: 0;
            list-style: none;
        }

            .bordered-list a {
                color: inherit;
                display: block;
                position: relative;
                padding: 10px 15px 8px 0;
                border-bottom: 1px solid rgba(0,0,0,.06);
            }

                .bordered-list a:after {
                    position: absolute;
                    right: 5px;
                    top: 0;
                    font-size: 12px;
                    line-height: 47px;
                    font-weight: 500;
                    content: '+';
                }

        .social-share-container {
            height: 30px;
        }

        .socials-share-title {
            line-height: 30px;
            display: inline-block;
            vertical-align: top;
            margin-right: 10px;
        }

        amp-social-share[type=email], amp-social-share[type=facebook], amp-social-share[type=gplus], amp-social-share[type=linkedin], amp-social-share[type=pinterest], amp-social-share[type=twitter] {
            background-image: none;
        }

        amp-social-share[type=whatsapp] {
            background-color: #189D0E;
        }

        amp-social-share[type=baidu] {
            background-color: #4252A2;
        }

        amp-social-share {
            font-size: 1.6rem;
            display: inline-block;
            text-align: center;
            line-height: 30px;
            height: 30px;
            width: 30px;
            border-radius: 50%;
            color: #FFF;
            margin-right: 5px;
        }

        .comment-item > h4, .comment-item > small {
            line-height: 1.5;
        }

        .comment-item > h4 {
            margin-top: 0;
            margin-bottom: 10px;
        }

        .comment-item {
            margin-bottom: 30px;
        }

            .comment-item > small a {
                float: right;
            }

            .comment-item > small span {
                float: left;
            }

            .comment-item.child {
                margin-left: 30px;
            }

        h3 + .comment-item {
            margin-top: 15px;
        }

        .comment-item > small div.stars {
            float: right;
        }

            .comment-item > small div.stars i {
                float: left;
                font-size: 1.4rem;
            }

        .details, .details p {
            font-size: 15px;
            color: #222222;
            font-family: Roboto Slab,Arial;
            -webkit-font-smoothing: antialiased;
            font-weight: 400;
        }

        .detailstags {
            border-top: solid 1px #eee;
            padding: 5px 0px;
            border-bottom: solid 1px #eee;
        }

            .detailstags a {
                background: #538aefc7;
                margin: 3px;
                color: #fff;
                border-radius: 5px;
                padding: 3px 5px;
                display: inline-block;
            }

        .detailsmore {
            font-size: 12px;
            color: #222222;
            font-family: Roboto Slab,Arial;
            -webkit-font-smoothing: antialiased;
            font-weight: 400;
            background: #d4d4d459;
            padding: 10px;
        }

            .detailsmore a {
                font-weight: 500;
                color: #ed1b24;
            }

        .boxtags {
        }

            .boxtags h3.tags {
                font-size: 15px;
                line-height: 16px;
            }

                .boxtags h3.tags a {
                    color: #c70009;
                }

            .boxtags .taglbableis {
                display: none;
            }

        .details .tinlienquantrongbaidoctrai {
            border: solid 1px #ccc;
            background: #ececec;
            margin: 0px 10px 5px 0px;
            padding: 5px 3px;
            width: 150px;
            float: left;
        }

            .details .tinlienquantrongbaidoctrai .tinlienquantrongbaitd1 {
            }

                .details .tinlienquantrongbaidoctrai .tinlienquantrongbaitd1 figure {
                }

            .details .tinlienquantrongbaidoctrai .tinlienquantrongbaitd2 {
            }

                .details .tinlienquantrongbaidoctrai .tinlienquantrongbaitd2 h4 {
                    margin: 5px 0px;
                    font-size: 12px;
                    font-weight: 600;
                    line-height: 18px;
                }

                .details .tinlienquantrongbaidoctrai .tinlienquantrongbaitd2 p {
                    display: none;
                }

        .details figure.easyimage {
            background: #f9f9f9;
            border: solid 1px #d6d6d6;
        }

        .details figcaption, .details figcaption p {
            padding: 5px 7px;
            font-size: 12px;
        }
        .details.summary strong {font-weight:600;}
    </style>
</head>
<body dir="ltr">
    <div class="topheader">
        <div class="text-center topsoclai">
            <a class="social-item" href="https://www.facebook.com/thuongtruong.com.vn/" target="_blank">
                <i class="fa fa-facebook"></i>
            </a>
            <a class="social-item">
                <i class="fa fa-google-plus"></i>
            </a>
            <a class="social-item">
                <i class="fa fa-youtube"></i>
            </a>
            <a class="social-item">
                <i class="fa fa-twitter"></i>
            </a>
            <a class="social-item" href="tel:+84913398394">
                <i class="fa fa-phone"></i>: 0913398394
            </a>
        </div>
    </div>
    <header itemscope itemtype="https://schema.org/WPHeader">
        <a class="pull-left fa fa-bars" on="tap:mainSideBar.toggle"></a>
        <a id="logo" href="/">
            <amp-img src="https://cdn.thuongtruong.com.vn/nvcms/img/logo.png?v=2.3.1" width="235" height="35"></amp-img>
        </a>
    </header>
    <div class="container-fluid">
        <div class="space-2"></div>
        <div class="row">
            <div class="col-sm-12">
                <div class="blog-item clearfix">
                    <div class="quangcaogg">
                        <amp-ad layout="fixed" width="300" height="250" type="adsense" data-ad-client="ca-pub-3311450421751656" data-ad-slot="3967719266"></amp-ad>
                    </div>
                    <h1 class="margin-0">
                        <asp:Literal ID="lbltilte" runat="server"></asp:Literal></h1>
                    <div class="subtitle">
                        <i class="fa fa-folder-open-o"></i>
                        <asp:Literal ID="ltrcat" runat="server"></asp:Literal>
                        <i class="fa fa-clock-o"></i>
                        <asp:Literal ID="ltrcreatedate" runat="server"></asp:Literal>
                        <i class="fa  fa-pencil"></i>
                        <asp:Literal ID="ltrbutdanh" runat="server"></asp:Literal>
                    </div>
                    <div class="divider colored"></div>
                    <%--<div class="preview">
                        <asp:Literal ID="ltravatar" runat="server"></asp:Literal>
                    </div>--%>
                    <div class="space"></div>
                    <div class="details summary">
                        <strong>
                            <asp:Literal ID="ltrsumary" runat="server"></asp:Literal></strong>
                    </div>
                    <div class="quangcaogg">
                        <amp-ad layout="fixed" width="300" height="250" type="adsense" data-ad-client="ca-pub-3311450421751656" data-ad-slot="1557178346"></amp-ad>
                    </div>
                    <div class="divider colored"></div>
                    <div class="details">

                        <p>
                            <asp:Literal ID="ltrcontent" runat="server"></asp:Literal>
                        </p>
                    </div>
                    <div class="detailstags">
                        <asp:Literal ID="ltrtag" runat="server"></asp:Literal>
                    </div>
                    <div class="quangcaogg">
                        <amp-ad layout="fixed" width="300" height="250" type="adsense" data-ad-client="ca-pub-3311450421751656" data-ad-slot="2739780962"></amp-ad>
                    </div>
                    <div class="detailsmore">
                        Bạn đang đọc bài viết
                        <asp:Literal ID="ltrchitietbaititle" runat="server"></asp:Literal>
                        tại chuyên mục
                        <asp:Literal ID="ltrchitietbaicat" runat="server"></asp:Literal>
                        của <a href="http://thuongtruong.com.vn/">Tạp chí Điện tử Thương Trường</a>. Mọi thông tin góp ý và chia sẻ, xin vui lòng liên hệ SĐT: 0913398394  hoặc gửi về hòm thư <a href="#">toasoanthuongtruong@gmail.com</a>
                    </div>

                    <%--<div class="divider-30 colored"></div>
                    <div class="social-share-container"><strong class="socials-share-title">Chia sẻ bài viết:</strong><amp-social-share type="facebook" width="30" height="30" layout="fixed" data-param-text="Hello world" data-param-href="<sc" data-param-app_id="145634995501895"><i class="fa fa-facebook"></i></amp-social-share><amp-social-share type="twitter" width="30" height="30" layout="fixed"><i class="fa fa-twitter"></i></amp-social-share><amp-social-share type="linkedin" width="30" height="30" layout="fixed"><i class="fa fa-linkedin"></i></amp-social-share><amp-social-share type="baidu" width="30" height="30" layout="fixed" data-share-endpoint="http://cang.baidu.com/do/add" data-param-iu="CANONICAL_URL" data-param-it="TITLE">B</amp-social-share><amp-social-share type="whatsapp" width="30" height="30" layout="fixed" data-share-endpoint="whatsapp://send" data-param-text="Check out this article: TITLE - CANONICAL_URL"><i class="fa fa-whatsapp"></i></amp-social-share></div>--%>
                </div>
                <div class="quangcaogg">
                    <amp-ad layout="fixed" width="300" height="250" type="adsense" data-ad-client="ca-pub-3311450421751656" data-ad-slot="6013140478"></amp-ad>
                </div>
                <div class="divider-30 colored"></div>
                <div class="blog-sidebar-box">
                    <h3 class="margin-0">Tin cùng chuyên mục</h3>
                    <ul class="media-list">
                        <asp:Repeater ID="rptlienquan" runat="server">
                            <ItemTemplate>
                                <li><a class="clearfix" href="https://www.google.com/amp/s/m.thuongtruong.com.vn/amp/<%#Ultis.BuildEntryLink(Convert.ToInt32(Eval("NewId")), Convert.ToString(Eval("Title")).ToLower()) %>.html">
                                    <amp-img src="<%# Ultis.FormatThumbImage(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImagePath")), 60, 60, "crop", "middlecenter", "") %>" layout="fixed" width="60" height="60" class="pull-left circle"></amp-img>
                                    <div>
                                        <h4 class="margin-0"><%# DataBinder.Eval(Container.DataItem,"title")%></h4>
                                        <span>
                                            <%# BL.FormatDate(Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"PublishedDate")))%>
                                        </span>
                                    </div>
                                    <i class="fa fa-angle-right"></i></a></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
                <div class="quangcaogg">
                    <amp-ad layout="fixed" width="300" height="250" type="adsense" data-ad-client="ca-pub-3311450421751656" data-ad-slot="2375435991"></amp-ad>
                </div>
                <div class="blog-sidebar-box boxtags">
                    <asp:Repeater ID="rptContenttags" runat="server" OnItemDataBound="rptContenttagsItemDataBound">
                        <ItemTemplate>
                            <h3 class="margin-0 tags" runat="server" visible="<%# Checkhien(Convert.ToString(Container.DataItem))%>"><a href="/tags.html?tag=<%# ReplaceChuoi.bodau2(Convert.ToString(Container.DataItem))%>">#<%# Convert.ToString(Container.DataItem)%></h3>
                            <asp:Label ID="tagid" CssClass="taglbableis" runat="server" Text='<%# ReplaceChuoi.bodau2(Convert.ToString(Container.DataItem))%>' />
                            <ul class="media-list" visible="<%# Checkhien(Convert.ToString(Container.DataItem))%>" runat="server">
                                <asp:Repeater ID="rptListNewsCatHot" runat="server">
                                    <ItemTemplate>
                                        <li><a class="clearfix" href="https://www.google.com/amp/s/m.thuongtruong.com.vn/amp/<%#Ultis.BuildEntryLink(Convert.ToInt32(Eval("NewId")), Convert.ToString(Eval("Title")).ToLower()) %>.html">
                                            <amp-img src="<%# Ultis.FormatThumbImage(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImagePath")), 60, 60, "crop", "middlecenter", "") %>" layout="fixed" width="60" height="60" class="pull-left circle"></amp-img>
                                            <div>
                                                <h4 class="margin-0">
                                                    <a href="https://www.google.com/amp/s/m.thuongtruong.com.vn/amp/<%#Ultis.BuildEntryLink(Convert.ToInt32(Eval("NewId")), Convert.ToString(Eval("Title")).ToLower()) %>.html">
                                                    <%# DataBinder.Eval(Container.DataItem,"title")%></a></h4>
                                                <span>
                                                    <%# BL.FormatDate(Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"PublishedDate")))%>
                                                </span>
                                            </div>
                                            <i class="fa fa-angle-right"></i></a></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>

                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <div class="quangcaogg">
                    <amp-ad layout="fixed" width="300" height="250" type="adsense" data-ad-client="ca-pub-3311450421751656" data-ad-slot="6013140478"></amp-ad>
                </div>
                <div class="bordered-title">
                    <h3>TIN MỚI CẬP NHẬT</h3>
                </div>
                <div class="tinlienquan">
                    <asp:Repeater ID="rptmoinhat" runat="server">
                        <ItemTemplate>
                            <div class="blog-item clearfix">
                                <a href="https://www.google.com/amp/s/m.thuongtruong.com.vn/amp/<%#Ultis.BuildEntryLink(Convert.ToInt32(Eval("NewId")), Convert.ToString(Eval("Title")).ToLower()) %>.html" class="preview">
                                    <amp-img layout="responsive" src="<%# Ultis.FormatThumbImage(Convert.ToString(DataBinder.Eval(Container.DataItem,"ImagePath")), 690, 388, "crop", "middlecenter", "") %>" width="690" height="388" class="responsive">
                                        </amp-img>
                                </a><a href="<%# Convert.ToInt32(Eval("NewId")) %>">
                                    <h3 class="margin-0"><%# DataBinder.Eval(Container.DataItem,"title")%></h3>
                                </a>
                                <div class="subtitle">
                                    <i class="fa fa-folder-open-o"></i><a href="https://www.google.com/amp/s/m.thuongtruong.com.vn/amp/<%#Ultis.BuildEntryLink(Convert.ToInt32(Eval("NewId")), Convert.ToString(Eval("Title")).ToLower()) %>.html"></a>
                                    &nbsp|&nbsp;<i class="fa fa-clock-o"></i> <%# BL.FormatDate(Convert.ToDateTime( DataBinder.Eval(Container.DataItem,"PublishedDate")))%>
                                </div>
                                <div class="space"></div>
                                <div class="details">
                                    <p><%# DataBinder.Eval(Container.DataItem,"Summary")%></p>
                                </div>
                            </div>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <div class="divider-30 colored"></div>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid">
        <div class="text-center">
            <p class="boxed-text"><strong>CQCQ: HIỆP HỘI CÁC NHÀ BÁN LẺ VIỆT NAM</strong></p>
            <p class="boxed-text">Tổng biên tập: Nhà báo Đinh Thị Mỹ Vân</p>
            <p class="boxed-text">Phụ trách điện tử: Tổng TKTS - Nhà báo <strong>Đinh Duy Hùng</strong></p>
            <p class="boxed-text">Tòa soạn: Tầng 6 tòa nhà số 8 Trần Đăng Ninh - P. Dịch vọng - Q. Cầu Giấy - Hà Nội</p>
            <p class="boxed-text">Giấy phép hoạt động báo chí điện tử số 75/GP-BTTT, do Bộ Thông tin &amp; Truyền thông cấp ngày: 23/02/2017</p>
            <p class="boxed-text">Điện thoại: +84 24 3734 9773 / Fax: +84 24 3939 3770</p>
            <p class="boxed-text">Đường dây nóng: 0913398394 </p>
            <p class="boxed-text">Email: toasoanthuongtruong@gmail.com</p>
            <p class="boxed-text">Liên hệ quảng cáo: 0888484455</p>
        </div>
        <div class="divider colored"></div>

        <div class="space-2"></div>
        <div class="text-center"><a href="#" class="social-ball fa fa-facebook"></a><a href="#" class="social-ball fa fa-twitter"></a><a href="#" class="social-ball fa fa-linkedin"></a><a href="#" class="social-ball fa fa-behance"></a><a href="#" class="social-ball fa fa-dribbble"></a></div>
        <div class="space"></div>
        <div class="text-center"><small>© Copyright 2017 - Mọi hình thức sao chép phải được sự chấp thuận bằng văn bản của Tạp chí Điện tử Thương Trường</small></div>
        <div class="space-2"></div>
    </div>
    <amp-sidebar id="mainSideBar" layout="nodisplay"><figure class="primary-bg">
        <figcaption>
            <h3 class="light-color">DANH MỤC</h3>
        <button on="tap:mainSideBar.toggle" class="fa fa-caret-left light-color"></button></figure>
        <nav id="menu" itemscope itemtype="http://schema.org/SiteNavigationElement">
            <a href="/" ><i class="fa fa-star-o"></i>Trang chủ</a>
		    <a href="/tin-tuc" ><i class="fa fa-star-o"></i>Tin tức</a>
            <a href="/thi-truong" ><i class="fa fa-star-o"></i>Thị trường</a>
            <a href="/hiep-hoi-va-doanh-nghiep" ><i class="fa fa-star-o"></i>Hiệp hội và doanh nghiệp</a>
            <a href="/kinh-te" ><i class="fa fa-star-o"></i>Kinh tế</a>
            <a href="/doi-song-tieu-dung" ><i class="fa fa-star-o"></i>Đời sống và tiêu dùng</a>
             <a href="chinh-sach-va-phap-luat" ><i class="fa fa-star-o"></i>Chính sách - Pháp luật</a>
            <a href="/thong-tin-hay-lua-chon-tot" ><i class="fa fa-star-o"></i>Thông tin hay lựa chọn tốt</a>
            <a href="/goc-nhin" ><i class="fa fa-star-o"></i>Góc nhìn</a>
            <a href="/video" ><i class="fa fa-star-o"></i>Video</a>
            
            </nav>
        <div class="divider colored"></div>
        </amp-sidebar>
    <amp-analytics type="googleanalytics" id="analytics1"><asp:Literal ID="ltrga" runat="server"></asp:Literal></amp-analytics>
</body>
</html>
