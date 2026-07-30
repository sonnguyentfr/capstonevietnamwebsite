<%@ Control Language="vb" AutoEventWireup="false" Explicit="true" CodeFile="Settings.ascx.vb" Inherits="NVCMS.Modules.FormLandingPage.SettingCustomeDisplay" %>
<%@ Register TagPrefix="dnn" TagName="TextEditor" Src="~/controls/TextEditor.ascx" %>
<style type="text/css">
    .setting_news {
        padding: 10px;
    }

        .setting_news table tr td {
            padding: 3px 0px;
        }

        .setting_news .list-radio label, .setting_news .list-checkbox label {
            padding-right: 10px;
        }

    .table tr td:first-child {
        width: 200px;
    }

    .table tr td {
        border: solid 1px #ebebeb;
        font-size: 12px;
        padding: 10px !important;
    }

        .table tr td input, .table tr td select {
            padding: 5px;
            border: solid 1px #ebebeb;
        }

    .tablesub tr td:first-child,
    .tablesub tr td:nth-child(3),
    .tablesub tr td:nth-child(5) {
        text-align: right;
        padding-right: 5px;
    }

    .tablesub tr td:nth-child(2),
    .tablesub tr td:nth-child(4),
    .tablesub tr td:nth-child(6) {
        width: 150px;
        font-size: 10px;
    }

    .tablesub tr td:first-child,
    .tablesub tr td:nth-child(2),
    .tablesub tr td:nth-child(5),
    .tablesub tr td:nth-child(6) {
        background: #f3f3f3;
    }
</style>
<div class="setting_news">
    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table table-bordered">
        <tbody>
            <tr>
                <td>Ảnh nền</td>
                <td>
                    <asp:FileUpload ID="file_upload" class="btn btn-xs btn-info multi" AllowMultiple="true" runat="server" />
                    <progress id="processuploadimage" style="display: none"></progress>
                    <br />
                    <asp:Label ID="lblMessage" runat="server" />
                    <br />
                    <asp:Image ID="ImgBackground" ImageUrl="" runat="server" />
                </td>
            </tr>
            <tr>
                <td>Tiêu đề form: </td>
                <td>
                    <asp:TextBox ID="txttiel" runat="server" Width="100%" /></td>
            </tr>
            <asp:UpdatePanel ID="upsukien" runat="server">
                <ContentTemplate>
                    <tr>
                        <td>Sự kiện: </td>
                        <td>
                            <asp:DropDownList ID="ddlSuken" runat="server" Width="600" AutoPostBack="true" OnSelectedIndexChanged="ddlSuken_SelectIndexChange"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>Đia điểm: </td>
                        <td>
                            <asp:DropDownList ID="ddlSukendiadiem" runat="server" Width="600" Enabled="false"></asp:DropDownList></td>
                    </tr>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" ID="UpdateProgress2">
                <ProgressTemplate>
                    <div class="loading" id="loadizng">
                        <img src="/images/loading3.gif" alt="Loading" width="200px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <tr>
                <td colspan="2">
                    <table cellspacing="0" cellpadding="0" border="0" width="100%" class="table table-bordered tablesub">
                        <tr>
                            <td colspan="6" style="text-align: left !important; background: none !important">
                                <h4>Cấu hình hiển thị form</h4>
                            </td>
                        </tr>
                        <tr>
                            <td>Họ và tên</td>
                            <td>
                                <asp:CheckBox ID="chkhovaten" runat="server" /></td>
                            <td>Quan tâm EB5</td>
                            <td>
                                <asp:CheckBox ID="chkEbfive" runat="server" />
                            </td>
                            <td>Ngày sinh</td>
                            <td>
                                <asp:CheckBox ID="chkNgaySinh" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td>
                                <asp:CheckBox ID="chkEMail" runat="server" /></td>
                            <td>Vai trò</td>
                            <td>
                                <asp:CheckBox ID="chkVaitro" runat="server" />Phụ Huynh / Học sinh</td>
                            <td>Yêu cầu tư vấn</td>
                            <td>
                                <asp:CheckBox ID="chkYecauTuvan" runat="server" /></td>
                        </tr>
                        <tr>
                            <td>Số điện thoại</td>
                            <td>
                                <asp:CheckBox ID="chkDienthoai" runat="server" /></td>
                            <td>Địa chỉ Tỉnh</td>
                            <td>
                                <asp:CheckBox ID="chkTinh" runat="server" /></td>
                            <td>Giới tính</td>
                            <td>
                                <asp:CheckBox ID="chkGiotinh" runat="server" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <asp:UpdatePanel ID="updateGuimail" runat="server">
                <ContentTemplate>
                    <tr>
                        <td>Gửi mail đến quản trị
                        </td>
                        <td>
                            <div class="list-radio">
                                <asp:RadioButton Checked="true" AutoPostBack="true" GroupName="GetType" ID="rd_KhongGui" runat="server" Text="Không" OnCheckedChanged="rdGetType_CheckedChanged" />
                                <asp:RadioButton AutoPostBack="true" GroupName="GetType" ID="rd_Gui" runat="server" Text="Có" OnCheckedChanged="rdGetType_CheckedChanged" />
                            </div>
                        </td>
                    </tr>
                    <tr id="tr_nhanmail" runat="server" visible="false">
                        <td>NHẬN Email: Danh sách mail<br />
                            <i>Các địa chỉ email cách nhau dấu<b> , (phẩy)</b>: it@capstonevietnam.com,manager.hn@capstonevietnam.com</i>
                        </td>
                        <td>
                            <asp:TextBox ID="txtemailnhan" runat="server" TextMode="MultiLine" Width="100%" Height="40px" /></td>
                    </tr>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress runat="server" ID="updateGuimailProgress1">
                <ProgressTemplate>
                    <div class="loading" id="loadizng">
                        <img src="/images/loading3.gif" alt="Loading" width="200px" />
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <tr>
                <td colspan="2">Nội dung giới thiệu</td>
            </tr>
            <tr>
                <td colspan="2">
                    <dnn:TextEditor DefaultMode="basic" ID="txtNoiDung" Width="100%" Height="600px" runat="server" />
                </td>
            </tr>
        </tbody>
    </table>
</div>
<asp:HiddenField ID="hdf_moduleid" runat="server" Value="0" />
<script type="text/javascript">
    //Xử lý upload file đính kém
    $(document).ready(function () {
        $("#<%=file_upload.ClientID%>").on("change", function () {
            $("#processuploadimage").show();
            var data = new FormData();
            var fileInput = document.getElementById('<%=file_upload.ClientID%>');
            var moduleid = document.getElementById('<%=hdf_moduleid.ClientID%>').value;
            for (i = 0; i < fileInput.files.length; i++) {
                var sfilename = fileInput.files[i].name;
                data.append(sfilename, fileInput.files[i]);
            }
            uploadToServer(data, moduleid);
            $(this).val('');
        });
        function uploadToServer(formData, moduleid) {
            $.ajax({
                url: '/DesktopModules/NVCMS.FormLandingPage/Display/UploadFile.ashx?moduleid=' + moduleid,
                type: 'POST',
                data: formData,
                cache: false,
                contentType: false,
                processData: false,
                async: false,
                success: OnSuccess,
                xhr: function () {
                    var fileXhr = $.ajaxSettings.xhr();
                    if (fileXhr.upload) {
                        $("#processuploadimage").show();
                        fileXhr.upload.addEventListener("#processuploadimage", function (e) {
                            if (e.lengthComputable) {
                                $("#processuploadimage").attr({
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
        function OnSuccess(response) {
            $("#processuploadimage").hide();
            $("#<%=lblMessage.ClientID%>").append(response);
            //$('.anh-addToContent').off();
            $("#<%=ImgBackground.ClientID%>").attr('src', response);
            console.log(response);

        }
    });
</script>
