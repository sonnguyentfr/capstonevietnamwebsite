using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class CrmDataLadingRepository : MarketingRepository<student_from_ladipage>, ICrmDataLadingRepository
    {
        public CrmDataLadingRepository(CRMDbContext context) : base(context) { }

        public async Task<IEnumerable<student_from_ladipage>> GetByEmailOrPhoneAsync(string emails, string phones)
        {
            // Initialize the enumerable
            //IEnumerable<student_from_ladipage> lstdata = Enumerable.Empty<student_from_ladipage>();
            var lstdata = await _context.CrmDataLadings
                .AsNoTracking()
                .Where(x =>
                    (x.email != null && x.email.ToLower() == emails.ToLower()) ||
                    (x.so_dien_thoai != null && x.so_dien_thoai == phones.ToLower()))
                .ToListAsync();

            return lstdata;
        }

        public async Task<List<(student_from_ladipage Record, Exception Error)>> AddRangeSkipFailuresAsync(
            IReadOnlyList<student_from_ladipage> records)
        {
            var failed = new List<(student_from_ladipage, Exception)>();
            if (records.Count == 0) return failed;

            // 1. Thử cả lô (nhanh). SaveChanges chạy trong transaction nên lỗi thì rollback hết.
            try
            {
                await _dbSet.AddRangeAsync(records);
                await _context.SaveChangesAsync();
                return failed;
            }
            catch (DbUpdateException)
            {
                _context.ChangeTracker.Clear();
            }

            // 2. Lô lỗi → insert từng dòng, dòng lỗi thì bỏ qua để các dòng khác vẫn vào
            foreach (var record in records)
            {
                record.id = 0;   // xoá id tạm EF gán ở lần thử cả lô (cột IDENTITY)
                _dbSet.Add(record);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    _context.Entry(record).State = EntityState.Detached;
                    failed.Add((record, ex));
                }
            }

            return failed;
        }
    }
}