/*!
 * NVCMS.HeThong - quản trị menu admin theo portal (Portals\{PortalId}\MenuAdmin\menu.xml).
 * Không phụ thuộc jQuery/DNN ServicesFramework JS. Gọi Web API /DesktopModules/NVCMSHeThong/API/Menu/*.
 *
 * Trang DNN bọc mọi thứ trong <form> ASP.NET => Enter trong ô nhập gây postback, nên Enter bị chặn trong module.
 */
(function (global) {
    'use strict';

    // Skin _admin chỉ hiển thị 2 cấp (cấp 1: icon thanh trái, cấp 2: link không có con).
    var SKIN_LEVELS = 2;

    // ------------------------------------------------------------------ thao tác cây thuần (test được bằng node)
    var T = {};

    T.newNode = function (text) {
        return { text: text || 'Mục mới', description: '', navUrl: '', value: '', tabId: -1, target: '', params: '', extra: {}, children: [] };
    };

    /** Tìm vị trí của node: { list, index, depth (1 = cấp 1), parent }. null nếu không có. */
    T.locate = function (items, node) {
        function walk(list, depth, parent) {
            for (var i = 0; i < list.length; i++) {
                if (list[i] === node) return { list: list, index: i, depth: depth, parent: parent };
                var r = walk(list[i].children || [], depth + 1, list[i]);
                if (r) return r;
            }
            return null;
        }
        return walk(items, 1, null);
    };

    /** Độ sâu của nhánh con dưới node (0 = không có con). */
    T.height = function (node) {
        var h = 0;
        (node.children || []).forEach(function (c) { h = Math.max(h, 1 + T.height(c)); });
        return h;
    };

    T.move = function (items, node, delta) {
        var loc = T.locate(items, node);
        if (!loc) return false;
        var j = loc.index + delta;
        if (j < 0 || j >= loc.list.length) return false;
        loc.list.splice(loc.index, 1);
        loc.list.splice(j, 0, node);
        return true;
    };

    /** Đưa node thành con cuối của mục liền trên (cùng cấp). */
    T.indent = function (items, node, maxLevels) {
        var loc = T.locate(items, node);
        if (!loc || loc.index === 0) return false;
        if (loc.depth + 1 + T.height(node) > maxLevels) return false;
        var prev = loc.list[loc.index - 1];
        loc.list.splice(loc.index, 1);
        (prev.children = prev.children || []).push(node);
        return true;
    };

    /** Đưa node lên một cấp, đứng ngay sau mục cha. */
    T.outdent = function (items, node) {
        var loc = T.locate(items, node);
        if (!loc || !loc.parent) return false;
        var ploc = T.locate(items, loc.parent);
        loc.list.splice(loc.index, 1);
        ploc.list.splice(ploc.index + 1, 0, node);
        return true;
    };

    T.remove = function (items, node) {
        var loc = T.locate(items, node);
        if (!loc) return false;
        loc.list.splice(loc.index, 1);
        return true;
    };

    T.count = function (items) {
        var n = 0;
        (items || []).forEach(function (c) { n += 1 + T.count(c.children); });
        return n;
    };

    /** Cảnh báo về cách skin hiển thị mục này. */
    T.warnings = function (items, node) {
        var loc = T.locate(items, node);
        if (!loc) return [];
        var w = [];
        if (loc.depth > SKIN_LEVELS) w.push('Skin quản trị chỉ hiển thị 2 cấp - mục này sẽ không xuất hiện.');
        if (loc.depth === SKIN_LEVELS && node.children && node.children.length)
            w.push('Mục cấp 2 có mục con sẽ bị skin ẩn. Hãy chuyển các mục con lên cấp 2.');
        if (loc.depth === 1 && (!node.children || !node.children.length))
            w.push('Mục cấp 1 chưa có mục con: bấm vào icon sẽ không mở khối menu nào.');
        if (node.tabId < 0)
            w.push('Chưa gắn trang: chỉ Administrators nhìn thấy mục này.');
        if (loc.depth === 1) {
            var dup = items.some(function (o) { return o !== node && node.value && o.value && o.value.toLowerCase() === node.value.toLowerCase(); });
            if (dup) w.push('Mã (Value) bị trùng với mục cấp 1 khác.');
        }
        return w;
    };

    /** Kiểm tra trước khi lưu (server kiểm tra lại). Trả về thông báo lỗi đầu tiên hoặc ''. */
    T.validate = function (items) {
        var err = '', seen = {};
        function walk(list, depth) {
            list.forEach(function (n) {
                if (err) return;
                if (!String(n.text || '').trim()) { err = 'Có mục chưa nhập tên.'; return; }
                if (/^\s*javascript:/i.test(n.navUrl || '')) { err = 'Mục "' + n.text + '": link không hợp lệ.'; return; }
                if (depth === 1 && n.value) {
                    var k = n.value.trim().toLowerCase();
                    if (seen[k]) { err = 'Mã "' + n.value + '" bị trùng giữa các mục cấp 1.'; return; }
                    seen[k] = true;
                }
                walk(n.children || [], depth + 1);
            });
        }
        walk(items, 1);
        return err;
    };

    T.esc = function (v) {
        if (v === null || v === undefined) return '';
        return String(v).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    };

    if (typeof module !== 'undefined' && module.exports) { module.exports = T; return; }

    // ------------------------------------------------------------------ giao diện

    function MenuAdmin(root) {
        this.root = root;
        this.cfg = {
            moduleId: root.getAttribute('data-module-id'),
            tabId: root.getAttribute('data-tab-id'),
            apiRoot: root.getAttribute('data-api-root'),
            canEdit: root.getAttribute('data-can-edit') === '1'
        };
        this.items = [];
        this.selected = null;
        this.version = '';
        this.rootName = '';
        this.itemName = '';
        this.tabs = [];
        this.tabUrl = {};
        this.file = '';
        this.dirty = false;
        this.busy = false;
    }

    var P = MenuAdmin.prototype;

    P.ref = function (name) { return this.root.querySelector('[data-ref="' + name + '"]'); };

    P.init = function () {
        var self = this;
        if (!this.cfg.canEdit) {
            this.ref('sub').textContent = '';
            this.ref('readonly').hidden = false;
            this.root.querySelector('.nvht-actions').hidden = true;
            return;
        }

        this.root.addEventListener('click', function (e) {
            var btn = e.target.closest('[data-act]');
            if (btn && self.root.contains(btn)) { e.preventDefault(); self.action(btn.getAttribute('data-act'), btn); }
        });
        this.root.addEventListener('keydown', function (e) {
            if (e.key === 'Enter' && e.target.tagName === 'INPUT') e.preventDefault();
            if (e.key === 'Escape') self.ref('modal').hidden = true;
        });
        this.ref('editor-form').addEventListener('input', function (e) { self.onField(e.target); });
        this.ref('editor-form').addEventListener('change', function (e) { self.onField(e.target, true); });
        global.addEventListener('beforeunload', function (e) {
            if (self.dirty) { e.preventDefault(); e.returnValue = ''; }
        });

        Promise.all([this.loadTabs(), this.load()]);
    };

    // ------------------------------------------------------------------ API

    P.api = function (method, action, body) {
        var headers = { 'ModuleId': String(this.cfg.moduleId), 'TabId': String(this.cfg.tabId), 'Accept': 'application/json' };
        var init = { method: method, headers: headers, credentials: 'same-origin' };
        if (method !== 'GET') {
            var tokenEl = document.querySelector('input[name="__RequestVerificationToken"]');
            if (!tokenEl) return Promise.resolve({ success: false, message: 'Không tìm thấy token chống giả mạo. Hãy tải lại trang.' });
            headers['RequestVerificationToken'] = tokenEl.value;
            headers['Content-Type'] = 'application/json';
            init.body = JSON.stringify(body || {});
        }
        return fetch(this.cfg.apiRoot + 'Menu/' + action, init).then(function (r) {
            return r.json().catch(function () { return { success: false, message: 'Lỗi máy chủ (HTTP ' + r.status + ').' }; })
                .then(function (data) { data.status = r.status; return data; });
        }, function () {
            return { success: false, message: 'Không kết nối được máy chủ.' };
        });
    };

    P.load = function () {
        var self = this;
        this.alert('');
        return this.api('GET', 'Tree').then(function (res) {
            if (!res.success) { self.alert(res.message, 'error'); return; }
            self.items = res.items || [];
            self.version = res.version || '';
            self.rootName = res.rootName;
            self.itemName = res.itemName;
            self.selected = null;
            self.setDirty(false);
            self.file = res.file || '';
            self.ref('sub').innerHTML = (res.exists ? 'File ' : 'Chưa có file - sẽ tạo mới khi lưu: ') + '<code>' + T.esc(self.file) + '</code>';
            self.ref('grid').hidden = false;
            self.render();
        });
    };

    P.loadTabs = function () {
        var self = this;
        return this.api('GET', 'Tabs').then(function (res) {
            if (!res.success) return;
            self.tabs = res.tabs || [];
            self.tabs.forEach(function (t) { self.tabUrl[t.id] = t.url; });
            var sel = self.root.querySelector('[data-field="tabId"]');
            sel.innerHTML = '<option value="-1">(Không gắn trang)</option>' + self.tabs.map(function (t) {
                return '<option value="' + t.id + '">' + T.esc(t.name) + (t.visible ? '' : ' (ẩn)') + '</option>';
            }).join('');
            if (self.selected) self.fillForm();
        });
    };

    P.save = function (force) {
        var self = this;
        var err = T.validate(this.items);
        if (err) { this.alert(err, 'error'); return; }
        if (this.busy) return;
        this.busy = true;
        this.setSaving(true);
        this.api('POST', 'Save', { items: this.items, version: this.version, rootName: this.rootName, itemName: this.itemName, force: !!force })
            .then(function (res) {
                self.busy = false;
                self.setSaving(false);
                if (res.status === 409 && res.conflict) {
                    if (global.confirm(res.message + '\n\nLưu đè lên bản của người khác?')) self.save(true);
                    return;
                }
                if (!res.success) { self.alert(res.message, 'error'); return; }
                // giữ mục đang chọn theo vị trí sau khi server chuẩn hóa dữ liệu
                var path = self.pathOf(self.selected);
                self.items = res.items || [];
                self.version = res.version || '';
                self.selected = path ? self.nodeAt(path) : null;
                self.setDirty(false);
                self.render();
                self.ref('sub').innerHTML = 'File <code>' + T.esc(self.file) + '</code>';
                self.alert(res.message || 'Đã lưu.', 'ok');
            });
    };

    // ------------------------------------------------------------------ hành động

    P.action = function (act, btn) {
        var node = this.selected, items = this.items, changed = false;
        switch (act) {
            case 'reload':
                if (this.dirty && !global.confirm('Bỏ các thay đổi chưa lưu?')) return;
                this.load();
                return;
            case 'save':
                this.save(false);
                return;
            case 'add-root':
                node = T.newNode('Mục cấp 1 mới');
                items.push(node);
                this.selected = node;
                changed = true;
                break;
            case 'add-child':
                if (!node) return;
                var loc = T.locate(items, node);
                if (loc.depth >= SKIN_LEVELS) { this.alert('Skin quản trị chỉ hiển thị 2 cấp, không thêm mục con cho mục cấp 2.', 'error'); return; }
                var child = T.newNode('Mục con mới');
                node.children.push(child);
                this.selected = child;
                changed = true;
                break;
            case 'remove':
                if (!node) return;
                var n = T.count(node.children);
                if (!global.confirm('Xóa mục "' + node.text + '"' + (n ? ' cùng ' + n + ' mục con' : '') + '?')) return;
                T.remove(items, node);
                this.selected = null;
                changed = true;
                break;
            case 'select':
                this.selected = this.nodeAt(btn.getAttribute('data-path'));
                this.render();
                var first = this.root.querySelector('[data-field="text"]');
                if (first) first.focus();
                return;
            case 'up': case 'down': case 'indent': case 'outdent':
                node = this.nodeAt(btn.getAttribute('data-path'));
                this.selected = node;
                if (act === 'up') changed = T.move(items, node, -1);
                else if (act === 'down') changed = T.move(items, node, 1);
                else if (act === 'indent') {
                    changed = T.indent(items, node, SKIN_LEVELS);
                    if (!changed) this.alert('Không thể chuyển xuống cấp dưới (cần có mục cùng cấp phía trên và tối đa 2 cấp).', 'error');
                }
                else changed = T.outdent(items, node);
                if (!changed) { this.render(); return; }
                break;
            case 'backups':
                this.openBackups();
                return;
            case 'close-modal':
                this.ref('modal').hidden = true;
                return;
            case 'open-backup':
                this.openBackup(btn.getAttribute('data-name'));
                return;
            default:
                return;
        }
        if (changed) { this.setDirty(true); this.render(); }
        if (act === 'add-root' || act === 'add-child') {
            var input = this.root.querySelector('[data-field="text"]');
            if (input) { input.focus(); input.select(); }
        }
    };

    P.onField = function (el, isChange) {
        var f = el.getAttribute('data-field'), node = this.selected;
        if (!f || !node) return;
        if (f === 'tabId') {
            if (!isChange) return;
            var oldUrl = this.tabUrl[node.tabId] || '';
            node.tabId = parseInt(el.value, 10);
            var url = this.tabUrl[node.tabId] || '';
            // tự điền link nếu link đang trống hoặc đang là link của trang cũ
            if (url && (!node.navUrl || node.navUrl === oldUrl)) {
                node.navUrl = url;
                this.root.querySelector('[data-field="navUrl"]').value = url;
            }
        } else {
            node[f] = el.value;
        }
        this.setDirty(true);
        this.renderTree();
        this.renderWarnings();
    };

    // ------------------------------------------------------------------ bản sao lưu

    P.openBackups = function () {
        var box = this.ref('backups');
        this.ref('modal').hidden = false;
        box.innerHTML = '<p class="nvht-hint">Đang tải…</p>';
        this.api('GET', 'Backups').then(function (res) {
            if (!res.success) { box.innerHTML = '<p class="nvht-hint">' + T.esc(res.message) + '</p>'; return; }
            if (!res.backups.length) { box.innerHTML = '<p class="nvht-hint">Chưa có bản sao lưu nào.</p>'; return; }
            box.innerHTML = '<ul class="nvht-backups">' + res.backups.map(function (b) {
                var d = new Date(b.time);
                return '<li><span>' + T.esc(isNaN(d) ? b.name : d.toLocaleString('vi-VN')) + '<small>' + T.esc(b.name) + ' · ' + Math.ceil(b.size / 1024) + ' KB</small></span>' +
                    '<button type="button" class="nvht-btn nvht-btn-soft" data-act="open-backup" data-name="' + T.esc(b.name) + '">Mở</button></li>';
            }).join('') + '</ul>';
        });
    };

    P.openBackup = function (name) {
        var self = this;
        if (this.dirty && !global.confirm('Bỏ các thay đổi chưa lưu và mở bản sao lưu?')) return;
        this.api('GET', 'Backup?name=' + encodeURIComponent(name)).then(function (res) {
            if (!res.success) { self.alert(res.message, 'error'); return; }
            self.items = res.items || [];
            self.rootName = res.rootName;
            self.itemName = res.itemName;
            self.selected = null;
            self.ref('modal').hidden = true;
            self.setDirty(true);
            self.render();
            self.alert('Đang xem bản sao lưu ' + name + '. Bấm “Lưu menu” để khôi phục, hoặc “Tải lại” để bỏ.', 'info');
        });
    };

    // ------------------------------------------------------------------ hiển thị

    P.pathOf = function (node) {
        if (!node) return null;
        var path = [], cur = node, loc;
        while ((loc = T.locate(this.items, cur))) {
            path.unshift(loc.index);
            if (!loc.parent) break;
            cur = loc.parent;
        }
        return path.length ? path.join('.') : null;
    };

    P.nodeAt = function (path) {
        var list = this.items, node = null;
        String(path).split('.').forEach(function (i) {
            node = list ? list[parseInt(i, 10)] : null;
            list = node ? node.children : null;
        });
        return node;
    };

    P.render = function () {
        this.renderTree();
        this.fillForm();
    };

    P.renderTree = function () {
        var self = this, tree = this.ref('tree');
        if (!this.items.length) {
            tree.innerHTML = '<div class="nvht-empty">Menu đang trống. Bấm “+ Mục cấp 1” để bắt đầu.</div>';
            return;
        }
        function btn(act, path, label, title, disabled) {
            return '<button type="button" class="nvht-icon" data-act="' + act + '" data-path="' + path + '" title="' + title + '" aria-label="' + title + '"' + (disabled ? ' disabled' : '') + '>' + label + '</button>';
        }
        function list(nodes, depth, prefix) {
            return '<ul class="nvht-list nvht-depth-' + depth + '">' + nodes.map(function (n, i) {
                var path = prefix + i;
                var warn = T.warnings(self.items, n).length > 0;
                var hidden = depth > SKIN_LEVELS || (depth === SKIN_LEVELS && n.children && n.children.length);
                return '<li>' +
                    '<div class="nvht-node' + (n === self.selected ? ' is-on' : '') + (hidden ? ' is-hidden' : '') + '">' +
                    '<button type="button" class="nvht-node-main" data-act="select" data-path="' + path + '">' +
                    '<span class="nvht-node-text">' + T.esc(n.text || '(chưa đặt tên)') + '</span>' +
                    '<span class="nvht-node-url">' + T.esc(depth === 1 ? (n.description || n.value) : n.navUrl) + '</span>' +
                    (warn ? '<span class="nvht-dot" title="Có cảnh báo"></span>' : '') +
                    '</button>' +
                    '<span class="nvht-node-tools">' +
                    btn('up', path, '↑', 'Lên trên', i === 0) +
                    btn('down', path, '↓', 'Xuống dưới', i === nodes.length - 1) +
                    btn('outdent', path, '←', 'Lên cấp trên', depth === 1) +
                    btn('indent', path, '→', 'Thành mục con của mục phía trên', i === 0 || depth + 1 + T.height(n) > SKIN_LEVELS) +
                    '</span></div>' +
                    (n.children && n.children.length ? list(n.children, depth + 1, path + '.') : '') +
                    '</li>';
            }).join('') + '</ul>';
        }
        tree.innerHTML = list(this.items, 1, '');
    };

    P.fillForm = function () {
        var node = this.selected, form = this.ref('editor-form');
        this.ref('editor-empty').hidden = !!node;
        form.hidden = !node;
        if (!node) return;
        var loc = T.locate(this.items, node);
        this.ref('editor-title').textContent = node.text || 'Sửa mục';
        this.ref('editor-level').textContent = 'Cấp ' + loc.depth;
        form.querySelectorAll('[data-only="root"]').forEach(function (el) { el.hidden = loc.depth !== 1; });
        form.querySelector('[data-act="add-child"]').hidden = loc.depth >= SKIN_LEVELS;
        form.querySelectorAll('[data-field]').forEach(function (el) {
            var f = el.getAttribute('data-field');
            var v = node[f] === null || node[f] === undefined ? '' : String(node[f]);
            if (el.tagName === 'SELECT' && !el.querySelector('option[value="' + v.replace(/"/g, '') + '"]')) {
                var o = document.createElement('option');
                o.value = v;
                o.textContent = f === 'tabId' ? 'TabId ' + v + ' (không tìm thấy)' : v;
                el.appendChild(o);
            }
            el.value = v;
        });
        var extra = node.extra || {}, keys = Object.keys(extra), ex = this.ref('extra');
        ex.hidden = !keys.length;
        ex.innerHTML = keys.length ? '<span>Thuộc tính khác (giữ nguyên khi lưu):</span> ' + keys.map(function (k) {
            return '<code>' + T.esc(k) + '="' + T.esc(extra[k]) + '"</code>';
        }).join(' ') : '';
        this.renderWarnings();
    };

    P.renderWarnings = function () {
        if (!this.selected) return;
        var w = T.warnings(this.items, this.selected), box = this.ref('editor-warn');
        this.ref('editor-title').textContent = this.selected.text || 'Sửa mục';
        box.hidden = !w.length;
        box.innerHTML = w.map(function (s) { return '<div>' + T.esc(s) + '</div>'; }).join('');
    };

    P.setDirty = function (on) {
        this.dirty = on;
        var b = this.root.querySelector('[data-act="save"]');
        b.disabled = !on;
        b.textContent = on ? 'Lưu menu •' : 'Lưu menu';
    };

    P.setSaving = function (on) {
        var b = this.root.querySelector('[data-act="save"]');
        b.disabled = on || !this.dirty;
        if (on) b.textContent = 'Đang lưu…';
    };

    P.alert = function (msg, kind) {
        var a = this.ref('alert');
        a.hidden = !msg;
        a.className = 'nvht-alert nvht-alert-' + (kind || 'info');
        a.textContent = msg || '';
        if (msg && kind === 'ok') {
            clearTimeout(this._alertTimer);
            this._alertTimer = setTimeout(function () { a.hidden = true; }, 3000);
        }
    };

    function boot() {
        var roots = document.querySelectorAll('[data-nvht-menu]');
        Array.prototype.forEach.call(roots, function (r) {
            if (r.__nvht) return;
            r.__nvht = new MenuAdmin(r);
            r.__nvht.init();
        });
    }

    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', boot);
    else boot();
})(typeof window !== 'undefined' ? window : this);
