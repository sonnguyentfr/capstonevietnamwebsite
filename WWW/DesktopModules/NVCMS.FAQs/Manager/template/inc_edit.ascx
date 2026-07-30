<%@ Control Language="C#" AutoEventWireup="true" CodeFile="inc_edit.ascx.cs" Inherits="DesktopModules.NV_Events.Manager.template.inc_edit" %>
<p class="title" style="font-weight: bold; margin-bottom: 5px;"><asp:Literal ID="ltTitle" runat="server" /></p>
<div id="form_edit">
    <table style="width: 100%">
        <tr>
            <td valign="top" style="width: 750px">
                <table>
                    <tr>
                        <td>Tên Template</td>
                        <td><asp:TextBox ID="txtTemplateName" runat="server" Width="600px" /></td>
                    </tr>
                    <tr id="tr_FilePath" runat="server" visible="false">
                        <td>File</td>
                        <td><asp:TextBox ID="txtFilePath" runat="server" Width="600px" /></td>
                    </tr>
                     <tr>
                        <td>Nội dung</td>
                        <td>
                            <asp:TextBox runat="server" ID="txtValue" TextMode="MultiLine" Rows="12" Width="600" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Lưu" OnClick="btnSave_Click" />
                            <span>&nbsp;</span>
                            <asp:Button ID="btnList" runat="server" Text="Danh sách" OnClick="btnList_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Label ID="lbMessage" runat="server" />                
                        </td>
                    </tr>        
                </table>
            </td>
            <td valign="top" style="padding-left: 40px">
                <fieldset>
                    <legend><b style="color: red">DANH SÁCH TOKEN CÂU HỎI THƯỜNG GẶP</b></legend>
                    <div style="padding-top: 20px">
                    <ul>
                        <li>[QUESTION] : Câu hỏi</li>
                        <li>[ANSWER] : Câu trả lời</li>
                        <li>[DATE] : Ngày khởi tạo</li>                        
                    </ul>
                    <p><b><i>Token Sử dụng lặp lại nhiều tin bài</i></b></p>
                    <ul>
                        <li>[LIST] : Lặp lại số lượng sắp diễn ra</li>
                   </ul>
                    
                </div>
                </fieldset> 
                <fieldset>
                    <legend><b style="color: red">DANH SÁCH TOKEN HỎI ĐÁP</b></legend>
                    <div style="padding-top: 20px">
                    <ul>
                        <li>[USERNAME] : Tên người hỏi</li>
                        <li>[EMAIL] : Email người hỏi</li>
                        <li>[PHONE] : Số điện thoại</li>
                        <li>[ADDRESS] : Địa chỉ</li>
                        <li>[TITLE] : Tiêu đề Câu hỏi</li>
                        <li>[QUESTION] : Câu hỏi</li>
                        <li>[USERANSWER] : Tên người trả lời</li>
                        <li>[ANSWER] : Câu trả lời</li>
                        <li>[PUBLICHDATE] : Ngày xuất bản</li>                        
                    </ul>
                    <p><b><i>Token Sử dụng lặp lại nhiều tin bài</i></b></p>
                    <ul>
                        <li>[LIST] : Lặp lại số lượng sắp diễn ra</li>
                   </ul>
                </div>
                </fieldset>               
            </td>
        </tr>
    </table>        
</div>
