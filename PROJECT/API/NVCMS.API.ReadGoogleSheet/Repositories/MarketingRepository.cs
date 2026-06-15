using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using System.Linq.Expressions;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    /// <summary>
    /// Generic repository base dùng MarketingDbContext (DefaultCRMConnection).
    /// Tách biệt với Repository&lt;T&gt; để không ảnh hưởng code hiện tại.
    /// </summary>
    public class MarketingRepository<T> : IRepository<T> where T : class
    {
        protected readonly MarketingDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public MarketingRepository(MarketingDbContext context)
        {
            _context = context;
            _dbSet   = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);
    }
}
