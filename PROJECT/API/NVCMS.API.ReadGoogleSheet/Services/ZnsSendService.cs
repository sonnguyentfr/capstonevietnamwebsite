using Hangfire;
using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Common;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Jobs;
using NVCMS.API.ReadGoogleSheet.Models;
using NVCMS.API.ReadGoogleSheet.Repositories;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Services;

public class ZnsSendService : IZnsSendService
{
    private readonly IZnsTemplateRepository _templateRepo;
    private readonly IZaloZnsClient _zaloClient;
    private readonly IZnsSendLogRepository _sendLogRepo;
    private readonly IZnsSendQueueRepository _queueRepo;
    private readonly IZaloMessageLogRepository _messageLogRepo;
    private readonly IBackgroundJobClient _jobClient;
    private readonly CRMDbContext _crmDb;
    private readonly ILogger<ZnsSendService> _logger;

    public ZnsSendService(
        IZnsTemplateRepository templateRepo,
        IZaloZnsClient zaloClient,
        IZnsSendLogRepository sendLogRepo,
        IZnsSendQueueRepository queueRepo,
        IZaloMessageLogRepository messageLogRepo,
        IBackgroundJobClient jobClient,
        CRMDbContext crmDb,
        ILogger<ZnsSendService> logger)
    {
        _templateRepo = templateRepo;
        _zaloClient = zaloClient;
        _sendLogRepo = sendLogRepo;
        _queueRepo = queueRepo;
        _messageLogRepo = messageLogRepo;
        _jobClient = jobClient;
        _crmDb = crmDb;
        _logger = logger;
    }

    public async Task<ZnsSendResult> SendNowAsync(ZnsSendRequest request, CancellationToken cancellationToken = default)
        => await SendCoreAsync(request, queueId: null, cancellationToken);

    public async Task<ZnsEnqueueResult> EnqueueAsync(ZnsSendRequest request, CancellationToken cancellationToken = default)
    {
        var template = await _templateRepo.GetByTemplateIdAsync(request.TemplateId);
        if (template is null)
            return new ZnsEnqueueResult { Success = false, Message = "Template not found" };

        var recipients = await GetCampaignRecipientsAsync(request, cancellationToken);
        if (recipients.Count == 0)
            return new ZnsEnqueueResult { Success = false, Message = "No recipients found for campaign" };

        var items = new List<ZnsEnqueueItemResult>(recipients.Count);
        foreach (var recipient in recipients)
        {
            var resolvedData = await BuildTemplateDataAsync(template, request, recipient, cancellationToken);
            var queue = await _queueRepo.AddAsync(new ZnsSendQueue
            {
                TemplateId = request.TemplateId,
                Phone = recipient.Phone,
                TemplateDataJson = JsonSerializer.Serialize(resolvedData),
                Status = ZnsSendStatus.Queued,
                ScheduledAt = DateTime.UtcNow.AddHours(7),
                CampaignId = request.CampaignId,
                EventCatId = recipient.EventCatId,
                EventId = recipient.EventId,
                ContextType = request.ContextType,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow.AddHours(7),
                UpdatedAt = DateTime.UtcNow.AddHours(7)
            });

            var jobId = _jobClient.Enqueue<ZnsSendJob>(x => x.ExecuteAsync(queue.Id, CancellationToken.None));
            items.Add(new ZnsEnqueueItemResult
            {
                QueueId = queue.Id,
                JobId = jobId,
                Phone = recipient.Phone,
                TrackingId = recipient.TrackingId
            });

            _logger.LogInformation("ZNS enqueued queueId={QueueId}, jobId={JobId}, templateId={TemplateId}, phone={Phone}, campaignId={CampaignId}",
                queue.Id, jobId, request.TemplateId, MaskPhone(recipient.Phone), request.CampaignId);
        }

        return new ZnsEnqueueResult
        {
            Success = true,
            Message = "ZNS queued successfully",
            TotalRecipients = items.Count,
            Items = items
        };
    }

    public async Task<ZnsSendResult> SendFromQueueAsync(long queueId, CancellationToken cancellationToken = default)
    {
        var queue = await _queueRepo.GetByIdAsync(queueId);
        if (queue is null)
            return new ZnsSendResult { Success = false, Message = "Queue not found", ErrorCode = -1 };

        if (queue.Status == ZnsSendStatus.Sent)
            return new ZnsSendResult { Success = true, Message = "Already sent", MsgId = queue.MsgId, QueueId = queue.Id };

        queue.Status = ZnsSendStatus.Processing;
        queue.StartedAt = DateTime.UtcNow.AddHours(7);
        queue.UpdatedAt = DateTime.UtcNow.AddHours(7);
        await _queueRepo.UpdateAsync(queue);

        try
        {
            var data = JsonSerializer.Deserialize<Dictionary<string, object?>>(queue.TemplateDataJson)
                       ?? new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            var request = new ZnsSendRequest
            {
                TemplateId = queue.TemplateId,
                CampaignId = queue.CampaignId ?? 0,
                Phone = queue.Phone,
                TemplateData = data,
                Type = queue.Type,
                EventCatId = queue.EventCatId,
                EventId = queue.EventId,
                ContextType = queue.ContextType,
                CreatedBy = queue.CreatedBy
            };

            var result = await SendCoreAsync(request, queue.Id, cancellationToken);

            queue.Status = result.Success ? ZnsSendStatus.Sent : ZnsSendStatus.Failed;
            queue.CompletedAt = DateTime.UtcNow.AddHours(7);
            queue.ErrorCode = result.Success ? null : result.ErrorCode;
            queue.ErrorMessage = result.Success ? null : result.Message;
            queue.MsgId = result.MsgId;
            queue.UpdatedAt = DateTime.UtcNow.AddHours(7);
            await _queueRepo.UpdateAsync(queue);

            return result;
        }
        catch (Exception ex)
        {
            queue.RetryCount += 1;
            queue.Status = queue.RetryCount >= 3 ? ZnsSendStatus.Failed : ZnsSendStatus.Retry;
            queue.ErrorMessage = ex.Message;
            queue.UpdatedAt = DateTime.UtcNow.AddHours(7);
            await _queueRepo.UpdateAsync(queue);
            _logger.LogError(ex, "ZNS queue processing exception queueId={QueueId}", queueId);
            throw;
        }
    }

    private async Task<ZnsSendResult> SendCoreAsync(ZnsSendRequest request, long? queueId, CancellationToken cancellationToken)
    {
        if (!IsValidPhone(request.Phone))
            return new ZnsSendResult { Success = false, ErrorCode = -108, Message = "Phone number invalid", QueueId = queueId };

        var template = await _templateRepo.GetByTemplateIdAsync(request.TemplateId);
        if (template is null)
            return new ZnsSendResult { Success = false, ErrorCode = -404, Message = "Template not found", QueueId = queueId };

        if (!template.IsActive || !string.Equals(template.Status, "ENABLE", StringComparison.OrdinalIgnoreCase))
            return new ZnsSendResult { Success = false, ErrorCode = -400, Message = "Template is disabled", QueueId = queueId };

        var normalizedData = new Dictionary<string, object?>(request.TemplateData, StringComparer.OrdinalIgnoreCase);
        var validationError = ValidateTemplateData(template, normalizedData);
        if (validationError is not null)
            return new ZnsSendResult { Success = false, ErrorCode = -1121, Message = validationError, QueueId = queueId };

        var trackingId = string.IsNullOrWhiteSpace(request.TrackingId) ? Guid.NewGuid().ToString("N") : request.TrackingId!;
        normalizedData["tracking_id"] = trackingId;

        var requestJson = JsonSerializer.Serialize(new
        {
            phone = request.Phone,
            template_id = request.TemplateId,
            template_data = normalizedData,
            tracking_id = trackingId
        });

        var sendLog = await _sendLogRepo.AddAsync(new ZnsSendLog
        {
            ZnsTemplateId = template.Id,
            ZaloTemplateId = template.TemplateId,
            Phone = request.Phone!,
            ParamsJson = JsonSerializer.Serialize(normalizedData),
            RequestJson = requestJson,
            Status = ZnsSendStatus.Processing,
            Type = request.Type,
            CampaignId = request.CampaignId,
            EventCatId = request.EventCatId,
            EventId = request.EventId,
            ContextType = request.ContextType,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow.AddHours(7),
            UpdatedAt = DateTime.UtcNow.AddHours(7)
        });

        var fullName = ExtractFullName(normalizedData);
        var messageLog = await _messageLogRepo.AddAsync(new Zalo_Message_Log
        {
            Phone = request.Phone!,
            FullName = fullName,
            TemplateId = request.TemplateId,
            TrackingId = trackingId,
            Status = 0,
            Message = string.Empty,
            RequestJson = requestJson,
            ResponseJson = string.Empty,
            CreatedTime = DateTime.Now
        });

        ZaloApiEnvelope<ZaloSendResponseData>? envelope = null;
        try
        {
            envelope = await _zaloClient.SendMessageAsync(request.TemplateId, request.Phone!, normalizedData, trackingId, cancellationToken);
        }
        catch (Exception ex)
        {
            sendLog.Status = ZnsSendStatus.Failed;
            sendLog.ErrorCode = -500;
            sendLog.ErrorMessage = ex.Message;
            sendLog.ResponseJson = JsonSerializer.Serialize(new { exception = ex.Message });
            sendLog.UpdatedAt = DateTime.UtcNow.AddHours(7);
            await _sendLogRepo.UpdateAsync(sendLog);

            messageLog.Status = -500;
            messageLog.Message = ex.Message;
            messageLog.ResponseJson = JsonSerializer.Serialize(new { exception = ex.Message });
            messageLog.CreatedTime = DateTime.Now;
            await _messageLogRepo.UpdateAsync(messageLog);

            return new ZnsSendResult { Success = false, ErrorCode = -500, Message = ex.Message, QueueId = queueId };
        }

        sendLog.ResponseJson = JsonSerializer.Serialize(envelope);
        messageLog.ResponseJson = sendLog.ResponseJson;

        if (envelope is not null && envelope.Error == 0)
        {
            sendLog.Status = ZnsSendStatus.Sent;
            sendLog.ZaloMessageId = envelope.Data?.MsgId;
            sendLog.SendingMode = envelope.Data?.SendingMode;
            sendLog.SentTime = ParseDateTimeFromMs(envelope.Data?.SentTime) ?? DateTime.UtcNow.AddHours(7);
            sendLog.RemainingQuota = ParseInt(envelope.Data?.Quota?.RemainingQuota);
            sendLog.DailyQuota = ParseInt(envelope.Data?.Quota?.DailyQuota);
            sendLog.UpdatedAt = DateTime.UtcNow.AddHours(7);
            await _sendLogRepo.UpdateAsync(sendLog);

            messageLog.Status = 1;
            messageLog.Message = envelope.Message;
            await _messageLogRepo.UpdateAsync(messageLog);

            _logger.LogInformation("ZNS sent success templateId={TemplateId}, phone={Phone}, msgId={MsgId}, queueId={QueueId}",
                request.TemplateId, MaskPhone(request.Phone), sendLog.ZaloMessageId, queueId);

            return new ZnsSendResult
            {
                Success = true,
                Message = "ZNS sent successfully",
                MsgId = sendLog.ZaloMessageId,
                SentTime = envelope.Data?.SentTime,
                SendingMode = envelope.Data?.SendingMode,
                RemainingQuota = sendLog.RemainingQuota,
                DailyQuota = sendLog.DailyQuota,
                QueueId = queueId
            };
        }

        sendLog.Status = ZnsSendStatus.Failed;
        sendLog.ErrorCode = envelope?.Error;
        sendLog.ErrorMessage = envelope?.Message;
        sendLog.UpdatedAt = DateTime.UtcNow.AddHours(7);
        await _sendLogRepo.UpdateAsync(sendLog);

        messageLog.Status = envelope?.Error ?? -1;
        messageLog.Message = envelope?.Message ?? "Unknown ZNS error";
        await _messageLogRepo.UpdateAsync(messageLog);

        _logger.LogWarning("ZNS sent failed templateId={TemplateId}, phone={Phone}, errorCode={ErrorCode}, queueId={QueueId}, message={Message}",
            request.TemplateId, MaskPhone(request.Phone), envelope?.Error, queueId, envelope?.Message);

        return new ZnsSendResult
        {
            Success = false,
            ErrorCode = envelope?.Error ?? -1,
            Message = envelope?.Message ?? "Unknown ZNS error",
            QueueId = queueId
        };
    }

    private async Task<List<CampaignRecipient>> GetCampaignRecipientsAsync(ZnsSendRequest request, CancellationToken cancellationToken)
    {
        var query = _crmDb.Set<Marketing_Zalo_ListSdt>()
            .AsNoTracking()
            .Where(x => x.Marketing_Zalo_CampaignId == request.CampaignId);

        var phones = await query
            .OrderBy(x => x.Id)
            .Select(x => x.Phone)
            .Where(x => x != null && x != string.Empty)
            .Distinct()
            .ToListAsync(cancellationToken);

        var result = new List<CampaignRecipient>(phones.Count);
        foreach (var phone in phones)
        {
            result.Add(await BuildRecipientAsync(phone, request, cancellationToken));
        }

        return result;
    }

    private async Task<CampaignRecipient> BuildRecipientAsync(string phone, ZnsSendRequest request, CancellationToken cancellationToken)
    {
        var normalizedPhone = phone.Trim();
        var student = await FindStudentAsync(normalizedPhone, cancellationToken);
        var eventInfo = await GetEventInfoAsync(request, cancellationToken);
        var trackingId = !string.IsNullOrWhiteSpace(request.TrackingId)
            ? request.TrackingId!
            : BuildTrackingId(eventInfo.EventId, student?.Id, normalizedPhone);

        return new CampaignRecipient
        {
            Phone = normalizedPhone,
            TrackingId = trackingId,
            StudentCode = student?.Code,
            StudentFullName = student is null ? null : $"{student.Hotendem} {student.Ten}".Trim(),
            EventName = eventInfo.EventName,
            EventLocation = eventInfo.EventLocation,
            EventTime = eventInfo.EventTime,
            EventCatName = eventInfo.EventCatName,
            EventCatDescription = eventInfo.EventCatDescription,
            Qr = request.TemplateId == 627608
                ? BuildCheckinQr(student?.Code)
                : student?.Code,
            EventId = eventInfo.EventId,
            EventCatId = eventInfo.EventCatId,
            CampaignId = request.CampaignId
        };
    }

    private static string BuildCheckinQr(string? studentCode)
    {
        var code = string.IsNullOrWhiteSpace(studentCode) ? "NA" : studentCode.Trim();
        return $"http://crm.capstonevietnam.com/quantri/partner/checkin-eventm.html?studentcode={code}";
    }

    private async Task<Dictionary<string, object?>> BuildTemplateDataAsync(ZnsTemplate template, ZnsSendRequest request, CampaignRecipient recipient, CancellationToken cancellationToken)
    {
        var data = new Dictionary<string, object?>(request.TemplateData, StringComparer.OrdinalIgnoreCase);
        var sourceValues = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["phone"] = recipient.Phone,
            ["tracking_id"] = recipient.TrackingId,
            ["student_code"] = recipient.StudentCode ?? recipient.Phone,
            ["student_fullname"] = recipient.StudentFullName ?? "Có số điện thoại " + recipient.Phone,
            ["event_name"] = recipient.EventName ?? "NA",
            ["event_location"] = recipient.EventLocation ?? "NA",
            ["event_time"] = recipient.EventTime ?? "NA",
            ["time_date_list"] = recipient.EventTime ?? "NA",
            ["event_cat_name"] = recipient.EventCatName ?? "NA",
            ["event_cat_description"] = recipient.EventCatDescription ?? "NA",
            ["qr"] = recipient.Qr ?? "NA",
            ["campaign_id"] = recipient.CampaignId,
            ["event_cat_id"] = recipient.EventCatId,
            ["event_id"] = recipient.EventId
        };

        foreach (var param in template.Params.OrderBy(x => x.SortOrder))
        {
            if (data.ContainsKey(param.ParamName))
                continue;

            data[param.ParamName] = ResolveTemplateValue(param.ParamName, sourceValues);
        }

        data["tracking_id"] = recipient.TrackingId;
        data["phone"] = recipient.Phone;
        data["student_code"] = recipient.StudentCode ?? recipient.Phone;
        data["student_fullname"] = recipient.StudentFullName ?? "Có số điện thoại " + recipient.Phone;
        data["event_name"] = recipient.EventName ?? "NA";
        data["event_location"] = recipient.EventLocation ?? "NA";
        data["event_time"] = recipient.EventTime ?? "NA";
        data["time_date_list"] = recipient.EventTime ?? "NA";
        data["event_cat_name"] = recipient.EventCatName ?? "NA";
        data["event_cat_description"] = recipient.EventCatDescription ?? "NA";
        data["qr"] = recipient.Qr ?? "NA";

        return data;
    }

    private static object? ResolveTemplateValue(string paramName, IReadOnlyDictionary<string, object?> values)
    {
        var key = paramName.Trim().ToLowerInvariant();
        return key switch
        {
            "phone" => values["phone"],
            "tracking_id" => values["tracking_id"],
            "student_code" => values["student_code"],
            "student_fullname" => values["student_fullname"],
            "event_name" => values["event_name"],
            "event_location" => values["event_location"],
            "event_time" => values["event_time"],
            "time_date_list" => values["time_date_list"],
            "event_cat_name" => values["event_cat_name"],
            "event_cat_description" => values["event_cat_description"],
            "qr" => values["qr"],
            _ => null
        };
    }

    private async Task<(int? EventId, int? EventCatId, string? EventName, string? EventLocation, string? EventTime, string? EventCatName, string? EventCatDescription)> GetEventInfoAsync(ZnsSendRequest request, CancellationToken cancellationToken)
    {
        var eventQuery = _crmDb.Set<NV_Event>().AsNoTracking().Where(x => x.Isactive == true);
        var eventRow = request.EventId.HasValue
            ? await eventQuery.FirstOrDefaultAsync(x => x.Id == request.EventId.Value, cancellationToken)
            : await eventQuery.OrderByDescending(x => x.Id).FirstOrDefaultAsync(cancellationToken);

        if (eventRow is null)
            return (request.EventId, request.EventCatId, null, null, null, null, null);

        var catId = request.EventCatId ?? eventRow.CatId;
        var catRow = catId.HasValue
            ? await _crmDb.Set<NV_Events_Cat>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == catId.Value, cancellationToken)
            : null;

        var eventName = eventRow.Title ?? catRow?.CatName;
        var eventLocation = eventRow.Diadiem ?? catRow?.FairOrg;
        var eventTime = eventRow.Fromdatetime?.ToString("HH:mm dd/MM/yyyy") ?? catRow?.DateShow;
        var eventCatName = catRow?.CatName;
        var eventCatDescription = Truncate(catRow?.sendzalo_content ?? string.Empty, 200);

        return (eventRow.Id, catId, eventName, eventLocation, eventTime, eventCatName, eventCatDescription);
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
            return value;

        return value[..maxLength];
    }

    private async Task<Student_Info?> FindStudentAsync(string phone, CancellationToken cancellationToken)
    {
        var normalized = phone.Replace(" ", string.Empty);
        return await _crmDb.Set<Student_Info>()
            .AsNoTracking()
            .Where(x => x.Sodienthoai != null)
            .FirstOrDefaultAsync(x => x.Sodienthoai == normalized || x.Sodienthoai!.Replace(" ", string.Empty) == normalized, cancellationToken);
    }

    private static string BuildTrackingId(int? eventId, int? studentId, string phone)
        => $"event_{eventId ?? 0}_student_{studentId ?? 0}_{phone}";

    private static string? ValidateTemplateData(ZnsTemplate template, Dictionary<string, object?> data)
    {
        foreach (var p in template.Params.OrderBy(x => x.SortOrder))
        {
            var hasValue = data.TryGetValue(p.ParamName, out var rawValue);
            if (p.IsRequired && !hasValue)
                return $"Missing required parameter: {p.ParamName}";

            if (!hasValue)
                continue;

            var value = ToValueString(rawValue);
            if (!p.AcceptNull && string.IsNullOrWhiteSpace(value))
                return $"Parameter {p.ParamName} cannot be null or empty";

            if (string.IsNullOrWhiteSpace(value))
                continue;

            if (p.MinLength.HasValue && value.Length < p.MinLength.Value)
                return $"Parameter {p.ParamName} below minLength {p.MinLength.Value}";

            if (p.MaxLength.HasValue && value.Length > p.MaxLength.Value)
                return $"Parameter {p.ParamName} breaks max length {p.MaxLength.Value}";

            var type = (p.ParamType ?? "STRING").Trim().ToUpperInvariant();
            if (type == "DATE" && !IsDateLike(value))
                return $"Parameter {p.ParamName} is invalid DATE format";
        }

        return null;
    }

    private static string ExtractFullName(Dictionary<string, object?> data)
    {
        foreach (var key in new[] { "Fullname", "FullName", "full_name", "student_fullname", "Name" })
        {
            if (data.TryGetValue(key, out var raw) && raw is not null)
                return raw.ToString() ?? string.Empty;
        }
        return string.Empty;
    }

    private static string ToValueString(object? raw)
    {
        if (raw is null) return string.Empty;
        if (raw is JsonElement je)
        {
            return je.ValueKind switch
            {
                JsonValueKind.String => je.GetString() ?? string.Empty,
                JsonValueKind.Number => je.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                JsonValueKind.Null => string.Empty,
                _ => je.GetRawText()
            };
        }

        return raw.ToString() ?? string.Empty;
    }

    private static bool IsDateLike(string value)
    {
        if (DateTime.TryParse(value, out _)) return true;
        if (long.TryParse(value, out var epoch))
        {
            try { _ = DateTimeOffset.FromUnixTimeMilliseconds(epoch); return true; }
            catch { }
        }
        return false;
    }

    private static DateTime? ParseDateTimeFromMs(string? ms)
    {
        if (!long.TryParse(ms, out var v)) return null;
        try { return DateTimeOffset.FromUnixTimeMilliseconds(v).UtcDateTime; }
        catch { return null; }
    }

    private static int? ParseInt(string? s) => int.TryParse(s, out var x) ? x : null;

    private static bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone)) return false;
        if (!phone.StartsWith("84")) return false;
        return phone.All(char.IsDigit) && phone.Length is >= 10 and <= 13;
    }

    private static string MaskPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone) || phone.Length < 4) return "****";
        return new string('*', Math.Max(0, phone.Length - 4)) + phone[^4..];
    }

    private sealed class CampaignRecipient
    {
        public string Phone { get; set; } = string.Empty;
        public string TrackingId { get; set; } = string.Empty;
        public string? StudentCode { get; set; }
        public string? StudentFullName { get; set; }
        public string? EventName { get; set; }
        public string? EventLocation { get; set; }
        public string? EventTime { get; set; }
        public string? EventCatName { get; set; }
        public string? EventCatDescription { get; set; }
        public string? Qr { get; set; }
        public int? EventId { get; set; }
        public int? EventCatId { get; set; }
        public int CampaignId { get; set; }
    }
}
