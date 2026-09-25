using Hangfire;
using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class EmailMarketingService : IEmailMarketingService
    {
        private readonly IMarketingCampaignRepository   _campaignRepo;
        private readonly IMarketingListMailRepository   _listMailRepo;
        private readonly IMarketingUnsubRepository      _unsubRepo;
        private readonly IMarketingSendLogRepository    _sendLogRepo;
        private readonly IBackgroundJobClient           _jobClient;
        private readonly CRMDbContext                   _crmContext;
        private readonly ILogger<EmailMarketingService> _logger;

        public EmailMarketingService(
            IMarketingCampaignRepository   campaignRepo,
            IMarketingListMailRepository   listMailRepo,
            IMarketingUnsubRepository      unsubRepo,
            IMarketingSendLogRepository    sendLogRepo,
            IBackgroundJobClient           jobClient,
            CRMDbContext                   crmContext,
            ILogger<EmailMarketingService> logger)
        {
            _campaignRepo = campaignRepo;
            _listMailRepo = listMailRepo;
            _unsubRepo    = unsubRepo;
            _sendLogRepo  = sendLogRepo;
            _jobClient    = jobClient;
            _crmContext   = crmContext;
            _logger       = logger;
        }

        // ── SendCampaignAsync ─────────────────────────────────────────────────
        public async Task<SendCampaignResult> SendCampaignAsync(SendCampaignBodyRequest request)
        {
            _logger.LogInformation("SendCampaignAsync: campaignId={CampaignId}", request.CampaignId);

            // Bước 1 – Lấy toàn bộ email từ Marketing_Mail_ListMail
            var listMails = (await _listMailRepo.GetByCampaignIdAsync(request.CampaignId)).ToList();
            _logger.LogInformation("Found {Count} list-mail rows for campaign {Id}",
                listMails.Count, request.CampaignId);

            // Bước 2+3 – Lọc unsub, làm sạch email (trim/distinct/validate)
            var seen          = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var validMailItems = new List<Marketing_Mail_ListMail>();

            foreach (var lm in listMails)
            {
                var email = lm.Email?.Trim();
                if (string.IsNullOrEmpty(email)) continue;
                if (!IsValidEmail(email)) continue;
                if (!seen.Add(email)) continue;

                var isUnsub = await _unsubRepo.IsUnsubscribedAsync(
                    email.ToLowerInvariant(), lm.PortalId ?? 0);
                if (isUnsub)
                {
                    _logger.LogDebug("Skip unsubscribed {Email}", email);
                    continue;
                }

                validMailItems.Add(lm);
            }

            // Bước 4 – Insert 1 bản ghi Marketing_Mail_Campaign_Send cho lần gửi này
            var campaignSend = new MarketingMailCampaignSend
            {
                CampaignId     = request.CampaignId,
                TemplateId     = request.TemplateId,
                Subject        = request.Subject,
                Body           = request.Body,
                Status         = 0,
                TotalRecipient = validMailItems.Count,
                CreatedDate    = DateTime.Now
            };
            _crmContext.CampaignSends.Add(campaignSend);
            await _crmContext.SaveChangesAsync();

            _logger.LogInformation("Inserted Campaign_Send Id={CampaignSendId} for campaign {CampaignId}",
                campaignSend.Id, request.CampaignId);

            // Bước 5 – Insert Marketing_Mail_Send_Log (1 bản ghi / email), CampaignSendId = campaignSend.Id
            var sendLogs = validMailItems.Select(lm => new MarketingMailSendLog
            {
                CampaignSendId = campaignSend.Id,
                ListMailId     = lm.id,
                Email          = lm.Email!.Trim(),
                Status         = MailSendStatus.Queued,
                CreatedDate    = DateTime.Now,
                SenderEmailId = request.EmailAccountId
            }).ToList();

            if (sendLogs.Count > 0)
                await _sendLogRepo.AddRangeAsync(sendLogs);

            _logger.LogInformation("Inserted {Count} Queued send-logs for campaignSendId={CampaignSendId}",
                sendLogs.Count, campaignSend.Id);

            // Bước 6 – Enqueue Hangfire job trực tiếp
            _jobClient.Enqueue<CampaignBatchJob>(job =>
                job.ExecuteAsync(
                    campaignSend.Id,
                    request.Subject,
                    request.Body,
                    request.EmailAccountId,
                    CancellationToken.None));

            _logger.LogInformation("Enqueued CampaignBatchJob for campaignSendId={Id}", campaignSend.Id);

            return new SendCampaignResult
            {
                Success        = true,
                CampaignId     = request.CampaignId,
                TotalRecipient = sendLogs.Count
            };
        }

        // ── SendEventDetailStaticAsync ────────────────────────────────────────
        public async Task<SendEventDetailStaticResult> SendEventDetailStaticAsync(SendEventDetailStaticRequest request)
        {
            var ids    = request.Ids.Where(x => x > 0).Distinct().ToList();
            var result = new SendEventDetailStaticResult { TotalRequested = ids.Count };
            if (ids.Count == 0)
                return Fail("Ids is empty");

            _logger.LogInformation("SendEventDetailStaticAsync: eventId={EventId}, eventCatId={EventCatId}, ids={Count}",
                request.EventId, request.EventCatId, ids.Count);

            // Bước 1 – Kiểm tra event / eventCat
            var eventRow = await _crmContext.NV_Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.EventId);
            if (eventRow is null)
                return Fail($"Event {request.EventId} not found");
            if (eventRow.CatId.HasValue && eventRow.CatId.Value != request.EventCatId)
                return Fail($"Event {request.EventId} does not belong to eventCat {request.EventCatId}");

            var catRow = await _crmContext.NV_EventsCats.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.EventCatId);
            if (catRow is null)
                return Fail($"EventCat {request.EventCatId} not found");

            // Bước 2 – NV_Events_Student → Student_Info
            var eventStudents = await _crmContext.NV_EventsStudents
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            var studentIds = eventStudents.Values.Where(x => x.StudentId.HasValue).Select(x => x.StudentId!.Value).Distinct().ToList();
            var students = await _crmContext.StudentInfos
                .AsNoTracking()
                .Where(x => studentIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            // Bước 3 – Lọc: sai event, không có học sinh, email lỗi, trùng học sinh, đã unsub.
            // Không lọc trùng email: phụ huynh đăng ký nhiều con cùng 1 email, mỗi con cần mail/QR riêng.
            var seenStudents = new HashSet<int>();
            var valid = new List<(NV_Events_Student EventStudent, Student_Info Student, string Email)>();
            foreach (var id in ids)
            {
                void Skip(string reason, string? email = null)
                    => result.Skipped.Add(new SendEventDetailStaticSkipped { EventStudentId = id, Email = email, Reason = reason });

                if (!eventStudents.TryGetValue(id, out var es)) { Skip("NV_Events_Student not found"); continue; }
                if (es.EventId != request.EventId) { Skip($"Belongs to event {es.EventId}, not {request.EventId}"); continue; }
                if (!es.StudentId.HasValue || !students.TryGetValue(es.StudentId.Value, out var student)) { Skip("Student_Info not found"); continue; }

                var email = student.Email?.Trim();
                if (string.IsNullOrEmpty(email) || !IsValidEmail(email)) { Skip("Email invalid", email); continue; }
                if (!seenStudents.Add(student.Id)) { Skip($"Duplicate student {student.Id} in request", email); continue; }
                if (await _unsubRepo.IsUnsubscribedAsync(email.ToLowerInvariant(), student.PortalId ?? 0)) { Skip("Unsubscribed", email); continue; }

                valid.Add((es, student, email));
            }

            if (valid.Count == 0)
                return Fail("No valid recipients");

            var subject = string.IsNullOrWhiteSpace(request.Subject)
                ? $"Xác nhận đăng ký tham dự - {catRow.CatName}"
                : request.Subject.Trim();

            // Bước 4 – Insert Marketing_Mail_Campaign_Send (CampaignId = 0: không thuộc campaign)
            var campaignSend = new MarketingMailCampaignSend
            {
                CampaignId     = 0,
                TemplateId     = 0,
                Subject        = subject,
                Body           = EventDetailStaticEmailJob.TemplateName,
                Status         = 0,
                TotalRecipient = valid.Count,
                CreatedDate    = DateTime.UtcNow.AddHours(7)
            };
            _crmContext.CampaignSends.Add(campaignSend);
            await _crmContext.SaveChangesAsync();

            // Bước 5 – Insert Marketing_Mail_Send_Log (1 bản ghi / học sinh)
            var now = DateTime.UtcNow.AddHours(7);
            var sendLogs = valid.Select(v => new MarketingMailSendLog
            {
                CampaignSendId = campaignSend.Id,
                ListMailId     = 0,
                Email          = v.Email,
                Status         = MailSendStatus.Queued,
                CreatedDate    = now,
                SenderEmailId  = request.EmailAccountId
            }).ToList();
            await _sendLogRepo.AddRangeAsync(sendLogs);

            // Bước 6 – Enqueue job; body render riêng từng học sinh trong job
            var recipients = sendLogs.Select((log, i) => new EventDetailStaticRecipient
            {
                LogId          = log.Id,
                EventStudentId = valid[i].EventStudent.Id,
                StudentId      = valid[i].Student.Id
            }).ToList();

            var jobId = _jobClient.Enqueue<EventDetailStaticEmailJob>(job =>
                job.ExecuteAsync(
                    campaignSend.Id,
                    request.EventId,
                    request.EventCatId,
                    subject,
                    request.EmailAccountId,
                    recipients,
                    CancellationToken.None));

            _logger.LogInformation("Enqueued EventDetailStaticEmailJob jobId={JobId} campaignSendId={Id} recipients={Count} skipped={Skipped}",
                jobId, campaignSend.Id, recipients.Count, result.Skipped.Count);

            result.Success        = true;
            result.CampaignSendId = campaignSend.Id;
            result.TotalRecipient = recipients.Count;
            result.JobId          = jobId;
            result.Message        = $"Queued {recipients.Count}/{ids.Count}, skipped {result.Skipped.Count}";
            return result;

            SendEventDetailStaticResult Fail(string message)
            {
                result.Success = false;
                result.Message = message;
                return result;
            }
        }

        // ── CreateCampaign ────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> CreateCampaignAsync(CreateCampaignRequest request)
        {
            var campaign = new Marketing_Mail_Campaing
            {
                Title       = request.Title,
                Description = request.Description,
                PortalId    = request.PortalId,
                UserId      = request.UserId,
                CreatedDate = DateTime.UtcNow.AddHours(7)
            };

            await _campaignRepo.AddAsync(campaign);
            _logger.LogInformation("Created campaign {Id} '{Title}'", campaign.id, campaign.Title);

            return new CampaignStatusResponse
            {
                Id    = campaign.id,
                Title = campaign.Title
            };
        }

        // ── GetStatistics ─────────────────────────────────────────────────────
        public async Task<CampaignStatisticsResponse> GetStatisticsAsync(int campaignId)
        {
            var campaign = await _campaignRepo.GetByIdAsync(campaignId);
            if (campaign is null)
                throw new KeyNotFoundException($"Campaign {campaignId} not found");

            // Lấy tất cả Campaign_Send Ids thuộc campaign này
            var campaignSendIds = await _crmContext.CampaignSends
                .Where(cs => cs.CampaignId == campaignId)
                .Select(cs => cs.Id)
                .ToListAsync();

            var logs = campaignSendIds.Count > 0
                ? (await _crmContext.SendLogs
                    .Where(l => campaignSendIds.Contains(l.CampaignSendId))
                    .ToListAsync())
                : new List<MarketingMailSendLog>();

            return new CampaignStatisticsResponse
            {
                CampaignId      = campaign.id,
                Title           = campaign.Title ?? string.Empty,
                StatusLabel     = string.Empty,
                TotalRecipients = logs.Count,
                SentCount       = logs.Count(l => l.Status == MailSendStatus.Sent),
                DeliveredCount  = logs.Count(l => l.Status == MailSendStatus.Delivered),
                OpenedCount     = logs.Count(l => l.Status == MailSendStatus.Opened),
                ClickedCount    = logs.Count(l => l.Status == MailSendStatus.Clicked),
                BouncedCount    = 0,
                ComplaintCount  = 0,
                UnsubscribedCount = 0,
                FailedCount     = logs.Count(l => l.Status == MailSendStatus.Failed),
            };
        }

        // ── Helper ────────────────────────────────────────────────────────────
        private static bool IsValidEmail(string email)
        {
            try { _ = new System.Net.Mail.MailAddress(email); return true; }
            catch { return false; }
        }
    }
}
