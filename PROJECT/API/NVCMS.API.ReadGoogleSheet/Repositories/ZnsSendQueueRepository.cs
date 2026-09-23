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

    public async Task AddRangeAsync(IReadOnlyCollection<ZnsSendQueue> entities, CancellationToken cancellationToken = default)
    {
        if (entities.Count == 0) return;
        _db.ZnsSendQueues.AddRange(entities);
        await _db.SaveChangesAsync(cancellationToken);
        // Giải phóng entity đã lưu để DbContext không phình bộ nhớ khi chạy nhiều lô
        _db.ChangeTracker.Clear();
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
