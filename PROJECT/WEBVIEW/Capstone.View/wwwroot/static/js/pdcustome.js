(function () {
    'use strict';

    pdfjsLib.GlobalWorkerOptions.workerSrc =
        '/static/js/pdf.worker.min.js';

    var docs = [];

    document.querySelectorAll('.fg-doc').forEach(function (el, idx) {
        docs.push({
            idx         : idx,
            url         : el.dataset.url,
            loaded      : false,
            pdfDoc      : null,
            currentPage : 1,
            totalPages  : 0,
            scale       : 1.4,
            rendering   : false,
            canvas  : document.getElementById('fg-canvas-'  + idx),
            spinner : document.getElementById('fg-spinner-' + idx),
            iframe  : document.getElementById('fg-iframe-'  + idx),
            curEl   : document.getElementById('fg-cur-'     + idx),
            totEl   : document.getElementById('fg-tot-'     + idx),
            prevBtn : document.getElementById('fg-prev-'    + idx),
            nextBtn : document.getElementById('fg-next-'    + idx),
            thumbContainer : document.getElementById('fg-thumbs-' + idx),
            stage   : document.getElementById('fg-stage-'   + idx),
            pageinfo: document.getElementById('fg-pageinfo-'+ idx)
        });
    });

    function updateNav(d) {
        if (d.prevBtn) d.prevBtn.disabled = d.currentPage <= 1;
        if (d.nextBtn) d.nextBtn.disabled = d.currentPage >= d.totalPages;
    }

    function setActiveThumb(d, pageNum) {
        if (!d.thumbContainer) return;
        d.thumbContainer.querySelectorAll('.fg-thumb-item').forEach(function (t, i) {
            t.classList.toggle('is-active', i === pageNum - 1);
        });
        var active = d.thumbContainer.children[pageNum - 1];
        if (active) active.scrollIntoView({ block: 'nearest', behavior: 'smooth' });
    }

    function renderPage(d, pageNum) {
        if (!d.pdfDoc || d.rendering) return;
        d.rendering    = true;
        d.currentPage  = pageNum;
        if (d.curEl) d.curEl.textContent = pageNum;
        setActiveThumb(d, pageNum);
        d.canvas.classList.remove('is-visible');
        if (d.spinner) d.spinner.classList.remove('fg-hidden');

        d.pdfDoc.getPage(pageNum).then(function (page) {
            var vp = page.getViewport({ scale: d.scale });
            d.canvas.width  = vp.width;
            d.canvas.height = vp.height;
            return page.render({ canvasContext: d.canvas.getContext('2d'), viewport: vp }).promise;
        }).then(function () {
            d.rendering = false;
            d.canvas.classList.add('is-visible');
            if (d.spinner) d.spinner.classList.add('fg-hidden');
            updateNav(d);
        }).catch(function () {
            d.rendering = false;
            if (d.spinner) d.spinner.classList.add('fg-hidden');
        });
    }

    function renderThumb(d, pageNum, itemEl) {
        if (!d.pdfDoc) return;
        d.pdfDoc.getPage(pageNum).then(function (page) {
            var vp = page.getViewport({ scale: 0.22 });
            var c  = itemEl.querySelector('canvas');
            c.width  = vp.width;
            c.height = vp.height;
            page.render({ canvasContext: c.getContext('2d'), viewport: vp });
        });
    }

    function buildThumbs(d) {
        if (!d.thumbContainer) return;
        d.thumbContainer.innerHTML = '';
        for (var p = 1; p <= d.totalPages; p++) {
            (function (pageNum) {
                var item = document.createElement('div');
                item.className = 'fg-thumb-item' + (pageNum === 1 ? ' is-active' : '');
                var c   = document.createElement('canvas');
                var num = document.createElement('span');
                num.className   = 'fg-thumb-num';
                num.textContent = pageNum;
                item.appendChild(c);
                item.appendChild(num);
                item.addEventListener('click', function () { renderPage(d, pageNum); });
                d.thumbContainer.appendChild(item);

                if ('IntersectionObserver' in window) {
                    var obs = new IntersectionObserver(function (entries) {
                        entries.forEach(function (e) {
                            if (!e.isIntersecting) return;
                            renderThumb(d, pageNum, item);
                            obs.unobserve(item);
                        });
                    }, { root: d.thumbContainer.parentElement, rootMargin: '300px' });
                    obs.observe(item);
                } else {
                    renderThumb(d, pageNum, item);
                }
            }(p));
        }
    }

    function showIframeFallback(d) {
        if (d.spinner) d.spinner.classList.add('fg-hidden');
        d.canvas.classList.add('fg-hidden');
        if (d.iframe) {
            d.iframe.src = d.url;
            d.iframe.classList.remove('fg-hidden');
        }
        if (d.prevBtn) d.prevBtn.style.display  = 'none';
        if (d.nextBtn) d.nextBtn.style.display  = 'none';
        if (d.pageinfo) d.pageinfo.style.display = 'none';
        var sidebar = d.thumbContainer ? d.thumbContainer.closest('.fg-sidebar') : null;
        if (sidebar) sidebar.style.display = 'none';
    }

    function loadDoc(d) {
        if (d.loaded) return;
        d.loaded = true;
        if (d.spinner) d.spinner.classList.remove('fg-hidden');

        pdfjsLib.getDocument({
            url            : d.url,
            cMapUrl        : 'https://cdnjs.cloudflare.com/ajax/libs/pdf.js/3.11.174/cmaps/',
            cMapPacked     : true,
            withCredentials: false
        }).promise.then(function (pdf) {
            d.pdfDoc     = pdf;
            d.totalPages = pdf.numPages;
            if (d.totEl) d.totEl.textContent = pdf.numPages;
            buildThumbs(d);
            renderPage(d, 1);
        }).catch(function () {
            showIframeFallback(d);
        });
    }

    /* tab switching */
    var tabBtns = document.querySelectorAll('.fg-doc-tab');
    tabBtns.forEach(function (btn) {
        btn.addEventListener('click', function () {
            var idx = parseInt(btn.dataset.idx, 10);
            tabBtns.forEach(function (b) { b.classList.remove('is-active'); });
            btn.classList.add('is-active');
            document.querySelectorAll('.fg-doc').forEach(function (el) {
                el.classList.remove('is-active');
            });
            var target = document.getElementById('fg-doc-' + idx);
            if (target) target.classList.add('is-active');
            if (docs[idx]) loadDoc(docs[idx]);
        });
    });

    /* prev / next */
    docs.forEach(function (d) {
        if (d.prevBtn) {
            d.prevBtn.addEventListener('click', function () {
                if (d.currentPage > 1) renderPage(d, d.currentPage - 1);
            });
        }
        if (d.nextBtn) {
            d.nextBtn.addEventListener('click', function () {
                if (d.currentPage < d.totalPages) renderPage(d, d.currentPage + 1);
            });
        }
    });

    /* zoom */
    document.querySelectorAll('.js-zoom-in').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var d = docs[parseInt(btn.dataset.idx, 10)];
            if (!d || !d.pdfDoc) return;
            d.scale = Math.min(d.scale + 0.25, 4);
            renderPage(d, d.currentPage);
        });
    });
    document.querySelectorAll('.js-zoom-out').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var d = docs[parseInt(btn.dataset.idx, 10)];
            if (!d || !d.pdfDoc) return;
            d.scale = Math.max(d.scale - 0.25, 0.5);
            renderPage(d, d.currentPage);
        });
    });

    /* fullscreen */
    document.querySelectorAll('.js-fullscreen').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var shell = document.querySelector('#fg-doc-' + parseInt(btn.dataset.idx, 10) + ' .fg-shell');
            if (!shell) return;
            if (!document.fullscreenElement) {
                (shell.requestFullscreen || shell.webkitRequestFullscreen).call(shell);
            } else {
                (document.exitFullscreen || document.webkitExitFullscreen).call(document);
            }
        });
    });

    /* keyboard */
    document.addEventListener('keydown', function (e) {
        var active = docs.find(function (d) {
            var el = document.getElementById('fg-doc-' + d.idx);
            return el && el.classList.contains('is-active');
        });
        if (!active || !active.pdfDoc) return;
        if (e.key === 'ArrowRight' || e.key === 'ArrowDown') {
            if (active.currentPage < active.totalPages) renderPage(active, active.currentPage + 1);
        } else if (e.key === 'ArrowLeft' || e.key === 'ArrowUp') {
            if (active.currentPage > 1) renderPage(active, active.currentPage - 1);
        }
    });

    /* touch swipe */
    docs.forEach(function (d) {
        var stage = d.stage;
        if (!stage) return;
        var sx = 0;
        stage.addEventListener('touchstart', function (e) {
            sx = e.changedTouches[0].clientX;
        }, { passive: true });
        stage.addEventListener('touchend', function (e) {
            var dx = e.changedTouches[0].clientX - sx;
            if (Math.abs(dx) < 40 || !d.pdfDoc) return;
            if (dx < 0 && d.currentPage < d.totalPages) renderPage(d, d.currentPage + 1);
            if (dx > 0 && d.currentPage > 1)            renderPage(d, d.currentPage - 1);
        }, { passive: true });
    });

    /* ctrl+wheel zoom */
    docs.forEach(function (d) {
        var stage = d.stage;
        if (!stage) return;
        stage.addEventListener('wheel', function (e) {
            if (!e.ctrlKey && !e.metaKey) return;
            e.preventDefault();
            if (!d.pdfDoc) return;
            d.scale += e.deltaY < 0 ? 0.15 : -0.15;
            d.scale  = Math.min(Math.max(d.scale, 0.5), 4);
            renderPage(d, d.currentPage);
        }, { passive: false });
    });

    /* auto-load first active doc */
    docs.forEach(function (d) {
        var el = document.getElementById('fg-doc-' + d.idx);
        if (el && el.classList.contains('is-active')) loadDoc(d);
    });

}());