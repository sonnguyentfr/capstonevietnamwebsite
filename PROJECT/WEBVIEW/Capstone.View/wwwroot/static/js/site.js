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
        var url = '/api/Banner/click/' + encodeURIComponent(a.getAttribute('data-banner-id'));
        if (navigator.sendBeacon) {
            navigator.sendBeacon(url);
        } else {
            fetch(url, { method: 'POST', keepalive: true }).catch(function () { });
        }
    }
    document.addEventListener('click', trackBannerClick, true);
    document.addEventListener('auxclick', trackBannerClick, true);

})();