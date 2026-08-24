using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public interface IZnsSendQueueRepository
{
    Task<ZnsSendQueue> AddAsync(ZnsSendQueue entity);
    Task<ZnsSendQueue?> GetByIdAsync(long id);
    Task UpdateAsync(ZnsSendQueue entity);
}
