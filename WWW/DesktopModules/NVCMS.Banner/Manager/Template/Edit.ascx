<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Edit.ascx.vb" Inherits="NVCMS.Modules.BannerAdv.EditTemplate" %>
<!-- ace -->
<script src="/static/_Admin/js/ace/ace.js"></script>
<div class="nk-block nk-block-lg">
    <div class="nk-block-head">
        <div class="nk-block-head-content">
            <h4 class="title nk-block-title"><%=PortalSettings.ActiveTab.Description %></h4>
        </div>
    </div>
    <div class="card card-bordered">
        <div class="card-inner">
            <div class="gy-3">
                <div class="row g-3">
                    <div class="col-lg-7">
                        <div class="row g-3">
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label class="form-label" for="site-name">Tiêu đề</label>
                                </div>
                            </div>
                            <div class="col-lg-9">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" id="Title" runat="server" required="required" class="form-control">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3 align-center">
                            <div class="col-lg-3">
                                <div class="form-group">
                                    <label class="form-label">File Template</label>
                                </div>
                            </div>
                            <div class="col-lg-9">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <input type="text" id="txtFilePath" runat="server" required="required" class="form-control" disabled>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row g-3 align-center">
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <label class="form-label">Template Code</label>
                                </div>
                            </div>
                            <div class="col-lg-12">
                                <div class="form-group">
                                    <div class="form-control-wrap">
                                        <textarea id="txtcode" runat="server" cols="10" rows="10" class="codetemplate form-control" style="height: 400px;"></textarea>
                                        <asp:HiddenField ID="hdf_textcode" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row g-3">
                            <div class="col-lg-7 offset-lg-5">
                                <div class="form-group mt-2">
                                    <asp:LinkButton ID="lbtUpdate" runat="server" CssClass="btn btn-success">Cập nhật</asp:LinkButton>
                                    <asp:LinkButton ID="lbtCancel" runat="server" CssClass="btn btn-primary">Hủy Thao tác</asp:LinkButton>
                                    <asp:LinkButton ID="lbtDel" runat="server" CssClass="btn btn-danger">Xóa</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-5">
                        <fieldset>
                            <legend><b style="color: red">DANH SÁCH TOKEN</b></legend>
                            <div style="padding-top: 20px">
                                <p><b><i>Token Sử dụng cho chi tiết 1 tin bài</i></b></p>
                                <ul>
                                    <li>[IMAGE] : Đường dẫn ảnh</li>
                                    <li>[DESCRIPTION] : Mô tả nội dung</li>
                                    <li>[URL] : Địa chỉ liên kết</li>
                                </ul>
                                <p><b><i>Token Sử dụng lặp lại nhiều tin bài</i></b></p>
                                <ul>
                                    <li>[LIST] : Lặp lại số lượng</li>
                                </ul>

                            </div>
                        </fieldset>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <!-- card -->
</div>
<style type="text/css">
    .ace_editor {
        height: 500px;
    }
</style>
<script>
    //ACE
   <%-- var editor = ace.edit("<%=txtcode.ClientId%>");
    editor.setTheme("ace/theme/twilight");
    editor.session.setMode("ace/mode/html");
    //Get Value vao hidden field--%>
    $(document).ready(function () {
        // Javascript editor
        var HeadScript = ace.edit("<%=txtcode.ClientId%>");
        HeadScript.setTheme("ace/theme/monokai");
        HeadScript.getSession().setMode("ace/mode/html");
        HeadScript.setShowPrintMargin(false);
        HeadScript.getSession().on('change', function (e) {
            $('#<%=hdf_textcode.ClientID%>').val(HeadScript.getValue());
        });
    });
</script>
