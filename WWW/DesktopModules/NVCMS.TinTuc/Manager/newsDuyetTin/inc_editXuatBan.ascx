<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="inc_editXuatBan.ascx.vb" Inherits="DesktopModules.TinTuc.Manager.news.newsedit" EnableViewState="true" %>
<script src="/Providers/HtmlEditorProviders/DNNConnect.CKE/js/ckeditor/4.15.1/ckeditor.js"></script>
<link rel="stylesheet" href="/static/_admin/assets/css/nvcmsadmin.css">
<script type="text/javascript" src="/js/base64.js"></script>
<script src="/js/Common.js" type="text/javascript"></script>
<script type="text/javascript" src="/static/_admin/js/jquery.charactercounter.js"></script>
<link href="/static/_admin/js/jquery.tagsinput/bootstrap-tagsinput.css" rel="stylesheet" />
<script src="/static/_admin/js/jquery.tagsinput/bootstrap-tagsinput.js"></script>
<link rel="stylesheet" href="/Portals/_default/Skins/_admin/controls/newsedit.css" />
<link rel="stylesheet" href="/static/_admin/assets/css/libs/jstree.css?ver=2.7.0">
<script src="/static/_admin/assets/js/libs/jstree.js?ver=2.7.0"></script>
<style type="text/css">
    .newsnotes {
        min-height: auto !important;
    }
</style>
<script type="text/javascript">
    //Xu ly viec tuong tac anh
    function ActionImage() {
        var arrList = [];
        $('.anh-removeSelected').off();
        $('.anh-removeSelected').on('click', function () {
            $(this).closest("li").remove();
        });
        $('.anh-addToAvatar').on('click', function () {
            var imgDD = document.getElementById('<%= imgDD.ClientID%>');
            var imgpath = $(this).attr("data-img");
            imgDD.innerHTML = "<img src='" + imgpath + "' width='100%'/>";
            document.getElementById('<%=txtImagePath.ClientID %>').value = imgpath;
        });
        //Chen vao bai
        $('.anh-addToContent').on('click', function () {
            var title1 = ($('#<%=txtTitle.ClientID %>').val());
            var titlebai = title1.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
            var editor = CKEDITOR.instances.<%=teContent.ClientID%>;
            var iid = $(this).attr("data-id");
            var item = arrList.find(item => item.id === iid);
            var title = $(this).attr("data-title");
            var image = $(this).attr("data-img");

            var extension = getExt(image)
            if (extension == "pdf") {
                editor.insertHtml("<p style='text-align: center;'><iframe src='" + image + "' width='100%' height='700px' /></iframe></p><p><a href='" + image + "' target=_blank>" + title + "</a></p><p></p>");
            }
            if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "jfif" || extension == "bmp") {
                editor.insertHtml("<figure class='easyimage easyimage-full'><a data-fancybox='anhtrongbaiviet' data-caption='" + titlebai + "' title='" + titlebai + "' href='" + image + "'><img alt='" + titlebai + "' src='" + image + "'></a><figcaption>" + + "</figcaption></figure><p></p>");
            }
            if (extension == "mp4" || extension == "mpeg") {
                editor.insertHtml("<figure class='easyimage easyimage-full'><video style='width:700px' src='" + image + "' controls='controls'></video><figcaption>" + title1 + "</figcaption></figure><p></p>");
            }


        });
        //Chen vao bai
        $('.anh-addToContent2').on('click', function () {
            var title1 = ($('#<%=txtTitle.ClientID %>').val());
            var titlebai = title1.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
            var editor = CKEDITOR.instances.<%=teContent.ClientID%>;
            var iid = $(this).attr("data-id");
            var item = arrList.find(item => item.id === iid);
            var title = $(this).attr("data-title");
            var image = $(this).attr("data-img");

            var extension = getExt(image)
            if (extension == "pdf") {
                editor.insertHtml("<p style='text-align: center;'><iframe src='" + image + "' width='100%' height='700px' /></iframe></p><p><a href='" + image + "' target=_blank>" + title + "</a></p><p></p>");
            }
            if (extension == "png" || extension == "jpg" || extension == "jpeg" || extension == "gif" || extension == "jfif" || extension == "bmp") {
                editor.insertHtml("<a data-fancybox='anhtrongbaiviet' data-caption='" + titlebai + "' title='" + titlebai + "' href='" + image + "'><img alt='" + titlebai + "' src='" + image + "'></a>");
            }
            if (extension == "mp4" || extension == "mpeg") {
                editor.insertHtml("<figure class='easyimage easyimage-full'><video style='width:700px' src='" + image + "' controls='controls'></video><figcaption>" + title1 + "</figcaption></figure><p></p>");
            }


        });
        //Chen link vao text đã chọn
        $('.anh-addToContentLink').on('click', function () {
            var selection = CKEDITOR.instances['<%=teContent.ClientID%>'].getSelection();
            if (selection.getType() == CKEDITOR.SELECTION_ELEMENT) {
                var selectedContent = selection.getSelectedElement().$.outerHTML;
            }
            else if (selection.getType() == CKEDITOR.SELECTION_TEXT) {
                if (CKEDITOR.env.ie) {
                    selection.unlock(true);
                    selectedContent = selection.getNative().createRange().text;

                } else {
                    selectedContent = selection.getNative();
                    var editor = CKEDITOR.instances.<%=teContent.ClientID%>; //get a reference to the editor
                    var title = $(this).attr("data-title");
                    var titlebai = title1.replace(/[`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '');
                    var image = $(this).attr("data-img");
                    var selectedContentnew = "<a target=_blank title='" + titlebai + "' href='" + image + "'>" + selectedContent + "</a>";
                    editor.insertHtml(selectedContentnew)
                }
            }
        });
    }
</script>
<div class="nk-block-head nk-block-head-sm">
    <div class="nk-block-between">
        <div class="nk-block-head-content">
            <h3 class="nk-block-title page-title"><%=PortalSettings.ActiveTab.Description %></h3>
        </div>
    </div>
    <!-- .nk-block-between -->
</div>
<div class="nk-block">
    <div class="row g-gs">
        <div class="col-md-9 col-lg-9 col-xxl-9">
            <div class="card card-bordered">
                <div class="card-header border-bottom">
                    <ul class="cc_button">
                        <li>
                            <asp:LinkButton ID="lbtSave" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass=" btn btn-sm  btn-outline-primary" OnClientClick="formModified=false; updateFormAttachedMedia(); saveNews(); return false;">
                                <span>Lưu thay đổi</span><em class="icon ni ni-save-fill"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <a href="#" class="btn  btn-sm  btn-primary" data-toggle="modal" data-target="#modal-newsnote-submit">
                                <span>Trả lại tác giả</span><em class="icon ni ni-arrow-up-left"></em></a>
                        </li>
                        <li>
                            <a href="#" class="btn  btn-sm  btn-warning" data-toggle="modal" data-target="#modal-newsnote-submit2">
                                <span>Trả lại biên tập</span><em class="icon ni ni-arrow-up-left"></em>
                            </a>
                        </li>
                        <li>
                            <asp:HyperLink ID="lbtXemTruoc" Target="_blank" runat="server" Font-Bold="True" CssClass="btn btn-info btn-sm">
                            <span>Xem Trước</span> <em class="icon ni ni-eye-alt"></em>
                            </asp:HyperLink>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtSaveXB" Visible="false" ValidationGroup="InputValidate" runat="server" CssClass="btn  btn-sm  btn-danger" OnClientClick="formModified=false; updateFormAttachedMedia(); return checkvalidatexuatban(); saveNews(); return false;">
                                <span>Xuất bản ngay</span> <em class="icon ni ni-check-circle"></em>
                            </asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lbtCancelTop" runat="server" Font-Bold="True" CssClass="__neo_submit-BTN btn btn-sm  btn-outline-danger">
                                <em class="icon ni ni-arrow-left"></em><span>Thoát</span></asp:LinkButton></li>
                        <li style="float: right;">
                            <asp:LinkButton ID="lbtDeleteTop" runat="server" Font-Bold="True" CssClass="btn btn-sm btn-dark" OnClientClick="formModified=false; return confirm('Bạn có thực sự muốn xóa các tin đã chọn không?');">
                                <span>Xóa bài</span><em class="icon ni ni-trash"></em>
                            </asp:LinkButton></li>
                        <li style="float: right; padding-right: 10px; display: none"><a class="__neo_submit-BTN btn btn-sm  btn-outline-danger" onclick="OpenDALETDialog(); refreshFiles();">
                            <em class="icon ni ni-folders"></em><span>Chọn Multimedia
                            </span>
                        </a></li>

                    </ul>

                </div>
                <div class="card-inner">
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtTitle" Font-Names="Nunito" runat="server" CssClass="form-control form-control-xl form-control-outlined editor-f-22 editor-font" ValidationGroup="InputValidate"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtTitle.ClientID %>">Nhập tiêu đề</label>
                            <asp:RequiredFieldValidator ValidationGroup="InputValidate" ControlToValidate="txtTitle" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Nhập tiêu đề cho bài viết"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator
                                ID="valTitle" runat="server" ControlToValidate="txtTitle" ValidationGroup="InputValidate"
                                Display="Dynamic" CssClass="NormalRed" ErrorMessage="Tiêu đề phải chứa ít nhất 3 ký tự"
                                ForeColor="" ValidationExpression=".{3}.*"></asp:RegularExpressionValidator>
                            <div id="seotitle" class="chuanseo col-sm-12">
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtSummary" runat="server" CssClass="form-control form-control-outlined editor-f-18 editor-font" Height="60px" TextMode="MultiLine" ToolTip="Nhập Sapo tin bài" MaxLength="1000"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtSummary.ClientID %>">Tóm tắt bài viết</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="overline-title-alt mb-2">Tin liên quan</div>
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <a id="page-popup" onclick="popupwindow(900,600); return false;" class="btn btn-xs btn-info"><em class="icon ni ni-reports-alt"></em><span>Chọn tin liên quan</span></a>
                                <div id="__neo_article_relations-SCROLL" class="border border-primary p-2">
                                    <div class="list-lq" id="divrelated">
                                        <ul>
                                            <asp:Repeater runat="server" ID="rptRelated">
                                                <ItemTemplate>
                                                    <li>
                                                        <div style="width: auto; float: left" id='idtinlienquazzz<%# Eval("NewId")%>'>
                                                            <a href="#"><strong><%# HtmlUtils.StripPunctuation(CStr(Eval("Title")), True).Replace("'", "")%> </strong></a>
                                                            <a title="linkbai" style="display: none" class="<%# Ultis.FormatLink(BL.GetMappingTabIDByCategoryID(CType(DataBinder.Eval(Container.DataItem, "CategoryId"), Integer)), CType(DataBinder.Eval(Container.DataItem, "NewId"), Integer), CType(DataBinder.Eval(Container.DataItem, "Title"), String)) %>" href="#">&nbsp;</a>
                                                            <a title="sumary" style="display: none" class="<%# Eval("Summary")%>" href="#">&nbsp; </a>
                                                            <a title="imagepath" style="display: none" class="<%# Eval("ImagePath")%>" href="#">&nbsp;</a>
                                                            <a title="tieude" class="<%# HtmlUtils.StripPunctuation(CStr(Eval("Title")), True).Replace("'", "")%>" href="<%# Eval("newid")%>">&nbsp;</a>
                                                        </div>
                                                        <a class="delRelated" onclick="javascript:delRl(this,<%# Eval("NewId")%>);" data-toggle="tooltip" data-placement="top" title="Xóa tin" style="cursor: pointer;"><em class="icon ni ni-trash-fill"></em></a>
                                                        <a class="insertRelated" onclick="javascript:insertRelated('idtinlienquazzz<%# Eval("NewId")%>');" title="Chèn vào bài viết?" style="cursor: pointer;">[Dài] </a>
                                                        <a class="insertRelated" onclick="javascript:insertRelated4('idtinlienquazzz<%# Eval("NewId")%>');" title="Chèn vào bài viết?" style="cursor: pointer;">[Dài KHÔNG ẢNH] </a>
                                                        <a class="insertRelated" onclick="javascript:insertRelated3('idtinlienquazzz<%# Eval("NewId")%>');" title="Chèn vào bài viết?" style="cursor: pointer;">[Phải] </a>
                                                        <a class="insertRelated" onclick="javascript:insertRelated2('idtinlienquazzz<%# Eval("NewId")%>');" title="Chèn vào bài viết?" style="cursor: pointer;">[Trái] </a>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </div>
                                </div>

                                <!-- .nk-tb-list -->
                            </div>
                        </div>
                    </div>
                    <div class="card-title-group align-start pb-3 g-2">
                        <div class="card-title card-title-sm">
                            <h6 class="title">Nội dung bài viết</h6>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <textarea id="teContent" width="100%" runat="server" font-size="22px" height="1500px" validationgroup="InputValidate"></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtButDanh" runat="server" CssClass="form-control form-control-xl form-control-outlined"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtButDanh.ClientID %>">Bút Danh Tác giả</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <label class="form-label" for="<%=txtTags.ClientID %>">Tag bài viết</label>
                            <asp:TextBox ID="txtTags" runat="server" CssClass="form-control" MaxLength="1000" data-role="tagsinput"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:TextBox ID="txtSource" runat="server" CssClass="form-control form-control-xl form-control-outlined"></asp:TextBox>
                            <label class="form-label-outlined" for="<%=txtSource.ClientID %>">Link nguồn (nếu copy từ nơi khác)</label>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="overline-title-alt mb-2">Lưu ý / bút phê</div>
                        <asp:UpdatePanel ID="udpLuuy" runat="server">
                            <ContentTemplate>
                                <div class="form-control-wrap">
                                    <ul class="newsnote">
                                        <asp:Repeater ID="rptNotes" runat="server">
                                            <ItemTemplate>
                                                <li>
                                                    <b><%#BL.GetButDanh(PortalId, Eval("UserId")) %></b> <small><%#BL.FormatDate(Eval("CreatedDate")) %></small>: <%#Eval("NoiDung") %>
                                                </li>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </ul>
                                    <label class="form-label" for="default-textarea">Lời nhắn:</label>
                                    <div class="form-control-wrap">
                                        <asp:TextBox ID="txtNoteNews" runat="server" TextMode="MultiLine" CssClass="form-control no-resize newsnotes"></asp:TextBox>
                                        <asp:RequiredFieldValidator ValidationGroup="InputValidateNewsNote" ControlToValidate="txtNoteNews" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Nhập nội dung lời nhắn!"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNoteNews" ValidationGroup="InputValidateNewsNote"
                                            Display="Dynamic" CssClass="NormalRed" ErrorMessage="Nhập nhiều vào"
                                            ForeColor="" ValidationExpression=".{3}.*"></asp:RegularExpressionValidator>
                                    </div>
                                    <asp:LinkButton ID="lbtSendNewsNote" ValidationGroup="InputValidateNewsNote" runat="server" Font-Bold="True" CssClass="btn btn-dim btn-info btn-sm">
                                            <span>Lưu thay đổi</span><em class="icon ni ni-save-fill"></em>
                                    </asp:LinkButton>

                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <!-- .card -->
        </div>
        <!-- .col -->
        <div class="col-md-3 col-lg-3 col-xxl-3">
            <div class="card card-bordered h-100">
                <div class="card-inner">
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">Từ khóa chủ đạo</h6>
                            <div class="form-control-wrap">
                                <asp:TextBox ID="txtkeyword" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title" style="margin-bottom: 20px;">Thể loại chuyên mục</h6>
                            <div class="form-control-wrap">
                                <asp:DropDownList ID="ddlImage" runat="server" ValidationGroup="InputValidate" CssClass="form-select form-control">
                                    <asp:ListItem Value="-1" Text="Chọn thể loại"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="TT - Tin tức tổng hợp"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="TH - Bài Tổng hợp"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="TS - Tin sản xuất"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Bài - Bài sản xuất"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="PA - Bài Phản ánh"></asp:ListItem>
                                    <asp:ListItem Value="5" Text="PV - Bài Phỏng vấn"></asp:ListItem>
                                    <asp:ListItem Value="6" Text="PS - Phóng sự điều tra"></asp:ListItem>
                                    <asp:ListItem Value="9" Text="PR - Bài PR"></asp:ListItem>
                                    <asp:ListItem Value="12" Text="TDN - Tin dẫn nguồn"></asp:ListItem>
                                </asp:DropDownList>
                                <label class="form-label-outlined" for="<%=ddlImage.ClientID %>">Chọn thể loại</label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlImage" Display="Dynamic" CssClass="NormalRed"
                                    ErrorMessage="Chưa chọn thể loại" InitialValue="-1" ValidationGroup="InputValidate"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-control-wrap">
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select form-control" Width="100%" ValidationGroup="InputValidate" onchange="changeCategory();" DataTextField="CategoryName" DataValueField="CategoryID"></asp:DropDownList>
                            <label class="form-label-outlined" for="<%=ddlCategory.ClientID %>">Chọn Chuyên mục</label>
                            <asp:RequiredFieldValidator ID="valCategory" runat="server" ControlToValidate="ddlCategory" Display="Dynamic" CssClass="NormalRed"
                                ErrorMessage="Chưa chọn chuyên mục" InitialValue="0" ValidationGroup="InputValidate"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group" style="display:none">
                        <div class="card card-bordered">
                            <div class="card-inner">
                                <div class="overline-title-alt mb-2">Chuyên mục phụ</div>
                                <div id="checkbox-tree">
                                    <ul>
                                        <asp:Literal ID="ltrCourseUnit" runat="server"></asp:Literal>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">Cấu hình hiện thị</h6>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkAMP" runat="server">
                                        <label class="custom-control-label" for="<%=chkAMP.ClientID %>">AMP</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkBaiMoiNhat" runat="server">
                                        <label class="custom-control-label" for="<%=chkBaiMoiNhat.ClientID %>">Bài mới</label>
                                    </div>
                                </div>

                            </div>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkQuangCao" runat="server">
                                        <label class="custom-control-label" for="<%=chkQuangCao.ClientID %>">Quảng cáo</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkPR" runat="server">
                                        <label class="custom-control-label" for="<%=chkPR.ClientID %>">Bài PR</label>
                                    </div>
                                </div>

                            </div>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkVideo" runat="server">
                                        <label class="custom-control-label" for="<%=chkVideo.ClientID %>">Bài Video</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkPhoto" runat="server">
                                        <label class="custom-control-label" for="<%=chkPhoto.ClientID %>">Bài ảnh</label>
                                    </div>
                                </div>

                            </div>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkHotCat" runat="server">
                                        <label class="custom-control-label" for="<%=chkHotCat.ClientID %>">Nổi bật mục</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkHotSite" runat="server">
                                        <label class="custom-control-label" for="<%=chkHotSite.ClientID %>">Nổi bật trang</label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">Cấu hình tin bài</h6>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkconfighotslide" runat="server">
                                        <label class="custom-control-label" for="<%=chkconfighotslide.ClientID %>">Slide</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkconfigtinnong" runat="server">
                                        <label class="custom-control-label" for="<%=chkconfigtinnong.ClientID %>">Nóng</label>
                                    </div>
                                </div>

                            </div>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkconfigxuhuongdoc" runat="server">
                                        <label class="custom-control-label" for="<%=chkconfigxuhuongdoc.ClientID %>">Xu hướng đọc</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox" style="display: none">
                                        <input type="checkbox" class="custom-control-input" id="chkAnNoiDung" runat="server">
                                        <label class="custom-control-label" for="<%=chkAnNoiDung.ClientID %>">Ẩn nội dung</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox text-danger" id="Dechoxemnoidung" runat="server" visible="false">
                                        <input type="checkbox" class="custom-control-input" id="chkisAnLink" runat="server">
                                        <label class="custom-control-label text-danger" for="<%=chkisAnLink.ClientID %>">Không hiện bài viết</label>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">Thông báo</h6>
                            <div class="cauhinhtin g-2 align-center flex-wrap">
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkSendMail" runat="server">
                                        <label class="custom-control-label" for="<%=chkSendMail.ClientID %>">Gửi mail</label>
                                    </div>
                                </div>
                                <div class="g">
                                    <div class="custom-control custom-control-sm custom-checkbox">
                                        <input type="checkbox" class="custom-control-input" id="chkSendSMS" runat="server">
                                        <label class="custom-control-label" for="<%=chkSendSMS.ClientID %>">Tin nhắn</label>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title">NGÀY ĐĂNG</h6>
                            <asp:TextBox ID="txtCreateDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group" id="lblngayxuatban" runat="server">
                        <div class="card-inner2">
                            <h6 class="overline-title title text-danger">NGÀY GIỜ XUẤT BẢN</h6>
                            <asp:TextBox ID="txtPublishedDate" runat="server" CssClass="form-control datetimepicker"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="card-inner2">
                            <h6 class="overline-title title inline" style="display: inline">Nhuận bút</h6>
                            <a class="btn btn-danger btn-xs" data-toggle="collapse" href="#themtacgia" role="button" aria-expanded="false" aria-controls="collapseExample">Thêm tác giả</a>
                            <div class="form-control-wrap">
                                <div class="form-text-hint">
                                    <span class="overline-title">vnđ</span>
                                </div>
                                <asp:TextBox ID="txtCredit" runat="server" CssClass="form-control auto currency" Enabled="false"></asp:TextBox>
                            </div>
                            <div class="form-control-wrap">
                                <asp:UpdatePanel ID="upnhuan" runat="server">
                                    <ContentTemplate>
                                        <div class="x_content nhuanbut">
                                            <div class="form-horizontal form-label-left ">
                                                <table class="table">
                                                    <thead class="thead-light">
                                                        <tr>
                                                            <th scope="col" style="width: 50px">Kiểu</th>
                                                            <th scope="col">Tác giả</th>
                                                            <th scope="col" style="<%=hiennhuanbut() %>">Tiền</th>

                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater ID="rptTacGiaNhuanBut" runat="server">
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td><%#BL.FormatNhuanButLoaitin(Eval("Type")) %></td>
                                                                    <td><%#BL.GetButDanh(Eval("Portalid"), Eval("UserId")) %></td>
                                                                    <td style="width: 140px; <%=hiennhuanbut() %>">
                                                                        <asp:TextBox runat="server" ID="tiennhuanbut" CssClass="tiennhuanbutnha auto form-control currency" Text='<%#Eval("Credit") %>'></asp:TextBox></td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <asp:Button ID="cmdXoanhuan" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName="cmdXoanhuan" Text="[Xóa]" OnClick="cmdXoanhuan" CssClass="btn btn-warning btn-xs" runat="server" title="Xóa" />
                                                                        <asp:Button ID="cmdUpdateNhuan" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "id") %>' CommandName="cmdUpdateNhuan" Text="[Lưu]" OnClick="cmdUpdateNhuan" CssClass="btn btn-info btn-xs" runat="server" title="cập nhật" />
                                                                    </td>

                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="x_content nhuanbutthem collapse" id="themtacgia">
                                            <div class="row gy-4">
                                                <div class="col-sm-5">
                                                    <div class="form-group">
                                                        <div class="form-control-wrap">
                                                            <asp:DropDownList runat="server" ID="ddlnhuanbuttype" CssClass="form-select form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-7">
                                                    <div class="form-group">
                                                        <div class="form-control-wrap">
                                                            <asp:DropDownList runat="server" ID="ddlnhuanbutuser" CssClass="form-select form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-sm-12" style="<%=hiennhuanbut() %>">
                                                    <div class="form-group">
                                                        <asp:TextBox runat="server" ID="txtcredit1" CssClass="tiennhuanbutnha auto form-control auto currency" Text="0"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <div class="form-group">
                                                        <asp:LinkButton ID="lbtaddtacgia" runat="server" CssClass="__neo_submit-BTN btn btn-sm  btn-outline-danger" OnClientClick="return checkvalidatenhuan();">
                                                            <em class="icon ni ni-save-fill"></em><span>Thêm</span>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                                <div class="col-sm-6">
                                                    <asp:Literal ID="ltrtacgiathong" runat="server"></asp:Literal>
                                                </div>
                                            </div>
                                        </div>
                                        <script type="text/javascript">
                                            var prm = Sys.WebForms.PageRequestManager.getInstance();
                                            if (prm != null) {
                                                prm.add_endRequest(function (sender, e) {
                                                    if (sender._postBackSettings.panelsToUpdate != null) {
                                                        Tongtiennhuanbut();
                                                    }
                                                });
                                            };

                                        </script>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress runat="server" ID="PageUpdateProgress">
                                    <ProgressTemplate>
                                        <div class="loading">
                                            <div></div>
                                            <div></div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="form-group">
                            <div class="form-control-wrap">
                                <a href="#" class="btn btn-xs btn-info"><em class="icon ni ni-reports-alt"></em><span>Ảnh đại diện</span></a>
                                <div class="border border-primary p-2">
                                    <asp:HiddenField ID="txtImagePath" runat="server" />
                                    <div id="imgDD" runat="server">
                                        <img src="/images/no_avatar.gif" alt="Ảnh đại diện" />
                                    </div>
                                </div>

                                <!-- .nk-tb-list -->
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="overline-title-alt mb-2">Tải ảnh / Media</div>
                        <div class="form-group">
                            <div class="form-control-wrap uploadbtn">
                                <asp:FileUpload ID="file_upload" class="btn btn-xs btn-info multi" AllowMultiple="true" runat="server" />
                                <progress id="fileProgress" style="display: none"></progress>
                                <div class="border border-primary">
                                    <asp:UpdatePanel ID="upimage" runat="server">
                                        <ContentTemplate>
                                            <div class="form-horizontal form-label-left">
                                                <div class="form-group">
                                                    <small>Click vào ảnh / Video để xem kích thước lớn</small>
                                                    <div class="col-md-12 scrollbar" id="anhupload">
                                                        <ul class="anh-upload">
                                                            <asp:Repeater ID="rptphotoatt" runat="server">
                                                                <ItemTemplate>
                                                                    <li class="anh-daupload">
                                                                        <div class="anh-khunganh">
                                                                            <a data-fancybox data-caption="" href='<%#Eval("ImageFull") %>'>
                                                                                <img src="<%#Ultis.GetBackround(Eval("ImageExtension"), Eval("ImageFull")) %>" style="max-width: 100%;" /></a>
                                                                            <input <%#Ultis.Enableanh(Eval("ImageExtension")) %> type="checkbox" data-img="<%# Eval("ImageFull")%>" class="anh-addToAvatar" />
                                                                        </div>
                                                                        <div class="anh-thongtin">
                                                                            <asp:Button ID="btnxoaanh" Visible='<%#ChoXoaAnh(Eval("mediaid")) %>' CommandArgument='<%#Eval("mediaid") %>' CommandName="btnxoaanh" Text="Xóa" OnClick="btnxoaanh" CssClass="anh-addToContent btn" runat="server" />
                                                                            <a class="anh-addToContent btn" data-title="<%# Eval("ImageFull")%>" data-img="<%# Eval("ImageFull")%>"><em class="icon ni ni-download"></em></a>
                                                                            <a class="anh-addToContent2 btn" data-title="<%# Eval("ImageFull")%>" data-img="<%# Eval("ImageFull")%>"><em class="icon ni ni-camera"></em></a>
                                                                            <a class="anh-addToContentLink btn" data-title="<%# Eval("ImageFull")%>" data-img="<%# Eval("ImageFull")%>">
                                                                                <em class="icon ni ni-link"></em></a>
                                                                        </div>
                                                                        <div style="clear: both"></div>
                                                                    </li>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                            <asp:Label ID="lblMessage" runat="server" />
                                                        </ul>
                                                    </div>
                                                    <script type="text/javascript">
                                                        $(document).ready(function () {
                                                            ActionImage();
                                                        });

                                                    </script>
                                                    <div class="col-sm-12">
                                                        <asp:HiddenField ID="hdf_orgtong" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdf_photoattach" runat="server" Value="0" />
                                                        <asp:HiddenField ID="hdf_itemid" runat="server" Value="0" />
                                                    </div>

                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdateProgress runat="server" ID="UpdateProgress1">
                                        <ProgressTemplate>
                                            <div id="loading">
                                                <div class="loading">
                                                    <div></div>
                                                    <div></div>
                                                </div>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>

                                <!-- .nk-tb-list -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- .card -->
        </div>

        <!-- .col -->
    </div>
    <div class="border-top nvcms-sticky-btn p-2">
        <asp:LinkButton ID="lbtSave2" ValidationGroup="InputValidate" runat="server" Font-Bold="True" CssClass="__neo_submit-BTN btn btn-sm  btn-outline-primary" OnClientClick="formModified=false; updateFormAttachedMedia(); saveNews(); return false;">
                <span>Lưu thay đổi</span><em class="icon ni ni-save-fill"></em>
        </asp:LinkButton>
        <asp:LinkButton ID="lbtSaveGuiBientao2" Visible="false" ValidationGroup="InputValidate" runat="server" CssClass="btn  btn-sm  btn-primary" OnClientClick="formModified=false; updateFormAttachedMedia(); saveNews(); return false;">
                <span>Gửi Biên tập</span><em class="icon ni ni-send-alt"></em>
        </asp:LinkButton>

    </div>
</div>
<%--Modal Lưu ý bút phê Trả lại tác giả--%>
<div class="modal fade" tabindex="-1" id="modal-newsnote-submit">
    <div class="modal-dialog modal-xl modal-dialog-top" role="document">
        <div class="modal-content">
            <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                <em class="icon ni ni-cross"></em>
            </a>
            <div class="modal-header">
                <h5 class="modal-title">Bút phê Trả lại bài viết Tác giả</h5>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <div class="form-control-wrap">
                        <asp:TextBox ID="txtLuubutphephop" runat="server" TextMode="MultiLine" CssClass="form-control no-resize newsnotes"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="InputValidateNewsNotepop" ControlToValidate="txtLuubutphephop" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator4" runat="server" ErrorMessage="Nhập nội dung lời nhắn!"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator
                            ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtLuubutphephop" ValidationGroup="InputValidateNewsNotepop"
                            Display="Dynamic" CssClass="NormalRed" ErrorMessage="Nhập nhiều vào"
                            ForeColor="" ValidationExpression=".{3}.*"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="lbtReturnTopWithMess" ValidationGroup="InputValidateNewsNotepop" runat="server" Font-Bold="True" CssClass="btn btn-warning">
                            <span>Trả lời kèm bút phê</span><em class="icon ni ni-save-fill"></em>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lbtReturnTop" runat="server" Font-Bold="True" CssClass="btn btn-secondary">
                            <span>Trả lại</span><em class="icon ni ni-save-fill"></em>
                    </asp:LinkButton>
                </div>
                <div class="form-group">
                    <ul class="newsnote">
                        <asp:Repeater ID="rptNotes2" runat="server">
                            <ItemTemplate>
                                <li>
                                    <b><%#BL.GetButDanh(PortalId, Eval("UserId")) %></b> <small><%#BL.FormatDate(Eval("CreatedDate")) %></small>: <%#Eval("NoiDung") %>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
<%--Modal Lưu ý bút phê Trả lại Biên tập--%>
<div class="modal fade" tabindex="-1" id="modal-newsnote-submit2">
    <div class="modal-dialog modal-xl modal-dialog-top" role="document">
        <div class="modal-content">
            <a href="#" class="close" data-dismiss="modal" aria-label="Close">
                <em class="icon ni ni-cross"></em>
            </a>
            <div class="modal-header">
                <h5 class="modal-title">Bút phê Trả lại bài viết Biên tập</h5>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <div class="form-control-wrap">
                        <asp:TextBox ID="txtLuubutphephop2" runat="server" TextMode="MultiLine" CssClass="form-control no-resize newsnotes"></asp:TextBox>
                        <asp:RequiredFieldValidator ValidationGroup="InputValidateNewsNotepop2" ControlToValidate="txtLuubutphephop2" CssClass="NormalRed" Display="Dynamic" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Nhập nội dung lời nhắn!"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator
                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtLuubutphephop2" ValidationGroup="InputValidateNewsNotepop2"
                            Display="Dynamic" CssClass="NormalRed" ErrorMessage="Nhập nhiều vào"
                            ForeColor="" ValidationExpression=".{3}.*"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:LinkButton ID="lbtReturnBienTapTopWithMess" ValidationGroup="InputValidateNewsNotepop2" runat="server" Font-Bold="True" CssClass="btn btn-warning">
                            <span>Trả lời kèm bút phê</span><em class="icon ni ni-save-fill"></em>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lbtReturnBienTapTop" runat="server" Font-Bold="True" CssClass="btn btn-secondary">
                            <span>Trả lại</span><em class="icon ni ni-save-fill"></em>
                    </asp:LinkButton>
                </div>
                <div class="form-group">
                    <ul class="newsnote">
                        <asp:Repeater ID="rptNotes3" runat="server">
                            <ItemTemplate>
                                <li>
                                    <b><%#BL.GetButDanh(PortalId, Eval("UserId")) %></b> <small><%#BL.FormatDate(Eval("CreatedDate")) %></small>: <%#Eval("NoiDung") %>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="dialog-dalet" title="Chèn MULTIMEDIA vào bài" style="display: none;">
    <div id="filter-form" style="top: 4px;">
        Nguồn:
        <asp:DropDownList runat="server" ID="drlSource" onchange="formModified=false;refreshFiles();">
            <asp:ListItem Value="1" Text="Từ nguồn Nhạc/Video"></asp:ListItem>
            <asp:ListItem Value="3" Text="Tin UPLOAD"></asp:ListItem>
            <asp:ListItem Value="2" Text="Từ FTP"></asp:ListItem>
        </asp:DropDownList>
        Thể loại: 
        <select id="sltType" onchange="formModified=false;refreshFiles();">
            <option value="2">Video</option>
            <option value="1">Nhạc</option>
            <option value="3">Hình ảnh</option>
        </select>
        <a class="StandardButton" style="margin-top: 3px;" onclick="refreshFiles();">
            <img src="/images/icons/arrow_refresh.png" alt="" /></a>
        <a class="StandardButton" onclick="getPrevFiles();">
            <img src="/images/icons/resultset_previous.png" alt="" />Trước</a>
        Trang
        <label id="lcurPage">1</label>/<label id="ltotalPage">2</label>
        <a class="StandardButton" onclick="getNextFiles();">
            <img src="/images/icons/resultset_next.png" alt="" />Tiếp</a>

    </div>
    <table id="myTable" class="tablesorter tablesorter-blue">
        <thead>
            <tr class="tablesorter-headerRow">
                <th class="tablesorter-header">Tệp tin
                    <input name="filter" id="filter" value="" maxlength="30" size="45" type="text" /></th>
                <th data-sorter="shortDate" data-date-format="ddmmyyyy" class="tablesorter-header">Thời gian</th>
                <th class="tablesorter-header">Size</th>
            </tr>
        </thead>
        <tbody>
            <tr class="TRgrid">
                <td></td>
                <td width="110px"></td>
                <td width="60px"></td>
            </tr>
        </tbody>
    </table>
    <img id="LoadingImage" src="/images/loading.gif" alt="" style="position: absolute; top: 100px; left: 310px;" />
</div>
<asp:HiddenField ID="hdf_WF" runat="server" />
<asp:HiddenField ID="hdf_WF_Text" runat="server" />
<asp:HiddenField ID="hdf_list_files" runat="server" />
<asp:HiddenField ID="hdf_subCategories" runat="server" />
<asp:HiddenField ID="hdf_nodung" runat="server" />
<asp:HiddenField ID="hdf_nguontin" runat="server" />
<asp:HiddenField ID="hdf_IMG_files" runat="server" />
<asp:HiddenField ID="hdf_dongtg" runat="server" />
<asp:HiddenField ID="hdf_Category" runat="server" />
<asp:HiddenField ID="hdf_theloai" runat="server" Value="1" />
<asp:HiddenField ID="hdf_Related" runat="server" />
<asp:HiddenField ID="hdf_Tags" runat="server" />
<asp:HiddenField ID="hdf_nhuanbut" runat="server" />
<div id="divclipboardswf"></div>
<script type="text/javascript">
    $(document).ready(function () {
        //Upload anh

        $("#<%=file_upload.ClientID%>").on("change", function () {
            var data = new FormData();
            var fileInput = document.getElementById('<%=file_upload.ClientID%>');
            var itemid = document.getElementById('<%=hdf_itemid.ClientID%>').value;
            for (i = 0; i < fileInput.files.length; i++) {
                var sfilename = fileInput.files[i].name;
                data.append(sfilename, fileInput.files[i]), itemid;
                $.ajax({
                    url: '/DesktopModules/NVCMS.TinTuc/Manager/Services/UploadImage.ashx?itemid=' + itemid,
                    type: 'POST',
                    data: data,
                    cache: false,
                    contentType: false,
                    processData: false,
                    async: false,
                    success: OnSuccess,
                    xhr: function () {
                        var fileXhr = $.ajaxSettings.xhr();
                        if (fileXhr.upload) {
                            $("progress").show();
                            fileXhr.upload.addEventListener("progress", function (e) {
                                if (e.lengthComputable) {
                                    $("#fileProgress").attr({
                                        value: e.loaded,
                                        max: e.total
                                    });
                                }
                            }, false);
                        }
                        return fileXhr;
                    }
                });

            }
            //chkatchtbl();  
            $('#<%=file_upload.ClientID%>').val('');

        });

        function OnSuccess(response) {
            $("#fileProgress").hide();
            $("#<%=lblMessage.ClientID%>").append(response);

            $('.anh-addToContent').off();
            ActionImage();
        }
        $("#<%=txtTitle.ClientID%>").characterCounter({
            limit: 70,
            counterFormat: '%1/70'
        });
        $("#<%=txtSummary.ClientID%>").characterCounter({
            limit: 300,
            counterFormat: '%1/300'
        });
    });
    //=======================================
    function setAnhDD(idDiv) {
        var imgpath = $('#' + idDiv + ' a[title="Play"]').attr("class") + "/" + $('#' + idDiv + ' a[title="Play"]').attr("href");
        var imgDD = document.getElementById('<%= imgDD.ClientID%>');
        imgDD.innerHTML = "<img src='" + imgpath + "' width='120px'/>";
        document.getElementById('<%=txtImagePath.ClientID %>').value = imgpath;
    }
    function insertMedia(headtext, idDiv) {
        var editor = $find("<%=teContent.ClientID%>"); //get a reference to the editor
        var wrapped = $("#" + idDiv);
        wrapped.find('a[title="ShowCodeBlock"]').children().remove().end().remove();
        editor.pasteHtml("<br /><div style='text-align: center;'>" + headtext.toString() + wrapped.html() + "</div><br/>");
    }
    function insertImages(idDiv) {
        var editor = $find("<%=teContent.ClientID%>"); //get a reference to the editor
        var folder = $('#' + idDiv + ' a[title="Play"]').attr("class");
        var filename = $('#' + idDiv + ' a[title="Play"]').attr("href");
        editor.pasteHtml("<br /><div style='text-align: center;'><img alt='' src='" + folder + '/' + filename + "' width='500px' /></div><div style='padding-top: 2px; text-align: center;'>" + filename + "</div><br />");
    }

</script>
<script type="text/javascript">
    function showSub() {
        document.getElementById('tdCMPhu').style.display = (document.getElementById('tdCMPhu').style.display == 'none') ? 'block' : 'none';
    }
    function showSummary() {
        document.getElementById('tdSummary').style.display = (document.getElementById('tdSummary').style.display == 'none') ? 'block' : 'none';
    }
    function fnSelect(objId) {
        fnDeSelect();
        //getSelectionHtml();
        if (document.selection) {
            var range = document.body.createTextRange();
            range.moveToElementText(document.getElementById(objId));
            range.selectAllChildren();
        }
        else if (window.getSelection) {
            var range = document.createRange();
            range.selectNode(document.getElementById(objId));
            window.getSelection().addRange(range);
        }
    }

    function fnDeSelect() {
        if (document.selection) document.selection.empty();
        else if (window.getSelection)
            window.getSelection().removeAllRanges();
    }

    function getSelectionHtml() {
        var html = "";
        if (typeof window.getSelection != "undefined") {
            var sel = window.getSelection();
            if (sel.rangeCount) {
                var container = document.createElement("div");
                for (var i = 0, len = sel.rangeCount; i < len; ++i) {
                    container.appendChild(sel.getRangeAt(i).cloneContents());
                }
                html = container.innerHTML;
            }
        } else if (typeof document.selection != "undefined") {
            if (document.selection.type == "Text") {
                html = document.selection.createRange().htmlText;
            }
        }
        return html;
    }

    /*Get Parameter value*/
    function gup(name) {
        name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
        var regexS = "[\\?&]" + name + "=([^&#]*)";
        var regex = new RegExp(regexS);
        var results = regex.exec(window.location.href);
        if (results == null)
            return "";
        else
            return results[1];
    }
    function copyToClipboard(s) {
        var text = $('#' + s).html();

        //clip.setText(text);

        if (window.clipboardData) {
            window.clipboardData.setData('text', text);
        }
        else {
            var clipboarddiv = document.getElementById('divclipboardswf');
            if (clipboarddiv == null) {
                clipboarddiv = document.createElement('div');
                clipboarddiv.setAttribute("name", "divclipboardswf");
                clipboarddiv.setAttribute("id", "divclipboardswf");
                document.body.appendChild(clipboarddiv);
            }
            clipboarddiv.innerHTML = '<object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" width="100%" height="100%"><param name="movie" value="/Portals/0/clipboard.swf" /><param name="flashvars" value="clipboard=' + encodeURIComponent(text) + '" /><param name="allowScriptAccess" value="always" /><embed src="/Portals/0/clipboard.swf" FlashVars="clipboard=' + encodeURIComponent(text) + '" width="0" height="0" allowScriptAccess="always" type="application/x-shockwave-flash"></embed></object>';
        }
        $('#' + s).select();
        $('#imgShowCopied').fadeTo("slow", 1.0).fadeTo("slow", 0);
        return false;
    }
    function showhideUpload() {
        if ($('.pnlUploadCss').css('visibility') == 'hidden') {
            $('.pnlUploadCss').css('visibility', 'visible');
        } else {
            $('.pnlUploadCss').css('visibility', 'hidden');
        }
    }
</script>
<script type="text/javascript">
    jQuery(function ($) {
        $('#checkbox-tree').jstree({
            "core": {
                "themes": {
                    "responsive": true
                }
            },
            "plugins": ["wholerow", "checkbox"]
        }).on('click', function () {
            document.getElementById('<%=hdf_subCategories.ClientID%>').value = $('#checkbox-tree').jstree("get_selected").join(",");
        });
    })
</script>
<asp:Literal ID="ltrscriptSubcat" runat="server"></asp:Literal>
<script language="javascript" type="text/javascript">
    var udpID;
    var sID = gup('itemid');
    var data = Base64.encode(document.getElementById('<%=hdf_nodung.ClientID %>').value);
    var title = '';
    var category = document.getElementById('<%=hdf_Category.ClientID %>').value;
    var subcategory = document.getElementById('<%=hdf_subCategories.ClientID %>').value;
    var summary = '';
    var img = '';
    var bHotCat = 'false';
    var bHotSite = 'false';
    var bisVideo = 'false';
    var bisPhoto = 'false';
    var bisPR = 'false';
    var bisShowBaiMoi = 'false';
    var bisAMP = 'false';
    var bisHienQuangCao = 'false';
    var bisAnNoiDung = 'false';
    var bisAnLink = 'false';
    var he = '';
    var nguontin = document.getElementById('<%=hdf_nguontin.ClientID %>').value;
    var dongtg = -1;
    var dongtgtext = "";
    var arrdongtg = document.getElementById('<%=hdf_dongtg.ClientID %>').value;
    var luuy = '';
    var links = document.getElementById('<%=hdf_Related.ClientID %>').value;
    var anhdd = document.getElementById('<%=txtImagePath.ClientID %>').value;
    var imgDD = '';
    var mediaList = '';
    var butdanh = '';
    var SourceText = '';
    var keyword = '';
    var imgList = '';
    var txtTags = "";
    var chkconfighotslide = 'false';
    var chkconfigtinnong = 'false';
    var chkconfigxuhuongdoc = 'false';

    var elEditor = null;

    var editor = CKEDITOR.replace('<%=teContent.ClientID %>');
    elEditor = editor.getData();
    editor.on('change', function (evt) {
        elEditor = editor.getData();
        formModified = true;
    })
    function ClientNodeChecked(sender, eventArgs) {
        formModified = true;
        subcategory = "";
        subcategory = $('#checkbox-tree').jstree("get_selected").join(",");
    }

    function FetchData() {
        title = Base64.encode($('#<%=txtTitle.ClientID %>').val());
        category = $('#<%=hdf_Category.ClientID %>').val();
        summary = Base64.encode($('#<%=txtSummary.ClientID %>').val());
        txtTags = Base64.encode($('#<%=txtTags.ClientID %>').val());
        img = $('#<%=ddlImage.ClientID %> :selected').val();
        bHotCat = (document.getElementById('<%=chkHotCat.ClientID %>')) ? document.getElementById('<%=chkHotCat.ClientID %>').checked : false;
        bHotSite = (document.getElementById('<%=chkHotSite.ClientID %>')) ? document.getElementById('<%=chkHotSite.ClientID %>').checked : false;

        bisVideo = (document.getElementById('<%=chkVideo.ClientID %>')) ? document.getElementById('<%=chkVideo.ClientID %>').checked : false;
        bisPhoto = (document.getElementById('<%=chkPhoto.ClientID %>')) ? document.getElementById('<%=chkPhoto.ClientID %>').checked : false;
        bisPR = (document.getElementById('<%=chkPR.ClientID %>')) ? document.getElementById('<%=chkPR.ClientID %>').checked : false;
        bisShowBaiMoi = (document.getElementById('<%=chkBaiMoiNhat.ClientID %>')) ? document.getElementById('<%=chkBaiMoiNhat.ClientID %>').checked : false;
        bisAMP = (document.getElementById('<%=chkAMP.ClientID %>')) ? document.getElementById('<%=chkAMP.ClientID %>').checked : false;
        bisHienQuangCao = (document.getElementById('<%=chkQuangCao.ClientID %>')) ? document.getElementById('<%=chkQuangCao.ClientID %>').checked : false;
        bisAnNoiDung = (document.getElementById('<%=chkAnNoiDung.ClientID %>')) ? document.getElementById('<%=chkAnNoiDung.ClientID %>').checked : false;
        bisAnLink = (document.getElementById('<%=chkisAnLink.ClientID %>')) ? document.getElementById('<%=chkisAnLink.ClientID %>').checked : false;
        butdanh = Base64.encode($('#<%=txtButDanh.ClientID %>').val());
        SourceText = Base64.encode($('#<%=txtSource.ClientID %>').val());
        keyword = Base64.encode($('#<%=txtkeyword.ClientID %>').val());
        arrdongtg = document.getElementById('<%=hdf_dongtg.ClientID %>').value;
        luuy = "";
        anhdd = Base64.encode($('#<%=txtImagePath.ClientID %>').val());
        mediaList = Base64.encode($('#<%=hdf_list_files.ClientID %>').val());
        imgList = Base64.encode($('#<%=hdf_IMG_files.ClientID %>').val());
        links = Base64.encode($('#<%=hdf_Related.ClientID %>').val());

        chkconfighotslide = (document.getElementById('<%=chkconfighotslide.ClientID %>')) ? document.getElementById('<%=chkconfighotslide.ClientID %>').checked : false;
        chkconfigtinnong = (document.getElementById('<%=chkHotCat.ClientID %>')) ? document.getElementById('<%=chkconfigtinnong.ClientID %>').checked : false;
        chkconfigxuhuongdoc = (document.getElementById('<%=chkHotCat.ClientID %>')) ? document.getElementById('<%=chkconfigxuhuongdoc.ClientID %>').checked : false;
    }

    function updateSuccess(result, ctx) {
        sID = result;
        if (sID != '0' || sID != 0) {
            notifySuccess('Tự động lưu thành công!');
            //Already saved
            formModified = false;
        }
        else {
            notifyError('Lưu thất bại');
            //formModified = false;
        }
        //stopAutoSave();
    }
    function updateError(result, ctx) {
        notifyError('Lưu thất bại');
    }

    var rqAutoSaveInterval = <%= requestAutoSaveInterval %>;
    function callbackSaveData() {
        if (formModified) {
            if (Page_ClientValidate("InputValidate")) {
                FetchData();
                data = Base64.encode(elEditor);
                //console.log(elEditor);
                AutoSave(sID + "~!@|" + title + "~!@|" + img + "~!@|" + bHotCat + "~!@|" + bHotSite + "~!@|" + category + "~!@|" + subcategory + "~!@|" + summary + "~!@|" + he + "~!@|" + data + "~!@|" + nguontin + "~!@|" + arrdongtg + "~!@|" + luuy + "~!@|" + links + "~!@|" + anhdd + "~!@|" + mediaList + "~!@|" + imgList + "~!@|" + bisVideo + "~!@|" + bisPhoto + "~!@|" + bisPR + "~!@|" + bisShowBaiMoi + "~!@|" + bisAMP + "~!@|" + bisHienQuangCao + "~!@|" + bisAnNoiDung + "~!@|" + bisAnLink + "~!@|" + keyword + "~!@|" + butdanh + "~!@|" + SourceText + "~!@|" + chkconfighotslide + "~!@|" + chkconfigtinnong + "~!@|" + chkconfigxuhuongdoc + "~!@|" + txtTags);
            }
        }
    }

    function startAutoSave() {
        udpID = setInterval(callbackSaveData, rqAutoSaveInterval);
    }

    function stopAutoSave() {
        clearInterval(udpID);
    }
    startAutoSave();
    function saveNews() {
        if (Page_ClientValidate("InputValidate")) {
            FetchData();
            data = Base64.encode(elEditor);
            AutoSave(sID + "~!@|" + title + "~!@|" + img + "~!@|" + bHotCat + "~!@|" + bHotSite + "~!@|" + category + "~!@|" + subcategory + "~!@|" + summary + "~!@|" + he + "~!@|" + data + "~!@|" + nguontin + "~!@|" + arrdongtg + "~!@|" + luuy + "~!@|" + links + "~!@|" + anhdd + "~!@|" + mediaList + "~!@|" + imgList + "~!@|" + bisVideo + "~!@|" + bisPhoto + "~!@|" + bisPR + "~!@|" + bisShowBaiMoi + "~!@|" + bisAMP + "~!@|" + bisHienQuangCao + "~!@|" + bisAnNoiDung + "~!@|" + bisAnLink + "~!@|" + keyword + "~!@|" + butdanh + "~!@|" + SourceText + "~!@|" + chkconfighotslide + "~!@|" + chkconfigtinnong + "~!@|" + chkconfigxuhuongdoc + "~!@|" + txtTags);
        }
    }
    //Update all included media
    function updateFormAttachedMedia() {
        var divContent = document.createElement('DIV');
        divContent.innerHTML = elEditor;
        var foundELs = $(divContent).find('a[title="Play"]');
        $.each(foundELs, function () {
            var aEl = $(this);
            folder = $(this).attr("class");
            filename = getDecodeString($(this).attr("href").match(/[^\/\\]+$/));
            addValue('<%= hdf_list_files.ClientID %>', folder + "|" + filename);
        });

        var foundIMGELs = $(divContent).find('a[title="IMAGES"]');
        $.each(foundIMGELs, function () {
            var aIMGEl = $(this);
            folderIMG = $(this).attr("class");
            filenameIMG = getDecodeString($(this).attr("href").match(/[^\/\\]+$/));
            addValue('<%= hdf_IMG_files.ClientID %>', folderIMG + "|" + filenameIMG);
        });
    }
</script>
<script type="text/javascript">
    //không cho bấm F5
    $(document).ready(function () {
        $(window).keydown(function (event) {
            if (event.keyCode == 116) {
                event.preventDefault();
                return false;
            }
        });
        $(".tiennhuanbutnha").on('keyup change', Tongtiennhuanbut);
    });
    //Capturing when the user modifies a field
    var warnMessage = 'Tin bài đã bị thay đổi và thoát không đúng cách!\n' +
        '1. Bạn phải chọn: Ở lại trang (Stay on page) \n' +
        '2. Bấm Lưu thay đổi (hoặc Thực hiện tác vụ) \n' +
        '3. Sau đó bấm nút Ɔ Thoát (Phải sử dụng nút này để thoát). \n' +
        '(Nếu không, tin bài này sẽ bị khóa, người khác không mở để duyệt sửa được!)';
    var formModified = new Boolean();
    formModified = false;
    $('input:not(:button,:submit),textarea,select').on('change', function () {
        formModified = true;
        //startAutoSave();
    });
    // Checking if the user has modified the form upon closing window
    $('input:submit').on('click', function (e) {

        formModified = false;
    });
    window.onbeforeunload = function () {
        if (formModified != false) return warnMessage;
    }

    function OnChangeContent(sender, args) {
        formModified = true;
    }
    function removeValue(id, value) {
        var arrTemp = $('#' + id).val();
        var sResult = "";
        var arr = new Array();
        arr = arrTemp.split(";");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] && arr[i] != value) {
                sResult += ";" + arr[i];
            }
        }
        $('#' + id).val(sResult.substring(1, sResult.length));

    }
    function addValue(id, value) {
        if ($('#' + id).val().indexOf(value) == -1) {
            if ($('#' + id).val() == '' || $('#' + id).val() == null)
                $('#' + id).val(value);
            else
                $('#' + id).val($('#' + id).val() + ';' + value);
        }

    }

    var preVal;
    function changeCategory() {
        var eSelect = document.getElementById('<%= ddlCategory.ClientID %>').value;
        document.getElementById('<%= hdf_Category.ClientID %>').value = eSelect;
        $("#checkbox-tree").jstree().deselect_all(true);
        $("#checkbox-tree").jstree().select_node(eSelect, true);
    }
    <%--function changeWF() {
        var eWFSelect = document.getElementById('<%= ddlWFTop.ClientID%>');
        if (eWFSelect.selectedIndex > -1) {
            var iWF = eWFSelect.options[eWFSelect.selectedIndex].value;
            var strWF = eWFSelect.options[eWFSelect.selectedIndex].text;
            document.getElementById('<%= hdf_WF.ClientID %>').value = iWF;
            document.getElementById('<%= hdf_WF_Text.ClientID %>').value = strWF;
        }
    }--%>
</script>
<script type="text/javascript">
    function OpenDialog() {
        $("#dialog-modal").dialog({
            width: 435,
            height: 180,
            modal: false,
            position: { my: "top", at: "top", of: window }
        });
        $("#dialog-modal").parent().appendTo($("form:first"));

    }
    function CloseDialog() {
        //$("#dialog-modal").dialog("close");
        $("#dialog-modal").dialog("destroy"); $("form:first").remove('#dialog-modal');
    }
    function OpenDALETDialog() {
        $("#dialog-dalet").dialog({
            width: 700,
            height: 470,
            modal: false,
            position: { my: "top", at: "top", of: window }
        });
        $("#dialog-dalet").parent().appendTo($("form:first"));

    }
    function CloseDALETDialog() {
        $("#dialog-dalet").dialog("close");
        //$("#dialog-dalet").dialog("destroy"); $("form:first").remove('#dialog-dalet');
    }
    function OpenMSGDialog() {
        $("#dialog-msg").dialog({
            title: "Thông tin",
            width: 700,
            height: 250,
            modal: false,
            position: { my: "top", at: "top", of: window }
        });
        $("#dialog-msg").parent().appendTo($("form:first"));

    }
    function CloseMSGDialog() {
        //$("#dialog-modal").dialog("close");
        $("#dialog-msg").dialog("destroy"); $("form:first").remove('#dialog-msg');
    }

</script>
<script language="javascript" type="text/javascript">
    function checkvalidatexuatban() {
        var ngayxuatban = document.getElementById('<%=txtPublishedDate.ClientID%>').value;
        if (ngayxuatban == "" || ngayxuatban == '____/__/__ __:__') {
            alert("Bạn chưa chọn ngày xuất bản!");
            document.getElementById('<%=txtPublishedDate.ClientID%>').focus();
            return false;
        }
        var txtCredit = document.getElementById('<%=txtCredit.ClientID%>').value;
        if (txtCredit == "" || txtCredit == "0") {
            alert("Bạn chưa chấm nhuận bút!");
            document.getElementById('<%=txtCredit.ClientID%>').focus();
            return false;
        }

    }
    function checkvalidatenhuan() {
        var ddlnhuanbuttype2 = document.getElementById('<%=ddlnhuanbuttype.ClientID%>').value;
        if (ddlnhuanbuttype2 == 0) {
            alert("Bạn chưa chọn thể loại bài!");
            document.getElementById('<%=ddlnhuanbuttype.ClientID%>').focus();
            return false;
        }
        var ddlnhuanbutuser2 = document.getElementById('<%=ddlnhuanbutuser.ClientID%>').value;
        if (ddlnhuanbutuser2 == -1) {
            alert("Bạn chưa chọn Tác giả!");
            document.getElementById('<%=ddlnhuanbutuser.ClientID%>').focus();
            return false;
        }
        $('#<%=hdf_nhuanbut.ClientID %>').val($('#<%=txtCredit.ClientID%>').val());
    }
    function Tongtiennhuanbut() {
        var sum = 0;
        $(".tiennhuanbutnha").each(function () {
            //add only if the value is number
            var res = this.value.replace(".", "");
            if (!isNaN(res) && this.value.length != 0 && (!Number.isInteger(res))) {
                sum += parseFloat(res);
            }
        });
        $("#<%=txtCredit.ClientID%>").val(sum);
    }
    $(function () {
        var theTable = $('table.tablesorter');

        $("#filter").keyup(function () {
            //$.uiTableFilter(theTable, this.value);
            refreshFiles();
        });

        //        $('#filter-form').submit(function () {
        //            theTable.find("tbody > tr:visible > td:eq(1)").mousedown();
        //            return false;
        //        }).focus(); //Give focus to input field
    });
    var totalPage = 0;
    var curpage = 1;
    function onFetchSuccess(response, ctx) {
        var xmlDoc = $.parseXML(response);
        var xml = $(xmlDoc);
        var tbfiles = xml.find("tblFiles");
        var row = $(".tablesorter tr.TRgrid:last-child").clone(true);
        $(".tablesorter tr.TRgrid:not(:last-child)").remove();
        $.each(tbfiles, function () {
            var file = $(this);
            totalPage = $(this).find("TotalPage").text();

            $("td", row).eq(0).html($(this).find("FileName").text());

            $("td", row).eq(1).html($(this).find("DateModified").text());
            $("td", row).eq(2).html($(this).find("FileSize").text());

            row.addClass("TRgrid-Hover");
            $(".tablesorter tr.TRgrid:last-child").before(row);

            row = $(".tablesorter tr.TRgrid:last-child").clone(true);
        });
        $("#LoadingImage").hide();
        //Pager
        $('#lcurPage').html(curpage);
        $('#ltotalPage').html(totalPage);

        if ($('#sltType :selected').val() == 3) {
            $('a[title="IMAGES"]').on('click', function () {
                window.open($(this).attr("class") + "/" + $(this).attr("href"));
                return false;
            });
        }
        else {
            $('a[title="Play"]').on('click', function () {
                ViewMedia($(this).attr("class") + "/" + getDecodeString($(this).attr("href").match(/[^\/\\]+$/)));
                return false;
            });
        }
        $('a[title="Download"]').on('click', function () {
            window.open($(this).attr("class") + "/" + getEncodedString($(this).attr("href").match(/[^\/\\]+$/)));
            return false;
        });
    }
    function onFetchError(result, ctx) { $("#LoadingImage").hide(); }

    function refreshFiles() {
        formModified = false;
        $("#LoadingImage").show();
        curpage = 1;
        FetchFiles(curpage + "|" + $('#<%=drlSource.ClientID %> :selected').val() + "|" + $('#sltType :selected').val() + "|" + $('#filter').val());
    }
    function getPrevFiles() {
        formModified = false;
        if (curpage > 1) {
            $("#LoadingImage").show();
            curpage = curpage - 1;
            FetchFiles(curpage + "|" + $('#<%=drlSource.ClientID %> :selected').val() + "|" + $('#sltType :selected').val() + "|" + $('#filter').val());
        }
    }
    function getNextFiles() {
        formModified = false;
        if (curpage < totalPage) {
            $("#LoadingImage").show();
            curpage += 1;
            FetchFiles(curpage + "|" + $('#<%=drlSource.ClientID %> :selected').val() + "|" + $('#sltType :selected').val() + "|" + $('#filter').val());
        }
    }
    // Click 2 play
    var storagePath = '<%= StorageFolder %>';
    $('a[title="Play"]').on('click', function () {
        ViewMedia(storagePath + "/" + getDecodeString($(this).attr("href").match(/[^\/\\]+$/)));
        return false;
    });
    $('a[title="Download"]').on('click', function () {
        window.open(storagePath + "/" + getEncodedString($(this).attr("href").match(/[^\/\\]+$/)));
        return false;
    });
</script>
<script type="text/javascript">
    function popupwindow(w, h) {
        var left = (screen.width / 2) - (w / 2);
        var top = (screen.height / 2) - (h / 2);
        return window.open("/DesktopModules/NVCMS.TinTuc/Manager/controls/_tinlienquan.aspx", "Chọn tin liên quan", 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=20, left=' + left);
    }
    function HandlePopupResult(result) {
        var arr = new Array();
        arr = result.split(";");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] != null && arr[i] != '') {
                var id = arr[i].split("|")[0];
                var title = Base64.decode(arr[i].split("|")[1]);
                var ttitle2 = "'idtinlienquazzz" + id + "'";
                var simagepathx = Base64.decode(arr[i].split("|")[2]);
                var simagepath = simagepathx.substring(0, simagepathx.length - 1);
                var ssumary = Base64.decode(arr[i].split("|")[3]);
                var slinkbai = Base64.decode(arr[i].split("|")[4]);
                addValue('<%= hdf_Related.ClientID %>', id);
                $('.list-lq ul').append('<li>'
                    + '<div style="width: auto; float: left" id=' + ttitle2 + '>'
                    + '<a href="#"><strong>' + title + '</strong></a>'
                    + '<a title="linkbai" style="display: none" class="' + slinkbai + '" href="#">&nbsp;</a>'
                    + '<a title="sumary" style="display: none" class="' + ssumary + '" href="#">&nbsp; </a>'
                    + '<a title="imagepath" style="display: none" class="' + simagepath + '" href="#">&nbsp;</a>'
                    + '<a title="tieude" class="' + title + '" href="' + id + '">&nbsp;</a>'
                    + '</div>'
                    + '<a class="delRelated" onclick="javascript:delRl(this,' + id + ';" title="Loại bỏ tin này?" style="cursor: pointer;"><span class="removeSelected"><em class="icon ni ni-trash-fill"></em></span></a>'
                    + '<a class="insertRelated" onclick="javascript:insertRelated(' + ttitle2 + ');" title="Chèn vào bài viết?" style="cursor: pointer;">[Dài] </a>'
                    + '<a class="insertRelated" onclick="javascript:insertRelated4(' + ttitle2 + ');" title="Chèn vào bài viết?" style="cursor: pointer;">[Dài KHÔNG ẢNH] </a>'
                    + '<a class="insertRelated" onclick="javascript:insertRelated3(' + ttitle2 + ');" title="Chèn vào bài viết?" style="cursor: pointer;">[Phải] </a>'
                    + '<a class="insertRelated" onclick="javascript:insertRelated2(' + ttitle2 + ');" title="Chèn vào bài viết?" style="cursor: pointer;">[Trái] </a>'
                    + '</li >');
            }
        }
    }
    function delRl(sender, id) {
        if (confirm("Bạn có chắc chắn muốn xóa?") == true) {
            //Remove file from FileList
            removeValue('<%= hdf_Related.ClientID %>', id);
            $(sender).parent().remove();

            return false;
        }
    }
    function removeValue(id, value) {
        var arrTemp = $('#' + id).val();
        var sResult = "";
        var arr = new Array();
        arr = arrTemp.split(";");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] && arr[i] != value) {
                sResult += ";" + arr[i];
            }
        }
        $('#' + id).val(sResult.substring(1, sResult.length));

    }
    function insertRelated(idDiv) {
        var title1 = idDiv;
        var filename = $('#' + idDiv + ' a[title="tieude"]').attr("class");
        var idtin = $('#' + idDiv + ' a[title="tieude"]').attr("href");
        var imagepath2 = $('#' + idDiv + ' a[title="imagepath"]').attr("class");
        var imagepath = imagepath2 + '?width=120&height=100&mode=crop&anchor=middlecenter';
        var sumary = $('#' + idDiv + ' a[title="sumary"]').attr("class");
        var linkbai2 = $('#' + idDiv + ' a[title="linkbai"]').attr("class");
        if (linkbai2 != null) {
            var linkbai = linkbai2.replace("cms.thuongtruong.com.vn", "thuongtruong.com.vn");
        }
        else {
            var linkbai = linkbai2;
        }

        var editor = CKEDITOR.instances.<%=teContent.ClientID%>; //get a reference to the editor
        editor.insertHtml("<table class='tinlienquantrongbai' align='center'><tbody><tr><td class='tinlienquantrongbaitd1'><img alt='" + filename + "' style='width: 100%;' src='" + imagepath + "' /></td><td class='tinlienquantrongbaitd2'><h4><a href=" + linkbai + ">" + filename + "</a></h4><p>" + sumary + "</p></td></tr></tbody></table>");
        alert("Đã chèn xong tin liên quan vào bài! Nếu không muốn giữ lại danh sách! Bạn có thể xóa tin");
    }
    //cHEN TIN DAI KHONG ANH
    function insertRelated4(idDiv) {
        var title1 = idDiv;
        var filename = $('#' + idDiv + ' a[title="tieude"]').attr("class");
        var idtin = $('#' + idDiv + ' a[title="tieude"]').attr("href");
        var imagepath2 = $('#' + idDiv + ' a[title="imagepath"]').attr("class");
        var imagepath = imagepath2 + '?width=120&height=100&mode=crop&anchor=middlecenter';
        var sumary = $('#' + idDiv + ' a[title="sumary"]').attr("class");
        var linkbai2 = $('#' + idDiv + ' a[title="linkbai"]').attr("class");
        if (linkbai2 != null) {
            var linkbai = linkbai2.replace("cms.thuongtruong.com.vn", "thuongtruong.com.vn");
        }
        else {
            var linkbai = linkbai2;
        }

        var editor = CKEDITOR.instances.<%=teContent.ClientID%>; //get a reference to the editor
        editor.insertHtml("<table align='center' class='tinlienquantrongbaikhonganh'><tbody><tr><td><p><strong><a href='" + linkbai + "' title='" + filename + "'>" + filename + " </a></strong>" + sumary + "</p></td></tr></tbody></table>");
        alert("Đã chèn xong tin liên quan vào bài! Nếu không muốn giữ lại danh sách! Bạn có thể xóa tin");
    }
    function insertRelated2(idDiv) {
        var title1 = idDiv;
        var filename = $('#' + idDiv + ' a[title="tieude"]').attr("class");
        var idtin = $('#' + idDiv + ' a[title="tieude"]').attr("href");
        var imagepath2 = $('#' + idDiv + ' a[title="imagepath"]').attr("class");
        var imagepath = imagepath2 + '?width=200&height=160&mode=crop&anchor=middlecenter';
        var sumary = $('#' + idDiv + ' a[title="sumary"]').attr("class");
        var linkbai2 = $('#' + idDiv + ' a[title="linkbai"]').attr("class");
        var linkbai = linkbai2.replace("cms.thuongtruong.com.vn", "thuongtruong.com.vn");
        var editor = CKEDITOR.instances.<%=teContent.ClientID%>; //get a reference to the editor
        editor.insertHtml("<table class='tinlienquantrongbaidoctrai' align='center'><tbody><tr><td class='tinlienquantrongbaitd1'><img alt='" + filename + "' style='width: 100%;' src='" + imagepath + "' /></td></tr><tr><td class='tinlienquantrongbaitd2'><h4><a href=" + linkbai + ">" + filename + "</a></h4><p>" + sumary + "</p></td></tr></tbody></table>");
        alert("Đã chèn xong tin liên quan vào bài! Nếu không muốn giữ lại danh sách! Bạn có thể xóa tin");
    }
    function insertRelated3(idDiv) {
        var title1 = idDiv;
        var filename = $('#' + idDiv + ' a[title="tieude"]').attr("class");
        var idtin = $('#' + idDiv + ' a[title="tieude"]').attr("href");
        var imagepath2 = $('#' + idDiv + ' a[title="imagepath"]').attr("class");
        var imagepath = imagepath2 + '?width=200&height=160&mode=crop&anchor=middlecenter';
        var sumary = $('#' + idDiv + ' a[title="sumary"]').attr("class");
        var linkbai2 = $('#' + idDiv + ' a[title="linkbai"]').attr("class");
        var linkbai = linkbai2.replace("cms.thuongtruong.com.vn", "thuongtruong.com.vn");
        var editor = CKEDITOR.instances.<%=teContent.ClientID%>; //get a reference to the editor
        editor.insertHtml("<table class='tinlienquantrongbaidocphai' align='center'><tbody><tr><td class='tinlienquantrongbaitd1'><img alt='" + filename + "' style='width: 100%;' src='" + imagepath + "' /></td></tr><tr><td class='tinlienquantrongbaitd2'><h4><a href=" + linkbai + ">" + filename + "</a></h4><p>" + sumary + "</p></td></tr></tbody></table>");
        alert("Đã chèn xong tin liên quan vào bài! Nếu không muốn giữ lại danh sách! Bạn có thể xóa tin");
    }
</script>
