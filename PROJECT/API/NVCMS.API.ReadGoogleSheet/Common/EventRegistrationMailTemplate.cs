namespace NVCMS.API.ReadGoogleSheet.Common;

/// <summary>
/// Bản port nguyên văn template mail xác nhận đăng ký sự kiện từ
/// NVCMS.Modules.Scheduler / CopyDataStudentFromLadiScheduledJob.vb
/// (EventContentMailSendUser + EventContentCODESendUser).
///
/// Giữ nguyên HTML để email khách hàng nhận được không đổi so với job DNN cũ.
/// </summary>
public static class EventRegistrationMailTemplate
{
    private const string CodePlaceholder = "__MACODE__";

    /// <summary>
    /// Dựng nội dung mail xác nhận.
    /// </summary>
    /// <param name="isSendCode">NV_Events_Cat.sendcode - có chèn mã QR/barcode hay không.</param>
    /// <param name="titleMail">Preheader ẩn (NV_Events_Cat.titleMail).</param>
    /// <param name="firstName">Họ đệm.</param>
    /// <param name="lastName">Tên.</param>
    /// <param name="catName">Tên nhóm sự kiện.</param>
    /// <param name="thoiGianTu">Thời gian bắt đầu đã format.</param>
    /// <param name="thoiGianDen">Thời gian kết thúc đã format.</param>
    /// <param name="diaDiem">Địa điểm.</param>
    /// <param name="contentMail">Nội dung HTML cấu hình ở nhóm sự kiện (đã HtmlDecode).</param>
    /// <param name="urlDomain">Domain sinh link check-in trong QR.</param>
    /// <param name="studentCode">Mã đăng ký của khách.</param>
    public static string BuildCustomerBody(
        bool isSendCode,
        string? titleMail,
        string? firstName,
        string? lastName,
        string? catName,
        string? thoiGianTu,
        string? thoiGianDen,
        string? diaDiem,
        string? contentMail,
        string urlDomain,
        string studentCode)
    {
        var body = $@"<!DOCTYPE html><html><head><meta content='fair.capstonevietnam.com' http-equiv='Copyright'><meta http-equiv='Content-Type' content='text/html;charset=utf-8'><meta name='viewport' content='width=device-width,initial-scale=1.0'><title>Thư xác nhận tham gia triển lãm</title><meta content='Demo' http-equiv='Version'><style type='text/css'>body{{margin:0;padding:0;background-color:#fff;color:#777;font-family:Arial,Helvetica,sans-serif;font-size:12px;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;width:100%!important}}a,a:link,a:visited{{color:#2c8fd6;text-decoration:underline}}a:active,a:hover{{text-decoration:none;color:#125f96!important}}h1,h1 a,h2,h2 a,h3,h3 a{{color:#2c8fd6!important}}h2{{padding:0 0 10px;margin:0 0 10px}}h2.name{{padding:0 0 7px;margin:0 0 7px}}h3{{padding:0 0 5px;margin:0 0 5px}}p{{margin:0 0 14px;padding:0}}img{{border:0;-ms-interpolation-mode:bicubic;max-width:100%}}a img{{border:none}}table td{{border-collapse:collapse}}td.quote{{font-family:Georgia,'Times New Roman',Times,serif;font-size:18px;line-height:20pt;color:#2c8fd6}}span.noLink a,span.phone a{{color:2c8fd6;text-decoration:none}}.ExternalClass,.ReadMsgBody{{width:100%}}@media (max-width:767px){{td[class=container],td[class=shareContainer],td[class=topContainer]{{padding-left:20px!important;padding-right:20px!important}}table[class=row]{{width:100%!important;max-width:600px!important}}img[class=banner],img[class=wideImage]{{width:100%!important;height:auto!important;max-width:100%}}}}@media (max-width:560px){{td[class=socialIconsContainer],td[class=twoFromThree]{{display:block;width:100%!important}}td[class=authorInfo],td[class=inner2]{{padding-right:30px!important}}td[class=socialIconsContainer]{{border-top:0!important}}td[class=socialIcons2],td[class=socialIcons]{{padding-top:0!important;text-align:left!important;padding-left:30px!important;padding-bottom:20px!important}}}}@media (max-width:480px){{td[class=inner],td[class=inner_image]{{padding-left:30px!important;padding-right:30px!important}}body,html{{margin-right:auto;margin-left:auto}}td[class=oneFromTwo]{{display:block;width:100%!important}}img[class=CToWUd]{{position: absolute;top: 50%;left: 50%;max-width:100%}}td[class=inner_image]{{padding-bottom:25px!important}}img[class=wideImage]{{width:auto!important;margin:0 auto}}td[class=viewOnline]{{display:none!important}}td[class=date]{{font-size:14px!important;padding:10px 30px!important;background-color:#f4f4f4;text-align:left!important}}td[class=title]{{font-size:24px!important;line-height:32px!important}}table[class=quoteContainer]{{width:100%!important;float:none}}td[class=quote]{{padding-right:0!important}}td[class=spacer]{{padding-top:18px!important}}}}@media (max-width:380px){{td[class=authorInfo],td[class=icon],td[class=socialIcons2]{{text-align:center!important}}td[class=shareContainer]{{padding:0 10px!important}}td[class=topContainer]{{padding:10px 10px 0!important;background-color:#e9e9e9!important}}td[class=container]{{padding:0 10px 10px!important}}table[class=row]{{min-width:240px!important}}img[class=wideImage]{{width:100%!important;max-width:255px}}td[class=spacer2]{{display:none!important}}td[class=spacer3]{{padding-top:23px!important}}table[class=iconContainer],table[class=iconContainer_right]{{width:100%!important;float:none!important}}table[class=authorPicture]{{float:none!important;margin:0 auto!important;width:80px!important}}td[class=icon]{{padding:5px 0 25px!important}}td[class=icon] img{{display:inline!important}}img[class=buttonRight]{{float:none!important}}img[class=bigButton]{{width:100%!important;max-width:224px;height:auto!important}}h2[class=website]{{font-size:22px!important}}}}#loader{{display:block;position:absolute;left:50%;top:0;margin:20px 0 20px -110px}}</style><!--[if IE]><style type='text/css'>@media (max-width:560px){{td[class=twoFromThree],td[class=socialIconsContainer]{{float:left;padding:0px;}}}}@media only screen and (max-width:480px){{    td[class=oneFromTwo]{{float:left;padding:0px;}}}}@media (max-width:380px){{span[class=phone]{{display:block !important;}}}}</style><![endif]-->
 </head>
 <body>
     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;'>
         <tbody> <tr><td style='display:none'>{titleMail}</td></tr>
             <tr>
                 <td class='topContainer' style='padding-left:5px; padding-right:5px; background-color:#2c8fd6;'>
                     <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                         <tbody>
                             <tr>
                                 <td class='oneFromTwo' width='50%' valign='middle' style='border-bottom:1px dotted #dddddd'>
                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                         <tbody>
                                             <tr>
                                                 <td class='inner' style='padding-top:10px; padding-right:15px; padding-bottom:10px; padding-left:30px;'>
                                                     <img alt='Capstone Vietnam' src='http://fair.capstonevietnam.com/LogoCapstonemail.png' border='0' align='left' style='display:block;'>
                                                 </td>
                                             </tr>
                                         </tbody>
                                     </table>
                                 </td>
                                 <td class='oneFromTwo' width='50%' valign='middle' style='border-bottom:1px dotted #dddddd'>
                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                         <tbody>
                                             <tr>
                                                 <td class='date' style='padding-top:20px; padding-right:30px; font-size:12px; padding-bottom:20px; padding-left:15px;font-family:Arial, Helvetica, sans-serif; line-height:100%; color:#2c8fd6; text-align:right;'>
                                                     <a href='https://www.facebook.comcapstonevietnam1?fref=ts' target='_blank'>Facebook</a>
                                                     <a href='https://twitter.comcapstonevietnam' target='_blank'>Twitter</a>
                                                     <a href='https://www.youtube.comuserStudyUSA1' target='_blank'>Youtube</a>
                                                 </td>
                                             </tr>
                                         </tbody>
                                     </table>
                                 </td>
                             </tr>
                         </tbody>
                     </table>
                 </td>
             </tr>
             <tr>
                 <td class='container' style='padding-left:5px; padding-right:5px; padding-bottom:20px; background-color:#e9e9e9;'>
                     <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                         <tbody>
                             <tr>
                                 <td Class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>
                                     Xin chào bạn: <b>{firstName} {lastName}</b>
                                 </td>
                             </tr>
                             <tr>
                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'>
                                     Cảm ơn bạn đã đăng ký tham dự
                                 </td>
                             </tr>
                             <tr>
                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:20px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:20px; line-height:26px; text-align: center; color:#b11116; font-weight:600;'>
                                     {catName}
                                 </td>
                             </tr>
                             <tr>
                                 <td class='title' colspan='2' style='padding-top:0px; padding-right:30px; padding-bottom:20px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; text-align: center; font-weight: bold; color:#1d1d1d; font-weight:300;'>
                                     do Capstone Education tổ chức
                                 </td>
                             </tr>
                             <tr>
                                 <td class='title' colspan='2' style='padding-top:15px; padding-right:30px; padding-bottom:20px; padding-left:30px;border-top:1px #dddddd dotted;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:22px; line-height:26px; text-align: center; font-weight: bold; color:#1d1d1d; font-weight:300;'>
                                     <p>Thời gian: {thoiGianTu} - đến: {thoiGianDen}</p>
                                     <p> Địa điểm: {diaDiem}</p>

                                 </td>
                             </tr>
                         </tbody>
                     </table>
 {CodePlaceholder}
                     <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                         <tbody>
                             <tr>
                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; border-top:1px #dddddd dotted;padding-bottom:20px; padding-left:30px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:27px; line-height:36px;color:#b11116; font-weight:300;'>

                                 </td>
                             </tr>
                             <tr>
                                 <td class='oneFromTwo' valign='top'>
                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                         <tbody>
                                             <tr>
                                                 <td class='inner' style='padding-left:15px; padding-right:30px; padding-bottom:32px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:14px; line-height:15pt; color:#1d1d1d;'>
                                                     {contentMail}
                                                 </td>
                                             </tr>
                                         </tbody>
                                     </table>
                                 </td>
                             </tr>
                         </tbody>
                     </table>
                     <table class='row' width='600' bgcolor='#f4f4f4' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
                         <tbody>
                             <tr>
                                 <td class='twoFromThree' width='50%' valign='top' style='border-top:1px #dddddd dotted;'>
                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                         <tbody>
                                             <tr>
                                                 <td class='inner2' style='padding-top:25px; padding-left:30px; padding-right:15px; padding-bottom:5px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'>
                                                     <h2 class='website' style='margin-top:0px; margin-bottom:10px !important; padding-top:0px; padding-bottom:10px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:24px; line-height:100%; color:#2c8fd6; font-weight:300;'>
                                                         <a style='text-decoration:none; color:#2c8fd6;' href='http://capstonevietnam.com'>
                                                             Capstone Vietnam
                                                         </a>
                                                     </h2>
                                                     <img src='http://fair.capstonevietnam.com/LogoCapstonemail.png' />
                                                 </td>
                                             </tr>
                                         </tbody>
                                     </table>
                                 </td>
                                 <td class='socialIconsContainer' width='50%' valign='bottom' style='border-top:1px #dddddd dotted;'>
                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                         <tbody>
                                             <tr>
                                                 <td class='inner2' style='padding-top:25px; padding-left:30px; padding-right:15px; padding-bottom:5px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'>
                                                     www.capstonevietnam.com<br >
                                                     duhoc@capstonevietnam.com<br >
                                                     FB: www.facebook.com/CapstoneVN<br />
                                                 </td>
                                             </tr>
                                         </tbody>
                                     </table>
                                 </td>
                             </tr>
                             <tr>
                                 <td class='twoFromThree' width='50%' valign='top' style='border-top:1px #dddddd dotted;'>
                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                         <tbody>
                                             <tr>
                                                 <td class='inner2' style='padding-top:5px; padding-left:30px; padding-right:15px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'>
                                                     <b>VP Hà Nội: 2 Lê Quý Đôn, P. Hai Bà Trưng</b><br />
                                                     T: 024 3938 8455 | Hotline: 0989 336 860
                                                 </td>
                                             </tr>
                                         </tbody>
                                     </table>
                                 </td>
                                 <td class='socialIconsContainer' width='50%' valign='bottom' style='border-top:1px #dddddd dotted;'>
                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                                         <tbody>
                                             <tr>
                                                 <td class='inner2' style='padding-top:5px; padding-left:30px; padding-right:15px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'>
                                                     <b>VP Hồ Chí Minh: 22 Trần Quý Khoách, P.Tân Định</b><br />
                                                     T: 028 3848 2628 | Hotline: 0918 215 445
                                                 </td>
                                             </tr>
                                         </tbody>
                                     </table>
                                 </td>
                             </tr>
                         </tbody>
                     </table>
                 </td>
             </tr>
         </tbody>
     </table>
 </body>
 </html> ";

        return isSendCode
            ? body.Replace(CodePlaceholder, BuildCodeBlock(urlDomain, studentCode))
            : body.Replace(CodePlaceholder, string.Empty);
    }

    /// <summary>Khối QR + barcode mã đăng ký (EventContentCODESendUser).</summary>
    private static string BuildCodeBlock(string urlDomain, string studentCode)
    {
        return $@"<table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'>
    <tbody>
        <tr>
            <td  style='padding-right:30px; padding-left:30px; border-top:1px #dddddd dotted;'>
                <table cellpadding='0' cellspacing='0' style='border-collapse:collapse; border-spacing:0;'>
                    <tbody>
   <tr>
       <td style='padding-top:15px; padding-right:5px; padding-bottom:5px; padding-left:5px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:12px; line-height:15pt; color:#1d1d1d;'>
           Dưới đây là mã số tham dự triển lãm, bạn vui lòng lưu lại, in thư này để xác nhận tại bàn đăng ký vào tham dự triển lãm:
       </td>
   </tr>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td class='title' style='border-bottom:1px #dddddd dotted;padding-top:5px; padding-right:30px; padding-bottom:10px; text-align: center; padding-left:30px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:20px; line-height:36px;color:#0087ff; font-weight:400;'>
                MÃ SỐ ĐĂNG KÝ CỦA BẠN LÀ .
            </td>
        </tr>
        <tr>
            <td class='oneFromTwo' width='100%' valign='top'>
                <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;padding-top:20px;'>
                    <tbody>
   <tr>
       <td class='inner_image' style='padding-top:15px; padding-left:30px; padding-right:15px; padding-bottom:35px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'>
           <img class='wideImage' alt='image' src='https://crm.capstonevietnam.com/Services/QrcodeHandler.ashx?data={urlDomain}/quantri/partner/checkin-eventm.html?studentcode={studentCode}&width=400&height=400' height='200' width='200' border='0' vspace='0' hspace='0' style='display:block;'>
       </td>
   </tr>
                    </tbody>
                </table>
            </td></tr><tr>
            <td class='oneFromTwo' width='100%' valign='top'>
                <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'>
                    <tbody>
   <tr>
       <td class='inner' style='padding-left:0px;padding-top:15px; padding-right:0px; padding-bottom:32px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'>
           <img class='wideImage' alt='image' src='https://crm.capstonevietnam.com/Services/BarcodeHandler.ashx?data={studentCode}&width=340&height=120' height='200' width='500' border='0' vspace='0' hspace='0' style='display:block;'>
       </td>
   </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table> ";
    }
}
