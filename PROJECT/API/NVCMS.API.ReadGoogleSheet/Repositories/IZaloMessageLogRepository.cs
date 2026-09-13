using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public interface IZaloMessageLogRepository
{
    Task<Zalo_Message_Log> AddAsync(Zalo_Message_Log entity);
}
