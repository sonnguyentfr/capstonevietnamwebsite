<%@ Control Language="C#" AutoEventWireup="true" CodeFile="inc_edit.ascx.cs" Inherits="DesktopModules.TinTuc.Manager.template.inc_edit" %>

<script src="/static/_Admin/js/ace/ace.js"></script>
<div class="row">
    <div class="col-md-8 col-xs-12">
        <div class="x_content">
            <br>
            <div class="form-horizontal form-label-left" novalidate="">
                <h4>
                    <asp:Literal ID="ltTitle" runat="server" /> <asp:Label ID="lbMessage" runat="server"></asp:Label>
                </h4>
                <div class="form-group">
                    <label class="control-label col-md-2 col-sm-4 col-xs-12" for="first-name">
                        Tên Template <span class="required">*</span>
                    </label>
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <input type="text" id="txtTemplateName" runat="server" required="required" class="form-control">
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2 col-sm-4 col-xs-12" for="first-name">
                        File Template: <span class="required">*</span>
                    </label>
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <input type="text" id="txtFilePath" runat="server" class="form-control">
                    </div>
                </div>
                <div class="form-group">
                    <label class="control-label col-md-2 col-sm-4 col-xs-12" for="last-name">
                        Template Code <span class="required">*</span>
                    </label>
                    <div class="col-md-11 col-sm-6 col-xs-12">
                        <textarea id="txtValue" runat="server" cols="10" rows="10" class="codetemplate form-control" style="height: 400px;"></textarea>
                        <asp:HiddenField ID="hdf_textcode" runat="server" />
                    </div>
                </div>
                <div class="ln_solid"></div>
                <div class="form-group">
                    <div class="col-md-6 col-sm-6 col-xs-12 col-md-offset-3">
                        <%--<asp:LinkButton ID="lbtUpdate" runat="server" CssClass="btn btn-success" >Cập nhật</asp:LinkButton>--%>
                        <asp:Button ID="btnSave" runat="server" Text="Lưu" CssClass="btn btn-success"  OnClick="btnSave_Click" />
                        <asp:LinkButton ID="lbtCancel" runat="server" CssClass="btn btn-primary" OnClick="lbtCance_Click">Hủy Thao tác</asp:LinkButton>
                        <asp:LinkButton ID="lbtDel" runat="server" CssClass="btn btn-danger" Visible="false">Xóa</asp:LinkButton>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div class="col-md-4 col-xs-12">
        <div class="x_content">
            <fieldset>
                <legend><b style="color: red">DANH SÁCH TOKEN</b></legend>
                <div style="padding-top: 20px">
                    <p><b><i>Token Sử dụng cho chi tiết 1 tin bài</i></b></p>
                    <p><b><i>Token Sử dụng cho chi tiết 1 tin bài</i></b></p>
                    <ul>
                        <li>[NAME] : Tiêu đề tin bài</li>
                        <li>[IMAGE] : Ảnh đại diện của tin bài</li>
                        <li>[DESCRIPTION] : Mô tả của tin bài</li>
                        <li>[URL] : Địa chỉ liên kết đến chi tiết tin bài</li>                        
                    </ul>
                    <p><b><i>Token Sử dụng lặp lại nhiều tin bài</i></b></p>
                    <ul>
                        <li>[LIST_TOP] : Lặp lại số lượng tin TOP</li>
                        <li>[LIST_MORE] : Lặp lại số lượng tin mở rộng</li>
                   </ul>
                    <p><b><i>Token khác</i></b></p>
                    <ul>
                        <li>[TOP_ONE] : Tin bài top 1</li>
                        <li>[TOP_TWO] : Tin bài top 2</li>
                        <li>[TOP_?] : Tin bài top ?</li>
                    </ul>

                </div>
            </fieldset>
        </div>
    </div>
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
        var HeadScript = ace.edit("<%=txtValue.ClientID%>");
        HeadScript.setTheme("ace/theme/monokai");
        HeadScript.getSession().setMode("ace/mode/html");
        HeadScript.setShowPrintMargin(false);
        HeadScript.getSession().on('change', function (e) {
            $('#<%=hdf_textcode.ClientID%>').val(HeadScript.getValue());
        });
    });
</script>
