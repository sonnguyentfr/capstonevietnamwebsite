using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public interface IZnsSendLogRepository
{
    Task<ZnsSendLog> AddAsync(ZnsSendLog entity);
    Task UpdateAsync(ZnsSendLog entity);
}
