/* NVCMS.TinTuc - man soan bai (newsedit) giao dien v3: thu gon the, tab khung phai, keo tha tep, o pass bai viet.
   Chi la lop giao dien, khong dung toi du lieu / luong luu bai. */
(function () {
    'use strict';
    var KEY = 'ne.ed.side';

    function root() { return document.querySelector('.ne-v3'); }

    function showSide(tab) {
        var r = root(); if (!r) return;
        tab = tab === 'media' ? 'media' : 'info';
        var tabs = r.querySelectorAll('[data-ne-side]');
        for (var i = 0; i < tabs.length; i++) {
            var on = tabs[i].getAttribute('data-ne-side') === tab;
            tabs[i].classList.toggle('active', on);
            tabs[i].setAttribute('aria-selected', on ? 'true' : 'false');
        }
        var panes = r.querySelectorAll('[data-side-pane]');
        for (var j = 0; j < panes.length; j++) {
            panes[j].classList.toggle('is-off', panes[j].getAttribute('data-side-pane') !== tab);
        }
        try { window.localStorage.setItem(KEY, tab); } catch (e) { /* bo qua */ }
    }
    window.neShowSide = showSide;

    function toggleCard(card) {
        var collapsed = card.classList.toggle('is-collapsed');
        var b = card.querySelector('[data-ne-toggle]');
        if (b) b.setAttribute('aria-expanded', collapsed ? 'false' : 'true');
    }

    function init() {
        var r = root(); if (!r) return;

        r.addEventListener('click', function (e) {
            var t = e.target;
            var tog = t.closest ? t.closest('[data-ne-toggle]') : null;
            if (tog) { e.preventDefault(); toggleCard(tog.closest('.tt-card')); return; }
            var side = t.closest ? t.closest('[data-ne-side]') : null;
            if (side) { e.preventDefault(); showSide(side.getAttribute('data-ne-side')); return; }
            var eye = t.closest ? t.closest('[data-ne-eye]') : null;
            if (eye) {
                e.preventDefault();
                var inp = r.querySelector('.ne-pass');
                if (inp) inp.type = inp.type === 'password' ? 'text' : 'password';
            }
        });

        var saved = 'info';
        try { saved = window.localStorage.getItem(KEY) || 'info'; } catch (e) { /* bo qua */ }
        showSide(saved);

        // Keo tha tep vao khung tai len: gan vao the input co san roi kich hoat su kien change (luong upload cu giu nguyen)
        var drop = r.querySelector('.tt-drop');
        var input = r.querySelector('[data-side-pane="media"] input[type=file]');
        if (drop && input) {
            ['dragenter', 'dragover'].forEach(function (n) {
                drop.addEventListener(n, function (e) { e.preventDefault(); drop.classList.add('is-over'); });
            });
            ['dragleave', 'drop'].forEach(function (n) {
                drop.addEventListener(n, function (e) { e.preventDefault(); drop.classList.remove('is-over'); });
            });
            drop.addEventListener('drop', function (e) {
                var f = e.dataTransfer && e.dataTransfer.files;
                if (!f || !f.length) return;
                try {
                    input.files = f;
                    var ev = document.createEvent('HTMLEvents');
                    ev.initEvent('change', true, false);
                    input.dispatchEvent(ev);
                } catch (err) { /* trinh duyet cu: dung nut chon tep */ }
            });
            // chon tep xong thi mo tab Anh & tep de thay ket qua upload
            input.addEventListener('change', function () {
                showSide('media');
                var mp = r.querySelector('.tt-mp');
                if (mp) { mp.classList.remove('is-flash'); void mp.offsetWidth; mp.classList.add('is-flash'); }
            });
        }
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();

/* ============================================================================
   Dan / keo tha anh vao noi dung: tu dong upload + dua vao danh sach anh, tep.
   Dan tu Google Docs / Word / web: lam sach style (giu co ban), upload lai anh.
   Dung lai handler upload cu (UploadImage.ashx) nen khong doi du lieu / luong luu bai.
   Can window.NE_CFG = { editor: <id textarea CKEditor>, itemId: <id hdf_itemid>, msg: <id lblMessage> } do .ascx khai bao.
   ============================================================================ */
(function () {
    'use strict';
    var UP = '/DesktopModules/NVCMS.TinTuc/Manager/Services/UploadImage.ashx';
    var UP_URL = '/DesktopModules/NVCMS.TinTuc/Manager/Services/UploadImageUrl.ashx';
    var seq = 0;

    function cfg() { return window.NE_CFG || null; }
    // bat log: trong Console go  localStorage.setItem('ne.debug','1')  roi tai lai trang
    function dbg() { try { if (window.localStorage.getItem('ne.debug') === '1') console.log.apply(console, ['[ne-paste]'].concat([].slice.call(arguments))); } catch (e) { /* bo qua */ } }
    function itemId() { var c = cfg(), h = c && document.getElementById(c.itemId); return (h && h.value) || '0'; }
    function say(ok, msg) { var f = ok ? window.notifySuccess : window.notifyError; if (typeof f === 'function') { try { f(msg); } catch (e) { /* bo qua */ } } }
    function extOf(t) { return ({ 'image/png': 'png', 'image/jpeg': 'jpg', 'image/jpg': 'jpg', 'image/gif': 'gif', 'image/webp': 'webp', 'image/bmp': 'bmp' })[(t || '').toLowerCase()] || 'png'; }
    function isImageFile(f) { return f && /^image\/(png|jpe?g|gif|webp|bmp)$/i.test(f.type || ''); }

    // ------------------------------------------------------------------ upload
    function post(url, body) {
        return fetch(url, { method: 'POST', body: body, credentials: 'same-origin', headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.text();
        });
    }
    // handler tra ve <li> co data-img='<url anh>'; them li do vao danh sach anh/tep nhu luong upload cu
    function finish(html) {
        var m = /data-img='([^']+)'/.exec(html || '');
        if (!m) throw new Error('Khong nhan duoc duong dan anh');
        var c = cfg();
        if (window.jQuery && c && c.msg) {
            window.jQuery('#' + c.msg).append(html);
            window.jQuery('.anh-addToContent').off();
            if (typeof window.ActionImage === 'function') window.ActionImage();
        }
        return m[1];
    }
    function uploadBlob(blob, ext) {
        var name = 'dan-anh-' + Date.now() + '-' + (++seq) + '.' + ext;
        var fd = new FormData();
        fd.append(name, blob, name);
        return post(UP + '?itemid=' + encodeURIComponent(itemId()), fd).then(finish);
    }
    function uploadRemote(src) {
        var fd = new FormData();
        fd.append('url', src);
        return post(UP_URL + '?itemid=' + encodeURIComponent(itemId()), fd).then(finish);
    }
    function dataUrlToBlob(s) {
        var i = s.indexOf(','), head = s.slice(5, i), data = s.slice(i + 1);
        var mime = head.split(';')[0] || 'image/png', bin;
        bin = /;base64/i.test(head) ? atob(data) : decodeURIComponent(data);
        var arr = new Uint8Array(bin.length);
        for (var k = 0; k < bin.length; k++) arr[k] = bin.charCodeAt(k);
        return new Blob([arr], { type: mime });
    }
    function uploadSrc(src) {
        if (/^data:image\//i.test(src)) { var b = dataUrlToBlob(src); return uploadBlob(b, extOf(b.type)); }
        if (/^blob:/i.test(src)) return fetch(src).then(function (r) { return r.blob(); }).then(function (b) { return uploadBlob(b, extOf(b.type)); });
        if (/^(https?:)?\/\//i.test(src)) {
            var u = src.indexOf('//') === 0 ? location.protocol + src : src;
            return fetch(u, { mode: 'cors', credentials: 'omit' }).then(function (r) {
                if (!r.ok) throw new Error('HTTP ' + r.status);
                return r.blob();
            }).then(function (b) {
                if (!/^image\//i.test(b.type || '')) throw new Error('khong phai anh');
                return { ok: true, b: b };
            }, function () { return { ok: false }; }).then(function (r) {
                dbg('tai anh', u, r.ok ? 'bang trinh duyet' : 'bang may chu');
                return r.ok ? uploadBlob(r.b, extOf(r.b.type)) : uploadRemote(u);
            });
        }
        return Promise.reject(new Error('Nguon anh khong ho tro'));
    }
    function needsUpload(src) {
        if (!src) return false;
        if (/^(data:image\/|blob:)/i.test(src)) return true;
        var m = /^(?:https?:)?\/\/([^\/?#]+)/i.exec(src);
        return !!m && m[1].toLowerCase() !== location.host.toLowerCase();
    }

    // ------------------------------------------------------------------ lam sach HTML dan vao
    var DROP = { SCRIPT: 1, STYLE: 1, META: 1, LINK: 1, TITLE: 1, HEAD: 1, XML: 1, 'O:P': 1, NOSCRIPT: 1, IFRAME: 1, OBJECT: 1, EMBED: 1, FORM: 1, INPUT: 1, BUTTON: 1, SELECT: 1, TEXTAREA: 1, COLGROUP: 1, COL: 1, SVG: 1, CANVAS: 1 };
    var KEEP = { P: 1, H1: 1, H2: 1, H3: 1, H4: 1, H5: 1, H6: 1, BLOCKQUOTE: 1, UL: 1, OL: 1, LI: 1, TABLE: 1, THEAD: 1, TBODY: 1, TFOOT: 1, TR: 1, TD: 1, TH: 1, BR: 1, HR: 1, SUB: 1, SUP: 1, PRE: 1, CODE: 1 };
    var BLOCK = { P: 1, H1: 1, H2: 1, H3: 1, H4: 1, H5: 1, H6: 1, BLOCKQUOTE: 1, UL: 1, OL: 1, LI: 1, TABLE: 1, THEAD: 1, TBODY: 1, TFOOT: 1, TR: 1, TD: 1, TH: 1, HR: 1, PRE: 1, DIV: 1, SECTION: 1, ARTICLE: 1, FIGURE: 1, CENTER: 1 };
    var INLINE_FMT = { SPAN: 1, FONT: 1, B: 1, STRONG: 1, I: 1, EM: 1, U: 1, S: 1, STRIKE: 1, DEL: 1, INS: 1, MARK: 1, SMALL: 1, BIG: 1, LABEL: 1 };

    function el(t) { return document.createElement(t); }
    function hasBlock(n) {
        var c = n.querySelectorAll('*');
        for (var i = 0; i < c.length; i++) if (BLOCK[c[i].tagName.toUpperCase()]) return true;
        return false;
    }
    function unwrapGoogle(h) {
        var m = /^https?:\/\/www\.google\.com\/url\?(?:[^#]*&)?q=([^&]+)/i.exec(h || '');
        if (m) { try { return decodeURIComponent(m[1]); } catch (e) { /* giu nguyen */ } }
        return h || '';
    }
    function fmtOf(n, t) {
        var st = n.style || {}, fw = (st.fontWeight || '').toLowerCase(), fs = (st.fontStyle || '').toLowerCase(), td = ((st.textDecoration || '') + ' ' + (st.textDecorationLine || '')).toLowerCase();
        var inLink = !!(n.closest && n.closest('a'));
        var isB = t === 'B' || t === 'STRONG', isI = t === 'I' || t === 'EM';
        var b = (fw === 'bold' || fw === 'bolder' || parseInt(fw, 10) >= 600) || (isB && fw !== 'normal' && !(parseInt(fw, 10) < 600));
        var i = fs === 'italic' || fs === 'oblique' || (isI && fs !== 'normal');
        var u = !inLink && (t === 'U' || td.indexOf('underline') >= 0);
        var s = t === 'S' || t === 'STRIKE' || t === 'DEL' || td.indexOf('line-through') >= 0;
        return { b: b, i: i, u: u, s: s };
    }
    function walk(src, dst) {
        for (var k = 0; k < src.childNodes.length; k++) {
            var n = src.childNodes[k];
            if (n.nodeType === 3) { dst.appendChild(document.createTextNode(n.nodeValue)); continue; }
            if (n.nodeType !== 1) continue;
            var t = n.tagName.toUpperCase();
            if (DROP[t]) continue;
            var sty = n.style || {};
            if (sty.display === 'none' || /mso-hide:\s*all/i.test(n.getAttribute('style') || '')) continue;
            if (t === 'IMG') {
                var s = n.getAttribute('src');
                if (!s) continue;
                var im = el('img'); im.setAttribute('src', s); im.setAttribute('alt', n.getAttribute('alt') || '');
                dst.appendChild(im); continue;
            }
            if (t === 'A') {
                var href = unwrapGoogle(n.getAttribute('href') || '');
                if (!/^(https?:|mailto:|tel:|#|\/)/i.test(href)) { walk(n, dst); continue; }
                var a = el('a'); a.setAttribute('href', href);
                if (/^https?:/i.test(href)) { a.setAttribute('target', '_blank'); a.setAttribute('rel', 'noopener'); }
                walk(n, a);
                if (a.textContent.replace(/\s/g, '') || a.querySelector('img')) dst.appendChild(a);
                continue;
            }
            if (INLINE_FMT[t]) {
                var f = fmtOf(n, t), inner = dst, made = [];
                if (hasBlock(n)) { walk(n, dst); continue; }          // vd. Google Docs boc ca doan trong <b>
                [['b', 'strong'], ['i', 'em'], ['u', 'u'], ['s', 's']].forEach(function (p) {
                    if (f[p[0]]) { var w = el(p[1]); inner.appendChild(w); inner = w; made.push(w); }
                });
                walk(n, inner);
                continue;
            }
            if (BLOCK[t] && !KEEP[t]) {                                // div, section, figure...
                if (hasBlock(n)) walk(n, dst); else { var p = el('p'); walk(n, p); dst.appendChild(p); }
                continue;
            }
            if (KEEP[t]) {
                var e = el(t.toLowerCase());
                if (t === 'TD' || t === 'TH') { ['colspan', 'rowspan'].forEach(function (a) { if (n.getAttribute(a)) e.setAttribute(a, n.getAttribute(a)); }); }
                if (t === 'TABLE') { e.setAttribute('border', '1'); e.setAttribute('cellspacing', '0'); e.setAttribute('cellpadding', '6'); e.setAttribute('width', '100%'); }
                walk(n, e); dst.appendChild(e); continue;
            }
            walk(n, dst);                                              // the la: bo the, giu noi dung
        }
    }
    function post_(out) {
        // bo the dinh dang rong
        var again = true;
        while (again) {
            again = false;
            var emp = out.querySelectorAll('strong,em,u,s,a');
            for (var i = 0; i < emp.length; i++) {
                if (!emp[i].textContent.replace(/[\s\u00a0]/g, '') && !emp[i].querySelector('img')) { emp[i].parentNode.removeChild(emp[i]); again = true; }
            }
        }
        // <li><p>..</p></li> -> <li>..</li>
        var lis = out.querySelectorAll('li');
        for (var j = 0; j < lis.length; j++) {
            var li = lis[j];
            if (li.children.length === 1 && li.firstElementChild.tagName === 'P' && !/\S/.test(Array.prototype.filter.call(li.childNodes, function (x) { return x.nodeType === 3; }).map(function (x) { return x.nodeValue; }).join(''))) {
                var p = li.firstElementChild; while (p.firstChild) li.insertBefore(p.firstChild, p); li.removeChild(p);
            }
        }
        // doan rong
        var ps = out.querySelectorAll('p');
        for (var q = 0; q < ps.length; q++) {
            if (!ps[q].textContent.replace(/[\s\u00a0]/g, '') && !ps[q].querySelector('img,table,hr')) ps[q].parentNode.removeChild(ps[q]);
        }
        // xuong dong thua trong van ban thuong
        var w = document.createTreeWalker(out, NodeFilter.SHOW_TEXT, null, false), tn, list = [];
        while ((tn = w.nextNode())) list.push(tn);
        list.forEach(function (x) { if (!(x.parentNode.closest && x.parentNode.closest('pre'))) x.nodeValue = x.nodeValue.replace(/\s*[\r\n]+\s*/g, ' '); });
        // co khoi: boc cac doan chu roi o muc goc vao <p>; chi co chu thi giu nguyen (dan giua doan)
        var hasB = false, c = out.children;
        for (var z = 0; z < c.length; z++) if (BLOCK[c[z].tagName.toUpperCase()]) hasB = true;
        if (hasB) {
            var run = null, kids = Array.prototype.slice.call(out.childNodes);
            kids.forEach(function (x) {
                var isBlk = x.nodeType === 1 && BLOCK[x.tagName.toUpperCase()];
                if (isBlk) { run = null; return; }
                if (x.nodeType === 3 && !x.nodeValue.trim()) { out.removeChild(x); return; }
                if (!run) { run = el('p'); out.insertBefore(run, x); }
                run.appendChild(x);
            });
        }
    }
    function cleanHtml(html) {
        var doc = new DOMParser().parseFromString(html, 'text/html'), out = el('div');
        walk(doc.body, out);
        post_(out);
        return out;
    }

    // ------------------------------------------------------------------ CKEditor
    function filesOf(dt) {
        var arr = [];
        if (dt && dt.getFilesCount) { for (var i = 0; i < dt.getFilesCount(); i++) { var f = dt.getFile(i); if (isImageFile(f)) arr.push(f); } }
        return arr;
    }
    function insertUploaded(editor, files) {
        var done = 0, fail = 0;
        files.reduce(function (chain, f) {
            return chain.then(function () {
                return uploadBlob(f, extOf(f.type)).then(function (url) {
                    editor.insertHtml("<a data-fancybox='anhtrongbaiviet' href='" + url + "'><img alt='' src='" + url + "'></a>");
                    done++;
                }, function () { fail++; });
            });
        }, Promise.resolve()).then(function () {
            window.formModified = true; editor.fire('saveSnapshot');
            if (done) say(true, 'Đã tải ' + done + ' ảnh lên và thêm vào danh sách ảnh, tệp');
            if (fail) say(false, 'Không tải được ' + fail + ' ảnh');
        });
    }
    function processPending(editor, srcs) {
        var imgs = editor.editable().find('img'), mine = [], byUrl = {}, i;
        for (i = 0; i < imgs.count(); i++) {
            var im = imgs.getItem(i), cur = im.data('cke-saved-src') || im.getAttribute('src');
            if (srcs.indexOf(cur) >= 0 && !im.$.__neBusy) { im.$.__neBusy = true; im.$.style.opacity = '.5'; mine.push({ im: im, src: cur }); }
        }
        if (!mine.length) return;
        var uniq = []; mine.forEach(function (m) { if (uniq.indexOf(m.src) < 0) uniq.push(m.src); });
        var ok = 0, fail = 0;
        var jobs = uniq.map(function (s) {
            return uploadSrc(s).then(function (url) {
                byUrl[s] = url; ok++;
            }, function () { fail++; });
        });
        Promise.all(jobs).then(function () {
            mine.forEach(function (m) {
                m.im.$.style.opacity = '';
                if (!m.im.$.getAttribute('style')) m.im.$.removeAttribute('style');
                m.im.$.__neBusy = false;
                var u = byUrl[m.src];
                if (u) { m.im.setAttribute('src', u); m.im.setAttribute('data-cke-saved-src', u); }
            });
            window.formModified = true; editor.fire('saveSnapshot');
            if (ok) say(true, 'Đã tải ' + ok + ' ảnh lên và thêm vào danh sách ảnh, tệp');
            if (fail) say(false, 'Không tải được ' + fail + ' ảnh, ảnh giữ nguyên link gốc');
        });
    }
    function bind(editor) {
        var pending = null;
        editor.on('paste', function (evt) {
            var d = evt.data, dt = d.dataTransfer;
            var raw = d.dataValue || '';
            if (!raw && dt && dt.getData) { try { raw = dt.getData('text/html') || ''; } catch (e) { /* bo qua */ } }
            var files = filesOf(dt);
            dbg('paste', { type: d.type, html: raw.length, files: files.length });
            var hasContent = /<img\b/i.test(raw) || raw.replace(/<[^>]*>/g, '').replace(/&nbsp;|\s/g, '');
            if (files.length && !hasContent) { evt.cancel(); insertUploaded(editor, files); return; }
            if (!raw || d.type === 'text') return;
            // noi dung copy ngay trong trinh soan thao: giu nguyen
            try { if (dt && dt.getTransferType && dt.getTransferType(editor) === CKEDITOR.DATA_TRANSFER_INTERNAL) return; } catch (e) { /* bo qua */ }
            var out = cleanHtml(raw), srcs = [];
            Array.prototype.forEach.call(out.querySelectorAll('img'), function (im) {
                var s = im.getAttribute('src'); if (needsUpload(s) && srcs.indexOf(s) < 0) srcs.push(s);
            });
            d.dataValue = out.innerHTML;
            pending = srcs;
            dbg('anh can tai len', srcs.length);
        }, null, null, 2);
        editor.on('afterPaste', function () {
            if (!pending || !pending.length) return;
            var s = pending; pending = null;
            processPending(editor, s);
        });
        var ed = editor.editable();
        ed.attachListener(ed, 'paste', function (ev) {
            var cd = ev.data && ev.data.$ && ev.data.$.clipboardData;
            if (!cd || !cd.items) return;
            var files = [], hasStr = false, i;
            for (i = 0; i < cd.items.length; i++) {
                var it = cd.items[i];
                if (it.kind === 'file' && /^image\//i.test(it.type)) { var f = it.getAsFile(); if (f) files.push(f); }
                else if (it.kind === 'string' && (it.type === 'text/html' || it.type === 'text/plain')) hasStr = true;
            }
            dbg('paste goc', { items: cd.items.length, files: files.length, hasStr: hasStr });
            if (files.length && !hasStr) { ev.cancel(); ev.data.preventDefault(); insertUploaded(editor, files); }
        }, null, null, 0);
        editor.on('drop', function (evt) {
            var files = filesOf(evt.data.dataTransfer);
            if (!files.length) return;
            evt.cancel();
            try { if (evt.data.dropRange) editor.getSelection().selectRanges([evt.data.dropRange]); } catch (e) { /* bo qua */ }
            insertUploaded(editor, files);
        }, null, null, 1);
    }
    if (window.CKEDITOR) {
        window.CKEDITOR.on('instanceReady', function (ev) {
            var c = cfg();
            if (c && ev.editor.name === c.editor) bind(ev.editor);
        });
    }
})();

/* ============================================================================
   Bo cuc viet bai (lay trinh soan thao lam trung tam):
   - Toolbar CKEditor sap theo muc do dung (hang 1: viet bai; hang 2: nang cao), dinh khi cuon
   - Nut "Anh & tep cua bai" ngay tren toolbar, Ctrl+S luu, thanh trang thai da luu / chua luu
   - Che do tap trung (an khung phai), dem ky tu tom tat
   Chi la lop giao dien: khong doi du lieu, ID control hay luong luu bai.
   ============================================================================ */
(function () {
    'use strict';
    var BASE = '/DesktopModules/NVCMS.TinTuc/Manager/news/';
    var FKEY = 'ne.ed.focus';

    function root() { return document.querySelector('.ne-v3'); }
    function q(sel, ctx) { return (ctx || document).querySelector(sel); }
    function hhmm() { var d = new Date(), p = function (n) { return (n < 10 ? '0' : '') + n; }; return p(d.getHours()) + ':' + p(d.getMinutes()); }

    // ---------------------------------------------------------------- trang thai luu
    function setState(kind, text) {
        var s = document.getElementById('neState'); if (!s) return;
        s.className = 'ne-state is-' + kind; s.textContent = text;
    }
    function hookSave() {
        ['updateSuccess', 'updateError'].forEach(function (n) {
            var f = window[n];
            if (typeof f !== 'function' || f.__ne) return;
            var w = function (result) {
                var res = f.apply(this, arguments);
                try {
                    if (n === 'updateSuccess') {
                        var ok = !(result === '0' || result === 0 || result === '' || result == null);
                        setState(ok ? 'saved' : 'error', ok ? 'Đã lưu lúc ' + hhmm() : 'Lưu thất bại');
                    } else setState('error', 'Lưu lỗi');
                } catch (e) { /* bo qua */ }
                return res;
            };
            w.__ne = 1; window[n] = w;
        });
    }

    // ---------------------------------------------------------------- che do tap trung
    function setFocus(on) {
        var r = root(); if (!r) return;
        r.classList.toggle('ne-focus', !!on);
        var b = document.getElementById('neFocusBtn');
        if (b) { b.classList.toggle('active', !!on); var sp = b.querySelector('span'); if (sp) sp.textContent = on ? 'Thoát tập trung' : 'Tập trung'; }
        try { window.localStorage.setItem(FKEY, on ? '1' : '0'); } catch (e) { /* bo qua */ }
        setTimeout(function () { try { window.dispatchEvent(new Event('resize')); } catch (e) { /* bo qua */ } syncTop(); }, 60);
    }

    // vi tri dinh cua toolbar CKEditor = duoi header (64px) va thanh thao tac neu thanh nay dang dinh
    function syncTop() {
        var r = root(), bar = r && q('.tt-ed-actionbar', r); if (!bar) return;
        var stuck = window.getComputedStyle(bar).position === 'sticky';
        document.documentElement.style.setProperty('--ne-toolbar-top', (64 + (stuck ? bar.offsetHeight + 10 : 8)) + 'px');
    }

    function saveNow() {
        var b = q('[id$="lbtSave"]');
        if (b) b.click();
    }
    function openMedia() {
        var r = root(); if (!r) return;
        setFocus(false);
        if (window.neShowSide) window.neShowSide('media');
        var p = q('[data-side-pane="media"]', r);
        if (p) {
            if (window.matchMedia('(max-width:1199.98px)').matches) p.scrollIntoView({ behavior: 'smooth', block: 'start' });
            p.classList.remove('is-flash'); void p.offsetWidth; p.classList.add('is-flash');
        }
    }

    // ---------------------------------------------------------------- CKEditor
    function registerPlugin() {
        if (!window.CKEDITOR || CKEDITOR.plugins.registered.nemedia) return;
        CKEDITOR.plugins.add('nemedia', {
            init: function (editor) {
                editor.addCommand('neMedia', { canUndo: false, readOnly: 1, exec: openMedia });
                editor.addCommand('neSave', { canUndo: false, readOnly: 1, exec: saveNow });
                editor.ui.addButton('NeMedia', { label: 'Ảnh & tệp của bài', command: 'neMedia', toolbar: 'insert,0', icon: BASE + 'ne-media.svg' });
                editor.setKeystroke(CKEDITOR.CTRL + 83, 'neSave');          // Ctrl+S
            }
        });
    }
    // ---------------------------------------------------------------- Chen box (combo "Chen box" + nut Templates)
    // Moi box = 1 muc trong NE_BOXES. HTML dung inline style (khong phu thuoc CSS trang chi tiet, ACF dang tat allowedContent = true).
    // Mau thuong hieu Capstone: navy #28166F, vang #F8C100, xanh la #00923E. Link Download de "#": chon nut roi Ctrl+K de gan link tep.
    // Box dung inline style, KHONG co font-family / co chu co dinh: theo font cua bai (co chu tinh theo em).
    function dlSection(title, items, sub) {
        var h = '<table cellspacing="0" cellpadding="0" style="width:100%;border-collapse:separate;border-spacing:0;border:1px solid #DDE1EE;border-radius:10px;overflow:hidden;margin:0 0 22px 0;"><tbody>' +
            '<tr><td colspan="2" style="background:#28166F;color:#ffffff;padding:13px 18px;border:0;border-left:6px solid #F8C100;font-weight:bold;letter-spacing:0.6px;text-transform:uppercase;">' + title + '</td></tr>';
        if (sub) h += '<tr><td colspan="2" style="background:#FFF6D1;color:#28166F;padding:10px 18px;border:0;border-left:6px solid #F8C100;border-bottom:1px solid #F1E3A6;font-weight:bold;">' + sub + '</td></tr>';
        items.forEach(function (name, i) {
            var bg = i % 2 ? '#F7F8FC' : '#ffffff', bd = i === items.length - 1 ? 'border:0;' : 'border:0;border-bottom:1px solid #E8EAF2;';
            h += '<tr><td style="background:' + bg + ';color:#222222;padding:13px 18px;' + bd + 'vertical-align:middle;"><span style="color:#00923E;font-size:0.7em;">&#9679;</span>&nbsp;&nbsp;' + name + '</td>' +
                '<td style="background:' + bg + ';padding:9px 18px;' + bd + 'width:130px;text-align:right;vertical-align:middle;white-space:nowrap;">' +
                '<a href="#" style="display:inline-block;background:#00923E;color:#ffffff !important;text-decoration:none;font-size:0.85em;font-weight:bold;padding:7px 18px;border-radius:20px;">&darr;&nbsp;Download</a></td></tr>';
        });
        return h + '</tbody></table>';
    }
    var BX_TITLE = 'margin:0 0 .4em;font-weight:bold;text-transform:uppercase;font-size:.8em;letter-spacing:.04em;';
    var NE_BOXES = [
        {
            key: 'note', label: 'Ô thông tin / ghi chú', icon: 'ni-info', description: 'Khung nền vàng nhạt: tóm tắt số liệu, lưu ý cho bạn đọc.',
            html: function () {
                return '<div style="background:#FFF8E1;border:1px solid #F3E2A9;border-left:5px solid #F8C100;border-radius:6px;padding:14px 18px;margin:1.2em 0;"><p style="margin:0 0 .5em;"><strong>Thông tin cần biết</strong></p><ul style="margin:0;padding-left:1.4em;"><li>Ý thứ nhất</li><li>Ý thứ hai</li></ul></div><p></p>';
            }
        },
        {
            key: 'box', label: 'Box có tiêu đề', icon: 'ni-layout', description: 'Khung nền xám, viền navy phía trên, có dòng tiêu đề: bối cảnh, thông tin thêm.',
            html: function () {
                return '<div style="background:#F5F6FA;border-top:3px solid #28166F;padding:14px 18px;margin:1.2em 0;"><p style="' + BX_TITLE + 'color:#28166F;">Tiêu đề box</p><p style="margin:0;">Nội dung box: bối cảnh, thông tin thêm cho bài viết.</p></div><p></p>';
            }
        },
        {
            key: 'box-right', label: 'Box bên lề phải', icon: 'ni-layout-alt', description: 'Box nhỏ đứng bên phải, chữ bài chạy quanh.',
            html: function () {
                return '<div style="float:right;width:260px;max-width:45%;background:#F5F6FA;border-top:3px solid #28166F;padding:12px 14px;margin:.3em 0 1em 1.2em;"><p style="' + BX_TITLE + 'color:#28166F;">Tiêu đề box</p><p style="margin:0;">Nội dung ngắn, 2–4 câu.</p></div><p></p>';
            }
        },
        {
            key: 'keypoints', label: 'Điểm nhấn (tóm tắt nhanh)', icon: 'ni-list-check', description: 'Các ý chính của bài, đặt đầu bài hoặc sau sapo.',
            html: function () {
                return '<div style="background:#F1FAF5;border-left:5px solid #00923E;border-radius:6px;padding:14px 18px;margin:1.2em 0;"><p style="' + BX_TITLE + 'color:#00923E;">Điểm nhấn</p><ul style="margin:0;padding-left:1.4em;"><li>Ý chính thứ nhất</li><li>Ý chính thứ hai</li><li>Ý chính thứ ba</li></ul></div><p></p>';
            }
        },
        {
            key: 'stat', label: 'Số liệu nổi bật', icon: 'ni-bar-chart', description: 'Một con số lớn kèm giải thích ngắn.',
            html: function () {
                return '<div style="text-align:center;padding:18px 12px;margin:1.2em 0;border-top:1px solid #DDE1EE;border-bottom:1px solid #DDE1EE;"><p style="margin:0;font-size:2.6em;font-weight:bold;line-height:1.1;color:#28166F;">13,4%</p><p style="margin:.4em 0 0;color:#6B7A99;">Mô tả con số: chỉ tiêu, thời gian, nguồn.</p></div><p></p>';
            }
        },
        {
            key: 'quote', label: 'Trích dẫn nổi bật', icon: 'ni-quote-left', description: 'Câu nói đáng chú ý, chữ lớn, căn giữa.',
            html: function () {
                return '<blockquote style="margin:1.4em 0;padding:.4em 1.2em;border:0;background:none;border-top:3px solid #F8C100;border-bottom:3px solid #F8C100;text-align:center;font-size:1.25em;font-style:italic;color:#28166F;"><p style="margin:.4em 0;">“Câu trích dẫn nổi bật của nhân vật.”</p><p style="margin:.2em 0 .4em;font-size:.7em;font-style:normal;color:#6B7A99;">– Tên nhân vật, chức vụ</p></blockquote><p></p>';
            }
        },
        {
            key: 'alert', label: 'Lưu ý / cảnh báo', icon: 'ni-alert-circle', description: 'Khung viền đỏ: lưu ý quan trọng, cảnh báo cho bạn đọc.',
            html: function () {
                return '<div style="background:#FDF1F0;border:1px solid #F0C2BE;border-left:5px solid #C0392B;border-radius:6px;padding:14px 18px;margin:1.2em 0;"><p style="' + BX_TITLE + 'color:#C0392B;">Lưu ý</p><p style="margin:0;">Nội dung khuyến cáo, cảnh báo cho bạn đọc.</p></div><p></p>';
            }
        },
        {
            key: 'interview', label: 'Phỏng vấn (Hỏi – Đáp)', icon: 'ni-chat', description: 'Câu hỏi in đậm, câu trả lời thường.',
            html: function () {
                return '<p><strong>PV: Câu hỏi thứ nhất?</strong></p><p>– Câu trả lời của nhân vật.</p><p><strong>PV: Câu hỏi thứ hai?</strong></p><p>– Câu trả lời của nhân vật.</p>';
            }
        },
        {
            key: 'table', label: 'Bảng số liệu', icon: 'ni-table-view', description: 'Bảng 3 cột có dòng tiêu đề và nguồn.',
            html: function () {
                var th = 'background:#28166F;color:#ffffff;padding:8px 12px;border:1px solid #28166F;text-align:left;', td = 'padding:8px 12px;border:1px solid #DDE1EE;vertical-align:top;';
                var row = '<tr><td style="' + td + '">…</td><td style="' + td + '">…</td><td style="' + td + '">…</td></tr>';
                return '<table style="width:100%;border-collapse:collapse;margin:1.2em 0;"><thead><tr><th style="' + th + '">Chỉ tiêu</th><th style="' + th + '">Năm trước</th><th style="' + th + '">Năm nay</th></tr></thead><tbody>' + row + row + '</tbody></table><p style="font-size:.85em;color:#6B7A99;">Nguồn: …</p><p></p>';
            }
        },
        {
            key: 'downloads', label: 'Bảng tải tài liệu', icon: 'ni-download', description: 'Các nhóm tệp cho bạn đọc tải về (lịch, mẫu biểu, đặt phòng, vé): mỗi dòng một nút Download.',
            html: function () {
                return dlSection('Schedule', ['Tentative Schedule_Capstone Education Fairs_Spring 2027']) +
                    dlSection('Shipping Options', ['Air Concepts Intro Letter', "Air Concepts Shipper's Pre-Alert Form", 'Air Concepts Client Information Form', 'Air Concepts Box Labels'], 'Air Concepts') +
                    dlSection('Hotels', ['Hotel Booking Hanoi', 'Hotel Booking Ho Chi Minh City', 'Hotel Booking Danang']) +
                    dlSection('Air-Ticket', ['Air-Ticket']) + '<p></p>';
            }
        }
    ];
    function boxByKey(k) { for (var i = 0; i < NE_BOXES.length; i++) if (NE_BOXES[i].key === k) return NE_BOXES[i]; return null; }
    function boxInsert(editor, k) {
        var b = boxByKey(k); if (!b) return;
        editor.focus(); editor.fire('saveSnapshot');
        editor.insertHtml(b.html(), 'unfiltered_html');
        window.formModified = true; editor.fire('saveSnapshot');
    }
    function registerBoxPlugin() {
        if (!window.CKEDITOR || CKEDITOR.plugins.registered.nebox) return;
        // Templates: hien cung danh sach box (cung voi mau mac dinh cua CKEditor)
        CKEDITOR.addTemplates('nebox', { imagesPath: '', templates: NE_BOXES.map(function (b) { return { title: b.label, description: b.description, html: b.html() }; }) });
        CKEDITOR.plugins.add('nebox', {
            init: function (editor) {
                editor.ui.addRichCombo('NeBox', {
                    label: 'Chèn box', title: 'Chèn box vào bài tại con trỏ',
                    panel: { css: [CKEDITOR.skin.getPath('editor')], multiSelect: false, attributes: { 'aria-label': 'Chèn box' } },
                    init: function () {
                        this.startGroup('Chèn box');
                        for (var i = 0; i < NE_BOXES.length; i++) this.add(NE_BOXES[i].key, NE_BOXES[i].label, NE_BOXES[i].label);
                    },
                    onClick: function (value) { boxInsert(editor, value); }
                });
            }
        });
    }

    // Cac man dung CKEditor mac dinh (config.js): duyet tin, theo doi tin bai, tin da xuat ban (huy xuat ban) -> tu them "Chen box" truoc nut Templates.
    // Man soan tin v3 (neEditorConfig) da co san nebox nen bo qua; trang Magazine dung customConfig rieng nen khong dong vao.
    if (window.CKEDITOR) {
        window.CKEDITOR.on('instanceCreated', function (ev) {
            ev.editor.on('configLoaded', function () {
                var c = ev.editor.config, done = false;
                if (/\bnebox\b/.test(c.extraPlugins || '') || /configMagazine/i.test(c.customConfig || '') || !Array.isArray(c.toolbar)) return;
                c.toolbar.forEach(function (g) {
                    var i = (!done && g && Array.isArray(g.items)) ? g.items.indexOf('Templates') : -1;
                    if (i >= 0) { g.items.splice(i, 0, 'NeBox'); done = true; }
                });
                if (!done) return;
                registerBoxPlugin();
                c.extraPlugins = (c.extraPlugins ? c.extraPlugins + ',' : '') + 'nebox';
                c.templates = 'nebox,default';
            });
        });
    }

    // Cau hinh rieng cho man soan tin (khong dung config.js dung chung voi cac module khac)
    window.neEditorConfig = function () {
        registerPlugin(); registerBoxPlugin();
        return {
            height: Math.max(560, window.innerHeight - 280),
            toolbarCanCollapse: false,
            toolbarStartupExpanded: true,
            toolbar: [
                { name: 'styles', items: ['Styles', 'Format'] },
                { name: 'colors', items: ['TextColor', 'BGColor'] },
                { name: 'tools', items: ['Maximize'] },
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'RemoveFormat'] },
                { name: 'document', items: ['Templates', 'Youtube', 'oembed'] },
                { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
                { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'Blockquote', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                { name: 'links', items: ['Link', 'Unlink'] },
                { name: 'insert', items: ['NeMedia', 'Image', 'Table', 'HorizontalRule', 'SpecialChar'] },
                { name: 'view', items: ['Source'] },
                { name: 'biz', items: ['dangkydo', 'dangkydoen'] }
            ],
            // giu cac plugin cua config.js (extraPlugins cua instance ghi de config.js) va bat them dem tu
            extraPlugins: 'youtube,oembed,filetools,uploadwidget,dangkydo,dangkydoen,chuky,chukyen,nemedia,nebox,wordcount',
            templates: 'nebox,default',
            removePlugins: 'easyimage',
            format_tags: 'p;h2;h3;h4',
            contentsCss: [CKEDITOR.basePath + 'contents.css', BASE + 'newsedit-content.css?v=3'],
            bodyClass: 'ne-content',
            wordcount: { showParagraphs: false, showWordCount: true, showCharCount: true, countSpacesAsChars: true, countHTML: false, maxWordCount: -1, maxCharCount: -1 }
        };
    };
    if (window.CKEDITOR) {
        window.CKEDITOR.on('instanceReady', function (ev) {
            var c = window.NE_CFG;
            if (c && ev.editor.name === c.editor) ev.editor.on('change', function () { setState('dirty', 'Chưa lưu'); });
        });
    }

    // ---------------------------------------------------------------- Sap xep cho de dung (nguoi dung it quen may tinh)
    // Chi doi giao dien: giu nguyen ID control, code-behind, du lieu va luong xu ly. Moi buoc bo qua neu khong tim thay phan tu.
    function txtOf(n) { return (n.textContent || '').replace(/\s+/g, ' ').trim(); }
    function relabel(r, sel, map) {
        Array.prototype.forEach.call(r.querySelectorAll(sel), function (n) {
            var t = txtOf(n); if (map[t] && !n.children.length) n.textContent = map[t];
        });
    }
    function swapCls(el, from, to) { if (el) { el.classList.remove(from); el.classList.add(to); } }
    function badge(card, cls, text) {
        var t = card && q('.tt-card-toggle > span', card);
        if (!t || q('.ne-badge', card)) return;
        t.insertAdjacentHTML('afterend', '<span class="ne-badge ' + cls + '">' + text + '</span>');
    }
    function hintAfter(el, text) {
        if (!el || !el.parentNode || el.parentNode.querySelector('.ne-hint')) return;
        var d = document.createElement('div'); d.className = 'tt-card-hint ne-hint'; d.textContent = text;
        el.parentNode.insertBefore(d, el.nextSibling);
    }
    // nut [Dai] [Dai KHONG ANH] [Phai] [Trai] cua tin lien quan -> nut nho "Dài", "Dài không ảnh", "Phải", "Trái" (giu nguyen onclick)
    function tidyRelated(r) {
        var box = r.querySelector('#divrelated'); if (!box) return;
        var run = function () {
            Array.prototype.forEach.call(box.querySelectorAll('a.insertRelated'), function (a) {
                var m = /^\s*\[(.*?)\]\s*$/.exec(a.textContent || '');
                if (!m) return;
                var t = m[1].charAt(0) + m[1].slice(1).toLowerCase();
                a.textContent = t; a.classList.add('ne-ins'); a.title = 'Chèn tin này vào vị trí con trỏ trong bài, kiểu: ' + t;
            });
        };
        run();
        if (window.MutationObserver) new MutationObserver(run).observe(box, { childList: true, subtree: true });
    }
    function tidyLayout(r) {
        if (r.__neTidy) return; r.__neTidy = 1;
        var byId = function (id) { return document.getElementById(id); };
        // 1) thanh thao tac: nut hay bam nham (Thoat, Lay tin bai) mau trung tinh; mau do chi danh cho Xoa bai
        swapCls(q('[id$="lbtCancelTop"]', r), 'btn-outline-danger', 'btn-outline-light');
        var lay = q('a[data-target="#modalLayTinBai"]', r);
        if (lay) { swapCls(lay, 'btn-outline-danger', 'btn-outline-light'); lay.title = 'Nhập link để lấy nội dung một bài viết về đây'; }
        swapCls(q('[id$="lbtDeleteTop"]', r), 'btn-dark', 'btn-outline-danger');
        relabel(r, '.tt-ed-actions .btn span', { 'Xem Trước': 'Xem trước', 'Gửi Biên tập': 'Gửi biên tập', 'Gửi Xuất bản': 'Gửi xuất bản' });
        // 2) ten muc: bo chu HOA / tieng Anh lan tieng Viet
        relabel(r, 'h6.overline-title', { 'NGÀY GIỜ XUẤT BẢN': 'Ngày giờ xuất bản', 'Cấu hình hiện display': 'Tùy chọn hiển thị', 'Cấu hình tin bài': 'Tùy chọn tin bài' });
        relabel(r, 'label.form-label-outlined', { 'Bút Danh Tác giả': 'Bút danh tác giả', 'Chọn Chuyên mục': 'Chuyên mục chính' });
        // nut trong o "Luu y / but phe" chi luu loi nhan -> khong dat ten giong nut Luu thay doi cua bai
        relabel(r, '[id$="lbtSendNewsNote"] span', { 'Lưu thay đổi': 'Gửi lời nhắn' });
        var tt = q('[id$="txtTitle"]', r); if (tt && tt.placeholder === 'Nhập tiêu đề') tt.placeholder = 'Nhập tiêu đề bài viết (bắt buộc)';
        // 3) thu tu khung ben phai: viec bat buoc truoc (chuyen muc), roi anh dai dien, ngay xuat ban, hien thi, link, truong, loi nhan
        var cards = ['ne-card-classify', 'ne-card-avatar', 'ne-card-publish', 'ne-card-display', 'ne-card-register', 'ne-card-school', 'ne-card-royalty', 'ne-card-notes']
            .map(byId).filter(Boolean);
        if (cards.length > 1 && cards.every(function (c) { return c.parentNode === cards[0].parentNode; })) {
            var host = cards[0].parentNode, earliest = cards[0];
            cards.forEach(function (c) { if (earliest.compareDocumentPosition(c) & 2) earliest = c; });   // 2 = c nam truoc earliest
            var ref = document.createTextNode(''); host.insertBefore(ref, earliest);
            cards.forEach(function (c) { host.insertBefore(c, ref); });
        }
        // 4) the "Bat buoc" / "Khong bat buoc" (bat buoc theo validator: chuyen muc; ngay xuat ban khi bam Xuat ban ngay)
        badge(byId('ne-card-classify'), 'ne-badge-req', 'Bắt buộc');
        if (q('[id$="txtPublishedDate"]', r)) badge(byId('ne-card-publish'), 'ne-badge-req', 'Cần khi xuất bản');
        ['ne-card-related', 'ne-card-extra', 'ne-card-register', 'ne-card-school'].forEach(function (id) { badge(byId(id), 'ne-badge-opt', 'Không bắt buộc'); });
        // 5) cau chu huong dan ngan
        hintAfter(q('[id$="txtPublishedDate"]', r), 'Bắt buộc khi bấm “Xuất bản ngay”.');
        hintAfter(q('#page-popup', r), 'Đặt con trỏ vào chỗ muốn chèn trong bài, rồi bấm kiểu hiển thị (Dài, Dài không ảnh, Phải, Trái) cạnh tin cần chèn.');
        hintAfter(q('[id$="txtNoteNews"]', r), 'Lời nhắn chỉ lưu vào mục Lưu ý / bút phê, không thay cho nút Lưu thay đổi ở thanh trên.');
        // 6) Chi dung o "Tin tuc - Su kien Trang chu" (slide noi bat): an cac o tick con lai. Chi an tren man hinh: gia tri da luu cua bai giu nguyen, luu bai van doc dung nhu cu.
        ['chkAMP', 'chkBaiMoiNhat', 'chkQuangCao', 'chkPR', 'chkVideo', 'chkPhoto', 'chkHotCat', 'chkHotSite', 'chkconfigtinnong', 'chkconfigxuhuongdoc'].forEach(function (k) {
            var c = q('input[id$="' + k + '"]', r), w = c && (c.closest('.g') || c.closest('.custom-control'));
            if (w) w.classList.add('ne-hide');
        });
        var disp = byId('ne-card-display');
        if (disp) {
            var shown = function (el, stop) { for (; el && el !== stop; el = el.parentNode) { if (el.nodeType === 1 && window.getComputedStyle(el).display === 'none') return false; } return true; };
            var hasShown = function (box) { return Array.prototype.some.call(box.querySelectorAll('.custom-control'), function (c) { return shown(c, box.parentNode); }); };
            ['.cauhinhtin', '.card-inner2'].forEach(function (sel) {
                Array.prototype.forEach.call(disp.querySelectorAll(sel), function (box) {
                    if (hasShown(box)) return;
                    box.classList.add('ne-hide');
                    if (sel === '.card-inner2' && box.parentNode.classList.contains('form-group')) box.parentNode.classList.add('ne-hide');
                });
            });
        }
        tidyRelated(r);
    }

    // ---------------------------------------------------------------- Tu nhan dien ten truong trong bai
    // Quet noi dung bai: thay ten truong (dung ten day du trong danh sach truong) thi (1) them vao "Truong lien quan",
    // (2) gan link /truong-doi-tac/{slug}-{id} vao lan xuat hien dau tien. Chi THEM, khong xoa / sua truong da chon.
    // Danh sach truong lay tu Services/SchoolNames.ashx (chi doc). Khong doi du lieu hay luong luu bai.
    function dbg() { try { if (window.localStorage.getItem('ne.debug') === '1') console.log.apply(console, ['[ne-school]'].concat([].slice.call(arguments))); } catch (e) { /* bo qua */ } }
    var SCH = { list: null, loading: false };
    var SCH_URL = '/DesktopModules/NVCMS.TinTuc/Manager/Services/SchoolNames.ashx';
    var SKEY = 'ne.school.auto';

    function schHidden() { return q('input[id$="hdf_Schools"]'); }
    function schEditor() { var c = window.NE_CFG; return (c && window.CKEDITOR && window.CKEDITOR.instances[c.editor]) || null; }

    // chuan hoa de so khop: bo dau, d->d, chu thuong, gop khoang trang. m[i] = vi tri ky tu goc cua ky tu thu i trong chuoi da chuan hoa
    function normMap(text) {
        var s = '', m = [];
        for (var i = 0; i < text.length; i++) {
            var ch = text.charAt(i), n;
            if (ch === 'đ' || ch === 'Đ') n = 'd';
            else if (/\s/.test(ch)) n = ' ';
            else n = ch.normalize ? ch.normalize('NFD').replace(/[̀-ͯ]/g, '') : ch;
            n = n.toLowerCase();
            for (var k = 0; k < n.length; k++) {
                var c = n.charAt(k);
                if (c === ' ' && s.charAt(s.length - 1) === ' ') continue;
                s += c; m.push(i);
            }
        }
        return { s: s, m: m };
    }
    function isWordCh(c) { return /[a-z0-9]/.test(c); }
    // cac vi tri xuat hien nguyen tu (khong dinh vao chu khac) cua key trong chuoi da chuan hoa
    function occ(norm, key) {
        var out = [], i = norm.s.indexOf(key);
        while (i >= 0) {
            if (!isWordCh(norm.s.charAt(i - 1)) && !isWordCh(norm.s.charAt(i + key.length))) out.push(i);
            i = norm.s.indexOf(key, i + 1);
        }
        return out;
    }
    // vi tri xuat hien dau tien khong nam de len ten truong dai hon da nhan (vd "University of Texas" trong "University of Texas at Austin")
    function freeOcc(norm, sc, longer) {
        var mine = occ(norm, sc.key);
        for (var a = 0; a < mine.length; a++) {
            var covered = false;
            for (var b = 0; b < longer.length && !covered; b++) {
                var L = longer[b]; if (L.key.length <= sc.key.length || L.key.indexOf(sc.key) < 0) continue;
                var os = occ(norm, L.key);
                for (var c = 0; c < os.length; c++) if (mine[a] < os[c] + L.key.length && os[c] < mine[a] + sc.key.length) { covered = true; break; }
            }
            if (!covered) return mine[a];
        }
        return -1;
    }

    function loadSchools() {
        if (SCH.list || SCH.loading) return;
        SCH.loading = true;
        var x = new XMLHttpRequest();
        x.open('GET', SCH_URL + '?_=' + Date.now(), true);
        x.onreadystatechange = function () {
            if (x.readyState !== 4) return;
            SCH.loading = false;
            if (x.status !== 200) { dbg('SchoolNames', x.status); return; }
            try {
                var arr = JSON.parse(x.responseText) || [], out = [];
                for (var i = 0; i < arr.length; i++) {
                    var it = { id: arr[i].i, name: arr[i].n, slug: arr[i].s, logo: arr[i].l || '', key: normMap(arr[i].n).s.replace(/^ | $/g, '') };
                    if (it.key.length >= 5 && it.slug) out.push(it);
                }
                out.sort(function (a, b) { return b.key.length - a.key.length; });
                SCH.list = out;
            } catch (e) { dbg('SchoolNames parse', e); }
        };
        x.send();
    }

    function textNodesOf(root) {
        var w = root.ownerDocument.createTreeWalker(root, 4, null, false), n, out = [];
        while ((n = w.nextNode())) out.push(n);
        return out;
    }
    function inSkip(node, root) {
        for (var p = node.parentNode; p && p !== root; p = p.parentNode) if (/^(A|H[1-6]|SCRIPT|STYLE)$/i.test(p.nodeName)) return true;
        return false;
    }
    function hasLinkTo(root, id) {
        var as = root.getElementsByTagName('a');
        for (var i = 0; i < as.length; i++) {
            var h = as[i].getAttribute('data-cke-saved-href') || as[i].getAttribute('href') || '';
            if (h.indexOf('/truong-doi-tac/') >= 0 && new RegExp('-' + id + '(?:[/?#]|$)').test(h)) return true;
        }
        return false;
    }
    // gan link vao lan xuat hien dau tien (trong 1 doan chu lien tuc, khong nam trong link khac / tieu de)
    function linkFirst(root, sc, longer) {
        var doc = root.ownerDocument, nodes = textNodesOf(root);
        for (var i = 0; i < nodes.length; i++) {
            var tn = nodes[i];
            if (inSkip(tn, root)) continue;
            var nm = normMap(tn.nodeValue), at = freeOcc(nm, sc, longer);
            if (at < 0) continue;
            var start = nm.m[at], end = nm.m[at + sc.key.length - 1] + 1;
            while (end < tn.nodeValue.length && /[̀-ͯ]/.test(tn.nodeValue.charAt(end))) end++;
            var mid = tn.splitText(start);
            mid.splitText(end - start);
            var href = '/truong-doi-tac/' + sc.slug + '-' + sc.id;
            var a = doc.createElement('a');
            a.setAttribute('href', href); a.setAttribute('data-cke-saved-href', href);
            a.setAttribute('target', '_blank'); a.setAttribute('title', sc.name);
            mid.parentNode.insertBefore(a, mid); a.appendChild(mid);
            return true;
        }
        return false;
    }

    // Tag tu khoa tu noi dung bai: (1) tu dien thuat ngu pho bien (sua danh sach TAG_DICT de them / bot), (2) cum danh tu rieng viet hoa
    // xuat hien tu 2 lan tro len (khong tinh dau cau). re = bieu thuc tim, min = so lan xuat hien toi thieu.
    function W(src, flags) { return new RegExp('(?<![\\p{L}\\p{N}])(?:' + src + ')(?![\\p{L}\\p{N}])', (flags || '') + 'gu'); }
    var TAG_DICT = [
        { t: 'Học bổng', re: W('học bổng', 'i'), min: 1 },
        { t: 'Visa', re: W('visa', 'i'), min: 1 },
        { t: 'IELTS', re: W('ielts', 'i'), min: 1 },
        { t: 'TOEFL', re: W('toefl', 'i'), min: 1 },
        { t: 'SAT', re: W('SAT'), min: 1 },
        { t: 'GPA', re: W('GPA'), min: 1 },
        { t: 'A-Level', re: W('A[- ]?Level', 'i'), min: 1 },
        { t: 'IB', re: W('IB'), min: 2 },
        { t: 'Thạc sĩ', re: W('thạc sĩ', 'i'), min: 1 },
        { t: 'Tuyển sinh', re: W('tuyển sinh', 'i'), min: 2 },
        { t: 'Du học', re: W('du học', 'i'), min: 2 },
        { t: 'Du học Mỹ', re: W('Mỹ|Hoa Kỳ'), min: 3 },
        { t: 'Du học Canada', re: W('Canada'), min: 3 },
        { t: 'Du học Úc', re: W('Úc|Australia'), min: 3 },
        { t: 'Du học Anh', re: W('Vương quốc Anh|nước Anh|Anh Quốc|UK'), min: 3 },
        { t: 'Du học New Zealand', re: W('New Zealand'), min: 3 },
        { t: 'Du học Singapore', re: W('Singapore'), min: 3 },
        { t: 'Du học Hàn Quốc', re: W('Hàn Quốc'), min: 3 },
        { t: 'Du học Nhật Bản', re: W('Nhật Bản'), min: 3 }
    ];
    function kwTags(raw, schoolKeys) {
        var out = [], i, m;
        for (i = 0; i < TAG_DICT.length; i++) { m = raw.match(TAG_DICT[i].re); if (m && m.length >= TAG_DICT[i].min) out.push(TAG_DICT[i].t); }
        var re = /\p{Lu}[\p{L}'’-]*(?:[ \u00a0]+\p{Lu}[\p{L}'’-]*){1,3}/gu, cnt = {}, disp = {};
        while ((m = re.exec(raw))) {
            var before = raw.slice(0, m.index).replace(/[ \u00a0]+$/, '');
            if (!before || /[.!?:;\n"“”]$/.test(before)) continue; // dau cau / dau doan: co the chi viet hoa vi dau cau
            var key = normMap(m[0]).s;
            if (key.length < 5) continue;
            cnt[key] = (cnt[key] || 0) + 1; disp[key] = m[0].replace(/\u00a0/g, ' ');
        }
        var keys = Object.keys(cnt).filter(function (k) {
            if (cnt[k] < 2) return false;
            for (var j = 0; j < schoolKeys.length; j++) if (schoolKeys[j].indexOf(k) >= 0) return false;
            return true;
        }).sort(function (a, b) { return cnt[b] - cnt[a]; });
        for (i = 0; i < keys.length && i < 4; i++) out.push(disp[keys[i]]);
        return out;
    }

    // them ten truong vao o "Tag bai viet" (bo qua tag da co, khong vuot 1000 ky tu)
    function schAddTags(names, max) {
        var inp = q('input[id$="txtTags"], textarea[id$="txtTags"]'), out = [];
        if (!inp) return out;
        var $ = window.jQuery, cur = (inp.value || '').toLowerCase().split(',').map(function (x) { return x.trim(); });
        names.forEach(function (n) {
            var t = n.replace(/,/g, ' ').replace(/\s+/g, ' ').trim();
            if (!t || cur.indexOf(t.toLowerCase()) >= 0) return;
            if (out.length >= max || (inp.value || '').length + t.length + 1 > 1000) return;
            if ($ && $.fn && $.fn.tagsinput && $(inp).data('tagsinput')) $(inp).tagsinput('add', t);
            else inp.value = inp.value ? inp.value + ',' + t : t;
            cur.push(t.toLowerCase()); out.push(t);
        });
        return out;
    }

    // Quet bai. Tra ve { err } hoac { found, added, linked, tagged }
    function schScan() {
        var ed = schEditor(), hid = schHidden();
        if (!ed || !hid) return { err: 'noeditor' };
        if (!SCH.list) loadSchools();
        var list = SCH.list || [];
        var body = null, root = null;
        try { if (ed.mode === 'wysiwyg' && ed.document) body = ed.document.getBody().$; } catch (e) { body = null; }
        root = body;
        if (!root) { try { root = new DOMParser().parseFromString(ed.getData(), 'text/html').body; } catch (e) { return { err: 'noeditor' }; } }

        var raw = textNodesOf(root).map(function (n) { return n.nodeValue; }).join('\n');
        var all = normMap(raw);
        var accepted = [], i;
        for (i = 0; i < list.length; i++) {
            var sc = list[i];
            if (all.s.indexOf(sc.key) < 0) continue;
            if (freeOcc(all, sc, accepted) >= 0) accepted.push(sc);
        }

        var ids = (hid.value || '').split(','), added = [], linked = [];
        for (i = 0; i < accepted.length; i++) {
            if (ids.indexOf(String(accepted[i].id)) < 0 && typeof window.HandleSchoolResult === 'function') {
                window.HandleSchoolResult(accepted[i].id, accepted[i].name, accepted[i].logo);
                added.push(accepted[i].name);
            }
        }
        var ti = q('input[id$="txtTitle"], textarea[id$="txtTitle"]');
        var tagNames = accepted.map(function (x) { return x.name; });
        try { tagNames = tagNames.concat(kwTags(((ti && ti.value) || '') + '\n' + raw, list.map(function (x) { return x.key; }))); } catch (e) { dbg('kwTags', e); }
        var tagged = schAddTags(tagNames, 8); // toi da 8 tag moi moi lan quet, truong uu tien truoc
        if (body) {
            var snap = false;
            for (i = 0; i < accepted.length; i++) {
                if (hasLinkTo(body, accepted[i].id)) continue;
                if (!snap) { try { ed.fire('saveSnapshot'); } catch (e) { /* bo qua */ } snap = true; }
                if (linkFirst(body, accepted[i], accepted.slice(0, i))) linked.push(accepted[i].name);
            }
            if (linked.length) { try { ed.fire('saveSnapshot'); ed.fire('change'); } catch (e) { /* bo qua */ } }
        }
        return { found: accepted.length, added: added, linked: linked, tagged: tagged, noList: !SCH.list };
    }

    // thong bao nho goc man hinh, tu tat sau ~7 giay (bam vao de tat ngay)
    function schToast(text) {
        var t = document.getElementById('neToast');
        if (!t) {
            t = document.createElement('div'); t.id = 'neToast'; t.className = 'ne-toast'; t.setAttribute('role', 'status');
            t.addEventListener('click', function () { t.classList.remove('is-show'); });
            document.body.appendChild(t);
        }
        t.textContent = text; t.classList.add('is-show');
        clearTimeout(schToast.h); schToast.h = setTimeout(function () { t.classList.remove('is-show'); }, 7000);
    }

    function schMsg(res, auto) {
        var m = document.getElementById('neSchoolMsg'); if (!m) return;
        var t = '', cls = 'ne-hint';
        if (res.err === 'loading') t = 'Đang tải danh sách trường, bạn bấm lại sau vài giây nhé.';
        else if (res.err) t = 'Chưa quét được nội dung bài.';
        else if (!res.found && !res.tagged.length) { t = res.noList ? 'Danh sách trường chưa tải xong, bạn bấm quét lại sau vài giây nhé.' : 'Không thấy tên trường hay từ khóa nào để thêm.'; cls += ' is-none'; }
        else if (!res.added.length && !res.linked.length && !res.tagged.length) { t = 'Trường, link và tag trong bài đã có đủ.'; cls += ' is-none'; }
        else {
            var p = [];
            if (res.added.length) p.push('Đã thêm vào Trường liên quan: ' + res.added.join(', '));
            if (res.linked.length) p.push('Đã gắn link cho: ' + res.linked.join(', '));
            if (res.tagged.length) p.push('Đã thêm tag: ' + res.tagged.join(', '));
            t = (auto ? 'Tự động — ' : '') + p.join('. ') + '.'; cls += ' is-ok';
        }
        m.className = cls; m.textContent = t;
        if (auto && !res.err && (res.tagged.length || res.added.length || res.linked.length)) {
            var parts = [];
            if (res.tagged.length) parts.push(res.tagged.length + ' tag');
            if (res.added.length) parts.push(res.added.length + ' trường liên quan');
            if (res.linked.length) parts.push(res.linked.length + ' link trường trong bài');
            schToast('Đã tự điền: ' + parts.join(', ') + '. Xem lại ở khối "Trường liên quan" và ô Tag bài viết.');
        }
    }

    function initSchoolAuto(r) {
        var pick = r && q('#btn-chon-truong', r), hid = schHidden();
        if (!pick || !hid || document.getElementById('neSchoolAuto')) return;
        var wrap = pick.closest ? pick.closest('.form-control-wrap') : pick.parentNode; if (!wrap) return;
        var on = true; try { on = window.localStorage.getItem(SKEY) !== '0'; } catch (e) { /* bo qua */ }
        var box = document.createElement('div'); box.id = 'neSchoolAuto'; box.className = 'ne-school-auto';
        box.innerHTML = '<div class="ne-sa-row">' +
            '<button type="button" id="neSchoolScanBtn" class="btn btn-xs btn-outline-primary"><em class="icon ni ni-search"></em><span>Quét bài: trường, link, tag</span></button>' +
            '<label class="ne-sa-toggle"><input type="checkbox" id="neSchoolAutoChk"' + (on ? ' checked' : '') + '> Tự làm khi bấm Lưu</label></div>' +
            '<div class="ne-hint">Hệ thống đọc tiêu đề và nội dung bài: tìm tên trường (đúng tên đầy đủ) để thêm vào danh sách bên dưới và gắn link tới trang trường ở lần xuất hiện đầu tiên; đồng thời thêm vào ô Tag bài viết tên trường và các từ khóa chính (học bổng, IELTS, du học Mỹ…). Muốn bỏ thì xóa trường, tag hoặc link tương ứng.</div>' +
            '<div id="neSchoolMsg" class="ne-hint" role="status"></div>';
        wrap.parentNode.insertBefore(box, wrap);
        loadSchools();
        document.getElementById('neSchoolAutoChk').addEventListener('change', function () {
            try { window.localStorage.setItem(SKEY, this.checked ? '1' : '0'); } catch (e) { /* bo qua */ }
        });
        document.getElementById('neSchoolScanBtn').addEventListener('click', function () {
            var res = schScan(); schMsg(res, false);
            if (res && !res.err && (res.added.length || res.linked.length || res.tagged.length)) setState('dirty', 'Chưa lưu');
        });
        // tu quet truoc khi luu (cac nut Luu / Gui deu co id chua "lbtSave"); chay o pha capture nen xong truoc khi bai duoc doc de luu
        document.addEventListener('click', function (e) {
            var b = e.target && e.target.closest ? e.target.closest('[id*="lbtSave"]') : null;
            var chk = document.getElementById('neSchoolAutoChk');
            if (!b || !chk || !chk.checked) return;
            try { var res = schScan(); if (!res.err) schMsg(res, true); } catch (err) { dbg('auto scan', err); }
        }, true);
    }

    // ---------------------------------------------------------------- Hang nut tren khung Noi dung (giong NVCMS v3)
    // Chen anh / tep (mo khung Anh & tep), 2 anh cung hang (chon 2 anh da tai len cua bai), Tin lien quan (mo hop chon tin), Chen box (menu co icon).
    var CK_IMG_RX = /\.(png|jpe?g|gif|jfif|bmp|webp)(\?|#|$)/i;
    function neEd() { var c = window.NE_CFG; return (c && window.CKEDITOR && window.CKEDITOR.instances[c.editor]) || null; }
    function escH(s) { return String(s == null ? '' : s).replace(/[&<>"']/g, function (c) { return ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' })[c]; }); }
    function closeDd() {
        Array.prototype.forEach.call(document.querySelectorAll('.ne-dd.is-open'), function (d) {
            d.classList.remove('is-open'); var b = d.querySelector('[data-ne-dd]'); if (b) b.setAttribute('aria-expanded', 'false');
        });
    }
    // anh da tai len cua bai (cac nut "Chen vao bai" o khung Anh & tep), bo trung
    function mediaImages() {
        var seen = {}, out = [];
        Array.prototype.forEach.call(document.querySelectorAll('.anh-addToContent[data-img]'), function (a) {
            var u = a.getAttribute('data-img');
            if (!u || seen[u] || !CK_IMG_RX.test(u)) return;
            seen[u] = 1; out.push({ url: u, name: a.getAttribute('data-title') || '' });
        });
        return out;
    }
    function insertImgRow(ed, a, b) {
        var fig = function (u) {
            return '<figure style="flex:1 1 220px;margin:0;text-align:center;"><img alt="" src="' + escH(u) + '" style="width:100%;height:auto;display:block;" /><figcaption style="font-size:.85em;color:#6B7A99;margin-top:.4em;">Chú thích ảnh</figcaption></figure>';
        };
        ed.focus(); ed.fire('saveSnapshot');
        ed.insertHtml('<div style="display:flex;flex-wrap:wrap;gap:12px;margin:1.2em 0;">' + fig(a) + fig(b) + '</div><p></p>', 'unfiltered_html');
        window.formModified = true; ed.fire('saveSnapshot');
    }
    function imgRowPicker() {
        var ed = neEd(); if (!ed) return;
        var old = document.getElementById('neImgRowModal'); if (old && old.parentNode) old.parentNode.removeChild(old);
        var imgs = mediaImages(), sel = [], body;
        if (!imgs.length) body = '<p class="ne-modal-empty">Bài chưa có ảnh nào. Hãy tải ảnh lên ở khung <strong>Ảnh &amp; tệp</strong> trước, rồi bấm lại nút này.</p>';
        else body = '<p class="ne-modal-hint">Bấm chọn <strong>2 ảnh</strong>. Ảnh chọn trước nằm bên trái, bấm lại để bỏ chọn.</p><div class="ne-modal-grid">' + imgs.map(function (x, i) {
            return '<button type="button" class="ne-pick" data-i="' + i + '" title="' + escH(x.name) + '"><img src="' + escH(x.url) + '?width=240&amp;height=160&amp;mode=crop&amp;anchor=middle" alt=""><span class="ne-pick-no"></span></button>';
        }).join('') + '</div>';
        var m = document.createElement('div'); m.id = 'neImgRowModal'; m.className = 'ne-modal';
        m.innerHTML = '<div class="ne-modal-box" role="dialog" aria-modal="true" aria-label="Chèn 2 ảnh cùng hàng"><div class="ne-modal-head"><strong>2 ảnh cùng hàng</strong><button type="button" class="ne-modal-x" data-m="close" aria-label="Đóng">&times;</button></div>' +
            '<div class="ne-modal-body">' + body + '</div><div class="ne-modal-foot">' +
            (imgs.length ? '<button type="button" class="btn btn-sm btn-primary" data-m="ok" disabled>Chèn 2 ảnh</button>' : '<button type="button" class="btn btn-sm btn-primary" data-m="media">Mở khung Ảnh &amp; tệp</button>') +
            '<button type="button" class="btn btn-sm btn-outline-light" data-m="close">Hủy</button></div></div>';
        document.body.appendChild(m);
        function onKey(e) { if (e.key === 'Escape') close(); }
        function close() { if (m.parentNode) m.parentNode.removeChild(m); document.removeEventListener('keydown', onKey); }
        document.addEventListener('keydown', onKey);
        function paint() {
            Array.prototype.forEach.call(m.querySelectorAll('.ne-pick'), function (b) {
                var p = sel.indexOf(+b.getAttribute('data-i'));
                b.classList.toggle('is-on', p >= 0); b.querySelector('.ne-pick-no').textContent = p >= 0 ? String(p + 1) : '';
            });
            var ok = m.querySelector('[data-m="ok"]'); if (ok) ok.disabled = sel.length !== 2;
        }
        m.addEventListener('click', function (e) {
            var t = e.target, a;
            if (t === m || t.closest('[data-m="close"]')) { close(); return; }
            if (t.closest('[data-m="media"]')) { close(); openMedia(); return; }
            if (t.closest('[data-m="ok"]')) { if (sel.length === 2) { insertImgRow(ed, imgs[sel[0]].url, imgs[sel[1]].url); close(); } return; }
            a = t.closest('.ne-pick');
            if (a) { var i = +a.getAttribute('data-i'), p = sel.indexOf(i); if (p >= 0) sel.splice(p, 1); else if (sel.length >= 2) sel[1] = i; else sel.push(i); paint(); }
        });
    }
    function initContentBar(r) {
        var ta = q('textarea[id$="teContent"]', r);
        if (!ta || document.getElementById('neContentBar')) return;
        var wrap = ta.closest ? ta.closest('.form-control-wrap') : ta.parentNode;
        if (!wrap || !wrap.parentNode) return;
        var items = NE_BOXES.map(function (b) {
            return '<li><a href="#" role="menuitem" data-ne-box="' + escH(b.key) + '" title="' + escH(b.description) + '"><em class="icon ni ' + escH(b.icon || 'ni-template') + '"></em><span>' + escH(b.label) + '</span></a></li>';
        }).join('');
        var bar = document.createElement('div'); bar.id = 'neContentBar'; bar.className = 'ne-content-label';
        bar.innerHTML = '<label class="form-label">Nội dung</label><div class="ne-content-tools">' +
            '<button type="button" class="btn btn-sm btn-white btn-outline-light" data-ne-act="media"><em class="icon ni ni-img"></em><span>Chèn ảnh / tệp</span></button>' +
            '<button type="button" class="btn btn-sm btn-white btn-outline-light" data-ne-act="imgrow" title="Chèn 2 ảnh nằm cạnh nhau trên cùng một hàng"><em class="icon ni ni-view-col"></em><span>2 ảnh cùng hàng</span></button>' +
            '<button type="button" class="btn btn-sm btn-white btn-outline-light" data-ne-act="related" title="Chọn tin liên quan"><em class="icon ni ni-link-alt"></em><span>Tin liên quan</span></button>' +
            '<div class="ne-dd"><button type="button" class="btn btn-sm btn-white btn-outline-light" data-ne-dd aria-haspopup="true" aria-expanded="false"><em class="icon ni ni-template"></em><span>Chèn box</span><em class="icon ni ni-chevron-down"></em></button>' +
            '<div class="ne-dd-menu" role="menu"><ul>' + items + '</ul></div></div></div>';
        wrap.parentNode.insertBefore(bar, wrap);
        // trang kieu cu con dong tieu de "Noi dung bai viet" ngay phia tren: an di cho khoi trung voi nhan "Noi dung" cua hang nut
        var grp = wrap.parentNode, prev = grp && grp.previousElementSibling;
        if (prev && prev.classList.contains('card-title-group') && /N\u1ed9i dung/i.test(prev.textContent || '')) prev.style.display = 'none';
        bar.addEventListener('click', function (e) {
            var t = e.target, a;
            if (!t.closest) return;
            if ((a = t.closest('[data-ne-box]'))) { e.preventDefault(); var ed = neEd(); if (ed) boxInsert(ed, a.getAttribute('data-ne-box')); closeDd(); return; }
            if ((a = t.closest('[data-ne-dd]'))) {
                e.preventDefault();
                var dd = a.parentNode, open = !dd.classList.contains('is-open');
                closeDd(); if (open) { dd.classList.add('is-open'); a.setAttribute('aria-expanded', 'true'); }
                return;
            }
            if ((a = t.closest('[data-ne-act]'))) {
                e.preventDefault();
                var k = a.getAttribute('data-ne-act');
                if (k === 'media') openMedia();
                else if (k === 'imgrow') imgRowPicker();
                else if (k === 'related' && typeof window.popupwindow === 'function') window.popupwindow(900, 600);
            }
        });
        document.addEventListener('click', function (e) { if (!e.target.closest || !e.target.closest('.ne-dd')) closeDd(); });
        document.addEventListener('keydown', function (e) { if (e.key === 'Escape') closeDd(); });
    }

    // ---------------------------------------------------------------- dung giao dien
    function init() {
        var r = root(); if (!r) return;
        var ul = q('.tt-ed-actions', r);
        if (ul && !document.getElementById('neTools')) {
            var li = document.createElement('li'); li.id = 'neTools'; li.className = 'ne-tools-li';
            li.innerHTML = '<span id="neState" class="ne-state is-saved" role="status" aria-live="polite">Đã lưu</span>' +
                '<button type="button" id="neFocusBtn" class="btn btn-sm btn-outline-light ne-focus-btn" title="Chế độ tập trung: ẩn khung bên phải, chỉ còn tiêu đề, tóm tắt và nội dung"><em class="icon ni ni-maximize"></em><span>Tập trung</span></button>';
            var del = ul.querySelector('li[style*="float"]');
            ul.insertBefore(li, del || null);
            document.getElementById('neFocusBtn').addEventListener('click', function () { setFocus(!r.classList.contains('ne-focus')); });
        }
        var on = false; try { on = window.localStorage.getItem(FKEY) === '1'; } catch (e) { /* bo qua */ }
        if (on) setFocus(true);
        // chua luu khi go vao o nhap
        r.addEventListener('input', function (e) { var t = e.target; if (t && /^(INPUT|TEXTAREA)$/.test(t.tagName) && t.type !== 'file') setState('dirty', 'Chưa lưu'); });
        // dem ky tu tom tat
        var s = q('textarea[id$="txtSummary"]', r);
        if (s && !s.__neC) {
            s.__neC = 1;
            var c = document.createElement('div'); c.className = 'ne-count'; s.parentNode.appendChild(c);
            var upd = function () { var n = s.value.length; c.textContent = n + ' ký tự' + (n > 160 ? ' — nên ≤ 160 để hiển thị tốt khi chia sẻ / trên Google' : ''); c.classList.toggle('is-over', n > 160); };
            s.addEventListener('input', upd); upd();
        }
        // tieu de / tom tat: dung placeholder thay cho nhan noi (nhan de bi dai, lech khi chu to)
        Array.prototype.forEach.call(r.querySelectorAll('.tt-card-write label.form-label-outlined'), function (l) {
            var id = l.getAttribute('for'), inp = id && document.getElementById(id);
            if (inp && !inp.placeholder) inp.placeholder = (l.textContent || '').trim();
            l.classList.add('ne-label-hidden');
        });
        // bai moi chua co tieu de: dua con tro vao tieu de
        var t = q('[id$="txtTitle"]', r); if (t && !t.value) { try { t.focus(); } catch (e) { /* bo qua */ } }
        try { tidyLayout(r); } catch (e) { dbg('tidyLayout', e); }
        try { initContentBar(r); } catch (e) { dbg('contentBar', e); }
        try { initSchoolAuto(r); } catch (e) { dbg('schoolAuto', e); }
        hookSave(); syncTop();
        window.addEventListener('resize', syncTop);
        window.addEventListener('load', function () { hookSave(); syncTop(); });
        // Ctrl+S ngoai khung soan
        document.addEventListener('keydown', function (e) { if ((e.ctrlKey || e.metaKey) && (e.key === 's' || e.key === 'S')) { e.preventDefault(); saveNow(); } });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
