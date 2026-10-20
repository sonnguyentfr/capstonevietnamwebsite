/* site.js - Capstone Vietnam */
(function () {
    'use strict';

    /* =========================================================
       AOS (Animate On Scroll) - main.min.js khong co, phai init o day
    ========================================================= */
    if (typeof AOS !== 'undefined') {
        AOS.init({
            duration: 800,
            once: true,
            offset: 80
        });
    }

    /* =========================================================
       TIM TRUONG - chuyen den trang tim truong voi query params
    ========================================================= */
    window.timTruong = function () {
        var ten = (document.getElementById('search-ten-truong') || {}).value || '';
        var quocGia = (document.getElementById('search-quoc-gia') || {}).value || '';
        var bacHoc = (document.getElementById('search-bac-hoc') || {}).value || '';
        var nganh = (document.getElementById('search-nganh-hoc') || {}).value || '';
        var hocPhi = (document.getElementById('search-hoc-phi') || {}).value || '';
        var params = new URLSearchParams({
            ten: ten,
            quocgia: quocGia,
            bachoc: bacHoc,
            nganh: nganh,
            hocphi: hocPhi
        });
        window.location.href = '/tim-truong?' + params.toString();
    };

    /* =========================================================
       BANNER CLICK - dem click banner (a[data-banner-id])
       sendBeacon khong chan dieu huong; auxclick = click chuot giua
    ========================================================= */
    function trackBannerClick(e) {
        if (e.type === 'auxclick' && e.button !== 1) return;
        var a = e.target.closest ? e.target.closest('a[data-banner-id]') : null;
        if (!a) return;
        postBeacon('/api/Banner/click/' + encodeURIComponent(a.getAttribute('data-banner-id')));
    }
    document.addEventListener('click', trackBannerClick, true);
    document.addEventListener('auxclick', trackBannerClick, true);

    /* =========================================================
       BANNER VIEW - dem luot xem banner ([data-banner-view])
       Tinh 1 lan / banner / lan tai trang khi hien >= 50% trong viewport
       (slide swiper chua hien thi thi chua tinh). Gom id, gui 1 beacon.
    ========================================================= */
    function postBeacon(url) {
        if (navigator.sendBeacon) {
            navigator.sendBeacon(url);
        } else {
            fetch(url, { method: 'POST', keepalive: true }).catch(function () { });
        }
    }

    var bannerViewSeen = {};
    var bannerViewQueue = [];
    var bannerViewTimer = null;

    function flushBannerViews() {
        bannerViewTimer = null;
        if (!bannerViewQueue.length) return;
        postBeacon('/api/Banner/view?ids=' + encodeURIComponent(bannerViewQueue.join(',')));
        bannerViewQueue = [];
    }

    function queueBannerView(id) {
        if (!id || bannerViewSeen[id]) return;
        bannerViewSeen[id] = true;
        bannerViewQueue.push(id);
        if (!bannerViewTimer) bannerViewTimer = setTimeout(flushBannerViews, 1500);
    }

    var bannerEls = document.querySelectorAll('[data-banner-view]');
    if (bannerEls.length && 'IntersectionObserver' in window) {
        var bannerObserver = new IntersectionObserver(function (entries) {
            entries.forEach(function (en) {
                if (!en.isIntersecting) return;
                queueBannerView(en.target.getAttribute('data-banner-view'));
                bannerObserver.unobserve(en.target);
            });
        }, { threshold: 0.5 });
        bannerEls.forEach(function (el) { bannerObserver.observe(el); });
        document.addEventListener('visibilitychange', function () {
            if (document.visibilityState === 'hidden') flushBannerViews();
        });
    }

})();