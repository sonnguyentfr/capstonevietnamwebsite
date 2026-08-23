using NVCMS.API.ReadGoogleSheet.Repositories;
using NVCMS.API.ReadGoogleSheet.Models;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Services;

public class ZnsTemplateService : IZnsTemplateService
{
    private readonly IZaloZnsClient _client;
    private readonly IZnsTemplateRepository _repo;
    private readonly ILogger<ZnsTemplateService> _logger;

    public ZnsTemplateService(IZaloZnsClient client, IZnsTemplateRepository repo, ILogger<ZnsTemplateService> logger)
    {
        _client = client;
        _repo = repo;
        _logger = logger;
    }

    public async Task<int> SyncTemplatesAsync(CancellationToken cancellationToken = default)
    {
        var listEnvelope = await _client.GetTemplateListAsync(cancellationToken);
        if (listEnvelope.Error != 0 || listEnvelope.Data is null)
            throw new InvalidOperationException($"Zalo list template failed: {listEnvelope.Error} - {listEnvelope.Message}");

        var list = listEnvelope.Data;
        var currentIds = list.Select(x => x.TemplateId).ToHashSet();
        var changedCount = 0;

        foreach (var item in list)
        {
            var existed = await _repo.GetByTemplateIdAsync(item.TemplateId);
            var needDetail = existed is null
                || !string.Equals(existed.TemplateName, item.TemplateName, StringComparison.Ordinal)
                || existed.CreatedTime != item.CreatedTime
                || !string.Equals(existed.Status, item.Status, StringComparison.Ordinal)
                || !string.Equals(existed.TemplateQuality, item.TemplateQuality, StringComparison.Ordinal);

            var shallow = await _repo.UpsertShallowAsync(item);

            if (!needDetail)
                continue;

            var detailEnvelope = await _client.GetTemplateDetailAsync(item.TemplateId, cancellationToken);
            if (detailEnvelope.Error != 0 || detailEnvelope.Data is null)
            {
                _logger.LogWarning("Template detail sync failed for templateId={TemplateId}, error={Error}, message={Message}",
                    item.TemplateId, detailEnvelope.Error, detailEnvelope.Message);
                continue;
            }

            var detailJson = JsonSerializer.Serialize(detailEnvelope.Data);
            await _repo.ReplaceDetailAsync(shallow.Id, detailEnvelope.Data, detailJson);
            changedCount++;
        }

        await _repo.MarkMissingAsInactiveAsync(currentIds.ToList());
        _logger.LogInformation("ZNS template sync done. total={Total}, changed={Changed}", list.Count, changedCount);

        return changedCount;
    }

    public Task<List<ZnsTemplate>> GetTemplatesAsync(bool onlyActive = true)
        => _repo.GetAllAsync(onlyActive);

    public Task<ZnsTemplate?> GetTemplateAsync(long templateId)
        => _repo.GetByTemplateIdAsync(templateId);
}
