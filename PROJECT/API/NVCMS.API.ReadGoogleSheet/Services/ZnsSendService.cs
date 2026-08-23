using Hangfire;
using NVCMS.API.ReadGoogleSheet.Common;
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
    private readonly IBackgroundJobClient _jobClient;
    private readonly ILogger<ZnsSendService> _logger;

    public ZnsSendService(
        IZnsTemplateRepository templateRepo,
        IZaloZnsClient zaloClient,
        IZnsSendLogRepository sendLogRepo,
        IZnsSendQueueRepository queueRepo,
        IBackgroundJobClient jobClient,
        ILogger<ZnsSendService> logger)
    {
        _templateRepo = templateRepo;
        _zaloClient = zaloClient;
        _sendLogRepo = sendLogRepo;
        _queueRepo = queueRepo;
        _jobClient = jobClient;
        _logger = logger;
    }

    public async Task<ZnsSendResult> SendNowAsync(ZnsSendRequest request, CancellationToken cancellationToken = default)
    {
        return await SendCoreAsync(request, queueId: null, cancellationToken);
    }

    public async Task<(long queueId, string jobId)> EnqueueAsync(ZnsSendRequest request, CancellationToken cancellationToken = default)
    {
        var queue = new ZnsSendQueue
        {
            TemplateId = request.TemplateId,
            Phone = request.Phone,
            TemplateDataJson = JsonSerializer.Serialize(request.TemplateData),
            Status = ZnsSendStatus.Queued,
            ScheduledAt = DateTime.UtcNow,
            CampaignId = request.CampaignId,
            EventCatId = request.EventCatId,
            EventId = request.EventId,
            ContextType = request.ContextType,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        queue = await _queueRepo.AddAsync(queue);

        var jobId = _jobClient.Enqueue<ZnsSendJob>(x => x.ExecuteAsync(queue.Id, CancellationToken.None));

        _logger.LogInformation("ZNS enqueued queueId={QueueId}, jobId={JobId}, templateId={TemplateId}, phone={Phone}",
            queue.Id, jobId, request.TemplateId, MaskPhone(request.Phone));

        return (queue.Id, jobId);
    }

    public async Task<ZnsSendResult> SendFromQueueAsync(long queueId, CancellationToken cancellationToken = default)
    {
        var queue = await _queueRepo.GetByIdAsync(queueId);
        if (queue is null)
            return new ZnsSendResult { Success = false, Message = "Queue not found", ErrorCode = -1 };

        if (queue.Status == ZnsSendStatus.Sent)
        {
            return new ZnsSendResult
            {
                Success = true,
                Message = "Already sent",
                MsgId = queue.MsgId,
                QueueId = queue.Id
            };
        }

        queue.Status = ZnsSendStatus.Processing;
        queue.StartedAt = DateTime.UtcNow;
        queue.UpdatedAt = DateTime.UtcNow;
        await _queueRepo.UpdateAsync(queue);

        try
        {
            var data = JsonSerializer.Deserialize<Dictionary<string, object?>>(queue.TemplateDataJson)
                       ?? new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);

            var request = new ZnsSendRequest
            {
                TemplateId = queue.TemplateId,
                Phone = queue.Phone,
                TemplateData = data,
                CampaignId = queue.CampaignId,
                EventCatId = queue.EventCatId,
                EventId = queue.EventId,
                ContextType = queue.ContextType,
                CreatedBy = queue.CreatedBy
            };

            var result = await SendCoreAsync(request, queue.Id, cancellationToken);

            queue.Status = result.Success ? ZnsSendStatus.Sent : ZnsSendStatus.Failed;
            queue.CompletedAt = DateTime.UtcNow;
            queue.ErrorCode = result.Success ? null : result.ErrorCode;
            queue.ErrorMessage = result.Success ? null : result.Message;
            queue.MsgId = result.MsgId;
            queue.UpdatedAt = DateTime.UtcNow;
            await _queueRepo.UpdateAsync(queue);

            return result;
        }
        catch (Exception ex)
        {
            queue.RetryCount += 1;
            queue.Status = queue.RetryCount >= 3 ? ZnsSendStatus.Failed : ZnsSendStatus.Retry;
            queue.ErrorMessage = ex.Message;
            queue.UpdatedAt = DateTime.UtcNow;
            await _queueRepo.UpdateAsync(queue);

            _logger.LogError(ex, "ZNS queue processing exception queueId={QueueId}", queueId);
            throw;
        }
    }

    private async Task<ZnsSendResult> SendCoreAsync(ZnsSendRequest request, long? queueId, CancellationToken cancellationToken)
    {
        var started = DateTime.UtcNow;

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

        var trackingId = Guid.NewGuid().ToString("N");
        var requestJson = JsonSerializer.Serialize(new
        {
            templateId = request.TemplateId,
            phone = request.Phone,
            templateData = normalizedData,
            trackingId
        });

        var sendLog = await _sendLogRepo.AddAsync(new ZnsSendLog
        {
            ZnsTemplateId = template.Id,
            ZaloTemplateId = template.TemplateId,
            Phone = request.Phone,
            ParamsJson = JsonSerializer.Serialize(normalizedData),
            RequestJson = requestJson,
            Status = ZnsSendStatus.Processing,
            CampaignId = request.CampaignId,
            EventCatId = request.EventCatId,
            EventId = request.EventId,
            ContextType = request.ContextType,
            CreatedBy = request.CreatedBy,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        });

        var envelope = await _zaloClient.SendMessageAsync(request.TemplateId, request.Phone, normalizedData, trackingId, cancellationToken);
        sendLog.ResponseJson = JsonSerializer.Serialize(envelope);

        if (envelope.Error == 0)
        {
            sendLog.Status = ZnsSendStatus.Sent;
            sendLog.ZaloMessageId = envelope.Data?.MsgId;
            sendLog.SendingMode = envelope.Data?.SendingMode;
            sendLog.SentTime = ParseDateTimeFromMs(envelope.Data?.SentTime) ?? DateTime.UtcNow;
            sendLog.RemainingQuota = ParseInt(envelope.Data?.Quota?.RemainingQuota);
            sendLog.DailyQuota = ParseInt(envelope.Data?.Quota?.DailyQuota);
            sendLog.UpdatedAt = DateTime.UtcNow;
            await _sendLogRepo.UpdateAsync(sendLog);

            var duration = (DateTime.UtcNow - started).TotalMilliseconds;
            _logger.LogInformation("ZNS sent success templateId={TemplateId}, phone={Phone}, msgId={MsgId}, queueId={QueueId}, durationMs={Duration}",
                request.TemplateId, MaskPhone(request.Phone), sendLog.ZaloMessageId, queueId, duration);

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
        sendLog.ErrorCode = envelope.Error;
        sendLog.ErrorMessage = envelope.Message;
        sendLog.UpdatedAt = DateTime.UtcNow;
        await _sendLogRepo.UpdateAsync(sendLog);

        _logger.LogWarning("ZNS sent failed templateId={TemplateId}, phone={Phone}, errorCode={ErrorCode}, queueId={QueueId}, message={Message}",
            request.TemplateId, MaskPhone(request.Phone), envelope.Error, queueId, envelope.Message);

        return new ZnsSendResult
        {
            Success = false,
            ErrorCode = envelope.Error,
            Message = envelope.Message,
            QueueId = queueId
        };
    }

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
        if (DateTime.TryParse(value, out _))
            return true;

        if (long.TryParse(value, out var epoch))
        {
            try
            {
                _ = DateTimeOffset.FromUnixTimeMilliseconds(epoch);
                return true;
            }
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
}
