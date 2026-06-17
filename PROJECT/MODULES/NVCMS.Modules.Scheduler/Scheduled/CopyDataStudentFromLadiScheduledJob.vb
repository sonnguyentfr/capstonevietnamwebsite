Imports System.Threading.Tasks
Imports System.Web
Imports DotNetNuke.Services.Scheduling
Imports NVCMS.Modules.EventsWebsite
Imports NVCMS.Modules.Student

Namespace NVCMS.Modules.Scheduler

    Public Class CopyDataStudentFromLadiScheduledJob
        Inherits SchedulerClient

        Dim _EventsWebsite_CatController As New EventsWebsite_CatController
        Dim _EventsWebsiteController As New EventsWebsiteController
        Dim _StudentFromLadipageController As New StudentFromLadipageController
        Dim _StudentInfoController As New StudentInfoController
        Dim _EventsStudentController As New EventsStudentController


        Public Sub New(item As ScheduleHistoryItem)
            MyBase.New()
            Me.ScheduleHistoryItem = item
        End Sub


        ' ==========================
        ' ENTRY POINT (sync)
        ' ==========================
        Public Overrides Sub DoWork()
            ' Chạy async
            DoWorkAsync().GetAwaiter().GetResult()
        End Sub



        ' ==========================
        ' HÀM CHÍNH – ASYNC
        ' ==========================
        Private Async Function DoWorkAsync() As Task
            Try
                ScheduleHistoryItem.AddLogNote("Job bắt đầu...")

                Dim events = GetOnlineEvents()
                Dim token As String = TokenService.GetToken()
                For Each ev In events
                    If String.IsNullOrEmpty(ev.link_data_google_sheet) Then
                        ScheduleHistoryItem.AddLogNote($"Event {ev.Id} KHÔNG có Google Sheet ID → bỏ qua.")
                        Continue For
                    End If

                    Await GetAndInsertData(
                        ev.Id
                    )
                Next

                ScheduleHistoryItem.Succeeded = True
                ScheduleHistoryItem.AddLogNote("Job hoàn thành thành công!")

            Catch ex As Exception
                ScheduleHistoryItem.Succeeded = False
                ScheduleHistoryItem.AddLogNote("Lỗi job tổng: " & ex.ToString())
                Me.Errored(ex)

            Finally
                ScheduleHistoryItem.AddLogNote("Job chạy xong.")
            End Try
        End Function

        ' ==========================
        ' LẤY DANH SÁCH EVENT ONLINE
        ' ==========================
        Private Function GetOnlineEvents() As ArrayList
            Return _EventsWebsite_CatController.Events_Cat_GetAllShowOnline(50)
        End Function

        Private Async Function GetAndInsertData(eventId As Integer) As Task
            Try
                Dim s As String = ""
                ScheduleHistoryItem.AddLogNote($"Event {eventId} → Gọi API Import...")
                Dim arrstudentNotUpdateCrm As ArrayList
                arrstudentNotUpdateCrm = _StudentFromLadipageController._Info_GetByEventCatId(eventId, False)
                Dim sCode As String = ""
                Dim objEventCat As Events_CatInfo = _EventsWebsite_CatController.Events_Cat_GetByID(eventId, 50)
                sCode += objEventCat.Code
                sCode += DateTime.Now.ToString("yMM")
                For i As Integer = 0 To arrstudentNotUpdateCrm.Count - 1
                    Dim objStudentFromLadipageInfo As StudentFromLadipageInfo = CType(arrstudentNotUpdateCrm(i), StudentFromLadipageInfo)
                    If Not objStudentFromLadipageInfo Is Nothing Then
                        With objStudentFromLadipageInfo
                            '1. Nếu chưa được đẩy vào CRM thì mới thực hiện đẩy
                            If objStudentFromLadipageInfo.is_update_crm = False Then
                                Dim objEvent As EventsInfo
                                objEvent = _EventsWebsiteController.Events_GetByID(objStudentFromLadipageInfo.event_dia_diem_id, 50)
                                'Khai báo nội dung gửi mail
                                Dim sendcode As String = objEventCat.sendCode
                                Dim catname As String = objEventCat.CatName
                                Dim titlemail As String = objEventCat.titleMail
                                Dim thoijantu As String = objEvent.fromdatetime.ToString("dd/MM/yyyy HH:mm")
                                Dim thoijanden As String = objEvent.enddatetime.ToString("dd/MM/yyyy HH:mm")
                                Dim diamdiem As String = objEvent.diadiem
                                Dim noidung As String = HttpUtility.HtmlDecode(objEventCat.ContentMail)
                                Dim sfirstname As String = ""
                                Dim slastname As String = ""
                                Dim txtemail As String = ""
                                Dim ItemID As Integer = 0
                                If Not objEvent Is Nothing Then
                                    With objEvent
                                        '2. Check xem user này đã tồn tại trên CRM chưa
                                        Dim objStudentEmailExist As StudentInfoInfo
                                        objStudentEmailExist = _StudentInfoController._Info_GetByEmail(objStudentFromLadipageInfo.email)
                                        '2.1 Nếu email đã tồn tại chỉ cần insert vào sự kiên
                                        If (Not objStudentEmailExist Is Nothing) Then
                                            With objStudentEmailExist
                                                ItemID = objStudentEmailExist.id
                                                sfirstname = objStudentEmailExist.Hotendem
                                                slastname = objStudentEmailExist.Ten
                                                txtemail = objStudentEmailExist.Email
                                                '2.2 Check xem Học sinh này đã đăng ký sự kiện này chưa
                                                Dim objeventstudent As EventsStudentInfo
                                                objeventstudent = _EventsStudentController.Events_Student_SelectByEventstudentid(objStudentFromLadipageInfo.event_dia_diem_id, objStudentEmailExist.id)
                                                If Not objeventstudent Is Nothing Then
                                                    _EventsStudentController.Events_Student_UpdateStudentNguon(objStudentFromLadipageInfo.event_dia_diem_id, objStudentEmailExist.id, "ladi")
                                                    _EventsStudentController.Events_Student_UpdateStudentNguonTutao(objStudentFromLadipageInfo.event_dia_diem_id, objStudentEmailExist.id, objStudentFromLadipageInfo.link & ",")
                                                Else
                                                    ' Thay thế Request.Item("s") & "," bằng String.Empty hoặc giá trị phù hợp
                                                    _EventsStudentController.Events_Student_Insert(
                                                        objStudentFromLadipageInfo.event_dia_diem_id,
                                                        objStudentFromLadipageInfo.event_id,
                                                        objStudentEmailExist.id,
                                                        objStudentEmailExist.CODE,
                                                        SuKienZ.StatusCu,
                                                        "ladi",
                                                        DateTime.Now,
                                                        50,
                                                        objStudentFromLadipageInfo.link
                                                    )
                                                End If
                                            End With
                                        Else
                                            '2.2 Check sđt xem có chưa
                                            Dim objStudentsdtExist As StudentInfoInfo
                                            objStudentsdtExist = _StudentInfoController._Info_GetBySodienthoai(objStudentFromLadipageInfo.so_dien_thoai)
                                            If (Not objStudentsdtExist Is Nothing) Then
                                                With objStudentsdtExist
                                                    ItemID = objStudentsdtExist.id
                                                    sfirstname = objStudentsdtExist.Hotendem
                                                    slastname = objStudentsdtExist.Ten
                                                    txtemail = objStudentsdtExist.Email
                                                    '2.2 Check xem Học sinh này đã đăng ký sự kiện này chưa
                                                    Dim objeventstudent As EventsStudentInfo
                                                    objeventstudent = _EventsStudentController.Events_Student_SelectByEventstudentid(objStudentFromLadipageInfo.event_dia_diem_id, objStudentsdtExist.id)
                                                    If Not objeventstudent Is Nothing Then
                                                        _EventsStudentController.Events_Student_UpdateStudentNguon(objStudentFromLadipageInfo.event_dia_diem_id, objStudentsdtExist.id, "ladi")
                                                        _EventsStudentController.Events_Student_UpdateStudentNguonTutao(objStudentFromLadipageInfo.event_dia_diem_id, objStudentsdtExist.id, objStudentFromLadipageInfo.link & ",")
                                                    Else
                                                        ' Thay thế Request.Item("s") & "," bằng String.Empty hoặc giá trị phù hợp
                                                        _EventsStudentController.Events_Student_Insert(
                                                            objStudentFromLadipageInfo.event_dia_diem_id,
                                                            objStudentFromLadipageInfo.event_id,
                                                            objStudentsdtExist.id,
                                                            objStudentsdtExist.CODE,
                                                            SuKienZ.StatusCu,
                                                            "ladi",
                                                            DateTime.Now,
                                                            50,
                                                            objStudentFromLadipageInfo.link
                                                        )
                                                    End If
                                                End With
                                            Else
                                                '3. Nếu chưa tồn tại thì insert mới
                                                sfirstname = objStudentFromLadipageInfo.hotendem
                                                slastname = objStudentFromLadipageInfo.ten
                                                txtemail = objStudentFromLadipageInfo.email
                                                Dim objstudent As New StudentInfoInfo
                                                With objstudent
                                                    .VP = objEvent.Vanphong
                                                    .Type = 1
                                                    .Hotendem = objStudentFromLadipageInfo.hotendem
                                                    .Ten = objStudentFromLadipageInfo.ten
                                                    .Ngaysinh = If(objStudentFromLadipageInfo.ngay_sinh IsNot Nothing, Convert.ToDateTime(objStudentFromLadipageInfo.ngay_sinh), "01/01/1970")
                                                    .Sodienthoai = NormalizeVnPhone(objStudentFromLadipageInfo.so_dien_thoai)
                                                    .Email = objStudentFromLadipageInfo.email
                                                    .Sex = If(objStudentFromLadipageInfo.gioi_tinh IsNot Nothing, objStudentFromLadipageInfo.gioi_tinh, 0)
                                                    .HocVanTruongdanghoc = objStudentFromLadipageInfo.truong_dang_hoc
                                                    .TuVanKhac = objStudentFromLadipageInfo.thong_tin_khac
                                                    .UserId = 1
                                                    .FollowPhuongThuc = 15
                                                    .FollowUpStatus = 1
                                                    .CreatedDate = objStudentFromLadipageInfo.created_date
                                                    .TuVanEditDate = DateTime.Now
                                                    .TuVanApproveDate = DateTime.Now
                                                    .HocVanApproveDate = DateTime.Now
                                                    .HocVanEditDate = DateTime.Now
                                                    .FollowUpDateUpdate = DateTime.Now
                                                    .TuVanKhac = "<mark>Khách đăng ký Online tại: " & objEventCat.CatName & "</mark>."
                                                    .TuVanHocVanmongmuon = ""
                                                    .TuVanNamdi = ""
                                                    .TuVanKyhoc = "0"
                                                    .TuVanNganhhoc = ""
                                                    .TuVanTruongdukien = ""
                                                    .TuVanQuocgia = "0"
                                                End With
                                                ItemID = _StudentInfoController._Info_Insert(objstudent)
                                                'Update code
                                                _StudentInfoController._Info_InsertCode(ItemID, sCode & ItemID)
                                                'Lay thong tin khach hang luon
                                                Dim objStudentInfo As StudentInfoInfo
                                                objStudentInfo = _StudentInfoController._Info_GetByID(ItemID)
                                                '--- Luu vet thong tin
                                                Dim ctlFl As New StudentFollow_LogController
                                                ctlFl._Follow_Log_Insert(ItemID, "KHÁCH HÀNG: [" & sfirstname & " " & slastname & "] - ĐĂNG KÝ TẠI " & objEventCat.CatName, DateTime.Now, 50)
                                                _EventsStudentController.Events_Student_Insert(objStudentFromLadipageInfo.event_dia_diem_id, eventId, ItemID, sCode & ItemID, SuKienZ.StatusOnline, "ladi", DateTime.Now, 50, "ladi" & ",")

                                                _EventsStudentController.Events_Student_UpdateStudentNguon(objStudentFromLadipageInfo.event_dia_diem_id, ItemID, "ladi")
                                                _EventsStudentController.Events_Student_UpdateStudentNguonTutao(objStudentFromLadipageInfo.event_dia_diem_id, ItemID, objStudentFromLadipageInfo.link & ",")
                                            End If
                                        End If
                                        '

                                    End With
                                End If
                                'Xong thì update trạng thái đã đẩy CRM
                                _StudentFromLadipageController._Info_Update_Crm(objStudentFromLadipageInfo.id)
                                'Gửi mail thông báo
                                '---------------
                                Dim urldomain As String = "http://crm.capstonevietnam.com"
                                'Gui mail toi khach hang

                                Dim sTitle As String = "Thư xác nhận đăng ký thành công: " & objEventCat.CatName
                                Dim sBody As String = ""

                                Dim sName As String = "Capstone Vietnam <no-reply@capstonevietnam.com>"
                                'Dim sName As String = "Capstone Vietnam <duhoc@capstonevietnam.com>"
                                sBody = EventContentMailSendUser(sendcode, titlemail, sfirstname, slastname, catname, thoijantu, thoijanden, diamdiem, noidung, urldomain, sCode & ItemID)
                                If objEventCat.sendmail = True Then
                                    If txtemail = "" Or txtemail = "nA" Or txtemail = "Na" Or txtemail = "na" Or txtemail = "NA" Then
                                    Else
                                        SendMail(sName, txtemail, objEventCat.Email, "it@capstonevietnam.com; marketing.hn@capstonevietnam.com", sTitle, sBody)
                                        System.Threading.Thread.Sleep(1000)
                                    End If
                                Else
                                    SendMail(sName, objEventCat.Email, "", "it@capstonevietnam.com; marketing.hn@capstonevietnam.com", sTitle, sBody)
                                    System.Threading.Thread.Sleep(1000)
                                End If
                            End If
                        End With
                    End If


                Next

            Catch ex As TaskCanceledException
                ScheduleHistoryItem.AddLogNote($"Event {eventId} - TIMEOUT (30s): {ex.Message}")

            Catch ex As Exception
                ScheduleHistoryItem.AddLogNote($"Event {eventId} - Lỗi API: {ex.Message}")
            End Try
        End Function
        Private Function GetStudentInfo(ByVal studentId As Integer) As StudentInfoInfo
            Dim objStudentInfo As StudentInfoInfo
            objStudentInfo = _StudentInfoController._Info_GetByID(studentId)
            Return objStudentInfo

        End Function
        Public Shared Function EventContentMailSendUser(ByVal isSendCode As Boolean, ByVal titleMail As String, ByVal txtFirstName As String, txtLastName As String, CatName As String, thoijantu As String,
                                                    thoijanden As String, diamdiem As String, ContentMail As String, urldomain As String, sCode As String) As String
            Dim result As String = ""
            Dim sBody As String = "<!DOCTYPE html><html><head><meta content='fair.capstonevietnam.com' http-equiv='Copyright'><meta http-equiv='Content-Type' content='text/html;charset=utf-8'><meta name='viewport' content='width=device-width,initial-scale=1.0'><title>Thư xác nhận tham gia triển lãm</title><meta content='Demo' http-equiv='Version'><style type='text/css'>body{margin:0;padding:0;background-color:#fff;color:#777;font-family:Arial,Helvetica,sans-serif;font-size:12px;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;width:100%!important}a,a:link,a:visited{color:#2c8fd6;text-decoration:underline}a:active,a:hover{text-decoration:none;color:#125f96!important}h1,h1 a,h2,h2 a,h3,h3 a{color:#2c8fd6!important}h2{padding:0 0 10px;margin:0 0 10px}h2.name{padding:0 0 7px;margin:0 0 7px}h3{padding:0 0 5px;margin:0 0 5px}p{margin:0 0 14px;padding:0}img{border:0;-ms-interpolation-mode:bicubic;max-width:100%}a img{border:none}table td{border-collapse:collapse}td.quote{font-family:Georgia,'Times New Roman',Times,serif;font-size:18px;line-height:20pt;color:#2c8fd6}span.noLink a,span.phone a{color:2c8fd6;text-decoration:none}.ExternalClass,.ReadMsgBody{width:100%}@media (max-width:767px){td[class=container],td[class=shareContainer],td[class=topContainer]{padding-left:20px!important;padding-right:20px!important}table[class=row]{width:100%!important;max-width:600px!important}img[class=banner],img[class=wideImage]{width:100%!important;height:auto!important;max-width:100%}}@media (max-width:560px){td[class=socialIconsContainer],td[class=twoFromThree]{display:block;width:100%!important}td[class=authorInfo],td[class=inner2]{padding-right:30px!important}td[class=socialIconsContainer]{border-top:0!important}td[class=socialIcons2],td[class=socialIcons]{padding-top:0!important;text-align:left!important;padding-left:30px!important;padding-bottom:20px!important}}@media (max-width:480px){td[class=inner],td[class=inner_image]{padding-left:30px!important;padding-right:30px!important}body,html{margin-right:auto;margin-left:auto}td[class=oneFromTwo]{display:block;width:100%!important}img[class=CToWUd]{position: absolute;top: 50%;left: 50%;max-width:100%}td[class=inner_image]{padding-bottom:25px!important}img[class=wideImage]{width:auto!important;margin:0 auto}td[class=viewOnline]{display:none!important}td[class=date]{font-size:14px!important;padding:10px 30px!important;background-color:#f4f4f4;text-align:left!important}td[class=title]{font-size:24px!important;line-height:32px!important}table[class=quoteContainer]{width:100%!important;float:none}td[class=quote]{padding-right:0!important}td[class=spacer]{padding-top:18px!important}}@media (max-width:380px){td[class=authorInfo],td[class=icon],td[class=socialIcons2]{text-align:center!important}td[class=shareContainer]{padding:0 10px!important}td[class=topContainer]{padding:10px 10px 0!important;background-color:#e9e9e9!important}td[class=container]{padding:0 10px 10px!important}table[class=row]{min-width:240px!important}img[class=wideImage]{width:100%!important;max-width:255px}td[class=spacer2]{display:none!important}td[class=spacer3]{padding-top:23px!important}table[class=iconContainer],table[class=iconContainer_right]{width:100%!important;float:none!important}table[class=authorPicture]{float:none!important;margin:0 auto!important;width:80px!important}td[class=icon]{padding:5px 0 25px!important}td[class=icon] img{display:inline!important}img[class=buttonRight]{float:none!important}img[class=bigButton]{width:100%!important;max-width:224px;height:auto!important}h2[class=website]{font-size:22px!important}}#loader{display:block;position:absolute;left:50%;top:0;margin:20px 0 20px -110px}</style><!-- Internet Explorer fix --><!--[if IE]><style type='text/css'>@media (max-width:560px){td[class=twoFromThree],td[class=socialIconsContainer]{float:left;padding:0px;}}@media only screen and (max-width:480px){    td[class=oneFromTwo]{float:left;padding:0px;}}@media (max-width:380px){span[class=phone]{display:block !important;}}</style><![endif]--><!-- / Internet Explorer fix --> " _
                & " </head> " _
                & " <body> " _
                & "     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;'> " _
                & "         <tbody> <tr><td style='display:none'>" & titleMail & "</td></tr>" _
                & "             <tr> " _
                & "                 <td class='topContainer' style='padding-left:5px; padding-right:5px; background-color:#2c8fd6;'> " _
                & "                     <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'> " _
                & "                         <tbody> " _
                & "                             <tr> " _
                & "                                 <td class='oneFromTwo' width='50%' valign='middle' style='border-bottom:1px dotted #dddddd'> " _
                & "                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
                & "                                         <tbody> " _
                & "                                             <tr> " _
                & "                                                 <td class='inner' style='padding-top:10px; padding-right:15px; padding-bottom:10px; padding-left:30px;'> " _
                & "                                                     <img alt='Capstone Vietnam' src='http://fair.capstonevietnam.com/LogoCapstonemail.png' border='0' align='left' style='display:block;'> " _
                & "                                                 </td> " _
                & "                                             </tr> " _
                & "                                         </tbody> " _
                & "                                     </table> " _
                & "                                 </td> " _
                & "                                 <td class='oneFromTwo' width='50%' valign='middle' style='border-bottom:1px dotted #dddddd'> " _
                & "                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
                & "                                         <tbody> " _
                & "                                             <tr> " _
                & "                                                 <td class='date' style='padding-top:20px; padding-right:30px; font-size:12px; padding-bottom:20px; padding-left:15px;font-family:Arial, Helvetica, sans-serif; line-height:100%; color:#2c8fd6; text-align:right;'> " _
                & "                                                     <a href='https://www.facebook.comcapstonevietnam1?fref=ts' target='_blank'>Facebook</a> " _
                & "                                                     <a href='https://twitter.comcapstonevietnam' target='_blank'>Twitter</a> " _
                & "                                                     <a href='https://www.youtube.comuserStudyUSA1' target='_blank'>Youtube</a> " _
                & "                                                 </td> " _
                & "                                             </tr> " _
                & "                                         </tbody> " _
                & "                                     </table> " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                         </tbody> " _
                & "                     </table> " _
                & "                 </td> " _
                & "             </tr> " _
                & "             <tr> " _
                & "                 <td class='container' style='padding-left:5px; padding-right:5px; padding-bottom:20px; background-color:#e9e9e9;'> " _
                & "                     <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'> " _
                & "                         <tbody> " _
                & "                             <tr> " _
                & "                                 <td Class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'> " _
                & "                                     Xin chào bạn: <b>" & txtFirstName & " " & txtLastName & "</b>" _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                             <tr> " _
                & "                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:10px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; color:#1d1d1d; font-weight:300;'> " _
                & "                                     Cảm ơn bạn đã đăng ký tham dự " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                             <tr> " _
                & "                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; padding-bottom:20px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:20px; line-height:26px; text-align: center; color:#b11116; font-weight:600;'> " _
                & "                                     " & CatName & " " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                             <tr> " _
                & "                                 <td class='title' colspan='2' style='padding-top:0px; padding-right:30px; padding-bottom:20px; padding-left:30px;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:16px; line-height:20px; text-align: center; font-weight: bold; color:#1d1d1d; font-weight:300;'> " _
                & "                                     do Capstone Education tổ chức " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                             <tr> " _
                & "                                 <td class='title' colspan='2' style='padding-top:15px; padding-right:30px; padding-bottom:20px; padding-left:30px;border-top:1px #dddddd dotted;font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:22px; line-height:26px; text-align: center; font-weight: bold; color:#1d1d1d; font-weight:300;'> " _
                & "                                     <p>Thời gian: " & thoijantu & " - đến: " & thoijanden & "</p> " _
                & "                                     <p> Địa điểm: " & diamdiem & "</p> " _
                & "  " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                         </tbody> " _
                & "                     </table> " _
                & " __MACODE__" _
                & "                     <table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'> " _
                & "                         <tbody> " _
                & "                             <tr> " _
                & "                                 <td class='title' colspan='2' style='padding-top:5px; padding-right:30px; border-top:1px #dddddd dotted;padding-bottom:20px; padding-left:30px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:27px; line-height:36px;color:#b11116; font-weight:300;'> " _
                & "                                     " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                             <tr> " _
                & "                                 <td class='oneFromTwo' valign='top'> " _
                & "                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
                & "                                         <tbody> " _
                & "                                             <tr> " _
                & "                                                 <td class='inner' style='padding-left:15px; padding-right:30px; padding-bottom:32px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:14px; line-height:15pt; color:#1d1d1d;'> " _
                & "                                                     " & ContentMail & " " _
                & "                                                 </td> " _
                & "                                             </tr> " _
                & "                                         </tbody> " _
                & "                                     </table> " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                         </tbody> " _
                & "                     </table> " _
                & "                     <table class='row' width='600' bgcolor='#f4f4f4' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'> " _
                & "                         <tbody> " _
                & "                             <tr> " _
                & "                                 <td class='twoFromThree' width='50%' valign='top' style='border-top:1px #dddddd dotted;'> " _
                & "                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
                & "                                         <tbody> " _
                & "                                             <tr> " _
                & "                                                 <td class='inner2' style='padding-top:25px; padding-left:30px; padding-right:15px; padding-bottom:5px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'> " _
                & "                                                     <h2 class='website' style='margin-top:0px; margin-bottom:10px !important; padding-top:0px; padding-bottom:10px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:24px; line-height:100%; color:#2c8fd6; font-weight:300;'> " _
                & "                                                         <a style='text-decoration:none; color:#2c8fd6;' href='http://capstonevietnam.com'> " _
                & "                                                             Capstone Vietnam " _
                & "                                                         </a> " _
                & "                                                     </h2> " _
                & "                                                     <img src='http://fair.capstonevietnam.com/LogoCapstonemail.png' /> " _
                & "                                                 </td> " _
                & "                                             </tr> " _
                & "                                         </tbody> " _
                & "                                     </table> " _
                & "                                 </td> " _
                & "                                 <td class='socialIconsContainer' width='50%' valign='bottom' style='border-top:1px #dddddd dotted;'> " _
                & "                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
                & "                                         <tbody> " _
                & "                                             <tr> " _
                & "                                                 <td class='inner2' style='padding-top:25px; padding-left:30px; padding-right:15px; padding-bottom:5px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'> " _
                & "                                                     www.capstonevietnam.com<br > " _
                & "                                                     duhoc@capstonevietnam.com<br > " _
                & "                                                     FB: www.facebook.com/CapstoneVN<br /> " _
                & "                                                 </td> " _
                & "                                             </tr> " _
                & "                                         </tbody> " _
                & "                                     </table> " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                             <tr> " _
                & "                                 <td class='twoFromThree' width='50%' valign='top' style='border-top:1px #dddddd dotted;'> " _
                & "                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
                & "                                         <tbody> " _
                & "                                             <tr> " _
                & "                                                 <td class='inner2' style='padding-top:5px; padding-left:30px; padding-right:15px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'> " _
                & "                                                     <b>VP Hà Nội: 2 Lê Quý Đôn, P. Hai Bà Trưng</b><br /> " _
            & "                                                     T: 024 3938 8455 | Hotline: 0989 336 860 " _
                & "                                                 </td> " _
                & "                                             </tr> " _
                & "                                         </tbody> " _
                & "                                     </table> " _
                & "                                 </td> " _
                & "                                 <td class='socialIconsContainer' width='50%' valign='bottom' style='border-top:1px #dddddd dotted;'> " _
                & "                                     <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
                & "                                         <tbody> " _
                & "                                             <tr> " _
                & "                                                 <td class='inner2' style='padding-top:5px; padding-left:30px; padding-right:15px; padding-bottom:25px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'> " _
                & "                                                     <b>VP Hồ Chí Minh: 22 Trần Quý Khoách, P.Tân Định</b><br /> " _
            & "                                                     T: 028 3848 2628 | Hotline: 0918 215 445 " _
                & "                                                 </td> " _
                & "                                             </tr> " _
                & "                                         </tbody> " _
                & "                                     </table> " _
                & "                                 </td> " _
                & "                             </tr> " _
                & "                         </tbody> " _
                & "                     </table> " _
                & "                 </td> " _
                & "             </tr> " _
                & "         </tbody> " _
                & "     </table> " _
                & " </body> " _
                & " </html> "


            If isSendCode = False Then
                result = sBody.Replace("__MACODE__", "")
            Else
                result = sBody.Replace("__MACODE__", EventContentCODESendUser(urldomain, sCode))
            End If

            Return result.ToString()
        End Function
        Public Function NormalizeVnPhone(phone As String) As String
            If String.IsNullOrWhiteSpace(phone) Then Return ""

            Dim p As String = phone.Trim()

            ' bỏ ký tự thừa
            p = p.Replace(" ", "").Replace(".", "").Replace("-", "")

            If p.StartsWith("+84") Then
                p = "84" & p.Substring(3)
            ElseIf p.StartsWith("84") Then
                ' giữ nguyên
            ElseIf p.StartsWith("0") Then
                p = "84" & p.Substring(1)
            ElseIf p.Length = 9 Then
                p = "84" & p
            End If

            Return p
        End Function
        Public Shared Function EventContentCODESendUser(urldomain As String, sCode As String) As String
            Dim result As String = ""
            Dim scodekhachang = "<table class='row' width='600' bgcolor='#ffffff' align='center' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; text-align:left; border-spacing:0; max-width:100%;'> " _
            & "    <tbody> " _
            & "        <tr> " _
            & "            <td  style='padding-right:30px; padding-left:30px; border-top:1px #dddddd dotted;'> " _
            & "                <table cellpadding='0' cellspacing='0' style='border-collapse:collapse; border-spacing:0;'> " _
            & "                    <tbody> " _
            & "   <tr> " _
            & "       <td style='padding-top:15px; padding-right:5px; padding-bottom:5px; padding-left:5px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:12px; line-height:15pt; color:#1d1d1d;'> " _
            & "           Dưới đây là mã số tham dự triển lãm, bạn vui lòng lưu lại, in thư này để xác nhận tại bàn đăng ký vào tham dự triển lãm: " _
            & "       </td> " _
            & "   </tr> " _
            & "                    </tbody> " _
            & "                </table> " _
            & "            </td> " _
            & "        </tr> " _
            & "        <tr> " _
            & "            <td class='title' style='border-bottom:1px #dddddd dotted;padding-top:5px; padding-right:30px; padding-bottom:10px; text-align: center; padding-left:30px; font-family:Segoe UI, Helvetica Neue, Helvetica, Arial, sans-serif; font-size:20px; line-height:36px;color:#0087ff; font-weight:400;'> " _
            & "                MÃ SỐ ĐĂNG KÝ CỦA BẠN LÀ ." _
            & "            </td> " _
            & "        </tr> " _
            & "        <tr> " _
            & "            <td class='oneFromTwo' width='100%' valign='top'> " _
            & "                <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;padding-top:20px;'> " _
            & "                    <tbody> " _
            & "   <tr> " _
            & "       <td class='inner_image' style='padding-top:15px; padding-left:30px; padding-right:15px; padding-bottom:35px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'> " _
            & "           <img class='wideImage' alt='image' src='https://crm.capstonevietnam.com/Services/QrcodeHandler.ashx?data=" & urldomain & "/quantri/partner/checkin-eventm.html?studentcode=" & sCode & "&width=400&height=400' height='200' width='200' border='0' vspace='0' hspace='0' style='display:block;'> " _
            & "       </td> " _
            & "   </tr> " _
            & "                    </tbody> " _
            & "                </table> " _
            & "            </td></tr><tr> " _
            & "            <td class='oneFromTwo' width='100%' valign='top'> " _
            & "                <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse; border-spacing:0;'> " _
            & "                    <tbody> " _
            & "   <tr> " _
            & "       <td class='inner' style='padding-left:0px;padding-top:15px; padding-right:0px; padding-bottom:32px; font-family:Arial, Helvetica, sans-serif; font-size:12px; line-height:15pt; color:#777777;'> " _
            & "           <img class='wideImage' alt='image' src='https://crm.capstonevietnam.com/Services/BarcodeHandler.ashx?data=" & sCode & "&width=340&height=120' height='200' width='500' border='0' vspace='0' hspace='0' style='display:block;'> " _
            & "       </td> " _
            & "   </tr> " _
            & "                    </tbody> " _
            & "                </table> " _
            & "            </td> " _
            & "        </tr> " _
            & "    </tbody> " _
            & "</table> "

            result = scodekhachang
            Return result.ToString()
        End Function
        Private Sub SendMail(strFrom As String, strTo As String, strCC As String, strBCC As String, strSubject As String, strBody As String)
            Dim strSMTP As String = "email-smtp.ap-southeast-1.amazonaws.com:587"
            Dim account As String = "AKIAU63W444UQCIOEFTD"
            Dim password As String = "BBxtUK/c9kVBXyqA5EGkn5bAHlrY5S+Ie05PlZWe8u74"
            DotNetNuke.Services.Mail.Mail.SendMail(strFrom, strTo, strCC, strBCC, DotNetNuke.Services.Mail.MailPriority.High, strSubject, DotNetNuke.Services.Mail.MailFormat.Html, System.Text.Encoding.UTF8, strBody, "", strSMTP, "1", account, password, True)
            System.Threading.Thread.Sleep(100)
        End Sub
    End Class
End Namespace
