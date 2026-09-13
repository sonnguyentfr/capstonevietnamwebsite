using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public class ZnsSendLogRepository : IZnsSendLogRepository
{
    private readonly CRMDbContext _db;

    public ZnsSendLogRepository(CRMDbContext db)
    {
        _db = db;
    }

    public async Task<ZnsSendLog> AddAsync(ZnsSendLog entity)
    {
        _db.ZnsSendLogs.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(ZnsSendLog entity)
    {
        _db.ZnsSendLogs.Update(entity);
        await _db.SaveChangesAsync();
    }
}
