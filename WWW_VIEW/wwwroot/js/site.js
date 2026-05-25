/* site.js - Capstone Vietnam */
(function () {
    'use strict';

    /* =========================================================
       BACK TO TOP
    ========================================================= */
    var btnTop = document.getElementById('backToTop');
    if (btnTop) {
        window.addEventListener('scroll', function () {
            btnTop.style.display = window.scrollY > 300 ? 'flex' : 'none';
        });
        btnTop.addEventListener('click', function () {
            window.scrollTo({ top: 0, behavior: 'smooth' });
        });
    }

    /* =========================================================
       MOBILE MENU - copy nav into mobile-wrap
    ========================================================= */
    var mobileWrap = document.querySelector('.mobile-wrap .navbar-nav-list');
    var mainMenu   = document.querySelector('.navbar-nav');
    if (mobileWrap && mainMenu) {
        mobileWrap.innerHTML = mainMenu.innerHTML;
    }

    var btnMenu    = document.getElementById('buttonMenu');
    var mobileEl   = document.querySelector('.mobile-wrap');
    if (btnMenu && mobileEl) {
        btnMenu.addEventListener('click', function () {
            mobileEl.classList.toggle('is-open');
            document.body.classList.toggle('menu-open');
        });
    }

    /* =========================================================
       SEARCH TOGGLE
    ========================================================= */
    var btnSearch  = document.querySelector('.button-search');
    var searchWrap = document.querySelector('.search-wrap');
    if (btnSearch && searchWrap) {
        btnSearch.addEventListener('click', function () {
            searchWrap.classList.toggle('is-open');
            if (searchWrap.classList.contains('is-open')) {
                searchWrap.querySelector('input').focus();
            }
        });
        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') searchWrap.classList.remove('is-open');
        });
    }

    /* =========================================================
       MEGA MENU HOVER (desktop) / CLICK (mobile)
    ========================================================= */
    var megaItems = document.querySelectorAll('.has-mega-menu');
    megaItems.forEach(function (item) {
        var toggle = item.querySelector('.menu-toggle');
        if (toggle) {
            toggle.addEventListener('click', function (e) {
                e.stopPropagation();
                item.classList.toggle('is-open');
            });
        }
    });
    document.addEventListener('click', function () {
        megaItems.forEach(function (i) { i.classList.remove('is-open'); });
    });

    /* =========================================================
       COUNT UP ANIMATION
    ========================================================= */
    function animateCountUp(el) {
        var target = parseInt(el.getAttribute('data-count'), 10);
        var duration = 2000;
        var start = 0;
        var step = target / (duration / 16);
        var timer = setInterval(function () {
            start += step;
            if (start >= target) {
                el.textContent = target.toLocaleString('vi-VN');
                clearInterval(timer);
            } else {
                el.textContent = Math.floor(start).toLocaleString('vi-VN');
            }
        }, 16);
    }

    var countEls = document.querySelectorAll('.count-up');
    if (countEls.length && 'IntersectionObserver' in window) {
        var obs = new IntersectionObserver(function (entries) {
            entries.forEach(function (entry) {
                if (entry.isIntersecting && !entry.target.dataset.done) {
                    entry.target.dataset.done = '1';
                    animateCountUp(entry.target);
                }
            });
        }, { threshold: 0.3 });
        countEls.forEach(function (el) { obs.observe(el); });
    }

    /* =========================================================
       TIM TRUONG
    ========================================================= */
    window.timTruong = function () {
        var ten     = (document.getElementById('search-ten-truong') || {}).value || '';
        var quocGia = (document.getElementById('search-quoc-gia')   || {}).value || '';
        var bacHoc  = (document.getElementById('search-bac-hoc')    || {}).value || '';
        var nganh   = (document.getElementById('search-nganh-hoc')  || {}).value || '';
        var hocPhi  = (document.getElementById('search-hoc-phi')    || {}).value || '';
        var params  = new URLSearchParams({ ten: ten, quocgia: quocGia, bachoc: bacHoc, nganh: nganh, hocphi: hocPhi });
        window.location.href = '/tim-truong?' + params.toString();
    };

})();
