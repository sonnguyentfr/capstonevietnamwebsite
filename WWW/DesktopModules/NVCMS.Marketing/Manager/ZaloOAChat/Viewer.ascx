<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Viewer.ascx.vb" Inherits="NVCMS.Modules.Marketing.ZaloOAChatViewer" %>
<link href="<%= AssetUrl("css/zalooachat.css") %>" rel="stylesheet" type="text/css" />

<div class="nk-content zoa-root">
    <div class="container-fluid">
        <div class="nk-content-inner">
            <div class="nk-content-body">
                <% If Not CanUseChat Then %>
                <div class="alert alert-warning mt-3">Bạn không có quyền sử dụng Zalo OA Chat. Liên hệ quản trị để được cấp quyền.</div>
                <% Else %>
                <div class="nk-block-head nk-block-head-sm">
                    <div class="nk-block-between">
                        <div class="nk-block-head-content">
                            <h3 class="nk-block-title page-title">Zalo OA Chat</h3>
                            <div class="nk-block-des text-soft">
                                <p>Chưa đọc: <b id="zoaTotalUnread">0</b> tin / <b id="zoaUnreadConversations">0</b> hội thoại</p>
                            </div>
                        </div>
                        <div class="nk-block-head-content">
                            <ul class="nk-block-tools g-2">
                                <li>
                                    <button type="button" id="zoaNotifyBtn" class="btn btn-outline-light btn-sm">
                                        <em class="icon ni ni-bell"></em><span>Bật thông báo</span>
                                    </button>
                                </li>
                                <% If IsChatAdmin Then %>
                                <li>
                                    <button type="button" id="zoaImportBtn" class="btn btn-outline-light btn-sm" title="Nhập lịch sử chat có sẵn từ Zalo OA">
                                        <em class="icon ni ni-download-cloud"></em><span>Nhập lịch sử</span>
                                    </button>
                                </li>
                                <% End If %>
                            </ul>
                        </div>
                    </div>
                </div>

                <div class="nk-chat zoa-chat">
                    <!-- ── Danh sách hội thoại ── -->
                    <div class="nk-chat-aside">
                        <div class="nk-chat-aside-head">
                            <div class="nk-chat-aside-user">
                                <div class="title">Hội thoại</div>
                            </div>
                            <ul class="nk-chat-aside-tools g-2">
                                <li>
                                    <a href="#" id="zoaRefreshList" class="btn btn-round btn-icon btn-light" title="Làm mới"><em class="icon ni ni-reload"></em></a>
                                </li>
                            </ul>
                        </div>
                        <div class="nk-chat-aside-search">
                            <div class="form-group">
                                <div class="form-control-wrap">
                                    <div class="form-icon form-icon-left"><em class="icon ni ni-search"></em></div>
                                    <input type="text" id="zoaSearch" class="form-control form-round" maxlength="100" placeholder="Tìm tên, SĐT, nội dung...">
                                </div>
                            </div>
                            <ul class="zoa-filter nav nav-tabs nav-tabs-s2">
                                <li class="nav-item"><a class="nav-link active" href="#" data-filter="">Tất cả</a></li>
                                <li class="nav-item"><a class="nav-link" href="#" data-filter="unread">Chưa đọc</a></li>
                                <li class="nav-item"><a class="nav-link" href="#" data-filter="OPEN">Đang mở</a></li>
                                <li class="nav-item"><a class="nav-link" href="#" data-filter="CLOSED">Đã đóng</a></li>
                            </ul>
                        </div>
                        <div class="nk-chat-aside-body" id="zoaListScroll">
                            <div class="nk-chat-list">
                                <ul class="chat-list" id="zoaConversationList"></ul>
                                <div class="text-center py-2">
                                    <button type="button" id="zoaLoadMoreList" class="btn btn-sm btn-dim btn-outline-light" style="display: none">Tải thêm</button>
                                </div>
                                <div id="zoaListEmpty" class="text-center text-soft py-4" style="display: none">Chưa có hội thoại nào.</div>
                            </div>
                        </div>
                    </div>

                    <!-- ── Khung chat ── -->
                    <div class="nk-chat-body" id="zoaChatBody">
                        <div class="nk-chat-blank" id="zoaBlank">
                            <div class="nk-chat-blank-icon"><em class="icon icon-circle icon-circle-xxl bg-white ni ni-chat"></em></div>
                            <div class="nk-chat-blank-btn text-soft">Chọn một hội thoại để bắt đầu</div>
                        </div>

                        <div class="nk-chat-head" id="zoaChatHead" style="display: none">
                            <ul class="nk-chat-head-info">
                                <li class="nk-chat-body-close">
                                    <a href="#" id="zoaBack" class="btn btn-icon btn-trigger ml-n1"><em class="icon ni ni-arrow-left"></em></a>
                                </li>
                                <li class="nk-chat-head-user">
                                    <div class="user-card">
                                        <div class="user-avatar" id="zoaHeadAvatar"></div>
                                        <div class="user-info">
                                            <div class="lead-text" id="zoaHeadName"></div>
                                            <div class="sub-text" id="zoaHeadSub"></div>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                            <ul class="nk-chat-head-tools">
                                <li><a href="#" id="zoaRefreshChat" class="btn btn-icon btn-trigger text-primary" title="Làm mới"><em class="icon ni ni-reload"></em></a></li>
                                <li><a href="#" id="zoaToggleStatus" class="btn btn-sm btn-outline-primary" title="Đóng / mở lại hội thoại"></a></li>
                                <li class="mr-n1"><a href="#" id="zoaToggleProfile" class="btn btn-icon btn-trigger text-primary" title="Thông tin khách"><em class="icon ni ni-alert-circle-fill"></em></a></li>
                            </ul>
                        </div>

                        <div class="nk-chat-panel" id="zoaPanel" style="display: none">
                            <div class="text-center py-2" id="zoaLoadOlderWrap" style="display: none">
                                <button type="button" id="zoaLoadOlder" class="btn btn-sm btn-dim btn-outline-light">Tải tin cũ hơn</button>
                            </div>
                            <div id="zoaMessages"></div>
                        </div>

                        <div class="zoa-warning alert alert-warning mb-0" id="zoaWindowWarning" style="display: none">
                            Khách chưa nhắn tin cho OA trong 7 ngày gần nhất - Zalo có thể từ chối tin tư vấn (lỗi -230).
                       
                        </div>

                        <div class="nk-chat-editor" id="zoaEditor" style="display: none">
                            <div class="nk-chat-editor-upload ml-n1">
                                <a href="#" id="zoaAttachBtn" class="btn btn-sm btn-icon btn-trigger text-primary" title="Gửi ảnh (JPG/PNG ≤1MB) hoặc tệp (PDF/DOC/DOCX/CSV ≤5MB)"><em class="icon ni ni-clip"></em></a>
                                <input type="file" id="zoaFile" accept=".jpg,.jpeg,.png,.pdf,.doc,.docx,.csv" style="display: none" />
                            </div>
                            <div class="nk-chat-editor-form">
                                <div class="form-control-wrap">
                                    <textarea class="form-control form-control-simple no-resize" rows="1" id="zoaInput" maxlength="2000" placeholder="Nhập tin nhắn... (Enter để gửi, Shift+Enter xuống dòng)"></textarea>
                                </div>
                            </div>
                            <ul class="nk-chat-editor-tools g-2">
                                <li>
                                    <button type="button" id="zoaSend" class="btn btn-round btn-primary btn-icon" title="Gửi"><em class="icon ni ni-send-alt"></em></button>
                                </li>
                            </ul>
                        </div>

                        <!-- ── Thông tin khách ── -->
                        <div class="nk-chat-profile" id="zoaProfile">
                            <div class="user-card user-card-s2 my-4">
                                <div class="user-avatar md" id="zoaProfileAvatar"></div>
                                <div class="user-info">
                                    <h5 id="zoaProfileName"></h5>
                                    <span class="sub-text" id="zoaProfileFollow"></span>
                                </div>
                            </div>
                            <div class="chat-profile">
                                <div class="chat-profile-group">
                                    <div class="chat-profile-head">
                                        <h6 class="title overline-title">Thông tin</h6>
                                    </div>
                                    <div class="chat-profile-body">
                                        <div class="chat-profile-body-inner">
                                            <ul class="zoa-profile-list" id="zoaProfileInfo"></ul>
                                            <button type="button" id="zoaSyncProfile" class="btn btn-sm btn-dim btn-outline-primary mt-2">
                                                <em class="icon ni ni-reload"></em><span>Đồng bộ từ Zalo</span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <script type="text/javascript">
                    window.ZaloOAChatConfig = {
                        moduleId: <%= ModuleId %>,
                        serviceRoot: "/DesktopModules/NVCMS/API/ZaloOAChat/",
                        userId: <%= UserId %>,
                        userName: <%= JsString(UserInfo.DisplayName) %>,
                        isAdmin: <%= IsChatAdmin.ToString().ToLowerInvariant() %>
                    };
                </script>
                <script type="text/javascript" src="<%= AssetUrl("js/zalooachat.js") %>"></script>
                <% End If %>
            </div>
        </div>
    </div>
</div>
