using Microsoft.EntityFrameworkCore;
using NVCMS.API.ReadGoogleSheet.Data;
using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public class MailAccountRepository : IMailAccountRepository
    {
        private readonly CRMDbContext _context;

        public MailAccountRepository(CRMDbContext context)
        {
            _context = context;
        }

        public Task<Marketing_Mail_Account?> GetByIdAsync(int id) =>
            _context.MailAccounts.FirstOrDefaultAsync(x => x.Id == id);
    }
}
