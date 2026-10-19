/*!
 * NVCMS.Marketing - Phân tích Marketing (Google Analytics 4) cho trang chủ quản trị (skin _admin/controls/index/U_NhuanBut.ascx).
 * API: /DesktopModules/NVCMS/API/MarketingAnalytics/{Init, Overview, Content, Article, Acquisition, Campaigns, Campaign, Audience}
 * (NVCMS.API - Controller/Analytics; khóa GA4 chỉ ở máy chủ, cấu hình ở Cấu hình website › Google Analytics).
 * Phần 1: tiện ích nền (gọi API kèm anti-forgery DNN, sidebar, toast, CSV, URL) - không phụ thuộc jQuery.
 * Phần 2: màn hình 6 tab: Tổng quan · Gợi ý hành động · Nội dung · Nguồn & UTM · Chiến dịch · Khán giả.
 */
(function (global) {
    'use strict';

    var A = {};

    A.esc = function (s) {
        return String(s === null || s === undefined ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    };
    function pad2(n) { return (n < 10 ? '0' : '') + n; }
    /** ISO -> Date; ngày rỗng của SQL/DNN (<= 1900) -> null. */
    A.parseDate = function (iso) {
        if (!iso) return null;
        if (iso instanceof Date) return iso;
        var m = /^(\d{4})-(\d{2})-(\d{2})(?:[T ](\d{2}):(\d{2})(?::(\d{2}))?)?/.exec(String(iso));
        if (!m || +m[1] <= 1900) return null;
        return new Date(+m[1], +m[2] - 1, +m[3], +(m[4] || 0), +(m[5] || 0), +(m[6] || 0));
    };
    A.fmtDate = function (iso, withTime) {
        var d = A.parseDate(iso);
        if (!d) return '';
        var s = pad2(d.getDate()) + '/' + pad2(d.getMonth() + 1) + '/' + d.getFullYear();
        return withTime ? s + ' ' + pad2(d.getHours()) + ':' + pad2(d.getMinutes()) : s;
    };
    A.pageList = function (page, totalPages) {
        var out = [], i;
        if (totalPages <= 7) { for (i = 1; i <= totalPages; i++) out.push(i); return out; }
        out.push(1);
        var from = Math.max(2, page - 1), to = Math.min(totalPages - 1, page + 1);
        if (from > 2) out.push('…');
        for (i = from; i <= to; i++) out.push(i);
        if (to < totalPages - 1) out.push('…');
        out.push(totalPages);
        return out;
    };
    A.pager = function (page, pageSize, total) {
        var pages = Math.max(1, Math.ceil((total || 0) / (pageSize || 1)));
        if (pages <= 1) return '';
        var html = '<ul class="pagination justify-content-center justify-content-md-start">';
        html += '<li class="page-item' + (page <= 1 ? ' disabled' : '') + '"><a class="page-link" href="#" data-page="' + (page - 1) + '">Trước</a></li>';
        A.pageList(page, pages).forEach(function (p) {
            if (p === '…') html += '<li class="page-item"><span class="page-link"><em class="icon ni ni-more-h"></em></span></li>';
            else html += '<li class="page-item' + (p === page ? ' active' : '') + '"><a class="page-link" href="#" data-page="' + p + '">' + p + '</a></li>';
        });
        html += '<li class="page-item' + (page >= pages ? ' disabled' : '') + '"><a class="page-link" href="#" data-page="' + (page + 1) + '">Sau</a></li>';
        return html + '</ul>';
    };

    /** Gọi DNN Web API: kèm TabId / ModuleId và RequestVerificationToken (ValidateAntiForgeryToken). Promise {data, message}; lỗi -> {status, message}. */
    A.api = function (root, el) {
        var moduleId = el ? el.getAttribute('data-module-id') : '', tabId = el ? el.getAttribute('data-tab-id') : '';
        function token() {
            var t = global.document.querySelector('input[name="__RequestVerificationToken"]');
            return t ? t.value : '';
        }
        function qs(params) {
            var parts = [];
            Object.keys(params || {}).forEach(function (k) {
                var v = params[k];
                if (v === null || v === undefined || v === '') return;
                parts.push(encodeURIComponent(k) + '=' + encodeURIComponent(v));
            });
            return parts.length ? '?' + parts.join('&') : '';
        }
        return {
            get: function (action, params) {
                var headers = { 'Accept': 'application/json', 'RequestVerificationToken': token() };
                if (+tabId > 0) headers.TabId = tabId;
                if (+moduleId > 0) headers.ModuleId = moduleId;
                return global.fetch(root + action + qs(params), { method: 'GET', headers: headers, credentials: 'same-origin' }).then(function (res) {
                    return res.text().then(function (txt) { return A.unwrap(res.status, res.ok, txt); });
                });
            }
        };
    };
    /** ApiResponse (camelCase hoặc PascalCase) -> {data, message}; lỗi -> throw {status, message}. */
    A.unwrap = function (status, ok, txt) {
        var json = null;
        try { json = txt ? JSON.parse(txt) : null; } catch (e) { json = null; }
        var pick = function (a, b) { return json && typeof json === 'object' ? (json[a] !== undefined ? json[a] : json[b]) : undefined; };
        if (!ok || !json || pick('success', 'Success') === false) {
            var msg = pick('message', 'Message') || (typeof json === 'string' ? json : '') || (status === 401 ? 'Phiên đăng nhập đã hết, hãy tải lại trang.' : 'Lỗi ' + status);
            throw { status: status, message: msg };
        }
        return { data: pick('data', 'Data'), message: pick('message', 'Message') };
    };
    A.toast = function (msg, type) {
        type = type || 'success';
        if (global.NioApp && global.NioApp.Toast) { global.NioApp.Toast(A.esc(msg), type, { position: 'top-right' }); return; }
        if (global.console) global.console.log('[' + type + '] ' + msg);
    };
    A.fail = function (err) { A.toast((err && err.message) || 'Có lỗi xảy ra.', 'error'); };

    /** Sidebar trượt từ phải (xem chi tiết), Esc / bấm nền mờ / nút đóng để đóng. opt: {title, subtitle, body, size: md|lg|xl}. */
    var sidebars = [];
    A.sidebar = function (opt) {
        opt = opt || {};
        var doc = global.document, level = sidebars.length;
        var overlay = doc.createElement('div');
        overlay.className = 'nvga-sb-overlay';
        var el = doc.createElement('aside');
        el.className = 'nvga-sb nvga-sb-' + (opt.size || 'md');
        el.setAttribute('role', 'dialog');
        el.setAttribute('aria-modal', 'true');
        el.tabIndex = -1;
        overlay.style.zIndex = 1035 + level * 10;
        el.style.zIndex = 1040 + level * 10;
        el.innerHTML = '<div class="nvga-sb-head"><div><h5 class="title mb-0" data-sb="title"></h5><span class="sub-text" data-sb="sub"></span></div>' +
            '<a href="#" class="btn btn-icon btn-trigger" data-dismiss="sidebar" aria-label="Đóng"><em class="icon ni ni-cross"></em></a></div>' +
            '<div class="nvga-sb-body">' + (opt.body || '') + '</div>';
        doc.body.appendChild(overlay);
        doc.body.appendChild(el);
        doc.body.classList.add('nvga-sb-open');
        var closed = false, api = {
            el: el,
            body: el.querySelector('.nvga-sb-body'),
            setTitle: function (title, sub) {
                el.querySelector('[data-sb="title"]').textContent = title || '';
                var s = el.querySelector('[data-sb="sub"]');
                s.textContent = sub || '';
                s.hidden = !sub;
            },
            close: function () {
                if (closed) return;
                closed = true;
                el.classList.remove('is-open');
                overlay.classList.remove('show');
                var i = sidebars.indexOf(api);
                if (i >= 0) sidebars.splice(i, 1);
                if (!sidebars.length) doc.body.classList.remove('nvga-sb-open');
                global.setTimeout(function () {
                    if (el.parentNode) el.parentNode.removeChild(el);
                    if (overlay.parentNode) overlay.parentNode.removeChild(overlay);
                }, 320);
            }
        };
        api.setTitle(opt.title, opt.subtitle);
        el.addEventListener('click', function (e) {
            var d = e.target.closest && e.target.closest('[data-dismiss="sidebar"]');
            if (d) { e.preventDefault(); api.close(); }
        });
        overlay.addEventListener('click', function () { api.close(); });
        sidebars.push(api);
        global.requestAnimationFrame(function () { el.classList.add('is-open'); overlay.classList.add('show'); el.focus(); });
        return api;
    };
    if (global.document) {
        global.document.addEventListener('keydown', function (e) {
            if ((e.key === 'Escape' || e.keyCode === 27) && sidebars.length) sidebars[sidebars.length - 1].close();
        });
    }

    var TT = { A: A, screens: {} };
    TT.debounce = function (fn, ms) {
        var t;
        return function () {
            var self = this, args = arguments;
            clearTimeout(t);
            t = setTimeout(function () { fn.apply(self, args); }, ms || 300);
        };
    };
    TT.dateInput = function (d) {
        if (!d) return '';
        if (!(d instanceof Date)) d = A.parseDate(d);
        if (!d) return '';
        return d.getFullYear() + '-' + pad2(d.getMonth() + 1) + '-' + pad2(d.getDate());
    };
    TT.ago = function (iso, now) {
        var d = A.parseDate(iso);
        if (!d) return '';
        var s = Math.round(((now || new Date()) - d) / 1000);
        if (s < 45) return 'vừa xong';
        if (s < 3600) return Math.max(1, Math.round(s / 60)) + ' phút trước';
        if (s < 86400) return Math.round(s / 3600) + ' giờ trước';
        return A.fmtDate(iso, true);
    };
    /** Mảng object -> CSV (UTF-8 BOM, ; phân tách cho Excel tiếng Việt), chặn CSV injection. */
    TT.toCsv = function (rows, cols) {
        var q = function (v) {
            var s = v === null || v === undefined ? '' : String(v);
            if (/^[=+\-@\t\r]/.test(s)) s = "'" + s;
            return '"' + s.replace(/"/g, '""') + '"';
        };
        var lines = [cols.map(function (c) { return q(c.title); }).join(';')];
        (rows || []).forEach(function (r) { lines.push(cols.map(function (c) { return q(c.fmt ? c.fmt(r[c.key], r) : r[c.key]); }).join(';')); });
        return '﻿' + lines.join('\r\n');
    };
    TT.download = function (filename, text) {
        var blob = new global.Blob([text], { type: 'text/csv;charset=utf-8' });
        var a = global.document.createElement('a');
        a.href = global.URL.createObjectURL(blob);
        a.download = filename;
        global.document.body.appendChild(a);
        a.click();
        setTimeout(function () { global.URL.revokeObjectURL(a.href); a.parentNode.removeChild(a); }, 500);
    };
    TT.query = function () {
        var o = {}, s = (global.location && global.location.search || '').replace(/^\?/, '');
        s.split('&').forEach(function (p) {
            if (!p) return;
            var i = p.indexOf('='), k = decodeURIComponent((i < 0 ? p : p.slice(0, i)).replace(/\+/g, ' '));
            o[k] = i < 0 ? '' : decodeURIComponent(p.slice(i + 1).replace(/\+/g, ' '));
        });
        return o;
    };
    TT.setQuery = function (params, replace) {
        if (!global.history || !global.history.replaceState) return;
        var q = TT.query();
        Object.keys(params).forEach(function (k) {
            var v = params[k];
            if (v === null || v === undefined || v === '' || v === false) delete q[k]; else q[k] = v;
        });
        var s = Object.keys(q).map(function (k) { return encodeURIComponent(k) + '=' + encodeURIComponent(q[k]); }).join('&');
        global.history[replace ? 'replaceState' : 'pushState'](global.history.state, '', global.location.pathname + (s ? '?' + s : '') + global.location.hash);
    };
    TT.ctx = function (root) {
        return {
            root: root,
            A: A,
            api: A.api(root.getAttribute('data-api-root'), root),
            attr: function (n) { return root.getAttribute('data-' + n) || ''; },
            block: function (msg) {
                root.innerHTML = '<div class="alert alert-pro alert-danger"><div class="alert-text"><h6>Không thể mở màn hình</h6><p>' + A.esc(msg || 'Có lỗi xảy ra.') + '</p></div></div>';
            }
        };
    };
    TT.register = function (name, fn) {
        var run = function () {
            Array.prototype.forEach.call(global.document.querySelectorAll('[data-nvga-screen="' + name + '"]'), function (root) {
                if (root.getAttribute('data-booted')) return;
                root.setAttribute('data-booted', '1');
                try { fn(root, TT.ctx(root)); }
                catch (e) { if (global.console) global.console.error(e); TT.ctx(root).block('Lỗi khởi tạo màn hình: ' + (e && e.message)); }
            });
        };
        if (global.document.readyState === 'loading') global.document.addEventListener('DOMContentLoaded', run); else run();
    };

    global.NVGA = TT;
})(typeof window !== 'undefined' ? window : this);

(function (global) {
    'use strict';

    var TT = global.NVGA;
    var A = TT.A, esc = A.esc;

    // ================================================================== M: hàm thuần
    var M = {};

    /** Thứ tự màu cố định (đã kiểm tra mù màu), dùng cùng bảng điều khiển tin tức; "kỳ trước" = xám. */
    M.PALETTE = ['#2a78d6', '#eb6834', '#1baf7a', '#eda100', '#e87ba4'];
    M.PREV = '#a9b4c4';
    M.SEQ = ['#cde2fb', '#9ec5f4', '#6da7ec', '#3987e5', '#256abf', '#184f95', '#0d366b'];
    M.EMPTY_CELL = '#f1f3f7';
    M.TABS = [
        { key: 'overview', name: 'Tổng quan', icon: 'ni-dashboard' },
        { key: 'insights', name: 'Gợi ý hành động', icon: 'ni-bulb' },
        { key: 'content', name: 'Nội dung', icon: 'ni-file-text' },
        { key: 'acquisition', name: 'Nguồn & UTM', icon: 'ni-link' },
        { key: 'campaigns', name: 'Chiến dịch', icon: 'ni-target' },
        { key: 'audience', name: 'Khán giả', icon: 'ni-users' }
    ];
    M.LEVELS = {
        danger: { name: 'Khẩn', icon: 'ni-alert-circle-fill', order: 0 },
        warning: { name: 'Cần xử lý', icon: 'ni-alert-fill', order: 1 },
        opportunity: { name: 'Cơ hội', icon: 'ni-trend-up', order: 2 },
        success: { name: 'Đang tốt', icon: 'ni-check-circle-fill', order: 3 },
        info: { name: 'Thông tin', icon: 'ni-info-fill', order: 4 }
    };
    M.AREAS = {
        traffic: 'Truy cập', channel: 'Kênh', seo: 'SEO', utm: 'UTM', audience: 'Khán giả', content: 'Nội dung',
        campaign: 'Chiến dịch', goal: 'Mục tiêu', setup: 'Cấu hình đo lường'
    };
    M.SEGMENTS = {
        star: { name: 'Ngôi sao', cls: 'badge-success', hint: 'Xem nhiều + đọc sâu: đẩy mạnh, viết tiếp chủ đề' },
        potential: { name: 'Tiềm năng', cls: 'badge-info', hint: 'Đọc rất sâu nhưng ít người xem: cần phân phối thêm' },
        optimize: { name: 'Cần tối ưu', cls: 'badge-warning', hint: 'Xem nhiều nhưng đọc lướt: sửa tiêu đề / nội dung / CTA' },
        normal: { name: 'Bình thường', cls: 'badge-light', hint: '' },
        low: { name: 'Ít dữ liệu', cls: 'badge-gray', hint: 'Dưới 5 lượt xem' }
    };
    M.TRENDS = {
        up: { name: 'Đang lên', icon: 'ni-arrow-up-right', cls: 'is-good' },
        down: { name: 'Đang giảm', icon: 'ni-arrow-down-right', cls: 'is-bad' },
        'new': { name: 'Mới', icon: 'ni-star', cls: 'is-neutral' }
    };
    M.MEDIUMS = ['cpc', 'paid-social', 'social', 'email', 'display', 'referral', 'affiliate', 'video', 'sms', 'push', 'qr', 'banner'];
    M.SOURCES = ['facebook', 'zalo', 'google', 'tiktok', 'youtube', 'instagram', 'linkedin', 'newsletter', 'messenger', 'coccoc'];

    M.num = function (n, digits, keep) {
        n = Number(n);
        if (!isFinite(n)) return '–';
        digits = digits || 0;
        var f = Math.abs(n).toFixed(digits), parts = f.split('.');
        var s = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, '.') + (parts[1] ? ',' + parts[1] : '');
        if (digits && !keep) s = s.replace(/,?0+$/, '');
        return (n < 0 && Number(f) !== 0 ? '-' : '') + s;
    };
    M.pct = function (ratio, digits) {
        if (ratio === null || ratio === undefined || !isFinite(Number(ratio))) return '–';
        return M.num(Number(ratio) * 100, digits === undefined ? 1 : digits) + '%';
    };
    M.compact = function (n) {
        n = Number(n) || 0;
        var a = Math.abs(n);
        if (a < 1000) return M.num(n, a < 10 ? 1 : 0);
        if (a < 1e6) return M.num(n / 1e3, a < 1e4 ? 1 : 0) + 'k';
        if (a < 1e9) return M.num(n / 1e6, a < 1e7 ? 1 : 0) + ' tr';
        return M.num(n / 1e9, 1) + ' tỷ';
    };
    function pad2(n) { return (n < 10 ? '0' : '') + n; }
    /** Giây -> "1:05" / "1:02:05". */
    M.duration = function (sec) {
        var s = Math.max(0, Math.round(Number(sec) || 0)), h = Math.floor(s / 3600), m = Math.floor((s % 3600) / 60), x = s % 60;
        return h ? h + ':' + pad2(m) + ':' + pad2(x) : m + ':' + pad2(x);
    };
    /** So kỳ trước: {dir, text, tone}; opt.points = tỷ lệ (chênh điểm %), opt.lowerBetter. */
    M.delta = function (cur, prev, opt) {
        opt = opt || {};
        if (prev === null || prev === undefined || cur === null || cur === undefined) return null;
        cur = Number(cur); prev = Number(prev);
        if (!isFinite(cur) || !isFinite(prev)) return null;
        var r = { dir: 'flat', text: '' };
        if (opt.points) {
            var d = (cur - prev) * 100;
            if (Math.abs(d) < 0.05) r.text = '0 điểm';
            else { r.dir = d > 0 ? 'up' : 'down'; r.text = (d > 0 ? '+' : '−') + M.num(Math.abs(d), 1) + ' điểm'; }
        } else if (prev === 0) {
            if (cur === 0) r.text = '0%';
            else { r.dir = cur > 0 ? 'up' : 'down'; r.text = 'mới'; }
        } else {
            var p = (cur - prev) / Math.abs(prev) * 100;
            if (Math.abs(p) < 0.5) r.text = '0%';
            else { r.dir = p > 0 ? 'up' : 'down'; r.text = (p > 0 ? '+' : '−') + M.num(Math.abs(p), Math.abs(p) < 10 ? 1 : 0) + '%'; }
        }
        r.tone = r.dir === 'flat' ? 'flat' : opt.neutral ? 'neutral' : ((r.dir === 'up') !== !!opt.lowerBetter ? 'good' : 'bad');
        return r;
    };
    M.niceScale = function (max, count) {
        count = count || 4;
        if (!(max > 0)) max = count;
        var raw = max / count, p = Math.pow(10, Math.floor(Math.log(raw) / Math.LN10)), n = raw / p;
        var steps = p < 10 ? [1, 2, 5, 10] : [1, 2, 2.5, 5, 10], m = 10;
        for (var i = 0; i < steps.length; i++) if (n <= steps[i] + 1e-9) { m = steps[i]; break; }
        var step = Math.max(1, m * p), top = Math.ceil(max / step - 1e-9) * step, ticks = [];
        for (var k = 0; k * step <= top + 1e-9; k++) ticks.push(k * step);
        return { max: top, ticks: ticks };
    };
    M.fold = function (s) { return String(s || '').normalize('NFD').replace(/[̀-ͯ]/g, '').replace(/đ/g, 'd').replace(/Đ/g, 'D').toLowerCase(); };
    M.rangeText = function (from, to) {
        var f = A.fmtDate(from), t = A.fmtDate(to);
        return f === t ? f : f + ' – ' + t;
    };
    M.dayLabel = function (iso) { var d = A.parseDate(iso); return d ? pad2(d.getDate()) + '/' + pad2(d.getMonth() + 1) : ''; };
    M.sortInsights = function (list) {
        return (list || []).slice().sort(function (a, b) {
            var x = (M.LEVELS[a.level] || M.LEVELS.info).order, y = (M.LEVELS[b.level] || M.LEVELS.info).order;
            return x - y || (b.impact || 0) - (a.impact || 0);
        });
    };
    /** Chuẩn hóa một giá trị UTM: chữ thường, khoảng trắng / ký tự lạ -> "-", bỏ dấu tiếng Việt. */
    M.utmValue = function (s) {
        return M.fold(s).trim().replace(/[^a-z0-9._\-]+/g, '-').replace(/-+/g, '-').replace(/^-|-$/g, '');
    };
    /** Dựng URL gắn UTM (giữ query sẵn có, thay utm_* cũ). Trả '' khi URL không phải http(s). */
    M.utmUrl = function (url, p) {
        url = String(url || '').trim();
        if (!/^https?:\/\/[^\s/]+/i.test(url)) return '';
        var hash = '', i = url.indexOf('#');
        if (i >= 0) { hash = url.slice(i); url = url.slice(0, i); }
        var q = url.indexOf('?'), base = q >= 0 ? url.slice(0, q) : url, qs = q >= 0 ? url.slice(q + 1) : '';
        var keep = qs ? qs.split('&').filter(function (x) { return x && !/^utm_(source|medium|campaign|content|term|id)=/i.test(x); }) : [];
        ['source', 'medium', 'campaign', 'content', 'term'].forEach(function (k) {
            var v = M.utmValue(p && p[k]);
            if (v) keep.push('utm_' + k + '=' + encodeURIComponent(v));
        });
        return base + (keep.length ? '?' + keep.join('&') : '') + hash;
    };
    /** Bài / trang: tuyệt đối http(s) hoặc đường dẫn tương đối; còn lại bỏ. */
    M.safeUrl = function (u) {
        u = String(u || '').trim();
        if (/^https?:\/\//i.test(u)) return u;
        if (/^\/(?!\/)/.test(u)) return u;
        return '';
    };

    if (typeof module !== 'undefined' && module.exports) { module.exports = M; return; }

    // ================================================================== màn hình
    TT.register('marketing', function (root, ctx) {
        var init = {}, st = {
            tab: 'overview', range: '28d', from: '', to: '',
            data: {}, err: {}, key: {}, busy: {},
            ins: { level: '', area: '' },
            ct: { group: 'article', q: '', segment: '', trend: '', categoryId: 0, sort: 'views', dir: 'desc', page: 1 },
            acq: { dim: 'channel', sort: 'sessions', dir: -1 },
            camp: { sort: 'sessions', dir: -1 },
            catSort: { key: 'views', dir: -1 }
        };
        var seq = {};
        var store = {
            get: function (k) { try { return JSON.parse(global.localStorage.getItem('nvc.mk.' + k)); } catch (e) { return null; } },
            set: function (k, v) { try { global.localStorage.setItem('nvc.mk.' + k, JSON.stringify(v)); } catch (e) { /* trình duyệt chặn lưu */ } }
        };

        ctx.api.get('Init').then(function (r) {
            init = r.data || {};
            var saved = store.get('range') || {};
            if (saved.range && (init.ranges || []).some(function (x) { return x.key === saved.range; })) { st.range = saved.range; st.from = saved.from || ''; st.to = saved.to || ''; }
            if (st.range === 'custom' && !(st.from && st.to)) st.range = '28d';
            var t = TT.query().mk || store.get('tab');
            if (M.TABS.some(function (x) { return x.key === t; })) st.tab = t;
            if (!init.configured) { setup(); return; }
            layout();
            show(st.tab);
            if (st.tab !== 'overview' && st.tab !== 'insights') load('overview');   // gợi ý (số trên tab) + trung bình site để tô màu bảng
            global.addEventListener('resize', TT.debounce(redraw, 200));
        }, function (err) { ctx.block(err && err.message); });

        function ref(r) { return root.querySelector('[data-ref="' + r + '"]'); }
        function params(extra) {
            var p = { range: st.range };
            if (st.range === 'custom') { p.from = st.from; p.to = st.to; }
            Object.keys(extra || {}).forEach(function (k) { p[k] = extra[k]; });
            return p;
        }
        function rangeKey() { return st.range + '|' + st.from + '|' + st.to; }

        // ------------------------------------------------------------ chưa cấu hình
        function setup() {
            root.innerHTML = '<div class="tt-dash-head"><div class="tt-dash-hello"><h3 class="nk-block-title page-title">Phân tích Marketing</h3>' +
                '<p class="text-soft">Số liệu từ Google Analytics 4</p></div></div>' +
                '<div class="card card-bordered tt-dash-setup"><div class="card-inner"><div class="tt-setup-ic"><em class="icon ni ni-bar-chart" aria-hidden="true"></em></div><div>' +
                '<h5>Chưa kết nối Google Analytics 4</h5>' +
                '<p>Quản trị vào <b>Cấu hình website › Google Analytics</b> và khai báo:</p>' +
                '<ol class="mk-steps"><li><b>Property ID</b> của GA4 (GA4 › Admin › Property details, dạng số 9–10 chữ số).</li>' +
                '<li>Tạo <b>service account</b> trong Google Cloud (bật <i>Google Analytics Data API</i>), tải khóa JSON rồi dán vào ô "Khóa service account" (hoặc nhập đường dẫn tệp trong App_Data).</li>' +
                '<li>Trong GA4 › Admin › Property access management, thêm email của service account với quyền <b>Viewer</b>.</li>' +
                '<li>(Nên có) Đánh dấu <b>Key event</b> cho các hành động chuyển đổi: gửi form, click hotline / Zalo, đăng ký.</li></ol>' +
                '<p class="text-soft mb-0">Khóa chỉ lưu ở máy chủ, không gửi xuống trình duyệt.</p>' +
                '</div></div></div>';
        }

        // ------------------------------------------------------------ khung
        function layout() {
            root.innerHTML =
                '<div class="tt-dash-head">' +
                '<div class="tt-dash-hello"><h3 class="nk-block-title page-title">Phân tích Marketing</h3>' +
                '<p class="text-soft" data-ref="sub">Google Analytics 4 · Property ' + esc(init.propertyId || '') + '</p></div>' +
                '<div class="tt-dash-upd"><span data-ref="live"></span><span class="tt-dash-stamp" data-ref="stamp" aria-live="polite"></span>' +
                '<button type="button" class="btn btn-white btn-outline-light tt-dash-refresh" data-act="refresh" title="Tải lại số liệu"><em class="icon ni ni-reload"></em><span>Làm mới</span></button></div>' +
                '</div>' +
                '<div class="tt-dash-tabs"><ul class="nav nav-tabs tt-tabs" role="tablist">' + M.TABS.map(function (t) {
                    return '<li class="nav-item"><a class="nav-link" href="#" role="tab" data-tab="' + t.key + '" aria-selected="false"><em class="icon ni ' + t.icon + '" aria-hidden="true"></em><span>' + esc(t.name) + '</span>' +
                        (t.key === 'insights' ? ' <span class="badge badge-pill badge-danger mk-tab-badge" data-ref="ins-badge" hidden></span>' : '') + '</a></li>';
                }).join('') + '</ul></div>' +
                '<div class="tt-dash-bar"><div class="tt-dash-range"><div class="tt-seg" role="group" aria-label="Khoảng thời gian">' + (init.ranges || []).map(function (r) {
                    return '<button type="button" class="tt-seg-btn" data-range="' + esc(r.key) + '" aria-pressed="false">' + esc(r.name) + '</button>';
                }).join('') + '</div>' +
                '<div class="tt-dash-custom" data-ref="custom" hidden><label><span>Từ</span><input type="date" class="form-control" data-f="from"></label>' +
                '<label><span>Đến</span><input type="date" class="form-control" data-f="to"></label></div></div>' +
                '<span class="mk-vs text-soft" data-ref="vs"></span></div>' +
                M.TABS.map(function (t) { return '<div class="tt-dash-pane" data-pane="' + t.key + '" role="tabpanel" hidden></div>'; }).join('');
            syncRange();
            root.addEventListener('click', onClick);
            root.addEventListener('change', onChange);
            root.addEventListener('input', TT.debounce(onInput, 350));
            root.addEventListener('pointermove', onPointer);
            root.addEventListener('pointerleave', hideTip);
        }

        function syncRange() {
            Array.prototype.forEach.call(root.querySelectorAll('[data-range]'), function (b) {
                var on = b.getAttribute('data-range') === st.range;
                b.classList.toggle('on', on);
                b.setAttribute('aria-pressed', on ? 'true' : 'false');
            });
            var c = ref('custom');
            c.hidden = st.range !== 'custom';
            c.querySelector('[data-f="from"]').value = st.from || TT.dateInput(new Date(Date.now() - 27 * 864e5));
            c.querySelector('[data-f="to"]').value = st.to || TT.dateInput(new Date());
        }

        function show(tab) {
            st.tab = tab;
            store.set('tab', tab);
            TT.setQuery({ mk: tab === 'overview' ? null : tab }, true);
            Array.prototype.forEach.call(root.querySelectorAll('[data-tab]'), function (a) {
                var on = a.getAttribute('data-tab') === tab;
                a.classList.toggle('active', on);
                a.setAttribute('aria-selected', on ? 'true' : 'false');
            });
            Array.prototype.forEach.call(root.querySelectorAll('[data-pane]'), function (p) { p.hidden = p.getAttribute('data-pane') !== tab; });
            load(tab === 'insights' ? 'overview' : tab);
            if (tab === 'insights') renderInsights();
        }

        // ------------------------------------------------------------ nạp dữ liệu
        var LOADERS = {
            overview: function () { return ctx.api.get('Overview', params()); },
            content: function () {
                var c = st.ct;
                return ctx.api.get('Content', params({ group: c.group, q: c.q, segment: c.segment, trend: c.trend, categoryId: c.categoryId || '', sort: c.sort, dir: c.dir, page: c.page, pageSize: 50 }));
            },
            acquisition: function () { return ctx.api.get('Acquisition', params({ dim: st.acq.dim })); },
            campaigns: function () { return ctx.api.get('Campaigns', params()); },
            audience: function () { return ctx.api.get('Audience', params()); }
        };
        function keyOf(name) {
            var k = rangeKey();
            if (name === 'content') k += '|' + JSON.stringify(st.ct);
            if (name === 'acquisition') k += '|' + st.acq.dim;
            return k;
        }
        function load(name, force) {
            var k = keyOf(name);
            if (!force && st.key[name] === k && (st.data[name] || st.busy[name])) { render(name); return; }
            st.key[name] = k;
            var my = seq[name] = (seq[name] || 0) + 1;
            st.busy[name] = true;
            pane(name).classList.add('tt-busy');
            if (!st.data[name]) pane(name).innerHTML = skeleton();
            LOADERS[name]().then(function (r) {
                if (seq[name] !== my) return;
                st.data[name] = r.data; st.err[name] = null;
            }, function (err) {
                if (seq[name] !== my) return;
                st.err[name] = err;
            }).then(function () {
                if (seq[name] !== my) return;
                st.busy[name] = false;
                pane(name).classList.remove('tt-busy');
                render(name);
                if (name === 'overview') { renderInsights(); stamp(); }
            });
        }
        function pane(name) { return root.querySelector('[data-pane="' + name + '"]'); }
        function skeleton() {
            var k = '';
            for (var i = 0; i < 4; i++) k += '<div class="card card-bordered tt-kpi"><div class="card-inner"><div class="tt-skeleton" style="width:60%"></div><div class="tt-skeleton mt-2" style="height:26px;width:80%"></div></div></div>';
            return '<div class="tt-kpis mt-2">' + k + '</div><div class="tt-skeleton mt-3" style="height:260px"></div>';
        }
        function render(name) {
            var p = pane(name);
            if (!p) return;
            if (st.err[name] && !st.data[name]) { p.innerHTML = errorCard(st.err[name], name); return; }
            if (st.err[name]) A.toast(st.err[name].message, 'error');
            if (!st.data[name]) return;
            ({ overview: renderOverview, content: renderContent, acquisition: renderAcq, campaigns: renderCampaigns, audience: renderAudience })[name]();
        }
        function redraw() {
            var n = st.tab === 'insights' ? null : st.tab;
            if (n && st.data[n]) render(n);
        }
        function stamp() {
            var o = st.data.overview && st.data.overview.overview;
            if (o) ref('stamp').textContent = 'Cập nhật ' + TT.ago(o.generatedAt);
            var rt = st.data.overview && st.data.overview.realtime;
            ref('live').innerHTML = rt ? '<span class="tt-live" title="' + esc('Người dùng hoạt động 30 phút qua' + (rt.pages && rt.pages.length ? ' - đang xem nhiều: ' + rt.pages.map(function (x) { return x.title; }).join(' · ') : '')) + '"><i class="tt-live-dot" aria-hidden="true"></i><b>' +
                esc(M.num(rt.activeUsers)) + '</b> đang hoạt động</span>' : '';
            if (o) ref('vs').textContent = M.rangeText(o.from, o.to) + ' · so với ' + M.rangeText(o.prevFrom, o.prevTo);
        }

        // ------------------------------------------------------------ phần tử dùng chung
        function errorCard(err, name) {
            return '<div class="alert alert-pro ' + (err && err.status === 403 ? 'alert-warning' : 'alert-danger') + ' tt-dash-msg mt-2"><div class="alert-text"><h6>' +
                (err && err.status === 403 ? 'Không có quyền xem' : 'Không tải được số liệu') + '</h6><p>' + esc((err && err.message) || 'Có lỗi xảy ra.') + '</p>' +
                (err && err.status === 403 ? '' : '<button type="button" class="btn btn-sm btn-white btn-outline-light mt-1" data-act="retry" data-name="' + name + '"><em class="icon ni ni-reload"></em><span>Thử lại</span></button>') +
                '</div></div>';
        }
        function deltaHtml(d, title) {
            if (!d) return '';
            var icon = d.dir === 'up' ? 'ni-arrow-up' : d.dir === 'down' ? 'ni-arrow-down' : 'ni-minus';
            return '<span class="tt-delta is-' + d.tone + '"' + (title ? ' title="' + esc(title) + '"' : '') + '><em class="icon ni ' + icon + '" aria-hidden="true"></em>' +
                '<span class="sr-only">' + (d.dir === 'up' ? 'Tăng ' : d.dir === 'down' ? 'Giảm ' : 'Không đổi ') + '</span>' + esc(d.text) + '</span>';
        }
        function kpi(o) {
            return '<div class="card card-bordered tt-kpi' + (o.hero ? ' is-hero' : '') + '"><div class="card-inner">' +
                '<div class="tt-kpi-label"><em class="icon ni ' + (o.icon || 'ni-activity') + '" aria-hidden="true"></em><span>' + esc(o.label) + '</span>' +
                (o.help ? '<em class="icon ni ni-info mk-help" title="' + esc(o.help) + '" aria-label="' + esc(o.help) + '"></em>' : '') + '</div>' +
                '<div class="tt-kpi-value">' + esc(o.value) + '</div>' +
                '<div class="tt-kpi-foot">' + deltaHtml(o.delta, o.prev ? 'Kỳ trước: ' + o.prev : '') + (o.delta ? '<span class="tt-kpi-vs">so với kỳ trước</span>' : '') + '</div>' +
                (o.sub ? '<div class="tt-kpi-sub">' + o.sub + '</div>' : '') + '</div></div>';
        }
        function card(r, title, body, opt) {
            opt = opt || {};
            return '<div class="card card-bordered tt-dcard' + (opt.cls ? ' ' + opt.cls : '') + '"><div class="card-inner">' +
                '<div class="tt-dcard-head"><h6 class="title">' + esc(title) + (opt.sub ? ' <small class="text-soft fw-normal">' + esc(opt.sub) + '</small>' : '') + '</h6>' +
                '<div class="tt-dcard-tools">' + (opt.tools || '') + '</div></div><div data-ref="' + r + '">' + (body || '') + '</div></div></div>';
        }
        function empty(text, icon) { return '<div class="tt-empty"><em class="icon ni ' + (icon || 'ni-bar-chart') + '" aria-hidden="true"></em><p>' + esc(text) + '</p></div>'; }
        function hbars(rows, opt) {
            opt = opt || {};
            if (!rows.length) return empty(opt.empty || 'Chưa có số liệu.');
            var max = 0;
            rows.forEach(function (r) { max = Math.max(max, Number(r.value) || 0); });
            return '<ul class="tt-hbars">' + rows.map(function (r) {
                var w = max ? Math.max(1, (Number(r.value) || 0) / max * 100) : 0;
                return '<li' + (r.act ? ' class="mk-click" ' + r.act : '') + '><div class="tt-hbar-top"><span class="tt-hbar-name" title="' + esc(r.name) + '">' + esc(r.name) + (r.sub ? ' <small>' + esc(r.sub) + '</small>' : '') + '</span>' +
                    '<span class="tt-hbar-val"><b>' + esc(r.valueText !== undefined ? r.valueText : M.num(r.value)) + '</b>' + (r.extra ? ' <span>' + r.extra + '</span>' : '') + '</span></div>' +
                    '<div class="tt-hbar-track"><span style="width:' + (Math.round(w * 10) / 10) + '%' + (r.color ? ';background:' + r.color : '') + '"></span></div></li>';
            }).join('') + '</ul>';
        }
        function stack(rows, label) {
            var total = rows.reduce(function (s, r) { return s + (Number(r.value) || 0); }, 0);
            if (!total) return empty('Chưa có số liệu.');
            var segs = rows.slice(0, 4), rest = rows.slice(4).reduce(function (s, r) { return s + (Number(r.value) || 0); }, 0);
            if (rest > 0) segs = segs.concat([{ name: 'Khác', value: rest, other: true }]);
            return '<div class="tt-stack" role="img" aria-label="' + esc(label) + '">' + segs.map(function (s, i) {
                var share = s.value / total;
                return '<span class="tt-seg-part" title="' + esc(s.name + ': ' + M.num(s.value) + ' (' + M.pct(share) + ')') + '" style="flex-grow:' + Math.max(0.0001, share) + ';background:' + (s.other ? '#b7c2d0' : M.PALETTE[i]) + '"></span>';
            }).join('') + '</div><ul class="tt-stack-legend">' + segs.map(function (s, i) {
                return '<li><i style="background:' + (s.other ? '#b7c2d0' : M.PALETTE[i]) + '"></i><span class="tt-sl-name">' + esc(s.name) + '</span><span class="tt-sl-val"><b>' + esc(M.num(s.value)) + '</b>' + esc(M.pct(s.value / total)) + '</span>' +
                    (s.extra ? '<span class="mk-sl-extra">' + s.extra + '</span>' : '') + '</li>';
            }).join('') + '</ul>';
        }
        function segBadge(s) { var x = M.SEGMENTS[s]; return x && s !== 'normal' && s !== 'low' ? '<span class="badge badge-dim ' + x.cls + '" title="' + esc(x.hint) + '">' + esc(x.name) + '</span>' : ''; }
        function trendBadge(t) { var x = M.TRENDS[t]; return x ? '<span class="tt-delta ' + x.cls + '"><em class="icon ni ' + x.icon + '" aria-hidden="true"></em>' + esc(x.name) + '</span>' : ''; }
        function linkOut(url, text) {
            var u = M.safeUrl(url);
            return u ? '<a class="mk-out" href="' + esc(u) + '" target="_blank" rel="noopener" title="Mở trang"><em class="icon ni ni-external" aria-hidden="true"></em><span class="sr-only">' + esc(text || 'Mở') + '</span></a>' : '';
        }

        /**
         * Bảng có sắp xếp: cols [{k, label, cls, val(row) -> số để sắp, html(row)}]; sort {key, dir}; opt.onRow(row, i) -> attr thêm vào tr.
         */
        function table(name, cols, rows, sort, opt) {
            opt = opt || {};
            var col = cols.filter(function (c) { return c.k === sort.key; })[0];
            if (col && col.val) rows = rows.slice().sort(function (a, b) { return ((col.val(a) || 0) - (col.val(b) || 0)) * sort.dir; });
            if (opt.limit) rows = rows.slice(0, opt.limit);
            if (!rows.length) return empty(opt.empty || 'Chưa có số liệu.');
            return '<div class="table-responsive mk-table"><table class="table tt-stable"><thead><tr>' + cols.map(function (c) {
                if (!c.val || opt.fixed) return '<th class="' + (c.cls || '') + '">' + esc(c.label) + '</th>';
                var on = sort.key === c.k;
                return '<th class="' + (c.cls || '') + '"' + (on ? ' aria-sort="' + (sort.dir > 0 ? 'ascending' : 'descending') + '"' : '') + '><button type="button" class="tt-sort' + (on ? ' on' : '') + '" data-act="sort" data-table="' + name + '" data-k="' + c.k + '">' +
                    esc(c.label) + '<em class="icon ni ' + (on && sort.dir > 0 ? 'ni-arrow-up' : 'ni-arrow-down') + '" aria-hidden="true"></em></button></th>';
            }).join('') + '</tr></thead><tbody>' + rows.map(function (r, i) {
                return '<tr' + (opt.onRow ? ' ' + opt.onRow(r, i) : '') + '>' + cols.map(function (c) { return '<td class="' + (c.cls || '') + '" data-th="' + esc(c.label) + '">' + c.html(r, i) + '</td>'; }).join('') + '</tr>';
            }).join('') + '</tbody></table></div>';
        }
        /** Cột chuẩn cho MkRow (phiên, người dùng, tương tác, chuyển đổi) - dùng ở nguồn, chiến dịch, khán giả. */
        function rowCols(nameLabel, nameHtml, opt) {
            opt = opt || {};
            var cols = [{ k: 'name', label: nameLabel, cls: 'mk-c-name', html: nameHtml }];
            cols.push({ k: 'sessions', label: 'Phiên', cls: 'text-right tt-num', val: function (r) { return r.sessions; }, html: function (r) {
                return '<b>' + esc(M.num(r.sessions)) + '</b>' + (r.prev ? '<div>' + deltaHtml(M.delta(r.sessions, r.prev.sessions), 'Kỳ trước: ' + M.num(r.prev.sessions)) + '</div>' : '');
            } });
            if (!opt.compact) {
                cols.push({ k: 'users', label: 'Người dùng', cls: 'text-right tt-num', val: function (r) { return r.users; }, html: function (r) { return esc(M.num(r.users)); } });
                cols.push({ k: 'newUserRate', label: 'Khách mới', cls: 'text-right tt-num', val: function (r) { return r.newUserRate; }, html: function (r) { return esc(M.pct(r.newUserRate, 0)); } });
            }
            cols.push({ k: 'engagementRate', label: 'Tỷ lệ tương tác', cls: 'text-right tt-num', val: function (r) { return r.engagementRate; }, html: function (r) { return engCell(r.engagementRate); } });
            cols.push({ k: 'avgEngagementSec', label: 'TG tương tác TB', cls: 'text-right tt-num', val: function (r) { return r.avgEngagementSec; }, html: function (r) { return esc(M.duration(r.avgEngagementSec)); } });
            if (!opt.compact) cols.push({ k: 'viewsPerSession', label: 'Trang / phiên', cls: 'text-right tt-num', val: function (r) { return r.viewsPerSession; }, html: function (r) { return esc(M.num(r.viewsPerSession, 2)); } });
            cols.push({ k: 'conversions', label: 'Chuyển đổi', cls: 'text-right tt-num', val: function (r) { return r.conversions; }, html: function (r) { return r.conversions ? '<b>' + esc(M.num(r.conversions)) + '</b>' : '<span class="text-soft">0</span>'; } });
            cols.push({ k: 'conversionRate', label: 'Tỷ lệ CĐ', cls: 'text-right tt-num', val: function (r) { return r.conversionRate; }, html: function (r) { return esc(M.pct(r.conversionRate, 2)); } });
            if (!opt.compact) cols.push({ k: 'share', label: 'Tỷ trọng', cls: 'mk-c-bar', val: function (r) { return r.share; }, html: function (r) {
                return '<div class="tt-cbar"><div class="tt-hbar-track"><span style="width:' + Math.max(1, Math.round((r.share || 0) * 1000) / 10) + '%"></span></div><span class="tt-num">' + esc(M.pct(r.share)) + '</span></div>';
            } });
            return cols;
        }
        /** Tỷ lệ tương tác tô theo trung bình site: thấp hơn 15 điểm -> đỏ, cao hơn 10 điểm -> xanh. */
        function engCell(v) {
            var o = st.data.overview && st.data.overview.overview, avg = o ? o.summary.engagementRate : null;
            var cls = avg === null ? '' : v < avg - 0.15 ? 'tt-bench is-bad' : v > avg + 0.1 ? 'tt-bench is-good' : '';
            return '<span class="' + cls + '"' + (avg !== null ? ' title="Trung bình site: ' + esc(M.pct(avg)) + '"' : '') + '>' + esc(M.pct(v)) + '</span>';
        }
        function csvBtn(act) { return '<button type="button" class="btn btn-sm btn-white btn-outline-light" data-act="' + act + '"><em class="icon ni ni-download"></em><span>CSV</span></button>'; }
        function rowsCsv(rows) {
            return TT.toCsv(rows, [
                { key: 'name', title: 'Tên' }, { key: 'sessions', title: 'Phiên' }, { key: 'prevSessions', title: 'Phiên kỳ trước', fmt: function (v, r) { return r.prev ? r.prev.sessions : ''; } },
                { key: 'users', title: 'Người dùng' }, { key: 'newUsers', title: 'Người dùng mới' }, { key: 'engagementRate', title: 'Tỷ lệ tương tác', fmt: function (v) { return M.pct(v); } },
                { key: 'avgEngagementSec', title: 'TG tương tác TB (giây)' }, { key: 'viewsPerSession', title: 'Trang / phiên' }, { key: 'conversions', title: 'Chuyển đổi' },
                { key: 'conversionRate', title: 'Tỷ lệ chuyển đổi', fmt: function (v) { return M.pct(v, 2); } }
            ]);
        }

        // ------------------------------------------------------------ biểu đồ đường (1 trục tung)
        /** cfg: {labels[], tips[], series:[{name, color, values[], dash, area}], fmt, h}. */
        function lineChart(box, cfg) {
            var w = Math.max(260, Math.floor(box.clientWidth || 600)), h = cfg.h || 220, n = cfg.labels.length;
            var max = 0;
            cfg.series.forEach(function (s) { s.values.forEach(function (v) { max = Math.max(max, Number(v) || 0); }); });
            var sc = M.niceScale(max, 4), fmt = cfg.axis || M.compact, labW = 0;
            sc.ticks.forEach(function (t) { labW = Math.max(labW, String(fmt(t)).length); });
            var padL = Math.round(labW * 6.4 + 12), padR = 10, padT = 8, padB = 24, cw = w - padL - padR, ch = h - padT - padB;
            var X = function (i) { return padL + (n <= 1 ? cw / 2 : i * cw / (n - 1)); };
            var Y = function (v) { return padT + ch - (Math.max(0, Number(v) || 0) / sc.max) * ch; };
            var svg = '';
            sc.ticks.forEach(function (t) {
                svg += '<line x1="' + padL + '" x2="' + (w - padR) + '" y1="' + Y(t).toFixed(1) + '" y2="' + Y(t).toFixed(1) + '" class="tt-gl' + (t === 0 ? ' tt-axis' : '') + '"/>' +
                    '<text x="' + (padL - 6) + '" y="' + (Y(t) + 4).toFixed(1) + '" class="tt-tick" text-anchor="end">' + esc(fmt(t)) + '</text>';
            });
            cfg.series.forEach(function (s) {
                var pts = s.values.map(function (v, i) { return X(i).toFixed(1) + ' ' + Y(v).toFixed(1); });
                if (!pts.length) return;
                if (s.area) svg += '<path d="M' + pts.join(' L') + ' L' + X(n - 1).toFixed(1) + ' ' + Y(0).toFixed(1) + ' L' + X(0).toFixed(1) + ' ' + Y(0).toFixed(1) + 'Z" fill="' + s.color + '" fill-opacity=".08"/>';
                svg += n > 1 ? '<path d="M' + pts.join(' L') + '" fill="none" stroke="' + s.color + '" stroke-width="2" stroke-linejoin="round" stroke-linecap="round"' + (s.dash ? ' stroke-dasharray="4 4"' : '') + '/>'
                    : '<circle cx="' + X(0) + '" cy="' + Y(s.values[0]) + '" r="4" fill="' + s.color + '"/>';
            });
            var maxLab = 0;
            cfg.labels.forEach(function (l) { maxLab = Math.max(maxLab, String(l).length); });
            var every = Math.max(1, Math.ceil(n / Math.max(1, Math.floor(cw / (maxLab * 6.4 + 14)))));
            cfg.labels.forEach(function (l, i) { if (i % every === 0 || i === n - 1 && n < 8) svg += '<text x="' + X(i).toFixed(1) + '" y="' + (h - 6) + '" class="tt-tick" text-anchor="middle">' + esc(l) + '</text>'; });
            svg += '<line class="tt-xhair" data-xh x1="0" x2="0" y1="' + padT + '" y2="' + (padT + ch) + '" visibility="hidden"/>';
            cfg.series.forEach(function (s, k) { svg += '<circle class="tt-hdot" data-hd="' + k + '" r="4" fill="' + s.color + '" stroke="#fff" stroke-width="2" visibility="hidden"/>'; });
            box.innerHTML = '<svg width="' + w + '" height="' + h + '" viewBox="0 0 ' + w + ' ' + h + '" role="img" aria-label="' + esc(cfg.aria || '') + '">' + svg + '</svg><div class="tt-tip" hidden></div>';
            box._chart = { n: n, X: X, Y: Y, padL: padL, cw: cw, cfg: cfg };
        }
        function legend(items) {
            return '<div class="tt-legend">' + items.map(function (it) {
                return '<span><i class="is-line" style="background:' + it.color + (it.dash ? ';background:repeating-linear-gradient(90deg,' + it.color + ' 0 4px,transparent 4px 7px)' : '') + '"></i>' + esc(it.name) + '</span>';
            }).join('') + '</div>';
        }
        function onPointer(e) {
            var box = e.target.closest && e.target.closest('.tt-chart');
            if (!box || !box._chart) { hideTip(); return; }
            if (curTip && curTip !== box) hideTip();
            var c = box._chart, rb = box.getBoundingClientRect(), x = e.clientX - rb.left;
            var i = c.n <= 1 ? 0 : Math.round((x - c.padL) / (c.cw / (c.n - 1)));
            i = Math.max(0, Math.min(c.n - 1, i));
            var tip = box.querySelector('.tt-tip'), fmt = c.cfg.fmt || M.num;
            tip.innerHTML = '<b>' + esc(c.cfg.tips ? c.cfg.tips[i] : c.cfg.labels[i]) + '</b>' + c.cfg.series.map(function (s) {
                return '<span class="tt-tip-row"><i class="tt-key" style="background:' + s.color + '"></i><strong>' + esc(fmt(s.values[i])) + '</strong> ' + esc(s.tipName || s.name) + '</span>';
            }).join('');
            tip.hidden = false;
            var px = c.X(i);
            tip.style.left = Math.max(0, Math.min(rb.width - tip.offsetWidth, px - tip.offsetWidth / 2)) + 'px';
            tip.style.top = '-8px';
            tip.style.transform = 'translateY(-100%)';
            var xh = box.querySelector('[data-xh]');
            xh.setAttribute('x1', px); xh.setAttribute('x2', px); xh.setAttribute('visibility', 'visible');
            c.cfg.series.forEach(function (s, k) {
                var d = box.querySelector('[data-hd="' + k + '"]');
                d.setAttribute('cx', px); d.setAttribute('cy', c.Y(s.values[i])); d.setAttribute('visibility', 'visible');
            });
            curTip = box;
        }
        var curTip = null;
        function hideTip() {
            if (!curTip) return;
            var t = curTip.querySelector('.tt-tip');
            if (t) t.hidden = true;
            Array.prototype.forEach.call(curTip.querySelectorAll('[data-xh],[data-hd]'), function (x) { x.setAttribute('visibility', 'hidden'); });
            curTip = null;
        }
        /** Bản đồ nhiệt thứ x giờ (7 x 24, Thứ Hai trước). */
        function heatmap(heat) {
            var max = 0, days = ['T2', 'T3', 'T4', 'T5', 'T6', 'T7', 'CN'], full = ['Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy', 'Chủ nhật'];
            (heat || []).forEach(function (r) { r.forEach(function (v) { max = Math.max(max, v); }); });
            if (!max) return empty('Chưa có số liệu theo giờ.');
            var color = function (v) { if (!v) return M.EMPTY_CELL; var i = Math.min(M.SEQ.length - 1, Math.floor(v / max * M.SEQ.length)); return M.SEQ[i]; };
            var head = '<tr><th></th>';
            for (var h = 0; h < 24; h++) head += '<th class="tt-hh">' + (h % 3 === 0 ? h : '') + '</th>';
            return '<div class="tt-heat"><table class="tt-heat-t"><caption class="sr-only">Phiên theo thứ và giờ</caption><thead>' + head + '</tr></thead><tbody>' + heat.map(function (r, d) {
                return '<tr><th scope="row">' + days[d] + '</th>' + r.map(function (v, hh) {
                    return '<td style="background:' + color(v) + '" title="' + esc(full[d] + ' ' + hh + 'h–' + (hh + 1) + 'h: ' + M.num(v) + ' phiên') + '"></td>';
                }).join('') + '</tr>';
            }).join('') + '</tbody></table></div><div class="tt-heat-legend"><span>Ít</span>' + M.SEQ.map(function (c) { return '<i style="background:' + c + '"></i>'; }).join('') + '<span>Nhiều (' + esc(M.num(max)) + ' phiên)</span></div>';
        }

        // ------------------------------------------------------------ Tổng quan
        function renderOverview() {
            var d = st.data.overview, o = d.overview, s = o.summary, p = o.previous, el = pane('overview');
            var ins = M.sortInsights(o.insights).filter(function (x) { return x.level !== 'info' && x.level !== 'success'; });
            var target = o.target;
            el.innerHTML =
                (ins.length ? '<div class="mk-ins-strip">' + insightList(ins.slice(0, 4), true) +
                    '<button type="button" class="btn btn-sm btn-link" data-act="tab" data-tab-go="insights">Xem tất cả ' + esc(M.num(o.insights.length)) + ' gợi ý<em class="icon ni ni-chevron-right"></em></button></div>' : '') +
                '<div class="tt-kpis mk-kpis">' +
                kpi({ hero: true, icon: 'ni-users', label: 'Người dùng hoạt động', value: M.num(s.users), delta: M.delta(s.users, p.users), prev: M.num(p.users), sub: '<span class="text-soft">' + esc(M.num(s.newUsers)) + ' người dùng mới (' + esc(M.pct(s.newUserRate, 0)) + ')</span>' }) +
                kpi({ icon: 'ni-activity', label: 'Phiên truy cập', value: M.num(s.sessions), delta: M.delta(s.sessions, p.sessions), prev: M.num(p.sessions) }) +
                kpi({ icon: 'ni-eye', label: 'Lượt xem trang', value: M.num(s.views), delta: M.delta(s.views, p.views), prev: M.num(p.views), sub: '<span class="text-soft">' + esc(M.num(s.viewsPerSession, 2)) + ' trang / phiên</span>' }) +
                kpi({ icon: 'ni-hot', label: 'Tỷ lệ tương tác', value: M.pct(s.engagementRate), delta: M.delta(s.engagementRate, p.engagementRate, { points: true }), prev: M.pct(p.engagementRate),
                    help: 'Phiên kéo dài trên 10 giây, hoặc có chuyển đổi, hoặc xem từ 2 trang. Tỷ lệ thoát = 100% − tỷ lệ tương tác.' }) +
                kpi({ icon: 'ni-clock', label: 'Thời gian tương tác TB', value: M.duration(s.avgEngagementSec), delta: M.delta(s.avgEngagementSec, p.avgEngagementSec), prev: M.duration(p.avgEngagementSec), help: 'Thời gian trang ở trạng thái hiển thị / người dùng.' }) +
                kpi({ icon: 'ni-target', label: 'Chuyển đổi', value: M.num(s.conversions), delta: o.hasConversions ? M.delta(s.conversions, p.conversions) : null, prev: M.num(p.conversions),
                    sub: o.hasConversions ? '<span class="text-soft">' + esc(M.pct(s.conversionRate, 2)) + ' / phiên</span>' : '<span class="text-soft">Chưa có key event</span>',
                    help: (init.keyEvents && init.keyEvents.length ? 'Sự kiện: ' + init.keyEvents.join(', ') : 'Tổng key event của GA4') }) +
                (target ? kpi({ icon: 'ni-flag', label: target.month ? 'Mục tiêu tháng (dự kiến)' : 'Tiến độ mục tiêu', value: M.pct(target.pace, 0),
                    sub: '<div class="tt-meter"><span style="width:' + Math.min(100, Math.round(target.pace * 100)) + '%;background:' + (target.pace >= 1 ? '#1baf7a' : target.pace >= 0.9 ? '#eda100' : '#e85347') + '"></span></div>' +
                        '<span class="text-soft">' + esc(target.month ? 'Dự kiến ' + M.num(target.projected) + ' / ' + M.num(target.goal) + ' phiên' : M.num(target.actual) + ' / ' + M.num(target.expected) + ' phiên cần có') + '</span>' })
                    : kpi({ icon: 'ni-repeat', label: 'Khách quay lại', value: M.pct(returningShare(o.userTypes), 0), sub: '<span class="text-soft">tỷ lệ người dùng</span>' })) +
                '</div>' +
                '<div class="tt-grid tt-g21">' + card('o-series', 'Phiên & lượt xem theo ngày', '', { sub: 'nét đứt = kỳ trước' }) + card('o-channels', 'Kênh truy cập', '') + '</div>' +
                '<div class="tt-grid tt-g21">' + card('o-top', 'Bài viết xem nhiều', '', { tools: '<button type="button" class="btn btn-sm btn-link" data-act="tab" data-tab-go="content">Tất cả bài</button>' }) +
                card('o-dev', 'Thiết bị & khách hàng', '') + '</div>' +
                '<div class="tt-grid tt-g11">' + card('o-camp', 'Chiến dịch (utm_campaign)', '', { tools: '<button type="button" class="btn btn-sm btn-link" data-act="tab" data-tab-go="campaigns">Chi tiết</button>' }) +
                card('o-sm', 'Nguồn / Phương tiện', '', { tools: '<button type="button" class="btn btn-sm btn-link" data-act="tab" data-tab-go="acquisition">Phân tích UTM</button>' }) + '</div>' +
                '<div class="tt-grid tt-g21">' + card('o-heat', 'Khung giờ truy cập (thứ × giờ)', '') + card('o-cats', 'Chuyên mục: cầu & cung', '') + '</div>' +
                '<div class="tt-grid tt-g11">' + card('o-types', 'Hiệu quả theo loại trang', '', { sub: 'theo trang vào đầu tiên' }) + card('o-landing', 'Trang đích (trang vào đầu tiên)', '') + '</div>' +
                '<div class="tt-grid tt-g11">' + card('o-geo', 'Quốc gia', '') + card('o-thanks', 'Lead từ form đăng ký tư vấn', '') + '</div>';

            // biểu đồ ngày
            var box = ref('o-series');
            if (!o.series.length) box.innerHTML = empty('Chưa có số liệu theo ngày.');
            else {
                box.innerHTML = legend([{ name: 'Phiên', color: M.PALETTE[0] }, { name: 'Lượt xem', color: M.PALETTE[1] }, { name: 'Phiên kỳ trước', color: M.PREV, dash: true }]) + '<div class="tt-chart"></div>' +
                    dataTable(['Ngày', 'Phiên', 'Lượt xem', 'Người dùng', 'Chuyển đổi', 'Phiên kỳ trước'], o.series.map(function (x) { return [A.fmtDate(x.date), M.num(x.sessions), M.num(x.views), M.num(x.users), M.num(x.conversions), M.num(x.prevSessions)]; }));
                lineChart(box.querySelector('.tt-chart'), {
                    labels: o.series.map(function (x) { return M.dayLabel(x.date); }), tips: o.series.map(function (x) { return A.fmtDate(x.date); }), aria: 'Phiên và lượt xem theo ngày',
                    series: [{ name: 'Lượt xem', color: M.PALETTE[1], area: true, values: o.series.map(function (x) { return x.views; }) },
                        { name: 'Phiên', color: M.PALETTE[0], values: o.series.map(function (x) { return x.sessions; }) },
                        { name: 'Phiên kỳ trước', color: M.PREV, dash: true, values: o.series.map(function (x) { return x.prevSessions; }) }]
                });
            }
            ref('o-channels').innerHTML = hbars(o.channels.slice(0, 9).map(function (c) {
                var d0 = c.prev ? M.delta(c.sessions, c.prev.sessions) : null;
                return { name: c.name, value: c.sessions, extra: esc(M.pct(c.share)) + ' ' + deltaHtml(d0), sub: 'tương tác ' + M.pct(c.engagementRate, 0) + (o.hasConversions ? ' · CĐ ' + M.num(c.conversions) : '') };
            }), { empty: 'Chưa có số liệu kênh.' });
            ref('o-top').innerHTML = contentTable(o.topContent, { rank: true, compact: true });
            ref('o-dev').innerHTML = stack(o.devices.map(function (x) { return { name: x.name, value: x.sessions, extra: 'tương tác ' + esc(M.pct(x.engagementRate, 0)) }; }), 'Phiên theo thiết bị') +
                '<div class="tt-subhead">Khách mới / quay lại</div>' + stack(o.userTypes.map(function (x) { return { name: x.name, value: x.users, extra: o.hasConversions ? 'CĐ ' + esc(M.pct(x.conversionRate, 2)) : '' }; }), 'Người dùng mới và quay lại');
            ref('o-camp').innerHTML = o.campaigns.length ? table('o-camp', rowCols('Chiến dịch', function (r) { return campLink(r.key); }, { compact: true }).filter(function (c) { return c.k !== 'avgEngagementSec'; }), o.campaigns, { key: 'sessions', dir: -1 }, { limit: 8, fixed: true })
                : empty('Chưa có truy cập gắn utm_campaign trong kỳ.', 'ni-target');
            ref('o-sm').innerHTML = hbars(o.sourceMedium.slice(0, 10).map(function (x) {
                return { name: x.name, value: x.sessions, extra: x.prev ? deltaHtml(M.delta(x.sessions, x.prev.sessions)) : '', sub: 'tương tác ' + M.pct(x.engagementRate, 0) };
            }));
            ref('o-heat').innerHTML = heatmap(o.heat);
            ref('o-cats').innerHTML = catBars(o.categories);
            ref('o-landing').innerHTML = table('o-landing', [
                { k: 'name', label: 'Trang đích', cls: 'mk-c-name', html: function (r) { return '<span class="mk-path" title="' + esc(r.name) + '">' + esc(r.name) + '</span>'; } },
                { k: 'sessions', label: 'Phiên', cls: 'text-right tt-num', html: function (r) { return '<b>' + esc(M.num(r.sessions)) + '</b>'; } },
                { k: 'er', label: 'Tương tác', cls: 'text-right tt-num', html: function (r) { return engCell(r.engagementRate); } },
                { k: 'cv', label: 'Chuyển đổi', cls: 'text-right tt-num', html: function (r) { return esc(M.num(r.conversions)); } }
            ], o.landing, { key: '', dir: -1 }, { limit: 10, fixed: true });
            ref('o-geo').innerHTML = hbars(o.countries.map(function (x) { return { name: x.name, value: x.sessions, extra: esc(M.pct(x.share)) }; }));
            ref('o-types').innerHTML = table('o-types', rowCols('Loại trang', function (r) { return esc(r.name); }, { compact: true }).filter(function (c) { return c.k !== 'avgEngagementSec'; }),
                (o.pageTypes || []).filter(function (x) { return x.key !== 'lead'; }), { key: 'sessions', dir: -1 }, { fixed: true, empty: 'Chưa có số liệu trang đích.' });
            ref('o-thanks').innerHTML = '<div class="mk-lead"><div class="tt-kpi-value">' + esc(M.num(o.thankYouViews)) + '</div>' +
                '<p class="text-soft">lượt xem trang <code>/dang-ky-tu-van/thanh-cong</code> trong kỳ' + (s.sessions ? ' · ' + esc(M.pct(o.thankYouViews / s.sessions, 2)) + ' / phiên' : '') + '.</p>' +
                (o.hasConversions ? '<p class="mb-0">Chuyển đổi GA4 (key event): <b>' + esc(M.num(s.conversions)) + '</b>.</p>'
                    : '<p class="mb-0 text-warning">GA4 chưa có key event: tạo sự kiện <code>generate_lead</code> khi xem trang này rồi đánh dấu Key event để đo kênh / chiến dịch / bài nào mang về lead.</p>') + '</div>';
            stamp();
        }
        function returningShare(types) {
            var all = 0, ret = 0;
            (types || []).forEach(function (x) { all += x.users; if (x.key === 'returning') ret += x.users; });
            return all ? ret / all : null;
        }
        function dataTable(head, rows) {
            return '<details class="mk-details"><summary>Xem bảng số liệu</summary><div class="tt-dtable"><table class="table table-sm"><thead><tr>' +
                head.map(function (h, i) { return '<th' + (i ? ' class="text-right"' : '') + '>' + esc(h) + '</th>'; }).join('') + '</tr></thead><tbody>' +
                rows.map(function (r) { return '<tr>' + r.map(function (c, i) { return '<td' + (i ? ' class="text-right tt-num"' : '') + '>' + esc(c) + '</td>'; }).join('') + '</tr>'; }).join('') + '</tbody></table></div></details>';
        }
        function campLink(name) {
            return '<button type="button" class="tt-plink" data-act="campaign" data-name="' + esc(name) + '">' + esc(name) + '</button>';
        }
        /** Chuyên mục: thanh tỷ trọng xem (xanh) cạnh tỷ trọng bài xuất bản (xám), nhãn cầu > cung / cung > cầu. */
        function catBars(cats) {
            cats = (cats || []).filter(function (c) { return c.views > 0 || c.published > 0; }).slice(0, 8);
            if (!cats.length) return empty('Chưa đối chiếu được bài với chuyên mục (kiểm tra mẫu NewId trong đường dẫn ở Cấu hình website).', 'ni-list');
            return '<div class="tt-legend"><span><i style="background:' + M.PALETTE[0] + '"></i>% lượt xem</span><span><i style="background:' + M.PREV + '"></i>% bài xuất bản trong kỳ</span></div>' +
                '<ul class="mk-cats">' + cats.map(function (c) {
                    var tag = c.balance === 'under' ? '<span class="badge badge-dim badge-success">Cầu &gt; cung</span>' : c.balance === 'over' ? '<span class="badge badge-dim badge-warning">Cung &gt; cầu</span>' : '';
                    return '<li><div class="tt-hbar-top"><span class="tt-hbar-name">' + esc(c.name) + ' ' + tag + '</span><span class="tt-hbar-val"><b>' + esc(M.num(c.views)) + '</b><span>' + esc(c.published + ' bài') + '</span></span></div>' +
                        '<div class="mk-dual"><span style="width:' + Math.max(0.5, c.viewShare * 100) + '%;background:' + M.PALETTE[0] + '" title="' + esc(M.pct(c.viewShare) + ' lượt xem') + '"></span></div>' +
                        '<div class="mk-dual"><span style="width:' + Math.max(c.published ? 0.5 : 0, c.publishShare * 100) + '%;background:' + M.PREV + '" title="' + esc(M.pct(c.publishShare) + ' bài xuất bản') + '"></span></div></li>';
                }).join('') + '</ul>';
        }

        // ------------------------------------------------------------ Gợi ý hành động
        function insightList(list, compact) {
            return '<ul class="tt-alerts mk-ins' + (compact ? ' is-compact' : '') + '">' + list.map(function (x) {
                var lv = M.LEVELS[x.level] || M.LEVELS.info, tab = M.TABS.filter(function (t) { return t.key === x.tab; })[0];
                return '<li class="tt-alert is-' + esc(x.level === 'opportunity' ? 'info' : x.level) + (x.level === 'opportunity' ? ' is-opp' : '') + '"><em class="icon ni ' + lv.icon + ' tt-alert-ic" aria-hidden="true"></em>' +
                    '<div class="tt-alert-body"><div class="tt-alert-title"><span class="mk-lv">' + esc(lv.name) + '</span> ' + esc(x.title) + '</div>' +
                    '<div class="tt-alert-msg">' + esc(x.detail) + '</div>' +
                    (compact ? '' : '<div class="mk-act"><b>Việc cần làm:</b> ' + esc(x.action) + '</div>') +
                    '<div class="mk-ins-foot"><span class="badge badge-pill badge-outline-light">' + esc(M.AREAS[x.area] || x.area) + '</span>' +
                    (tab && tab.key !== 'overview' ? '<button type="button" class="btn btn-sm btn-link" data-act="tab" data-tab-go="' + tab.key + '">' + esc(tab.name) + '<em class="icon ni ni-chevron-right"></em></button>' : '') + '</div></div></li>';
            }).join('') + '</ul>';
        }
        function renderInsights() {
            var el = pane('insights'), d = st.data.overview;
            var badge = ref('ins-badge');
            if (!d) { if (st.err.overview) el.innerHTML = errorCard(st.err.overview, 'overview'); else el.innerHTML = skeleton(); return; }
            var all = M.sortInsights(d.overview.insights), urgent = all.filter(function (x) { return x.level === 'danger' || x.level === 'warning'; }).length;
            badge.hidden = !urgent;
            badge.textContent = urgent;
            var list = all.filter(function (x) { return (!st.ins.level || x.level === st.ins.level) && (!st.ins.area || x.area === st.ins.area); });
            var count = function (f) { return all.filter(f).length; };
            var areas = Object.keys(M.AREAS).filter(function (a) { return all.some(function (x) { return x.area === a; }); });
            el.innerHTML = '<div class="mk-ins-head"><div><h5 class="mb-1">Gợi ý hành động cho Marketing</h5><p class="text-soft mb-0">Tự động phân tích ' + esc(M.rangeText(d.overview.from, d.overview.to)) +
                ' so với kỳ trước: xu hướng, kênh, nội dung, chiến dịch, UTM, khán giả. Xếp theo mức độ ưu tiên.</p></div></div>' +
                '<div class="mk-filters"><div class="tt-seg" role="group" aria-label="Mức độ"><button type="button" class="tt-seg-btn' + (!st.ins.level ? ' on' : '') + '" data-act="ins-level" data-v="">Tất cả (' + all.length + ')</button>' +
                Object.keys(M.LEVELS).filter(function (k) { return count(function (x) { return x.level === k; }); }).map(function (k) {
                    return '<button type="button" class="tt-seg-btn' + (st.ins.level === k ? ' on' : '') + '" data-act="ins-level" data-v="' + k + '">' + esc(M.LEVELS[k].name) + ' (' + count(function (x) { return x.level === k; }) + ')</button>';
                }).join('') + '</div>' +
                '<select class="form-select form-control form-control-sm mk-sel" data-f="ins-area" aria-label="Lĩnh vực"><option value="">Mọi lĩnh vực</option>' + areas.map(function (a) {
                    return '<option value="' + a + '"' + (st.ins.area === a ? ' selected' : '') + '>' + esc(M.AREAS[a]) + '</option>';
                }).join('') + '</select></div>' +
                (list.length ? insightList(list, false) : empty('Không có gợi ý nào ở bộ lọc này.', 'ni-check-circle'));
        }

        // ------------------------------------------------------------ Nội dung
        function contentTable(rows, opt) {
            opt = opt || {};
            if (!rows || !rows.length) return empty(opt.empty || 'Chưa có số liệu bài viết.', 'ni-file-text');
            var c = st.ct, sortable = !opt.compact;
            var th = function (k, label, cls) {
                if (!sortable) return '<th class="' + (cls || '') + '">' + esc(label) + '</th>';
                var on = c.sort === k;
                return '<th class="' + (cls || '') + '"><button type="button" class="tt-sort' + (on ? ' on' : '') + '" data-act="ct-sort" data-k="' + k + '">' + esc(label) +
                    '<em class="icon ni ' + (on && c.dir === 'asc' ? 'ni-arrow-up' : 'ni-arrow-down') + '" aria-hidden="true"></em></button></th>';
            };
            var start = opt.offset || 0;
            return '<div class="table-responsive mk-table"><table class="table tt-stable mk-ctable"><thead><tr>' + (opt.rank ? '<th class="tt-c-rank">#</th>' : '') + '<th>' + (c.group === 'page' && !opt.compact ? 'Trang' : 'Bài viết') + '</th>' +
                th('views', 'Lượt xem', 'text-right') + (opt.compact ? '' : th('users', 'Người đọc', 'text-right') + th('vpu', 'Xem / người', 'text-right')) +
                th('engagement', 'Đọc TB', 'text-right') + th('conversions', 'Chuyển đổi', 'text-right') + (opt.compact ? '' : th('growth', 'So kỳ trước', 'text-right')) + '</tr></thead><tbody>' +
                rows.map(function (r, i) {
                    var meta = [];
                    if (r.categoryName) meta.push(esc(r.categoryName));
                    if (r.authorName && !opt.compact) meta.push(esc(r.authorName));
                    if (r.publishedDate && !opt.compact) meta.push('Đăng ' + esc(A.fmtDate(r.publishedDate)));
                    if (r.paths > 1) meta.push(esc(r.paths + ' URL'));
                    return '<tr>' + (opt.rank ? '<td class="tt-c-rank">' + (start + i + 1) + '</td>' : '') +
                        '<td class="tt-c-title"><div class="mk-title"><button type="button" class="tt-plink mk-alink" data-act="article" data-key="' + esc(r.key) + '" title="' + esc(r.path) + '">' + esc(r.title || r.path) + '</button>' + linkOut(r.url, r.title) + '</div>' +
                        '<div class="tt-meta">' + meta.map(function (m) { return '<span>' + m + '</span>'; }).join('') + '</div>' +
                        '<div class="mk-badges">' + segBadge(r.segment) + trendBadge(r.trend) + '</div></td>' +
                        '<td class="text-right tt-num" data-th="Lượt xem"><b>' + esc(M.num(r.views)) + '</b>' + (opt.compact ? '' : '<div class="mk-share">' + esc(M.pct(r.share, 2)) + '</div>') + '</td>' +
                        (opt.compact ? '' : '<td class="text-right tt-num" data-th="Người đọc">' + esc(M.num(r.users)) + '</td><td class="text-right tt-num" data-th="Xem / người">' + esc(M.num(r.viewsPerUser, 2)) + '</td>') +
                        '<td class="text-right tt-num" data-th="Đọc TB">' + esc(M.duration(r.avgEngagementSec)) + '</td>' +
                        '<td class="text-right tt-num" data-th="Chuyển đổi">' + (r.conversions ? '<b>' + esc(M.num(r.conversions)) + '</b>' : '<span class="text-soft">0</span>') + '</td>' +
                        (opt.compact ? '' : '<td class="text-right tt-num" data-th="So kỳ trước">' + (r.prevViews ? deltaHtml(M.delta(r.views, r.prevViews), 'Kỳ trước: ' + M.num(r.prevViews)) : '<span class="text-soft">mới</span>') + '</td>') +
                        '</tr>';
                }).join('') + '</tbody></table></div>';
        }

        function renderContent() {
            var d = st.data.content, c = st.ct, el = pane('content');
            if (!el.querySelector('[data-ref="ct-body"]')) {
                el.innerHTML = '<div class="mk-toolbar">' +
                    '<div class="form-control-wrap mk-search"><div class="form-icon form-icon-left"><em class="icon ni ni-search"></em></div>' +
                    '<input type="search" class="form-control" data-f="ct-q" placeholder="Lọc theo tiêu đề bài viết hoặc đường dẫn (không cần dấu)…" aria-label="Lọc theo tiêu đề"></div>' +
                    '<div class="tt-seg" role="group" aria-label="Gộp theo"><button type="button" class="tt-seg-btn" data-act="ct-group" data-v="article">Theo bài viết</button><button type="button" class="tt-seg-btn" data-act="ct-group" data-v="page">Theo URL</button></div>' +
                    '<select class="form-select form-control mk-sel" data-f="ct-cat" aria-label="Chuyên mục"></select>' +
                    csvBtn('ct-csv') + '</div>' +
                    '<div class="mk-chips" data-ref="ct-chips"></div>' +
                    '<div data-ref="ct-sum"></div>' +
                    '<div class="card card-bordered tt-dcard mt-2"><div class="card-inner"><div data-ref="ct-body"></div><div class="mt-3" data-ref="ct-pager"></div></div></div>' +
                    card('ct-cats', 'Chuyên mục: lượt xem so với số bài xuất bản', '', { sub: 'chỉ số cầu/cung > 1,5: nên tăng sản lượng; < 0,5: sản xuất thừa' }) +
                    '<p class="tt-foot-note" data-ref="ct-note"></p>';
                el.querySelector('[data-f="ct-q"]').value = c.q;
            }
            Array.prototype.forEach.call(el.querySelectorAll('[data-act="ct-group"]'), function (b) { b.classList.toggle('on', b.getAttribute('data-v') === c.group); });
            var cats = (d.categories || []).slice().sort(function (a, b) { return a.name.localeCompare(b.name, 'vi'); });
            el.querySelector('[data-f="ct-cat"]').innerHTML = '<option value="0">Mọi chuyên mục</option>' + cats.map(function (x) {
                return '<option value="' + x.id + '"' + (String(x.id) === String(c.categoryId) ? ' selected' : '') + '>' + esc(x.name) + '</option>';
            }).join('');
            var seg = d.segments || {}, tr = d.trends || {}, total = 0;
            Object.keys(seg).forEach(function (k) { total += seg[k]; });
            var chip = function (act, v, label, n, hint, on) {
                return '<button type="button" class="mk-chip' + (on ? ' on' : '') + '" data-act="' + act + '" data-v="' + v + '"' + (hint ? ' title="' + esc(hint) + '"' : '') + '>' + esc(label) + ' <b>' + esc(M.num(n || 0)) + '</b></button>';
            };
            ref('ct-chips').innerHTML = chip('ct-seg', '', 'Tất cả', total, '', !c.segment && !c.trend) +
                ['star', 'potential', 'optimize'].map(function (k) { return chip('ct-seg', k, M.SEGMENTS[k].name, seg[k], M.SEGMENTS[k].hint, c.segment === k); }).join('') +
                '<span class="mk-chip-sep"></span>' +
                ['up', 'down', 'new'].map(function (k) { return chip('ct-trend', k, M.TRENDS[k].name, tr[k], '', c.trend === k); }).join('');
            var sm = d.summary || {}, th = d.thresholds || {};
            ref('ct-sum').innerHTML = '<div class="mk-sum"><span><b>' + esc(M.num(d.total)) + '</b> ' + (d.group === 'page' ? 'trang' : 'bài / trang') + '</span>' +
                '<span><b>' + esc(M.num(sm.views)) + '</b> lượt xem' + (d.siteViews ? ' (' + esc(M.pct(sm.views / d.siteViews)) + ' toàn site)' : '') + ' ' + deltaHtml(M.delta(sm.views, sm.prevViews)) + '</span>' +
                '<span><b>' + esc(M.duration(sm.avgEngagementSec)) + '</b> đọc TB</span>' +
                '<span><b>' + esc(M.num(sm.conversions)) + '</b> chuyển đổi</span></div>';
            ref('ct-body').innerHTML = contentTable(d.rows, { rank: true, offset: (d.page - 1) * d.pageSize, empty: c.q ? 'Không có bài nào khớp "' + c.q + '" trong kỳ.' : 'Chưa có số liệu.' });
            ref('ct-pager').innerHTML = A.pager(d.page, d.pageSize, d.total);
            ref('ct-cats').innerHTML = catTable(d.categories || []);
            ref('ct-note').textContent = 'Phân loại theo phân vị của các bài ≥ 5 lượt xem trong kỳ: xem nhiều ≥ ' + M.num(th.v75) + ' lượt (top 25%); đọc sâu ≥ ' + M.duration(th.e50) + ' (trung vị), rất sâu ≥ ' + M.duration(th.e75) +
                '; đọc lướt < ' + M.duration(th.e25) + '. "Đang lên" = lượt xem gấp đôi kỳ trước, "Đang giảm" = mất từ một nửa.' + (d.truncated ? ' GA4 trả tối đa 10.000 trang; các trang ít xem nhất không hiển thị.' : '');
        }
        function catTable(cats) {
            return table('cats', [
                { k: 'name', label: 'Chuyên mục', cls: 'mk-c-name', html: function (r) {
                    return '<button type="button" class="tt-plink" data-act="ct-catgo" data-id="' + r.id + '">' + esc(r.name) + '</button>' +
                        (r.balance === 'under' ? ' <span class="badge badge-dim badge-success">Cầu &gt; cung</span>' : r.balance === 'over' ? ' <span class="badge badge-dim badge-warning">Cung &gt; cầu</span>' : '');
                } },
                { k: 'views', label: 'Lượt xem', cls: 'text-right tt-num', val: function (r) { return r.views; }, html: function (r) { return '<b>' + esc(M.num(r.views)) + '</b><div>' + deltaHtml(M.delta(r.views, r.prevViews)) + '</div>'; } },
                { k: 'viewShare', label: '% lượt xem', cls: 'text-right tt-num', val: function (r) { return r.viewShare; }, html: function (r) { return esc(M.pct(r.viewShare)); } },
                { k: 'published', label: 'Bài xuất bản', cls: 'text-right tt-num', val: function (r) { return r.published; }, html: function (r) { return esc(M.num(r.published)); } },
                { k: 'publishShare', label: '% sản lượng', cls: 'text-right tt-num', val: function (r) { return r.publishShare; }, html: function (r) { return esc(M.pct(r.publishShare)); } },
                { k: 'demandIndex', label: 'Cầu / cung', cls: 'text-right tt-num', val: function (r) { return r.demandIndex === null ? 99 : r.demandIndex; }, html: function (r) {
                    return r.demandIndex === null ? '<span class="text-soft">–</span>' : '<span class="tt-bench ' + (r.demandIndex >= 1.5 ? 'is-good' : r.demandIndex <= 0.5 ? 'is-bad' : '') + '">' + esc(M.num(r.demandIndex, 2)) + '</span>';
                } },
                { k: 'articles', label: 'Bài có lượt xem', cls: 'text-right tt-num', val: function (r) { return r.articles; }, html: function (r) { return esc(M.num(r.articles)); } },
                { k: 'avgEngagementSec', label: 'Đọc TB', cls: 'text-right tt-num', val: function (r) { return r.avgEngagementSec; }, html: function (r) { return esc(M.duration(r.avgEngagementSec)); } }
            ], cats, st.catSort, { empty: 'Chưa đối chiếu được bài với chuyên mục.' });
        }
        function loadContent(reset) {
            if (reset) st.ct.page = 1;
            load('content');
        }

        /** Sidebar chi tiết bài / trang. */
        function openArticle(key) {
            var sb = A.sidebar({ title: 'Đang tải…', size: 'lg', body: '<div class="tt-dash mk-dash"><div class="tt-skeleton" style="height:320px"></div></div>' });
            ctx.api.get('Article', params({ key: key })).then(function (r) {
                var d = r.data || {}, it = d.item, x = d.data;
                if (!it || !x) { sb.setTitle('Không có số liệu'); sb.body.innerHTML = '<div class="tt-dash">' + empty('Bài chưa có lượt xem trong khoảng đang chọn.', 'ni-file-text') + '</div>'; return; }
                sb.setTitle(it.title || it.path, M.rangeText(d.from, d.to) + (it.categoryName ? ' · ' + it.categoryName : '') + (it.authorName ? ' · ' + it.authorName : ''));
                var t = x.total || {}, p = x.previous || {};
                var tips = { star: 'Bài đang làm tốt: ghim trang chủ, chia sẻ lại, viết bài tiếp nối, gắn CTA chuyển đổi.', potential: 'Người đọc rất quan tâm nhưng ít người thấy: chia sẻ lên Fanpage / Zalo OA / email, đặt liên kết từ các bài top, thử tiêu đề / ảnh hấp dẫn hơn.',
                    optimize: 'Nhiều người vào nhưng rời nhanh: kiểm tra tiêu đề có khớp nội dung, sapo, ảnh, độ dài đoạn, tốc độ tải, thêm bài liên quan & CTA.' };
                var trendTip = it.trend === 'down' ? 'Lượt xem giảm mạnh: cập nhật nội dung / số liệu mới, kiểm tra thứ hạng từ khóa và chia sẻ lại.' : it.trend === 'up' ? 'Đang lên: tận dụng xu hướng bằng bài cùng chủ đề và liên kết sang trang chuyển đổi.' : '';
                sb.body.innerHTML = '<div class="tt-dash mk-dash">' +
                    '<div class="mk-badges mb-2">' + segBadge(it.segment) + trendBadge(it.trend) + (it.url ? ' <a href="' + esc(M.safeUrl(it.url)) + '" target="_blank" rel="noopener" class="small">Mở bài <em class="icon ni ni-external"></em></a>' : '') + '</div>' +
                    (tips[it.segment] || trendTip ? '<div class="alert alert-light mk-tip"><em class="icon ni ni-bulb"></em> ' + esc([tips[it.segment], trendTip].filter(Boolean).join(' ')) + '</div>' : '') +
                    '<div class="tt-kpis mk-kpis-4">' +
                    kpi({ icon: 'ni-eye', label: 'Lượt xem', value: M.num(t.views), delta: M.delta(t.views, p.views), prev: M.num(p.views) }) +
                    kpi({ icon: 'ni-users', label: 'Người đọc', value: M.num(t.users), delta: M.delta(t.users, p.users), prev: M.num(p.users) }) +
                    kpi({ icon: 'ni-clock', label: 'Đọc TB / người', value: M.duration(t.avgEngagementSec), delta: M.delta(t.avgEngagementSec, p.avgEngagementSec), prev: M.duration(p.avgEngagementSec),
                        sub: '<span class="text-soft">Trung vị site ' + esc(M.duration(d.thresholds && d.thresholds.e50)) + '</span>' }) +
                    kpi({ icon: 'ni-target', label: 'Chuyển đổi', value: M.num(t.conversions), delta: M.delta(t.conversions, p.conversions), prev: M.num(p.conversions) }) +
                    '</div>' +
                    card('a-series', 'Lượt xem theo ngày', '<div class="tt-chart"></div>') +
                    '<div class="tt-grid tt-g11">' + card('a-ch', 'Kênh dẫn tới bài', hbars(slices(x.channels))) + card('a-src', 'Nguồn / phương tiện', hbars(slices(x.sources))) + '</div>' +
                    '<div class="tt-grid tt-g11">' + card('a-camp', 'Chiến dịch', hbars(slices(x.campaigns), { empty: 'Không có chiến dịch nào dẫn tới bài.' })) + card('a-dev', 'Thiết bị', stack(x.devices.map(function (s) { return { name: s.name, value: s.value }; }), 'Thiết bị')) + '</div>' +
                    '<div class="tt-grid tt-g11">' + card('a-ref', 'Trang dẫn tới (referrer)', hbars(slices(x.referrers), { empty: 'Không có trang dẫn tới.' })) + card('a-city', 'Thành phố', hbars(slices(x.cities))) + '</div>' +
                    (d.paths && d.paths.length > 1 ? '<div class="tt-subhead">Các URL của bài</div><ul class="mk-paths">' + d.paths.map(function (u) { return '<li>' + esc(u) + '</li>'; }).join('') + '</ul>' : '') +
                    '</div>';
                var box = sb.body.querySelector('.tt-chart');
                lineChart(box, {
                    labels: x.series.map(function (s) { return M.dayLabel(s.date); }), tips: x.series.map(function (s) { return A.fmtDate(s.date); }), aria: 'Lượt xem bài theo ngày', h: 200,
                    series: [{ name: 'Lượt xem', color: M.PALETTE[0], area: true, values: x.series.map(function (s) { return s.views; }) },
                        { name: 'Người đọc', color: M.PALETTE[1], values: x.series.map(function (s) { return s.users; }) }]
                });
                sb.el.addEventListener('pointermove', onPointer);
            }, function (err) { sb.setTitle('Không tải được'); sb.body.innerHTML = '<div class="tt-dash">' + errorCard(err, '') + '</div>'; });
        }
        function slices(list) {
            return (list || []).slice(0, 8).map(function (s) {
                var name = s.name || s.key || '(không rõ)';
                return { name: name, value: s.value, extra: esc(M.pct(s.share)) };
            });
        }

        // ------------------------------------------------------------ Nguồn & UTM
        function renderAcq() {
            var d = st.data.acquisition, el = pane('acquisition'), a = st.acq;
            el.innerHTML = '<div class="mk-toolbar"><div class="tt-seg" role="group" aria-label="Phân tích theo">' + (init.dims || []).map(function (x) {
                return '<button type="button" class="tt-seg-btn' + (x.key === d.dim ? ' on' : '') + '" data-act="acq-dim" data-v="' + esc(x.key) + '">' + esc(x.name) + '</button>';
            }).join('') + '</div>' + csvBtn('acq-csv') + '</div>' +
                '<div class="tt-kpis mk-kpis-4">' +
                kpi({ icon: 'ni-activity', label: 'Phiên', value: M.num(d.total.sessions), delta: M.delta(d.total.sessions, d.previous.sessions), prev: M.num(d.previous.sessions) }) +
                kpi({ icon: 'ni-hot', label: 'Tỷ lệ tương tác', value: M.pct(d.total.engagementRate), delta: M.delta(d.total.engagementRate, d.previous.engagementRate, { points: true }), prev: M.pct(d.previous.engagementRate) }) +
                kpi({ icon: 'ni-target', label: 'Chuyển đổi', value: M.num(d.total.conversions), delta: M.delta(d.total.conversions, d.previous.conversions), prev: M.num(d.previous.conversions) }) +
                kpi({ icon: 'ni-percent', label: 'Tỷ lệ chuyển đổi', value: M.pct(d.total.conversionRate, 2), delta: M.delta(d.total.conversionRate, d.previous.conversionRate, { points: true }), prev: M.pct(d.previous.conversionRate, 2) }) +
                '</div>' +
                card('acq-table', 'Theo ' + d.dimName, table('acq', rowCols(d.dimName, function (r) {
                    var label = r.name || '(trống)';
                    return d.dim === 'campaign' && !/^\(/.test(r.key) ? campLink(r.key) : '<span class="mk-path" title="' + esc(label) + '">' + esc(label) + '</span>';
                }), d.rows, { key: a.sort, dir: a.dir }), { sub: d.more ? '(' + d.more + ' dòng nhỏ khác không hiển thị)' : '' }) +
                '<div class="tt-grid tt-g11">' + card('acq-utm', 'Kiểm tra quy ước UTM', utmIssues(d.utm)) + card('acq-builder', 'Tạo link UTM chuẩn', builder()) + '</div>';
            builderUpdate();
        }
        function utmIssues(list) {
            if (!list || !list.length) return '<div class="tt-alert-ok"><em class="icon ni ni-check-circle-fill" aria-hidden="true"></em><span>UTM trong kỳ đúng quy ước</span></div>';
            return '<ul class="tt-alerts mk-utm">' + list.map(function (u) {
                return '<li class="tt-alert is-warning"><em class="icon ni ni-alert-fill tt-alert-ic" aria-hidden="true"></em><div class="tt-alert-body"><div class="tt-alert-title">' + esc(u.field) + ' <span class="tt-alert-count">' + esc(M.num(u.sessions)) + ' phiên</span></div>' +
                    '<div class="tt-alert-msg">' + esc(u.message) + '</div><div class="mk-vals">' + u.values.map(function (v) { return '<code>' + esc(v) + '</code>'; }).join(' ') + '</div></div></li>';
            }).join('') + '</ul>';
        }
        function builder() {
            var saved = store.get('utm') || {};
            var f = function (k, label, ph, list) {
                return '<div class="form-group"><label class="form-label" for="mk-utm-' + k + '">' + esc(label) + '</label><input class="form-control" id="mk-utm-' + k + '" data-utm="' + k + '" value="' + esc(saved[k] || '') + '" placeholder="' + esc(ph) + '"' + (list ? ' list="mk-utm-' + k + '-l"' : '') + '>' +
                    (list ? '<datalist id="mk-utm-' + k + '-l">' + list.map(function (x) { return '<option value="' + esc(x) + '">'; }).join('') + '</datalist>' : '') + '</div>';
            };
            return '<div class="mk-builder">' + f('url', 'Đường dẫn đích *', 'https://…/bai-viet-123.html') +
                '<div class="mk-b2">' + f('source', 'utm_source * (nơi đăng)', 'facebook, zalo, newsletter…', M.SOURCES) + f('medium', 'utm_medium * (loại kênh)', 'social, cpc, email…', M.MEDIUMS) + '</div>' +
                f('campaign', 'utm_campaign * (tên đợt)', 'tuyen-sinh-thang-10') +
                '<div class="mk-b2">' + f('content', 'utm_content (mẫu / vị trí)', 'banner-a, video-1') + f('term', 'utm_term (từ khóa)', 'du-hoc-my') + '</div>' +
                '<label class="form-label">Link đã gắn UTM</label><div class="mk-out-url"><code data-ref="utm-out" class="text-soft">Nhập đường dẫn, source, medium, campaign…</code>' +
                '<button type="button" class="btn btn-sm btn-primary" data-act="utm-copy" disabled><em class="icon ni ni-copy"></em><span>Sao chép</span></button></div>' +
                '<p class="tt-foot-note mb-0">Tự chuyển chữ thường, bỏ dấu, thay khoảng trắng bằng "-" để không bị tách dòng trong GA4.</p></div>';
        }
        function builderUpdate() {
            var p = {};
            Array.prototype.forEach.call(root.querySelectorAll('[data-utm]'), function (i) { p[i.getAttribute('data-utm')] = i.value; });
            if (!root.querySelector('[data-utm]')) return;
            store.set('utm', p);
            var ok = p.url && p.source && p.medium && p.campaign, out = ok ? M.utmUrl(p.url, p) : '', o = ref('utm-out');
            o.textContent = out || (p.url && !M.utmUrl(p.url, {}) ? 'Đường dẫn phải bắt đầu bằng http:// hoặc https://' : 'Nhập đường dẫn, source, medium, campaign…');
            o.classList.toggle('text-soft', !out);
            root.querySelector('[data-act="utm-copy"]').disabled = !out;
        }

        // ------------------------------------------------------------ Chiến dịch
        function renderCampaigns() {
            var d = st.data.campaigns, el = pane('campaigns'), c = d.campaigns, flags = {};
            (d.flags || []).forEach(function (f) { flags[f.key] = f; });
            el.innerHTML = '<div class="tt-kpis mk-kpis-4">' +
                kpi({ icon: 'ni-target', label: 'Phiên từ chiến dịch', value: M.num(c.sessions), sub: '<span class="text-soft">' + esc(M.pct(d.campaignShare)) + ' tổng phiên site</span>' }) +
                kpi({ icon: 'ni-flag', label: 'Số chiến dịch có truy cập', value: M.num(d.rows.filter(function (x) { return x.sessions > 0; }).length) }) +
                kpi({ icon: 'ni-hot', label: 'Tỷ lệ tương tác', value: M.pct(c.engagementRate), sub: '<span class="text-soft">site: ' + esc(M.pct(d.site.engagementRate)) + '</span>' }) +
                kpi({ icon: 'ni-percent', label: 'Chuyển đổi (tỷ lệ)', value: M.num(c.conversions), sub: '<span class="text-soft">' + esc(M.pct(c.conversionRate, 2)) + ' / phiên · site ' + esc(M.pct(d.site.conversionRate, 2)) + '</span>' }) +
                '</div>' +
                card('c-series', 'Top 5 chiến dịch theo ngày (phiên)', d.series.length ? '' : empty('Chưa có chiến dịch nào trong kỳ.', 'ni-target')) +
                card('c-table', 'Hiệu quả từng chiến dịch', '', { tools: csvBtn('camp-csv'), sub: 'bấm tên để xem chi tiết' });
            if (d.series.length) {
                var box = ref('c-series');
                box.innerHTML = legend(d.series.map(function (s, i) { return { name: s.name, color: M.PALETTE[i] }; })) + '<div class="tt-chart"></div>' +
                    dataTable(['Ngày'].concat(d.series.map(function (s) { return s.name; })), d.dates.map(function (dt, i) { return [A.fmtDate(dt)].concat(d.series.map(function (s) { return M.num(s.values[i]); })); }));
                lineChart(box.querySelector('.tt-chart'), {
                    labels: d.dates.map(M.dayLabel), tips: d.dates.map(function (x) { return A.fmtDate(x); }), aria: 'Phiên theo ngày của 5 chiến dịch lớn nhất',
                    series: d.series.map(function (s, i) { return { name: s.name, color: M.PALETTE[i], values: s.values }; })
                });
            }
            ref('c-table').innerHTML = table('camp', rowCols('Chiến dịch', function (r) {
                var f = flags[r.key], tags = '';
                if (f && f.bestConv) tags += '<span class="badge badge-dim badge-success">Chuyển đổi tốt</span>';
                if (f && f.noConv) tags += '<span class="badge badge-dim badge-danger">0 chuyển đổi</span>';
                if (f && f.lowEng) tags += '<span class="badge badge-dim badge-warning">Tương tác thấp</span>';
                return campLink(r.key) + (r.sub ? '<div class="tt-meta"><span>' + esc(r.sub) + '</span></div>' : '') + (tags ? '<div class="mk-badges">' + tags + '</div>' : '');
            }), d.rows, { key: st.camp.sort, dir: st.camp.dir }, { empty: 'Chưa có truy cập gắn utm_campaign trong kỳ. Dùng công cụ tạo link UTM ở tab Nguồn & UTM.' });
        }
        function openCampaign(name) {
            var sb = A.sidebar({ title: name, subtitle: 'Đang tải…', size: 'xl', body: '<div class="tt-dash mk-dash"><div class="tt-skeleton" style="height:320px"></div></div>' });
            ctx.api.get('Campaign', params({ name: name })).then(function (r) {
                var d = r.data || {}, t = d.total, p = d.previous;
                sb.setTitle('Chiến dịch: ' + d.name, M.rangeText(d.from, d.to) + ' · so với ' + M.rangeText(d.prevFrom, d.prevTo));
                var small = function (list, label) { return table('x', rowCols(label, function (x) { return '<span class="mk-path" title="' + esc(x.name) + '">' + esc(x.name || '(trống)') + '</span>'; }, { compact: true }), list || [], { key: 'sessions', dir: -1 }, { limit: 15, fixed: true }); };
                sb.body.innerHTML = '<div class="tt-dash mk-dash"><div class="tt-kpis mk-kpis-4">' +
                    kpi({ icon: 'ni-activity', label: 'Phiên', value: M.num(t.sessions), delta: M.delta(t.sessions, p.sessions), prev: M.num(p.sessions) }) +
                    kpi({ icon: 'ni-users', label: 'Người dùng (mới)', value: M.num(t.users), sub: '<span class="text-soft">' + esc(M.num(t.newUsers)) + ' mới</span>', delta: M.delta(t.users, p.users), prev: M.num(p.users) }) +
                    kpi({ icon: 'ni-hot', label: 'Tỷ lệ tương tác', value: M.pct(t.engagementRate), delta: M.delta(t.engagementRate, p.engagementRate, { points: true }), prev: M.pct(p.engagementRate), sub: '<span class="text-soft">TG TB ' + esc(M.duration(t.avgEngagementSec)) + '</span>' }) +
                    kpi({ icon: 'ni-target', label: 'Chuyển đổi', value: M.num(t.conversions), delta: M.delta(t.conversions, p.conversions), prev: M.num(p.conversions), sub: '<span class="text-soft">' + esc(M.pct(t.conversionRate, 2)) + ' / phiên</span>' }) +
                    '</div>' + card('cd-series', 'Theo ngày', legend([{ name: 'Phiên', color: M.PALETTE[0] }, { name: 'Phiên có tương tác', color: M.PALETTE[2] }]) + '<div class="tt-chart"></div>') +
                    card('cd-sm', 'Nguồn / phương tiện', small(d.sourceMedium, 'Nguồn / phương tiện')) +
                    '<div class="tt-grid tt-g11">' + card('cd-ct', 'Mẫu quảng cáo / vị trí (utm_content)', small(d.content, 'utm_content')) + card('cd-term', 'Từ khóa (utm_term)', small(d.term, 'utm_term')) + '</div>' +
                    card('cd-lp', 'Trang đích', small(d.landing, 'Trang đích')) +
                    '<div class="tt-grid tt-g11">' + card('cd-dev', 'Thiết bị', stack(d.devices.map(function (x) { return { name: x.name, value: x.sessions, extra: 'tương tác ' + esc(M.pct(x.engagementRate, 0)) }; }), 'Thiết bị')) +
                    card('cd-city', 'Thành phố', hbars(d.cities.slice(0, 8).map(function (x) { return { name: x.name, value: x.sessions, extra: esc(M.pct(x.share)) }; }))) + '</div></div>';
                lineChart(sb.body.querySelector('.tt-chart'), {
                    labels: d.series.map(function (x) { return M.dayLabel(x.date); }), tips: d.series.map(function (x) { return A.fmtDate(x.date) + (x.conversions ? ' · ' + M.num(x.conversions) + ' chuyển đổi' : ''); }), aria: 'Phiên của chiến dịch theo ngày', h: 200,
                    series: [{ name: 'Phiên', color: M.PALETTE[0], area: true, values: d.series.map(function (x) { return x.sessions; }) },
                        { name: 'Phiên có tương tác', color: M.PALETTE[2], values: d.series.map(function (x) { return x.engaged; }) }]
                });
                sb.el.addEventListener('pointermove', onPointer);
            }, function (err) { sb.setTitle(name, 'Không tải được'); sb.body.innerHTML = '<div class="tt-dash">' + errorCard(err, '') + '</div>'; });
        }

        // ------------------------------------------------------------ Khán giả
        function renderAudience() {
            var d = st.data.audience, el = pane('audience');
            var small = function (list, label, name) { return table(name, rowCols(label, function (x) { return esc(x.name || '(không rõ)'); }, { compact: true }), list || [], { key: 'sessions', dir: -1 }, { limit: 10, fixed: true }); };
            var known = function (list) { return (list || []).filter(function (x) { return x.key !== 'unknown' && x.key !== '(not set)'; }); };
            var ages = known(d.ages), genders = known(d.genders);
            el.innerHTML =
                '<div class="tt-grid tt-g11">' + card('au-dev', 'Thiết bị', small(d.devices, 'Thiết bị', 'au1')) + card('au-type', 'Khách mới / quay lại', small(d.userTypes, 'Loại khách', 'au2')) + '</div>' +
                '<div class="tt-grid tt-g11">' + card('au-age', 'Độ tuổi', ages.length ? hbars(ages.map(function (x) { return { name: x.name, value: x.users, valueText: M.num(x.users) + ' người', extra: 'tương tác ' + esc(M.pct(x.engagementRate, 0)) }; }))
                    : empty('GA4 chưa có dữ liệu nhân khẩu học (cần bật Google Signals trong GA4 và đủ ngưỡng người dùng).', 'ni-users')) +
                card('au-gender', 'Giới tính', genders.length ? stack(genders.map(function (x) { return { name: x.name, value: x.users }; }), 'Giới tính') : empty('Chưa có dữ liệu giới tính.', 'ni-users')) + '</div>' +
                '<div class="tt-grid tt-g11">' + card('au-country', 'Quốc gia', small(d.countries, 'Quốc gia', 'au3')) + card('au-city', 'Thành phố', small(d.cities, 'Thành phố', 'au4')) + '</div>' +
                '<div class="tt-grid tt-g11">' + card('au-os', 'Hệ điều hành', hbars(d.os.map(function (x) { return { name: x.name, value: x.sessions, extra: esc(M.pct(x.share)) }; }))) +
                card('au-br', 'Trình duyệt', hbars(d.browsers.map(function (x) { return { name: x.name, value: x.sessions, extra: esc(M.pct(x.share)) }; }))) + '</div>' +
                '<div class="tt-grid tt-g21">' + card('au-heat', 'Khung giờ truy cập (thứ × giờ)', heatmap(d.heat)) + card('au-lang', 'Ngôn ngữ trình duyệt', hbars(d.languages.map(function (x) { return { name: x.name, value: x.sessions, extra: esc(M.pct(x.share)) }; }))) + '</div>';
        }

        // ------------------------------------------------------------ sự kiện
        function onClick(e) {
            var t = e.target.closest('[data-tab],[data-range],[data-act],[data-page]');
            if (!t || !root.contains(t)) return;
            if (t.hasAttribute('data-tab')) { e.preventDefault(); show(t.getAttribute('data-tab')); return; }
            if (t.hasAttribute('data-range')) {
                st.range = t.getAttribute('data-range');
                syncRange();
                if (st.range === 'custom') { st.from = ref('custom').querySelector('[data-f="from"]').value; st.to = ref('custom').querySelector('[data-f="to"]').value; }
                rangeChanged();
                return;
            }
            if (t.hasAttribute('data-page')) {
                e.preventDefault();
                var pg = +t.getAttribute('data-page');
                if (pg > 0 && !t.closest('.disabled')) { st.ct.page = pg; load('content'); }
                return;
            }
            var act = t.getAttribute('data-act'), v = t.getAttribute('data-v');
            switch (act) {
                case 'refresh': st.data = {}; st.key = {}; load(st.tab === 'insights' ? 'overview' : st.tab, true); break;
                case 'retry': load(t.getAttribute('data-name'), true); break;
                case 'tab': show(t.getAttribute('data-tab-go')); global.scrollTo({ top: root.getBoundingClientRect().top + global.pageYOffset - 80, behavior: 'smooth' }); break;
                case 'ins-level': st.ins.level = v; renderInsights(); break;
                case 'ct-group': st.ct.group = v; loadContent(true); break;
                case 'ct-seg': st.ct.segment = v; if (!v) st.ct.trend = ''; loadContent(true); break;
                case 'ct-trend': st.ct.trend = st.ct.trend === v ? '' : v; loadContent(true); break;
                case 'ct-sort':
                    var k = t.getAttribute('data-k');
                    st.ct.dir = st.ct.sort === k && st.ct.dir === 'desc' ? 'asc' : 'desc';
                    st.ct.sort = k;
                    loadContent(true);
                    break;
                case 'ct-catgo': st.ct.categoryId = +t.getAttribute('data-id'); loadContent(true); pane('content').scrollIntoView({ behavior: 'smooth' }); break;
                case 'ct-csv': contentCsv(); break;
                case 'article': openArticle(t.getAttribute('data-key')); break;
                case 'campaign': openCampaign(t.getAttribute('data-name')); break;
                case 'acq-dim': st.acq.dim = v; load('acquisition'); break;
                case 'acq-csv': TT.download('nguon-' + st.acq.dim + '.csv', rowsCsv(st.data.acquisition.rows)); break;
                case 'camp-csv': TT.download('chien-dich.csv', rowsCsv(st.data.campaigns.rows)); break;
                case 'utm-copy': copy(ref('utm-out').textContent); break;
                case 'sort': sortTable(t.getAttribute('data-table'), t.getAttribute('data-k')); break;
            }
        }
        function sortTable(name, k) {
            var s = name === 'acq' ? st.acq : name === 'camp' ? st.camp : name === 'cats' ? st.catSort : null;
            if (!s) return;
            var cur = name === 'cats' ? s.key : s.sort;
            s.dir = cur === k ? -s.dir : -1;
            if (name === 'cats') s.key = k; else s.sort = k;
            render(name === 'acq' ? 'acquisition' : name === 'camp' ? 'campaigns' : 'content');
        }
        function onChange(e) {
            var f = e.target.getAttribute('data-f');
            if (f === 'from' || f === 'to') {
                st[f] = e.target.value;
                if (st.from && st.to) rangeChanged();
            } else if (f === 'ct-cat') { st.ct.categoryId = +e.target.value || 0; loadContent(true); }
            else if (f === 'ins-area') { st.ins.area = e.target.value; renderInsights(); }
        }
        function onInput(e) {
            var f = e.target.getAttribute('data-f');
            if (f === 'ct-q') {
                var q = e.target.value.trim();
                if (q === st.ct.q) return;
                st.ct.q = q;
                loadContent(true);
            } else if (e.target.hasAttribute('data-utm')) builderUpdate();
        }
        function rangeChanged() {
            if (st.range === 'custom' && !(st.from && st.to)) return;
            store.set('range', { range: st.range, from: st.from, to: st.to });
            st.data = {}; st.key = {};
            load(st.tab === 'insights' ? 'overview' : st.tab);
            if (st.tab !== 'overview' && st.tab !== 'insights') load('overview');   // gợi ý + trung bình site dùng cho tô màu bảng
        }
        function contentCsv() {
            var c = st.ct;
            ctx.api.get('Content', params({ group: c.group, q: c.q, segment: c.segment, trend: c.trend, categoryId: c.categoryId || '', sort: c.sort, dir: c.dir, page: 1, pageSize: 500 })).then(function (r) {
                TT.download('noi-dung.csv', TT.toCsv(r.data.rows, [
                    { key: 'title', title: 'Tiêu đề' }, { key: 'path', title: 'Đường dẫn' }, { key: 'categoryName', title: 'Chuyên mục' }, { key: 'authorName', title: 'Tác giả' },
                    { key: 'publishedDate', title: 'Ngày đăng', fmt: function (v) { return A.fmtDate(v); } }, { key: 'views', title: 'Lượt xem' }, { key: 'prevViews', title: 'Lượt xem kỳ trước' },
                    { key: 'users', title: 'Người đọc' }, { key: 'avgEngagementSec', title: 'Đọc TB (giây)' }, { key: 'conversions', title: 'Chuyển đổi' },
                    { key: 'segment', title: 'Phân loại', fmt: function (v) { return (M.SEGMENTS[v] || {}).name || ''; } }, { key: 'trend', title: 'Xu hướng', fmt: function (v) { return (M.TRENDS[v] || {}).name || ''; } }
                ]));
            }, A.fail);
        }
        function copy(text) {
            var done = function () { A.toast('Đã sao chép link UTM.'); };
            if (global.navigator.clipboard && global.isSecureContext) { global.navigator.clipboard.writeText(text).then(done, function () { fallback(); }); return; }
            fallback();
            function fallback() {
                var ta = global.document.createElement('textarea');
                ta.value = text; ta.style.position = 'fixed'; ta.style.opacity = '0';
                global.document.body.appendChild(ta); ta.select();
                try { global.document.execCommand('copy'); done(); } catch (e) { A.toast('Không sao chép được, hãy chọn và sao chép thủ công.', 'warning'); }
                ta.parentNode.removeChild(ta);
            }
        }
    });

})(typeof window !== 'undefined' ? window : this);
