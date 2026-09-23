/*!
 * Zalo OA Chat - DNN module (DesktopModules/NVCMS.Marketing/Manager/ZaloOAChat)
 * Gọi /DesktopModules/NVCMS/API/ZaloOAChat/* qua DNN ServicesFramework (anti-forgery).
 * JSON trả về dạng camelCase: { success, message, data, totalRecords, errorCode, traceId }.
 * Thời gian từ server là UTC ISO-8601 ("...Z"); hiển thị theo giờ trình duyệt.
 * Mọi nội dung từ khách được escape trước khi đưa vào DOM (chống XSS).
 */
(function ($, window, document) {
    'use strict';

    var cfg = window.ZaloOAChatConfig;
    if (!cfg || !$ || !$.ServicesFramework) {
        if (window.console) console.error('ZaloOAChat: thiếu cấu hình hoặc DNN ServicesFramework.');
        return;
    }

    var sf = $.ServicesFramework(cfg.moduleId);
    var POLL_ACTIVE_MS = 5000, POLL_LIST_MS = 10000, POLL_HIDDEN_MS = 30000;
    var PAGE_SIZE_LIST = 20, PAGE_SIZE_MSG = 30;
    var NOTIFY_KEY = 'zoa.notify.enabled';

    var state = {
        filter: '',
        keyword: '',
        listPage: 0,
        listTotal: 0,
        conversations: [],          // theo thứ tự hiển thị
        activeId: null,
        active: null,
        messages: {},               // key -> message (key = id hoặc 'tmp-' + clientMessageId)
        oldestId: 0,
        hasOlder: false,
        since: null,                // chuỗi ISO UTC updatedAt lớn nhất đã nhận
        lastInboundId: null,        // cho thông báo trình duyệt
        totalUnread: 0,
        sending: false,
        timers: {}
    };

    // ── API ────────────────────────────────────────────────────────────────

    function api(method, action, data, extra) {
        var opts = $.extend({
            type: method,
            url: cfg.serviceRoot + action,
            beforeSend: sf.setModuleHeaders,
            dataType: 'json'
        }, extra || {});

        if (method === 'GET') {
            opts.data = data;
        } else if (!(data instanceof FormData)) {
            opts.contentType = 'application/json; charset=utf-8';
            opts.data = JSON.stringify(data || {});
        } else {
            opts.data = data;
            opts.processData = false;
            opts.contentType = false;
        }

        return $.ajax(opts).then(
            function (res) { return res; },
            function (xhr) {
                var res = xhr.responseJSON || {};
                return $.Deferred().reject({
                    message: res.message || res.Message || ('Lỗi kết nối (' + xhr.status + ')'),
                    errorCode: res.errorCode,
                    traceId: res.traceId,
                    data: res.data,
                    status: xhr.status
                }).promise();
            });
    }

    function toast(msg, type) {
        if (window.NioApp && NioApp.Toast) NioApp.Toast(msg, type || 'info', { position: 'top-right' });
        else if (window.console) console.log(msg);
    }

    function apiError(err, prefix) {
        var msg = (prefix ? prefix + ': ' : '') + (err && err.message ? err.message : 'Có lỗi xảy ra');
        if (err && err.traceId) msg += ' (mã: ' + err.traceId + ')';
        toast(msg, 'danger');
    }

    // ── Helpers ────────────────────────────────────────────────────────────

    function esc(s) {
        return String(s == null ? '' : s)
            .replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;').replace(/'/g, '&#39;');
    }

    function safeUrl(u) {
        return typeof u === 'string' && /^https?:\/\//i.test(u) ? u : null;
    }

    function linkify(escapedText) {
        return escapedText.replace(/(https?:\/\/[^\s<]+)/g, function (url) {
            return '<a href="' + url + '" target="_blank" rel="noopener noreferrer">' + url + '</a>';
        });
    }

    function parseDate(s) { return s ? new Date(s) : null; }

    function pad(n) { return n < 10 ? '0' + n : '' + n; }

    function fmtTime(s) {
        var d = parseDate(s); if (!d) return '';
        return pad(d.getHours()) + ':' + pad(d.getMinutes());
    }

    function fmtDay(d) {
        return pad(d.getDate()) + '/' + pad(d.getMonth() + 1) + '/' + d.getFullYear();
    }

    function fmtDateTime(s) {
        var d = parseDate(s); if (!d) return '';
        return fmtDay(d) + ' ' + pad(d.getHours()) + ':' + pad(d.getMinutes());
    }

    function fmtRelative(s) {
        var d = parseDate(s); if (!d) return '';
        var diff = (Date.now() - d.getTime()) / 1000;
        if (diff < 60) return 'Vừa xong';
        if (diff < 3600) return Math.floor(diff / 60) + ' phút';
        var today = new Date(); today.setHours(0, 0, 0, 0);
        if (d >= today) return fmtTime(s);
        if (diff < 7 * 86400) return Math.ceil((today - d) / 86400000) + ' ngày';
        return pad(d.getDate()) + '/' + pad(d.getMonth() + 1);
    }

    function initials(name) {
        var parts = String(name || '?').trim().split(/\s+/);
        var a = parts[0] ? parts[0][0] : '?';
        var b = parts.length > 1 ? parts[parts.length - 1][0] : '';
        return (a + b).toUpperCase();
    }

    function avatarHtml(url, name) {
        var u = safeUrl(url);
        return u ? '<img src="' + esc(u) + '" alt="" loading="lazy">' : '<span>' + esc(initials(name)) + '</span>';
    }

    function displayName(c) {
        return (c && (c.displayName || c.sharedName || c.userAlias)) || ('Khách ' + (c && c.zaloUserId ? c.zaloUserId.slice(-4) : ''));
    }

    function newGuid() {
        if (window.crypto && crypto.randomUUID) return crypto.randomUUID();
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    function storageGet(k) { try { return window.localStorage.getItem(k); } catch (e) { return null; } }
    function storageSet(k, v) { try { window.localStorage.setItem(k, v); } catch (e) { /* bỏ qua */ } }

    // ── Danh sách hội thoại ────────────────────────────────────────────────

    function listParams(page) {
        var p = { pageIndex: page, pageSize: PAGE_SIZE_LIST, keyword: state.keyword };
        if (state.filter === 'unread') p.unreadOnly = true;
        else if (state.filter) p.status = state.filter;
        return p;
    }

    function loadConversations(reset) {
        var page = reset ? 0 : state.listPage + 1;
        return api('GET', 'GetConversations', listParams(page)).done(function (res) {
            var items = res.data || [];
            state.listPage = page;
            state.listTotal = res.totalRecords || 0;
            state.conversations = reset ? items : mergeConversations(state.conversations, items);
            renderConversationList();
        }).fail(function (err) { apiError(err, 'Không tải được hội thoại'); });
    }

    /** Polling: tải lại trang đầu, giữ các trang đã tải thêm. */
    function refreshConversationHead() {
        return api('GET', 'GetConversations', listParams(0)).done(function (res) {
            state.listTotal = res.totalRecords || 0;
            var fresh = res.data || [];
            var freshIds = {};
            $.each(fresh, function (_, c) { freshIds[c.id] = true; });
            var rest = $.grep(state.conversations, function (c) { return !freshIds[c.id]; });
            state.conversations = state.listPage > 0 ? fresh.concat(rest) : fresh;
            renderConversationList();

            if (state.activeId) {
                $.each(fresh, function (_, c) {
                    if (c.id === state.activeId) { state.active = $.extend(state.active || {}, c); renderHeader(); }
                });
            }
        });
    }

    function mergeConversations(list, more) {
        var ids = {};
        $.each(list, function (_, c) { ids[c.id] = true; });
        return list.concat($.grep(more, function (c) { return !ids[c.id]; }));
    }

    function renderConversationList() {
        var html = $.map(state.conversations, conversationItemHtml).join('');
        $('#zoaConversationList').html(html);
        $('#zoaListEmpty').toggle(state.conversations.length === 0);
        $('#zoaLoadMoreList').toggle(state.conversations.length < state.listTotal);
    }

    function conversationItemHtml(c) {
        var name = displayName(c);
        var prefix = c.lastMessageSenderType === 'AGENT' || c.lastMessageSenderType === 'OA' ? 'Bạn: ' : '';
        var cls = 'chat-item' + (c.id === state.activeId ? ' current' : '') + (c.unreadCount > 0 ? ' is-unread' : '');
        return '<li class="' + cls + '" data-id="' + c.id + '">' +
            '<a class="chat-link" href="#">' +
            '<div class="chat-media user-avatar bg-primary">' + avatarHtml(c.avatarUrl, name) + '</div>' +
            '<div class="chat-info">' +
            '<div class="chat-from"><div class="name">' + esc(name) +
            (c.status === 'CLOSED' ? '<span class="zoa-status-closed">Đã đóng</span>' : '') +
            '</div><span class="time">' + esc(fmtRelative(c.lastMessageAt)) + '</span></div>' +
            '<div class="chat-context"><div class="text"><p>' + esc(prefix + (c.lastMessagePreview || '')) + '</p></div>' +
            (c.unreadCount > 0 ? '<div class="zoa-badge-unread">' + (c.unreadCount > 99 ? '99+' : c.unreadCount) + '</div>' : '') +
            '</div></div></a></li>';
    }

    // ── Hội thoại đang mở ──────────────────────────────────────────────────

    function openConversation(id) {
        stopTimer('active');
        state.activeId = id;
        state.active = null;
        state.messages = {};
        state.oldestId = 0;
        state.hasOlder = false;
        state.since = null;

        $('#zoaBlank').hide();
        $('#zoaChatHead, #zoaPanel, #zoaEditor').show();
        $('#zoaChatBody').addClass('show-chat');
        $('#zoaMessages').html('<div class="text-center text-soft py-4">Đang tải...</div>');
        renderConversationList();

        $.when(api('GET', 'GetConversation', { id: id }), api('GET', 'GetMessages', { conversationId: id, pageSize: PAGE_SIZE_MSG }))
            .done(function (convRes, msgRes) {
                if (state.activeId !== id) return;
                state.active = convRes[0].data;
                var list = msgRes[0].data || [];
                addMessages(list);
                state.hasOlder = list.length >= PAGE_SIZE_MSG;
                renderHeader();
                renderMessages(true);
                loadProfile();
                markRead();
                startTimer('active', pollActive, POLL_ACTIVE_MS);
            })
            .fail(function (err) { apiError(err, 'Không mở được hội thoại'); });
    }

    function renderHeader() {
        var c = state.active; if (!c) return;
        var name = displayName(c);
        $('#zoaHeadAvatar').attr('class', 'user-avatar bg-primary').html(avatarHtml(c.avatarUrl, name));
        $('#zoaHeadName').text(name);
        var sub = [];
        if (c.sharedPhone) sub.push(c.sharedPhone);
        if (c.lastCustomerMessageAt) sub.push('Khách nhắn lần cuối: ' + fmtDateTime(c.lastCustomerMessageAt));
        if (c.isFollower === false) sub.push('Chưa quan tâm OA');
        $('#zoaHeadSub').text(sub.join(' · '));
        $('#zoaToggleStatus').text(c.status === 'CLOSED' ? 'Mở lại' : 'Đóng hội thoại')
            .toggleClass('btn-outline-primary', c.status === 'CLOSED')
            .toggleClass('btn-outline-secondary', c.status !== 'CLOSED');
        $('#zoaWindowWarning').toggle(!c.canReplyWithin7Days);
    }

    function msgKey(m) { return m.id ? 'id-' + m.id : 'tmp-' + m.clientMessageId; }

    function addMessages(list) {
        $.each(list, function (_, m) {
            // Tin thật về từ server thay tin tạm (optimistic) cùng clientMessageId
            if (m.id && m.clientMessageId && state.messages['tmp-' + m.clientMessageId]) delete state.messages['tmp-' + m.clientMessageId];
            state.messages[msgKey(m)] = m;
            if (m.updatedAt && (!state.since || m.updatedAt > state.since)) state.since = m.updatedAt;
        });
        // Cursor tải tin cũ = tin sớm nhất theo (SentAt, Id), không phải Id nhỏ nhất (tin nhập lịch sử có Id lớn)
        var sorted = sortedMessages();
        for (var i = 0; i < sorted.length; i++) {
            if (sorted[i].id) { state.oldestId = sorted[i].id; break; }
        }
    }

    function sortedMessages() {
        var arr = $.map(state.messages, function (m) { return m; });
        arr.sort(function (a, b) {
            var ta = parseDate(a.sentAt).getTime(), tb = parseDate(b.sentAt).getTime();
            if (ta !== tb) return ta - tb;
            return (a.id || Number.MAX_SAFE_INTEGER) - (b.id || Number.MAX_SAFE_INTEGER);
        });
        return arr;
    }

    function renderMessages(scrollToBottom, keepOffsetFrom) {
        var panel = document.getElementById('zoaPanel');
        var nearBottom = panel.scrollHeight - panel.scrollTop - panel.clientHeight < 80;
        var html = [], lastDay = null;

        $.each(sortedMessages(), function (_, m) {
            var d = parseDate(m.sentAt);
            var day = d ? fmtDay(d) : '';
            if (day !== lastDay) {
                html.push('<div class="chat-sap"><div class="chat-sap-meta"><span>' + esc(day) + '</span></div></div>');
                lastDay = day;
            }
            html.push(messageHtml(m));
        });

        $('#zoaMessages').html(html.length ? html.join('') : '<div class="text-center text-soft py-4">Chưa có tin nhắn.</div>');
        $('#zoaLoadOlderWrap').toggle(state.hasOlder);

        if (keepOffsetFrom != null) panel.scrollTop = panel.scrollHeight - keepOffsetFrom;
        else if (scrollToBottom || nearBottom) panel.scrollTop = panel.scrollHeight;
    }

    function attachmentInfo(m) {
        if (!m.attachmentsJson) return null;
        try {
            var a = JSON.parse(m.attachmentsJson);
            if ($.isArray(a)) {                       // webhook: [{type, payload:{url, thumbnail, name}}]
                var p = (a[0] && a[0].payload) || {};
                return { url: p.url, thumb: p.thumbnail || p.thumb, name: p.name };
            }
            return { url: a.url || a.Url, thumb: a.thumb, name: a.fileName || a.FileName };   // gửi đi / lịch sử
        } catch (e) { return null; }
    }

    function contentHtml(m) {
        var att = attachmentInfo(m) || {};
        var url = safeUrl(att.url), thumb = safeUrl(att.thumb) || url;
        var text = m.content || '';

        switch (m.messageType) {
            case 'IMAGE':
                var imgHtml = thumb ? '<a href="' + esc(url || thumb) + '" target="_blank" rel="noopener noreferrer"><img class="zoa-img" src="' + esc(thumb) + '" alt="" loading="lazy"></a>' : '[Hình ảnh]';
                var caption = text && text !== att.url && text !== att.name ? '<div class="mt-1">' + linkify(esc(text)) + '</div>' : '';
                return imgHtml + caption;
            case 'STICKER':
                return thumb ? '<img class="zoa-sticker" src="' + esc(thumb) + '" alt="sticker" loading="lazy">' : '[Sticker]';
            case 'FILE':
                var name = att.name || text || 'Tệp đính kèm';
                return (url ? '<a href="' + esc(url) + '" target="_blank" rel="noopener noreferrer">' : '') +
                    '<em class="icon ni ni-file-docs"></em> ' + esc(name) + (url ? '</a>' : '');
            case 'LOCATION':
                var coords = /^-?\d+(\.\d+)?,-?\d+(\.\d+)?$/.test(text) ? text : null;
                return coords
                    ? '<a href="https://www.google.com/maps?q=' + encodeURIComponent(coords) + '" target="_blank" rel="noopener noreferrer"><em class="icon ni ni-map-pin"></em> Vị trí</a>'
                    : '[Vị trí]';
            case 'AUDIO':
            case 'VIDEO':
                return url ? '<a href="' + esc(url) + '" target="_blank" rel="noopener noreferrer">[' + (m.messageType === 'AUDIO' ? 'Âm thanh' : 'Video') + ']</a>' : '[' + m.messageType + ']';
            default:
                return text ? linkify(esc(text)) : '<i class="text-soft">[' + esc(m.messageType) + ']</i>';
        }
    }

    function statusHtml(m) {
        switch (m.status) {
            case 'PENDING': return '<em class="icon ni ni-clock" title="Đang gửi"></em>';
            case 'SENT': return '<em class="icon ni ni-check" title="Đã gửi"></em>';
            case 'DELIVERED': return '<em class="icon ni ni-check-circle" title="Đã nhận"></em>';
            case 'SEEN': return '<em class="icon ni ni-check-circle-fill text-success" title="Đã xem"></em>';
            case 'FAILED':
                return '<em class="icon ni ni-alert-circle text-danger" title="' + esc(m.errorMessage || 'Gửi lỗi') + '"></em>' +
                    (m.id ? ' <span class="zoa-retry" data-id="' + m.id + '">Gửi lại</span>' : '');
            default: return '';
        }
    }

    function messageHtml(m) {
        if (m.direction === 'SYS' || m.senderType === 'SYSTEM') {
            return '<div class="zoa-system">' + esc(m.content) + ' · ' + esc(fmtTime(m.sentAt)) + '</div>';
        }

        var mine = m.direction === 'OUT';
        var who = mine
            ? (m.senderType === 'AGENT' ? (m.agentUserId === cfg.userId ? 'Bạn' : 'Nhân viên #' + (m.agentUserId || '')) : 'OA')
            : displayName(state.active);
        var cls = 'chat ' + (mine ? 'is-me' : 'is-you') + (m.status === 'FAILED' ? ' zoa-failed' : '') + (m.status === 'PENDING' ? ' zoa-pending' : '');
        var failReason = m.status === 'FAILED' && m.errorMessage ? '<li class="text-danger">' + esc(m.errorMessage) + '</li>' : '';

        return '<div class="' + cls + '">' +
            (mine ? '' : '<div class="chat-avatar"><div class="user-avatar bg-primary">' + avatarHtml(state.active && state.active.avatarUrl, who) + '</div></div>') +
            '<div class="chat-content"><div class="chat-bubbles"><div class="chat-bubble"><div class="chat-msg">' + contentHtml(m) + '</div></div></div>' +
            '<ul class="chat-meta"><li>' + esc(who) + '</li><li>' + esc(fmtTime(m.sentAt)) + '</li>' +
            (mine ? '<li class="zoa-meta-status">' + statusHtml(m) + '</li>' : '') + failReason + '</ul>' +
            '</div></div>';
    }

    function loadOlder() {
        if (!state.activeId || !state.hasOlder || !state.oldestId) return;
        var id = state.activeId, panel = document.getElementById('zoaPanel');
        var offset = panel.scrollHeight - panel.scrollTop;
        $('#zoaLoadOlder').prop('disabled', true);
        api('GET', 'GetMessages', { conversationId: id, beforeId: state.oldestId, pageSize: PAGE_SIZE_MSG })
            .done(function (res) {
                if (state.activeId !== id) return;
                var list = res.data || [];
                addMessages(list);
                state.hasOlder = list.length >= PAGE_SIZE_MSG;
                renderMessages(false, offset);
            })
            .fail(function (err) { apiError(err, 'Không tải được tin cũ'); })
            .always(function () { $('#zoaLoadOlder').prop('disabled', false); });
    }

    function pollActive() {
        if (!state.activeId) return;
        var id = state.activeId;
        var since = state.since || new Date(Date.now() - 60000).toISOString();
        return api('GET', 'GetMessageChanges', { conversationId: id, since: since }).done(function (res) {
            if (state.activeId !== id) return;
            var list = res.data || [];
            if (!list.length) return;
            var hasInbound = $.grep(list, function (m) { return m.direction === 'IN'; }).length > 0;
            addMessages(list);
            renderMessages(false);
            if (hasInbound && !document.hidden) markRead();
        });
    }

    function markRead() {
        if (!state.activeId) return;
        var id = state.activeId;
        api('POST', 'MarkRead', { Id: id }).done(function () {
            $.each(state.conversations, function (_, c) { if (c.id === id) c.unreadCount = 0; });
            renderConversationList();
            pollSummary();
        });
    }

    // ── Gửi tin ────────────────────────────────────────────────────────────

    function sendText() {
        var $input = $('#zoaInput');
        var text = $.trim($input.val());
        if (!text || !state.activeId || state.sending) return;
        if (text.length > 2000) { toast('Tin nhắn tối đa 2000 ký tự.', 'warning'); return; }

        var clientId = newGuid();
        var temp = {
            clientMessageId: clientId, conversationId: state.activeId, direction: 'OUT', senderType: 'AGENT',
            agentUserId: cfg.userId, messageType: 'TEXT', content: text, status: 'PENDING', sentAt: new Date().toISOString()
        };
        state.messages[msgKey(temp)] = temp;
        renderMessages(true);
        $input.val('').trigger('input');

        state.sending = true;
        api('POST', 'SendMessage', { ConversationId: state.activeId, ClientMessageId: clientId, MessageType: 'TEXT', Text: text })
            .done(function (res) { if (res.data) { addMessages([res.data]); renderMessages(true); } })
            .fail(function (err) {
                if (err.data && err.data.id) addMessages([err.data]);       // tin đã lưu FAILED, có nút "Gửi lại"
                else { temp.status = 'FAILED'; temp.errorMessage = err.message; }
                renderMessages(true);
                apiError(err, 'Gửi tin thất bại');
            })
            .always(function () { state.sending = false; refreshConversationHead(); });
    }

    function sendFile(file) {
        if (!file || !state.activeId) return;
        var isImage = /\.(jpe?g|png)$/i.test(file.name), isDoc = /\.(pdf|docx?|csv)$/i.test(file.name);
        if (!isImage && !isDoc) { toast('Chỉ hỗ trợ JPG/PNG hoặc PDF/DOC/DOCX/CSV.', 'warning'); return; }
        if (isImage && file.size > 1024 * 1024) { toast('Ảnh tối đa 1MB (giới hạn Zalo).', 'warning'); return; }
        if (isDoc && file.size > 5 * 1024 * 1024) { toast('Tệp tối đa 5MB (giới hạn Zalo).', 'warning'); return; }

        var clientId = newGuid();
        var temp = {
            clientMessageId: clientId, conversationId: state.activeId, direction: 'OUT', senderType: 'AGENT',
            agentUserId: cfg.userId, messageType: isImage ? 'IMAGE' : 'FILE', content: file.name,
            status: 'PENDING', sentAt: new Date().toISOString()
        };
        state.messages[msgKey(temp)] = temp;
        renderMessages(true);

        var fd = new FormData();
        fd.append('conversationId', state.activeId);
        fd.append('clientMessageId', clientId);
        fd.append('file', file, file.name);

        api('POST', 'SendAttachment', fd)
            .done(function (res) { if (res.data) { addMessages([res.data]); renderMessages(true); } })
            .fail(function (err) {
                if (err.data && err.data.id) addMessages([err.data]);
                else { temp.status = 'FAILED'; temp.errorMessage = err.message; }
                renderMessages(true);
                apiError(err, 'Gửi tệp thất bại');
            })
            .always(function () { refreshConversationHead(); });
    }

    function retryMessage(id) {
        var m = state.messages['id-' + id]; if (!m) return;
        m.status = 'PENDING'; renderMessages(false);
        api('POST', 'RetryMessage', { Id: id })
            .done(function (res) { if (res.data) addMessages([res.data]); })
            .fail(function (err) {
                if (err.data && err.data.id) addMessages([err.data]); else m.status = 'FAILED';
                apiError(err, 'Gửi lại thất bại');
            })
            .always(function () { renderMessages(false); });
    }

    // ── Trạng thái hội thoại / hồ sơ khách ─────────────────────────────────

    function toggleStatus() {
        var c = state.active; if (!c) return;
        var next = c.status === 'CLOSED' ? 'OPEN' : 'CLOSED';
        if (next === 'CLOSED' && !window.confirm('Đóng hội thoại này? Khách nhắn tin mới sẽ tự mở lại.')) return;
        api('POST', 'SetStatus', { Id: c.id, Status: next }).done(function () {
            c.status = next; renderHeader(); refreshConversationHead();
            toast(next === 'CLOSED' ? 'Đã đóng hội thoại.' : 'Đã mở lại hội thoại.', 'success');
        }).fail(function (err) { apiError(err, 'Không đổi được trạng thái'); });
    }

    function loadProfile() {
        var c = state.active; if (!c) return;
        api('GET', 'GetCustomer', { id: c.customerId }).done(function (res) { renderProfile(res.data); });
    }

    function renderProfile(p) {
        if (!p) return;
        var name = displayName(p);
        $('#zoaProfileAvatar').attr('class', 'user-avatar md bg-primary').html(avatarHtml(p.avatarUrl, name));
        $('#zoaProfileName').text(name);
        $('#zoaProfileFollow').text(p.isFollower === true ? 'Đang quan tâm OA' : (p.isFollower === false ? 'Chưa quan tâm OA' : ''));

        var tags = [];
        try { tags = p.tagsJson ? JSON.parse(p.tagsJson) : []; } catch (e) { tags = []; }
        var rows = [
            ['Zalo user_id', p.zaloUserId],
            ['Tên chia sẻ', p.sharedName],
            ['SĐT', p.sharedPhone],
            ['Ngày sinh', p.sharedDob],
            ['Giới tính', p.gender],
            ['Địa chỉ', p.sharedAddress],
            ['Nhãn', tags.join(', ')],
            ['Tương tác đầu', fmtDateTime(p.firstInteractionAt)],
            ['Tương tác cuối', fmtDateTime(p.lastInteractionAt)],
            ['Đồng bộ hồ sơ', fmtDateTime(p.lastProfileSyncAt)]
        ];
        $('#zoaProfileInfo').html($.map(rows, function (r) {
            return r[1] ? '<li><span>' + esc(r[0]) + '</span><span>' + esc(r[1]) + '</span></li>' : null;
        }).join(''));
    }

    function syncProfile() {
        var c = state.active; if (!c) return;
        var $btn = $('#zoaSyncProfile').prop('disabled', true);
        api('POST', 'SyncCustomer', { Id: c.customerId })
            .done(function (res) { renderProfile(res.data); toast('Đã đồng bộ hồ sơ khách.', 'success'); refreshConversationHead(); })
            .fail(function (err) { apiError(err, 'Không đồng bộ được hồ sơ'); })
            .always(function () { $btn.prop('disabled', false); });
    }

    function toggleProfile() {
        var $p = $('#zoaProfile').toggleClass('visible');
        $('#zoaChatBody').toggleClass('profile-shown', $p.hasClass('visible') && window.innerWidth >= 1280);
    }

    // ── Chưa đọc + thông báo trình duyệt ───────────────────────────────────

    function pollSummary() {
        var params = state.lastInboundId ? { afterMessageId: state.lastInboundId } : {};
        return api('GET', 'GetUnreadSummary', params).done(function (res) {
            var s = res.data || {};
            state.totalUnread = s.totalUnread || 0;
            $('#zoaTotalUnread').text(state.totalUnread);
            $('#zoaUnreadConversations').text(s.unreadConversations || 0);
            updateTitle();

            if (state.lastInboundId === null) {
                state.lastInboundId = s.latestInboundMessageId || 0;   // lần đầu: không thông báo tồn đọng
                return;
            }
            var news = s.newMessages || [];
            if (news.length) {
                notify(news);
                state.lastInboundId = Math.max(state.lastInboundId, s.latestInboundMessageId || 0);
                refreshConversationHead();
            }
        });
    }

    var baseTitle = document.title;
    function updateTitle() {
        document.title = (state.totalUnread > 0 ? '(' + state.totalUnread + ') ' : '') + baseTitle;
    }

    function notificationsEnabled() {
        return 'Notification' in window && Notification.permission === 'granted' && storageGet(NOTIFY_KEY) !== '0';
    }

    function notify(news) {
        if (!notificationsEnabled()) return;
        $.each(news.slice(0, 3), function (_, m) {
            if (!document.hidden && m.conversationId === state.activeId) return;
            try {
                var body = m.messageType === 'TEXT' ? (m.content || '') : '[' + m.messageType + '] ' + (m.content || '');
                var n = new Notification('Zalo: ' + (m.displayName || 'Khách hàng'), {
                    body: body.length > 140 ? body.slice(0, 140) + '…' : body,
                    icon: safeUrl(m.avatarUrl) || undefined,
                    tag: 'zoa-conv-' + m.conversationId,
                    renotify: true
                });
                n.onclick = function () { window.focus(); openConversation(m.conversationId); n.close(); };
            } catch (e) { /* một số trình duyệt chỉ cho Notification qua service worker */ }
        });
    }

    function renderNotifyButton() {
        var $b = $('#zoaNotifyBtn');
        if (!('Notification' in window)) { $b.prop('disabled', true).find('span').text('Trình duyệt không hỗ trợ thông báo'); return; }
        if (Notification.permission === 'denied') { $b.prop('disabled', true).find('span').text('Thông báo bị chặn'); return; }
        $b.find('span').text(notificationsEnabled() ? 'Tắt thông báo' : 'Bật thông báo');
        $b.toggleClass('btn-primary', notificationsEnabled()).toggleClass('btn-outline-light', !notificationsEnabled());
    }

    function toggleNotify() {
        if (!('Notification' in window)) return;
        if (notificationsEnabled()) { storageSet(NOTIFY_KEY, '0'); renderNotifyButton(); return; }
        storageSet(NOTIFY_KEY, '1');
        if (Notification.permission === 'granted') { renderNotifyButton(); return; }
        Notification.requestPermission().then(function () { renderNotifyButton(); });
    }

    function importHistory() {
        if (!window.confirm('Nhập lịch sử chat có sẵn từ Zalo OA? Tác vụ chạy nền, tin đã có sẽ không bị trùng.')) return;
        api('POST', 'ImportHistory', {}).done(function (res) {
            toast((res.message || 'Đã đưa vào hàng đợi.') + (res.data && res.data.jobId ? ' Job #' + res.data.jobId : ''), 'success');
        }).fail(function (err) { apiError(err, 'Không nhập được lịch sử'); });
    }

    // ── Timer (giãn chu kỳ khi tab ẩn) ─────────────────────────────────────

    function startTimer(name, fn, ms) {
        stopTimer(name);
        var tick = function () {
            var p = fn();
            var next = function () { state.timers[name] = setTimeout(tick, document.hidden ? POLL_HIDDEN_MS : ms); };
            if (p && p.always) p.always(next); else next();
        };
        state.timers[name] = setTimeout(tick, ms);
    }

    function stopTimer(name) {
        if (state.timers[name]) { clearTimeout(state.timers[name]); delete state.timers[name]; }
    }

    // ── Sự kiện ────────────────────────────────────────────────────────────

    function bind() {
        $('#zoaConversationList').on('click', '.chat-link', function (e) {
            e.preventDefault();
            openConversation(parseInt($(this).closest('.chat-item').data('id'), 10));
        });

        var searchTimer;
        $('#zoaSearch').on('input', function () {
            clearTimeout(searchTimer);
            var v = $.trim($(this).val());
            searchTimer = setTimeout(function () { state.keyword = v; loadConversations(true); }, 350);
        });

        $('.zoa-filter').on('click', '.nav-link', function (e) {
            e.preventDefault();
            $('.zoa-filter .nav-link').removeClass('active');
            $(this).addClass('active');
            state.filter = $(this).data('filter') || '';
            loadConversations(true);
        });

        $('#zoaLoadMoreList').on('click', function () { loadConversations(false); });
        $('#zoaRefreshList').on('click', function (e) { e.preventDefault(); loadConversations(true); });
        $('#zoaRefreshChat').on('click', function (e) { e.preventDefault(); if (state.activeId) openConversation(state.activeId); });
        $('#zoaLoadOlder').on('click', loadOlder);
        $('#zoaPanel').on('scroll', function () { if (this.scrollTop < 40 && state.hasOlder) loadOlder(); });

        $('#zoaSend').on('click', sendText);
        $('#zoaInput').on('keydown', function (e) {
            if (e.key === 'Enter' && !e.shiftKey && !e.isComposing) { e.preventDefault(); sendText(); }
        }).on('input', function () {
            this.style.height = 'auto';
            this.style.height = Math.min(this.scrollHeight, 150) + 'px';
        });

        $('#zoaAttachBtn').on('click', function (e) { e.preventDefault(); $('#zoaFile').trigger('click'); });
        $('#zoaFile').on('change', function () { sendFile(this.files && this.files[0]); this.value = ''; });

        $('#zoaMessages').on('click', '.zoa-retry', function () { retryMessage(parseInt($(this).data('id'), 10)); });
        $('#zoaToggleStatus').on('click', function (e) { e.preventDefault(); toggleStatus(); });
        $('#zoaToggleProfile').on('click', function (e) { e.preventDefault(); toggleProfile(); });
        $('#zoaSyncProfile').on('click', syncProfile);
        $('#zoaBack').on('click', function (e) { e.preventDefault(); $('#zoaChatBody').removeClass('show-chat'); });
        $('#zoaNotifyBtn').on('click', toggleNotify);
        $('#zoaImportBtn').on('click', importHistory);

        document.addEventListener('visibilitychange', function () {
            if (!document.hidden) { pollSummary(); pollActive(); updateTitle(); }
        });
    }

    $(function () {
        bind();
        renderNotifyButton();
        loadConversations(true);
        pollSummary();
        startTimer('list', function () { return $.when(pollSummary(), refreshConversationHead()); }, POLL_LIST_MS);
    });

})(window.jQuery, window, document);
