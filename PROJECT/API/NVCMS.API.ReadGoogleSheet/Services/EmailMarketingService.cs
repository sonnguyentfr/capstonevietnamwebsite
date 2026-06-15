using Hangfire;
using NVCMS.API.ReadGoogleSheet.Entities;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;

namespace NVCMS.API.ReadGoogleSheet.Services
{
    public class EmailMarketingService : IEmailMarketingService
    {
        private readonly IMarketingCampaignRepository    _campaignRepo;
        private readonly IMarketingListMailRepository    _listMailRepo;
        private readonly IMarketingUnsubRepository       _unsubRepo;
        private readonly IMarketingHangfireLogRepository _logRepo;
        private readonly IMarketingCampaignSendRepository _campaignSendRepo;
        private readonly IMarketingSendLogRepository     _sendLogRepo;
        private readonly IBackgroundJobClient            _jobClient;
        private readonly ILogger<EmailMarketingService>  _logger;

        private const int StatusDraft     = 0;
        private const int StatusQueued    = 1;
        private const int StatusSending   = 2;
        private const int StatusCompleted = 3;
        private const int StatusFailed    = 4;
        private const int StatusPaused    = 5;

        public EmailMarketingService(
            IMarketingCampaignRepository     campaignRepo,
            IMarketingListMailRepository     listMailRepo,
            IMarketingUnsubRepository        unsubRepo,
            IMarketingHangfireLogRepository  logRepo,
            IMarketingCampaignSendRepository campaignSendRepo,
            IMarketingSendLogRepository      sendLogRepo,
            IBackgroundJobClient             jobClient,
            ILogger<EmailMarketingService>   logger)
        {
            _campaignRepo     = campaignRepo;
            _listMailRepo     = listMailRepo;
            _unsubRepo        = unsubRepo;
            _logRepo          = logRepo;
            _campaignSendRepo = campaignSendRepo;
            _sendLogRepo      = sendLogRepo;
            _jobClient        = jobClient;
            _logger           = logger;
        }

        // ── SendCampaignAsync (NEW FLOW) ──────────────────────────────────────
        public async Task<SendCampaignResult> SendCampaignAsync(SendCampaignBodyRequest request)
        {
            _logger.LogInformation("SendCampaignAsync: campaignId={CampaignId}", request.CampaignId);

            // Bước 1 – Tạo CampaignSend
            var campaignSend = new MarketingMailCampaignSend
            {
                CampaignId    = request.CampaignId,
                Subject       = request.Subject,
                Body          = request.Body,
                Status        = "Queued",
                CreatedDate   = DateTime.UtcNow,
                TotalRecipient = 0,
                TotalSent      = 0,
                TotalDelivered = 0,
                TotalOpened    = 0,
                TotalClicked   = 0,
                TotalBounced   = 0,
                TotalComplaint = 0
            };
            await _campaignSendRepo.AddAsync(campaignSend);
            _logger.LogInformation("Created CampaignSend id={Id}", campaignSend.Id);

            // Bước 2 – Lấy danh sách email
            var listMails = (await _listMailRepo.GetByCampaignIdAsync(request.CampaignId)).ToList();
            _logger.LogInformation("Found {Count} list-mail rows for campaign {Id}", listMails.Count, request.CampaignId);

            var sendLogs = new List<MarketingMailSendLog>();
            foreach (var lm in listMails)
            {
                var email = lm.Email?.Trim().ToLowerInvariant();
                if (string.IsNullOrEmpty(email)) continue;
                if (!IsValidEmail(email)) continue;

                var isUnsub = await _unsubRepo.IsUnsubscribedAsync(email, lm.PortalId ?? 0);
                if (isUnsub)
                {
                    _logger.LogDebug("Skip unsubscribed {Email}", email);
                    continue;
                }

                sendLogs.Add(new MarketingMailSendLog
                {
                    CampaignSendId = campaignSend.Id,
                    ListMailId     = lm.id,
                    Email          = email,
                    Status         = "Queued"
                });
            }

            // Bước 3 – Lưu SendLog
            if (sendLogs.Count > 0)
            {
                await _sendLogRepo.AddRangeAsync(sendLogs);
                _logger.LogInformation("Created {Count} SendLog rows for CampaignSend {Id}", sendLogs.Count, campaignSend.Id);
            }

            // Bước 4 – Cập nhật TotalRecipient
            campaignSend.TotalRecipient = sendLogs.Count;
            await _campaignSendRepo.UpdateAsync(campaignSend);

            // Bước 5 – Enqueue Hangfire
            _jobClient.Enqueue<CampaignBatchJob>(job => job.ExecuteAsync(campaignSend.Id, CancellationToken.None));
            _logger.LogInformation("Enqueued CampaignBatchJob for CampaignSendId={Id}", campaignSend.Id);

            // Bước 6 – Trả kết quả ngay
            return new SendCampaignResult
            {
                Success        = true,
                CampaignSendId = campaignSend.Id,
                TotalRecipient = sendLogs.Count
            };
        }

        // ── CreateCampaign ────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> CreateCampaignAsync(CreateCampaignRequest request)
        {
            var campaign = new Marketing_Mail_Campaing
            {
                Title       = request.Title,
                Description = request.Description,
                Subject     = request.Subject,
                TemplateId  = request.TemplateId,
                ScheduledAt = request.ScheduledAt,
                PortalId    = request.PortalId,
                UserId      = request.UserId,
                Status      = StatusDraft,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _campaignRepo.AddAsync(campaign);
            _logger.LogInformation("Created campaign {Id} '{Title}'", campaign.id, campaign.Title);

            return MapToStatusResponse(campaign, 0, 0, 0);
        }

        // ── ImportRecipients ──────────────────────────────────────────────────
        public async Task<int> ImportRecipientsAsync(AddRecipientsRequest request)
        {
            if (!request.Recipients.Any()) return 0;

            var existing = await _listMailRepo.GetByCampaignIdAsync(request.CampaignId);
            var existingEmails = existing
                .Select(r => r.Email?.ToLowerInvariant())
                .ToHashSet();

            var toInsert = new List<Marketing_Mail_ListMail>();
            foreach (var recipient in request.Recipients)
            {
                var emailLower = recipient.Email.Trim().ToLowerInvariant();
                if (string.IsNullOrEmpty(emailLower)) continue;
                if (existingEmails.Contains(emailLower)) continue;

                var isUnsub = await _unsubRepo.IsUnsubscribedAsync(emailLower, request.PortalId);
                if (isUnsub) continue;

                toInsert.Add(new Marketing_Mail_ListMail
                {
                    CampaingId      = request.CampaignId,
                    Email           = emailLower,
                    FullName        = recipient.FullName,
                    UserId          = request.UserId,
                    PortalId        = request.PortalId,
                    Status          = false,
                    sendcount       = 0,
                    RetryCount      = 0,
                    RecipientStatus = 0,
                    Datetime        = DateTime.UtcNow
                });
                existingEmails.Add(emailLower);
            }

            if (!toInsert.Any()) return 0;

            await _listMailRepo.AddRangeAsync(toInsert);
            _logger.LogInformation("Imported {Count} recipients into campaign {Id}", toInsert.Count, request.CampaignId);
            return toInsert.Count;
        }

        // ── ScheduleCampaign ──────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> ScheduleCampaignAsync(int campaignId, DateTime scheduledAt)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status != StatusDraft && campaign.Status != StatusPaused)
                throw new InvalidOperationException($"Campaign {campaignId} must be Draft or Paused to schedule.");

            campaign.Status      = StatusQueued;
            campaign.ScheduledAt = scheduledAt;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);
            await AddLogAsync(campaignId, $"Campaign scheduled at {scheduledAt:u}");

            var counts = await _listMailRepo.GetCampaignCountsAsync(campaignId);
            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── PauseCampaign ─────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> PauseCampaignAsync(int campaignId)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status != StatusSending && campaign.Status != StatusQueued)
                throw new InvalidOperationException($"Campaign {campaignId} must be Sending or Queued to pause.");

            campaign.Status      = StatusPaused;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);
            await AddLogAsync(campaignId, "Campaign paused");

            var counts = await _listMailRepo.GetCampaignCountsAsync(campaignId);
            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── ResumeCampaign ────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> ResumeCampaignAsync(int campaignId)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status != StatusPaused)
                throw new InvalidOperationException($"Campaign {campaignId} must be Paused to resume.");

            campaign.Status      = StatusQueued;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);
            await AddLogAsync(campaignId, "Campaign resumed");

            var counts = await _listMailRepo.GetCampaignCountsAsync(campaignId);
            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── StopCampaign ──────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> StopCampaignAsync(int campaignId)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status == StatusCompleted || campaign.Status == StatusDraft)
                throw new InvalidOperationException($"Campaign {campaignId} cannot be stopped in state {campaign.Status}.");

            var counts       = await _listMailRepo.GetCampaignCountsAsync(campaignId);
            campaign.Status      = counts.Sent > 0 ? StatusCompleted : StatusFailed;
            campaign.CompletedAt = DateTime.UtcNow;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);
            await AddLogAsync(campaignId, $"Campaign stopped. Final status: {campaign.Status}");

            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── GetStatistics ─────────────────────────────────────────────────────
        public async Task<CampaignStatisticsResponse> GetStatisticsAsync(int campaignId)
        {
            var campaign   = await GetCampaignOrThrowAsync(campaignId);
            var recipients = (await _listMailRepo.GetByCampaignIdAsync(campaignId)).ToList();

            return new CampaignStatisticsResponse
            {
                CampaignId        = campaign.id,
                Title             = campaign.Title,
                StatusLabel       = MapStatusLabel(campaign.Status ?? 0),
                TotalRecipients   = recipients.Count,
                SentCount         = recipients.Count(r => (r.RecipientStatus ?? 0) >= 1),
                DeliveredCount    = recipients.Count(r => (r.RecipientStatus ?? 0) >= 2),
                OpenedCount       = recipients.Count(r => r.OpenedAt.HasValue),
                ClickedCount      = recipients.Count(r => r.ClickedAt.HasValue),
                BouncedCount      = recipients.Count(r => r.RecipientStatus == 5),
                ComplaintCount    = recipients.Count(r => r.RecipientStatus == 6),
                UnsubscribedCount = recipients.Count(r => r.RecipientStatus == 7),
                FailedCount       = recipients.Count(r => r.RecipientStatus == 5 || r.RecipientStatus == 6),
                StartedAt         = campaign.StartedAt,
                CompletedAt       = campaign.CompletedAt
            };
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private async Task<Marketing_Mail_Campaing> GetCampaignOrThrowAsync(int campaignId)
        {
            var c = await _campaignRepo.GetByIdAsync(campaignId);
            if (c is null) throw new KeyNotFoundException($"Campaign {campaignId} not found");
            return c;
        }

        private async Task AddLogAsync(int campaignId, string message)
        {
            try
            {
                await _logRepo.AddAsync(new MarketingMailHangfireLog
                {
                    CampaignId  = campaignId,
                    Status      = "Info",
                    Message     = message,
                    CreatedDate = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to write hangfire log for campaign {Id}", campaignId);
            }
        }

        private static bool IsValidEmail(string email)
        {
            try { _ = new System.Net.Mail.MailAddress(email); return true; }
            catch { return false; }
        }

        private static CampaignStatusResponse MapToStatusResponse(
            Marketing_Mail_Campaing c, int total, int sent, int failed) => new()
        {
            Id              = c.id,
            Title           = c.Title,
            Subject         = c.Subject ?? string.Empty,
            Status          = c.Status,
            TotalRecipients = total,
            SentCount       = sent,
            FailedCount     = failed,
            ScheduledAt     = c.ScheduledAt,
            StartedAt       = c.StartedAt,
            CompletedAt     = c.CompletedAt
        };

        private static string MapStatusLabel(int status) => status switch
        {
            0 => "Draft",
            1 => "Queued",
            2 => "Sending",
            3 => "Completed",
            4 => "Failed",
            5 => "Paused",
            _ => "Unknown"
        };
    }
}

        private readonly IMarketingCampaignRepository   _campaignRepo;
        private readonly IMarketingListMailRepository   _listMailRepo;
        private readonly IMarketingUnsubRepository      _unsubRepo;
        private readonly IMarketingHangfireLogRepository _logRepo;
        private readonly ILogger<EmailMarketingService> _logger;

        // Status constants – khớp với comment trong entity
        private const int StatusDraft     = 0;
        private const int StatusQueued    = 1;
        private const int StatusSending   = 2;
        private const int StatusCompleted = 3;
        private const int StatusFailed    = 4;
        private const int StatusPaused    = 5;

        public EmailMarketingService(
            IMarketingCampaignRepository    campaignRepo,
            IMarketingListMailRepository    listMailRepo,
            IMarketingUnsubRepository       unsubRepo,
            IMarketingHangfireLogRepository logRepo,
            ILogger<EmailMarketingService>  logger)
        {
            _campaignRepo = campaignRepo;
            _listMailRepo = listMailRepo;
            _unsubRepo    = unsubRepo;
            _logRepo      = logRepo;
            _logger       = logger;
        }

        // ── CreateCampaign ────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> CreateCampaignAsync(CreateCampaignRequest request)
        {
            var campaign = new Marketing_Mail_Campaing
            {
                Title       = request.Title,
                Description = request.Description,
                Subject     = request.Subject,
                TemplateId  = request.TemplateId,
                ScheduledAt = request.ScheduledAt,
                PortalId    = request.PortalId,
                UserId      = request.UserId,
                Status      = StatusDraft,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            await _campaignRepo.AddAsync(campaign);
            _logger.LogInformation("Created campaign {Id} '{Title}'", campaign.id, campaign.Title);

            return MapToStatusResponse(campaign, 0, 0, 0);
        }

        // ── ImportRecipients ──────────────────────────────────────────────────
        public async Task<int> ImportRecipientsAsync(AddRecipientsRequest request)
        {
            if (!request.Recipients.Any()) return 0;

            // Lấy danh sách email đã có trong campaign để tránh trùng
            var existing = await _listMailRepo.GetByCampaignIdAsync(request.CampaignId);
            var existingEmails = existing
                .Select(r => r.Email?.ToLowerInvariant())
                .ToHashSet();

            var toInsert = new List<Marketing_Mail_ListMail>();
            foreach (var recipient in request.Recipients)
            {
                var emailLower = recipient.Email.Trim().ToLowerInvariant();
                if (string.IsNullOrEmpty(emailLower)) continue;
                if (existingEmails.Contains(emailLower)) continue;

                // Bỏ qua nếu đã unsubscribe
                var isUnsub = await _unsubRepo.IsUnsubscribedAsync(emailLower, request.PortalId);
                if (isUnsub)
                {
                    _logger.LogDebug("Skipping unsubscribed email {Email}", emailLower);
                    continue;
                }

                toInsert.Add(new Marketing_Mail_ListMail
                {
                    CampaingId      = request.CampaignId,
                    Email           = emailLower,
                    FullName        = recipient.FullName,
                    UserId          = request.UserId,
                    PortalId        = request.PortalId,
                    Status          = false,
                    sendcount       = 0,
                    RetryCount      = 0,
                    RecipientStatus = 0,        // Pending
                    Datetime        = DateTime.UtcNow
                });

                existingEmails.Add(emailLower);
            }

            if (!toInsert.Any()) return 0;

            await _listMailRepo.AddRangeAsync(toInsert);
            _logger.LogInformation("Imported {Count} recipients into campaign {CampaignId}",
                toInsert.Count, request.CampaignId);

            return toInsert.Count;
        }

        // ── ScheduleCampaign ──────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> ScheduleCampaignAsync(int campaignId, DateTime scheduledAt)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status != StatusDraft && campaign.Status != StatusPaused)
                throw new InvalidOperationException(
                    $"Campaign {campaignId} must be in Draft or Paused state to schedule. Current: {campaign.Status}");

            campaign.Status      = StatusQueued;
            campaign.ScheduledAt = scheduledAt;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);

            await AddLogAsync(campaignId, $"Campaign scheduled at {scheduledAt:u}");
            _logger.LogInformation("Campaign {Id} scheduled at {At}", campaignId, scheduledAt);

            var counts = await _listMailRepo.GetCampaignCountsAsync(campaignId);
            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── PauseCampaign ─────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> PauseCampaignAsync(int campaignId)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status != StatusSending && campaign.Status != StatusQueued)
                throw new InvalidOperationException(
                    $"Campaign {campaignId} must be Sending or Queued to pause. Current: {campaign.Status}");

            campaign.Status      = StatusPaused;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);

            await AddLogAsync(campaignId, "Campaign paused");
            _logger.LogInformation("Campaign {Id} paused", campaignId);

            var counts = await _listMailRepo.GetCampaignCountsAsync(campaignId);
            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── ResumeCampaign ────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> ResumeCampaignAsync(int campaignId)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status != StatusPaused)
                throw new InvalidOperationException(
                    $"Campaign {campaignId} must be Paused to resume. Current: {campaign.Status}");

            campaign.Status      = StatusQueued;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);

            await AddLogAsync(campaignId, "Campaign resumed → Queued");
            _logger.LogInformation("Campaign {Id} resumed", campaignId);

            var counts = await _listMailRepo.GetCampaignCountsAsync(campaignId);
            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── StopCampaign ──────────────────────────────────────────────────────
        public async Task<CampaignStatusResponse> StopCampaignAsync(int campaignId)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);

            if (campaign.Status == StatusCompleted || campaign.Status == StatusDraft)
                throw new InvalidOperationException(
                    $"Campaign {campaignId} cannot be stopped in state {campaign.Status}");

            var counts = await _listMailRepo.GetCampaignCountsAsync(campaignId);

            // Nếu đã gửi ít nhất 1 thì Completed, ngược lại Failed
            campaign.Status      = counts.Sent > 0 ? StatusCompleted : StatusFailed;
            campaign.CompletedAt = DateTime.UtcNow;
            campaign.UpdatedDate = DateTime.UtcNow;
            await _campaignRepo.UpdateAsync(campaign);

            await AddLogAsync(campaignId, $"Campaign stopped manually. Final status: {campaign.Status}");
            _logger.LogInformation("Campaign {Id} stopped → status {Status}", campaignId, campaign.Status);

            return MapToStatusResponse(campaign, counts.Total, counts.Sent, counts.Failed);
        }

        // ── GetStatistics ─────────────────────────────────────────────────────
        public async Task<CampaignStatisticsResponse> GetStatisticsAsync(int campaignId)
        {
            var campaign = await GetCampaignOrThrowAsync(campaignId);
            var recipients = await _listMailRepo.GetByCampaignIdAsync(campaignId);
            var list = recipients.ToList();

            return new CampaignStatisticsResponse
            {
                CampaignId       = campaign.id,
                Title            = campaign.Title,
                StatusLabel      = MapStatusLabel(campaign.Status),
                TotalRecipients  = list.Count,
                SentCount        = list.Count(r => (r.RecipientStatus ?? 0) >= 1),
                DeliveredCount   = list.Count(r => (r.RecipientStatus ?? 0) >= 2),
                OpenedCount      = list.Count(r => r.OpenedAt.HasValue),
                ClickedCount     = list.Count(r => r.ClickedAt.HasValue),
                BouncedCount     = list.Count(r => r.RecipientStatus == 5),
                ComplaintCount   = list.Count(r => r.RecipientStatus == 6),
                UnsubscribedCount = list.Count(r => r.RecipientStatus == 7),
                FailedCount      = list.Count(r => r.RecipientStatus == 5 || r.RecipientStatus == 6),
                StartedAt        = campaign.StartedAt,
                CompletedAt      = campaign.CompletedAt
            };
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private async Task<Marketing_Mail_Campaing> GetCampaignOrThrowAsync(int campaignId)
        {
            var campaign = await _campaignRepo.GetByIdAsync(campaignId);
            if (campaign is null)
                throw new KeyNotFoundException($"Campaign {campaignId} not found");
            return campaign;
        }

        private async Task AddLogAsync(int campaignId, string message)
        {
            try
            {
                await _logRepo.AddAsync(new MarketingMailHangfireLog
                {
                    CampaignId  = campaignId,
                    Status      = "Info",
                    Message     = message,
                    CreatedDate = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to write hangfire log for campaign {Id}", campaignId);
            }
        }

        private static CampaignStatusResponse MapToStatusResponse(
            Marketing_Mail_Campaing campaign, int total, int sent, int failed) => new()
        {
            Id               = campaign.id,
            Title            = campaign.Title,
            Subject          = campaign.Subject ?? string.Empty,
            Status           = campaign.Status,
            TotalRecipients  = total,
            SentCount        = sent,
            FailedCount      = failed,
            ScheduledAt      = campaign.ScheduledAt,
            StartedAt        = campaign.StartedAt,
            CompletedAt      = campaign.CompletedAt
        };

        private static string MapStatusLabel(int status) => status switch
        {
            0 => "Draft",
            1 => "Queued",
            2 => "Sending",
            3 => "Completed",
            4 => "Failed",
            5 => "Paused",
            _ => "Unknown"
        };
    }
}
