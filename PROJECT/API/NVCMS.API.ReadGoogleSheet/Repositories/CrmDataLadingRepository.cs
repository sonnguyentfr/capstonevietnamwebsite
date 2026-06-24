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
    }
}