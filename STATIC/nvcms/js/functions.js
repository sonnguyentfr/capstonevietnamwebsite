function doSearchSite() {
    var e = "";
    "" != (e = document.getElementById("seach-box").value).toString() ? window.open("/tim-kiem?q=" + encodeURI(e), "_self") : alert("Nhập từ khóa tìm kiếm");
}
function doSearchSitem() {
    var e = "";
    "" != (e = document.getElementById("seach-boxm").value).toString() ? window.open("/tim-kiem?q=" + encodeURI(e), "_self") : alert("Nhập từ khóa tìm kiếm");
}
function checkKeypressSearchTop(e) {
    13 == (window.event ? event.keyCode : e.keyCode) && doSearchSite();
}
!(function (e) {
    "use strict";
    e(".loader-item").delay(700).fadeOut(),
        e("#pageloader").delay(1200).fadeOut("slow"),
        e(".extra-large-caption").fitText(1.5, { minFontSize: "26px", maxFontSize: "80px" }),
        e(".large-caption").fitText(1.5, { minFontSize: "26px", maxFontSize: "60px" }),
        e(".medium-caption").fitText(2, { minFontSize: "20px", maxFontSize: "30px" }),
        e(".small-caption").fitText(2.4, { minFontSize: "20px", maxFontSize: "26px" }),
        e(".extra-small-caption").fitText(2.4, { minFontSize: "16px", maxFontSize: "22px" }),
        e(".error-msg").fitText(2, { minFontSize: "36px", maxFontSize: "90px" }),
        e(".img-overlay1", "#parallax-section").parallax("100%", 0.8),
        e("#calendar").datepicker(),
        e("#fixed-navbar").headroom({ tolerance: 5, offset: e("#main-section").offset().top, classes: { pinned: "headroom-pinned", unpinned: "headroom-unpinned" } }),
        e("#fixed-navbar").affix({ offset: { top: e("#fixed-navbar").offset().top } }),
        e("[data-sidenav]").sidenav(),
        e(".navbar-toggle").attr("id", e("#sidenav-toggle").attr("id")),
        e(".navbar-togglemore").attr("id", e("#sidenav-togglemore").attr("id")),
        e("#mobile-nav").headroom({ offset: e("#main-section").offset().top, classes: { pinned: "headroom-pinned", unpinned: "headroom-unpinned" } }),
        e("#mobile-nav").affix({ offset: { top: e(".top-menu").height() } }),
        e(".newsfeed-1").easyTicker({ direction: "up", easing: "easeOutSine", speed: "slow", interval: 5e3, height: "auto", visible: 3, mousePause: 1 }),
        e(".newsfeed-2").easyTicker({ direction: "up", easing: "easeOutSine", speed: "slow", interval: 5e3, height: "auto", visible: 4, mousePause: 1 }),
        e(".newsfeed-3").easyTicker({ direction: "up", easing: "easeOutSine", speed: "slow", interval: 5e3, height: "auto", visible: 5, mousePause: 1 }),
        e(".newsfeed-4").easyTicker({ direction: "up", easing: "easeOutSine", speed: "slow", interval: 3e3, height: "auto", visible: 6, mousePause: 1 }),
        e(".newsfeed-5").easyTicker({ direction: "up", easing: "easeOutSine", speed: "slow", interval: 3e3, height: "auto", visible: 7, mousePause: 1 }),
        e(".newsfeed-6").easyTicker({ direction: "up", easing: "easeOutSine", speed: "slow", interval: 3e3, height: "auto", visible: 8, mousePause: 1 }),
        e(".newsticker").easyTicker({ direction: "up", easing: "easeOutSine", speed: "slow", interval: 4e3, height: "auto", visible: 1, mousePause: 1, controls: { up: ".up", down: ".down" } }),
        e().UItoTop({ easingType: "easeOutQuart" }),
        e("#news-slider,#sidebar-schedule-slider").owlCarousel({
            autoPlay: 5e3,
            stopOnHover: !0,
            navigation: !0,
            navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
            paginationSpeed: 1e3,
            goToFirstSpeed: 2e3,
            singleItem: !0,
            autoHeight: !0,
            transitionStyle: "fade",
        }),
        e("#news-sliderm").owlCarousel({ autoPlay: 5e3, stopOnHover: !0, navigation: !0, transitionStyle: "fade", loop: !0, navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"], items: 4 }),
        e("#news-homevanhoa").owlCarousel({ autoPlay: 5e3, stopOnHover: !0, navigation: !0, transitionStyle: "fade", loop: !0, navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"], items: 3 }),
        e("#big-gallery-slider-1").owlCarousel({ navigation: !0, navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"], items: 3 }),
        e("#big-gallery-slider-2").owlCarousel({ navigation: !0, navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"], items: 4 }),
        e("#big-gallery-slider-3").owlCarousel({ navigation: !0, navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"], items: 5 }),
        e("#topbanner").owlCarousel({ autoPlay: 5e3, stopOnHover: !0, navigation: !0, paginationSpeed: 1e3, goToFirstSpeed: 2e3, singleItem: !0, autoHeight: !0, transitionStyle: "fade" }),
        e("#footer-slider").owlCarousel({
            autoPlay: 5e3,
            paginationSpeed: 1e3,
            goToFirstSpeed: 2e3,
            stopOnHover: !0,
            navigation: !0,
            navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
            items: 7,
            itemsDesktop: [1400, 6],
            itemsDesktopSmall: [900, 3],
            itemsTablet: [600, 2],
            itemsMobile: !1,
        }),
        e("#small-gallery-slider").owlCarousel({
            navigation: !0,
            navigationText: ["<i class='fa-angle-left'></i>", "<i class='fa-angle-right'></i>"],
            items: 4,
            itemsDesktop: [1400, 3],
            itemsDesktopSmall: [900, 2],
            itemsTablet: [600, 1],
            itemsMobile: !1,
        }),
        (function e() {
            var t = new Date(),
                i = t.getDay(),
                a = t.getMonth(),
                o = t.getDate(),
                n = t.getFullYear(),
                s = t.getHours(),
                r = t.getMinutes(),
                l = t.getSeconds();
            s < 10 && (s = "0" + s), r < 10 && (r = "0" + r), l < 10 && (l = "0" + l);
            var d = ["Chủ nhật", "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7"][i] + ", ngày " + o + "/" + ["01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"][a] + "/" + n,
                p = s + ":" + r + ":" + l + " GMT+7";
            (document.getElementById("date").innerHTML = d), (document.getElementById("time").innerHTML = p), requestAnimationFrame(e);
        })(),
        e("#subscribeForm")
            .ketchup()
            .submit(function () {
                if (e(this).ketchup("isValid")) {
                    var t = e(this).attr("action");
                    e.ajax({
                        url: t,
                        type: "POST",
                        data: { email: e("#address").val() },
                        success: function (t) {
                            e("#result").html(t);
                        },
                        error: function () {
                            e("#result").html("Sorry, an error occurred.");
                        },
                    });
                }
                return !1;
            });
    function t(t) {
        return (function (t, i) {
            e.getJSON("https://api.openweathermap.org/data/2.5/weather", { APPID: "aaf8038825527fa05de584f4c6f2939a", lat: t.lat, lon: t.long, units: i }, function (t) {
                var a = { city: t.name, wind_speed: (2.2369362920544 * parseFloat(t.wind.speed)).toFixed(1), type: t.weather[0].main, humidity: t.main.humidity, temp: t.main.temp };
                return (
                    (function (t, i) {
                        e(".weather-city-text", "#weather").text(t.city), e("#type").text("Mây: "), e("#humidity").text("Độ ẩm: " + t.humidity + "%");
                        var a = "m/s";
                        "imperial" === i && (a = "mph");
                        e("#wind").text("Tốc độ gió: " + t.wind_speed + a),
                            e(".temperature", "#weather").text(t.temp),
                            (o = t.type),
                            (n = {
                                Clear: ["wi-day-sunny", "#CFD8DC"],
                                Rain: ["wi-rain", "#CFD8DC"],
                                Clouds: ["wi-cloudy", "#CFD8DC"],
                                Mist: ["wi-fog", "#CFD8DC"],
                                Thunderstorm: ["wi-thunderstorm", "#CFD8DC"],
                                Snow: ["wi-snow", "#CFD8DC"],
                            }),
                            e(".weather-icon", "#weather").removeClass("wi-day-sunny wi-rain wi-cloudy wi-fog wi-thunderstorm wi-snow").addClass(n[o][0]).css({ color: n[o][1] });
                        var o, n;
                    })(a, i),
                    a
                );
            });
        })({ lat: 20.54, long: 105.91 }, t);
    }
    e(document).ready(function () {
        t("metric");
    }),
        e(function () {
            e(".lazy").lazy({ scrollDirection: "vertical", effect: "fadeIn", visibleOnly: !0 }), e(".sticky").stick_in_parent({ offset_top: 10 });
        }),
        e(".video-container").fitVids(),
        e(".sidebar-scroll").mCustomScrollbar({
            setWidth: !1,
            setHeight: 600,
            setTop: 0,
            setLeft: 0,
            axis: "y",
            scrollbarPosition: "outside",
            scrollInertia: 950,
            autoDraggerLength: !0,
            autoHideScrollbar: !1,
            autoExpandScrollbar: !1,
            alwaysShowScrollbar: 0,
            snapAmount: null,
            snapOffset: 0,
            mouseWheel: { enable: !0, scrollAmount: 200, axis: "y", preventDefault: !1, deltaFactor: "auto", normalizeDelta: !0, invert: !1, disableOver: ["select", "option", "keygen", "datalist", "textarea"] },
            scrollButtons: { enable: !1, scrollType: "stepless", scrollAmount: "auto" },
            keyboard: { enable: !0, scrollType: "stepless", scrollAmount: "auto" },
            contentTouchScroll: 25,
            advanced: {
                autoExpandHorizontalScroll: !1,
                autoScrollOnFocus: "input,textarea,select,button,datalist,keygen,a[tabindex],area,object,[contenteditable='true']",
                updateOnContentResize: !0,
                updateOnImageLoad: !0,
                updateOnSelectorChange: !1,
                releaseDraggableSelectors: !1,
            },
            theme: "light",
            callbacks: {
                onInit: !1,
                onScrollStart: !1,
                onScroll: !1,
                onTotalScroll: !1,
                onTotalScrollBack: !1,
                whileScrolling: !1,
                onTotalScrollOffset: 0,
                onTotalScrollBackOffset: 0,
                alwaysTriggerOffsets: !0,
                onOverflowY: !1,
                onOverflowX: !1,
                onOverflowYNone: !1,
                onOverflowXNone: !1,
            },
            live: !1,
            liveSelector: null,
        }),
        jQuery(window).scroll(function () {
            if (jQuery(".sidebar-fixed").length) {
                if (parseInt(jQuery("#sidebar").height()) >= parseInt(jQuery(".left-content").height())) return jQuery(".sidebar-fixed.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px"), !1;
                var e = jQuery(".sidebar-fixed"),
                    t = jQuery("#wpadminbar").length > 0 ? 32 : 0;
                if (jQuery(window).scrollTop() + 10 + t >= e.offset().top) {
                    var i = parseInt(jQuery(window).scrollTop()) - parseInt(e.offset().top) + 10 + t,
                        a = parseInt(e.height()),
                        o = parseInt(i) + parseInt(e.offset().top);
                    parseInt(jQuery(".ketnoithuonghieuz").offset().top) <= a + o || e.addClass("is-now-fixed").css("paddingTop", i + "px");
                } else jQuery(".sidebar-fixed.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px");
            }
            if (jQuery(".sidebar-fixed2").length) {
                if (parseInt(jQuery("#sidebar").height()) >= parseInt(jQuery(".left-content").height())) return jQuery(".sidebar-fixed2.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px"), !1;
                (e = jQuery(".sidebar-fixed2")), (t = jQuery("#wpadminbar").length > 0 ? 32 : 0);
                if (jQuery(window).scrollTop() + 10 + t >= e.offset().top) {
                    (i = parseInt(jQuery(window).scrollTop()) - parseInt(e.offset().top) + 10 + t), (a = parseInt(e.height())), (o = parseInt(i) + parseInt(e.offset().top));
                    parseInt(jQuery(".module.dark").offset().top) <= a + o || e.addClass("is-now-fixed2").css("paddingTop", i + "px");
                } else jQuery(".sidebar-fixed2.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px");
            }
            if (jQuery(".sidebar-fixed3").length) {
                if (parseInt(jQuery("#sidebar").height()) >= parseInt(jQuery(".left-content").height())) return jQuery(".sidebar-fixed3.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px"), !1;
                (e = jQuery(".sidebar-fixed3")), (t = jQuery("#wpadminbar").length > 0 ? 32 : 0);
                if (jQuery(window).scrollTop() + 10 + t >= e.offset().top) {
                    (i = parseInt(jQuery(window).scrollTop()) - parseInt(e.offset().top) + 10 + t), (a = parseInt(e.height())), (o = parseInt(i) + parseInt(e.offset().top));
                    parseInt(jQuery(".videomoinhatz").offset().top) <= a + o || e.addClass("is-now-fixed2").css("paddingTop", i + "px");
                } else jQuery(".sidebar-fixed3.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px");
            }
            if (jQuery(".sidebar-fixed4").length) {
                if (parseInt(jQuery("#sidebar").height()) >= parseInt(jQuery(".left-content").height())) return jQuery(".sidebar-fixed4.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px"), !1;
                (e = jQuery(".sidebar-fixed4")), (t = jQuery("#wpadminbar").length > 0 ? 32 : 0);
                if (jQuery(window).scrollTop() + 10 + t >= e.offset().top) {
                    (i = parseInt(jQuery(window).scrollTop()) - parseInt(e.offset().top) + 10 + t), (a = parseInt(e.height())), (o = parseInt(i) + parseInt(e.offset().top));
                    parseInt(jQuery(".videomoinhatz").offset().top) <= a + o || e.addClass("is-now-fixed2").css("paddingTop", i + "px");
                } else jQuery(".sidebar-fixed4.is-now-fixed").removeClass("is-now-fixed").css("paddingTop", "0px");
            }
        });
})(jQuery);
function share_zing() { var u = location.href; window.open("http://link.apps.zing.vn/share?u=" + encodeURIComponent(u)); }
function share_linkhay() { var u = location.href; window.open("http://linkhay.com/submit?url=" + encodeURIComponent(u)); }
function share_twitter() { var u = location.href; t = document.title; window.open("http://twitter.com/home?status=" + encodeURIComponent(u)); }
function share_facebook() { var u = location.href; t = document.title; window.open("http://www.facebook.com/share.php?u=" + encodeURIComponent(u) + "&t=" + encodeURIComponent(t)); }
function share_google() { var u = location.href; t = document.title; window.open("http://www.google.com/bookmarks/mark?op=edit&bkmk=" + encodeURIComponent(u) + "&title=" + t + "&annotation=" + t); }