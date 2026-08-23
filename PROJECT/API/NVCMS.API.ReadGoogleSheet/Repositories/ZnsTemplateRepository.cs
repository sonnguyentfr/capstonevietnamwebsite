using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Models;
using System.Text.Json;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public class ZnsTemplateRepository : IZnsTemplateRepository
{
    private readonly ApplicationDbContext _db;

    public ZnsTemplateRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<ZnsTemplate?> GetByTemplateIdAsync(long templateId)
    {
        return await _db.ZnsTemplates
            .Include(x => x.Params)
            .Include(x => x.Buttons)
            .FirstOrDefaultAsync(x => x.TemplateId == templateId);
    }

    public async Task<List<ZnsTemplate>> GetAllAsync(bool onlyActive = false)
    {
        var q = _db.ZnsTemplates
            .Include(x => x.Params)
            .Include(x => x.Buttons)
            .AsQueryable();

        if (onlyActive)
            q = q.Where(x => x.IsActive);

        return await q.OrderByDescending(x => x.UpdatedAt).ToListAsync();
    }

    public async Task<ZnsTemplate> UpsertShallowAsync(ZaloTemplateListItemDto dto)
    {
        var now = DateTime.UtcNow;
        var entity = await _db.ZnsTemplates.FirstOrDefaultAsync(x => x.TemplateId == dto.TemplateId);
        if (entity is null)
        {
            entity = new ZnsTemplate
            {
                TemplateId = dto.TemplateId,
                TemplateName = dto.TemplateName,
                CreatedTime = dto.CreatedTime,
                Status = dto.Status,
                TemplateQuality = dto.TemplateQuality,
                IsActive = string.Equals(dto.Status, "ENABLE", StringComparison.OrdinalIgnoreCase),
                LastSyncedAt = now,
                CreatedAt = now,
                UpdatedAt = now
            };
            _db.ZnsTemplates.Add(entity);
        }
        else
        {
            entity.TemplateName = dto.TemplateName;
            entity.CreatedTime = dto.CreatedTime;
            entity.Status = dto.Status;
            entity.TemplateQuality = dto.TemplateQuality;
            entity.IsActive = string.Equals(dto.Status, "ENABLE", StringComparison.OrdinalIgnoreCase);
            entity.LastSyncedAt = now;
            entity.UpdatedAt = now;
        }

        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task ReplaceDetailAsync(long templateDbId, ZaloTemplateDetailDto detail, string detailJson)
    {
        await using var tx = await _db.Database.BeginTransactionAsync();
        var now = DateTime.UtcNow;

        var entity = await _db.ZnsTemplates.FirstAsync(x => x.Id == templateDbId);
        entity.TemplateName = detail.TemplateName;
        entity.Status = detail.Status;
        entity.TemplateQuality = detail.TemplateQuality;
        entity.TemplateTag = detail.TemplateTag;
        entity.Timeout = detail.Timeout;
        entity.PreviewUrl = detail.PreviewUrl;
        entity.Price = ParseDecimal(detail.Price);
        entity.PriceUid = ParseDecimal(detail.PriceUid);
        entity.PriceSdt = ParseDecimal(detail.PriceSdt);
        entity.ApplyTemplateQuota = detail.ApplyTemplateQuota ?? false;
        entity.Reason = detail.Reason;
        entity.IsActive = string.Equals(detail.Status, "ENABLE", StringComparison.OrdinalIgnoreCase);
        entity.DetailJson = detailJson;
        entity.LastSyncedAt = now;
        entity.UpdatedAt = now;

        var oldParams = _db.ZnsTemplateParams.Where(x => x.ZnsTemplateId == templateDbId);
        var oldButtons = _db.ZnsTemplateButtons.Where(x => x.ZnsTemplateId == templateDbId);
        _db.ZnsTemplateParams.RemoveRange(oldParams);
        _db.ZnsTemplateButtons.RemoveRange(oldButtons);

        var newParams = detail.ListParams.Select((p, i) => new ZnsTemplateParam
        {
            ZnsTemplateId = templateDbId,
            ParamName = p.Name,
            IsRequired = p.Require,
            ParamType = string.IsNullOrWhiteSpace(p.Type) ? "STRING" : p.Type.Trim().ToUpperInvariant(),
            MaxLength = p.MaxLength,
            MinLength = p.MinLength,
            AcceptNull = p.AcceptNull,
            SortOrder = i + 1,
            DisplayName = p.Name,
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();

        var newButtons = detail.ListButtons.Select((b, i) => new ZnsTemplateButton
        {
            ZnsTemplateId = templateDbId,
            ButtonType = b.Type,
            Title = b.Title,
            Content = b.Content,
            SortOrder = i + 1,
            CreatedAt = now,
            UpdatedAt = now
        }).ToList();

        _db.ZnsTemplateParams.AddRange(newParams);
        _db.ZnsTemplateButtons.AddRange(newButtons);

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
    }

    public async Task MarkMissingAsInactiveAsync(IReadOnlyCollection<long> currentTemplateIds)
    {
        var now = DateTime.UtcNow;
        var missing = await _db.ZnsTemplates
            .Where(x => !currentTemplateIds.Contains(x.TemplateId) && x.IsActive)
            .ToListAsync();

        foreach (var item in missing)
        {
            item.IsActive = false;
            item.Status = "MISSING";
            item.LastSyncedAt = now;
            item.UpdatedAt = now;
        }

        if (missing.Count > 0)
            await _db.SaveChangesAsync();
    }

    private static decimal? ParseDecimal(string? s)
        => decimal.TryParse(s, out var d) ? d : null;
}
