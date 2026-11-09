<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="menuadmin.ascx.vb" Inherits="NVCMS.Modules.HeThong.menuadmin" %>
<%-- Quản trị menu admin theo portal đang đăng nhập (Portals\{PortalId}\MenuAdmin\menu.xml). Dữ liệu lấy qua Web API Menu (Scripts/menu-admin.js). --%>
<link rel="stylesheet" href="<%= AssetRoot %>Styles/hethong.css?v=<%= AssetVersion %>" />

<div class="nvht" data-nvht-menu="1"
     data-module-id="<%= ModuleId %>"
     data-tab-id="<%= TabId %>"
     data-can-edit="<%= If(CanEdit, "1", "0") %>"
     data-api-root="<%= Attr(ApiRoot) %>">

    <header class="nvht-head">
        <div>
            <span class="nvht-eyebrow">NVCMS · Hệ thống</span>
            <h2 class="nvht-title">Menu quản trị</h2>
            <p class="nvht-sub" data-ref="sub">Đang tải…</p>
        </div>
        <div class="nvht-actions">
            <button type="button" class="nvht-btn nvht-btn-ghost" data-act="backups">Bản sao lưu</button>
            <button type="button" class="nvht-btn nvht-btn-ghost" data-act="reload">Tải lại</button>
            <button type="button" class="nvht-btn nvht-btn-primary" data-act="save" disabled>Lưu menu</button>
        </div>
    </header>

    <div class="nvht-alert" data-ref="alert" role="alert" hidden></div>

    <div class="nvht-grid" data-ref="grid" hidden>
        <section class="nvht-card">
            <div class="nvht-card-head">
                <h3>Cấu trúc menu</h3>
                <button type="button" class="nvht-btn nvht-btn-soft" data-act="add-root">+ Mục cấp 1</button>
            </div>
            <p class="nvht-hint">Cấp 1 là icon trên thanh trái, cấp 2 là các link trong khối menu bên cạnh. Chọn một mục để sửa; dùng nút ↑ ↓ để đổi thứ tự, → / ← để đổi cấp.</p>
            <div class="nvht-tree" data-ref="tree"></div>
        </section>

        <section class="nvht-card nvht-editor" data-ref="editor">
            <div class="nvht-empty" data-ref="editor-empty">Chọn một mục bên trái để sửa.</div>
            <div data-ref="editor-form" hidden>
                <div class="nvht-card-head">
                    <h3 data-ref="editor-title">Sửa mục</h3>
                    <span class="nvht-badge" data-ref="editor-level"></span>
                </div>
                <div class="nvht-warn" data-ref="editor-warn" hidden></div>

                <label class="nvht-field">
                    <span>Tên hiển thị <b>*</b></span>
                    <input type="text" data-field="text" maxlength="200" autocomplete="off" />
                </label>
                <label class="nvht-field" data-only="root">
                    <span>Tiêu đề khối / tooltip</span>
                    <input type="text" data-field="description" autocomplete="off" />
                    <small>Hiện làm tooltip của icon và tiêu đề khối menu con.</small>
                </label>
                <label class="nvht-field">
                    <span>Trang gắn với mục</span>
                    <select data-field="tabId"></select>
                    <small>Quyết định ai thấy mục (quyền xem trang) và trạng thái đang chọn. Chọn trang sẽ tự điền link.</small>
                </label>
                <label class="nvht-field">
                    <span>Link</span>
                    <input type="text" data-field="navUrl" autocomplete="off" placeholder="/quan-tri/..." />
                </label>
                <div class="nvht-row">
                    <label class="nvht-field">
                        <span>Mã (Value)</span>
                        <input type="text" data-field="value" autocomplete="off" />
                        <small data-only="root">Để trống sẽ tự sinh. Phải duy nhất giữa các mục cấp 1.</small>
                    </label>
                    <label class="nvht-field">
                        <span>Mở link</span>
                        <select data-field="target">
                            <option value="">Cùng tab</option>
                            <option value="_blank">Tab mới (_blank)</option>
                        </select>
                    </label>
                </div>
                <label class="nvht-field">
                    <span>Tham số (Params)</span>
                    <input type="text" data-field="params" autocomplete="off" />
                </label>
                <div class="nvht-extra" data-ref="extra" hidden></div>

                <div class="nvht-editor-foot">
                    <button type="button" class="nvht-btn nvht-btn-soft" data-act="add-child">+ Thêm mục con</button>
                    <button type="button" class="nvht-btn nvht-btn-danger" data-act="remove">Xóa mục</button>
                </div>
            </div>
        </section>
    </div>

    <div class="nvht-modal" data-ref="modal" hidden>
        <div class="nvht-modal-box" role="dialog" aria-modal="true" aria-label="Bản sao lưu">
            <div class="nvht-card-head">
                <h3>Bản sao lưu</h3>
                <button type="button" class="nvht-btn nvht-btn-ghost" data-act="close-modal">Đóng</button>
            </div>
            <p class="nvht-hint">Mỗi lần lưu, bản cũ được sao lưu (giữ 30 bản gần nhất). Chọn “Mở” để nạp vào trình sửa, kiểm tra rồi bấm “Lưu menu” để khôi phục.</p>
            <div data-ref="backups"></div>
        </div>
    </div>

    <p class="nvht-note" data-ref="readonly" hidden>Bạn cần quyền sửa (Edit) trên module này để quản trị menu.</p>
</div>

<script src="<%= AssetRoot %>Scripts/menu-admin.js?v=<%= AssetVersion %>"></script>
