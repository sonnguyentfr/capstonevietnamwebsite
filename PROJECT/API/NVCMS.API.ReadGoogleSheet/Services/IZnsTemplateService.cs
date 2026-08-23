using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Services;

public interface IZnsTemplateService
{
    Task<int> SyncTemplatesAsync(CancellationToken cancellationToken = default);
    Task<List<ZnsTemplate>> GetTemplatesAsync(bool onlyActive = true);
    Task<ZnsTemplate?> GetTemplateAsync(long templateId);
}
