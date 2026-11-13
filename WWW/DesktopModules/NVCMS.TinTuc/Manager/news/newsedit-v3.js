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
    // Cau hinh rieng cho man soan tin (khong dung config.js dung chung voi cac module khac)
    window.neEditorConfig = function () {
        registerPlugin();
        return {
            height: Math.max(560, window.innerHeight - 280),
            toolbarCanCollapse: false,
            toolbarStartupExpanded: true,
            toolbar: [
                { name: 'format', items: ['Format'] },
                { name: 'basic', items: ['Bold', 'Italic', 'Underline', 'Strike', 'RemoveFormat'] },
                { name: 'lists', items: ['NumberedList', 'BulletedList', 'Blockquote'] },
                { name: 'align', items: ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
                { name: 'links', items: ['Link', 'Unlink'] },
                { name: 'insert', items: ['NeMedia', 'Image', 'Youtube', 'oembed', 'Table'] },
                { name: 'history', items: ['Undo', 'Redo'] },
                { name: 'view', items: ['Maximize', 'Source'] },
                '/',
                { name: 'style2', items: ['Style', 'TextColor', 'BGColor', 'Subscript', 'Superscript'] },
                { name: 'para2', items: ['Outdent', 'Indent', 'HorizontalRule', 'SpecialChar', 'PageBreak'] },
                { name: 'clip', items: ['PasteText', 'PasteFromWord', 'CopyFormatting', 'Find', 'Replace', 'ShowBlocks'] },
                { name: 'biz', items: ['Templates', 'dangkydo', 'dangkydoen'] }
            ],
            // giu cac plugin cua config.js (extraPlugins cua instance ghi de config.js) va bat them dem tu
            extraPlugins: 'youtube,oembed,filetools,uploadwidget,dangkydo,dangkydoen,chuky,chukyen,nemedia,wordcount',
            removePlugins: 'easyimage',
            format_tags: 'p;h2;h3;h4',
            contentsCss: [CKEDITOR.basePath + 'contents.css', BASE + 'newsedit-content.css'],
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
        hookSave(); syncTop();
        window.addEventListener('resize', syncTop);
        window.addEventListener('load', function () { hookSave(); syncTop(); });
        // Ctrl+S ngoai khung soan
        document.addEventListener('keydown', function (e) { if ((e.ctrlKey || e.metaKey) && (e.key === 's' || e.key === 'S')) { e.preventDefault(); saveNow(); } });
    }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', init); else init();
})();
