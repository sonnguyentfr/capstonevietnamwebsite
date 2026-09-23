using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public interface IZnsSendQueueRepository
{
    Task<ZnsSendQueue> AddAsync(ZnsSendQueue entity);
    Task AddRangeAsync(IReadOnlyCollection<ZnsSendQueue> entities, CancellationToken cancellationToken = default);
    Task<ZnsSendQueue?> GetByIdAsync(long id);
    Task UpdateAsync(ZnsSendQueue entity);
}
