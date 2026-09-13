using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Models;

namespace NVCMS.API.ReadGoogleSheet.Repositories;

public class ZnsSendQueueRepository : IZnsSendQueueRepository
{
    private readonly CRMDbContext _db;

    public ZnsSendQueueRepository(CRMDbContext db)
    {
        _db = db;
    }

    public async Task<ZnsSendQueue> AddAsync(ZnsSendQueue entity)
    {
        _db.ZnsSendQueues.Add(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public async Task<ZnsSendQueue?> GetByIdAsync(long id)
    {
        return await _db.ZnsSendQueues.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(ZnsSendQueue entity)
    {
        _db.ZnsSendQueues.Update(entity);
        await _db.SaveChangesAsync();
    }
}
