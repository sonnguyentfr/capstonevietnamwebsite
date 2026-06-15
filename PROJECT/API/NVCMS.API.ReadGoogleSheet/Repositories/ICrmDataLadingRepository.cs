using NVCMS.API.ReadGoogleSheet.Entities;

namespace NVCMS.API.ReadGoogleSheet.Repositories
{
    public interface ICrmDataLadingRepository : IRepository<student_from_ladipage>
    {
        Task<IEnumerable<student_from_ladipage>> GetByEmailOrPhoneAsync(string emails, string phones);
    }
}