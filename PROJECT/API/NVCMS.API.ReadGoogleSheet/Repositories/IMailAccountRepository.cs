using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface IMailAccountRepository
    {
        Task<Marketing_Mail_Account?> GetByIdAsync(int id);
    }
}
