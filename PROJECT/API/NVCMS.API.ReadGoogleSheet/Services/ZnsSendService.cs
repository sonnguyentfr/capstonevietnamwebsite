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
    private readonly IConfiguration _configuration;
    private readonly ILogger<ZnsSendService> _logger;

    /// <summary>Template ZNS có QR check-in: qr = link check-in theo mã học sinh thay vì mã thuần.</summary>
    private const long CheckinQrTemplateId = 627608;

    public ZnsSendService(
        IZnsTemplateRepository templateRepo,
        IZaloZnsClient zaloClient,
        IZnsSendLogRepository sendLogRepo,
        IZnsSendQueueRepository queueRepo,
        IZaloMessageLogRepository messageLogRepo,
        IBackgroundJobClient jobClient,
        CRMDbContext crmDb,
        IConfiguration configuration,
        ILogger<ZnsSendService> logger)
    {
        _configuration = configuration;
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

    /// <summary>
    /// Chỉ validate + đếm SĐT rồi đẩy 1 job nền (ZnsCampaignEnqueueJob) xử lý toàn bộ campaign.
    /// Không build recipient trong HTTP request để tránh timeout với campaign lớn.
    /// </summary>
    public async Task<ZnsEnqueueResult> EnqueueAsync(ZnsSendRequest request, CancellationToken cancellationToken = default)
    {
        var template = await _templateRepo.GetByTemplateIdAsync(request.TemplateId);
        if (template is null)
            return new ZnsEnqueueResult { Success = false, Message = "Template not found" };

        var totalRecipients = await CampaignPhonesQuery(request.CampaignId).CountAsync(cancellationToken);
        if (totalRecipients == 0)
            return new ZnsEnqueueResult { Success = false, Message = "No recipients found for campaign" };

        var args = new ZnsCampaignEnqueueArgs
        {
            TemplateId = request.TemplateId,
            CampaignId = request.CampaignId,
            TemplateDataJson = JsonSerializer.Serialize(request.TemplateData),
            TrackingId = request.TrackingId,
            Type = request.Type,
            EventCatId = request.EventCatId,
            EventId = request.EventId,
            ContextType = request.ContextType,
            CreatedBy = request.CreatedBy,
            RequestedAt = DateTime.UtcNow.AddHours(7)
        };

        var jobId = _jobClient.Enqueue<ZnsCampaignEnqueueJob>(x => x.ExecuteAsync(args, CancellationToken.None));

        _logger.LogInformation("ZNS campaign enqueue job created jobId={JobId}, campaignId={CampaignId}, templateId={TemplateId}, total={Total}",
            jobId, request.CampaignId, request.TemplateId, totalRecipients);

        return new ZnsEnqueueResult
        {
            Success = true,
            Message = "ZNS campaign queued successfully",
            TotalRecipients = totalRecipients,
            Items = [new ZnsEnqueueItemResult { JobId = jobId }]
        };
    }

    /// <summary>
    /// Chạy trong ZnsCampaignEnqueueJob: xử lý SĐT theo lô, mỗi lô 1 query student + 1 lần SaveChanges.
    /// </summary>
    public async Task<int> ProcessCampaignEnqueueAsync(ZnsCampaignEnqueueArgs args, CancellationToken cancellationToken = default)
    {
        const int batchSize = 500;

        var request = new ZnsSendRequest
        {
            TemplateId = args.TemplateId,
            CampaignId = args.CampaignId,
            TemplateData = JsonSerializer.Deserialize<Dictionary<string, object?>>(args.TemplateDataJson)
                           ?? new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase),
            TrackingId = args.TrackingId,
            Type = args.Type,
            EventCatId = args.EventCatId,
            EventId = args.EventId,
            ContextType = args.ContextType,
            CreatedBy = args.CreatedBy
        };

        var template = await _templateRepo.GetByTemplateIdAsync(request.TemplateId);
        if (template is null)
        {
            _logger.LogWarning("ZNS campaign enqueue: template not found templateId={TemplateId}", request.TemplateId);
            return 0;
        }

        // Event info giống nhau cho mọi SĐT → chỉ lấy 1 lần
        var eventInfo = await GetEventInfoAsync(request, cancellationToken);

        var phones = await CampaignPhonesQuery(request.CampaignId).ToListAsync(cancellationToken);
        phones = phones.Select(p => p.Trim()).Where(p => p.Length > 0).Distinct().ToList();

        var total = 0;
        foreach (var chunk in phones.Chunk(batchSize))
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Bỏ SĐT đã enqueue ở lần chạy trước (khi job bị retry giữa chừng)
            var alreadyQueued = await _crmDb.ZnsSendQueues
                .AsNoTracking()
                .Where(x => x.CampaignId == request.CampaignId
                            && x.TemplateId == request.TemplateId
                            && x.CreatedAt >= args.RequestedAt
                            && chunk.Contains(x.Phone))
                .Select(x => x.Phone)
                .ToListAsync(cancellationToken);
            var pending = alreadyQueued.Count == 0
                ? chunk
                : chunk.Except(alreadyQueued).ToArray();
            if (pending.Length == 0)
                continue;

            var students = await FindStudentsAsync(pending, cancellationToken);
            var now = DateTime.UtcNow.AddHours(7);

            var queues = new List<ZnsSendQueue>(pending.Length);
            foreach (var phone in pending)
            {
                students.TryGetValue(phone.Replace(" ", string.Empty), out var student);
                var recipient = BuildRecipient(phone, request, eventInfo, student);
                var resolvedData = await BuildTemplateDataAsync(template, request, recipient, cancellationToken);

                queues.Add(new ZnsSendQueue
                {
                    TemplateId = request.TemplateId,
                    Phone = recipient.Phone,
                    TemplateDataJson = JsonSerializer.Serialize(resolvedData),
                    Status = ZnsSendStatus.Queued,
                    ScheduledAt = now,
                    Type = request.Type,
                    CampaignId = request.CampaignId,
                    EventCatId = recipient.EventCatId,
                    EventId = recipient.EventId,
                    ContextType = request.ContextType,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            await _queueRepo.AddRangeAsync(queues, cancellationToken);

            foreach (var queue in queues)
            {
                var queueId = queue.Id;
                _jobClient.Enqueue<ZnsSendJob>(x => x.ExecuteAsync(queueId, CancellationToken.None));
            }

            total += queues.Count;
            _logger.LogInformation("ZNS campaign enqueue progress campaignId={CampaignId}, {Done}/{Total}",
                request.CampaignId, total, phones.Count);
        }

        return total;
    }

    /// <summary>
    /// Gửi ZNS cho danh sách NV_Events_Student đã chọn (màn hình thống kê sự kiện).
    /// Sàng lọc từng dòng, dòng hợp lệ → 1 ZnsSendQueue + 1 ZnsSendJob; dòng lỗi trả về trong Skipped kèm lý do.
    /// </summary>
    public async Task<ZnsEventStudentEnqueueResult> EnqueueEventStudentsAsync(ZnsEventStudentSendRequest request, CancellationToken cancellationToken = default)
    {
        const string type = "eventStatic";
        const int batchSize = 500;

        var ids = request.Ids.Where(x => x > 0).Distinct().ToList();
        var result = new ZnsEventStudentEnqueueResult { TotalRequested = ids.Count };
        if (ids.Count == 0)
            return Fail(result, "Ids is empty");

        var template = await _templateRepo.GetByTemplateIdAsync(request.TemplateId);
        if (template is null)
            return Fail(result, "Template not found");
        if (!template.IsActive || !string.Equals(template.Status, "ENABLE", StringComparison.OrdinalIgnoreCase))
            return Fail(result, "Template is disabled");

        var eventRow = await _crmDb.NV_Events.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.EventId, cancellationToken);
        if (eventRow is null)
            return Fail(result, $"Event {request.EventId} not found");
        if (eventRow.CatId.HasValue && eventRow.CatId.Value != request.EventCatId)
            return Fail(result, $"Event {request.EventId} does not belong to eventCat {request.EventCatId}");

        var catRow = await _crmDb.NV_EventsCats.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.EventCatId, cancellationToken);
        if (catRow is null)
            return Fail(result, $"EventCat {request.EventCatId} not found");

        var isCheckinTemplate = request.TemplateId == CheckinQrTemplateId;
        var hotline = _configuration["CapstoneInfo:Hotline"] ?? string.Empty;
        var eventName = eventRow.Title ?? catRow.CatName ?? "NA";
        var eventInfo = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
        {
            ["event_name"] = eventName,
            ["event_location"] = eventRow.Diadiem ?? eventName,
            ["event_cat_name"] = catRow.CatName ?? "NA",
            ["event_cat_description"] = Truncate(catRow.sendzalo_content ?? string.Empty, 200),
            ["event_time"] = catRow.FromDate?.ToString("HH:mm dd/MM/yyyy") ?? catRow.DateShow ?? "NA",
            ["event_cat_shortlink"] = catRow.Link_pr ?? string.Empty,
            ["hotline"] = hotline
        };

        var seenPhones = new HashSet<string>();
        foreach (var chunk in ids.Chunk(batchSize))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var eventStudents = await _crmDb.NV_EventsStudents
                .AsNoTracking()
                .Where(x => chunk.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            var studentIds = eventStudents.Values.Where(x => x.StudentId.HasValue).Select(x => x.StudentId!.Value).Distinct().ToList();
            var students = await _crmDb.StudentInfos
                .AsNoTracking()
                .Where(x => studentIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            // Lọc hợp lệ trước, SĐT chuẩn hoá về dạng 84xxx
            var candidates = new List<(int Id, string Phone, Student_Info Student, string? Code)>();
            foreach (var id in chunk)
            {
                if (!eventStudents.TryGetValue(id, out var es))
                {
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = id, Reason = "NV_Events_Student not found" });
                    continue;
                }
                if (es.EventId != request.EventId)
                {
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = id, Reason = $"Belongs to event {es.EventId}, not {request.EventId}" });
                    continue;
                }
                if (!es.StudentId.HasValue || !students.TryGetValue(es.StudentId.Value, out var student))
                {
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = id, Reason = "Student_Info not found" });
                    continue;
                }

                var phone = CopyStudentFromLadiJob.NormalizeVnPhone(student.Sodienthoai);
                if (!IsValidPhone(phone))
                {
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = id, Phone = student.Sodienthoai, Reason = "Phone number invalid" });
                    continue;
                }
                if (!seenPhones.Add(phone))
                {
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = id, Phone = phone, Reason = "Duplicate phone in request" });
                    continue;
                }

                var code = string.IsNullOrWhiteSpace(student.Code) ? es.StudentCode?.Trim() : student.Code.Trim();
                if (isCheckinTemplate && string.IsNullOrWhiteSpace(code))
                {
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = id, Phone = phone, Reason = "Missing student code for check-in QR" });
                    continue;
                }

                candidates.Add((id, phone, student, code));
            }

            if (!request.AllowResend && candidates.Count > 0)
            {
                var phones = candidates.Select(x => x.Phone).ToList();
                var alreadyQueued = (await _crmDb.ZnsSendQueues
                    .AsNoTracking()
                    .Where(x => x.EventId == request.EventId
                                && x.TemplateId == request.TemplateId
                                && phones.Contains(x.Phone)
                                && (x.Status == ZnsSendStatus.Queued || x.Status == ZnsSendStatus.Processing
                                    || x.Status == ZnsSendStatus.Retry || x.Status == ZnsSendStatus.Sent))
                    .Select(x => x.Phone)
                    .Distinct()
                    .ToListAsync(cancellationToken)).ToHashSet();

                foreach (var c in candidates.Where(c => alreadyQueued.Contains(c.Phone)))
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = c.Id, Phone = c.Phone, Reason = "Already queued/sent for this event and template" });
                candidates.RemoveAll(c => alreadyQueued.Contains(c.Phone));
            }

            var now = DateTime.UtcNow.AddHours(7);
            var queues = new List<ZnsSendQueue>(candidates.Count);
            foreach (var c in candidates)
            {
                var data = new Dictionary<string, object?>(eventInfo, StringComparer.OrdinalIgnoreCase)
                {
                    ["student_fullname"] = $"{c.Student.Hotendem} {c.Student.Ten}".Trim() is { Length: > 0 } name ? name : "Có số điện thoại " + c.Phone,
                    ["student_code"] = c.Code ?? c.Phone,
                    ["phone"] = c.Phone,
                    ["qr"] = isCheckinTemplate ? BuildCheckinQr(c.Code) : c.Code ?? "NA"
                };

                // Chỉ gửi đúng các param template khai báo (nếu đã đồng bộ param)
                if (template.Params.Count > 0)
                {
                    var paramNames = template.Params.Select(p => p.ParamName).ToHashSet(StringComparer.OrdinalIgnoreCase);
                    data = data.Where(kv => paramNames.Contains(kv.Key))
                               .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
                }

                var validationError = ValidateTemplateData(template, data);
                if (validationError is not null)
                {
                    result.Skipped.Add(new ZnsEventStudentSkipped { EventStudentId = c.Id, Phone = c.Phone, Reason = validationError });
                    continue;
                }

                queues.Add(new ZnsSendQueue
                {
                    TemplateId = request.TemplateId,
                    Phone = c.Phone,
                    TemplateDataJson = JsonSerializer.Serialize(data),
                    Status = ZnsSendStatus.Queued,
                    ScheduledAt = now,
                    Type = type,
                    CampaignId = 0,
                    EventCatId = request.EventCatId,
                    EventId = request.EventId,
                    ContextType = type,
                    CreatedBy = request.CreatedBy,
                    CreatedAt = now,
                    UpdatedAt = now
                });
            }

            await _queueRepo.AddRangeAsync(queues, cancellationToken);

            foreach (var queue in queues)
            {
                var queueId = queue.Id;
                var jobId = _jobClient.Enqueue<ZnsSendJob>(x => x.ExecuteAsync(queueId, CancellationToken.None));
                result.Items.Add(new ZnsEnqueueItemResult { QueueId = queueId, JobId = jobId, Phone = queue.Phone });
            }
        }

        result.TotalQueued = result.Items.Count;
        result.Success = result.TotalQueued > 0;
        result.Message = result.Success
            ? $"Queued {result.TotalQueued}/{result.TotalRequested}, skipped {result.Skipped.Count}"
            : "No valid recipients to queue";

        _logger.LogInformation("ZNS eventStatic enqueue eventId={EventId}, eventCatId={EventCatId}, templateId={TemplateId}, queued={Queued}, skipped={Skipped}",
            request.EventId, request.EventCatId, request.TemplateId, result.TotalQueued, result.Skipped.Count);

        return result;

        static ZnsEventStudentEnqueueResult Fail(ZnsEventStudentEnqueueResult r, string message)
        {
            r.Success = false;
            r.Message = message;
            return r;
        }
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

    private IQueryable<string> CampaignPhonesQuery(int campaignId)
        => _crmDb.Set<Marketing_Zalo_ListSdt>()
            .AsNoTracking()
            .Where(x => x.Marketing_Zalo_CampaignId == campaignId && x.Phone != null && x.Phone != string.Empty)
            .Select(x => x.Phone!)
            .Distinct();

    private static CampaignRecipient BuildRecipient(
        string phone,
        ZnsSendRequest request,
        (int? EventId, int? EventCatId, string? EventName, string? EventLocation, string? EventTime, string? EventCatName, string? EventCatDescription) eventInfo,
        Student_Info? student)
    {
        var normalizedPhone = phone.Trim();
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
            Qr = request.TemplateId == CheckinQrTemplateId
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

    /// <summary>
    /// Tra student cho cả lô SĐT. Key = SĐT đã bỏ khoảng trắng.
    /// Lần 1 so khớp chính xác (dùng được index); lần 2 chỉ cho số chưa tìm thấy, so khớp sau khi bỏ khoảng trắng.
    /// </summary>
    private async Task<Dictionary<string, Student_Info>> FindStudentsAsync(IReadOnlyCollection<string> phones, CancellationToken cancellationToken)
    {
        var normalized = phones.Select(p => p.Replace(" ", string.Empty)).Distinct().ToList();
        var result = new Dictionary<string, Student_Info>(normalized.Count);

        var exact = await _crmDb.Set<Student_Info>()
            .AsNoTracking()
            .Where(x => x.Sodienthoai != null && normalized.Contains(x.Sodienthoai))
            .ToListAsync(cancellationToken);
        foreach (var s in exact)
            result.TryAdd(s.Sodienthoai!, s);

        var missing = normalized.Where(p => !result.ContainsKey(p)).ToList();
        if (missing.Count == 0)
            return result;

        var fuzzy = await _crmDb.Set<Student_Info>()
            .AsNoTracking()
            .Where(x => x.Sodienthoai != null && x.Sodienthoai.Contains(" ")
                        && missing.Contains(x.Sodienthoai.Replace(" ", string.Empty)))
            .ToListAsync(cancellationToken);
        foreach (var s in fuzzy)
            result.TryAdd(s.Sodienthoai!.Replace(" ", string.Empty), s);

        return result;
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
