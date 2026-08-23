using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public interface IZnsTemplateRepository
{
    Task<ZnsTemplate?> GetByTemplateIdAsync(long templateId);
    Task<List<ZnsTemplate>> GetAllAsync(bool onlyActive = false);
    Task<ZnsTemplate> UpsertShallowAsync(ZaloTemplateListItemDto dto);
    Task ReplaceDetailAsync(long templateDbId, ZaloTemplateDetailDto detail, string detailJson);
    Task MarkMissingAsInactiveAsync(IReadOnlyCollection<long> currentTemplateIds);
}
