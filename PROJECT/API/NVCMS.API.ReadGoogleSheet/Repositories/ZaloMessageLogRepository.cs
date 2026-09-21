using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public class ZaloMessageLogRepository : IZaloMessageLogRepository
{
    private readonly CRMDbContext _db;

    public ZaloMessageLogRepository(CRMDbContext db)
    {
        _db = db;
    }

    public async Task<Zalo_Message_Log> AddAsync(Zalo_Message_Log entity)
    {
        _db.Set<Zalo_Message_Log>().Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(Zalo_Message_Log entity)
    {
        _db.Set<Zalo_Message_Log>().Update(entity);
        await _db.SaveChangesAsync();
    }
}
